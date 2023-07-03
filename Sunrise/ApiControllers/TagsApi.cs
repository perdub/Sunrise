using Microsoft.AspNetCore.Mvc;

namespace Sunrise.Api;

[Route("api/tags")]
public class TagsApi : Controller
{
    //сохранение доступа к контексту дб локально
    SunriseContext db;
    public TagsApi(SunriseContext c)
    {
        db=c;
    }

    //получение пользователя через его имя
    [Route("get/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Types.Tag))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetTag(int id){
        try
        {
            return Ok(db.GetTagInfo(id));
        }
        catch(Sunrise.Types.Exceptions.NotFoundObjectException)
        {
            return NotFound();
        }
    }

    [Route("get/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Types.Tag))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetTag(string name){
        try
        {
            return Ok(db.GetTagInfo(name));
        }
        catch(Sunrise.Types.Exceptions.NotFoundObjectException)
        {
            return NotFound();
        }
    }
}