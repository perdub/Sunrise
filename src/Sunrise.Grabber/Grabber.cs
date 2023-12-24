using System.Reflection;

namespace Sunrise.Grabber;

public class Grabber
{
    private Dictionary<string, Type> _grabbers;
    private HttpClient _client;

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
        return (ResourceGrabber)Activator.CreateInstance(_grabbers[domain], new object[] { _client });
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

    public Grabber(){
        _grabbers = new Dictionary<string, Type>();
        _client = new HttpClient();
        MapGrabbers();
    }
}