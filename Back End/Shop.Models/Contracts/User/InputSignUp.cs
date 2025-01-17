using System.ComponentModel.DataAnnotations;

namespace Shop.Models.Contracts.User;

public class InputSignUp
{
	[Required]
	public required string FirstName { get; set; }
	[Required]
	public required string LastName { get; set; }
	[Required]
	public required string UserName { get; set; }
	[Required]
	public required string Password { get; set; }
}
