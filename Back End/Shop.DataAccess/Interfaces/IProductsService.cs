using System;
using Shop.Models.Contracts;
using Shop.Models.Contracts.Product;

namespace Shop.DataAccess.Interfaces;

public interface IProductsService
{
	public Task<OpResult<IEnumerable<ProductDTO>>> GetAllProductsAsync();
	public Task<OpResult<ProductDTO>> GetProductByIdAsync(int id);
	public Task<OpResult<ProductDTO>> GetProductsByGroupIdAsync(int groupId);
	public Task<OpResult<ProductDTO>> GetProductsByCategoryIdAsync(int categoryId);
	public Task<OpResult<ProductDTO>> UpsertProduct(InputProduct product, int? id);
	public Task<OpResult> DeleteProduct(int id);
}
