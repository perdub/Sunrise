using FFMpegCore;
using Sunrise.Types;

namespace Sunrise.Convert;

public class VideoConverter : AbstractConvert
{
    public override ContentType ContentType => ContentType.Video;

    public override async Task<string[]> Convert(string globalPath, Func<string, string> nameGenerator)
    {
        string[] fileNames = new string[]{
            nameGenerator("png"),//ffmpegcore изза какихто причин принудительно меняет тип выходного файла снапшота на png
            nameGenerator("mp4"),
            globalPath
        };
        //анализ видеофайла
        var media = FFProbe.Analyse(globalPath);
        
        //создание обьекта описывающий размер скриншота для превью
        System.Drawing.Size s = new System.Drawing.Size(Constants.PREVIEW_SIZE, Constants.PREVIEW_SIZE);

        int vid_h = 0;
        int vid_w = 0;


        //попытка симетрично заполнить размер относительно первого потока
        var a = media.VideoStreams.FirstOrDefault();
        if(a!=null){
            if(a.Width>a.Height){
                s.Width = Constants.PREVIEW_SIZE;
                vid_w = Constants.VIDEO_CONVERTED_SIZE;

                s.Height = (a.Height*Constants.PREVIEW_SIZE)/a.Width;
                vid_h=(a.Height*Constants.VIDEO_CONVERTED_SIZE)/a.Width;
            }
            else{
                s.Height = Constants.PREVIEW_SIZE;
                vid_h = Constants.VIDEO_CONVERTED_SIZE;

                s.Width = (a.Width*Constants.PREVIEW_SIZE)/a.Height;
                vid_w=(a.Width*Constants.VIDEO_CONVERTED_SIZE)/a.Height;
            }
        }

        Guid convertId = Guid.NewGuid();
        Console.WriteLine($"New convert task with id {convertId} started.");

        //сжатие в base.mp4
        await FFMpegArguments
            .FromFileInput(globalPath)
            .OutputToFile(
                fileNames[1],
                true,
                (options)=>{
                    options.UsingThreads(Constants.FFMPEG_THREADS_COUNT);
                    options.WithVideoCodec(FFMpegCore.Enums.VideoCodec.LibX264);
                    options.WithFastStart();
                    options.WithSpeedPreset(FFMpegCore.Enums.Speed.VeryFast);
                    options.WithCustomArgument($"-metadata comment=\"{getMetadata()}\"");
                    options.WithCustomArgument($"-crf 28 -profile:v baseline -level 1 -vf \"scale={vid_w}:{vid_h}\"");
                }
            )
            .NotifyOnProgress((percentage)=>{
                Console.WriteLine($"Convert task {convertId} complete on {percentage}%");
            }, media.Duration)
            .ProcessAsynchronously();

        //сохранение превью
        FFMpeg.Snapshot(globalPath, fileNames[0], s);

        return fileNames;
    }
}