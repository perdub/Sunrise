using Microsoft.AspNetCore.Mvc;
using Sunrise.Types;

namespace Sunrise.Api;

[Route("api")]
public class LoginApi : Controller
{
    //сохранение доступа к контексту дб локально
    SunriseContext db;
    public LoginApi(SunriseContext c)
    {
        db=c;
    }

    //создание сессии
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserLoginResult))]
    public IActionResult Login([FromBody] UserLoginInfo uli)
    {
        UserLoginResult res;
        var u = db.GetUser(uli.name);
        if(!u.CheckPassword(uli.password)){
            res = new UserLoginResult(LoginResult.InvalidCredentials, "incorrect login or password", "");
            return BadRequest(res);
        }

        Session newSession;
        if(uli.rememberMe){
            newSession = new Session(u);
        }
        else
        {
            newSession = new Session(u, TimeSpan.FromMinutes(20));
        }
        db.Sessions.Add(newSession);
        db.SaveChanges();

        HttpContext.Response.Cookies.Append("suntoken", newSession.SessionId);

        return Ok(new UserLoginResult(LoginResult.OK,"",newSession.SessionId));
    }

    [Route("sessions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Session[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetSessions()
    {
        object isUser;
        if(HttpContext.Items.TryGetValue("isUser", out isUser)){
            if((bool)isUser){
                object id;
                HttpContext.Items.TryGetValue("userId", out id);
                Guid user = (Guid)id;
                var sessions = db.Sessions.Where(x=>x.UserId==user).ToArray();
                return Ok(sessions);
            }
            Unauthorized("need sing in");
        }

        return Unauthorized();
    }
}