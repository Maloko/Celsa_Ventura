using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
	public sealed class D_PerfilComp_Ciclo
	{
		public static int PerfilComp_Ciclo_Insert(E_PerfilComp_Ciclo E_PerfilComp_Ciclo)
		{
            int Id = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Ciclo_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdPerfilCompCiclo", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idperfilcompciclo;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idperfilcomp;
                cmd.Parameters.Add("@IdCiclo", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idciclo;
                cmd.Parameters.Add("@FrecuenciaCambio", SqlDbType.Decimal).Value =  E_PerfilComp_Ciclo.Frecuenciacambio;
                cmd.Parameters.Add("@IdEstadoPCC", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idestadopcc;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value= E_PerfilComp_Ciclo.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idusuariocreacion;
                cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value =  E_PerfilComp_Ciclo.Fechacreacion;
                cmd.Parameters.Add("@HostCreacion", SqlDbType.VarChar,100).Value = E_PerfilComp_Ciclo.Hostcreacion;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idusuariomodificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value= E_PerfilComp_Ciclo.Fechamodificacion;
                cmd.Parameters.Add("@HostModificacion", SqlDbType.VarChar, 100).Value = E_PerfilComp_Ciclo.Hostmodificacion;
                cmd.ExecuteNonQuery();
                Id = Int32.Parse(cmd.Parameters["@IdCiclo"].Value.ToString());
                cx.Close();
            }
            return Id;
		}

        public static DataTable PerfilComp_Ciclo_GetItem(E_PerfilComp_Ciclo E_PerfilComp_Ciclo)
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Ciclo_GetItem", cx);
                cmd.Parameters.Add("@IdPerfilCompCiclo", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idperfilcompciclo;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

		/// <summary>
		/// Selects all records from the PerfilComp_Ciclo table.
		/// <summary>
		/// <returns>DataSet</returns>
		public static DataTable PerfilComp_Ciclo_Combo()
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Ciclo_Combo", cx);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

        public static DataTable PerfilComp_Ciclo_List(E_Perfil E_Perfil)
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Ciclo_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_Perfil.Idperfil;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

        public static int PerfilComp_Ciclo_Update(E_PerfilComp_Ciclo E_PerfilComp_Ciclo)
		{
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Ciclo_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdPerfilCompCiclo", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idperfilcompciclo;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idperfilcomp;
                cmd.Parameters.Add("@IdCiclo", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idciclo;
                cmd.Parameters.Add("@FrecuenciaCambio", SqlDbType.Decimal).Value =  E_PerfilComp_Ciclo.Frecuenciacambio;
                cmd.Parameters.Add("@IdEstadoPCC", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idestadopcc;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value= E_PerfilComp_Ciclo.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idusuariocreacion;
                cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value =  E_PerfilComp_Ciclo.Fechacreacion;
                cmd.Parameters.Add("@HostCreacion", SqlDbType.VarChar,100).Value = E_PerfilComp_Ciclo.Hostcreacion;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idusuariomodificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value= E_PerfilComp_Ciclo.Fechamodificacion;
                cmd.Parameters.Add("@HostModificacion", SqlDbType.VarChar, 100).Value = E_PerfilComp_Ciclo.Hostmodificacion;
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
		}

        public static int PerfilComp_Ciclo_Delete(E_PerfilComp_Ciclo E_PerfilComp_Ciclo)
		{
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Ciclo_Delete", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilCompCiclo", SqlDbType.Int).Value = E_PerfilComp_Ciclo.Idperfilcompciclo;
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
		}
	}
}
