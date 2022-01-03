using System.Data;
using Data;
using Entities;
using Utilitarios;

namespace Business
{
    public class B_Neumatico_Ciclo
    {
        public DataTable NeumaticoCiclo_List(E_Neumatico_Ciclo obje)
        {
            NeumaticoCiclo_Debug("NeumaticoCiclo_List", obje);
            return D_Neumatico_Ciclo.NeumaticoCiclo_List(obje);
        }
        public int NeumaticoCiclo_DeleteMasivo(E_Neumatico_Ciclo obje)
        {
            NeumaticoCiclo_Debug("NeumaticoCiclo_DeleteMasivo", obje);
            return D_Neumatico_Ciclo.NeumaticoCiclo_DeleteMasivo(obje);
        }
        public int NeumaticoCiclo_Insert(E_Neumatico_Ciclo obje)
        {
            NeumaticoCiclo_Debug("NeumaticoCiclo_Insert", obje);
            return D_Neumatico_Ciclo.NeumaticoCiclo_Insert(obje);
        }

        public int NeumaticoCiclo_Delete(E_Neumatico_Ciclo obje)
        {
            NeumaticoCiclo_Debug("NeumaticoCiclo_Delete", obje);
            return D_Neumatico_Ciclo.NeumaticoCiclo_Delete(obje);
        }

        public static void NeumaticoCiclo_Debug(string Metodo, E_Neumatico_Ciclo E_Neumatico_Ciclo)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdNeumaticoCiclo= " + E_Neumatico_Ciclo.IdNeumaticoCiclo.ToString();
            Parametros = Parametros + ", IdNeumatico = " + obj.NullableTrim(E_Neumatico_Ciclo.IdNeumatico.ToString());
            Parametros = Parametros + ", IdCiclo = " + obj.NullableTrim(E_Neumatico_Ciclo.IdCiclo.ToString());
            Parametros = Parametros + ", Frecuencia = " + obj.NullableTrim(E_Neumatico_Ciclo.Frecuencia.ToString());
            Parametros = Parametros + ", Contador = " + obj.NullableTrim(E_Neumatico_Ciclo.Contador.ToString());
            Parametros = Parametros + ", IdEstadoNC = " + obj.NullableTrim(E_Neumatico_Ciclo.IdEstadoNC.ToString());
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_Neumatico_Ciclo.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_Neumatico_Ciclo.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_Neumatico_Ciclo.IdUsuarioModificacion.ToString());
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
