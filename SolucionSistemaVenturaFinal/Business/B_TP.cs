using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_TP
    {
        public int UCCompTransfer_UpdateCascade(E_TP E_TP)
        {
            TP_Debug("UCCompTransfer_UpdateCascade", E_TP);
            return D_TP.UCCompTransfer_UpdateCascade(E_TP);
        }
        public int UCCompTransfer_Delete(E_TP E_TP)
        {
            TP_Debug("UCCompTransfer_Delete", E_TP);
            return D_TP.UCCompTransfer_Delete(E_TP);
        }
        public int UCCompTransfer_Update(E_TP E_TP)
        {
            TP_Debug("UCCompTransfer_Update", E_TP);
            return D_TP.UCCompTransfer_Update(E_TP);
        }
        public DataTable UCCompTransfer_GetItem(E_TP E_TP)
        {
            TP_Debug("UCCompTransfer_GetItem", E_TP);
            DataTable tbl = new DataTable();
            tbl = D_TP.UCCompTransfer_GetItem(E_TP);
            return tbl;
        }

        public DataTable UCCompTransfer_BeforeChange(E_TP E_TP)
        {
            TP_Debug("UCCompTransfer_BeforeChange", E_TP);
            return D_TP.UCCompTransfer_BeforeChange(E_TP);
        }

        public static void TP_Debug(string Metodo, E_TP E_TP)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdUCCompTransfer = " + E_TP.IdUCCompTransfer;
            Parametros = Parametros + ", IdPerfilCompOrigen = " + E_TP.IdPerfilCompOrigen;
            Parametros = Parametros + ", IdPerfilCompPadreOrigen = " + E_TP.IdPerfilCompPadreOrigen;
            Parametros = Parametros + ", IdUCOrigen =" + E_TP.IdUCOrigen;
            Parametros = Parametros + ", IdPerfilCompDestino =" + E_TP.IdPerfilCompDestino;
            Parametros = Parametros + ", IdPerfilCompPadreDestino = " + obj.NullableTrim(E_TP.IdPerfilCompPadreDestino.ToString());
            Parametros = Parametros + ", IdUCDestino =" + E_TP.IdUCDestino;
            Parametros = Parametros + ", IdPerfilComp =" + E_TP.IdPerfilComp;
            Parametros = Parametros + ", IdUCComp = " + E_TP.IdUCComp;
            Parametros = Parametros + ", IdTipoTransfer = " + E_TP.IdTipoTransfer;
            Parametros = Parametros + ", FechaTransfer = " + obj.NullableTrim(E_TP.FechaTransfer.ToShortDateString());
            Parametros = Parametros + ", FechaDevolucion = " + obj.NullableTrim(E_TP.FechaDevolucion.ToShortDateString());
            Parametros = Parametros + ", Observacion = " + obj.NullableTrim(E_TP.Observacion);
            Parametros = Parametros + ", IdEstadoTransfer = " + E_TP.IdEstadoTransfer;
            Parametros = Parametros + ", IdUsuario = " + E_TP.IdUsuario;
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
