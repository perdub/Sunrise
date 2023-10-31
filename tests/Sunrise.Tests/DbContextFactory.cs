using Test.Helpers;
using TestSupport.EfHelpers;

namespace Sunrise.Tests;

public static class DbFactory
{
    public static SunriseContext GetDbContext(this object p)
    {
        var options = p.CreatePostgreSqlUniqueClassOptions<SunriseContext>();
        var db =  new SunriseContext(options);

        db.Database.EnsureClean();

        return db;
    }

    public static SunriseContext AddTestData(this SunriseContext sc)
    {
        var user = new Sunrise.Types.User("testUser", "12345678", "mail@mail.io");

        sc.Users.Add(user);

        var fileInfo = new Sunrise.Types.FileInfo();

        fileInfo.Sha1 = Sunrise.Utilities.HashCreator.GetSha1(new byte[]{0x01, 0x02, 0xff});
        fileInfo.Paths = new string[]{
            "storage/aa/bb/cc.jpg",
            "storage/dd/ee/ff.jpg",
            "storage/00/55/f3.png"
        };

        var post = new Sunrise.Types.Post(user.Id, fileInfo);

        sc.Files.Add(fileInfo);
        sc.Posts.Add(post);

        sc.SaveChanges();

        sc.ChangeTracker.Clear();

        return sc;
    }
}