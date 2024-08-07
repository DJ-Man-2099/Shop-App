using System;
using Shop.Models;

namespace Shop.DataAccess.Interfaces;

public interface ICategoriesService
{
	Task<OpResult<IEnumerable<Category>>> GetCategoriesAsync();

	Task<OpResult<Category>> GetCategoryAsync(int id);

	Task<OpResult<Category>> GetBaseCategoryAsync();
	Task<OpResult<Category>> SetBaseCategoryPriceAsync(float price);

	Task<OpResult<Category>> UpsertCategoryAsync(InputCategory category, int? id);

	Task<OpResult<object>> DeleteCategoryAsync(int id);

	Task<OpResult<Category>> ChangeBaseCategoryAsync(int id);

}
