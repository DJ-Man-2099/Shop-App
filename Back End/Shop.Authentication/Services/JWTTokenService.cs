using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shop.Authentication.Interfaces;
using Shop.DataAccess;
using Shop.Models.Contracts;
using Shop.Models.DB;

namespace Shop.Authentication.Services;

public class JWTTokenService : ITokenService
{
	private readonly JwtSecurityTokenHandler _tokenHandler;
	private readonly AppDBContext _context;
	private readonly string _key;
	private readonly byte[] _encodedKey;
	private readonly string _issuer;
	private readonly string _audience;

	public JWTTokenService(JwtSecurityTokenHandler tokenHandler, AppDBContext context, IConfiguration configuration)
	{
		_tokenHandler = tokenHandler;
		_context = context;
		_key = configuration["JwtConfig:Key"]!;
		_issuer = configuration["JwtConfig:Issuer"]!;
		_audience = configuration["JwtConfig:Audience"]!;
		_encodedKey = Encoding.ASCII.GetBytes(_key);
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

	public OpResult<User> ValidateToken(string token)
	{
		if (token == null)
			return OpResult<User>.UnAuthenticatedError("Token is null");
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

			var jwtToken = (JwtSecurityToken)validatedToken;
			var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
			var user = _context.Users.Find(userId);

			// return user id from JWT token if validation successful
			return new OpResult<User>
			{
				Value = user
			};
		}
		catch
		{
			// return null if validation fails
			return OpResult<User>.UnAuthenticatedError("Token is invalid");
		}
	}
}
