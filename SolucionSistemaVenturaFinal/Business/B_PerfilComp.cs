using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_PerfilComp
    {
        public int PerfilComp_Insert(E_PerfilComp E_PerfilComp)
        {
            PerfilComp_Debug("PerfilComp_Insert", E_PerfilComp);
            return Data.D_PerfilComp.PerfilComp_Insert(E_PerfilComp);
        }

        public int PerfilComp_Update(E_PerfilComp E_PerfilComp)
        {
            PerfilComp_Debug("PerfilComp_Update", E_PerfilComp);
            return Data.D_PerfilComp.PerfilComp_Update(E_PerfilComp);
        }

        public int PerfilComp_Delete(E_PerfilComp E_PerfilComp)
        {
            PerfilComp_Debug("PerfilComp_Delete", E_PerfilComp);
            return Data.D_PerfilComp.PerfilComp_Delete(E_PerfilComp);
        }

        public DataTable PerfilComp_List(E_PerfilComp E_PerfilComp)
        {
            PerfilComp_Debug("PerfilComp_List", E_PerfilComp);
            return Data.D_PerfilComp.PerfilComp_List(E_PerfilComp);
        }

        public DataTable PerfilComp_ListWithNoParent(E_PerfilComp E_PerfilComp)
        {
            PerfilComp_Debug("PerfilComp_ListWithNoParent", E_PerfilComp);
            return D_PerfilComp.PerfilComp_ListWithNoParent(E_PerfilComp);
        }

        public DataTable PerfilComp_GetItem(E_PerfilComp E_PerfilComp)
        {
            PerfilComp_Debug("PerfilComp_GetItem", E_PerfilComp);
            return D_PerfilComp.PerfilComp_GetItem(E_PerfilComp);
        }

        public DataTable PerfilComp_Combo()
        {
            E_PerfilComp obj = new E_PerfilComp();
            PerfilComp_Debug("PerfilComp_Combo", obj);
            return D_PerfilComp.PerfilComp_Combo();
        }

        public DataTable PerfilComp_GetBeforeDel(E_PerfilComp E_PerfilComp)
        {
            PerfilComp_Debug("PerfilComp_GetBeforeDel", E_PerfilComp);
            return D_PerfilComp.PerfilComp_GetBeforeDel(E_PerfilComp);;
        }

        public DataTable PerfilComp_GetBeforeChange(E_PerfilComp E_PerfilComp, int IdTipoConsulta)
        {
            PerfilComp_Debug("PerfilComp_GetBeforeChange", E_PerfilComp);
            return D_PerfilComp.PerfilComp_GetBeforeChange(E_PerfilComp, IdTipoConsulta);
        }

        public static void PerfilComp_Debug(string Metodo, E_PerfilComp E_PerfilComp)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdPerfilComp = " + E_PerfilComp.Idperfilcomp.ToString();
            Parametros = Parametros + ", CodPerfilComp = " + obj.NullableTrim(E_PerfilComp.Codperfilcomp);
            Parametros = Parametros + ", PerfilComp = " + obj.NullableTrim(E_PerfilComp.Perfilcomp);
            Parametros = Parametros + ", IdPerfilCompPadre = " + E_PerfilComp.Idperfilcomppadre.ToString();
            Parametros = Parametros + ", IdPerfil = " + E_PerfilComp.Idperfil.ToString();
            Parametros = Parametros + ", CodigoSAP = " + obj.NullableTrim(E_PerfilComp.Codigosap);
            Parametros = Parametros + ", DescripcionSAP = " + obj.NullableTrim(E_PerfilComp.Descripcionsap);
            Parametros = Parametros + ", IdEstadoPC = " + E_PerfilComp.Idestadopc.ToString();
            Parametros = Parametros + ", FlagActivo = " + E_PerfilComp.Flagactivo.ToString();
            Parametros = Parametros + ", IdUsuarioCreacion = " + E_PerfilComp.Idusuariocreacion.ToString();
            Parametros = Parametros + ", IdUsuarioModificacion = " + E_PerfilComp.Idusuariomodificacion.ToString();
            Debug.EscribirDebug(Metodo, Parametros);
        }

        public DataTable Actividad_ComboByPerfil(E_PerfilComp E_PerfilComp)
        {
            PerfilComp_Debug("Actividad_ComboByPerfil", E_PerfilComp);
            return D_PerfilComp.Actividad_ComboByPerfil(E_PerfilComp);
        }

        public DataTable Tarea_ComboByPerfil(E_PerfilComp E_PerfilComp)
        {
            PerfilComp_Debug("Tarea_ComboByPerfil", E_PerfilComp);
            return D_PerfilComp.Tarea_ComboByPerfil(E_PerfilComp);
        }

        public DataTable PerfilDetalle_ComboByPerfil(E_PerfilComp E_PerfilComp)
        {
            PerfilComp_Debug("PerfilDetalle_ComboByPerfil", E_PerfilComp);
            return D_PerfilComp.PerfilDetalle_ComboByPerfil(E_PerfilComp);
        }
    }
}
