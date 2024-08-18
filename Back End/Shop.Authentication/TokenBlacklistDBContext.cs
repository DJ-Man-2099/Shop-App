using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Shop.Models.DB;

namespace Shop.Authentication;

public class TokenBlacklistDBContext : DbContext
{

	public TokenBlacklistDBContext(DbContextOptions<TokenBlacklistDBContext> options) : base(options)
	{
	}

	public DbSet<TokenBlacklist> TokenBlacklists { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			// Only configure if options were not already passed via DI
			var builder = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("tokendbsettings.json");

			var configuration = builder.Build();
			var dbType = configuration?.GetConnectionString("DefaultTokenDatabase");
			// throw new Exception($"Config is null? {Configuration?.GetConnectionString(dbType!)}");
			optionsBuilder.UseSqlite(configuration?.GetConnectionString(dbType!),
			b => { b.MigrationsAssembly("Shop.Authentication"); b.MigrationsHistoryTable(tableName: HistoryRepository.DefaultTableName); });
		}
		base.OnConfiguring(optionsBuilder);
	}
}
