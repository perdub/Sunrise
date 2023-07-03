namespace Sunrise.Utilities;

public static class HashCreator
{
    public static string GetSha512(this string s)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(s);
        using (var hash = System.Security.Cryptography.SHA512.Create())
        {
            var hashedInputBytes = hash.ComputeHash(bytes);
            var hashedInputStringBuilder = new System.Text.StringBuilder(128);
            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }
    }
}
