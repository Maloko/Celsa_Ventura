using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_CargaMasiva
    {
        public static DataTable CargaMasiva_GetItem(E_CargaMasiva objE)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("CargaMasiva_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdCarga", SqlDbType.Int).Value = objE.IdCargaMasiva;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
    }
}
