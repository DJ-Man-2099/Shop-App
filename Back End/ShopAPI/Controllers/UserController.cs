using Microsoft.AspNetCore.Mvc;
using Shop.Authentication;
using Shop.Authentication.Services;
using Shop.DataAccess.Interfaces;
using Shop.Models.Contracts.User;

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
	[TokenAuthorize]
	public async Task<IActionResult> GetUserById(int id)
	{
		var result = await _userService.GetUserByIdAsync(id);
		if (result.Succeeded)
		{
			return Ok(result.Value);
		}
		return NotFound(result.Errors);
	}

	[HttpGet]
	[TokenAuthorize]
	public async Task<IActionResult> GetUsers()
	{
		var result = await _userService.GetUsersAsync();
		if (result.Succeeded)
		{
			return Ok(result.Value);
		}
		return BadRequest(result.Errors);
	}

	[HttpPost]
	[TokenAuthorize(roles: [Roles.Admin])]
	public async Task<IActionResult> SignUpWorkerUser(InputSignUpUser user)
	{
		var result = await _userService.SignUpWorkerAsync(user);
		if (result.Succeeded)
		{
			return Ok(result.Value);
		}
		return BadRequest(result.Errors);
	}

	[HttpPost("admin")]
	[TokenAuthorize(roles: [Roles.Admin])]
	public async Task<IActionResult> SignUpAdminUser(InputSignUpUser user)
	{
		var result = await _userService.SignUpAdminAsync(user);
		if (result.Succeeded)
		{
			return Ok(result.Value);
		}
		return BadRequest(result.Errors);
	}

	[HttpPatch("{id:int}")]
	[TokenAuthorize(roles: [Roles.Admin])]
	public async Task<IActionResult> UpdateUser(int id, UserUpdateDTO user)
	{
		var result = await _userService.UpdateUserAsync(id, user);
		if (result.Succeeded)
		{
			return Ok();
		}
		return BadRequest(result.Errors);
	}

	[HttpDelete("{id:int}")]
	[TokenAuthorize(roles: [Roles.Admin])]
	public async Task<IActionResult> DeleteUser(int id)
	{
		var result = await _userService.DeleteUserById(id);
		if (result.Succeeded)
		{
			return Ok();
		}
		return NotFound(result.Errors);
	}

}
