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