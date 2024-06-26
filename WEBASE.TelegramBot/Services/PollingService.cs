﻿

using Microsoft.Extensions.Logging;
using WEBASE.TelegramBot.Abstract;

namespace WEBASE.TelegramBot.Services;

public class PollingService : PollingServiceBase<ReceiverService>
{
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
        : base(serviceProvider, logger)
    {
    }
}
