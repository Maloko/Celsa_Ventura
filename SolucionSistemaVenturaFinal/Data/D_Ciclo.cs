using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_Ciclo
    {
        public static int Ciclo_Insert(E_Ciclo E_Ciclo)
        {
            int Id = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Ciclo_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdCiclo", SqlDbType.Int).Value = E_Ciclo.Idciclo;
                cmd.Parameters.Add("@IdTipoCiclo", SqlDbType.Int).Value = E_Ciclo.Idtipociclo;
                cmd.Parameters.Add("@Ciclo", SqlDbType.VarChar, 100).Value = E_Ciclo.Ciclo;
                cmd.Parameters.Add("@IdEstadoPC", SqlDbType.Int).Value = E_Ciclo.Idestadopc;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_Ciclo.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_Ciclo.Idusuariocreacion;
                cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = E_Ciclo.Fechacreacion;
                cmd.Parameters.Add("@HostCreacion", SqlDbType.VarChar, 50).Value = E_Ciclo.Hostcreacion;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_Ciclo.Idusuariomodificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_Ciclo.Fechamodificacion;
                cmd.Parameters.Add("@HostModificacion", SqlDbType.VarChar, 50).Value = E_Ciclo.Hostcreacion;

                cmd.ExecuteNonQuery();
                Id = Int32.Parse(cmd.Parameters["@IdCiclo"].Value.ToString());
                cx.Close();
            }
            return Id;
        }


        public static DataTable Ciclo_GetItem(int idCiclo)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Ciclo_GetItem", cx);
                cmd.Parameters.AddWithValue("@IdCiclo", idCiclo);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }


        public static DataTable Ciclo_Combo()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Ciclo_Combo", cx);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Ciclo_List(E_Ciclo E_Ciclo)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Ciclo_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTipoCiclo", SqlDbType.Int).Value = E_Ciclo.Idtipociclo;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Ciclo_Update(E_Ciclo E_Ciclo)
        {
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Ciclo_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdCiclo", SqlDbType.Int).Value = E_Ciclo.Idciclo;
                cmd.Parameters.Add("@IdTipoCiclo", SqlDbType.Int).Value = E_Ciclo.Idtipociclo;
                cmd.Parameters.Add("@Ciclo", SqlDbType.VarChar, 100).Value = E_Ciclo.Ciclo;
                cmd.Parameters.Add("@IdEstadoPC", SqlDbType.Int).Value = E_Ciclo.Idestadopc;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_Ciclo.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_Ciclo.Idusuariocreacion;
                cmd.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = E_Ciclo.Fechacreacion;
                cmd.Parameters.Add("@HostCreacion", SqlDbType.VarChar, 50).Value = E_Ciclo.Hostcreacion;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_Ciclo.Idusuariomodificacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_Ciclo.Fechamodificacion;
                cmd.Parameters.Add("@HostModificacion", SqlDbType.VarChar, 50).Value = E_Ciclo.Hostcreacion;

                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
        }

        public static int Ciclo_Delete(int idCiclo)
        {
            int cant = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Ciclo_Delete", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdCiclo", SqlDbType.Int).Value = idCiclo;
                cant = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return cant;
        }

        public static DataTable Ciclo_ComboByPerfil(E_Perfil E_Perfil)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Ciclo_ComboByPerfil", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_Perfil.Idperfil;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Ciclo_ComboByUC(E_Perfil E_Perfil)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Ciclo_ComboByUC", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_Perfil.IdUC;
                cmd.Parameters.Add("@IdTipoCiclo", SqlDbType.Int).Value = E_Perfil.IdTipoCiclo;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
    }
}
