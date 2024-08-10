
namespace Shop.Models.Contracts.Product;
using Shop.Models.Contracts.Group;
using Shop.Models.DB;

public class ProductDTO
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required float ManufacturePrice { get; set; }
	public required float WeightGrams { get; set; }
	public required float WeightMilliGrams { get; set; }
	public virtual required GroupDTO Group { get; set; }

	public static ProductDTO FromProduct(Product product) => new ProductDTO
	{
		Id = product.Id,
		Name = product.Name,
		ManufacturePrice = product.ManufacturePrice,
		WeightGrams = product.WeightGrams,
		WeightMilliGrams = product.WeightMilliGrams,
		Group = GroupDTO.FromGroup(product.Group),
	};
}
