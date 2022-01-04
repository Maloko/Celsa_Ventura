using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_HR
    {
        public static int HR_UpdateCascade(E_HR E_HR, DataTable tblHRComp)
        {
            int rpta = 1;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("HR_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHR", SqlDbType.Int).Value = E_HR.IdHR;
                cmd.Parameters.Add("@CodHR", SqlDbType.VarChar, 12).Value = E_HR.CodHR;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_HR.IdUC;
                cmd.Parameters.Add("@FechaHR", SqlDbType.DateTime).Value = E_HR.FechaHR;
                cmd.Parameters.Add("@CodSolicitanteSAP", SqlDbType.VarChar, 50).Value = E_HR.CodSolicitanteSAP;
                cmd.Parameters.Add("@NombreSolicitanteSAP", SqlDbType.VarChar, 50).Value = E_HR.NombreSolicitanteSAP;
                cmd.Parameters.Add("@IdEstadoHR", SqlDbType.Int).Value = E_HR.IdEstadoHR;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 400).Value = E_HR.Observacion;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_HR.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_HR.IdUsuario;
                cmd.Parameters.Add("@tblHRComp", SqlDbType.Structured).Value = tblHRComp;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_HR.FechaModificacion;
                #region Celsa
                cmd.Parameters.Add("@CodPrioridad", SqlDbType.Int).Value = E_HR.CodPrioridad;
                cmd.Parameters.Add("@CodTipoRequerimiento", SqlDbType.Int).Value = E_HR.CodTipoRequerimiento;
                #endregion

                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cx.Open();
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static DataTable HR_List(E_HR E_HR)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HR_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstadoHR", SqlDbType.Int).Value = E_HR.IdEstadoHR;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable HR_Combo(E_HR E_HR)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HR_Combo", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstadoHR", SqlDbType.Int).Value = E_HR.IdEstadoHR;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable HR_ComboByFilters(E_HR E_HR)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HR_ComboByFilters", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstadoHR", SqlDbType.Int).Value = E_HR.IdEstadoHR;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_HR.IdUC;
                cmd.Parameters.Add("@FechaHR", SqlDbType.DateTime).Value = E_HR.FechaHR;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        

        public static DataTable HR_GetItem(E_HR E_HR)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HR_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHR", SqlDbType.Int).Value = E_HR.IdHR;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable HRComp_BeforeCreate(E_HR E_HR, DataTable tblHRComp)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HRComp_BeforeCreate", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_HR.IdUC;
                cmd.Parameters.Add("@FechaHR", SqlDbType.DateTime).Value = E_HR.FechaHR;
                cmd.Parameters.Add("@tblHRComp", SqlDbType.Structured).Value = tblHRComp;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable HRComp_List(E_HR E_HR)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HRComp_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHR", SqlDbType.Int).Value = E_HR.IdHR;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
    }
}
