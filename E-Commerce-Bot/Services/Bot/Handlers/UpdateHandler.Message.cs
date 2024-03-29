using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Enums;
using E_Commerce_Bot.Services.Bot.Buttons;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = E_Commerce_Bot.Entities.User;

namespace E_Commerce_Bot.Services.Bot
{
    public partial class UpdateHandler
    {
        private async Task BotOnMessageRecieved(ITelegramBotClient botClient, Message? message)
        {
            if (message is null) return;
            Entities.User user = await _userService.GetByIdAsync(message.Chat.Id);
            if (user is null)
            {
                if (message.Text == "/start")
                {
                    await botClient.SendTextMessageAsync(
                                       message.Chat.Id,
                                        "Salom\nIltimos ismingizni kiriting:",
                                        replyMarkup: new ReplyKeyboardRemove(),
                                       parseMode: ParseMode.Html);
                    await _userService.AddAsync(new User
                    {
                        Id = message.Chat.Id,
                        Name = message.Chat.Username,
                        UserProcess = Entities.UserProcess.GoFullNameRequest,
                        Basket = new Basket(),
                        ProcessHelper = new ProcessHelper()
                    });
                    //await _userService.UpdateAsync(user);
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                                       message.Chat.Id,
                                        "Salom\n botdan foydalanish uchun start tugmasini bosing:",
                                        replyMarkup: new ReplyKeyboardMarkup(new KeyboardButton("/start"))
                                        {
                                            ResizeKeyboard = true,
                                            OneTimeKeyboard = true
                                        },
                                       parseMode: ParseMode.Html);
                }
            }
            else if (message.Text == "🛒 Savatcha")
            {
                await HandleBasketButtonAsync(user, botClient, message);
            }
            else if (message.Text == "🚖 Buyurtuma berish")
            {
                user.UserProcess = UserProcess.OnCommentOrder;
                await _userService.UpdateAsync(user);
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "Buyurtmaga izoh qoldiring()",
                    replyMarkup: KeyboardButtons.OnCommentBooking());
            }
            else if (message.Text == "⬅️ Ortga")
            {
                await HandleBackButtonAsync(user, botClient, message);
            }
            else if (user is User)
            {
                await HandleUserProcess(user, botClient, message);
            }
            else
            {
                await HandleUnknownCommand(user, botClient, message);
            }

            //else if (message.Text == "🚖 Buyurtuma berish")
            //{

            //}


            //else if (message.Text == "✅ Tasdiqlash")
            //{

            //}
            //else if (message.Text == "❌ Bekor qilish")
            //{


        }
        private async Task HandleUserProcess(User user, ITelegramBotClient botClient, Message message)
        {
            Task res = user.UserProcess switch
            {
                UserProcess.GoFullNameRequest => HandleFullNameRequestAsync(user, botClient, message),
                UserProcess.GoContactRequest => HandleContactRequestAsync(user, botClient, message),
                UserProcess.MainMenu => HandleMainMenuAsync(user, botClient, message),
                UserProcess.DeliveryTypeRequest => HandleInDeliveryTypeRequestAsync(user, botClient, message),
                UserProcess.LocationRequest => HandleLocationRequestAsync(user, botClient, message),
                UserProcess.InCategory => HandleInCategoryAsync(user, botClient, message),
                UserProcess.InProduct => HandleInProductAsync(user, botClient, message),
                UserProcess.AmountRequest => HandleAmountRequestAsync(user, botClient, message),
                UserProcess.InBasket => HandleActionInBasketAsync(user, botClient, message),
                UserProcess.OnCommentOrder => HandleOnCommentOrderAsync(user, botClient, message),
                UserProcess.OnSelectPaymentType => HandleOnSelectPaymentTypeAsync(user, botClient, message),
                UserProcess.AtConfirmationOrder => HandleAtConfirmationOrderAsync(user, botClient, message),
                _ => HandleUnknownCommand(user, botClient, message)
            };
            await res;
        }

        private async Task HandleAtConfirmationOrderAsync(User user, ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "✅ Tasdiqlash")
            {
                user.UserProcess = UserProcess.InPaymentProcess;
                List<Product> products = user.Basket.Items.Select(x => x.Product).ToList();
                user.Orders.Add(
                    new Order()
                    {
                        Longitute = user.ProcessHelper.Longitute,
                        Latitude = user.ProcessHelper.Latitude,
                        Comment = user.ProcessHelper.Comment,
                        Products = products,
                        OrderType = user.ProcessHelper.OrderType,
                        PaymentType = user.ProcessHelper.PaymentType,
                        User = user
                    });
                await _userService.UpdateAsync(user);
                var totalPrice = user.Basket.Items.Select(x => x.Count * x.Product.Price).Sum();
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "Sizning buyurtmangiz qabul qilindi. Iltimos to'lovni amalga oshiring!");
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    $"{user.Orders.Last().PaymentType} orqali to'lash" +
                    $"To'lov qiymati:{user.Orders.Last().TotalPrice}" +
                    $"To'lovni amalga oshirish uchun \"✅ To'lash\" tugmasini bosing.",
                    replyMarkup: InlineButton.Pay(user.Orders.Last().TotalPrice, user.Orders.Last().Id));
            }
            else
            {
                await SendCategories(user, botClient, message);
            }


        }

        private async Task HandleOnSelectPaymentTypeAsync(User user, ITelegramBotClient botClient, Message message)
        {
            string paymentType = PaymentTypes.Types.FirstOrDefault(message.Text);
            if (paymentType != null)
            {
                user.UserProcess = UserProcess.AtConfirmationOrder;
                user.ProcessHelper.PaymentType = paymentType;
                await _userService.UpdateAsync(user);
                var txt = new StringBuilder("Sizning buyurtmangiz:\n");
                string deliveryType = user.ProcessHelper.OrderType == OrderType.Delivery ? "🚖 Yetkazib berish"
                    : "🏃 Olib ketish";

                txt.Append($"Buyurtma turi:{deliveryType}");
                txt.Append($"Telefon:{user.PhoneNumber}");
                txt.Append($"To'lov usuli:{paymentType}");
                txt.Append($"Izohlar:{user.ProcessHelper.Comment}");
                foreach (var item in user.Basket.Items)
                {
                    txt.Append($"{item.Product.Name}\n" +
                        $"{item.Count} * {item.Product.Price} = {item.Count * item.Product.Price}\n");
                }
                txt.Append($"Jami: {user.Basket.Items.Select(x => x.Count * x.Product.Price).Sum()}");
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    $"{txt}",
                    replyMarkup: KeyboardButtons.AfterSelectPaymentType());
                user.UserProcess = UserProcess.AtConfirmationOrder;
                await _userService.UpdateAsync(user);
            }
        }

        private async Task HandleOnCommentOrderAsync(User user, ITelegramBotClient botClient, Message message)
        {
            user.ProcessHelper.Comment = message.Text;
            user.UserProcess = UserProcess.OnSelectPaymentType;
            await _userService.UpdateAsync(user);
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Buyurtmangiz uchun to'lov turini tanlang",
                replyMarkup: KeyboardButtons.OnSelectPaymentType());
        }

        private async Task HandlePlaceAnOrderButtonAsync(User user, ITelegramBotClient botClient, Message message)
        {
            user.UserProcess = UserProcess.OnCommentOrder;
            if (user.Orders is null)
            {
                user.Orders = new List<Order>();
            }
            double totalPrice = user.Basket.Items.Select(x => x.Product.Price * x.Count).ToList().Sum();
            user.Orders.Add(
                new Order()
                {
                    Longitute = (double)user.ProcessHelper.Longitute,
                    Latitude = (double)user.ProcessHelper.Latitude,
                    OrderType = OrderType.Delivery,
                    User = user,
                    UserId = user.Id

                });
            await _userService.UpdateAsync(user);
        }

        private async Task HandleMainMenuAsync(User user, ITelegramBotClient botClient, Message message)
        {
            var r = message.Text switch
            {
                "🛍 Buyurtma berish" => HandleOrderButtonAsync(user, botClient, message),
                "⚙️ Sozlamalar" => HandleSettingsAsync(user, botClient, message),
                "☎️ Biz bilan aloqa" => HandleContactUsAsync(user, botClient, message),
                "✍️ Fikr bildirish" => HandleFeedbackAsync(user, botClient, message),
                "ℹ️ Ma'lumot" => HandleInformationAsync(user, botClient, message)
            };
            await r;

        }

        private async Task HandleInformationAsync(User user, ITelegramBotClient botClient, Message message)
        {
            //var categories = await _categoryService.GetAllAsync();
            //await botClient.SendTextMessageAsync(
            //    message.Chat.Id,
            //    "Menyu",
            //    replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList(), )
            //    );
        }

        private Task HandleFeedbackAsync(User user, ITelegramBotClient botClient, Message message)
        {
            throw new NotImplementedException();
        }

        private Task HandleContactUsAsync(User user, ITelegramBotClient botClient, Message message)
        {
            throw new NotImplementedException();
        }

        private async Task HandleSettingsAsync(User user, ITelegramBotClient botClient, Message message)
        {
            var categories = await _categoryService.GetAllAsync();
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Menyu",
                replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList(),
                new List<KeyboardButton> { KeyboardButtons.PlaceAnOrderButton(), KeyboardButtons.BasketButton(), KeyboardButtons.BackButton() })
                );
        }

        private async Task HandleInCategoryAsync(User user, ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "🚖 Buyurtuma berish")
            {

            }
            else if (message.Text == "🛒 Savatcha")
            {

            }
            Category category = await _categoryService.GetByNameAsync(message.Text);
            if (category is Category)
            {
                await SendProducts(user, botClient, message, category.Id);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "Uzur noto'g'ri ma'lumot kiritildi");
                await SendCategories(user, botClient, message);
            }
        }
        private async Task HandleInDeliveryTypeRequestAsync(User user, ITelegramBotClient botClient, Message message)
        {
            if (user.ProcessHelper is null)
            {
                user.ProcessHelper = new ProcessHelper();
            }
            if (message.Text == "🚖 Yetkazib berish")
            {
                user.ProcessHelper.OrderType = OrderType.Delivery;
                user.UserProcess = UserProcess.LocationRequest;
                await _userService.UpdateAsync(user);
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "📍 Lokatsiya yuboring ..",
                    replyMarkup: KeyboardButtons.SendLocationRequest());
            }
            else if (message.Text == "🏃 Olib ketish")
            {
                user.ProcessHelper.OrderType = OrderType.PickUp;
                user.UserProcess = UserProcess.InCategory;
                await _userService.UpdateAsync(user);
                List<Category> categories = await _categoryService.GetAllAsync();
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "",
                    replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList(),
                    new List<KeyboardButton> { KeyboardButtons.PlaceAnOrderButton(), KeyboardButtons.BasketButton(), KeyboardButtons.BackButton() })
                );
            }

        }
        private async Task HandleActionInBasketAsync(User user, ITelegramBotClient botClient, Message message)
        {
            string text = message.Text.Split(" ")[1];
            if (text == "Tozalash")
            {
                user.Basket.Items.Clear();
                await _userService.UpdateAsync(user);
                await SendCategories(user, botClient, message);
            }
            else
            {
                var deletedProdcut = await _productService.GetByNameAsync(text);
                user.Basket.Items.Remove(user.Basket.Items.FirstOrDefault(x => x.Product.Id == deletedProdcut.Id));
                await _userService.UpdateAsync(user);
                await HandleBasketButtonAsync(user, botClient, message);
            }
        }

        private async Task HandleBasketButtonAsync(User user, ITelegramBotClient botClient, Message message)
        {
            if (user.Basket.Items.Count == 0)
            {
                await SendCategories(user, botClient, message);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "*«❌ Maxsulot nomi»* - savatdan o'chirish \r\n *«🔄 Tozalash»* - savatni butunlay bo'shatish");
                var products = new StringBuilder("📥 Savat:");
                foreach (var item in user.Basket.Items)
                {
                    products = products.Append($"\n{item.Product.Name}\n {item.Count} * {item.Product.Price} = {item.Count * item.Product.Price}\n");
                }
                double totalPrice = user.Basket.Items.Select(x => x.Product.Price * x.Count).ToList().Sum();
                user.UserProcess = UserProcess.InBasket;
                await _userService.UpdateAsync(user);
                products = products.Append($"\nJami:{totalPrice}");
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    $"{products}",
                    replyMarkup: KeyboardButtons.DeleteOrRemoveBasket(user.Basket.Items.Select(x => x.Product.Name)));
            }
        }

        private async Task HandleBackButtonAsync(User user, ITelegramBotClient botClient, Message message)
        {
            string action = user.UserProcess.ToString();
            if (action == "InCategory")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Buyurtmani o'zingiz olib keting, yoki Yetkazib berishni tanlang",
                replyMarkup: KeyboardButtons.SelectOrderType());
                user.UserProcess = UserProcess.DeliveryTypeRequest;
                await _userService.UpdateAsync(user);
            }
            else if (action == "InProduct")
            {
                await SendCategories(user, botClient, message);
            }
            else if (action == "InBasket")
            {
                await SendCategories(user, botClient, message);
            }
            else if (action == "AmountRequest")
            {
                await SendProducts(user, botClient, message, (int)user.ProcessHelper.CategoryId);
            }
            else
            {
                user.UserProcess = UserProcess.MainMenu;
                await _userService.UpdateAsync(user);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Juda yaxshi birgalikda buyurtma beramizmi? 😃", replyMarkup: KeyboardButtons.MainMenu());
            }
        }

        private async Task HandleInProductAsync(User user, ITelegramBotClient botClient, Message message)
        {

            Product product = await _productService.GetByNameAsync(message.Text);
            if (product is Product)
            {
                user.UserProcess = UserProcess.AmountRequest;
                user.ProcessHelper.ProductId = product.Id;
                user.ProcessHelper.CategoryId = product.CategoryId;
                await _userService.UpdateAsync(user);
                await botClient.SendPhotoAsync(
                    message.Chat.Id,
                    photo: InputFile.FromUri("https://scontent.fbhk1-3.fna.fbcdn.net/v/t1.6435-9/101732775_3166364486746355_5046697266992119808_n.jpg?_nc_cat=100&ccb=1-7&_nc_sid=c2f564&_nc_ohc=X8LV8qIcS1UAX9h5lVm&_nc_ht=scontent.fbhk1-3.fna&oh=00_AfCtHJrBx1U0UT87jgGl-8_1kDW5MncNESKhdyHmBgMD9Q&oe=6602D164"),
                    caption: $"{product.Name}({product.Description})\nNarxi: {product.Price}"
                    );
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "Miqdorini tanlang yoki kiriting:",
                    replyMarkup: KeyboardButtons.SelectAmountBtn());
            }
            else
            {

            }

        }

        private async Task HandleAmountRequestAsync(User user, ITelegramBotClient botClient, Message message)
        {
            var product = await _productService.GetByIdAsync((int)user.ProcessHelper.ProductId);
            if (int.TryParse(message.Text, out int amount))
            {
                if (user.Basket.Items.FirstOrDefault(x => x.ProductId == product.Id) is null)
                {
                    user.Basket.Items.Add(
                    new Item()
                    {
                        Product = product,
                        ProductId = product.Id,
                        Count = amount,
                        Basket = user.Basket
                    });
                }
                else
                {
                    user.Basket.Items.FirstOrDefault(x => x.Product.Id == product.Id)
                        .Count = user.Basket.Items.FirstOrDefault(x => x.Product.Id == product.Id).Count + amount;
                }
                await SendCategories(user, botClient, message);
            }

        }
        private async Task HandleLocationRequestAsync(User user, ITelegramBotClient botClient, Message message)
        {
            user.ProcessHelper.Longitute = message.Location.Longitude;
            user.ProcessHelper.Latitude = message.Location.Latitude;
            await SendCategories(user, botClient, message);
        }

        private async Task HandleContactRequestAsync(User user, ITelegramBotClient botClient, Message message)
        {
            if (message.Contact is null)
            {
                if (Regex.Match(message.Text, @"(?:[+][9]{2}[8][0-9]{2}[0-9]{3}[0-9]{2}[0-9]{2})").Success)
                {
                    user.PhoneNumber = message.Text;
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                     user.Id,
                   "Telefon raqam quyidagidek formatda bo'lishi kerak \"+998912345678\""
               );
                    logger.LogInformation($"Number doesn't match: {user.Id} {user.Name}");
                    return;
                }
            }
            else user.PhoneNumber = message.Contact.PhoneNumber;
            user.UserProcess = Entities.UserProcess.MainMenu;
            await _userService.UpdateAsync(user);
            await botClient.SendTextMessageAsync(
                user.Id,
                "Botdan foydalanishingiz mumkin 😊",
                replyMarkup: KeyboardButtons.MainMenu()
            );
        }

        private async Task HandleFullNameRequestAsync(User user, ITelegramBotClient botClient, Message message)
        {
            user.Name = message.Text;
            user.UserProcess = Entities.UserProcess.GoContactRequest;
            await _userService.UpdateAsync(user);

            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                $"{user.Name}, iltimos kontaktingizni yoki telefon raqamingizni jo'nating (misol: +998912345678):\n",
                replyMarkup: KeyboardButtons.SendContactRequest());
        }

        private async Task HandleUnknownCommand(User user, ITelegramBotClient botClient, Message message)
        {
            user.UserProcess = UserProcess.MainMenu;
            await _userService.UpdateAsync(user);
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Botga xush kelibsiz",
                replyMarkup: KeyboardButtons.MainMenu()
                );
        }
        private async Task HandleOrderButtonAsync(User user, ITelegramBotClient botClient, Message message)
        {
            user.UserProcess = UserProcess.DeliveryTypeRequest;
            user.Basket.Items.Clear();
            await _userService.UpdateAsync(user);
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Buyurtmani o'zingiz olib keting, yoki Yetkazib berishni tanlang",
                replyMarkup: KeyboardButtons.SelectOrderType());
        }
        private async Task SendCategories(User user, ITelegramBotClient botClient, Message message)
        {
            var categories = await _categoryService.GetAllAsync();
            user.UserProcess = UserProcess.InCategory;
            await _userService.UpdateAsync(user);
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Nimadan boshlaymiz?",
                replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList(),
                new List<KeyboardButton> { KeyboardButtons.PlaceAnOrderButton(), KeyboardButtons.BasketButton(), KeyboardButtons.BackButton() }));
        }
        private async Task SendProducts(User user, ITelegramBotClient botClient, Message message, int id)
        {
            var categories = await _categoryService.GetByIdAsync(id);
            await botClient.SendTextMessageAsync(message.Chat.Id, "Menyu bilan tanishish uchun «⏬ Ruyxat» ni bosing yoki taomni tanlang", replyMarkup: KeyboardButtons.MakeReplyMarkup(
                categories.Products.Select(x => x.Name).ToList(),
                new List<KeyboardButton> { KeyboardButtons.PlaceAnOrderButton(), KeyboardButtons.BasketButton(), KeyboardButtons.BackButton() }));
            user.UserProcess = UserProcess.InProduct;
            await _userService.UpdateAsync(user);
        }
    }
}
