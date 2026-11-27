<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TechStore.Cliente.Default" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>TechStore - Productos</title>
    <link href="Content/sitio.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <h1>TechStore S.A.S</h1>
            <p>Cliente web que consume el servicio SOAP de productos</p>
        </header>
        <main>
            <asp:Label ID="lblMensaje" runat="server" EnableViewState="false" Visible="false" />

            <section>
                <h2>Consultar productos</h2>
                <div class="fila">
                    <asp:DropDownList ID="ddlCriterio" runat="server">
                        <asp:ListItem Value="categoria">Por categoría</asp:ListItem>
                        <asp:ListItem Value="id">Por Id</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtBuscar" runat="server" />
                    <asp:Button ID="btnBuscar" runat="server" Text="Consultar" OnClick="btnBuscar_Click" CausesValidation="false" />
                    <asp:Button ID="btnTodos" runat="server" Text="Ver todos" OnClick="btnTodos_Click" CausesValidation="false" CssClass="secundario" />
                </div>
                <div class="tabla">
                    <asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="false" DataKeyNames="Id"
                        OnRowCommand="gvProductos_RowCommand" EmptyDataText="No hay productos para mostrar.">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                            <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="$ {0:N0}" />
                            <asp:BoundField DataField="Stock" HeaderText="Stock" />
                            <asp:BoundField DataField="Sucursal" HeaderText="Sucursal" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id") %>' CausesValidation="false">Editar</asp:LinkButton>
                                    <asp:LinkButton runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" CssClass="peligro" OnClientClick="return confirm('¿Eliminar este producto?');">Eliminar</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </section>

            <section>
                <h2><asp:Literal ID="litTitulo" runat="server" Text="Registrar producto" /></h2>
                <asp:HiddenField ID="hfId" runat="server" />
                <div class="campos">
                    <label>Nombre<asp:TextBox ID="txtNombre" runat="server" MaxLength="100" /></label>
                    <label>Categoría<asp:TextBox ID="txtCategoria" runat="server" MaxLength="50" /></label>
                    <label>Precio<asp:TextBox ID="txtPrecio" runat="server" TextMode="Number" step="0.01" /></label>
                    <label>Stock<asp:TextBox ID="txtStock" runat="server" TextMode="Number" /></label>
                    <label>Sucursal<asp:TextBox ID="txtSucursal" runat="server" MaxLength="50" /></label>
                </div>
                <div class="fila">
                    <asp:Button ID="btnGuardar" runat="server" Text="Registrar" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CausesValidation="false" CssClass="secundario" />
                </div>
            </section>
        </main>
    </form>
</body>
</html>
