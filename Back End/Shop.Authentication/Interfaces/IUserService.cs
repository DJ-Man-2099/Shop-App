using System;
using Shop.Models;

namespace Shop.DataAccess.Interfaces;

public interface IUserService
{
	public Task<OpResult<User>> GetUserByIdAsync(int id);
	public Task<OpResult<IEnumerable<User>>> GetUsersAsync();
	public Task<OpResult<UserDTO>> SignUpAdminAsync(SignUpDTO user);
	public Task<OpResult<UserDTO>> SignUpWorkerAsync(SignUpDTO user);
	public Task<OpResult> DeleteUserById(int id);
	public Task<OpResult<UserDTO>> SignInAsync(string userName, string password);

}
