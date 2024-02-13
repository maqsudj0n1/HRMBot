namespace HRMBot.Extentions;

public static class CorsAppBuilderExtentions
{
    public static void ConfigureCors(this IApplicationBuilder app)
    {
        if (AppSettings.Instance.Cors.UseCors)
            app.UseCors("AllowedOrgins");
        else
            app.UseCors("AllowAll");
    }
}
