using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_Tarea
    {
        public static DataTable Tarea_List(E_Tarea E_Tarea)
        {
            Tarea_Debug("Tarea_List", E_Tarea);
            return D_Tarea.Tarea_List(E_Tarea);
        }

        public static DataTable Tarea_GetItem(E_Tarea E_Tarea)
        {
            Tarea_Debug("Tarea_GetItem", E_Tarea);
            return D_Tarea.Tarea_GetItem(E_Tarea);
        }

        public static DataTable Tarea_GetItemByDesc(E_Tarea E_Tarea)
        {
            Tarea_Debug("Tarea_GetItemByDesc", E_Tarea);
            DataTable tbl = new DataTable();
            tbl = D_Tarea.Tarea_GetItemByDesc(E_Tarea);
            return tbl;
        }

        public static int Tarea_Update(E_Tarea E_Tarea)
        {
            Tarea_Debug("Tarea_Update", E_Tarea);
            return D_Tarea.Tarea_Update(E_Tarea);
        }

        public static int Tarea_Insert(E_Tarea E_Tarea)
        {
            Tarea_Debug("Tarea_Insert", E_Tarea);
            return D_Tarea.Tarea_Insert(E_Tarea);
        }

        public DataTable Tarea_Combo()
        {
            E_Tarea obj = new E_Tarea();
            Tarea_Debug("Tarea_Combo", obj);
            return D_Tarea.Tarea_Combo();
        }

        public static void Tarea_Debug(string Metodo, E_Tarea E_Tarea)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdTarea = " + E_Tarea.IdTarea.ToString();
            Parametros = Parametros + ", CodTarea = " + obj.NullableTrim(E_Tarea.CodTarea);
            Parametros = Parametros + ", Tarea = " + obj.NullableTrim(E_Tarea.Tarea);
            Parametros = Parametros + ", IdActividad = " + E_Tarea.IdActividad.ToString();
            Parametros = Parametros + ", IdEstadoT = " + E_Tarea.IdEstadoT.ToString();
            Parametros = Parametros + ", FlagActivo = " + E_Tarea.FlagActivo.ToString();
            Parametros = Parametros + ", IdUsuarioCreacion = " + E_Tarea.IdUsuarioCreacion.ToString();
            Parametros = Parametros + ", IdUsuarioModificacion = " + E_Tarea.IdUsuarioModificacion.ToString();
            Debug.EscribirDebug(Metodo, Parametros);
        }
        public DataTable Tarea_ComboByAct(E_Tarea E_Tarea)
        {
            Tarea_Debug("Tarea_ComboByAct", E_Tarea);
            DataTable tbl = new DataTable();
            tbl = D_Tarea.Tarea_ComboByAct(E_Tarea);
            return tbl;
        }
    }
}
