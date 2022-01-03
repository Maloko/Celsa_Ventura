using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public static class D_Rol
    {
        public static DataTable Rol_GetItemByUsuario(E_Usuario objE)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Rol_GetItemByUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = objE.IdUsuario;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;  
        }

        public static DataTable Rol_Combo()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter("Rol_Combo", cn);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static DataTable Rol_List(E_Rol obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Rol_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = obje.FlagActivo;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;               
        }

        public static int Rol_Insert(E_Rol obje)
        {
            int NewRol=0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Rol_Insert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = obje.IdRol;
                cmd.Parameters.Add("@Rol", SqlDbType.VarChar, 50).Value = obje.Rol;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                cn.Open();
                NewRol = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return NewRol;
        }

        public static DataTable Rol_Get(E_Rol obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Rol_GetItem", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = obje.IdRol;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static int Rol_Update(E_Rol obje)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Rol_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = obje.IdRol;
                cmd.Parameters.Add("@Rol", SqlDbType.VarChar, 50).Value = obje.Rol;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obje.IdUsuarioModificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = obje.FechaModificacion;
                cn.Open();
                n = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return n;
        }

        public static DataTable Menu_List()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter("Menu_List", cn);
                cn.Open();
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;            
        }
        public static int Rol_Menu_InsertMasivo(E_Rol obje)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("RolMenu_InsertMasivo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = obje.IdRol;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                cmd.Parameters.Add("@IdMenus", SqlDbType.VarChar).Value = obje.IdMenus2Insert;
                cn.Open();
                n = cmd.ExecuteNonQuery();
                cn.Close();
            }
            return n;
        }
        public static void Rol_Menu_UpdateMasivo(E_Rol obje)
        {
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("RolMenu_UpdateMasivo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = obje.IdRol;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obje.IdUsuarioModificacion;
                cmd.Parameters.Add("@IdMenus", SqlDbType.VarChar).Value = obje.IdMenus2Update;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public static DataTable Rol_Menu_List(E_Rol obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("RolMenu_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = obje.IdRol;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }
    }
}
