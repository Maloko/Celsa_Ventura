using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_OTComp
    {
        public static DataTable OTComp_List(E_OTComp E_OTComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("OTComp_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdOT", SqlDbType.Int).Value = E_OTComp.IdOT;
                cmd.Parameters.Add("@CodUC", SqlDbType.VarChar,20).Value = E_OTComp.CodUC;
                cmd.Parameters.Add("@IdItem", SqlDbType.VarChar, 20).Value = E_OTComp.IdItem;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
        }

        public static DataSet OT_ListCascade(E_OTComp E_OTComp)
        {
            DataSet tbl = new DataSet();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("OT_ListCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdOT", SqlDbType.Int).Value = E_OTComp.IdOT;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataSet OT_ListCascadeDet(E_OTComp E_OTComp)
        {
            DataSet tbl = new DataSet();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("OT_ListCascadeDet", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdOT", SqlDbType.Int).Value = E_OTComp.IdOT;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int OTTarea_UpdateCascade(E_OT E_OT, DataTable tblOTActividad, DataTable tblOTTareaDetalle, DataTable tblOTHerramienta, DataTable tblOTArticulo, DataTable tblOTArticuloDet)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("OTTarea_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdOT", SqlDbType.Int).Value = E_OT.IdOT;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FechaCierre", SqlDbType.VarChar,17).Value = Convert.ToDateTime(E_OT.FechaCierre).ToString("yyyyMMdd HH:mm");
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_OT.IdUsuario;
                cmd.Parameters.Add("@tblOTActividad", SqlDbType.Structured).Value = tblOTActividad;
                cmd.Parameters.Add("@tblOTTareaDetalle", SqlDbType.Structured).Value = tblOTTareaDetalle;
                cmd.Parameters.Add("@tblOTHerramienta", SqlDbType.Structured).Value = tblOTHerramienta;
                cmd.Parameters.Add("@tblOTArticulo", SqlDbType.Structured).Value = tblOTArticulo;
                cmd.Parameters.Add("@tblOTArticuloDet", SqlDbType.Structured).Value = tblOTArticuloDet;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_OT.FechaModificacion;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }
    }
}
