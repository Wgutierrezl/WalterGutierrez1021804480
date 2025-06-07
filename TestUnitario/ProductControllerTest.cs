using Microsoft.AspNetCore.Mvc;
using Moq;
using PruebaUnitariaBack.Controllers;
using PruebaUnitariaBack.Interface;
using PruebaUnitariaBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestUnitario
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductoController _controller;

        public ProductControllerTest()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductoController(_mockService.Object);
        }


        //PRUEBA POST, AGREGAR UN PRODUCTO CASO CORRECTO
        [Fact]
        public async Task AgregarProducto_DevuelveProducto_SiEsExitoso()
        {
            // Arrange
            var dto = new ProductoDTO { Nombre = "Producto1", Descripcion = "Test", Precio = 10, Cantidad = 2 };
            var producto = new Producto { Nombre = dto.Nombre, Descripcion = dto.Descripcion, Precio = dto.Precio, Cantidad = dto.Cantidad };

            _mockService.Setup(s => s.AddProduct(dto)).ReturnsAsync(producto);

            // Act
            var result = await _controller.AgregarProducto(dto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Producto>>(result);
            var returnValue = Assert.IsType<Producto>(actionResult.Value);
            Assert.Equal("Producto1", returnValue.Nombre);
        }



        //PRUEBA POST, CASO ERRONEO, DEVUELVE UN BADREQUEST
        [Fact]
        public async Task AgregarProducto_DevuelveBadRequest_SiProductoEsNulo()
        {
            // Act
            var result = await _controller.AgregarProducto(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Debes de llenar todos los campos del producto", badRequest.Value);
        }

        [Fact]
        public async Task AgregarProducto_DevuelveBadRequest_SiPrecioOCantidadSonInvalidos()
        {
            // Arrange
            var dto = new ProductoDTO
            {
                Nombre = "Test",
                Descripcion = "Desc",
                Precio = -10, // inválido
                Cantidad = -1 // inválido
            };

            // Forzar la validación del modelo
            _controller.ModelState.AddModelError("Precio", "El precio debe ser mayor a cero");
            _controller.ModelState.AddModelError("Cantidad", "La cantidad debe ser al menos 1");

            // Act
            var result = await _controller.AgregarProducto(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            var error = Assert.IsType<SerializableError>(badRequest.Value);
            Assert.True(error.ContainsKey("Precio"));
            Assert.True(error.ContainsKey("Cantidad"));
        }


        //PRUEBA GET, CASO CORRECTO, DEVUELVE EL VALOR NOMBRE DEL PRODUCTO
        [Fact]
        public async Task BuscarUnProducto_DevuelveProducto_SiExiste()
        {
            // Arrange
            var producto = new Producto { Id = 1, Nombre = "Producto1" };
            _mockService.Setup(s => s.GetProduct(1)).ReturnsAsync(producto);

            // Act
            var result = await _controller.BuscarUnProducto(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Producto>>(result);
            var returnValue = Assert.IsType<Producto>(actionResult.Value);
            Assert.Equal("Producto1", returnValue.Nombre);
        }


        //PRUEBA GET, CASO ERRONEO, DEVUELVE NOT FOUND
        [Fact]
        public async Task BuscarUnProducto_DevuelveNotFound_SiNoExiste()
        {
            // Arrange
            _mockService.Setup(s => s.GetProduct(99)).ReturnsAsync((Producto)null);

            // Act
            var result = await _controller.BuscarUnProducto(99);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No se ha encontrado el producto que buscas", notFound.Value);
        }


        //PRUEBA UPDATE, CASO CORRECTO, ACTUALIZA CORRECTAMENTE UN PRODUCTO
        [Fact]
        public async Task ActualizarProducto_DevuelveProductoActualizado_SiEsExitoso()
        {
            // Arrange
            var dto = new ProductoDTO { Nombre = "Nuevo", Descripcion = "Actualizado", Precio = 20, Cantidad = 3 };
            var producto = new Producto { Id = 1, Nombre = "Viejo", Descripcion = "Antiguo", Precio = 10, Cantidad = 1 };

            _mockService.Setup(s => s.GetProduct(1)).ReturnsAsync(producto);
            _mockService.Setup(s => s.UpdateProduct(producto, dto)).ReturnsAsync(new Producto
            {
                Id = 1,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Cantidad = dto.Cantidad
            });

            // Act
            var result = await _controller.ActualizarProducto(1, dto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Producto>>(result);
            var updatedProduct = Assert.IsType<Producto>(actionResult.Value);
            Assert.Equal("Nuevo", updatedProduct.Nombre);
        }


        //PRUEBA UPDATE, CASO ERRONEO, DEVUELVE UN NOT FOUNT 
        [Fact]
        public async Task ActualizarProducto_DevuelveNotFound_SiProductoNoExiste()
        {
            // Arrange
            var dto = new ProductoDTO { Nombre = "X", Descripcion = "X", Precio = 0, Cantidad = 0 };
            _mockService.Setup(s => s.GetProduct(1)).ReturnsAsync((Producto)null);

            // Act
            var result = await _controller.ActualizarProducto(1, dto);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No se ha logrado encontrar el producto que buscas", notFound.Value);
        }


        //PRUEBA DELETE, CASO CORRECTO, ELIMINA CORRECTAMENTE UN PRODUCTO
        [Fact]
        public async Task EliminarProducto_DevuelveOk_SiProductoExiste()
        {
            // Arrange
            var producto = new Producto { Id = 1, Nombre = "ProductoEliminar" };
            _mockService.Setup(s => s.GetProduct(1)).ReturnsAsync(producto);

            // Act
            var result = await _controller.EliminarProducto(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<Dictionary<string, string>>(okResult.Value);
            Assert.Equal("Producto eliminado correctamente", message["Message"]);
        }


        //PRUEBA DELETE, CASO ERRONEO, DEVUELVE UN NOT FOUND
        [Fact]
        public async Task EliminarProducto_DevuelveNotFound_SiProductoNoExiste()
        {
            // Arrange
            _mockService.Setup(s => s.GetProduct(999)).ReturnsAsync((Producto)null);

            // Act
             var result = await _controller.EliminarProducto(999);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No se ha logrado encontrar el producto que buscas eliminar", notFound.Value);
        }

        //Primer cambio, agregar un nuevo test, Controlador vacio
        [Fact]
        public void ProductoController_ExisteInstancia_DeberiaSerVerdadero()
        {
            // Assert
            //Prueba de nuevo
            Assert.NotNull(_controller);
        }
    }
}
