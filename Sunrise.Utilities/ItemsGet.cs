#pragma warning disable CS8600, CS8605
namespace Sunrise.Utilities;
using System.Web;
//provide Extensions to extract values from httpcontext.items
public static class ItemsGet
{
    //расширение для получения залогинены ли мы
    public static bool IsUser(this IDictionary<object, object?> i)
    {
        object a;
        bool r = i.TryGetValue("isUser", out a);
        return r ? (bool)a : false;
    }
    public static Guid UserId(this IDictionary<object, object?> i)
    {
        object a;
        bool r = i.TryGetValue("userId", out a);
        return r ? (Guid)a : Guid.Empty;
    }
}