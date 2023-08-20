namespace Sunrise.Tunnels;

public class TunnelsManager
{
    IConfiguration _config;
    ILogger<TunnelsManager> _logger;
    public TunnelsManager(ILogger<TunnelsManager> logger, IConfiguration config)
    {
        _logger=logger;
        _config = config;
    }

    public void BuildTunnels(params ITunnel[] tunnels)
    {
        foreach(var a in tunnels){
            a.StartTunnel(_logger, _config);
        }
    }
}