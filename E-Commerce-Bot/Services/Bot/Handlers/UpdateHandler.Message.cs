using E_Commerce_Bot.Entities;
using E_Commerce_Bot.Enums;
using E_Commerce_Bot.Helpers;
using E_Commerce_Bot.Recources;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Message = Telegram.Bot.Types.Message;
using User = E_Commerce_Bot.Entities.User;

namespace E_Commerce_Bot.Services.Bot
{
    public partial class UpdateHandler
    {
        private async Task BotOnMessageRecieved(ITelegramBotClient botClient, Message? message)
        {
            if (message is null) return;
            Entities.User user = await _userRepo.GetByIdAsync(message.Chat.Id);
            if (message.Text == "/start" && user.UserStateId == UserState.SEND_GREETING)
            {
                await _botResponseService.SendGreetingAsync(user.Id);
                await _botResponseService.SendLangugaesAsync(user.Id);
                user.UserStateId = UserState.SELECT_LANGUAGE;
                await _userRepo.UpdateAsync(user);
            }
            else if (message.Text == _localization.GetValue(Button.Basket))
            {
                await _basketHandler.HandleBasketButtonAsync(user, message);
            }
            else if (message.Text == _localization.GetValue(Button.Back))
            {
                await _backHandler.HandleBackButtonAsync(user);
            }
            else if (user is User)
            {
                await HandleUserProcess(user, botClient, message);
            }
            else if (user is null)
            {
                await HandleUnknownCommandAsync(user, message);
            }

        }
        private async Task HandleUserProcess(User user, ITelegramBotClient botClient, Message message)
        {
            Task res = user.UserStateId switch
            {
                2 => HandleSelectLanguageAsync(user, message),
                3 => HandleContactRequestAsync(user, message),
                4 => HandleVerifyCodeAsync(user, botClient, message),
                5 => HandleFullNameRequestAsync(user, message),
                6 => HandleMainMenuAsync(user, message),
                7 => HandleSettingsAsync(user, message),
                20 => HandleSelectLanguageInSettingsAsync(user, message),
                21 => HandleChangePhoneAsync(user, message),
                8 => _orderHandler.HandleInDeliveryTypeRequestAsync(user, message),
                11 => _orderHandler.HandleLocationRequestAsync(user, message),
                12 => _orderHandler.HandleInCategoryAsync(user, message),
                13 => _orderHandler.HandleInProductAsync(user, message),
                14 => _orderHandler.HandleAmountRequestAsync(user, message),
                19 => _basketHandler.HandleActionInBasketAsync(user, message),
                15 => _orderHandler.HandleOnCommentOrderAsync(user, message),
                16 => _orderHandler.HandleOnSelectPaymentTypeAsync(user, message),
                17 => _orderHandler.HandleAtConfirmationOrderAsync(user, message),
                _ => HandleUnknownCommandAsync(user, message)
            };
            await res;
        }

        #region ActionsInSettings
        private async Task HandleSelectLanguageInSettingsAsync(User user, Message message)
        {
            user.UserStateId = UserState.MAIN_MENU;
            user.Language = message.Text switch
            {
                "O'zbekcha🇺🇿" => "uz",
                "English🇬🇧" => "en",
                "Русский🇷🇺" => "ru"
            };
            SetCulture.SetUserCulture(user.Language);
            await _botResponseService.SendMainMenuAsync(user.Id);
            await _userRepo.UpdateAsync(user);
        }
        private async Task HandleChangePhoneAsync(User user, Message message)
        {
            string testCode = "1111";
            if (message.Contact is null)
            {
                if (Regex.Match(message.Text, @"(?:[+][9]{2}[8][0-9]{2}[0-9]{3}[0-9]{2}[0-9]{2})").Success)
                {

                    //await SendSms(message.Contact.PhoneNumber, code); //avaible when sms working
                    await _botResponseService.SendMessageAsync(user.Id, _localization.GetValue(Recources.Message.EnterCode));
                    user.UserStateId = UserState.VERIFY_CODE;
                    user.Code = testCode;
                    user.PhoneNumber = message.Text;
                    await _userRepo.UpdateAsync(user);
                }
                else
                {
                    await _botResponseService.InValidPhoneNumberAsync(user.Id);
                }
            }
            else
            {
                //  await SendSms(message.Contact.PhoneNumber, code);
                await _botResponseService.SendMessageAsync(user.Id, _localization.GetValue(Recources.Message.EnterCode));
                user.UserStateId = UserState.VERIFY_CODE;
                user.Code = testCode;
                user.PhoneNumber = message.Contact.PhoneNumber;
                await _userRepo.UpdateAsync(user);
            }
        }
        private async Task HandleSettingsAsync(User user, Message message)
        {
            if (message.Text.Contains(_localization.GetValue(Button.ChangeLanguage)))
            {
                await _settingsHandler.HandleChangeLanguageAsync(user, message);
            }
            else if (message.Text == _localization.GetValue(Button.ChangePhone))
            {
                await _settingsHandler.HandleSettingsAsync(user, message);
            }
        }
        #endregion

        #region RegisterProcess
        private async Task HandleSelectLanguageAsync(User user, Message message)
        {
            user.UserStateId = UserState.CONTACT_REQUEST;
            user.Language = message.Text switch
            {
                "O'zbekcha🇺🇿" => "uz",
                "English🇬🇧" => "en",
                "Русский🇷🇺" => "ru"
            };
            SetCulture.SetUserCulture(user.Language);
            await _botResponseService.SendContactRequestAsync(user.Id);
            await _userRepo.UpdateAsync(user);
        }
        private async Task HandleVerifyCodeAsync(User user, ITelegramBotClient botClient, Message message)
        {
            if (string.Equals(user.Code, message.Text))
            {
                user.UserStateId = UserState.FULLNAME;
                user.PhoneNumber = user.ProcessHelper.UserPhoneNumber;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendMessageAsync(user.Id, _localization.GetValue(Recources.Message.FullName));
            }
            else
            {
                await _botResponseService.SendMessageAsync(user.Id, _localization.GetValue(Recources.Message.ErrorCode));
            }
        }
        private async Task HandleContactRequestAsync(User user, Message message)
        {
            string code = GetSmsCode.Get();// when sms working
            string testCode = "1111";
            if (message.Contact is null)
            {
                if (Regex.Match(message.Text, @"(?:[+][9]{2}[8][0-9]{2}[0-9]{3}[0-9]{2}[0-9]{2})").Success)
                {

                    //await SendSms(message.Contact.PhoneNumber, code); //avaible when sms working
                    await _botResponseService.SendMessageAsync(user.Id, _localization.GetValue(Recources.Message.EnterCode));
                    user.UserStateId = UserState.VERIFY_CODE;
                    user.Code = testCode;
                    user.PhoneNumber = message.Text;
                    await _userRepo.UpdateAsync(user);
                }
                else
                {
                    await _botResponseService.InValidPhoneNumberAsync(user.Id);
                }
            }
            else
            {
                //  await SendSms(message.Contact.PhoneNumber, code);
                await _botResponseService.SendMessageAsync(user.Id, _localization.GetValue(Recources.Message.EnterCode));
                user.UserStateId = UserState.VERIFY_CODE;
                user.Code = testCode;
                user.PhoneNumber = message.Contact.PhoneNumber;
                await _userRepo.UpdateAsync(user);
            }

        }
        private async Task HandleFullNameRequestAsync(User user, Message message)
        {
            user.Name = message.Text;
            if (Admin.SuperAdmin.Contains(message.From.Id.ToString()))
            {
                user.UserStateId = UserState.ADMIN_MENU;
                await _botResponseService.SendAdminMainMenu(user.Id);
            }
            else
            {
                await _botResponseService.SendMainMenuAsync(user.Id);
                user.UserStateId = UserState.MAIN_MENU;
            }
            await _userRepo.UpdateAsync(user);
        }
        #endregion


        private async Task HandleMainMenuAsync(User user, Message message)
        {
            user.Basket.Items.Clear();
            await _userRepo.UpdateAsync(user);
            if (message.Text == _localization.GetValue(Button.Order))
            {
                await _orderHandler.HandleOrderButtonAsync(user, message);
            }
            else if (message.Text == _localization.GetValue(Button.Settings))
            {
                await _settingsHandler.HandleSettingsAsync(user, message);
            }
            else if (message.Text == $"{_localization.GetValue(Button.ContactUs)}")
            {
                await HandleContactUsAsync(user, message);
            }
            else if (message.Text == $"{_localization.GetValue(Button.FeedBack)}")
            {
                await HandleFeedbackAsync(user, message);
            }
            else if (message.Text == $"{_localization.GetValue(Button.Information)}")
            {
                await HandleInformationAsync(user, message);
            }
            else
            {
                await SendMainMenu(user);// state hato va input to'g'ri kelmasa
            }
        }

        private async Task HandleInformationAsync(User user, Message message)
        {
            //var categories = await _categoryService.GetAllAsync();
            //await botClient.SendTextMessageAsync(
            //    message.Chat.Id,
            //    "Menyu",
            //    replyMarkup: KeyboardButtons.MakeReplyMarkup(categories.Select(x => x.Name).ToList(), )
            //    );
        }

        private async Task HandleFeedbackAsync(User user, Message message)
        {
            throw new NotImplementedException();
        }

        private async Task HandleContactUsAsync(User user, Message message)
        {
            throw new NotImplementedException();
        }


        private async Task HandleUnknownCommandAsync(User user, Message message)
        {
            if (user is null)
            {
                await _botResponseService.SendGreetingAsync(user.Id);
                await _botResponseService.SendLangugaesAsync(user.Id);
                user.UserStateId = UserState.SELECT_LANGUAGE;
            }
            else if (user.PhoneNumber is null)
            {
                await _botResponseService.SendContactRequestAsync(user.Id);
                user.UserStateId = UserState.SELECT_LANGUAGE;
            }
            else if (Admin.SuperAdmin.Contains(message.From.Id.ToString()))
            {
                await _botResponseService.SendAdminMainMenu(user.Id);
            }
            else
            {
                await _botResponseService.SendMainMenuAsync(user.Id);
            }
            await _userRepo.UpdateAsync(user);
        }
        private async Task SendSms(string phoneNumber, string code)
        {
            string token = await _tokenService.GetSmsTokenAsync();
            await SmsService.SendSms(token, phoneNumber, code);
        }
        public async Task SendMainMenu(User user)
        {
            if (Admin.SuperAdmin.Contains(user.Id.ToString()))
            {
                user.UserStateId = UserState.ADMIN_MENU;
                await _botResponseService.SendAdminMainMenu(user.Id);
            }
            else
            {
                await _botResponseService.SendMainMenuAsync(user.Id);
                user.UserStateId = UserState.MAIN_MENU;
            }
            await _userRepo.UpdateAsync(user);
        }
    }
}
