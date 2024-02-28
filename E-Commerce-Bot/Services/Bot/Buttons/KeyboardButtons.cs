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
        public static IReplyMarkup SelectOrderType()
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
        public static IReplyMarkup DeleteOrRemoveCart(IEnumerable<string> products)
        {
            var buttons = new List<List<KeyboardButton>>();
            foreach (var product in products)
            {
                buttons.Add(
                    new List<KeyboardButton>()
                        {
                            new KeyboardButton($"❌{product}")
                        });
            }
            buttons.Add(
                new List<KeyboardButton>()
                {
                            new KeyboardButton("⬅️ Ortga"),
                            new KeyboardButton("🔄 Tozalash")
                 });
            buttons.Add(
                new List<KeyboardButton>()
                {
                            new KeyboardButton("🚖 Buyurtuma berish")
                 });
            return new ReplyKeyboardMarkup(buttons)
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true,
            };
        }
        public static IReplyMarkup OnCommentBooking()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[]{"Qo'shimcha fikr yoq"},
                new KeyboardButton[]{"⬅️ Ortga","⬅️ Menyu"},

            });
        }
        public static IReplyMarkup OnSelectPaymentType()
        {
            //var f = "Click orqali to'lash\r\nTo'lov qiymati: 360000 so'm.\r\nTo'lovni amalga oshirish uchun \"✅ To'lash\" tugmasini bosing.\r\n\r\nFish and Bread Bot, [2/28/2024 3:15 PM]\r\nSizning buyurtmangiz qabul qilindi. Iltimos to'lovni amalga oshiring!"
            //var t = "https://my.click.uz/services/pay?service_id=19225&merchant_id=7531&amount=360000&transaction_param=1084927&return_url=https://t.me/BeshqozonBot"
            return new ReplyKeyboardMarkup(new[]
        {
                new KeyboardButton[]{"💵 Naqd pul"},
                new KeyboardButton[]{"💳 Payme","💳 Click" },
                 new KeyboardButton[]{"⬅️ Ortga","⬅️ Menyu"},

            });
        }
        public static IReplyMarkup AfterSelectPaymentType()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[]{"✅ Tasdiqlash"},
                new KeyboardButton[]{"❌ Bekor qilish" },

            });
        }
        public static IReplyMarkup AfterConfirmOrder()
        {
            return new InlineKeyboardMarkup(
                InlineKeyboardButton.WithPayment("✅ To'lash"));
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
