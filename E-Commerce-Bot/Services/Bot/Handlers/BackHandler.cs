using E_Commerce_Bot.Entities;
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
            Task res = user.UserProcess switch
            {
                Enums.UserProcess.inCategory => _botResponseService.SenMainMenu(user.Id),
                Enums.UserProcess.inDelivery => _botResponseService.SenMainMenu(user.Id),
                Enums.UserProcess.inPaymentProcess => _botResponseService.SenMainMenu(user.Id),
                Enums.UserProcess.inBasket => _botResponseService.SenMainMenu(user.Id),
                Enums.UserProcess.fullName => _botResponseService.SenMainMenu(user.Id),
                Enums.UserProcess.onSelectPaymentType => _botResponseService.SenMainMenu(user.Id),
                Enums.UserProcess.onCommentOrder => _botResponseService.SenMainMenu(user.Id),
                Enums.UserProcess.amountRequest => _botResponseService.SenMainMenu(user.Id),
                Enums.UserProcess.atConfirmationOrder => _botResponseService.SenMainMenu(user.Id)
            };
            await res;
        }
    }
}
