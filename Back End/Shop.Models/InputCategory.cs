using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models;

public class InputCategory
{
	[Required]
	public required string Name { get; set; }

	[Required]
	public required int Standard { get; set; }

	public float? Price { get; set; }

}
