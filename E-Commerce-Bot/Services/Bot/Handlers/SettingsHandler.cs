using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Persistence.Repositories;
using Telegram.Bot.Types;
using User = E_Commerce_Bot.Entities.User;

namespace E_Commerce_Bot.Services.Bot.Handlers
{
    public class SettingsHandler
    {
        private readonly IBaseRepository<Entities.User> _userRepo;
        private readonly IBaseRepository<Entities.Category> _categoryRepo;
        private readonly IBaseRepository<Entities.Product> _productRepo;
        private readonly IBotResponseService _botResponseService;
        private readonly ILocalizationHandler localization;

        public SettingsHandler(IBaseRepository<Entities.User> userRepo,
            IBotResponseService botResponseService,
            ILocalizationHandler localization,
            IBaseRepository<Product> productRepo)
        {
            _userRepo = userRepo;
            _botResponseService = botResponseService;
            this.localization = localization;
            _productRepo = productRepo;
        }

        public async Task HandleSettingsAsync(User user, Message message)
        {
            await _botResponseService.SendSettingsMenuAsync(user.Id, user.Language);
            user.UserState = Enums.UserState.inSettings;
            await _userRepo.UpdateAsync(user);
        }
        public async Task HandleChangeLanguageAsync(User user, Message message)
        {
            await _botResponseService.SendLangugaesAsync(user.Id);
            user.UserState = Enums.UserState.SelectLanguageInSettings;
            await _userRepo.UpdateAsync(user);
        }
        public async Task HandleChangePhoneAsync(User user)
        {
            await _botResponseService.SendContactRequestAsync(user.Id);
            user.UserState = Enums.UserState.changePhoneInSettings;
            await _userRepo.UpdateAsync(user);
        }
    }
}
