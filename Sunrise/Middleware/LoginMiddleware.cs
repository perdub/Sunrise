//часть конвеера которая будет проверять залогинен ли пользователь и его jwt токен

namespace Sunrise.Middleware;

public class LoginMiddleware
{
    RequestDelegate next;
    public LoginMiddleware(RequestDelegate rd)
    {
        next = rd;
    }

    public async Task InvokeAsync(HttpContext context, CacheService cs, ILogger<LoginMiddleware> _logger)
    {
        string? token;
        if (context.Request.Cookies.TryGetValue("suntoken", out token))
        {
            if (string.IsNullOrEmpty(token))
            {
                await next.Invoke(context);
                return;
            }
            //todo: открыть токен и добавить пользователя
            var s = await cs.GetSessionAsync(token);
            if (s == null)
            {
                _logger.Log(LogLevel.Trace, "Fall to find session.", token);
                context.Items.Add("isUser", false);
                await next.Invoke(context);
                return;
            }
            if (!s.IsActive())
            {
                context.Items.Add("tokenExpired", true);
                cs.dbcontext.Sessions.Remove(s);
            }
            else
            {
                //метод обновления сессии
                await cs.UpdateSessionActivityInCache(s, ()=>{
                    cs.dbcontext.Sessions.Update(s);
                });
                context.Items.Add("isUser", true);
                context.Items.Add("userId", s.UserId);
            }
            await cs.dbcontext.SaveChangesAsync();
        }
        else
        {
            context.Items.Add("isUser", false);

        }


        await next.Invoke(context);
    }
}