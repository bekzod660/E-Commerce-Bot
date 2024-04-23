using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Persistence.Repositories;
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
            if (text == localization.GetValue(Recources.Message.OnBasket))
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
                await HandleBasketButtonAsync(user, message);
            }
        }

        public async Task HandleBasketButtonAsync(User user, Message message)
        {
            if (user.Basket.Items.Count == 0)
            {
                await _botResponseService.SendCategories(user.Id, user.Language);
            }
            else
            {

            }
        }
    }
}
