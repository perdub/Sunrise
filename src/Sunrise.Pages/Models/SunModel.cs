using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Sunrise.Pages;

public abstract class SunModel : PageModel
{
    public string GetHostUrl(){
        var req = HttpContext.Request;
        StringBuilder sb = new ();
        sb.Append("http://");

        string? xFowardedHost = req.Headers["X-Forwarded-Host"].FirstOrDefault();

        if(xFowardedHost is not null){
            sb.Append(xFowardedHost);
        }
        else{
            sb.Append(req.Host);
        }

        return sb.ToString();
    }
}