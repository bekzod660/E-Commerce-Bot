using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Enums;

namespace E_Commerce_Bot.Extensions
{
    public static class UserExtensions
    {
        public static User UserStateManager(this User user)
        {
            user.UserState = user.UserState switch
            {
                UserState.InCategory => UserState.AdminMenu,
                Enums.UserState.inCategory => UserState.mainMenu,
                Enums.UserState.inSettings => UserState.mainMenu,
                Enums.UserState.inDelivery => UserState.mainMenu,
                Enums.UserState.inPaymentProcess => UserState.mainMenu,
                Enums.UserState.inBasket => UserState.mainMenu,
                Enums.UserState.fullName => UserState.mainMenu,
                Enums.UserState.onSelectPaymentType => UserState.mainMenu,
                Enums.UserState.onCommentOrder => UserState.mainMenu,
                Enums.UserState.amountRequest => UserState.mainMenu,
                Enums.UserState.atConfirmationOrder => UserState.mainMenu,
                _ => UserState.mainMenu
            };
            return user;
        }
    }
}
