

using Test.Helpers;

namespace Sunrise.Tests;

public class UnitTest1
{
    [AllureXunit]
    public void DbTestConnection()
    {
        var a = this.GetDbContext();

        Assert.NotNull(a);
    }
}