using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Authentication;
using Shop.Authentication.Services;
using Shop.DataAccess.Interfaces;
using Shop.Models.Contracts;
using Shop.Models.Contracts.Group;

namespace ShopAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
	private readonly IGroupsService _service;

	public GroupController(IGroupsService service)
	{
		_service = service;
	}

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> GetAllGroups()
	{
		var result = await _service.GetGroupsAsync();
		return Ok(result.Value!);
	}
	[HttpGet("category/{id:int}")]
	[Authorize]
	public async Task<IActionResult> GetGroupsByCategoryId(int id)
	{
		var result = await _service.GetGroupsByCategoryIdAsync(id);
		return Ok(result.Value!);
	}
	[HttpGet("{id:int}")]
	[Authorize]
	public async Task<IActionResult> GetGroup(int id)
	{
		var result = await _service.GetGroupAsync(id);
		if (!result.Succeeded)
		{
			return NotFound(result.Errors![OpResult.NotFoundCode]);
		}
		return Ok(result.Value!);
	}

	[HttpPost]
	[HttpPatch("{id:int}")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> UpsertGroup(InputGroup group, int? id = null)
	{
		var result = await _service.UpsertGroupAsync(group, id);
		if (!result.Succeeded)
		{
			if (result.Errors!.ContainsKey(OpResult.NotFoundCode))
			{
				return NotFound(result.Errors![OpResult.NotFoundCode]);
			}
			return StatusCode(500, result.Errors[OpResult.ServerErrorCode]);
		}
		return Ok(result.Value!);
	}

	[HttpDelete("{id:int}")]
	[Authorize]
	public async Task<IActionResult> DeleteGroup(int id)
	{
		var result = await _service.DeleteGroupAsync(id);
		if (!result.Succeeded)
		{
			if (result.Errors!.ContainsKey(OpResult.NotFoundCode))
			{
				return NotFound(result.Errors![OpResult.NotFoundCode]);
			}
			return StatusCode(500, result.Errors[OpResult.ServerErrorCode]);
		}
		return Ok(result.Value!);
	}
}
