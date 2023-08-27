namespace Sunrise.Tests;
//todo:rewrite this from ftba to resources
[AllureOwner("perdub")]
[AllureTag("CheckTypes")]
public class CheckTypeTests
{
    [AllureXunit(DisplayName = "Test to check example jpg file" )]
    public void CheckJpg()
    {
        var arr = ftba.Files.Files._test_jpg;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Image, result);
    }
    [AllureXunit(DisplayName = "Test to check example png file" )]
    public void CheckPng()
    {
        var arr = ftba.Files.Files._test_png;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Image, result);
    }
    [AllureXunit(DisplayName = "Test to check example webp file" )]
    public void CheckWebp()
    {
        var arr = ftba.Files.Files._test_webp;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Image, result);
    }
    [AllureXunit(DisplayName = "Test to check example gif file" )]
    public void CheckGif()
    {
        var arr = ftba.Files.Files._test_gif;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Gif, result);
    }
    [AllureXunit(DisplayName = "Test to check example mp4 file" )]
    public void CheckMp4()
    {
        var arr = ftba.Files.Files._testmp4_with_audio_mp4;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Video, result);
    }
    [AllureXunit(DisplayName = "Test to check example avi file" )]
    public void CheckAvi()
    {
        var arr = ftba.Files.Files._testAudioVideoInterleave_avi;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Video, result);
    }
}