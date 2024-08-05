using System;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;

namespace Shop.Testing;

public class BaseUnitTest
{
	protected readonly SQLiteContext _context;
	public BaseUnitTest()
	{

		_context = new(TestDBConnection.GetConnection());
		_context.Database.EnsureDeleted();
		_context.Database.Migrate(); // Apply migrations
	}

	public void Dispose()
	{
		_context.Database.EnsureDeleted();
		_context.Dispose();
	}
}
