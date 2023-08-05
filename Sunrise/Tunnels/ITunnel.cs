namespace Sunrise.Tunnels;

public interface ITunnel
{
    void StartTunnel(ILogger logger, IConfiguration config);
}