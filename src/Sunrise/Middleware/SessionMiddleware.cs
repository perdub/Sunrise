using Sunrise.Database;
using Microsoft.EntityFrameworkCore;

namespace Sunrise.Middleware;

public class SessionMiddleware{
    private readonly RequestDelegate _next;
    private ILogger<SessionMiddleware> _logger;

    private CookieOptions _SunidCookieOptions;

    public SessionMiddleware(RequestDelegate next,
        ILogger<SessionMiddleware> logger)
    {
        _next = next;
        _logger = logger;

        _SunidCookieOptions = new CookieOptions{
            MaxAge = TimeSpan.FromHours(1),
            SameSite = SameSiteMode.Strict
        };
    }

    public async Task InvokeAsync(HttpContext content, SunriseContext _context){
        var token = content.Request.Cookies["Suntoken"];
        bool IsLogged = false;
        if(string.IsNullOrWhiteSpace(token)){
            string? xFowardedFor = content.Request.Headers["X-Forwarded-For"];

            if(!string.IsNullOrEmpty(xFowardedFor)){
                _logger.LogTrace($"Request to {content.Request.Path}, ngrok X-Fowarded-For header:{xFowardedFor}");
            }
        }
        else{
            //add support for token procces
            var session = await _context.Sessions.Include(a=>a.Account).Where(a=>a.SessionId==token).FirstOrDefaultAsync();
            if(session == null){
                _logger.LogTrace("Token exsist, but session not found");
                goto invoke;
            }
            var account = session.Account;
            //set accout id in cookie 
            content.Response.Cookies.Append("Sunid", account.AccountId.ToString(), _SunidCookieOptions);
            content.Response.Cookies.Append("Sunname", account.Username, _SunidCookieOptions);
            //set params
            IsLogged = true;
            content.Items.Add("PrivilegeLevel", (int)account.PrivilegeLevel);
        }
        invoke:
        content.Items.Add("IsLogged", IsLogged);
        await _next(content);
    }
}