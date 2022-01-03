using System;
using System.Data;
using System.Data.SqlClient;
using Entities;


namespace Data
{
    public class D_UCComp
    {
       
        public static DataTable UCComp_List(E_UCComp E_UCComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCComp_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_UCComp.IdPerfil;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_UCComp.IdUC;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable UCComp_ListWithNoParent(E_UCComp E_UCComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCComp_ListWithNoParent", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_UCComp.IdPerfil;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_UCComp.IdUC;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable UCComp_GetBeforeChange(E_UCComp E_UCComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCComp_GetBeforeChange", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUCComp", SqlDbType.Int).Value = E_UCComp.IdUCComp;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        //UCComp_GetBeforeChange
        public static int UCComp_UpdateCascade(E_UCComp obje, DataTable tblUCCompCambioPerfil)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCComp_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = obje.IdUC;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = obje.IdPerfil;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = obje.FechaModificacion;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@tblUCCompCambioPerfil", SqlDbType.Structured).Value = tblUCCompCambioPerfil;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

    }
}
