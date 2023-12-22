using Microsoft.AspNetCore.Mvc;
using Sunrise.Database;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Sunrise.Mvc;

[Route("tags")]
#pragma warning disable CS8652
public class TagsController(TagsCache cache, IMemoryCache mCache) : Controller{
    [HttpGet]
    [Route("complete")]
    public async Task<IActionResult> Complete(string tag = ""){
        var tags = await getTagsJson();
        HttpContext.Response.Headers.CacheControl = $"max-age={5*60}";
        return Ok(tags);
    }
    private async Task<string> getTagsJson(){
        if(mCache.TryGetValue("all_tags_json", out string tagsJson)){
            return tagsJson;
        }
        StringBuilder bld = new StringBuilder();
        bld.Append("{\"tags\":\"");
        var tags = await cache.GetTagsToComplete();
        bld.Append(tags);
        bld.Append("\"}");
        string json = bld.ToString();
        mCache.Set("all_tags_json", json, TimeSpan.FromSeconds(20));
        return json;
    }
}