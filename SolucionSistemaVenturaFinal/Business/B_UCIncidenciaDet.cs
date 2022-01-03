using Data;
using Entities;
using System.Data;
using Utilitarios;

namespace Business
{
    public class B_UCIncidenciaDet
    {
        public DataTable UCIncidenciaDet_List(E_UCIncidenciaDet E_UCIncidenciaDet)
        {
            DataTable tbl = new DataTable();
            tbl = D_UCIncidenciaDet.UCIncidenciaDet_List(E_UCIncidenciaDet);
            return tbl;
        }

        public DataTable UCIncidenciaDet_GetItem(E_UCIncidenciaDet E_UCIncidenciaDet)
        {
            DataTable tbl = new DataTable();
            tbl = D_UCIncidenciaDet.UCIncidenciaDet_GetItem(E_UCIncidenciaDet);
            return tbl;
        }

        public static void UCIncidencia_Debug(string Metodo, E_UCIncidenciaDet E_UCIncidenciaDet)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;

            Parametros = "IdUCIncidenciaDet = " + obj.NullableTrim(E_UCIncidenciaDet.IdUCIncidenciaDet.ToString());
            Parametros = Parametros + ", IdUCIncidencia = " + obj.NullableTrim(E_UCIncidenciaDet.IdUCIncidencia.ToString());
            Parametros = Parametros + ", IdCiclo = " + obj.NullableTrim(E_UCIncidenciaDet.IdCiclo.ToString());
            Parametros = Parametros + ", ContadorInicial =" + obj.NullableTrim(E_UCIncidenciaDet.ContadorInicial.ToString());
            Parametros = Parametros + ", ContadorFinal =" + obj.NullableTrim(E_UCIncidenciaDet.ContadorFinal.ToString());
            Parametros = Parametros + ", IdEstadoIncidenciaDet = " + obj.NullableTrim(E_UCIncidenciaDet.IdEstadoIncidenciaDet.ToString());
            Parametros = Parametros + ", FlagActivo =" + obj.NullableTrim(E_UCIncidenciaDet.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion =" + obj.NullableTrim(E_UCIncidenciaDet.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", FechaCreacion = " + obj.NullableTrim(E_UCIncidenciaDet.FechaCreacion.ToString());
            Parametros = Parametros + ", HostCreacion = " + obj.NullableTrim(E_UCIncidenciaDet.HostCreacion);
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_UCIncidenciaDet.IdUsuarioModificacion.ToString());
            Parametros = Parametros + ", FechaModificacion = " + obj.NullableTrim(E_UCIncidenciaDet.FechaModificacion.ToString());
            Parametros = Parametros + ", HostModificacion = " + obj.NullableTrim(E_UCIncidenciaDet.HostModificacion);
            Parametros = Parametros + ", IdUC = " + obj.NullableTrim(E_UCIncidenciaDet.IdUC.ToString());
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
