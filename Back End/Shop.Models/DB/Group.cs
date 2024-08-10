using System.ComponentModel.DataAnnotations;

namespace Shop.Models.DB;

public class Group
{
	[Key]
	public int Id { get; set; }

	[Required]
	public required string Name { get; set; }

	[Required]
	public virtual required Category Category { get; set; }

	public int CategoryId { get; set; }

	public virtual ICollection<Product>? Products { get; set; }


}
