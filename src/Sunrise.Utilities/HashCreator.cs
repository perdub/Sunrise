namespace Sunrise.Utilities;

using System.Security.Cryptography;
using System.Text;

public static class HashCreator
{
    public static string GetSha512(this string s)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(s);
        using (var hash = SHA512.Create())
        {
            var hashedInputBytes = hash.ComputeHash(bytes);
            var hashedInputStringBuilder = new System.Text.StringBuilder(128);
            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }
    }
    static SHA1 sha1 = SHA1.Create();
    public static string GetSha1(this byte[] file)
    {
        var hash = sha1.ComputeHash(file);
        StringBuilder sb = new StringBuilder(20);
        foreach(var a in hash)
            sb.Append(a.ToString("x2"));
        return sb.ToString();
    }
}
