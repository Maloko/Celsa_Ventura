using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_UC
    {
        public DataTable UC_List(E_UC E_UC)
        {
            E_UC obj = new E_UC();
            UC_Debug("UC_List", obj);
            DataTable tbl = new DataTable();
            tbl = Data.D_UC.UC_List(E_UC);
            return tbl;
        }

        public int UC_InsertMasivo(E_UC E_UC, DataTable tblPerfilComponentes, DataTable tblPerfilCompCiclo)
        {
            UC_Debug("UC_UpdateCascade", E_UC);
            D_UC objUc = new D_UC();
            int rpta = objUc.UC_InsertMasivo(E_UC, tblPerfilComponentes, tblPerfilCompCiclo);
            return rpta;
        }

        public static DataTable B_UC_Neumatico_List(E_UC E_UC)
        {
            UC_Debug("UC_Neumatico_List", E_UC);
            DataTable tbl = new DataTable();
            tbl = D_UC.UC_Neumatico_List(E_UC);
            return tbl;
        }

        public DataTable B_UC_GetItem(E_UC E_UC)
        {
            UC_Debug("UC_GetItem", E_UC);
            DataTable tbl = new DataTable();
            tbl = D_UC.UC_GetItem(E_UC);
            return tbl;
        }

        public E_UC B_UC_GetItemByCodUC(E_UC E_UC)
        {
            E_UC = D_UC.UC_GetItemByCodUC(E_UC);
            return E_UC;
        }

        public DataTable B_UC_Combo(E_UC E_UC)
        {
            UC_Debug("B_UC_Combo", E_UC);
            DataTable tbl = new DataTable();
            tbl = D_UC.UC_Combo(E_UC);
            return tbl;
        }

        public DataTable B_UC_ComboWithPN(E_UC E_UC)
        {
            UC_Debug("UC_ComboWithPN", E_UC);
            DataTable tbl = new DataTable();
            tbl = D_UC.UC_ComboWithPN(E_UC);
            return tbl;
        }

        public DataTable UC_ComboByUC(E_UC E_UC)
        {
            UC_Debug("UC_ComboByUC", E_UC);
            DataTable tbl = new DataTable();
            tbl = D_UC.UC_ComboByUC(E_UC);
            return tbl;
        }

        public DataTable B_UC_ComboNeumatico()
        {
            DataTable tbl = new DataTable();
            tbl = D_UC.UC_ComboNeumatico();
            return tbl;
        }

        public static DataTable B_Neumatico_ListByUC(E_UC E_UC)
        {
            UC_Debug("B_Neumatico_ListByUC", E_UC);
            DataTable tbl = new DataTable();
            tbl = D_UC.UC_Neumatico_ListByUC(E_UC);
            return tbl;
        }

        public int UCCambioEstado_UpdateCascade(E_UC E_UC, DataTable tblUCEstado)
        {
            UC_Debug("UCCambioEstado_UpdateCascade", E_UC);
            return D_UC.UCCambioEstado_UpdateCascade(E_UC, tblUCEstado);
        }

        public DataTable UC_GetBeforeChange(string IdUc)
        {
            E_UC E_UC = new Entities.E_UC();
            UC_Debug("UC_GetBeforeChange", E_UC);
            return D_UC.UC_GetBeforeChange(IdUc);
        }

        public  int UC_CargaMasiva(E_UC objE, DataTable tblUC, DataTable tblUCComp,DataTable tblItemCiclo)
        {
            UC_Debug("UC_CargaMasiva", objE);
            return D_UC.UC_CargaMasiva(objE, tblUC, tblUCComp, tblItemCiclo);
        }

        public static void UC_Debug(string Metodo, E_UC E_UC)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "CodUC = " + obj.NullableTrim(E_UC.CodUc);
            Parametros = Parametros + ", IdPerfil = " + obj.NullableTrim(E_UC.IdPerfil.ToString());
            Parametros = Parametros + ", IdPerfilNeumatico = " + obj.NullableTrim(E_UC.IdPerfilNeumatico.ToString());
            Parametros = Parametros + ", IdTipoUnidad =" + obj.NullableTrim(E_UC.IdTipoUnidad);
            Parametros = Parametros + ", IdUc =" + obj.NullableTrim(E_UC.IdUc.ToString());
            Parametros = Parametros + ", ContadorAcum = " + obj.NullableTrim(E_UC.ContadorAcum.ToString());
            Parametros = Parametros + ", Observacion =" + obj.NullableTrim(E_UC.Observacion);
            Parametros = Parametros + ", PlacaSerie =" + obj.NullableTrim(E_UC.PlacaSerie);
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_UC.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_UC.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_UC.IdUsuarioModificacion.ToString());
            Debug.EscribirDebug(Metodo, Parametros);
        }

        #region REQUERIMIENTO_03_CELSA
        public DataTable ContadoresxUC_List(string IdUc,int cicloPorDefecto)
        {
            return D_UC.ContadoresxUC_List(IdUc, cicloPorDefecto);
        }

        public void UC_UpdateFechaUltimaControl(E_OT E_OT)
        {
             D_UC.UC_UpdateFechaUltimoControl(E_OT);
        }

        #endregion
    }
}
