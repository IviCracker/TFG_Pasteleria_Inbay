using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg.Paginas
{
    public partial class listaDeseos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductos();
                CargarProductosCarrito();
                ObtenerPrecioTotal();
            }
        }
        protected void searchButton_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Redirigir a la página de resultados de búsqueda con la consulta de búsqueda como un parámetro de consulta
                Response.Redirect($"ResultadosBusqueda.aspx?query={Server.UrlEncode(searchQuery)}");
            }
        }

        protected int comprobarCarrito(int idCliente)
        {
            int totalRegistros = 0;

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string query = "SELECT COUNT(*) AS TotalRegistros FROM carrito WHERE id_cliente = @id_cliente";


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_cliente", idCliente);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        // Si el resultado no es nulo, conviértelo a int
                        if (result != null)
                        {
                            totalRegistros = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
                Console.WriteLine("Error: " + ex.Message);
            }

            return totalRegistros;
        }
        protected void CargarProductos()
        {
            string rutaImagenes = "../imagenesProductos/";

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";

            int id_cliente = ObtenerIdCliente();
            if (id_cliente == 0)
            {
                return; // Detener la ejecución si no se obtiene un id_cliente válido
            }

            string query = @"
                SELECT p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock,
                ISNULL(AVG(vp.valoracion), 0) AS ValoracionMedia
                FROM lista_deseos_cliente ldc
                JOIN producto p ON ldc.id_producto = p.id_producto
                LEFT JOIN valoracion_producto vp ON p.id_producto = vp.id_producto
                WHERE ldc.id_cliente = @id_cliente
                GROUP BY p.id_producto, p.Nombre, p.Descripcion, p.Precio, p.Stock";

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@id_cliente", id_cliente); // Agregar el parámetro del id_cliente
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        HashSet<int> productosProcesados = new HashSet<int>();

                        while (reader.Read())
                        {
                            int idProducto = Convert.ToInt32(reader["id_producto"]);
                            if (productosProcesados.Contains(idProducto))
                            {
                                continue; // Si el producto ya ha sido procesado, saltar a la siguiente iteración
                            }

                            productosProcesados.Add(idProducto);


                            string nombre = reader["Nombre"].ToString();
                            string descripcion = reader["Descripcion"].ToString();
                            decimal precio = Convert.ToDecimal(reader["Precio"]);
                            int stock = Convert.ToInt32(reader["Stock"]);
                            double valoracion = Convert.ToDouble(reader["ValoracionMedia"]);

                            // Convertir la valoración a estrellas
                            string valoracionEstrellas;




                            string imagenUrl = $"{rutaImagenes}{nombre}.png";



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
                            <p class='precio-valoracion-producto' style='font-size: 16px;'>{precio} €</p>
                        </div>
                    </div>";

                            // Agregar el inicio del HTML del producto al contenedor
                            productosListaDeseosContainer.Controls.Add(new LiteralControl(productoHtmlInicio));




                            // Agregar el botón dinámico al contenedor
                            productosListaDeseosContainer.Controls.Add(botonListaDeseos);


                            // Agregar el fin del HTML del producto al contenedor
                            productosListaDeseosContainer.Controls.Add(new LiteralControl(productoHtmlFin));


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí (por ejemplo, registrándola, mostrando un mensaje de error, etc.)
                // Aquí puedes personalizar la forma en que deseas manejar la excepción
                // Por ejemplo:
                // Console.WriteLine("Error: " + ex.Message);
                // Log.Error("Error al cargar productos", ex);
                // MostrarMensajeError("Error al cargar productos. Por favor, inténtalo de nuevo más tarde.");
            }
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


        protected void CargarProductosCarrito()
        {
            string rutaImagenes = "../imagenesProductos/";

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";

            int id_cliente = ObtenerIdCliente();
            if (id_cliente == 0)
            {
                return; // Detener la ejecución si no se obtiene un id_cliente válido
            }

            string query = @"
                SELECT p.id_producto, p.Nombre, p.Precio, ca.Cantidad
                FROM carrito ca
                JOIN producto p ON ca.id_producto = p.id_producto
                WHERE ca.id_cliente = @id_cliente";

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@id_cliente", id_cliente); // Agregar el parámetro del id_cliente
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        HashSet<int> productosProcesados = new HashSet<int>();

                        while (reader.Read())
                        {
                            int idProducto = Convert.ToInt32(reader["id_producto"]);
                            if (productosProcesados.Contains(idProducto))
                            {
                                continue; // Si el producto ya ha sido procesado, saltar a la siguiente iteración
                            }

                            productosProcesados.Add(idProducto);

                            string nombre = reader["Nombre"].ToString();

                            decimal precio = Convert.ToDecimal(reader["Precio"]);
                            int cantidad = Convert.ToInt32(reader["Cantidad"]);
                            string imagenUrl = $"{rutaImagenes}{nombre}.png";

                            // Crear un elemento <div> con el nombre del producto
                            string productoHtml = $@"
                                <div class='productoCarrito' onclick='mostrarDetalleProducto(""{nombre}"", ""{imagenUrl}"")'>
                                    <div class='imagen-producto-carrito'>
                                        <img src='{imagenUrl}' alt='{nombre}'/>
                                    </div>
                                    <div class='datos-producto-carrito' style='text-align: center;'>
                                        <p class='nombre-producto-carrito' style='text-align: left; font-size: 24px;'>{nombre}</p>
                                        
                                        <p class='precio-producto-carrito' style='text-align: left; font-size: 20px;'>Precio: {precio:C}</p>
                                        <p class='cantidad-producto-carrito' style='text-align: left; font-size: 20px;'>Cantidad: {cantidad}</p>
                                        
                                    </div>
                                </div>";

                            productosCarritoContainer.Controls.Add(new LiteralControl(productoHtml));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí (por ejemplo, registrándola, mostrando un mensaje de error, etc.)
                // Aquí puedes personalizar la forma en que deseas manejar la excepción
                // Por ejemplo:
                // Console.WriteLine("Error: " + ex.Message);
                // Log.Error("Error al cargar productos", ex);
                // MostrarMensajeError("Error al cargar productos. Por favor, inténtalo de nuevo más tarde.");
            }
        }

        protected void VerCarritoBtn_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            int idCliente = ObtenerIdCliente();
            int idPedido = GenerarIdPedido(); // Supongamos que tienes una función para generar el id_pedido

            string queryCarrito = "SELECT id_producto, cantidad FROM carrito WHERE id_cliente = @id_cliente";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand commandCarrito = new SqlCommand(queryCarrito, connection))
                    {
                        commandCarrito.Parameters.AddWithValue("@id_cliente", idCliente);
                        connection.Open();

                        using (SqlDataReader reader = commandCarrito.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int idProducto = Convert.ToInt32(reader["id_producto"]);
                                int cantidad = Convert.ToInt32(reader["cantidad"]);

                                InsertarDetallePedido(idPedido, idProducto, cantidad);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
                Console.WriteLine("Error: " + ex.Message);
            }

            InsertarEstadoPedido(idCliente, idPedido);
            borrarCarrito(idCliente);
            Response.Redirect("pago.aspx");
        }

        private void borrarCarrito(int idCliente)
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string queryDelete = "DELETE FROM carrito WHERE id_cliente = @id_cliente";


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand commandDelete = new SqlCommand(queryDelete, connection))
                    {
                        commandDelete.Parameters.AddWithValue("@id_cliente", idCliente);


                        connection.Open();
                        commandDelete.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        private void InsertarEstadoPedido(int idCliente, int id_pedido)
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string queryInsert = "INSERT INTO pedido (id_pedido, id_cliente, Fecha_pedido, Estado_pedido) " +
                                 "VALUES (@id_pedido, @id_cliente, @fecha_pedido, @estado_pedido)";

            DateTime fechaPedido = DateTime.Now;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand commandInsert = new SqlCommand(queryInsert, connection))
                    {
                        commandInsert.Parameters.AddWithValue("@id_pedido", id_pedido);
                        commandInsert.Parameters.AddWithValue("@id_cliente", idCliente);
                        commandInsert.Parameters.AddWithValue("@fecha_pedido", fechaPedido);
                        commandInsert.Parameters.AddWithValue("@estado_pedido", "pendiente");

                        connection.Open();
                        commandInsert.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
                Console.WriteLine("Error: " + ex.Message);
            }
        }



        private void InsertarDetallePedido(int idPedido, int idProducto, int cantidad)
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string queryInsert = "INSERT INTO detalle_pedido (id_pedido, id_producto, cantidad, precio_unitario) " +
                "VALUES (@id_pedido, @id_producto, @cantidad, (SELECT Precio FROM producto WHERE id_producto = @id_producto))";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand commandInsert = new SqlCommand(queryInsert, connection))
                    {
                        commandInsert.Parameters.AddWithValue("@id_pedido", idPedido);
                        commandInsert.Parameters.AddWithValue("@id_producto", idProducto);
                        commandInsert.Parameters.AddWithValue("@cantidad", cantidad);

                        connection.Open();
                        commandInsert.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private int GenerarIdPedido()
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string query = "SELECT ISNULL(MAX(id_pedido), 0) + 1 AS NextId FROM detalle_pedido";

            int nextId = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        nextId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
                Console.WriteLine("Error: " + ex.Message);
            }

            return nextId;
        }




        private void ObtenerPrecioTotal()
        {
            string precioTotal = "0.00"; // Valor inicial del precio total

            // Obtener el id_cliente
            int id_cliente = ObtenerIdCliente();

            // Verificar si se obtuvo un id_cliente válido
            if (id_cliente != 0)
            {
                string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";

                string query = @"
            SELECT SUM(p.Precio * ca.Cantidad) AS PrecioTotal
            FROM carrito ca
            JOIN producto p ON ca.id_producto = p.id_producto
            WHERE ca.id_cliente = @id_cliente";

                try
                {
                    using (SqlConnection conexion = new SqlConnection(connectionString))
                    {
                        SqlCommand comando = new SqlCommand(query, conexion);
                        comando.Parameters.AddWithValue("@id_cliente", id_cliente); // Agregar el parámetro del id_cliente
                        conexion.Open();

                        object result = comando.ExecuteScalar(); // Ejecutar la consulta y obtener el resultado

                        if (result != null) // Si se obtiene un resultado válido
                        {
                            precioTotal = Convert.ToDecimal(result).ToString("C"); // Convertir el resultado a decimal y formatearlo como moneda
                        }
                    }

                    precioTotal = "Total a pagar: " + precioTotal;
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí (por ejemplo, registrándola, mostrando un mensaje de error, etc.)
                    // Aquí puedes personalizar la forma en que deseas manejar la excepción
                    // Por ejemplo:
                    // Console.WriteLine("Error: " + ex.Message);
                    // Log.Error("Error al calcular el precio total", ex);
                    // MostrarMensajeError("Error al calcular el precio total. Por favor, inténtalo de nuevo más tarde.");
                }
            }

            cartInfoContainer.Controls.Add(new LiteralControl(precioTotal));

        }

        protected void AgregarAListaDeseos_Click(object sender, EventArgs e)
        {
            // Obtener el ID del cliente y el ID del producto
            int idCliente = ObtenerIdCliente();
            ImageButton clickedButton = (ImageButton)sender;
            string nombreProducto = clickedButton.CommandArgument;

            // Convertir el CommandArgument a entero y usarlo para obtener el ID del producto
            int idProducto = ObtenerIdProducto(nombreProducto);

            // Verificar si el producto ya está en la lista de deseos del cliente
            if (comprobarListaDeseados(nombreProducto))
            {
                // Si el producto ya está en la lista, eliminarlo
                EliminarProductoDeListaDeseos(idCliente, idProducto);
            }
            else
            {
                // Si el producto no está en la lista, agregarlo
                AgregarProductoAListaDeseos(idCliente, idProducto);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "PostBackScript", "__doPostBack('', '');", true);
            // Actualizar la vista, si es necesario
            //CargarProductosCarrito(); // O cualquier otra lógica de actualización de la interfaz de usuario
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

            ScriptManager.RegisterStartupScript(this, GetType(), "PostBackScript", "__doPostBack('', '');", true);
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


        private void EliminarProductoDeListaDeseos(int idCliente, int idProducto)
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";

            string query = "DELETE FROM lista_deseos_cliente WHERE id_cliente = @idCliente AND id_producto = @idProducto";

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    comando.Parameters.AddWithValue("@idProducto", idProducto);
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
            }
        }


    }
}
