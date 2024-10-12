using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Enums;
using E_Commerce_Bot.Helpers;
using E_Commerce_Bot.Persistence.Repositories;
using Telegram.Bot.Types;

namespace E_Commerce_Bot.Services.Bot.Handlers
{
    public class OrderHandler
    {
        private readonly IBaseRepository<Entities.User> _userRepo;
        private readonly IBaseRepository<Entities.Category> _categoryRepo;
        private readonly IBaseRepository<Entities.Product> _productRepo;
        private readonly IBotResponseService _botResponseService;
        private readonly ILocalizationHandler localization;

        public OrderHandler(IBaseRepository<Entities.User> userRepo,
            IBotResponseService botResponseService,
            ILocalizationHandler localization,
            IBaseRepository<Product> productRepo,
            IBaseRepository<Category> categoryRepo)
        {
            _userRepo = userRepo;
            _botResponseService = botResponseService;
            this.localization = localization;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task HandleOrderButtonAsync(Entities.User user, Message message)
        {
            user.UserStateId = UserState.SELECT_DELIVERY_TYPE;
            //user.Basket.Items.Clear();
            await _userRepo.UpdateAsync(user);
            await _botResponseService.SendDeliveryTypesAsync(user.Id);
        }
        public async Task HandleInDeliveryTypeRequestAsync(Entities.User user, Message message)
        {
            //if (user.ProcessHelper is null)
            //{
            //    user.ProcessHelper = new ProcessHelper();
            //}
            if (message.Text == $"{localization.GetValue(Recources.Button.Delivery)}")
            {
                user.ProcessHelper.OrderTypeId = OrderType.Delivery;
                user.UserStateId = UserState.LOCATION_REQUEST;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendLocationRequestAsync(user.Id);
            }
            else if (message.Text == $"{localization.GetValue(Recources.Button.PickUp)}")
            {
                user.ProcessHelper.OrderTypeId = OrderType.Pick_Up;
                user.UserStateId = UserState.IN_CATEGORY;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendCategoriesAsync(user.Id, user.Language);
            }
            else
            {
                user.UserStateId = UserState.MAIN_MENU;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendMainMenuAsync(user.Id);
            }
        }
        public async Task HandleLocationRequestAsync(Entities.User user, Message message)
        {
            user.ProcessHelper.Longitute = message.Location.Longitude;
            user.UserStateId = UserState.IN_CATEGORY;
            user.ProcessHelper.Latitude = message.Location.Latitude;
            await _userRepo.UpdateAsync(user);
            await _botResponseService.SendCategoriesAsync(user.Id, user.Language);
        }

        public async Task HandleInCategoryAsync(Entities.User user, Message message)
        {
            if (message.Text == localization.GetValue(Recources.Button.StartOrder))
            {
                if (user.Basket is null)
                {
                    await _botResponseService.SendMessageAsync(user.Id, localization.GetValue(Recources.Message.EmptyBasket));
                }
                else
                {
                    await _botResponseService.SendCommentRequest(user.Id);
                    user.UserStateId = UserState.ON_COMMENT_TO_ORDER;
                }
            }
            else
            {
                Category category = await _categoryRepo.GetByNameAsync(message.Text, user.Language);
                if (category is Category)
                {
                    List<string> products = Translator.Translate(user.Language, category.Products.ToList());
                    await _botResponseService.SendProductsAsync(user.Id, products);
                }
                else
                {

                }
                user.UserStateId = UserState.IN_PRODUCT;
            }
            await _userRepo.UpdateAsync(user);

        }

        public async Task HandleInProductAsync(Entities.User user, Message message)
        {

            Product product = await _productRepo.GetByNameAsync(message.Text, user.Language);
            if (product is Product)
            {
                user.UserStateId = UserState.AMOUNT_REQUEST;
                user.ProcessHelper.ProductId = product.Id;
                user.ProcessHelper.CategoryId = product.CategoryId;
                await _botResponseService.SendProductAsync(user.Id, product.Translate(user.Language));
                await _botResponseService.SendAmountRequestAsync(user.Id);
            }
            else
            {
                user.UserStateId = UserState.MAIN_MENU;
                await _botResponseService.SendMainMenuAsync(user.Id);
            }
            await _userRepo.UpdateAsync(user);
        }

        public async Task HandleAmountRequestAsync(Entities.User user, Message message)
        {
            var product = await _productRepo.GetByIdAsync((int)user.ProcessHelper.ProductId);
            if (int.TryParse(message.Text, out int amount))
            {
                if (user.Basket.Items.FirstOrDefault(x => x.ProductId == product.Id) is null)
                {
                    user.Basket.Items.Add(
                    new Item()
                    {
                        Product = product,
                        ProductId = product.Id,
                        Count = amount,
                        Basket = user.Basket
                    });
                }
                else
                {
                    user.Basket.Items.FirstOrDefault(x => x.Product.Id == product.Id)
                        .Count = user.Basket.Items.FirstOrDefault(x => x.Product.Id == product.Id).Count + amount;
                }
                await _botResponseService.SendCategoriesAsync(user.Id, user.Language);
            }
            user.UserStateId = UserState.IN_CATEGORY;
            await _userRepo.UpdateAsync(user);
        }

        public async Task HandleOnCommentOrderAsync(Entities.User user, Message message)
        {
            user.ProcessHelper.Comment = message.Text;
            user.UserStateId = UserState.ON_SELECT_PAYMENT_TYPE;
            await _userRepo.UpdateAsync(user);
            await _botResponseService.SendPaymentTypeAsync(user.Id);
        }
        public async Task HandleOnSelectPaymentTypeAsync(Entities.User user, Message message)
        {
            string paymentType = PaymentTypes.Types.FirstOrDefault(message.Text);
            if (paymentType != null)
            {
                user.UserStateId = UserState.AT_CONFIRMATION_AMOUNT;
                user.ProcessHelper.PaymentType = paymentType;
                await _userRepo.UpdateAsync(user);

            }
        }
        public async Task HandleAtConfirmationOrderAsync(Entities.User user, Message message)
        {
            if (message.Text == "✅ Tasdiqlash")
            {
                user.UserStateId = UserState.IN_PAYMENT_PROCESS;
                List<Product> products = user.Basket.Items.Select(x => x.Product).ToList();
                user.Orders.Add(
                    new Order()
                    {
                        Longitute = user.ProcessHelper.Longitute,
                        Latitude = user.ProcessHelper.Latitude,
                        Comment = user.ProcessHelper.Comment,
                        Products = products,
                        OrderTypeId = user.ProcessHelper.OrderTypeId,
                        PaymentType = user.ProcessHelper.PaymentType,
                        User = user
                    });
                await _userRepo.UpdateAsync(user);
                var totalPrice = user.Basket.Items.Select(x => x.Count * x.Product.Price).Sum();
                //await botClient.SendTextMessageAsync(
                //    message.Chat.Id,
                //    "Sizning buyurtmangiz qabul qilindi. Iltimos to'lovni amalga oshiring!");
                //await botClient.SendTextMessageAsync(
                //    message.Chat.Id,
                //    $"{user.Orders.Last().PaymentType} orqali to'lash" +
                //    $"To'lov qiymati:{user.Orders.Last().TotalPrice}" +
                //    $"To'lovni amalga oshirish uchun \"✅ To'lash\" tugmasini bosing.",
                //    replyMarkup: InlineButton.Pay(user.Orders.Last().TotalPrice, user.Orders.Last().Id));
            }
            else
            {
                await _botResponseService.SendCategoriesAsync(user.Id, user.Language);
            }
        }
        public async Task HandlePlaceAnOrderButtonAsync(Entities.User user, Message message)
        {
            user.UserStateId = UserState.ON_COMMENT_TO_ORDER;
            if (user.Orders is null)
            {
                user.Orders = new List<Order>();
            }
            double totalPrice = user.Basket.Items.Select(x => x.Product.Price * x.Count).ToList().Sum();
            user.Orders.Add(
                new Order()
                {
                    Longitute = (double)user.ProcessHelper.Longitute,
                    Latitude = (double)user.ProcessHelper.Latitude,
                    OrderTypeId = OrderType.Delivery,
                    User = user,
                    UserId = user.Id

                });
            await _userRepo.UpdateAsync(user);
        }
    }
}
