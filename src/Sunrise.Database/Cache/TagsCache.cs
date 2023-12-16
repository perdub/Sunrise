using Sunrise.Types;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace Sunrise.Database;
#pragma warning disable CS8652
public class TagsCache(SunriseContext context, IMemoryCache cache)
{
    public async Task<Tag> GetTag(string tag)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// try to get tags that starts with the given string
    /// </summary>
    public Tag[] GetTagsToComplete(string start){
        string key = string.IsNullOrEmpty(start) ? "All" : start;

        var cont = cache.TryGetValue(key, out Tag[] tags);
        if(cont){
            return tags;
        }
        /*
        while(true){
            
            if(cache.TryGetValue("empty"+key, out int f)){
                if(f==1){
                    return Array.Empty<Tag>();
                }

            }
            else{
                key = key[..^1];
                if(key.Length == 1){
                    break;
                }
            }
        }//*/

        
        tags = context.Tags
            .AsNoTracking()
            .OrderByDescending(a=>a.PostCount)
            .Where(a=>a.TagText.StartsWith(start))
            .Take(10)
            .ToArray();

        if(tags.Length < 10){
            var ftags = new Tag[10];
            Array.Copy(tags, ftags, tags.Length);
            int diff = 10 - tags.Length;

            var ctags = context.Tags
                .AsNoTracking()
                .OrderByDescending(a=>a.PostCount)
                .Where(a=>a.TagText.Contains(start))
                .ToArray();

            ctags = ctags
                .Where(a=>!tags.Any(b=>b.TagId == a.TagId))
                .Take(diff)
                .ToArray();
            
            Array.Copy(ctags, 0, ftags, tags.Length, ctags.Length);

            tags = ftags;
        }

        var l = new List<Tag>(10);
        foreach(var t in tags){
            if(t is not null)
                l.Add(t);
            else
                break;
        }
        tags = l.ToArray();

        cache.Set(key, tags, TimeSpan.FromMinutes(10));
/*
        if(tags.Length == 0){
            cache.Set("empty"+key, 1, TimeSpan.FromMinutes(10));
        }//*/

        return tags;
    }
}