using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg.Paginas
{
    public partial class Administracion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarPedidos();
        }



        private void CargarPedidos()
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string consulta = @"
                SELECT 
                    p.id_pedido,
                    p.id_cliente,
                    p.Fecha_pedido,
                    p.Estado_pedido,
                    dp.id_detalle,
                    dp.id_producto,
                    dp.Cantidad,
                    dp.Precio_unitario,
                    c.Nombre AS NombreCliente,
                    c.Correo,
                    c.Telefono,
                    c.Direccion,
                    pr.Nombre AS NombreProducto
                FROM 
                    pedido p
                JOIN 
                    detalle_pedido dp ON p.id_pedido = dp.id_pedido
                JOIN 
                    cliente c ON p.id_cliente = c.id_cliente
                JOIN 
                    producto pr ON dp.id_producto = pr.id_producto
                ORDER BY 
                    p.id_pedido, dp.id_detalle;
            ";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();

                    int currentPedidoId = -1;
                    decimal totalPedido = 0;
                    StringBuilder detallesPedido = new StringBuilder();
                    bool firstRecord = true;
                    string nombreCliente = "", correo = "", telefono = "", direccion = "", fechaPedido = "", estadoPedido = "";

                    while (reader.Read())
                    {
                        int idPedido = Convert.ToInt32(reader["id_pedido"]);

                        if (idPedido != currentPedidoId)
                        {
                            if (!firstRecord)
                            {
                                // Crear div del pedido anterior
                                CrearDivPedido(currentPedidoId, nombreCliente, correo, telefono, direccion, fechaPedido, estadoPedido, totalPedido, detallesPedido.ToString());
                            }

                            // Reiniciar acumuladores para el nuevo pedido
                            currentPedidoId = idPedido;
                            totalPedido = 0;
                            detallesPedido.Clear();

                            nombreCliente = reader["NombreCliente"].ToString();
                            correo = reader["Correo"].ToString();
                            telefono = reader["Telefono"].ToString();
                            direccion = reader["Direccion"].ToString();
                            fechaPedido = reader["Fecha_pedido"].ToString();
                            estadoPedido = reader["Estado_pedido"].ToString();

                            firstRecord = false;
                        }

                        // Acumular detalles del producto actual
                        detallesPedido.Append($@"
                            <p>Producto: {reader["NombreProducto"]}</p>
                            <p>Cantidad: {reader["Cantidad"]}</p>
                            <p>Precio Unitario: {reader["Precio_unitario"]}</p>
                        ");

                        // Calcular total del pedido
                        decimal precioUnitario = Convert.ToDecimal(reader["Precio_unitario"]);
                        int cantidad = Convert.ToInt32(reader["Cantidad"]);
                        totalPedido += precioUnitario * cantidad;
                    }

                    if (!firstRecord)
                    {
                        // Crear div del último pedido
                        CrearDivPedido(currentPedidoId, nombreCliente, correo, telefono, direccion, fechaPedido, estadoPedido, totalPedido, detallesPedido.ToString());
                    }

                    reader.Close();
                }
            }
        }
        protected int comprobarEstado(int idPedido)
        {
            int totalRegistros = 0;

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string query = "SELECT COUNT(*) AS TotalRegistros FROM pedido WHERE id_pedido = @id_Pedido and Estado_pedido = 'pendiente'";


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@id_Pedido", idPedido);

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
       
        private void CrearDivPedido(int idPedido, string nombreCliente, string correo, string telefono, string direccion, string fechaPedido, string estadoPedido, decimal totalPedido, string detallesPedido)
        {
           

            string tituloClase = estadoPedido == "Pendiente" ? "titulo-pendiente" : "titulo-completado";

            // Crear el botón y asociar el evento de clic
            Button botonAceptarPedido = new Button
            {
                CssClass = "btnSubmit",
                ID = $"btn-carrito-{idPedido}",
                Text = "Completar Pedido",
                CommandArgument = idPedido.ToString()
            };
            botonAceptarPedido.Click += new EventHandler(aceptarPedido_Click);

            // Crear un contenedor literal para el HTML
            System.Web.UI.WebControls.Literal literalHtml = new System.Web.UI.WebControls.Literal
            {
                Text = $@"
<div class='cliente'>
    <h3 class='{tituloClase}'>Pedido ID: {idPedido}</h3>
    <p>Cliente: {nombreCliente} ({correo})</p>
    <p>Teléfono: {telefono}</p>
    <p>Dirección: {direccion}</p>
</div>
<div class='pedido'>
    <p>Fecha del Pedido: {fechaPedido}</p>
    <p>Estado del Pedido: {estadoPedido}</p>
    <h4>Detalle del Pedido:</h4>
    <div class='detalles-pedido'>
        {detallesPedido}
    </div>
    <br>
    <h4>Total del Pedido: {totalPedido:C}</h4>
</div>"
            };

            // Crear el div contenedor para el pedido
            System.Web.UI.HtmlControls.HtmlGenericControl divPedido = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            divPedido.Attributes["class"] = "panel"; // Aplicar la clase 'panel'

            // Agregar el HTML al contenedor
            divPedido.Controls.Add(literalHtml);

            // Agregar el botón al div con clase 'pedido' solo si el estado es 'Pendiente'
            if (comprobarEstado(idPedido) > 0)
            {
                PedidosContainer.Controls.Add(botonAceptarPedido);
            }

            // Agregar el div al contenedor de pedidos
            PedidosContainer.Controls.Add(divPedido);

        }




        protected void aceptarPedido_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int idPedido = int.Parse(btn.CommandArgument);

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                    string consulta = "UPDATE pedido SET estado_pedido = @estadoPedido WHERE id_pedido = @idPedido";

                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@estadoPedido", "Completado");
                        comando.Parameters.AddWithValue("@idPedido", idPedido);

                        int rowsAffected = comando.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Si la actualización fue exitosa, recargar la página o actualizar el estado en la UI
                            Response.Redirect(Request.RawUrl); // Recargar la página actual
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                
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