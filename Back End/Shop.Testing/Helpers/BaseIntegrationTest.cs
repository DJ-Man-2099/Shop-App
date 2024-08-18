using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Authentication;
using Shop.Authentication.Interfaces;
using Shop.DataAccess;
using Shop.Models.DB;

namespace Shop.Testing.Helpers;

public class BaseIntegrationTest : IClassFixture<BaseWebAppFactory>
{
	protected readonly BaseWebAppFactory _factory;
	protected readonly HttpClient _client;

	protected string AdminToken { get; set; } = string.Empty;
	protected string WorkerToken { get; set; } = string.Empty;


	public BaseIntegrationTest(BaseWebAppFactory factory)
	{
		_factory = factory;
		_client = _factory.CreateClient();
		ResetDatabase().GetAwaiter().GetResult();
	}

	protected async Task ResetDatabase()
	{
		using var scope = _factory.Services.CreateScope();
		// throw new("Resetting database");
		var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
		db.Database.EnsureDeleted();
		db.Database.Migrate();

		var manager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
		foreach (var role in Roles.roles)
		{
			var exists = await manager.RoleExistsAsync(role);
			if (!exists)
			{
				await manager.CreateAsync(new IdentityRole<int>(role));
			}
		}

		var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<User>>();
		var testAdmin = new User
		{
			UserName = "TestAdmin",
			FirstName = "Test",
			LastName = "Test",
		};
		await signInManager.UserManager.CreateAsync(testAdmin, "P@ssw0rd");
		await signInManager.UserManager.AddToRoleAsync(testAdmin, Roles.Admin);
		var testWorker = new User
		{
			UserName = "TestAdmin",
			FirstName = "Test",
			LastName = "Test",
		};
		await signInManager.UserManager.CreateAsync(testWorker, "P@ssw0rd");
		await signInManager.UserManager.AddToRoleAsync(testWorker, Roles.Worker);

		var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
		AdminToken = tokenService.GenerateToken(testAdmin);
		WorkerToken = tokenService.GenerateToken(testWorker);
	}
}
