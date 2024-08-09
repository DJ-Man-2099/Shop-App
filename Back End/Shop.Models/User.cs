using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Shop.Models;

public class User : IdentityUser<int>
{
}
