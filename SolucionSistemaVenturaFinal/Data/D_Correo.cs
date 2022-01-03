using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public static class D_Correo
    {
        public static DataTable Correo_List()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Correo_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }
    }
}
