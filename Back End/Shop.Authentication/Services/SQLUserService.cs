using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Shop.Authentication.Interfaces;
using Shop.Authentication.Services;
using Shop.DataAccess.Interfaces;
using Shop.Models;

namespace Shop.DataAccess.Services;

public class SQLUserService : AuthenticationStateProvider, IUserService
{
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public SQLUserService(SignInManager<User> signInManager, ITokenService tokenService)
    {
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<OpResult> DeleteUserById(int id)
    {
        throw new NotImplementedException();
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<OpResult<User>> GetUserByIdAsync(int id)
    {
        Console.WriteLine("User is authorized");
        throw new NotImplementedException();
    }

    public async Task<OpResult<IEnumerable<User>>> GetUsersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<OpResult<UserDTO>> SignInAsync(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<OpResult<UserDTO>> SignUpAdminAsync(SignUpDTO user)
    {
        throw new NotImplementedException();
    }

    public async Task<OpResult<UserDTO>> SignUpWorkerAsync(SignUpDTO user)
    {
        throw new NotImplementedException();
    }
}
