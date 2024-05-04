using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg.Paginas
{
    public partial class DatosTarjeta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioActual"] == null)
            {
                // La sesión ha caducado o el usuario no está autenticado, redirigir a una página de inicio de sesión
                Response.Redirect("~/index.aspx");
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

        protected void BtnGuardarDatosTarjeta_Click(object sender, EventArgs e)
        {
            // Obtener los nuevos valores de los campos de texto
            string NumeroTarjeta = txtNumeroTarjeta.Text;
            string NombreTarjeta = txtNombreTarjeta.Text;
            string mesExp = mesExpiracion.Text;
            string anioExp = anioExpiracion.Text;
            string CVV = txtCVV.Text;

            // Obtener el ID del cliente
            int idCliente = ObtenerIdCliente();

            string connectionString = "DataBase=tfg;DataSource=localhost;user=root;Port=3306";
            string queryCheckExistence = "SELECT COUNT(*) FROM informacion_tarjeta WHERE id_cliente = @id_cliente";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Verificar si existe una tarjeta para el cliente
                MySqlCommand commandCheck = new MySqlCommand(queryCheckExistence, connection);
                commandCheck.Parameters.AddWithValue("@id_cliente", idCliente);
                connection.Open();

                int tarjetasCount = Convert.ToInt32(commandCheck.ExecuteScalar());

                if (tarjetasCount == 0)
                {
                    // Si no hay tarjetas asociadas al cliente, realizamos una inserción
                    string queryInsert = "INSERT INTO informacion_tarjeta (id_cliente, numero_tarjeta, nombre_tarjeta, mes_expiracion, anio_expiracion, cvv) VALUES (@id_cliente, @numero_tarjeta, @nombre_tarjeta, @mes_expiracion, @anio_expiracion, @cvv)";

                    MySqlCommand commandInsert = new MySqlCommand(queryInsert, connection);
                    commandInsert.Parameters.AddWithValue("@id_cliente", idCliente);
                    commandInsert.Parameters.AddWithValue("@numero_tarjeta", NumeroTarjeta);
                    commandInsert.Parameters.AddWithValue("@nombre_tarjeta", NombreTarjeta);
                    commandInsert.Parameters.AddWithValue("@mes_expiracion", mesExp);
                    commandInsert.Parameters.AddWithValue("@anio_expiracion", anioExp);
                    commandInsert.Parameters.AddWithValue("@cvv", CVV);

                    commandInsert.ExecuteNonQuery();
                }
                else
                {
                    // Si ya existe una tarjeta, realizamos una actualización
                    string queryUpdate = "UPDATE informacion_tarjeta SET numero_tarjeta = @numero_tarjeta, nombre_tarjeta = @nombre_tarjeta, mes_expiracion = @mes_expiracion, anio_expiracion = @anio_expiracion, cvv = @cvv WHERE id_cliente = @id_cliente";

                    MySqlCommand commandUpdate = new MySqlCommand(queryUpdate, connection);
                    commandUpdate.Parameters.AddWithValue("@numero_tarjeta", NumeroTarjeta);
                    commandUpdate.Parameters.AddWithValue("@nombre_tarjeta", NombreTarjeta);
                    commandUpdate.Parameters.AddWithValue("@mes_expiracion", mesExp);
                    commandUpdate.Parameters.AddWithValue("@anio_expiracion", anioExp);
                    commandUpdate.Parameters.AddWithValue("@cvv", CVV);
                    commandUpdate.Parameters.AddWithValue("@id_cliente", idCliente);

                    commandUpdate.ExecuteNonQuery();
                }
            }
        }


        protected int ObtenerIdCliente()
        {
            int id_cliente = 0; // Inicializamos el ID del cliente como 0

            string connectionString = "DataBase=tfg;DataSource=localhost;user=root;Port=3306"; // Reemplaza "tucontraseña" con la contraseña de tu base de datos
            string query = "SELECT id_cliente FROM cliente WHERE Nombre = @nombre"; // Consulta para obtener el ID del cliente basado en el nombre

            string nombreUsuario = Session["UsuarioActual"].ToString();
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand comando = new MySqlCommand(query, conexion);
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


        protected void CargarValoresCampos()
        {
            // Verificar si la sesión está activa y el usuario está autenticado
            if (Session["UsuarioActual"] != null)
            {
                int id_cliente = ObtenerIdCliente(); // Obtener el ID del cliente

                // Consulta para obtener la información de la tarjeta del cliente
                string connectionString = "DataBase=tfg;DataSource=localhost;user=root;Port=3306";
                string query = "SELECT numero_tarjeta, nombre_tarjeta, mes_expiracion, anio_expiracion, cvv FROM informacion_tarjeta WHERE id_cliente = @id_cliente";

                using (MySqlConnection conexion = new MySqlConnection(connectionString))
                {
                    MySqlCommand comando = new MySqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@id_cliente", id_cliente); // Agregar el parámetro del ID del cliente
                    conexion.Open();

                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtNumeroTarjeta.Text = reader["numero_tarjeta"].ToString();
                            txtNombreTarjeta.Text = reader["nombre_tarjeta"].ToString();
                            mesExpiracion.SelectedValue = reader["mes_expiracion"].ToString();
                            anioExpiracion.SelectedValue = reader["anio_expiracion"].ToString();
                            txtCVV.Text = reader["cvv"].ToString();
                        }
                    }
                }
            }
            else
            {
                // La sesión ha caducado o el usuario no está autenticado, redirigir a una página de inicio de sesión
                Response.Redirect("../index.aspx");
            }
        }



    }
}