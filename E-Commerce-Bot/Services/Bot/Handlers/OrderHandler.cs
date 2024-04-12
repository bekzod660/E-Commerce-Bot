using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Enums;
using E_Commerce_Bot.Helpers;
using E_Commerce_Bot.Persistence.Repositories;
using E_Commerce_Bot.Services.Bot.Buttons;
using System.Text;
using Telegram.Bot;
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

        public OrderHandler(IBaseRepository<Entities.User> userRepo, IBotResponseService botResponseService, ILocalizationHandler localization, IBaseRepository<Product> productRepo)
        {
            _userRepo = userRepo;
            _botResponseService = botResponseService;
            this.localization = localization;
            _productRepo = productRepo;
        }

        public async Task HandleOrderButtonAsync(Entities.User user, Message message)
        {
            user.UserProcess = UserProcess.selectDeliveryType;
            user.Basket.Items.Clear();
            await _userRepo.UpdateAsync(user);
            await _botResponseService.SendDeliveryTypes(user.Id);
        }
        public async Task HandleInDeliveryTypeRequestAsync(Entities.User user, Message message)
        {
            if (user.ProcessHelper is null)
            {
                user.ProcessHelper = new ProcessHelper();
            }
            if (message.Text == $"{localization.GetValue(Recources.Button.Delivery)}")
            {
                user.ProcessHelper.OrderType = OrderType.Delivery;
                user.UserProcess = UserProcess.locationRequest;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendLocationRequest(user.Id);
            }
            else if (message.Text == $"{localization.GetValue(Recources.Button.PickUp)}")
            {
                user.ProcessHelper.OrderType = OrderType.PickUp;
                user.UserProcess = UserProcess.inCategory;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendCategories(user.Id, user.Language);
            }

        }
        public async Task HandleLocationRequestAsync(Entities.User user, Message message)
        {
            user.ProcessHelper.Longitute = message.Location.Longitude;
            user.UserProcess = UserProcess.inCategory;
            user.ProcessHelper.Latitude = message.Location.Latitude;
            await _userRepo.UpdateAsync(user);
            await _botResponseService.SendCategories(user.Id, user.Language);
        }

        public async Task HandleInCategoryAsync(Entities.User user, Message message)
        {
            if (message.Text == localization.GetValue(Recources.Button.StartOrder))
            {
                if (user.Basket is null)
                {
                    await _botResponseService.SendMessages(user.Id, localization.GetValue(Recources.Message.EmptyBasket));
                }

            }
            else
            {
                Category category = await _categoryRepo.GetByNameAsync(message.Text);
                if (category is Category)
                {
                    List<string> products = Translator.Translate($"name_{user.Language}", category.Products.ToList());
                    await _botResponseService.SendProducts(user.Id, products);
                }
                else
                {

                }
                user.UserProcess = UserProcess.inProduct;
                await _userRepo.UpdateAsync(user);
            }

        }

        public async Task HandleInProductAsync(Entities.User user, Message message)
        {

            Product product = await _productRepo.GetByNameAsync(message.Text);
            if (product is Product)
            {
                user.UserProcess = UserProcess.amountRequest;
                user.ProcessHelper.ProductId = product.Id;
                user.ProcessHelper.CategoryId = product.CategoryId;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendAmountRequest(user.Id);
            }
            else
            {

            }

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
                await _botResponseService.SendCategories(user.Id, user.Language);
            }

        }

        public async Task HandleAtConfirmationOrderAsync(Entities.User user, Message message)
        {
            if (message.Text == "✅ Tasdiqlash")
            {
                user.UserProcess = UserProcess.inPaymentProcess;
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
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "Sizning buyurtmangiz qabul qilindi. Iltimos to'lovni amalga oshiring!");
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    $"{user.Orders.Last().PaymentType} orqali to'lash" +
                    $"To'lov qiymati:{user.Orders.Last().TotalPrice}" +
                    $"To'lovni amalga oshirish uchun \"✅ To'lash\" tugmasini bosing.",
                    replyMarkup: InlineButton.Pay(user.Orders.Last().TotalPrice, user.Orders.Last().Id));
            }
            else
            {
                await _botResponseService.SendCategories(user.Id, user.Language);
            }


        }

        public async Task HandleOnSelectPaymentTypeAsync(Entities.User user, Message message)
        {
            string paymentType = PaymentTypes.Types.FirstOrDefault(message.Text);
            if (paymentType != null)
            {
                user.UserProcess = UserProcess.atConfirmationOrder;
                user.ProcessHelper.PaymentType = paymentType;
                await _userRepo.UpdateAsync(user);
                var txt = new StringBuilder("Sizning buyurtmangiz:\n");
                string deliveryType = user.ProcessHelper.OrderType == OrderType.Delivery ? "🚖 Yetkazib berish"
                    : "🏃 Olib ketish";

                txt.Append($"Buyurtma turi:{deliveryType}");
                txt.Append($"Telefon:{user.PhoneNumber}");
                txt.Append($"To'lov usuli:{paymentType}");
                txt.Append($"Izohlar:{user.ProcessHelper.Comment}");
                foreach (var item in user.Basket.Items)
                {
                    txt.Append($"{item.Product.Name}\n" +
                        $"{item.Count} * {item.Product.Price} = {item.Count * item.Product.Price}\n");
                }
                txt.Append($"Jami: {user.Basket.Items.Select(x => x.Count * x.Product.Price).Sum()}");
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    $"{txt}",
                    replyMarkup: KeyboardButtons.AfterSelectPaymentType());
                user.UserProcess = UserProcess.atConfirmationOrder;
                await _userRepo.UpdateAsync(user);
            }
        }

        public async Task HandleOnCommentOrderAsync(Entities.User user, Message message)
        {
            user.ProcessHelper.Comment = message.Text;
            user.UserProcess = UserProcess.onSelectPaymentType;
            await _userRepo.UpdateAsync(user);
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Buyurtmangiz uchun to'lov turini tanlang",
                replyMarkup: KeyboardButtons.OnSelectPaymentType());
        }

        public async Task HandlePlaceAnOrderButtonAsync(Entities.User user, Message message)
        {
            user.UserProcess = UserProcess.onCommentOrder;
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
