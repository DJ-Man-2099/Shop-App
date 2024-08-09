using System;

namespace Shop.Models;

public class UserDTO
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public int id { get; set; }
	public string token { get; set; }
}
