using Shop.Models.Contracts;
using Shop.Models.DB;

namespace Shop.DataAccess.Interfaces;

public interface IGroupsService
{
	Task<OpResult<IEnumerable<GroupDTO>>> GetGroupsAsync();
	Task<OpResult<GroupDTO>> GetGroupAsync(int id);
	Task<OpResult<IEnumerable<GroupDTO>>> GetGroupsByCategoryIdAsync(int categoryId);
	Task<OpResult<GroupDTO>> UpsertGroupAsync(InputGroup Group, int? id);
	Task<OpResult> DeleteGroupAsync(int id);

}
