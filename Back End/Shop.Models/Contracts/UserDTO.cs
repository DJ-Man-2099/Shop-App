namespace Shop.Models.Contracts;

public class UserDTO
{
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public int id { get; set; }
	public string? token { get; set; }
	public required string[] roles { get; set; }
}
