using System.Data;
using Data;
using Entities;
using Utilitarios;

namespace Business
{
    public  class B_TablaMaestra
    {

        public static DataTable TablaMaestra_Combo(E_TablaMaestra E_TablaMaestra)
        {
            //TablaMaestra_Debug("TablaMaestra_Combo", E_TablaMaestra); //Se esta escribiendo cada 20 sec
            return D_TablaMaestra.TablaMaestra_Combo(E_TablaMaestra); 
        }

        public DataTable TablaMaestra_List(E_TablaMaestra objE)
        {
            TablaMaestra_Debug("TablaMaestra_List", objE);
            return D_TablaMaestra.TablaMaestra_List(objE);
        }

        public DataTable TablaMaestra_GetItem(E_TablaMaestra objE)
        {
            TablaMaestra_Debug("TablaMaestra_GetItem", objE);
            return D_TablaMaestra.TablaMaestra_GetItem(objE);
        }

        public int TablaMaestra_Insert(E_TablaMaestra objE)
        {
            TablaMaestra_Debug("TablaMaestra_Insert", objE);
            return D_TablaMaestra.TablaMaestra_Insert(objE);
        }

        public int TablaMaestra_Update(E_TablaMaestra objE)
        {
            TablaMaestra_Debug("TablaMaestra_Update", objE);
            return D_TablaMaestra.TablaMaestra_Update(objE);
        }
        public int TablaMaestra_UpdateMasivo(E_TablaMaestra objE, DataTable tblTablaMaestra)
        {
            TablaMaestra_Debug("TablaMaestra_UpdateMasivo", objE);
            return D_TablaMaestra.TablaMaestra_UpdateMasivo(objE, tblTablaMaestra);

        }
        public static void TablaMaestra_Debug(string Metodo, E_TablaMaestra E_TablaMaestra)
        {
            Utilitarios.Utilitarios obj = new  Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdTabla = " + E_TablaMaestra.IdTabla.ToString();
            Parametros = Parametros + ", IdColumna = " + E_TablaMaestra.IdColumna.ToString();
            Parametros = Parametros + ", Valor = " + obj.NullableTrim(E_TablaMaestra.Valor);
            Parametros = Parametros + ", Descripcion = " + obj.NullableTrim(E_TablaMaestra.Descripcion);
            Parametros = Parametros + ", IdColumnaPadre = " + E_TablaMaestra.IdColumnaPadre.ToString();
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_TablaMaestra.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + E_TablaMaestra.IdUsuarioCreacion.ToString();
            Parametros = Parametros + ", IdUsuarioModificacion = " + E_TablaMaestra.IdUsuarioModificacion.ToString();
            Debug.EscribirDebug(Metodo, Parametros);
        }

        public static DataTable TablaMaestraByIdTabla(int idTabla)
        {
            return D_TablaMaestra.TablaMaestraByIdTabla(idTabla);
        }



    }
}
