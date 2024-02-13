using HRMBot.BizLogicLayer.Services;
using HRMBot.DataLayer.Repositories;
using HRMBot.Services;

namespace HRMBot.Extensions;
public static class ConfigServiceExtentions
{
    public static void ConfigureConfigs(this IServiceCollection services)
    {
        services.AddSingleton(AppSettings.Instance.Culture);
        services.AddSingleton(AppSettings.Instance.Jwt);
        services.AddSingleton(AppSettings.Instance.Cookie);
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IEmployeeService,EmployeeService>();

    }
}
