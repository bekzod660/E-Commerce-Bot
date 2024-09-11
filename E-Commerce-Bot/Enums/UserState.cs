namespace E_Commerce_Bot.Enums
{
    public enum UserState
    {
        #region Register
        sendGreeting,
        selectLanguage,
        contactRequest,
        verifyCode,
        fullName,
        #endregion

        #region UserMainMenu
        mainMenu,
        inSettings,
        #endregion

        #region UserOrdering
        selectDeliveryType,
        inDelivery,
        inPickUp,
        locationRequest,
        inCategory,
        inProduct,
        amountRequest,
        onCommentOrder,
        onSelectPaymentType,
        atConfirmationOrder,
        inPaymentProcess,
        inBasket,
        #endregion

        #region UserSettings
        SelectLanguageInSettings,
        changePhoneInSettings,
        #endregion

        #region Admin
        AdminMenu,
        InCategory
        #endregion
    }
}
