namespace Sunrise.Pages;

using Sunrise.Database;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public int PostsCount;
    public int UsersCount;
    public int TagsCount;
    public TimeSpan Uptime => DateTime.Now - _systemProcess.StartTime;
    public async Task<IActionResult> OnGet(int json = 0)
    {
        var counts = _context
            .Database
            .SqlQuery<int>($"SELECT count(*) from \"Posts\" UNION ALL SELECT count(*) from \"Tags\" UNION ALL SELECT count(*) from \"Accounts\";")
            .ToArray();
        
        PostsCount = counts[0];
        UsersCount = counts[2];
        TagsCount = counts[1];

        if(json!=0){
            return Content("{\"posts\":"+PostsCount+",\"users\":"+UsersCount+",\"uptime\":"+Uptime+",\"tags\":"+TagsCount+"}", "application/json");
        }
        return Page();
    }
}