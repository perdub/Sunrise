namespace Sunrise.Utilities;
//класс с параметрами для создания jwt токена
public class JwtOptions
{
    public const string ISSUER = "Sunrise";
    public const string AUDIENCE = "sunrise";

    const string key = "1qпидw23er4__()ўіцы'\\jvugufk9eiefjпидорir~!22++--";
    public static byte[] GetBinaryKey()
    {
        return System.Text.Encoding.UTF8.GetBytes(key);
    }
}