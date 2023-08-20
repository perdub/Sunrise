namespace Sunrise.Tests;

public class CheckTypeTests
{
    [Fact]
    public void CheckJpg()
    {
        var arr = ftba.Files.Files._test_jpg;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Image, result);
    }
    [Fact]
    public void CheckPng()
    {
        var arr = ftba.Files.Files._test_png;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Image, result);
    }
    [Fact]
    public void CheckWebp()
    {
        var arr = ftba.Files.Files._test_webp;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Image, result);
    }
    [Fact]
    public void CheckGif()
    {
        var arr = ftba.Files.Files._test_gif;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Gif, result);
    }
    [Fact]
    public void CheckMp4()
    {
        var arr = ftba.Files.Files._testmp4_with_audio_mp4;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Video, result);
    }
    [Fact]
    public void CheckAvi()
    {
        var arr = ftba.Files.Files._testAudioVideoInterleave_avi;

        var result = Sunrise.Types.ContentTypeChecker.CheckType(arr);

        Assert.Equal(Sunrise.Types.ContentType.Video, result);
    }
}