using Entities;
using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public class D_Neumatico_ControlDet
    {
        public static DataTable D_NeumaticoControlDet_List(E_Neumatico_ControlDet E_Neumatico_ControlDet)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("NeumaticoControlDet_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNC", SqlDbType.Int).Value = E_Neumatico_ControlDet.IdNC;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable D_NeumaticoControlDet_ListByUC(E_Neumatico_ControlDet E_Neumatico_ControlDet)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("NeumaticoControlDet_ListByUC", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_Neumatico_ControlDet.IdUC;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        

    }
}
