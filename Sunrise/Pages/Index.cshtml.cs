using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sunrise.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    SunriseContext cs;
    public IndexModel(ILogger<IndexModel> logger, SunriseContext s)
    {
     cs=s;
        _logger = logger;
    }

    public Types.Post[] posts {get;private set;}
    public async Task OnGetAsync(string[] tags, int offset = 0)
    {
        var find = new Api.FindApi(cs);
        posts = await find.Find(tags, offset);
    }
    public string getPreview(Guid fileId){
        return cs.Files.Find(fileId).Paths[0];
    }
}
