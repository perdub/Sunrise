using FFMpegCore;
using FFMpegCore.Pipes;

namespace Sunrise.Convert;

public class VideoConverter : AbstractConverter
{
    private string _fileExtension;
    private Stream _input;
    private byte[] _video;
    private string fullOriginalPath;
    private Action<string> _log;
    public VideoConverter(string globalPrefixPath, byte[] video, string fileExtension, Action<string> log) : base(globalPrefixPath){
        _input = video.ToStream();
        _video = video;
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
        File.WriteAllBytes(fN, _video);
        return fN;
    }

    internal override string BuildPreview()
    {
        string fN = GenerateFileName("png");

        _input.Position = 0;

        FFMpegArguments
            .FromFileInput(fullOriginalPath)
            .OutputToFile(fN, false, (o)=>{
                o.WithCustomArgument("-frames:v 1");
            })
            .ProcessSynchronously();

        return fN;
    }

    internal override string BuildSample()
    {
        string fN = GenerateFileName("mp4");

        _input.Position = 0;

        Guid convertId = Guid.NewGuid();

        FFMpegArguments
            .FromFileInput(fullOriginalPath)
            .OutputToFile(fN, false, (o)=>{
                o.WithVideoCodec(FFMpegCore.Enums.VideoCodec.LibX264);
                o.WithoutMetadata();
                o.WithFastStart();
                o.WithSpeedPreset(FFMpegCore.Enums.Speed.SuperFast);
                o.WithCustomArgument($"-metadata comment=\"Sunrise\"");
                o.WithCustomArgument($"-crf 28 -profile:v baseline -level 1");
            })
            .NotifyOnProgress((p)=>{
                _log($"{convertId}:{p}% are ready!");
            })
            .ProcessSynchronously();

        return fN;
    }

    internal override bool NeedToCreateSample()
    {
        var meta = FFProbe.Analyse(_input);
        _input.Position = 0;
        //если длинна больше 5 минут или вес больше 100 мб мы пропускаем потому что у меня нет столько вычислительных ресурсов((
        if(meta.Duration > TimeSpan.FromMinutes(5) || _video.Length > 100*1024*1024)
            return false;
        return true;
    }
}