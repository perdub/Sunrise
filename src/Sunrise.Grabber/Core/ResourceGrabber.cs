namespace Sunrise.Grabber;

internal abstract class ResourceGrabber
{
    public abstract void Initialize(string url);
    public abstract Task<GrabResult[]> Grab();
    public HttpClient Client { get; set; }
    public ResourceGrabber(HttpClient client)
    {
        Client = client;
    }
}