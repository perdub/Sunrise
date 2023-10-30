using Microsoft.EntityFrameworkCore;
using Sunrise.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Sunrise.Database;

public class SunriseContext: DbContext
{
    private IConfiguration _config;
    private ILogger<SunriseContext> _logger;

    public SunriseContext(IConfiguration config, ILogger<SunriseContext> logger)
    {
        _config = config;
        _logger = logger;
    }

    public DbSet<Account> Accounts{get;set;}
    public DbSet<Sunrise.Types.File> Files{get;set;}
    public DbSet<Tag> Tags{get;set;}
    public DbSet<Post> Posts{get;set;}
    public DbSet<Session> Sessions{get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=пароль_от_postgres");
    }
}