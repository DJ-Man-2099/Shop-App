using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shop.Models.DB;

namespace Shop.DataAccess;

public class AppDBContext : IdentityDbContext<User, IdentityRole<int>, int>
{

	protected readonly IConfiguration? Configuration;

	public AppDBContext(
		IConfiguration? configuration = null,
		DbContextOptions<AppDBContext>? options = null) : base(options ?? new DbContextOptions<AppDBContext>())
	{
		if (configuration != null)
		{ Configuration = configuration; }
		else
		{
			var builder = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json");

			Configuration = builder.Build();
		}
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var dbType = Configuration?.GetConnectionString("DefaultDatabase");

		optionsBuilder.UseSqlite(Configuration?.GetConnectionString(dbType!),
		b => b.MigrationsAssembly("Shop.DataAccess"));

		base.OnConfiguring(optionsBuilder);
	}

	public DbSet<Category> Categories { get; set; }
	public DbSet<Group> Groups { get; set; }
	public DbSet<Product> Products { get; set; }

}
