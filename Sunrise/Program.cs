namespace Sunrise;

using Sunrise.Integrations;
using Sunrise.Logger;
using Sunrise.Storage;
public class Program
{
    const string VERSION = "0.1.0";
    #region Config
    readonly static string CONFIG = 
    #if DEBUG
    "debug";
    #endif
    #if RELEASE
    "release";
    #endif
    #endregion
    public async static Task Main(string[] args)
    {
        Logger.Logger l = new Sunrise.Logger.Logger(new ConsoleLogger(), new FileLogger());
        l.Write($"Sunrise {VERSION}-{CONFIG}\nIf config is debug, version can be incorrect.");
        l.Write("Enter in application.");

        enableTelegramBot();

        ContentServer s = new ContentServer(
                "storage"
        );

        AspnetHoster hoster = new AspnetHoster();
        await hoster.StartApp();
    }
    static async Task enableTelegramBot(){
        IBot q = new Sunrise.Integrations.TelegramBot();
        await q.Initialization(()=>{return "6328214388:AAGvPxAZxzk6zbKES1q4v-c-Ih-_gKTMZFk";});
        q.OnInputEvent += (message, id)=>{
            q.Send(()=>{
                return id;
            }, message.Reverse().ToString());
        };
    }
}