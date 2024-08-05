using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models;

public class Category
{
	[Key]
	public int Id { get; set; }

	[Required]
	public required string Name { get; set; }

	[Required]
	public required int Standard { get; set; }

	public required float Price { get; set; }

	[DefaultValue(false)]
	public bool IsPrimary { get; set; }

	public virtual ICollection<Group>? Groups { get; set; }


}
