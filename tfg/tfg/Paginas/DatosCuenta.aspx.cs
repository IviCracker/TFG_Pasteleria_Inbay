using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg.Paginas
{
    public partial class DatosCuenta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioActual"] == null)
            {
                // La sesión ha caducado o el usuario no está autenticado, redirigir a una página de inicio de sesión
                Response.Redirect("../Paginas/Registro.aspx");
            }
            else
            {
                CargarProductosCarrito();
                ObtenerPrecioTotal();
                if (!IsPostBack)
                {
                    // Cargar los valores de los campos solo en el primer cargado de la página
                    CargarValoresCampos();
                }
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
        protected void btnEditarNombre_Click(object sender, EventArgs e)
        {
            btnActualizar();
        }

        protected void btnEditarCorreo_Click(object sender, EventArgs e)
        {
            btnActualizar();
        }

        protected void btnEditarTelefono_Click(object sender, EventArgs e)
        {
            btnActualizar();
        }

        protected void btnEditarDireccion_Click(object sender, EventArgs e)
        {
            btnActualizar();
        }

        protected void btnEditarContraseña_Click(object sender, EventArgs e)
        {
            btnActualizar();
        }


        protected void CargarValoresCampos()
        {
            // Verificar si la sesión está activa y el usuario está autenticado
            if (Session["UsuarioActual"] != null)
            {
                string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";

                string query = "SELECT Nombre, Correo, Telefono, Direccion, Contraseña FROM cliente WHERE id_cliente = @id_cliente"; // Ajusta esta consulta según tu base de datos y tu esquema de tablas
                int id_cliente = ObtenerIdCliente(); // Supongamos que tienes una función para obtener el ID del cliente

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@id_cliente", id_cliente); // Agrega el parámetro del ID del cliente
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtNombre.Text = reader["Nombre"].ToString();
                            txtCorreo.Text = reader["Correo"].ToString();
                            txtTelefono.Text = reader["Telefono"].ToString();
                            txtDireccion.Text = reader["Direccion"].ToString();
                            txtContraseña.Text = reader["Contraseña"].ToString();
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


        protected void btnActualizar()
        {
            // Obtener los nuevos valores de los campos de texto
            string nuevoNombre = txtNombre.Text;
            string nuevoCorreo = txtCorreo.Text;
            string nuevoTelefono = txtTelefono.Text;
            string nuevaDireccion = txtDireccion.Text;
            string nuevaContraseña = txtContraseña.Text;

            // Obtener el ID del cliente
            int idCliente = ObtenerIdCliente();

            // Realizar la actualización en la base de datos
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;"; // Aquí debes poner tu cadena de conexión

            string query = "UPDATE cliente SET nombre = @nombre, correo = @correo, telefono = @telefono, direccion = @direccion, contraseña = @contraseña WHERE id_cliente = @id_cliente";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", nuevoNombre);
                command.Parameters.AddWithValue("@correo", nuevoCorreo);
                command.Parameters.AddWithValue("@telefono", nuevoTelefono);
                command.Parameters.AddWithValue("@direccion", nuevaDireccion);
                command.Parameters.AddWithValue("@contraseña", nuevaContraseña);
                command.Parameters.AddWithValue("@id_cliente", idCliente);

                connection.Open();
                command.ExecuteNonQuery();
                Session["UsuarioActual"] = nuevoNombre;
            }
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
            Response.Redirect("Paginas/pago.aspx");
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
    }
}