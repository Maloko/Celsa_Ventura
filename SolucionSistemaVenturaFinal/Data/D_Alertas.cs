using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_Alertas
    {
        public static DataTable Alertas_GetItems(E_Alertas E_Alertas)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Alertas_GetItems", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_Alertas.IdUsuario;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
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
