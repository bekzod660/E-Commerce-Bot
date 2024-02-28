namespace E_Commerce_Bot.Entities
{
    public enum Process
    {
        None,
        MainMenu,
        LocationRequest,
        AddressRequest,
        FullNameRequest,
        ContactRequest,
        InCategory,
        DeliveryTypeRequest,
        InDelivery,
        InPickUp,
        AmountRequest,
        InProduct,
        InCart,
        OnCommentOrder,
        OnSelectPaymentType,
        AfterSelectPaymentType,
        AtConfirmationOrder
    }
}
