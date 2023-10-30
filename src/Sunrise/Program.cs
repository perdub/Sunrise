using Microsoft.Extensions.DependencyInjection;

namespace Sunrise;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<Sunrise.Database.SunriseContext>();

        var app = builder.Build();

        app.MapRazorPages();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}
