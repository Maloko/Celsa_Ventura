using System;
using System.Data;
using Entities;
using System.Data.SqlClient;

namespace Data
{
    public class D_Programacion
    {
        public static DataTable Bitacora_List()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Bitacora_ListByFilters", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@IdTipoGeneracion", SqlDbType.Int).Value = E_Programacion.TipoMantenimiento;
                //cmd.Parameters.Add("@FlagEstadoNulo", SqlDbType.Bit).Value = E_Programacion.FlagActividadPendiente;
                //cmd.Parameters.Add("@FlagMostrarTodos", SqlDbType.Bit).Value = E_Programacion.IdFlagTodos;
                //cmd.Parameters.Add("@FlagOrdenaPrioridad", SqlDbType.Bit).Value = E_Programacion.FlagActivarPriorizacion;
                //cmd.Parameters.Add("@FechaProgramacion", SqlDbType.DateTime).Value = E_Programacion.FechaProgramacion;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable ObtenerDatosHI()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("VS_SP_ObtenerDatosHI", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@IdTipoGeneracion", SqlDbType.Int).Value = E_Programacion.TipoMantenimiento;
                //cmd.Parameters.Add("@FlagEstadoNulo", SqlDbType.Bit).Value = E_Programacion.FlagActividadPendiente;
                //cmd.Parameters.Add("@FlagMostrarTodos", SqlDbType.Bit).Value = E_Programacion.IdFlagTodos;
                //cmd.Parameters.Add("@FlagOrdenaPrioridad", SqlDbType.Bit).Value = E_Programacion.FlagActivarPriorizacion;
                //cmd.Parameters.Add("@FechaProgramacion", SqlDbType.DateTime).Value = E_Programacion.FechaProgramacion;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable Bitacora_GetStock(string LineNum)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("VS_SP_Bitacora_GetStock", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LineNum", SqlDbType.VarChar).Value = LineNum;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int Programacion_BeforeCreate(DataTable tblBitacora)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Programacion_BeforeCreate", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@tblBitacora", SqlDbType.Structured).Value = tblBitacora;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Convert.ToInt32(cmd.Parameters["@IdError"].Value);
                cx.Close();
            }
            return rpta;
        }

        public static int ProgramacionDet_Load(E_Programacion E_Programacion)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("ProgramacionDet_Load", cx);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TipoMantenimiento", SqlDbType.Int).Value = E_Programacion.TipoMantenimiento;
                cmd.Parameters.Add("@FechaProgramacion", SqlDbType.DateTime).Value = E_Programacion.FechaProgramacion;
                cmd.Parameters.Add("@FlagActividadPendiente", SqlDbType.VarChar, 100).Value = E_Programacion.FlagActividadPendiente;
                cmd.Parameters.Add("@FlagActivarPriorizacion", SqlDbType.Int).Value = E_Programacion.FlagActivarPriorizacion;
                cmd.Parameters.Add("@IdUsuarioCreacion", SqlDbType.Int).Value = E_Programacion.IdUsuarioCreacion;
                cmd.Parameters.Add("@IdProgramacionRegistrada", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdProgramacionRegistrada"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Convert.ToInt32(cmd.Parameters["@IdProgramacionRegistrada"].Value);
                cx.Close();
            }
            return rpta;
        }

        public static int Programacion_UpdateCascade(E_Programacion E_Programacion, DataTable tblBitacora, DataTable tblProgramacionDet)
        {
            int rpta = 0;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Programacion_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdProgramacion", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@CodProgramacion", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@FechaProgramacion", SqlDbType.DateTime).Value = E_Programacion.FechaProgramacion;
                cmd.Parameters.Add("@Observacion", SqlDbType.VarChar).Value = E_Programacion.Observacion;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_Programacion.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_Programacion.IdUsuarioCreacion;
                cmd.Parameters.Add("@tblBitacora", SqlDbType.Structured).Value = tblBitacora;
                cmd.Parameters.Add("@tblProgramacionDet", SqlDbType.Structured).Value = tblProgramacionDet;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                rpta = Convert.ToInt32(cmd.Parameters["@IdError"].Value);
                cx.Close();
            }
            return rpta;
        }

        #region REQUERIMIENTO_07
        public static DataTable Bitacora_List_All(int IdUC, int IdPM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("Bitacora_ListByFilters_All", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdUC", SqlDbType.Int).Value = IdUC;
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = IdPM;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }
        #endregion
    }
}
