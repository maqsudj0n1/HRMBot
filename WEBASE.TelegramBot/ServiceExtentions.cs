

using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using WEBASE.TelegramBot.Configs;
using WEBASE.TelegramBot.Services;

namespace WEBASE.TelegramBot;

public static class ServiceExtensions
{
    public static void ConfigureTelegramServices(this IServiceCollection services, TelegramBotConfig botConfig)
    {

        services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    //TelegramBotConfig? botConfig = sp.GetConfiguration<TelegramBotConfig>();
                    TelegramBotClientOptions options = new(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });
        // services.AddSingleton(botConfig);
        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
    }
}
