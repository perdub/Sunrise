using Microsoft.AspNetCore.Mvc;
using Sunrise.Database;
using Sunrise.Types.Enums;
using Sunrise.Types;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Sunrise.Mvc.Controllers;
[Route("random")]
public class RandomController(SunriseContext context, IConfiguration config) : Controller{
    [HttpGet]
    [Route("image")]
    public async Task<IActionResult> RandomImage(
        bool redirect = false
    ){
        var f = await context.Files
            .AsNoTracking()
            .Where(a=>a.PostType == PostType.Image)
            .OrderBy(a=>EF.Functions.Random())
            .Take(1)
            .FirstOrDefaultAsync();

        if(f is null){
            return NotFound("No files found");
        }

        //путь к файлу
        var n = f.GetItemPath(ContentVariant.Sample).Replace("/sunrise", ""); //блять не спрашивайте пж

        #if DEBUG
        HttpContext.Response.Headers.Add("S-FileId", f.FileId.ToString());
        HttpContext.Response.Headers.Add("S-FileName", n);
        #endif
        HttpContext.Response.Headers.Add("Cache-Control", "no-store, no-cache, max-age=0");

        if(redirect){
            var prefix = config.GetValue<string>("RequestPath");
            return Redirect(string.Concat(prefix, n).Replace("\\", "/"));
        }
        var pathPrefix = config.GetValue<string>("StoragePath");
        var fullPath = Path.Combine(pathPrefix, n[1..]);
        var stream = System.IO.File.Open(fullPath, System.IO.FileMode.Open);
        
        return File(stream, "image/jpg");
    }
}