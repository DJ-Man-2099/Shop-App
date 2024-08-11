using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Shop.Models.Contracts;
using Shop.Models.DB;

namespace Shop.Authentication.Interfaces;

public interface ITokenService
{
	public string GenerateToken(User user);
	public Task<OpResult<ClaimsIdentity>> ValidateToken(SecurityToken token);
	public Task<OpResult<ClaimsIdentity>> ValidateStringToken(string token);

}
