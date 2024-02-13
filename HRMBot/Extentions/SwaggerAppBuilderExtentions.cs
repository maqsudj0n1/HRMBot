namespace HRMBot.Extentions;

public static class SwaggerAppBuilderExtentions
{
    public static void ConfigureSwagger(this IApplicationBuilder app)
    {
        if (AppSettings.Instance.Swagger.Enabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
