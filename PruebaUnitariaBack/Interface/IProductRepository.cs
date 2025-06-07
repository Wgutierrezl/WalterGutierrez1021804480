using PruebaUnitariaBack.Models;

namespace PruebaUnitariaBack.Interface
{
    public interface IProductRepository
    {
        Task<Producto> AddNewProduct(Producto producto);
        Task<Producto> UpdateProduct(Producto producto);
        Task<Producto> GetOnlyProduct(int id);
        Task DeleteOnlyProduct(Producto producto);
        Task SaveChangesAsync();

    }
}
