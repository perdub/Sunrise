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
        r.previewPath = REQUEST_PATH + previewPath.Substring(globalPrefix.Length);
        r.samplePath = REQUEST_PATH + samplePath.Substring(globalPrefix.Length);
        r.originalPath = REQUEST_PATH + originalPath.Substring(globalPrefix.Length);
        return r;
    }
}