using MySql.Data.MySqlClient;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                CargarProductos();
            }
        }

        protected void CargarProductos()
        {
            string connectionString = "DataBase=tfg;DataSource=localhost;user=root;Port=3306";
            string query = "SELECT Nombre, Descripcion, Precio, Stock, Imagen FROM producto WHERE destacado = 1";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                MySqlCommand comando = new MySqlCommand(query, conexion);
                conexion.Open();

                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nombre = reader["Nombre"].ToString();
                        string descripcion = reader["Descripcion"].ToString();
                        decimal precio = Convert.ToDecimal(reader["Precio"]);
                        int stock = Convert.ToInt32(reader["Stock"]);

                        // Obtener la imagen como un array de bytes
                        byte[] imagenBytes = (byte[])reader["Imagen"];
                        string imagenBase64 = Convert.ToBase64String(imagenBytes);
                        string imagenUrl = $"data:image/jpeg;base64,{imagenBase64}";

                        // Crear un elemento <div> con la imagen y el nombre del producto
                        string productoHtml = $"<div class='producto'>" +
                                               $"<img src='{imagenUrl}' alt='{nombre}' onclick='openModal(\"{nombre}\", \"{imagenUrl}\")' data-nombre='{nombre}' />" +
                                               $"<p class='nombre-producto'>{nombre}</p>" +
                                               $"</div>";

                        productosDestacadosContainer.Controls.Add(new LiteralControl(productoHtml));
                    }
                }
            }
        }


        // Método para cargar dinámicamente la información del producto en el modal
        [System.Web.Services.WebMethod]
        public static string CargarInformacionProducto(string nombreProducto)
        {
            string descripcion = "";
            decimal precio = 0;
            int stock = 0;

            string connectionString = "DataBase=tfg;DataSource=localhost;user=root;Port=3306";
            string query = "SELECT Descripcion, Precio, Stock FROM producto WHERE Nombre = @nombreProducto";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombreProducto", nombreProducto);
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        descripcion = reader["Descripcion"].ToString();
                        precio = Convert.ToDecimal(reader["Precio"]);
                        stock = Convert.ToInt32(reader["Stock"]);
                    }
                }
            }

            // Formatear la respuesta como un JSON
            return $"{{\"descripcion\": \"{descripcion}\", \"precio\": {precio}, \"stock\": {stock}}}";
        }

      
    }
}
