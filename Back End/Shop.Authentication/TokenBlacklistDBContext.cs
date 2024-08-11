using Microsoft.EntityFrameworkCore;
using Shop.Models.DB;

namespace Shop.Authentication;

public class TokenBlacklistDBContext : DbContext
{
	public TokenBlacklistDBContext(DbContextOptions<TokenBlacklistDBContext> options) : base(options) { }

	public DbSet<TokenBlacklist> TokenBlacklists { get; set; }
}
