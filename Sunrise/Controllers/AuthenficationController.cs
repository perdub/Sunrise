using Microsoft.AspNetCore.Mvc;
using Sunrise.Utilities;
using Sunrise.Types;
namespace Sunrise.Controllers;

[Route("auth")]
public class AuthenficationController : Controller
{
    //сохранение доступа к контексту дб локально
    SunriseContext cs;
    public AuthenficationController(SunriseContext c)
    {
        cs=c;
    }
    
    [Route("registry")]
    public async Task<IActionResult> Registry([FromBody] UserRegistrationInfov2 data)
    {
        VerifyType type = (VerifyType)data.type;
        //создание пользователя и задания для верификации
        User u = new User(data.username, data.password);
        Verify v = new Verify(u);

        string tgboturl = null;
        switch(type){
            case VerifyType.Email:
                u.Email = data.verifyFiled;
                break;
            case VerifyType.TelegramBot:
                tgboturl = "http://t.me/SunriseImageBoardBot/?start="+v.Key;
                break;
        }

        Session s = new Session(u, TimeSpan.FromDays(10));

        HttpContext.Response.Cookies.Append(Constants.SESSION_COOKIE_NAME, s.SessionId);

        cs.Users.Add(u);
        cs.Verify.Add(v);
        cs.Sessions.Add(s);
        cs.SaveChanges();


        return Ok(new {open=tgboturl ?? "/"});
    }
}