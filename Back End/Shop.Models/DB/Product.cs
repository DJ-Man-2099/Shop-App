using System.ComponentModel.DataAnnotations;

namespace Shop.Models.DB;

public class Product
{

	[Key]
	public int Id { get; set; }

	[Required]
	public required string Name { get; set; }

	[Required]
	public required float ManufacturePrice { get; set; }

	[Required]
	public required float WeightGrams { get; set; }
	[Required]
	public required float WeightMilliGrams { get; set; }

	[Required]
	public virtual required Group Group { get; set; }

	[Required]
	public required int GroupId { get; set; }


}
