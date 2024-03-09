using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Sunrise.Types;
using Sunrise.Database;

namespace Sunrise.Mvc.Controllers;

[Route("/account")]
public class AccountController : Controller{
    private SunriseContext _context;
    private ILogger<AccountController> _logger;
    private IConfiguration _config;
    private IServiceProvider _provider;

    public AccountController(SunriseContext context, ILogger<AccountController> logger, IConfiguration config, IServiceProvider provider)
    {
        _context = context;
        _logger = logger;
        _config = config;
        _provider = provider;
    }


    [Route("create")]
    [HttpPost]
    [EnableRateLimiting("AccountCreate")]
    public async Task<IActionResult> CreateAccount(string username, string password, string email){
        var a = _context.Accounts.Where(a=>a.Username == username).FirstOrDefault();
        #region Error if username is already taken
        if(a is not null){
            return Conflict(
                EndpointError.BuildError(
                    "Username is taken.",
                    $"Username {username} is already taken, please try another username.")
            );
        }
        #endregion
        
        Account newAccount = new Account(username, password, _config.GetValue<string>("GlobalSalt"), email);
        _context.Accounts.Add(newAccount);
        _context.SaveChanges();

        var sessionBuilder = (Sunrise.Builders.SessionBuilder)_provider.GetService(typeof(Sunrise.Builders.SessionBuilder));
        var newSession = sessionBuilder.BuildSession(newAccount);

        HttpContext.Response.Cookies.Append("Suntoken", newSession.SessionId);

        return Ok();
    }
    
    [HttpPost]
    [Route("login")]
    [EnableRateLimiting("AccountLogin")]
    public async Task<IActionResult> Login(string username, string password, string rto=null){
        var a = _context.Accounts.Where(a=>a.Username == username).FirstOrDefault();
        if(a is null){
            return Conflict(
                EndpointError.BuildError(
                    "Invalid credentails.",
                    "Incorrect login or password.")
            );
        }
        bool isAllow = a.CheckPassword(password, _config.GetValue<string>("GlobalSalt"));
        if(!isAllow){
            return Conflict(
                EndpointError.BuildError(
                    "Invalid credentails.",
                    "Incorrect login or password.")
            );
        }

        var sessionBuilder = (Sunrise.Builders.SessionBuilder)_provider.GetService(typeof(Sunrise.Builders.SessionBuilder));
        var newSession = sessionBuilder.BuildSession(a);

        HttpContext.Response.Cookies.Append("Suntoken", newSession.SessionId);

        return rto is not null ? Redirect(rto) : Ok();
    }
}