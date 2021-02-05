using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace _100DaysOfCode_ASP_MVC.Models
{
    public class conexion
    {
        protected SqlConnection conn;

        string user = "TuUsuario";
        string pass = "TuContraseña";
        string bd = "TuBaseDeDatos";
        string server = "TuServer";

        protected void EstablecerConexion()
        {
            string cs = "User ID="+user+ "; Password="+pass+ ";Initial Catalog="+bd+ "; Server="+server+";";

            conn = new SqlConnection(cs);
        }
    }
}