namespace Sunrise.Tests;
[AllureOwner("perdub")]
[AllureTag("TagProcess")]
public class TagProcess
{
    [AllureXunit]
    public void TagsProcessTest1girl()
    {
        string tagInput = "1girl";
        string tagExpected = "1girl";

        string result = Sunrise.Utilities.TagProcess.Process(tagInput);

        Assert.Equal(tagExpected, result);
    }

    [AllureXunit]
    public void TagsProcessTest1girlTrim()
    {
        string tagInput = "  1girl   \n";
        string tagExpected = "1girl";

        string result = Sunrise.Utilities.TagProcess.Process(tagInput);

        Assert.Equal(tagExpected, result);
    }

    [AllureXunit]
    public void TagsProcessTest1girlRemoveSymbols()
    {
        string tagInput = "1girl!?_-a";
        string tagExpected = "1girl__a";

        string result = Sunrise.Utilities.TagProcess.Process(tagInput);

        Assert.Equal(tagExpected, result);
    }
}