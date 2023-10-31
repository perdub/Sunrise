namespace Sunrise.Convert;

public abstract class AbstractConvert
{
    
    public abstract Task<string[]> Convert(string globalPath, Func<string, string> nameGenerator);
    public abstract Sunrise.Types.ContentType ContentType{get;}
    //получение пути для нового файла
    protected string getNewFileDirection(string globalPath, string newFileName){
        return Path.Combine(Path.GetDirectoryName(globalPath), newFileName);
    }

    protected string getMetadata(){
        return $"Sunrise, process in {DateTime.UtcNow.ToLongDateString()}";
    }

}