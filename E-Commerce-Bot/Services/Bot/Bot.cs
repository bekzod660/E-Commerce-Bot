
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace E_Commerce_Bot.Services.Bot
{
    public class Bot : BackgroundService
    {
        private readonly ILogger<Bot> logger;
        private readonly ITelegramBotClient botClient;
        private readonly IUpdateHandler updateHandler;

        public Bot(IUpdateHandler updateHandler, ITelegramBotClient botClient, ILogger<Bot> logger)
        {
            this.updateHandler = updateHandler;
            this.botClient = botClient;
            this.logger = logger;
            botClient.StartReceiving(new DefaultUpdateHandler(updateHandler.HandleUpdateAsync, updateHandler.HandlePollingErrorAsync));
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                var bot = await botClient.GetMeAsync(cancellationToken: cancellationToken);
                logger.LogInformation("Bot {username} is started", bot.Username);

                //botClient.StartReceiving(
                //    updateHandler: updateHandler,
                //    receiverOptions: default,
                //    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to connect to bot server.");
            }
        }
    }
}
