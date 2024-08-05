using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shop.Models;

namespace Shop.DataAccess;

public class SQLiteContext : IdentityDbContext<User, IdentityRole<int>, int>
{

	protected readonly IConfiguration Configuration;

	public SQLiteContext(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	// Default constructor (For migrations)
	public SQLiteContext()
	{
		var builder = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json");

		Configuration = builder.Build();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite(Configuration?.GetConnectionString("SqliteDatabase"),
		b => b.MigrationsAssembly("Shop.DataAccess"));
		base.OnConfiguring(optionsBuilder);
	}

	public DbSet<Category> Categories { get; set; }
	public DbSet<Group> Groups { get; set; }
	public DbSet<Product> Products { get; set; }

}
