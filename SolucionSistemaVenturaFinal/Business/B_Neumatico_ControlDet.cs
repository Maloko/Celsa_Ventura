using Entities;
using Data;
using System.Data;
using Utilitarios;

namespace Business
{
    public class B_Neumatico_ControlDet
    {
        public DataTable NeumaticoControlDet_List(E_Neumatico_ControlDet E_Neumatico_ControlDet)
        {
            NeumaticoControlDet_Debug("NeumaticoControlDet_List", E_Neumatico_ControlDet);
            return D_Neumatico_ControlDet.D_NeumaticoControlDet_List(E_Neumatico_ControlDet);
        }

        public DataTable NeumaticoControlDet_ListByUC(E_Neumatico_ControlDet E_Neumatico_ControlDet)
        {
            NeumaticoControlDet_Debug("NeumaticoControlDet_ListByUC", E_Neumatico_ControlDet);
            return D_Neumatico_ControlDet.D_NeumaticoControlDet_ListByUC(E_Neumatico_ControlDet);
        }

        
        public static void NeumaticoControlDet_Debug(string Metodo, E_Neumatico_ControlDet E_Neumatico_ControlDet)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdNCD= " + E_Neumatico_ControlDet.IdNCD.ToString();
            Parametros = Parametros + ", IdNC = " + obj.NullableTrim(E_Neumatico_ControlDet.IdNC.ToString());
            Parametros = Parametros + ", Posicion = " + obj.NullableTrim(E_Neumatico_ControlDet.Posicion.ToString());
            Parametros = Parametros + ", IdNeumatico = " + obj.NullableTrim(E_Neumatico_ControlDet.IdNeumatico.ToString());
            Parametros = Parametros + ", Valor = " + obj.NullableTrim(E_Neumatico_ControlDet.Valor.ToString());
            Parametros = Parametros + ", ValorNuevo = " + obj.NullableTrim(E_Neumatico_ControlDet.ValorNuevo.ToString());
            Parametros = Parametros + ", FlagReencauche = " + obj.NullableTrim(E_Neumatico_ControlDet.FlagReencauche.ToString());
            Parametros = Parametros + ", Observacion = " + obj.NullableTrim(E_Neumatico_ControlDet.Observacion);
            Parametros = Parametros + ", IdEstadoNCD = " + obj.NullableTrim(E_Neumatico_ControlDet.IdEstadoNCD.ToString());
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_Neumatico_ControlDet.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_Neumatico_ControlDet.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", FechaCreacion = " + obj.NullableTrim(E_Neumatico_ControlDet.FechaCreacion.ToString());
            Parametros = Parametros + ", HostCreacion = " + obj.NullableTrim(E_Neumatico_ControlDet.HostCreacion);
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_Neumatico_ControlDet.IdUsuarioModificacion.ToString());
            Parametros = Parametros + ", FechaModificacion = " + obj.NullableTrim(E_Neumatico_ControlDet.FechaModificacion.ToString());
            Parametros = Parametros + ", HostModificacion = " + obj.NullableTrim(E_Neumatico_ControlDet.HostModificacion);
            Parametros = Parametros + ", IdUC = " + obj.NullableTrim(E_Neumatico_ControlDet.IdUC.ToString());
            Parametros = Parametros + ", IdCiclo = " + obj.NullableTrim(E_Neumatico_ControlDet.IdCiclo.ToString());
            Parametros = Parametros + ", IdUsuario = " + obj.NullableTrim(E_Neumatico_ControlDet.IdUsuario.ToString());
            Debug.EscribirDebug(Metodo, Parametros);

        }
    }
}
