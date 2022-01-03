using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_UCComp_Ciclo
    {
        public static DataTable PerfilComp_Ciclo_List(E_UCComp_Ciclo E_UCComp_Ciclo)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Ciclo_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_UCComp_Ciclo.IdPerfil;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Item_Ciclo_List(E_UCComp E_UCComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Item_Ciclo_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_UCComp.IdUC;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
    }
}
