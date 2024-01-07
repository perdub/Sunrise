using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Sunrise.Grabber;

[Resource("fapello.su")]
internal partial class FapelloGrabber : ResourceGrabber
{
    private List<GrabResult> results = new List<GrabResult>();
    public override async Task<GrabResult[]> Grab()
    {
        _logger.LogTrace("New grab: "+_url);

        var req = await Client.GetStringAsync(_url);
        await parseAndAdd(req);

        int page = 1;
        do{
            var html = await requestNext(page);
            await parseAndAdd(html);
            page++;
        }
        while(notEmpryResponce);

        _logger.LogTrace("End grab: total: "+results.Count);

        return results.ToArray();
    }

    public override void Initialize(string url)
    {
        _url = url;
        var a = new Uri(_url);
        model_info = a.AbsolutePath;
    }

    private async Task parseAndAdd(string html){
        var matches = Regex.Matches(html, REGEX_PATTERN);
        var ta = new Task[matches.Count];
        for (int i = 0; i < matches.Count; i++)
        {
            var url = matches[i].Value;
            url = url.Substring(1, url.Length - 2);
            url = sanitizeLink(url);
            ta[i] = download(url);
        }
        await Task.WhenAll(ta);
    }

    private async Task download(string url){
        var image = await Client.GetByteArrayAsync(url);
        results.Add(new GrabResult{Data = image, Success = true});
    }

    private async Task<string> requestNext(int page){
        string url = $"https://fapello.su/ajax/model_new{model_info}page-{page}/photos";

        _logger.LogTrace("Request /ajax/model_new/\n\tPage:"+page);

        var s = await Client.GetStringAsync(url);

        if(string.IsNullOrWhiteSpace(s)){
            notEmpryResponce = false;
            
        }
        return s;
    }

    //clear links from *.md.* because it`s give best quality
    private string sanitizeLink(string link){
        return link.Replace(".md", string.Empty).Replace(".th", string.Empty);
    }

    private bool notEmpryResponce = true;

    private string _url;
    private string model_info = "";
    private ILogger<FapelloGrabber> _logger;
    public FapelloGrabber(ILogger<FapelloGrabber> logger, HttpClient client) : base(client)
    {
        _logger = logger;
    }
    private const string REGEX_PATTERN = @"""https://(simp\d\.jpg\.church||i\.pixl\.li)/(images).+?""";
}