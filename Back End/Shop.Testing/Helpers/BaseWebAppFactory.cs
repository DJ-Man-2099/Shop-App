using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shop.Authentication;
using Shop.Authentication.Interfaces;
using Shop.Authentication.Services;
using Shop.DataAccess;

namespace Shop.Testing.Helpers;

public class BaseWebAppFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Testing");
		builder.ConfigureServices(services =>
		{
			var descriptor = services.SingleOrDefault(
				d => d.ServiceType ==
					typeof(DbContextOptions<AppDBContext>));

			services.Remove(descriptor!);
			descriptor = services.SingleOrDefault(
				d => d.ServiceType ==
					typeof(DbContextOptions<TokenBlacklistDBContext>));

			services.Remove(descriptor!);
			// descriptor = services.SingleOrDefault(
			// 	d => d.ServiceType ==
			// 		typeof(ITokenService));

			// services.Remove(descriptor!);

			// services.AddScoped<ITokenService, TestTokenService>();

			var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

			services.AddDbContext<AppDBContext>(options =>
			{

				var connection = new SqliteConnection(configuration?.GetConnectionString("SqliteDatabase"));

				options.UseSqlite(connection);
			});
			services.AddDbContext<TokenBlacklistDBContext>(options =>
			{
				var connection = new SqliteConnection(configuration?.GetConnectionString("SqliteTokenDatabase"));


				options.UseSqlite(connection);
			});
		});
	}
}
