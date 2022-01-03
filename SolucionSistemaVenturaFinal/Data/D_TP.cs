using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_TP
    {
        public static int UCCompTransfer_UpdateCascade(E_TP E_TP)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCCompTransfer_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilCompOrigen", SqlDbType.Int).Value = E_TP.IdPerfilCompOrigen;
                cmd.Parameters.Add("@IdPerfilCompPadreOrigen", SqlDbType.Int).Value = E_TP.IdPerfilCompPadreOrigen;
                cmd.Parameters.Add("@IdUCOrigen", SqlDbType.Int).Value = E_TP.IdUCOrigen;
                cmd.Parameters.Add("@IdPerfilCompDestino", SqlDbType.Int).Value = E_TP.IdPerfilCompDestino;
                cmd.Parameters.Add("@IdPerfilCompPadreDestino", SqlDbType.Int).Value = E_TP.IdPerfilCompPadreDestino;
                cmd.Parameters.Add("@IdUCDestino", SqlDbType.Int).Value = E_TP.IdUCDestino;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_TP.IdPerfilComp;
                cmd.Parameters.Add("@IdUCComp", SqlDbType.Int).Value = E_TP.IdUCComp;
                cmd.Parameters.Add("@IdTipoTransfer", SqlDbType.Int).Value = E_TP.IdTipoTransfer;
                cmd.Parameters.Add("@FechaTransfer", SqlDbType.DateTime).Value = E_TP.FechaTransfer;
                cmd.Parameters.Add("@FechaDevolucion", SqlDbType.DateTime).Value = E_TP.FechaDevolucion;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = E_TP.Observacion;
                cmd.Parameters.Add("@Bitacora", SqlDbType.VarChar, 200).Value = E_TP.Bitacora;
                cmd.Parameters.Add("@IdEstadoTransfer", SqlDbType.Int).Value = E_TP.IdEstadoTransfer;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_TP.IdUsuario;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_TP.FechaModificacion;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static int UCCompTransfer_Delete(E_TP E_TP)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCCompTransfer_Delete", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUCCompTransfer", SqlDbType.Int).Value = E_TP.IdUCCompTransfer;
                cmd.Parameters.Add("@IdTipoTransfer", SqlDbType.Int).Value = E_TP.IdTipoTransfer;
                cmd.Parameters.Add("@FechaTransfer", SqlDbType.DateTime).Value = E_TP.FechaTransfer;
                cmd.Parameters.Add("@FechaDevolucion", SqlDbType.DateTime).Value = E_TP.FechaDevolucion;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = E_TP.Observacion;
                cmd.Parameters.Add("@IdEstadoTransfer", SqlDbType.Int).Value = E_TP.IdEstadoTransfer;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_TP.IdUsuario;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_TP.FechaModificacion;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static int UCCompTransfer_Update(E_TP E_TP)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCCompTransfer_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUCCompTransfer", SqlDbType.Int).Value = E_TP.IdUCCompTransfer;
                cmd.Parameters.Add("@IdTipoTransfer", SqlDbType.Int).Value = E_TP.IdTipoTransfer;
                cmd.Parameters.Add("@FechaTransfer", SqlDbType.DateTime).Value = E_TP.FechaTransfer;
                cmd.Parameters.Add("@FechaDevolucion", SqlDbType.DateTime).Value = E_TP.FechaDevolucion;
                cmd.Parameters.Add("@IdEstadoTransfer", SqlDbType.Int).Value = E_TP.IdEstadoTransfer;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = E_TP.Observacion;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = E_TP.IdUsuario;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_TP.FechaModificacion;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }
        public static DataTable UCCompTransfer_GetItem(E_TP E_TP)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCCompTransfer_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUCCompTransfer", SqlDbType.Int).Value = E_TP.IdUCCompTransfer;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
        public static DataTable UCCompTransfer_BeforeChange(E_TP E_TP)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UCCompTransfer_BeforeChange", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilCompOrigen", SqlDbType.Int).Value = E_TP.IdPerfilCompOrigen;
                cmd.Parameters.Add("@IdUCOrigen", SqlDbType.Int).Value = E_TP.IdUCOrigen;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
    }
}
