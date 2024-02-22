using E_Commerce_Bot.Services.Bot.Buttons;
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
                    Name = message.Chat.Username
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
                    if (user.UserProcess != Entities.Process.EnteringFullName)
                    {
                        user.Name = message.Text;
                        user.UserProcess = Entities.Process.SendingContact;
                        await _userService.UpdateAsync(user);
                        await botClient.SendTextMessageAsync(
                           message.Chat.Id,
                           $"{user.Id}, iltimos kontaktingizni yoki telefon raqamingizni jo'nating (misol: +998912345678):\n,"
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
            }
        }
    }
}
