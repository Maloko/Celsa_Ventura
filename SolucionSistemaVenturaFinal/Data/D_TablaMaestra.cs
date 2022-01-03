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

        public static DataTable TablaMaestra_List(E_TablaMaestra objE)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("TablaMaestra_List", cx);
                cx.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTabla", SqlDbType.Int).Value = objE.IdTabla;
                cmd.Parameters.Add("@IdColumna", SqlDbType.Int).Value = objE.IdColumna;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = objE.FlagActivo;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable TablaMaestra_GetItem(E_TablaMaestra obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("TablaMaestra_GetItem", cx);
                cx.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTabla", SqlDbType.Int).Value = obje.IdTabla;
                cmd.Parameters.Add("@IdColumna", SqlDbType.Int).Value = obje.IdColumna;
                SqlDataAdapter da = new SqlDataAdapter(cmd);                
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int TablaMaestra_Insert(E_TablaMaestra obje)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("TablaMaestra_Insert", cx);
                cx.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTabla", SqlDbType.Int).Value = obje.IdTabla;
                cmd.Parameters.Add("@IdColumna", SqlDbType.Int).Value = obje.IdColumna;
                cmd.Parameters.Add("@Valor", SqlDbType.VarChar, 100).Value = obje.Valor;
                cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 500).Value = obje.Descripcion;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                n = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return n;
        }

        public static int TablaMaestra_Update(E_TablaMaestra obje)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("TablaMaestra_Update", cx);
                cx.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTabla", SqlDbType.Int).Value = obje.IdTabla;
                cmd.Parameters.Add("@IdColumna", SqlDbType.Int).Value = obje.IdColumna;
                cmd.Parameters.Add("@Valor", SqlDbType.VarChar, 100).Value = obje.Valor;
                cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 500).Value = obje.Descripcion;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                n = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return n;
        }

        public static int TablaMaestra_UpdateMasivo(E_TablaMaestra objE, DataTable tblTablaMaestra)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("TablaMaestra_UpdateMasivo", cx);
                cx.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = objE.IdUsuarioCreacion;
                cmd.Parameters.Add("@tblTablaMaestra", SqlDbType.Structured).Value = tblTablaMaestra;
                n = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return n;
        }


    }
}
