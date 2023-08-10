using Microsoft.AspNetCore.Mvc;
using Sunrise.Utilities;
namespace Sunrise.Controllers;

[Route("upload")]
public class Upload : Controller
{
    //сохранение доступа к контексту дб локально
    SunriseContext cs;
    ILogger<Upload> _logger;
    public Upload(SunriseContext c, ILogger<Upload> logger)
    {
        cs = c;
        _logger = logger;
    }

    /*загрузка картинки
    [RequestSizeLimit(1024 * 1024 * 128)]
    [Route("image")]
    [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadImage()
    {
        if (!HttpContext.Items.IsUser())
        {
            return Unauthorized(new { message = "login reqried" });
        }
        var uploaded = HttpContext.Request.Form.Files;
        var tags = HttpContext.Request.Form["tags"];
        var res = await Sunrise.Storage.ContentServer.Singelton.SaveImage(Guid.NewGuid(), uploaded[0].OpenReadStream().ToByteArray(), Path.GetExtension(uploaded[0].FileName));
        var tagsArr = await (new Api.TagsApi(cs).GetTagsAsync(tags[0].Split(), cs));
        Types.Post newPost = new Types.Post(HttpContext.Items.UserId(), res);

        foreach (var tag in tagsArr)
        {
            newPost.Tags.Add(tag);
            tag.Post.Add(newPost);
            tag.PostCount++;
        }
        cs.Tags.UpdateRange(tagsArr);
        cs.Posts.Add(newPost);
        cs.Files.Add(res);

        Sunrise.Logger.Logger.Singelton.Write($"Tags in json: {System.Text.Json.JsonSerializer.Serialize<Types.Tag[]>(tagsArr)}");
        await cs.SaveChangesAsync();
        //проверка на нужен ли редирект на домашнюю страницу
        var r = String.IsNullOrEmpty(HttpContext.Request.Query["redirecttohome"]);

        return r ? Ok(new { message = "sussesful", post = newPost, file = res }) : RedirectPermanent("/");
    }//*/
    [RequestSizeLimit(1024 * 1024 * 1024)]
    [Route("items")]
    public async Task<IActionResult> UploadEndpoint()
    {
        Guid uploadTask = Guid.NewGuid();
        _logger.Log(LogLevel.Debug, $"New upload started, id - {uploadTask}");
        if (!HttpContext.Items.IsUser())
        {
            return Unauthorized(new { message = "login reqried" });
        }
        var items = HttpContext.Request.Form.Files;
        var userid = HttpContext.Items.UserId();
        string tags = HttpContext.Request.Form["tags"][0];
        var uploadTasks = new List<Task>();
        foreach (var i in items)
        {
            uploadTasks.Add(Task.Run(async () =>
            {
                try
                {
                    //здесь создается новый контекст бд для каждого файла ибо они конфилктуют
                    var db = new SunriseContext();
                    await db.Upload(userid, i.OpenReadStream().ToByteArray(), Path.GetExtension(i.FileName), tags);
                }
                catch (Sunrise.Types.Exceptions.InvalidObjectTypeException iot)
                {
                    _logger.Log(LogLevel.Warning, iot, iot.Message);
                }

            }));
        }

        await Task.WhenAll(uploadTasks);
        _logger.Log(LogLevel.Debug, $"{uploadTask} upload task finished, served {uploadTasks.Count()} posts.");

        var r = String.IsNullOrEmpty(HttpContext.Request.Query["redirecttohome"]);

        return r ? Ok(new { message = "sussesful" }) : RedirectPermanent("/");
    }
}