using System;

namespace Shop.Models;

public class SignUpDTO
{
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string UserName { get; set; }
	public required string Password { get; set; }
}
