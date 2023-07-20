using System.Text;
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
    public int Page{get;private set;}
    public Types.Post[] posts {get;private set;}
    public async Task OnGetAsync(string[] tags, int pageid = 1)
    {
        Page =  pageid-1;
        _logger.Log(LogLevel.Information, $"New index request, page {Page}");
        var find = new Api.FindApi(cs);
        //page*POST_PER_PAGE даёт нужное смещение
        posts = await find.Find(tags, Page*Constants.POST_PER_PAGE);
    }
    public string getPreview(Guid fileId){
        return cs.Files.Find(fileId).Paths[0];
    }

    //этот метод создаёт адрес для страницы и сохраняет теги и тому подобное
    public string BuildUrl(int page = 0){
        StringBuilder sb = new ();
        sb.Append("/?");

        bool isPageNotSet = true;

        foreach(var a in HttpContext.Request.Query){
            if(a.Key=="pageid"){
                //increment page
                sb.Append($"{a.Key}={page+1}");
                isPageNotSet = false;
            }
            else{
                sb.Append($"{a.Key}={a.Value}");
            }
        }
        if(isPageNotSet){
            sb.Append($"pageid={page+1}");
        }
        return sb.ToString();
    }

    public bool IsLastPage(int page)
    {
        return cs.Posts.Count() < page+1 * Constants.POST_PER_PAGE;
    }
}
