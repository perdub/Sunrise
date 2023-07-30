namespace Sunrise.Utilities.Convert;

public abstract class AbstractConvert
{
    public abstract Task Convert(string globalPath);
    //получение пути для нового файла
    protected string getNewFileDirection(string globalPath, string newFileName){
        return Path.Combine(Path.GetDirectoryName(globalPath), newFileName);
    }
}