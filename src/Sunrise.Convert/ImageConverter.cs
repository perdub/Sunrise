using SixLabors.ImageSharp;
using Sunrise.Types;

namespace Sunrise.Convert;

public class ImageConverter : AbstractConvert
{
    public override ContentType ContentType => ContentType.Image;

    public override async Task<string[]> Convert(string globalPath, Func<string, string> nameGenerator)
    {
        string[] fileNames = new string[]{
            nameGenerator("jpg"),
            nameGenerator("jpg"),
            globalPath
        };

        var img = await Image.LoadAsync(globalPath);


        var metadata = img.Metadata.IptcProfile;
        if(metadata == null){
            metadata = img.Metadata.IptcProfile = new SixLabors.ImageSharp.Metadata.Profiles.Iptc.IptcProfile();
        }

        metadata.SetValue(SixLabors.ImageSharp.Metadata.Profiles.Iptc.IptcTag.Source, getMetadata());
        
        Size basesize = newSize(img.Size);
        img.Mutate((x)=>{
            x.Resize(basesize);
        });
        await img.SaveAsJpegAsync(getNewFileDirection(globalPath, fileNames[1]));

        img.Mutate((x)=>{
            x.Resize(getPreviewSize(img.Size));
        });

        await img.SaveAsJpegAsync(getNewFileDirection(globalPath, fileNames[0]));
        
        img.Dispose();

        return fileNames;
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
            return new Size(0,Constants.PREVIEW_SIZE);
        }
        return new Size(Constants.PREVIEW_SIZE,0);
    }
}