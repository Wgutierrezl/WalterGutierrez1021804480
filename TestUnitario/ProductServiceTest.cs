using Moq;
using PruebaUnitariaBack.Interface;
using PruebaUnitariaBack.Models;
using PruebaUnitariaBack.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnitario
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProductService _service;

        public ProductServiceTest()
        {
            _mockRepository = new Mock<IProductRepository>();
            _service = new ProductService(_mockRepository.Object);
        }



        //Prueba de POST, agregar un producto correctamente
        [Fact]
        public async Task AddProduct_RetornaProducto_SiEsExitoso()
        {
            // Arrange
            var dto = new ProductoDTO
            {
                Nombre = "Test",
                Descripcion = "Desc",
                Precio = 10,
                Cantidad = 5
            };

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Cantidad = dto.Cantidad
            };

            _mockRepository.Setup(r => r.AddNewProduct(It.IsAny<Producto>()))
                     .ReturnsAsync(producto);

            // Act
            var result = await _service.AddProduct(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Nombre);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        //Prueba de POST, caso erroneo, lanzar excepcion si falla el repositorio
        [Fact]
        public async Task AddProduct_LanzaExcepcion_SiRepositorioFalla()
        {
            // Arrange
            var dto = new ProductoDTO { Nombre = "Test" };
            _mockRepository.Setup(r => r.AddNewProduct(It.IsAny<Producto>()))
                          .ThrowsAsync(new Exception("Error en repositorio"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.AddProduct(dto));
        }

        //Prueba GET, retornar un producto de manera correcta
        [Fact]
        public async Task GetProduct_RetornaProducto_SiExiste()
        {
            // Arrange
            var producto = new Producto { Id = 1, Nombre = "ProductoTest" };
            _mockRepository.Setup(r => r.GetOnlyProduct(1)).ReturnsAsync(producto);

            // Act
            var result = await _service.GetProduct(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ProductoTest", result.Nombre);
        }

        //Prueba GET caso erroneo, retornar un null si no existe
        [Fact]
        public async Task GetProduct_RetornaNull_SiNoExiste()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetOnlyProduct(1)).ReturnsAsync((Producto)null);

            // Act
            var result = await _service.GetProduct(1);

            // Assert
            Assert.Null(result);
        }

        //Prueba DELETE, caso correcto, elimina un producto correctamente
        [Fact]
        public async Task DeleteProduct_EjecutaEliminacionCorrecta()
        {
            // Arrange
            var producto = new Producto { Id = 1 };

            // Act
            await _service.DeleteProduct(producto);

            // Assert
            _mockRepository.Verify(r => r.DeleteOnlyProduct(producto), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        //Prueba UPDATE, caso correcto, actualizar un producto correctamente

        [Fact]
        public async Task UpdateProduct_ModificaCorrectamente()
        {
            // Arrange
            var producto = new Producto
            {
                Id = 1,
                Nombre = "Original",
                Descripcion = "Old",
                Precio = 10,
                Cantidad = 1
            };

            var dto = new ProductoDTO
            {
                Nombre = "Nuevo",
                Descripcion = "NewDesc",
                Precio = 20,
                Cantidad = 3
            };

            var productoModificado = new Producto
            {
                Id = 1,
                Nombre = "Nuevo",
                Descripcion = "NewDesc",
                Precio = 20,
                Cantidad = 3
            };

            _mockRepository.Setup(r => r.UpdateProduct(It.IsAny<Producto>()))
                           .ReturnsAsync(productoModificado);

            // Act
            var result = await _service.UpdateProduct(producto, dto);

            // Assert
            Assert.Equal("Nuevo", result.Nombre);
            Assert.Equal("NewDesc", result.Descripcion);
            Assert.Equal(20, result.Precio);
            Assert.Equal(3, result.Cantidad);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        //PRUEBA UPDATE, caso erroneo, lanza una excepcion si falla la peticion

        [Fact]
        public async Task UpdateProduct_LanzaExcepcion_SiFallaElRepositorio()
        {
            // Arrange
            var producto = new Producto { Id = 1 };
            var dto = new ProductoDTO { Nombre = "Error" };

            _mockRepository.Setup(r => r.UpdateProduct(It.IsAny<Producto>()))
                           .ThrowsAsync(new Exception("Fallo"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateProduct(producto, dto));
        }

        //prueba Delete, Lanza excepcion si falla la consulta
        [Fact]
        public async Task DeleteProduct_LanzaExcepcion_SiFallaElRepositorio()
        {
            // Arrange
            var producto = new Producto { Id = 1 };

            _mockRepository.Setup(r => r.DeleteOnlyProduct(It.IsAny<Producto>()))
                           .ThrowsAsync(new Exception("Fallo"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.DeleteProduct(producto));
        }

    }
}
