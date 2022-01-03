using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_OTIProv
    {
        public static int OTInforme_UpdateCascade(E_OTIProv E_OTIProv, DataTable tblOTIPComp_Actividad)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("OTInforme_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdOTInforme", SqlDbType.Int).Value = E_OTIProv.IdOTInforme;
                cmd.Parameters.Add("@IdOT", SqlDbType.Int).Value = E_OTIProv.IdOT;
                cmd.Parameters.Add("@NombreFile", SqlDbType.VarChar, 100).Value = E_OTIProv.NombreFile;
                cmd.Parameters.Add("@RUCProv", SqlDbType.VarChar, 20).Value = E_OTIProv.RUCProv;
                cmd.Parameters.Add("@CodProveedor", SqlDbType.VarChar, 20).Value = E_OTIProv.CodProveedor;
                cmd.Parameters.Add("@RazonSocialProv", SqlDbType.VarChar, 100).Value = E_OTIProv.RazonSocialProv;
                cmd.Parameters.Add("@NroOCProv", SqlDbType.VarChar, 50).Value = E_OTIProv.NroOCProv;
                cmd.Parameters.Add("@Costo", SqlDbType.Decimal).Value = E_OTIProv.Costo;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = E_OTIProv.Observacion;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_OTIProv.FlagActivo;
                cmd.Parameters.Add("@tblOTActividad", SqlDbType.Structured).Value = tblOTIPComp_Actividad;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_OTIProv.IdUsuario;
                cmd.Parameters.Add("@IdEstadoOT", SqlDbType.Int).Value = E_OTIProv.IdEstadoOT;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_OTIProv.FechaModificacion;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }
        public static DataTable OTInforme_List(E_OTIProv E_OTIProv)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("OTInforme_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdOT", SqlDbType.Int).Value = E_OTIProv.IdOT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                return tbl;
            }
        }
    }
}
