using HRMBot.BizLogicLayer.Services;
using HRMBot.BizLogicLayer.Services.Models;
using HRMBot.DataLayer;
using HRMBot.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = HRMBot.DataLayer.EfClasses.User;
namespace WEBASE.TelegramBot.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _client;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly IEmployeeService _employeeService;


    public UpdateHandler(ITelegramBotClient client, IUnitOfWork unitOfWork, IUserService userService, IEmployeeService employeeService)
    {
        _client = client;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _employeeService = employeeService;
    }
    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {

        if (update.Message == null && update.CallbackQuery == null) return;
        User? user = new User();
        string cmd = update.Message?.Text;
        var chatId = update?.Message?.Chat?.Id ?? update.CallbackQuery?.From.Id;
        user = _unitOfWork.Context.Users.FirstOrDefault(x => x.ChatId == chatId);
        if (user == null)
        {
            _userService.Create(new CreateUserDto()
            {
                ChatId = update.Message.Chat.Id,
                Step = -1,
                UserName = update.Message.Chat.Username,
                StateId = 1
            });
            user = _unitOfWork.Context.Users.FirstOrDefault(x => x.ChatId == update.Message.Chat.Id);
        }

        if (user?.Step == -1 && (update.Message?.Contact != null || cmd.StartsWith("+998")))
        {
            string number = GetCorrectUserName(update.Message?.Contact?.PhoneNumber != null ? update.Message?.Contact?.PhoneNumber : cmd);
            var t = await CheckUserExist(number);
            if (t > 0)
            {
                user.PhoneNumber = number;
                user.IsEmployee = true;
                user.EmployeeId = t;
                user.Step++;
                _unitOfWork.Save();
            }
            cmd = "/register";
        }
        else if (user.Step == 0 && cmd == "Ishda bo'lmagan vaqtni kiritish")
        {
            cmd = "/calendar";
            user.Step++;
            _unitOfWork.Save();
        }

        else if (user.Step > 0)
        {
            cmd = "/createemployee";
        }

        await (cmd switch
        {
            "/start" => StartHandler(_client, update, user),
            "/register" => Register(_client, update, user),
            "/calendar" => MakeCalendar(_client, update.CallbackQuery, user, DateTime.UtcNow.Year, DateTime.UtcNow.Month),
            "/createemployee" => CreateEmployee(_client, update, user),
            _ => StartHandler(_client, update, user),
        });
    }
    private async Task StartHandler(ITelegramBotClient _client, Update update, User user)
    {
        if (user.Step == -1)
        {
            await RequestContact(_client, update, user);
        }
    }
    private async Task Register(ITelegramBotClient _client, Update update, User user)
    {
        if (user.IsEmployee)
        {
            KeyboardButton[][] keys = new KeyboardButton[][]
            {
               new[]
               {
                    new KeyboardButton("Ishda bo'lmagan vaqtni kiritish"),
               }
            };
            ReplyKeyboardMarkup markup = new(keys)
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true,
            };
            await _client.SendTextMessageAsync(user.ChatId, $"Assalomu aleykum 😊", replyMarkup: markup);
        }
        else
        {
            await _client.SendTextMessageAsync(user.ChatId, $"{user.PhoneNumber} Ushbu raqam bazada mavjud emas ☹");
        }
    }
    private async Task CreateEmployee(ITelegramBotClient _client, Update update, User user)
    {
        switch (user.Step)
        {

            case 1:
                {
                    if (int.TryParse(update.CallbackQuery.Data, out int res))
                    {
                        user.StartDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, res);
                        user.Step++;
                        _unitOfWork.Save();

                        await _client.EditMessageReplyMarkupAsync(
                               chatId: user.ChatId,
                               messageId: update.CallbackQuery.Message.MessageId,
                               replyMarkup: await GetTimePickerInlineKeyboard(user)
                              );


                        await _client.EditMessageTextAsync(
                                chatId: user.ChatId,
                                messageId: update.CallbackQuery.Message.MessageId,
                                text: $"Vaqtini tanglang: {user.SelectedHour:00}:{user.SelectedMinute:00}",
                                replyMarkup: await GetTimePickerInlineKeyboard(user)
                                                           );
                    }
                    else
                    {
                        await _client.SendTextMessageAsync(user.ChatId, "Choose valid date");
                    }

                    break;
                }
            case 2:
                {
                    await HandleCallbackQuery(_client, update.CallbackQuery, user);
                    break;
                }
            case 3:
                {
                    if (int.TryParse(update.CallbackQuery.Data, out int res))
                    {
                        user.EndDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, int.Parse(update.CallbackQuery.Data));
                        user.Step++;
                        _unitOfWork.Save();

                        await _client.EditMessageReplyMarkupAsync(
                               chatId: user.ChatId,
                               messageId: update.CallbackQuery.Message.MessageId,
                               replyMarkup: await GetTimePickerInlineKeyboard(user)
                              );


                        await _client.EditMessageTextAsync(
                                chatId: user.ChatId,
                                messageId: update.CallbackQuery.Message.MessageId,
                                text: $"Vaqtini tanglang: {user.SelectedHour:00}:{user.SelectedMinute:00}",
                                replyMarkup: await GetTimePickerInlineKeyboard(user)
                                                           );
                    }
                    else
                    {
                        await _client.SendTextMessageAsync(user.ChatId, "Choose valid date");
                    }

                    break;
                }
            case 4:
                {
                    await HandleCallbackQuery(_client, update.CallbackQuery, user);
                    if (update.CallbackQuery.Data == "submit-time")
                        goto case 5;
                    else break;

                }
            case 5:
                {
                    InlineKeyboardMarkup inlineKeyboard;
                    var data = await _employeeService.MissedDaysTypeSelectList();
                    List<List<InlineKeyboardButton>> keyboardRows = new List<List<InlineKeyboardButton>>();
                    List<InlineKeyboardButton> currentRow = new List<InlineKeyboardButton>();

                    foreach (var item in data)
                    {
                        currentRow.Add(InlineKeyboardButton.WithCallbackData(item.text,
                        item.value.ToString()));

                        if (currentRow.Count == 1)
                        {
                            keyboardRows.Add(new List<InlineKeyboardButton>(currentRow));
                            currentRow.Clear();
                        }
                    }

                    inlineKeyboard = new InlineKeyboardMarkup(keyboardRows);
                    var caption = "Ishda bo'lmagan vaqtingizning sababini tanlang:";

                    user.Step++;
                    _unitOfWork.Save();

                    await _client.EditMessageTextAsync(
                   chatId: user.ChatId,
                   messageId: update.CallbackQuery.Message.MessageId,
                   text: caption,
                   replyMarkup: inlineKeyboard
                                              );

                    break;
                }
            case 6:
                {
                    user.MissedDayTypeId = int.Parse(update.CallbackQuery.Data);
                    _unitOfWork.Save();
                    var res = await _employeeService.CreateByEmployee(new EmployeeCreateRequestDto()
                    {
                        EmployeeManageId = user.EmployeeId,
                        EndAt = user.EndDate.Value,
                        StartAt = user.StartDate.Value,
                        MissedDaysTypeId = (int)user.MissedDayTypeId,
                        WithoutReason = true,
                        MissedDays = (int)(user.EndDate - user.StartDate).Value.TotalDays,

                    });

                    if (res != null)
                    {

                        await _client.DeleteMessageAsync(user.ChatId, update.CallbackQuery.Message.MessageId);

                        await _client.SendTextMessageAsync(user.ChatId, $"Muvoffaqiyatli saqlandi✅");
                        user.Step = 0;
                        _unitOfWork.Save();
                    }
                    else
                    {
                        await _client.SendTextMessageAsync(user.ChatId, $"Error ...");
                    }
                    break;
                }
        }
    }
    private async Task MakeCalendar(ITelegramBotClient _client, CallbackQuery callbackQuery, User user, int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var inlineKeyboard = new List<InlineKeyboardButton[]>();
        var daysOfWeekRow = new[]
        {
            InlineKeyboardButton.WithCallbackData("Mo"),
            InlineKeyboardButton.WithCallbackData("Tu"),
            InlineKeyboardButton.WithCallbackData("We"),
            InlineKeyboardButton.WithCallbackData("Th"),
            InlineKeyboardButton.WithCallbackData("Fr"),
            InlineKeyboardButton.WithCallbackData("Sa"),
            InlineKeyboardButton.WithCallbackData("Su")
        };
        inlineKeyboard.Add(daysOfWeekRow);

        for (int week = 0; week < 6; week++)
        {
            var weekRow = new List<InlineKeyboardButton>();
            for (int dayOfWeek = 1; dayOfWeek <= 7; dayOfWeek++)
            {
                int day = week * 7 + dayOfWeek - (int)new DateTime(year, month, 1).DayOfWeek + 1;
                if (day > 0 && day <= daysInMonth)
                {
                    weekRow.Add(InlineKeyboardButton.WithCallbackData(day.ToString(), $"{day}"));
                }
                else
                {
                    if (week == 5 && dayOfWeek == 1)
                        break;
                    weekRow.Add(InlineKeyboardButton.WithCallbackData(" ", "ignore"));
                }
            }
            inlineKeyboard.Add(weekRow.ToArray());
        }

        //var navigationRow = new[]
        //{
        //    InlineKeyboardButton.WithCallbackData("<", $"prev-{month-1}"),
        //    InlineKeyboardButton.WithCallbackData(">", $"next-{month+1}")
        //};
        //inlineKeyboard.Add(navigationRow);

        var inlineKeyboardMarkup = new InlineKeyboardMarkup(inlineKeyboard);

        if (user.Step <= 1)
        {
            await _client.SendTextMessageAsync(
                   chatId: user.ChatId,
                   text: $"Ishda bo'lmagan vaqtingizning boshlanish sanasini tanglang: {new DateTime(year, month, 1).ToString("MMMM yyyy")}",
                   replyMarkup: inlineKeyboardMarkup
                                             );
        }
        else
        {
            await _client.EditMessageReplyMarkupAsync(
                  chatId: callbackQuery.Message.Chat.Id,
                  messageId: callbackQuery.Message.MessageId,
                  replyMarkup: inlineKeyboardMarkup
                  );

            await _client.EditMessageTextAsync(
                           chatId: user.ChatId,
                           messageId: callbackQuery.Message.MessageId,
                           text: $"Ishda bo'lmagan vaqtingizning tugash sanasini tanglang: {new DateTime(year, month, 1).ToString("MMMM yyyy")}",
                           replyMarkup: inlineKeyboardMarkup
                                                      );

        }
    }
    private async Task<int> CheckUserExist(string phoneNumber)
    {
        var res = await _employeeService.CheckEmployee(phoneNumber);
        return res;

    }
    private async Task RequestContact(ITelegramBotClient _client, Update update, User user)
    {
        var contactButton = _unitOfWork.Context.Buttons.Where(x => x.OrderCode == "request_contact").Include(x => x.Translates).ToArray();
        ReplyKeyboardMarkup RequestReplyKeyboard = new(
             new[]
             {
                    KeyboardButton.WithRequestContact(contactButton[0].Translates.AsQueryable().FirstOrDefault(ButtonTranslate.GetExpr(TranslateColumn.full_name, user.LanguageId))?.TranslateText ?? contactButton[0].FullName),
             })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        await _client.SendTextMessageAsync(
           chatId: update.Message.Chat.Id,
           text: "Assalomu aleykum telefon raqamingizni kiriting\nNamuna +998911233456",
           replyMarkup: RequestReplyKeyboard
           );
    }
    public static string GetCorrectUserName(string phoneNumber)
    {
        phoneNumber = phoneNumber.Replace("+", "").Replace("-", "").Replace(" ", "");
        if (!phoneNumber.StartsWith("998") && phoneNumber.Length != 12)
            phoneNumber = $"998{phoneNumber}";
        return phoneNumber;
    }

    private async Task<InlineKeyboardMarkup> GetTimePickerInlineKeyboard(User user)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            // Hour row
            new[]
            {
                InlineKeyboardButton.WithCallbackData("-", "decrement-hour"),
                InlineKeyboardButton.WithCallbackData($"{user.SelectedHour:00}", "current-hour"),
                InlineKeyboardButton.WithCallbackData("+", "increment-hour")
            },
            // Minute row
            new[]
            {
                InlineKeyboardButton.WithCallbackData("-", "decrement-minute"),
                InlineKeyboardButton.WithCallbackData($"{user.SelectedMinute:00}", "current-minute"),
                InlineKeyboardButton.WithCallbackData("+", "increment-minute")
            },
            // Submit/Cancel row
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Submit", "submit-time"),
                InlineKeyboardButton.WithCallbackData("Cancel", "cancel-time")
            }
        });

        return inlineKeyboard;

    }

    private async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery, User user)
    {
        switch (callbackQuery.Data)
        {
            case "increment-hour":
                user.SelectedHour = (user.SelectedHour + 1) % 24;
                _unitOfWork.Save();
                break;
            case "decrement-hour":
                user.SelectedHour = (user.SelectedHour - 1 + 24) % 24;
                _unitOfWork.Save();
                break;
            case "increment-minute":
                user.SelectedMinute = (user.SelectedMinute + 5) % 60;
                _unitOfWork.Save();
                break;
            case "decrement-minute":
                user.SelectedMinute = (user.SelectedMinute - 5 + 60) % 60;
                _unitOfWork.Save();
                break;
            case "submit-time":
                await ProcessFinalTime(botClient, callbackQuery, user);
                break;
            case "cancel-time":
                user.SelectedHour = 0;
                user.SelectedHour = 0;
                _unitOfWork.Save();

                break;
        }

        if (callbackQuery.Data != "submit-time")
        {
            await botClient.EditMessageTextAsync(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: $"Vaqtini tanglang: {user.SelectedHour:00}:{user.SelectedMinute:00}",
            replyMarkup: await GetTimePickerInlineKeyboard(user)
        );

            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
        }

    }

    private async Task ProcessFinalTime(ITelegramBotClient botClient, CallbackQuery callbackQuery, User user)
    {
        if (user.Step != 4)
        {
            user.StartDate = new DateTime(user.StartDate.Value.Year, user.StartDate.Value.Month, user.StartDate.Value.Day, (int)user.SelectedHour, (int)user.SelectedMinute, 0);
            user.Step++;
            _unitOfWork.Save();

            await MakeCalendar(_client, callbackQuery, user, DateTime.UtcNow.Year, DateTime.UtcNow.Month);
        }
        else
        {
            user.EndDate = new DateTime(user.EndDate.Value.Year, user.EndDate.Value.Month, user.EndDate.Value.Day, (int)user.SelectedHour, (int)user.SelectedMinute, 0);
            user.Step++;
            _unitOfWork.Save();

            //await MakeCalendar(_client, callbackQuery, user, DateTime.UtcNow.Year, DateTime.UtcNow.Month);
        }
    }

}
