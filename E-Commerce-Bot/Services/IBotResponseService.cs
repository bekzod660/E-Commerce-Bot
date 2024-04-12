namespace E_Commerce_Bot.Services
{
    public interface IBotResponseService
    {
        Task SendGreeting(long userId);
        Task SendLangugaes(long userId);
        Task SendContactRequest(long userId);
        Task InValidPhoneNumber(long userId);
        Task SenMainMenu(long userId);
        Task SendMessages(long userId, string message);
        Task SendDeliveryTypes(long userId);
        Task SendLocationRequest(long userId);
        Task SendCategories(long userId, string language);
        Task SendProducts(long userId, List<string> products);
        Task SendProduct(long userId);
        Task SendAmountRequest(long userId);
        Task SendCommentRequest(long userId);
    }
}
