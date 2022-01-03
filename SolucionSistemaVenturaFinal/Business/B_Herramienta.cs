using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_Herramienta
    {
        public DataTable Herramienta_Combo()
        {
            E_Herramienta obj = new E_Herramienta();
            Herramienta_Debug("Actividad_Combo", obj);
            return D_Herramienta.Herramienta_Combo();
        }
        public static int Herramienta_UpdateCascade(E_Herramienta obje,DataTable tblSerie)
        {
            Herramienta_Debug("Herramienta_UpdateCascade", obje);
            return D_Herramienta.Herramienta_UpdateCascade(obje, tblSerie);
        }

        public static DataTable Herramienta_List(E_Herramienta E_Herramienta)
        {
            Herramienta_Debug("Herramienta_List", E_Herramienta);
            return D_Herramienta.Herramienta_List(E_Herramienta);
        }

        public static DataTable Herramienta_GetItem(E_Herramienta E_Herramienta)
        {
            Herramienta_Debug("Herramienta_GetItem", E_Herramienta);
            return D_Herramienta.Herramienta_GetItem(E_Herramienta);
        }

        public static int Herramienta_Insert(E_Herramienta E_Herramienta)
        {
            Herramienta_Debug("Herramienta_Insert", E_Herramienta);
            return D_Herramienta.Herramienta_Insert(E_Herramienta);
        }

        public static int Herramienta_Update(E_Herramienta E_Herramienta)
        {
            Herramienta_Debug("Herramienta_Update", E_Herramienta);
            return D_Herramienta.Herramienta_Update(E_Herramienta);
        }

        public static DataTable Herramienta_GetItemByDesc(E_Herramienta E_Herramienta)
        {
            E_Herramienta obj = new E_Herramienta();
            Herramienta_Debug("Herramienta_GetItemByDesc", obj);
            return D_Herramienta.Herramienta_GetItemByDesc(E_Herramienta);
        }

        public int Herramienta_GetCantItems(E_Herramienta E_Herramienta)
        {
            Herramienta_Debug("Herramienta_GetCantItems", E_Herramienta);
            return D_Herramienta.Herramienta_GetCantItems(E_Herramienta);
        }

        public static void Herramienta_Debug(string Metodo, E_Herramienta E_Herramienta)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdHerramienta = " + E_Herramienta.IdHerramienta.ToString();
            Parametros = Parametros + ", CodHerramienta = " + obj.NullableTrim(E_Herramienta.CodHerramienta);
            Parametros = Parametros + ", Herramienta = " + obj.NullableTrim(E_Herramienta.Herramienta);
            Parametros = Parametros + ", Observacion = " + obj.NullableTrim(E_Herramienta.Observacion);
            Parametros = Parametros + ", IdEstadoH = " + E_Herramienta.IdEstadoH.ToString();
            Parametros = Parametros + ", FlagActivo = " + E_Herramienta.FlagActivo.ToString();
            Parametros = Parametros + ", IdUsuarioCreacion = " + E_Herramienta.IdUsuarioCreacion.ToString();
            Parametros = Parametros + ", IdUsuarioModificacion = " + E_Herramienta.IdUsuarioModificacion.ToString();
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
