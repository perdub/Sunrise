using System.Text.RegularExpressions;
using AngleSharp;
using Microsoft.Extensions.Logging;

namespace Sunrise.Grabber;

[Resource("idol.sankakucomplex.com")]
internal partial class IdolSankakuComplexGrabber : ResourceGrabber
{
    public override async Task<GrabResult[]> Grab()
    {
        _logger.LogDebug("idol.sankakucomplex.com grab: "+_url);

        if(_url.Contains("post")){
            return new GrabResult[] { await parsePost(_url) };
        }

        var html = await Client.GetStringAsync(_url);
        var d = await _context.OpenAsync(a=>a.Content(html));

        var postsOnPage = d.GetElementsByClassName("thumb");

        Task[] tasks = new Task[postsOnPage.Length];
        res = new GrabResult[postsOnPage.Length];

        for(int i = 0; i < postsOnPage.Length; i++){
            var post = postsOnPage[i];
            var src = (AngleSharp.Html.Dom.IHtmlAnchorElement)post.Children[0];
            var url = src.Href;
            url = url.Replace("localhost", "idol.sankakucomplex.com").Replace("http:", "https:");
            if(url.Contains("users/login")) {
                res[i] = new GrabResult { Success = false };
                tasks[i] = Task.CompletedTask;
                continue;
            }
            tasks[i] = parseThumb(url, i);
            await Task.Delay(150);
        }

        await Task.WhenAll(tasks);
        return res;
    }
    private async Task parseThumb(string postUrl, int resultIndex){
        res[resultIndex] = await parsePost(postUrl);
    }
    private async Task<GrabResult> parsePost(string postUrl){
        var responce = await Client.GetAsync(postUrl);
        if(!responce.IsSuccessStatusCode){
            _logger.LogDebug("post grab error: "+postUrl+" status: "+responce.StatusCode);
            return new GrabResult { Success = false };
        }
        string html = await responce.Content.ReadAsStringAsync();
        
        var d = await _context.OpenAsync(a=>a.Content(html));
        var src = d.GetElementById("highres");
        if(src is null){
            return new GrabResult { Success = false };
        }
        AngleSharp.Html.Dom.IHtmlAnchorElement srcAnchor = (AngleSharp.Html.Dom.IHtmlAnchorElement)src;
        var imageUrl = srcAnchor.Href;
        var imageRaw = await Client.GetByteArrayAsync(imageUrl);
        return new GrabResult { Data = imageRaw, Success = true };
    }

    public override void Initialize(string url)
    {
        _url = url;
    }
    private GrabResult[] res;
    private string _url;
    private IBrowsingContext _context;
    private ILogger<IdolSankakuComplexGrabber> _logger;
    public IdolSankakuComplexGrabber(HttpClient client, IBrowsingContext context, ILogger<IdolSankakuComplexGrabber> logger) : base(client)
    {
        _context = context;
        _logger = logger;
    }
    
}