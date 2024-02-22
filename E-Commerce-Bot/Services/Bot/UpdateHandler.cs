using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace E_Commerce_Bot.Services.Bot
{
    public partial class UpdateHandler : IUpdateHandler
    {
        private readonly ILogger<UpdateHandler> logger;
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly CategoryService _categoryService;
        private readonly CartService _cartService;

        public UpdateHandler(CartService cartService, CategoryService categoryService, OrderService orderService, ProductService productService, UserService userService, ILogger<UpdateHandler> logger)
        {
            _cartService = cartService;
            _categoryService = categoryService;
            _orderService = orderService;
            _productService = productService;
            _userService = userService;
            this.logger = logger;
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageRecieved(botClient, update.Message),
                UpdateType.CallbackQuery => BotOnCallbackQuery(botClient, update.CallbackQuery)
            };

            try
            {
                await handler;
            }
            catch (Exception ex)
            {

            }
        }




    }
}
