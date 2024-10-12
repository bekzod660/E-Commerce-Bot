using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Enums;

namespace E_Commerce_Bot.Extensions
{
    public static class UserExtensions
    {
        public static User UserStateManager(this User user)
        {
            user.UserStateId = user.UserStateId switch
            {
                Enums.UserState.AdminInCategory => UserState.ADMIN_MENU,
                Enums.UserState.IN_CATEGORY => UserState.MAIN_MENU,
                Enums.UserState.SETTINGS => UserState.MAIN_MENU,
                Enums.UserState.IN_DELIVERY => UserState.MAIN_MENU,
                Enums.UserState.IN_PAYMENT_PROCESS => UserState.MAIN_MENU,
                Enums.UserState.IN_BASKET => UserState.MAIN_MENU,
                Enums.UserState.FULLNAME => UserState.MAIN_MENU,
                Enums.UserState.ON_SELECT_PAYMENT_TYPE => UserState.MAIN_MENU,
                Enums.UserState.ON_COMMENT_TO_ORDER => UserState.MAIN_MENU,
                Enums.UserState.AMOUNT_REQUEST => UserState.MAIN_MENU,
                Enums.UserState.AT_CONFIRMATION_AMOUNT => UserState.MAIN_MENU,
                _ => UserState.MAIN_MENU
            };
            return user;
        }
    }
}
