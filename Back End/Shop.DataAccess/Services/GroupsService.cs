using System;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccess.Interfaces;
using Shop.Models.Contracts;
using Shop.Models.Contracts.Group;
using Shop.Models.DB;

namespace Shop.DataAccess.Services;

public class GroupsService : IGroupsService
{
	readonly AppDBContext _context;

	public GroupsService(AppDBContext context)
	{
		_context = context;
	}

	public async Task<OpResult> DeleteGroupAsync(int id)
	{
		var group = await _context.Groups.FindAsync(id);
		if (group == null)
		{
			return OpResult.NotFound("Group not Found");
		}

		_context.Groups.Remove(group);
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return OpResult.ServerError($"Error deleting Group with Name {group.Name}");
		}

		return new OpResult();
	}

	public async Task<OpResult<GroupDTO>> GetGroupAsync(int id)
	{
		var group = await _context.Groups.Include(g => g.Category)
										.FirstOrDefaultAsync(g => g.Id == id);
		if (group == null)
		{
			return OpResult<GroupDTO>.NotFound("Group not Found");
		}
		return new OpResult<GroupDTO>
		{
			Value = GroupToGroupDTO(group)
		};
	}

	private static GroupDTO GroupToGroupDTO(Group group)
	{
		return new GroupDTO
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

	public async Task<OpResult<IEnumerable<GroupDTO>>> GetGroupsAsync()
	{
		return new OpResult<IEnumerable<GroupDTO>>
		{
			Value = await _context.Groups.Include(g => g.Category).Select(group => GroupToGroupDTO(group)).ToListAsync()
		};
	}

	public async Task<OpResult<GroupDTO>> UpsertGroupAsync(InputGroup Group, int? id)
	{
		if (id == null)
		{
			return await AddNew(Group);
		}
		else
		{
			return await UpdateExisting(Group, id.Value);
		}
	}

	private async Task<OpResult<GroupDTO>> UpdateExisting(InputGroup group, int id)
	{
		var existingGroup = await _context.Groups.Include(g => g.Category).FirstOrDefaultAsync(g => g.Id == id);
		if (existingGroup == null)
		{
			return OpResult<GroupDTO>.NotFound("Group not Found");
		}

		existingGroup.Name = group.Name ?? existingGroup.Name;
		if (group.CategoryId.HasValue)
		{
			var category = await _context.Categories.FindAsync(group.CategoryId.Value);
			if (category == null)
			{
				return OpResult<GroupDTO>.NotFound("Category not Found");
			}
			existingGroup.Category = category;
		}

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return OpResult<GroupDTO>.ServerError($"Failed to update Group with name: {group.Name}");
		}

		return new OpResult<GroupDTO>
		{
			Value = GroupToGroupDTO(existingGroup)
		};
	}

	private async Task<OpResult<GroupDTO>> AddNew(InputGroup group)
	{
		if (group.Name == null)
		{
			return OpResult<GroupDTO>.BadRequest("Name is Required");
		}
		if (!group.CategoryId.HasValue)
		{
			return OpResult<GroupDTO>.BadRequest("Category is Required");
		}
		var category = await _context.Categories.FindAsync(group.CategoryId.Value);
		if (category == null)
		{
			return OpResult<GroupDTO>.NotFound("Category not Found");
		}
		var newGroup = new Group
		{
			Name = group.Name!,
			Category = category,
		};
		_context.Groups.Add(newGroup);
		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return OpResult<GroupDTO>.ServerError($"Error Adding Group with Name {group.Name}");
		}
		return new OpResult<GroupDTO>
		{
			Value = GroupToGroupDTO(newGroup)
		};
	}

	public async Task<OpResult<IEnumerable<GroupDTO>>> GetGroupsByCategoryIdAsync(int categoryId)
	{
		var category = await _context.Categories.Include(c => c.Groups).FirstOrDefaultAsync(c => c.Id == categoryId);
		if (category == null)
		{
			return OpResult<IEnumerable<GroupDTO>>.NotFound("Category not Found");
		}

		return new OpResult<IEnumerable<GroupDTO>>
		{
			Value = [.. category.Groups!.Select(group => GroupToGroupDTO(group))]
		};
	}

}
