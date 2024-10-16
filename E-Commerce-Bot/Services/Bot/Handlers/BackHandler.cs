﻿using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Extensions;
using E_Commerce_Bot.Persistence.Repositories;
using Telegram.Bot;

namespace E_Commerce_Bot.Services.Bot.Handlers
{
    public class BackHandler
    {
        private readonly IBaseRepository<Entities.User> _userRepo;
        private readonly IBaseRepository<Entities.Category> _categoryRepo;
        private readonly IBaseRepository<Entities.Product> _productRepo;
        private readonly IBotResponseService _botResponseService;
        private readonly ILocalizationHandler localization;
        private readonly ITelegramBotClient _botClient;

        public BackHandler(IBaseRepository<Entities.User> userRepo,
            IBotResponseService botResponseService, ILocalizationHandler localization, IBaseRepository<Product> productRepo, ITelegramBotClient botClient)
        {
            _userRepo = userRepo;
            _botResponseService = botResponseService;
            this.localization = localization;
            _productRepo = productRepo;
            _botClient = botClient;
        }

        public async Task HandleBackButtonAsync(User user)
        {
            user = user.UserStateManager();
            await _userRepo.UpdateAsync(user);
            Task res = user.UserStateId switch
            {
                2 => _botResponseService.SendMainMenuAsync(user.Id),
                3 => _botResponseService.SendMainMenuAsync(user.Id),
                4 => _botResponseService.SendMainMenuAsync(user.Id),
                8 => _botResponseService.SendMainMenuAsync(user.Id),
                9 => _botResponseService.SendCategoriesAsync(user.Id, user.Language),
                10 => _botResponseService.SendMainMenuAsync(user.Id),
                11 => _botResponseService.SendMainMenuAsync(user.Id),
                12 => _botResponseService.SendMainMenuAsync(user.Id),
                13 => _botResponseService.SendMainMenuAsync(user.Id),
                14 => _botResponseService.SendMainMenuAsync(user.Id),
                _ => _botResponseService.SendMainMenuAsync(user.Id)
            };
            await res;
        }
    }
}
