using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Sunrise.Types;
using Sunrise.Storage;
using Sunrise.Database;
using Sunrise.Grabber;

namespace Sunrise.Mvc.Controllers;


public class UploadController : Controller{
    private Storage.Storage _storage;
    private IServiceProvider _provider;
    private SunriseContext _context;
    private Sunrise.Grabber.Grabber _grabber;
    private ILogger<UploadController> _logger;

    public UploadController(Storage.Storage storage, IServiceProvider provider, SunriseContext context, Sunrise.Grabber.Grabber grabber, ILogger<UploadController> logger)
    {
        _provider = provider;
        _storage = storage;
        _context = context;
        _grabber = grabber;
        _logger = logger;
    }


    [Route("/upload")]
    [HttpPost]
    public async Task<IActionResult> UploadPost(){
        var sessionKey = HttpContext.Request.Cookies["Suntoken"];
        var isSessionExsist = (_context.Sessions.Where(a=>a.SessionId == sessionKey).FirstOrDefault()) != null;
        
        if(!isSessionExsist){
            return Unauthorized();
        }

        var file = HttpContext.Request.Form.Files;
        var tags = HttpContext.Request.Form["tags"][0];
        await _storage.SavePost(file[0].OpenReadStream().ToByteArray(), sessionKey, tags.Split(" "));

        return Ok();
    }

    [Route("/api/upload")]
    [HttpPost]
    //set max request size to 1GB
    [RequestSizeLimit(1024*1024*1024)]
    public async Task<IActionResult> ApiUpload(){
        var sessionKey = HttpContext.Request.Cookies["Suntoken"];

        if(sessionKey is null){
            return Unauthorized();
        }

        var isSessionExsist = (_context.Sessions.Where(a=>a.SessionId == sessionKey).FirstOrDefault()) != null;
        
        if(!isSessionExsist){
            return Unauthorized();
        }


        var ms = new MemoryStream();
        await HttpContext.Request.Body.CopyToAsync(ms);
        ms.Position = 0;
        var tags = HttpContext.Request.Headers["Tags"].ToArray();

        await _storage.SavePost(ms.ToByteArray(), sessionKey, tags);

        return Ok();
    }

    [Route("/grab")]
    public async Task<IActionResult> Grab(string grabUrl){
        var sessionKey = HttpContext.Request.Cookies["Suntoken"];

        if(sessionKey is null){
            return Unauthorized();
        }

        var isSessionExsist = (_context.Sessions.Where(a=>a.SessionId == sessionKey).FirstOrDefault()) != null;
        
        if(!isSessionExsist){
            return Unauthorized();
        }

        if(string.IsNullOrWhiteSpace(grabUrl)){
            return BadRequest();
        }

        var grabResult = await _grabber.Grab(new Uri(grabUrl));
        if(grabResult is null){
            return BadRequest();
        }
        foreach(var result in grabResult){
            if(!result.Success || result.Data is null || result.Data.Length == 0){
                _logger.LogDebug(message:"Unsuccesful grab result", result);
            }
            var tags = result.Tags is not null ? result.Tags.Split(" ") : Array.Empty<string>();
            await _storage.SavePost(result.Data, sessionKey, tags);
        }

        return Redirect("/forms/upload");
    }
}