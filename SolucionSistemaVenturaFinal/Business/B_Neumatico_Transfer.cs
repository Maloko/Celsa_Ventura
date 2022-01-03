using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_Neumatico_Transfer
    {
        public int Neumatico_InsertMasivo(E_Neumatico_Transfer E_Neumatico_Transfer, DataTable tbl1,DataTable  tbl2)
        {
            Neumatico_Transfer_Debug("NeumaticoTransfer_UpdateCascade", E_Neumatico_Transfer);
            return D_Neumatico_Transfer.Neumatico_Transfer_InsertMasivo(E_Neumatico_Transfer, tbl1,tbl2);
        }

        public static void Neumatico_Transfer_Debug(string Metodo, E_Neumatico_Transfer obje)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdNeumaticoTransfer = " + obj.NullableTrim(obje.IdNeumaticoTransfer.ToString());
            Parametros = Parametros + ", CodNeumaticoTransfer = " + obj.NullableTrim(obje.CodNeumaticoTransfer);
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(obje.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuario = " + obj.NullableTrim(obje.IdUsuario.ToString());
            Debug.EscribirDebug(Metodo, Parametros);

        }

        
    }
}
