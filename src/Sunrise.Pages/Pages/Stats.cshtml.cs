namespace Sunrise.Pages;

using Sunrise.Database;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class StatsModel : SunModel
{
    private SunriseContext _context;
    private static Process _systemProcess;
    static StatsModel(){
        _systemProcess = Process.GetCurrentProcess();
    }
    public StatsModel(SunriseContext context)
    {
        _context = context;
    }
    public int PostsCount => _context.Posts.Count();
    public int UsersCount => _context.Accounts.Count();
    public int TagsCount => _context.Tags.Count();
    public TimeSpan Uptime => DateTime.UtcNow - _systemProcess.StartTime;
    public async Task<IActionResult> OnGet(int json = 0)
    {
        if(json!=0){
            return Content("{\"posts\":"+PostsCount+",\"users\":"+UsersCount+",\"uptime\":"+Uptime+",\"tags\":"+TagsCount+"}", "application/json");
        }
        return Page();
    }
}