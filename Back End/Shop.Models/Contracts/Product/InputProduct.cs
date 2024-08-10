using System;

namespace Shop.Models.Contracts.Product;

public class InputProduct
{
	public string? Name { get; set; }
	public float? ManufacturePrice { get; set; }
	public float? WeightGrams { get; set; }
	public float? WeightMilliGrams { get; set; }
	public int? GroupId { get; set; }
}
