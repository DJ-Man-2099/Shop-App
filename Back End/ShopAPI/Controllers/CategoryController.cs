using System;
using Microsoft.AspNetCore.Mvc;
using Shop.DataAccess.Interfaces;
using Shop.Models;

namespace ShopAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController
{
	private readonly ICategoriesService _categoriesService;

	public CategoryController(ICategoriesService categoriesService)
	{
		_categoriesService = categoriesService;
	}

	[HttpGet]
	public async Task<IEnumerable<Category>> GetCategories()
	{
		var result = await _categoriesService.GetCategoriesAsync();
		return result.Value!;
	}

	[HttpGet("{id}")]
	public async Task<Category> GetCategory(int id)
	{
		var result = await _categoriesService.GetCategoryAsync(id);
		return result.Value!;
	}

	[HttpPost]
	public async Task<Category> UpsertCategory(InputCategory category, int? id)
	{
		var result = await _categoriesService.UpsertCategoryAsync(category, id);
		if (result.Succeeded)
		{
			return result.Value!;
		}
		else
		{
			Console.WriteLine(result.Errors!);
			throw new Exception("Failed to upsert category");
		}
	}

}
