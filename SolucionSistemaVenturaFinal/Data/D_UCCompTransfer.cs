using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_UCCompTransfer
    {        
        public static DataTable UCCompTransfer_List(E_UCCompTransfer obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("UCCompTransfer_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstadoTransfer", SqlDbType.Int).Value = obje.IdEstadoTransfer;
                SqlDataAdapter da = new SqlDataAdapter(cmd);                
                cn.Open();
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }
    }
}
