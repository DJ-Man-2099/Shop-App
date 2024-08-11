using System.ComponentModel.DataAnnotations;

namespace Shop.Models.Contracts.User;

public class InputSignIn
{
	[Required]
	public required string UserName { get; set; }
	[Required]
	public required string Password { get; set; }
}
