namespace Shop.Models.Contracts.User;
using Shop.Models.DB;
public class UserDTO
{
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string UserName { get; set; }
	public int id { get; set; }
	public string? token { get; set; }
	public required string[] roles { get; set; }

	public static UserDTO FromUser(User user, IList<string> roles, string? token = null)
	{
		return new UserDTO
		{
			FirstName = user.FirstName,
			LastName = user.LastName,
			UserName = user.UserName!,
			id = user.Id,
			roles = [.. roles],
			token = token
		};
	}
}
