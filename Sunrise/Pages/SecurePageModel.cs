//абстрактный класс который имеет метод для проверки уровня полномочий пользователя
namespace Sunrise.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Sunrise.Types;
using Sunrise.Utilities;

public abstract class SecurePageModel : PageModel
{
    internal SunriseContext _context;
    public SecurePageModel(SunriseContext dbContext)
    {
        _context = dbContext;
    }

    public bool IsAllow(PrivilegeLevel userPrivilege = PrivilegeLevel.Moderator)
    {
        if(!HttpContext.Items.IsUser()){
            return false;
        }

        var u = _context.Users.Find(HttpContext.Items.UserId());
        if(u==null && (int)u.Level < (int)userPrivilege)
        {
            return false;
        }

        return true;
    }
}