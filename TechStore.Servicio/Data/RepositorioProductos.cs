using System;
using System.Collections.Generic;
using System.Linq;
using TechStore.Servicio.Models;

namespace TechStore.Servicio.Data
{
    /// <summary>
    /// Almacenamiento en memoria. Es estático porque ASMX crea una instancia del servicio
    /// en cada petición; el lock protege la lista frente a peticiones simultáneas.
    /// </summary>
    public static class RepositorioProductos
    {
        private static readonly object Candado = new object();
        private static int _siguienteId = 6;
        private static readonly List<Producto> Productos = new List<Producto>
        {
            new Producto { Id = 1, Nombre = "Portátil Lenovo IdeaPad 3", Categoria = "Computadores", Precio = 2399000m, Stock = 12, Sucursal = "Barranquilla" },
            new Producto { Id = 2, Nombre = "Mouse Logitech M170", Categoria = "Periféricos", Precio = 45000m, Stock = 80, Sucursal = "Bogotá" },
            new Producto { Id = 3, Nombre = "Monitor Samsung 24 pulgadas", Categoria = "Monitores", Precio = 599000m, Stock = 25, Sucursal = "Medellín" },
            new Producto { Id = 4, Nombre = "Teclado mecánico Redragon", Categoria = "Periféricos", Precio = 189000m, Stock = 40, Sucursal = "Barranquilla" },
            new Producto { Id = 5, Nombre = "Disco SSD 1TB Kingston", Categoria = "Almacenamiento", Precio = 279000m, Stock = 33, Sucursal = "Cali" }
        };

        public static List<Producto> Listar()
        {
            lock (Candado) return Productos.Select(Copia).ToList();
        }

        public static Producto Obtener(int id)
        {
            lock (Candado)
            {
                var p = Productos.FirstOrDefault(x => x.Id == id);
                return p == null ? null : Copia(p);
            }
        }

        public static List<Producto> PorCategoria(string categoria)
        {
            var filtro = (categoria ?? "").Trim();
            lock (Candado)
                return Productos
                    .Where(x => x.Categoria.IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    .Select(Copia).ToList();
        }

        public static Producto Agregar(Producto producto)
        {
            lock (Candado)
            {
                var nuevo = Copia(producto);
                nuevo.Id = _siguienteId++;
                Productos.Add(nuevo);
                return Copia(nuevo);
            }
        }

        public static Producto Actualizar(Producto producto)
        {
            lock (Candado)
            {
                var indice = Productos.FindIndex(x => x.Id == producto.Id);
                if (indice < 0) return null;
                Productos[indice] = Copia(producto);
                return Copia(producto);
            }
        }

        public static Producto Eliminar(int id)
        {
            lock (Candado)
            {
                var p = Productos.FirstOrDefault(x => x.Id == id);
                if (p != null) Productos.Remove(p);
                return p;
            }
        }

        // Se entregan copias para que nadie modifique la lista por fuera del repositorio.
        private static Producto Copia(Producto p)
        {
            return new Producto
            {
                Id = p.Id, Nombre = p.Nombre, Categoria = p.Categoria,
                Precio = p.Precio, Stock = p.Stock, Sucursal = p.Sucursal
            };
        }
    }
}
