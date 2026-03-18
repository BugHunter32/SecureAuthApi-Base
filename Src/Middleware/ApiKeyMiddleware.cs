namespace SecureAuthApi.Src.Middleware;

public class ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key missing.");
            return;
        }

        var apiKey = config.GetValue<string>("Authentication:ApiKey");
        if (apiKey != extractedApiKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key.");
            return;
        }

        await next(context);
    }
}