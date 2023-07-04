using Microsoft.AspNetCore.Mvc;
using Sunrise.Utilities;
namespace Sunrise.Controllers;

[Route("upload")]
public class Upload : Controller
{
    //сохранение доступа к контексту дб локально
    SunriseContext db;
    public Upload(SunriseContext c)
    {
        db=c;
    }

    //получение пользователя через его имя
    [Route("image")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadImage(){
        if(!HttpContext.Items.IsUser()){
            return Unauthorized(new {message="login reqried"});
        }
        var uploaded = HttpContext.Request.Form.Files;
        var tags = HttpContext.Request.Form["tags"];
        var res = await Sunrise.Storage.ContentServer.Singelton.Save(Guid.NewGuid(), uploaded[0].OpenReadStream().ToByteArray(), Path.GetExtension(uploaded[0].FileName));
        Types.Post newPost = new Types.Post(HttpContext.Items.UserId(), res.Id);
        db.Posts.Add(newPost);
        db.Files.Add(res);
        await db.SaveChangesAsync();

        

        return Ok(new {message="sussesful"});
    }

    
}