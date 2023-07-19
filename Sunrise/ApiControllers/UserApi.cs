using Microsoft.AspNetCore.Mvc;

namespace Sunrise.Api;

[Route("api/users")]
public class UserApi : Controller
{
    //сохранение доступа к контексту дб локально
    SunriseContext cs;
    public UserApi(SunriseContext c)
    {
        cs=c;
    }



    //получение пользователя через его имя
    [Route("getuser/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Types.User))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(string name){
        try
        {
            return Ok(cs.GetUserApiView(name));
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
    public async Task<IActionResult> GetUser(Guid id){
        try
        {
            return Ok(cs.GetUserApiView(id));
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
    public async Task<IActionResult> GetUser(){
        object isUser;
        //получение булевого значения которое указывает на то пользователь ли мы
        if(HttpContext.Items.TryGetValue("isUser", out isUser)){
            if((bool)isUser){
                object id;
                HttpContext.Items.TryGetValue("userId", out id);
                Guid user = (Guid)id;//получение айди пользователя и юзера, после чего возврат api представления
                var u = cs.GetUserApiView(user);
                return Ok(u);
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
        var w = cs.CreateNewUser(uri);
        if(!w){
            return BadRequest("User already exsist.");
        }
        return Ok("200 OK");
    }
}