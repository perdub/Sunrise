using Microsoft.AspNetCore.Mvc;
using Sunrise.Database;

namespace Sunrise.Mvc;

[Route("tags")]
public class TagsController(TagsCache cache) : Controller{
    [HttpGet]
    [Route("complete")]
    public async Task<IActionResult> Complete(string tag = ""){
        var tags = cache.GetTagsToComplete(tag);
        HttpContext.Response.Headers.CacheControl = $"max-age={5*60}";
        return Ok(tags);
    }
}