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
            user.UserState = UserState.selectDeliveryType;
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
                user.ProcessHelper.OrderType = OrderType.Delivery;
                user.UserState = UserState.locationRequest;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendLocationRequestAsync(user.Id);
            }
            else if (message.Text == $"{localization.GetValue(Recources.Button.PickUp)}")
            {
                user.ProcessHelper.OrderType = OrderType.PickUp;
                user.UserState = UserState.inCategory;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendCategoriesAsync(user.Id, user.Language);
            }
            else
            {
                user.UserState = UserState.mainMenu;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendMainMenuAsync(user.Id);
            }
        }
        public async Task HandleLocationRequestAsync(Entities.User user, Message message)
        {
            user.ProcessHelper.Longitute = message.Location.Longitude;
            user.UserState = UserState.inCategory;
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
                    user.UserState = UserState.onCommentOrder;
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
                user.UserState = UserState.inProduct;
            }
            await _userRepo.UpdateAsync(user);

        }

        public async Task HandleInProductAsync(Entities.User user, Message message)
        {

            Product product = await _productRepo.GetByNameAsync(message.Text, user.Language);
            if (product is Product)
            {
                user.UserState = UserState.amountRequest;
                user.ProcessHelper.ProductId = product.Id;
                user.ProcessHelper.CategoryId = product.CategoryId;
                await _botResponseService.SendProductAsync(user.Id, product.Translate(user.Language));
                await _botResponseService.SendAmountRequestAsync(user.Id);
            }
            else
            {
                user.UserState = UserState.mainMenu;
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
            user.UserState = UserState.inCategory;
            await _userRepo.UpdateAsync(user);
        }

        public async Task HandleOnCommentOrderAsync(Entities.User user, Message message)
        {
            user.ProcessHelper.Comment = message.Text;
            user.UserState = UserState.onSelectPaymentType;
            await _userRepo.UpdateAsync(user);
            await _botResponseService.SendPaymentTypeAsync(user.Id);
        }
        public async Task HandleOnSelectPaymentTypeAsync(Entities.User user, Message message)
        {
            string paymentType = PaymentTypes.Types.FirstOrDefault(message.Text);
            if (paymentType != null)
            {
                user.UserState = UserState.atConfirmationOrder;
                user.ProcessHelper.PaymentType = paymentType;
                await _userRepo.UpdateAsync(user);

            }
        }
        public async Task HandleAtConfirmationOrderAsync(Entities.User user, Message message)
        {
            if (message.Text == "✅ Tasdiqlash")
            {
                user.UserState = UserState.inPaymentProcess;
                List<Product> products = user.Basket.Items.Select(x => x.Product).ToList();
                user.Orders.Add(
                    new Order()
                    {
                        Longitute = user.ProcessHelper.Longitute,
                        Latitude = user.ProcessHelper.Latitude,
                        Comment = user.ProcessHelper.Comment,
                        Products = products,
                        OrderType = user.ProcessHelper.OrderType,
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
            user.UserState = UserState.onCommentOrder;
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
                    OrderType = OrderType.Delivery,
                    User = user,
                    UserId = user.Id

                });
            await _userRepo.UpdateAsync(user);
        }
    }
}
