using System.Data;
using Data;
using Entities;
using Utilitarios;

namespace Business
{
    public class B_PerfilNeumatico
    {

        public static int PerfilNeumatico_Insert(E_PerfilNeumatico E_PerfilNeumatico)
        {
            int ds = D_PerfilNeumatico.PerfilNeumatico_Insert(E_PerfilNeumatico);
            return ds;
        }

        public static string PerfilNeumatico_Update(E_PerfilNeumatico E_PerfilNeumatico)
        {
            string ds = D_PerfilNeumatico.PerfilNeumatico_Update(E_PerfilNeumatico);
            return ds;
        }

        public static string PerfilNeumatico_Delete(E_PerfilNeumatico E_PerfilNeumatico)
        {
            string ds = D_PerfilNeumatico.PerfilNeumatico_Delete(E_PerfilNeumatico);
            return ds;
        }

        public static DataTable PerfilNeumatico_List(E_PerfilNeumatico E_PerfilNeumatico)
        {
            DataTable tbl = new DataTable();
            tbl = D_PerfilNeumatico.PerfilNeumatico_List(E_PerfilNeumatico);
            return tbl;
        }

        public static DataTable PerfilNeumatico_GetItem(E_PerfilNeumatico E_PerfilNeumatico)
        {
            DataTable tbl = new DataTable();
            tbl = D_PerfilNeumatico.PerfilNeumatico_GetItem(E_PerfilNeumatico);
            return tbl;
        }

        public static DataTable PerfilNeumatico_GetItemByDesc(E_PerfilNeumatico E_PerfilNeumatico)
        {
            DataTable tbl = new DataTable();
            tbl = D_PerfilNeumatico.PerfilNeumatico_GetItemByDesc(E_PerfilNeumatico);
            return tbl;
        }

        public static DataTable PerfilNeumatico_Combo()
        {
            DataTable tbl = new DataTable();
            tbl = D_PerfilNeumatico.PerfilNeumatico_Combo();
            return tbl;
        }

        public int PerfilNeumatico_UpdateCascade(E_PerfilNeumatico E_PerfilNeumatico, DataTable tbl)
        {
            PerfilNeumatico_Debug("PerfilNeumatico_UpdateCascade", E_PerfilNeumatico);
            return D_PerfilNeumatico.PerfilNeumatico_UpdateCascade(E_PerfilNeumatico, tbl);
        }

        public int PerfilNeumatico_InsertMasivo(E_PerfilNeumatico objE,DataTable tblPN,DataTable tblPNEje)
        {
            PerfilNeumatico_Debug("PerfilNeumatico_InsertMasivo", objE);
            return D_PerfilNeumatico.PerfilNeumatico_InsertMasivo(objE, tblPN, tblPNEje);
        }
        public static void PerfilNeumatico_Debug(string Metodo, E_PerfilNeumatico E_PerfilNeumatico)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdPerfilNeumatico= " + E_PerfilNeumatico.IdPerfilNeumatico.ToString();
            Parametros = Parametros + ", CodPerfilNeumatico = " + obj.NullableTrim(E_PerfilNeumatico.CodPerfilNeumatico);
            Parametros = Parametros + ", PerfilNeumatico = " + obj.NullableTrim(E_PerfilNeumatico.PerfilNeumatico);
            Parametros = Parametros + ", NroEjes = " + obj.NullableTrim(E_PerfilNeumatico.NroEjes.ToString());
            Parametros = Parametros + ", NroLlantaRepuesto = " + obj.NullableTrim(E_PerfilNeumatico.NroLlantaRepuesto.ToString());
            Parametros = Parametros + ", Observacion = " + obj.NullableTrim(E_PerfilNeumatico.Observacion);
            Parametros = Parametros + ", IdEstadoPN = " + E_PerfilNeumatico.IdEstadoPN.ToString();
            Parametros = Parametros + ", FlagActivo = " + E_PerfilNeumatico.FlagActivo.ToString();
            Parametros = Parametros + ", IdUsuarioCreacion = " + E_PerfilNeumatico.IdUsuarioCreacion.ToString();
            Parametros = Parametros + ", FechaCreacion = " + E_PerfilNeumatico.FechaCreacion.ToString();
            Parametros = Parametros + ", IdUsuarioModificacion = " + E_PerfilNeumatico.IdUsuarioModificacion.ToString();
            Parametros = Parametros + ", FechaModificacion = " + E_PerfilNeumatico.FechaModificacion.ToString();

            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
