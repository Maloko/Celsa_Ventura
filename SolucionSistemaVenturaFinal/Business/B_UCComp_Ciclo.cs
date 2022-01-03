using System.Data;
using Entities;
using Utilitarios;

namespace Business
{
    public class B_UCComp_Ciclo
    {
        public DataTable PerfilComp_Ciclo_List(E_UCComp_Ciclo E_UCComp_Ciclo)
        {
            DataTable tbl = new DataTable();
            tbl = Data.D_UCComp_Ciclo.PerfilComp_Ciclo_List(E_UCComp_Ciclo);
            return tbl;
        }

        public DataTable Item_Ciclo_List(E_UCComp E_UCComp)
        {
            DataTable tbl = new DataTable();
            tbl = Data.D_UCComp_Ciclo.Item_Ciclo_List(E_UCComp);
            return tbl;
        }

        public static void UCComp_Ciclo_Debug(string Metodo, E_UCComp_Ciclo E_UCComp_Ciclo)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdUCCompCiclo= " + obj.NullableTrim(E_UCComp_Ciclo.IdUCCompCiclo.ToString());
            Parametros = Parametros + ", IdUCComp = " + obj.NullableTrim(E_UCComp_Ciclo.IdUCComp.ToString());
            Parametros = Parametros + ", IdPerfilCompCiclo = " + obj.NullableTrim(E_UCComp_Ciclo.IdPerfilCompCiclo.ToString());
            Parametros = Parametros + ", FrecuenciaCambio = " + obj.NullableTrim(E_UCComp_Ciclo.FrecuenciaCambio.ToString());
            Parametros = Parametros + ", Contador = " + obj.NullableTrim(E_UCComp_Ciclo.Contador.ToString());
            Parametros = Parametros + ", FrecuenciaExtendida = " + obj.NullableTrim(E_UCComp_Ciclo.FrecuenciaExtendida.ToString());
            Parametros = Parametros + ", FlagCicloPrincipal = " + obj.NullableTrim(E_UCComp_Ciclo.FlagCicloPrincipal.ToString());
            Parametros = Parametros + ", IdEstadoCiclo = " + obj.NullableTrim(E_UCComp_Ciclo.IdEstadoCiclo.ToString());
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_UCComp_Ciclo.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_UCComp_Ciclo.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", FechaCreacion = " + obj.NullableTrim(E_UCComp_Ciclo.FechaCreacion.ToString());
            Parametros = Parametros + ", HostCreacion = " + obj.NullableTrim(E_UCComp_Ciclo.HostCreacion);
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_UCComp_Ciclo.IdUsuarioModificacion.ToString());
            Parametros = Parametros + ", FechaModificacion = " + obj.NullableTrim(E_UCComp_Ciclo.FechaModificacion.ToString());
            Parametros = Parametros + ", HostModificacion = " + obj.NullableTrim(E_UCComp_Ciclo.HostModificacion);
            Parametros = Parametros + ", IdPerfil = " + obj.NullableTrim(E_UCComp_Ciclo.IdPerfil.ToString());
            Debug.EscribirDebug(Metodo, Parametros);

        }
    }
}
