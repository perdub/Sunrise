//todo: переписать это ибо тут дохуя дублирующегося кода

namespace Sunrise.Storage;

using Sunrise.Utilities;

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
        _folderName = folderName;
        var info = Directory.CreateDirectory(folderName);
        globalStoragePath = Path.GetFullPath(folderName);
    }

    string globalStoragePath, _folderName;

    string buildpath(string hash)
    {
        //hash[x].ToString() нужен из-за того что без этого язык пытается сложить два char и получает int
        var path = Path.Combine(
            globalStoragePath,
            hash[0].ToString()+hash[1].ToString(),
            hash[2].ToString()+hash[3].ToString(),
            hash[4].ToString()+hash[5].ToString()
        );
        Directory.CreateDirectory(path);
        return path;
    }

    public string GenerateFileName(string extension)
    {
        //генерация случайного хеша
        byte[] randHash = new byte[32];
        Random.Shared.NextBytes(randHash);
        var hash = randHash.GetSha1();

        return string.Concat(
            Path.Combine(
                buildpath(hash),
                hash.Substring(6)
            ),
            '.',
            DateTime.UtcNow.Ticks,
            '.',
            extension
        );
    }

    public async Task<Types.FileInfo> SaveItem(
        Sunrise.Convert.AbstractConvert converter,
        Guid id,
        byte[] arr,
        string fileExtension
    ){
        var fileHash = arr.GetSha1();
        var path = buildpath(fileHash);

        Types.FileInfo file = new Types.FileInfo();
        
        file.Id = id;
        file.ContentType = converter.ContentType;
        file.Sha1 = fileHash;

        var originalFilePath = Path.Combine(
            path,
            fileHash.Substring(6)+".o"+fileExtension
        );

        await File.WriteAllBytesAsync(originalFilePath, arr);

        var res = await converter.Convert(originalFilePath, GenerateFileName);

        for(int i = 0;i<res.Length;i++)
        {
            res[i] = res[i].Substring(res[i].IndexOf(_folderName));
        }

        file.Paths = res;

        return file;
    }   
    /*
    public async Task<Types.FileInfo> SaveImage(Guid id, byte[] f, string fileExtension)
    {
        string path = buildpath(id);
        Types.FileInfo info = new Types.FileInfo();
        info.ContentType = Sunrise.Types.ContentType.Image;
        info.Id = id;
        Directory.CreateDirectory(path);
        string imgpath = path + "original" + fileExtension;
        await File.WriteAllBytesAsync(imgpath, f);
        Sunrise.Convert.AbstractConvert c = new Sunrise.Convert.ImageConverter();
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
        Sunrise.Convert.AbstractConvert c = new Sunrise.Convert.VideoConverter();
        await c.Convert(itempath);
        info.Paths = new string[]{
            Path.Combine("storage", id.ToString(), "preview.png").Replace("\\","/"),
            Path.Combine("storage", id.ToString(), "base.mp4").Replace("\\","/"),
            Path.Combine("storage", id.ToString(), "original"+fileExtension).Replace("\\","/")
        };
        return info;
    }//*/
}
