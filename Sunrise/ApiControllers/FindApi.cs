using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sunrise.Api;

[Route("api/find")]
public class FindApi : Controller
{
    //сохранение доступа к контексту дб локально
    CacheService cs;
    public FindApi(CacheService c)
    {
        cs = c;
    }

    
    //поиск
    public async Task<Types.Post[]> Find(string[] tagsSearch, int offset = 0, int count = 50)
    {
        Sunrise.Logger.Logger.Singelton.Write("Start find in db!");
        //todo: rewrite this shit!!!
        Types.Tag[] tags = new Types.Tag[tagsSearch.Length];
        for (int i = 0; i < tagsSearch.Length; i++)
        {
            tags[i] = await cs.GetTagAsync(tagsSearch[i]);
        }
        var idrep = tags.Select(a => a.TagId).ToArray();
        tags = tags.OrderBy(x => x.PostCount).ToArray();
        var r = cs.dbcontext.Posts.Include(q => q.Tags)
            .OrderByDescending(g => g.PostCreationTime)
            .AsEnumerable()
            .Where((x) =>
            {
                HashSet<int> tg = new HashSet<int>(x.Tags.Select(w => w.TagId));
                return tg.IsSupersetOf(idrep);
            })
            .Skip(offset)
            .Take(count);
            Sunrise.Logger.Logger.Singelton.Write($"Find end, found {r.Count()} posts!");
        return r.ToArray();
    }
}