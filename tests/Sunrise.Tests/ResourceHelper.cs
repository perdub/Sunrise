using System.Reflection;

namespace Sunrise.Tests;

public static class ResourceHelper
{
    public static byte[] GetResource(string resourceName)
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        Stream? stream = asm.GetManifestResourceStream(resourceName);

        if(stream==null)
            throw new NullReferenceException();

        var arr = Sunrise.Utilities.StreamExtensions.ToByteArray(stream);
        stream.Dispose();
        return arr;
    }
}