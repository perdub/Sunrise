using System.Reflection;

namespace Sunrise.Grabber;

[AttributeUsage(AttributeTargets.Class)]
public class Resource : Attribute{
    public Resource(string domain){
        _domain = domain;
    }
    private string _domain;
    public string Domain => _domain;
}