using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tristitja.Auth.Local;
using Tristitja.Auth.Local.Model;
using Tristitja.Share.Configuration;

namespace Tristitja.Share;

public class AppDbContext : DbContext, IAuthLocalDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
    
    private readonly DatabaseOptions _databaseOptions;

    public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<DatabaseOptions> databaseOptions)
        : base(options)
    {
        _databaseOptions = databaseOptions.Value;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .UseNpgsql(GetDbConnectionString())
            .UseSnakeCaseNamingConvention();
    }
    
    private string GetDbConnectionString()
    {
        string connectionString = "";
      
        var host = _databaseOptions.Host;
        var port = _databaseOptions.Port ?? "5432";
        var username = _databaseOptions.Username;
        var password = _databaseOptions.Password;
        var database = _databaseOptions.Database;
      
        connectionString = $"Host={host};Port={port};Username={username}";

        if (password is not null)
        {
            connectionString += $";Password={password}";
        }

        if (database is not null)
        {
            connectionString += $";Database={database}";
        }
      
        return connectionString;
    }
}