using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace E_Commerce_Bot.Extensions
{
    public static class TelegramExtension
    {
        public static User GetUser(this Update update)
            => update switch
            {
                UpdateType.Message => update.Message.From,
                UpdateType.EditedMessage => update.EditedMessage.From,
                UpdateType.CallbackQuery => update.CallbackQuery.From,
                UpdateType.InlineQuery => update.InlineQuery.From,
                _ => throw new Exception("We dont supportas update type {update.Type} yet")
            };
    }
}
