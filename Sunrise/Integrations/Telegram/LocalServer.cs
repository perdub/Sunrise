namespace Sunrise.Integrations.Telegram;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public static class LocalServer{
    const string WINDOWS_BIN_PATH = "/binarys/telegram-bot-server/Windows/telegram-bot-api.exe";
    const string LINUX_BIN_PATH = "/binarys/telegram-bot-server/Linux/telegram-bot-api";

    public static void Run(ILogger logger, string api_id, string api_hash, string workdir, string tempdir, int port = 8081, int statPort = 5678, bool hideConsole=true)
    {
        Process p = new Process();
        //проверка платформы и выбор исполняемого файла для нее
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){
            p.StartInfo.FileName = Environment.CurrentDirectory + WINDOWS_BIN_PATH;
            logger.LogDebug("OS - Windows.");
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)){
            p.StartInfo.FileName = Environment.CurrentDirectory + LINUX_BIN_PATH;
            logger.LogDebug("OS - Linux.");
        }
        else{
            logger.LogError("Unsupported platform to Telegram Local Bot Api Server.");
            throw new PlatformNotSupportedException("unsupported platform");
        }
        //создание строки с параметрами
        StringBuilder sb = new ();
        sb.Append($"--local ");
        sb.Append($"--dir={workdir} ");
        sb.Append($"--temp-dir={tempdir} ");
        sb.Append($"--http-port={port} ");
        sb.Append($"--http-stat-port={statPort} ");

        logger.LogInformation($"Telegram Local Bot Api Server params is {sb.ToString()}\napi-id and api-hash not shown due to security reasons.");

        sb.Append($"--api-id={api_id} ");
        sb.Append($"--api-hash={api_hash} ");

        p.StartInfo.Arguments = sb.ToString();

        if(hideConsole==false){
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
        }

        p.Start();

        logger.LogInformation($"Telegram Local Bot Server Api started, PID is {p.Id}");
    }
}