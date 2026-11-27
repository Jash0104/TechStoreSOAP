namespace TechStore.Servicio.Models
{
    /// <summary>Respuesta de las operaciones de escritura.</summary>
    public class Resultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public Producto Producto { get; set; }
    }
}
