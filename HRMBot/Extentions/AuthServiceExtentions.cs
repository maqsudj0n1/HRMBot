using HRMBot.Core.Security;

namespace HRMBot.Extensions;

public static class CultureServiceExtentions
{
    public static void ConfigureAuthServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<WEBASE.Security.IAuthService>(x => x.GetService<IAuthService>());
    }
}
