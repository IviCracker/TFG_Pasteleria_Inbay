using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg.Paginas
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioActual"] != null)
            {
                CargarProductosCarrito();
                ObtenerPrecioTotal();
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

        protected void btnMostrarRegistro_Click(object sender, EventArgs e)
        {
            // Ocultar el formulario de inicio de sesión y mostrar el formulario de registro
            textoIniciarSesion.Style["display"] = "none";
            textoCrearCuenta.Style["display"] = "block";

        }


        protected void lnkIniciarSesion_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registro.aspx");
        }




        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string nombreOCorreo = txtNombreUsuario.Text; // El campo txtNombreUsuario ahora puede contener el nombre de usuario o el correo electrónico
            string contraseña = txtContraseñaInicioSesion.Text;

            try
            {
                string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";


                string consulta = "SELECT COUNT(*) FROM cliente WHERE (nombre = @nombreOCorreo OR correo = @nombreOCorreo) AND contraseña = @contraseña";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombreOCorreo", nombreOCorreo);
                        comando.Parameters.AddWithValue("@contraseña", contraseña);

                        int count = Convert.ToInt32(comando.ExecuteScalar());

                        if (count > 0)
                        {

                            if (nombreOCorreo.Contains("@"))
                            {
                                string consultaSesion = "SELECT Nombre FROM cliente WHERE (nombre = @nombreOCorreo OR correo = @nombreOCorreo) AND contraseña = @contraseña";
                                using (SqlCommand comandoNombreSesion = new SqlCommand(consultaSesion, conexion))
                                {
                                    // Suponiendo que tienes los parámetros @nombreOCorreo y @contraseña definidos previamente
                                    comandoNombreSesion.Parameters.AddWithValue("@nombreOCorreo", nombreOCorreo);
                                    comandoNombreSesion.Parameters.AddWithValue("@contraseña", contraseña);

                                    // Ejecutas la consulta
                                    string nombreUsuario = comandoNombreSesion.ExecuteScalar() as string; // Suponiendo que el nombre es un string

                                    // Almacenar el nombre en la sesión si se encontró un resultado
                                    if (!string.IsNullOrEmpty(nombreUsuario))
                                    {
                                        Session["UsuarioActual"] = nombreUsuario;
                                    }
                                }
                            }
                            else
                            {
                                Session["UsuarioActual"] = nombreOCorreo;
                            }
                            conexion.Close();

                            HttpContext.Current.Response.Redirect("../default.aspx");
                            // Aquí puedes redirigir a otra página si el inicio de sesión es exitoso
                        }
                        else
                        {
                            lblErrorInicioSesion.Text = "Nombre de usuario, correo electrónico y/o contraseña incorrectos.";
                            lblErrorInicioSesion.Visible = true;
                            txtNombreUsuario.CssClass = "textbox error"; // Aplicamos una clase CSS para marcar el campo en rojo
                            txtContraseñaInicioSesion.CssClass = "textbox error"; // Aplicamos una clase CSS para marcar el campo en rojo
                        }
                    }
                }
            }
            catch (Exception falloInicioSesion)
            {
                lblErrorInicioSesion.Text = "Error al intentar iniciar sesión.";
                lblErrorInicioSesion.Visible = true;
                txtNombreUsuario.CssClass = "textbox error"; // Aplicamos una clase CSS para marcar el campo en rojo
                txtContraseñaInicioSesion.CssClass = "textbox error"; // Aplicamos una clase CSS para marcar el campo en rojo
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

            InsertarEstadoPedido(idCliente);
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

        private void InsertarEstadoPedido(int idCliente)
        {
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
            string queryInsert = "INSERT INTO pedido (id_cliente, Fecha_pedido, Estado_pedido) " +
                                 "VALUES (@id_cliente, @fecha_pedido, @estado_pedido)";

            DateTime fechaPedido = DateTime.Now;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand commandInsert = new SqlCommand(queryInsert, connection))
                    {
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

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset error labels and CSS classes
                ResetErrorLabels();

                string nombre = txtNombre.Text.Trim();
                string correo = txtCorreo.Text.Trim();
                string telefono = txtTelefono.Text.Trim();
                string direccion = txtDireccion.Text.Trim();
                string contraseña = txtContraseña.Text.Trim();
                string repetContraseña = txtRepetContraseña.Text.Trim();

                bool isValid = true;

                // Validations
                if (string.IsNullOrEmpty(nombre))
                {
                    lblErrorNombre.Text = "El nombre es obligatorio.";
                    lblErrorNombre.Visible = true;
                    txtNombre.CssClass = "textbox error-textbox";
                    isValid = false;
                }
                else if (UsuarioExiste(nombre))
                {
                    lblErrorNombre.Text = "El nombre de usuario ya está registrado.";
                    lblErrorNombre.Visible = true;
                    txtNombre.CssClass = "textbox error-textbox";
                    isValid = false;
                }

                if (string.IsNullOrEmpty(correo))
                {
                    lblErrorCorreo.Text = "El correo es obligatorio.";
                    lblErrorCorreo.Visible = true;
                    txtCorreo.CssClass = "textbox error-textbox";
                    isValid = false;
                }
                else if (!ValidarCorreo(correo))
                {
                    lblErrorCorreo.Text = "Formato de correo inválido.";
                    lblErrorCorreo.Visible = true;
                    txtCorreo.CssClass = "textbox error-textbox";
                    isValid = false;
                }
                else if (UsuarioExiste(correo))
                {
                    lblErrorCorreo.Text = "El correo electrónico ya está registrado.";
                    lblErrorCorreo.Visible = true;
                    txtCorreo.CssClass = "textbox error-textbox";
                    isValid = false;
                }

                if (string.IsNullOrEmpty(telefono))
                {
                    lblErrorTelefono.Text = "El teléfono es obligatorio.";
                    lblErrorTelefono.Visible = true;
                    txtTelefono.CssClass = "textbox error-textbox";
                    isValid = false;
                }
                else if (!EsNumero(telefono))
                {
                    lblErrorTelefono.Text = "El teléfono solo debe contener números.";
                    lblErrorTelefono.Visible = true;
                    txtTelefono.CssClass = "textbox error-textbox";
                    isValid = false;
                }

                if (!ValidarContraseña(contraseña))
                {
                    lblErrorContraseña.Text = "La contraseña debe tener al menos 1 mayúscula, 1 minúscula y un dígito.";
                    lblErrorContraseña.Visible = true;
                    txtContraseña.CssClass = "textbox error-textbox";
                    isValid = false;
                }

                if (contraseña != repetContraseña)
                {
                    lblErrorRepetContraseña.Text = "Las contraseñas no coinciden.";
                    lblErrorRepetContraseña.Visible = true;
                    txtRepetContraseña.CssClass = "textbox error-textbox";
                    isValid = false;
                }

                if (!isValid) return;

                // Database Insertion
                string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    string consulta = "INSERT INTO cliente(nombre, correo, telefono, direccion, contraseña) VALUES(@nombre, @correo, @telefono, @direccion, @contraseña)";
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombre", nombre);
                        comando.Parameters.AddWithValue("@correo", correo);
                        comando.Parameters.AddWithValue("@telefono", telefono);
                        comando.Parameters.AddWithValue("@direccion", direccion);
                        comando.Parameters.AddWithValue("@contraseña", contraseña);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                        conexion.Close();
                    }
                }

                // Almacenar el nombre en la sesión si el registro es exitoso
                Session["UsuarioActual"] = nombre;

                // Redireccionar a la página de inicio de sesión u otra página después del registro
                Response.Redirect("../default.aspx");
            }
            catch (Exception ex)
            {
                // Manejar la excepción
            }
        }

        private void ResetErrorLabels()
        {
            lblErrorNombre.Visible = false;
            lblErrorCorreo.Visible = false;
            lblErrorTelefono.Visible = false;
            lblErrorDireccion.Visible = false;
            lblErrorContraseña.Visible = false;
            lblErrorRepetContraseña.Visible = false;

            txtNombre.CssClass = txtCorreo.CssClass = txtTelefono.CssClass = txtDireccion.CssClass = txtContraseña.CssClass = txtRepetContraseña.CssClass = "textbox";
        }
        private bool UsuarioExiste(string correo)
        {
            // Implementación para verificar si el correo ya existe en la base de datos
            // Este es solo un pseudocódigo
            string connectionString = "DataBase=tfg;DataSource=localhost;user=root;Port=3306";
            string query = "SELECT COUNT(*) FROM cliente WHERE correo = @correo";
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@correo", correo);
                conexion.Open();
                int result = Convert.ToInt32(comando.ExecuteScalar());
                conexion.Close();
                return result > 0;
            }
        }
        private bool ValidarCorreo(string correo)
        {
            // Regex for a basic email validation
            return Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool ValidarContraseña(string contraseña)
        {
            // Simple password validation that could be expanded
            return contraseña.Length >= 4 && contraseña.Any(char.IsDigit) && contraseña.Any(char.IsUpper) && contraseña.Any(char.IsLower);
        }

        private bool EsNumero(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }



    }
}