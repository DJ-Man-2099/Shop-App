using System;

namespace Shop.Models.Contracts.Group;

public class GroupDTO
{
	public required int Id { set; get; }
	public required string Name { set; get; }
	public required GroupCategoryDTO Category { set; get; }
}

public class GroupCategoryDTO
{
	public required int Id { set; get; }
	public required string Name { set; get; }
}
