using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Sunrise.Database;

namespace Sunrise.Pages;
#pragma warning disable CS8652
public abstract class SecureModel(SunriseContext context) : SunModel
#pragma warning restore CS8652
{
    public bool Allow(Sunrise.Types.Enums.PrivilegeLevel requiredLevel){
        var tokenCookie = HttpContext.Request.Cookies["Suntoken"];
        if(tokenCookie is null){
            return false;
        }
        var token =tokenCookie.ToString();

        var session = context.Sessions
            .AsNoTracking()
            .Include(a => a.Account)
            .Where(a => a.SessionId == token)
            .FirstOrDefault();
        
        if(session is null){
            return false;
        }

        if(session.Account.PrivilegeLevel < requiredLevel){
            return false;
        }

        return true;
    }
}