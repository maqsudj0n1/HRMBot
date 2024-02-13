using HRMBot.Extensions;
using HRMBot.Extentions;
using HRMBot;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using WEBASE.AspNet;
using WEBASE.i18n;
using WEBASE.Integration.Manuals.Extensions;
using WEBASE.TelegramBot;
using WEBASE.DependencyInjection;
using HRMBot.BizLogicLayer.Services;

var builder = WebApplication.CreateBuilder(args);
AppSettings.Init(builder.Configuration.Get<AppSettings>());
builder.Services.ConfigureConfigs();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IMimeMappingService, MimeMappingService>(p => new MimeMappingService(new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider()));

builder.Services.ConfigureDbServices();
builder.Services.ConfigureAuthServices();
builder.Services.AddScoped<ICultureHelper, CultureHelper>();
builder.Services.ConfigureGenericServices();
builder.Services.ConfigureCrmServices(AppSettings.Instance.EmployeeManage);
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

builder.Services.ConfigureCorsServices();
builder.Services.Configure<RequestLocalizationOptions>(
        opts =>
        {
            var supportedCultures = new List<CultureInfo> { new CultureInfo("ru") };
            opts.DefaultRequestCulture = new RequestCulture("ru");
            // Formatting numbers, dates, etc.
            opts.SupportedCultures = supportedCultures;
            // UI strings that we have localized.
            opts.SupportedUICultures = supportedCultures;
        });
builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});
builder.Services.AddMinio(AppSettings.Instance.Minio);
builder.Services.ConfigureTelegramServices(AppSettings.Instance.TelegramBot);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
BaseServiceProvider.Initialize(() => app.Services.GetService<IHttpContextAccessor>().HttpContext?.RequestServices);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
