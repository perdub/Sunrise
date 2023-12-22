namespace Sunrise.Convert;

public struct ConvertedResult{
    public string previewPath;
    public bool sampleExsist;
    public string samplePath;
    public string originalPath;

    const string REQUEST_PATH = "/sunrise";

    public ConvertedResult DeletePrefix(string globalPrefix){
        ConvertedResult r;
        r.sampleExsist = sampleExsist;
        r.previewPath = previewPath.Substring(globalPrefix.Length);

        if(sampleExsist)
            r.samplePath = samplePath.Substring(globalPrefix.Length);
        else
            r.samplePath = samplePath;

        r.originalPath = originalPath.Substring(globalPrefix.Length);
        return r;
    }
}