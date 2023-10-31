using Sunrise.Database;

namespace Sunrise.Middleware;

public class SessionMiddleware{
    private readonly RequestDelegate _next;
    private ILogger<SessionMiddleware> _logger;
    private SunriseContext _context;

    public SessionMiddleware(RequestDelegate next,
        ILogger<SessionMiddleware> logger,
        SunriseContext sunriseContext)
    {
        _next = next;
        _logger = logger;
        _context = sunriseContext;
    }

    public async Task InvokeAsync(HttpContext content){
        var token = content.Request.Cookies["Suntoken"];
        if(string.IsNullOrWhiteSpace(token)){
            string? xFowardedFor = content.Request.Headers["X-Forwarded-For"];

            if(!string.IsNullOrEmpty(xFowardedFor)){
                _logger.LogTrace($"Request to {content.Request.Path}, ngrok X-Fowarded-For header:{xFowardedFor}");

            }

            await _next(content);
            return;
        }
        else{
            //add support for token procces

            await _next(content);
        }
    }
}