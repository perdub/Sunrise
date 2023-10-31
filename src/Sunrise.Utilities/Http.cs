namespace Sunrise.Utilities;

using Newtonsoft.Json;

public static class Http
{
    static HttpClient _client;

    static Http()
    {
        _client = new HttpClient();
    }

    public static async Task<dynamic> GetJsonRequest(string url)
    {
        var a = await _client.GetAsync(url);
        string b = await a.Content.ReadAsStringAsync();
        dynamic result = Newtonsoft.Json.Linq.JObject.Parse(b);
        return result;
    }

}