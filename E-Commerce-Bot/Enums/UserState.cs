namespace E_Commerce_Bot.Enums
{
    public static class UserState
    {
        #region Register
        public const int SEND_GREETING = 1;
        public const int SELECT_LANGUAGE = 2;
        public const int CONTACT_REQUEST = 3;
        public const int VERIFY_CODE = 4;
        public const int FULLNAME = 5;
        #endregion

        #region UserMainMenu
        public const int MAIN_MENU = 6;
        public const int SETTINGS = 7;
        #endregion

        #region UserOrdering
        public const int SELECT_DELIVERY_TYPE = 8;
        public const int IN_DELIVERY = 9;
        public const int IN_PICK_UP = 10;
        public const int LOCATION_REQUEST = 11;
        public const int IN_CATEGORY = 12;
        public const int IN_PRODUCT = 13;
        public const int AMOUNT_REQUEST = 14;
        public const int ON_COMMENT_TO_ORDER = 15;
        public const int ON_SELECT_PAYMENT_TYPE = 16;
        public const int AT_CONFIRMATION_AMOUNT = 17;
        public const int IN_PAYMENT_REQUEST = 18;
        public const int IN_PAYMENT_PROCESS = 18;
        public const int IN_BASKET = 19;
        #endregion

        #region UserSettings
        public const int SELECT_LANGUAGE_IN_SETTINGS = 20;
        public const int CHANGE_PHONE_IN_SETTINGS = 21;
        #endregion

        #region Admin
        public const int ADMIN_MENU = 22;
        public const int AdminInCategory = 23;
        #endregion
    }
}
