using System;
using System.Data;
using Entities;
using System.Data.SqlClient;

namespace Data
{
    public class D_UCIncidencia
    {
        public static DataTable UCIncidencia_Combo()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlDataAdapter da = new SqlDataAdapter("UCIncidencia_Combo", cx);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable UCIncidencia_List(E_UCIncidencia E_UCIncidencia)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCIncidencia_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_UCIncidencia.FlagActivo;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int UCIncidencia_Delete(E_UCIncidencia E_UCIncidencia)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCIncidencia_Delete", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUCIncidencia", SqlDbType.Int).Value = E_UCIncidencia.IdUCIncidencia;
                rpta = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return rpta;
        }

        public static int UCIncidencia_UpdateCascade(E_UCIncidencia E_UCIncidencia, DataTable tblUCIncidenciaDet, out string DescError)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();                
                SqlCommand cmd = new SqlCommand("UCIncidencia_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUCIncidencia", SqlDbType.Int).Value = E_UCIncidencia.IdUCIncidencia;
                cmd.Parameters.Add("@CodIncidencia", SqlDbType.VarChar, 20).Value = E_UCIncidencia.CodIncidencia;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_UCIncidencia.IdUC;
                cmd.Parameters.Add("@FechaIncidencia", SqlDbType.DateTime).Value = E_UCIncidencia.FechaIncidencia;
                cmd.Parameters.Add("@CodSolicitante", SqlDbType.VarChar, 20).Value = E_UCIncidencia.CodSolicitante;
                cmd.Parameters.Add("@CodResponsable", SqlDbType.VarChar, 20).Value = E_UCIncidencia.CodResponsable;
                cmd.Parameters.Add("@IdEstadoIncidencia", SqlDbType.Int).Value = E_UCIncidencia.IdEstadoIncidencia;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_UCIncidencia.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_UCIncidencia.IdUsuario;
                cmd.Parameters.Add("@tblUCIncidenciaDet", SqlDbType.Structured).Value = tblUCIncidenciaDet;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@DescError", SqlDbType.VarChar).Value = "";
                cmd.Parameters["@DescError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                DescError = cmd.Parameters["@DescError"].Value.ToString();
                cx.Close();
            }
            return rpta;
        }

        public static DataTable UCIncidencia_GetItem(E_UCIncidencia E_UCIncidencia)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCIncidencia_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUCIncidencia", SqlDbType.Int).Value = E_UCIncidencia.IdUCIncidencia;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
    }
}
