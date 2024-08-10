namespace Shop.Models.Contracts;

public class UserUpdateDTO
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? UserName { get; set; }
	public string[]? Roles { get; set; }
}
