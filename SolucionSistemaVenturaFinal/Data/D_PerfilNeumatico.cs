using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{

    public class D_PerfilNeumatico
    {


        public static int PerfilNeumatico_Insert(E_PerfilNeumatico E_PerfilNeumatico)
        {
            int X = 0;

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumatico_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_PerfilNeumatico.IdPerfilNeumatico;
                cmd.Parameters["@IdPerfilNeumatico"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@CodPerfilNeumatico", SqlDbType.VarChar, 20).Value = E_PerfilNeumatico.CodPerfilNeumatico;
                cmd.Parameters.Add("@PerfilNeumatico", SqlDbType.VarChar, 50).Value = E_PerfilNeumatico.PerfilNeumatico;
                cmd.Parameters.Add("@NroEjes", SqlDbType.Int).Value = E_PerfilNeumatico.NroEjes;
                cmd.Parameters.Add("@NroLlantaRepuesto", SqlDbType.Int).Value = E_PerfilNeumatico.NroLlantaRepuesto;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = E_PerfilNeumatico.Observacion;
                cmd.Parameters.Add("@IdEstadoPN", SqlDbType.Int).Value = E_PerfilNeumatico.IdEstadoPN;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PerfilNeumatico.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilNeumatico.IdUsuarioCreacion;                
                cmd.ExecuteNonQuery();
                X = Int32.Parse(cmd.Parameters["@IdPerfilNeumatico"].Value.ToString());
                cx.Close();
            }

            return X;
        }


        public static DataTable PerfilNeumatico_GetItem(E_PerfilNeumatico E_PerfilNeumatico)
        {
            DataTable tbl = new DataTable();

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumatico_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_PerfilNeumatico.IdPerfilNeumatico;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }

            return tbl;
        }


        public static DataTable PerfilNeumatico_GetItemByDesc(E_PerfilNeumatico E_PerfilNeumatico)
        {
            DataTable tbl = new DataTable();

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumatico_GetItemByDesc", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_PerfilNeumatico.IdPerfilNeumatico;
                cmd.Parameters.Add("@PerfilNeumatico", SqlDbType.VarChar, 50).Value = E_PerfilNeumatico.PerfilNeumatico;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }

            return tbl;
        }

        public static DataTable PerfilNeumatico_Combo()
        {
            DataTable tbl = new DataTable();

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlDataAdapter da = new SqlDataAdapter("PerfilNeumatico_Combo", cx);
                da.Fill(tbl);
                cx.Close();
            }

            return tbl;
        }


        public static DataTable PerfilNeumatico_List(E_PerfilNeumatico E_PerfilNeumatico)
        {
            DataTable tbl = new DataTable();

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumatico_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PerfilNeumatico.FlagActivo;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }

            return tbl;
        }

        public static string PerfilNeumatico_Update(E_PerfilNeumatico E_PerfilNeumatico)
        {

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {

                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumatico_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_PerfilNeumatico.IdPerfilNeumatico;
                cmd.Parameters.Add("@CodPerfilNeumatico", SqlDbType.VarChar, 20).Value = E_PerfilNeumatico.CodPerfilNeumatico;
                cmd.Parameters.Add("@PerfilNeumatico", SqlDbType.VarChar, 50).Value = E_PerfilNeumatico.PerfilNeumatico;
                cmd.Parameters.Add("@NroEjes", SqlDbType.Int).Value = E_PerfilNeumatico.NroEjes;
                cmd.Parameters.Add("@NroLlantaRepuesto", SqlDbType.Int).Value = E_PerfilNeumatico.NroLlantaRepuesto;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = E_PerfilNeumatico.Observacion;
                cmd.Parameters.Add("@IdEstadoPN", SqlDbType.Int).Value = E_PerfilNeumatico.IdEstadoPN;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PerfilNeumatico.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioModificación", SqlDbType.Int).Value = E_PerfilNeumatico.IdUsuarioModificacion;
                cmd.ExecuteNonQuery();
                cx.Close();
            }

            return "{OK:ok}";
        }


        public static string PerfilNeumatico_Delete(E_PerfilNeumatico E_PerfilNeumatico)
        {
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumatico_Delete", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_PerfilNeumatico.IdPerfilNeumatico;
                cmd.ExecuteNonQuery();
                cx.Close();
            }

            return "{OK:ok}";
        }

        public static int PerfilNeumatico_UpdateCascade(E_PerfilNeumatico objE, DataTable tblPerfilNeumaticoEje)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumatico_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = objE.IdPerfilNeumatico;                
                cmd.Parameters.Add("@CodPerfilNeumatico", SqlDbType.VarChar,20).Value = objE.CodPerfilNeumatico;
                cmd.Parameters.Add("@PerfilNeumatico", SqlDbType.VarChar, 50).Value = objE.PerfilNeumatico;
                cmd.Parameters.Add("@NroEjes", SqlDbType.Int).Value = objE.NroEjes;
                cmd.Parameters.Add("@NroLlantaRepuesto", SqlDbType.Int).Value = objE.NroLlantaRepuesto;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar,200).Value = objE.Observacion;
                cmd.Parameters.Add("@IdEstadoPN", SqlDbType.Int).Value = objE.IdEstadoPN;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = objE.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = objE.IdUsuarioCreacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = objE.FechaModificacion;
                cmd.Parameters.Add("@tblPerfilNeumaticoEje", SqlDbType.Structured).Value = tblPerfilNeumaticoEje;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static int PerfilNeumatico_InsertMasivo(E_PerfilNeumatico objE, DataTable tblPerfilNeumatico, DataTable tblPerfilNeumaticoEje)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumatico_InsertMasivo", cx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@tblPerfilNeumatico", SqlDbType.Structured).Value = tblPerfilNeumatico;
                cmd.Parameters.Add("@tblPerfilNeumaticoEje", SqlDbType.Structured).Value = tblPerfilNeumaticoEje;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = objE.IdUsuarioCreacion;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }
    }
}
