using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Authentication;
using Shop.Authentication.Interfaces;
using Shop.Authentication.Services;
using Shop.DataAccess.Interfaces;
using Shop.Models.Contracts;
using Shop.Models.Contracts.User;
using Shop.Models.DB;

namespace Shop.DataAccess.Services;

public class SQLUserService : AuthenticationStateProvider, IUserService
{
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly AppDBContext _context;
    private readonly DbSet<User> _users;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public SQLUserService(SignInManager<User> signInManager,
                          ITokenService tokenService,
                          AppDBContext context,
                          IHttpContextAccessor httpContextAccessor)
    {
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
        _users = _context.Users;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OpResult> DeleteUserById(int id)
    {
        var user = await _users.FindAsync(id);
        if (user == null)
        {
            return new OpResult
            {
                Succeeded = false,
                Errors = new Dictionary<string, string> { { "Id", "User Not found" } }
            };
        }
        _users.Remove(user);
        await _context.SaveChangesAsync();
        return new OpResult();
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(token))
        {
            return GetNullState();
        }

        var tokenData = _tokenService.ValidateToken(token);
        if (!tokenData.Succeeded)
        {
            return GetNullState();
        }

        var user = tokenData.Value!;

        var claims = new List<Claim>{
            new Claim(ClaimTypes.Name, user.UserName!),
        };
        var roles = await _signInManager.UserManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var claimsIdentity = new ClaimsIdentity(claims, "InAppLogin");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return new AuthenticationState(claimsPrincipal);
    }

    private static AuthenticationState GetNullState()
    {
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<OpResult<UserDTO>> GetUserByIdAsync(int id)
    {
        var user = await _users.FindAsync(id);
        if (user == null)
        {
            return new OpResult<UserDTO>
            {
                Succeeded = false,
                Errors = new Dictionary<string, string> { { "Id", "User Not found" } }
            };
        }
        var roles = await _signInManager.UserManager.GetRolesAsync(user);
        return new OpResult<UserDTO>
        {
            Value = UserDTOFromUser(user, roles)
        };
    }

    public async Task<OpResult<IEnumerable<UserDTO>>> GetUsersAsync()
    {
        var users = new List<UserDTO>();

        var result = await _users.ToListAsync();

        foreach (var user in result)
        {
            var roles = await _signInManager.UserManager.GetRolesAsync(user);
            users.Add(UserDTOFromUser(user, roles));
        }

        return new OpResult<IEnumerable<UserDTO>>
        {
            Value = users
        };
    }

    public async Task<OpResult<UserDTO>> SignInAsync(string userName, string password)
    {
        var user = await _signInManager.UserManager.FindByNameAsync(userName);
        if (user == null)
        {
            return new OpResult<UserDTO>
            {
                Succeeded = false,
                Errors = new Dictionary<string, string> { { "UserName", "User Name not Found" } }
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return new OpResult<UserDTO>
            {
                Succeeded = false,
                Errors = new Dictionary<string, string> { { "Password", "Incorrect Password" } }
            };
        }

        var roles = await _signInManager.UserManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user);

        return new OpResult<UserDTO>
        {
            Value = UserDTOFromUser(user, roles, token)
        };
    }

    private static UserDTO UserDTOFromUser(User user, IList<string> roles, string? token = null)
    {
        return new UserDTO
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            id = user.Id,
            roles = [.. roles],
            token = token
        };
    }

    private async Task<OpResult<User>> CreateNewUser(InputSignUpUser user)
    {
        var existingUser = await _signInManager.UserManager.FindByNameAsync(user.UserName);
        if (existingUser != null)
        {
            return new OpResult<User>
            {
                Succeeded = false,
                Errors = new Dictionary<string, string> { { "UserName", "User Name Already Exists" } }
            };
        }
        var currentUser = new User
        {
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };
        var result = await _signInManager.UserManager.CreateAsync(currentUser,
                                                                  password: user.Password);
        if (!result.Succeeded)
        {
            var Errors = new Dictionary<string, string>();
            foreach (var error in result.Errors)
            {
                Errors[error.Code] = error.Description;
            }
            return new OpResult<User>
            {
                Succeeded = false,
                Errors = Errors
            };
        }

        return new OpResult<User>
        {
            Value = currentUser
        };
    }

    private async Task<OpResult<UserDTO>> SignUpAsync(InputSignUpUser user, string role)
    {
        var result = await CreateNewUser(user);
        if (!result.Succeeded)
        {
            return new OpResult<UserDTO>
            {
                Succeeded = false,
                Errors = result.Errors
            };
        }

        var currentUser = result.Value!;

        await _signInManager.UserManager.AddToRoleAsync(currentUser, role: role);

        var generatedToken = _tokenService.GenerateToken(currentUser);

        return new OpResult<UserDTO>
        {
            Value = UserDTOFromUser(currentUser, [role], generatedToken)
        };
    }

    public async Task<OpResult<UserDTO>> SignUpWorkerAsync(InputSignUpUser user)
    {
        return await SignUpAsync(user, role: Roles.Worker);
    }
    public async Task<OpResult<UserDTO>> SignUpAdminAsync(InputSignUpUser user)
    {
        return await SignUpAsync(user, role: Roles.Admin);
    }

    public async Task<OpResult> UpdateUserAsync(int id, UserUpdateDTO user)
    {
        var currentUser = await _users.FindAsync(id);
        if (currentUser == null)
        {
            return new OpResult
            {
                Succeeded = false,
                Errors = new Dictionary<string, string> { { "Id", "User not Found" } }
            };
        }

        currentUser.FirstName = user.FirstName ?? currentUser.FirstName;
        currentUser.LastName = user.LastName ?? currentUser.LastName;
        currentUser.UserName = user.UserName ?? currentUser.UserName;

        if (user.Roles != null)
        {
            var currentRoles = await _signInManager.UserManager.GetRolesAsync(currentUser);

            var validRoles = user.Roles.Intersect(Roles.roles).ToList();
            if (validRoles.Count > 0)
            {
                // Remove current roles
                var removeRolesResult = await _signInManager.UserManager.RemoveFromRolesAsync(currentUser, currentRoles);
                if (!removeRolesResult.Succeeded)
                {
                    return new OpResult
                    {
                        Succeeded = false,
                        Errors = removeRolesResult.Errors.ToDictionary(e => e.Code, e => e.Description)
                    };
                }

                var addRolesResult = await _signInManager.UserManager.AddToRolesAsync(currentUser, user.Roles);
                if (!addRolesResult.Succeeded)
                {
                    return new OpResult
                    {
                        Succeeded = false,
                        Errors = addRolesResult.Errors.ToDictionary(e => e.Code, e => e.Description)
                    };
                }
            }
        }

        // Save changes to the database
        await _context.SaveChangesAsync();

        return new OpResult { Succeeded = true };
    }
}