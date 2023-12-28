using System.Text.RegularExpressions;

namespace Sunrise.Grabber;

[Resource("telegra.ph")]
internal partial class TelegraphGrabber : ResourceGrabber
{
    public override async Task<GrabResult[]> Grab()
    {
        var req = await Client.GetStringAsync(_url);
        var images = Regex.Matches(req, REGEX_PATTERN);

        res = new GrabResult[images.Count];
        tasks = new Task[images.Count];

        var u = new Uri(_url);
        string prefix = u.Scheme+"://"+u.Host;

        for(int i = 0; i < images.Count; i++){
            var urlPrefix = images[i].Value.Substring(1, images[i].Value.Length - 2);
            var fullUrl = prefix + urlPrefix;
            tasks[i] = DownloadImage(i, fullUrl);
        }

        await Task.WhenAll(tasks);
        return res;
    }

    private GrabResult[] res;
    private Task[] tasks;
    private async Task DownloadImage(int num, string url){
        var image = await Client.GetByteArrayAsync(url);
        res[num] = new GrabResult{Data=image, Success = true};
    }

    public override void Initialize(string url)
    {
        _url = url;
    }

    

    private string _url;
    public TelegraphGrabber(HttpClient client) : base(client)
    {
        
    }
    private const string REGEX_PATTERN = @"""/file/.+?""";
}