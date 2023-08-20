namespace Sunrise.Utilities;

public static class HttpExtensions
{
    public static async Task<string> GetNgrokTunnelUrl(){
        var a = await Http.GetJsonRequest("http://localhost:4040/api/tunnels");
        return a.tunnels[0].public_url;
    }
}