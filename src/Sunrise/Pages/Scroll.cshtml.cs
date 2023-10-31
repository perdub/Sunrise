using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sunrise.Types;

namespace Sunrise.Pages;

public class ScrollModel : PageModel
{
    private readonly ILogger<ScrollModel> _logger;
    SunriseContext _context;
    public ScrollModel(ILogger<ScrollModel> logger, SunriseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task OnGetAsync()
    {}
}
