using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sunrise.Utilities;
namespace Sunrise.Pages;

public class RegistrationModel : PageModel
{
    private readonly ILogger<RegistrationModel> _logger;

    public RegistrationModel(ILogger<RegistrationModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        if(HttpContext.Items.IsUser()){
            //return RedirectToPage($"/users/{HttpContext.Items.UserId()}/");
        }
        


        return Page();
    }
}

