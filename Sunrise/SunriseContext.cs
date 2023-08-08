namespace Sunrise;
using Microsoft.EntityFrameworkCore;
using Sunrise.Types;
using Sunrise.Utilities;

using Log = ILogger<SunriseContext>;

public class SunriseContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Sunrise.Storage.Types.FileInfo> Files => Set<Sunrise.Storage.Types.FileInfo>();

    public DbSet<Verify> Verify => Set<Verify>();

    Log? _logger;

    public SunriseContext(Log? logger = null, bool ensureCreated = false)
    {
        _logger = logger;

        if (ensureCreated)
            Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = Program.Config.GetValue<string>("dbConnectionString");
        _logger?.LogDebug("Init connection to database...");
        optionsBuilder.UseSqlite(string.IsNullOrWhiteSpace(connectionString) ? "Data Source=app.db" : connectionString);
        _logger?.LogDebug("Connected!");
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
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

    public async Task Upload(Guid userId, byte[] raw, string filename, string tags)
    {
        ContentType type = raw.CheckType();
        Guid fileId = Guid.NewGuid();
        var st = Sunrise.Storage.ContentServer.Singelton;
        Storage.Types.FileInfo fi;
        _logger?.LogDebug($"File {fileId} has {raw.TryGrabHeader()} header.");
        switch (type)
        {
            case ContentType.Image:
                fi = await st.SaveImage(fileId, raw, filename);
                break;
            case ContentType.Video:
                fi = await st.SaveVideo(fileId, raw, filename);
                break;
            default:
                throw new Types.Exceptions.InvalidObjectTypeException($"Unknown type. Header: {raw.TryGrabHeader()}");
        }

        Post newPost = new Post(userId, fileId);
        var tagsArr = GetOrCreateTags(tags.Split(' '));

        foreach (var tag in tagsArr)
        {
            newPost.Tags.Add(tag);
            tag.Post.Add(newPost);
            tag.PostCount++;
        }

        Tags.UpdateRange(tagsArr);
        Posts.Add(newPost);
        Files.Add(fi);

        try
        {
            await SaveChangesAsync();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException due)
        {
            _logger?.LogError(due.ToString());
        }
    }

    Tag[] GetOrCreateTags(string[] tags)
    {
        Tag[] result = new Tag[tags.Length];
        for (int i = 0; i < tags.Length; i++)
        {
            string fr = tags[i].Process();
            var a = Tags.Where(a => a.SearchText == fr).FirstOrDefault();
            if (a == null)
            {
                a = new Tag(fr);
                Tags.Add(a);
            }
            result[i] = a;
        }
        SaveChanges();
        return result;
    }

    public async Task<bool> UpdatePostTag(Guid postId, string newTags)
    {
        //todo: rewrite this

        //вполне можно не удалять сначала все теги и потом добавлять новые, а проходить по всем тегам и удалять/добавлять их
        var post = Posts.Include(a=>a.Tags).Where(a=>a.Id==postId).FirstOrDefault();
        if(post == null){
            return false; 
        }

        int cv = post.Tags.Count();
        for(int i = 0;i<cv;i++){
            post.Tags[i].PostCount--;
            post.Tags[i].Post.Remove(post);
        }

        post.Tags = new List<Tag>();

        Tag[] arr = GetOrCreateTags(newTags.Split());
        for(int i = 0; i<arr.Length;i++){
            post.Tags.Add(arr[i]);
            arr[i].Post.Add(post);
            arr[i].PostCount++;
        }

        await SaveChangesAsync();
        
        return true;
    }

    public async void DeletePost(Guid postID)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder bld)
    {
        bld.Entity<Sunrise.Storage.Types.FileInfo>().Property(x => x.Paths)
            .HasConversion(
                a => System.Text.Json.JsonSerializer.Serialize(a, System.Text.Json.JsonSerializerOptions.Default),
                b => System.Text.Json.JsonSerializer.Deserialize<string[]>(b, System.Text.Json.JsonSerializerOptions.Default)
            );
    }
}