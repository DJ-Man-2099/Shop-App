using Shop.DataAccess.Services;
using Shop.Models;
using Shop.Testing.Helpers;
using Shouldly;

namespace Shop.Testing;

public class CategoriesTest : BaseUnitTest
{
	private readonly SQLCategoriesService _service;
	public CategoriesTest() : base()
	{
		_service = new SQLCategoriesService(_context);
	}

	// Get Operations
	[Fact]
	public async Task TestGetEmpty()
	{
		var categories = await _service.GetCategoriesAsync();

		categories.Succeeded.ShouldBeTrue();
		categories.Value.ShouldNotBeNull();
		categories.Value.Count().ShouldBe(0);
	}

	[Fact]
	public async Task TestForNotFoundCategory()
	{
		var result = await _service.GetCategoryAsync(1);

		result.Succeeded.ShouldBeFalse();
		result.Errors.ShouldNotBeNull();
		result.Errors.ShouldContainKey("Id");
		result.Errors["Id"].ShouldBe("Category not found");
	}

	[Fact]
	public async Task TestForGetAllCategories()
	{
		var category = new InputCategory
		{
			Name = "Base Category",
			Standard = 5,
			Price = 10
		};

		await _service.UpsertCategoryAsync(category);

		var categories = await _service.GetCategoriesAsync();

		categories.Succeeded.ShouldBeTrue();
		categories.Value.ShouldNotBeNull();
		categories.Value.Count().ShouldBe(1);

		category = new InputCategory
		{
			Name = "Test",
			Standard = 10,
		};

		await _service.UpsertCategoryAsync(category);

		categories = await _service.GetCategoriesAsync();

		categories.Succeeded.ShouldBeTrue();
		categories.Value.ShouldNotBeNull();
		categories.Value.Count().ShouldBe(2);
	}
	/////////////////////////////////////////////////////////////////////////////////////
	// Add Operations
	[Fact]
	public async Task TestAddBaseCategory()
	{
		var category = new InputCategory
		{
			Name = "Base Category",
			Standard = 5,
			Price = 10
		};

		var result = await _service.UpsertCategoryAsync(category);

		result.Succeeded.ShouldBeTrue();
		var returnedCategory = result.Value;
		var expectedCategory = new Category
		{
			Id = 1,
			Name = "Base Category",
			Standard = 5,
			Price = 10,
			IsPrimary = true
		};
		returnedCategory.ShouldBeEquivalentTo(expectedCategory);
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

		var result = await _service.UpsertCategoryAsync(category);

		result.Succeeded.ShouldBeTrue();

		category = new InputCategory
		{
			Name = Name,
			Standard = Standard,
		};

		result = await _service.UpsertCategoryAsync(category);

		result.Succeeded.ShouldBeTrue();
		var returnedCategory = result.Value;
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

	[Theory]
	[InlineData(["", 5, 10f, "Name", "Name is required"])]
	[InlineData(["Test", null, 10f, "Standard", "Standard is required"])]
	[InlineData(["Test", 5, null, "Price", "Price is required"])]
	public async Task TestForInvalidCategoriesErrors(string Name, int? Standard, float? Price, string ErrorKey, string ErrorMessage)
	{
		var category = new InputCategory
		{
			Name = Name,
			Standard = Standard,
			Price = Price
		};

		var result = await _service.UpsertCategoryAsync(category);

		result.Succeeded.ShouldBeFalse();
		result.Errors.ShouldNotBeNull();
		result.Errors.ShouldContainKey(ErrorKey);
		result.Errors[ErrorKey].ShouldBe(ErrorMessage);

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

		await _service.UpsertCategoryAsync(category);

		category = new InputCategory
		{
			Name = "Test",
			Standard = 5,
		};

		var result = await _service.UpsertCategoryAsync(category);

		result.Succeeded.ShouldBeFalse();
		result.Errors.ShouldNotBeNull();
		result.Errors.ShouldContainKey("Standard");
		result.Errors["Standard"].ShouldBe($"Category with Standard: {category.Standard} already exists");

	}
	///////////////////////////////////////////////////////////////////////////////////
	// Update Operations
	[Fact]
	public async Task TestForUpdateCategories()
	{
		var category = new InputCategory
		{
			Name = "Base Category",
			Standard = 5,
			Price = 10
		};

		var result = await _service.UpsertCategoryAsync(category);

		result.Succeeded.ShouldBeTrue();

		category = new InputCategory
		{
			Name = "Test",
			Standard = 5,
		};

		result = await _service.UpsertCategoryAsync(category, 1);

		result.Succeeded.ShouldBeTrue();
		var returnedCategory = result.Value;
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

		var result = await _service.UpsertCategoryAsync(category);

		result.Succeeded.ShouldBeTrue();
		category = new InputCategory
		{
			Name = "Other Category",
			Standard = 2,
		};

		result = await _service.UpsertCategoryAsync(category);

		result.Succeeded.ShouldBeTrue();
		result.Value.ShouldNotBeNull();
		result.Value.Price.ShouldBe(4);

		category = new InputCategory
		{
			Standard = 10,
		};

		result = await _service.UpsertCategoryAsync(category, 1);

		result.Succeeded.ShouldBeTrue();
		var returnedCategory = result.Value;
		var expectedCategory = new Category
		{
			Id = 1,
			Name = "Base Category",
			Standard = 10,
			Price = 10,
			IsPrimary = true
		};
		returnedCategory.ShouldBeEquivalentTo(expectedCategory);

		result = await _service.GetCategoryAsync(2);

		result.Succeeded.ShouldBeTrue();
		result.Value.ShouldNotBeNull();
		result.Value.Price.ShouldBe(2);

	}

}
