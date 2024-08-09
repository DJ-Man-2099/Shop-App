using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Shop.Models;

public class User : IdentityUser<int>
{
	[Required]
	public string FirstName { set; get; }
	[Required]
	public string LastName { set; get; }
}
