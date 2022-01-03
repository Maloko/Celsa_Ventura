using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_Alertas
    {
        public static DataSet Alerta_Call()
        {
            DataSet tbl = new DataSet();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlDataAdapter da = new SqlDataAdapter("VS_SP_Alerta_Call", cx);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Alertas_Envio_Log_GetReenvios()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlDataAdapter da = new SqlDataAdapter("Alertas_Envio_Log_GetReenvios", cx);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Usuario_ListByFilterType()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlDataAdapter da = new SqlDataAdapter("Usuario_ListByFilterType", cx);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Alertas_Envio_Log_UpdateCascade(E_Alertas E_Alertas, DataTable tblAlertasLog)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Alertas_Envio_Log_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_Alertas.IdUsuarioCreacion;
                cmd.Parameters.Add("@tblAlertas_Envio_Log", SqlDbType.Structured).Value = tblAlertasLog;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static int Alertas_UpdateCascade(E_Alertas E_Alertas, DataTable tblAlertas)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Alertas_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_Alertas.IdUsuarioCreacion;
                cmd.Parameters.Add("@tblAlertas", SqlDbType.Structured).Value = tblAlertas;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }
    }
}
