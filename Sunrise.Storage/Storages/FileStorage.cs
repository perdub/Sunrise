using Sunrise.Storage.Types;

namespace Sunrise.Storage;

public class FileStorage : Storage
{
    string globalStoragePath = "";
    public FileStorage(string path) : base()
    {
        globalStoragePath = path;
        Directory.CreateDirectory(path);
    }

    public override async Task<MemoryStream> LoadFile(Guid id, string fileName)
    {
        MemoryStream result = new MemoryStream();
        var fs = File.Open(path(id)+completefilename(id,fileName), FileMode.Open);
        await fs.CopyToAsync(result);
        result.Position = 0;
        return result;
    }
    

    string completefilename(Guid id, string fileName)
    {
        return new DirectoryInfo(path(id)).GetFiles($"{fileName}.*")[0].Name;
    }

    string path(Guid id){
        string item= id.ToString();
        return globalStoragePath+'\\'+id.ToString()+'\\';
    }

    public override async Task<string> SaveAsync(Guid id, byte[] f, string fileName)
    {
        //код записи в файл и создания пути
        string item= id.ToString();
        item = globalStoragePath+'\\'+item+'\\';
        Directory.CreateDirectory(item);
        var w = File.Create(item+"original"+fileName);
        await w.WriteAsync(f, 0, f.Length);
        w.Close();

        return Path.GetFullPath(item);
    }

}