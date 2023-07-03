using Microsoft.AspNetCore.Mvc;

namespace Sunrise.Api;

[Route("api/users")]
public class UserApi : Controller
{
    //сохранение доступа к контексту дб локально
    SunriseContext db;
    public UserApi(SunriseContext c)
    {
        db=c;
    }

    //получение пользователя через его имя
    [Route("getuser/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Types.User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetUser(string name){
        try
        {
            return Ok(db.GetUserApiView(name));
        }
        catch(Sunrise.Types.Exceptions.NotFoundObjectException)
        {
            return NotFound();
        }
    }

    //получение пользователя через его id
    [Route("getuser/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Types.User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetUser(Guid id){
        try
        {
            return Ok(db.GetUserApiView(id));
        }
        catch(Sunrise.Types.Exceptions.NotFoundObjectException)
        {
            return NotFound();
        }
    }

    //полуучение активного пользователя
    [Route("getuser")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Types.User))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetUser(){
        object isUser;
        if(HttpContext.Items.TryGetValue("isUser", out isUser)){
            if((bool)isUser){
                object id;
                HttpContext.Items.TryGetValue("userId", out id);
                Guid user = (Guid)id;
                var sessions = db.Sessions.Where(x=>x.UserId==user).FirstOrDefault();
                var u = db.GetUser(sessions.UserId);
                return Ok(u.GetApiView());
            }
            Unauthorized("need sing in");
        }

        return Unauthorized();
    }

    //создание пользователя
    [Route("create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateNewUser([FromBody] Types.UserRegistrationInfo uri)
    {
        var w = db.CreateNewUser(uri);
        if(!w){
            return BadRequest("User already exsist.");
        }
        return Ok("200 OK");
    }
}