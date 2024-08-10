using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shop.DataAccess.Interfaces;
using Shop.Models.Contracts;
using Shop.Models.Contracts.Product;
using Shop.Models.DB;

namespace Shop.DataAccess.Services;

public class ProductsService : IProductsService
{
	private readonly AppDBContext _context;

	public ProductsService(AppDBContext context)
	{
		_context = context;
	}

	public async Task<OpResult> DeleteProduct(int id)
	{
		var product = await _context.Products.FindAsync(id);
		if (product == null)
		{
			return OpResult.NotFound("Product not Found");
		}
		_context.Products.Remove(product);
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (System.Exception)
		{
			return OpResult.ServerError($"Error deleting Product with Name {product.Name}");
		}
		return new OpResult();
	}

	public async Task<OpResult<IEnumerable<ProductDTO>>> GetAllProductsAsync()
	{
		try
		{
			return new OpResult<IEnumerable<ProductDTO>>
			{
				Value = await GetFullProduct().Select(p => ProductDTO.FromProduct(p))
											.ToListAsync()
			};
		}
		catch (System.Exception)
		{
			return OpResult<IEnumerable<ProductDTO>>.ServerError("Error fetching products");
		}
	}

	private IIncludableQueryable<Product, Category> GetFullProduct()
	{
		return _context.Products.Include(p => p.Group)
								.Include(p => p.Group.Category);
	}

	public async Task<OpResult<ProductDTO>> GetProductsByCategoryIdAsync(int categoryId)
	{
		var category = await _context.Categories.FindAsync(categoryId);
		if (category == null)
		{
			return OpResult<ProductDTO>.NotFound("Category not Found");
		}
		var product = await GetFullProduct()
							.FirstOrDefaultAsync(p => p.Group.CategoryId == categoryId);
		if (product == null)
		{
			return OpResult<ProductDTO>.NotFound("Product not Found");
		}
		return new OpResult<ProductDTO>
		{
			Value = ProductDTO.FromProduct(product)
		};
	}

	public async Task<OpResult<ProductDTO>> GetProductsByGroupIdAsync(int groupId)
	{
		var group = await _context.Groups.FindAsync(groupId);
		if (group == null)
		{
			return OpResult<ProductDTO>.NotFound("Group not Found");
		}
		var product = await GetFullProduct()
							.FirstOrDefaultAsync(p => p.GroupId == groupId);
		if (product == null)
		{
			return OpResult<ProductDTO>.NotFound("Product not Found");
		}
		return new OpResult<ProductDTO>
		{
			Value = ProductDTO.FromProduct(product)
		};
	}

	public async Task<OpResult<ProductDTO>> GetProductByIdAsync(int id)
	{
		var product = await GetFullProduct().FirstOrDefaultAsync(p => p.Id == id);
		if (product == null)
		{
			return OpResult<ProductDTO>.NotFound("Product not Found");
		}
		return new OpResult<ProductDTO>
		{
			Value = ProductDTO.FromProduct(product)
		};
	}

	public async Task<OpResult<ProductDTO>> UpsertProduct(InputProduct product, int? id)
	{
		if (id == null)
		{
			return await AddNew(product);
		}
		else
		{
			return await UpdateProduct(product, id);
		}
	}

	private async Task<OpResult<ProductDTO>> UpdateProduct(InputProduct product, int? id)
	{
		var existingProduct = await GetFullProduct().FirstOrDefaultAsync(p => p.Id == id);
		if (existingProduct == null)
		{
			return OpResult<ProductDTO>.NotFound("Product not Found");
		}
		existingProduct.Name = product.Name ?? existingProduct.Name;
		existingProduct.ManufacturePrice = product.ManufacturePrice ?? existingProduct.ManufacturePrice;
		existingProduct.WeightGrams = product.WeightGrams ?? existingProduct.WeightGrams;
		existingProduct.WeightMilliGrams = product.WeightMilliGrams ?? existingProduct.WeightMilliGrams;
		if (product.GroupId.HasValue)
		{
			var group = await _context.Groups.FindAsync(product.GroupId);
			if (group == null)
			{
				return OpResult<ProductDTO>.NotFound("Group not Found");
			}
			existingProduct.Group = group;
		}
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return OpResult<ProductDTO>.ServerError($"Error updating Product with Name {product.Name}");
		}
		return new OpResult<ProductDTO>
		{
			Value = ProductDTO.FromProduct(existingProduct)
		};
	}

	private async Task<OpResult<ProductDTO>> AddNew(InputProduct product)
	{
		if (product.Name == null)
		{
			return OpResult<ProductDTO>.BadRequest("Name is Required");
		}
		if (!product.ManufacturePrice.HasValue)
		{
			return OpResult<ProductDTO>.BadRequest("Manufacture Price is Required");
		}
		if (!product.WeightGrams.HasValue)
		{
			return OpResult<ProductDTO>.BadRequest("Weight in Grams is Required");
		}
		if (!product.WeightMilliGrams.HasValue)
		{
			return OpResult<ProductDTO>.BadRequest("Weight in MilliGrams is Required");
		}
		if (!product.GroupId.HasValue)
		{
			return OpResult<ProductDTO>.BadRequest("GroupId is Required");
		}
		var group = await _context.Groups.Include(g => g.Category).FirstOrDefaultAsync(g => g.Id == product.GroupId.Value);
		if (group == null)
		{
			return OpResult<ProductDTO>.NotFound("Group not Found");
		}
		var newProduct = new Product
		{
			Name = product.Name!,
			ManufacturePrice = product.ManufacturePrice.Value,
			WeightGrams = product.WeightGrams.Value,
			WeightMilliGrams = product.WeightMilliGrams.Value,
			Group = group
		};
		_context.Products.Add(newProduct);
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return OpResult<ProductDTO>.ServerError($"Error adding Product with Name {product.Name}");
		}
		return new OpResult<ProductDTO>
		{
			Value = ProductDTO.FromProduct(newProduct)
		};

	}
}
