using Sunrise.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Sunrise.Database;
#pragma warning disable CS8652
public class TagsCache(SunriseContext context, IMemoryCache cache)
{
    /// <summary>
    /// try to get all tags and save it as string
    /// </summary>
    public async Task<string> GetTagsToComplete(){
        if(cache.TryGetValue("all_tags_raw", out string tagsRaw)){
            return tagsRaw;
        }

        var tags = await context.Tags
            .AsNoTracking()
            .Select(t => t.TagText)
            .ToArrayAsync();

        StringBuilder bld = new StringBuilder();
        foreach(var tag in tags){
            bld.Append(tag);
            bld.Append(';');
        }
        string raw = bld.ToString();
        cache.Set("all_tags_raw", raw);

        return raw;
    }
}