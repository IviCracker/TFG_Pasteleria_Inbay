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
                productoHtml = $"<p><a href='../index.aspx'>Inicio</a> / <a href='productos.aspx'>Productos</a> / {tipoProducto}</p>";
            }
            else
            {
                productoHtml = $"<a href='../index.aspx'>Inicio</a> / <a href='productos.aspx'>Productos</a>";
            }

            subcabecera.Controls.Add(new LiteralControl(productoHtml));
        }

        protected void CargarProductos(string tipo)
        {
            string connectionString = "DataBase=tfg;DataSource=localhost;user=root;Port=3306";
            string query;

            query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, p.Imagen, AVG(vp.valoracion) AS ValoracionMedia " +
                    $"FROM producto p " +
                    $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                    $"WHERE p.Tipo = '{tipo}' " +
                    $"GROUP BY p.id_producto " +
                    $"ORDER BY p.Precio ASC";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand comando = new MySqlCommand(query, conexion);
                conexion.Open();

                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nombre = reader["Nombre"].ToString();
                        string descripcion = reader["Descripcion"].ToString();
                        decimal precio = Convert.ToDecimal(reader["Precio"]);
                        int stock = Convert.ToInt32(reader["Stock"]);
                        double valoracionMedia = reader["ValoracionMedia"] == DBNull.Value ? 0 : Convert.ToDouble(reader["ValoracionMedia"]);

                        // Convertir la valoración a estrellas
                        string valoracionEstrellas = ConvertirValoracionAEstrellas(valoracionMedia);

                        // Obtener la imagen como un array de bytes
                        byte[] imagenBytes = (byte[])reader["Imagen"];
                        string imagenBase64 = Convert.ToBase64String(imagenBytes);
                        string imagenUrl = $"data:image/jpeg;base64,{imagenBase64}";

                        // Crear un elemento <div> con la imagen, el nombre, el precio y la valoración media del producto
                        string productoHtml = $"<div class='producto'>" +
                       $"<div class='imagen-producto' style='background-color: white; width: 250px; height: 250px; display: flex; justify-content: center; align-items: center; overflow: hidden;'>" +
                       $"<img src='{imagenUrl}' alt='{nombre}' style='width: auto; height: 100%; object-fit: cover;' onclick='openModal(\"{nombre}\", \"{imagenUrl}\")' data-nombre='{nombre}' />" +
                       $"</div>" +
                       $"<div class='datos-producto' style='text-align: left; padding-top:10px;'>" +
                       $"<p class='nombre-producto' style='text-align: left;font-family: Pompiere; font-size: 18px;'>{nombre}</p>" +
                       $"<p class='precio-valoracion-producto' style='font-family: Pompiere; font-size: 16px;'>Precio: {precio} € {valoracionEstrellas}</p>" +
                       $"</div>" +
                       $"</div>";


                        productosDisponibles.Controls.Add(new LiteralControl(productoHtml));
                    }
                }
            }
            tipoProducto = tipo.ToString();
            cargarSubcabecera();
        }


        protected void CargarProductosOrden(string TipoOrden)
        {
            string connectionString = "DataBase=tfg;DataSource=localhost;user=root;Port=3306";
            string query = "";

            if (tipoProducto == null)
            {
                tipoProducto = "tarta";
            }

            resetearColoresFiltro();
            switch (TipoOrden)
            {
                case "nombre":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, p.Imagen, AVG(vp.valoracion) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{tipoProducto}' " +
                            $"GROUP BY p.id_producto " +
                            $"ORDER BY p.Nombre ASC";
                   
                    break;

                case "precioBajo":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, p.Imagen, AVG(vp.valoracion) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{tipoProducto}' " +
                            $"GROUP BY p.id_producto " +
                            $"ORDER BY p.Precio ASC";
                    
                    break;

                case "precioAlto":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, p.Imagen, AVG(vp.valoracion) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{tipoProducto}' " +
                            $"GROUP BY p.id_producto " +
                            $"ORDER BY p.Precio DESC";
                   
                    break;

                case "valoracion":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, p.Imagen, AVG(vp.valoracion) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{tipoProducto}' " +
                            $"GROUP BY p.id_producto " +
                            $"ORDER BY ValoracionMedia DESC";
                    
                    break;


                  
            }

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand comando = new MySqlCommand(query, conexion);
                conexion.Open();

                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nombre = reader["Nombre"].ToString();
                        string descripcion = reader["Descripcion"].ToString();
                        decimal precio = Convert.ToDecimal(reader["Precio"]);
                        int stock = Convert.ToInt32(reader["Stock"]);
                        double valoracionMedia = reader["ValoracionMedia"] == DBNull.Value ? 0 : Convert.ToDouble(reader["ValoracionMedia"]);

                        // Convertir la valoración a estrellas
                        string valoracionEstrellas = ConvertirValoracionAEstrellas(valoracionMedia);

                        // Obtener la imagen como un array de bytes
                        byte[] imagenBytes = (byte[])reader["Imagen"];
                        string imagenBase64 = Convert.ToBase64String(imagenBytes);
                        string imagenUrl = $"data:image/jpeg;base64,{imagenBase64}";

                        // Crear un elemento <div> con la imagen, el nombre, el precio y la valoración media del producto
                        string productoHtml = $"<div class='producto'>" +
                                               $"<img src='{imagenUrl}' alt='{nombre}' onclick='openModal(\"{nombre}\", \"{imagenUrl}\")' data-nombre='{nombre}' />" +
                                               $"<p class='nombre-producto'>{nombre}</p>" +
                                               $"<p class='precio-producto'>Precio: {precio} €</p>" +
                                               $"<p class='valoracion-media-producto'>{valoracionEstrellas}</p>" +
                                               $"</div>";

                        productosDisponibles.Controls.Add(new LiteralControl(productoHtml));
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