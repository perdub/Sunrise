namespace Sunrise;
using Sunrise.Logger;
using Sunrise.Storage;
public class Program
{
    public async static Task Main(string[] args)
    {
        Logger.Logger l = new Sunrise.Logger.Logger(new ConsoleLogger(), new FileLogger());
        l.Write("Enter in application.");

        ContentServer s = new ContentServer(
                "storage"
        );

        AspnetHoster hoster = new AspnetHoster();
        await hoster.StartApp();
    }
}