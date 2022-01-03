using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_UCComp
    {
        public DataTable UCComp_List(E_UCComp E_UCComp)
        {
            UCComp_Debug("UCComp_List", E_UCComp);
            DataTable tbl = new DataTable();
            tbl = Data.D_UCComp.UCComp_List(E_UCComp);
            return tbl;
        }

        public DataTable UCComp_ListWithNoParent(E_UCComp E_UCComp)
        {
            UCComp_Debug("UCComp_ListWithNoParent", E_UCComp);
            DataTable tbl = new DataTable();
            tbl = D_UCComp.UCComp_ListWithNoParent(E_UCComp);
            return tbl;
        }

        public int UCComp_UpdateCascade(E_UCComp obje, DataTable tbl)
        {
            UCComp_Debug("UCComp_UpdateCascade", obje);
            return D_UCComp.UCComp_UpdateCascade(obje, tbl);
        }

        public DataTable UCComp_GetBeforeChange(E_UCComp E_UCComp)
        {
            UCComp_Debug("UCComp_GetBeforeChange", E_UCComp);
            return D_UCComp.UCComp_GetBeforeChange(E_UCComp);
        }

        public static void UCComp_Debug(string Metodo, E_UCComp E_UCComp)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdUCComp = " + obj.NullableTrim(E_UCComp.IdUCComp.ToString());
            Parametros = Parametros + ", IdPerfilComp = " + obj.NullableTrim(E_UCComp.IdPerfilComp.ToString());
            Parametros = Parametros + ", IdUC = " + obj.NullableTrim(E_UCComp.IdUC.ToString());
            Parametros = Parametros + ", IdTipoDetalle =" + obj.NullableTrim(E_UCComp.IdTipoDetalle.ToString());
            Parametros = Parametros + ", IdItem =" + obj.NullableTrim(E_UCComp.IdItem.ToString());
            Parametros = Parametros + ", NroSerie = " + obj.NullableTrim(E_UCComp.NroSerie);
            Parametros = Parametros + ", CodigoSAP =" + obj.NullableTrim(E_UCComp.CodigoSAP);
            Parametros = Parametros + ", DescripcionSAP =" + obj.NullableTrim(E_UCComp.DescripcionSAP);
            Parametros = Parametros + ", IdEstadoUCComp = " + obj.NullableTrim(E_UCComp.IdEstadoUCComp.ToString());
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_UCComp.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_UCComp.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_UCComp.IdUsuarioModificacion.ToString());
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
