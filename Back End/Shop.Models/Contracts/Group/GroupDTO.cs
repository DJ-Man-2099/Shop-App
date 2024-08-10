using System;

namespace Shop.Models.Contracts.Group;
using Shop.Models.DB;

public class GroupDTO
{
	public required int Id { set; get; }
	public required string Name { set; get; }
	public required GroupCategoryDTO Category { set; get; }

	public static GroupDTO FromGroup(Group group) => new()
	{
		Name = group.Name,
		Id = group.Id,
		Category = new GroupCategoryDTO
		{
			Id = group.CategoryId,
			Name = group.Category.Name
		}
	};
}

public class GroupCategoryDTO
{
	public required int Id { set; get; }
	public required string Name { set; get; }
}
