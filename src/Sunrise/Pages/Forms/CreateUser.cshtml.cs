using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sunrise.Pages.Forms;

public class CreateUserModel : PageModel
{
    private readonly ILogger<CreateUserModel> _logger;

    public CreateUserModel(ILogger<CreateUserModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        
    }
}
