using Utilitarios;
using System.Data;
using Entities;
using Data;

namespace Business
{
    public class B_UCCompTransfer
    {
        public static DataTable UCCompTransfer_List(E_UCCompTransfer obje)
        {
            //UC_Debug("UCCompTransfer_List", obje);
            return D_UCCompTransfer.UCCompTransfer_List(obje);
        }

        public static void UC_Debug(string Metodo, E_UCCompTransfer obje)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdUCCompTransfer = " + obj.NullableTrim(obje.IdUCCompTransfer.ToString());
            Parametros = Parametros + ", CodUCCompTransfer = " + obj.NullableTrim(obje.CodUCCompTransfer.ToString());
            Parametros = Parametros + ", IdUCComp = " + obj.NullableTrim(obje.IdUCComp.ToString());
            Parametros = Parametros + ", IdTipoTransfer =" + obj.NullableTrim(obje.IdTipoTransfer.ToString());
            Parametros = Parametros + ", FechaTransfer =" + obj.NullableTrim(obje.FechaTransfer.ToString());
            Parametros = Parametros + ", ContadorFechaDevolucionAcum = " + obj.NullableTrim(obje.FechaDevolucion.ToString());
            Parametros = Parametros + ", IdPerfil =" + obj.NullableTrim(obje.IdPerfil.ToString());
            Parametros = Parametros + ", IdUCOrigen =" + obj.NullableTrim(obje.IdUCOrigen.ToString());
            Parametros = Parametros + ", IdUCDestino =" + obj.NullableTrim(obje.IdUCDestino.ToString());
            Parametros = Parametros + ", Observacion =" + obj.NullableTrim(obje.Observacion);
            Parametros = Parametros + ", IdEstadoTransfer =" + obj.NullableTrim(obje.IdEstadoTransfer.ToString());
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(obje.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(obje.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(obje.IdUsuarioModificacion.ToString());
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
