using System;
using Shop.Models;

namespace Shop.Authentication.Interfaces;

public interface ITokenService
{
	public string GenerateToken(User user);
	public OpResult<User> ValidateToken(string token);

}