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
                cargarSubcabecera();
            }
        }





        protected void TipoProductoPanes_Click(object sender, EventArgs e)
        {
            CargarProductos("pan");
        }
        protected void TipoProductoBollos_Click(object sender, EventArgs e)
        {
            CargarProductos("bollo");
        }
        protected void TipoProductoPasteles_Click(object sender, EventArgs e)
        {
            CargarProductos("pastel");
        }
        protected void TipoProductoTartas_Click(object sender, EventArgs e)
        {
            CargarProductos("tarta");
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



        string nombre;
        string descripcion;
        decimal precio;
        int stock;
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





            query = $"SELECT Nombre, Descripcion, Precio, Stock, Imagen FROM producto WHERE Tipo = '{tipo}' ORDER BY Precio ASC";

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

                        // Obtener la imagen como un array de bytes
                        byte[] imagenBytes = (byte[])reader["Imagen"];
                        string imagenBase64 = Convert.ToBase64String(imagenBytes);
                        string imagenUrl = $"data:image/jpeg;base64,{imagenBase64}";

                        // Crear un elemento <div> con la imagen y el nombre del producto
                        string productoHtml = $"<div class='producto'>" +
                                               $"<img src='{imagenUrl}' alt='{nombre}' onclick='openModal(\"{nombre}\", \"{imagenUrl}\")' data-nombre='{nombre}' />" +
                                               $"<p class='nombre-producto'>{nombre}</p>" +
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
                    query = $"SELECT Nombre, Descripcion, Precio, Stock, Imagen FROM producto WHERE Tipo = '{tipoProducto}' ORDER BY Nombre ASC";

                    LinkButtonNombre.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";
                    break;

                case "precioBajo":
                    query = $"SELECT Nombre, Descripcion, Precio, Stock, Imagen FROM producto WHERE Tipo = '{tipoProducto}' ORDER BY Precio ASC";
                    LinkButtonPrecioBajo.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";
                    break;

                case "precioAlto":
                    query = $"SELECT Nombre, Descripcion, Precio, Stock, Imagen FROM producto WHERE Tipo = '{tipoProducto}' ORDER BY Precio DESC";
                    LinkButtonPrecioAlto.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";

                    break;

                case "valoracion":
                    //query = $"SELECT Nombre, Descripcion, Precio, Stock, Imagen FROM producto WHERE Tipo = '{tipoProducto}' ORDER BY Precio DESC";
                    //LinkButtonMejorValorado.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";
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

                        // Obtener la imagen como un array de bytes
                        byte[] imagenBytes = (byte[])reader["Imagen"];
                        string imagenBase64 = Convert.ToBase64String(imagenBytes);
                        string imagenUrl = $"data:image/jpeg;base64,{imagenBase64}";

                        // Crear un elemento <div> con la imagen y el nombre del producto
                        string productoHtml = $"<div class='producto'>" +
                                               $"<img src='{imagenUrl}' alt='{nombre}' onclick='openModal(\"{nombre}\", \"{imagenUrl}\")' data-nombre='{nombre}' />" +
                                               $"<p class='nombre-producto'>{nombre}</p>" +
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







    }
}