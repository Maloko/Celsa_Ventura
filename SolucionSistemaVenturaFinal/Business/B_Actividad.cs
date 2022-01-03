using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_Actividad
    {
        public DataTable Actividad_Combo()
        {
            E_Actividad obj = new E_Actividad();
            Actividad_Debug("Actividad_Combo", obj);
            return D_Actividad.Actividad_Combo();
        }

        public DataTable Actividad_List(E_Actividad E_Actividad)
        {
            Actividad_Debug("Actividad_List", E_Actividad);
            return D_Actividad.Actividad_List(E_Actividad);
        }

        public DataTable Actividad_GetItem(E_Actividad E_Actividad)
        {
            Actividad_Debug("Actividad_GetItem", E_Actividad);
            return D_Actividad.Actividad_GetItem(E_Actividad);
        }

        public DataTable Actividad_GetItemByDesc(E_Actividad E_Actividad)
        {            
            Actividad_Debug("Actividad_GetItemByDesc", E_Actividad);
            return D_Actividad.Actividad_GetItemByDesc(E_Actividad);
        }

        public int Actividad_Update(E_Actividad E_Actividad)
        {
            Actividad_Debug("Actividad_Update", E_Actividad);
            return D_Actividad.Actividad_Update(E_Actividad);
        }

        public int Actividad_Insert(E_Actividad E_Actividad)
        {
            Actividad_Debug("Actividad_Insert", E_Actividad);
            return D_Actividad.Actividad_Insert(E_Actividad);
        }

        public int Actividad_GetBeforeChange(E_Actividad E_Actividad)
        {
            Actividad_Debug("Actividad_GetBeforeChange", E_Actividad);
            return D_Actividad.Actividad_GetBeforeChange(E_Actividad);
        }

        public static void Actividad_Debug(string Metodo, E_Actividad E_Actividad)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdActividad = " + E_Actividad.IdActividad.ToString();
            Parametros = Parametros + ", CodActividad = " + obj.NullableTrim(E_Actividad.CodActividad);
            Parametros = Parametros + ", Actividad = " + obj.NullableTrim(E_Actividad.Actividad);
            Parametros = Parametros + ", IdEstadoActividad = " + E_Actividad.IdEstadoActividad.ToString();
            Parametros = Parametros + ", FlagActivo = " + E_Actividad.FlagActivo.ToString();
            Parametros = Parametros + ", IdUsuarioCreacion = " + E_Actividad.IdUsuarioCreacion.ToString();
            Parametros = Parametros + ", IdUsuarioModificacion = " + E_Actividad.IdUsuarioModificacion.ToString();
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
