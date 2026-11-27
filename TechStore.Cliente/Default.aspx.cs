using System;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using TechStore.Cliente.ServicioProductos;

namespace TechStore.Cliente
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) Consumir(CargarTodos);
        }

        // ---------- Consultar
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            var texto = txtBuscar.Text.Trim();
            Consumir(servicio =>
            {
                if (ddlCriterio.SelectedValue == "id")
                {
                    int id;
                    if (!int.TryParse(texto, out id)) { Avisar("Escriba un Id numérico.", false); return; }
                    var producto = servicio.ConsultarProductoPorId(id);
                    Enlazar(producto == null ? new Producto[0] : new[] { producto });
                }
                else
                {
                    Enlazar(servicio.BuscarPorCategoria(texto));
                }
            });
        }

        protected void btnTodos_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            Consumir(CargarTodos);
        }

        // ---------- Registrar y actualizar
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            decimal precio;
            int stock;
            // Los campos type=number envían el punto como separador decimal.
            if (!decimal.TryParse(txtPrecio.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out precio) ||
                !int.TryParse(txtStock.Text, out stock))
            {
                Avisar("El precio y el stock deben ser números.", false);
                return;
            }

            var producto = new Producto
            {
                Nombre = txtNombre.Text.Trim(),
                Categoria = txtCategoria.Text.Trim(),
                Precio = precio,
                Stock = stock,
                Sucursal = txtSucursal.Text.Trim()
            };

            Consumir(servicio =>
            {
                Resultado resultado;
                if (hfId.Value.Length == 0)
                {
                    resultado = servicio.RegistrarProducto(producto);
                }
                else
                {
                    producto.Id = int.Parse(hfId.Value);
                    resultado = servicio.ActualizarProducto(producto);
                }

                Avisar(resultado.Mensaje, resultado.Exito);
                if (resultado.Exito) LimpiarFormulario();
                CargarTodos(servicio);
            });
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // ---------- Editar y eliminar desde la tabla
        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Editar" && e.CommandName != "Eliminar") return;
            var id = int.Parse((string)e.CommandArgument);

            Consumir(servicio =>
            {
                if (e.CommandName == "Eliminar")
                {
                    var resultado = servicio.EliminarProducto(id);
                    Avisar(resultado.Mensaje, resultado.Exito);
                    if (hfId.Value == id.ToString()) LimpiarFormulario();
                    CargarTodos(servicio);
                    return;
                }

                var p = servicio.ConsultarProductoPorId(id);
                if (p == null) { Avisar("El producto ya no existe.", false); CargarTodos(servicio); return; }

                hfId.Value = p.Id.ToString();
                txtNombre.Text = p.Nombre;
                txtCategoria.Text = p.Categoria;
                txtPrecio.Text = p.Precio.ToString(CultureInfo.InvariantCulture);
                txtStock.Text = p.Stock.ToString();
                txtSucursal.Text = p.Sucursal;
                litTitulo.Text = "Actualizar producto " + p.Id;
                btnGuardar.Text = "Actualizar";
            });
        }

        // ---------- Apoyo
        /// <summary>Crea el proxy SOAP, ejecuta la llamada y controla los errores de comunicación.</summary>
        private void Consumir(Action<ProductoService> accion)
        {
            using (var servicio = new ProductoService())
            {
                servicio.Url = ConfigurationManager.AppSettings["UrlServicio"];
                try
                {
                    accion(servicio);
                }
                catch (WebException)
                {
                    Avisar("No fue posible conectar con el servicio SOAP. Verifique que TechStore.Servicio esté en ejecución.", false);
                }
                catch (SoapException)
                {
                    Avisar("El servicio SOAP respondió con un error.", false);
                }
            }
        }

        private void CargarTodos(ProductoService servicio)
        {
            Enlazar(servicio.ConsultarProductos());
        }

        private void Enlazar(Producto[] productos)
        {
            gvProductos.DataSource = productos;
            gvProductos.DataBind();
        }

        private void Avisar(string mensaje, bool exito)
        {
            lblMensaje.Text = Server.HtmlEncode(mensaje);
            lblMensaje.CssClass = exito ? "aviso exito" : "aviso error";
            lblMensaje.Visible = true;
        }

        private void LimpiarFormulario()
        {
            hfId.Value = "";
            txtNombre.Text = txtCategoria.Text = txtPrecio.Text = txtStock.Text = txtSucursal.Text = "";
            litTitulo.Text = "Registrar producto";
            btnGuardar.Text = "Registrar";
        }
    }
}
