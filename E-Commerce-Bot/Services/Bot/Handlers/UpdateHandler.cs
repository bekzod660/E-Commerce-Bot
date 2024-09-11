using E_Commerce_Bot.Enums;
using E_Commerce_Bot.Extensions;
using E_Commerce_Bot.Helpers;
using E_Commerce_Bot.Persistence.Repositories;
using E_Commerce_Bot.Services.Bot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = E_Commerce_Bot.Entities.User;

namespace E_Commerce_Bot.Services.Bot
{
    public partial class UpdateHandler : IUpdateHandler
    {
        private readonly ILogger<UpdateHandler> logger;
        private readonly IBaseRepository<User> _userRepo;
        private readonly IBotResponseService _botResponseService;
        private readonly TokenService _tokenService;
        private readonly ILocalizationHandler _localization;
        private readonly OrderHandler _orderHandler;
        private readonly BasketHandler _basketHandler;
        private readonly BackHandler _backHandler;
        private readonly SettingsHandler _settingsHandler;
        public UpdateHandler(IBaseRepository<User> userRepo,
            ILogger<UpdateHandler> logger,
            IBotResponseService botResponseService,
            TokenService tokenService,
            ILocalizationHandler localization,
            OrderHandler orderHandler,
            BasketHandler basketHandler,
            BackHandler backHandler,
            SettingsHandler settingsHandler)
        {
            _userRepo = userRepo;
            this.logger = logger;
            _botResponseService = botResponseService;
            _tokenService = tokenService;
            _localization = localization;
            _orderHandler = orderHandler;
            _basketHandler = basketHandler;
            _backHandler = backHandler;
            _settingsHandler = settingsHandler;
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            User user = await _userRepo.GetByIdAsync(update.Message.From.Id);
            Telegram.Bot.Types.User _user = update.GetUser();
            if (user == null)
            {
                await _userRepo.AddAsync(new User
                {
                    Id = _user.Id,
                    Name = _user.FirstName + _user.LastName,
                    Language = _user.LanguageCode,
                    UserProcess = UserProcess.sendGreeting,
                    ProcessHelper = new Entities.ProcessHelper()
                });
                SetCulture.SetUserCulture(_user.LanguageCode);
            }
            else
            {
                SetCulture.SetUserCulture(user.Language);
            }
            if (_user.Id.ToString() == "587512349")
            {
                await BotOnMessageRecievedAdmin(botClient, update.Message);
            }
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageRecieved(botClient, update.Message),
                UpdateType.CallbackQuery => BotOnCallbackQuery(botClient, update.CallbackQuery)
            };

            try
            {
                await handler;
            }
            catch (Exception ex)
            {
                await _botResponseService.SendMainMenu(user.Id);
            }
        }




    }
}
