using Microsoft.EntityFrameworkCore;
using Shop.DataAccess.Interfaces;
using Shop.Models;

namespace Shop.DataAccess.Services;

public class SQLCategoriesService : ICategoriesService
{
	private readonly SQLiteContext _context;

	public SQLCategoriesService(SQLiteContext context)
	{
		_context = context;
	}

	public async Task<OpResult<IEnumerable<Category>>> GetCategoriesAsync()
	{
		var categories = await _context.Categories.ToListAsync();
		return new OpResult<IEnumerable<Category>> { Value = categories };
	}

	public async Task<OpResult<Category>> GetCategoryAsync(int id)
	{
		var category = await _context.Categories.FindAsync(id);
		return new OpResult<Category> { Value = category };
	}

	public async Task<OpResult<Category>> UpsertCategoryAsync(InputCategory category, int? id)
	{
		if (id == null)
		{
			return await AddNew(category);
		}
		else
		{
			return await UpdateExisting(category, id.Value);
		}
	}

	private async Task<OpResult<Category>> AddNew(InputCategory category)
	{
		var baseCategory = await _context.Categories.FirstOrDefaultAsync(c => c.IsPrimary);
		float price;
		bool isPrimary = baseCategory == null;
		if (baseCategory == null)
		{
			if (category.Price == null)
			{
				return new OpResult<Category>
				{
					Errors = new Dictionary<string, string> { { "Price", "Price is required" } }
				};
			}
			price = category.Price.Value;
		}
		else
		{
			price = baseCategory.Price * category.Standard / baseCategory.Standard;
		}
		var newCategory = new Category
		{
			Name = category.Name,
			Standard = category.Standard,
			Price = price,
			IsPrimary = isPrimary
		};
		_context.Categories.Add(newCategory);
		await _context.SaveChangesAsync();
		return new OpResult<Category> { Value = newCategory };
	}

	private async Task<OpResult<Category>> UpdateExisting(InputCategory category, int id)
	{
		var existingCategory = await _context.Categories.FindAsync(id);
		if (existingCategory == null)
		{
			return new OpResult<Category>
			{
				Errors = new Dictionary<string, string> { { "Id", "Category not found" } }
			};
		}
		existingCategory.Name = category.Name;
		existingCategory.Standard = category.Standard;
		if (existingCategory.IsPrimary && category.Price != existingCategory.Price)
		{
			existingCategory.Price = category.Price ?? existingCategory.Price;
			var categories = await _context.Categories.Where(c => !c.IsPrimary).ToListAsync();
			foreach (var c in categories)
			{
				c.Price = category.Price!.Value * c.Standard / existingCategory.Standard;
			}
		}
		await _context.SaveChangesAsync();
		return new OpResult<Category> { Value = existingCategory };
	}
}
