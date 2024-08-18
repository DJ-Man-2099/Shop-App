using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Shop.Models.DB;

namespace Shop.DataAccess;

public class AppDBContext : IdentityDbContext<User, IdentityRole<int>, int>
{

    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Only configure if options were not already passed via DI
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("dbsettings.json");
            var configuration = builder.Build();

            var dbType = configuration?.GetConnectionString("DefaultDatabase");
            // throw new Exception($"Config is null? {configuration?.GetConnectionString(dbType!)}");
            optionsBuilder.UseSqlite(configuration?.GetConnectionString(dbType!),
            b =>
            {
                b.MigrationsAssembly("Shop.DataAccess");
                b.MigrationsHistoryTable(tableName: HistoryRepository.DefaultTableName);
            });
        }
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Product> Products { get; set; }

}
