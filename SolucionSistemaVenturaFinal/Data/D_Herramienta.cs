using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_Herramienta
    {
        public static DataTable Herramienta_Combo()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Herramienta_Combo", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Herramienta_List(E_Herramienta obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Herramienta_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstadoH", SqlDbType.Int).Value = obje.IdEstadoH;
                SqlDataAdapter da = new SqlDataAdapter(cmd);                
                cn.Open();
                da.Fill(tbl); 
                cn.Close(); 
            }
            return tbl;
        }

        public static int Herramienta_UpdateCascade(E_Herramienta obje, DataTable tblSerie)
        {
            int rpta = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Herramienta_UpdateCascade", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                //Neumatico
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = obje.IdHerramienta;
                cmd.Parameters.Add("@CodHerramienta", SqlDbType.VarChar, 20).Value = obje.CodHerramienta;
                cmd.Parameters.Add("@Herramienta", SqlDbType.VarChar, 100).Value = obje.Herramienta;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = obje.Observacion;
                cmd.Parameters.Add("@IdEstadoH", SqlDbType.Int).Value = obje.IdEstadoH;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Int).Value = obje.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = obje.IdUsuarioCreacion;
                
                cmd.Parameters.Add("@tblHerramientaItem", SqlDbType.Structured).Value = tblSerie;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = obje.FechaModificacion;
                cn.Open();
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                //Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return rpta;
        }
        public static int Herramienta_Insert(E_Herramienta obj)
        {
            int IdNuevo = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Herramienta_Insert", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = obj.IdHerramienta;
                cmd.Parameters["@IdHerramienta"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@CodHerramienta", SqlDbType.VarChar, 20).Value = obj.CodHerramienta;
                cmd.Parameters.Add("@Herramienta", SqlDbType.VarChar, 100).Value = obj.Herramienta;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = obj.Observacion;
                cmd.Parameters.Add("@IdEstadoH", SqlDbType.Int).Value = obj.IdEstadoH;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = obj.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = obj.IdUsuarioCreacion;
                cmd.ExecuteNonQuery();
                IdNuevo = Int32.Parse(cmd.Parameters["@IdHerramienta"].Value.ToString());
                cx.Close();
            }
            return IdNuevo;
        }

        public static DataTable Herramienta_GetItem(E_Herramienta obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Herramienta_GetItem", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = obje.IdHerramienta;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cn.Open();
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }
        public static int Herramienta_Update(E_Herramienta obj)
        {
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("Herramienta_Update", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = obj.IdHerramienta;
                cmd.Parameters.Add("@CodHerramienta", SqlDbType.VarChar, 20).Value = obj.CodHerramienta;
                cmd.Parameters.Add("@Herramienta", SqlDbType.VarChar, 100).Value = obj.Herramienta;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = obj.Observacion;
                cmd.Parameters.Add("@IdEstadoH", SqlDbType.Int).Value = obj.IdEstadoH;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = obj.FlagActivo;
                cmd.Parameters.Add("@IdUsuarioModificacion", SqlDbType.Int).Value = obj.IdUsuarioModificacion;
                cn.Open();
                n = cmd.ExecuteNonQuery();
                cn.Close();
            }
            return n;
        }

        public static DataTable Herramienta_GetItemByDesc(E_Herramienta E_Herramienta)
        {
            DataTable tbl = new DataTable();

            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Herramienta_GetItemByDesc", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = E_Herramienta.IdHerramienta;
                cmd.Parameters.Add("@Herramienta", SqlDbType.VarChar, 100).Value = E_Herramienta.Herramienta;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Herramienta_GetCantItems(E_Herramienta obj)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Herramienta_GetCantItems", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHerramienta", SqlDbType.Int).Value = obj.IdHerramienta;
                cmd.Parameters.Add("@Cantidad", SqlDbType.Int).Value = 0;
                cmd.Parameters["@Cantidad"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                n = Int32.Parse(cmd.Parameters["@Cantidad"].Value.ToString());
                cx.Close();
            }
            return n;
        }
    }
}
