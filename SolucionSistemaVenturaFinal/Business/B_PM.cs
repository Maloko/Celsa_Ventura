using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_PM
    {
        public static DataTable PM_List(E_PM E_PM)
        {
            PM_Debug("PM_List", E_PM);
            DataTable tbl = new DataTable();
            tbl = D_PM.PM_List(E_PM);
            return tbl;
        }
        public int Perfil_InsertMasivo(E_PM E_PM, DataTable tblPMComp, DataTable tblMPComp_Actividad, DataTable tblPMFrecuencias)
        {
            PM_Debug("Perfil_InsertMasivo", E_PM);
            int rpta = 11;
            rpta = D_PM.Perfil_InsertMasivo(E_PM, tblPMComp, tblMPComp_Actividad, tblPMFrecuencias);
            return rpta;
        }

        public DataTable PM_GetItem(E_PM E_PM)
        {
            PM_Debug("PM_GetItem", E_PM);
            DataTable tbl = new DataTable();
            tbl =  D_PM.PM_GetItem(E_PM);
            return tbl;
        }

        public DataTable PMComp_Actividad_GetBeforeChange(E_PM E_PM)
        {
            PM_Debug("PMComp_Actividad_GetBeforeChange", E_PM);
            DataTable tbl = new DataTable();
            tbl = D_PM.PMComp_Actividad_GetBeforeChange(E_PM);
            return tbl;
        }

        public DataTable PM_GetBeforeChange(E_PM E_PM)
        {
            PM_Debug("PM_GetBeforeChange", E_PM);
            DataTable tbl = new DataTable();
            tbl = D_PM.PM_GetBeforeChange(E_PM);
            return tbl;
        }

        public DataTable PMComp_Frecuencia_List(E_PM E_PM)
        {
            PM_Debug("PMComp_Frecuencia_List", E_PM);
            DataTable tbl = new DataTable();
            tbl = D_PM.PMComp_Frecuencia_List(E_PM);
            return tbl;
        }

        public DataTable PMComp_Actividad_List(E_PM E_PM)
        {
            PM_Debug("PMComp_Actividad_List", E_PM);
            DataTable tbl = new DataTable();
            tbl = D_PM.PMComp_Actividad_List(E_PM);
            return tbl;
        }

        public DataTable PMComp_List(E_PM E_PM)
        {
            PM_Debug("PMComp_List", E_PM);
            DataTable tbl = new DataTable();
            tbl = D_PM.PMComp_List(E_PM);
            return tbl;
        }

        public DataTable PM_ListByPerfil(E_PM E_PM)
        {
            PM_Debug("PM_ListByPerfil", E_PM);
            return D_PM.PM_ListByPerfil(E_PM);
        }

        public int PM_UpdateCascadePrioridad(E_PM E_PM, DataTable tblPM)
        {
            PM_Debug("PM_UpdateCascadePrioridad", E_PM);
            return D_PM.PM_UpdateCascadePrioridad(E_PM, tblPM);
        }

        public DataTable PM_Combo()
        {
            E_PM E_PM = new E_PM();
            PM_Debug("PM_Combo", E_PM);
            DataTable tbl = new DataTable();
            tbl = Data.D_PM.PM_Combo();
            return tbl;
        }

        public DataTable PM_CombobyPerfil(E_PM E_PM)
        {
            PM_Debug("PM_CombobyPerfil", E_PM);
            DataTable tbl = new DataTable();
            tbl = Data.D_PM.PM_CombobyPerfil(E_PM);
            return tbl;
        }
        public DataTable Actividad_ComboByPM(E_PM E_PM)
        {
            PM_Debug("Actividad_ComboByPM", E_PM);
            DataTable tbl = new DataTable();
            tbl = Data.D_PM.Actividad_ComboByPM(E_PM);
            return tbl;
        }
        public DataTable Tarea_ComboByPM(E_PM E_PM)
        {
            PM_Debug("Tarea_ComboByPM", E_PM);
            DataTable tbl = new DataTable();
            tbl = Data.D_PM.Tarea_ComboByPM(E_PM);
            return tbl;
        }

        public DataTable PerfilDetalle_ComboByPM(E_PM E_PM)
        {
            PM_Debug("PerfilDetalle_ComboByPM", E_PM);
            DataTable tbl = new DataTable();
            tbl = Data.D_PM.PerfilDetalle_ComboByPM(E_PM);
            return tbl;
        }
        public static void PM_Debug(string Metodo, E_PM E_PM)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdPM = " + obj.NullableTrim(E_PM.IdPM.ToString());
            Parametros = Parametros + ", CodPM = " + obj.NullableTrim(E_PM.CodPM);
            Parametros = Parametros + ", PM = " + obj.NullableTrim(E_PM.PM);
            Parametros = Parametros + ", IdPerfil =" + obj.NullableTrim(E_PM.IdPerfil.ToString());
            Parametros = Parametros + ", Porc01 =" + obj.NullableTrim(E_PM.Porc01.ToString());
            Parametros = Parametros + ", Porc02 = " + obj.NullableTrim(E_PM.Porc02.ToString());
            Parametros = Parametros + ", IdTipoOTDefecto =" + obj.NullableTrim(E_PM.IdTipoOTDefecto.ToString());
            Parametros = Parametros + ", IdEstadoPM =" + obj.NullableTrim(E_PM.IdEstadoPM.ToString());
            Parametros = Parametros + ", Prioridad = " + obj.NullableTrim(E_PM.Prioridad.ToString());
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_PM.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_PM.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_PM.IdUsuarioModificacion.ToString());
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
