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
                    new KeyboardButton("ℹ️ Ma'lumot")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("☎️ Biz bilan aloqa"),
                    new KeyboardButton("⚙️ Sozlamalar")
                }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
        public static IReplyMarkup InSelectOrderType()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("🚖 Yetkazib berish"),
                new KeyboardButton("🏃 Olib ketish"),
                BackButton()

            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
        public static KeyboardButton BackButton()
        {
            return new KeyboardButton("⬅️ Ortga");
        }
        public static KeyboardButton CartButton()
        {
            return new KeyboardButton("🛒 Savatcha");
        }
        public static IReplyMarkup SelectedDeliveryType()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                   KeyboardButton.WithRequestLocation("📍 Lokatsiya jo'natish"),
                   BackButton()
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
        public static IReplyMarkup SelectedPickUp(List<string> menu)
        {
            return MakeReplyMarkup(menu);
        }
        public static IReplyMarkup Menu(List<string> menu)
        {
            //var buttons = new List<List<KeyboardButton>>();
            //if(menu.Count % 2 == 0)
            //{

            //}
            return MakeReplyMarkup(menu);
        }
        public static List<List<KeyboardButton>> MakeButton(List<string> items)
        {
            var buttons = new List<List<KeyboardButton>>();
            for (int i = 0; i < items.Count / 2; i++)
            {
                buttons.Add(
                    new List<KeyboardButton>()
                        {
                            new KeyboardButton(items[2*i]),
                            new KeyboardButton(items[2*i +1])
                        }
                    );
            }
            return buttons;
        }
        public static IReplyMarkup MakeReplyMarkup(List<string> item)
        {

            var buttons = MakeButton(item);
            if (item.Count % 2 != 0)
            {
                buttons.Add(
                    new List<KeyboardButton>()
                    {
                        new KeyboardButton($"{item.Last()}")
                    });
            }
            buttons.Add(
                new List<KeyboardButton>()
                {
                    CartButton(),
                    BackButton()
                });
            return new ReplyKeyboardMarkup(buttons)
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
        public static IReplyMarkup SelectAmountBtn()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[]{"1", "2", "3"},
                new KeyboardButton[]{"4", "5", "6"},
                new KeyboardButton[]{"7", "8", "9"},
                new KeyboardButton[]{CartButton(),BackButton() }
            });
        }
        public static IReplyMarkup Cart(List<string> items)
        {
            var buttons = new List<List<KeyboardButton>>();
            for (int i = 0; i < items.Count / 2; i++)
            {
                buttons.Add(new List<KeyboardButton>()
                {
                    new KeyboardButton(items[2*i]),
                    new KeyboardButton(items[2*i +1])
                });
            }

            buttons.Add(new List<KeyboardButton>()
                {
                    new KeyboardButton("<= ortga"),
                    new KeyboardButton("Tozalash"),
                    new KeyboardButton("Buyurtma Berish")
                });
            return new ReplyKeyboardMarkup(buttons);
        }
    }
}
