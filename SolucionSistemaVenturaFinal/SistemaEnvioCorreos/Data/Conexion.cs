using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
namespace Data
{
    public class Conexion
    {
        public static SqlConnection ObtenerConexion()
        {
            //System.Configuration.ConfigurationManager.ConnectionStrings["BDVentura"].ConnectionString;
            string ConexionDb = "Server=" + ConfigurationManager.AppSettings["SERVER"] + "; " +
            " Integrated Security = False; " +
            "Database=" + ConfigurationManager.AppSettings["BD"]+ ";" +
            "Persist Security Info=False; " +
            "User=" + ConfigurationManager.AppSettings["USER"] + "; " +
            "Password=" + ConfigurationManager.AppSettings["PWD"] + ";";

            return new SqlConnection(ConexionDb);
        }
    }
}
