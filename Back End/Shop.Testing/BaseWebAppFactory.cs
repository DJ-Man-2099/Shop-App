using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;

namespace Shop.Testing;

public class BaseWebAppFactory : WebApplicationFactory<Program>
{

	// private static SqliteConnection GetConnection()
	// {
	// 	var connection = new SqliteConnection("Data Source=Integration-Shop-Test.db");
	// 	return connection;
	// }

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices((context, services) =>
		{
			// Remove the app's ApplicationDbContext registration.
			var descriptor = services.SingleOrDefault(
				d => d.ServiceType ==
					typeof(DbContextOptions<SQLiteContext>));

			if (descriptor != null)
			{
				services.Remove(descriptor);
			}

			// Add a database context (ApplicationDbContext) as A Scoped Service
			// using a SQLite database for testing.

			var connection = new SqliteConnection("Data Source=Integration-Shop-Test.db");
			services.AddScoped(provider =>
			{
				var context = new SQLiteContext(options: new DbContextOptionsBuilder<SQLiteContext>()
					.UseSqlite(connection)
					.Options);
				context.Database.EnsureDeleted();
				context.Database.Migrate();
				return context;
			});

		});
	}
}
