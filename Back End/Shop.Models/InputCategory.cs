using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models;

public class InputCategory
{
	public string? Name { get; set; }

	public int? Standard { get; set; }

	public float? Price { get; set; }

}
