using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PruebaUnitariaBack.Models
{
    public class Producto
    {
        [Key] 
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad {  get; set; }  
    }
}
