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
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeButtons(categories.Select(x => x.Name).ToList())
                        );
                }
                else if (message.Text == "⚙️ Sozlamalar")
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeButtons(categories.Select(x => x.Name).ToList())
                        );
                }
                else if (message.Text == "☎️ Biz bilan aloqa")
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeButtons(categories.Select(x => x.Name).ToList())
                        );
                }
                else if (message.Text == "✍️ Fikr bildirish")
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeButtons(categories.Select(x => x.Name).ToList())
                        );
                }
                else if (message.Text == "ℹ️ Ma'lumot")
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeButtons(categories.Select(x => x.Name).ToList())
                        );
                }
                else if (message.Text == "Shashliklar")
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeButtons(categories.Select(x => x.Name).ToList())
                        );
                }

                else
                {
                    var categories = await _categoryService.GetAllAsync();
                    await botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Menyu",
                        replyMarkup: KeyboardButtons.MakeButtons(categories.Select(x => x.Name).ToList())
                        );
                }
            }
        }
        private async Task HandleCategory(string text, ITelegramBotClient botClient, Message message)
        {
            var categories = await _categoryService.GetAllAsync();
            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Menyu",
                replyMarkup: KeyboardButtons.MakeButtons(categories.Select(x => x.Name).ToList())
                );
        }
    }
}
