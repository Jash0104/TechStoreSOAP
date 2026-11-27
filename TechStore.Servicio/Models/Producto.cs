namespace TechStore.Servicio.Models
{
    /// <summary>Producto tecnológico. Viaja serializado como XML dentro del mensaje SOAP.</summary>
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Sucursal { get; set; }
    }
}
