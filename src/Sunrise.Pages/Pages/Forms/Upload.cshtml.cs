using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Sunrise.Pages.Forms;

public class UploadPostModel : PageModel
{
    private readonly ILogger<UploadPostModel> _logger;

    public UploadPostModel(ILogger<UploadPostModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        var httpContext = (bool)HttpContext.Items["IsLogged"];
        if(!httpContext){
            return NotFound();
        }

        return Page();
    }
}