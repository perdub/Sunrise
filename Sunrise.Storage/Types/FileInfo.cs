namespace Sunrise.Storage.Types;

public class FileInfo
{
    public Guid Id { get; set; }
    public Sunrise.Types.ContentType ContentType { get; set; }
    //если storagetype=Local - используй path, если network - url
    public string[] Paths { get; set; }
}