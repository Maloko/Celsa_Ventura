using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_Usuario
    {
        public static DataTable Usuario_List(E_Usuario obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Usuario_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = obje.FlagActivo;
                SqlDataAdapter da = new SqlDataAdapter(cmd);                
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Usuario_Insert(E_Usuario obje)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Usuario_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = obje.IdUsuario;
                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 50).Value = obje.Codigo;
                cmd.Parameters.Add("@Apellidos", SqlDbType.VarChar, 50).Value = obje.Apellidos;
                cmd.Parameters.Add("@Nombres", SqlDbType.VarChar, 50).Value = obje.Nombres;
                cmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 50).Value = obje.Usuario;
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = obje.IdRol;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 200).Value = obje.Email;
                cmd.Parameters.Add("@Licenciado", SqlDbType.Bit).Value = obje.Licenciado;
                cmd.Parameters.Add("@FlagManager", SqlDbType.Bit).Value = obje.FlagManager;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                cx.Open();
                n = Convert.ToInt32(cmd.ExecuteScalar());
                cx.Close();
            }
            return n;
        }

        public static int Usuario_Update(E_Usuario obje)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Usuario_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = obje.IdUsuario;
                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 50).Value = obje.Codigo;
                cmd.Parameters.Add("@Apellidos", SqlDbType.VarChar, 50).Value = obje.Apellidos;
                cmd.Parameters.Add("@Nombres", SqlDbType.VarChar, 50).Value = obje.Nombres;
                cmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 50).Value = obje.Usuario;
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = obje.IdRol;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 200).Value = obje.Email;
                cmd.Parameters.Add("@Licenciado", SqlDbType.Bit).Value = obje.Licenciado;
                cmd.Parameters.Add("@FlagManager", SqlDbType.Bit).Value = obje.FlagManager;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obje.IdUsuarioModificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = obje.FechaModificacion;
                cx.Open();
                n = Convert.ToInt32(cmd.ExecuteScalar());
                cx.Close();
            }
            return n;
        }

        public static DataTable Usuario_GetItem(E_Usuario obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Usuario_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = obje.IdUsuario;
                cmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 20).Value = obje.Usuario;
                SqlDataAdapter da = new SqlDataAdapter(cmd);                
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }


        public static int UsuarioBloqueo_Insert(E_Usuario objE)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UsuarioBloqueo_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdBloqueo", objE.IdBloqueo);
                cmd.Parameters.AddWithValue("@Usuario", objE.Usuario);
                n = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return n;
        }

        public static DataTable UsuarioBloqueo_GetItem(E_Usuario objE)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UsuarioBloqueo_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Usuario", objE.Usuario);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        #region REQUERIMIENTO_03_CELSA
        public static DataTable Usuario_CorreoG()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Usuario_CorreoG", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
        #endregion

    }
}
