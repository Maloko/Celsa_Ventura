using System;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Data
{
    public class D_PM
    {
        public static DataTable PM_List(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PM_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PM.FlagActivo;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable PM_GetItem(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PM_GetItem", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = E_PM.IdPM;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable PMComp_Actividad_GetBeforeChange(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PMComp_Actividad_GetBeforeChange", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = E_PM.IdPM;
                cmd.Parameters.Add("@IdPMComp", SqlDbType.Int).Value = E_PM.IdPMComp;
                cmd.Parameters.Add("@IdActividad", SqlDbType.Int).Value = E_PM.IdPMActividad;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable PM_GetBeforeChange(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PM_GetBeforeChange", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPM", SqlDbType.VarChar).Value = E_PM.IdPM;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable PMComp_Frecuencia_List(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PMComp_Frecuencia_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = E_PM.IdPM;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable PMComp_Actividad_List(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PMComp_Actividad_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = E_PM.IdPM;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable PMComp_List(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PMComp_List", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = E_PM.IdPM;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable PM_ListByPerfil(E_PM obje)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PM_ListByPerfil", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = obje.IdPerfil;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static int PM_UpdateCascadePrioridad(E_PM obje, DataTable tblPM)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PM_UpdateCascadePrioridad", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                //usuario
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = obje.IdPM;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = obje.IdUsuarioModificacion;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                //PMs
                cmd.Parameters.Add("@tblPM", SqlDbType.Structured).Value = tblPM;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        public static int Perfil_InsertMasivo(E_PM E_PM, DataTable tblPMComp, DataTable tblMPComp_Actividad, DataTable tblPMFrecuencias)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PM_UpdateCascade", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                //Perfil
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = E_PM.IdPM;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@PM", SqlDbType.VarChar, 100).Value = E_PM.PM;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PM.IdPerfil;
                cmd.Parameters.Add("@IdCiclo", SqlDbType.Int).Value = E_PM.IdCiclo;
                cmd.Parameters.Add("@Porc01", SqlDbType.Decimal).Value = E_PM.Porc01;
                cmd.Parameters.Add("@Porc02", SqlDbType.Decimal).Value = E_PM.Porc02;
                cmd.Parameters.Add("@IdTipoOTDefecto", SqlDbType.Int).Value = E_PM.IdTipoOTDefecto;
                cmd.Parameters.Add("@IdEstadoPM", SqlDbType.Int).Value = E_PM.IdEstadoPM;
                cmd.Parameters.Add("@Prioridad", SqlDbType.Int).Value = E_PM.Prioridad;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PM.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_PM.IdUsuarioCreacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_PM.FechaModificacion;
                //Detalles
                cmd.Parameters.Add("@tblPMComp", SqlDbType.Structured).Value = tblPMComp;
                cmd.Parameters.Add("@tblPMCompActi", SqlDbType.Structured).Value = tblMPComp_Actividad;
                cmd.Parameters.Add("@tblPMCompFrec", SqlDbType.Structured).Value = tblPMFrecuencias;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }

        

        public static DataTable PM_Combo()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter("PM_Combo", cn);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static DataTable PM_CombobyPerfil(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("PM_CombobyPerfil", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PM.IdPerfil;
                SqlDataAdapter da = new SqlDataAdapter(cmd);                
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static DataTable Actividad_ComboByPM(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Actividad_ComboByPM", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PM.IdPerfilComp;
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = E_PM.IdPM;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static DataTable Tarea_ComboByPM(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Tarea_ComboByPM", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = E_PM.IdPM;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PM.IdPerfilComp;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        public static DataTable PerfilDetalle_ComboByPM(E_PM E_PM)
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("PerfilDetalle_ComboByPM", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = E_PM.IdPM;
                cmd.Parameters.Add("@IdPerfilComp", SqlDbType.Int).Value = E_PM.IdPerfilComp;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                cn.Close();
            }
            return tbl;
        }

        #region REQUERIMIENTO_02_CELSA
        public static int Perfil_InsertMasivo_OT(E_PM E_PM, DataTable tblPMComp, DataTable tblMPComp_Actividad, DataTable tblPMFrecuencias)
        {
            int rpta = 11;
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("PM_UpdateCascade_OT", cx);
                cmd.CommandType = CommandType.StoredProcedure;
                //Perfil
                cmd.Parameters.Add("@IdPM", SqlDbType.Int).Value = E_PM.IdPM;
                cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = 0;
                cmd.Parameters["@IdError"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@PM", SqlDbType.VarChar, 100).Value = E_PM.PM;
                cmd.Parameters.Add("@IdPerfil", SqlDbType.Int).Value = E_PM.IdPerfil;
                cmd.Parameters.Add("@IdCiclo", SqlDbType.Int).Value = E_PM.IdCiclo;
                cmd.Parameters.Add("@Porc01", SqlDbType.Decimal).Value = E_PM.Porc01;
                cmd.Parameters.Add("@Porc02", SqlDbType.Decimal).Value = E_PM.Porc02;
                cmd.Parameters.Add("@IdTipoOTDefecto", SqlDbType.Int).Value = E_PM.IdTipoOTDefecto;
                cmd.Parameters.Add("@IdEstadoPM", SqlDbType.Int).Value = E_PM.IdEstadoPM;
                cmd.Parameters.Add("@Prioridad", SqlDbType.Int).Value = E_PM.Prioridad;
                cmd.Parameters.Add("@FlagActivo", SqlDbType.Bit).Value = E_PM.FlagActivo;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = E_PM.IdUsuarioCreacion;
                cmd.Parameters.Add("@FechaModificacion", SqlDbType.DateTime).Value = E_PM.FechaModificacion;
                cmd.Parameters.Add("@FechaProgramacion", SqlDbType.DateTime).Value = E_PM.FechaProg;
                //Detalles
                cmd.Parameters.Add("@tblPMComp", SqlDbType.Structured).Value = tblPMComp;
                cmd.Parameters.Add("@tblPMCompActi", SqlDbType.Structured).Value = tblMPComp_Actividad;
                cmd.Parameters.Add("@tblPMCompFrec", SqlDbType.Structured).Value = tblPMFrecuencias;
                cmd.ExecuteNonQuery();
                rpta = Int32.Parse(cmd.Parameters["@IdError"].Value.ToString());
                cx.Close();
            }
            return rpta;
        }
        #endregion
    }
}
