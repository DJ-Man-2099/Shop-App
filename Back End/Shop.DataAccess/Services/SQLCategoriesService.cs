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
		if (category == null)
		{
			return new OpResult<Category>
			{
				Succeeded = false,
				Errors = new Dictionary<string, string> { { "Id", "Category not found" } }
			};
		}
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
		if (string.IsNullOrEmpty(category.Name))
		{
			return new OpResult<Category>
			{
				Succeeded = false,
				Errors = new Dictionary<string, string> { { "Name", "Name is required" } }
			};
		}
		if (!category.Standard.HasValue)
		{
			return new OpResult<Category>
			{
				Succeeded = false,
				Errors = new Dictionary<string, string> { { "Standard", "Standard is required" } }
			};
		}
		var baseCategory = await _context.Categories.FirstOrDefaultAsync(c => c.IsPrimary);
		float price;
		bool isPrimary = baseCategory == null;
		if (baseCategory == null)
		{
			if (category.Price == null)
			{
				return new OpResult<Category>
				{
					Succeeded = false,
					Errors = new Dictionary<string, string> { { "Price", "Price is required" } }
				};
			}
			price = category.Price.Value;
		}
		else
		{
			price = baseCategory.Price * category.Standard.Value / baseCategory.Standard;
		}
		var newCategory = new Category
		{
			Name = category.Name,
			Standard = category.Standard.Value,
			Price = price,
			IsPrimary = isPrimary
		};
		_context.Categories.Add(newCategory);
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return new OpResult<Category>
			{
				Succeeded = false,
				Errors = new Dictionary<string, string> { { "Standard", $"Category with Standard: {category.Standard} already exists" } }
			};
		}
		return new OpResult<Category> { Value = newCategory };
	}

	private async Task<OpResult<Category>> UpdateExisting(InputCategory category, int id)
	{
		var existingCategory = await _context.Categories.FindAsync(id);
		if (existingCategory == null)
		{
			return new OpResult<Category>
			{
				Succeeded = false,
				Errors = new Dictionary<string, string> { { "Id", "Category not found" } }
			};
		}
		existingCategory.Name = category.Name ?? existingCategory.Name;
		if (existingCategory.IsPrimary)
		{
			// Update all other categories if price or standard has changed
			if ((category.Price.HasValue && category.Price != existingCategory.Price) ||
			(category.Standard.HasValue && category.Standard != existingCategory.Standard))
			{
				existingCategory.Standard = category.Standard ?? existingCategory.Standard;
				existingCategory.Price = category.Price ?? existingCategory.Price;
				var categories = await _context.Categories.Where(c => !c.IsPrimary).ToListAsync();
				foreach (var c in categories)
				{
					c.Price = category.Price!.Value * c.Standard / existingCategory.Standard;
				}
			}

		}
		else
		{
			if (category.Standard.HasValue
				&& category.Standard != existingCategory.Standard)
			{
				//update price as well
				existingCategory.Standard = category.Standard.Value;
				var baseCategory = await _context.Categories.FirstAsync(c => c.IsPrimary);
				existingCategory.Price = baseCategory.Price * category.Standard.Value / baseCategory.Standard;
			}

		}
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return new OpResult<Category>
			{
				Succeeded = false,
				Errors = new Dictionary<string, string> { { "Standard", $"Category with Standard: {category.Standard} already exists" } }
			};
		}
		return new OpResult<Category> { Value = existingCategory };
	}
}
