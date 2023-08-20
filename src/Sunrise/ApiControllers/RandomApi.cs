//апи для получения случаной картинки
using Microsoft.AspNetCore.Mvc;

namespace Sunrise.Api;

public class RandomApi: Controller
{
    SunriseContext db;
    ILogger logger;
    Random r;
    public RandomApi(SunriseContext sc, ILogger<RandomApi> l, Random rand)
    {
        db=sc;
        logger = l;
        r=rand;
    }

    Types.FileInfo? loadRandom(){
        var files = db.Files.Where(x => x.ContentType == Types.ContentType.Image);
        int allimg = files.Count();
        int t = r.Next(allimg);
        var file = files.Skip(t).FirstOrDefault();
        return file;
    }

    [HttpGet("random")]
    public async Task<IActionResult> RandomGet(){
        var file = loadRandom();
        if(file==null)
            return NotFound("No avalible images.");

        var st = System.IO.File.Open(file.Paths[0], FileMode.Open);
        
        return File(st, "image/jpg");
    }

    [HttpPost("random")]
    public async Task<IActionResult> RandomPost(){
        var file = loadRandom();
        if(file==null)
            return NotFound("No avalible images.");
        return Ok(new { path=file.Paths[1] });
    }
}