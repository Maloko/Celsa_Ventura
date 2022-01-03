using System;
using System.Data;
using System.Data.SqlClient;
using Entities;
namespace Data
{
	public sealed class D_PerfilDetalle
	{
        public static int PerfilDetalle_Insert(E_PerfilDetalle E_PerfilDetalle)
		{
            int Id = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilDetalle_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilDetalle", SqlDbType.Int).Value = E_PerfilDetalle.Idperfildetalle;
                cmd.Parameters.Add("@IdPerfilCompActividad", SqlDbType.Int).Value = E_PerfilDetalle.Idperfilcompactividad;
                cmd.Parameters.Add("@IdTipoArticulo", SqlDbType.Int).Value = E_PerfilDetalle.Idtipoarticulo;
                cmd.Parameters.Add("@IdArticulo", SqlDbType.VarChar,100).Value = E_PerfilDetalle.Idarticulo;
                cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = E_PerfilDetalle.Cantidad;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PerfilDetalle.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilDetalle.Idusuariocreacion;
                cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = E_PerfilDetalle.Fechacreacion;
                cmd.Parameters.Add("@HostCreacion", SqlDbType.VarChar,50).Value = E_PerfilDetalle.Hostcreacion;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_PerfilDetalle.Idusuariomodificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_PerfilDetalle.Fechamodificacion;
                cmd.Parameters.Add("@HostModificacion", SqlDbType.VarChar, 50).Value = E_PerfilDetalle.Hostmodificacion;
                cmd.ExecuteNonQuery();
                Id = Int32.Parse(cmd.Parameters["@IdCiclo"].Value.ToString());
                cx.Close();
            }
            return Id;
		}

        public static DataTable PerfilDetalle_GetItem(E_PerfilDetalle E_PerfilDetalle)
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilDetalle_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilDetalle", SqlDbType.Int).Value = E_PerfilDetalle.Idperfildetalle;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

		public static DataTable PerfilDetalle_Combo()
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilDetalle_Combo", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

        public static DataTable PerfilDetalle_List(E_Perfil E_Perfil)
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilDetalle_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_Perfil.Idperfil;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

        public static int PerfilDetalle_Update(E_PerfilDetalle E_PerfilDetalle)
		{
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilDetalle_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilDetalle", SqlDbType.Int).Value = E_PerfilDetalle.Idperfildetalle;
                cmd.Parameters.Add("@IdPerfilCompActividad", SqlDbType.Int).Value = E_PerfilDetalle.Idperfilcompactividad;
                cmd.Parameters.Add("@IdTipoArticulo", SqlDbType.Int).Value = E_PerfilDetalle.Idtipoarticulo;
                cmd.Parameters.Add("@IdArticulo", SqlDbType.VarChar,100).Value = E_PerfilDetalle.Idarticulo;
                cmd.Parameters.Add("@Cantidad", SqlDbType.Decimal).Value = E_PerfilDetalle.Cantidad;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PerfilDetalle.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilDetalle.Idusuariocreacion;
                cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = E_PerfilDetalle.Fechacreacion;
                cmd.Parameters.Add("@HostCreacion", SqlDbType.VarChar,50).Value = E_PerfilDetalle.Hostcreacion;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_PerfilDetalle.Idusuariomodificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_PerfilDetalle.Fechamodificacion;
                cmd.Parameters.Add("@HostModificacion", SqlDbType.VarChar, 50).Value = E_PerfilDetalle.Hostmodificacion;
                
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
		}

        public static int PerfilDetalle_Delete(E_PerfilDetalle E_PerfilDetalle)
		{
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilDetalle_Delete", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilDetalle", SqlDbType.Int).Value = E_PerfilDetalle.Idperfildetalle;
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
		}
	}
}
