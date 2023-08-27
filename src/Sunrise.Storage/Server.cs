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
    public ContentServer(string folderName, string prefix) : this()
    {
        _folderName = folderName;
        _prefix=prefix;
        var info = Directory.CreateDirectory(folderName);
        globalStoragePath = Path.GetFullPath(folderName);
    }

    string globalStoragePath, _folderName, _prefix;

    string buildpath(string hash)
    {
        //hash[x].ToString() нужен из-за того что без этого язык пытается сложить два char и получает int
        var path = Path.Combine(
            globalStoragePath,
            //создание подпапки
            hash[0].ToString()+hash[1].ToString(),
            hash[2].ToString()+hash[3].ToString()
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
                hash.Substring(Constants.FILE_NAME_SYMBOL_SLICE_COUNT)
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
        //подсчёт хеша
        var fileHash = arr.GetSha1();
        var path = buildpath(fileHash);
        //создание обьекта файла и запись типа хеша и айди
        Types.FileInfo file = new Types.FileInfo();
        
        file.Id = id;
        file.ContentType = converter.ContentType;
        file.Sha1 = fileHash;
        //создание имени оригинального файла
        var originalFilePath = Path.Combine(
            path,
            fileHash.Substring(Constants.FILE_NAME_SYMBOL_SLICE_COUNT)+".o"+fileExtension
        );
        //запись изходного файла
        await File.WriteAllBytesAsync(originalFilePath, arr);
        //конвертация
        var res = await converter.Convert(originalFilePath, GenerateFileName);
        //обрезка(?) путей
        for(int i = 0;i<res.Length;i++)
        {
            res[i] = res[i]
                .Substring(res[i].IndexOf(_folderName))
                .Replace(_folderName, _prefix);
        }
        //присвоение путей обьекту
        file.Paths = res;

        return file;
    }
}
