using System.Net;
using Shop.Models.Contracts;
using Shop.Models.Contracts.Category;
using Shop.Models.DB;
using Shop.Testing.Helpers;
using Shouldly;

namespace Shop.Testing;

public class CategoriesControllerIntegrationTest : BaseIntegrationTest
{
	public CategoriesControllerIntegrationTest(BaseWebAppFactory factory) : base(factory)
	{
		_client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AdminToken}");
	}

	// Get Testing
	[Fact]
	public async Task GetAllCategories()
	{
		var response = await _client.GetAsync("/api/Category");

		response.EnsureSuccessStatusCode();
		response.Content.ShouldNotBeNull();

		var responseObject = await response.Content.ReadFromJsonAsync<List<CategoryDTO>>();
		responseObject.ShouldBeEmpty();
	}

	[Fact]
	public async Task GetCategoryById()
	{
		var category = new InputCategory
		{
			Name = "Test Category",
			Standard = 10,
			Price = 100
		};
		var response = await _client.PostAsJsonAsync("/api/Category", category);
		response.EnsureSuccessStatusCode();

		response = await _client.GetAsync("/api/Category/1");
		response.EnsureSuccessStatusCode();
		response.Content.ShouldNotBeNull();
		response.StatusCode.ShouldBe(HttpStatusCode.OK);

		var responseObject = await response.Content.ReadFromJsonAsync<CategoryDTO>();
		responseObject!.Id.ShouldBe(1);
		responseObject!.Type.ShouldBe(CategoryDTO.Primary);
		responseObject!.Name.ShouldBe("Test Category");
		responseObject!.Standard.ShouldBe(10);
		responseObject!.Price.ShouldBe(100);

		response = await _client.GetAsync("/api/Category");
		response.EnsureSuccessStatusCode();
		response.Content.ShouldNotBeNull();
		var responseList = await response.Content.ReadFromJsonAsync<List<CategoryDTO>>();
		responseList!.Count.ShouldBe(1);
		responseList[0].ShouldBeEquivalentTo(responseObject);
	}

	[Fact]
	public async Task GetCategoryById_NotFound()
	{
		var response = await _client.GetAsync("/api/Category/1");
		response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		response.Content.ShouldNotBeNull();
		var responseObject = await response.Content.ReadAsStringAsync();
		responseObject!.ShouldBe("Category not found");
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

		var responseObject = await response.Content.ReadFromJsonAsync<CategoryDTO>();
		var ExpectedCategory = new CategoryDTO
		{
			Id = 1,
			Name = "Test Category",
			Standard = 10,
			Price = 100,
			Type = CategoryDTO.Primary
		};
		responseObject.ShouldBeEquivalentTo(ExpectedCategory);
	}

	[Theory]
	[InlineData(null, 10, 100f, "Name is required")]
	[InlineData("Test Category", null, 100f, "Standard is required")]
	[InlineData("Test Category", 10, null, "Price is required")]
	public async Task PostCategory_BadRequest(string name, int? standard, float? price, string errorMessage)
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

		var responseObject = await response.Content.ReadAsStringAsync();
		responseObject!.ShouldBe(errorMessage);
	}

	[Theory]
	[InlineData(["Test 1", 1, 2, CategoryDTO.Secondary])]
	[InlineData(["Test 2", 4, 8, CategoryDTO.Secondary])]
	public async Task TestAddMultipleSecondary(string Name, int Standard, float ExpectedPrice, string ExpectedType)
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
		var returnedCategory = await result.Content.ReadFromJsonAsync<CategoryDTO>();
		var expectedCategory = new CategoryDTO
		{
			Id = 2,
			Name = Name,
			Standard = Standard,
			Price = ExpectedPrice,
			Type = ExpectedType
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

		result.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
		var responseObject = await result.Content.ReadAsStringAsync();
		responseObject.ShouldNotBeNull();
		responseObject.ShouldBe($"Failed to add Category with Standard: {category.Standard}");

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

		var id = (await result.Content.ReadFromJsonAsync<CategoryDTO>())!.Id;


		category = new InputCategory
		{
			Name = "Test",
			Standard = 5,
		};

		result = await _client.PatchAsJsonAsync($"/api/Category/{id}", category);

		result.EnsureSuccessStatusCode();
		var returnedCategory = await result.Content.ReadFromJsonAsync<CategoryDTO>();
		var expectedCategory = new CategoryDTO
		{
			Id = 1,
			Name = "Test",
			Standard = 5,
			Price = 10,
			Type = CategoryDTO.Primary
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
		var returnedCategory = await result.Content.ReadFromJsonAsync<CategoryDTO>();
		returnedCategory.ShouldNotBeNull();
		returnedCategory.Price.ShouldBe(4);

		category = new InputCategory
		{
			Standard = 10,
		};

		result = await _client.PatchAsJsonAsync($"/api/Category/{1}", category);

		result.EnsureSuccessStatusCode();
		returnedCategory = await result.Content.ReadFromJsonAsync<CategoryDTO>();
		var expectedCategory = new CategoryDTO
		{
			Id = 1,
			Name = "Base Category",
			Standard = 10,
			Price = 10,
			Type = CategoryDTO.Primary
		};
		returnedCategory.ShouldBeEquivalentTo(expectedCategory);

		result = await _client.GetAsync($"/api/Category/{2}");

		result.EnsureSuccessStatusCode();
		returnedCategory = await result.Content.ReadFromJsonAsync<CategoryDTO>();
		returnedCategory.ShouldNotBeNull();
		returnedCategory.Price.ShouldBe(2);

	}
}
