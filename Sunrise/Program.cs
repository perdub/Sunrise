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
        RunTelegramBot(args);


        Logger.Logger l = new Sunrise.Logger.Logger(new ConsoleLogger(), new FileLogger());
        l.Write($"Sunrise {VERSION}-{CONFIG}\nIf config is debug, version can be incorrect.");
        l.Write("Enter in application.");


        ContentServer s = new ContentServer(
                "storage"
        );

        AspnetHoster hoster = new AspnetHoster();
        await hoster.StartApp(args);
    }

    static void RunTelegramBot(string[] args){
#pragma warning disable CS8604
        //создание логгера
        var lb = LoggerFactory.Create((x)=>{
            x.AddConsole();
        });
        var logger = lb.CreateLogger<Sunrise.Integrations.Telegram.TelegramBot>();
        //создание временной конфигурации
        var configBuilder = new ConfigurationBuilder();
        if (File.Exists("telegram.json"))
        {
            configBuilder.AddJsonFile("telegram.json");
        }
        else if(File.Exists("telegram.Example.json"))
        {
            configBuilder.AddJsonFile("telegram.Example.json");
        }
        configBuilder
            .AddEnvironmentVariables()
            .AddCommandLine(args);


        IConfiguration tempConfig = configBuilder.Build();
        if (tempConfig.GetValue<bool>("active_telegram_bot"))
        {
            bool useLocalServer = tempConfig.GetValue<bool>("localServer:useServer");
            if(useLocalServer){
                Sunrise.Integrations.Telegram.LocalServer.Run(
                    logger,
                    tempConfig.GetValue<string>("localServer:api_id"),
                    tempConfig.GetValue<string>("localServer:api_hash"),
                    tempConfig.GetValue<string>("localServer:workingDir"),
                    tempConfig.GetValue<string>("localServer:tempDir"),
                    tempConfig.GetValue<int>("localServer:httpPort"),
                    tempConfig.GetValue<int>("localServer:httpStatPort"),
                    tempConfig.GetValue<bool>("localServer:hideConsole"));
            }
            new Sunrise.Integrations.Telegram.TelegramBot(
                tempConfig.GetValue<string>("telegram_bot"),
                logger,
                useLocalServer,
                $"http://localhost:{tempConfig.GetValue<int>("localServer:httpPort")}");
        }
#pragma warning restore
    }
}
