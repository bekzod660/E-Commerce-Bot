using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Services.Bot.Buttons;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = E_Commerce_Bot.Entities.User;

namespace E_Commerce_Bot.Services.Bot
{
    public partial class UpdateHandler
    {
        private async Task BotOnMessageRecieved(ITelegramBotClient botClient, Message? message)
        {
            Entities.User user = await _userService.GetByIdAsync(message.Chat.Id);
            if (message.Text == "/start" && (user is null))
            {
                await _userService.AddAsync(new User
                {
                    Id = message.Chat.Id,
                    Name = message.Chat.Username,
                    UserProcess = Entities.Process.EnteringFullName
                });

                await botClient.SendTextMessageAsync(
               message.Chat.Id,
                "Salom\n\nIltimos ismingizni kiriting:",
               parseMode: ParseMode.Html);
            }
            else
            {
                if (user.UserProcess != Entities.Process.None)
                {
                    if (user.UserProcess == Entities.Process.EnteringFullName)
                    {
                        user.Name = message.Text;
                        user.UserProcess = Entities.Process.SendingContact;
                        await _userService.UpdateAsync(user);
                        await botClient.SendTextMessageAsync(
                           message.Chat.Id,
                           $"{user.Name}, iltimos kontaktingizni yoki telefon raqamingizni jo'nating (misol: +998912345678):\n",
                            replyMarkup: KeyboardButtons.SendContactRequest()
                           );
                    }

                    else if (user.UserProcess == Entities.Process.SendingContact)
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
                        user.UserProcess = Entities.Process.None;
                        await _userService.UpdateAsync(user);
                        if (_cartService.GetByIdAsync(message.Chat.Id) is null || _cartService.GetByIdAsync(message.Chat.Id).Result.Items.Count == 0)
                            await botClient.SendTextMessageAsync(
                                user.Id,
                                "Botdan foydalanishingiz mumkin 😊",
                                replyMarkup: KeyboardButtons.MainMenu()
                            );
                    }
                }
                else if (message.Text == "🛍 Buyurtma berish")
                {
                    user.UserProcess = Process.InSelectDeliveryType;
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Buyurtmani o'zingiz olib keting, yoki Yetkazib berishni tanlang",
                        replyMarkup: KeyboardButtons.SelectedDeliveryType());
                }
                else if (message.Text == "⚙️ Sozlamalar")
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList())
                        );
                }
                else if (message.Text == "☎️ Biz bilan aloqa")
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList())
                        );
                }
                else if (message.Text == "✍️ Fikr bildirish")
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList())
                        );
                }
                else if (message.Text == "ℹ️ Ma'lumot")
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList())
                        );
                }
                else if (message.Text == "🚖 Yetkazib berish" &&
                    user.UserProcess == Process.InSelectDeliveryType)
                {
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "📍 Lokatsiya yuboring ..",
                        replyMarkup: KeyboardButtons.SendLocationRequest());

                }
                else if (message.Location != null)
                {

                }
                else if (message.Text == "Shashliklar")
                {
                    await HandleCategoryAsync(message.Text, botClient, message);
                }
                else if (message.Text == "Jaz")
                {
                    var product = await _productService.GetByNameAsync("Jaz");
                    if (product != null)
                    {
                        if (user.Cart is null)
                        {
                            user.Cart = new Entities.Cart();
                        }
                        if (user.Cart.Items is null)
                        {
                            user.Cart.Items = new List<Item>();
                        }
                        user.Cart.Items.Add(new Item()
                        {
                            Product = product,

                        });
                    }
                    user.UserProcess = Process.InSelectAmount;
                    await _userService.UpdateAsync(user);
                    await HandleProductAsync(message.Text, botClient, message);
                    await SelectAmountButton(botClient, message);
                }
                else if (message.Text == "↪️ Orqaga")
                {
                    var action = user.UserProcess.ToString();
                    await HandleBackButton(botClient, message, action);

                }
                else if (message.Text == "🛒 Savatcha")
                {
                    string products = "";
                    foreach (var item in user.Cart.Items)
                    {
                        products = products + $"{item.Product.Name}\n";
                    }
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        $"{products}");
                }
                else if (user.UserProcess == Process.InSelectAmount && int.TryParse(message.Text, out int amount))
                {

                    user.Cart.Items.Add(new Entities.Item
                    {
                        Count = amount
                    });
                    await _userService.UpdateAsync(user);
                }
                else
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList())
                        );
                }
            }
        }
        private async Task HandleCategoryAsync(string text, ITelegramBotClient botClient, Message message)
        {
            var category = await _categoryService.GetByNameAsync(text);
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Menyu",
                replyMarkup: KeyboardButtons.MakeReplyMarkup(category.Products.Select(x => x.Name).ToList())
                );
        }
        private async Task HandleProductAsync(string text, ITelegramBotClient botClient, Message message)
        {
            var product = await _productService.GetByNameAsync(text);
            await botClient.SendPhotoAsync(
                message.Chat.Id,
                photo: InputFile.FromUri("https://scontent.fbhk1-3.fna.fbcdn.net/v/t1.6435-9/101732775_3166364486746355_5046697266992119808_n.jpg?_nc_cat=100&ccb=1-7&_nc_sid=c2f564&_nc_ohc=X8LV8qIcS1UAX9h5lVm&_nc_ht=scontent.fbhk1-3.fna&oh=00_AfCtHJrBx1U0UT87jgGl-8_1kDW5MncNESKhdyHmBgMD9Q&oe=6602D164"),
                caption: $"{product.Name}({product.Description})\nNarxi: {product.Price}"
                );
        }
        private async Task HandleBackButton(ITelegramBotClient botClient,
                                              Message message,
                                              string action)
        {
            var result = action switch
            {
                "MainMenu" => await botClient.SendTextMessageAsync(message.Chat.Id, ",", replyMarkup: KeyboardButtons.MainMenu()),
                "InSelectDeliveryType" => await botClient.SendTextMessageAsync(message.Chat.Id, ",", replyMarkup: KeyboardButtons.MainMenu()),
                "InDelivery" => await botClient.SendTextMessageAsync(message.Chat.Id, ",", replyMarkup: KeyboardButtons.MainMenu()),
                "InPickUp" => await botClient.SendTextMessageAsync(message.Chat.Id, ",", replyMarkup: KeyboardButtons.MainMenu()),
                "InSelectAmount" => await botClient.SendTextMessageAsync(message.Chat.Id, ",", replyMarkup: KeyboardButtons.MainMenu())
            };
        }

        private async Task SelectAmountButton(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Miqdorini tanlang yoki kiriting:",
                replyMarkup: KeyboardButtons.SelectAmountBtn());
        }
    }
}
