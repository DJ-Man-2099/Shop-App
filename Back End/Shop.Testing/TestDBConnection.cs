using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;

namespace Shop.Testing;

public static class TestDBConnection
{

	public static DbContextOptions<SQLiteContext> GetConnection()
	{
		DbContextOptions<SQLiteContext> _dbContextOptions;
		var builder = new DbContextOptionsBuilder<SQLiteContext>();
		var connection = new SqliteConnection("DataSource=Shop-Test.db");
		_dbContextOptions = builder.UseSqlite(connection).Options;
		return _dbContextOptions;
	}

}
