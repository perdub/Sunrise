namespace Sunrise.Api;
using Microsoft.AspNetCore.Mvc;

[Route("files")]
public class FileApi : Controller
{
    SunriseContext db;
    public FileApi(SunriseContext c)
    {
        db=c;
    }
    [Route("get/original/image/{id:guid}")]
    public async Task<IActionResult> Get(Guid id){
        var a = db.Files.Find(id);
        string finalPath = a.Paths[0];//изменить способ получения путей, ибо сейчас мы просто берём только первый
        DirectoryInfo dir = new DirectoryInfo(finalPath);
        if(a==null)
            return NotFound("Not found");
        if(a.ContentType==Types.ContentType.Image)
            finalPath+="original";

        FileInfo[] original = dir.GetFiles("original.*"); //по идее там должно быть только один файл с таким названием

        var stream = System.IO.File.Open(Path.GetFullPath(original[0].FullName), FileMode.Open);
        
        return File(stream, $"image/{original[0].Extension.Substring(1).ToLower()}");
    }
}