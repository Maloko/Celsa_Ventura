using System.Data;
using Data;
using Entities;
using Utilitarios;

namespace Business
{
    public class B_PerfilNeumaticoEje
    {

        public static int PerfilNeumaticoEje_Insert(E_PerfilNeumaticoEje E_PerfilNeumaticoEje)
        {
            int ds = D_PerfilNeumaticoEje.PerfilNeumaticoEje_Insert(E_PerfilNeumaticoEje);
            return ds;
        }

        public static string PerfilNeumaticoEje_Update(E_PerfilNeumaticoEje E_PerfilNeumaticoEje)
        {
            string ds = D_PerfilNeumaticoEje.PerfilNeumaticoEje_Update(E_PerfilNeumaticoEje);
            return ds;
        }

        public static string PerfilNeumaticoEje_Delete(int idPerfilNeumatico)
        {
            string ds = D_PerfilNeumaticoEje.PerfilNeumaticoEje_Delete(idPerfilNeumatico);
            return ds;
        }

        public static DataTable PerfilNeumaticoEje_List(E_PerfilNeumaticoEje E_PerfilNeumaticoEje)
        {
            DataTable tbl = new DataTable();
            tbl = D_PerfilNeumaticoEje.PerfilNeumaticoEje_List(E_PerfilNeumaticoEje);
            return tbl;
        }

        public static DataTable PerfilNeumaticoEje_GetItem(int idPerfilNeumaticoEje)
        {
            DataTable tbl = new DataTable();
            tbl = D_PerfilNeumaticoEje.PerfilNeumaticoEje_GetItem(idPerfilNeumaticoEje);
            return tbl;
        }

        public static DataTable PerfilNeumaticoEje_Combo()
        {
            DataTable tbl = new DataTable();
            tbl = D_PerfilNeumaticoEje.PerfilNeumaticoEje_Combo();
            return tbl;
        }

        public static void PerfilNeumaticoEje_Debug(string Metodo, E_PerfilNeumaticoEje E_PerfilNeumaticoEje)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdPerfilNeumaticoEje = " + E_PerfilNeumaticoEje.IdPerfilNeumaticoEje.ToString();
            Parametros = Parametros + ", IdPerfilNeumatico = " + E_PerfilNeumaticoEje.IdPerfilNeumatico.ToString();
            Parametros = Parametros + ", Eje = " + obj.NullableTrim(E_PerfilNeumaticoEje.Eje.ToString());
            Parametros = Parametros + ", NroLlantas = " + obj.NullableTrim(E_PerfilNeumaticoEje.NroLlantas.ToString());
            Parametros = Parametros + ", FlagActivo = " + E_PerfilNeumaticoEje.FlagActivo.ToString();
            Parametros = Parametros + ", IdUsuarioCreacion = " + E_PerfilNeumaticoEje.IdUsuarioCreacion.ToString();
            Parametros = Parametros + ", IdUsuarioModificacion = " + E_PerfilNeumaticoEje.IdUsuarioModificacion.ToString();

            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
