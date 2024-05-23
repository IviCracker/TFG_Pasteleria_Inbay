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
using System.Web.UI.HtmlControls;

namespace tfg.Paginas
{
    public partial class Productos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Establecer el tipo de producto en la primera carga de la página
                Session["TipoProducto"] = "Tartas";
                CargarProductos(Session["TipoProducto"].ToString());
                Session["TipoOrden"] = "nombre";

            }
            else
            {
                if(variable == 1) {
                    CargarProductos(Session["TipoProducto"].ToString());
                }
                else
                {
                    CargarProductosOrden(Session["TipoOrden"].ToString());
                    
                }

                variable = 0;
            }
        }


        static int variable = 0;


        protected void TipoProductoPanes_Click(object sender, EventArgs e)
        {
            variable = 1;
            Session["TipoProducto"] = "pan";
            ScriptManager.RegisterStartupScript(this, GetType(), "PostBackScript", "__doPostBack('', '');", true);
            

        }
        protected void TipoProductoBollos_Click(object sender, EventArgs e)
        {
            variable = 1;
            Session["TipoProducto"] = "Bollos";
            ScriptManager.RegisterStartupScript(this, GetType(), "PostBackScript", "__doPostBack('', '');", true);
            
        }
        protected void TipoProductoPasteles_Click(object sender, EventArgs e)
        {
            variable = 1;
            Session["TipoProducto"] = "pastel";
            ScriptManager.RegisterStartupScript(this, GetType(), "PostBackScript", "__doPostBack('', '');", true);
            
        }
        protected void TipoProductoTartas_Click(object sender, EventArgs e)
        {
            variable = 1;
            Session["TipoProducto"] = "Tartas";
            ScriptManager.RegisterStartupScript(this, GetType(), "PostBackScript", "__doPostBack('', '');", true);
        }

        public void OrdenarPorNombre_Click(object sender, EventArgs e)
        {
            variable = 2;
            Session["TipoOrden"] = "nombre";
            ScriptManager.RegisterStartupScript(this, GetType(), "PostBackScript", "__doPostBack('', '');", true);
        }

        public void OrdenarPorPrecioBajo_Click(object sender, EventArgs e)
        {
            variable = 2;
            Session["TipoOrden"] = "precioBajo";
            
            ScriptManager.RegisterStartupScript(this, GetType(), "PostBackScript", "__doPostBack('', '');", true);
        }

        public void OrdenarPorPrecioAlto_Click(object sender, EventArgs e)
        {
            variable = 2;
            Session["TipoOrden"] = "precioAlto";
            ScriptManager.RegisterStartupScript(this, GetType(), "PostBackScript", "__doPostBack('', '');", true);

        }

        public void OrdenarPorMejorValorado_Click(object sender, EventArgs e)
        {
            variable = 2;
            Session["TipoOrden"] = "valoracion";
            ScriptManager.RegisterStartupScript(this, GetType(), "PostBackScript", "__doPostBack('', '');", true);

        }






        protected void cargarSubcabecera()
        {
            subcabecera.Controls.Add(new LiteralControl(""));

            string productoHtml;

            if (ViewState["ProductoTipo"].ToString() != null)
            {
                productoHtml = $"<p><a href='../default.aspx'>Inicio</a> / <a href='productos.aspx'>Productos</a> / {ViewState["ProductoTipo"].ToString()}</p>";
            }
            else
            {
                productoHtml = $"<a href='../default.aspx'>Inicio</a> / <a href='productos.aspx'>Productos</a>";
            }

            subcabecera.Controls.Add(new LiteralControl(productoHtml));
        }

        protected void CargarProductos(string tipo)
        {
            // Guardar el tipo de producto en ViewState para usar en postbacks
            ViewState["ProductoTipo"] = tipo;

            // Directorio donde se almacenan las imágenes de los productos
            string rutaImagenes = "../imagenesProductos/";

            // Seleccionar los productos de acuerdo al tipo especificado
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string query = @"
        SELECT 
            p.Nombre, 
            p.Descripcion, 
            p.Precio, 
            p.Stock, 
            ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia 
        FROM 
            producto p 
        LEFT JOIN 
            valoracion_producto vp ON p.id_producto = vp.id_producto 
        WHERE 
            p.Tipo = @Tipo 
        GROUP BY 
            p.id_producto, 
            p.Nombre, 
            p.Descripcion, 
            p.Precio, 
            p.Stock 
        ORDER BY 
            p.Precio ASC";

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
                            string descripcion = reader["Descripcion"].ToString();
                            decimal precio = Convert.ToDecimal(reader["Precio"]);
                            int stock = Convert.ToInt32(reader["Stock"]);
                            double valoracion = Convert.ToDouble(reader["ValoracionMedia"]);

                            // Convertir la valoración a estrellas
                            string valoracionEstrellas;

                            if (valoracion == 0)
                            {
                                double notaCero = 5.05;
                                valoracionEstrellas = ConvertirValoracionAEstrellas(notaCero);
                            }
                            else
                            {
                                valoracionEstrellas = ConvertirValoracionAEstrellas(valoracion);
                            }

                       
                            string imagenUrl = $"{rutaImagenes}{nombre}.png";

                          
                            string idBotonCarrito = $"btn-carrito-{nombre.Replace(" ", "-")}";

                          
                           
                            ImageButton botonCarrito = new ImageButton();
                            botonCarrito.CssClass = "btn-carrito";
                            botonCarrito.ID = idBotonCarrito;
                            botonCarrito.ImageUrl = "~/imagenes/carrito.png";

                            botonCarrito.CommandArgument = nombre;
                            botonCarrito.Click += new ImageClickEventHandler(AgregarACarrito_Click);

                            //---------------------------------------
                            string idBotonDeseos = $"btn-deseos-{nombre.Replace(" ", "-")}";

                       
                            
                            ImageButton botonListaDeseos = new ImageButton();
                            botonListaDeseos.CssClass = "btn-deseos";
                            botonListaDeseos.ID = idBotonDeseos;

                            if (Session["UsuarioActual"] != null)
                            {
                                if (comprobarListaDeseados(nombre) == true)
                                {
                                    botonListaDeseos.ImageUrl = "~/imagenes/corazonLleno.png";
                                }
                                else
                                {
                                    botonListaDeseos.ImageUrl = "~/imagenes/corazonVacio.png";
                                }
                            }
                            else
                            {
                                botonListaDeseos.ImageUrl = "~/imagenes/corazonVacio.png";
                            }



                            botonListaDeseos.CommandArgument = nombre;
                            botonListaDeseos.Click += new ImageClickEventHandler(AgregarAListaDeseos_Click);






                            // Generar el HTML del producto
                            string productoHtmlInicio = $@"
                    <div class='producto'>
                        <div class='imagen-producto'  style='height: 250px; display: flex; justify-content: center; align-items: center; text-align:center; overflow: hidden; position: relative;'>
                            <img src='{imagenUrl}' alt='{nombre}' onclick='mostrarDetalleProducto(""{nombre}"", ""{imagenUrl}"", ""{precio}"", ""{valoracion}"")'style='width: auto; height: 100%; object-fit: cover;' />
                            <div class='panel-hover'>
                                
                    ";

                            string productoHtmlFin = $@"
                            </div>
                        </div>
                        <div class='datos-producto' style='text-align: center; padding-top:10px;'>
                            <p class='nombre-producto' style='text-align: center; font-size: 18px;'>{nombre}</p><br/>
                            <p>{valoracionEstrellas}</p>
                            <p class='precio-valoracion-producto' style='font-size: 16px;'>{precio} €</p>
                        </div>
                    </div>";

                            // Agregar el inicio del HTML del producto al contenedor
                            productosDisponibles.Controls.Add(new LiteralControl(productoHtmlInicio));


                            productosDisponibles.Controls.Add(botonCarrito);

                            // Agregar el botón dinámico al contenedor
                            productosDisponibles.Controls.Add(botonListaDeseos);
                           

                            // Agregar el fin del HTML del producto al contenedor
                            productosDisponibles.Controls.Add(new LiteralControl(productoHtmlFin));
                        }
                    }
                }
            }

            // Configurar la subcabecera y colores del filtro si es necesario
            ViewState["ProductoTipo"] = tipo.ToString();
            cargarSubcabecera();
            resetearColoresFiltro();
        }




        protected void CargarProductosOrden(string TipoOrden)
        {
            string rutaImagenes = "../imagenesProductos/";

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";

            string query = "";

            if (ViewState["ProductoTipo"].ToString() == null)
            {
                ViewState["ProductoTipo"] = "tarta";
            }

            resetearColoresFiltro();
            switch (TipoOrden)
            {
                case "nombre":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{ViewState["ProductoTipo"].ToString()}' " +
                            $"GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
                            $"ORDER BY p.Nombre ASC";
                    LinkButtonNombre.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";

                    break;

                case "precioBajo":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{ViewState["ProductoTipo"].ToString()}' " +
                            $"GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
                            $"ORDER BY p.Precio ASC";
                    LinkButtonPrecioBajo.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";

                    break;

                case "precioAlto":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo = '{ViewState["ProductoTipo"].ToString()}' " +
                            $"GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
                            $"ORDER BY p.Precio DESC";
                    LinkButtonPrecioAlto.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";

                    break;

                case "valoracion":
                    query = $"SELECT p.Nombre, p.Descripcion, p.Precio, p.Stock, ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia " +
                            $"FROM producto p " +
                            $"LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto " +
                            $"WHERE p.Tipo ='{ViewState["ProductoTipo"].ToString()}'" +
                            $"GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock " +
                            $"ORDER BY ValoracionMedia DESC";
                    LinkButtonMejorValorado.Attributes["style"] = "margin: 5px; padding: 8px 16px; border: 1px solid orange; border-radius: 4px; color: #333; text-decoration: none; text-align: center; transition: background-color 0.5s ease, border 0.3s ease; display: block; width: 100%; margin-bottom: 8px;";

                    break;
            }

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Tipo", ViewState["ProductoTipo"].ToString());
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombre = reader["Nombre"].ToString();
                            string descripcion = reader["Descripcion"].ToString();
                            decimal precio = Convert.ToDecimal(reader["Precio"]);
                            int stock = Convert.ToInt32(reader["Stock"]);
                            double valoracion = Convert.ToDouble(reader["ValoracionMedia"]);
                            // Convertir la valoración a estrellas
                            string valoracionEstrellas;

                            if (valoracion == 0)
                            {
                                double notaCero = 5.05;
                                valoracionEstrellas = ConvertirValoracionAEstrellas(notaCero);
                            }
                            else
                            {
                                valoracionEstrellas = ConvertirValoracionAEstrellas(valoracion);
                            }

                            // Obtener la ruta de la imagen del producto
                            string imagenUrl = $"{rutaImagenes}{nombre}.png";

                            string idBotonCarrito = $"btn-carrito-{nombre.Replace(" ", "-")}";

                            // Crear el botón dinámico
                            // Crear el botón dinámico como un botón HTML en lugar de un botón ASP.NET
                            ImageButton botonCarrito = new ImageButton();
                            botonCarrito.CssClass = "btn-carrito"; // Añadir clase CSS
                            botonCarrito.ID = idBotonCarrito;
                            botonCarrito.ImageUrl = "~/imagenes/carrito.png";

                            botonCarrito.CommandArgument = nombre;
                            botonCarrito.Click += new ImageClickEventHandler(AgregarACarrito_Click);

                            // Generar un identificador único para el botón basado en el nombre del producto
                            string idBotonDeseos = $"btn-deseos-{nombre.Replace(" ", "-")}";
                            ImageButton botonListaDeseos = new ImageButton();
                            botonListaDeseos.CssClass = "btn-deseos"; // Añadir clase CSS
                            botonListaDeseos.ID = idBotonDeseos;

                            if (Session["UsuarioActual"] != null)
                            {
                                if (comprobarListaDeseados(nombre) == true)
                                {
                                    botonListaDeseos.ImageUrl = "~/imagenes/corazonLleno.png";
                                }
                                else
                                {
                                    botonListaDeseos.ImageUrl = "~/imagenes/corazonVacio.png";
                                }
                            }
                            else
                            {
                                botonListaDeseos.ImageUrl = "~/imagenes/corazonVacio.png";
                            }



                            botonListaDeseos.CommandArgument = nombre;
                            botonListaDeseos.Click += new ImageClickEventHandler(AgregarAListaDeseos_Click);





                            // Generar el HTML del producto
                            string productoHtmlInicio = $@"
                    <div class='producto'>
                        <div class='imagen-producto'  style='height: 250px; display: flex; justify-content: center; align-items: center; text-align:center; overflow: hidden; position: relative;'>
                            <img src='{imagenUrl}' alt='{nombre}' onclick='mostrarDetalleProducto(""{nombre}"", ""{imagenUrl}"", ""{precio}"", ""{valoracion}"")'style='width: auto; height: 100%; object-fit: cover;' />
                            <div class='panel-hover'>
                                
                    ";

                            string productoHtmlFin = $@"
                            </div>
                        </div>
                        <div class='datos-producto' style='text-align: center; padding-top:10px;'>
                            <p class='nombre-producto' style='text-align: center; font-size: 18px;'>{nombre}</p><br/>
                            <p>{valoracionEstrellas}</p>
                            <p class='precio-valoracion-producto' style='font-size: 16px;'>{precio} €</p>
                        </div>
                    </div>";

                            // Agregar el inicio del HTML del producto al contenedor
                            productosDisponibles.Controls.Add(new LiteralControl(productoHtmlInicio));


                            productosDisponibles.Controls.Add(botonCarrito);

                            // Agregar el botón dinámico al contenedor
                            productosDisponibles.Controls.Add(botonListaDeseos);


                            // Agregar el fin del HTML del producto al contenedor
                            productosDisponibles.Controls.Add(new LiteralControl(productoHtmlFin));
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


        


        protected int ObtenerIdCliente()
        {
            // Verificar si la sesión del usuario está activa
            if (Session["UsuarioActual"] == null)
            {
                // Redirigir al usuario a la página de registro si no hay sesión activa
                Response.Redirect("Registro.aspx");
                return 0; // Devolver 0 para detener la ejecución del método
            }

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
        protected bool comprobarListaDeseados(string nombreProducto)
        {

            int idCliente = ObtenerIdCliente();
            int idProducto = ObtenerIdProducto(nombreProducto);

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string query = "SELECT 1 FROM lista_deseos_cliente WHERE id_cliente = @idCliente AND id_producto = @idProducto";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@idCliente", idCliente); // Agregar el parámetro del ID del cliente
                comando.Parameters.AddWithValue("@idProducto", idProducto); // Agregar el parámetro del ID del producto

                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    // Verificar si hay algún resultado
                    if (reader.Read())
                    {
                        return true; // Si hay un resultado, el producto está en la lista de deseos
                    }
                    else
                    {
                        return false; // Si no hay resultados, el producto no está en la lista de deseos
                    }
                }
            }
        }
        protected void AgregarAListaDeseos_Click(object sender, EventArgs e)
        {
            // Obtener el ID del cliente y el ID del producto
            int idCliente = ObtenerIdCliente();
            ImageButton clickedButton = (ImageButton)sender;
            string commandArgument = clickedButton.CommandArgument;

            // Convertir el CommandArgument a entero y usarlo para obtener el ID del producto
            int idProducto = ObtenerIdProducto(commandArgument);

            // Agregar el producto a la lista de deseos del cliente en la base de datos
            AgregarProductoAListaDeseos(idCliente, idProducto);
        }
        protected void AgregarACarrito_Click(object sender, EventArgs e)
        {
            // Obtener el ID del cliente y el ID del producto
            int idCliente = ObtenerIdCliente();
            ImageButton clickedButton = (ImageButton)sender;
            string commandArgument = clickedButton.CommandArgument;

            // Convertir el CommandArgument a entero y usarlo para obtener el ID del producto
            int idProducto = ObtenerIdProducto(commandArgument);

            // Agregar el producto a la lista de deseos del cliente en la base de datos
            AgregarProductoACarrito(idCliente, idProducto);
        }


        protected void AgregarProductoACarrito(int idCliente, int idProducto)
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";

            // Consulta para verificar la existencia del producto en el carrito
            string queryCheck = "SELECT cantidad FROM carrito WHERE id_cliente = @idCliente AND id_producto = @idProducto";

            // Consulta para insertar el producto si no existe
            string queryInsert = "INSERT INTO carrito (id_cliente, id_producto, cantidad) VALUES (@idCliente, @idProducto, @cantidad)";

            // Consulta para actualizar la cantidad del producto si ya existe
            string queryUpdate = "UPDATE carrito SET cantidad = cantidad + 1 WHERE id_cliente = @idCliente AND id_producto = @idProducto";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comandoCheck = new SqlCommand(queryCheck, conexion);
                comandoCheck.Parameters.AddWithValue("@idCliente", idCliente);
                comandoCheck.Parameters.AddWithValue("@idProducto", idProducto);

                conexion.Open();

                var cantidad = comandoCheck.ExecuteScalar();

                if (cantidad == null) // El producto no existe en el carrito
                {
                    SqlCommand comandoInsert = new SqlCommand(queryInsert, conexion);
                    comandoInsert.Parameters.AddWithValue("@idCliente", idCliente);
                    comandoInsert.Parameters.AddWithValue("@idProducto", idProducto);
                    comandoInsert.Parameters.AddWithValue("@cantidad", 1);

                    comandoInsert.ExecuteNonQuery(); // Insertar el producto en el carrito con cantidad = 1
                }
                else // El producto ya existe en el carrito
                {
                    SqlCommand comandoUpdate = new SqlCommand(queryUpdate, conexion);
                    comandoUpdate.Parameters.AddWithValue("@idCliente", idCliente);
                    comandoUpdate.Parameters.AddWithValue("@idProducto", idProducto);

                    comandoUpdate.ExecuteNonQuery(); // Incrementar la cantidad del producto en el carrito
                }
            }
        }

    }
}