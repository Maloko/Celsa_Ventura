using Entities;
using Utilitarios;
using System.Data;

namespace Business
{
    public class B_FormatoImpresion
    {
        public int FormatoImpresion_Insert(E_FormatoImpresion E_FormatoImpresion)
        {
            int ds = Data.D_FormatoImpresion.FormatoImpresion_Insert(E_FormatoImpresion);
            FormatoImpresion_Debug("FormatoImpresion_Insert", E_FormatoImpresion);
            return ds;
        }

        public static DataTable FormatoImpresion_GetItem(int IdMenu)
        {
            DataTable tbl = new DataTable();
            tbl = Data.D_FormatoImpresion.FormatoImpresion_GetItem(IdMenu);
            return tbl;
        }

        public static DataTable FormatoImpresion_GetFile(int IdFormatoImpresion)
        {
            DataTable tbl = new DataTable();
            tbl = Data.D_FormatoImpresion.FormatoImpresion_GetFile(IdFormatoImpresion);
            return tbl;
        }

        public static int FormatoImpresion_Update(E_FormatoImpresion E_FormatoImpresion)
        {
            int n = 0;
            n = Data.D_FormatoImpresion.FormatoImpresion_Update(E_FormatoImpresion);
            return n;
        }

        public static void FormatoImpresion_Debug(string Metodo, E_FormatoImpresion E_FormatoImpresion)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;

            Parametros = "IdFormatoImpresion = " + obj.NullableTrim(E_FormatoImpresion.IdFormatoImpresion.ToString());
            Parametros = Parametros + ", IdMenu = " + obj.NullableTrim(E_FormatoImpresion.IdMenu.ToString());
            Parametros = Parametros + ", NombreArchivo = " + obj.NullableTrim(E_FormatoImpresion.NombreArchivo.ToString());
            Parametros = Parametros + ", FlagActivo = " + E_FormatoImpresion.Flagactivo.ToString();
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_FormatoImpresion.Idusuariocreacion.ToString());
            Parametros = Parametros + ", FechaCreacion = " + obj.NullableTrim(E_FormatoImpresion.Fechacreacion);
            Parametros = Parametros + ", HostCreacion = " + obj.NullableTrim(E_FormatoImpresion.Hostcreacion);
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_FormatoImpresion.Idusuariomodificacion.ToString());
            Parametros = Parametros + ", FechaModificacion = " + obj.NullableTrim(E_FormatoImpresion.Fechamodificacion);
            Parametros = Parametros + ", HostModificacion = " + obj.NullableTrim(E_FormatoImpresion.Hostmodificacion);
            Debug.EscribirDebug(Metodo, Parametros);
        }

    }
}
