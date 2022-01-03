using System.Data;
using Entities;
using System.Data.SqlClient;

namespace Data
{
    public class D_UCIncidenciaDet
    {
        //        [dbo].[UCIncidenciaDet_List]
        //@IdUC INT
        public static DataTable UCIncidenciaDet_List(E_UCIncidenciaDet E_UCIncidenciaDet)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCIncidenciaDet_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUCIncidencia", SqlDbType.Int).Value = E_UCIncidenciaDet.IdUCIncidencia;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_UCIncidenciaDet.IdUC;
                cmd.Parameters.Add("@FlagNuevo", SqlDbType.Int).Value = E_UCIncidenciaDet.FlagNuevo;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable UCIncidenciaDet_GetItem(E_UCIncidenciaDet E_UCIncidenciaDet)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCIncidenciaDet_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUCIncidencia", SqlDbType.Int).Value = E_UCIncidenciaDet.IdUCIncidencia;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
    }
}
