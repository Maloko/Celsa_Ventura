using System;
using System.Data;
using System.Data.SqlClient;
using Entities;
namespace Data
{

	public class D_PerfilComp
	{

		public static int PerfilComp_Insert(E_PerfilComp E_PerfilComp)
		{
            int Id = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp.Idperfilcomp;
                cmd.Parameters["@IdPerfilComp"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@PerfilComp", SqlDbType.VarChar, 100).Value = E_PerfilComp.Perfilcomp;
                cmd.Parameters.Add("@IdPerfilCompPadre", SqlDbType.Int).Value = E_PerfilComp.Idperfilcomppadre;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PerfilComp.Idperfil;
                cmd.Parameters.Add("@CodigoSAP", SqlDbType.VarChar, 20).Value =  E_PerfilComp.Codigosap;
                cmd.Parameters.Add("@DescripcionSAP",SqlDbType.VarChar, 100).Value =  E_PerfilComp.Descripcionsap;
                cmd.Parameters.Add("@IdEstadoPC", SqlDbType.Int).Value = E_PerfilComp.Idestadopc;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value =  E_PerfilComp.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilComp.Idusuariocreacion;

                cmd.ExecuteNonQuery();
                Id = Int32.Parse(cmd.Parameters["@IdPerfilComp"].Value.ToString());
                cx.Close();
            }
            return Id;
		}

        public static DataTable PerfilComp_GetItem(E_PerfilComp E_PerfilComp)
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_GetItem", cx);
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp.Idperfilcomp;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

		public static DataTable PerfilComp_Combo()
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Combo", cx);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

        public static DataTable PerfilComp_List(E_PerfilComp E_PerfilComp)
		{
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PerfilComp.Idperfil;
                cmd.Parameters.Add("@TipoEstado", SqlDbType.Int).Value = E_PerfilComp.Idestadopc;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
		}

        public static DataTable PerfilComp_ListWithNoParent(E_PerfilComp E_PerfilComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_ListWithNoParent", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PerfilComp.Idperfil;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;	
        }

        public static int PerfilComp_Update(E_PerfilComp E_PerfilComp)
		{
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp.Idperfilcomp;
                cmd.Parameters.Add("@CodPerfilComp", SqlDbType.VarChar, 20).Value = E_PerfilComp.Codperfilcomp;
                cmd.Parameters.Add("@PerfilComp", SqlDbType.VarChar, 20).Value = E_PerfilComp.Perfilcomp;
                cmd.Parameters.Add("@IdPerfilCompPadre", SqlDbType.Int).Value = E_PerfilComp.Idperfilcomppadre;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PerfilComp.Idperfil;
                cmd.Parameters.Add("@CodigoSAP", SqlDbType.VarChar, 20).Value = E_PerfilComp.Codigosap;
                cmd.Parameters.Add("@DescripcionSAP", SqlDbType.VarChar, 20).Value = E_PerfilComp.Descripcionsap;
                cmd.Parameters.Add("@IdEstadoPC", SqlDbType.Int).Value = E_PerfilComp.Idestadopc;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PerfilComp.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilComp.Idusuariocreacion;
                cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = E_PerfilComp.Fechacreacion;
                cmd.Parameters.Add("@HostCreacion", SqlDbType.VarChar, 20).Value = E_PerfilComp.Hostcreacion;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_PerfilComp.Idusuariomodificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_PerfilComp.Fechamodificacion;
                cmd.Parameters.Add("@HostModificacion", SqlDbType.VarChar, 20).Value = E_PerfilComp.Hostmodificacion;

                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
		}

        public static int PerfilComp_Delete(E_PerfilComp E_PerfilComp)
		{
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_Delete", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp.Idperfilcomp;
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
		}

        public static DataTable PerfilComp_GetBeforeDel(E_PerfilComp E_PerfilComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_GetBeforeDel", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp.Idperfilcomp;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable PerfilComp_GetBeforeChange(E_PerfilComp E_PerfilComp, int IdTipoConsulta)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilComp_GetBeforeChange", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTipoConsulta", SqlDbType.Int).Value = IdTipoConsulta;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PerfilComp.Idperfil;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Actividad_ComboByPerfil(E_PerfilComp E_PerfilComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Actividad_ComboByPerfil", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp.Idperfilcomp;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PerfilComp.Idperfil;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Tarea_ComboByPerfil(E_PerfilComp E_PerfilComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Tarea_ComboByPerfil", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp.Idperfilcomp;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PerfilComp.Idperfil;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable PerfilDetalle_ComboByPerfil(E_PerfilComp E_PerfilComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilDetalle_ComboByPerfil", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PerfilComp.Idperfilcomp;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PerfilComp.Idperfil;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

	}
}
