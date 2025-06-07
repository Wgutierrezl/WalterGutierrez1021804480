using Microsoft.EntityFrameworkCore;
using PruebaUnitariaBack.Data;
using PruebaUnitariaBack.Interface;
using PruebaUnitariaBack.Models;

namespace PruebaUnitariaBack.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _contexto;

        public ProductRepository(DataContext context)
        {
            _contexto=context;
        }
        public async Task<Producto> AddNewProduct(Producto producto)
        {
            await _contexto.Producto.AddAsync(producto);
            return producto;
        }

        public async Task DeleteOnlyProduct(Producto producto)
        {
            _contexto.Producto.Remove(producto);
        }

        public async Task<Producto> GetOnlyProduct(int id)
        {
            return await _contexto.Producto.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _contexto.SaveChangesAsync();
        }

        public Task<Producto> UpdateProduct(Producto producto)
        {
            _contexto.Entry(producto).State = EntityState.Modified;
            return Task.FromResult(producto);
        }
    }
}
