namespace Sunrise.Storage;

public class ContentServer
{
    #region Singelton
    static ContentServer _singelton;
    public static ContentServer Singelton { get { return _singelton; } }

    public ContentServer()
    {
        _singelton = this;
    }
    #endregion
    public ContentServer(string folderName) : this()
    {
        var info = Directory.CreateDirectory(folderName);
        globalStoragePath = Path.GetFullPath(folderName);
    }

    string globalStoragePath;
    List<Item> items = new List<Sunrise.Storage.Item>();

    string buildpath(Guid id)
    {
        return $"{globalStoragePath}//{id.ToString()}//";
    }

    public async Task<Types.FileInfo> SaveImage(Guid id, byte[] f, Sunrise.Types.ContentType type, string fileExtension)
    {
        string path = buildpath(id);
        Types.FileInfo info = new Types.FileInfo();
        info.ContentType = type;
        Directory.CreateDirectory(path);
        string imgpath = path + "original" + fileExtension;
        await File.WriteAllBytesAsync(imgpath, f);
        Sunrise.Utilities.Convert.AbstractConvert c = new Sunrise.Utilities.Convert.ImageConverter();
        await c.Convert(imgpath);
        info.Paths = new string[]{
            Path.Combine(path, "base.jpg"),
            Path.Combine(path, "preview.jpg"),
            imgpath
        };
        return info;
    }

    public IEnumerator<Items.Item> GetItems(Guid id)
    {
        yield return new Items.ImageItem();
    }
}
