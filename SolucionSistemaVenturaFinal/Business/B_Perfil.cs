using System.Data;
using Entities;
using Data;
using Utilitarios;

namespace Business
{
    public class B_Perfil
    {
        public int Perfil_InsertMasivo(E_Perfil E_Perfil, DataTable tblPerfilComp, DataTable tblPerfilComp_Actividad, DataTable tblPerfilComp_Ciclo, DataTable tblPerfilTarea, DataTable tblPerfilDetalleHerramienta, DataTable tblPerfilDetalleRepuesto, DataTable tblPerfilDetalleConsumible)
        {
            D_Perfil objPerfil = new D_Perfil();
            Perfil_Debug("Perfil_InsertMasivo", E_Perfil);
            return objPerfil.Perfil_InsertMasivo(E_Perfil, tblPerfilComp, tblPerfilComp_Actividad, tblPerfilComp_Ciclo, tblPerfilTarea, tblPerfilDetalleHerramienta, tblPerfilDetalleRepuesto, tblPerfilDetalleConsumible);
        }

        public int Perfil_Insert(E_Perfil E_Perfil)
        {
            Perfil_Debug("Perfil_Insert", E_Perfil);
            return Data.D_Perfil.Perfil_Insert(E_Perfil);
        }

        public int Perfil_Update(E_Perfil E_Perfil)
        {
            Perfil_Debug("Perfil_Update", E_Perfil);
            return Data.D_Perfil.Perfil_Update(E_Perfil);
        }

        public int Perfil_Delete(E_Perfil E_Perfil)
        {
            Perfil_Debug("Perfil_Delete", E_Perfil);
            return Data.D_Perfil.Perfil_Delete(E_Perfil);
        }

        public DataTable Perfil_List()
        {
            E_Perfil obj = new E_Perfil();
            Perfil_Debug("Perfil_List", obj);
            return Data.D_Perfil.Perfil_List();
        }

        public DataTable Perfil_ComboWithPM()
        {
            E_Perfil obj = new E_Perfil();
            Perfil_Debug("Perfil_ComboWithPM", obj);
            return D_Perfil.Perfil_ComboWithPM();
        }

        public DataTable Perfil_GetItem(E_Perfil E_Perfil)
        {
            Perfil_Debug("Perfil_GetItem", E_Perfil);
            return D_Perfil.Perfil_GetItem(E_Perfil);
        }

        public DataTable Perfil_Combo()
        {
            E_Perfil obj = new E_Perfil();
            Perfil_Debug("Perfil_Combo", obj);
            return D_Perfil.Perfil_Combo();
        }

        public DataTable Perfil_GetItemByDesc(E_Perfil E_Perfil)
        {
            Perfil_Debug("Perfil_Combo", E_Perfil);
            return D_Perfil.Perfil_GetItemByDesc(E_Perfil);
        }
        public int Perfil_CargaMasiva(E_Perfil objE, DataTable tblP, DataTable tblPC, DataTable tblPCCiclo, DataTable tblPCActividad, DataTable tblPerfilTarea, DataTable tblPerfildetalle)
        {
            Perfil_Debug("Perfil_CargaMasiva", objE);
            return D_Perfil.Perfil_CargaMasiva(objE, tblP, tblPC, tblPCCiclo, tblPCActividad, tblPerfilTarea, tblPerfildetalle);
        }
        public static void Perfil_Debug(string Metodo, E_Perfil E_Perfil)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;
            Parametros = "IdPerfil = " + E_Perfil.Idperfil.ToString();
            Parametros = Parametros + ", Codperfil = " + obj.NullableTrim(E_Perfil.Codperfil);
            Parametros = Parametros + ", Perfil = " + obj.NullableTrim(E_Perfil.Perfil);
            Parametros = Parametros + ", Idtipounidad = " + obj.NullableTrim(E_Perfil.Idtipounidad);
            Parametros = Parametros + ", IdPerfilNeumatico = " + E_Perfil.Idperfilneumatico.ToString();
            Parametros = Parametros + ", IdEstadoP = " + E_Perfil.Idestadop.ToString();
            Parametros = Parametros + ", FlagActivo = " + E_Perfil.Flagactivo.ToString();
            Parametros = Parametros + ", IdUsuarioCreacion = " + E_Perfil.Idusuariocreacion.ToString();
            Parametros = Parametros + ", IdUsuarioModificacion = " + E_Perfil.Idusuariomodificacion.ToString();
            Debug.EscribirDebug(Metodo, Parametros);
        }

        public  E_Perfil GetPerfilByCodUC(E_UC objUC)
        {
            return D_Perfil.GetPerfilByCodUC(objUC);
        }

    }
}
