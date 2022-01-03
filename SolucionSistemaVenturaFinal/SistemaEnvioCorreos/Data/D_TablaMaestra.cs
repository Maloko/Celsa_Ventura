using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entities;
using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public static class D_TablaMaestra
    {
        public static DataTable TablaMaestra_Combo(E_TablaMaestra E_TablaMaestra)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("TablaMaestra_Combo", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdTabla", E_TablaMaestra.IdTabla);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int TablaMaestra_UpdateMasivo(E_TablaMaestra E_TablaMaestra, DataTable tblTablaMaestra)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("TablaMaestra_UpdateMasivo", cx);
                cx.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = E_TablaMaestra.IdUsuarioCreacion;
                cmd.Parameters.Add("@tblTablaMaestra", SqlDbType.Structured).Value = tblTablaMaestra;
                n = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return n;
        }
    }
}
