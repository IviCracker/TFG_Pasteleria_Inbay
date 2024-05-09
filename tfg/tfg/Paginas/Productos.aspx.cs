using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Google.Protobuf.Reflection.ExtensionRangeOptions.Types;
using static System.Net.Mime.MediaTypeNames;
using System.Data.SqlClient;

namespace tfg.Paginas
{
    public partial class Productos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductos("Tartas");
            }
        }





        protected void TipoProductoPanes_Click(object sender, EventArgs e)
        {
            CargarProductos("pan");
        }
        protected void TipoProductoBollos_Click(object sender, EventArgs e)
        {
            CargarProductos("Bollos");
        }
        protected void TipoProductoPasteles_Click(object sender, EventArgs e)
        {
            CargarProductos("pastel");
        }
        protected void TipoProductoTartas_Click(object sender, EventArgs e)
        {
            CargarProductos("Tartas");
        }

        public void OrdenarPorNombre_Click(object sender, EventArgs e)
        {
            CargarProductosOrden("nombre");

        }

        public void OrdenarPorPrecioBajo_Click(object sender, EventArgs e)
        {
            CargarProductosOrden("precioBajo");
        }

        public void OrdenarPorPrecioAlto_Click(object sender, EventArgs e)
        {
            CargarProductosOrden("precioAlto");

        }

        public void OrdenarPorMejorValorado_Click(object sender, EventArgs e)
        {
            CargarProductosOrden("valoracion");

        }




        static string tipoProducto;

        protected void cargarSubcabecera()
        {
            subcabecera.Controls.Add(new LiteralControl(""));

            string productoHtml;

            if (tipoProducto != null)
            {
                productoHtml = $"<p><a href='../default.aspx'>Inicio</a> / <a href='productos.aspx'>Productos</a> / {tipoProducto}</p>";
            }
            else
            {
                productoHtml = $"<a href='../default.aspx'>Inicio</a> / <a href='productos.aspx'>Productos</a>";
            }

            subcabecera.Controls.Add(new LiteralControl(productoHtml));
        }

        protected void CargarProductos(string tipo)
        {
            // Directorio donde se almacenan las imágenes de los productos
            string rutaImagenes = "../imagenesProductos/";

            // Seleccionar los productos de acuerdo al tipo especificado
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string query = "SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
               "FROM producto p " +
               "LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
               "WHERE p.Tipo = @Tipo " +
               "GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
               "ORDER BY p.Precio ASC";


            // Crear la conexión y el comando SQL
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    // Establecer el tipo como parámetro para evitar la inyección de SQL
                    comando.Parameters.AddWithValue("@Tipo", tipo);

                    // Abrir la conexión
                    conexion.Open();

                    // Ejecutar el comando y leer los resultados
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombre = reader["Nombre"].ToString();
                            decimal precio = Convert.ToDecimal(reader["Precio"]);
                            double valoracionMedia = reader["ValoracionMedia"] == DBNull.Value ? 0 : Convert.ToDouble(reader["ValoracionMedia"]);

                            // Convertir la valoración a estrellas
                            string valoracionEstrellas = ConvertirValoracionAEstrellas(valoracionMedia);

                            // Obtener la ruta de la imagen del producto
                            string imagenUrl = $"{rutaImagenes}{nombre}.png";

                            // Crear un elemento <div> con el nombre, el precio y la valoración media del producto
                            string productoHtml = $@"
                <div class='producto' onclick='mostrarDetalleProducto(""{nombre}"", ""{imagenUrl}"", {precio}, {valoracionMedia})'>
                    <div class='imagen-producto' style=' width: 250px; height: 250px; display: flex; justify-content: center; align-items: center; overflow: hidden;'>
                        <img src='{imagenUrl}' alt='{nombre}' style='width: auto; height: 100%; object-fit: cover;' />
                    </div>
                    <div class='datos-producto' style='text-align: left; padding-top:10px;'>
                        <p class='nombre-producto' style='text-align: left;font-family: Pompiere; font-size: 18px;'>{nombre}</p>
                        <p class='precio-valoracion-producto' style='font-family: Pompiere; font-size: 16px;'>Precio: {precio}      {valoracionEstrellas}</p>
                    </div>
                </div>";

                            // Agregar el producto generado dinámicamente al contenedor 'productosDisponibles'
                            productosDisponibles.Controls.Add(new LiteralControl(productoHtml));
                        }
                    }
                }
            }
            tipoProducto = tipo.ToString();
            cargarSubcabecera();
        }




        protected void CargarProductosOrden(string TipoOrden)
        {
            string rutaImagenes = "../imagenesProductos/";

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";

            string query = "";

            if (tipoProducto == null)
            {
                tipoProducto = "tarta";
            }

            resetearColoresFiltro();
            switch (TipoOrden)
            {
                case "nombre":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{tipoProducto}' " +
                            $"GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
                            $"ORDER BY p.Nombre ASC";
                    break;

                case "precioBajo":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{tipoProducto}' " +
                            $"GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
                            $"ORDER BY p.Precio ASC";
                    break;

                case "precioAlto":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{tipoProducto}' " +
                            $"GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
                            $"ORDER BY p.Precio DESC";
                    break;

                case "valoracion":
                    query = "SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
        "FROM producto p " +
        "LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
        "WHERE p.Tipo = @Tipo " +
        "GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
        "ORDER BY ValoracionMedia DESC";
                    break;
            }

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Tipo", tipoProducto);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombre = reader["Nombre"].ToString();
                            string descripcion = reader["Descripcion"].ToString();
                            decimal precio = Convert.ToDecimal(reader["Precio"]);
                            int stock = Convert.ToInt32(reader["Stock"]);
                            double valoracionMedia = Convert.ToDouble(reader["ValoracionMedia"]);
                            string imagenUrl = $"{rutaImagenes}{nombre}.png";
                            // Convertir la valoración a estrellas
                            string valoracionEstrellas = ConvertirValoracionAEstrellas(valoracionMedia);

                            // Crear un elemento <div> con la imagen, el nombre, el precio y la valoración media del producto
                            string productoHtml = $@"
                <div class='producto' onclick='mostrarDetalleProducto(""{nombre}"", ""{imagenUrl}"", {precio}, {valoracionMedia})'>
                    <div class='imagen-producto' style=' width: 250px; height: 250px; display: flex; justify-content: center; align-items: center; overflow: hidden;'>
                        <img src='{imagenUrl}' alt='{nombre}' style='width: auto; height: 100%; object-fit: cover;' />
                    </div>
                    <div class='datos-producto' style='text-align: left; padding-top:10px;'>
                        <p class='nombre-producto' style='text-align: left;font-family: Pompiere; font-size: 18px;'>{nombre}</p>
                        <p class='precio-valoracion-producto' style='font-family: Pompiere; font-size: 16px;'>Precio: {precio}      {valoracionEstrellas}</p>
                    </div>
                </div>";

                            productosDisponibles.Controls.Add(new LiteralControl(productoHtml));
                        }
                    }
                }
            }

            cargarSubcabecera();
        }






        protected void resetearColoresFiltro()
        {
            // Definir los ID de los botones específicos
            string[] botonesIds = { "LinkButtonNombre", "LinkButtonPrecioBajo", "LinkButtonPrecioAlto", "LinkButtonMejorValorado" };

            // Recorrer los ID de los botones específicos
            foreach (string botonId in botonesIds)
            {
                // Buscar el control por su ID
                LinkButton boton = FindControl(botonId) as LinkButton;

                // Verificar si se encontró el botón y si tiene la clase "boton-filtro"
                if (boton != null && boton.CssClass == "boton-filtro")
                {
                    // Quitar cualquier estilo CSS existente
                    boton.Attributes["style"] = "";


                }
            }
        }

        protected string ConvertirValoracionAEstrellas(double valoracion)
        {
            // Calcula el número de estrellas completas
            int estrellasCompletas = (int)Math.Floor(valoracion / 2);

            // Calcula si hay una estrella adicional para agregar si la valoración es impar
            bool agregarEstrellaMedia = valoracion % 2 != 0;

            // Crea una cadena con las estrellas completas
            string estrellas = new string('★', estrellasCompletas);

            // Si es necesario, agrega una estrella media
            if (agregarEstrellaMedia)
            {
                estrellas += '½';
            }

            return estrellas;
        }






    }
}