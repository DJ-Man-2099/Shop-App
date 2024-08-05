using System;
using Shop.Models;

namespace Shop.DataAccess.Interfaces;

public interface ICategoriesService
{
	Task<OpResult<IEnumerable<Category>>> GetCategoriesAsync();

	Task<OpResult<Category>> GetCategoryAsync(int id);

	Task<OpResult<Category>> UpsertCategoryAsync(InputCategory category, int? id);


}
