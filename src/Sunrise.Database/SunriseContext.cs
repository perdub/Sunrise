using Microsoft.EntityFrameworkCore;
using Sunrise.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.DependencyInjection;

namespace Sunrise.Database;

public class SunriseContext : DbContext
{
    private IConfiguration _config;
    private ILogger<SunriseContext> _logger;

    [ActivatorUtilitiesConstructor]
    public SunriseContext(IConfiguration config, ILogger<SunriseContext> logger)
    {
        _config = config;
        _logger = logger;
    }

    internal SunriseContext(DbContextOptions<SunriseContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Sunrise.Types.File> Files { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Session> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_config.GetValue<string>("ConnectionString"));
        }
    }
}