using Microsoft.AspNetCore.Mvc;
using Sunrise.Types;
using Sunrise.Utilities;
namespace Sunrise.Api;

[Route("api")]
public class LoginApi : Controller
{
    //сохранение доступа к контексту дб локально
    SunriseContext cs;
    public LoginApi(SunriseContext c)
    {
        cs = c;
    }

    //создание сессии
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserLoginResult))]
    public async Task<IActionResult> Login([FromBody] UserLoginInfo uli)
    {
        UserLoginResult res;
        var u = cs.GetUser(uli.name);
        if (!u.CheckPassword(uli.password))
        {
            res = new UserLoginResult(LoginResult.InvalidCredentials, "incorrect login or password", "");
            return BadRequest(res);
        }

        Session newSession;
        if (uli.rememberMe)
        {
            newSession = new Session(u);
        }
        else
        {
            newSession = new Session(u, TimeSpan.FromMinutes(20));
        }
        cs.Sessions.Add(newSession);
        cs.SaveChanges();

        HttpContext.Response.Cookies.Append(Constants.SESSION_COOKIE_NAME, newSession.SessionId);

        return Ok(new UserLoginResult(LoginResult.OK, "", newSession.SessionId));
    }

    [Route("sessions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Session[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetSessions()
    {
        object isUser;
        if (HttpContext.Items.TryGetValue("isUser", out isUser))
        {
            if ((bool)isUser)
            {
                object id;
                HttpContext.Items.TryGetValue("userId", out id);
                Guid user = (Guid)id;
                //todo: добавить работу с кешированием!!!
                var sessions = cs.Sessions.Where(x => x.UserId == user).ToArray();
                return Ok(sessions);
            }
            Unauthorized("need sing in");
        }

        return Unauthorized();
    }

    [Route("logout")]
    [Route("/logout")]
    public async Task<IActionResult> Logout()
    {
        if (!HttpContext.Items.IsUser())
        {
            return Redirect("/");
        }
        if (HttpContext.Request.Cookies.TryGetValue(Constants.SESSION_COOKIE_NAME, out string? token)
            && token!=null)
        {
            var session = await cs.Sessions.FindAsync(token);
            if(session!=null){
                cs.Sessions.Remove(session);
                await cs.SaveChangesAsync();
            }
        }
        return Redirect("/");
    }
}