//отвечает за кеширование всего и вся(запросов к бд, картинок с диска, и так далее)
//вы должны использовать этот класс только для получения обьектов, если же вы хотите добавить пользовтеля
//то опратитесь напрямую к SunriseContext через публичное свойство
using Microsoft.Extensions.Caching.Memory;
using Sunrise.Types;

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

    public CacheService(SunriseContext sc, IMemoryCache memc)
    {
        dbcontext = sc;
        l1cache = memc;
    }

    public async Task<User?> GetUserAsync(Guid id)
    {
        bool r = l1cache.TryGetValue<User>(id, out User? u);

        if(r==false){
            u = await dbcontext.Users.FindAsync(id);
            if(u!=null){
                l1cache.Set<User>(id, u, userCacheOptions);
            }
        }
        return u;
    }

    public async Task<Session?> GetSessionAsync(Guid sessionId){
        bool r = l1cache.TryGetValue<Session>(sessionId, out Session? u);

        if(r==false){
            u = await dbcontext.Sessions.FindAsync(sessionId);
            if(u!=null){
                l1cache.Set<Session>(sessionId, u, userCacheOptions);
            }
        }
        return u;
    }

}