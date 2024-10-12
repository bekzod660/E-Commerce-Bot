using E_Commerce_Bot.Extensions;
using E_Commerce_Bot.Recources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace E_Commerce_Bot.Services.Bot
{
    public partial class UpdateHandler
    {
        private async Task BotOnCallbackQuery(ITelegramBotClient client, CallbackQuery query, CancellationToken cancellationToken = default)
        {
            Entities.User user = await _userRepo.GetByIdAsync(query.Message.Chat.Id);
            var task = query.Data switch
            {
                _ when query.Data == Button.Back
                     => HandleBackButtonAsync(user),
                _ when query.Data == Button.Categories
                    => _botResponseService.SendInlineCategoryAsync(query.Message.Chat.Id, "uz"),

            };
            await client.DeleteMessageAsync(query.Message.Chat.Id, query.Message.MessageId, cancellationToken);
            await task;
        }
        public async Task HandleBackButtonAsync(Entities.User user)
        {
            user = user.UserStateManager();
            await _userRepo.UpdateAsync(user);
            Task res = user.UserStateId switch
            {
                5 => _botResponseService.SendAdminMainMenu(user.Id),
                6 => _botResponseService.SendAdminMainMenu(user.Id),
                7 => _botResponseService.SendAdminMainMenu(user.Id),
                8 => _botResponseService.SendAdminMainMenu(user.Id),
                9 => _botResponseService.SendAdminMainMenu(user.Id),
                10 => _botResponseService.SendAdminMainMenu(user.Id),
                11 => _botResponseService.SendAdminMainMenu(user.Id),
                12 => _botResponseService.SendAdminMainMenu(user.Id),
                13 => _botResponseService.SendAdminMainMenu(user.Id),
                14 => _botResponseService.SendAdminMainMenu(user.Id),
                _ => _botResponseService.SendAdminMainMenu(user.Id)
            };
            await res;
        }

    }
}
