using SixLabors.ImageSharp;

namespace Sunrise.Utilities.Convert;

public class ImageConverter : AbstractConvert
{
    const int PREVIEWSIZE = 200;
    public override async Task Convert(string globalPath)
    {
        var img = await Image.LoadAsync(globalPath);
        
        Size basesize = newSize(img.Size);
        img.Mutate((x)=>{
            x.Resize(basesize);
        });
        await img.SaveAsJpegAsync(Path.Combine(Path.GetDirectoryName(globalPath), "base.jpg"));

        img.Mutate((x)=>{
            x.Resize(getPreviewSize(img.Size));
        });

        await img.SaveAsJpegAsync(Path.Combine(Path.GetDirectoryName(globalPath), "preview.jpg"));
        
        img.Dispose();
    }

    //проверка на размер изображения, если ислишко большой для базового - урезаем
    Size newSize(Size s){
        if(s.Width>4096){
            s.Height = (4096*s.Height)/s.Width;
            s.Width = 4096;
        }
        if(s.Height>4096){
            s.Width = (4096*s.Width)/s.Height;
            s.Height = 4096;
        }
        return s;
    }
    Size getPreviewSize(Size s){
        if(s.Height>s.Width){
            return new Size(0,PREVIEWSIZE);
        }
        return new Size(PREVIEWSIZE,0);
    }
}