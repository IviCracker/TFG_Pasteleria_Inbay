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
                Response.Redirect("~/default.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    // Cargar los valores de los campos solo en el primer cargado de la página
                    CargarValoresCampos();
                }
            }
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


    }
}