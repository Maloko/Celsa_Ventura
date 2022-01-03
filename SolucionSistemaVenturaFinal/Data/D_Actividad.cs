using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_Actividad
    {
        
        public static DataTable Actividad_Combo()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlDataAdapter da = new SqlDataAdapter("Actividad_Combo", cx);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Actividad_List(E_Actividad obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Actividad_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstado", SqlDbType.Int).Value = obje.IdEstadoActividad;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Actividad_GetItem(E_Actividad obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Actividad_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = obje.IdActividad;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
            }
            return tbl;
        }

        public static DataTable Actividad_GetItemByDesc(E_Actividad obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Actividad_GetItemByDesc", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = obje.IdActividad;
                cmd.Parameters.Add("@Actividad", SqlDbType.VarChar, 50).Value = obje.Actividad;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Actividad_Update(E_Actividad obje)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Actividad_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = obje.IdActividad;
                cmd.Parameters.Add("@CodActividad", SqlDbType.VarChar, 20).Value = obje.CodActividad;
                cmd.Parameters.Add("@Actividad", SqlDbType.VarChar, 100).Value = obje.Actividad;
                cmd.Parameters.Add("@IdEstadoActividad", SqlDbType.Int).Value = obje.IdEstadoActividad;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obje.IdUsuarioModificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = obje.FechaModificacion;
                cx.Open();                
                n = Convert.ToInt32(cmd.ExecuteScalar());
                cx.Close();
            }
            return n;
        }

        public static int Actividad_Insert(E_Actividad obje)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Actividad_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = obje.IdActividad;
                cmd.Parameters.Add("@CodActividad", SqlDbType.VarChar, 20).Value = obje.CodActividad;
                cmd.Parameters.Add("@Actividad", SqlDbType.VarChar, 100).Value = obje.Actividad;
                cmd.Parameters.Add("@IdEstadoActividad", SqlDbType.Int).Value = obje.IdEstadoActividad;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                cx.Open();
                n = Convert.ToInt32(cmd.ExecuteScalar());
                cx.Close();
            }
            return n;
        }

        public static int Actividad_GetBeforeChange(E_Actividad obje)
        {
            int Contador = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Actividad_GetBeforeChange", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = obje.IdActividad;
                cx.Open();
                Contador = Convert.ToInt32(cmd.ExecuteScalar());
                cx.Close();
            }
            return Contador;
        }

    }
}
