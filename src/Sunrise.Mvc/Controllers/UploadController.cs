using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Sunrise.Types;
using Sunrise.Storage;
using Sunrise.Database;

namespace Sunrise.Mvc.Controllers;


public class UploadController : Controller{
    private Storage.Storage _storage;
    private IServiceProvider _provider;
    private SunriseContext _context;

    public UploadController(Storage.Storage storage, IServiceProvider provider, SunriseContext context)
    {
        _provider = provider;
        _storage = storage;
        _context = context;
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
}