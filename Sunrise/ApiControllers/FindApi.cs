using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sunrise.Api;

[Route("api/find")]
public class FindApi : Controller
{
    //сохранение доступа к контексту дб локально
    SunriseContext cs;

    ILogger<FindApi>? _logger;

    public FindApi(SunriseContext c, ILogger<FindApi>? logger = null)
    {
        _logger = logger;
        cs = c;
    }

    
    //поиск
    public async Task<Types.Post[]> Find(string[] tagsSearch, int offset = 0, int count = Constants.POST_PER_PAGE)
    {
        _logger?.LogDebug("Start find in db!");
        //todo: rewrite this shit!!!
        Types.Tag[] tags = new Types.Tag[tagsSearch.Length];
        for (int i = 0; i < tagsSearch.Length; i++)
        {
            tags[i] = await cs.Tags.Where(a=>a.SearchText==tagsSearch[i]).FirstOrDefaultAsync();
        }
        var idrep = tags.Select(a => a.TagId).ToArray();
        tags = tags.OrderBy(x => x.PostCount).ToArray();
        var r = cs.Posts.Include(q => q.Tags).Include(a=>a.File)
            .Where(a=>(int)a.Status>0)
            .OrderByDescending(g => g.PostCreationTime)
            .AsEnumerable()
            .Where((x) =>
            {
                HashSet<int> tg = new HashSet<int>(x.Tags.Select(w => w.TagId));
                return tg.IsSupersetOf(idrep);
            })
            .Skip(offset)
            .Take(count);
            _logger?.LogDebug($"Find end, found {r.Count()} posts!");
        return r.ToArray();
    }
}