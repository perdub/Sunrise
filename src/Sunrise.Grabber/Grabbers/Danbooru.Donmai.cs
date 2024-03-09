using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Sunrise.Grabber;

[Resource("danbooru.donmai.us")]
internal partial class DanbooruGrabber : ResourceGrabber
{
    public override async Task<GrabResult[]> Grab()
    {
        _logger.LogDebug("Request to Danbooru API...");
        var message = new HttpRequestMessage(HttpMethod.Get, _url);
        message.Headers.Add("User-Agent", "Sunrise.Bot");
        var response = await Client.SendAsync(message);
        var json = await response.Content.ReadAsStringAsync();
        _logger.LogDebug("Parse Json result...");
        var post = Danbooru.FromJson(json);
        _logger.LogDebug("Download file...");
        message = new HttpRequestMessage(HttpMethod.Get, post.FileUrl);
        message.Headers.Add("User-Agent", "Sunrise.Bot");
        response = await Client.SendAsync(message);
        var image = await response.Content.ReadAsByteArrayAsync();
        _logger.LogDebug("Danbooru parsed!");
        return new GrabResult[] { new GrabResult { Data = image, Success = true, Tags = post.TagString } };
    }

    public override void Initialize(string url)
    {
        _url = string.Concat(url,".json");
    }


    private string _url;
    private ILogger<DanbooruGrabber> _logger;
    public DanbooruGrabber(HttpClient client, ILogger<DanbooruGrabber> logger) : base(client)
    {
        _logger = logger;
    }
}