using Microsoft.AspNetCore.Mvc;
using Sunrise.Utilities;
namespace Sunrise.Controllers;

[Route("upload")]
public class Upload : Controller
{
    //сохранение доступа к контексту дб локально
    CacheService cs;
    public Upload(CacheService c)
    {
        cs=c;
    }

    //загрузка картинки
    [RequestSizeLimit(1024*1024*128)]
    [Route("image")]
    [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadImage(){
        if(!HttpContext.Items.IsUser()){
            return Unauthorized(new {message="login reqried"});
        }
        var uploaded = HttpContext.Request.Form.Files;
        var tags = HttpContext.Request.Form["tags"];
        var res = await Sunrise.Storage.ContentServer.Singelton.SaveImage(Guid.NewGuid(), uploaded[0].OpenReadStream().ToByteArray(), Sunrise.Types.ContentType.Image, Path.GetExtension(uploaded[0].FileName));
        var tagsArr = await (new Api.TagsApi(cs).GetTagsAsync(tags[0].Split(), cs));
        Types.Post newPost = new Types.Post(HttpContext.Items.UserId(), res.Id);

        foreach(var tag in tagsArr)
        {
            newPost.Tags.Add(tag);
            tag.Post.Add(newPost);
            tag.PostCount++;
        }
        cs.dbcontext.Tags.UpdateRange(tagsArr);
        cs.dbcontext.Posts.Add(newPost);
        cs.dbcontext.Files.Add(res);
        await cs.dbcontext.SaveChangesAsync();
        //проверка на нужен ли редирект на домашнюю страницу
        var r = String.IsNullOrEmpty(HttpContext.Request.Query["redirecttohome"]);

        return r ? Ok(new {message="sussesful", post = newPost, file=res}) : RedirectPermanent("/");
    }

    
}