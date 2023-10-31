using Microsoft.AspNetCore.Mvc;
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
}