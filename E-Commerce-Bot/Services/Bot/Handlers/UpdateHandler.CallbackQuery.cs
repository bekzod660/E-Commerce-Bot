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
            Task res = user.UserState switch
            {
                Enums.UserState.inCategory => _botResponseService.SendAdminMainMenu(user.Id),
                Enums.UserState.inSettings => _botResponseService.SendAdminMainMenu(user.Id),
                Enums.UserState.inDelivery => _botResponseService.SendAdminMainMenu(user.Id),
                Enums.UserState.inPaymentProcess => _botResponseService.SendAdminMainMenu(user.Id),
                Enums.UserState.inBasket => _botResponseService.SendAdminMainMenu(user.Id),
                Enums.UserState.fullName => _botResponseService.SendAdminMainMenu(user.Id),
                Enums.UserState.onSelectPaymentType => _botResponseService.SendAdminMainMenu(user.Id),
                Enums.UserState.onCommentOrder => _botResponseService.SendAdminMainMenu(user.Id),
                Enums.UserState.amountRequest => _botResponseService.SendAdminMainMenu(user.Id),
                Enums.UserState.atConfirmationOrder => _botResponseService.SendAdminMainMenu(user.Id),
                _ => _botResponseService.SendAdminMainMenu(user.Id)
            };
            await res;
        }

    }
}
