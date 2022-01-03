using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_Ciclo
    {
        public int Ciclo_Insert(E_Ciclo E_Ciclo)
        {
            Ciclo_Debug("Ciclo_Insert", E_Ciclo);
            return D_Ciclo.Ciclo_Insert(E_Ciclo);
        }

        public int Ciclo_Update(E_Ciclo E_Ciclo)
        {
            Ciclo_Debug("Ciclo_Update", E_Ciclo);
            return D_Ciclo.Ciclo_Update(E_Ciclo);
        }

        public int Ciclo_Delete(int IdCiclo)
        {
            return D_Ciclo.Ciclo_Delete(IdCiclo);
        }

        public DataTable Ciclo_List(E_Ciclo E_Ciclo)
        {
            Ciclo_Debug("Ciclo_List", E_Ciclo);
            return D_Ciclo.Ciclo_List(E_Ciclo);
        }

        public DataTable Ciclo_GetItem(int IdCiclo)
        {
            return D_Ciclo.Ciclo_GetItem(IdCiclo);
        }

        public DataTable Ciclo_Combo()
        {
            return D_Ciclo.Ciclo_Combo();
        }
        public DataTable Ciclo_ComboByPerfil(E_Perfil E_Perfil)
        {
            return D_Ciclo.Ciclo_ComboByPerfil(E_Perfil);
        }

        public DataTable Ciclo_ComboByUC(E_Perfil E_Perfil)
        {
            return D_Ciclo.Ciclo_ComboByUC(E_Perfil);
        }

        public static void Ciclo_Debug(string Metodo, E_Ciclo E_Ciclo)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;

            Parametros = "IdCiclo = " + obj.NullableTrim(E_Ciclo.Idciclo.ToString());
            Parametros = Parametros + ", IdTipoCiclo = " + obj.NullableTrim(E_Ciclo.Idtipociclo.ToString());
            Parametros = Parametros + ", Ciclo = " + obj.NullableTrim(E_Ciclo.Ciclo.ToString());
            Parametros = Parametros + ", IdEstadoPC = " + obj.NullableTrim(E_Ciclo.Idestadopc.ToString());
            Parametros = Parametros + ", FlagActivo = " + E_Ciclo.Flagactivo.ToString();
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_Ciclo.Idusuariocreacion.ToString());
            Parametros = Parametros + ", FechaCreacion = " + obj.NullableTrim(E_Ciclo.Fechacreacion);
            Parametros = Parametros + ", HostCreacion = " + obj.NullableTrim(E_Ciclo.Hostcreacion);
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_Ciclo.Idusuariomodificacion.ToString());
            Parametros = Parametros + ", FechaModificacion = " + obj.NullableTrim(E_Ciclo.Fechamodificacion);
            Parametros = Parametros + ", HostModificacion = " + obj.NullableTrim(E_Ciclo.Hostmodificacion);
            Debug.EscribirDebug(Metodo, Parametros);
        }
    }
}
