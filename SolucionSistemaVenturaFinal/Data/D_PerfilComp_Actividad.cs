using System;
using System.Data;
using System.Data.SqlClient;
using Entities;
namespace Data
{
	public sealed class D_PerfilComp_Actividad
	{
        public static int PerfilComp_Actividad_Insert(E_PerfilComp_Actividad E_PerfilComp_Actividad)
		{
			int Id = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Actividad_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilCompActividad", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idperfilcompactividad;
                cmd.Parameters["@IdPerfilCompActividad"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idperfilcomp;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idactividad;
                cmd.Parameters.Add("@FlagUso", SqlDbType.Bit).Value = E_PerfilComp_Actividad.Flaguso;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value  =  E_PerfilComp_Actividad.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idusuariocreacion;

                cmd.ExecuteNonQuery();
                Id = Int32.Parse(cmd.Parameters["@IdPerfilCompActividad"].Value.ToString());
                cx.Close();
            }
            return Id;
		}

        public static DataTable PerfilComp_Actividad_GetItem(E_PerfilComp_Actividad E_PerfilComp_Actividad)
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Actividad_GetItem", cx);
                cmd.Parameters.Add("@IdPerfilCompActividad", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idperfilcompactividad;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

		public static DataTable PerfilComp_Actividad_Combo()
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Actividad_Combo", cx);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

        public static DataTable PerfilComp_Actividad_List(E_Perfil E_Perfil)
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Actividad_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_Perfil.Idperfil;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

        public static int PerfilComp_Actividad_Update(E_PerfilComp_Actividad E_PerfilComp_Actividad)
		{
			int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Actividad_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilCompActividad", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idperfilcompactividad;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idperfilcomp;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idactividad;
                cmd.Parameters.Add("@FlagUso", SqlDbType.Bit).Value = E_PerfilComp_Actividad.Flaguso;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value  =  E_PerfilComp_Actividad.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idusuariocreacion;
                cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = E_PerfilComp_Actividad.Fechacreacion;
                cmd.Parameters.Add("@HostCreacion", SqlDbType.VarChar, 100).Value = E_PerfilComp_Actividad.Hostcreacion;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idusuariomodificación;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value =  E_PerfilComp_Actividad.Fechamodificacion;
                cmd.Parameters.Add("@HostModificacion", SqlDbType.VarChar, 100).Value = E_PerfilComp_Actividad.Hostmodificacion;
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
		}

        public static int PerfilComp_Actividad_Delete(E_PerfilComp_Actividad E_PerfilComp_Actividad)
		{
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Actividad_Delete", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilCompActividad", SqlDbType.Int).Value = E_PerfilComp_Actividad.Idperfilcompactividad;
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
		}
	}
}
