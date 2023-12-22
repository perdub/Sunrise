using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sunrise.Database;

public class BloggingContextFactory : IDesignTimeDbContextFactory<SunriseContext>
{
    public SunriseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SunriseContext>();

        string ConnectionString;
        if(args.Length<1){
            ConnectionString = "Host=localhost;Port=5432;Database=MyDb;Username=postgres;Password=root";
        }
        else{
            ConnectionString = args[0];
        }

        optionsBuilder.UseNpgsql(ConnectionString);

        return new SunriseContext(optionsBuilder.Options);
    }
}