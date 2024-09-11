using E_Commerce_Bot.DTOs;
using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Enums;
using E_Commerce_Bot.Helpers;
using E_Commerce_Bot.Persistence.Repositories;
using E_Commerce_Bot.Recources;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace E_Commerce_Bot.Services
{
    public class BotResponseService : IBotResponseService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IBaseRepository<Entities.User> _userRepo;
        private readonly ILocalizationHandler localization;
        private readonly IBaseRepository<Category> _categoryRepo;

        public BotResponseService(ITelegramBotClient botClient, IBaseRepository<Entities.User> userRepo, ILocalizationHandler localization, IBaseRepository<Category> categoryRepo)
        {
            _botClient = botClient;
            _userRepo = userRepo;
            this.localization = localization;
            _categoryRepo = categoryRepo;
        }
        #region Register
        public async Task SendContactRequestAsync(long userId)
        {
            var message = await _botClient.SendTextMessageAsync(
            text: localization.GetValue(Recources.Message.SendContactRequest),
            replyMarkup: new ReplyKeyboardMarkup(KeyboardButton.WithRequestContact(localization.GetValue(Button.ContactRequest))) { ResizeKeyboard = true },
            chatId: userId);
        }

        public async Task SendGreetingAsync(long userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            var name = user.Name;

            //logger.LogTrace("Sending a greeting to {name}", name);

            var message = await _botClient.SendTextMessageAsync(
            text: localization.GetValue(Recources.Message.Greeting, name),
                chatId: userId);
        }

        public async Task SendLangugaesAsync(long userId)
        {
            var markup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("O'zbekcha🇺🇿"),
                new KeyboardButton("English🇬🇧"),
                new KeyboardButton("Русский🇷🇺")
            })
            { ResizeKeyboard = true };
            var message = await _botClient.SendTextMessageAsync(
               text: localization.GetValue(Recources.Message.SelectLanguage),
               chatId: userId,
               replyMarkup: markup);
        }
        #endregion       

        public async Task SendMessageAsync(long userId, string message)
        {
            await _botClient.SendTextMessageAsync(
                     userId,
                   localization.GetValue(message),
                   replyMarkup: new ReplyKeyboardRemove());
        }

        public async Task SendMainMenuAsync(long userId)
        {
            var matrix = new[]
            {
                new[] { Button.Order },                    // First row with 1 button
                new[] { Button.Branches, Button.MyOrders }, // Second row with 2 buttons
                new[] { Button.FeedBack, Button.ContactUs }, // Second row with 2 buttons
                new[] { Button.Information, Button.Settings } // Third row with 2 buttons
            };

            await _botClient.SendTextMessageAsync(
                userId,
                $"{localization.GetValue(Recources.Message.Wellcome)}",
                replyMarkup: GetReplyKeyboardMarkup(matrix));
        }

        #region OrderMenu
        public async Task SendDeliveryTypesAsync(long userId)
        {
            var matrix = new[]
           {
               new[] {Button.Delivery, Button.PickUp },
               new[]{ localization.GetValue(Button.Back)}
            };
            await _botClient.SendTextMessageAsync(
                userId,
                $"{localization.GetValue(Recources.Message.SelectDeliveryType)}",
                replyMarkup: GetReplyKeyboardMarkup(matrix));
        }

        public async Task SendLocationRequestAsync(long userId)
        {
            var markup = new ReplyKeyboardMarkup(new[]
            {
                new[]
                    {
                        KeyboardButton.WithRequestLocation($"{localization.GetValue(Button.SendLocation)}")
                    },
                new[]
                    {
                       new KeyboardButton( localization.GetValue(Button.Back))
                    }
            })
            {
                ResizeKeyboard = true
            };
            await _botClient.SendTextMessageAsync(
               userId,
               $"{localization.GetValue(Recources.Message.LocationRequest)}",
               replyMarkup: markup);
        }

        public async Task SendCategoriesAsync(long userId, string language)
        {
            List<Category> categories = await _categoryRepo.GetAllAsync();
            List<string> _categories = Translator.Translate(language, categories);
            await _botClient.SendTextMessageAsync(
               userId,
               $"{localization.GetValue(Recources.Message.Incategoory)}",
               replyMarkup: GetReplyKeyboardMarkup(new[] { new[] {localization.GetValue(Button.Back),
                   localization.GetValue(Button.Basket) },_categories.ToArray() }));
        }

        public async Task SendProductsAsync(long userId, List<string> products)
        {
            var matrix = new[]
            {
                new[]{Button.Back, Button.Basket},
                products.ToArray()
            };
            await _botClient.SendTextMessageAsync(
               userId,
               $"{localization.GetValue(Recources.Message.Incategoory)}",
               replyMarkup: GetReplyKeyboardMarkup(matrix));
        }

        public async Task SendProductAsync(long userId, ProductDto product)
        {
            await _botClient.SendPhotoAsync(
                    userId,
                    photo: Telegram.Bot.Types.InputFile.FromUri("https://scontent.fbhk1-3.fna.fbcdn.net/v/t1.6435-9/101732775_3166364486746355_5046697266992119808_n.jpg?_nc_cat=100&ccb=1-7&_nc_sid=c2f564&_nc_ohc=X8LV8qIcS1UAX9h5lVm&_nc_ht=scontent.fbhk1-3.fna&oh=00_AfCtHJrBx1U0UT87jgGl-8_1kDW5MncNESKhdyHmBgMD9Q&oe=6602D164"),
                    caption: $"{product.Name}({product.Description})\nNarxi: {product.Price}"
                    );
        }

        public async Task SendAmountRequestAsync(long userId)
        {
            var matrix = new string[][]
              {
                new [] { "1", "2", "3" },
                new [] { "4", "5", "6" },
                new [] { "7", "8", "9" },
                new [] { localization.GetValue(Button.Back), localization.GetValue(Button.Back)}
               };

            await _botClient.SendTextMessageAsync(
             userId,
             $"{localization.GetValue(Recources.Message.InAmount)}",
             replyMarkup: GetReplyKeyboardMarkup(matrix));
        }

        public async Task SendCommentRequest(long userId)
        {
            var matrix = new string[][]
            {
                new[] {localization.GetValue(Button.CommentToOrder)},
                 new[] { localization.GetValue(Button.Back), localization.GetValue(Button.Back) }
            };
            await _botClient.SendTextMessageAsync(
             userId,
             $"{localization.GetValue(Recources.Message.OnCommentToOrder)}",
             replyMarkup: GetReplyKeyboardMarkup(matrix));
        }
        #endregion

        #region Settings
        public async Task SendSettingsMenuAsync(long userId, string userLanguage)
        {
            var markup = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton($"{GetFlag(userLanguage)} {localization.GetValue(Button.ChangeLanguage)}"),
                    new KeyboardButton(localization.GetValue(Button.ChangePhone))
                },
                new[]
                {
                    new KeyboardButton(localization.GetValue(Button.Back))
                }
            })
            {
                ResizeKeyboard = true
            };

            await _botClient.SendTextMessageAsync(
                userId,
                $"{localization.GetValue(Recources.Button.Settings)}",
                replyMarkup: markup);
        }
        #endregion
        public async Task InValidPhoneNumberAsync(long userId)
        {
            await _botClient.SendTextMessageAsync(
                     userId,
                   $"{localization.GetValue(Recources.Message.InValidPhoneNumber)} +998912345678");
        }
        private ReplyKeyboardMarkup GetReplyKeyboardMarkup(string[][] buttons)
        {
            var markup = new KeyboardButton[buttons.GetLength(0)][];

            for (int i = 0; i < buttons.GetLength(0); i++)
            {
                markup[i] = buttons[i].Select(x => new KeyboardButton(localization.GetValue(x))).ToArray();
            }
            return new ReplyKeyboardMarkup(markup) { ResizeKeyboard = true };
        }
        private string GetCurrentLanguage(string userLanguage, string language)
            => string.Equals(userLanguage, language, StringComparison.CurrentCultureIgnoreCase) ? "✅" : string.Empty;
        private string GetFlag(string language)
        {
            return language switch
            {
                "uz" => "🇺🇿",
                "ru" => "🇷🇺",
                "en" => "🇬🇧"
            };
        }
        private InlineKeyboardMarkup GetInlineKeyboard(string[][] matrix)
        {
            var buttonMatrix = new InlineKeyboardButton[matrix.GetLength(0)][];
            for (int i = 0; i < matrix.GetLength(0); i++)
                buttonMatrix[i] = matrix[i]
                    .Select(x => InlineKeyboardButton.WithCallbackData(localization.GetValue(x), x)).ToArray();

            return new InlineKeyboardMarkup(buttonMatrix);
        }
        #region Admin 
        public async Task SendAdminMainMenu(long userId)
        {
            var matrix = new[]
            {
                new[]{ Button.Categories},
                new[]{Button.Orders, Button.Settings}
            };
            await _botClient.SendTextMessageAsync(
             chatId: userId,
             text: "<b>Salom</b>",
             replyMarkup: GetInlineKeyboard(matrix),
             parseMode: ParseMode.Html);
        }
        #endregion
        public async Task SendInlineCategoryAsync(long userId, string language)
        {
            List<Category> categories = await _categoryRepo.GetAllAsync();
            List<string> _categories = Translator.Translate(language, categories);
            await _botClient.SendTextMessageAsync(
               userId,
               $"{localization.GetValue(Recources.Message.Incategoory)}",
               replyMarkup: GetInlineKeyboard(new[] { new[] { localization.GetValue(Button.Back) }, _categories.ToArray() }));
        }

        #region Basket
        public async Task SendProductsBasket(long userId, Entities.User user)
        {
            await _botClient.SendTextMessageAsync(
               userId,
               localization.GetValue(Recources.Message.Basket));
            var products = new StringBuilder(localization.GetValue(Button.Basket));
            foreach (var item in user.Basket.Items)
            {
                products = products.Append($"\n{item.Product.Translate(user.Language).Name}\n {item.Count} * {item.Product.Price} = {item.Count * item.Product.Price}\n");
            }
            var buttons = new List<List<KeyboardButton>>();
            var _products = user.Basket.Items.Select(x => x.Product.Name_Uz);
            foreach (var product in _products)
            {
                buttons.Add(
                    new List<KeyboardButton>()
                        {
                            new KeyboardButton($"❌ {product}")
                        });
            }
            buttons.Add(
                new List<KeyboardButton>()
                {
                            new KeyboardButton(localization.GetValue(Button.Back)),
                            new KeyboardButton(localization.GetValue(Button.EmptyBasket))
                 });
            buttons.Add(
                new List<KeyboardButton>()
                {
                            new KeyboardButton("🚖 Buyurtuma berish")
                 });
            double totalPrice = user.Basket.Items.Select(x => x.Product.Price * x.Count).ToList().Sum();
            user.UserState = UserState.inBasket;
            await _userRepo.UpdateAsync(user);
            products = products.Append($"\n{localization.GetValue(Recources.Message.BasketAll)}:{totalPrice}");
            await _botClient.SendTextMessageAsync(
            user.Id,
            $"{products}",
            replyMarkup: new ReplyKeyboardMarkup(buttons));
        }
        #endregion
    }
}
