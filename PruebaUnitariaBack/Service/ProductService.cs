using PruebaUnitariaBack.Interface;
using PruebaUnitariaBack.Models;

namespace PruebaUnitariaBack.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo=repo;
        }
        public async Task<Producto> AddProduct(ProductoDTO productoDTO)
        {
            var producto = new Producto
            {
                Nombre = productoDTO.Nombre,
                Descripcion = productoDTO.Descripcion,
                Precio = productoDTO.Precio,
                Cantidad = productoDTO.Cantidad,
            };


            var productcreated=await _repo.AddNewProduct(producto);
            await _repo.SaveChangesAsync();

            return productcreated;
            
        }

        public async Task DeleteProduct(Producto producto)
        {
          
            await _repo.DeleteOnlyProduct(producto);
            await _repo.SaveChangesAsync();
        }

        public async Task<Producto> GetProduct(int id)
        {
            return await _repo.GetOnlyProduct(id);
        }

        public async Task<Producto> UpdateProduct(Producto producto, ProductoDTO productoDTO)
        {
            producto.Nombre=productoDTO.Nombre;
            producto.Descripcion = productoDTO.Descripcion;
            producto.Precio = productoDTO.Precio;
            producto.Cantidad = productoDTO.Cantidad;

            var ProductModified=await _repo.UpdateProduct(producto);
            await _repo.SaveChangesAsync();

            return ProductModified; 
        }


    }
}
