using System;
using System.Data;
using Entities;
using Data;
using Utilitarios;
namespace Business
{
    public class B_OT
    {
        public int B_OT_Insert(E_OT E_OT)
        {
            OT_Debug("B_OT_Insert", E_OT);
            return D_OT.OT_Insert(E_OT);
        }

        public DataTable B_OT_List(E_OT E_OT)
        {
            OT_Debug("B_OT_List", E_OT);
            return D_OT.OT_List(E_OT);
        }

        public DataTable OTHerramienta_GetTreeVieNrSeries(string IdOT)
        {
            E_OT E_OT = new E_OT();
            OT_Debug("OTHerramienta_GetTreeVieNrSeries", E_OT);
            return D_OT.OTHerramienta_GetTreeVieNrSeries(IdOT);
        }

        public DataTable OTHerramienta_GetTreeVieNrSeriesByIdHerramienta(DataTable tblOTHerramienta)
        {
            E_OT E_OT = new E_OT();
            OT_Debug("OTHerramienta_GetTreeVieNrSeriesByIdHerramienta", E_OT);
            return D_OT.OTHerramienta_GetTreeVieNrSeriesByIdHerramienta(tblOTHerramienta);
        }

        public int OT_UpdateCascada(E_OT E_OT, DataTable tblOT, DataTable tblOTReprog)
        {
            D_OT objOT = new D_OT();
            return objOT.OTReprog_UpdateCascada(E_OT, tblOT, tblOTReprog);
        }

        public int OTHerramientas_UpdateNroSeries(DataTable tblNroSeriesAsignadas, DateTime FechaModificacion, int IdUsuario)
        {
            D_OT objOT = new D_OT();
            return objOT.OTHerramientas_UpdateNroSeries(tblNroSeriesAsignadas, FechaModificacion, IdUsuario);
        }

        public int OTEstado_UpdateCascada(E_OT E_OT, DataTable tblOT, DataTable tblOTEstado, DataTable tblNroSeriesAsignadas)
        {
            D_OT objOT = new D_OT();
            return objOT.OTEstado_UpdateCascada(E_OT, tblOT, tblOTEstado, tblNroSeriesAsignadas);
        }

        public DataTable OTActividad_Combo(E_OT E_OT)
        {
            OT_Debug("OTActividad_Combo", E_OT);
            return D_OT.OTActividad_Combo(E_OT);
        }

        public DataTable OTArticulo_GetNroSolByOT(E_OT E_OT)
        {
            OT_Debug("OTArticulo_GetNroSolByOT", E_OT);
            return D_OT.OTArticulo_GetNroSolByOT(E_OT);
        }

        public DataTable OT_Get(E_OT E_OT)
        {
            OT_Debug("OT_Get", E_OT);
            return D_OT.OT_Get(E_OT);
        }

        public DataTable OTHerramienta_ComboMasive(string IdOTS)
        {
            E_OT E_OT = new E_OT();
            OT_Debug("OT_Get", E_OT);
            return D_OT.OTHerramienta_ComboMasive(IdOTS);
        }

        public DataSet OT_UpdateCascada(E_OT E_OT, DataTable tblOTComp, DataTable tblOTActividad, DataTable tblOTTarea, DataTable tblOTHerramienta, DataTable tblOTRepuesto, DataTable tblOTConsumible)
        {
            D_OT objOT = new D_OT();
            return objOT.OT_UpdateCascada(E_OT, tblOTComp, tblOTActividad, tblOTTarea, tblOTHerramienta, tblOTRepuesto, tblOTConsumible);
        }

        public DataTable OTTarea_Combo(E_OT E_OT)
        {
            OT_Debug("OTTarea_Combo", E_OT);
            return D_OT.OTTarea_Combo(E_OT);
        }
        public DataTable OTArticulo_Combo(E_OT E_OT)
        {
            OT_Debug("OTArticulo_Combo", E_OT);
            return D_OT.OTArticulo_Combo(E_OT);
        }
        public DataTable OTHerramienta_Combo(E_OT E_OT)
        {
            OT_Debug("OTHerramienta_Combo", E_OT);
            return D_OT.OTHerramienta_Combo(E_OT);
        }

        public int OT_UpdateEstado(E_OT E_OT)
        {
            OT_Debug("OT_UpdateEstado", E_OT);
            return D_OT.OT_UpdateEstado(E_OT);
        }
        public int OT_Delete(E_OT E_OT)
        {
            OT_Debug("OT_Delete", E_OT);
            return D_OT.OT_Delete(E_OT);
        }

        public int OTArticulo_Update(int IdTipo, DataTable tblOTArticuloSol)
        {
            int cantfil = 0;
            cantfil = D_OT.OTArticulo_Update(IdTipo,tblOTArticuloSol);
            return cantfil;
        }

        public int OTCompActividadEstado_Insert(E_OT E_OT, DataTable tblOTActividad)
        {
            return D_OT.OTCompActividadEstado_Insert(E_OT, tblOTActividad);
        }

        public DataSet OTCompActividadEstado_Listar(E_OT E_OT)
        {
            OT_Debug("OTCompActividadEstado_Listar", E_OT);
            return D_OT.OTCompActividadEstado_Listar(E_OT);        
        }

        public int OTCompActividadEstado_Update(E_OT E_OT)
        {
            OT_Debug("OTCompActividadEstado_Update", E_OT);
            return D_OT.OTCompActividadEstado_Update(E_OT);
        }
        public  int PerfilCompActividad_Max()
        {
            return D_OT.PerfilCompActividad_Max();
        }

        public DataTable OTArticulo_ListSolSAP(E_OT E_OT)
        {
            OT_Debug("OTArticulo_ListSolSAP", E_OT);
            return D_OT.OTArticulo_ListSolSAP(E_OT);
        }

        public DataTable Item_ListSinUC()
        {
            return D_OT.Item_ListSinUC();
        }
        public int OTReprog_Count(E_OT E_OT)
        {
            OT_Debug("OTReprog_Count", E_OT);
            return D_OT.OTReprog_Count(E_OT);
        }

        public int OT_UpdatebyItem(DataTable tblConsumible, E_OT E_OT, DataTable tblFrecuencias)
        {
            return D_OT.OT_UpdatebyItem(tblConsumible, E_OT, tblFrecuencias);
        }

        public int PerfilCompActividad_Update(E_OT E_OT)
        {
            OT_Debug("PerfilCompActividad_Update", E_OT);
            return D_OT.PerfilCompActividad_Update(E_OT);
        }

        public static void OT_Debug(string Metodo, E_OT E_OT)
        {
            Utilitarios.Utilitarios obj = new Utilitarios.Utilitarios();
            DebugHandler Debug = new DebugHandler();
            string Parametros;

            Parametros = "IdOT = " + obj.NullableTrim(E_OT.IdOT.ToString());
            Parametros = Parametros + ", CodOT = " + obj.NullableTrim(E_OT.CodOT);
            Parametros = Parametros + ", NombreOT = " + obj.NullableTrim(E_OT.NombreOT);
            Parametros = Parametros + ", IdTipoOT = " + obj.NullableTrim(E_OT.IdTipoOT.ToString());
            Parametros = Parametros + ", FlagSinUC = " + obj.NullableTrim(E_OT.FlagSinUC.ToString());
            Parametros = Parametros + ", IdUC = " + obj.NullableTrim(E_OT.IdUC.ToString());
            Parametros = Parametros + ", FechaProg = " + obj.NullableTrim(E_OT.FechaProg.ToString());
            Parametros = Parametros + ", FechaReprog = " + obj.NullableTrim(E_OT.FechaReprog.ToString());
            Parametros = Parametros + ", FechaLiber = " + obj.NullableTrim(E_OT.FechaLiber.ToString());
            Parametros = Parametros + ", CodResponsable = " + obj.NullableTrim(E_OT.CodResponsable);
            Parametros = Parametros + ", NombreResponsable = " + obj.NullableTrim(E_OT.NombreResponsable);
            Parametros = Parametros + ", IdTipoGeneracion = " + obj.NullableTrim(E_OT.IdTipoGeneracion.ToString());
            Parametros = Parametros + ", IdEstadoOT = " + obj.NullableTrim(E_OT.IdEstadoOT.ToString());
            Parametros = Parametros + ", MotivoPostergacion = " + obj.NullableTrim(E_OT.MotivoPostergacion);
            Parametros = Parametros + ", Observacion = " + obj.NullableTrim(E_OT.Observacion);
            Parametros = Parametros + ", FlagActivo = " + obj.NullableTrim(E_OT.FlagActivo.ToString());
            Parametros = Parametros + ", IdUsuarioCreacion = " + obj.NullableTrim(E_OT.IdUsuarioCreacion.ToString());
            Parametros = Parametros + ", FechaCreacion = " + obj.NullableTrim(E_OT.FechaCreacion.ToString());
            Parametros = Parametros + ", HostCreacion = " + obj.NullableTrim(E_OT.HostCreacion);
            Parametros = Parametros + ", IdUsuarioModificacion = " + obj.NullableTrim(E_OT.IdUsuarioModificacion.ToString());
            Parametros = Parametros + ", FechaModificacion = " + obj.NullableTrim(E_OT.FechaModificacion.ToString());
            Parametros = Parametros + ", CodUC = " + obj.NullableTrim(E_OT.CodUC);
            Parametros = Parametros + ", IdsOTCompActividadEstado = " + obj.NullableTrim(E_OT.IdsOTCompActividadEstado);
            Debug.EscribirDebug(Metodo, Parametros);
        }
        
    }
}
