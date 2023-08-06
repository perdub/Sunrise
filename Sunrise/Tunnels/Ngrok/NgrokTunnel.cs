namespace Sunrise.Tunnels.Ngrok;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public class NgrokTunnel : ITunnel
{
    Process ngrok;
    const string WINDOWS_BIN_PATH = "/binarys/ngrok-tunnel/Windows/ngrok.exe";
    const string LINUX_BIN_PATH = "/binarys/ngrok-tunnel/Linux/ngrok.exe";
    public async void StartTunnel(ILogger logger, IConfiguration config)
    {
        if(config.GetValue<bool>("ngrok:useNgrok")==false){
            logger.LogWarning($"\"useNgrok\" is false, ngrok agent will not start.");
        }
        ngrok = new Process();

        bool useShell = config.GetValue<bool>("ngrok:useShell");

        if(useShell)
        {
            ngrok.StartInfo.FileName = "ngrok";
        }
        else
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)){
               ngrok.StartInfo.FileName = Environment.CurrentDirectory + WINDOWS_BIN_PATH;
               logger.LogDebug("OS - Windows.");
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)){
               ngrok.StartInfo.FileName = Environment.CurrentDirectory + LINUX_BIN_PATH;
                logger.LogDebug("OS - Linux.");
            }
            else{
                logger.LogError("Unsupported platform to default Ngrok.");
            }
        }

        StringBuilder param = new ();
        param.Append("http ");
        param.Append(config.GetValue<string>("ngrok:port")+' ');

        if(!useShell && !config.GetValue<bool>("ngrok:skipAuthefication")){
            param.Append("--authtoken=");
            param.Append(config.GetValue<string>("ngrok:authtoken")+' ');
        }

        bool useDomain = config.GetValue<bool>("ngrok:useDomain");

        if(useDomain){
            param.Append("--domain=");
            param.Append(config.GetValue<string>("ngrok:domain")+' ');
        }

        param.Append("--region=");
        param.Append(config.GetValue<string>("ngrok:region")+' ');

        param.Append(config.GetValue<string>("ngrok:otherParams"));

        ngrok.StartInfo.Arguments = param.ToString();

        ngrok.StartInfo.RedirectStandardOutput = true;

        ngrok.Start();

        logger.LogInformation($"Ngrok tunnel started, PID is {ngrok.Id}");

        string addres = await Sunrise.Utilities.HttpExtensions.GetNgrokTunnelUrl();
        logger.LogInformation($"Ngrok tunnel addres is {addres}");
    }

    public async Task<string> GetTunnelUrl(ILogger logger, IConfiguration config)
    {
        return await Sunrise.Utilities.HttpExtensions.GetNgrokTunnelUrl();
    }
}