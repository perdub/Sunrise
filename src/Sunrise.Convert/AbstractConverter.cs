using System.Text;

namespace Sunrise.Convert;

public abstract class AbstractConverter : IDisposable{
    internal string globalPathPrefix;
    public AbstractConverter(string _globalPathPrefix)
    {
        this.globalPathPrefix = _globalPathPrefix;
    }

    //надо ли создавать вариант семпла(вариант который является сжатым оригинальным файлом)
    abstract internal bool NeedToCreateSample();

    //методы для сохранения разных варинтов ассета
    abstract internal string BuildPreview();
    abstract internal string BuildSample();
    abstract internal string BuildOriginal();

    //метод конвертации
    //todo: made all coonvertations async
    public virtual ConvertedResult Convert(){
        ConvertedResult result;

        result.originalPath = BuildOriginal();
        bool needToSample = NeedToCreateSample();

        result.sampleExsist = needToSample;
        if(needToSample){
            result.samplePath = BuildSample();
        }
        else{
            result.samplePath = "";
        }

        result.previewPath = BuildPreview();
        return result;
    }

    //метод для создания имён файлов
    public string GenerateFileName(string fileExtension, bool isOriginal = false){
        var iHash = SharpHash.Base.HashFactory.Crypto.CreateSHA2_256();
        var hash = iHash.ComputeString((DateTime.UtcNow.Ticks % DateTime.UtcNow.Microsecond).ToString(), System.Text.Encoding.UTF8);
        var sHash = hash.ToString();
        StringBuilder sb = new StringBuilder();

        string fDir = sHash[0].ToString()+sHash[1].ToString();
        string sDir = sHash[2].ToString()+sHash[3].ToString();
        string tDir = sHash[4].ToString()+sHash[5].ToString();

        string subDirs = Path.Combine(fDir,sDir,tDir);

        string gP = Path.Combine(globalPathPrefix, subDirs);

        Directory.CreateDirectory(gP);

        sb.Append(gP);
        sb.Append(Path.DirectorySeparatorChar);
        sb.Append(sHash);
        sb.Append('.');
        sb.Append(DateTime.UtcNow.Ticks);
        sb.Append('.');
        sb.Append(fileExtension);

        return sb.ToString();
    }

    public abstract void Dispose();
}