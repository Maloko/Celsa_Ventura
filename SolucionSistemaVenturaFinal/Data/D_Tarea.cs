using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_Tarea
    {        
        public static DataTable Tarea_List(E_Tarea obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Tarea_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstadoT", SqlDbType.Int).Value = obje.IdEstadoT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
            }
            return tbl;
        }

        public static DataTable Tarea_GetItem(E_Tarea obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Tarea_GetItem", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTarea", SqlDbType.Int).Value = obje.IdTarea;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
            }
            return tbl;
        }
        public static DataTable Tarea_GetItemByDesc(E_Tarea obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Tarea_GetItemByDesc", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTarea", SqlDbType.Int).Value = obje.IdTarea;
                cmd.Parameters.Add("@Tarea", SqlDbType.VarChar, 50).Value = obje.Tarea;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
        public static int Tarea_Update(E_Tarea obje)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Tarea_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTarea", SqlDbType.Int).Value = obje.IdTarea;
                cmd.Parameters.Add("@CodTarea", SqlDbType.VarChar, 20).Value = obje.CodTarea;
                cmd.Parameters.Add("@Tarea", SqlDbType.VarChar, 100).Value = obje.Tarea;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = obje.IdActividad;
                cmd.Parameters.Add("@IdEstadoT", SqlDbType.Int).Value = obje.IdEstadoT;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obje.IdUsuarioModificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = obje.FechaModificacion;
                cn.Open();
                n = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return n;
        }

        public static int Tarea_Insert(E_Tarea obje)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Tarea_Insert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTarea", SqlDbType.Int).Value = obje.IdTarea;
                cmd.Parameters.Add("@CodTarea", SqlDbType.VarChar, 20).Value = obje.CodTarea;
                cmd.Parameters.Add("@Tarea", SqlDbType.VarChar, 100).Value = obje.Tarea;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = obje.IdActividad;
                cmd.Parameters.Add("@IdEstadoT", SqlDbType.Int).Value = obje.IdEstadoT;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                cn.Open();
                n = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return n;
        }

        public static DataTable Tarea_Combo()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Tarea_Combo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cn.Open();
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static DataTable Tarea_ComboByAct(E_Tarea E_Tarea)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Tarea_ComboByAct", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = E_Tarea.IdActividad;
                cmd.Parameters.Add("@Actividad", SqlDbType.VarChar, 100).Value = E_Tarea.Actividad;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
    }
}
