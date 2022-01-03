using System;
using Entities;
using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public class D_Neumatico_Control
    {
        public static DataTable Neumatico_Control_List(E_Neumatico_Control E_Neumatico_Control)
        {
            DataTable tbl = new DataTable();

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("NeumaticoControl_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstadoNC", SqlDbType.Int).Value = E_Neumatico_Control.IdEstadoNC;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable NeumaticoControl_GetItem(E_Neumatico_Control E_Neumatico_Control)
        {
            DataTable tbl = new DataTable();

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("NeumaticoControl_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNC", SqlDbType.Int).Value = E_Neumatico_Control.IdNC;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int NeumaticoControl_UpdateCascada(E_Neumatico_Control E_Neumatico_Control, DataTable tblNCDet)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("NeumaticoControl_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdNC", SqlDbType.Int).Value = E_Neumatico_Control.IdNC;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@CodNC", SqlDbType.VarChar, 20).Value = E_Neumatico_Control.CodNC;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_Neumatico_Control.IdUC;
                cmd.Parameters.Add("@FechaControl", SqlDbType.VarChar, 14).Value = E_Neumatico_Control.FechaControl.ToString("yyyyMMdd HH:mm");
                cmd.Parameters.Add("@IdEstadoNC", SqlDbType.Int).Value = E_Neumatico_Control.IdEstadoNC;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_Neumatico_Control.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_Neumatico_Control.IdUsuario;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_Neumatico_Control.FechaModificacion;
                cmd.Parameters.Add("@tblNCDet", SqlDbType.Structured).Value = tblNCDet;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

    }
}
