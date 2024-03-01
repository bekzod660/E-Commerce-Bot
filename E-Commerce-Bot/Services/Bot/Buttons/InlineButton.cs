using Telegram.Bot.Types.ReplyMarkups;

namespace E_Commerce_Bot.Services.Bot.Buttons
{
    public static class InlineButton
    {
        public static InlineKeyboardMarkup Pay(double mount, int orderId)
        {
            return new InlineKeyboardMarkup(
                InlineKeyboardButton.WithUrl(
                   text: "✅ To'lash",
                   url: "https://my.click.uz/services/pay?service_id=19225&merchant_id=7531&amount=300000&transaction_param=1086236&return_url=https://t.me/BeshqozonBot")
                );
        }
    }
}
