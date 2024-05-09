
using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg
{
    public partial class Default : System.Web.UI.Page
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
            string rutaImagenes = "imagenesProductos/";

            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=proyectopasteleriainbay_;Uid=proyectopasteleriainbay_;Pwd=proyectopasteleriainbay_;";

            string query = "SELECT Nombre, Descripcion, Precio, Stock FROM producto WHERE destacado = 1";

            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand comando = new SqlCommand(query, conexion);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombre = reader["Nombre"].ToString();
                            string descripcion = reader["Descripcion"].ToString();
                            decimal precio = Convert.ToDecimal(reader["Precio"]);
                            int stock = Convert.ToInt32(reader["Stock"]);
                            string imagenUrl = $"{rutaImagenes}{nombre}.png";

                            // Crear un elemento <div> con el nombre del producto
                            string productoHtml = $@"
                <div class='producto' onclick='mostrarDetalleProducto(""{nombre}"", ""{imagenUrl}"")'>
                    <div class='imagen-producto' style=' width: 250px; height: 250px; display: flex; justify-content: center; align-items: center; overflow: hidden;'>
                        <img src='{imagenUrl}' alt='{nombre}' style='width: auto; height: 100%; object-fit: cover;' />
                    </div>
                    <div class='datos-producto' style='text-align: left; padding-top:10px;'>
                        <p class='nombre-producto' style='text-align: left;font-family: Oswald; font-size: 18px;'>{nombre}</p>
                    </div>
                </div>";

                            productosDestacadosContainer.Controls.Add(new LiteralControl(productoHtml));
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








    }
}

