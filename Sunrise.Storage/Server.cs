//todo: переписать это ибо тут дохуя дублирующегося кода

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

    string buildpath(Guid id)
    {
        return $"{globalStoragePath}//{id.ToString()}//";
    }

    public async Task<Types.FileInfo> SaveImage(Guid id, byte[] f, string fileExtension)
    {
        string path = buildpath(id);
        Types.FileInfo info = new Types.FileInfo();
        info.ContentType = Sunrise.Types.ContentType.Image;
        info.Id = id;
        Directory.CreateDirectory(path);
        string imgpath = path + "original" + fileExtension;
        await File.WriteAllBytesAsync(imgpath, f);
        Sunrise.Utilities.Convert.AbstractConvert c = new Sunrise.Utilities.Convert.ImageConverter();
        await c.Convert(imgpath);
        info.Paths = new string[]{
            Path.Combine("storage", id.ToString(), "preview.jpg").Replace("\\","/"),
            Path.Combine("storage", id.ToString(), "base.jpg").Replace("\\","/"),
            Path.Combine("storage", id.ToString(), "original"+fileExtension).Replace("\\","/")
        };
        return info;
    }

    public async Task<Types.FileInfo> SaveVideo(Guid id, byte[] raw, string fileExtension)
    {
        string path = buildpath(id);
        Types.FileInfo info = new Types.FileInfo();
        info.ContentType = Sunrise.Types.ContentType.Video;
        info.Id = id;
        Directory.CreateDirectory(path);
        string itempath = path + "original" + fileExtension;
        await File.WriteAllBytesAsync(itempath, raw);
        Sunrise.Utilities.Convert.AbstractConvert c = new Sunrise.Utilities.Convert.VideoConverter();
        await c.Convert(itempath);
        info.Paths = new string[]{
            Path.Combine("storage", id.ToString(), "preview.png").Replace("\\","/"),
            Path.Combine("storage", id.ToString(), "base.mp4").Replace("\\","/"),
            Path.Combine("storage", id.ToString(), "original"+fileExtension).Replace("\\","/")
        };
        return info;
    }
}
