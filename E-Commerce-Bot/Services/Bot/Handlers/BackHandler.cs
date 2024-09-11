using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Extensions;
using E_Commerce_Bot.Persistence.Repositories;
using Telegram.Bot;

namespace E_Commerce_Bot.Services.Bot.Handlers
{
    public class BackHandler
    {
        private readonly IBaseRepository<Entities.User> _userRepo;
        private readonly IBaseRepository<Entities.Category> _categoryRepo;
        private readonly IBaseRepository<Entities.Product> _productRepo;
        private readonly IBotResponseService _botResponseService;
        private readonly ILocalizationHandler localization;
        private readonly ITelegramBotClient _botClient;

        public BackHandler(IBaseRepository<Entities.User> userRepo,
            IBotResponseService botResponseService, ILocalizationHandler localization, IBaseRepository<Product> productRepo, ITelegramBotClient botClient)
        {
            _userRepo = userRepo;
            _botResponseService = botResponseService;
            this.localization = localization;
            _productRepo = productRepo;
            _botClient = botClient;
        }

        public async Task HandleBackButtonAsync(User user)
        {
            user = user.UserStateManager();
            await _userRepo.UpdateAsync(user);
            Task res = user.UserState switch
            {
                Enums.UserState.inCategory => _botResponseService.SendMainMenuAsync(user.Id),
                Enums.UserState.inSettings => _botResponseService.SendMainMenuAsync(user.Id),
                Enums.UserState.inDelivery => _botResponseService.SendMainMenuAsync(user.Id),
                Enums.UserState.inPaymentProcess => _botResponseService.SendMainMenuAsync(user.Id),
                Enums.UserState.inBasket => _botResponseService.SendCategoriesAsync(user.Id, user.Language),
                Enums.UserState.fullName => _botResponseService.SendMainMenuAsync(user.Id),
                Enums.UserState.onSelectPaymentType => _botResponseService.SendMainMenuAsync(user.Id),
                Enums.UserState.onCommentOrder => _botResponseService.SendMainMenuAsync(user.Id),
                Enums.UserState.amountRequest => _botResponseService.SendMainMenuAsync(user.Id),
                Enums.UserState.atConfirmationOrder => _botResponseService.SendMainMenuAsync(user.Id),
                _ => _botResponseService.SendMainMenuAsync(user.Id)
            };
            await res;
        }
    }
}
