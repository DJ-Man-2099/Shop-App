using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models.DB;

public class TokenBlacklist
{
	[Key]
	public required string Token { set; get; }

}
