using FFmpeg.NET;

namespace Sunrise.Convert;

public class VideoConverter : AbstractConverter
{
    private string _fileExtension;
    private StreamInput _input;
    private byte[] _video;
    private Engine _ffmpegEngine;
    public VideoConverter(string globalPrefixPath, byte[] video, string fileExtension) : base(globalPrefixPath){
        _input = new StreamInput(video.ToStream());
        _video = video;
        _fileExtension = fileExtension;
        _ffmpegEngine = new Engine();
    }

    public override void Dispose()
    {
        throw new NotImplementedException();
    }

    internal override string BuildOriginal()
    {
        string fN = GenerateFileName(_fileExtension, true);
        File.WriteAllBytes(fN, _video);
        return fN;
    }

    internal override string BuildPreview()
    {
        string fN = GenerateFileName("png");
        var convertTask = _ffmpegEngine.GetThumbnailAsync(_input, new OutputFile(fN), CancellationToken.None);
        convertTask.Wait();
        return fN;
    }

    internal override string BuildSample()
    {
        string fN = GenerateFileName("mp4");

        /*var opt = new ConversionOptions{
            VideoCodec = FFmpeg.NET.Enums.VideoCodec.h264_amf,
            VideoFormat = FFmpeg.NET.Enums.VideoFormat.mp4,
            VideoSize = FFmpeg.NET.Enums.VideoSize.Hd720
        };*/

        var task = _ffmpegEngine.ConvertAsync(_input, new OutputFile(fN), CancellationToken.None);
        task.Wait();
        return fN;
    }

    internal override bool NeedToCreateSample()
    {
        var meta = _input.MetaData;
        //if(meta.Duration > TimeSpan.FromMinutes(5) || (meta.VideoData.BitRateKbs != null && meta.VideoData.BitRateKbs > 1536))
            return true;
        return false;
    }
}