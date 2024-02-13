using HRMBot.BizLogicLayer.Services.Models;
using WEBASE.AspNet.Security;
using WEBASE.i18n;
using WEBASE.Minio;
using WEBASE.TelegramBot.Configs;

namespace HRMBot;

public class AppSettings
{
    public static AppSettings Instance { get; private set; }
    public CultureConfig Culture { get; set; } = new();
    public CookieConfig Cookie { get; set; }
    public JwtConfig Jwt { get; set; }
    public CorsConfig Cors { get; set; } = new();
    public DatabaseConfig Database { get; set; } = new();
    public TelegramBotConfig TelegramBot { get; set; } = new();
    public MinioConfig Minio { get; set; } = new();
    public SwaggerConfig Swagger { get; set; } = new();
    public EmployeeManageConfig EmployeeManage { get; set; }
    public static void Init(AppSettings instance)
    {
        Instance = instance;
    }
}

public class CorsConfig
{
    public bool UseCors { get; set; }
    public string AllowedOrgins { get; set; }
}
public class DatabaseConfig
{
    public PgSqlConfig PgSql { get; set; } = new();
}
public class PgSqlConfig
{
    public string ConnectionString { get; set; } = "";
}
public class SwaggerConfig
{
    public bool Enabled { get; set; }
    public string Prefix { get; set; }
}

public class EmployeeIntegration
{
    public string BaseUri { get; set; }
    public string UrlForCreateByEmployee { get; set; }
    public string UrlForGetEmployeeManageId { get; set; }
    public string UrlForMissedDaysTypeSelectList { get; set; }

}