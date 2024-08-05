using System;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;

namespace Shop.Testing.Helpers;

public class BaseIntegrationTest : IClassFixture<BaseWebAppFactory>
{
	protected readonly BaseWebAppFactory _factory;
	protected readonly HttpClient _client;


	public BaseIntegrationTest(BaseWebAppFactory factory)
	{
		_factory = factory;
		_client = _factory.CreateClient();
		ResetDatabase();
	}

	protected void ResetDatabase()
	{
		using (var scope = _factory.Services.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<SQLiteContext>();
			db.Database.EnsureDeleted();
			db.Database.Migrate();
		}
	}
}
