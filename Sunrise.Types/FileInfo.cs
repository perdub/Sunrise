namespace Sunrise.Types;

public class FileInfo
{
    public Guid Id { get; set; }
    public Sunrise.Types.ContentType ContentType { get; set; }
    public string[] Paths { get; set; }

    public string Sha1 {get;set;}
}