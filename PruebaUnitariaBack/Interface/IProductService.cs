using PruebaUnitariaBack.Models;

namespace PruebaUnitariaBack.Interface
{
    public interface IProductService
    {
        Task<Producto> AddProduct(ProductoDTO producto);   
        Task<Producto> UpdateProduct(Producto producto,ProductoDTO productoDTO);
        Task<Producto> GetProduct(int id);
        Task DeleteProduct(Producto producto);
    }
}
