using System;

namespace Shop.Authentication.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Authentication.Interfaces;
using Shop.Models.DB;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//
// Summary:
//     A filter that confirms request authorization using a token.
public class TokenAuthorizeAttribute : Attribute, IAuthorizationFilter
{
	private readonly string[] _roles;

	//
	// Summary:
	//     A filter that confirms request authorization using a token.
	public TokenAuthorizeAttribute()
	{
		_roles = [];
	}
	//
	// Summary:
	//     A filter that confirms request authorization using a token.
	public TokenAuthorizeAttribute(params string[] roles)
	{
		_roles = roles;
	}

	public async void OnAuthorization(AuthorizationFilterContext context)
	{
		// Console.WriteLine("OnAuthorization");
		var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
		var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
		var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

		if (string.IsNullOrEmpty(token))
		{
			context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
			return;
		}
		var userValidation = tokenService.ValidateToken(token);
		if (!userValidation.Succeeded)
		{
			context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
			return;
		}
		// var user = userValidation.Value!;
		// var userRoles = await signInManager.UserManager.GetRolesAsync(user);

		// if (_roles.Length > 0 && !_roles.Any(userRoles.Contains))
		// {
		// 	context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
		// 	return;
		// }
	}
}
