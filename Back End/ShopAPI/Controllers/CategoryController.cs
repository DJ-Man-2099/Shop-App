using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shop.DataAccess.Interfaces;
using Shop.Models;

namespace ShopAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private readonly ICategoriesService _categoriesService;

	public CategoryController(ICategoriesService categoriesService)
	{
		_categoriesService = categoriesService;
	}

	[HttpGet]
	public async Task<IActionResult> GetCategories()
	{
		var result = await _categoriesService.GetCategoriesAsync();
		return Ok(result.Value!);
	}

	[HttpGet("base")]
	public async Task<IActionResult> GetBaseCategory()
	{
		var result = await _categoriesService.GetBaseCategoryAsync();
		if (!result.Succeeded)
		{
			return NotFound(result.Errors!);
		}
		return Ok(result.Value!);
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetCategory(int id)
	{
		var result = await _categoriesService.GetCategoryAsync(id);
		if (!result.Succeeded)
		{
			return NotFound(result.Errors!);
		}
		return Ok(result.Value!);
	}

	[HttpPost]
	[HttpPatch("{id:int}")]
	public async Task<IActionResult> UpsertCategory(InputCategory category, int? id)
	{
		var result = await _categoriesService.UpsertCategoryAsync(category, id);
		if (result.Succeeded)
		{
			return Ok(result.Value!);
		}
		else
		{
			return BadRequest(result.Errors!);
		}
	}

	[HttpPatch("base")]
	public async Task<IActionResult> SetBaseCategoryPrice(PriceUpdateModel price)
	{
		Console.WriteLine(price.Price);
		var result = await _categoriesService.SetBaseCategoryPriceAsync(price.Price);
		if (!result.Succeeded)
		{
			return NotFound(result.Errors!);
		}
		return Ok(result.Value!);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteCategory(int id)
	{
		var result = await _categoriesService.DeleteCategoryAsync(id);
		if (!result.Succeeded)
		{
			return NotFound(result.Errors!);
		}
		return Ok();
	}

}
public class PriceUpdateModel
{
	public float Price { get; set; }
}
