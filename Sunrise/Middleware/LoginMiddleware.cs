//часть конвеера которая будет проверять залогинен ли пользователь и его jwt токен

namespace Sunrise.Middleware;

public class LoginMiddleware
{
    RequestDelegate next;
    public LoginMiddleware(RequestDelegate rd)
    {
        next = rd;
    }

    public async Task InvokeAsync(HttpContext context, SunriseContext cs, ILogger<LoginMiddleware> _logger)
    {
        string? token;
        if (context.Request.Cookies.TryGetValue(Constants.SESSION_COOKIE_NAME, out token))
        {
            if (string.IsNullOrEmpty(token))
            {
                await next.Invoke(context);
                return;
            }
            //todo: открыть токен и добавить пользователя
            var s = cs.Sessions.Where(a => a.SessionId == token).FirstOrDefault();
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
                cs.Sessions.Remove(s);
            }
            else
            {
                //метод обновления сессии
                    cs.Sessions.Update(s);
                context.Items.Add("isUser", true);
                context.Items.Add("userId", s.UserId);
            }
            await cs.SaveChangesAsync();
        }
        else
        {
            context.Items.Add("isUser", false);

        }


        await next.Invoke(context);
    }
}