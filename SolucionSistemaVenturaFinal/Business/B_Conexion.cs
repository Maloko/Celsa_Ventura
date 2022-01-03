using Data;
using System.Data.SqlClient;
namespace Business
{
    public class B_Conexion
    {
        public SqlConnection ObtenerConexion()
        {
            return Conexion.ObtenerConexion();
        }

    }
}
