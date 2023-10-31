using Xunit.Abstractions;

namespace Sunrise.Tests;
[AllureOwner("perdub")]
[AllureTag("FileConvert")]
public class FileConvert
{
    private readonly ITestOutputHelper output;

    public FileConvert(ITestOutputHelper output)
    {
        this.output = output;
    }

    [AllureXunit(DisplayName = "Test to convert image")]
    public void ConvertImage()
    {
        //act
        var image = ResourceHelper.GetResource("Sunrise.Tests.Convert.jpgconverttest.jpg");
        var converter = new Sunrise.Convert.ImageConverter();
        var originalPath = getFileName("jpg");
        File.WriteAllBytes(originalPath, image);

        //arrange
        var result = converter.Convert(originalPath, getFileName).Result;

        //accert
        //if realy i dont know what we need check in this test so we look on count of output path
        //todo: change check method!
        Assert.Equal(3, result.Length);

        Allure.XUnit.AllureAttachments.File("original", originalPath).Wait();
        Allure.XUnit.AllureAttachments.File("preview", result[0]).Wait();
        Allure.XUnit.AllureAttachments.File("base", result[1]).Wait();

        output.WriteLine("Converted images from Sunrise.Tests.FileConvert.ConvertImage()");
        foreach(var a in result){
            output.WriteLine(a);
        }
    }
    string getFileName(string ex){
            return Path.ChangeExtension(Path.GetTempFileName(), ex);
        }


[AllureXunit(DisplayName = "Test to convert video")]
        public void ConvertVideo()
    {
        //act
        var image = ResourceHelper.GetResource("Sunrise.Tests.Convert.mp4converttest.mp4");
        var converter = new Sunrise.Convert.VideoConverter();
        var originalPath = getFileName("mp4");
        File.WriteAllBytes(originalPath, image);

        //arrange
        var result = converter.Convert(originalPath, getFileName).Result;

        //accert
        //if realy i dont know what we need check in this test so we look on count of output path
        //todo: change check method!
        Assert.Equal(3, result.Length);

        //on this point test are passed and we only upload his results lol

        Allure.XUnit.AllureAttachments.File("original", originalPath).Wait();
        Allure.XUnit.AllureAttachments.File("preview", result[0]).Wait();
        Allure.XUnit.AllureAttachments.File("base", result[1]).Wait();

        output.WriteLine("Converted video from Sunrise.Tests.FileConvert.ConvertVideo()");
        foreach(var a in result){
            output.WriteLine(a);
        }
    }
}