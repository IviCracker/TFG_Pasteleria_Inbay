using System;
using System.Collections.Generic;
using System.Linq;
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

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
           
        }

        protected void btnIniciarSesion_Click(object sender, EventArgs e)
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
            Response.Redirect("../Paginas/Registro.aspx");
        }



    }
}