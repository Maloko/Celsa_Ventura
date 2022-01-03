using System.Data;
using Entities;
using System.Data.SqlClient;

namespace Data
{
    public class D_Empresa
    {
        public static int Empresa_AutoUpdate()
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("VS_SP_ObtenerDatosSociedad", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                n = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return n;
        }

        public static int Empresa_CargarLicencia(E_Empresa objE)
        {
            int n = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Empresa_LoadLicense", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RUC", SqlDbType.VarChar, 20).Value = objE.RUC;
                cmd.Parameters.Add("@Empresa", SqlDbType.VarChar, 100).Value = objE.Empresa;
                cmd.Parameters.Add("@Licencia", SqlDbType.VarBinary, objE.Licencia.Length).Value = objE.Licencia;
                n = cmd.ExecuteNonQuery();
                cx.Close();
            }
            return n;
        }

        public static DataTable Empresa_GetItem(E_Empresa objE)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Empresa_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = objE.IdEmpresa;
                cmd.Parameters.Add("@RUC", SqlDbType.VarChar, 20).Value = objE.RUC;
                cmd.Parameters.Add("@Empresa", SqlDbType.VarChar, 100).Value = objE.Empresa;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
        
    }
}
