using Microsoft.AspNetCore.Mvc;
using Shop.Authentication;
using Shop.Authentication.Services;
using Shop.DataAccess.Interfaces;
using Shop.Models.Contracts;
using Shop.Models.Contracts.Product;

namespace ShopAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
	private readonly IProductsService _service;

	public ProductController(IProductsService service)
	{
		_service = service;
	}

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> GetAllProducts()
	{
		var result = await _service.GetAllProductsAsync();
		if (!result.Succeeded)
		{
			return StatusCode(500, result.Errors![OpResult.ServerErrorCode]);
		}
		return Ok(result.Value!);
	}
	[HttpGet("{id:int}")]
	[Authorize]
	public async Task<IActionResult> GetProductById(int id)
	{
		var result = await _service.GetProductByIdAsync(id);
		if (!result.Succeeded)
		{
			return NotFound(result.Errors![OpResult.NotFoundCode]);
		}
		return Ok(result.Value!);
	}
	[HttpGet("group/{groupId:int}")]
	[Authorize]
	public async Task<IActionResult> GetProductsByGroupId(int groupId)
	{
		var result = await _service.GetProductsByGroupIdAsync(groupId);
		if (!result.Succeeded)
		{
			return NotFound(result.Errors![OpResult.NotFoundCode]);
		}
		return Ok(result.Value!);
	}
	[HttpGet("category/{categoryId:int}")]
	[Authorize]
	public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
	{
		var result = await _service.GetProductsByCategoryIdAsync(categoryId);
		if (!result.Succeeded)
		{
			return NotFound(result.Errors![OpResult.NotFoundCode]);
		}
		return Ok(result.Value!);
	}
	[HttpPost]
	[HttpPatch("{id:int}")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> UpsertProduct(InputProduct product, int? id = null)
	{
		var result = await _service.UpsertProduct(product, id);
		if (!result.Succeeded)
		{
			if (result.Errors!.ContainsKey(OpResult.BadRequestCode))
			{
				return BadRequest(result.Errors![OpResult.BadRequestCode]);
			}
			else if (result.Errors!.ContainsKey(OpResult.NotFoundCode))
			{
				return NotFound(result.Errors![OpResult.NotFoundCode]);
			}
			return StatusCode(500, result.Errors![OpResult.ServerErrorCode]);
		}
		return Ok(result.Value!);
	}
	[HttpDelete("{id:int}")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> DeleteProductById(int id)
	{
		var result = await _service.DeleteProduct(id);
		if (!result.Succeeded)
		{
			if (result.Errors!.ContainsKey(OpResult.NotFoundCode))
			{
				return NotFound(result.Errors![OpResult.NotFoundCode]);
			}
			return StatusCode(500, result.Errors![OpResult.ServerErrorCode]);
		}
		return Ok(result.Value!);
	}
}
