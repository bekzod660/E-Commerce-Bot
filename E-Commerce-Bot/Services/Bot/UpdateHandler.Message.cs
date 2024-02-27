using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Services.Bot.Buttons;
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
            Entities.User user = await _userService.GetByIdAsync(message.Chat.Id);
            if (message.Text == "/start" && (user is null))
            {
                await _userService.AddAsync(new User
                {
                    Id = message.Chat.Id,
                    Name = message.Chat.Username,
                    UserProcess = Entities.Process.FullNameRequest
                });

                await botClient.SendTextMessageAsync(
               message.Chat.Id,
                "Salom\n\nIltimos ismingizni kiriting:",
               parseMode: ParseMode.Html);
            }
            else if (user is not null)
            {
                if (user.UserProcess == Entities.Process.FullNameRequest)
                {
                    user.Name = message.Text;
                    user.UserProcess = Entities.Process.ContactRequest;
                    await _userService.UpdateAsync(user);
                    await botClient.SendTextMessageAsync(
                       message.Chat.Id,
                       $"{user.Name}, iltimos kontaktingizni yoki telefon raqamingizni jo'nating (misol: +998912345678):\n",
                        replyMarkup: KeyboardButtons.SendContactRequest()
                       );
                }

                else if (user.UserProcess == Entities.Process.ContactRequest)
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
                    // if (_cartService.GetByIdAsync(message.Chat.Id) is null || _cartService.GetByIdAsync(message.Chat.Id).Result.Items.Count == 0)
                    await botClient.SendTextMessageAsync(
                        user.Id,
                        "Botdan foydalanishingiz mumkin 😊",
                        replyMarkup: KeyboardButtons.MainMenu()
                    );
                }
                else if (message.Text == "🛍 Buyurtma berish")
                {
                    user.UserProcess = Process.DeliveryTypeRequest;
                    await _userService.UpdateAsync(user);
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Buyurtmani o'zingiz olib keting, yoki Yetkazib berishni tanlang",
                        replyMarkup: KeyboardButtons.InSelectOrderType());
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
                else if (message.Text == "⬅️ Ortga")
                {
                    string action = user.UserProcess.ToString();
                    if (action == "InCategory")
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Buyurtmani o'zingiz olib keting, yoki Yetkazib berishni tanlang", replyMarkup: KeyboardButtons.SelectedDeliveryType());
                    }
                    else if (action == "InProduct")
                    {
                        var categories = await _categoryService.GetAllAsync();
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Nimadan boshlaymiz?", replyMarkup: KeyboardButtons.MakeReplyMarkup(
                            categories.Select(x => x.Name).ToList()));
                    }
                    else if (action == "AmountRequest")
                    {
                        int c = (int)user.ProcessHelper.CategoryId;
                        var categories = await _categoryService.GetByIdAsync(c);
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Menyu bilan tanishish uchun «⏬ Ruyxat» ni bosing yoki taomni tanlang", replyMarkup: KeyboardButtons.MakeReplyMarkup(
                            categories.Products.Select(x => x.Name).ToList()));
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Juda yaxshi birgalikda buyurtma beramizmi? 😃", replyMarkup: KeyboardButtons.MainMenu());
                    }
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
                    user.UserProcess == Process.DeliveryTypeRequest)
                {
                    if (user.ProcessHelper is null)
                    {
                        user.ProcessHelper = new ProcessHelper();
                    }
                    user.ProcessHelper.Order = OrderType.Delivery;
                    user.UserProcess = Process.LocationRequest;
                    await _userService.UpdateAsync(user);
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "📍 Lokatsiya yuboring ..",
                        replyMarkup: KeyboardButtons.SendLocationRequest());

                }
                else if (message.Location != null &&
                    user.UserProcess == Process.LocationRequest)
                {
                    user.ProcessHelper.Longitute = message.Location.Longitude;
                    user.ProcessHelper.Latitude = message.Location.Latitude;
                    var categories = await _categoryService.GetAllAsync();
                    user.UserProcess = Process.InCategory;
                    await _userService.UpdateAsync(user);
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Nimadan boshlaymiz?",
                        replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList()));
                }
                else if (user.UserProcess == Process.InCategory)
                {
                    var category = await _categoryService.GetByNameAsync(message.Text);
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeReplyMarkup(category.Products.Select(x => x.Name).ToList())
                        );
                    user.UserProcess = Process.InProduct;
                    await _userService.UpdateAsync(user);
                }
                else if (user.UserProcess == Process.InProduct)
                {
                    var product = await _productService.GetByNameAsync(message.Text);
                    user.UserProcess = Process.AmountRequest;
                    user.ProcessHelper.ProductId = product.Id;
                    user.ProcessHelper.CategoryId = product.CategoryId;
                    await _userService.UpdateAsync(user);
                    await botClient.SendPhotoAsync(
                        message.Chat.Id,
                        photo: InputFile.FromUri("https://scontent.fbhk1-3.fna.fbcdn.net/v/t1.6435-9/101732775_3166364486746355_5046697266992119808_n.jpg?_nc_cat=100&ccb=1-7&_nc_sid=c2f564&_nc_ohc=X8LV8qIcS1UAX9h5lVm&_nc_ht=scontent.fbhk1-3.fna&oh=00_AfCtHJrBx1U0UT87jgGl-8_1kDW5MncNESKhdyHmBgMD9Q&oe=6602D164"),
                        caption: $"{product.Name}({product.Description})\nNarxi: {product.Price}"
                        );
                    await SelectAmountButton(botClient, message);
                }
                else if (user.UserProcess == Process.AmountRequest &&
                    int.TryParse(message.Text, out int amount))
                {
                    var product = await _productService.GetByIdAsync((int)user.ProcessHelper.ProductId);
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
                        ProductId = product.Id,
                        Count = amount

                    });

                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        user.Id,
                        "Davom etamizmi? 😉",
                        replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList()));
                    user.UserProcess = Process.InCategory;
                    await _userService.UpdateAsync(user);
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
                else if (message.Text != null)
                {
                    Task result = message.Text switch
                    {
                        _ => HandleUnknownCommand(botClient, message),
                    };
                    await result;
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
            else
            {
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "Start tugmasini bosing..",
                    replyMarkup: new ReplyKeyboardMarkup(
                        new KeyboardButton("/start")));
            }
        }
        private async Task HandleUnknownCommand(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Botga xush kelibsiz",
                replyMarkup: KeyboardButtons.MainMenu()
                );
        }

        private async Task SelectAmountButton(ITelegramBotClient botClient, Message message)
        {

        }

        private async Task Cart()
        {

        }
    }
}
