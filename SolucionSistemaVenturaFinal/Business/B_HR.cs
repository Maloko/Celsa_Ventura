using System.Data;
using Data;
using Entities;
using Utilitarios;

namespace Business
{
    public class B_HR
    {
        public int HR_UpdateCascade(E_HR E_HR, DataTable tblHRComp)
        {
            HR_Debug("HR_UpdateCascade", E_HR);
            return D_HR.HR_UpdateCascade(E_HR, tblHRComp);
        }

        public DataTable HR_List(E_HR E_HR)
        {
            HR_Debug("HR_List", E_HR);
            return D_HR.HR_List(E_HR);
        }

        public DataTable HR_GetItem(E_HR E_HR)
        {
            HR_Debug("HR_GetItem", E_HR);
            return D_HR.HR_GetItem(E_HR);
        }

        public DataTable HRComp_List(E_HR E_HR)
        {
            HR_Debug("HRComp_List", E_HR);
            return D_HR.HRComp_List(E_HR);
        }

        public DataTable HR_Combo(E_HR E_HR)
        {
            HR_Debug("HR_Combo", E_HR);
            return D_HR.HR_Combo(E_HR);
        }

        public DataTable HR_ComboByFilters(E_HR E_HR)
        {
            HR_Debug("HR_ComboByFilters", E_HR);
            return D_HR.HR_ComboByFilters(E_HR);
        }

        public DataTable HRComp_BeforeCreate(E_HR E_HR, DataTable tblHRComp)
        {
            HR_Debug("HRComp_BeforeCreate", E_HR);
            return D_HR.HRComp_BeforeCreate(E_HR, tblHRComp);
        }

        public static void HR_Debug(string Metodo, E_HR E_HR)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdHR = " + obj.NullableTrim(E_HR.IdHR.ToString());
            Parametros = Parametros + ", CodHR = " + obj.NullableTrim(E_HR.CodHR);
            Parametros = Parametros + ", IdUC = " + obj.NullableTrim(E_HR.IdUC.ToString());
            Parametros = Parametros + ", FechaHR =" + obj.NullableTrim(E_HR.FechaHR.ToString());
            Parametros = Parametros + ", CodSolicitanteSAP =" + obj.NullableTrim(E_HR.CodSolicitanteSAP);
            Parametros = Parametros + ", NombreSolicitanteSAP =" + obj.NullableTrim(E_HR.NombreSolicitanteSAP);
            Parametros = Parametros + ", IdEstadoHR = " + obj.NullableTrim(E_HR.IdEstadoHR.ToString());
            Parametros = Parametros + ", Observacion =" + obj.NullableTrim(E_HR.Observacion);
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_HR.IdUsuario.ToString());
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
