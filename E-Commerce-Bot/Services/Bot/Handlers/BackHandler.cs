using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Enums;
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
            user.UserProcess = user.UserProcess switch
            {
                Enums.UserProcess.inCategory => UserProcess.mainMenu,
                Enums.UserProcess.inSettings => UserProcess.mainMenu,
                Enums.UserProcess.inDelivery => UserProcess.mainMenu,
                Enums.UserProcess.inPaymentProcess => UserProcess.mainMenu,
                Enums.UserProcess.inBasket => UserProcess.mainMenu,
                Enums.UserProcess.fullName => UserProcess.mainMenu,
                Enums.UserProcess.onSelectPaymentType => UserProcess.mainMenu,
                Enums.UserProcess.onCommentOrder => UserProcess.mainMenu,
                Enums.UserProcess.amountRequest => UserProcess.mainMenu,
                Enums.UserProcess.atConfirmationOrder => UserProcess.mainMenu,
                _ => UserProcess.mainMenu
            };
            await _userRepo.UpdateAsync(user);
            Task res = user.UserProcess switch
            {
                Enums.UserProcess.inCategory => _botResponseService.SendMainMenu(user.Id),
                Enums.UserProcess.inSettings => _botResponseService.SendMainMenu(user.Id),
                Enums.UserProcess.inDelivery => _botResponseService.SendMainMenu(user.Id),
                Enums.UserProcess.inPaymentProcess => _botResponseService.SendMainMenu(user.Id),
                Enums.UserProcess.inBasket => _botResponseService.SendMainMenu(user.Id),
                Enums.UserProcess.fullName => _botResponseService.SendMainMenu(user.Id),
                Enums.UserProcess.onSelectPaymentType => _botResponseService.SendMainMenu(user.Id),
                Enums.UserProcess.onCommentOrder => _botResponseService.SendMainMenu(user.Id),
                Enums.UserProcess.amountRequest => _botResponseService.SendMainMenu(user.Id),
                Enums.UserProcess.atConfirmationOrder => _botResponseService.SendMainMenu(user.Id),
                _ => _botResponseService.SendMainMenu(user.Id)
            };
            await res;
        }
    }
}
