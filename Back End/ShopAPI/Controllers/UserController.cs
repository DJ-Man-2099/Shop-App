using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Authentication;
using Shop.Authentication.Services;
using Shop.DataAccess.Interfaces;
using Shop.Models.Contracts;
using Shop.Models.Contracts.User;
using Shop.Models.DB;

namespace ShopAPI.Controllers;
[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
	private readonly IUserService _userService;
	private readonly BlackListTokenService _blacklist;

	public UserController(IUserService userService, SignInManager<User> signInManager, BlackListTokenService blacklist)
	{
		_userService = userService;
		_blacklist = blacklist;
	}

	[HttpGet("{id:int}")]
	[Authorize(Roles = Roles.Admin)]
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
	[Authorize(Roles = Roles.Admin)]
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
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> SignUpWorkerUser(InputSignUp user)
	{
		var result = await _userService.SignUpWorkerAsync(user);
		if (result.Succeeded)
		{
			return Ok(result.Value);
		}
		return BadRequest(result.Errors);
	}

	[HttpPost("admin")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> SignUpAdminUser(InputSignUp user)
	{
		var result = await _userService.SignUpAdminAsync(user);
		if (result.Succeeded)
		{
			return Ok(result.Value);
		}
		return BadRequest(result.Errors);
	}

	[HttpPatch("{id:int}")]
	[Authorize(Roles = Roles.Admin)]
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
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> DeleteUser(int id)
	{
		var result = await _userService.DeleteUserById(id);
		if (result.Succeeded)
		{
			return Ok();
		}
		return NotFound(result.Errors);
	}

	[HttpPost("login")]
	public async Task<IActionResult> SignIn(InputSignIn signIn)
	{
		var result = await _userService.SignInAsync(signIn);
		if (result.Succeeded)
		{
			return Ok(result.Value);
		}
		if (result.Errors!.ContainsKey(OpResult.NotFoundCode))
		{
			return NotFound(result.Errors[OpResult.NotFoundCode]);
		}
		return BadRequest(result.Errors[OpResult.BadRequestCode]);
	}
	[HttpPost("logout")]
	[Authorize]
	public async new Task<IActionResult> SignOut()
	{
		var Token = await HttpContext.GetTokenAsync("access_token");
		_blacklist.AddTokenToBlacklist(Token!);
		return Ok();

	}
}
