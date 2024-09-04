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
            if (user is null)
            {
                if (message.Text == "/start" && user.UserProcess == UserProcess.sendGreeting)
                {
                    await _botResponseService.SendGreeting(user.Id);
                    await _botResponseService.SendLangugaes(user.Id);
                    user.UserProcess = UserProcess.contactRequest;
                    await _userRepo.UpdateAsync(user);
                }
            }
            else if (message.Text == _localization.GetValue(Button.Basket))
            {
                await _basketHandler.HandleActionInBasketAsync(user, message);
            }
            else if (message.Text == _localization.GetValue(Button.Back))
            {
                await _backHandler.HandleBackButtonAsync(user);
            }
            else if (user is User)
            {
                await HandleUserProcess(user, botClient, message);
            }
            else
            {
                await HandleUnknownCommand(user, message);
            }

        }
        private async Task HandleUserProcess(User user, ITelegramBotClient botClient, Message message)
        {
            Task res = user.UserProcess switch
            {
                UserProcess.contactRequest => HandleContactRequestAsync(user, message),
                UserProcess.verifyCode => HandleVerifyCodeAsync(user, botClient, message),
                UserProcess.fullName => HandleFullNameRequestAsync(user, message),
                UserProcess.mainMenu => HandleMainMenuAsync(user, message),
                UserProcess.selectDeliveryType => _orderHandler.HandleInDeliveryTypeRequestAsync(user, message),
                UserProcess.locationRequest => _orderHandler.HandleLocationRequestAsync(user, message),
                UserProcess.inCategory => _orderHandler.HandleInCategoryAsync(user, message),
                UserProcess.inProduct => _orderHandler.HandleInProductAsync(user, message),
                UserProcess.amountRequest => _orderHandler.HandleAmountRequestAsync(user, message),
                UserProcess.inBasket => _basketHandler.HandleActionInBasketAsync(user, message),
                UserProcess.onCommentOrder => _orderHandler.HandleOnCommentOrderAsync(user, message),
                UserProcess.onSelectPaymentType => _orderHandler.HandleOnSelectPaymentTypeAsync(user, message),
                UserProcess.atConfirmationOrder => _orderHandler.HandleAtConfirmationOrderAsync(user, message),
                _ => HandleUnknownCommand(user, message)
            };
            await res;
        }

        private async Task HandleVerifyCodeAsync(User user, ITelegramBotClient botClient, Message message)
        {
            if (string.Equals(user.Code, message.Text))
            {
                user.UserProcess = UserProcess.fullName;
                user.PhoneNumber = user.ProcessHelper.UserPhoneNumber;
                await _userRepo.UpdateAsync(user);
                await _botResponseService.SendMessages(user.Id, _localization.GetValue(Recources.Message.FullName));
            }
            else
            {
                await _botResponseService.SendMessages(user.Id, _localization.GetValue(Recources.Message.ErrorCode));
            }
        }
        private async Task HandleMainMenuAsync(User user, Message message)
        {
            if (message.Text == _localization.GetValue(Button.Order))
            {
                _orderHandler.HandleOrderButtonAsync(user, message);
            }
            else if (message.Text == _localization.GetValue(Button.Settings))
            {
                _settingsHandler.HandleSettingsAsync(user, message);
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
        private async Task HandleContactRequestAsync(User user, Message message)
        {
            string code = GetSmsCode.Get();// when sms working
            string testCode = "1111";
            if (message.Contact is null)
            {
                if (Regex.Match(message.Text, @"(?:[+][9]{2}[8][0-9]{2}[0-9]{3}[0-9]{2}[0-9]{2})").Success)
                {

                    //await SendSms(message.Contact.PhoneNumber, code); //avaible when sms working
                    await _botResponseService.SendMessages(user.Id, _localization.GetValue(Recources.Message.EnterCode));
                    user.UserProcess = UserProcess.verifyCode;
                    user.Code = code;
                    user.PhoneNumber = message.Text;
                    await _userRepo.UpdateAsync(user);
                }
                else
                {
                    await _botResponseService.InValidPhoneNumber(user.Id);
                }
            }
            else
            {
                //  await SendSms(message.Contact.PhoneNumber, code);
                await _botResponseService.SendMessages(user.Id, _localization.GetValue(Recources.Message.EnterCode));
                user.UserProcess = UserProcess.verifyCode;
                user.Code = code;
                user.PhoneNumber = message.Contact.PhoneNumber;
                await _userRepo.UpdateAsync(user);
            }

        }

        private async Task HandleFullNameRequestAsync(User user, Message message)
        {
            user.Name = message.Text;
            user.UserProcess = UserProcess.mainMenu;
            await _userRepo.UpdateAsync(user);
            await _botResponseService.SenMainMenu(user.Id);
        }

        private async Task HandleUnknownCommand(User user, Message message)
        {
            user.UserProcess = UserProcess.mainMenu;
            await _userRepo.UpdateAsync(user);
            await _botResponseService.SenMainMenu(user.Id);
        }
        private async Task SendSms(string phoneNumber, string code)
        {
            string token = await _tokenService.GetSmsTokenAsync();
            await SmsService.SendSms(token, phoneNumber, code);
        }
    }
}
