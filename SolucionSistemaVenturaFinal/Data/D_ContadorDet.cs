using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_ContadorDet
    {

        public static DataTable ContadorDet_ListByDate(E_ContadorDet objE)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("ContadorDet_ListByDate", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FechaIni", SqlDbType.SmallDateTime).Value = objE.FechaIni;
                cmd.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime).Value = objE.FechaFin;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }

            return tbl;
        }
        public static DataTable ContadorDet_List(E_ContadorDet objE)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("ContadorDet_List", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdTipoOperacion", SqlDbType.Int).Value = objE.IdTipoOperacion;
                cmd.Parameters.Add("@CodUC", SqlDbType.VarChar, 20).Value = objE.CodUc;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }

            return tbl;
        }


        public static DataTable ContadorDet_GetItem(E_ContadorDet objE)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("ContadorDet_GetItem", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdContadorDet", SqlDbType.Int).Value = objE.IdContadorDet;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static int ContadorDet_UpdateProcess(E_ContadorDet objE, out string DescError)
        {         
            int n = 0;
            using (SqlConnection cn = Conexion.ObtenerConexion())            
            {
                SqlCommand cmd = new SqlCommand("ContadorDet_UpdateProcess", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CodUC", SqlDbType.VarChar, 20).Value = objE.CodUc;
                cmd.Parameters.Add("@IdOrigenRegistro", SqlDbType.Int).Value = objE.IdOrigenRegistro;
                cmd.Parameters.Add("@IdEvento", SqlDbType.Int).Value = objE.IdEvento;
                cmd.Parameters.Add("@IdTipoOperacion", SqlDbType.Int).Value = objE.IdTipoOperacion;
                cmd.Parameters.Add("@NroDocOperacion", SqlDbType.VarChar, 20).Value = objE.NroDocOperacion;
                cmd.Parameters.Add("@IdDocCorregir", SqlDbType.Int).Value = objE.IdDocCorregir;
                cmd.Parameters.Add("@FechaHoraIni", SqlDbType.VarChar,20).Value = objE.FechaHoraIni.ToString("yyyyMMdd HH:mm");
                cmd.Parameters.Add("@FechaHoraFin", SqlDbType.VarChar, 20).Value = objE.FechaHoraFin.ToString("yyyyMMdd HH:mm");
                cmd.Parameters.Add("@ContadorIni", SqlDbType.Decimal, 18).Value = objE.ContadorIni;
                cmd.Parameters.Add("@ContadorFin", SqlDbType.Decimal, 18).Value = objE.ContadorFin;
                cmd.Parameters.Add("@CodSolicitante", SqlDbType.VarChar, 20).Value = objE.CodSolicitante;
                cmd.Parameters.Add("@CodResponsable", SqlDbType.VarChar, 20).Value = objE.CodResponsable;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 200).Value = objE.Observacion;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = objE.IdUsuario;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@DescError", SqlDbType.VarChar, 200).Value = string.Empty;
                cmd.Parameters["@DescError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                n = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                DescError = cmd.Parameters["@DescError"].Value.ToString();
                cn.Close();
            }
            return n;
        }

        public static DataTable ContadorDet_GetItemByNroDocOperacion(E_ContadorDet objE)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())            
            {
                SqlCommand cmd = new SqlCommand("ContadorDet_GetItemByNroDocOperacion", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NroDocOperacion", SqlDbType.VarChar, 20).Value = objE.NroDocOperacion;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();                
            }
            return tbl;
        }


        public static DataTable ContadorDet_GetLastRecord(E_ContadorDet objE, out string DescError)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("ContadorDet_LastRecord", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CodUC", SqlDbType.VarChar, 20).Value = objE.CodUc;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@DescError", SqlDbType.VarChar, 200).Value = string.Empty;
                cmd.Parameters["@DescError"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                DescError = cmd.Parameters["@DescError"].Value.ToString();
               
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static DataTable ContadorDet_GetPenultimateRecord(E_ContadorDet objE, out string DescError)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("ContadorDet_PenultimateRecord", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CodUC", SqlDbType.VarChar, 20).Value = objE.CodUc;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@DescError", SqlDbType.VarChar, 200).Value = string.Empty;
                cmd.Parameters["@DescError"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                DescError = cmd.Parameters["@DescError"].Value.ToString();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

    }
}
