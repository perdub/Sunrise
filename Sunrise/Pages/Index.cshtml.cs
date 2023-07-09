using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sunrise.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    CacheService cs;
    public IndexModel(ILogger<IndexModel> logger, CacheService s)
    {
     cs=s;
        _logger = logger;
    }

    public Types.Post[] posts {get;private set;}
    public async Task OnGetAsync(string[] tags)
    {
        var find = new Api.FindApi(cs);
        posts = await find.Find(tags);
    }
    public string getPreview(Guid fileId){
        return cs.GetFileAsync(fileId).Result.Paths[0];
    }
}
