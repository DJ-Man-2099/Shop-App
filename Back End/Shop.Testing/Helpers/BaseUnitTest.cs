using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;

namespace Shop.Testing.Helpers;

public class BaseUnitTest
{
	protected readonly AppDBContext _context;
	public BaseUnitTest()
	{
		DbContextOptions<AppDBContext> _dbContextOptions;
		var builder = new DbContextOptionsBuilder<AppDBContext>();
		var config = TestConfiguration.GetConfiguration();
		var connection = new SqliteConnection(config.GetConnectionString("UnitDatabase"));
		// throw new Exception($"Connection string: {connection.ConnectionString}");
		_dbContextOptions = builder.UseSqlite(connection).Options;
		_context = new(_dbContextOptions);
		_context.Database.EnsureDeleted();
		_context.Database.Migrate(); // Apply migrations
	}

	public void Dispose()
	{
		_context.Database.EnsureDeleted();
		_context.Dispose();
	}
}
