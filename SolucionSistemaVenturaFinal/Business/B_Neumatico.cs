using System.Data;
using Data;
using Entities;
using Utilitarios;

namespace Business
{
    public class B_Neumatico
    {
        public DataTable Neumatico_List(E_Neumatico E_Neumatico)
        {            
            Neumatico_Debug("Neumatico_List", E_Neumatico);            
            return D_Neumatico.Neumatico_List(E_Neumatico);
        }
        public static int Neumatico_Insert(E_Neumatico obje)
        {
            Neumatico_Debug("Neumatico_Insert", obje);
            return D_Neumatico.Neumatico_Insert(obje);
        }
        public static int Neumatico_Update(E_Neumatico obje)
        {
            Neumatico_Debug("Neumatico_Update", obje);
            return D_Neumatico.Neumatico_Update(obje);
        }
        public static DataTable Neumatico_GetItem(E_Neumatico obje)
        {
            Neumatico_Debug("Neumatico_GetItem", obje);
            return D_Neumatico.Neumatico_GetItem(obje);
        }

        public DataTable B_Neumatico_Combo()
        {
            return D_Neumatico.Neumatico_Combo();
        }

        public static int Neumatico_UpdateBaja(E_Neumatico obje)
        {
            Neumatico_Debug("Neumatico_UpdateBaja", obje);
            return D_Neumatico.Neumatico_UpdateBaja(obje);
        }

        public int Neumatico_UpdateCascade(E_Neumatico E_Neumatico, DataTable tblCiclo)
        {
            Neumatico_Debug("Neumatico_InsertMasivo", E_Neumatico);
            return D_Neumatico.Neumatico_UpdateCascade(E_Neumatico, tblCiclo);
        }

        public DataTable Neumatico_GetItemBySerie(E_Neumatico E_Neumatico)
        {
            Neumatico_Debug("Neumatico_GetItemBySerie", E_Neumatico);
            return D_Neumatico.Neumatico_GetItemBySerie(E_Neumatico);
        }

        public DataTable Neumatico_UltimoMovimiento(E_Neumatico E_Neumatico)
        {
            Neumatico_Debug("Neumatico_UltimoMovimiento",E_Neumatico);
            return D_Neumatico.Neumatico_UltimoMovimiento(E_Neumatico);
        }

        public int Neumatico_InsertMasivo(E_Neumatico E_Neumatico, DataTable tblN,DataTable tblNC)
        {
            Neumatico_Debug("Neumatico_InsertMasivo", E_Neumatico);
            return D_Neumatico.Neumatico_InsertMasivo(E_Neumatico, tblN, tblNC);
        }

        public static void Neumatico_Debug(string Metodo, E_Neumatico E_Neumatico)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;

            Parametros = "IdNeumatico= " + E_Neumatico.IdNeumatico.ToString();
            Parametros = Parametros + ", NroSerie = " + obj.NullableTrim(E_Neumatico.NroSerie);
            Parametros = Parametros + ", CodigoSAP = " + obj.NullableTrim(E_Neumatico.CodigoSAP);
            Parametros = Parametros + ", DescripcionSAP = " + obj.NullableTrim(E_Neumatico.DescripcionSAP);
            Parametros = Parametros + ", IdDisenio = " + obj.NullableTrim(E_Neumatico.IdDisenio.ToString());
            Parametros = Parametros + ", IdTipoBanda = " + obj.NullableTrim(E_Neumatico.IdTipoBanda.ToString());
            Parametros = Parametros + ", FechaAlta = " + obj.NullableTrim(E_Neumatico.FechaAlta.ToString());
            Parametros = Parametros + ", FechaBaja = " + E_Neumatico.FechaBaja;
            Parametros = Parametros + ", Observacion = " + obj.NullableTrim(E_Neumatico.Observacion);
            Parametros = Parametros + ", IdEstadoN = " + obj.NullableTrim(E_Neumatico.IdEstadoN.ToString());
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_Neumatico.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_Neumatico.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_Neumatico.IdUsuarioModificacion.ToString());
            Parametros = Parametros + ", IdUC = " + obj.NullableTrim(E_Neumatico.IdUC.ToString());
            Debug.EscribirDebug(Metodo, Parametros);

        }
    }
}
