using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Shop.Authentication.Interfaces;
using Shop.DataAccess;
using Shop.Models.Contracts;
using Shop.Models.DB;

namespace Shop.Authentication.Services;

public class JWTTokenService : ITokenService
{
	private readonly JwtSecurityTokenHandler _tokenHandler;
	private readonly SignInManager<User> _signInManager;
	private readonly BlackListTokenService _blacklistService;
	private readonly string _key;
	private readonly byte[] _encodedKey;
	private readonly string _issuer;
	private readonly string _audience;

	public JWTTokenService(JwtSecurityTokenHandler tokenHandler, SignInManager<User> signInManager, IConfiguration configuration, BlackListTokenService blacklistService)
	{
		_tokenHandler = tokenHandler;
		_signInManager = signInManager;
		_key = configuration["JwtConfig:Key"]!;
		_issuer = configuration["JwtConfig:Issuer"]!;
		_audience = configuration["JwtConfig:Audience"]!;
		_encodedKey = Encoding.ASCII.GetBytes(_key);
		_blacklistService = blacklistService;
	}

	public string GenerateToken(User user)
	{
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity([new Claim("id", user.Id.ToString())]),
			Expires = DateTime.UtcNow.AddDays(7),
			Audience = _audience,
			Issuer = _issuer,
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_encodedKey),
			SecurityAlgorithms.HmacSha256Signature)
		};
		var token = _tokenHandler.CreateToken(tokenDescriptor);
		return _tokenHandler.WriteToken(token);
	}

	public async Task<OpResult<ClaimsIdentity>> ValidateStringToken(string token)
	{
		if (token == null)
			return OpResult<ClaimsIdentity>.UnAuthenticatedError("Token is invalid");

		try
		{

			_tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				IssuerSigningKey = new SymmetricSecurityKey(_encodedKey),
				ValidateLifetime = true,
				ValidateAudience = true,
				ValidIssuer = _issuer,
				ValidAudience = _audience,
				ValidateIssuer = true
			}, out SecurityToken validatedToken);

			return await ValidateToken(validatedToken);
		}
		catch
		{
			// return null if validation fails
			return OpResult<ClaimsIdentity>.UnAuthenticatedError("Token is invalid");
		}

	}

	public async Task<OpResult<ClaimsIdentity>> ValidateToken(SecurityToken token)
	{
		Console.WriteLine(token.ToString()!);
		Console.WriteLine(_blacklistService.IsTokenBlacklisted(token.UnsafeToString()!));
		Console.WriteLine(string.Join(", ", _blacklistService.GetBlacklistedTokens().Select(t => t.Token)));
		if (token == null || _blacklistService.IsTokenBlacklisted(token.UnsafeToString()!))
			return OpResult<ClaimsIdentity>.UnAuthenticatedError("Token is null");
		try
		{

			var jwtToken = (JsonWebToken)token;
			// Console.WriteLine("This Fails here");
			var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
			var user = await _signInManager.UserManager.FindByIdAsync(userId.ToString());
			if (user == null)
				return OpResult<ClaimsIdentity>.UnAuthenticatedError("User not found");
			var claims = new List<Claim>
			{
				new("id", userId.ToString()),
				new(ClaimTypes.Name, user!.UserName!)
			};
			var roles = await _signInManager.UserManager.GetRolesAsync(user);
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			var identity = new ClaimsIdentity(claims, "jwt");
			// return user id from JWT token if validation successful
			return new OpResult<ClaimsIdentity>
			{
				Value = identity
			};
		}
		catch
		{
			// return null if validation fails
			return OpResult<ClaimsIdentity>.UnAuthenticatedError("Token is invalid");
		}
	}
}
