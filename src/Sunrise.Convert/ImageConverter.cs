using SixLabors.ImageSharp;

namespace Sunrise.Convert;

public class ImageConverter : AbstractConverter
{
    private Image _image;
    private int _imageSize;
    private string _fileExtension;

    public ImageConverter(string globalPathPrefix, byte[] image, string fileExtension) : base(globalPathPrefix)
    {
        _image = Image.Load(image);
        _imageSize = image.Length;
        _fileExtension = fileExtension;
    }

    internal override string BuildOriginal()
    {
        string fN = GenerateFileName(_fileExtension, true);
        _image.Save(fN);
        return fN;
    }

    internal override string BuildPreview()
    {
        string fN = GenerateFileName("jpg");
        _image.Mutate(o=>o.Resize(getPreviewSize(_image.Size)));
        _image.SaveAsJpeg(fN);
        return fN;
    }

    internal override string BuildSample()
    {
        string fN = GenerateFileName("jpg");
        _image.Mutate(o=>o.Resize(newSize(_image.Size)));
        _image.SaveAsJpeg(fN, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder{Quality = 70});
        return fN;
    }

    internal override bool NeedToCreateSample()
    {
        if(_image.Height>2048 || _image.Width>2048 || _imageSize > 1024*1024){
            return true;
        }
        return false;
    }

    private Size newSize(Size s){
        if(s.Width>2048){
            s.Height = (2048*s.Height)/s.Width;
            s.Width = 2048;
        }
        if(s.Height>2048){
            s.Width = (2048*s.Width)/s.Height;
            s.Height = 2048;
        }
        return s;
    }
    private Size getPreviewSize(Size s){
        if(s.Height>s.Width){
            return new Size(0, 300);
        }
        return new Size(300, 0);
    }

    public override void Dispose()
    {
        _image.Dispose();
    }
}