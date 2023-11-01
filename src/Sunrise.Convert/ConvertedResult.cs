namespace Sunrise.Convert;

public struct ConvertedResult{
    public string previewPath;
    public bool sampleExsist;
    public string samplePath;
    public string originalPath;

    public ConvertedResult DeletePrefix(string globalPrefix){
        ConvertedResult r;
        r.sampleExsist = sampleExsist;
        r.previewPath = previewPath.Substring(globalPrefix.Length);
        r.samplePath = samplePath.Substring(globalPrefix.Length);
        r.originalPath = originalPath.Substring(globalPrefix.Length);
        return r;
    }
}