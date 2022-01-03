using System;
using System.Data;
using System.Data.SqlClient;
using Entities;
namespace Data
{
    public class D_HerramientaItem
    {        
        public static DataTable HerramientaItem_List(E_HerramientaItem obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("HerramientaItem_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = obje.IdHerramienta;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cn.Open();
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static int HerramientaItem_Delete(E_HerramientaItem obje)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("HerramientaItem_Delete", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramientaItem", SqlDbType.Int).Value = obje.IdHerramientaItem;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obje.IdUsuarioModificacion;
                cn.Open();
                n = cmd.ExecuteNonQuery();
                cn.Close();
            }
            return n;
        }

        public static int HerramientaItem_Insert(E_HerramientaItem obj)
        {
            int IdNuevo = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("HerramientaItem_Insert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramientaItem", SqlDbType.Int).Value = obj.IdHerramientaItem;
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = obj.IdHerramienta;
                cmd.Parameters.Add("@NroSerie", SqlDbType.VarChar, 50).Value = obj.NroSerie;
                cmd.Parameters.Add("@IdEstadoDisponible", SqlDbType.Int).Value = obj.IdEstadoDisponible;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = obj.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = obj.IdUsuarioCreacion;
                cn.Open();
                IdNuevo = cmd.ExecuteNonQuery();
                IdNuevo = Int32.Parse(cmd.Parameters["@IdHerramienta"].Value.ToString());
                cn.Close();
            }
            return IdNuevo;
        }

        public static DataTable HerramientaItem_GetItem(E_HerramientaItem obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("HerramientaItem_GetItem", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramientaItem", SqlDbType.Int).Value = obje.IdHerramientaItem;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cn.Open();
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }
        public static int HerramientaItem_Update(E_HerramientaItem obj)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("HerramientaItem_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramientaItem", SqlDbType.Int).Value = obj.IdHerramientaItem;
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = obj.IdHerramienta;
                cmd.Parameters.Add("@NroSerie", SqlDbType.VarChar, 50).Value = obj.NroSerie;
                cmd.Parameters.Add("@IdEstadoDisponible", SqlDbType.Int).Value = obj.IdEstadoDisponible;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = obj.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obj.IdUsuarioModificacion;
                cn.Open();                
                n = cmd.ExecuteNonQuery();
                cn.Close();
            }
            return n;
        }
        public static int HerramientaItem_DeleteMasivo(E_HerramientaItem obj)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("HerramientaItem_DeleteMasivo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = obj.IdHerramienta;
                cn.Open();
                n = cmd.ExecuteNonQuery();
                cn.Close();
            }
            return n;
        }
        public static DataTable HerramientaItem_GetItemByDesc(E_HerramientaItem E_HerramientaItem)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("HerramientaItem_GetItemByDesc", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = E_HerramientaItem.IdHerramienta;
                cmd.Parameters.Add("@NroSerie", SqlDbType.VarChar, 50).Value = E_HerramientaItem.NroSerie;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cn.Open();
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }
    }
}
