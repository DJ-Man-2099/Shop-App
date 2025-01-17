using Microsoft.EntityFrameworkCore;
using Shop.DataAccess.Interfaces;
using Shop.Models.DB;
using Shop.Models.Contracts;
using Shop.Models.Contracts.Category;

namespace Shop.DataAccess.Services;

//TODO: Check All SaveChangesAsync

public class CategoriesService : ICategoriesService
{
	private readonly AppDBContext _context;

	public CategoriesService(AppDBContext context)
	{
		_context = context;
	}

	public async Task<OpResult<CategoryDTO>> GetBaseCategoryAsync()
	{
		var category = await _context.Categories.FirstOrDefaultAsync(c => c.IsPrimary);
		if (category == null)
		{
			return OpResult<CategoryDTO>.NotFound("Base Category not found");
		}
		return new OpResult<CategoryDTO> { Value = CategoryDTO.FromBaseCategory(category) };
	}

	public async Task<OpResult<CategoryDTO>> SetBaseCategoryPriceAsync(float price)
	{
		var category = await _context.Categories.FirstOrDefaultAsync(c => c.IsPrimary);
		if (category == null)
		{
			return OpResult<CategoryDTO>.NotFound("Base Category not found");
		}
		var tempCategory = new InputCategory { Price = price };
		return await UpdateExisting(tempCategory, category.Id);
	}

	public async Task<OpResult<IEnumerable<CategoryDTO>>> GetCategoriesAsync()
	{
		var categories = await _context.Categories.Select(c => CategoryDTO.FromCategory(c, c.IsPrimary ? CategoryDTO.Primary : CategoryDTO.Secondary)).ToListAsync();
		return new OpResult<IEnumerable<CategoryDTO>> { Value = categories };
	}

	public async Task<OpResult<CategoryDTO>> GetCategoryAsync(int id)
	{
		var category = await _context.Categories.FindAsync(id);
		if (category == null)
		{
			return OpResult<CategoryDTO>.NotFound("Category not found");
		}
		return new OpResult<CategoryDTO>
		{
			Value = CategoryDTO.FromCategory(category,
																	  category.IsPrimary ? CategoryDTO.Primary : CategoryDTO.Secondary)
		};
	}

	public async Task<OpResult<CategoryDTO>> UpsertCategoryAsync(InputCategory category, int? id = null)
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

	private async Task<OpResult<CategoryDTO>> AddNew(InputCategory category)
	{
		if (string.IsNullOrEmpty(category.Name))
		{
			return OpResult<CategoryDTO>.BadRequest("Name is required");
		}
		if (!category.Standard.HasValue)
		{
			return OpResult<CategoryDTO>.BadRequest("Standard is required");
		}
		var baseCategory = await _context.Categories.FirstOrDefaultAsync(c => c.IsPrimary);
		float price;
		bool isPrimary = baseCategory == null;
		if (baseCategory == null)
		{
			if (category.Price == null)
			{
				return OpResult<CategoryDTO>.BadRequest("Price is required");
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
			return OpResult<CategoryDTO>.ServerError($"Failed to add Category with Standard: {category.Standard}");
		}
		return new OpResult<CategoryDTO> { Value = CategoryDTO.FromCategory(newCategory, newCategory.IsPrimary ? CategoryDTO.Primary : CategoryDTO.Secondary) };
	}

	private async Task<OpResult<CategoryDTO>> UpdateExisting(InputCategory category, int id)
	{
		var existingCategory = await _context.Categories.FindAsync(id);
		if (existingCategory == null)
		{
			return OpResult<CategoryDTO>.NotFound("Category not found");
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
					c.Price = existingCategory.Price * c.Standard / existingCategory.Standard;
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
			return OpResult<CategoryDTO>.ServerError($"Failed to update Category with Standard: {category.Standard}");
		}
		return new OpResult<CategoryDTO> { Value = CategoryDTO.FromCategory(existingCategory, existingCategory.IsPrimary ? CategoryDTO.Primary : CategoryDTO.Secondary) };
	}

	public async Task<OpResult> DeleteCategoryAsync(int id)
	{
		var existingCategory = await _context.Categories.FindAsync(id);
		if (existingCategory == null)
		{
			return OpResult.NotFound("Category not found");
		}
		_context.Categories.Remove(existingCategory);
		await _context.SaveChangesAsync();
		return new OpResult();
	}

	public async Task<OpResult<CategoryDTO>> ChangeBaseCategoryAsync(int id)
	{
		var existingCategory = await _context.Categories.FindAsync(id);
		if (existingCategory == null)
		{
			return OpResult<CategoryDTO>.NotFound("Category not found");
		}
		var baseCategory = await _context.Categories.FirstAsync(c => c.IsPrimary);
		baseCategory.IsPrimary = false;
		existingCategory.IsPrimary = true;
		await _context.SaveChangesAsync();

		return new OpResult<CategoryDTO> { Value = CategoryDTO.FromCategory(existingCategory, existingCategory.IsPrimary ? CategoryDTO.Primary : CategoryDTO.Secondary) };
	}
}
