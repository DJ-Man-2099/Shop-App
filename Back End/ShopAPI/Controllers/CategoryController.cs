using Microsoft.AspNetCore.Mvc;
using Shop.Authentication;
using Shop.Authentication.Services;
using Shop.DataAccess.Interfaces;
using Shop.Models.Contracts;
using Shop.Models.Contracts.Category;

namespace ShopAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private readonly ICategoriesService _service;

	public CategoryController(ICategoriesService categoriesService)
	{
		_service = categoriesService;
	}

	[HttpGet]
	[TokenAuthorize]
	public async Task<IActionResult> GetCategories()
	{
		var result = await _service.GetCategoriesAsync();
		return Ok(result.Value!);
	}

	[HttpGet("base")]
	[TokenAuthorize]
	public async Task<IActionResult> GetBaseCategory()
	{
		var result = await _service.GetBaseCategoryAsync();
		if (!result.Succeeded)
		{
			return NotFound(result.Errors!);
		}
		return Ok(result.Value!);
	}

	[HttpGet("{id:int}")]
	[TokenAuthorize]
	public async Task<IActionResult> GetCategory(int id)
	{
		var result = await _service.GetCategoryAsync(id);
		if (!result.Succeeded)
		{
			return NotFound(result.Errors!);
		}
		return Ok(result.Value!);
	}

	[HttpPost]
	[HttpPatch("{id:int}")]
	[TokenAuthorize]
	public async Task<IActionResult> UpsertCategory(InputCategory category, int? id = null)
	{
		var result = await _service.UpsertCategoryAsync(category, id);
		if (result.Succeeded)
		{
			return Ok(result.Value!);
		}
		else
		{
			if (result.Errors!.ContainsKey(OpResult.NotFoundCode))
			{
				return NotFound(result.Errors[OpResult.NotFoundCode]);
			}
			else if (result.Errors!.ContainsKey(OpResult.ServerErrorCode))
			{
				return StatusCode(500, result.Errors[OpResult.ServerErrorCode]);
			}
			return BadRequest(result.Errors[OpResult.BadRequestCode]);
		}
	}

	[HttpPatch("base")]
	[TokenAuthorize(roles: [Roles.Admin])]
	public async Task<IActionResult> SetBaseCategoryPrice(PriceUpdateModel price)
	{
		var result = await _service.SetBaseCategoryPriceAsync(price.Price);
		if (!result.Succeeded)
		{
			if (result.Errors!.ContainsKey(OpResult.NotFoundCode))
			{
				return NotFound(result.Errors[OpResult.NotFoundCode]);
			}
			return StatusCode(500, result.Errors[OpResult.ServerErrorCode]);
		}
		return Ok(result.Value!);
	}

	[HttpDelete("{id:int}")]
	[TokenAuthorize(roles: [Roles.Admin])]
	public async Task<IActionResult> DeleteCategory(int id)
	{
		var result = await _service.DeleteCategoryAsync(id);
		if (!result.Succeeded)
		{
			return NotFound(result.Errors!);
		}
		return Ok();
	}

	[HttpPost("changebase/{id:int}")]
	[TokenAuthorize(roles: [Roles.Admin])]
	public async Task<IActionResult> ChangeBaseCategory(int id)
	{
		var result = await _service.ChangeBaseCategoryAsync(id);
		if (!result.Succeeded)
		{
			return NotFound(result.Errors!);
		}
		return Ok(result.Value!);
	}

}
public class PriceUpdateModel
{
	public float Price { get; set; }
}
