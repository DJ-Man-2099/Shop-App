using System;
using Shop.Models.DB;

namespace Shop.Authentication.Services;

public class BlackListTokenService
{
	private readonly TokenBlacklistDBContext _context;
	public BlackListTokenService(TokenBlacklistDBContext context)
	{
		_context = context;
	}

	public void AddTokenToBlacklist(string token)
	{
		_context.TokenBlacklists.Add(new TokenBlacklist { Token = token });
		_context.SaveChanges();
	}

	public bool IsTokenBlacklisted(string token)
	{
		return _context.TokenBlacklists.Any(t => t.Token == token);
	}

	public IEnumerable<TokenBlacklist> GetBlacklistedTokens()
	{
		return _context.TokenBlacklists.ToList();
	}


}
