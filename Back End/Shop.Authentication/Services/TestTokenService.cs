using System;
using Shop.Authentication.Interfaces;
using Shop.Models;

namespace Shop.Authentication.Services;

public class TestTokenService : ITokenService
{
	private readonly string _token = "Testing";
	public string GenerateToken(User user)
	{
		return _token;
	}

	public OpResult<User> ValidateToken(string token)
	{
		if (token == _token)
		{
			return new OpResult<User>
			{
				Value = new User()
			};
		}
		return new OpResult<User>
		{
			Succeeded = false,
			Errors = new Dictionary<string, string>
			{
				{ "Token", "Invalid token" }
			}
		};
	}
}
