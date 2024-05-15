using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg.Paginas
{
    public partial class HacerPedido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AgregarAListaDeseos_Click(object sender, EventArgs e)
        {
            // Obtener el ID del cliente y el ID del producto
            int idCliente = ObtenerIdCliente();
            Button clickedButton = (Button)sender;
            string commandArgument = clickedButton.CommandArgument;

            // Convertir el CommandArgument a entero y usarlo para obtener el ID del producto
            int idProducto = ObtenerIdProducto(commandArgument);

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