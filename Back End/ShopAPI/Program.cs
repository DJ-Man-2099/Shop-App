using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Shop.Authentication;
using Shop.Authentication.Interfaces;
using Shop.Authentication.Services;
using Shop.DataAccess;
using Shop.DataAccess.Interfaces;
using Shop.DataAccess.Services;
using Shop.Models.Contracts;
using Shop.Models.DB;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

void ApplyMigrations(IApplicationBuilder app)
{

    using var scope = app.ApplicationServices.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        // var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
        // context.Database.Migrate();
        var tokenContext = scope.ServiceProvider.GetRequiredService<TokenBlacklistDBContext>();
        logger.LogInformation(tokenContext.Database.GetDbConnection().ConnectionString);
        tokenContext.Database.Migrate();
        var tokenDbConnectionString = tokenContext.Database.GetDbConnection().ConnectionString;
        logger.LogInformation($"TokenBlacklistDBContext migration applied successfully. Database path: {tokenDbConnectionString}");

    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying migrations.");
    }
}

async void AddRoles(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var manager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();


    foreach (var role in Roles.roles)
    {
        var exists = await manager.RoleExistsAsync(role);
        if (!exists)
        {
            await manager.CreateAsync(new IdentityRole<int>(role));
        }
    }
}

async void AddAdmin(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var manager = scope.ServiceProvider.GetRequiredService<SignInManager<User>>();

    var admin = new User
    {
        FirstName = "David",
        LastName = "John",
        UserName = "Admin"
    };
    var existingAdmin = await manager.UserManager.FindByNameAsync("Admin");
    if (existingAdmin == null)
    {
        var result = await manager.UserManager.CreateAsync(admin, "P@ssw0rd");
        if (result.Succeeded)
        {
            await manager.UserManager.AddToRoleAsync(admin, Roles.Admin);
        }
    }

}
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
    // Add other logging providers as needed
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<RoleManager<IdentityRole<int>>>();
builder.Services.AddScoped<ITokenService, JWTTokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<BlackListTokenService>();
builder.Services.AddScoped<JwtSecurityTokenHandler>();
builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDBContext>();
builder.Services
    .AddAuthentication(
        options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(
        jwtOptions =>
        {
            var key = builder.Configuration.GetValue<string>("JwtConfig:Key")
            ?? throw new Exception("JWT key not found in configuration");
            var keyBytes = Encoding.ASCII.GetBytes(key);
            jwtOptions.SaveToken = true;
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                ValidAudience = builder.Configuration["JwtConfig:Audience"],
                ValidateIssuer = true
            };


            jwtOptions.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
                    var result = await tokenService.ValidateToken(context.SecurityToken);
                    if (!result.Succeeded)
                    {
                        context.Fail(result.Errors![OpResult.UnAuthenticatedErrorCode]);
                        return;
                    }
                    var user = result.Value!;

                    context.Principal = new ClaimsPrincipal(user);
                }
            };
        });

// Add Authentication
builder.Services.AddAuthorization();// Add authorization services

builder.Services.AddDbContext<AppDBContext>();
builder.Services.AddDbContext<TokenBlacklistDBContext>();

builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IProductsService, ProductsService>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", builder =>
    {
        builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var app = builder.Build();

// Apply migrations
ApplyMigrations(app);
// Add Roles
AddRoles(app);
// Add Default Admin User
AddAdmin(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "browser")),
    RequestPath = "",
    EnableDefaultFiles = true,
    EnableDirectoryBrowsing = false
});

app.UseRouting();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.UseCors("AllowLocalhost4200");
app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    Console.WriteLine(context.User.Identity?.AuthenticationType);
    await next.Invoke();
});

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Use(async (context, next) =>
        {
            if (!context.Request.Path.Value!.StartsWith("/api"))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "node",
                    Arguments = "wwwroot/server/main.server.mjs",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = Process.Start(psi);
                var output = await process!.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();

                if (!string.IsNullOrEmpty(error))
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(error);
                }
                else
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync(output);
                }
            }
            else
            {
                await next();
            }
        });

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
