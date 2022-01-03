using System;
using System.Data.SqlClient;
using Entities;
using System.Data;

namespace Data
{
    public class D_FormatoImpresion
    {
        public static int FormatoImpresion_Insert(E_FormatoImpresion E_FormatoImpresion)
        {
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("VS_SP_FormatoImpresion_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdFormatoImpresion", SqlDbType.Int).Value = E_FormatoImpresion.IdFormatoImpresion;
                cmd.Parameters.Add("@IdMenu", SqlDbType.Int).Value = E_FormatoImpresion.IdMenu;
                cmd.Parameters.Add("@NombreArchivo", SqlDbType.VarChar, 100).Value = E_FormatoImpresion.NombreArchivo;
                cmd.Parameters.Add("@File", SqlDbType.VarBinary).Value = E_FormatoImpresion.File;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_FormatoImpresion.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_FormatoImpresion.Idusuariocreacion;                

                cmd.ExecuteNonQuery();
                E_FormatoImpresion.IdFormatoImpresion = Int32.Parse(cmd.Parameters["@IdFormatoImpresion"].Value.ToString());
                cx.Close();
            }
            return E_FormatoImpresion.IdFormatoImpresion;
        
        }

        public static DataTable FormatoImpresion_GetItem(int IdMenu)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("VS_SP_FormatoImpresion_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdMenu", SqlDbType.Int).Value = IdMenu;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable FormatoImpresion_GetFile(int IdFormatoImpresion)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("VS_SP_FormatoImpresion_GetFile", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdFormatoImpresion", SqlDbType.Int).Value = IdFormatoImpresion;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int FormatoImpresion_Update(E_FormatoImpresion E_FormatoImpresion)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("VS_SP_FormatoImpresion_Update", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdFormatoImpresion", SqlDbType.Int).Value = E_FormatoImpresion.IdFormatoImpresion;
                cmd.Parameters.Add("@IdMenu", SqlDbType.Int).Value = E_FormatoImpresion.IdMenu;
                cmd.Parameters.Add("@NombreArchivo", SqlDbType.VarChar, 100).Value = E_FormatoImpresion.NombreArchivo;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = E_FormatoImpresion.Flagactivo;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = E_FormatoImpresion.Idusuariomodificacion;
                n = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return n;
        }

    }
}
