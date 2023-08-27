using System.Reflection;
using Allure.Xunit.StepAttribute;
namespace Sunrise.Tests;

public static class ResourceHelper
{
    [AllureStep("Get file from application resourse, name:{resourceName}")]
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