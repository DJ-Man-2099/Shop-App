using Shop.Models.Contracts;
using Shop.Models.Contracts.Category;

namespace Shop.DataAccess.Interfaces;

public interface ICategoriesService
{
	Task<OpResult<IEnumerable<CategoryDTO>>> GetCategoriesAsync();

	Task<OpResult<CategoryDTO>> GetCategoryAsync(int id);

	Task<OpResult<CategoryDTO>> GetBaseCategoryAsync();
	Task<OpResult<CategoryDTO>> SetBaseCategoryPriceAsync(float price);

	Task<OpResult<CategoryDTO>> UpsertCategoryAsync(InputCategory category, int? id);

	Task<OpResult> DeleteCategoryAsync(int id);

	Task<OpResult<CategoryDTO>> ChangeBaseCategoryAsync(int id);

}
