using Telegram.Bot;
using Telegram.Bot.Types;

namespace E_Commerce_Bot.Services.Bot
{
    public partial class UpdateHandler
    {
        private async Task BotOnMessageRecievedAdmin(ITelegramBotClient botClient, Message? message)
        {
            await _botResponseService.SendAdminMainMenu(message.From.Id);
        }
    }
}
