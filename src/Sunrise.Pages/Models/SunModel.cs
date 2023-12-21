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

    public string GetStringRatingRepresentation(Sunrise.Types.Enums.PostRating rating){
        switch(rating){
            case Sunrise.Types.Enums.PostRating.VerySafe:
                return "Very safe";
            case Sunrise.Types.Enums.PostRating.Save:
                return "Save";
            case Sunrise.Types.Enums.PostRating.Warning:
                return "Warning";
            case Sunrise.Types.Enums.PostRating.Explicit:
                return "Explicit";
            case Sunrise.Types.Enums.PostRating.Guro:
                return "Guro";
            default:
                return "Unknown";
        }
    }
}