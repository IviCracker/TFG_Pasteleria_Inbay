using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg.Paginas
{
    public partial class DetalleProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Leer los parámetros de la URL
                string nombre = Request.QueryString["nombre"];
                string imagenUrl = Request.QueryString["imagenUrl"];
                decimal precio = Convert.ToDecimal(Request.QueryString["precio"]);
                double valoracion = Convert.ToDouble(Request.QueryString["valoracion"]);

                // Buscar el control detalleContainer en la página
                var detalleContainer = this.FindControl("detalleContainer");

                // Mostrar los detalles del producto
                string detalleHtml = $@"
                <div class='producto-detalle'>
                    <h2>{nombre}</h2>
                    <img src='{imagenUrl}' alt='{nombre}'>
                    <p>Precio: {precio}</p>
                    <p>Valoración: {valoracion}</p>
                </div>";

                // Verificar que el contenedor se haya encontrado
                if (detalleContainer != null && detalleContainer is Control)
                {
                    ((Control)detalleContainer).Controls.Add(new LiteralControl(detalleHtml));
                }
            }
        }


    }
}