using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Enums;
using E_Commerce_Bot.Persistence.Repositories;
using E_Commerce_Bot.Services.Bot.Buttons;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = E_Commerce_Bot.Entities.User;

namespace E_Commerce_Bot.Services.Bot.Handlers
{
    public class BasketHandler
    {
        private readonly IBaseRepository<Entities.User> _userRepo;
        private readonly IBaseRepository<Entities.Category> _categoryRepo;
        private readonly IBaseRepository<Entities.Product> _productRepo;
        private readonly IBotResponseService _botResponseService;
        private readonly ITelegramBotClient _botClient;
        private readonly ILocalizationHandler localization;

        public BasketHandler(IBaseRepository<Entities.User> userRepo,
            IBotResponseService botResponseService, ILocalizationHandler localization, IBaseRepository<Product> productRepo)
        {
            _userRepo = userRepo;
            _botResponseService = botResponseService;
            this.localization = localization;
            _productRepo = productRepo;
        }
        public async Task HandleActionInBasketAsync(User user, Message message)
        {
            string text = message.Text.Split(" ")[1];
            if (text == "Tozalash")
            {
                user.Basket.Items.Clear();
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendCategories(user.Id, user.Language);
            }
            else
            {
                var deletedProdcut = await _productRepo.GetByNameAsync(text);
                user.Basket.Items.Remove(user.Basket.Items.FirstOrDefault(x => x.Product.Id == deletedProdcut.Id));
                await _userRepo.UpdateAsync(user);
                await HandleBasketButtonAsync(user, _botClient, message);
            }
        }

        private async Task HandleBasketButtonAsync(User user, ITelegramBotClient botClient, Message message)
        {
            if (user.Basket.Items.Count == 0)
            {
                await _botResponseService.SendCategories(user.Id, user.Language);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "*«❌ Maxsulot nomi»* - savatdan o'chirish \r\n *«🔄 Tozalash»* - savatni butunlay bo'shatish");
                var products = new StringBuilder("📥 Savat:");
                foreach (var item in user.Basket.Items)
                {
                    products = products.Append($"\n{item.Product.Name_Uz}\n {item.Count} * {item.Product.Price} = {item.Count * item.Product.Price}\n");
                }
                double totalPrice = user.Basket.Items.Select(x => x.Product.Price * x.Count).ToList().Sum();
                user.UserProcess = UserProcess.inBasket;
                await _userRepo.UpdateAsync(user);
                products = products.Append($"\nJami:{totalPrice}");
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    $"{products}",
                    replyMarkup: KeyboardButtons.DeleteOrRemoveBasket(user.Basket.Items.Select(x => x.Product.Name_Uz)));
            }
        }
    }
}
