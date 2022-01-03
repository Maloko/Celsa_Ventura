using Data;
using Entities;
using System.Data;
using Utilitarios;

namespace Business
{
    public class B_HerramientaItem
    {
        public static DataTable Herramientaitem_List(E_HerramientaItem E_HerramientaItem)
        {
            HerramientaItem_Debug("Herramientaitem_List", E_HerramientaItem);
            return D_HerramientaItem.HerramientaItem_List(E_HerramientaItem);
        }

        public static int HerramientaItem_Delete(E_HerramientaItem obje)
        {
            HerramientaItem_Debug("HerramientaItem_Delete", obje);
            return D_HerramientaItem.HerramientaItem_Delete(obje);
        }
        public static DataTable Herramientaitem_GetItem(E_HerramientaItem E_HerramientaItem)
        {
            HerramientaItem_Debug("Herramientaitem_GetItem", E_HerramientaItem);
            return D_HerramientaItem.HerramientaItem_GetItem(E_HerramientaItem);
        }

        public static int Herramientaitem_Insert(E_HerramientaItem E_HerramientaItem)
        {
            HerramientaItem_Debug("Herramientaitem_Insert", E_HerramientaItem);
            return D_HerramientaItem.HerramientaItem_Insert(E_HerramientaItem);
        }

        public static int HerramientaItem_Update(E_HerramientaItem E_HerramientaItem)
        {
            HerramientaItem_Debug("HerramientaItem_Update", E_HerramientaItem);
            return D_HerramientaItem.HerramientaItem_Update(E_HerramientaItem);
        }

        public static int HerramientaItem_DeleteMasivo(E_HerramientaItem E_HerramientaItem)
        {
            HerramientaItem_Debug("HerramientaItem_DeleteMasivo", E_HerramientaItem);
            return D_HerramientaItem.HerramientaItem_DeleteMasivo(E_HerramientaItem);
        }

        public static DataTable HerramientaItem_GetItemByDesc(E_HerramientaItem E_HerramientaItem)
        {
            HerramientaItem_Debug("HerramientaItem_GetItemByDesc", E_HerramientaItem);
            return D_HerramientaItem.HerramientaItem_GetItemByDesc(E_HerramientaItem);
        }

        public static void HerramientaItem_Debug(string Metodo, E_HerramientaItem E_HerramientaItem)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdHerramientaItem = " + E_HerramientaItem.IdHerramientaItem.ToString();
            Parametros = Parametros + ", IdHerramienta = " + E_HerramientaItem.IdHerramienta.ToString();
            Parametros = Parametros + ", NroSerie = " + obj.NullableTrim(E_HerramientaItem.NroSerie);
            Parametros = Parametros + ", IdEstadoDisponible = " + E_HerramientaItem.IdEstadoDisponible.ToString();
            Parametros = Parametros + ", FlagActivo = " + E_HerramientaItem.FlagActivo.ToString();
            Parametros = Parametros + ", IdUsuarioCreacion = " + E_HerramientaItem.IdUsuarioCreacion.ToString();
            Parametros = Parametros + ", IdUsuarioModificacion = " + E_HerramientaItem.IdUsuarioModificacion.ToString();
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
