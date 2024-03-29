using E_Commerce_Bot.Extensions;
using E_Commerce_Bot.Helpers;
using E_Commerce_Bot.Persistence.Repositories;
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
        private readonly UserRepository _userRepo;

        public UpdateHandler(UserRepository userRepo, ILogger<UpdateHandler> logger)
        {
            _userRepo = userRepo;
            this.logger = logger;
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            User user = await _userRepo.GetByIdAsync(update.Message.From.Id);
            if (user == null)
            {
                Telegram.Bot.Types.User _user = update.GetUser();
                await _userRepo.AddAsync(new User
                {
                    Id = _user.Id,
                    Name = _user.FirstName + _user.LastName,
                    Language = _user.LanguageCode,
                    UserProcess = Entities.UserProcess.selectLanguage
                });
                SetCulture.SetUserCulture(_user.LanguageCode);
            }
            SetCulture.SetUserCulture(user.Language);
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

            }
        }




    }
}
