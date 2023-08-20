namespace Sunrise.Tests;

public class GetSha1Tests
{
    [Fact]
    public void Sha1HashBytes_0x12()
    {
        byte[] byteInput = new byte[]{0x12};
        string hashString = "c4f87a6290aee1acfc1f26083974ce94621fca64";

        string strHash = Sunrise.Utilities.HashCreator.GetSha1(byteInput);

        Assert.Equal(hashString, strHash);
    }

    [Fact]
    public void Sha1HashBytes_0x88996584ff()
    {
        byte[] byteInput = new byte[]{0x88, 0x99, 0x65, 0x84, 0xff};
        string hashString = "b17649158451d4afb94053ae7e4a159e64867f45";

        string strHash = Sunrise.Utilities.HashCreator.GetSha1(byteInput);

        Assert.Equal(hashString, strHash);
    }
}