using System.Data.SqlClient;


namespace Data
{
    public static class Conexion
    {

        public static SqlConnection ObtenerConexion()
        {
            var ConexionDb = System.Configuration.ConfigurationManager.ConnectionStrings["BDVentura"].ConnectionString;

            return new SqlConnection(ConexionDb);
        }


    }
}
