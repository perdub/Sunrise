namespace Sunrise;
using Microsoft.EntityFrameworkCore;
using Sunrise.Types;
public class SunriseContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Sunrise.Storage.Types.FileInfo> Files => Set<Sunrise.Storage.Types.FileInfo>();
    public SunriseContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Sunrise.Logger.Logger.Singelton.Write("Init connection to database...");
        optionsBuilder.UseSqlite("Data Source=app.db");
        Sunrise.Logger.Logger.Singelton.Write("Connected!");
    }



    ///<summary>
    ///Используйте `GetUserApiView` для отправки обьекта клиенту, `User` содержит кондифициальную информацию
    ///</summary>
    public User GetUser(string name, bool allowNull = false)
    {
        var q = Users.Where(x => x.Name == name).FirstOrDefault();
        if (q == null && allowNull == false)
        {
            throw new Sunrise.Types.Exceptions.NotFoundObjectException("User not found.");
        }
        return q;
    }

    ///<summary>
    ///Используйте `GetUserApiView` для отправки обьекта клиенту, `User` содержит кондифициальную информацию
    ///</summary>
    public User GetUser(Guid id, bool allowNull = false)
    {
        var q = Users.Where(x => x.Id == id).FirstOrDefault();
        if (q == null && allowNull == false)
        {
            throw new Sunrise.Types.Exceptions.NotFoundObjectException("User not found.");
        }
        return q;
    }

    public UserApiView GetUserApiView(string name)
    {
        return (UserApiView)GetUser(name).GetApiView();
    }

    public UserApiView GetUserApiView(Guid id)
    {
        return (UserApiView)GetUser(id).GetApiView();
    }

    public bool CreateNewUser(UserRegistrationInfo uri)
    {
        if (GetUser(uri.name, true) != null)
        {
            return false;
        }
        Users.Add(new User(uri));
        SaveChanges();
        return true;
    }

    public Tag GetTagInfo(int id)
    {
        return Tags.Find(id) ?? throw new Sunrise.Types.Exceptions.NotFoundObjectException("Tag not found.");
    }
    public Tag GetTagInfo(string name)
    {
        return Tags.Where(x => x.SearchText == name).FirstOrDefault() ?? throw new Sunrise.Types.Exceptions.NotFoundObjectException("Tag not found.");
    }

    protected override void OnModelCreating(ModelBuilder bld){
        bld.Entity<Sunrise.Storage.Types.FileInfo>().Property(x => x.Paths)
            .HasConversion(
                a => System.Text.Json.JsonSerializer.Serialize(a, System.Text.Json.JsonSerializerOptions.Default),
                b => System.Text.Json.JsonSerializer.Deserialize<string[]>(b, System.Text.Json.JsonSerializerOptions.Default)
            );
    }
}