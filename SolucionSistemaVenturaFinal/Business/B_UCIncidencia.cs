using Data;
using Entities;
using System.Data;
using Utilitarios;

namespace Business
{
    public class B_UCIncidencia
    {
        public DataTable UCIncidencia_GetItem(E_UCIncidencia E_UCIncidencia)
        {
            DataTable tbl = new DataTable();
            tbl = D_UCIncidencia.UCIncidencia_GetItem(E_UCIncidencia);
            UCIncidencia_Debug("UCIncidencia_GetItem", E_UCIncidencia);
            return tbl;
        }

        public DataTable UCIncidencia_Combo()
        {
            DataTable tbl = new DataTable();
            tbl = D_UCIncidencia.UCIncidencia_Combo();
            return tbl;
        }

        public DataTable UCIncidencia_List(E_UCIncidencia E_UCIncidencia)
        {
            DataTable tbl = new DataTable();
            tbl = D_UCIncidencia.UCIncidencia_List(E_UCIncidencia);
            UCIncidencia_Debug("UCIncidencia_List", E_UCIncidencia);
            return tbl;
        }

        public int UCIncidencia_Delete(E_UCIncidencia E_UCIncidencia)
        {
            int rpta = D_UCIncidencia.UCIncidencia_Delete(E_UCIncidencia);
            UCIncidencia_Debug("UCIncidencia_Delete", E_UCIncidencia);
            return rpta;
        }

        public int UCIncidencia_UpdateCascade(E_UCIncidencia E_UCIncidencia, DataTable tblUCIncidenciaDet, out string DescError)
        {
            int rpta = D_UCIncidencia.UCIncidencia_UpdateCascade(E_UCIncidencia, tblUCIncidenciaDet, out DescError);
            UCIncidencia_Debug("UCIncidencia_UpdateCascade", E_UCIncidencia);
            return rpta;
        }

        public static void UCIncidencia_Debug(string Metodo, E_UCIncidencia E_UCIncidencia)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdUCIncidencia = " + obj.NullableTrim(E_UCIncidencia.IdUCIncidencia.ToString());
            Parametros = Parametros + ", CodIncidencia = " + obj.NullableTrim(E_UCIncidencia.CodIncidencia);
            Parametros = Parametros + ", IdUC = " + obj.NullableTrim(E_UCIncidencia.IdUC.ToString());
            Parametros = Parametros + ", FechaHoraDesde =" + obj.NullableTrim(E_UCIncidencia.FechaHoraDesde.ToString());
            Parametros = Parametros + ", FechaHoraHasta =" + obj.NullableTrim(E_UCIncidencia.FechaHoraHasta.ToString());
            Parametros = Parametros + ", CodSolicitante = " + obj.NullableTrim(E_UCIncidencia.CodSolicitante);
            Parametros = Parametros + ", CodResponsable =" + obj.NullableTrim(E_UCIncidencia.CodResponsable);
            Parametros = Parametros + ", IdTipoIncidencia =" + obj.NullableTrim(E_UCIncidencia.IdTipoIncidencia.ToString());
            Parametros = Parametros + ", Observacion = " + obj.NullableTrim(E_UCIncidencia.Observacion);
            Parametros = Parametros + ", IdEstadoIncidencia = " + obj.NullableTrim(E_UCIncidencia.IdEstadoIncidencia.ToString());
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_UCIncidencia.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_UCIncidencia.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", FechaCreacion = " + obj.NullableTrim(E_UCIncidencia.FechaCreacion.ToString());
            Parametros = Parametros + ", HostCreacion = " + obj.NullableTrim(E_UCIncidencia.HostCreacion);
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_UCIncidencia.IdUsuarioModificacion.ToString());
            Parametros = Parametros + ", FechaModificacion = " + obj.NullableTrim(E_UCIncidencia.FechaModificacion.ToString());
            Parametros = Parametros + ", HostModificacion = " + obj.NullableTrim(E_UCIncidencia.HostModificacion);
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
