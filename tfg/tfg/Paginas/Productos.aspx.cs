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
                            string valoracionEstrellas;

                            if (valoracionMedia == 0)
                            {
                                double notaCero = 5.05;
                                valoracionEstrellas = ConvertirValoracionAEstrellas(notaCero);
                            }
                            else
                            {
                                valoracionEstrellas = ConvertirValoracionAEstrellas(valoracionMedia);
                            }

                            // Obtener la ruta de la imagen del producto
                            string imagenUrl = $"{rutaImagenes}{nombre}.png";

                            // Generar un identificador único para el botón basado en el nombre del producto
                            string idBotonDeseos = $"btn-deseos-{nombre.Replace(" ", "-")}";

                            // Crear un elemento <div> con el nombre, el precio y la valoración media del producto
                            string productoHtml = $@"
                            <div class='producto'>
                                <div class='imagen-producto' onclick='mostrarDetalleProducto(""{nombre}"", ""{imagenUrl}"", {precio}, {valoracionMedia})' style='height: 250px; display: flex; justify-content: center; align-items: center;text-align:center; overflow: hidden; position: relative;'>
                                    <img src='{imagenUrl}' alt='{nombre}' style='width: auto; height: 100%; object-fit: cover;' />
                                    <div class='panel-hover'>
                                        <button class='btn-carro'>Añadir al carro</button>
                                        <button id='{idBotonDeseos}' class='btn-deseos' style='background: none; border: none; padding: 0; cursor: pointer;'>
                                            <ion-icon name='heart-outline' style='font-size: 24px; color: #f8f8f8;'></ion-icon>
                                        </button>
                                    </div>
                                </div>
                                <div class='datos-producto' style='text-align: center; padding-top:10px;'>
                                    <p class='nombre-producto' style='text-align: center; font-size: 18px;'>{nombre}</p><br/>
                                    <p>{valoracionEstrellas}</p>
                                    <p class='precio-valoracion-producto' style='font-size: 16px;'>{precio} €</p>
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
            resetearColoresFiltro();
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
                    LinkButtonNombre.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";

                    break;

                case "precioBajo":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{tipoProducto}' " +
                            $"GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
                            $"ORDER BY p.Precio ASC";
                    LinkButtonPrecioBajo.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";

                    break;

                case "precioAlto":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{tipoProducto}' " +
                            $"GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
                            $"ORDER BY p.Precio DESC";
                    LinkButtonPrecioAlto.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";

                    break;

                case "valoracion":
                    query = "SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
        "FROM producto p " +
        "LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
        "WHERE p.Tipo = @Tipo " +
        "GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
        "ORDER BY ValoracionMedia DESC";
                    LinkButtonMejorValorado.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";

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
                            // Convertir la valoración a estrellas
                            string valoracionEstrellas;

                            if (valoracionMedia == 0)
                            {
                                double notaCero = 5.05;
                                valoracionEstrellas = ConvertirValoracionAEstrellas(notaCero);
                            }
                            else
                            {
                                valoracionEstrellas = ConvertirValoracionAEstrellas(valoracionMedia);
                            }

                            // Obtener la ruta de la imagen del producto
                            string imagenUrl = $"{rutaImagenes}{nombre}.png";

                            // Generar un identificador único para el botón basado en el nombre del producto
                            string idBotonDeseos = $"btn-deseos-{nombre.Replace(" ", "-")}";

                            // Crear un elemento <div> con el nombre, el precio y la valoración media del producto
                            string productoHtml = $@"
<div class='producto'>
    <div class='imagen-producto' onclick='mostrarDetalleProducto(""{nombre}"", ""{imagenUrl}"", {precio}, {valoracionMedia})' style='height: 250px; display: flex; justify-content: center; align-items: center;text-align:center; overflow: hidden; position: relative;'>
        <img src='{imagenUrl}' alt='{nombre}' style='width: auto; height: 100%; object-fit: cover;' />
        <div class='panel-hover'>
            <button class='btn-carro'>Añadir al carro</button>
            <button id='{idBotonDeseos}' class='btn-deseos' style='background: none; border: none; padding: 0; cursor: pointer;'>
                <ion-icon name='heart-outline' style='font-size: 24px; color: #f8f8f8;'></ion-icon>
            </button>
        </div>
    </div>
    <div class='datos-producto' style='text-align: center; padding-top:10px;'>
        <p class='nombre-producto' style='text-align: center; font-size: 18px;'>{nombre}</p><br/>
        <p>{valoracionEstrellas}</p>
        <p class='precio-valoracion-producto' style='font-size: 16px;'>{precio} €</p>
    </div>
</div>";

                            // Agregar el producto generado dinámicamente al contenedor 'productosDisponibles'
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

            // Si la fracción es mayor o igual a 0.5, aproxima hacia arriba agregando una estrella completa
            if (valoracion % 2 >= 0.5)
            {
                estrellasCompletas++;
            }

            // Crea una cadena con las estrellas completas en color amarillo
            string estrellas = $"<span style='color: #ffbe61;'>{new string('★', estrellasCompletas)}</span>";

            // Agrega estrellas vacías si es necesario
            int estrellasVacias = 5 - estrellasCompletas;
            estrellas += new string('☆', estrellasVacias);

            return estrellas;
        }


        protected void AgregarAListaDeseos_Click(object sender, EventArgs e)
        {
            // Obtener el ID del cliente y el ID del producto
            int idCliente = ObtenerIdCliente();
            int idProducto = ObtenerIdProducto(sender.ToString());

            // Agregar el producto a la lista de deseos del cliente en la base de datos
            AgregarProductoAListaDeseos(idCliente, idProducto);
        }


        protected int ObtenerIdCliente()
        {
            int id_cliente = 0; // Inicializamos el ID del cliente como 0

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string query = "SELECT id_cliente FROM cliente WHERE Nombre = @nombre"; // Consulta para obtener el ID del cliente basado en el nombre

            string nombreUsuario = Session["UsuarioActual"].ToString();
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombre", nombreUsuario); // Agregar el parámetro del nombre de usuario
                conexion.Open();

                object result = comando.ExecuteScalar(); // Ejecutar la consulta y obtener el resultado

                if (result != null) // Si se encuentra el ID del cliente
                {
                    id_cliente = Convert.ToInt32(result); // Convertir el resultado a entero y asignarlo a id_cliente
                }
            }

            return id_cliente;
        }

        protected int ObtenerIdProducto(string nombreProducto)
        {
            int idProducto = 0; // Inicializamos el ID del producto como 0

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string query = "SELECT id_producto FROM producto WHERE Nombre = @nombreProducto"; // Consulta para obtener el ID del producto basado en el nombre

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@nombreProducto", nombreProducto); // Agregar el parámetro del nombre del producto
                conexion.Open();

                object result = comando.ExecuteScalar(); // Ejecutar la consulta y obtener el resultado

                if (result != null) // Si se encuentra el ID del producto
                {
                    idProducto = Convert.ToInt32(result); // Convertir el resultado a entero y asignarlo a idProducto
                }
            }

            return idProducto;
        }

        protected void AgregarProductoAListaDeseos(int idCliente, int idProducto)
{
    string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
    string query = "INSERT INTO lista_deseos_cliente (id_cliente, id_producto) VALUES (@idCliente, @idProducto)"; // Consulta para insertar un producto en la lista de deseos del cliente

    using (SqlConnection conexion = new SqlConnection(connectionString))
    {
        SqlCommand comando = new SqlCommand(query, conexion);
        comando.Parameters.AddWithValue("@idCliente", idCliente); // Agregar el parámetro del ID del cliente
        comando.Parameters.AddWithValue("@idProducto", idProducto); // Agregar el parámetro del ID del producto
        conexion.Open();

        comando.ExecuteNonQuery(); // Ejecutar la consulta para insertar el producto en la lista de deseos del cliente
    }
}


    }
}