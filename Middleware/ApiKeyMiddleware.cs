namespace MyMinimalAPI.Middleware
{
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private
    const string APIKEY = "X-Api-Key";
    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(APIKEY, out
                var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            //await context.Response.WriteAsync("Api Key was not provided");
            return;
        }
        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
        var secretApiKey = appSettings[APIKEY];
        if (!secretApiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            //await context.Response.WriteAsync("Unauthorized client");
            return;
        }
        await _next(context);
    }
}
}