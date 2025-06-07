using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using PruebaUnitariaBack.Interface;
using PruebaUnitariaBack.Models;

namespace PruebaUnitariaBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductoController(IProductService service)
        {
            _service=service;
        }

        [HttpPost("AgregarProducto")]
        public async Task<ActionResult<Producto>> AgregarProducto([FromBody] ProductoDTO producto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (producto == null)
            {
                return BadRequest("Debes de llenar todos los campos del producto");
            }

            var productcreated = await _service.AddProduct(producto);

            if (productcreated == null)
            {
                return BadRequest("No se ha logrado crear el producto");
            }

            return productcreated;
        }

        [HttpPut("ActualizarProducto/{id}")]
        public async Task<ActionResult<Producto>> ActualizarProducto(int id, [FromBody] ProductoDTO producto)
        {
            if (producto == null)
            {
                return BadRequest("Debes de llenar todos los campos");
            }

            var productfind = await _service.GetProduct(id);
            if (productfind == null)
            {
                return NotFound("No se ha logrado encontrar el producto que buscas");
            }

            var productmodified = await _service.UpdateProduct(productfind,producto);
            if(productmodified ==null)
            {
                return BadRequest("No se ha logrado modificar el producto");
            }

            return productmodified;
        }


        [HttpDelete("EliminarProducto/{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var product=await _service.GetProduct(id);
            if (product == null)
            {
                return NotFound("No se ha logrado encontrar el producto que buscas eliminar");
            }

            await _service.DeleteProduct(product);

            return Ok(new Dictionary<string, string> { { "Message", "Producto eliminado correctamente" } });
        }

        [HttpGet("BuscarProducto/{id}")]
        public async Task<ActionResult<Producto>> BuscarUnProducto(int id)
        {
            var product = await _service.GetProduct(id);
            if (product == null)
            {
                return NotFound("No se ha encontrado el producto que buscas");
            }

            return product;
        }
    }
}
