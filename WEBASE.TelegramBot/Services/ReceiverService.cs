

using Microsoft.Extensions.Logging;
using Telegram.Bot;
using WEBASE.TelegramBot.Abstract;

namespace WEBASE.TelegramBot.Services;

public class ReceiverService : ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<UpdateHandler>> logger)
        : base(botClient, updateHandler, logger)
    {
    }
}
