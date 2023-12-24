using System.Text.RegularExpressions;

namespace Sunrise.Grabber;

[Resource("pin.it")]
internal partial class PinterestGrabber : ResourceGrabber
{
    public override async Task<GrabResult[]> Grab()
    {
        var req = await Client.GetStringAsync(_url);
        var imageUrl = getFileUrl(req);
        var image = await Client.GetByteArrayAsync(imageUrl);
        return new GrabResult[] { new GrabResult { Data = image, Success = true } };
    }

    public override void Initialize(string url)
    {
        _url = url;
    }

    private string getFileUrl(string html){
        var matches = Regex.Matches(html, REGEX_PATTERN);
        var url = matches[^1].Value;
        url = url.Substring(1, url.Length - 2);
        return url;
    }

    private string _url;
    public PinterestGrabber(HttpClient client) : base(client)
    {
        
    }
    private const string REGEX_PATTERN = @"""https:\/\/i.pinimg.com\/originals.+?""";
}