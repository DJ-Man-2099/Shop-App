using Shop.Models.DB;

namespace Shop.Models.Contracts;

public class InputGroup
{
	public string? Name { set; get; }

	public int? CategoryId { set; get; }
}
