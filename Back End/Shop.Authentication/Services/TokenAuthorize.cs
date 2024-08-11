using System;

namespace Shop.Authentication.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Authentication.Interfaces;
using Shop.Models.DB;
using System.Security.Claims;

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
		var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
		var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
		var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

		if (string.IsNullOrEmpty(token))
		{
			context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
			return;
		}
		var userValidation = await tokenService.ValidateStringToken(token);
		if (!userValidation.Succeeded)
		{
			context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
			return;
		}
		var user = userValidation.Value!;
		var userRoles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

		if (_roles.Length > 0 && !_roles.Any(userRoles.Contains))
		{
			context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
			return;
		}
	}
}
