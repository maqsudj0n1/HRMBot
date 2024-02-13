using HRMBot.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using WEBASE.EF;

namespace HRMBot.Extentions;

public static class DbServiceExtentions
{
    public static void ConfigureDbServices(this IServiceCollection services)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<EfCoreContext, PgSqlContext>(options =>
        {
            options.UseNpgsql(AppSettings.Instance.Database.PgSql.ConnectionString);
#if DEBUG
            options.EnableSensitiveDataLogging(true);
#endif
        });
        services.AddScoped<BaseDbContext>(x => x.GetService<EfCoreContext>());
    }
}
