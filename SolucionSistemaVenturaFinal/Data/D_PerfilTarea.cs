using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
	public sealed class D_PerfilTarea
	{
        public static int PerfilTarea_Insert(E_PerfilTarea E_PerfilTarea)
		{
            int Id = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilTarea_Insert", cx);

                cmd.Parameters.Add("@IdPerfilTarea", SqlDbType.Int).Value = E_PerfilTarea.Idperfiltarea;
                cmd.Parameters.Add("@IdPerfilCompActividad", SqlDbType.Int).Value = E_PerfilTarea.Idperfilcompactividad;
                cmd.Parameters.Add("@IdTarea", SqlDbType.Int).Value = E_PerfilTarea.Idtarea;
                cmd.Parameters.Add("@HorasHombre", SqlDbType.Decimal).Value = E_PerfilTarea.Horashombre;
                cmd.Parameters.Add("@IdEstadoPT", SqlDbType.VarChar,50).Value = E_PerfilTarea.Idestadopt;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PerfilTarea.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilTarea.Idusuariocreacion;
                cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = E_PerfilTarea.Fechacreacion;
                cmd.Parameters.Add("@HostCreacion", SqlDbType.VarChar,50).Value = E_PerfilTarea.Hostcreacion;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_PerfilTarea.Idusuariomodificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value =  E_PerfilTarea.Fechamodificacion;
                cmd.Parameters.Add("@HostModificacion", SqlDbType.VarChar,50).Value = E_PerfilTarea.Hostmodificacion;

                cmd.ExecuteNonQuery();
                Id = Int32.Parse(cmd.Parameters["@IdCiclo"].Value.ToString());
                cx.Close();
            }
            return Id;
		}

		public static DataTable PerfilTarea_GetItem(int idPerfilTarea)
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilTarea_GetItem", cx);
                cmd.Parameters.AddWithValue("@IdPerfilTarea", idPerfilTarea);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

		public static DataTable PerfilTarea_Combo()
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilTarea_Combo", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

        public static DataTable PerfilTarea_List(E_PerfilTarea E_PerfilTarea)
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilTarea_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PerfilTarea.IdPerfil;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

        public static int PerfilTarea_Update(E_PerfilTarea E_PerfilTarea)
		{
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilTarea_Update", cx);

                cmd.Parameters.Add("@IdPerfilTarea", SqlDbType.Int).Value = E_PerfilTarea.Idperfiltarea;
                cmd.Parameters.Add("@IdPerfilCompActividad", SqlDbType.Int).Value = E_PerfilTarea.Idperfilcompactividad;
                cmd.Parameters.Add("@IdTarea", SqlDbType.Int).Value = E_PerfilTarea.Idtarea;
                cmd.Parameters.Add("@HorasHombre", SqlDbType.Decimal).Value = E_PerfilTarea.Horashombre;
                cmd.Parameters.Add("@IdEstadoPT", SqlDbType.VarChar, 50).Value = E_PerfilTarea.Idestadopt;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PerfilTarea.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilTarea.Idusuariocreacion;
                cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = E_PerfilTarea.Fechacreacion;
                cmd.Parameters.Add("@HostCreacion", SqlDbType.VarChar, 50).Value = E_PerfilTarea.Hostcreacion;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_PerfilTarea.Idusuariomodificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_PerfilTarea.Fechamodificacion;
                cmd.Parameters.Add("@HostModificacion", SqlDbType.VarChar, 50).Value = E_PerfilTarea.Hostmodificacion;
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;

		}

		public static int PerfilTarea_Delete(int idPerfilTarea)
		{
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilTarea_Delete", cx);

                cmd.Parameters.Add("@IdPerfilTarea", SqlDbType.Int).Value = idPerfilTarea;
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
		}
	}
}
