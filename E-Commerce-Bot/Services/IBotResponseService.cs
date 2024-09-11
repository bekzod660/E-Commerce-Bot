using E_Commerce_Bot.Entities;

namespace E_Commerce_Bot.Services
{
    public interface IBotResponseService
    {
        Task SendGreetingAsync(long userId);
        Task SendLangugaesAsync(long userId);
        Task SendContactRequestAsync(long userId);
        Task InValidPhoneNumberAsync(long userId);
        Task SendMainMenuAsync(long userId);
        Task SendSettingsMenuAsync(long userId, string userLanguge);
        Task SendMessageAsync(long userId, string message);
        Task SendDeliveryTypesAsync(long userId);
        Task SendLocationRequestAsync(long userId);
        Task SendCategoriesAsync(long userId, string language);
        Task SendProductsAsync(long userId, List<string> products);
        Task SendProduct(long userId);
        Task SendAmountRequestAsync(long userId);
        Task SendCommentRequest(long userId);
        #region Admin
        Task SendAdminMainMenu(long userId);
        Task SendInlineCategoryAsync(long userId, string language);
        #endregion

        #region Basket
        Task SendProductsBasket(long userId, User user);
        #endregion

    }
}
