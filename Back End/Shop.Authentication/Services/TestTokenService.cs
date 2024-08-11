using System;
using Shop.Authentication.Interfaces;
using Shop.Models.DB;
using Shop.Models.Contracts;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Shop.Authentication.Services;

public class TestTokenService : ITokenService
{
	private readonly string _token = "Testing";
	public string GenerateToken(User user)
	{
		return _token;
	}

	public Task<OpResult<ClaimsIdentity>> ValidateStringToken(string token)
	{
		throw new NotImplementedException();
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

	public Task<OpResult<ClaimsIdentity>> ValidateToken(SecurityToken token)
	{
		throw new NotImplementedException();
	}
}
