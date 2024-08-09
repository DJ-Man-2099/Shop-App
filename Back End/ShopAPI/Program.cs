using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Authentication.Interfaces;
using Shop.Authentication.Services;
using Shop.DataAccess;
using Shop.DataAccess.Interfaces;
using Shop.DataAccess.Services;
using Shop.Models;

var builder = WebApplication.CreateBuilder(args);

void ApplyMigrations(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<SQLiteContext>();
    context.Database.Migrate();
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<ITokenService, TestTokenService>();
builder.Services.AddScoped<IUserService, SQLUserService>();
builder.Services.AddIdentity<User, IdentityRole<int>>()
.AddEntityFrameworkStores<SQLiteContext>();
builder.Services.AddAuthorization(); // Add authorization services

builder.Services.AddDbContext<SQLiteContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDatabase")
));



builder.Services.AddScoped<ICategoriesService, SQLCategoriesService>();

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
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
app.UseAuthorization();
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
