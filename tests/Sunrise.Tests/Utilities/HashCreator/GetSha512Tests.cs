namespace Sunrise.Tests;
[AllureOwner("perdub")]
[AllureTag("Sha512Test")]
public class GetSha512Tests
{
    [AllureXunit]
    public void Sha512HashString_1girl()
    {
        string strInput = "1girl";
        string hashString = "CA6DEBBCDF7EF1C5D449267A70829CB48F6B3CA838B093917C024F7DA06A943E3C7AF3404FBAF0C6A031E214C57207DE676D45CBF3E7336954EAB2AB0D1B73CD";

        string strHash = Sunrise.Utilities.HashCreator.GetSha512(strInput);

        Assert.Equal(hashString, strHash);
    }

    [AllureXunit]
    public void Sha512HashString_12345678()
    {
        string strInput = "12345678";
        string hashString = "FA585D89C851DD338A70DCF535AA2A92FEE7836DD6AFF1226583E88E0996293F16BC009C652826E0FC5C706695A03CDDCE372F139EFF4D13959DA6F1F5D3EABE";

        string strHash = Sunrise.Utilities.HashCreator.GetSha512(strInput);

        Assert.Equal(hashString, strHash);
    }
}