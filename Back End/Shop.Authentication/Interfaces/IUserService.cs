using System;
using Shop.Models.DB;
using Shop.Models.Contracts;
using Shop.Models.Contracts.User;

namespace Shop.DataAccess.Interfaces;

public interface IUserService
{
	public Task<OpResult<UserDTO>> GetUserByIdAsync(int id);
	public Task<OpResult<IEnumerable<UserDTO>>> GetUsersAsync();
	public Task<OpResult<UserDTO>> SignUpAdminAsync(InputSignUpUser user);
	public Task<OpResult<UserDTO>> SignUpWorkerAsync(InputSignUpUser user);
	public Task<OpResult> DeleteUserById(int id);
	public Task<OpResult<UserDTO>> SignInAsync(string userName, string password);
	public Task<OpResult> UpdateUserAsync(int id, UserUpdateDTO user);

}
