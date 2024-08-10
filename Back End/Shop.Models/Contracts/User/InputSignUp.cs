namespace Shop.Models.Contracts.User;

public class InputSignUpUser
{
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string UserName { get; set; }
	public required string Password { get; set; }
}
