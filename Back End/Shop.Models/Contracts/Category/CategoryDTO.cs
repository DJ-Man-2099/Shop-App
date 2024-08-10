
namespace Shop.Models.Contracts.Category;
using Shop.Models.DB;

public class CategoryDTO
{
	public required int Id { set; get; }
	public required string Name { set; get; }
	public required int Standard { set; get; }
	public required float Price { set; get; }
	public required string Type { set; get; }

	public static CategoryDTO FromBaseCategory(Category category) => FromCategory(category, Primary);

	public static CategoryDTO FromOtherCategory(Category category) => FromCategory(category, Secondary);

	public static CategoryDTO FromCategory(Category category, string type) => new CategoryDTO
	{
		Id = category.Id,
		Name = category.Name,
		Standard = category.Standard,
		Price = category.Price,
		Type = type
	};

	public const string Primary = "Primary";
	public const string Secondary = "Secondary";
}
