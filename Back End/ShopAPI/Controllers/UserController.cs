using System;
using Microsoft.AspNetCore.Mvc;
using Shop.Authentication.Services;
using Shop.DataAccess.Interfaces;

namespace ShopAPI.Controllers;
[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
	private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpGet("{id:int}")]
	[TokenAuthorize(roles: ["Admin"])]
	public async Task<IActionResult> GetUserById(int id)
	{
		var result = await _userService.GetUserByIdAsync(id);
		if (result.Succeeded)
		{
			return Ok(result.Value);
		}
		return BadRequest(result.Errors);
	}
}
