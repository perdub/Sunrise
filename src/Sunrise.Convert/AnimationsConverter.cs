using FFMpegCore;
using FFMpegCore.Pipes;

namespace Sunrise.Convert;

public class AnimationsConverter : AbstractConverter
{
    private string _fileExtension;
    private Stream _input;
    private byte[] _gif;
    private string fullOriginalPath;
    private Action<string> _log;
    public AnimationsConverter(string globalPrefixPath, byte[] gif, string fileExtension, Action<string> log) : base(globalPrefixPath){
        //_input = gif.ToStream();
        _gif = gif;
        _fileExtension = fileExtension;
        _log = log;
    }

    public override void Dispose()
    {
        throw new NotImplementedException();
    }

    internal override string BuildOriginal()
    {
        string fN = GenerateFileName(_fileExtension, true);
        fullOriginalPath = fN;
        File.WriteAllBytes(fN, _gif);
        return fN;
    }

    internal override string BuildPreview()
    {
        return samplePath ?? build();
    }
    private string? samplePath = null;
    internal override string BuildSample()
    {
        return samplePath ?? build();
    }

    private string build(){
        string fN = GenerateFileName("gif");

        FFMpegArguments
            .FromFileInput(fullOriginalPath)
            .OutputToFile(fN, false, (o)=>{
                o.WithCustomArgument("-filter_complex \"[0:v]split[a][b];[a]palettegen[p];[b][p]paletteuse\"");
            })
            .ProcessSynchronously();

        samplePath = fN;

        return fN;

    }

    internal override bool NeedToCreateSample()
    {
        if(_gif.Length < 512*1024)
            return false;
        return true;
    }
}