namespace Sunrise;

using Sunrise.Integrations;
using Sunrise.Logger;
using Sunrise.Storage;
public class Program
{
    const string VERSION = "0.3.0";
    public static readonly DateTime StartTime = DateTime.UtcNow;
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
        var configBuilder = new ConfigurationBuilder();
        if (File.Exists("tokens.json"))
        {
            configBuilder.AddJsonFile("tokens.json");
        }
        else if(File.Exists("tokens.Example.json"))
        {
            configBuilder.AddJsonFile("tokens.Example.json");
        }
        configBuilder
            .AddEnvironmentVariables()
            .AddCommandLine(args);


        IConfiguration tempConfig = configBuilder.Build();
        if (tempConfig.GetValue<bool>("active_telegram_bot"))
        {
            new Sunrise.Integrations.Bots.TelegramBot(tempConfig.GetValue<string>("telegram_bot"));
        }

        Logger.Logger l = new Sunrise.Logger.Logger(new ConsoleLogger(), new FileLogger());
        l.Write($"Sunrise {VERSION}-{CONFIG}\nIf config is debug, version can be incorrect.");
        l.Write("Enter in application.");


        ContentServer s = new ContentServer(
                "storage"
        );

        AspnetHoster hoster = new AspnetHoster();
        await hoster.StartApp(args);
    }
}
