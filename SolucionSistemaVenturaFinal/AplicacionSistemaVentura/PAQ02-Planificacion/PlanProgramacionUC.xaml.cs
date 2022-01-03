using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Editors.Settings;
using System.Data;
using Utilitarios;
using Business;
using Entities;
using System.ComponentModel;
using System.Windows.Threading;
using System.Globalization;
namespace AplicacionSistemaVentura.PAQ02_Planificacion
{

    public partial class PlanProgramacionUC : UserControl
    {
        public PlanProgramacionUC()
        {
            InitializeComponent();
            UserControl_Loaded();
        }
        int gintUsuario = Utilitarios.Utilitarios.gintIdUsuario;
        int OTSReg = 0;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        string ErrorWorker = "";
        Utilitarios.ErrorHandler Error = new Utilitarios.ErrorHandler();
        E_Programacion objE_Programacion = new E_Programacion();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        E_PerfilTarea objE_PerfilTarea = new E_PerfilTarea();
        E_Perfil objE_Perfil = new E_Perfil();
        E_PerfilDetalle objE_PerfilDetalle = new E_PerfilDetalle();
        E_HI objE_HI = new E_HI();

        B_HI objB_HI = new B_HI();
        B_Perfil objB_Perfil = new B_Perfil();
        B_PerfilTarea objB_PerfilTarea = new B_PerfilTarea();
        B_PerfilDetalle objB_PerfilDetalle = new B_PerfilDetalle();
        B_Programacion objB_Programacion = new B_Programacion();
        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        B_OT objB_OT = new B_OT();
        E_OT objE_OT = new E_OT();
        E_TablaMaestra objTablaMestra = new E_TablaMaestra();

        B_TablaMaestra objTablaMaestra = new B_TablaMaestra();
        E_TablaMaestra objTM = new E_TablaMaestra();

        DataView dtv_maestra = new DataView();
        DataTable tblDatosOT = new DataTable();
        DataTable tblOTArticuloSol = new DataTable();
        DataView dtvMaestra = new DataView();
        DataTable tblVerStock = new DataTable();

        InterfazMTTO.iSBO_BE.BEOHEM OHEM = new InterfazMTTO.iSBO_BE.BEOHEM();
        InterfazMTTO.iSBO_BE.BEOHEMList OHEMlist = new InterfazMTTO.iSBO_BE.BEOHEMList();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        InterfazMTTO.iSBO_BE.BEUDUC UDUC = new InterfazMTTO.iSBO_BE.BEUDUC();
        InterfazMTTO.iSBO_BE.BEUDUCList tucuclist = new InterfazMTTO.iSBO_BE.BEUDUCList();
        InterfazMTTO.iSBO_BE.BEOITMList BEOITMList = new InterfazMTTO.iSBO_BE.BEOITMList();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        InterfazMTTO.iSBO_BE.BEOWTQ OWTQ = new InterfazMTTO.iSBO_BE.BEOWTQ();
        InterfazMTTO.iSBO_BE.BEWTQ1List WTQ1List = new InterfazMTTO.iSBO_BE.BEWTQ1List();
        InterfazMTTO.iSBO_BE.BEWTQ1 WTQ1 = new InterfazMTTO.iSBO_BE.BEWTQ1();
        InterfazMTTO.iSBO_BE.BEWTQ1List WTQ1_List = new InterfazMTTO.iSBO_BE.BEWTQ1List();
        InterfazMTTO.iSBO_BE.BEOIGE OIGE = new InterfazMTTO.iSBO_BE.BEOIGE();
        InterfazMTTO.iSBO_BE.BEIGE1 IGE1 = new InterfazMTTO.iSBO_BE.BEIGE1();
        InterfazMTTO.iSBO_BE.BEOITMList BEOITMListCon = new InterfazMTTO.iSBO_BE.BEOITMList();
        DataTable tblOTComp = new DataTable();
        DataTable tblActividades = new DataTable();
        DataTable tblProgramacionDatos = new DataTable();
        DataTable tblProgramacionDet = new DataTable();
        DataTable tblTareas = new DataTable();
        DataTable tblHerrEsp = new DataTable();
        DataTable tblRepuesto = new DataTable();
        DataTable tblConsumible = new DataTable();
        DataTable tblRegistrados = new DataTable();
        DataTable tblBitacora = new DataTable();
        DataSet dsOTComp = new DataSet();
        DataSet dsActividades = new DataSet();
        DataSet dsTareas = new DataSet();
        DataSet dsHerrEsp = new DataSet();
        DataSet dsRepuesto = new DataSet();
        DataSet dsConsumible = new DataSet();

        Boolean gbolAllActividades = true;

        string gstrFiltroTipoMmto = "";
        string gstrFiltroActPend = "";
        string gstrSortActPrio = "";
        string gstrFiltroTodos = "";

        string gstrEtiquetaProgramacionUC = "PlanProgramacionUC";


        DispatcherTimer TimerProgramacion = new DispatcherTimer();
        int gintTimer = 0;

        void OnFocus(object sender, RoutedEventArgs e)
        {
            (sender as Control).Background = System.Windows.Media.Brushes.LightYellow;
        }

        private void OutFocus(object sender, RoutedEventArgs e)
        {
            (sender as Control).Background = System.Windows.Media.Brushes.White;
        }

        public class ClsTipoOT
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }
        public class ClsResponsable
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        private IList<ClsTipoOT> ComboPerfilComp()
        {
            List<ClsTipoOT> ListCombo = new List<ClsTipoOT>();
            DataTable tbl = new DataTable();
            objE_TablaMaestra.IdTabla = 0;
            tbl = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
            DataView dtvMaestro = tbl.DefaultView;
            Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=20", dtvMaestro);
            for (int j = 0; j < dtvMaestro.Count; j++)
            {
                ListCombo.Add(new ClsTipoOT()
                {
                    Id = Convert.ToInt32(dtvMaestro[j]["Valor"]),
                    Text = dtvMaestro[j]["Descripcion"].ToString()
                });
            }
            return ListCombo;
        }

        private IList<ClsResponsable> ComboResponsable()
        {
            //Lista Responsable
            List<ClsResponsable> ListCombo = new List<ClsResponsable>();
            RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
            OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("R", ref RPTA);
            if (RPTA.ResultadoRetorno != 0)
            {
                GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
            }
            else
            {
                for (int j = 0; j < OHEMlist.Count; j++)
                {
                    ListCombo.Add(new ClsResponsable()
                    {
                        Id = OHEMlist[j].CodigoPersona,
                        Text = OHEMlist[j].NombrePersona
                    });
                }
            }
            return ListCombo;
        }

        private void UserControl_Loaded()
        {
            try
            {
                objTM.IdTabla = 0;
                dtvMaestra = B_TablaMaestra.TablaMaestra_Combo(objTM).DefaultView;

                dtpFechaProgram.EditValue = DateTime.Now;
                BtnAceptar.Visibility = Visibility.Hidden;
                BtnCancelar.Visibility = Visibility.Hidden;
                BtnVerOmitidos.Visibility = Visibility.Hidden;
                //BtnVerStock.Visibility = Visibility.Hidden;

                //Tabla Programación Detalles
                tblProgramacionDet.Columns.Add("IdProgramacion", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("LineNum", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("ProgramacionKey", Type.GetType("System.String"));
                tblProgramacionDet.Columns.Add("IdTipoProgramacion", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("IdTipoGeneracion", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("IdOT", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("IdUC", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("IdPM", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("CodPM", Type.GetType("System.String"));
                tblProgramacionDet.Columns.Add("PMDesc", Type.GetType("System.String"));
                tblProgramacionDet.Columns.Add("IdPMComp", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("IdPerfil", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("PerfilComp", Type.GetType("System.String"));
                tblProgramacionDet.Columns.Add("IdActividad", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("Actividad", Type.GetType("System.String"));
                tblProgramacionDet.Columns.Add("IdCiclo", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("Porc01", Type.GetType("System.Double"));
                tblProgramacionDet.Columns.Add("Porc02", Type.GetType("System.Double"));
                tblProgramacionDet.Columns.Add("IdTipoOTDefecto", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("Prioridad", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("FrecuenciaPlan", Type.GetType("System.Double"));
                tblProgramacionDet.Columns.Add("Contador", Type.GetType("System.Double"));
                tblProgramacionDet.Columns.Add("Calculo", Type.GetType("System.Double"));
                tblProgramacionDet.Columns.Add("Avance", Type.GetType("System.Double"));
                tblProgramacionDet.Columns.Add("PorcAvance", Type.GetType("System.Double"));
                tblProgramacionDet.Columns.Add("PorcRestante", Type.GetType("System.Double"));
                tblProgramacionDet.Columns.Add("ValorSemaforoAmarillo", Type.GetType("System.Double"));
                tblProgramacionDet.Columns.Add("ValorSemaforoNaranja", Type.GetType("System.Double"));
                tblProgramacionDet.Columns.Add("Semaforo", Type.GetType("System.String"));
                tblProgramacionDet.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblProgramacionDet.Columns.Add("IdProgramacionDet_Padre", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("IdProgramacion_Padre", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("FlagRealizado", Type.GetType("System.Boolean"));
                tblProgramacionDet.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                tblProgramacionDet.Columns.Add("TipoOT", Type.GetType("System.Int32"));
                tblProgramacionDet.Columns.Add("FechaProgramacion", Type.GetType("System.String"));

                //Tabla Bitacora
                tblBitacora.Columns.Add("LineNum", Type.GetType("System.Int32"));
                tblBitacora.Columns.Add("IdOT", Type.GetType("System.Int32"));
                tblBitacora.Columns.Add("IdEstado", Type.GetType("System.Int32"));
                tblBitacora.Columns.Add("Error", Type.GetType("System.String"));
                //Tabla OTComp
                tblOTComp.Columns.Add("IdOTComp", Type.GetType("System.Int32"));
                tblOTComp.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
                tblOTComp.Columns.Add("IdOT", Type.GetType("System.Int32"));
                tblOTComp.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblOTComp.Columns.Add("IdPerfilCompPadre", Type.GetType("System.Int32"));
                tblOTComp.Columns.Add("PerfilComp", Type.GetType("System.String"));
                tblOTComp.Columns.Add("IdTipoDetalle", Type.GetType("System.Int32"));
                tblOTComp.Columns.Add("IdItem", Type.GetType("System.Int32"));
                tblOTComp.Columns.Add("NroSerie", Type.GetType("System.String"));
                tblOTComp.Columns.Add("CodigoSAP", Type.GetType("System.String"));
                tblOTComp.Columns.Add("DescripcionSAP", Type.GetType("System.String"));
                tblOTComp.Columns.Add("IdEstadoOTComp", Type.GetType("System.Int32"));
                tblOTComp.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblOTComp.Columns.Add("IsChecked", Type.GetType("System.Boolean"));
                tblOTComp.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                //Tabla Actividades
                tblActividades.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblActividades.Columns.Add("IdOTComp", Type.GetType("System.Int32"));
                tblActividades.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblActividades.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblActividades.Columns.Add("IdActividad", Type.GetType("System.Int32"));
                tblActividades.Columns.Add("Actividad", Type.GetType("System.String"));
                tblActividades.Columns.Add("IsChecked", Type.GetType("System.Boolean"));
                tblActividades.Columns.Add("FlagUso", Type.GetType("System.Boolean"));
                tblActividades.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblActividades.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                //Tabla Tareas
                tblTareas.Columns.Add("IdOTTarea", Type.GetType("System.Int32"));
                tblTareas.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblTareas.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblTareas.Columns.Add("IdTarea", Type.GetType("System.Int32"));
                tblTareas.Columns.Add("OTTarea", Type.GetType("System.String"));
                tblTareas.Columns.Add("IdPerfilTarea", Type.GetType("System.Int32"));
                tblTareas.Columns.Add("CodResponsable", Type.GetType("System.String"));
                tblTareas.Columns.Add("CostoHoraHombre", Type.GetType("System.Double"));
                tblTareas.Columns.Add("HorasEstimada", Type.GetType("System.Double"));
                tblTareas.Columns.Add("HorasReal", Type.GetType("System.Double"));
                tblTareas.Columns.Add("IdEstadoOTT", Type.GetType("System.Int32"));
                tblTareas.Columns.Add("FlagAutomatico", Type.GetType("System.Boolean"));
                tblTareas.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblTareas.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                //Tabla Detalle Herramienta Especial
                tblHerrEsp.Columns.Add("IdOTHerramienta", Type.GetType("System.Int32"));
                tblHerrEsp.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblHerrEsp.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblHerrEsp.Columns.Add("IdHerramienta", Type.GetType("System.Int32"));
                tblHerrEsp.Columns.Add("Herramienta", Type.GetType("System.String"));
                tblHerrEsp.Columns.Add("Cantidad", Type.GetType("System.Int32"));
                tblHerrEsp.Columns.Add("IdEstado", Type.GetType("System.Int32"));
                tblHerrEsp.Columns.Add("NroDevolucion", Type.GetType("System.String"));
                tblHerrEsp.Columns.Add("FlagAutomatico", Type.GetType("System.Boolean"));
                tblHerrEsp.Columns.Add("FlagActivo", Type.GetType("System.Int32"));
                tblHerrEsp.Columns.Add("Nuevo", Type.GetType("System.Int32"));
                //Tabla Detalle Repuesto
                tblRepuesto.Columns.Add("IdOTArticulo", Type.GetType("System.Int32"));
                tblRepuesto.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblRepuesto.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblRepuesto.Columns.Add("IdTipoArticulo", Type.GetType("System.Int32"));
                tblRepuesto.Columns.Add("IdArticulo", Type.GetType("System.String"));
                tblRepuesto.Columns.Add("Articulo", Type.GetType("System.String"));
                tblRepuesto.Columns.Add("CantSol", Type.GetType("System.Int32"));
                tblRepuesto.Columns.Add("CantEnv", Type.GetType("System.Int32"));
                tblRepuesto.Columns.Add("CantUti", Type.GetType("System.Int32"));
                tblRepuesto.Columns.Add("CostoArticulo", Type.GetType("System.Double"));
                tblRepuesto.Columns.Add("Observacion", Type.GetType("System.String"));
                tblRepuesto.Columns.Add("CodResponsable", Type.GetType("System.String"));
                tblRepuesto.Columns.Add("FlagAutomatico", Type.GetType("System.Boolean"));
                tblRepuesto.Columns.Add("NroSerie", Type.GetType("System.String"));
                tblRepuesto.Columns.Add("Frecuencia", Type.GetType("System.Double"));
                tblRepuesto.Columns.Add("FlagActivo", Type.GetType("System.Int32"));
                tblRepuesto.Columns.Add("Nuevo", Type.GetType("System.Int32"));
                //Tabla Detalle Consumible
                tblConsumible.Columns.Add("IdOTArticulo", Type.GetType("System.Int32"));
                tblConsumible.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblConsumible.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblConsumible.Columns.Add("IdTipoArticulo", Type.GetType("System.Int32"));
                tblConsumible.Columns.Add("IdArticulo", Type.GetType("System.String"));
                tblConsumible.Columns.Add("Articulo", Type.GetType("System.String"));
                tblConsumible.Columns.Add("CantSol", Type.GetType("System.Int32"));
                tblConsumible.Columns.Add("CantEnv", Type.GetType("System.Int32"));
                tblConsumible.Columns.Add("CantUti", Type.GetType("System.Int32"));
                tblConsumible.Columns.Add("CostoArticulo", Type.GetType("System.Double"));
                tblConsumible.Columns.Add("Observacion", Type.GetType("System.String"));
                tblConsumible.Columns.Add("CodResponsable", Type.GetType("System.String"));
                tblConsumible.Columns.Add("FlagAutomatico", Type.GetType("System.Boolean"));
                tblConsumible.Columns.Add("NroSerie", Type.GetType("System.String"));
                tblConsumible.Columns.Add("Frecuencia", Type.GetType("System.Double"));
                tblConsumible.Columns.Add("FlagActivo", Type.GetType("System.Int32"));
                tblConsumible.Columns.Add("Nuevo", Type.GetType("System.Int32"));

                //Datos de Cada OT
                tblDatosOT.Columns.Add("IdOT", Type.GetType("System.Int32"));
                tblDatosOT.Columns.Add("NombreOT", Type.GetType("System.String"));
                tblDatosOT.Columns.Add("IdTipoOT", Type.GetType("System.Int32"));
                tblDatosOT.Columns.Add("FlagSinUC", Type.GetType("System.Int32"));
                tblDatosOT.Columns.Add("IdUC", Type.GetType("System.Int32"));
                tblDatosOT.Columns.Add("IdHI", Type.GetType("System.Int32"));
                tblDatosOT.Columns.Add("CodUC", Type.GetType("System.String"));
                tblDatosOT.Columns.Add("FechaProg", Type.GetType("System.DateTime"));
                tblDatosOT.Columns.Add("CodResponsable", Type.GetType("System.String"));
                tblDatosOT.Columns.Add("NombreResponsable", Type.GetType("System.String"));
                tblDatosOT.Columns.Add("IdTipoGeneracion", Type.GetType("System.Int32"));
                tblDatosOT.Columns.Add("IdEstadoOT", Type.GetType("System.Int32"));
                tblDatosOT.Columns.Add("MotivoPostergacion", Type.GetType("System.String"));
                tblDatosOT.Columns.Add("Observacion", Type.GetType("System.String"));
                tblDatosOT.Columns.Add("FlagActivo", Type.GetType("System.Int32"));
                tblDatosOT.Columns.Add("IdUsuario", Type.GetType("System.Int32"));

                //Tabla Articulo Solicitud
                tblOTArticuloSol.Columns.Add("IdOTArticulo", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroSolTransfer", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroLinSolTransfer", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroSalMercancia", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroLinSalMercancia", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroSolDevolucion", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroLinSolDevolucionr", Type.GetType("System.Int32"));

                tblRegistrados.Columns.Add("CodigoOT", Type.GetType("System.String"));
                tblRegistrados.Columns.Add("UC", Type.GetType("System.String"));
                tblRegistrados.Columns.Add("FechaProgramacion", Type.GetType("System.DateTime"));
                tblRegistrados.Columns.Add("Responsable", Type.GetType("System.String"));
                tblRegistrados.Columns.Add("msg", Type.GetType("System.String"));
                tblRegistrados.Columns.Add("msgSAP", Type.GetType("System.String"));

                dtv_maestra = B_TablaMaestra.TablaMaestra_Combo(objTablaMestra).DefaultView;

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                BEOITMListCon = new InterfazMTTO.iSBO_BE.BEOITMList();
                BEOITMListCon = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("O", ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }

                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.ProgressChanged += worker_ProgressChanged;

                TimerProgramacion.Tick += new EventHandler(TimerProgramacion_Tick);
                TimerProgramacion.Interval = new TimeSpan(0, 0, 1);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        
        private void TimerProgramacion_Tick(object sender, EventArgs e)
        {
            gintTimer += 1;
            if (gintTimer >= 2)
            {
                stkGrabarOTTerminado.Visibility = Visibility.Visible;
                stkPanelGrabarOT.Visibility = Visibility.Collapsed;
                gintTimer = 0;
                TimerProgramacion.Stop();
            }
        }

        private void ProgramacionList()
        {
            try
            {
                tblProgramacionDatos = new DataTable();

                if ((bool)RdnPreventivo.IsChecked)
                {
                    tblProgramacionDatos = objB_Programacion.Bitacora_List();
                    if (gstrFiltroActPend.Trim() != "" || gstrFiltroTodos.Trim() != "") { gstrFiltroTipoMmto += "AND "; }
                    if (gstrFiltroActPend.Trim() != "" && gstrFiltroTodos.Trim() != "") { gstrFiltroActPend += "AND "; }
                    string Filtro = gstrFiltroTipoMmto + gstrFiltroActPend + gstrFiltroTodos;
                    string Sort = gstrSortActPrio;
                    DataView dtvProgramacionDatos = new DataView(tblProgramacionDatos);
                    dtvProgramacionDatos.RowFilter = Filtro;
                    dtvProgramacionDatos.Sort = Sort;
                    tblProgramacionDatos = dtvProgramacionDatos.ToTable();
                }
                else if ((bool)RdnCorrectivo.IsChecked)
                {
                    tblProgramacionDatos = objB_Programacion.ObtenerDatosHI();
                }

                DataColumn dcIsChecked = new DataColumn() { ColumnName = "IsChecked", DefaultValue = false };
                DataColumn dcFechaProgramacion = new DataColumn() { ColumnName = "FechaProgramacion", DefaultValue = Convert.ToDateTime(dtpFechaProgram.EditValue) };
                DataColumn dcFlagRegistrado = new DataColumn() { ColumnName = "FlagRegistrado", DefaultValue = true };
                tblProgramacionDatos.Columns.Add(dcIsChecked);
                tblProgramacionDatos.Columns.Add(dcFechaProgramacion);
                tblProgramacionDatos.Columns.Add(dcFlagRegistrado);

                if ((bool)RdnPreventivo.IsChecked)
                {
                    dtgProgramacionPreventivo.ItemsSource = tblProgramacionDatos;
                }
                else if ((bool)RdnCorrectivo.IsChecked)
                {
                    DataColumn dcIdTipoOT = new DataColumn() { ColumnName = "IdTipoOT", DefaultValue = 0, DataType = System.Type.GetType("System.Int32") };
                    tblProgramacionDatos.Columns.Add(dcIdTipoOT);
                    dtgProgramacionCorrectivo.ItemsSource = tblProgramacionDatos;
                }

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCons_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gbolAllActividades = true;
                if (ValidaConsulta() == true) { return; }
                objE_Programacion.FechaProgramacion = Convert.ToDateTime(dtpFechaProgram.EditValue);

                if (RdnCorrectivo.IsChecked == true) { gstrFiltroTipoMmto = "IdTipoGeneracion = 2 "; objE_Programacion.TipoMantenimiento = 2; }
                else if (RdnPreventivo.IsChecked == true) { gstrFiltroTipoMmto = "IdTipoGeneracion = 1 "; objE_Programacion.TipoMantenimiento = 1; }

                objE_Programacion.FechaProgramacion = Convert.ToDateTime(dtpFechaProgram.EditValue);

                if (ChkAñadirActividades.IsChecked == true) { gstrFiltroActPend = ""; }
                else { gstrFiltroActPend = "IdOTCompActividadEstado = 0 "; }

                if (ChkActivarPriorizacion.IsChecked == true) { gstrSortActPrio = "Prioridad desc "; }
                else { gstrSortActPrio = ""; }

                if (ChkMostrarTodos.IsChecked == true) { gstrFiltroTodos = ""; }
                else { gstrFiltroTodos = "Semaforo <> 'Verde' "; }

                ProgramacionList();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private bool ValidaConsulta()
        {
            bool bolRpta = false;
            try
            {
                if (RdnCorrectivo.IsChecked == false && RdnPreventivo.IsChecked == false)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "OBLI_TIPO_MANT"), 2);
                }
                else if (Convert.ToDateTime(dtpFechaProgram.EditValue) < Convert.ToDateTime(Utilitarios.Utilitarios.Fecha_Hora_Servidor().Rows[0]["FechaServer"]))
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "LOGI_FECH_PROG"), 2);
                    dtpFechaProgram.Focus();
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            return bolRpta;
        }

        //private void TxTComentarios_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    try
        //    {
        //        TxTComentarios.Text = Utilitarios.Utilitarios.SoloAlfanumerico(TxTComentarios.Text);
        //        TxTComentarios.SelectionStart = TxTComentarios.Text.Length;
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobalClass.ip.Mensaje(ex.Message, 3);
        //        Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
        //    }
        //}

        private void btnGenOT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgProgramacionPreventivo.VisibleRowCount == 0 && dtgProgramacionCorrectivo.VisibleRowCount == 0) { return; }

                dsOTComp = new DataSet();
                dsActividades = new DataSet();
                dsTareas = new DataSet();
                dsHerrEsp = new DataSet();
                dsRepuesto = new DataSet();
                dsConsumible = new DataSet();
                tblProgramacionDet.Rows.Clear();
                tblDatosOT.Rows.Clear();

                if ((bool)RdnPreventivo.IsChecked)
                {
                    string LineNum = "";
                    LlenarStock(out LineNum);
                    if (tblVerStock.Rows.Count != 0)
                    {
                        int cant = 0;
                        foreach (DataRow drSotck in tblVerStock.Select("Stock = 0"))
                        {
                            cant = 1;
                            break;
                        }
                        if (cant == 1)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "ALER_STOC_OT"), 2);
                        }
                    }
                    PreparaciondeOTPreventivo();
                }
                else if ((bool)RdnCorrectivo.IsChecked)
                {
                    PreparaciondeOTCorrectivo();
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void PreparaciondeOTCorrectivo()
        {
            int CodigoPersonaDefecto = 0;
            DataTable tblConfirmacion = new DataTable();
            tblConfirmacion.Columns.Add("Hijo");
            tblConfirmacion.Columns.Add("Padre");
            tblConfirmacion.Columns.Add("descripcion");
            tblConfirmacion.Columns.Add("DetailSource", Type.GetType("System.Object"));
            tblConfirmacion.Columns.Add("Visibility");
            tblConfirmacion.Columns.Add("CodigoPersona", Type.GetType("System.Int32"));
            tblConfirmacion.Columns.Add("IdTipoOT");
            tblConfirmacion.Columns.Add("FechaProgramacion");
            tblConfirmacion.Columns.Add("IdUC");

            DataTable tblCorrectivo = (DataTable)dtgProgramacionCorrectivo.ItemsSource;
            int idHijo = 1;
            int IdTabla = 1;

            if (tblCorrectivo.Select("IsChecked = True").Length == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "OBLI_CANT_OT"), 2);
                return;
            }

            if (tblCorrectivo.Select("IsChecked = True AND IdTipoOT <= 0").Length != 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "OBLI_TIPO_OT"), 2);
                return;
            }

            foreach (DataRow drHI in tblCorrectivo.Select("IsChecked = True"))
            {
                objE_HI = new E_HI();
                objE_HI.IdHI = Convert.ToInt32(drHI["IdHI"]);

                //Obtiene Componentes de la Hoja de Inspección
                foreach (DataRow drHIComp in objB_HI.HIComp_List(objE_HI).Rows)
                {
                    DataRow drco = tblOTComp.NewRow();
                    drco["IdOTComp"] = 0;
                    drco["IdUCComp"] = Convert.ToInt32(drHIComp["IdUCComp"]);
                    drco["IdOT"] = 0;
                    drco["IdPerfilComp"] = Convert.ToInt32(drHIComp["IdPerfilComp"]);
                    drco["IdPerfilCompPadre"] = 0;
                    drco["PerfilComp"] = drHIComp["PerfilComp"].ToString();
                    drco["IdTipoDetalle"] = 0;
                    drco["NroSerie"] = drHIComp["NroSerie"].ToString();
                    drco["CodigoSAP"] = drHIComp["CodigoSAP"].ToString();
                    drco["DescripcionSAP"] = drHIComp["DescripcionSAP"].ToString();
                    drco["IdEstadoOTComp"] = 1;
                    drco["FlagActivo"] = true;
                    drco["IsChecked"] = false;
                    drco["Nuevo"] = true;
                    tblOTComp.Rows.Add(drco);
                }

                //Obtiene Actividades de la Hoja de Inspección
                int IdPerfilCompActividad = 1;
                foreach (DataRow drHIComp_Acti in objB_HI.HIComp_Actividad_List(objE_HI).Rows)
                {
                    DataRow drac = tblActividades.NewRow();
                    drac["IdOTCompActividad"] = 0;
                    drac["IdOTComp"] = 0;
                    drac["IdPerfilComp"] = Convert.ToInt32(drHIComp_Acti["IdPerfilComp"]);
                    drac["IdPerfilCompActividad"] = IdPerfilCompActividad;
                    drac["IdActividad"] = Convert.ToInt32(drHIComp_Acti["IdActividad"]);
                    drac["Actividad"] = drHIComp_Acti["Actividad"].ToString();
                    drac["IsChecked"] = !(Convert.ToInt32(drHI["IdTipoOT"]) == 1);
                    drac["FlagUso"] = Convert.ToBoolean(drHIComp_Acti["FlagUso"]);
                    drac["FlagActivo"] = true;
                    drac["Nuevo"] = true;
                    tblActividades.Rows.Add(drac);

                    //Obtiene Tareas de la Hoja de Inspección
                    foreach (DataRow drHITarea in objB_HI.HITarea_List(objE_HI).Select("IdHICompActividad = " + Convert.ToInt32(drHIComp_Acti["IdHICompActividad"])))
                    {
                        DataRow drTa = tblTareas.NewRow();
                        drTa["IdOTTarea"] = 0;
                        drTa["IdOTCompActividad"] = 0;
                        drTa["IdPerfilCompActividad"] = IdPerfilCompActividad;
                        drTa["IdTarea"] = Convert.ToInt32(drHITarea["IdTarea"]);
                        drTa["OTTarea"] = drHITarea["Tarea"].ToString();
                        drTa["IdPerfilTarea"] = 0;
                        drTa["CodResponsable"] = 0;
                        drTa["CostoHoraHombre"] = 0;
                        drTa["HorasEstimada"] = Convert.ToDouble(drHITarea["HorasHombre"]);
                        drTa["HorasReal"] = 0;
                        drTa["IdEstadoOTT"] = 1;
                        drTa["FlagAutomatico"] = true;
                        drTa["FlagActivo"] = true;
                        drTa["Nuevo"] = true;
                        tblTareas.Rows.Add(drTa);
                    }

                    //Obtiene Detalles de la Hoja de Inspección
                    foreach (DataRow drDetalles in objB_HI.HIDetalle_List(objE_HI).Select("IdHICompActividad = " + Convert.ToInt32(drHIComp_Acti["IdHICompActividad"])))
                    {
                        if (drDetalles["IdTipoArticulo"].ToString() == "1")
                        {
                            DataRow drHer = tblHerrEsp.NewRow();
                            drHer["IdOTHerramienta"] = 0;
                            drHer["IdOTCompActividad"] = 0;
                            drHer["IdPerfilCompActividad"] = IdPerfilCompActividad;
                            drHer["IdHerramienta"] = Convert.ToInt32(drDetalles["IdArticulo"]);
                            drHer["Herramienta"] = drDetalles["Articulo"].ToString();
                            drHer["Cantidad"] = Convert.ToDouble(drDetalles["Cantidad"]);
                            drHer["IdEstado"] = 1;
                            drHer["NroDevolucion"] = 0;
                            drHer["FlagAutomatico"] = true;
                            drHer["FlagActivo"] = true;
                            drHer["Nuevo"] = true;
                            tblHerrEsp.Rows.Add(drHer);
                        }
                        else if (drDetalles["IdTipoArticulo"].ToString() == "2")
                        {
                            DataRow drRe = tblRepuesto.NewRow();
                            drRe["IdOTArticulo"] = 0;
                            drRe["IdOTCompActividad"] = 0;
                            drRe["IdPerfilCompActividad"] = IdPerfilCompActividad;
                            drRe["IdTipoArticulo"] = Convert.ToInt32(drDetalles["IdTipoArticulo"]);
                            drRe["IdArticulo"] = drDetalles["IdArticulo"].ToString();
                            foreach (var drArt in BEOITMListCon.Where(emp => emp.CodigoArticulo == drDetalles["IdArticulo"].ToString()))
                            {
                                drRe["Articulo"] = drArt.DescripcionArticulo;
                                break;
                            }
                            drRe["CantSol"] = Convert.ToDouble(drDetalles["Cantidad"]);
                            drRe["CantEnv"] = 0;
                            drRe["CantUti"] = 0;
                            drRe["CostoArticulo"] = 0;
                            drRe["Observacion"] = "";
                            drRe["CodResponsable"] = "";
                            drRe["FlagAutomatico"] = true;
                            drRe["NroSerie"] = "";
                            drRe["Frecuencia"] = 0.00;
                            drRe["FlagActivo"] = true;
                            drRe["Nuevo"] = true;
                            tblRepuesto.Rows.Add(drRe);
                        }
                        else if (drDetalles["IdTipoArticulo"].ToString() == "3")
                        {
                            DataRow drCons = tblConsumible.NewRow();
                            drCons["IdOTArticulo"] = 0;
                            drCons["IdOTCompActividad"] = 0;
                            drCons["IdPerfilCompActividad"] = IdPerfilCompActividad;
                            drCons["IdTipoArticulo"] = Convert.ToInt32(drDetalles["IdTipoArticulo"]);
                            drCons["IdArticulo"] = drDetalles["IdArticulo"].ToString();
                            foreach (var drArt in BEOITMListCon.Where(emp => emp.CodigoArticulo == drDetalles["IdArticulo"].ToString()))
                            {
                                drCons["Articulo"] = drArt.DescripcionArticulo;
                                break;
                            }
                            drCons["CantSol"] = Convert.ToDouble(drDetalles["Cantidad"]);
                            drCons["CantEnv"] = 0;
                            drCons["CantUti"] = 0;
                            drCons["CostoArticulo"] = 0;
                            drCons["Observacion"] = "";
                            drCons["CodResponsable"] = "";
                            drCons["FlagAutomatico"] = true;
                            drCons["NroSerie"] = "";
                            drCons["Frecuencia"] = 0.00;
                            drCons["FlagActivo"] = true;
                            drCons["Nuevo"] = true;
                            tblConsumible.Rows.Add(drCons);
                        }
                    }
                    IdPerfilCompActividad++;
                }

                DataRow drOT = tblDatosOT.NewRow();
                drOT["IdOT"] = 0;
                drOT["NombreOT"] = "";
                drOT["IdTipoOT"] = Convert.ToInt32(drHI["IdTipoOT"]);
                drOT["FlagSinUC"] = 0;
                drOT["IdUC"] = Convert.ToInt32(drHI["IdUC"]);
                drOT["IdHI"] = Convert.ToInt32(drHI["IdHI"]);
                drOT["CodUC"] = drHI["CodUC"].ToString();
                drOT["FechaProg"] = Convert.ToDateTime(drHI["FechaProgramacion"]);
                drOT["CodResponsable"] = "0";
                drOT["NombreResponsable"] = "";
                drOT["IdTipoGeneracion"] = objE_Programacion.TipoMantenimiento;
                drOT["IdEstadoOT"] = 1;
                drOT["MotivoPostergacion"] = "";
                drOT["Observacion"] = Convert.ToString(TxTComentarios.Text);
                drOT["FlagActivo"] = 1;
                drOT["IdUsuario"] = gintUsuario;
                tblDatosOT.Rows.Add(drOT);


                tblOTComp.TableName = "Tabla" + IdTabla;
                tblActividades.TableName = "Tabla" + IdTabla;
                tblTareas.TableName = "Tabla" + IdTabla;
                tblHerrEsp.TableName = "Tabla" + IdTabla;
                tblRepuesto.TableName = "Tabla" + IdTabla;
                tblConsumible.TableName = "Tabla" + IdTabla;

                dsOTComp.Tables.Add(tblOTComp.Copy());
                dsActividades.Tables.Add(tblActividades.Copy());
                dsTareas.Tables.Add(tblTareas.Copy());
                dsHerrEsp.Tables.Add(tblHerrEsp.Copy());
                dsRepuesto.Tables.Add(tblRepuesto.Copy());
                dsConsumible.Tables.Add(tblConsumible.Copy());

                tblOTComp.Rows.Clear();
                tblActividades.Rows.Clear();
                tblTareas.Rows.Clear();
                tblHerrEsp.Rows.Clear();
                tblRepuesto.Rows.Clear();
                tblConsumible.Rows.Clear();



                DataRow drConfir;
                drConfir = tblConfirmacion.NewRow();
                drConfir["Hijo"] = idHijo;
                drConfir["Padre"] = "0";
                drConfir["descripcion"] = "Código Hoja de Inspección:" + drHI["CodHI"].ToString();
                drConfir["Visibility"] = "Visible";
                drConfir["DetailSource"] = ComboResponsable();
                if (OHEMlist.Count != 0) { CodigoPersonaDefecto = OHEMlist[0].CodigoPersona; }
                drConfir["CodigoPersona"] = CodigoPersonaDefecto;
                drConfir["IdTipoOT"] = Convert.ToInt32(drHI["IdTipoOT"]);
                drConfir["FechaProgramacion"] = Convert.ToDateTime(drHI["FechaProgramacion"]);
                drConfir["IdUC"] = Convert.ToInt32(drHI["IdUC"]);
                tblConfirmacion.Rows.Add(drConfir);
                idHijo++;

                IdTabla++;
            }

            /*
                tblDatosOT
                dsOTComp.Tables[i],
                dsActividades.Tables[i],
                dsTareas.Tables[i],
                dsHerrEsp.Tables[i],
                dsRepuesto.Tables[i],
                dsConsumible.Tables[i]
            */

            trvConfirmarRegistrosOT.ItemsSource = tblConfirmacion;
            treeListView2.ExpandAllNodes();
            stkPanelGrabarOT.Visibility = Visibility.Visible;
        }
        private void PreparaciondeOTPreventivo()
        {
            int CodigoPersonaDefecto = 0;

            DataTable tblConfirmacion = new DataTable();
            tblConfirmacion.Columns.Add("Hijo");
            tblConfirmacion.Columns.Add("Padre");
            tblConfirmacion.Columns.Add("descripcion");
            tblConfirmacion.Columns.Add("DetailSource", Type.GetType("System.Object"));
            tblConfirmacion.Columns.Add("Visibility");
            tblConfirmacion.Columns.Add("CodigoPersona", Type.GetType("System.Int32"));
            tblConfirmacion.Columns.Add("IdTipoOT");
            tblConfirmacion.Columns.Add("FechaProgramacion");
            tblConfirmacion.Columns.Add("IdUC");

            DataTable tblProgramacionChekeds = (DataTable)dtgProgramacionPreventivo.ItemsSource;
            foreach (DataRow drProgr in tblProgramacionChekeds.Select("FlagRegistrado = false"))
            {
                drProgr["FlagRegistrado"] = true;
            }
            DataView dtvProgramacion = new DataView(tblProgramacionChekeds);
            dtvProgramacion.RowFilter = "IsChecked = true AND FlagRegistrado = true";

            if (dtvProgramacion.Count == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "OBLI_CANT_OT"), 2);
                return;
            }

            foreach (DataRow dr2 in tblProgramacionChekeds.Select("IsChecked = true"))
            {
                if (Convert.ToInt32(dr2["IdTipoOT"]) < 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "OBLI_TIPO_OT"), 2);
                    return;
                }

                DataRow dr = tblProgramacionDet.NewRow();
                dr["IdProgramacion"] = 0;
                dr["LineNum"] = Convert.ToInt32(dr2["LineNum"]);
                dr["ProgramacionKey"] = dr2["ProgramacionKey"].ToString();
                dr["IdTipoProgramacion"] = Convert.ToInt32(dr2["IdTipoProgramacion"]);
                dr["IdTipoGeneracion"] = Convert.ToInt32(dr2["IdTipoGeneracion"]);

                dr["IdOT"] = 0;

                dr["IdUC"] = Convert.ToInt32(dr2["IdUC"]);
                dr["IdUCComp"] = Convert.ToInt32(dr2["IdUCComp"]);
                dr["IdPM"] = Convert.ToInt32(dr2["IdPM"]);
                dr["CodPM"] = dr2["CodPM"].ToString();
                dr["PMDesc"] = dr2["PMDesc"].ToString();
                dr["IdPMComp"] = Convert.ToInt32(dr2["IdPMComp"]);
                dr["IdPerfil"] = Convert.ToInt32(dr2["IdPerfil"]);
                dr["IdPerfilComp"] = Convert.ToInt32(dr2["IdPerfilComp"]);
                dr["PerfilComp"] = dr2["PerfilComp"].ToString();
                dr["IdActividad"] = Convert.ToInt32(dr2["IdActividad"]);
                dr["Actividad"] = dr2["Actividad"].ToString();
                dr["IdCiclo"] = Convert.ToInt32(dr2["IdCiclo"]);
                dr["Porc01"] = Convert.ToDouble(dr2["Porc01"]);
                dr["Porc02"] = Convert.ToDouble(dr2["Porc02"]);
                dr["IdTipoOTDefecto"] = Convert.ToInt32(dr2["IdTipoOT"]);
                dr["Prioridad"] = Convert.ToInt32(dr2["Prioridad"]);
                dr["FrecuenciaPlan"] = Convert.ToDouble(dr2["FrecuenciaPlan"]);
                dr["Contador"] = Convert.ToDouble(dr2["Contador"]);
                dr["Calculo"] = Convert.ToDouble(dr2["Calculo"]);
                dr["Avance"] = Convert.ToDouble(dr2["Avance"]);
                dr["PorcAvance"] = Convert.ToDouble(dr2["PorcAvance"]);
                dr["PorcRestante"] = Convert.ToDouble(dr2["PorcRestante"]);
                dr["ValorSemaforoAmarillo"] = Convert.ToDouble(dr2["ValorSemaforoAmarillo"]);
                dr["ValorSemaforoNaranja"] = Convert.ToDouble(dr2["ValorSemaforoNaranja"]);
                dr["Semaforo"] = dr2["Semaforo"].ToString();
                dr["FlagActivo"] = true;
                dr["IdProgramacionDet_Padre"] = 0;
                dr["IdProgramacion_Padre"] = 0;
                dr["FlagRealizado"] = false;
                dr["Nuevo"] = true;
                dr["TipoOT"] = Convert.ToInt32(dr2["IdTipoOT"]);
                dr["FechaProgramacion"] = DateTime.Parse(dr2["FechaProgramacion"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                tblProgramacionDet.Rows.Add(dr);
            }

            int idhijo = 1;
            int NombreTabla = 1;

            while (dtvProgramacion.Count != 0)
            {
                //IdTipoOT = 1 -> Interna | 2->Externa
                int IdTipoOT = Convert.ToInt32(dtvProgramacion[0]["IdTipoOT"]);
                DateTime FechaProgramacion = Convert.ToDateTime(dtvProgramacion[0]["FechaProgramacion"]);
                int IdUC = Convert.ToInt32(dtvProgramacion[0]["IdUC"]);
                string CodUC = dtvProgramacion[0]["CodUC"].ToString();
                int IdPerfilCompActividad = 0;
                int IdPerfil = Convert.ToInt32(dtvProgramacion[0]["IdPerfil"]);

                string TipoOT = "";
                DataTable tblmaestra = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=20", dtv_maestra);
                for (int i = 0; i < tblmaestra.Rows.Count; i++)
                {
                    if (Convert.ToInt32(tblmaestra.Rows[i]["Valor"]) == IdTipoOT)
                    {
                        TipoOT = tblmaestra.Rows[i]["Descripcion"].ToString();
                    }
                }

                string Header = dtvProgramacion[0]["UC"].ToString() + " - " + TipoOT + " - " + FechaProgramacion.ToString();
                DataRow drConfir;
                drConfir = tblConfirmacion.NewRow();
                drConfir["Hijo"] = idhijo;
                drConfir["Padre"] = "0";
                drConfir["descripcion"] = Header;
                drConfir["Visibility"] = "Visible";
                drConfir["DetailSource"] = ComboResponsable();
                if (OHEMlist.Count != 0) { CodigoPersonaDefecto = OHEMlist[0].CodigoPersona; }
                drConfir["CodigoPersona"] = CodigoPersonaDefecto;
                drConfir["IdTipoOT"] = IdTipoOT;
                drConfir["FechaProgramacion"] = FechaProgramacion;
                drConfir["IdUC"] = IdUC;
                tblConfirmacion.Rows.Add(drConfir);

                objE_PerfilTarea.IdPerfil = IdPerfil;
                DataView dtvTareas = new DataView(objB_PerfilTarea.PerfilTarea_List(objE_PerfilTarea));

                int CantRegs = dtvProgramacion.Count;
                int RegIndex = 0;
                while (CantRegs != 0)
                {
                    int dtvIdTipoOT = Convert.ToInt32(dtvProgramacion[RegIndex]["IdTipoOT"]);
                    DateTime dtvFechaProgramacion = Convert.ToDateTime(dtvProgramacion[RegIndex]["FechaProgramacion"]);
                    int dtvIdUC = Convert.ToInt32(dtvProgramacion[RegIndex]["IdUC"]);

                    if (dtvIdTipoOT == IdTipoOT && dtvFechaProgramacion == FechaProgramacion && dtvIdUC == IdUC)
                    {
                        IdPerfilCompActividad = Convert.ToInt32(dtvProgramacion[RegIndex]["IdPerfilCompActividad"]);

                        int ExisteComponente = tblOTComp.Select("IdPerfilComp = " + dtvProgramacion[RegIndex]["IdPerfilComp"].ToString()).Length;
                        int ExisteActividad = tblActividades.Select("IdPerfilComp = " + dtvProgramacion[RegIndex]["IdPerfilComp"].ToString() + " AND IdPerfilCompActividad = " + IdPerfilCompActividad).Length;

                        if (ExisteComponente == 0)
                        {
                            DataRow drco = tblOTComp.NewRow();
                            drco["IdOTComp"] = 0;
                            drco["IdUCComp"] = Convert.ToInt32(dtvProgramacion[RegIndex]["IdUCComp"]);
                            drco["IdOT"] = 0;
                            drco["IdPerfilComp"] = Convert.ToInt32(dtvProgramacion[RegIndex]["IdPerfilComp"]);
                            drco["IdPerfilCompPadre"] = 0;
                            drco["PerfilComp"] = dtvProgramacion[RegIndex]["PerfilComp"].ToString();
                            drco["IdTipoDetalle"] = 0;
                            drco["NroSerie"] = dtvProgramacion[RegIndex]["NroSerie"].ToString();
                            drco["CodigoSAP"] = dtvProgramacion[RegIndex]["CodigoSAP"].ToString();
                            drco["DescripcionSAP"] = dtvProgramacion[RegIndex]["DescripcionSAP"].ToString();
                            drco["IdEstadoOTComp"] = 1;
                            drco["FlagActivo"] = true;
                            drco["IsChecked"] = false;
                            drco["Nuevo"] = true;
                            tblOTComp.Rows.Add(drco);

                            if (Convert.ToBoolean(dtvProgramacion[RegIndex]["FlagUso"])) //Agregando los componentes de vida propia
                            {
                                DataRow row = tblConsumible.NewRow();
                                row["IdOTArticulo"] = 0;
                                row["IdOTCompActividad"] = 0;
                                row["IdPerfilCompActividad"] = Convert.ToInt32(dtvProgramacion[RegIndex]["IdPerfilCompActividad"]);
                                row["IdTipoArticulo"] = 4;
                                row["IdArticulo"] = dtvProgramacion[RegIndex]["CodigoSAP"].ToString();
                                row["Articulo"] = dtvProgramacion[RegIndex]["DescripcionSAP"].ToString();
                                row["CantSol"] = 1;
                                row["CantEnv"] = 0;
                                row["CantUti"] = 0;
                                row["Observacion"] = 0;
                                row["CostoArticulo"] = 0;
                                row["FlagAutomatico"] = true;
                                row["FlagActivo"] = true;
                                row["Nuevo"] = true;
                                tblConsumible.Rows.Add(row);
                            }
                        }

                        if (ExisteActividad == 0)
                        {
                            DataRow drac = tblActividades.NewRow();
                            drac["IdOTCompActividad"] = 0;
                            drac["IdOTComp"] = 0;
                            drac["IdPerfilComp"] = Convert.ToInt32(dtvProgramacion[RegIndex]["IdPerfilComp"]);
                            drac["IdPerfilCompActividad"] = Convert.ToInt32(dtvProgramacion[RegIndex]["IdPerfilCompActividad"]);
                            drac["IdActividad"] = Convert.ToInt32(dtvProgramacion[RegIndex]["IdActividad"]);
                            drac["Actividad"] = dtvProgramacion[RegIndex]["Actividad"].ToString();

                            if (IdTipoOT == 1)
                            {
                                drac["IsChecked"] = false;
                            }
                            else if (IdTipoOT == 2)
                            {
                                drac["IsChecked"] = true;
                            }
                            drac["FlagUso"] = false;
                            drac["FlagActivo"] = true;
                            drac["Nuevo"] = true;
                            tblActividades.Rows.Add(drac);


                            dtvTareas.RowFilter = "IdPerfilCompActividad = " + IdPerfilCompActividad;

                            for (int Tareas = 0; Tareas < dtvTareas.Count; Tareas++)
                            {
                                DataRow drTa = tblTareas.NewRow();
                                drTa["IdOTTarea"] = 0;
                                drTa["IdOTCompActividad"] = 0;
                                drTa["IdPerfilCompActividad"] = Convert.ToInt32(dtvTareas[Tareas]["IdPerfilCompActividad"]);
                                drTa["IdTarea"] = Convert.ToInt32(dtvTareas[Tareas]["IdTarea"]);
                                drTa["OTTarea"] = dtvTareas[Tareas]["Tarea"].ToString();
                                drTa["IdPerfilTarea"] = 0;
                                drTa["CodResponsable"] = 0;
                                drTa["CostoHoraHombre"] = 0;
                                drTa["HorasEstimada"] = Convert.ToDouble(dtvTareas[Tareas]["HorasHombre"]);
                                drTa["HorasReal"] = 0;
                                drTa["IdEstadoOTT"] = 1;
                                drTa["FlagAutomatico"] = true;
                                drTa["FlagActivo"] = true;
                                drTa["Nuevo"] = true;
                                tblTareas.Rows.Add(drTa);
                            }
                            objE_Perfil.Idperfil = IdPerfil;
                            DataTable tblPerfilDetalleDatos = objB_PerfilDetalle.PerfilDetalle_List(objE_Perfil);
                            tblPerfilDetalleDatos.Columns.Add("FlagRegistrado", Type.GetType("System.Boolean"));
                            for (int dt = 0; dt < tblPerfilDetalleDatos.Rows.Count; dt++)
                            {
                                tblPerfilDetalleDatos.Rows[dt]["FlagRegistrado"] = true;
                            }
                            DataView dtvPerfilDetalleDatos = new DataView(tblPerfilDetalleDatos);
                            dtvPerfilDetalleDatos.RowFilter = "FlagRegistrado = true";

                            while (dtvPerfilDetalleDatos.Count != 0)
                            {
                                if (Convert.ToInt32(dtvPerfilDetalleDatos[0]["IdPerfilCompActividad"]) == IdPerfilCompActividad)
                                {
                                    if (dtvPerfilDetalleDatos[0]["IdTipoArticulo"].ToString() == "1")
                                    {
                                        DataRow drHer = tblHerrEsp.NewRow();
                                        drHer["IdOTHerramienta"] = 0;
                                        drHer["IdOTCompActividad"] = 0;
                                        drHer["IdPerfilCompActividad"] = Convert.ToInt32(dtvPerfilDetalleDatos[0]["IdPerfilCompActividad"]);
                                        drHer["IdHerramienta"] = Convert.ToInt32(dtvPerfilDetalleDatos[0]["IdArticulo"]);
                                        drHer["Herramienta"] = dtvPerfilDetalleDatos[0]["Articulo"].ToString();
                                        drHer["Cantidad"] = Convert.ToDouble(dtvPerfilDetalleDatos[0]["Cantidad"]);
                                        drHer["IdEstado"] = 1;
                                        drHer["NroDevolucion"] = 0;
                                        drHer["FlagAutomatico"] = true;
                                        drHer["FlagActivo"] = true;
                                        drHer["Nuevo"] = true;
                                        tblHerrEsp.Rows.Add(drHer);
                                    }
                                    else if (dtvPerfilDetalleDatos[0]["IdTipoArticulo"].ToString() == "2")
                                    {
                                        DataRow drRe = tblRepuesto.NewRow();
                                        drRe["IdOTArticulo"] = 0;
                                        drRe["IdOTCompActividad"] = 0;
                                        drRe["IdPerfilCompActividad"] = Convert.ToInt32(dtvPerfilDetalleDatos[0]["IdPerfilCompActividad"]);
                                        drRe["IdTipoArticulo"] = Convert.ToInt32(dtvPerfilDetalleDatos[0]["IdTipoArticulo"]);
                                        drRe["IdArticulo"] = dtvPerfilDetalleDatos[0]["IdArticulo"].ToString();

                                        for (int j = 0; j < BEOITMListCon.Count; j++)
                                        {
                                            if (Convert.ToString(dtvPerfilDetalleDatos[0]["IdArticulo"]) == BEOITMListCon[j].CodigoArticulo)
                                            {
                                                drRe["Articulo"] = BEOITMListCon[j].DescripcionArticulo;
                                                break;
                                            }
                                        }

                                        drRe["CantSol"] = Convert.ToDouble(dtvPerfilDetalleDatos[0]["Cantidad"]);
                                        drRe["CantEnv"] = 0;
                                        drRe["CantUti"] = 0;
                                        drRe["CostoArticulo"] = 0;
                                        drRe["Observacion"] = "";
                                        drRe["CodResponsable"] = "";
                                        drRe["FlagAutomatico"] = true;
                                        drRe["NroSerie"] = "";
                                        drRe["Frecuencia"] = 0.00;
                                        drRe["FlagActivo"] = true;
                                        drRe["Nuevo"] = true;
                                        tblRepuesto.Rows.Add(drRe);
                                    }
                                    else if (dtvPerfilDetalleDatos[0]["IdTipoArticulo"].ToString() == "3")
                                    {
                                        DataRow drCons = tblConsumible.NewRow();
                                        drCons["IdOTArticulo"] = 0;
                                        drCons["IdOTCompActividad"] = 0;
                                        drCons["IdPerfilCompActividad"] = Convert.ToInt32(dtvPerfilDetalleDatos[0]["IdPerfilCompActividad"]);
                                        drCons["IdTipoArticulo"] = Convert.ToInt32(dtvPerfilDetalleDatos[0]["IdTipoArticulo"]);
                                        drCons["IdArticulo"] = dtvPerfilDetalleDatos[0]["IdArticulo"].ToString();

                                        for (int j = 0; j < BEOITMListCon.Count; j++)
                                        {
                                            if (Convert.ToString(dtvPerfilDetalleDatos[0]["IdArticulo"]) == BEOITMListCon[j].CodigoArticulo)
                                            {
                                                drCons["Articulo"] = BEOITMListCon[j].DescripcionArticulo;
                                                break;
                                            }
                                        }
                                        drCons["CantSol"] = Convert.ToDouble(dtvPerfilDetalleDatos[0]["Cantidad"]);
                                        drCons["CantEnv"] = 0;
                                        drCons["CantUti"] = 0;
                                        drCons["CostoArticulo"] = 0;
                                        drCons["Observacion"] = "";
                                        drCons["CodResponsable"] = "";
                                        drCons["FlagAutomatico"] = true;
                                        drCons["NroSerie"] = "";
                                        drCons["Frecuencia"] = 0.00;
                                        drCons["FlagActivo"] = true;
                                        drCons["Nuevo"] = true;
                                        tblConsumible.Rows.Add(drCons);
                                    }
                                }
                                dtvPerfilDetalleDatos[0]["FlagRegistrado"] = false;
                            }
                        }
                        dtvProgramacion[RegIndex]["FlagRegistrado"] = false;
                    }
                    else
                    {
                        RegIndex++;
                    }

                    CantRegs--;
                }

                DataRow drOT = tblDatosOT.NewRow();
                drOT["IdOT"] = 0;
                drOT["IdHI"] = 0; //agregado
                drOT["NombreOT"] = "";
                drOT["IdTipoOT"] = IdTipoOT;
                drOT["FlagSinUC"] = 0;
                drOT["IdUC"] = IdUC;
                drOT["CodUC"] = CodUC;
                drOT["FechaProg"] = FechaProgramacion;
                drOT["CodResponsable"] = "0";
                drOT["NombreResponsable"] = "";
                drOT["IdTipoGeneracion"] = objE_Programacion.TipoMantenimiento;
                drOT["IdEstadoOT"] = 1;
                drOT["MotivoPostergacion"] = "";
                drOT["Observacion"] = Convert.ToString(TxTComentarios.Text);
                drOT["FlagActivo"] = 1;
                drOT["IdUsuario"] = gintUsuario;
                tblDatosOT.Rows.Add(drOT);

                tblOTComp.TableName = "Tabla" + NombreTabla;
                tblActividades.TableName = "Tabla" + NombreTabla;
                tblTareas.TableName = "Tabla" + NombreTabla;
                tblHerrEsp.TableName = "Tabla" + NombreTabla;
                tblRepuesto.TableName = "Tabla" + NombreTabla;
                tblConsumible.TableName = "Tabla" + NombreTabla;

                dsOTComp.Tables.Add(tblOTComp.Copy());
                dsActividades.Tables.Add(tblActividades.Copy());
                dsTareas.Tables.Add(tblTareas.Copy());
                dsHerrEsp.Tables.Add(tblHerrEsp.Copy());
                dsRepuesto.Tables.Add(tblRepuesto.Copy());
                dsConsumible.Tables.Add(tblConsumible.Copy());

                for (int co = 0; co < tblOTComp.Rows.Count; co++)
                {
                    string idcomp = "C" + (co + 1) + idhijo;
                    drConfir = tblConfirmacion.NewRow();
                    drConfir["Hijo"] = idcomp;
                    drConfir["Padre"] = idhijo.ToString();
                    drConfir["descripcion"] = tblOTComp.Rows[co]["PerfilComp"].ToString();
                    drConfir["Visibility"] = "Hidden";
                    drConfir["CodigoPersona"] = 0;
                    tblConfirmacion.Rows.Add(drConfir);

                    DataView dtvActividadesTrv = new DataView(tblActividades);
                    dtvActividadesTrv.RowFilter = "IdPerfilComp = " + tblOTComp.Rows[co]["IdPerfilComp"].ToString();
                    for (int ac = 0; ac < dtvActividadesTrv.Count; ac++)
                    {
                        string idac = idcomp + "A" + (ac + 1);
                        drConfir = tblConfirmacion.NewRow();
                        drConfir["Hijo"] = idac;
                        drConfir["Padre"] = idcomp;
                        drConfir["descripcion"] = dtvActividadesTrv[ac]["Actividad"].ToString();
                        drConfir["Visibility"] = "Hidden";
                        drConfir["CodigoPersona"] = 0;
                        tblConfirmacion.Rows.Add(drConfir);
                    }
                }

                idhijo++;
                NombreTabla++;
                tblOTComp.Rows.Clear();
                tblActividades.Rows.Clear();
                tblTareas.Rows.Clear();
                tblHerrEsp.Rows.Clear();
                tblRepuesto.Rows.Clear();
                tblConsumible.Rows.Clear();
            }
            trvConfirmarRegistrosOT.ItemsSource = tblConfirmacion;
            treeListView2.ExpandAllNodes();
            stkPanelGrabarOT.Visibility = Visibility.Visible;
        }
        private void btnCancOT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (worker.IsBusy) { worker.CancelAsync(); }
                pgbInsertando.Value = 0;
                lblRegOTS.Content = "";
                lblRegOTS.Visibility = Visibility.Collapsed;
                pgbInsertando.Visibility = Visibility.Collapsed;
                stkPanelGrabarOT.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }


        private void btnAcepOT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < tblProgramacionDet.Rows.Count; i++)
                {
                        DataRow drbit = tblBitacora.NewRow();
                        drbit["LineNum"] = Convert.ToInt32(tblProgramacionDet.Rows[i]["LineNum"]);
                        drbit["IdOT"] = 0;
                        drbit["IdEstado"] = 0;
                        drbit["Error"] = "";
                        tblBitacora.Rows.Add(drbit);
                }
                int rpta =objB_Programacion.Programacion_BeforeCreate(tblBitacora);
                tblBitacora.Rows.Clear();
                if (rpta == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "LOGI_GRAB"), 2);
                    tblBitacora.Rows.Clear();
                    return;
                }

                DataView dtvRespon = new DataView((DataTable)trvConfirmarRegistrosOT.ItemsSource);
                dtvRespon.RowFilter = "CodigoPersona <> 0";
                for (int i = 0; i < dtvRespon.Count; i++)
                {
                    int IdTipoOT = Convert.ToInt32(dtvRespon[i]["IdTipoOT"]);
                    string FechaProgramacion = dtvRespon[i]["FechaProgramacion"].ToString();
                    int IdUC = Convert.ToInt32(dtvRespon[i]["IdUC"]);

                    for (int a = 0; a < tblDatosOT.Rows.Count; a++)
                    {
                        int IdTipoOTDatos = Convert.ToInt32(tblDatosOT.Rows[a]["IdTipoOT"]);
                        string FechaProgramacionDatos = tblDatosOT.Rows[a]["FechaProg"].ToString();
                        int IdUCDatos = Convert.ToInt32(tblDatosOT.Rows[a]["IdUC"]);
                        if ((IdTipoOT == IdTipoOTDatos) && (FechaProgramacion == FechaProgramacionDatos) && (IdUC == IdUCDatos))
                        {
                            tblDatosOT.Rows[a]["CodResponsable"] = dtvRespon[i]["CodigoPersona"].ToString();
                            for (int b = 0; b < OHEMlist.Count; b++)
                            {
                                if (Convert.ToInt32(dtvRespon[i]["CodigoPersona"]) == OHEMlist[b].CodigoPersona)
                                {
                                    tblDatosOT.Rows[a]["NombreResponsable"] = OHEMlist[b].NombrePersona;
                                    break;
                                }
                            }
                        }
                    }
                }
                pgbInsertando.Visibility = Visibility.Visible;
                lblRegOTS.Visibility = Visibility.Visible;
                lblRegOTS.Content = "";
                pgbInsertando.Value = 0;
                worker.WorkerSupportsCancellation = true;
                worker.WorkerReportsProgress = true;
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int value = 100 / tblDatosOT.Rows.Count;
                for (int i = 0; i < tblDatosOT.Rows.Count; i++)
                {
                    System.Threading.Thread.Sleep(500);
                    int IdTipoOT = Convert.ToInt32(tblDatosOT.Rows[i]["IdTipoOT"]);
                    //Setear Entidad OT
                    objE_OT = new E_OT();
                    objE_OT.IdOT = Convert.ToInt32(tblDatosOT.Rows[i]["IdOT"]);
                    objE_OT.NombreOT = tblDatosOT.Rows[i]["NombreOT"].ToString();
                    objE_OT.IdTipoOT = Convert.ToInt32(tblDatosOT.Rows[i]["IdTipoOT"]);
                    objE_OT.FlagSinUC = Convert.ToInt32(tblDatosOT.Rows[i]["FlagSinUC"]);
                    objE_OT.IdUC = Convert.ToInt32(tblDatosOT.Rows[i]["IdUC"]);
                    objE_OT.FechaProg = Convert.ToDateTime(tblDatosOT.Rows[i]["FechaProg"]);
                    objE_OT.CodResponsable = tblDatosOT.Rows[i]["CodResponsable"].ToString();
                    objE_OT.NombreResponsable = tblDatosOT.Rows[i]["NombreResponsable"].ToString();
                    objE_OT.IdTipoGeneracion = Convert.ToInt32(tblDatosOT.Rows[i]["IdTipoGeneracion"]);
                    objE_OT.IdEstadoOT = Convert.ToInt32(tblDatosOT.Rows[i]["IdEstadoOT"]);
                    objE_OT.MotivoPostergacion = tblDatosOT.Rows[i]["MotivoPostergacion"].ToString();
                    objE_OT.Observacion = tblDatosOT.Rows[i]["Observacion"].ToString();
                    objE_OT.FlagActivo = Convert.ToInt32(tblDatosOT.Rows[i]["FlagActivo"]);
                    objE_OT.IdUsuario = Convert.ToInt32(tblDatosOT.Rows[i]["IdUsuario"]);
                    objE_OT.FechaModificacion = DateTime.Now;
                    DataSet rpta = objB_OT.OT_UpdateCascada(objE_OT, dsOTComp.Tables[i], dsActividades.Tables[i], dsTareas.Tables[i], dsHerrEsp.Tables[i], dsRepuesto.Tables[i], dsConsumible.Tables[i]);
                    if (rpta.Tables.Count == 3)
                    {
                        try
                        {
                            int IdOT = Convert.ToInt32(rpta.Tables[0].Rows[0][0].ToString().Substring(2));
                            int TipoOT = objE_OT.IdTipoOT; string FechaProg = objE_OT.FechaProg.ToString(); int IdUC = objE_OT.IdUC;

                            try
                            {
                                objE_HI.IdHI = Convert.ToInt32(tblDatosOT.Rows[i]["IdHI"]);
                                objE_HI.IdOT = IdOT;
                                int rptaes = objB_HI.HI_UpdateEstado(objE_HI);
                            }
                            catch { }
                            
                            DataView dtvProgDet = new DataView(tblProgramacionDet);
                            dtvProgDet.RowFilter = "TipoOT = " + TipoOT + " AND FechaProgramacion = '" + Convert.ToDateTime(FechaProg).ToString("dd/MM/yyyy HH:mm:ss") + "' AND IdUC = " + IdUC;
                            for (int dr = 0; dr < dtvProgDet.Count; dr++)
                            {
                                for (int drprog = 0; drprog < tblProgramacionDet.Rows.Count; drprog++)
                                {
                                    if (Convert.ToInt32(dtvProgDet[dr]["LineNum"]) == Convert.ToInt32(tblProgramacionDet.Rows[drprog]["LineNum"]))
                                    {
                                        DataRow drbit = tblBitacora.NewRow();
                                        drbit["LineNum"] = Convert.ToInt32(dtvProgDet[dr]["LineNum"]);
                                        drbit["IdOT"] = IdOT;
                                        drbit["IdEstado"] = 1;
                                        drbit["Error"] = "";
                                        tblBitacora.Rows.Add(drbit);

                                        tblProgramacionDet.Rows[drprog]["IdOT"] = IdOT;
                                    }
                                }
                            }

                            tucuclist = InterfazMTTO.iSBO_BL.UnidadControl_BL.ListaUnidadControl(tblDatosOT.Rows[i]["CodUC"].ToString(), ref RPTA);

                            DataRow[] drCodUC = tblProgramacionDatos.Select("IdUC = " + Convert.ToInt32(tblDatosOT.Rows[i]["IdUC"]));
                            DataRow drRegOT = tblRegistrados.NewRow();
                            drRegOT["CodigoOT"] = rpta.Tables[0].Rows[0][0].ToString();
                            drRegOT["UC"] = drCodUC[0]["UC"].ToString();
                            drRegOT["FechaProgramacion"] = Convert.ToDateTime(tblDatosOT.Rows[i]["FechaProg"]);
                            drRegOT["Responsable"] = tblDatosOT.Rows[i]["NombreResponsable"].ToString();
                            drRegOT["msg"] = "[MSG MMTO] OT Registrada correctamente";


                            //PAQ03_Ejecucion.EjecGestionOT OT = new PAQ03_Ejecucion.EjecGestionOT();
                            //OT.GrabarSAP(rpta, dsActividades.Tables[i], dsRepuesto.Tables[i], dsConsumible.Tables[i], tblOTArticuloSol, Convert.ToDateTime(tblDatosOT.Rows[i]["FechaProg"]));
                            //Grabar SAP
                            objE_TablaMaestra.IdTabla = 42;
                            DataTable tblAlmacen = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);

                            objE_TablaMaestra.IdTabla = 43;
                            DataTable tblTipoOperacion = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);

                            OWTQ = new InterfazMTTO.iSBO_BE.BEOWTQ();
                            OWTQ.NroOrdenTrabajo = rpta.Tables[0].Rows[0][0].ToString();
                            OWTQ.AlmacenEntrada = tblAlmacen.Rows[1]["Valor"].ToString(); //Alm Mantenimient
                            OWTQ.AlmacenSalida = tblAlmacen.Rows[0]["Valor"].ToString();//General
                            OWTQ.FechaSolicitud = Convert.ToDateTime(tblDatosOT.Rows[i]["FechaProg"]);

                            WTQ1List = new InterfazMTTO.iSBO_BE.BEWTQ1List();
                            int cant = 0;
                            for (int a = 0; a < dsRepuesto.Tables[i].Rows.Count; a++)
                            {
                                if (Convert.ToBoolean(dsRepuesto.Tables[i].Rows[a]["Nuevo"]) == true)
                                {
                                    InterfazMTTO.iSBO_BE.BEWTQ1 BEWTQ1 = new InterfazMTTO.iSBO_BE.BEWTQ1();
                                    BEWTQ1.NroOrdenTrabajo = rpta.Tables[0].Rows[0][0].ToString();
                                    BEWTQ1.NroLinea = Convert.ToInt32(rpta.Tables[1].Rows[cant]["IdOTArticulo"]);//De la BD
                                    BEWTQ1.CodigoArticulo = Convert.ToString(dsRepuesto.Tables[i].Rows[a]["IdArticulo"]);
                                    BEWTQ1.CantidadSolicitada = Convert.ToInt32(dsRepuesto.Tables[i].Rows[a]["CantSol"]);
                                    BEWTQ1.TipoOperacion = tblTipoOperacion.Rows[0]["Valor"].ToString(); //Tabla Maestra  --> 12
                                    BEWTQ1.CCosto1 = tucuclist[0].CentroCosto1;
                                    BEWTQ1.CCosto2 = tucuclist[0].CentroCosto2;
                                    BEWTQ1.CCosto3 = tucuclist[0].CentroCosto3;
                                    BEWTQ1.CCosto4 = tucuclist[0].CentroCosto4;
                                    BEWTQ1.CCosto5 = tucuclist[0].CentroCosto5;
                                    WTQ1List.Add(BEWTQ1);
                                    cant++;
                                }
                            }

                            cant = 0;
                            for (int b = 0; b < dsConsumible.Tables[i].Rows.Count; b++)
                            {
                                if (Convert.ToBoolean(dsConsumible.Tables[i].Rows[b]["Nuevo"]) == true)
                                {
                                    InterfazMTTO.iSBO_BE.BEWTQ1 BEWTQ1 = new InterfazMTTO.iSBO_BE.BEWTQ1();
                                    BEWTQ1.NroOrdenTrabajo = rpta.Tables[0].Rows[0][0].ToString();
                                    BEWTQ1.NroLinea = Convert.ToInt32(rpta.Tables[2].Rows[cant]["IdOTArticulo"]);//De la BD
                                    BEWTQ1.CodigoArticulo = Convert.ToString(dsConsumible.Tables[i].Rows[b]["IdArticulo"]);
                                    BEWTQ1.CantidadSolicitada = Convert.ToInt32(dsConsumible.Tables[i].Rows[b]["CantSol"]);
                                    BEWTQ1.TipoOperacion = tblTipoOperacion.Rows[0]["Valor"].ToString(); //Tabla Maestra  --> 12
                                    BEWTQ1.CCosto1 = tucuclist[0].CentroCosto1;
                                    BEWTQ1.CCosto2 = tucuclist[0].CentroCosto2;
                                    BEWTQ1.CCosto3 = tucuclist[0].CentroCosto3;
                                    BEWTQ1.CCosto4 = tucuclist[0].CentroCosto4;
                                    BEWTQ1.CCosto5 = tucuclist[0].CentroCosto5;
                                    WTQ1List.Add(BEWTQ1);
                                    cant++;
                                }
                            }
                            if (WTQ1List.Count > 0 && Convert.ToInt32(IdTipoOT) != 2) //Revisar 
                            {
                                WTQ1List = InterfazMTTO.iSBO_BL.SolicitudTransferencia_BL.RegistraSolicitudTransferencia(OWTQ, WTQ1List, ref RPTA);
                                if (RPTA.ResultadoRetorno == 0)
                                {
                                    tblOTArticuloSol.Rows.Clear();
                                    //Actualizar datos OTArticulo
                                    for (int c = 0; c < WTQ1List.Count; c++)
                                    {
                                        DataRow dr = tblOTArticuloSol.NewRow();
                                        dr["IdOTArticulo"] = WTQ1List[c].NroLineaOT;
                                        dr["NroSolTransfer"] = WTQ1List[c].NroSolicitudTransferencia;
                                        dr["NroLinSolTransfer"] = WTQ1List[c].NroLinea;
                                        tblOTArticuloSol.Rows.Add(dr);
                                    }
                                    int x = objB_OT.OTArticulo_Update(1, tblOTArticuloSol);
                                    drRegOT["msgSAP"] = "[MSG SAP] Solicitud registrada";
                                }
                                else
                                {
                                    drRegOT["msgSAP"] = RPTA.DescripcionErrorUsuario;
                                }
                            }
                            tblRegistrados.Rows.Add(drRegOT);
                        }
                        catch (Exception ex)
                        {
                            DataRow[] drCodUC = tblProgramacionDatos.Select("IdUC = " + Convert.ToInt32(tblDatosOT.Rows[i]["IdUC"]));
                            DataRow drRegOT = tblRegistrados.NewRow();
                            drRegOT["CodigoOT"] = rpta.Tables[0].Rows[0][0].ToString();
                            drRegOT["UC"] = drCodUC[0]["UC"].ToString();
                            drRegOT["FechaProgramacion"] = Convert.ToDateTime(tblDatosOT.Rows[i]["FechaProg"]);
                            drRegOT["Responsable"] = tblDatosOT.Rows[i]["NombreResponsable"].ToString();
                            drRegOT["msg"] = "[MSG MMTO] Se registró la OT";
                            drRegOT["msgSAP"] = "[MSG SAP] Solicitud no registrada Error: " + ex.Message;
                            tblRegistrados.Rows.Add(drRegOT);
                        }
                        OTSReg++;
                        worker.ReportProgress(value);
                    }
                    else
                    {
                        //GlobalClass.ip.Mensaje(rpta.Tables[0].Rows[0]["ERR_MESSAGE"].ToString(), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                worker.ReportProgress(100);
                ErrorWorker = ex.Message;
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                int cantOTS = tblDatosOT.Rows.Count;
                pgbInsertando.Value = 100;
                lblRegOTS.Content = String.Format("{0} de {1} . . .", OTSReg.ToString(), cantOTS);

                if (OTSReg != 0)
                {
                    tblDatosOT.Rows.Clear();
                    if ((cantOTS - OTSReg) == 0)
                    {
                        GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "GRAB_PROG"), OTSReg.ToString()), 1);
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "GRAB_PROG_ERROR"), OTSReg.ToString(), (cantOTS - OTSReg).ToString(), ErrorWorker), 1);
                    }
                    //Grabando La Programacion
                    tblProgramacionDet.Columns.Remove("TipoOT");
                    tblProgramacionDet.Columns.Remove("FechaProgramacion");
                    objE_Programacion = new E_Programacion();
                    objE_Programacion.FechaProgramacion = Convert.ToDateTime(dtpFechaProgram.EditValue);
                    objE_Programacion.Observacion = TxTComentarios.Text;
                    objE_Programacion.FlagActivo = true;

                    int rpta = objB_Programacion.Programacion_UpdateCascade(objE_Programacion, tblBitacora, tblProgramacionDet);
                    if (rpta != 0)
                    {
                        GlobalClass.ip.Mensaje("Error al Actualizar la bitacora", 3);
                    }
                    tblProgramacionDet.Columns.Add("TipoOT", Type.GetType("System.Int32"));
                    tblProgramacionDet.Columns.Add("FechaProgramacion", Type.GetType("System.String"));
                }
                else
                {
                    GlobalClass.ip.Mensaje(ErrorWorker, 3);
                    pgbInsertando.Value = 0;
                }
                trvConfirmarRegistrosOT.ItemsSource = null;
                tblBitacora.Rows.Clear();
                tblProgramacionDet.Rows.Clear();
                dtgListaOTResgistradas.ItemsSource = tblRegistrados;
                OTSReg = 0;
                TimerProgramacion.Start();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                lblRegOTS.Content = String.Format("{0} de {1} . . .", OTSReg.ToString(), tblDatosOT.Rows.Count);
                pgbInsertando.Value += e.ProgressPercentage;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnAceptarTer_Click(object sender, RoutedEventArgs e)
        {
            tblRegistrados.Rows.Clear();
            if (worker.IsBusy) { worker.CancelAsync(); }
            pgbInsertando.Value = 0;
            lblRegOTS.Content = "";
            trvConfirmarRegistrosOT.ItemsSource = null;
            dtgListaOTResgistradas.ItemsSource = null;
            dtgProgramacionPreventivo.ItemsSource = null;
            stkGrabarOTTerminado.Visibility = Visibility.Collapsed;
            pgbInsertando.Visibility = Visibility.Collapsed;
            lblRegOTS.Visibility = Visibility.Collapsed;
            btnCons_Click(null, null);
        }

        void LlenarStock(out string salida)
        {
            string LineNum = "";

            if ((bool)RdnPreventivo.IsChecked)
            {
                foreach (DataRow drData in tblProgramacionDatos.Select("IsChecked = true"))
                {
                    LineNum += drData["LineNum"].ToString() + "|";
                }
            }
            else if ((bool)RdnCorrectivo.IsChecked)
            {
                foreach (DataRow drData in tblProgramacionDatos.Select("IsChecked = true"))
                {
                    LineNum += drData["IdHI"].ToString() + ",";
                }
            }
            

            DataTable tblAlmacenes = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 42", dtvMaestra);
            DataTable tmpGetStock = objB_Programacion.Bitacora_GetStock(LineNum);

            tblVerStock = new DataTable();
            tblVerStock.Columns.Add("UC");
            tblVerStock.Columns.Add("Componente");
            tblVerStock.Columns.Add("Actividad");
            tblVerStock.Columns.Add("TipoArticulo");
            tblVerStock.Columns.Add("IdArticulo");
            tblVerStock.Columns.Add("Articulo");
            tblVerStock.Columns.Add("Cantidad");
            tblVerStock.Columns.Add("Stock");

            DataRow dr;
            for (int i = 0; i < tmpGetStock.Rows.Count; i++)
            {
                dr = tblVerStock.NewRow();
                dr["UC"] = tmpGetStock.Rows[i]["UC"].ToString();
                dr["Componente"] = tmpGetStock.Rows[i]["Componente"].ToString();
                dr["Actividad"] = tmpGetStock.Rows[i]["Actividad"].ToString();
                dr["TipoArticulo"] = tmpGetStock.Rows[i]["TipoArticulo"].ToString();
                dr["IdArticulo"] = tmpGetStock.Rows[i]["IdArticulo"].ToString();
                BEOITMList = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos(tmpGetStock.Rows[i]["IdArticulo"].ToString(), tblAlmacenes.Rows[0]["Valor"].ToString(), ref RPTA);
                if (RPTA.CodigoErrorUsuario == "000")
                {
                    dr["Articulo"] = BEOITMList[0].DescripcionArticulo;
                    dr["Stock"] = BEOITMList[0].CantidadDisponible;
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }
                dr["Cantidad"] = Convert.ToInt32(tmpGetStock.Rows[i]["Cantidad"]);
                tblVerStock.Rows.Add(dr);
            }
            salida = LineNum;
        }

        private void btnVerStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgProgramacionPreventivo.VisibleRowCount == 0 && dtgProgramacionCorrectivo.VisibleRowCount == 0) { return; }
                string LineNum = "";

                LlenarStock(out LineNum);

                if (LineNum == "")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "OBLI_CANT_OT"), 2);
                    return;
                }

                if (tblVerStock.Rows.Count == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaProgramacionUC, "ALER_DETA_ASIG"), 2);
                    return;
                }

                dtgVerStock.ItemsSource = tblVerStock;
                dtgVerStock.Columns["UC"].Header = "Unidad de Control";
                dtgVerStock.Columns["Componente"].Header = "Componente";
                dtgVerStock.Columns["Actividad"].Header = "Actividad";
                dtgVerStock.Columns["TipoArticulo"].Header = "Tipo Art.";
                dtgVerStock.Columns["Articulo"].Header = "Articulo";
                dtgVerStock.Columns["Cantidad"].Header = "Cant. Solicitada";
                dtgVerStock.Columns["Stock"].Header = "Cant. Stock";
                dtgVerStock.GroupBy("UC");
                dtgVerStock.GroupBy("Componente");
                dtgVerStock.GroupBy("Actividad");

                dtgVerStock.Columns["IdArticulo"].Width = 40;
                dtgVerStock.Columns["TipoArticulo"].Width = 40;
                dtgVerStock.Columns["Cantidad"].Width = 40;
                dtgVerStock.Columns["Stock"].Width = 40;
                dtgVerStock.ExpandAllGroups();

                stkVerStock.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAcepStock_Click(object sender, RoutedEventArgs e)
        {
            dtgVerStock.ItemsSource = null;
            stkVerStock.Visibility = Visibility.Collapsed;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (gbolAllActividades)
            {
                for (int i = 0; i < tblProgramacionDatos.Rows.Count; i++)
                {
                    tblProgramacionDatos.Rows[i]["IsChecked"] = gbolAllActividades;
                }
                gbolAllActividades = false;
            }
            else if (!gbolAllActividades)
            {
                for (int i = 0; i < tblProgramacionDatos.Rows.Count; i++)
                {
                    tblProgramacionDatos.Rows[i]["IsChecked"] = gbolAllActividades;
                }
                gbolAllActividades = true;
            }
            dtgProgramacionPreventivo.ItemsSource = tblProgramacionDatos;
        }


        private void RdnPreventivo_Checked(object sender, RoutedEventArgs e)
        {
            ChkActivarPriorizacion.IsEnabled = true;
            ChkAñadirActividades.IsEnabled = true;
            ChkMostrarTodos.IsEnabled = true;

            dtgProgramacionPreventivo.Visibility = Visibility.Visible;
            dtgProgramacionCorrectivo.Visibility = Visibility.Collapsed;
        }

        private void RdnCorrectivo_Checked(object sender, RoutedEventArgs e)
        {
            ChkActivarPriorizacion.IsChecked = false;
            ChkAñadirActividades.IsChecked = false;
            ChkMostrarTodos.IsChecked = false;
            ChkActivarPriorizacion.IsEnabled = false;
            ChkAñadirActividades.IsEnabled = false;
            ChkMostrarTodos.IsEnabled = false;
            dtgProgramacionCorrectivo.Visibility = Visibility.Visible;
            dtgProgramacionPreventivo.Visibility = Visibility.Collapsed;
        }
    }
}
