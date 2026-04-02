using Product.Models;

namespace Product.Interfaces
{
    public interface IProduct
    {
        Task<IEnumerable<Models.Product>> GetAllProductsAsync();
        Task<Models.Product?> GetProductByIdAsync(int id);
        Task<Models.Product> AddProductAsync(Models.Product product);
        Task<Models.Product?> UpdateProductAsync(int id, Models.Product product);
        Task<bool> DeleteProductAsync(int id);
    }
}
