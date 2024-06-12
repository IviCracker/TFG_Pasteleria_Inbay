using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg.Paginas
{
    public partial class Pago : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarValoresCampos();
            CargarUltimoPedido();
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
        protected void BtnEditarDatosTarjeta_Click(object sender, EventArgs e)
        {
            Response.Redirect("DatosTarjeta.aspx");
        }
        protected void BtnPagar_Click(object sender, EventArgs e)
        {
            Response.Redirect("../default.aspx");
        }

        protected void CargarValoresCampos()
        {
            // Verificar si la sesión está activa y el usuario está autenticado
            if (Session["UsuarioActual"] != null)
            {
                int id_cliente = ObtenerIdCliente(); // Obtener el ID del cliente

                // Consulta para obtener la información de la tarjeta del cliente
                string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;"; // Aquí debes poner tu cadena de conexión
                string query = "SELECT numero_tarjeta, nombre_tarjeta, mes_expiracion, anio_expiracion, cvv FROM informacion_tarjeta WHERE id_cliente = @id_cliente";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@id_cliente", id_cliente); // Agregar el parámetro del ID del cliente
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtNumeroTarjeta.Text = reader["numero_tarjeta"].ToString();
                            txtNombreTarjeta.Text = reader["nombre_tarjeta"].ToString();
                            mesExpiracion.SelectedIndex = Convert.ToInt32(reader["mes_expiracion"]);
                            anioExpiracion.SelectedIndex = Convert.ToInt32(reader["anio_expiracion"]);

                            txtCVV.Text = reader["cvv"].ToString();
                        }
                    }
                }
            }
            else
            {
                // La sesión ha caducado o el usuario no está autenticado, redirigir a una página de inicio de sesión
                Response.Redirect("../default.aspx");
            }
        }
        private void CargarUltimoPedido()
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            int idCliente = ObtenerIdCliente();

            // Consulta para obtener el último pedido
            string queryPedido = @"
    SELECT TOP 1 p.id_pedido, p.Fecha_pedido, p.Estado_pedido,
           (SELECT SUM(dp2.cantidad * dp2.precio_unitario)
            FROM detalle_pedido dp2
            WHERE dp2.id_pedido = p.id_pedido) AS precio_total_pedido
    FROM pedido p
    WHERE p.id_cliente = @id_cliente
    ORDER BY p.Fecha_pedido DESC";

            // Consulta para obtener los detalles de los productos del último pedido
            string queryDetalles = @"
    SELECT p.Nombre AS NombreProducto, d.Cantidad, d.Precio_unitario
    FROM detalle_pedido d
    JOIN producto p ON d.id_producto = p.id_producto
    WHERE d.id_pedido = @id_pedido";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Obtener el último pedido
                    int idPedido;
                    DateTime fechaPedido;
                    string estadoPedido;
                    decimal precioTotalPedido;

                    using (SqlCommand commandPedido = new SqlCommand(queryPedido, connection))
                    {
                        commandPedido.Parameters.AddWithValue("@id_cliente", idCliente);

                        using (SqlDataReader readerPedido = commandPedido.ExecuteReader())
                        {
                            if (readerPedido.Read())
                            {
                                idPedido = Convert.ToInt32(readerPedido["id_pedido"]);
                                fechaPedido = Convert.ToDateTime(readerPedido["Fecha_pedido"]);
                                estadoPedido = readerPedido["Estado_pedido"].ToString();
                                precioTotalPedido = Convert.ToDecimal(readerPedido["precio_total_pedido"]);
                            }
                            else
                            {
                                // Manejo si no se encontraron pedidos para el cliente
                                ultimoPedidosContainer.Controls.Clear();
                                ultimoPedidosContainer.Controls.Add(new LiteralControl("<p>No hay pedidos registrados para este cliente.</p>"));
                                return;
                            }
                        }
                    }

                    // Obtener los detalles de los productos del último pedido
                    List<string> detallesPedido = new List<string>();

                    using (SqlCommand commandDetalles = new SqlCommand(queryDetalles, connection))
                    {
                        commandDetalles.Parameters.AddWithValue("@id_pedido", idPedido);

                        using (SqlDataReader readerDetalles = commandDetalles.ExecuteReader())
                        {
                            while (readerDetalles.Read())
                            {
                                string nombreProducto = readerDetalles["NombreProducto"].ToString();
                                int cantidad = Convert.ToInt32(readerDetalles["Cantidad"]);
                                decimal precioUnitario = Convert.ToDecimal(readerDetalles["Precio_unitario"]);

                                // Agregar detalles del producto a la lista
                                string detalleProductoHtml = $@"
                        <div class='detalle-pedido'>
                            <p>Producto: {nombreProducto}</p>
                            <p>Cantidad: {cantidad}</p>
                            <p>Precio Unitario: {precioUnitario:C}</p>
                        </div>";

                                detallesPedido.Add(detalleProductoHtml);
                            }
                        }
                    }

                    // Crear HTML para el último pedido con todos los detalles
                    string pedidoHtml = $@"
            <div class='pedido'>
                <h3>{fechaPedido:dd/MM/yyyy}</h3>
                <div class='detalles-pedido'>
                    {string.Join("", detallesPedido)}
                    <p class='precio-total-pedido'>Precio Total Pedido: {precioTotalPedido:C}</p>
                </div>
            </div>";

                    // Limpiar y agregar el HTML del pedido al contenedor
                    ultimoPedidosContainer.Controls.Clear();
                    ultimoPedidosContainer.Controls.Add(new LiteralControl(pedidoHtml));
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
                Console.WriteLine("Error: " + ex.Message);
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
        protected void VerCarritoBtn_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            int idCliente = ObtenerIdCliente();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consultar si existe una tarjeta de crédito
                    string queryTarjeta = "SELECT COUNT(*) FROM informacion_tarjeta WHERE id_cliente = @id_cliente";
                    using (SqlCommand commandTarjeta = new SqlCommand(queryTarjeta, connection))
                    {
                        commandTarjeta.Parameters.AddWithValue("@id_cliente", idCliente);
                        int cantidadTarjetas = (int)commandTarjeta.ExecuteScalar();

                        if (cantidadTarjetas > 0)
                        {
                            int idPedido = GenerarIdPedido(); // Supongamos que tienes una función para generar el id_pedido

                            string queryCarrito = "SELECT id_producto, cantidad FROM carrito WHERE id_cliente = @id_cliente";

                            // Consultar el carrito
                            using (SqlCommand commandCarrito = new SqlCommand(queryCarrito, connection))
                            {
                                commandCarrito.Parameters.AddWithValue("@id_cliente", idCliente);

                                using (SqlDataReader reader = commandCarrito.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        int idProducto = Convert.ToInt32(reader["id_producto"]);
                                        int cantidad = Convert.ToInt32(reader["cantidad"]);

                                        InsertarDetallePedido(idPedido, idProducto, cantidad);
                                        ActualizarStock(idProducto, cantidad);
                                    }
                                }
                            }

                            // Insertar estado del pedido y borrar el carrito
                            InsertarEstadoPedido(idCliente, idPedido);
                            borrarCarrito(idCliente);
                            Response.Redirect("Pago.aspx");
                        }
                        else
                        {
                            // Si no hay tarjetas, redirigir a la página para agregar una tarjeta
                            Response.Redirect("DatosTarjeta.aspx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        private void ActualizarStock(int idProducto, int cantidad)
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string queryUpdateStock = "UPDATE producto SET Stock = Stock - @cantidad WHERE id_producto = @id_producto";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand commandUpdateStock = new SqlCommand(queryUpdateStock, connection))
                    {
                        commandUpdateStock.Parameters.AddWithValue("@id_producto", idProducto);
                        commandUpdateStock.Parameters.AddWithValue("@cantidad", cantidad);

                        connection.Open();
                        commandUpdateStock.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
                Console.WriteLine("Error: " + ex.Message);
            }
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
    }
}