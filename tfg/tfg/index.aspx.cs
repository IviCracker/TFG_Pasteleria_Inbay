using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tfg
{
    public partial class index : System.Web.UI.Page
    {
        MySqlConnection conexion = new MySqlConnection("DataBase=tfg;DataSource=localhost;user=root;Port=3306");
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}