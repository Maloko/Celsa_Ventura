using System;
using Entities;
using System.Data;
using System.Data.SqlClient;
namespace Data
{
    public class D_HI
    {
        public static int HI_UpdateCascade(E_HI E_HI, DataTable tblHIComp, DataTable tblHIComp_Actividad, DataTable tblHITarea, DataTable tblHIDetalle, DataTable tblHIHorasDetalle)
        {
            int rpta = 1;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("HI_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHI", SqlDbType.Int).Value = E_HI.IdHI;
                cmd.Parameters.Add("@CodHI", SqlDbType.VarChar, 12).Value = E_HI.CodHI;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = E_HI.IdUC;
                cmd.Parameters.Add("@FechaInspeccion", SqlDbType.DateTime).Value = E_HI.FechaInspeccion;
                cmd.Parameters.Add("@IdOT", SqlDbType.Int).Value = E_HI.IdOT;
                cmd.Parameters.Add("@IdHR", SqlDbType.Int).Value = E_HI.IdHR;
                cmd.Parameters.Add("@FlagRequiereOT", SqlDbType.Bit).Value = E_HI.FlagRequiereOT;
                cmd.Parameters.Add("@FlagProgramado", SqlDbType.Bit).Value = E_HI.FlagProgramado;
                cmd.Parameters.Add("@CodResponsableSAP", SqlDbType.VarChar, 20).Value = E_HI.CodResponsableSAP;
                cmd.Parameters.Add("@NombreResponsableSAP", SqlDbType.VarChar, 100).Value = E_HI.NombreResponsableSAP;
                cmd.Parameters.Add("@FechaInicial", SqlDbType.DateTime).Value = E_HI.FechaInicial;
                cmd.Parameters.Add("@FechaFinal", SqlDbType.DateTime).Value = E_HI.FechaFinal;
                cmd.Parameters.Add("@KmInicial", SqlDbType.Decimal).Value = E_HI.KmInicial;
                cmd.Parameters.Add("@KmFinal", SqlDbType.Decimal).Value = E_HI.KmFinal;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar, 400).Value = E_HI.Observacion;
                cmd.Parameters.Add("@IdEstadoHI", SqlDbType.Int).Value = E_HI.IdEstadoHI;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_HI.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_HI.IdUsuario;
                cmd.Parameters.Add("@tblHIComp", SqlDbType.Structured).Value = tblHIComp;
                cmd.Parameters.Add("@tblHIComp_Actividad", SqlDbType.Structured).Value = tblHIComp_Actividad;
                cmd.Parameters.Add("@tblHITarea", SqlDbType.Structured).Value = tblHITarea;
                cmd.Parameters.Add("@tblHIDetalle", SqlDbType.Structured).Value = tblHIDetalle;
                cmd.Parameters.Add("@tblHIHorasDetalle", SqlDbType.Structured).Value = tblHIHorasDetalle;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_HI.FechaModificacion;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cx.Open();
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static int HI_UpdateEstado(E_HI E_HI)
        {
            int rpta = 1;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("HI_UpdateEstado", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHI", SqlDbType.Int).Value = E_HI.IdHI;
                cmd.Parameters.Add("@IdOT", SqlDbType.Int).Value = E_HI.IdOT;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cx.Open();
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static DataTable HI_List(E_HI E_HI)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HI_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdEstadoHI", SqlDbType.Int).Value = E_HI.IdEstadoHI;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable HI_GetItem(E_HI E_HI)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HI_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHI", SqlDbType.Int).Value = E_HI.IdHI;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable HIComp_List(E_HI E_HI)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HIComp_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHI", SqlDbType.Int).Value = E_HI.IdHI;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable HIComp_Actividad_List(E_HI E_HI)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HIComp_Actividad_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHI", SqlDbType.Int).Value = E_HI.IdHI;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable HIDetalle_List(E_HI E_HI)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HIDetalle_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHI", SqlDbType.Int).Value = E_HI.IdHI;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable HITarea_List(E_HI E_HI)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HITarea_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHI", SqlDbType.Int).Value = E_HI.IdHI;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable HIHorasDetalle_List(E_HI E_HI)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("HIHorasDetalle_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdHI", SqlDbType.Int).Value = E_HI.IdHI;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
    }
}
