using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Reflection;

namespace Sunrise.Grabber;

public class Grabber
{
    private Dictionary<string, Type> _grabbers;
    private HttpClient _client;
    private IServiceProvider _provider;

    private void MapGrabbers(){
        var assm = Assembly.GetExecutingAssembly();
        var types = assm.GetTypes();

        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<Resource>();
            bool isInheritFromBaseGrabber = type.BaseType == typeof(ResourceGrabber);
            if(attribute is not null && isInheritFromBaseGrabber){
                _grabbers.Add(attribute.Domain, type);
            }
        }
    }
    private ResourceGrabber? CreateGrabberInstance(string domain){
        if(!_grabbers.ContainsKey(domain)){
            return null;
        }
        var instance = ActivatorUtilities.CreateInstance(_provider, _grabbers[domain], new object[] { _client });
        return (ResourceGrabber)instance;
    }

    public async Task<GrabResult[]?> Grab(Uri uri){
        if(!_grabbers.TryGetValue(uri.Host, out Type type)){
            return null;
        }
        var grabber = CreateGrabberInstance(uri.Host);
        grabber.Initialize(uri.AbsoluteUri);
        var r = await grabber.Grab();
        return r;
    }

    public Grabber(IServiceProvider provider){
        _provider = provider;
        _grabbers = new Dictionary<string, Type>();

        var h = new HttpClientHandler();
        h.AllowAutoRedirect = true;
        h.CookieContainer.Capacity = 1024;
        h.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli;
        _client = new HttpClient(h);

        MapGrabbers();
    }
}