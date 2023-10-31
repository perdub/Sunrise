namespace Sunrise.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class UserModel : PageModel
{
    public Guid UserId {get;set;}

    public UserModel()
    {
        
    }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        if(id==null){
            return NotFound();
        }

        UserId = id;

        return Page();
    }
}