using System;
using Shop.Models;

namespace Shop.DataAccess.Interfaces;

public interface IUserService
{
	public Task<OpResult<UserDTO>> GetUserByIdAsync(int id);
	public Task<OpResult<IEnumerable<UserDTO>>> GetUsersAsync();
	public Task<OpResult<UserDTO>> SignUpAdminAsync(SignUpDTO user);
	public Task<OpResult<UserDTO>> SignUpWorkerAsync(SignUpDTO user);
	public Task<OpResult> DeleteUserById(int id);
	public Task<OpResult<UserDTO>> SignInAsync(string userName, string password);
	public Task<OpResult> UpdateUserAsync(int id, UserUpdateDTO user);

}
