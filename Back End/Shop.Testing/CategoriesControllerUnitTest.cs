using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;
using Shop.Models;
using Shouldly;

namespace Shop.Testing;

public class CategoriesControllerUnitTest : BaseIntegrationTest
{
	public CategoriesControllerUnitTest(BaseWebAppFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task GetAllCategories()
	{

		var response = await _client.GetAsync("/api/Category");

		response.EnsureSuccessStatusCode();
	}
	[Fact]
	public async Task PostCategory()
	{
		var InputCategory = new InputCategory
		{
			Name = "Test Category",
			Standard = 10,
			Price = 100
		};

		var ExpectedCategory = new Category
		{
			Id = 1,
			Name = "Test Category",
			Standard = 10,
			Price = 100,
			IsPrimary = true
		};
		var response = await _client.PostAsJsonAsync("/api/Category", InputCategory);

		response.EnsureSuccessStatusCode();

		response.Content.ShouldNotBeNull();
		// Assert.Fail(await response.Content.ReadAsStringAsync());
		var responseObject = await response.Content.ReadFromJsonAsync<Category>();
		responseObject.ShouldBeEquivalentTo(ExpectedCategory);

	}
}
