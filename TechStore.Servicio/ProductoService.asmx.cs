using System.Collections.Generic;
using System.Web.Services;
using TechStore.Servicio.Data;
using TechStore.Servicio.Models;

namespace TechStore.Servicio
{
    /// <summary>Servicio web SOAP de productos de TechStore S.A.S.</summary>
    [WebService(Namespace = "http://techstore.com/productos",
                Description = "Gestión de productos de TechStore S.A.S")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ProductoService : WebService
    {
        [WebMethod(Description = "Consulta todos los productos.")]
        public List<Producto> ConsultarProductos()
        {
            return RepositorioProductos.Listar();
        }

        [WebMethod(Description = "Consulta un producto por su Id.")]
        public Producto ConsultarProductoPorId(int id)
        {
            return RepositorioProductos.Obtener(id);
        }

        [WebMethod(Description = "Consulta los productos de una categoría.")]
        public List<Producto> BuscarPorCategoria(string categoria)
        {
            return RepositorioProductos.PorCategoria(categoria);
        }

        [WebMethod(Description = "Registra un producto nuevo.")]
        public Resultado RegistrarProducto(Producto producto)
        {
            var error = Validar(producto);
            if (error != null) return Fallo(error);

            return Ok("Producto registrado.", RepositorioProductos.Agregar(producto));
        }

        [WebMethod(Description = "Actualiza un producto existente.")]
        public Resultado ActualizarProducto(Producto producto)
        {
            var error = Validar(producto);
            if (error != null) return Fallo(error);

            var actualizado = RepositorioProductos.Actualizar(producto);
            return actualizado == null
                ? Fallo("No existe el producto " + producto.Id + ".")
                : Ok("Producto actualizado.", actualizado);
        }

        [WebMethod(Description = "Elimina un producto por su Id.")]
        public Resultado EliminarProducto(int id)
        {
            var eliminado = RepositorioProductos.Eliminar(id);
            return eliminado == null
                ? Fallo("No existe el producto " + id + ".")
                : Ok("Producto eliminado.", eliminado);
        }

        private static string Validar(Producto p)
        {
            if (p == null) return "No se recibió el producto.";
            if (string.IsNullOrWhiteSpace(p.Nombre)) return "El nombre es obligatorio.";
            if (string.IsNullOrWhiteSpace(p.Categoria)) return "La categoría es obligatoria.";
            if (p.Precio <= 0) return "El precio debe ser mayor que cero.";
            if (p.Stock < 0) return "El stock no puede ser negativo.";
            return null;
        }

        private static Resultado Ok(string mensaje, Producto producto)
        {
            return new Resultado { Exito = true, Mensaje = mensaje, Producto = producto };
        }

        private static Resultado Fallo(string mensaje)
        {
            return new Resultado { Exito = false, Mensaje = mensaje };
        }
    }
}
