using Telegram.Bot.Types.ReplyMarkups;

namespace E_Commerce_Bot.Services.Bot.Buttons
{
    public static class KeyboardButtons
    {
        public static IReplyMarkup SendContactRequest()
        {
            return new ReplyKeyboardMarkup(new[]
            {
              KeyboardButton.WithRequestContact("📞 Kontaktni jo'natish")
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
        public static IReplyMarkup SendLocationRequest()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("📍 Lokatsiya jo'natish")
                {
                    RequestLocation = true
                }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
        public static IReplyMarkup MainMenu()
        {
            return new ReplyKeyboardMarkup(new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("🛍 Buyurtma berish")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("✍️ Fikr bildirish"),
                    new KeyboardButton("ℹ️ Ma'lumot"),
                    new KeyboardButton("☎️ Biz bilan aloqa"),
                    new KeyboardButton("⚙️ Sozlamalar")
                }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
        public static IReplyMarkup WhenOrderSelected()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("🚖 Yetkazib berish"),
                new KeyboardButton("🏃 Olib ketish")
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }

        public static IReplyMarkup WhenSelectedDelivery()
        {
            return new ReplyKeyboardMarkup(
                KeyboardButton.WithRequestLocation("📍 Lokatsiya jo'natish"))
            { ResizeKeyboard = true };
        }
        public static IReplyMarkup WhenSelectedPickUp()
        {

        }
        public static IReplyMarkup MakeButtons(List<string> item)
        {
            var buttons = new List<List<KeyboardButton>>();
            for (int i = 0; i < item.Count / 2; i++)
            {
                buttons.Add(
                    new List<KeyboardButton>()
                        {
                            new KeyboardButton(item[2*i]),
                            new KeyboardButton(item[2* +1])
                        }
                    );
            }

            buttons.Add(
                new List<KeyboardButton>()
                {
                    new KeyboardButton("🛒 Savatcha"),
                    new KeyboardButton("↪️ Orqaga")
                });
            return new ReplyKeyboardMarkup(buttons)
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }

    }
}
