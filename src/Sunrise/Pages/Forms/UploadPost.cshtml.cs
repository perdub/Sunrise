using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sunrise.Pages.Forms;

public class UploadPostModel : PageModel
{
    private readonly ILogger<UploadPostModel> _logger;

    public UploadPostModel(ILogger<UploadPostModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        
    }
}
