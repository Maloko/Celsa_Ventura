using System;
using System.Data;
using Entities;
using System.Data.SqlClient;

namespace Data
{

    public sealed class D_PerfilNeumaticoEje
    {


        public static int PerfilNeumaticoEje_Insert(E_PerfilNeumaticoEje E_PerfilNeumaticoEje)
        {
            int X = 0;

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumaticoEje_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilNeumaticoEje", SqlDbType.Int).Value = E_PerfilNeumaticoEje.IdPerfilNeumaticoEje;
                cmd.Parameters["@IdPerfilNeumaticoEje"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_PerfilNeumaticoEje.IdPerfilNeumatico;
                cmd.Parameters.Add("@Eje", SqlDbType.VarChar, 100).Value = E_PerfilNeumaticoEje.Eje;
                cmd.Parameters.Add("@NroLlantas", SqlDbType.Int).Value = E_PerfilNeumaticoEje.NroLlantas;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PerfilNeumaticoEje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_PerfilNeumaticoEje.IdUsuarioCreacion;
                cmd.ExecuteNonQuery();
                X = Int32.Parse(cmd.Parameters["@IdPerfilNeumaticoEje"].Value.ToString());
                cx.Close();
            }

            return X;

        }


        public static DataTable PerfilNeumaticoEje_GetItem(int idPerfilNeumaticoEje)
        {
            DataTable tbl = new DataTable();

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("PerfilNeumaticoEje_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPerfilNeumaticoEje", idPerfilNeumaticoEje);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }

            return tbl;
        }


        public static DataTable PerfilNeumaticoEje_Combo()
        {
            DataTable tbl = new DataTable();

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlDataAdapter da = new SqlDataAdapter("PerfilNeumaticoEje_Combo", cx);
                da.Fill(tbl);
                cx.Close();
            }

            return tbl;
        }

        public static DataTable PerfilNeumaticoEje_List(E_PerfilNeumaticoEje E_PerfilNeumaticoEje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumaticoEje_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_PerfilNeumaticoEje.IdPerfilNeumatico;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static string PerfilNeumaticoEje_Update(E_PerfilNeumaticoEje E_PerfilNeumaticoEje)
        {
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();

                SqlCommand cmd = new SqlCommand("PerfilNeumaticoEje_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilNeumatico", SqlDbType.Int).Value = E_PerfilNeumaticoEje.IdPerfilNeumatico;
                cmd.Parameters.Add("@Eje", SqlDbType.VarChar, 100).Value = E_PerfilNeumaticoEje.Eje;
                cmd.Parameters.Add("@NroLlantas", SqlDbType.Int).Value = E_PerfilNeumaticoEje.NroLlantas;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PerfilNeumaticoEje.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = E_PerfilNeumaticoEje.IdUsuarioModificacion;
                cmd.ExecuteNonQuery();
                cx.Close();
            }

            return "{Ok:ok}";
        }

        public static string PerfilNeumaticoEje_Delete(int idPerfilNeumatico)
        {
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PerfilNeumaticoEje_Delete", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPerfilNeumatico", idPerfilNeumatico);
                cmd.ExecuteNonQuery();
                cx.Close();
            }

            return "{OK:ok}";
        }


    }
}
