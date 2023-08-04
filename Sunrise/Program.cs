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
        ConfigurationBuilder(args);

        ApplySettings();

        Logger.Logger l = new Sunrise.Logger.Logger(new ConsoleLogger(), new FileLogger());
        l.Write($"Sunrise {VERSION}-{CONFIG}\nIf config is debug, version can be incorrect.");
        l.Write("Enter in application.");
        
        TestDbConnection(Config.GetValue<bool>("createDbIfFall"));

        RunTelegramBot(args);

        ContentServer s = new ContentServer(
                "storage"
        );

        AspnetHoster hoster = new AspnetHoster();
        await hoster.StartApp(args);
    }

    static void ApplySettings(){
        
    }

    static void TestDbConnection(bool createIfFall = false){
        using(SunriseContext sunriseContext = new SunriseContext()){
            var l = LoggerBuilder<Program>();
            if(sunriseContext.Database.CanConnect()){
                l.LogInformation($"Sussesful connect to database.");
            }
            else{
                if(!createIfFall)
                {
                    l.LogCritical($"Fall to connect to database! Try to check you connection string and is your server avalible.");
                    Environment.Exit(1);
                }
                else
                {
                    l.LogWarning($"Fall to connect to database, try to create...");
                    sunriseContext.Database.EnsureCreated();
                }
            }
        }
    }

    static void RunTelegramBot(string[] args){
#pragma warning disable CS8604
        //создание логгера
        var logger = LoggerBuilder<Sunrise.Integrations.Telegram.TelegramBot>();
        //создание временной конфигурации
        if (Config.GetValue<bool>("active_telegram_bot"))
        {
            bool useLocalServer = Config.GetValue<bool>("localServer:useServer");
            if(useLocalServer){
                Sunrise.Integrations.Telegram.LocalServer.Run(
                    logger,
                    Config.GetValue<string>("localServer:api_id"),
                    Config.GetValue<string>("localServer:api_hash"),
                    Config.GetValue<string>("localServer:workingDir"),
                    Config.GetValue<string>("localServer:tempDir"),
                    Config.GetValue<int>("localServer:httpPort"),
                    Config.GetValue<int>("localServer:httpStatPort"),
                    Config.GetValue<bool>("localServer:hideConsole"));
            }
            new Sunrise.Integrations.Telegram.TelegramBot(
                Config.GetValue<string>("telegram_bot"),
                logger,
                useLocalServer,
                $"http://localhost:{Config.GetValue<int>("localServer:httpPort")}");
        }
#pragma warning restore
    }

    static ILoggerFactory factory = LoggerFactory.Create((x)=>{
            x.AddConsole();
            x.AddFile("logs/appstart.log");
        });
    static ILogger<T> LoggerBuilder<T>(){
        return factory.CreateLogger<T>();
    }
    public static IConfiguration Config {get;private set;}
    static void ConfigurationBuilder(string[] configuration)
    {
        var configBuilder = new ConfigurationBuilder();
        #region Settings
        if(File.Exists("settings.json"))
        {
            configBuilder.AddJsonFile("settings.json");
        }
        else if(File.Exists("settings.Example.json"))
        {
            configBuilder.AddJsonFile("settings.Example.json");
        }
        #endregion
        #region Telegram Config
        if (File.Exists("telegram.json"))
        {
            configBuilder.AddJsonFile("telegram.json");
        }
        else if(File.Exists("telegram.Example.json"))
        {
            configBuilder.AddJsonFile("telegram.Example.json");
        }
        #endregion
        configBuilder
            .AddEnvironmentVariables()
            .AddCommandLine(configuration);


        Config = configBuilder.Build();
    }
}
