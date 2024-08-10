using System.Net;
using Shop.Models.Contracts;
using Shop.Models.DB;
using Shop.Testing.Helpers;
using Shouldly;

namespace Shop.Testing;

public class CategoriesControllerUnitTest : BaseIntegrationTest
{
	public CategoriesControllerUnitTest(BaseWebAppFactory factory) : base(factory)
	{
		ResetDatabase();
	}

	// Get Testing
	[Fact]
	public async Task GetAllCategories()
	{
		var response = await _client.GetAsync("/api/Category");

		response.EnsureSuccessStatusCode();
		response.Content.ShouldNotBeNull();

		var responseObject = await response.Content.ReadFromJsonAsync<List<Category>>();
		responseObject.ShouldBeEmpty();
	}

	[Fact]
	public async Task GetCategoryById()
	{
		var InputCategory = new InputCategory
		{
			Name = "Test Category",
			Standard = 10,
			Price = 100
		};
		var response = await _client.PostAsJsonAsync("/api/Category", InputCategory);
		response.EnsureSuccessStatusCode();

		response = await _client.GetAsync("/api/Category/1");
		response.EnsureSuccessStatusCode();
		response.Content.ShouldNotBeNull();
		response.StatusCode.ShouldBe(HttpStatusCode.OK);

		var responseObject = await response.Content.ReadFromJsonAsync<Category>();
		responseObject!.Id.ShouldBe(1);
		responseObject!.IsPrimary.ShouldBe(true);
		responseObject!.Name.ShouldBe("Test Category");
		responseObject!.Standard.ShouldBe(10);
		responseObject!.Price.ShouldBe(100);

		response = await _client.GetAsync("/api/Category");
		response.EnsureSuccessStatusCode();
		response.Content.ShouldNotBeNull();
		var responseList = await response.Content.ReadFromJsonAsync<List<Category>>();
		responseList!.Count.ShouldBe(1);
		responseList[0].ShouldBeEquivalentTo(responseObject);
	}

	[Fact]
	public async Task GetCategoryById_NotFound()
	{
		var response = await _client.GetAsync("/api/Category/1");
		response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		response.Content.ShouldNotBeNull();
		var responseObject = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
		responseObject!.ShouldContainKey("Id");
		responseObject!["Id"].ShouldBe("Category not found");
	}

	//////////////////////////////////////////////////////////////////////////////////
	// Post Requests
	[Fact]
	public async Task PostCategory()
	{
		var InputCategory = new InputCategory
		{
			Name = "Test Category",
			Standard = 10,
			Price = 100
		};

		var response = await _client.PostAsJsonAsync("/api/Category", InputCategory);
		response.EnsureSuccessStatusCode();
		response.Content.ShouldNotBeNull();

		var responseObject = await response.Content.ReadFromJsonAsync<Category>();
		var ExpectedCategory = new Category
		{
			Id = 1,
			Name = "Test Category",
			Standard = 10,
			Price = 100,
			IsPrimary = true
		};
		responseObject.ShouldBeEquivalentTo(ExpectedCategory);
	}

	[Theory]
	[InlineData(null, 10, 100f, "Name", "Name is required")]
	[InlineData("Test Category", null, 100f, "Standard", "Standard is required")]
	[InlineData("Test Category", 10, null, "Price", "Price is required")]
	public async Task PostCategory_BadRequest(string name, int? standard, float? price, string errorKey, string errorMessage)
	{
		var InputCategory = new InputCategory
		{
			Name = name,
			Standard = standard,
			Price = price
		};

		var response = await _client.PostAsJsonAsync("/api/Category", InputCategory);
		response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
		response.Content.ShouldNotBeNull();

		var responseObject = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
		responseObject!.ShouldContainKey(errorKey);
		responseObject![errorKey].ShouldBe(errorMessage);
	}

	[Theory]
	[InlineData(["Test 1", 1, 2, false])]
	[InlineData(["Test 2", 4, 8, false])]
	public async Task TestAddMultipleSecondary(string Name, int Standard, float ExpectedPrice, bool ExpectedIsPrimary)
	{

		var category = new InputCategory
		{
			Name = "Base Category",
			Standard = 5,
			Price = 10
		};

		var result = await _client.PostAsJsonAsync("/api/Category", category);

		result.EnsureSuccessStatusCode();

		category = new InputCategory
		{
			Name = Name,
			Standard = Standard,
		};

		result = await _client.PostAsJsonAsync("/api/Category", category);

		result.EnsureSuccessStatusCode();
		var returnedCategory = await result.Content.ReadFromJsonAsync<Category>();
		var expectedCategory = new Category
		{
			Id = 2,
			Name = Name,
			Standard = Standard,
			Price = ExpectedPrice,
			IsPrimary = ExpectedIsPrimary
		};
		returnedCategory.ShouldBeEquivalentTo(expectedCategory);
	}

	[Fact]
	public async Task TestForDuplicateCategoryStandard()
	{
		var category = new InputCategory
		{
			Name = "Base Category",
			Standard = 5,
			Price = 10
		};

		await _client.PostAsJsonAsync("/api/Category", category);

		category = new InputCategory
		{
			Name = "Test",
			Standard = 5,
		};

		var result = await _client.PostAsJsonAsync("/api/Category", category);

		result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
		var responseObject = await result.Content.ReadFromJsonAsync<Dictionary<string, string>>();
		responseObject.ShouldNotBeNull();
		responseObject.ShouldContainKey("Standard");
		responseObject["Standard"].ShouldBe($"Failed to add Category with Standard: {category.Standard}");

	}
	///////////////////////////////////////////////////////////////////////////////////
	// Update Requests
	[Fact]
	public async Task TestForUpdateCategories()
	{
		var category = new InputCategory
		{
			Name = "Base Category",
			Standard = 5,
			Price = 10
		};

		var result = await _client.PostAsJsonAsync("/api/Category", category);


		result.EnsureSuccessStatusCode();

		var id = (await result.Content.ReadFromJsonAsync<Category>())!.Id;


		category = new InputCategory
		{
			Name = "Test",
			Standard = 5,
		};

		result = await _client.PatchAsJsonAsync($"/api/Category/{id}", category);

		result.EnsureSuccessStatusCode();
		var returnedCategory = await result.Content.ReadFromJsonAsync<Category>();
		var expectedCategory = new Category
		{
			Id = 1,
			Name = "Test",
			Standard = 5,
			Price = 10,
			IsPrimary = true
		};
		returnedCategory.ShouldBeEquivalentTo(expectedCategory);

	}
	[Fact]
	public async Task TestForUpdateBaseCategories()
	{
		var category = new InputCategory
		{
			Name = "Base Category",
			Standard = 5,
			Price = 10
		};

		var result = await _client.PostAsJsonAsync("/api/Category", category);

		result.EnsureSuccessStatusCode();

		category = new InputCategory
		{
			Name = "Other Category",
			Standard = 2,
		};
		result = await _client.PostAsJsonAsync("/api/Category/", category);

		result.EnsureSuccessStatusCode();
		var returnedCategory = await result.Content.ReadFromJsonAsync<Category>();
		returnedCategory.ShouldNotBeNull();
		returnedCategory.Price.ShouldBe(4);

		category = new InputCategory
		{
			Standard = 10,
		};

		result = await _client.PatchAsJsonAsync($"/api/Category/{1}", category);

		result.EnsureSuccessStatusCode();
		returnedCategory = await result.Content.ReadFromJsonAsync<Category>();
		var expectedCategory = new Category
		{
			Id = 1,
			Name = "Base Category",
			Standard = 10,
			Price = 10,
			IsPrimary = true
		};
		returnedCategory.ShouldBeEquivalentTo(expectedCategory);

		result = await _client.GetAsync($"/api/Category/{2}");

		result.EnsureSuccessStatusCode();
		returnedCategory = await result.Content.ReadFromJsonAsync<Category>();
		returnedCategory.ShouldNotBeNull();
		returnedCategory.Price.ShouldBe(2);

	}
}
