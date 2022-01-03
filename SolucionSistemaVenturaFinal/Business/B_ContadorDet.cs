using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_ContadorDet
    {
        public DataTable ContadorDet_ListByDate(E_ContadorDet objE)
        {
            return D_ContadorDet.ContadorDet_ListByDate(objE);
        }

        public DataTable ContadorDet_List(E_ContadorDet objE)
        {
            return D_ContadorDet.ContadorDet_List(objE);
        }

        public DataTable ContadorDet_GetItem(E_ContadorDet objE)
        {
            return D_ContadorDet.ContadorDet_GetItem(objE);
        }

        public int ContadorDet_UpdateProcess(E_ContadorDet objE, out string DescError)
        {
            ContadorDet_Debug("ContadorDet_UpdateProcess", objE);
            return D_ContadorDet.ContadorDet_UpdateProcess(objE, out DescError);
        }

        public DataTable ContadorDet_GetItemByNroDocOperacion(E_ContadorDet objE)
        {
            return D_ContadorDet.ContadorDet_GetItemByNroDocOperacion(objE);
        }

        public static void ContadorDet_Debug(string Metodo, E_ContadorDet Obje)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdContadorDet = " + obj.NullableTrim(Obje.IdContadorDet.ToString());
            Parametros = Parametros + ", CodUc = " + obj.NullableTrim(Obje.CodUc);
            Parametros = Parametros + ", IdOrigenRegistro = " + obj.NullableTrim(Obje.IdOrigenRegistro.ToString());
            Parametros = Parametros + ", IdEvento =" + obj.NullableTrim(Obje.IdEvento.ToString());
            Parametros = Parametros + ", IdTipoOperacion =" + obj.NullableTrim(Obje.IdTipoOperacion.ToString());
            Parametros = Parametros + ", NroDocOperacion = " + obj.NullableTrim(Obje.NroDocOperacion);
            Parametros = Parametros + ", IdDocCorregir =" + obj.NullableTrim(Obje.IdDocCorregir.ToString());
            Parametros = Parametros + ", FechaHoraIni =" + Obje.FechaHoraIni.ToString();
            Parametros = Parametros + ", FechaHoraFin = " + Obje.FechaHoraFin.ToString();
            Parametros = Parametros + ", ContadorIni = " + obj.NullableTrim(Obje.ContadorIni.ToString());
            Parametros = Parametros + ", ContadorFin = " + obj.NullableTrim(Obje.ContadorFin.ToString());
            Parametros = Parametros + ", CodSolicitante = " + obj.NullableTrim(Obje.CodSolicitante);
            Parametros = Parametros + ", CodResponsable = " + obj.NullableTrim(Obje.CodResponsable);
            Parametros = Parametros + ", Observacion = " + obj.NullableTrim(Obje.Observacion);
            Parametros = Parametros + ", IdUsuario = " + obj.NullableTrim(Obje.IdUsuario.ToString());
            Parametros = Parametros + ", FechaIni = " + obj.NullableTrim(Obje.FechaIni.ToString());
            Parametros = Parametros + ", FechaFin = " + obj.NullableTrim(Obje.FechaFin.ToString());
            Debug.EscribirDebug(Metodo, Parametros);
        }

        public DataTable ContadorDet_GetLastRecord(E_ContadorDet Obje, out string DescError)
        {
            return D_ContadorDet.ContadorDet_GetLastRecord(Obje,out DescError);
        }

        public DataTable ContadorDet_GetPenultimateRecord(E_ContadorDet Obje, out string DescError)
        {
            return D_ContadorDet.ContadorDet_GetPenultimateRecord(Obje, out DescError);
        }
    }
}
