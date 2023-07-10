//отвечает за кеширование всего и вся(запросов к бд, картинок с диска, и так далее)
//вы должны использовать этот класс только для получения обьектов, если же вы хотите добавить пользовтеля
//то опратитесь напрямую к SunriseContext через публичное свойство
using Microsoft.Extensions.Caching.Memory;
using Sunrise.Types;
using FileInfo = Sunrise.Storage.Types.FileInfo;
using Microsoft.EntityFrameworkCore;
namespace Sunrise;

public class CacheService
{
    readonly MemoryCacheEntryOptions userCacheOptions = new MemoryCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
    readonly MemoryCacheEntryOptions sessionCacheOptions = new MemoryCacheEntryOptions{
        Priority = CacheItemPriority.Low
    }
        .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
        .SetSlidingExpiration(TimeSpan.FromSeconds(30));
        

    public SunriseContext dbcontext {get;private set;}
    IMemoryCache l1cache;
    IConfiguration config;
    bool disableCache = false;
    public CacheService(SunriseContext sc, IMemoryCache memc, IConfiguration appConfig)
    {
        config=appConfig;
        disableCache = ("true" == config["disableCache"]);
        dbcontext = sc;
        l1cache = memc;
    }
    //получение пользователя через кеш
    public async Task<User?> GetUserAsync(Guid id)
    {

        bool r = l1cache.TryGetValue<User>(id, out User? u);

        if(r==false){
            u = await dbcontext.Users.FindAsync(id);
            if(u!=null && disableCache){
                l1cache.Set<User>(id, u, userCacheOptions);
            }
        }
        return u;
    }
    public async Task<User?> GetUserAsync(string username)
    {
        bool r = l1cache.TryGetValue<User>(username, out User? u);

        if(r==false){
            u = dbcontext.Users.Where(x => x.Name==username).FirstOrDefault();
            if(u!=null && disableCache){
                l1cache.Set<User>(username, u, userCacheOptions);
            }
        }
        return u;
    }
    //получение сессии через кеш
    public async Task<Session?> GetSessionAsync(Guid sessionId){
        //получение через ид сессии
        bool r = l1cache.TryGetValue<Session>(sessionId, out Session? u);

        if(r==false){
            u = await dbcontext.Sessions.FindAsync(sessionId);
            if(u!=null && disableCache){
                l1cache.Set<Session>(sessionId, u, sessionCacheOptions);
            }
        }
        return u;
    }
    public async Task<Session?> GetSessionAsync(string session){
        //получение через саму сессию
        bool r = l1cache.TryGetValue<Session>(session, out Session? u);

        if(r==false){
            u = dbcontext.Sessions.Where(x => x.SessionId == session).FirstOrDefault();
            if(u!=null && disableCache){
                l1cache.Set<Session>(session, u, sessionCacheOptions);
            }
        }
        return u;
    }
    public async Task UpdateSessionActivityInCache(Session s, Action dbUpdate){
        //обновляет сессию в самом обьекте сессии, затем в бд и а конце в кеше
        s.UpdateActivity(dbUpdate);
        l1cache.Set<Session>(s.SessionId, s);
    }
    //получение файла через кеш
    public async Task<FileInfo?> GetFileAsync(Guid fileId){
        bool r = l1cache.TryGetValue<FileInfo>(fileId, out FileInfo? u);

        if(r==false){
            u = await dbcontext.Files.FindAsync(fileId);
            if(u!=null && disableCache){
                l1cache.Set<FileInfo>(fileId, u, userCacheOptions);
            }
        }
        return u;
    }
    //получение поста через кеш
    public async Task<Post?> GetPostAsync(Guid postId){
        bool r = l1cache.TryGetValue<Post>(postId, out Post? u);

        if(r==false){
            u = await dbcontext.Posts.Include(x => x.Tags).Where(y => y.Id == postId).FirstOrDefaultAsync();
            if(u!=null && disableCache){
                l1cache.Set<Post>(postId, u, userCacheOptions);
            }
        }
        return u;
    }

    //получение тегов через кеш
    public async Task<Tag> GetTagAsync(int id){
        bool r = l1cache.TryGetValue<Tag>(id, out Tag? u);

        if(r==false){
            u = await dbcontext.Tags.FindAsync(id);
            if(u!=null && disableCache){
                l1cache.Set<Tag>(id, u, userCacheOptions);
            }
        }
        return u;
    }

    public async Task<Tag?> GetTagAsync(string SearchText){
        bool r = l1cache.TryGetValue<Tag>(SearchText, out Tag? u);

        if(r==false){
            u = dbcontext.Tags.Where(x=>x.SearchText == SearchText).FirstOrDefault();
            if(u!=null && disableCache){
                l1cache.Set<Tag>(SearchText, u, userCacheOptions);
            }
        }
        return u;
    }

}