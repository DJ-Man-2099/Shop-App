using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Shop.Models.DB;

[Index(nameof(Name), nameof(CategoryId), IsUnique = true)]
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
