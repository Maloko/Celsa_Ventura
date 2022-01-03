using Entities;
using Data;
using System.Data;
using Utilitarios;

namespace Business
{
    public class B_Neumatico_Control
    {
        public DataTable Neumatico_Control_List(E_Neumatico_Control E_Neumatico_Control)
        {
            NeumaticoControl_Debug("Neumatico_Control_List", E_Neumatico_Control);
            return D_Neumatico_Control.Neumatico_Control_List(E_Neumatico_Control);
        }

        public DataTable NeumaticoControl_GetItem(E_Neumatico_Control E_Neumatico_Control)
        {
            NeumaticoControl_Debug("NeumaticoControl_GetItem", E_Neumatico_Control);
            return D_Neumatico_Control.NeumaticoControl_GetItem(E_Neumatico_Control);
        }

        public int NeumaticoControl_UpdateCascada(E_Neumatico_Control E_Neumatico_Control, DataTable tblNCDet)
        {
            NeumaticoControl_Debug("NeumaticoControl_UpdateCascada", E_Neumatico_Control);
            return D_Neumatico_Control.NeumaticoControl_UpdateCascada(E_Neumatico_Control, tblNCDet);
        }

        public static void NeumaticoControl_Debug(string Metodo, E_Neumatico_Control E_Neumatico_Control)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdNC= " + E_Neumatico_Control.IdNC.ToString();
            Parametros = Parametros + ", CodNC = " + obj.NullableTrim(E_Neumatico_Control.CodNC);
            Parametros = Parametros + ", IdUC = " + obj.NullableTrim(E_Neumatico_Control.IdUC.ToString());
            Parametros = Parametros + ", FechaControl = " + E_Neumatico_Control.FechaControl.ToString();
            Parametros = Parametros + ", Ciclo = " + E_Neumatico_Control.Ciclo.ToString();
            Parametros = Parametros + ", IdEstadoNC = " + E_Neumatico_Control.IdEstadoNC.ToString();
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_Neumatico_Control.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + E_Neumatico_Control.IdUsuarioCreacion.ToString();
            Parametros = Parametros + ", FechaCreacion = " + obj.NullableTrim(E_Neumatico_Control.FechaCreacion.ToString());
            Parametros = Parametros + ", HostCreacion = " + obj.NullableTrim(E_Neumatico_Control.HostCreacion);
            Parametros = Parametros + ", IdUsuarioModificacion = " +obj.NullableTrim(E_Neumatico_Control.IdUsuarioModificacion);
            Parametros = Parametros + ", IdUsuario = " + E_Neumatico_Control.IdUsuario.ToString();
            Parametros = Parametros + ", FechaModificacion = " + obj.NullableTrim(E_Neumatico_Control.FechaModificacion.ToString());
            Parametros = Parametros + ", HostModificacion = " + obj.NullableTrim(E_Neumatico_Control.HostModificacion.ToString());
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
