using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;

namespace Shop.Testing.Helpers;

public class BaseUnitTest
{
	protected readonly SQLiteContext _context;
	public BaseUnitTest()
	{
		DbContextOptions<SQLiteContext> _dbContextOptions;
		var builder = new DbContextOptionsBuilder<SQLiteContext>();
		var config = TestConfiguration.GetConfiguration();
		var connection = new SqliteConnection(config.GetConnectionString("UnitDatabase"));
		_dbContextOptions = builder.UseSqlite(connection).Options;
		_context = new(options: _dbContextOptions);
		_context.Database.EnsureDeleted();
		_context.Database.Migrate(); // Apply migrations
	}

	public void Dispose()
	{
		_context.Database.EnsureDeleted();
		_context.Dispose();
	}
}
