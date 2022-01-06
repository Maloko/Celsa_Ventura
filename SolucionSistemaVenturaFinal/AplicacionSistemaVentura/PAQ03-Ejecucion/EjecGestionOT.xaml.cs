using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using Business;
using Entities;
using Utilitarios;
using System.Text.RegularExpressions;
using DevExpress.Xpf.Grid;
using Utilitarios.Enum;
using System.Net.Mail;

namespace AplicacionSistemaVentura.PAQ03_Ejecucion
{
    public partial class EjecGestionOT : UserControl
    {


        #region DeclaracionVariable
        public EjecGestionOT()
        {
            InitializeComponent();
            UserControl_Loaded();
        }
        public class SiteInfo
        {
            public string Category { get; set; }
            public string Hyperlink { get; set; }
            public string Name { get; set; }
        }
        int gintValorTiempoDefecto = 0;
        int gintTiempoDefecto = 0;
        int gintEstaProg;
        int gintEstaRPro;
        int gintEstaDete;
        int gintEstaLibe;
        int gintEstadoOT;
        int gintCodResponsable = 1;
        int gintIdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
        string gstrIdHerramientas = "";
        int codTipoReq = 0;

        double gdblFileSize = 0;
        double gdblMaxFileSize = 0;
        double gdblCostoTotal = 0;

        string gstrEtiquetaOT = "EjecGestionOT";
        string gstrNombResponsable = "Pepite Perez";
        string targetPath = "";// @"\\192.168.1.6\Compartido\Pruebas";
        string sourceFile = "";
        string fileName = "";
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        Boolean gbolRegHerramientas = false;
        Boolean gbolExisteTarea = false;
        Boolean gbolExisteInforme = false;
        Boolean gbolNuevoDocumento = false;

        DateTime FechaModificacion;

        InterfazMTTO.iSBO_BE.BEOHEM OHEM = new InterfazMTTO.iSBO_BE.BEOHEM();
        InterfazMTTO.iSBO_BE.BEOHEMList OHEMlist = new InterfazMTTO.iSBO_BE.BEOHEMList();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        InterfazMTTO.iSBO_BE.BEUDUC UDUC = new InterfazMTTO.iSBO_BE.BEUDUC();
        InterfazMTTO.iSBO_BE.BEUDUCList tucuclist = new InterfazMTTO.iSBO_BE.BEUDUCList();
        InterfazMTTO.iSBO_BE.BEOCRDList OCRDList = new InterfazMTTO.iSBO_BE.BEOCRDList();
        InterfazMTTO.iSBO_BE.BEOPORList OPORList = new InterfazMTTO.iSBO_BE.BEOPORList();
        InterfazMTTO.iSBO_BE.BEOITMList BEOITMList = new InterfazMTTO.iSBO_BE.BEOITMList();
        InterfazMTTO.iSBO_BE.BEOWTQ OWTQ = new InterfazMTTO.iSBO_BE.BEOWTQ();
        InterfazMTTO.iSBO_BE.BEWTQ1List WTQ1List = new InterfazMTTO.iSBO_BE.BEWTQ1List();
        InterfazMTTO.iSBO_BE.BEWTQ1 WTQ1 = new InterfazMTTO.iSBO_BE.BEWTQ1();
        InterfazMTTO.iSBO_BE.BEWTQ1List WTQ1_List = new InterfazMTTO.iSBO_BE.BEWTQ1List();
        InterfazMTTO.iSBO_BE.BEOIGE OIGE = new InterfazMTTO.iSBO_BE.BEOIGE();
        InterfazMTTO.iSBO_BE.BEIGE1 IGE1 = new InterfazMTTO.iSBO_BE.BEIGE1();
        InterfazMTTO.iSBO_BE.BEOITWList OITW_List = new InterfazMTTO.iSBO_BE.BEOITWList();

        Utilitarios.Utilitarios objBUtil = new Utilitarios.Utilitarios();
        Utilitarios.ErrorHandler Error = new Utilitarios.ErrorHandler();
        Utilitarios.DebugHandler Debug = new Utilitarios.DebugHandler();
        E_PM objEPM = new E_PM();
        E_OTComp objE_OTComp = new E_OTComp();
        E_UCComp objEUCComp = new E_UCComp();
        E_Tarea objETarea = new E_Tarea();
        E_PerfilComp objEPerfilComp = new E_PerfilComp();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        E_OT objE_OT = new E_OT();
        E_OTIProv objE_OTIProv = new E_OTIProv();
        E_Herramienta objE_Herramienta = new E_Herramienta();
        B_OTComp objB_OTComp = new B_OTComp();
        B_UCComp objBUCComp = new B_UCComp();
        B_Herramienta objHerramienta = new B_Herramienta();
        B_Tarea objTarea = new B_Tarea();
        B_Perfil objPerfil = new B_Perfil();
        B_PerfilComp objBPerfilComp = new B_PerfilComp();
        B_Actividad objActividad = new B_Actividad();
        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        B_PM objBPM = new B_PM();
        B_OT objB_OT = new B_OT();
        B_OTArticulo objB_OTArticulo = new B_OTArticulo();
        B_OTIProv objB_OTIProv = new B_OTIProv();

        DataTable tblNroSeriesAsignadas = new DataTable();
        DataTable tbl = new DataTable();
        DataTable tblPerfil = new DataTable();
        DataTable tblFechHoraServ = new DataTable();
        DataTable tblTempOT = new DataTable();
        DataTable tblOTComp = new DataTable();
        DataTable tblActividades = new DataTable();
        DataTable tblTareas = new DataTable();
        DataTable tblHerrEsp = new DataTable();
        DataTable tblRepuesto = new DataTable();
        DataTable tblConsumible = new DataTable();

        DataTable tblActividadestmp = new DataTable();
        DataTable tblTareastmp = new DataTable();
        DataTable tblHerrEsptmp = new DataTable();
        DataTable tblRepuestotmp = new DataTable();
        DataTable tblConsumibletmp = new DataTable();

        DataTable tblOTPost = new DataTable();
        DataTable tblOT = new DataTable();
        DataTable tblOTEstado = new DataTable();
        DataTable tblActividadCombo = new DataTable();
        DataSet tblOTCompDatos = new DataSet();
        DataSet tblOTCompDetDatos = new DataSet();
        DataTable tblOTCompTreeList = new DataTable();
        DataTable tbOTTareaTrabajador = new DataTable();
        DataTable tblHerrEspTarea = new DataTable();
        DataTable tblArticuloTarea = new DataTable();
        DataTable tblRIOTComp = new DataTable();
        DataTable tblRIActividades = new DataTable();
        DataTable tblOTInforme = new DataTable();
        DataView dtv_maestra = new DataView();
        DataTable tblOTArticuloSol = new DataTable();
        DataTable tblHerramientaDatosCambioEstado = new DataTable();
        DataTable tblFrecuencias = new DataTable();

        DataTable tblNroSerie = new DataTable();
        DataRow rowOT;
        //DataRow rowOT1;
        DataRow rowOTEstado;
        //DataRow rowOTPost;
        int IdOT = 0;
        int IdPerfilCompActividad = 0;
        int IdOTCompActividad = 0;
        int CodResponsable = 0;
        int IdTipoOrden = 0;
        int IdPerfilCompMax = 0;
        string gstrNroSerieSelec = "";
        int gintIdMenu = 0, gintIdOT = 0;
        string correodest;
        #endregion
        void OnFocus(object sender, RoutedEventArgs e)
        {
            (sender as Control).Background = System.Windows.Media.Brushes.LightYellow;
        }
        private void OutFocus(object sender, RoutedEventArgs e)
        {
            (sender as Control).Background = System.Windows.Media.Brushes.White;
        }
        void ListarOT()
        {

            int IdEstado = 0;
            if (rbtTodos.IsChecked == true) { IdEstado = 0; }
            if (rbtProgramado.IsChecked == true) { IdEstado = 1; }
            if (rbtReprogramado.IsChecked == true) { IdEstado = 2; }
            if (rbtDetenido.IsChecked == true) { IdEstado = 3; }
            if (rbtLiberado.IsChecked == true) { IdEstado = 4; }
            if (rbtCerrado.IsChecked == true) { IdEstado = 5; }
            if (rbtCancelar.IsChecked == true) { IdEstado = 6; }

            objE_OT = new E_OT();
            objE_OT.IdEstadoOT = IdEstado;
            tbl.Clear();
            tblTempOT.Clear();
            dtgOT.ItemsSource = tblTempOT.DefaultView;
            tbl = objB_OT.B_OT_List(objE_OT);

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                rowOT = tblTempOT.NewRow();
                rowOT[" "] = false;
                rowOT["IdOT"] = tbl.Rows[i]["IdOT"].ToString();
                rowOT["CodOT"] = tbl.Rows[i]["CodOT"].ToString();
                rowOT["CodUC"] = tbl.Rows[i]["CodUC"].ToString();
                rowOT["IdTipoOT"] = tbl.Rows[i]["IdTipoOT"].ToString();
                rowOT["TipoOT"] = tbl.Rows[i]["TipoOT"].ToString();
                rowOT["FlagSinUC"] = tbl.Rows[i]["FlagSinUC"].ToString();
                rowOT["IdUC"] = tbl.Rows[i]["IdUC"].ToString();
                rowOT["EstadoOT"] = tbl.Rows[i]["EstadoOT"].ToString();
                rowOT["FechaProg"] = tbl.Rows[i]["FechaProg"].ToString();
                rowOT["FechaLiber"] = tbl.Rows[i]["FechaLiber"].ToString();
                rowOT["FechaCierre"] = tbl.Rows[i]["FechaCierre"].ToString();
                rowOT["IdTipoGeneracion"] = tbl.Rows[i]["IdTipoGeneracion"].ToString();
                rowOT["IdEstadoOT"] = tbl.Rows[i]["IdEstadoOT"].ToString();
                rowOT["Observacion"] = tbl.Rows[i]["Observacion"].ToString();
                rowOT["MotivoPostergacion"] = tbl.Rows[i]["MotivoPostergacion"].ToString();
                rowOT["IdUsuarioCreacion"] = tbl.Rows[i]["IdUsuarioCreacion"].ToString();
                rowOT["FechaCreacion"] = tbl.Rows[i]["FechaCreacion"].ToString();
                rowOT["HostCreacion"] = tbl.Rows[i]["HostCreacion"].ToString();
                rowOT["IdUsuarioModificacion"] = tbl.Rows[i]["IdUsuarioModificacion"].ToString();
                rowOT["FechaModificacion"] = tbl.Rows[i]["FechaModificacion"].ToString();
                rowOT["HostModificacion"] = tbl.Rows[i]["HostModificacion"].ToString();
                rowOT["FlagActivo"] = tbl.Rows[i]["FlagActivo"].ToString();
                rowOT["TipoAveria"] = tbl.Rows[i]["TipoAveria"].ToString();
                tblTempOT.Rows.Add(rowOT);
            }
            dtgOT.ItemsSource = tblTempOT.DefaultView;
        }
        //private void txtObservacionCambioEstado_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    txtObservacionCambioEstado.Text = Utilitarios.Utilitarios.SoloAlfanumerico(txtObservacionCambioEstado.Text);
        //    txtObservacionCambioEstado.SelectionStart = txtObservacionCambioEstado.Text.Length;
        //}

        private void UserControl_Loaded()//object sender, RoutedEventArgs e)
        {
            try
            {
                btnImprimirOT.Visibility = Visibility.Hidden;
                GlobalClass.ControlSubMenu(this.GetType().Name, gridTabLista);
                dtgCambioEstado.AutoGenerateColumns = DevExpress.Xpf.Grid.AutoGenerateColumnsMode.None;
                dtgPostergacion.AutoGenerateColumns = DevExpress.Xpf.Grid.AutoGenerateColumnsMode.None;
                dtgOT.AutoGenerateColumns = DevExpress.Xpf.Grid.AutoGenerateColumnsMode.None;
                tblFechHoraServ = Utilitarios.Utilitarios.Fecha_Hora_Servidor();

                objE_TablaMaestra.IdTabla = 0;
                dtv_maestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).DefaultView;

                gintEstaProg = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=18", dtv_maestra).Rows[5]["Valor"]);
                gintEstaRPro = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=18", dtv_maestra).Rows[1]["Valor"]);
                gintEstaDete = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=18", dtv_maestra).Rows[2]["Valor"]);
                gintEstaLibe = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=18", dtv_maestra).Rows[3]["Valor"]);
                targetPath = @Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=1000", dtv_maestra).Rows[2]["Valor"].ToString();
                gdblMaxFileSize = Convert.ToDouble(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=1000", dtv_maestra).Rows[4]["Valor"]);

                gintTiempoDefecto = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1000", dtv_maestra).Rows[7]["Valor"]);
                gintValorTiempoDefecto = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 58", dtv_maestra).Select("IdColumna = " + gintTiempoDefecto)[0][2]);

                // Tabla temporal para poder llenar datos a la grilla y enviar a las demas tablas del form.
                tblTempOT.Columns.Add(" ");
                tblTempOT.Columns.Add("IdOT");
                tblTempOT.Columns.Add("CodOT");
                tblTempOT.Columns.Add("CodUC");
                tblTempOT.Columns.Add("NombreOT");
                tblTempOT.Columns.Add("IdTipoOT");
                tblTempOT.Columns.Add("TipoOT");
                tblTempOT.Columns.Add("FlagSinUC");
                tblTempOT.Columns.Add("IdUC");//
                tblTempOT.Columns.Add("EstadoOT");//
                tblTempOT.Columns.Add("FechaProg");
                tblTempOT.Columns.Add("FechaLiber");
                tblTempOT.Columns.Add("FechaCierre");
                tblTempOT.Columns.Add("IdTipoGeneracion");
                tblTempOT.Columns.Add("IdEstadoOT");//
                tblTempOT.Columns.Add("MotivoPostergacion");
                tblTempOT.Columns.Add("Observacion");
                tblTempOT.Columns.Add("FlagActivo");
                tblTempOT.Columns.Add("IdUsuarioCreacion");
                tblTempOT.Columns.Add("FechaCreacion");
                tblTempOT.Columns.Add("HostCreacion");
                tblTempOT.Columns.Add("IdUsuarioModificacion");
                tblTempOT.Columns.Add("FechaModificacion");
                tblTempOT.Columns.Add("HostModificacion");
                tblTempOT.Columns.Add("TipoAveria");
     

                //2 Tabla de Reprogramacion de OT
                tblOTPost.Columns.Add("IdOTReprog", Type.GetType("System.Int32"));
                tblOTPost.Columns.Add("CodOTReprog", Type.GetType("System.String"));
                tblOTPost.Columns.Add("IdOT", Type.GetType("System.Int32"));
                tblOTPost.Columns.Add("FechaReprog", Type.GetType("System.DateTime"));
                tblOTPost.Columns.Add("FechaProg", Type.GetType("System.DateTime"));
                tblOTPost.Columns.Add("Observacion", Type.GetType("System.String"));
                tblOTPost.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblOTPost.Columns.Add("Nuevo", Type.GetType("System.Int32"));

                // Tabla de Estado
                tblOT.Columns.Add("IdOT", Type.GetType("System.Int32"));//
                tblOT.Columns.Add("CodOT", Type.GetType("System.String"));
                tblOT.Columns.Add("NombreOT", Type.GetType("System.String"));
                tblOT.Columns.Add("IdTipoOT", Type.GetType("System.Int32"));//
                tblOT.Columns.Add("FlagSinUC", Type.GetType("System.Int32"));//
                tblOT.Columns.Add("IdUC", Type.GetType("System.Int32"));//
                tblOT.Columns.Add("EstadoOT", Type.GetType("System.String"));
                tblOT.Columns.Add("FechaProg", Type.GetType("System.DateTime"));
                tblOT.Columns.Add("FechaLiber", Type.GetType("System.DateTime"));
                tblOT.Columns.Add("FechaCierre", Type.GetType("System.DateTime"));
                tblOT.Columns.Add("CodResponsable", Type.GetType("System.String"));//
                tblOT.Columns.Add("NombreResponsable", Type.GetType("System.String"));//
                tblOT.Columns.Add("IdTipoGeneracion", Type.GetType("System.Int32"));//
                tblOT.Columns.Add("IdEstadoOT", Type.GetType("System.Int32"));//
                tblOT.Columns.Add("MotivoPostergacion", Type.GetType("System.String"));
                tblOT.Columns.Add("Observacion", Type.GetType("System.String"));
                tblOT.Columns.Add("FlagActivo", Type.GetType("System.Int32"));//
                tblOT.Columns.Add("Nuevo", Type.GetType("System.Int32"));//   

                // Tabla de Estado de OT
                tblOTEstado.Columns.Add("IdOTEstado", Type.GetType("System.Int32"));
                tblOTEstado.Columns.Add("IdOT", Type.GetType("System.Int32"));
                tblOTEstado.Columns.Add("IdEstadoInicial", Type.GetType("System.Int32"));
                tblOTEstado.Columns.Add("IdEstadoFinal", Type.GetType("System.Int32"));
                tblOTEstado.Columns.Add("FechaCambioEstado", Type.GetType("System.DateTime"));
                tblOTEstado.Columns.Add("Observacion", Type.GetType("System.String"));
                tblOTEstado.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblOTEstado.Columns.Add("Nuevo", Type.GetType("System.Int32"));
                rbtTodos.IsChecked = true;
                ListarOT();

                dtpfechaPost.EditValue = Convert.ToDateTime(tblFechHoraServ.Rows[0]["FechaServer"]);

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

                //Tabla Actividades temporal
                tblActividadestmp.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblActividadestmp.Columns.Add("IdOTComp", Type.GetType("System.Int32"));
                tblActividadestmp.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblActividadestmp.Columns.Add("PerfilComp", Type.GetType("System.String"));
                tblActividadestmp.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblActividadestmp.Columns.Add("IdActividad", Type.GetType("System.Int32"));
                tblActividadestmp.Columns.Add("Actividad", Type.GetType("System.String"));
                tblActividadestmp.Columns.Add("IsChecked", Type.GetType("System.Boolean"));
                tblActividadestmp.Columns.Add("FlagUso", Type.GetType("System.Boolean"));
                tblActividadestmp.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblActividadestmp.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                tblActividadestmp.Columns.Add("FlagPendiente", Type.GetType("System.Boolean"));

                //Tabla Tareas temporal
                tblTareastmp.Columns.Add("IdOTTarea", Type.GetType("System.Int32"));
                tblTareastmp.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblTareastmp.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblTareastmp.Columns.Add("IdTarea", Type.GetType("System.Int32"));
                tblTareastmp.Columns.Add("OTTarea", Type.GetType("System.String"));
                tblTareastmp.Columns.Add("IdPerfilTarea", Type.GetType("System.Int32"));
                tblTareastmp.Columns.Add("CodResponsable", Type.GetType("System.String"));
                tblTareastmp.Columns.Add("CostoHoraHombre", Type.GetType("System.Double"));
                tblTareastmp.Columns.Add("HorasEstimada", Type.GetType("System.Double"));
                tblTareastmp.Columns.Add("HorasReal", Type.GetType("System.Double"));
                tblTareastmp.Columns.Add("IdEstadoOTT", Type.GetType("System.Int32"));
                tblTareastmp.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblTareastmp.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                //Tabla Detalle Herramienta Especial temporal
                tblHerrEsptmp.Columns.Add("IdOTHerramienta", Type.GetType("System.Int32"));
                tblHerrEsptmp.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblHerrEsptmp.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblHerrEsptmp.Columns.Add("IdHerramienta", Type.GetType("System.Int32"));
                tblHerrEsptmp.Columns.Add("Herramienta", Type.GetType("System.String"));
                tblHerrEsptmp.Columns.Add("Cantidad", Type.GetType("System.Int32"));
                tblHerrEsptmp.Columns.Add("IdEstado", Type.GetType("System.Int32"));
                tblHerrEsptmp.Columns.Add("NroDevolucion", Type.GetType("System.String"));
                tblHerrEsptmp.Columns.Add("FlagActivo", Type.GetType("System.Int32"));
                tblHerrEsptmp.Columns.Add("Nuevo", Type.GetType("System.Int32"));
                //Tabla Detalle Repuesto temporal
                tblRepuestotmp.Columns.Add("IdOTArticulo", Type.GetType("System.Int32"));
                tblRepuestotmp.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblRepuestotmp.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblRepuestotmp.Columns.Add("IdTipoArticulo", Type.GetType("System.Int32"));
                tblRepuestotmp.Columns.Add("IdArticulo", Type.GetType("System.String"));
                tblRepuestotmp.Columns.Add("Articulo", Type.GetType("System.String"));
                tblRepuestotmp.Columns.Add("CantSol", Type.GetType("System.Int32"));
                tblRepuestotmp.Columns.Add("CantEnv", Type.GetType("System.Int32"));
                tblRepuestotmp.Columns.Add("CantUti", Type.GetType("System.Int32"));
                tblRepuestotmp.Columns.Add("CostoArticulo", Type.GetType("System.Double"));
                tblRepuestotmp.Columns.Add("Observacion", Type.GetType("System.String"));
                tblRepuestotmp.Columns.Add("CodResponsable", Type.GetType("System.String"));
                tblRepuestotmp.Columns.Add("FlagActivo", Type.GetType("System.Int32"));
                tblRepuestotmp.Columns.Add("Nuevo", Type.GetType("System.Int32"));
                //Tabla Detalle Consumible
                tblConsumibletmp.Columns.Add("IdOTArticulo", Type.GetType("System.Int32"));
                tblConsumibletmp.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblConsumibletmp.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblConsumibletmp.Columns.Add("IdTipoArticulo", Type.GetType("System.Int32"));
                tblConsumibletmp.Columns.Add("IdArticulo", Type.GetType("System.String"));
                tblConsumibletmp.Columns.Add("Articulo", Type.GetType("System.String"));
                tblConsumibletmp.Columns.Add("CantSol", Type.GetType("System.Int32"));
                tblConsumibletmp.Columns.Add("CantEnv", Type.GetType("System.Int32"));
                tblConsumibletmp.Columns.Add("CantUti", Type.GetType("System.Int32"));
                tblConsumibletmp.Columns.Add("CostoArticulo", Type.GetType("System.Double"));
                tblConsumibletmp.Columns.Add("Observacion", Type.GetType("System.String"));
                tblConsumibletmp.Columns.Add("CodResponsable", Type.GetType("System.String"));
                tblConsumibletmp.Columns.Add("FlagActivo", Type.GetType("System.Int32"));
                tblConsumibletmp.Columns.Add("Nuevo", Type.GetType("System.Int32"));


                tblOTCompTreeList.Columns.Add("Id", Type.GetType("System.Int32"));
                tblOTCompTreeList.Columns.Add("IdPadre", Type.GetType("System.Int32"));
                tblOTCompTreeList.Columns.Add("DescripcionReal", Type.GetType("System.String"));
                tblOTCompTreeList.Columns.Add("Descripcion", Type.GetType("System.String"));
                tblOTCompTreeList.Columns.Add("ActividadRealizada", Type.GetType("System.Boolean"));
                tblOTCompTreeList.Columns.Add("HorasEstimada", Type.GetType("System.Double"));
                tblOTCompTreeList.Columns.Add("HorasReales", Type.GetType("System.Double"));
                tblOTCompTreeList.Columns.Add("IdTipo", Type.GetType("System.Double"));
                tblOTCompTreeList.Columns.Add("IdReal", Type.GetType("System.Double"));
                tblOTCompTreeList.Columns.Add("IsEnabled", Type.GetType("System.Boolean"));
                tblOTCompTreeList.Columns.Add("IdOTComp", Type.GetType("System.Int32"));
                tblOTCompTreeList.Columns.Add("IdActividad", Type.GetType("System.Int32"));

                tbOTTareaTrabajador.Columns.Add("IdOTTareaDetalle", Type.GetType("System.Int32"));
                tbOTTareaTrabajador.Columns.Add("IdOTTarea", Type.GetType("System.Int32"));
                tbOTTareaTrabajador.Columns.Add("CodResponsable", Type.GetType("System.String"));
                tbOTTareaTrabajador.Columns.Add("Trabajador", Type.GetType("System.String"));
                tbOTTareaTrabajador.Columns.Add("CostoHoraHombre", Type.GetType("System.Double"));
                tbOTTareaTrabajador.Columns.Add("Fecha", Type.GetType("System.String"));
                tbOTTareaTrabajador.Columns.Add("HoraInicial", Type.GetType("System.String"));
                tbOTTareaTrabajador.Columns.Add("HoraFinal", Type.GetType("System.String"));
                tbOTTareaTrabajador.Columns.Add("HoraReal", Type.GetType("System.Double"));
                tbOTTareaTrabajador.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tbOTTareaTrabajador.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                tblHerrEspTarea.Columns.Add("IdOTComp", Type.GetType("System.Int32"));
                tblHerrEspTarea.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblHerrEspTarea.Columns.Add("IdOTHerramienta", Type.GetType("System.Int32"));
                tblHerrEspTarea.Columns.Add("IdArticulo", Type.GetType("System.Int32"));
                tblHerrEspTarea.Columns.Add("Codigo", Type.GetType("System.String"));
                tblHerrEspTarea.Columns.Add("Articulo", Type.GetType("System.String"));
                tblHerrEspTarea.Columns.Add("Cantidad", Type.GetType("System.Int32"));
                tblHerrEspTarea.Columns.Add("IdEstado", Type.GetType("System.Int32"));
                tblHerrEspTarea.Columns.Add("Estado", Type.GetType("System.String"));
                tblHerrEspTarea.Columns.Add("NroDevolucion", Type.GetType("System.String"));

                tblArticuloTarea.Columns.Add("IdOTComp", Type.GetType("System.Int32"));
                tblArticuloTarea.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblArticuloTarea.Columns.Add("IdOTArticulo", Type.GetType("System.Int32"));
                tblArticuloTarea.Columns.Add("IdTipoArticulo", Type.GetType("System.Int32"));
                tblArticuloTarea.Columns.Add("IdArticulo", Type.GetType("System.String"));
                tblArticuloTarea.Columns.Add("Articulo", Type.GetType("System.String"));
                tblArticuloTarea.Columns.Add("CantSol", Type.GetType("System.Int32"));
                tblArticuloTarea.Columns.Add("CantEnv", Type.GetType("System.Int32"));
                tblArticuloTarea.Columns.Add("CantUti", Type.GetType("System.Int32"));
                tblArticuloTarea.Columns.Add("NroSolicitudTransferencia", Type.GetType("System.Int32"));
                tblArticuloTarea.Columns.Add("CostoArticulo", Type.GetType("System.Double"));
                tblArticuloTarea.Columns.Add("Observacion", Type.GetType("System.String"));
                tblArticuloTarea.Columns.Add("CodResponsable", Type.GetType("System.String"));
                tblArticuloTarea.Columns.Add("Responsable", Type.GetType("System.String"));
                tblArticuloTarea.Columns.Add("Tipo", Type.GetType("System.String"));
                tblArticuloTarea.Columns.Add("NroSerie", Type.GetType("System.String"));
                tblArticuloTarea.Columns.Add("Frecuencia", Type.GetType("System.Double"));
                tblArticuloTarea.Columns.Add("FrecuenciaTie", Type.GetType("System.Double"));
                tblArticuloTarea.Columns.Add("FlagExtendida", Type.GetType("System.Boolean"));

                //Tabla RIOTComp
                tblRIOTComp.Columns.Add("IdOTComp", Type.GetType("System.Int32"));
                tblRIOTComp.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
                tblRIOTComp.Columns.Add("IdOT", Type.GetType("System.Int32"));
                tblRIOTComp.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblRIOTComp.Columns.Add("IdPerfilCompPadre", Type.GetType("System.Int32"));
                tblRIOTComp.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblRIOTComp.Columns.Add("PerfilComp", Type.GetType("System.String"));
                tblRIOTComp.Columns.Add("IdTipoDetalle", Type.GetType("System.Int32"));
                tblRIOTComp.Columns.Add("IdItem", Type.GetType("System.Int32"));
                tblRIOTComp.Columns.Add("NroSerie", Type.GetType("System.String"));
                tblRIOTComp.Columns.Add("CodigoSAP", Type.GetType("System.String"));
                tblRIOTComp.Columns.Add("DescripcionSAP", Type.GetType("System.String"));
                tblRIOTComp.Columns.Add("IdEstadoOTComp", Type.GetType("System.Int32"));
                tblRIOTComp.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblRIOTComp.Columns.Add("IsChecked", Type.GetType("System.Boolean"));
                tblRIOTComp.Columns.Add("IsEnabled", Type.GetType("System.Boolean"));
                tblRIOTComp.Columns.Add("IsActividad", Type.GetType("System.Boolean"));
                tblRIOTComp.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                //Tabla RIActividades
                tblRIActividades.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                tblRIActividades.Columns.Add("IdOTComp", Type.GetType("System.Int32"));
                tblRIActividades.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblRIActividades.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblRIActividades.Columns.Add("IdActividad", Type.GetType("System.Int32"));
                tblRIActividades.Columns.Add("Actividad", Type.GetType("System.String"));
                tblRIActividades.Columns.Add("IsChecked", Type.GetType("System.Boolean"));
                tblRIActividades.Columns.Add("FlagUso", Type.GetType("System.Boolean"));
                tblRIActividades.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblRIActividades.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                //Tabla Articulo Solicitud
                tblOTArticuloSol.Columns.Add("IdOTArticulo", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroSolTransfer", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroLinSolTransfer", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroSalMercancia", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroLinSalMercancia", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroSolDevolucion", Type.GetType("System.Int32"));
                tblOTArticuloSol.Columns.Add("NroLinSolDevolucionr", Type.GetType("System.Int32"));
                #region COSTO_ARTICULO_SALIDA
                tblOTArticuloSol.Columns.Add("CostoArticulo", Type.GetType("System.Double"));
                #endregion


                tblNroSeriesAsignadas = new DataTable();
                tblNroSeriesAsignadas.Columns.Add("IdOT", Type.GetType("System.Int32"));
                tblNroSeriesAsignadas.Columns.Add("IdHerramientaItem", Type.GetType("System.Int32"));
                tblNroSeriesAsignadas.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                //Cargar combo
                objE_TablaMaestra = new E_TablaMaestra();
                objE_TablaMaestra.IdTabla = 18; //Tabla Maestra: Estado OT
                CboEstado.ItemsSource = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
                CboEstado.DisplayMember = "Descripcion";
                CboEstado.ValueMember = "IdColumna";

                objE_TablaMaestra = new E_TablaMaestra();
                objE_TablaMaestra.IdTabla = 20; //Tabla Maestra: Tipo de Orden
                CboOrden.ItemsSource = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
                CboOrden.DisplayMember = "Descripcion";
                CboOrden.ValueMember = "IdColumna";

                objE_TablaMaestra = new E_TablaMaestra();
                objE_TablaMaestra.IdTabla = 21; //Tabla Maestra: Tipo de generación
                CboGeneracion.ItemsSource = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
                CboGeneracion.DisplayMember = "Descripcion";
                CboGeneracion.ValueMember = "IdColumna";

                objE_TablaMaestra = new E_TablaMaestra();
                objE_TablaMaestra.IdTabla = 37; //Tabla Maestra: Tipo de generación
                cboEstadoHerrEspTarea.ItemsSource = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
                cboEstadoHerrEspTarea.DisplayMember = "Descripcion";
                cboEstadoHerrEspTarea.ValueMember = "IdColumna";

                //Lista Responsable
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("R", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    CboResponsable.ItemsSource = OHEMlist;
                    CboResponsable.ValueMember = "CodigoPersona";
                    CboResponsable.DisplayMember = "NombrePersona";

                    cboTrabajador.ItemsSource = OHEMlist;
                    cboTrabajador.ValueMember = "CodigoPersona";
                    cboTrabajador.DisplayMember = "NombrePersona";

                    cboResponsableTarea.ItemsSource = OHEMlist;
                    cboResponsableTarea.ValueMember = "CodigoPersona";
                    cboResponsableTarea.DisplayMember = "NombrePersona";
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                //Listar Perfil
                tblPerfil = objPerfil.Perfil_Combo();
                CboPerfil.ItemsSource = tblPerfil;
                CboPerfil.DisplayMember = "Perfil";
                CboPerfil.ValueMember = "IdPerfil";

                //Llenar Combo Detalles
                cboHerrEsp.ItemsSource = objHerramienta.Herramienta_Combo();
                cboHerrEsp.DisplayMember = "Herramienta";
                cboHerrEsp.ValueMember = "IdHerramienta";

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                BEOITMList = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("C", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    cboConsumible.ItemsSource = BEOITMList;
                    cboConsumible.DisplayMember = "DescripcionArticulo";
                    cboConsumible.ValueMember = "CodigoArticulo";
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                BEOITMList = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("P", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    cboRepuesto.ItemsSource = BEOITMList;
                    cboRepuesto.DisplayMember = "DescripcionArticulo";
                    cboRepuesto.ValueMember = "CodigoArticulo";
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                OCRDList = InterfazMTTO.iSBO_BL.SocioNegocio_BL.ConsultaProveedor("Y", ref RPTA);

                if (RPTA.ResultadoRetorno == 0)
                {
                    cboRIProveedor.ItemsSource = OCRDList;
                    cboRIProveedor.ValueMember = "CodigoProveedor";
                    cboRIProveedor.DisplayMember = "DescripcionProveedor";
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                tblActividadCombo = objActividad.Actividad_Combo();
                cboActividad.ItemsSource = tblActividadCombo;
                cboActividad.DisplayMember = "Actividad";
                cboActividad.ValueMember = "IdActividad";

                //#region VisualizacionBotonImprimir
                //bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                //if (!VisualizaBotonImprimirDetalle)
                //{
                btnImprimir.Visibility = Visibility.Hidden;
                //}
                //#endregion

                GlobalClass.ip.SeleccionarTab(tabListadoOT);
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void BtnCambiarEstado_Click(object sender, RoutedEventArgs e)
        {
            if (dtgOT.VisibleRowCount == 0) { return; }
            dtgCambioEstado.ItemsSource = null;
            DataTable tblTempPost = new DataTable();
            tblTempPost.Columns.Add("CodOT");
            tblTempPost.Columns.Add("NombreOT");
            tblTempPost.Columns.Add("FechaProg");
            tblTempPost.Columns.Add("Observacion");
            FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();

            int contador = 0;
            if (!(RdnPostergar.IsChecked == true || RdnDetener.IsChecked == true || RdnLiberar.IsChecked == true || RdnProgramado.IsChecked == true))
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_CAMB_ESTA"), 2);
                return;
            }

            tblOT.Rows.Clear();
            for (int i = 0; i < tblTempOT.Rows.Count; i++)
            {
                if (Convert.ToBoolean(tblTempOT.Rows[i][" "]) == true)
                {

                    //Validación de estados
                    //Postergación
                    if (Convert.ToInt32(tblTempOT.Rows[i]["IdEstadoOT"].ToString()) != 1 && Convert.ToInt32(tblTempOT.Rows[i]["IdEstadoOT"].ToString()) != 2 && RdnPostergar.IsChecked == true)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_POST_OT"), 2);
                        tblOT.Rows.Clear();
                        return;
                    }
                    //Liberación
                    if (Convert.ToInt32(tblTempOT.Rows[i]["IdEstadoOT"].ToString()) != 1 && Convert.ToInt32(tblTempOT.Rows[i]["IdEstadoOT"].ToString()) != 2 && Convert.ToInt32(tblTempOT.Rows[i]["IdEstadoOT"].ToString()) != 3 && RdnLiberar.IsChecked == true)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_LIBE_OT"), 2);
                        tblOT.Rows.Clear();
                        return;
                    }
                    //DateTime fechaserver = Convert.ToDateTime(GlobalClass.ip.lblFechaHora.Text.Substring(0, 10) + " " + GlobalClass.ip.lblFechaHora.Text.Substring(13, 10)); //Error Cuando el Label es ""
                    DateTime FechaServidor = Convert.ToDateTime(Utilitarios.Utilitarios.Fecha_Hora_Servidor().Rows[0]["FechaServer"].ToString() + " " + Utilitarios.Utilitarios.Fecha_Hora_Servidor().Rows[0]["HoraServer"].ToString());
                    DateTime FechaProg = Convert.ToDateTime(tblTempOT.Rows[i]["FechaProg"]);
                    if (RdnLiberar.IsChecked == true && DateTime.Compare(FechaProg, FechaServidor) > 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_FECH_PROG"), 2);
                        tblOT.Rows.Clear();
                        return;
                    }

                    //Detener
                    if (Convert.ToInt32(tblTempOT.Rows[i]["IdEstadoOT"].ToString()) != 4 && RdnDetener.IsChecked == true)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_DETE_OT"), 2);
                        tblOT.Rows.Clear();
                        return;
                    }

                    //Cancelar
                    if (Convert.ToInt32(tblTempOT.Rows[i]["IdEstadoOT"].ToString()) != 1 && Convert.ToInt32(tblTempOT.Rows[i]["IdEstadoOT"].ToString()) != 2 && RdnProgramado.IsChecked == true)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_CANC_OT"), 2);
                        tblOT.Rows.Clear();
                        return;
                    }

                    DataRow rowOT1 = tblOT.NewRow();
                    rowOT1["IdOT"] = Convert.ToInt32(tblTempOT.Rows[i]["IdOT"].ToString());//
                    rowOT1["CodOT"] = tblTempOT.Rows[i]["CodOT"].ToString();
                    rowOT1["NombreOT"] = tblTempOT.Rows[i]["NombreOT"].ToString();
                    rowOT1["IdTipoOT"] = Convert.ToInt32(tblTempOT.Rows[i]["IdTipoOT"].ToString());//
                    rowOT1["FlagSinUC"] = Convert.ToBoolean(tblTempOT.Rows[i]["FlagSinUC"]);//
                    if (tblTempOT.Rows[i]["IdUC"] == DBNull.Value || tblTempOT.Rows[i]["IdUC"] == null || Convert.ToString(tblTempOT.Rows[i]["IdUC"]) == "")
                    {
                        rowOT1["IdUC"] = 0;
                    }
                    else
                    {
                        rowOT1["IdUC"] = Convert.ToInt32(tblTempOT.Rows[i]["IdUC"].ToString());

                    }//
                    rowOT1["EstadoOT"] = tblTempOT.Rows[i]["EstadoOT"].ToString();
                    rowOT1["FechaProg"] = Convert.ToDateTime(tblTempOT.Rows[i]["FechaProg"]);
                    //rowOT1["FechaLiber"] = Convert.ToDateTime(tblTempOT.Rows[i]["FechaLiber"].ToString());
                    //rowOT1["FechaCierre"] = Convert.ToDateTime(tblTempOT.Rows[i]["FechaCierre"].ToString());
                    rowOT1["CodResponsable"] = gintCodResponsable;//
                    rowOT1["NombreResponsable"] = gstrNombResponsable;//
                    rowOT1["IdTipoGeneracion"] = Convert.ToInt32(tblTempOT.Rows[i]["IdTipoGeneracion"].ToString());//
                    rowOT1["IdEstadoOT"] = Convert.ToInt32(tblTempOT.Rows[i]["IdEstadoOT"].ToString());//
                    rowOT1["MotivoPostergacion"] = tblTempOT.Rows[i]["MotivoPostergacion"].ToString();
                    rowOT1["Observacion"] = tblTempOT.Rows[i]["Observacion"].ToString();
                    rowOT1["FlagActivo"] = Convert.ToBoolean(tblTempOT.Rows[i]["FlagActivo"]);//
                    rowOT1["Nuevo"] = 0;
                    tblOT.Rows.Add(rowOT1);
                    contador++;
                    if (RdnPostergar.IsChecked == true && Convert.ToInt32(tblTempOT.Rows[i]["IdEstadoOT"]) == 4)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_POST_OTLI"), 2);
                        return;
                    }
                }
            }

            if (contador != 0)
            {
                if (RdnPostergar.IsChecked == true)
                {
                    stkPostergacion.Visibility = Visibility.Visible;
                    dtgPostergacion.ItemsSource = tblOT.DefaultView;

                }
                else if (RdnDetener.IsChecked == true || RdnLiberar.IsChecked == true || RdnProgramado.IsChecked == true)
                {
                    stkCambioEstado.Visibility = Visibility.Visible;
                    dtgCambioEstado.ItemsSource = tblOT.DefaultView;

                    if (RdnDetener.IsChecked == true)
                    {
                        gintEstadoOT = gintEstaDete;
                        lblNuevoEstado.Content = "Detenido";
                        lblTituloEstado.Content = "Cambiar estado a : Detenido";
                    }
                    else if (RdnLiberar.IsChecked == true)
                    {
                        gintEstadoOT = gintEstaLibe;
                        lblNuevoEstado.Content = "Liberado";
                        lblTituloEstado.Content = "Cambiar estado a : Liberado";
                    }
                    else if (RdnProgramado.IsChecked == true)
                    {
                        gintEstadoOT = gintEstaProg;
                        lblNuevoEstado.Content = "Cancelado";
                        lblTituloEstado.Content = "Cambiar estado a : Cancelado";
                    }

                    tblOTEstado.Rows.Clear();
                    for (int i = 0; i < tblTempOT.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(tblTempOT.Rows[i][" "]) == true)
                        {
                            rowOTEstado = tblOTEstado.NewRow();
                            rowOTEstado["IdOTEstado"] = 1;
                            rowOTEstado["IdOT"] = tblTempOT.Rows[i]["IdOT"];
                            rowOTEstado["IdEstadoInicial"] = tblTempOT.Rows[i]["IdEstadoOT"];
                            rowOTEstado["IdEstadoFinal"] = gintEstadoOT;
                            rowOTEstado["FechaCambioEstado"] = tblFechHoraServ.Rows[0]["FechaServer"];
                            rowOTEstado["Observacion"] = txtObservacionCambioEstado.Text;
                            rowOTEstado["FlagActivo"] = tblTempOT.Rows[i]["FlagActivo"];
                            rowOTEstado["Nuevo"] = 1;
                            tblOTEstado.Rows.Add(rowOTEstado);
                        }
                    }
                    //dtgCambioEstado.ItemsSource = tblOTEstado.DefaultView;
                    stkCambioEstado.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_CAMB_CHEC"), 2);
            }

        }
        private void btnAbrirCambiarEstado_Click(object sender, RoutedEventArgs e)
        {
            txtObservacionCambioEstado.Text = string.Empty;
            stkRegistroCambioEstado.Visibility = System.Windows.Visibility.Visible;
        }
        private void btnAceptarCambioEstado_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objE_OT.Observacion = txtObservacionCambioEstado.Text;
                objE_OT.IdUsuario = gintIdUsuario;
                objE_OT.IdEstadoOT = gintEstadoOT;
                objE_OT.IdHerramientaItems = gstrIdHerramientas;
                objE_OT.FechaModificacion = FechaModificacion;
                if (txtObservacionCambioEstado.Text.Length < 13)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_CAMB_COME"), 2);
                    dtpfechaPost.Focus();
                    return;
                }

                if (gintEstadoOT != 0)
                {
                    if (gbolRegHerramientas)
                    {
                        tblOT.Columns.Remove("EstadoOT");
                        int rpta = objB_OT.OTEstado_UpdateCascada(objE_OT, tblOT, tblOTEstado, tblNroSeriesAsignadas);
                        tblOT.Columns.Add("EstadoOT", Type.GetType("System.String"));
                        if (rpta == 1)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_ESTA"), 1);
                        }
                        else if (rpta == 0)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_MODI"), 2);
                            return;
                        }
                        else if (rpta == 1205)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_CONC"), 2);
                            return;
                        }

                        //GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_ESTA"), 1);
                        gbolRegHerramientas = false;
                        stkRegistroCambioEstado.Visibility = Visibility.Hidden;
                        stkCambioEstado.Visibility = Visibility.Hidden;
                        tblOTPost.Clear();
                        tblOT.Clear();
                        tbl.Clear();
                        tblNroSeriesAsignadas.Clear();
                        ListarOT();
                    }
                    else
                    {
                        objE_OT.IdOT = Convert.ToInt32(tblOT.Rows[0]["IdOT"]);

                        tblHerramientaDatosCambioEstado = objB_OT.OTHerramienta_Combo(objE_OT);
                        if ((bool)RdnLiberar.IsChecked)
                        {
                            //Abrir PopUp de Seleccion de Series de Herramientas Especiales
                            string IdOT = "";
                            bool stockHerramientas = true;
                            for (int i = 0; i < tblOT.Rows.Count; i++)
                            {
                                IdOT += tblOT.Rows[i]["IdOT"].ToString() + "|";
                            }

                            IdOT = IdOT.Substring(0, IdOT.Length - 1);
                            tblNroSerie = new DataTable();
                            tblNroSerie = objB_OT.OTHerramienta_GetTreeVieNrSeries(IdOT);

                            if (tblNroSerie.Rows.Count > 0 && stockHerramientas)
                            {
                                dtgNroSerie.ItemsSource = tblNroSerie;

                                dtgNroSerie.Columns["IdHerramientaItem"].Visible = false;
                                dtgNroSerie.Columns["IdOT"].Visible = false;
                                dtgNroSerie.Columns["IdOTCompActividad"].Visible = false;
                                dtgNroSerie.Columns["CantidadDis"].Visible = false;
                                dtgNroSerie.Columns["IsChecked"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                                dtgNroSerie.Columns["IsChecked"].VisibleIndex = tblNroSerie.Columns.Count - 1;
                                dtgNroSerie.Columns["CantidadSol"].Header = "Cantidad Solicitada";
                                dtgNroSerie.Columns["Codigo"].Header = "Código OT";
                                dtgNroSerie.GroupBy("Codigo");
                                dtgNroSerie.GroupBy("Herramienta");
                                dtgNroSerie.GroupBy("Actividad");
                                dtgNroSerie.GroupBy("CantidadSol");

                                dtgNroSerie.ExpandAllGroups();

                                stkHerramientasSeries.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                var rpt = DevExpress.Xpf.Core.DXMessageBox.Show(string.Format("No se encontró stock en las herramientas especiales o la OT ya fue liberada anteriormente \n¿Desea continuar?"), "Asignación de Herramientas Especiales", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (rpt == MessageBoxResult.Yes)
                                {
                                    gbolRegHerramientas = true;
                                    btnAceptarCambioEstado_Click(null, null);
                                }
                            }
                        }
                        else
                        {
                            gbolRegHerramientas = true;
                            btnAceptarCambioEstado_Click(null, null);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }

        }
        private void btnCancelarCambioEstado_Click(object sender, RoutedEventArgs e)
        {
            stkCambioEstado.Visibility = Visibility.Hidden;
            stkRegistroCambioEstado.Visibility = Visibility.Hidden;
        }
        private void btnCancelarRegistroCambioEstado_Click(object sender, RoutedEventArgs e)
        {
            stkRegistroCambioEstado.Visibility = Visibility.Hidden;
        }
        private void btnAbrirPostergar_Click(object sender, RoutedEventArgs e)
        {
            txtObservacionPostergacion.Text = string.Empty;
            dtpfechaPost.EditValue = Convert.ToDateTime(tblFechHoraServ.Rows[0]["FechaServer"]);
            stkRegistroPostergacion.Visibility = Visibility.Visible;
        }
        private void btnAceptarPostergacion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objE_OT.FechaReprog = Convert.ToDateTime(dtpfechaPost.EditValue);
                objE_OT.Observacion = txtObservacionPostergacion.Text;
                objE_OT.IdUsuario = gintIdUsuario;
                if (objE_OT.FechaReprog < Convert.ToDateTime(tblFechHoraServ.Rows[0]["FechaServer"]))
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_FECH_POST"), 2);
                    dtpfechaPost.Focus();
                    return;
                }

                if (txtObservacionPostergacion.Text.Length < 13)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_CAMB_COME"), 2);
                    txtObservacionPostergacion.Focus();
                    return;
                }

                tblOTPost.Rows.Clear();
                for (int i = 0; i < tblOT.Rows.Count; i++)
                {
                    DataRow rowOTPost = tblOTPost.NewRow();
                    rowOTPost["IdOTReprog"] = 0;
                    rowOTPost["CodOTReprog"] = 0;
                    rowOTPost["IdOT"] = Convert.ToInt32(tblOT.Rows[i]["IdOT"].ToString());
                    rowOTPost["FechaReprog"] = objE_OT.FechaReprog;
                    rowOTPost["FechaProg"] = Convert.ToDateTime(tblOT.Rows[i]["FechaProg"].ToString());
                    rowOTPost["Observacion"] = objE_OT.Observacion;
                    rowOTPost["FlagActivo"] = Convert.ToBoolean(tblOT.Rows[i]["FlagActivo"]);
                    rowOTPost["Nuevo"] = 1;
                    tblOTPost.Rows.Add(rowOTPost);

                    //objE_OT.IdOT = Convert.ToInt32(tblOT.Rows[i]["IdOT"].ToString());
                    //DataTable tblNroSoli = objB_OT.OTArticulo_GetNroSolByOT(objE_OT);
                    //for (int a = 0; a < tblNroSoli.Rows.Count; a++)
                    //{
                    //    OWTQ.NroSolicitudTransferencia = Convert.ToInt32(tblNroSoli.Rows[a]["NroSolTransfer"]);
                    //    OWTQ.FechaSolicitud = Convert.ToDateTime(dtpfechaPost.EditValue);
                    //    OWTQ.Estado = "U";
                    //    OWTQ.Comentarios = txtObservacionPostergacion.Text;
                    //    RPTA = InterfazMTTO.iSBO_BL.SolicitudTransferencia_BL.ActualizarSolicitudTransferencia(OWTQ);
                    //    if (RPTA.ResultadoRetorno != 0)
                    //    {
                    //        GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                    //        return;
                    //    }
                    //}
                }

                tblOT.Columns.Remove("EstadoOT");
                objE_OT.FechaModificacion = FechaModificacion;
                int rpta = objB_OT.OT_UpdateCascada(objE_OT, tblOT, tblOTPost);
                tblOT.Columns.Add("EstadoOT", Type.GetType("System.String"));
                if (rpta == 1)
                {
                    for (int i = 0; i < tblOT.Rows.Count; i++)
                    {
                        objE_OT.IdOT = Convert.ToInt32(tblOT.Rows[i]["IdOT"].ToString());
                        DataTable tblNroSoli = objB_OT.OTArticulo_GetNroSolByOT(objE_OT);
                        for (int a = 0; a < tblNroSoli.Rows.Count; a++)
                        {
                            OWTQ.NroSolicitudTransferencia = Convert.ToInt32(tblNroSoli.Rows[a]["NroSolTransfer"]);
                            OWTQ.FechaSolicitud = Convert.ToDateTime(dtpfechaPost.EditValue);
                            OWTQ.Estado = "U";
                            OWTQ.Comentarios = txtObservacionPostergacion.Text;
                            RPTA = InterfazMTTO.iSBO_BL.SolicitudTransferencia_BL.ActualizarSolicitudTransferencia(OWTQ);
                            if (RPTA.ResultadoRetorno != 0)
                            {
                                GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                            }
                        }
                    }
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_POST"), 1);
                }
                else if (rpta == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_MODI"), 2);
                    return;
                }
                else if (rpta == 1205)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_CONC"), 2);
                    return;
                }

                dtpfechaPost.EditValue = Convert.ToDateTime(tblFechHoraServ.Rows[0]["FechaServer"]);
                stkRegistroPostergacion.Visibility = Visibility.Hidden;
                stkPostergacion.Visibility = Visibility.Hidden;

                tblOTPost.Clear();
                tblOT.Clear();
                tbl.Clear();
                ListarOT();
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void btnCancelarPostergcion_Click(object sender, RoutedEventArgs e)
        {
            stkPostergacion.Visibility = Visibility.Hidden;
            stkRegistroPostergacion.Visibility = Visibility.Hidden;
        }
        private void btnCancelarRegistroPostergacion_Click(object sender, RoutedEventArgs e)
        {
            stkRegistroPostergacion.Visibility = Visibility.Hidden;

        }
        private void BtnRegistrarOT_Click(object sender, RoutedEventArgs e)
        {
            IdPerfilCompMax = 1000 + objB_OT.PerfilCompActividad_Max();
            LimpiarForm();
            GlobalClass.ip.SeleccionarTab(tabDetallesOT);
            //tabControl1.SelectedIndex = 1; //Tab Javier
            tabControl5.SelectedIndex = 0;
            tabControl2.SelectedIndex = 0;
            IdOT = 0;
            IdPerfilCompActividad = 0;
            IdOTCompActividad = 0;
            //tabItem3.IsEnabled = true; //Tab Javier
            BtnGrabarOT.Content = "Registrar";
            lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
            lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
        }
        private void BtnRegistrarTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgOT.VisibleRowCount == 0) { return; }
                int IdTipoOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdTipoOT"));
                int IdEstadoOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdEstadoOT"));

                if (IdTipoOT != 1 && IdTipoOT != 3)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_ESTA"), 2);
                    return;
                }
                else if (IdEstadoOT != 3 && IdEstadoOT != 4)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_ESTA"), 2);
                    return;
                }
                DesBloquearControlRegistroTarea();
                ListarDatosRegistraTarea();
                FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                GlobalClass.ip.SeleccionarTab(tabTareasOT);
                //tabControl1.SelectedIndex = 2; //Tab Javier
                //tabItem9.IsEnabled = true; //Tab Javier
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void BtnRegistrarTransferencia_Click(object sender, RoutedEventArgs e)
        {

            DevExpress.Xpf.Core.DXMessageBox.Show("Orden de Trabajo Registrada.");
            GlobalClass.ip.SeleccionarTab(tabListadoOT);
            //tabControl1.SelectedIndex = 0; //Tab Javier

        }
        void CambiarBotonesRegistroPRoveedor(bool bloquear)
        {
            //treeListView2.AllowEditing = !bloquear;
            //txtRIComentarios.IsReadOnly = bloquear;
            cboRIProveedor.IsReadOnly = bloquear;
            cboRIORdenCompra.IsReadOnly = bloquear;
        }
        private void BtnRegistrarInforme_Click(object sender, RoutedEventArgs e)
        {
            if (dtgOT.VisibleRowCount == 0) { return; }
            int IdTipoOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdTipoOT"));
            int IdEstadoOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdEstadoOT"));
            int IdOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdOT"));
            if (IdTipoOT != 2 && IdTipoOT != 3)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_IPRO_TIPO"), 2);
                return;
            }
            else if (IdEstadoOT != 3 && IdEstadoOT != 4)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_IPRO_ESTA"), 2);
                return;
            }
            FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
            //Verificar si tiene OTInformes
            objE_OTIProv.IdOT = IdOT;
            tblOTInforme = objB_OTIProv.OTInforme_List(objE_OTIProv);
            int CantExiste = tblOTInforme.Select("IdOT = " + IdOT).Length;
            if (CantExiste > 0)
            {
                EstadoForm(false, true, true);
                //tbRegInfo.IsEnabled = true; //Tab Javier
                //btnCargarDoc.Visibility = Visibility.Hidden;
                txbLinkCarga.Visibility = Visibility.Hidden;
                txbLinkDescarga.Visibility = Visibility.Visible;

                //btnDescDoc.Visibility = Visibility.Visible;

                LlenarConsultaRegInforme();
                CambiarBotonesRegistroPRoveedor(true);
                GlobalClass.ip.SeleccionarTab(tabInformesOT);
                ActualizarEstadoOTRIP.IsEnabled = (Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdEstadoOT")) != 5);
                //tabControl1.SelectedIndex = 3; //Tab Javier
            }
            else
            {
                lblAuditoria_creacionRP.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                lblAuditoria_modificacionRP.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                CambiarBotonesRegistroPRoveedor(false);
                EstadoForm(true, false, true);
                ActualizarEstadoOTRIP.IsEnabled = false;
                //tbRegInfo.IsEnabled = true; //Tab Javier
                LlenarDetallesRegInforme();
                GlobalClass.ip.SeleccionarTab(tabInformesOT);
                //tabControl1.SelectedIndex = 3; //Tab Javier
            }


        }
        private void BtnRegistrarInformeP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gdblFileSize > gdblMaxFileSize)
                {
                    GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_IPRO_TAMA_ARCH"), gdblMaxFileSize), 2);
                    return;
                }
                if (gbolNuevo == true && gbolEdicion == false)
                {
                    tblRIActividades.Rows.Clear();
                    if (ValidaCampoObligadoInformeProveedor() == true) { return; }
                    objE_OTIProv.IdOTInforme = 0;
                    objE_OTIProv.IdOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdOT")); ;
                    objE_OTIProv.NombreFile = lblRIUbicacion.Content.ToString();
                    objE_OTIProv.RUCProv = lblRIRuc.Content.ToString();
                    objE_OTIProv.CodProveedor = cboRIProveedor.EditValue.ToString();
                    objE_OTIProv.RazonSocialProv = cboRIProveedor.Text;
                    objE_OTIProv.NroOCProv = cboRIORdenCompra.Text;
                    objE_OTIProv.Costo = gdblCostoTotal;
                    objE_OTIProv.Observacion = txtRIComentarios.Text;
                    objE_OTIProv.FlagActivo = true;
                    objE_OTIProv.IdUsuario = gintIdUsuario;
                    objE_OTIProv.IdEstadoOT = 5;
                    objE_OTIProv.FechaModificacion = FechaModificacion;
                    foreach (DataRow drActiv in tblRIOTComp.Select("IsActividad = true AND IsChecked = true"))
                    {
                        DataRow dr;
                        dr = tblRIActividades.NewRow();
                        dr["IdOTCompActividad"] = Convert.ToInt32(drActiv["IdOTCompActividad"]);
                        dr["IdOTComp"] = Convert.ToInt32(drActiv["IdOTComp"]);
                        dr["IsChecked"] = Convert.ToBoolean(drActiv["IsChecked"]);
                        dr["Nuevo"] = false;
                        tblRIActividades.Rows.Add(dr);
                    }

                    if (tblRIActividades.Rows.Count == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_IPRO_ACTI"), 2);
                        return;
                    }

                    if (lblRIUbicacion.Content.ToString() != "")
                    {
                        try
                        {
                            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(targetPath);
                            System.Security.AccessControl.DirectorySecurity acl = di.GetAccessControl();
                            System.Security.AccessControl.AuthorizationRuleCollection rules = acl.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
                            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(currentUser);
                            //Comentado Por que no hay ActiveDirectory
                            //foreach (System.Security.AccessControl.AuthorizationRule rule in rules)
                            //{
                            //    System.Security.AccessControl.FileSystemAccessRule fsAccessRule = rule as System.Security.AccessControl.FileSystemAccessRule;
                            //    if (fsAccessRule == null) { continue; }
                            //    if ((fsAccessRule.FileSystemRights & System.Security.AccessControl.FileSystemRights.WriteData) > 0)
                            //    {
                            //        System.Security.Principal.NTAccount ntAccount = rule.IdentityReference as System.Security.Principal.NTAccount;
                            //        if (ntAccount == null) { continue; }
                            //        if (principal.IsInRole(ntAccount.Value)) { break; }
                            //        else
                            //        {
                            //            GlobalClass.ip.Mensaje("El usuario no tiene permisos de escritura", 2);
                            //            return;
                            //        }
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                            GlobalClass.ip.Mensaje("Existe un problema en la red, pongase en contacto con el administrador", 3);
                            return;
                        }

                        string pathString = System.IO.Path.Combine(targetPath, txtRICodOT.Text);
                        string destFile = System.IO.Path.Combine(pathString, fileName);
                        bool existe = System.IO.Directory.Exists(pathString);
                        if (!existe)
                        {
                            System.IO.Directory.CreateDirectory(pathString);
                        }

                        System.IO.File.Copy(sourceFile, destFile, true);
                    }

                    int rpta = objB_OTIProv.OTInforme_UpdateCascade(objE_OTIProv, tblRIActividades);
                    if (rpta == 1)
                    {
                        RPTA = InterfazMTTO.iSBO_BL.OrdenCompra_BL.ActualizaOrdenesCompra(Convert.ToInt32(cboRIORdenCompra.Text), txtRICodOT.Text);
                        if (RPTA.CodigoErrorUsuario != "000")
                        {
                            GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                        }
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_IPRO"), 1);
                    }
                    else if (rpta == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_CONC"), 2);
                        return;
                    }
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    tblRIActividades.Rows.Clear();
                    //if (ValidaCampoObligadoInformeProveedor() == true) { return; }
                    objE_OTIProv.IdOTInforme = Convert.ToInt32(tblOTInforme.Rows[0]["IdOTInforme"]);
                    objE_OTIProv.IdOT = Convert.ToInt32(tblOTInforme.Rows[0]["IdOT"]);
                    if (gbolNuevoDocumento)
                    {
                        objE_OTIProv.NombreFile = lblRIUbicacion.Content.ToString();
                    }
                    else
                    {
                        objE_OTIProv.NombreFile = lblRIUbicacionDescarga.Content.ToString();
                    }
                    objE_OTIProv.RUCProv = "";
                    objE_OTIProv.CodProveedor = "";
                    objE_OTIProv.RazonSocialProv = "";
                    objE_OTIProv.NroOCProv = "";
                    objE_OTIProv.Costo = 0;
                    objE_OTIProv.Observacion = txtRIComentarios.Text;
                    objE_OTIProv.FlagActivo = true;
                    objE_OTIProv.IdUsuario = gintIdUsuario;
                    objE_OTIProv.IdEstadoOT = 5;
                    objE_OTIProv.FechaModificacion = FechaModificacion;

                    int ExisteActChecked = tblRIOTComp.Select("IsChecked = true").Length;
                    if (ExisteActChecked == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_IPRO_ACTI"), 2);
                        return;
                    }

                    foreach (DataRow drActiv in tblRIOTComp.Select("IsActividad = true"))
                    {
                        DataRow dr;
                        dr = tblRIActividades.NewRow();
                        dr["IdOTCompActividad"] = Convert.ToInt32(drActiv["IdOTCompActividad"]);
                        dr["IdOTComp"] = Convert.ToInt32(drActiv["IdOTComp"]);
                        dr["IsChecked"] = Convert.ToBoolean(drActiv["IsChecked"]);
                        dr["Nuevo"] = false;
                        tblRIActividades.Rows.Add(dr);
                    }

                    if (lblRIUbicacion.Content.ToString() != "" && gbolNuevoDocumento)
                    {
                        try
                        {
                            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(targetPath);
                            System.Security.AccessControl.DirectorySecurity acl = di.GetAccessControl();
                            System.Security.AccessControl.AuthorizationRuleCollection rules = acl.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
                            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(currentUser);
                            //Comentado Por que no hay ActiveDirectory
                            //foreach (System.Security.AccessControl.AuthorizationRule rule in rules)
                            //{
                            //    System.Security.AccessControl.FileSystemAccessRule fsAccessRule = rule as System.Security.AccessControl.FileSystemAccessRule;
                            //    if (fsAccessRule == null) { continue; }
                            //    if ((fsAccessRule.FileSystemRights & System.Security.AccessControl.FileSystemRights.WriteData) > 0)
                            //    {
                            //        System.Security.Principal.NTAccount ntAccount = rule.IdentityReference as System.Security.Principal.NTAccount;
                            //        if (ntAccount == null) { continue; }
                            //        if (principal.IsInRole(ntAccount.Value)) { break; }
                            //        else
                            //        {
                            //            GlobalClass.ip.Mensaje("El usuario no tiene permisos de escritura", 2);
                            //            return;
                            //        }
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                            GlobalClass.ip.Mensaje("Existe un problema en la red, pongase en contacto con el administrador", 3);
                            return;
                        }

                        string pathString = System.IO.Path.Combine(targetPath, txtRICodOT.Text);
                        string destFile = System.IO.Path.Combine(pathString, fileName);
                        bool existe = System.IO.Directory.Exists(pathString);
                        if (!existe)
                        {
                            System.IO.Directory.CreateDirectory(pathString);
                        }

                        System.IO.File.Copy(sourceFile, destFile, true);
                    }

                    int rpta = objB_OTIProv.OTInforme_UpdateCascade(objE_OTIProv, tblRIActividades);
                    if (rpta == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "ACTU_IPRO"), 1);
                    }
                    else if (rpta == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_CONC"), 2);
                        return;
                    }
                }
                LimpiarDatosInformeProveedor();
                ListarOT();
                GlobalClass.ip.SeleccionarTab(tabListadoOT);
                //tabControl1.SelectedIndex = 0; //Tab Javier
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtObservacion_TextChanged(object sender, TextChangedEventArgs e)
        {
            //txtObservacion.Text = Utilitarios.Utilitarios.SoloAlfanumerico(txtObservacion.Text);
            //txtObservacion.SelectionStart = txtObservacion.Text.Length;
        }
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                e.CanExecute = false;
                e.Handled = true;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void CboPerfil_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                E_UC objEUC = new E_UC();
                B_UC objBUC = new B_UC();
                int IdPerfil = Convert.ToInt32(CboPerfil.EditValue);
                objEUC.IdPerfil = IdPerfil;

                CboUnidadControl.ItemsSource = objBUC.B_UC_Combo(objEUC);
                CboUnidadControl.DisplayMember = "PlacaSerie";
                CboUnidadControl.ValueMember = "CodUC";
                CboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);
                CboUnidadControl.EditValue = -1;
                CboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);
                tabItem4.IsEnabled = false;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void rbtPM_Checked(object sender, RoutedEventArgs e)
        {
            LblSelected.Content = "Seleccionar una actividad...";
            tblActividadestmp.Rows.Clear();
            tblTareastmp.Rows.Clear();
            tblConsumibletmp.Rows.Clear();
            tblRepuestotmp.Rows.Clear();
            tblHerrEsptmp.Rows.Clear();
            CboPerfilAC.Visibility = System.Windows.Visibility.Visible;

            objEPM = new E_PM();
            objEPM.IdPerfil = Convert.ToInt32(CboPerfil.EditValue);
            CboPerfilAC.ItemsSource = objBPM.PM_CombobyPerfil(objEPM);
            CboPerfilAC.DisplayMember = "PM";
            CboPerfilAC.ValueMember = "IdPM";

            lstboxActividad.ItemsSource = null;
            cboTarea.ItemsSource = null;
            CboPerfilAC.SelectedIndex = -1;
            trvComp.ItemsSource = null;
            if (CboUnidadControl.SelectedIndex != -1)
            {
                Utilitarios.TreeViewModelCompOT.LimpiarDatosTreeview();
                Utilitarios.TreeViewModelCompOT.tblListarPerfilComponentes = tblOTComp;
                trvComp.ItemsSource = Utilitarios.TreeViewModelCompOT.CargarDatosTreeViewPerfilComponente(1000, null);
            }

        }
        private void rbtPerfil_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                LblSelected.Content = "Seleccionar una actividad...";
                tblActividadestmp.Rows.Clear();
                tblTareastmp.Rows.Clear();
                tblConsumibletmp.Rows.Clear();
                tblRepuestotmp.Rows.Clear();
                tblHerrEsptmp.Rows.Clear();
                CboPerfilAC.Visibility = System.Windows.Visibility.Hidden;
                CboPerfilAC.ItemsSource = objPerfil.Perfil_Combo();
                CboPerfilAC.DisplayMember = "Perfil";
                CboPerfilAC.ValueMember = "IdPerfil";
                CboPerfilAC.EditValue = CboPerfil.EditValue;
                trvComp.ItemsSource = null;
                Utilitarios.TreeViewModelCompOT.LimpiarDatosTreeview();
                Utilitarios.TreeViewModelCompOT.tblListarPerfilComponentes = tblOTComp;
                trvComp.ItemsSource = Utilitarios.TreeViewModelCompOT.CargarDatosTreeViewPerfilComponente(1000, null);
                DataTable tblActividadDatos = new DataTable();
                DataTable tblTareaDatos = new DataTable();
                DataTable tblDetalleDatos = new DataTable();
                grvHerrpEsp.ItemsSource = null;
                grvTarea.ItemsSource = null;
                int idPerfilComp = 0;
                int ValorCombo = Convert.ToInt32(CboPerfilAC.EditValue);
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                InterfazMTTO.iSBO_BE.BEOITMList BEOITMListRep = new InterfazMTTO.iSBO_BE.BEOITMList();
                BEOITMListRep = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("R", ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                InterfazMTTO.iSBO_BE.BEOITMList BEOITMListCon = new InterfazMTTO.iSBO_BE.BEOITMList();
                BEOITMListCon = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("C", ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                if (rbtPerfil.IsChecked == true)
                {
                    objEPerfilComp = new E_PerfilComp();

                    objEPerfilComp.Idperfilcomp = idPerfilComp;

                    objEPerfilComp.Idperfil = ValorCombo;
                    tblActividadDatos = objBPerfilComp.Actividad_ComboByPerfil(objEPerfilComp);

                    foreach (DataRow drPFActividad in tblActividadDatos.Select("IsActivo = true"))
                    {
                        DataRow row = tblActividadestmp.NewRow();
                        row["IdOTCompActividad"] = 0;
                        row["IdOTComp"] = 0;
                        row["IdPerfilCompActividad"] = Convert.ToInt32(drPFActividad["IdPerfilCompActividad"]);
                        row["IdActividad"] = Convert.ToInt32(drPFActividad["IdActividad"]);
                        row["Actividad"] = drPFActividad["Actividad"].ToString();
                        row["IsChecked"] = false;
                        row["FlagUso"] = Convert.ToBoolean(drPFActividad["FlagUso"]);
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        row["IdPerfilComp"] = Convert.ToInt32(drPFActividad["IdPerfilComp"]);
                        row["PerfilComp"] = Convert.ToString(drPFActividad["PerfilComp"]);
                        row["FlagPendiente"] = false;
                        tblActividadestmp.Rows.Add(row);
                    }


                    ListarActividadesPendiente();


                    tblTareaDatos = objBPerfilComp.Tarea_ComboByPerfil(objEPerfilComp);
                    for (int i = 0; i < tblTareaDatos.Rows.Count; i++)
                    {
                        DataRow row = tblTareastmp.NewRow();
                        row["IdOTTarea"] = 0;
                        row["IdOTCompActividad"] = 0;
                        row["IdPerfilCompActividad"] = Convert.ToDouble(tblTareaDatos.Rows[i]["IdPerfilCompActividad"]); ;
                        row["IdTarea"] = Convert.ToDouble(tblTareaDatos.Rows[i]["IdTarea"]);
                        row["CodResponsable"] = CboResponsable.EditValue;
                        row["CostoHoraHombre"] = 0;
                        RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                        OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("S", ref RPTA);
                        if (RPTA.ResultadoRetorno == 0)
                        {
                            foreach (var drEmp in OHEMlist.Where(emp => emp.CodigoPersona == Convert.ToInt32(CboResponsable.EditValue)))
                            {
                                row["CostoHoraHombre"] = drEmp.CostoHoraHombre;
                            }
                            //for (int j = 0; j < OHEMlist.Count(); j++)
                            //{
                            //    if (Convert.ToString(CboResponsable.EditValue) == Convert.ToString(OHEMlist[j].CodigoPersona))
                            //    {
                            //        row["CostoHoraHombre"] = OHEMlist[j].CostoHoraHombre;
                            //    }
                            //}
                        }
                        else
                        {
                            GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                        }
                        row["HorasEstimada"] = Convert.ToDouble(tblTareaDatos.Rows[i]["HorasHombre"]);
                        row["HorasReal"] = 0;
                        row["OTTarea"] = tblTareaDatos.Rows[i]["Tarea"].ToString();
                        row["IdPerfilTarea"] = 0;
                        row["IdEstadoOTT"] = 1;
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        tblTareastmp.Rows.Add(row);
                    }
                    //Llenar Detalles
                    tblDetalleDatos = objBPerfilComp.PerfilDetalle_ComboByPerfil(objEPerfilComp);
                    for (int i = 0; i < tblDetalleDatos.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(tblDetalleDatos.Rows[i]["IdTipoArticulo"]) == 1)
                        {
                            DataRow row = tblHerrEsptmp.NewRow();
                            row["IdOTHerramienta"] = 0;
                            row["IdOTCompActividad"] = 0;
                            row["IdPerfilCompActividad"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["IdPerfilCompActividad"]);
                            row["IdHerramienta"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["IdArticulo"]); ;
                            row["Herramienta"] = Convert.ToString(tblDetalleDatos.Rows[i]["Articulo"]); ;
                            row["Cantidad"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["Cantidad"]); ;
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                            tblHerrEsptmp.Rows.Add(row);
                        }


                        else if (Convert.ToInt32(tblDetalleDatos.Rows[i]["IdTipoArticulo"]) == 3)
                        {
                            DataRow row = tblConsumibletmp.NewRow();
                            row["IdOTArticulo"] = 0;
                            row["IdOTCompActividad"] = 0;
                            row["IdPerfilCompActividad"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["IdPerfilCompActividad"]);
                            row["IdArticulo"] = Convert.ToString(tblDetalleDatos.Rows[i]["IdArticulo"]);
                            row["Articulo"] = Convert.ToString(tblDetalleDatos.Rows[i]["Articulo"]);
                            for (int j = 0; j < BEOITMListCon.Count; j++)
                            {
                                if (BEOITMListCon[j].CodigoArticulo == Convert.ToString(tblDetalleDatos.Rows[i]["IdArticulo"]))
                                {
                                    row["Articulo"] = BEOITMListCon[j].DescripcionArticulo;
                                    break;
                                }
                            }
                            row["IdTipoArticulo"] = tblDetalleDatos.Rows[i]["IdTipoArticulo"];
                            row["CantSol"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["Cantidad"]);
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                            tblConsumibletmp.Rows.Add(row);
                        }
                        else if (Convert.ToInt32(tblDetalleDatos.Rows[i]["IdTipoArticulo"]) == 2)
                        {
                            DataRow row = tblRepuestotmp.NewRow();
                            row["IdOTArticulo"] = 0;
                            row["IdOTCompActividad"] = 0;
                            row["IdPerfilCompActividad"] = Convert.ToDouble(tblDetalleDatos.Rows[i]["IdPerfilCompActividad"]);
                            row["IdArticulo"] = Convert.ToString(tblDetalleDatos.Rows[i]["IdArticulo"]);
                            row["Articulo"] = Convert.ToString(tblDetalleDatos.Rows[i]["Articulo"]);

                            foreach (var drArt in BEOITMListRep.Where(emp => emp.CodigoArticulo == tblDetalleDatos.Rows[i]["IdArticulo"].ToString()))
                            {
                                row["Articulo"] = drArt.DescripcionArticulo;
                            }

                            //for (int j = 0; j < BEOITMListRep.Count; j++)
                            //{
                            //    if (BEOITMListRep[j].CodigoArticulo == Convert.ToString(tblDetalleDatos.Rows[i]["IdArticulo"]))
                            //    {
                            //        row["Articulo"] = BEOITMListRep[j].DescripcionArticulo;
                            //        break;
                            //    }
                            //}
                            row["CantSol"] = Convert.ToDouble(tblDetalleDatos.Rows[i]["Cantidad"]);
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                            tblRepuestotmp.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }

        }
        private void rbtManual_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                LblSelected.Content = "Seleccionar una actividad...";
                tblActividadestmp.Rows.Clear();
                tblTareastmp.Rows.Clear();
                tblConsumibletmp.Rows.Clear();
                tblRepuestotmp.Rows.Clear();
                tblHerrEsptmp.Rows.Clear();
                if (chkSinUnidadControl.IsChecked == false)
                {
                    Utilitarios.TreeViewModelCompOT.LimpiarDatosTreeview();
                    Utilitarios.TreeViewModelCompOT.tblListarPerfilComponentes = tblOTComp;
                    trvComp.ItemsSource = Utilitarios.TreeViewModelCompOT.CargarDatosTreeViewPerfilComponente(1000, null);
                }

                DataTable tblActividadDatos = new DataTable();

                tblActividadDatos = (cboActividad.ItemsSource as DataTable);

                for (int i = 0; i < tblActividadDatos.Rows.Count; i++)
                {
                    DataRow row = tblActividadestmp.NewRow();
                    row["IdOTCompActividad"] = 0;
                    row["IdOTComp"] = 0;
                    row["IdPerfilCompActividad"] = IdPerfilCompMax;
                    row["IdActividad"] = Convert.ToInt32(tblActividadDatos.Rows[i]["IdActividad"]);
                    row["Actividad"] = tblActividadDatos.Rows[i]["Actividad"].ToString();
                    row["IsChecked"] = false;
                    row["FlagUso"] = false;
                    row["FlagActivo"] = true;
                    row["Nuevo"] = false;
                    row["IdPerfilComp"] = 0;
                    row["PerfilComp"] = "";
                    row["FlagPendiente"] = false;
                    tblActividadestmp.Rows.Add(row);
                    IdPerfilCompMax++;
                }

                ListarActividadesPendiente();

                CboPerfilAC.Visibility = System.Windows.Visibility.Hidden;
                CboPerfilAC.ItemsSource = null;
                trvComp.ItemsSource = null;
                if (chkSinUnidadControl.IsChecked == false)
                {
                    Utilitarios.TreeViewModelCompOT.LimpiarDatosTreeview();
                    Utilitarios.TreeViewModelCompOT.tblListarPerfilComponentes = tblOTComp;
                    trvComp.ItemsSource = Utilitarios.TreeViewModelCompOT.CargarDatosTreeViewPerfilComponente(1000, null);
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void chkSinUnidadControl_Click(object sender, RoutedEventArgs e)
        {
            CboPerfilAC.SelectedIndexChanged -= new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
            if (chkSinUnidadControl.IsChecked == true)
            {
                CboPerfil.IsEnabled = false;
                CboPerfil.SelectedIndex = -1;
                //CboUnidadControl.IsEnabled = false;
                CboUnidadControl.ItemsSource = null;
                rbtPM.IsEnabled = false;
                rbtPerfil.IsEnabled = false;
                CboPerfilAC.IsEnabled = false;
                CboPerfilAC.ItemsSource = null;
                rbtManual.IsChecked = true;
                CboPerfilAC.SelectedIndex = -1;
                cboTarea.ItemsSource = null;
                lstboxActividad.ItemsSource = null;
                //trvComp.IsEnabled = false;
                //ListarOTComp();
                //Listar Combo
                DataTable tlbCompSinUC = objB_OT.Item_ListSinUC();
                CboUnidadControl.ItemsSource = tlbCompSinUC.DefaultView;
                CboUnidadControl.ValueMember = "IdItem";
                CboUnidadControl.DisplayMember = "DescripcionSAP";
                lblUC.Content = "Componente:";

            }
            else
            {
                CboPerfil.IsEnabled = true;
                CboPerfil.Focus();
                CboPerfil.SelectedIndex = -1;
                CboUnidadControl.IsEnabled = true;
                CboUnidadControl.ItemsSource = true;
                rbtPM.IsEnabled = true;
                rbtPerfil.IsEnabled = true;
                CboPerfilAC.IsEnabled = true;
                CboPerfilAC.ItemsSource = null;
                rbtPM.IsChecked = true;
                CboPerfilAC.SelectedIndex = -1;
                cboTarea.ItemsSource = null;

                lstboxActividad.ItemsSource = null;
                // ListarOTComp();
                lblUC.Content = "Unidad de control:";
            }
            CboPerfilAC.SelectedIndexChanged += new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
            ActivaDetalleOT();

        }
        private void btnAceptarTarea_Click(object sender, RoutedEventArgs e)
        {
            if (IdPerfilCompActividad == 0 && IdOTCompActividad == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_ACTI"), 2);
                return;
            }
            if (CboResponsable.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RESP"), 2);
                return;
            }

            if (cboTarea.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_TARE"), 2);
                return;
            }
            if (txtHorasTarea.Text == "")
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_TARE_HORA"), 2);
                txtHorasTarea.Focus();
                return;
            }
            if (!txtHorasTarea.Text.Contains(":"))
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_TARE_HORA"), 2);
                txtHorasTarea.Focus();
                return;
            }
            if (txtHorasTarea.Text.Split(':')[1].Trim() == "")
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_TARE_HORA"), 2);
                txtHorasTarea.Focus();
                return;
            }
            if (txtHorasTarea.Text == "00:00")
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_TARE_HORA"), 2);
                txtHorasTarea.Focus();
                return;
            }


            //Validar Si existe tarea ya añadida
            for (int i = 0; i < tblTareas.Rows.Count; i++)
            {
                if (IdOT == 0)
                {
                    if (Convert.ToString(tblTareas.Rows[i]["IdTarea"]) == cboTarea.EditValue.ToString() && Convert.ToString(tblTareas.Rows[i]["IdPerfilCompActividad"]) == IdPerfilCompActividad.ToString())
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_TARE_EXIS"), 2);
                        return;
                    }
                }
                else
                {
                    if (IdOTCompActividad != 0)
                    {
                        if (Convert.ToString(tblTareas.Rows[i]["IdTarea"]) == cboTarea.EditValue.ToString() && Convert.ToString(tblTareas.Rows[i]["IdOTCompActividad"]) == IdOTCompActividad.ToString())
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_TARE_EXIS"), 2);
                            return;
                        }
                    }
                    else
                    {
                        if (Convert.ToString(tblTareas.Rows[i]["IdTarea"]) == cboTarea.EditValue.ToString() && Convert.ToString(tblTareas.Rows[i]["IdPerfilCompActividad"]) == IdPerfilCompActividad.ToString())
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_TARE_EXIS"), 2);
                            return;
                        }
                    }

                }
            }

            DataRow row = tblTareas.NewRow();
            row["IdOTTarea"] = 0;
            row["IdOTCompActividad"] = IdOTCompActividad;
            row["IdPerfilCompActividad"] = IdPerfilCompActividad;
            row["IdTarea"] = cboTarea.EditValue;
            row["OTTarea"] = cboTarea.Text;
            row["IdPerfilTarea"] = 0;
            row["CodResponsable"] = CboResponsable.EditValue;
            //Obtener Costo Hora
            row["CostoHoraHombre"] = 0;

            RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
            OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("R", ref RPTA);
            if (RPTA.ResultadoRetorno == 0)
            {
                for (int i = 0; i < OHEMlist.Count(); i++)
                {
                    if (Convert.ToString(CboResponsable.EditValue) == Convert.ToString(OHEMlist[i].CodigoPersona))
                    {
                        row["CostoHoraHombre"] = OHEMlist[i].CostoHoraHombre;
                    }
                }
            }
            else
            {
                GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
            }
            //convertir 
            //double hrest = Convert.ToDouble(txtHorasTarea.Text.Substring(11, 2)) + (Convert.ToDouble(txtHorasTarea.Text.Substring(14, 2)) / 60);
            string[] Horas = txtHorasTarea.Text.Split(':');
            Horas[1] = (Horas[1].Length == 1) ? Horas[1] + "0" : Horas[1];
            double hrest = Convert.ToDouble(Horas[0]) + (Convert.ToDouble(Horas[1]) / 60);

            row["HorasEstimada"] = Math.Round(hrest, 2); ;

            row["HorasReal"] = 0;
            row["IdEstadoOTT"] = 1;
            row["FlagAutomatico"] = false;
            row["FlagActivo"] = true;
            row["Nuevo"] = true;
            tblTareas.Rows.Add(row);

            if (IdOT == 0)
            {
            }
            else
            {
            }
            stkTareas.Visibility = System.Windows.Visibility.Hidden;
            cboTarea.SelectedIndex = -1;
            txtHorasTarea.Clear();
        }
        private void btnCancelarTarea_Click(object sender, RoutedEventArgs e)
        {
            stkTareas.Visibility = System.Windows.Visibility.Hidden;
            cboTarea.SelectedIndex = -1;
            txtHorasTarea.Clear();
        }
        private void btnCancelarHerEsp_Click(object sender, RoutedEventArgs e)
        {
            stkHerrEsp.Visibility = System.Windows.Visibility.Hidden;
            cboHerrEsp.SelectedIndex = -1;
            txtCantHerrEsp.Clear();
        }
        private void btnAceptarHerrEsp_Click(object sender, RoutedEventArgs e)
        {
            if (IdPerfilCompActividad == 0 && IdOTCompActividad == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_ACTI"), 2);
                return;
            }
            for (int i = 0; i < tblHerrEsp.Rows.Count; i++)
            {
                if (IdOT == 0)
                {
                    if (Convert.ToString(tblHerrEsp.Rows[i]["IdHerramienta"]) == cboHerrEsp.EditValue.ToString() && Convert.ToString(tblHerrEsp.Rows[i]["IdPerfilCompActividad"]) == IdPerfilCompActividad.ToString())
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_HERR_EXIS"), 2);
                        return;
                    }
                }
                else
                {
                    if (IdOTCompActividad != 0)
                    {
                        if (Convert.ToString(tblHerrEsp.Rows[i]["IdHerramienta"]) == cboHerrEsp.EditValue.ToString() && Convert.ToString(tblHerrEsp.Rows[i]["IdOTCompActividad"]) == IdOTCompActividad.ToString())
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_HERR_EXIS"), 2);
                            return;
                        }
                    }
                    else
                    {
                        if (Convert.ToString(tblHerrEsp.Rows[i]["IdHerramienta"]) == cboHerrEsp.EditValue.ToString() && Convert.ToString(tblHerrEsp.Rows[i]["IdPerfilCompActividad"]) == IdPerfilCompActividad.ToString())
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_HERR_EXIS"), 2);
                            return;
                        }
                    }
                }
            }

            if (cboHerrEsp.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_HERR"), 2);
                return;
            }

            if (txtCantHerrEsp.Text == "" || Convert.ToDouble(txtCantHerrEsp.Text) == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_HERR_CANT"), 2);
                return;
            }

            DataRow row = tblHerrEsp.NewRow();
            row["IdOTHerramienta"] = 0;
            row["IdOTCompActividad"] = IdOTCompActividad;
            row["IdPerfilCompActividad"] = IdPerfilCompActividad;
            row["IdHerramienta"] = cboHerrEsp.EditValue;
            row["Herramienta"] = cboHerrEsp.Text;
            row["Cantidad"] = txtCantHerrEsp.Text;
            row["FlagAutomatico"] = false;
            row["FlagActivo"] = true;
            row["Nuevo"] = true;
            tblHerrEsp.Rows.Add(row);

            if (IdOT == 0)
            {
                //Listar Herramientas Especiales
                DataView dtvHerEsp = tblHerrEsp.DefaultView;
                dtvHerEsp.RowFilter = "IdPerfilCompActividad = " + IdPerfilCompActividad.ToString();
                grvHerrpEsp.ItemsSource = dtvHerEsp;
            }
            else
            {
                //Listar Repuesto
                DataView dtvHerEsp = tblHerrEsp.DefaultView;
                dtvHerEsp.RowFilter = "IdOTCompActividad = " + IdOTCompActividad.ToString();
                grvHerrpEsp.ItemsSource = dtvHerEsp;
            }

            stkHerrEsp.Visibility = System.Windows.Visibility.Hidden;
            cboHerrEsp.SelectedIndex = -1;
            txtCantHerrEsp.Clear();
        }
        private void btnAceptarRepuesto_Click(object sender, RoutedEventArgs e)
        {
            if (IdPerfilCompActividad == 0 && IdOTCompActividad == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_ACTI"), 2);
                return;
            }

            if (cboRepuesto.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_REPU"), 2);
                return;
            }

            if (txtCantRepuesto.Text == "" || Convert.ToDouble(txtCantRepuesto.Text) == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_REPU_CANT"), 2);
                return;
            }

            for (int i = 0; i < tblRepuesto.Rows.Count; i++)
            {
                if (IdOT == 0)
                {
                    if (Convert.ToString(tblRepuesto.Rows[i]["IdArticulo"]) == cboRepuesto.EditValue.ToString() && Convert.ToString(tblRepuesto.Rows[i]["IdPerfilCompActividad"]) == IdPerfilCompActividad.ToString())
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_REPU_EXIS"), 2);
                        return;
                    }
                }
                else
                {
                    if (IdOTCompActividad != 0)
                    {
                        if (Convert.ToString(tblRepuesto.Rows[i]["IdArticulo"]) == cboRepuesto.EditValue.ToString() && Convert.ToString(tblRepuesto.Rows[i]["IdOTCompActividad"]) == IdOTCompActividad.ToString())
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_REPU_EXIS"), 2);
                            return;
                        }
                    }
                    else
                    {
                        if (Convert.ToString(tblRepuesto.Rows[i]["IdArticulo"]) == cboRepuesto.EditValue.ToString() && Convert.ToString(tblRepuesto.Rows[i]["IdPerfilCompActividad"]) == IdPerfilCompActividad.ToString())
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_REPU_EXIS"), 2);
                            return;
                        }
                    }

                }
            }


            DataRow row = tblRepuesto.NewRow();

            row["IdOTArticulo"] = 0;
            row["IdOTCompActividad"] = IdOTCompActividad;
            row["IdPerfilCompActividad"] = IdPerfilCompActividad;
            row["IdTipoArticulo"] = 2;
            row["IdArticulo"] = cboRepuesto.EditValue;
            row["Articulo"] = cboRepuesto.Text;
            row["CantSol"] = txtCantRepuesto.Text;
            row["CantEnv"] = 0;
            row["CantUti"] = 0;
            row["Observacion"] = 0;
            row["CostoArticulo"] = 0;
            row["FlagAutomatico"] = false;
            row["FlagActivo"] = true;
            row["Nuevo"] = true;

            tblRepuesto.Rows.Add(row);

            if (IdOT == 0)
            {
                //Listar Repuesto
                DataView dtvRepuesto = tblRepuesto.DefaultView;
                dtvRepuesto.RowFilter = "IdPerfilCompActividad = " + IdPerfilCompActividad.ToString();
                grvRepuesto.ItemsSource = dtvRepuesto;
            }
            else
            {
                //Listar Repuesto
                DataView dtvRepuesto = tblRepuesto.DefaultView;
                dtvRepuesto.RowFilter = "IdOTCompActividad = " + IdOTCompActividad.ToString();
                grvRepuesto.ItemsSource = dtvRepuesto;
            }


            stkRepuesto.Visibility = System.Windows.Visibility.Hidden;
            cboRepuesto.SelectedIndex = -1;
            txtCantRepuesto.Clear();
        }
        private void btnCancelarRepuesto_Click(object sender, RoutedEventArgs e)
        {
            stkRepuesto.Visibility = System.Windows.Visibility.Hidden;
            cboRepuesto.SelectedIndex = -1;
            txtCantRepuesto.Clear();
        }
        private void btnAceptarConsumible_Click(object sender, RoutedEventArgs e)
        {
            if (IdPerfilCompActividad == 0 && IdOTCompActividad == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_ACTI"), 2);
                return;
            }

            if (cboConsumible.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_CONS"), 2);
                return;
            }

            if (txtCantConsumible.Text == "" || Convert.ToDouble(txtCantConsumible.Text) == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_CONS_CANT"), 2);
                return;
            }

            for (int i = 0; i < tblConsumible.Rows.Count; i++)
            {
                if (IdOT == 0)
                {
                    if (Convert.ToString(tblConsumible.Rows[i]["IdArticulo"]) == cboConsumible.EditValue.ToString() && Convert.ToString(tblConsumible.Rows[i]["IdPerfilCompActividad"]) == IdPerfilCompActividad.ToString())
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_CONS_EXIS"), 2);
                        return;
                    }
                }
                else
                {
                    if (IdOTCompActividad != 0)
                    {
                        if (Convert.ToString(tblConsumible.Rows[i]["IdArticulo"]) == cboConsumible.EditValue.ToString() && Convert.ToString(tblConsumible.Rows[i]["IdOTCompActividad"]) == IdOTCompActividad.ToString())
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_CONS_EXIS"), 2);
                            return;
                        }
                    }
                    else
                    {
                        if (Convert.ToString(tblConsumible.Rows[i]["IdArticulo"]) == cboConsumible.EditValue.ToString() && Convert.ToString(tblConsumible.Rows[i]["IdPerfilCompActividad"]) == IdPerfilCompActividad.ToString())
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_CONS_EXIS"), 2);
                            return;
                        }
                    }
                }
            }

            DataRow row = tblConsumible.NewRow();

            row["IdOTArticulo"] = 0;
            row["IdOTCompActividad"] = IdOTCompActividad;
            row["IdPerfilCompActividad"] = IdPerfilCompActividad;
            row["IdTipoArticulo"] = 3;
            row["IdArticulo"] = cboConsumible.EditValue;
            row["Articulo"] = cboConsumible.Text;
            row["CantSol"] = txtCantConsumible.Text;
            row["CantEnv"] = 0;
            row["CantUti"] = 0;
            row["Observacion"] = 0;
            row["CostoArticulo"] = 0;
            row["FlagAutomatico"] = false;
            row["FlagActivo"] = true;
            row["Nuevo"] = true;

            tblConsumible.Rows.Add(row);

            if (IdOT == 0)
            {
                //Listar Consumible
                DataView dtvConsumible = tblConsumible.DefaultView; //Aqui filtrar por tipo 2
                dtvConsumible.RowFilter = "IdTipoArticulo = 3 and IdPerfilCompActividad = " + IdPerfilCompActividad.ToString();
                grvConsumible.ItemsSource = dtvConsumible;
            }
            else
            {
                //Listar Consumible
                DataView dtvConsumible = tblConsumible.DefaultView;
                dtvConsumible.RowFilter = "IdTipoArticulo = 3 and IdOTCompActividad = " + IdOTCompActividad.ToString();
                grvConsumible.ItemsSource = dtvConsumible;
            }

            stkConsumible.Visibility = System.Windows.Visibility.Hidden;
            cboConsumible.SelectedIndex = -1;
            txtCantConsumible.Clear();
        }
        private void btnCancelarConsumible_Click(object sender, RoutedEventArgs e)
        {
            stkConsumible.Visibility = System.Windows.Visibility.Hidden;
            cboConsumible.SelectedIndex = -1;
            txtCantConsumible.Clear();
        }
        private void btnAbrirTarea_Click(object sender, RoutedEventArgs e)
        {
            stkTareas.Visibility = System.Windows.Visibility.Visible;
            cboTarea.SelectedIndex = -1;
            txtHorasTarea.Clear();
        }
        private void btnAbrirHerrEsp_Click(object sender, RoutedEventArgs e)
        {
            stkHerrEsp.Visibility = System.Windows.Visibility.Visible;
            cboHerrEsp.SelectedIndex = -1;
            txtCantHerrEsp.Clear();
        }
        private void btnRepuesto_Click(object sender, RoutedEventArgs e)
        {
            stkRepuesto.Visibility = System.Windows.Visibility.Visible;
            cboRepuesto.SelectedIndex = -1;
            txtCantRepuesto.Clear();
        }
        private void btnConsumible_Click(object sender, RoutedEventArgs e)
        {
            stkConsumible.Visibility = System.Windows.Visibility.Visible;
            cboConsumible.SelectedIndex = -1;
            txtCantConsumible.Clear();
        }
        private void ListarOTComp()
        {

            try
            {
                tblOTComp.Rows.Clear();

                objE_OTComp = new E_OTComp();
                objEUCComp = new E_UCComp();
                objEPerfilComp = new E_PerfilComp();
                objEUCComp.IdPerfil = Convert.ToInt32(CboPerfil.EditValue);
                objEPerfilComp.Idperfil = Convert.ToInt32(CboPerfil.EditValue);
                objEPerfilComp.Idestadopc = 0;
                if (chkSinUnidadControl.IsChecked == true)
                {
                    objE_OTComp.IdOT = IdOT;
                    objE_OTComp.CodUC = "";
                    objE_OTComp.IdItem = Convert.ToInt32(CboUnidadControl.EditValue);
                }
                else
                {
                    objE_OTComp.IdOT = IdOT;
                    objE_OTComp.CodUC = CboUnidadControl.EditValue.ToString();
                    objE_OTComp.IdItem = 0;
                }

                DataTable tblPerfilComponentesDatos = objBPerfilComp.PerfilComp_List(objEPerfilComp);// objBUCComp.PerfilComp_List(objEUCComp);
                tblPerfilComponentesDatos.DefaultView.Sort = "IdPerfilComp asc";
                tblPerfilComponentesDatos.DefaultView.RowFilter = "FlagNeumatico = 0";
                tblPerfilComponentesDatos = tblPerfilComponentesDatos.DefaultView.ToTable(true);

                DataTable tblCompDatos = objB_OTComp.OTComp_List(objE_OTComp);
                tblCompDatos.DefaultView.Sort = "IdPerfilComp asc";
                tblCompDatos = tblCompDatos.DefaultView.ToTable(true);

                if (chkSinUnidadControl.IsChecked == true)
                {
                    if (tblCompDatos.Rows.Count > 0 && IdOT != 0)
                    {
                        DataRow row1 = tblOTComp.NewRow();
                        row1["IdOTComp"] = Convert.ToInt32(tblCompDatos.Rows[0]["IdOTComp"]);
                        row1["IdUCComp"] = Convert.ToInt32(tblCompDatos.Rows[0]["IdUCComp"]);
                        row1["IdOT"] = Convert.ToInt32(tblCompDatos.Rows[0]["IdOT"]);
                        row1["IdPerfilComp"] = 1;//Convert.ToInt32(tblCompDatos.Rows[0]["IdPerfilComp"]);
                        row1["IdPerfilCompPadre"] = 1000;
                        row1["PerfilComp"] = tblCompDatos.Rows[0]["DescripcionSAP"].ToString();
                        row1["NroSerie"] = tblCompDatos.Rows[0]["NroSerie"].ToString();
                        row1["CodigoSAP"] = tblCompDatos.Rows[0]["CodigoSAP"].ToString();
                        row1["DescripcionSAP"] = tblCompDatos.Rows[0]["DescripcionSAP"].ToString();
                        row1["IdEstadoOTComp"] = Convert.ToInt32(tblCompDatos.Rows[0]["IdEstadoOTComp"]);
                        row1["FlagActivo"] = true;
                        row1["Nuevo"] = false;
                        row1["IsChecked"] = true;
                        tblOTComp.Rows.Add(row1);
                    }
                    else
                    {
                        DataRow row1 = tblOTComp.NewRow();
                        row1["IdOTComp"] = 0;
                        row1["IdOT"] = 0;
                        row1["IdPerfilComp"] = 1;
                        row1["IdPerfilCompPadre"] = 1000;
                        row1["PerfilComp"] = CboUnidadControl.Text;
                        row1["IdTipoDetalle"] = 0;
                        row1["IdItem"] = CboUnidadControl.EditValue;
                        row1["NroSerie"] = "";
                        row1["CodigoSAP"] = "";
                        row1["DescripcionSAP"] = "";
                        row1["IdEstadoOTComp"] = 1;
                        row1["FlagActivo"] = true;
                        row1["Nuevo"] = true;
                        row1["IsChecked"] = false;
                        tblOTComp.Rows.Add(row1);
                    }
                }
                else
                {
                    DataRow row1 = tblOTComp.NewRow();
                    row1["IdOTComp"] = 0;
                    row1["IdUCComp"] = 0;
                    row1["IdOT"] = 0;
                    row1["IdPerfilComp"] = 0;
                    row1["IdPerfilCompPadre"] = 1000;
                    row1["PerfilComp"] = CboUnidadControl.Text;
                    row1["IdTipoDetalle"] = 0;
                    row1["IdItem"] = 1000;
                    row1["NroSerie"] = "";
                    row1["CodigoSAP"] = "";
                    row1["DescripcionSAP"] = "";
                    row1["IdEstadoOTComp"] = 1;
                    row1["FlagActivo"] = true;
                    row1["Nuevo"] = false;
                    row1["IsChecked"] = false;
                    tblOTComp.Rows.Add(row1);

                    int ucindex = 0;
                    int IdPerfilCompUC = 0;
                    for (int i = 0; i < tblPerfilComponentesDatos.Rows.Count; i++)
                    {
                        int IdPerfilComp = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilComp"]);
                        if (ucindex < tblCompDatos.Rows.Count) IdPerfilCompUC = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilComp"]);

                        DataRow row;
                        row = tblOTComp.NewRow();
                        if (IdPerfilCompUC == IdPerfilComp)
                        {
                            row["IdOTComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdOTComp"]);
                            row["IdUCComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdUCComp"]);
                            row["IdOT"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdOT"]);

                            if (IdOT == 0)
                            {
                                row["IdPerfilComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilComp"]);
                            }
                            else
                            {
                                row["IdPerfilComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilComp"]);
                            }
                            row["IdPerfilCompPadre"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilCompPadre"]);


                            //Verificar si tiene activiadades
                            int Cantacti = 0;
                            Cantacti = tblActividades.Select("IdPerfilComp = " + IdPerfilComp.ToString()).Length;

                            //if (Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdUCComp"]) == 0)
                            if (Cantacti == 0)
                            {
                                row["PerfilComp"] = tblCompDatos.Rows[ucindex]["PerfilComp"].ToString();
                            }
                            else
                            {
                                row["PerfilComp"] = "* " + tblCompDatos.Rows[ucindex]["PerfilComp"].ToString();
                            }
                            row["NroSerie"] = tblCompDatos.Rows[ucindex]["NroSerie"].ToString();
                            row["CodigoSAP"] = tblCompDatos.Rows[ucindex]["CodigoSAP"].ToString();
                            row["DescripcionSAP"] = tblCompDatos.Rows[ucindex]["DescripcionSAP"].ToString();
                            row["IdEstadoOTComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdEstadoOTComp"]);
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                            if (IdOT == 0)
                            {
                                row["IsChecked"] = true;
                            }
                            else
                            {
                                row["IsChecked"] = true;
                            }
                            tblOTComp.Rows.Add(row);
                            ucindex++;
                        }
                        else
                        {
                            row["IdOTComp"] = 0;
                            row["IdUCComp"] = 0;
                            row["IdOT"] = 0;
                            row["IdPerfilComp"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilComp"]);
                            row["IdPerfilCompPadre"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilCompPadre"]);

                            int Cantacti = tblActividades.Select("IdPerfilComp = " + IdPerfilComp.ToString()).Length;
                            if (Cantacti == 0)
                            {
                                row["PerfilComp"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["PerfilComp"]);
                            }
                            else
                            {
                                row["PerfilComp"] = "* " + Convert.ToString(tblPerfilComponentesDatos.Rows[i]["PerfilComp"]);
                            }

                            row["IdTipoDetalle"] = 0;

                            row["NroSerie"] = "";// dtvPerfilComp[0]["NroSerie"].ToString();
                            row["CodigoSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["CodigoSAP"]);
                            row["DescripcionSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["DescripcionSAP"]);
                            row["IdEstadoOTComp"] = 0;
                            row["FlagActivo"] = true;
                            row["Nuevo"] = true;
                            row["IsChecked"] = false;
                            tblOTComp.Rows.Add(row);
                        }
                    }
                }
                trvComp.ItemsSource = null;
                Utilitarios.TreeViewModelCompOT.LimpiarDatosTreeview();
                Utilitarios.TreeViewModelCompOT.tblListarPerfilComponentes = tblOTComp;
                trvComp.ItemsSource = Utilitarios.TreeViewModelCompOT.CargarDatosTreeViewPerfilComponente(1000, null);
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void CboUnidadControl_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            CboPerfilAC.SelectedIndexChanged -= new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
            CboPerfilAC.SelectedIndex = -1;
            cboTarea.ItemsSource = null;
            lstboxActividad.ItemsSource = null;
            if (CboUnidadControl.SelectedIndex != -1)
            {
                ListarOTComp();
            }
            tabItem4.IsEnabled = true;
            ActivaDetalleOT();
            CboPerfilAC.SelectedIndexChanged += new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
        }
        private void CboPerfilAC_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                tblActividadestmp.Rows.Clear();
                tblTareastmp.Rows.Clear();
                tblConsumibletmp.Rows.Clear();
                tblRepuestotmp.Rows.Clear();
                tblHerrEsptmp.Rows.Clear();
                DataTable tblActividadDatos = new DataTable();
                DataTable tblTareaDatos = new DataTable();
                DataTable tblDetalleDatos = new DataTable();

                grvHerrpEsp.ItemsSource = null;
                grvTarea.ItemsSource = null;
                //TreeViewModelCompOT trm = (TreeViewModelCompOT)trvComp.SelectedItem;
                int idPerfilComp = 0; // Convert.ToInt32(trm.IdMenu);

                int ValorCombo = Convert.ToInt32(CboPerfilAC.EditValue);
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                InterfazMTTO.iSBO_BE.BEOITMList BEOITMListRep = new InterfazMTTO.iSBO_BE.BEOITMList();
                BEOITMListRep = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("R", ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                InterfazMTTO.iSBO_BE.BEOITMList BEOITMListCon = new InterfazMTTO.iSBO_BE.BEOITMList();
                BEOITMListCon = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("C", ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                if (rbtPM.IsChecked == true)
                {
                    objEPM = new E_PM();
                    objEPM.IdPerfilComp = idPerfilComp;
                    objEPM.IdPM = ValorCombo;
                    tblActividadDatos = objBPM.Actividad_ComboByPM(objEPM);
                    //Llenar Actividad
                    for (int i = 0; i < tblActividadDatos.Rows.Count; i++)
                    {
                        DataRow row = tblActividadestmp.NewRow();
                        row["IdOTCompActividad"] = 0;
                        row["IdOTComp"] = 0;
                        row["IdPerfilCompActividad"] = Convert.ToInt32(tblActividadDatos.Rows[i]["IdPerfilCompActividad"]);
                        row["IdActividad"] = Convert.ToInt32(tblActividadDatos.Rows[i]["IdActividad"]);
                        row["Actividad"] = tblActividadDatos.Rows[i]["Actividad"].ToString();
                        row["IsChecked"] = false;
                        row["FlagUso"] = tblActividadDatos.Rows[i]["FlagUso"].ToString(); //Convert.ToBoolean(tblActividadDatos.Rows[i]["FlagExterna"]);
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        row["IdPerfilComp"] = Convert.ToInt32(tblActividadDatos.Rows[i]["IdPerfilComp"]);
                        row["PerfilComp"] = Convert.ToString(tblActividadDatos.Rows[i]["PerfilComp"]);
                        row["FlagPendiente"] = false;
                        tblActividadestmp.Rows.Add(row);
                    }


                    ListarActividadesPendiente();


                    //Llenar tareas
                    tblTareaDatos = objBPM.Tarea_ComboByPM(objEPM);
                    for (int i = 0; i < tblTareaDatos.Rows.Count; i++)
                    {
                        DataRow row = tblTareastmp.NewRow();
                        row["IdOTTarea"] = 0;
                        row["IdOTCompActividad"] = 0;
                        row["IdPerfilCompActividad"] = Convert.ToDouble(tblTareaDatos.Rows[i]["IdPerfilCompActividad"]); ;
                        row["IdTarea"] = Convert.ToDouble(tblTareaDatos.Rows[i]["IdTarea"]);
                        row["CodResponsable"] = CboResponsable.EditValue;
                        row["CostoHoraHombre"] = 0;
                        RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                        OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("S", ref RPTA);
                        if (RPTA.ResultadoRetorno == 0)
                        {
                            for (int j = 0; j < OHEMlist.Count(); j++)
                            {
                                if (Convert.ToString(CboResponsable.EditValue) == Convert.ToString(OHEMlist[j].CodigoPersona))
                                {
                                    row["CostoHoraHombre"] = OHEMlist[j].CostoHoraHombre;
                                }
                            }
                        }
                        else
                        {
                            GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                        }


                        row["HorasEstimada"] = Convert.ToDouble(tblTareaDatos.Rows[i]["HorasHombre"]);
                        row["HorasReal"] = 0;
                        row["OTTarea"] = tblTareaDatos.Rows[i]["Tarea"].ToString();
                        row["IdPerfilTarea"] = 0;
                        row["IdEstadoOTT"] = 1;
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        tblTareastmp.Rows.Add(row);
                    }

                    //Llenar Detalles -- Falta el SP
                    tblDetalleDatos = objBPM.PerfilDetalle_ComboByPM(objEPM);
                    for (int i = 0; i < tblDetalleDatos.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(tblDetalleDatos.Rows[i]["IdTipoArticulo"]) == 1)
                        {
                            DataRow row = tblHerrEsptmp.NewRow();
                            row["IdOTHerramienta"] = 0;
                            row["IdOTCompActividad"] = 0;
                            row["IdPerfilCompActividad"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["IdPerfilCompActividad"]);
                            row["IdHerramienta"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["IdArticulo"]); ;
                            row["Herramienta"] = Convert.ToString(tblDetalleDatos.Rows[i]["Articulo"]); ;
                            row["Cantidad"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["Cantidad"]); ;
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                            tblHerrEsptmp.Rows.Add(row);
                        }
                        else if (Convert.ToInt32(tblDetalleDatos.Rows[i]["IdTipoArticulo"]) == 2)
                        {
                            DataRow row = tblConsumibletmp.NewRow();
                            row["IdOTArticulo"] = 0;
                            row["IdOTCompActividad"] = 0;
                            row["IdPerfilCompActividad"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["IdPerfilCompActividad"]);
                            row["IdTipoArticulo"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["IdTipoArticulo"]);
                            row["IdArticulo"] = Convert.ToString(tblDetalleDatos.Rows[i]["IdArticulo"]);
                            row["Articulo"] = Convert.ToString(tblDetalleDatos.Rows[i]["Articulo"]);
                            for (int j = 0; j < BEOITMListCon.Count; j++)
                            {
                                if (Convert.ToString(tblDetalleDatos.Rows[i]["IdArticulo"]) == BEOITMListCon[j].CodigoArticulo)
                                {
                                    row["Articulo"] = BEOITMListCon[j].DescripcionArticulo;
                                }
                            }
                            row["CantSol"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["Cantidad"]);
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                            tblConsumibletmp.Rows.Add(row);
                        }
                        else if (Convert.ToInt32(tblDetalleDatos.Rows[i]["IdTipoArticulo"]) == 3)
                        {
                            DataRow row = tblRepuestotmp.NewRow();
                            row["IdOTArticulo"] = 0;
                            row["IdOTCompActividad"] = 0;
                            row["IdPerfilCompActividad"] = Convert.ToDouble(tblDetalleDatos.Rows[i]["IdPerfilCompActividad"]);
                            row["IdTipoArticulo"] = Convert.ToInt32(tblDetalleDatos.Rows[i]["IdTipoArticulo"]);
                            row["IdArticulo"] = Convert.ToString(tblDetalleDatos.Rows[i]["IdArticulo"]); ;
                            row["Articulo"] = Convert.ToString(tblDetalleDatos.Rows[i]["Articulo"]); ;
                            for (int j = 0; j < BEOITMListRep.Count; j++)
                            {
                                if (Convert.ToString(tblDetalleDatos.Rows[i]["IdArticulo"]) == BEOITMListRep[j].CodigoArticulo)
                                {
                                    row["Articulo"] = BEOITMListRep[j].DescripcionArticulo;
                                }
                            }
                            row["CantSol"] = Convert.ToDouble(tblDetalleDatos.Rows[i]["Cantidad"]); ;
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                            tblRepuestotmp.Rows.Add(row);
                        }
                    }

                }

                //BloquearControles();
                //Listar actividades listbox
                //DataView dtvActividades = tblActividades.DefaultView;
                //dtvActividades.RowFilter = "IdPerfilComp = " + idPerfilComp.ToString();
                //lstboxActividad.ItemsSource = dtvActividades;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void lstchkActividad_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                if (Convert.ToInt32(CboOrden.EditValue) == 1 || Convert.ToInt32(CboOrden.EditValue) == 2)
                {
                    (sender as CheckBox).IsChecked = false;
                    return;
                }

                for (int i = 0; i < tblActividades.Rows.Count; i++)
                {
                    if (Convert.ToString((sender as CheckBox).Tag) == Convert.ToString(tblActividades.Rows[i]["Actividad"]))
                    {
                        tblActividades.Rows[i]["IsChecked"] = true;
                        //Desactivar las Detalle ya asignadas
                        for (int j = 0; j < tblTareas.Rows.Count; j++)
                        {
                            if (IdOT == 0)
                            {
                                if (Convert.ToInt32(tblTareas.Rows[j]["IdPerfilCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdPerfilCompActividad"]))
                                {
                                    tblTareas.Rows[j]["FlagActivo"] = false;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(tblTareas.Rows[j]["IdOTCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdOTCompActividad"]))
                                {
                                    tblTareas.Rows[j]["FlagActivo"] = false;
                                }
                            }
                        }

                        for (int j = 0; j < tblHerrEsp.Rows.Count; j++)
                        {
                            if (IdOT == 0)
                            {
                                if (Convert.ToInt32(tblHerrEsp.Rows[j]["IdPerfilCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdPerfilCompActividad"]))
                                {
                                    tblHerrEsp.Rows[j]["FlagActivo"] = false;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(tblHerrEsp.Rows[j]["IdOTCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdOTCompActividad"]))
                                {
                                    tblHerrEsp.Rows[j]["FlagActivo"] = false;
                                }
                            }
                        }

                        for (int j = 0; j < tblConsumible.Rows.Count; j++)
                        {
                            if (IdOT == 0)
                            {
                                if (Convert.ToInt32(tblConsumible.Rows[j]["IdPerfilCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdPerfilCompActividad"]))
                                {
                                    tblConsumible.Rows[j]["FlagActivo"] = false;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(tblConsumible.Rows[j]["IdOTCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdOTCompActividad"]))
                                {
                                    tblConsumible.Rows[j]["FlagActivo"] = false;
                                }
                            }
                        }

                        for (int j = 0; j < tblRepuesto.Rows.Count; j++)
                        {
                            if (IdOT == 0)
                            {
                                if (Convert.ToInt32(tblRepuesto.Rows[j]["IdPerfilCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdPerfilCompActividad"]))
                                {
                                    tblRepuesto.Rows[j]["FlagActivo"] = false;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(tblRepuesto.Rows[j]["IdOTCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdOTCompActividad"]))
                                {
                                    tblRepuesto.Rows[j]["FlagActivo"] = false;
                                }
                            }
                        }

                        DataRowView dr = (lstboxActividad.SelectedItem) as DataRowView;
                        if (dr != null)
                        {
                            if (Convert.ToBoolean(dr.Row["IsChecked"]) == true)
                            {
                                tabControl2.IsEnabled = false;
                            }
                            else
                            {
                                tabControl2.IsEnabled = true;
                            }
                        }
                        break;
                    }
                }

            }
            else
            {

                if (Convert.ToInt32(CboOrden.EditValue) == 1 || Convert.ToInt32(CboOrden.EditValue) == 2)
                {
                    (sender as CheckBox).IsChecked = true;
                    return;
                }
                for (int i = 0; i < tblActividades.Rows.Count; i++)
                {
                    if (Convert.ToString((sender as CheckBox).Tag) == Convert.ToString(tblActividades.Rows[i]["Actividad"]))
                    {
                        tblActividades.Rows[i]["IsChecked"] = false;
                        //Activar las actividades ya asignadas
                        for (int j = 0; j < tblTareas.Rows.Count; j++)
                        {
                            if (IdOT == 0)
                            {
                                if (Convert.ToInt32(tblTareas.Rows[j]["IdPerfilCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdPerfilCompActividad"]))
                                {
                                    tblTareas.Rows[j]["FlagActivo"] = true;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(tblTareas.Rows[j]["IdOTCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdOTCompActividad"]))
                                {
                                    tblTareas.Rows[j]["FlagActivo"] = true;
                                }
                            }
                        }

                        for (int j = 0; j < tblHerrEsp.Rows.Count; j++)
                        {
                            if (IdOT == 0)
                            {
                                if (Convert.ToInt32(tblHerrEsp.Rows[j]["IdPerfilCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdPerfilCompActividad"]))
                                {
                                    tblHerrEsp.Rows[j]["FlagActivo"] = true;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(tblHerrEsp.Rows[j]["IdOTCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdOTCompActividad"]))
                                {
                                    tblHerrEsp.Rows[j]["FlagActivo"] = true;
                                }
                            }
                        }

                        for (int j = 0; j < tblConsumible.Rows.Count; j++)
                        {
                            if (IdOT == 0)
                            {
                                if (Convert.ToInt32(tblConsumible.Rows[j]["IdPerfilCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdPerfilCompActividad"]))
                                {
                                    tblConsumible.Rows[j]["FlagActivo"] = true;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(tblConsumible.Rows[j]["IdOTCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdOTCompActividad"]))
                                {
                                    tblConsumible.Rows[j]["FlagActivo"] = true;
                                }
                            }
                        }

                        for (int j = 0; j < tblRepuesto.Rows.Count; j++)
                        {
                            if (IdOT == 0)
                            {
                                if (Convert.ToInt32(tblRepuesto.Rows[j]["IdPerfilCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdPerfilCompActividad"]))
                                {
                                    tblRepuesto.Rows[j]["FlagActivo"] = true;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(tblRepuesto.Rows[j]["IdOTCompActividad"]) == Convert.ToInt32(tblActividades.Rows[i]["IdOTCompActividad"]))
                                {
                                    tblRepuesto.Rows[j]["FlagActivo"] = true;
                                }
                            }
                        }
                        DataRowView dr = (lstboxActividad.SelectedItem) as DataRowView;
                        if (dr != null)
                        {
                            if (Convert.ToBoolean(dr.Row["IsChecked"]) == true)
                            {
                                tabControl2.IsEnabled = false;
                            }
                            else
                            {
                                tabControl2.IsEnabled = true;
                            }
                        }
                        break;
                    }
                }
            }

        }

        private void trvComp_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                LblSelected.Content = "Seleccionar una actividad...";
                if (trvComp.SelectedItem == null) return;

                TreeViewModelCompOT trm = (TreeViewModelCompOT)trvComp.SelectedItem;
                int idPerfilComp = Convert.ToInt32(trm.IdMenu);
                int idotComp = Convert.ToInt32(trm.IdOTComp);

                if (IdOT == 0)
                {
                    int CantExisteDatos = tblActividades.Select("IdPerfilComp = " + idPerfilComp.ToString()).Length;
                    if (CantExisteDatos != 0)
                    {
                        DataView dtvActividades = tblActividades.DefaultView;
                        dtvActividades.RowFilter = "IdPerfilComp = " + idPerfilComp.ToString();
                        lstboxActividad.ItemsSource = dtvActividades;
                    }
                    else
                    {
                        CboPerfilAC.SelectedIndexChanged -= new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
                        //CboPerfilAC.SelectedIndex = -1;
                        CboPerfilAC.SelectedIndexChanged += new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
                        lstboxActividad.ItemsSource = null;
                    }
                }
                else
                {
                    int CantExisteDatos = tblActividades.Select("IdOTComp = " + idotComp.ToString()).Length;
                    if (CantExisteDatos != 0)
                    {
                        DataView dtvActividades = tblActividades.DefaultView;
                        dtvActividades.RowFilter = "IdOTComp = " + idotComp.ToString();
                        lstboxActividad.ItemsSource = dtvActividades;
                    }
                    else
                    {
                        CboPerfilAC.SelectedIndexChanged -= new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
                        //CboPerfilAC.SelectedIndex = -1;
                        CboPerfilAC.SelectedIndexChanged += new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
                        lstboxActividad.ItemsSource = null;
                    }
                }
                grvTarea.ItemsSource = false;
                grvConsumible.ItemsSource = false;
                grvHerrpEsp.ItemsSource = false;
                grvRepuesto.ItemsSource = false;
            }
            catch
            {
            }
        }
        private void btnAbrirActividad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewModelCompOT trm = (TreeViewModelCompOT)trvComp.SelectedItem;
                if (chkSinUnidadControl.IsChecked == false)
                {

                    if (trm == null || trm.IdMenu == 0 || trm.IdMenuPadre == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_COMP"), 2);
                        return;
                    }
                    int idPerfilComp = Convert.ToInt32(trm.IdMenu);
                    int idOTComp = Convert.ToInt32(trm.IdOTComp);
                    //Abrir Actividades q no estan agregadas
                    string idactividads = " '";
                    for (int i = 0; i < tblActividades.Rows.Count; i++)
                    {
                        if (IdOT == 0)
                        {
                            if (Convert.ToInt32(tblActividades.Rows[i]["IdPerfilComp"]) == idPerfilComp)
                            {
                                idactividads += tblActividades.Rows[i]["IdActividad"].ToString() + "','";
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(tblActividades.Rows[i]["IdOTComp"]) == idOTComp)
                            {
                                idactividads += tblActividades.Rows[i]["IdActividad"].ToString() + "','";
                            }
                        }
                    }
                    idactividads = idactividads.ToString().Substring(0, idactividads.Length - 2);

                    if (rbtManual.IsChecked == true)
                    {
                        //if (idactividads.Length > 0)
                        //{
                        //    DataView dtv = tblActividadCombo.DefaultView;
                        //    dtv.RowFilter = "IdActividad not in(" + idactividads + ")";
                        //    cboActividad.ItemsSource = dtv;
                        //}
                        //else
                        //{
                        //    cboActividad.ItemsSource = tblActividadCombo.DefaultView;
                        //}
                        //stkActividad.Visibility = System.Windows.Visibility.Visible;
                        //cboActividad.SelectedIndex = -1;
                        DataView dtv = tblActividadestmp.DefaultView;
                        if (tblActividadestmp.Rows.Count != 0)
                        {
                            dtv.RowFilter = "IdPerfilComp = '" + idPerfilComp + "' or IdPerfilComp = '0'";
                            if (idactividads.Length > 0)
                            {
                                dtv.RowFilter = "(IdPerfilComp = '" + idPerfilComp + "' or IdPerfilComp = '0' ) and IdActividad not in(" + idactividads + ")";
                            }
                        }
                        grvActividades.ItemsSource = dtv;
                        stckAgregarActividad.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {

                        if (trm == null)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_COMP"), 2);
                            return;
                        }
                        DataView dtv = tblActividadestmp.DefaultView;
                        if (tblActividadestmp.Rows.Count != 0)
                        {
                            dtv.RowFilter = "IdPerfilComp = '" + idPerfilComp + "'";
                            if (idactividads.Length > 0)
                            {
                                dtv.RowFilter = "IdPerfilComp = '" + idPerfilComp + "' and IdActividad not in(" + idactividads + ")";
                            }
                        }
                        grvActividades.ItemsSource = dtv;
                        stckAgregarActividad.Visibility = System.Windows.Visibility.Visible;
                    }
                }
                else
                {
                    tblActividadestmp.DefaultView.RowFilter = "";
                    DataView dtv = tblActividadestmp.DefaultView;
                    grvActividades.ItemsSource = dtv;
                    stckAgregarActividad.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void btnCancelarActividad_Click(object sender, RoutedEventArgs e)
        {
            stkActividad.Visibility = System.Windows.Visibility.Hidden;
            cboActividad.SelectedIndex = -1;
        }
        private void btnAceptarActividad_Click(object sender, RoutedEventArgs e)
        {
            DataRow row = tblActividades.NewRow();

            TreeViewModelCompOT trm = (TreeViewModelCompOT)trvComp.SelectedItem;
            if (trm == null)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_COMP"), 2);
                return;
            }

            int idComp = 0;
            if (IdOT == 0)
            {
                if (trm.IdMenu != 0)
                {
                    idComp = Convert.ToInt32(trm.IdMenu);
                }
                else
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_COMP"), 2);
                    return;
                }
            }
            else
            {
                idComp = idComp = Convert.ToInt32(trm.IdOTComp);
            }
            int IdOTCompActividad = 0;
            if (tblActividades.Rows.Count == 0)
            {
                IdOTCompActividad = 1;
            }
            else
            {
                IdOTCompActividad = Convert.ToInt32(tblActividades.Compute("max(IdOTCompActividad)", "")) + 1;
            }
            row["IdOTCompActividad"] = IdOTCompActividad;

            row["IdOTComp"] = idComp;
            row["IdPerfilComp"] = idComp;

            int IdPerfilCompActividad = 0;
            if (tblActividades.Rows.Count == 0)
            {
                IdPerfilCompActividad = 1;
            }
            else
            {
                IdPerfilCompActividad = Convert.ToInt32(tblActividades.Compute("max(IdPerfilCompActividad)", "")) + 1;
            }
            row["IdPerfilCompActividad"] = IdPerfilCompActividad;
            row["IdActividad"] = cboActividad.EditValue;
            row["Actividad"] = cboActividad.Text;
            if (Convert.ToInt32(CboOrden.EditValue) == 2)
            {
                row["IsChecked"] = true;
            }
            else
            {
                row["IsChecked"] = false;
            }
            row["FlagUso"] = true;
            row["FlagActivo"] = true;
            row["Nuevo"] = true;
            tblActividades.Rows.Add(row);

            stkActividad.Visibility = System.Windows.Visibility.Hidden;
            cboActividad.SelectedIndex = -1;


            int idPerfilComp = Convert.ToInt32(trm.IdMenu);
            int idotComp = Convert.ToInt32(trm.IdOTComp);

            if (IdOT == 0)
            {
                DataView dtvActividades = tblActividades.DefaultView;
                dtvActividades.RowFilter = "IdPerfilComp = " + idPerfilComp.ToString();
                lstboxActividad.ItemsSource = dtvActividades;
            }
            else
            {
                DataView dtvActividades = tblActividades.DefaultView;
                dtvActividades.RowFilter = "IdOTComp = " + idotComp.ToString();
                lstboxActividad.ItemsSource = dtvActividades;
            }

        }
        private void dtgOT_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgOT.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodOT")
                    {
                        e.Handled = true;
                        LimpiarForm();
                        //dtgOT.ItemsSource = tblTempOT.DefaultView;
                        GlobalClass.ip.SeleccionarTab(tabListadoOT);
                        //tabControl1.SelectedIndex = 0;
                        //ListarOT();
                        IdOT = 0;
                        IdPerfilCompActividad = 0;
                        IdOTCompActividad = 0;
                        //tabItem3.IsEnabled = false; //Tab Javier
                        //tabItem9.IsEnabled = false; //Tab Javier
                        IdPerfilCompMax = 1000 + objB_OT.PerfilCompActividad_Max();

                        CboPerfil.SelectedIndexChanged -= new RoutedEventHandler(CboPerfil_SelectedIndexChanged);
                        CboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);
                        CboPerfilAC.SelectedIndexChanged -= new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
                        rbtPM.Checked -= new RoutedEventHandler(rbtPM_Checked);
                        rbtManual.Checked -= new RoutedEventHandler(rbtManual_Checked);
                        rbtPerfil.Checked -= new RoutedEventHandler(rbtPerfil_Checked);

                        IdOT = Convert.ToInt32(dtgOT.GetCellDisplayText(tblvOT.FocusedRowHandle, "IdOT"));
                        gintIdOT = IdOT;
                        //Listar Datos Cabecera
                        objE_OT = new E_OT();
                        objE_OT.IdOT = IdOT;
                        DataTable tblOT = objB_OT.OT_Get(objE_OT);
                        int CantidadReprogr = objB_OT.OTReprog_Count(objE_OT);
                        if (tblOT.Rows.Count > 0)
                        {
                            LblNumeroDocumento.Text = tblOT.Rows[0]["CodOT"].ToString();
                            dtpFechaProgram.EditValue = Convert.ToDateTime(tblOT.Rows[0]["FechaProg"]);
                            dtpFechaLiberacion.Text = tblOT.Rows[0]["FechaLiber"].ToString();
                            dtpFechaCierre.Text = tblOT.Rows[0]["FechaCierre"].ToString();
                            CboOrden.EditValue = Convert.ToInt32(tblOT.Rows[0]["IdTipoOT"]);
                            CboEstado.EditValue = Convert.ToInt32(tblOT.Rows[0]["IdEstadoOT"]);
                            CboResponsable.EditValue = Convert.ToInt32(tblOT.Rows[0]["CodResponsable"]);
                            CboGeneracion.EditValue = Convert.ToInt32(tblOT.Rows[0]["IdTipoGeneracion"]);
                            txtComentario.Text = tblOT.Rows[0]["Observacion"].ToString();
                            txtMotivoPostergacion.Text = tblOT.Rows[0]["MotivoPostergacion"].ToString();
                            chkSinUnidadControl.IsChecked = Convert.ToBoolean(tblOT.Rows[0]["FlagSinUC"]);

                            lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblOT.Rows[0]["UsuarioCreacion"].ToString(), tblOT.Rows[0]["FechaCreacion"].ToString(), tblOT.Rows[0]["HostCreacion"].ToString());
                            lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblOT.Rows[0]["UsuarioModificacion"].ToString(), tblOT.Rows[0]["FechaModificacion"].ToString(), tblOT.Rows[0]["HostModificacion"].ToString());

                            if (CantidadReprogr > 0)
                            {
                                lblCantReprog.Content = "(" + CantidadReprogr.ToString() + ")";
                            }
                            else
                            {
                                lblCantReprog.Content = "";
                            }
                            //Bloquear Controles
                            //rbtManual.IsChecked = true;
                            chkSinUnidadControl.IsEnabled = false;
                            if (Convert.ToBoolean(tblOT.Rows[0]["FlagSinUC"]) == true)
                            {
                                CboPerfil.IsEnabled = false;
                                CboPerfil.SelectedIndex = -1;
                                CboUnidadControl.IsEnabled = false;
                                CboUnidadControl.ItemsSource = null;
                                rbtPM.IsEnabled = false;
                                rbtPerfil.IsEnabled = false;
                                CboPerfilAC.IsEnabled = false;
                                CboPerfilAC.ItemsSource = null;
                                rbtManual.IsChecked = true;
                                CboPerfilAC.SelectedIndex = -1;
                                cboTarea.ItemsSource = null;
                                lstboxActividad.ItemsSource = null;
                                //trvComp.IsEnabled = false;
                                CboUnidadControl.ItemsSource = objB_OT.Item_ListSinUC();
                                CboUnidadControl.DisplayMember = "DescripcionSAP";
                                CboUnidadControl.ValueMember = "IdItem";
                                CboUnidadControl.EditValue = Convert.ToInt32(tblOT.Rows[0]["IdItem"]);

                            }
                            else
                            {
                                CboPerfil.IsEnabled = true;
                                CboPerfil.Focus();
                                CboPerfil.SelectedIndex = -1;
                                CboUnidadControl.IsEnabled = true;
                                CboUnidadControl.ItemsSource = null;
                                rbtPM.IsEnabled = true;
                                rbtPerfil.IsEnabled = true;
                                CboPerfilAC.IsEnabled = true;
                                CboPerfilAC.ItemsSource = null;
                                rbtPM.IsChecked = true;
                                CboPerfilAC.SelectedIndex = -1;
                                cboTarea.ItemsSource = null;
                                lstboxActividad.ItemsSource = null;
                                //trvComp.IsEnabled = true;
                                CboPerfil.EditValue = Convert.ToInt32(tblOT.Rows[0]["IdPerfil"]);
                                E_UC objEUC = new E_UC();
                                B_UC objBUC = new B_UC();
                                int IdPerfil = Convert.ToInt32(CboPerfil.EditValue);
                                objEUC.IdPerfil = IdPerfil;
                                CboUnidadControl.ItemsSource = objBUC.B_UC_Combo(objEUC);
                                CboUnidadControl.DisplayMember = "PlacaSerie";
                                CboUnidadControl.ValueMember = "CodUC";
                                CboUnidadControl.EditValue = Convert.ToString(tblOT.Rows[0]["CodUC"]);
                            }
                        }

                        CboPerfil.IsEnabled = false;
                        CboUnidadControl.IsEnabled = false;

                        //trvComp.IsEnabled = false; -- Desactivar evento del treeview

                        DataTable tblActividadDatos = objB_OT.OTActividad_Combo(objE_OT);
                        //trvComp.SelectedValuePath = "2";
                        for (int i = 0; i < tblActividadDatos.Rows.Count; i++)
                        {
                            DataRow row = tblActividades.NewRow();
                            row["IdOTCompActividad"] = Convert.ToInt32(tblActividadDatos.Rows[i]["IdOTCompActividad"]);
                            row["IdOTComp"] = Convert.ToInt32(tblActividadDatos.Rows[i]["IdOTComp"]);
                            row["IdPerfilCompActividad"] = 0;
                            row["IdActividad"] = Convert.ToInt32(tblActividadDatos.Rows[i]["IdActividad"]);
                            row["Actividad"] = tblActividadDatos.Rows[i]["Actividad"].ToString();
                            row["IsChecked"] = Convert.ToBoolean(tblActividadDatos.Rows[i]["FlagExterna"]);
                            row["FlagUso"] = true;
                            row["FlagActivo"] = Convert.ToBoolean(tblActividadDatos.Rows[i]["FlagActivo"]);
                            row["Nuevo"] = false;
                            row["IdPerfilComp"] = Convert.ToInt32(tblActividadDatos.Rows[i]["IdPerfilComp"]);
                            tblActividades.Rows.Add(row);
                        }
                        //lstboxActividad.ItemsSource = tblActividades.DefaultView;
                        ListarOTComp();
                        //Llenar tabla tarea

                        DataTable tblTareaDatos = objB_OT.OTTarea_Combo(objE_OT);
                        for (int i = 0; i < tblTareaDatos.Rows.Count; i++)
                        {
                            DataRow row = tblTareas.NewRow();
                            row["IdOTTarea"] = 0;
                            row["IdOTCompActividad"] = Convert.ToDouble(tblTareaDatos.Rows[i]["IdOTCompActividad"]);
                            row["IdPerfilCompActividad"] = 0;
                            row["IdTarea"] = Convert.ToDouble(tblTareaDatos.Rows[i]["IdTarea"]);
                            row["CodResponsable"] = CboResponsable.EditValue;
                            row["CostoHoraHombre"] = 0;
                            RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                            OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("R", ref RPTA);
                            if (RPTA.ResultadoRetorno == 0)
                            {
                                for (int j = 0; j < OHEMlist.Count(); j++)
                                {
                                    if (Convert.ToString(CboResponsable.EditValue) == Convert.ToString(OHEMlist[j].CodigoPersona))
                                    {
                                        row["CostoHoraHombre"] = OHEMlist[j].CostoHoraHombre;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                            }
                            row["HorasEstimada"] = Convert.ToDouble(tblTareaDatos.Rows[i]["HorasEstimada"]);
                            row["HorasReal"] = 0;
                            row["OTTarea"] = tblTareaDatos.Rows[i]["Tarea"].ToString();
                            row["IdPerfilTarea"] = 0;
                            row["IdEstadoOTT"] = 1;
                            row["FlagAutomatico"] = true;
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                            tblTareas.Rows.Add(row);
                        }
                        //Llenar Herramienta

                        DataTable tblHerramientaDatos = objB_OT.OTHerramienta_Combo(objE_OT);

                        for (int i = 0; i < tblHerramientaDatos.Rows.Count; i++)
                        {
                            DataRow row = tblHerrEsp.NewRow();
                            row["IdOTHerramienta"] = 0;
                            row["IdOTCompActividad"] = Convert.ToInt32(tblHerramientaDatos.Rows[i]["IdOTCompActividad"]);
                            row["IdPerfilCompActividad"] = 0;
                            row["IdHerramienta"] = Convert.ToInt32(tblHerramientaDatos.Rows[i]["IdHerramienta"]); ;
                            row["Herramienta"] = Convert.ToString(tblHerramientaDatos.Rows[i]["Herramienta"]); ;
                            row["Cantidad"] = Convert.ToInt32(tblHerramientaDatos.Rows[i]["Cantidad"]); ;
                            row["FlagAutomatico"] = true;
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                            tblHerrEsp.Rows.Add(row);
                        }

                        //Llenar Articulo

                        DataTable tblArticuloDatos = objB_OT.OTArticulo_Combo(objE_OT);

                        for (int i = 0; i < tblArticuloDatos.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(tblArticuloDatos.Rows[i]["IdTipoArticulo"]) == 2)
                            {
                                DataRow row = tblRepuesto.NewRow();
                                row["IdOTArticulo"] = 0;
                                row["IdOTCompActividad"] = Convert.ToString(tblArticuloDatos.Rows[i]["IdOTCompActividad"]);
                                row["IdPerfilCompActividad"] = 0;
                                row["IdArticulo"] = Convert.ToString(tblArticuloDatos.Rows[i]["CodigoSAP"]); ;
                                row["Articulo"] = Convert.ToString(tblArticuloDatos.Rows[i]["DescripcionSAP"]); ;
                                row["CantSol"] = Convert.ToDouble(tblArticuloDatos.Rows[i]["CantSol"]); ;
                                row["FlagAutomatico"] = true;
                                row["FlagActivo"] = true;
                                row["Nuevo"] = false;
                                tblRepuesto.Rows.Add(row);
                            }
                            else
                            {
                                DataRow row = tblConsumible.NewRow();
                                row["IdOTArticulo"] = 0;
                                row["IdOTCompActividad"] = Convert.ToString(tblArticuloDatos.Rows[i]["IdOTCompActividad"]);
                                row["IdPerfilCompActividad"] = 0;
                                row["IdArticulo"] = Convert.ToString(tblArticuloDatos.Rows[i]["CodigoSAP"]);
                                row["Articulo"] = Convert.ToString(tblArticuloDatos.Rows[i]["DescripcionSAP"]);
                                row["CantSol"] = Convert.ToDouble(tblArticuloDatos.Rows[i]["CantSol"]);
                                row["IdTipoArticulo"] = Convert.ToDouble(tblArticuloDatos.Rows[i]["IdTipoArticulo"]);
                                row["FlagAutomatico"] = true;
                                row["FlagActivo"] = true;
                                row["Nuevo"] = false;
                                tblConsumible.Rows.Add(row);
                            }
                        }

                        //Verificar si tiene OTInformes
                        objE_OTIProv.IdOT = IdOT;
                        tblOTInforme = objB_OTIProv.OTInforme_List(objE_OTIProv);
                        int CantExiste = tblOTInforme.Select("IdOT = " + IdOT).Length;
                        if (CantExiste > 0)
                        {
                            EstadoForm(false, false, true);
                            treeListView2.AllowEditing = false;
                            txtRIComentarios.IsReadOnly = true;
                            DtpRIFechCierre.IsReadOnly = true;
                            btnCargarDoc.IsEnabled = false;

                            gbolExisteInforme = true;
                            //tbRegInfo.IsEnabled = true; //Tab Javier
                            //btnCargarDoc.Visibility = Visibility.Hidden;

                            //btnDescDoc.Visibility = Visibility.Visible;
                            txbLinkCarga.Visibility = Visibility.Hidden;
                            txbLinkDescarga.Visibility = Visibility.Visible;

                            LlenarConsultaRegInforme();
                            CambiarBotonesRegistroPRoveedor(true);
                            ActualizarEstadoOTRIP.IsEnabled = (Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdEstadoOT")) != 5);
                        }
                        else
                        {
                            gbolExisteInforme = false;
                            ActualizarEstadoOTRIP.IsEnabled = false;
                        }

                        dtpFechaProgram.IsEnabled = false;
                        CboOrden.IsEnabled = false;
                        tabDetallesOT.Header = "Modificar OT";

                        ListarDatosRegistraTarea();
                        BloquearControlRegistroTarea();
                        CboPerfil.SelectedIndexChanged += new RoutedEventHandler(CboPerfil_SelectedIndexChanged);
                        CboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);
                        CboPerfilAC.SelectedIndexChanged += new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
                        rbtPM.Checked += new RoutedEventHandler(rbtPM_Checked);
                        rbtManual.Checked += new RoutedEventHandler(rbtManual_Checked);
                        rbtPerfil.Checked += new RoutedEventHandler(rbtPerfil_Checked);
                        //tabItem3.IsEnabled = true; //Tab Javier
                        //tabItem9.IsEnabled = true; //Tab Javier
                        BtnGrabarOT.Content = "Actualizar";

                        if (Convert.ToInt32(tblOT.Rows[0]["IdEstadoOT"]) == 1 || Convert.ToInt32(tblOT.Rows[0]["IdEstadoOT"]) == 2)
                        {
                            CboResponsable.IsEnabled = true;
                            DesbloquearForm();
                        }
                        else if (Convert.ToInt32(tblOT.Rows[0]["IdEstadoOT"]) == 5)
                        {
                            CboResponsable.IsEnabled = false;
                            BloquearForm();
                        }
                        else
                        {
                            CboResponsable.IsEnabled = false;
                            DesbloquearForm();
                        }

                        GrabarSolPendienteSAP(IdOT, LblNumeroDocumento.Text);

                        tabItem4.IsEnabled = true;
                        //tabItem3.IsEnabled = true; //Tab Javier

                        FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();

                        if (gbolExisteTarea && gbolExisteInforme)
                        {
                            GlobalClass.ip.SeleccionarTab(tabDetallesOT, tabTareasOT, tabInformesOT);
                        }
                        else if (gbolExisteTarea)
                        {
                            GlobalClass.ip.SeleccionarTab(tabDetallesOT, tabTareasOT);
                        }
                        else if (gbolExisteInforme)
                        {
                            GlobalClass.ip.SeleccionarTab(tabDetallesOT, tabInformesOT);
                        }
                        else
                        {
                            GlobalClass.ip.SeleccionarTab(tabDetallesOT);
                        }
                        //tabControl1.SelectedIndex = 1; //Tab Javier
                        tabControl5.SelectedIndex = 0;
                        tabControl2.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        Boolean gbolIsRegNroSeries = false;
        Boolean gbolIsOTMod = false;
        private void BtnGrabarOT_Click(object sender, RoutedEventArgs e)
        {
            string CodOT = "";
            try
            {
                if (ValidaGrabacion() == false) { return; }
                if (ValidaTipoCambio() == false) { return; }
                if (ValidaAlmacenArticulo(tblRepuesto, tblConsumible) == false) { return; }
                gbolIsOTMod = false;
                if (!gbolIsRegNroSeries)
                {
                    tblNroSerie = new DataTable();
                    tblNroSerie = objB_OT.OTHerramienta_GetTreeVieNrSeriesByIdHerramienta(tblHerrEsp);
                    if (tblNroSerie.Rows.Count > 0)
                    {
                        dtgNroSerie.ItemsSource = tblNroSerie;
                        dtgNroSerie.Columns["IdHerramientaItem"].Visible = false;
                        dtgNroSerie.Columns["IdOT"].Visible = false;
                        dtgNroSerie.Columns["IdOTCompActividad"].Visible = false;
                        dtgNroSerie.Columns["CantidadDis"].Visible = false;
                        dtgNroSerie.Columns["IsChecked"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                        dtgNroSerie.Columns["IsChecked"].VisibleIndex = tblNroSerie.Columns.Count - 1;
                        dtgNroSerie.Columns["CantidadSol"].Header = "Cantidad Solicitada";
                        dtgNroSerie.Columns["Codigo"].Header = "Código OT";
                        dtgNroSerie.GroupBy("Codigo");
                        dtgNroSerie.GroupBy("Herramienta");
                        dtgNroSerie.GroupBy("Actividad");
                        dtgNroSerie.GroupBy("CantidadSol");
                        dtgNroSerie.ExpandAllGroups();
                        stkHerramientasSeries.Visibility = Visibility.Visible;
                        gbolIsOTMod = true;
                        return;
                    }
                }


                //Simplicar data de tabla
                DataTable tblComponentes = tblOTComp;
                for (int i = 0; i < tblOTComp.Rows.Count; i++)
                {
                    if (IdOT == 0)
                    {
                        tblOTComp.Rows[i]["Nuevo"] = true;
                    }
                    if (Convert.ToInt32(tblOTComp.Rows[i]["IdPerfilCompPadre"]) == 1000 && chkSinUnidadControl.IsChecked == false)
                    {
                        tblOTComp.Rows.RemoveAt(i);
                        i--;
                    }
                }

                //Actividades
                for (int i = 0; i < tblActividades.Rows.Count; i++)
                {
                    if (IdOT == 0)
                    {
                        tblActividades.Rows[i]["Nuevo"] = true;
                    }
                }

                //Tarea9
                for (int i = 0; i < tblTareas.Rows.Count; i++)
                {
                    if (IdOT == 0)
                    {
                        tblTareas.Rows[i]["Nuevo"] = true;
                    }
                }
                //Her
                for (int i = 0; i < tblHerrEsp.Rows.Count; i++)
                {
                    if (IdOT == 0)
                    {
                        tblHerrEsp.Rows[i]["Nuevo"] = true;
                    }
                }
                //Cons
                for (int i = 0; i < tblConsumible.Rows.Count; i++)
                {
                    if (IdOT == 0)
                    {
                        tblConsumible.Rows[i]["Nuevo"] = true;
                    }
                    tblConsumible.Rows[i]["IdTipoArticulo"] = 3;
                }
                //Rep
                for (int i = 0; i < tblRepuesto.Rows.Count; i++)
                {
                    if (IdOT == 0)
                    {
                        tblRepuesto.Rows[i]["Nuevo"] = true;
                    }
                    tblRepuesto.Rows[i]["IdTipoArticulo"] = 2;
                }

                E_UC objEUC = new E_UC();
                B_UC objBUC = new B_UC();
                int IdPerfil = Convert.ToInt32(CboPerfil.EditValue);
                objEUC.IdPerfil = IdPerfil;
                //Grabar OT
                //Setear Entidad OT

                objE_OT = new E_OT();
                objE_OT.IdOT = IdOT;
                objE_OT.NombreOT = "";
                objE_OT.IdTipoOT = Convert.ToInt32(CboOrden.EditValue);
                objE_OT.FlagSinUC = Convert.ToInt32(chkSinUnidadControl.IsChecked);
                if (chkSinUnidadControl.IsChecked == false)
                {
                    objE_OT.IdUC = 0;
                    DataView dtvUC = objBUC.B_UC_Combo(objEUC).DefaultView;
                    dtvUC.RowFilter = "CodUC like '" + CboUnidadControl.EditValue + "'";
                    if (dtvUC.Count > 0)
                    {
                        objE_OT.IdUC = Convert.ToInt32(dtvUC[0]["IdUC"]);
                    }
                }
                objE_OT.FechaProg = Convert.ToDateTime(dtpFechaProgram.EditValue);
                //objE_OT.FechaLiber = Convert.ToDateTime(dtpFechaLiberacion.Text);
                //objE_OT.FechaCierre = Convert.ToDateTime(dtpFechaCierre.Text);
                objE_OT.CodResponsable = Convert.ToString(CboResponsable.EditValue);
                objE_OT.NombreResponsable = Convert.ToString(CboResponsable.Text);
                objE_OT.IdTipoGeneracion = Convert.ToInt32(CboGeneracion.EditValue);
                objE_OT.IdEstadoOT = Convert.ToInt32(CboEstado.EditValue);
                objE_OT.MotivoPostergacion = Convert.ToString(txtMotivoPostergacion.Text);
                objE_OT.Observacion = txtComentario.Text;
                objE_OT.FlagActivo = 1;
                objE_OT.IdUsuario = gintIdUsuario;
                //Añadir componente con flagUso
                tblActividades.DefaultView.RowFilter = "FlagUso = 1 and Nuevo = 1";
                DataView dtvActividadUso = tblActividades.DefaultView;
                if (dtvActividadUso.Count > 0)
                {
                    DataRow row = tblConsumible.NewRow();

                    row["IdOTArticulo"] = 0;
                    row["IdOTCompActividad"] = dtvActividadUso[0]["IdOTCompActividad"];
                    row["IdPerfilCompActividad"] = dtvActividadUso[0]["IdPerfilCompActividad"];
                    row["IdTipoArticulo"] = 4;

                    if (IdOT == 0)
                    {
                        row["IdArticulo"] = tblOTComp.Select("IdPerfilComp = " + dtvActividadUso[0]["IdPerfilComp"], "")[0]["CodigoSAP"].ToString();
                        row["Articulo"] = tblOTComp.Select("IdPerfilComp = " + dtvActividadUso[0]["IdPerfilComp"], "")[0]["DescripcionSAP"].ToString();
                    }
                    else
                    {
                        row["IdArticulo"] = tblOTComp.Select("IdOTComp = " + dtvActividadUso[0]["IdOTComp"], "")[0]["CodigoSAP"].ToString();
                        row["Articulo"] = tblOTComp.Select("IdOTComp = " + dtvActividadUso[0]["IdOTComp"], "")[0]["DescripcionSAP"].ToString();
                    }

                    row["CantSol"] = 1;
                    row["CantEnv"] = 0;
                    row["CantUti"] = 0;
                    row["Observacion"] = 0;
                    #region COSTO_ARTICULO_CREACION_OT
                    //row["CostoArticulo"] = 0;
                    int TipoProceso = 1;
                    int DocEntry = 0;
                    RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                    OITW_List = InterfazMTTO.iSBO_BL.Articulo_BL.ObtenerCostoArticulo(row["IdArticulo"].ToString(), TipoProceso, DocEntry, ref RPTA);
                    if (RPTA.ResultadoRetorno == 0)
                    {
                        row["CostoArticulo"] = Convert.ToDouble(OITW_List[0].CostoArticulo);
                    }
                    else
                    {
                        row["CostoArticulo"] = 0;
                    }
                    #endregion
                    row["FlagAutomatico"] = true;
                    row["FlagActivo"] = true;
                    row["Nuevo"] = true;
                    tblConsumible.Rows.Add(row);
                }
                objE_OT.FechaModificacion = (FechaModificacion.Year != 1) ? FechaModificacion : DateTime.Now;
                DataSet rpta = objB_OT.OT_UpdateCascada(objE_OT, tblOTComp, tblActividades, tblTareas, tblHerrEsp, tblRepuesto, tblConsumible);
                if (rpta.Tables.Count == 3)
                {
                    CodOT = rpta.Tables[0].Rows[0][0].ToString();
                    //Grabar Id Bitacora
                    objE_OT = new E_OT();
                    objE_OT.IdOT = Convert.ToInt32(rpta.Tables[0].Rows[0][1]);
                    objE_OT.IdUsuario = gintIdUsuario;
                    int xcant = objB_OT.PerfilCompActividad_Update(objE_OT);
                    GrabarSAP(rpta, tblActividades, tblRepuesto, tblConsumible, tblOTArticuloSol);
                }
                else
                {
                    tblConsumible.DefaultView.RowFilter = "IdTipoArticulo <> 4 and Nuevo = true";
                    tblConsumible = tblConsumible.DefaultView.ToTable();

                    if (Convert.ToInt32(rpta.Tables[0].Rows[0]["ERR_NUMBER"].ToString()) == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_MODI"), 2);
                    }
                    else if (Convert.ToInt32(rpta.Tables[0].Rows[0]["ERR_NUMBER"].ToString()) == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_CONC"), 2);
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje(rpta.Tables[0].Rows[0]["ERR_MESSAGE"].ToString(), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                //Eliminado lógico de OT
                objE_OT = new E_OT();
                objE_OT.CodOT = CodOT;//rpta.Tables[0].Rows[0][0].ToString();
                int x = objB_OT.OT_Delete(objE_OT);
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        public void GrabarSAP(DataSet rpta, DataTable tblActividadesSAP, DataTable tblRepuestoSAP, DataTable tblConsumibleSAP, DataTable tblOTArticuloSolSAP)
        {
            try
            {
                //Actualizar la tabla OTCompActividad_Estado
                if (IdOT == 0)
                {
                    string Ids = "";
                    for (int i = 0; i < tblActividadesSAP.Rows.Count; i++)
                    {
                        if (Convert.ToString(tblActividadesSAP.Rows[i]["IdOTCompActividad"]) != "" && Convert.ToString(tblActividadesSAP.Rows[i]["IdOTCompActividad"]) != "0")
                        {
                            Ids += Convert.ToString(tblActividadesSAP.Rows[i]["IdOTCompActividad"]) + ", ";
                        }
                    }

                    if (Ids.Length > 2)
                    {
                        objE_OT = new E_OT();
                        objE_OT.CodOT = rpta.Tables[0].Rows[0][0].ToString();
                        objE_OT.IdsOTCompActividadEstado = Ids.ToString().Substring(0, Ids.Length - 2);
                        int x = objB_OT.OTCompActividadEstado_Update(objE_OT);
                    }
                }

                GrabarSolPendienteSAP(Convert.ToInt32(rpta.Tables[0].Rows[0][1]), rpta.Tables[0].Rows[0][0].ToString());
                LimpiarForm();
                GlobalClass.ip.SeleccionarTab(tabListadoOT);
                //tabControl1.SelectedIndex = 0; //Tab Javier
                ListarOT();
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_OT"), 1);
                stkHerramientasSeries.Visibility = Visibility.Collapsed;
                gbolRegHerramientas = false;
                //LimpiarTreelistNodos(treeListView1.Nodes); XAVI
            }
            catch (Exception ex)
            {
                //Eliminado lógico de OT
                objE_OT = new E_OT();
                objE_OT.CodOT = rpta.Tables[0].Rows[0][0].ToString();
                int x = objB_OT.OT_Delete(objE_OT);
                //return;
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }

        private void CboResponsable_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
            OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("S", ref RPTA);
            double CostoHorasHombre = 0;
            if (RPTA.ResultadoRetorno == 0)
            {
                for (int i = 0; i < OHEMlist.Count(); i++)
                {
                    if (Convert.ToString(CboResponsable.EditValue) == Convert.ToString(OHEMlist[i].CodigoPersona))
                    {
                        CostoHorasHombre = OHEMlist[i].CostoHoraHombre;
                    }
                }
            }
            else
            {
                GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
            }

            for (int i = 0; i < tblTareas.Rows.Count; i++)
            {
                tblTareas.Rows[i]["CodResponsable"] = CboResponsable.EditValue;
                tblTareas.Rows[i]["CostoHoraHombre"] = CostoHorasHombre;
            }
            ActivaDetalleOT();
        }
        private void LimpiarForm()
        {
            CboPerfil.SelectedIndexChanged -= new RoutedEventHandler(CboPerfil_SelectedIndexChanged);
            CboPerfilAC.SelectedIndexChanged -= new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
            //Limpiar Cabezera
            dtpFechaProgram.IsEnabled = true;
            LblNumeroDocumento.Text = "OTNuevo";
            dtpFechaProgram.EditValue = DateTime.Now;
            dtpFechaLiberacion.Text = "";
            dtpFechaCierre.Text = "";
            CboOrden.SelectedIndex = -1;
            CboEstado.SelectedIndex = -1;
            CboResponsable.SelectedIndex = -1;
            CboGeneracion.SelectedIndex = -1;
            txtComentario.Text = "";
            txtMotivoPostergacion.Text = "";
            lblCantReprog.Content = "";
            //Limpiar Detalle
            chkSinUnidadControl.IsChecked = false;
            CboPerfil.SelectedIndex = -1;
            CboUnidadControl.ItemsSource = null;
            CboPerfilAC.ItemsSource = null;
            rbtManual.IsChecked = false;
            rbtPerfil.IsChecked = false;
            rbtPM.IsChecked = false;
            rbtManual.IsEnabled = true;
            rbtPerfil.IsEnabled = true;
            rbtPM.IsEnabled = true;

            trvComp.ItemsSource = null;
            grvTarea.ItemsSource = null;
            grvHerrpEsp.ItemsSource = null;
            grvRepuesto.ItemsSource = null;
            grvConsumible.ItemsSource = null;

            //Limpiar Tablas
            tblOTComp.Rows.Clear();
            tblActividades.Rows.Clear();
            tblTareas.Rows.Clear();
            tblRepuesto.Rows.Clear();
            tblHerrEsp.Rows.Clear();
            tblConsumible.Rows.Clear();

            lstboxActividad.ItemsSource = null;
            CboEstado.EditValue = 1;
            CboGeneracion.EditValue = 3;

            rbtManual.IsChecked = false;

            chkSinUnidadControl.IsEnabled = true;
            CboPerfil.IsEnabled = true;
            CboUnidadControl.IsEnabled = true;
            CboOrden.IsEnabled = true;
            tabDetallesOT.Header = "Creación de OT";
            lblUC.Content = "Unidad de control:";
            //tabItem3.IsEnabled = false; //Tab Javier
            //tabItem9.IsEnabled = false; //Tab Javier
            CboResponsable.IsEnabled = true;
            CboPerfil.SelectedIndexChanged += new RoutedEventHandler(CboPerfil_SelectedIndexChanged);
            CboPerfilAC.SelectedIndexChanged += new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
            LimpiarTareasRealizadas();
            DesbloquearForm();

        }
        private void btnCancelarOT_Click(object sender, RoutedEventArgs e)
        {
            LimpiarForm();
            GlobalClass.ip.SeleccionarTab(tabListadoOT);
            //tabControl1.SelectedIndex = 0; //Tab Javier
            ListarOT();
            IdOT = 0;
            IdPerfilCompActividad = 0;
            IdOTCompActividad = 0;
            //tabItem3.IsEnabled = false; //Tab Javier
            //tabItem9.IsEnabled = false; //Tab Javier
        }
        private void txtHorasTarea_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //txtHorasTarea.Text = Utilitarios.Utilitarios.SoloNumeroDecimal(txtHorasTarea.Text);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void txtCantHerrEsp_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textboxSender = (TextBox)sender;
                var cursorPosition = textboxSender.SelectionStart;
                textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9]", "");
                textboxSender.SelectionStart = cursorPosition;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void txtCantRepuesto_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textboxSender = (TextBox)sender;
                var cursorPosition = textboxSender.SelectionStart;
                textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9]", "");
                textboxSender.SelectionStart = cursorPosition;

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void txtCantConsumible_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textboxSender = (TextBox)sender;
                var cursorPosition = textboxSender.SelectionStart;
                textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9]", "");
                textboxSender.SelectionStart = cursorPosition;

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private bool ValidaGrabacion()
        {
            bool val = true;

            try
            {
                Convert.ToDateTime(dtpFechaProgram.EditValue);
            }
            catch
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_FECH_PROG"), 2);
                val = false;
                return val;
            }

            tblFechHoraServ = Utilitarios.Utilitarios.Fecha_Hora_Servidor();
            DateTime dtFechaServer = Convert.ToDateTime(Convert.ToDateTime(tblFechHoraServ.Rows[0]["FechaServer"]).ToString("dd/MM/yyyy"));
            if (IdOT == 0)
            {
                if (Convert.ToDateTime(dtpFechaProgram.EditValue) < dtFechaServer)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_FECH_PROG_OT"), 2);
                    val = false;
                    return val;
                }
            }

            if (CboEstado.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_ESTA"), 2);
                val = false;
                return val;
            }

            if (CboOrden.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_TIPO"), 2);
                val = false;
                return val;
            }

            if (CboResponsable.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RESP"), 2);
                val = false;
                return val;
            }


            if (tblActividades.Rows.Count == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_ACTI_ASIG"), 2);
                val = false;
                return val;
            }


            if (tblTareas.Rows.Count == 0 && Convert.ToInt32(CboOrden.EditValue) != 2)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_TARE_ASIG"), 2);
                val = false;
                return val;
            }


            return val;
        }
        private void lstboxActividad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //MessageBox.Show(lstboxActividad.Tag.ToString());
                DataRowView dr = (lstboxActividad.SelectedItem) as DataRowView;

                if (dr == null) return;

                objETarea.IdActividad = 0;
                objETarea.Actividad = dr.Row["Actividad"].ToString();
                LblSelected.Content = dr.Row["Actividad"].ToString();
                int IdOTComp = Convert.ToInt32(dr.Row["IdOTComp"]);
                cboTarea.ItemsSource = objTarea.Tarea_ComboByAct(objETarea);
                cboTarea.DisplayMember = "Tarea";
                cboTarea.ValueMember = "IdTarea";

                if (Convert.ToBoolean(dr.Row["IsChecked"]) == true)
                {
                    tabControl2.IsEnabled = false;
                }
                else
                {
                    tabControl2.IsEnabled = true;
                }
                lstboxActividad.ItemsSource = tblActividades.DefaultView;
                //Encontrar IdPerfilCompActividad por nombre de actividad
                int IdPerCompActividad = 0;
                int IdOTCompActividads = 0;
                for (int i = 0; i < tblActividades.Rows.Count; i++)
                {
                    if ((Convert.ToString(dr.Row["Actividad"]) == Convert.ToString(tblActividades.Rows[i]["Actividad"]) &&
                        (Convert.ToString(dr.Row["IdOTComp"]) == Convert.ToString(tblActividades.Rows[i]["IdOTComp"]) && Convert.ToString(dr.Row["IdOTComp"]) != "0"))
                        ||
                        (Convert.ToString(dr.Row["Actividad"]) == Convert.ToString(tblActividades.Rows[i]["Actividad"]) &&
                        (Convert.ToString(dr.Row["IdPerfilComp"]) == Convert.ToString(tblActividades.Rows[i]["IdPerfilComp"]) && Convert.ToString(dr.Row["IdOTComp"]) == "0")))
                    {
                        IdPerCompActividad = Convert.ToInt32(tblActividades.Rows[i]["IdPerfilCompActividad"]);
                        IdOTCompActividads = Convert.ToInt32(tblActividades.Rows[i]["IdOTCompActividad"]);
                        break;
                    }
                }
                IdPerfilCompActividad = IdPerCompActividad;
                IdOTCompActividad = IdOTCompActividads;
                if (IdOT == 0)
                {

                    //Listar Tareas
                    DataView dtvTarea = tblTareas.DefaultView;
                    dtvTarea.RowFilter = "IdPerfilCompActividad = " + IdPerCompActividad.ToString();
                    grvTarea.ItemsSource = dtvTarea;

                    //Listar Herramientas Especiales
                    DataView dtvHerEsp = tblHerrEsp.DefaultView;
                    dtvHerEsp.RowFilter = "IdPerfilCompActividad = " + IdPerCompActividad.ToString();
                    grvHerrpEsp.ItemsSource = dtvHerEsp;

                    //Listar Consumible
                    DataView dtvConsumible = tblConsumible.DefaultView;
                    dtvConsumible.RowFilter = "IdTipoArticulo = 3 and IdPerfilCompActividad = " + IdPerCompActividad.ToString();
                    grvConsumible.ItemsSource = dtvConsumible;

                    //Listar Repuesto
                    DataView dtvRepuesto = tblRepuesto.DefaultView;
                    dtvRepuesto.RowFilter = "IdPerfilCompActividad = " + IdPerCompActividad.ToString();
                    grvRepuesto.ItemsSource = dtvRepuesto;
                }
                else
                {
                    if (IdOTCompActividad != 0)
                    {
                        //Listar Tareas
                        DataView dtvTarea = tblTareas.DefaultView;
                        dtvTarea.RowFilter = "IdOTCompActividad = " + IdOTCompActividad.ToString();
                        grvTarea.ItemsSource = dtvTarea;

                        //Listar Herramientas Especiales
                        DataView dtvHerEsp = tblHerrEsp.DefaultView;
                        dtvHerEsp.RowFilter = "IdOTCompActividad = " + IdOTCompActividad.ToString();
                        grvHerrpEsp.ItemsSource = dtvHerEsp;

                        //Listar Consumible
                        DataView dtvConsumible = tblConsumible.DefaultView;
                        dtvConsumible.RowFilter = "IdTipoArticulo = 3 and IdOTCompActividad = " + IdOTCompActividad.ToString();
                        grvConsumible.ItemsSource = dtvConsumible;

                        //Listar Repuesto
                        DataView dtvRepuesto = tblRepuesto.DefaultView;
                        dtvRepuesto.RowFilter = "IdOTCompActividad = " + IdOTCompActividad.ToString();
                        grvRepuesto.ItemsSource = dtvRepuesto;
                    }
                    else
                    {
                        //Listar Tareas
                        DataView dtvTarea = tblTareas.DefaultView;
                        dtvTarea.RowFilter = "IdPerfilCompActividad = " + IdPerCompActividad.ToString();
                        grvTarea.ItemsSource = dtvTarea;

                        //Listar Herramientas Especiales
                        DataView dtvHerEsp = tblHerrEsp.DefaultView;
                        dtvHerEsp.RowFilter = "IdPerfilCompActividad = " + IdPerCompActividad.ToString();
                        grvHerrpEsp.ItemsSource = dtvHerEsp;

                        //Listar Consumible
                        DataView dtvConsumible = tblConsumible.DefaultView;
                        dtvConsumible.RowFilter = "IdTipoArticulo = 3 and IdPerfilCompActividad = " + IdPerCompActividad.ToString();
                        grvConsumible.ItemsSource = dtvConsumible;

                        //Listar Repuesto
                        DataView dtvRepuesto = tblRepuesto.DefaultView;
                        dtvRepuesto.RowFilter = "IdPerfilCompActividad = " + IdPerCompActividad.ToString();
                        grvRepuesto.ItemsSource = dtvRepuesto;
                    }

                }
            }
            catch
            {

            }

        }
        private void btnAñadirTrabajador_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int IdTipo = Convert.ToInt32(trlComponente.GetCellValue(trlViewComp.FocusedRowHandle, "IdTipo"));
                if (IdTipo == 3)
                {
                    stkTrabajadorTarea.Visibility = System.Windows.Visibility.Visible;
                    cboTrabajador.SelectedIndex = -1;
                    dtpFechaTarea.EditValue = null;
                    txthoraini.Clear();
                    txthorafin.Clear();
                    cboTrabajador.EditValue = CodResponsable;
                }
                else
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_TARE"), 2);
                }
            }
            catch
            {
            }
        }
        private void btnAgregarTrabajador_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (cboTrabajador.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_TRAB"), 2);
                    return;
                }

                //Valida Fecha
                if (dtpFechaTarea.EditValue == null)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_FECH"), 2);
                    return;
                }
                DateTime FechaInicial;
                DateTime FechaFinal;
                TimeSpan horas;
                FechaInicial = Convert.ToDateTime(Convert.ToDateTime(dtpFechaTarea.EditValue).ToString("dd/MM/yyyy") + " " + Convert.ToDateTime(txthoraini.EditValue).ToString("HH:mm"));
                FechaFinal = Convert.ToDateTime(Convert.ToDateTime(dtpFechaTarea.EditValue).ToString("dd/MM/yyyy") + " " + Convert.ToDateTime(txthorafin.EditValue).ToString("HH:mm"));

                if (FechaFinal < FechaInicial)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_RTAR_HORA"), 2);
                    return;
                }


                if (Convert.ToDateTime(LblFechaLiberacion.Content) > FechaInicial)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_RTAR_FECH"), 2);
                    dtpFechaCierreT.Focus();
                    return;
                }

                //Validar si existe un trabajador en dentro de las horas
                //int CantExis = tbOTTareaTrabajador.Select("CodResponsable = '" + Convert.ToString(cboTrabajador.EditValue) + "' and Fecha='" + FechaInicial + "'").Length;
                for (int i = 0; i < tbOTTareaTrabajador.Rows.Count; i++)
                {
                    if (Convert.ToString(tbOTTareaTrabajador.Rows[i]["CodResponsable"]) == Convert.ToString(cboTrabajador.EditValue))
                    {
                        DateTime dt = Convert.ToDateTime(tbOTTareaTrabajador.Rows[i]["Fecha"].ToString() + " " + tbOTTareaTrabajador.Rows[i]["HoraInicial"].ToString());
                        DateTime dt2 = Convert.ToDateTime(tbOTTareaTrabajador.Rows[i]["Fecha"].ToString() + " " + tbOTTareaTrabajador.Rows[i]["HoraFinal"].ToString());
                        if ((FechaInicial > dt && FechaInicial < dt2))
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_RTAR_RANG"), 2);
                            return;
                        }
                        if (FechaFinal > dt && FechaFinal < dt2)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_RTAR_RANG"), 2);
                            return;
                        }
                    }
                }


                int IdOTTarea = Convert.ToInt32(trlComponente.GetCellValue(trlViewComp.FocusedRowHandle, "IdReal"));
                DataRow row = tbOTTareaTrabajador.NewRow();
                row["IdOTTarea"] = IdOTTarea; //Extraer de grilla

                if (tbOTTareaTrabajador.Rows.Count > 0)
                {
                    row["IdOTTareaDetalle"] = Convert.ToInt32(tbOTTareaTrabajador.Compute("max(IdOTTareaDetalle)", "")) + 1;
                }
                else
                {
                    row["IdOTTareaDetalle"] = 1;
                }

                row["CodResponsable"] = cboTrabajador.EditValue;
                row["Trabajador"] = cboTrabajador.Text;
                row["CostoHoraHombre"] = 0;
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("R", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    for (int i = 0; i < OHEMlist.Count(); i++)
                    {
                        if (Convert.ToString(cboTrabajador.EditValue) == Convert.ToString(OHEMlist[i].CodigoPersona))
                        {
                            row["CostoHoraHombre"] = OHEMlist[i].CostoHoraHombre;
                        }
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                string fecha = Convert.ToDateTime(dtpFechaTarea.EditValue).ToString("dd/MM/yyyy");
                string horai = Convert.ToDateTime(txthoraini.EditValue).ToString("HH:mm");
                string horaf = Convert.ToDateTime(txthorafin.EditValue).ToString("HH:mm");

                if (tbOTTareaTrabajador.Rows.Count == 0)
                {
                    row["Fecha"] = fecha;// Convert.ToDateTime(dtpFechaTarea.EditValue).ToString("dd/MM/yyyy");
                    row["HoraInicial"] = horai;// Convert.ToDateTime(txthoraini.EditValue).ToString("HH:mm");
                    row["HoraFinal"] = horaf;// Convert.ToDateTime(txthorafin.EditValue).ToString("HH:mm");
                    horas = (FechaFinal - FechaInicial);
                    row["HoraReal"] = Math.Round(horas.TotalHours, 2);  //Restar Hora
                    row["FlagActivo"] = true;
                    row["Nuevo"] = true;
                }
                else
                {

                    for (int i = 0; i < tbOTTareaTrabajador.Rows.Count; i++)
                    {
                        string Fechatabla = tbOTTareaTrabajador.Rows[i]["Fecha"].ToString();
                        string Horaitabla = tbOTTareaTrabajador.Rows[i]["HoraInicial"].ToString();
                        string Horaftabla = tbOTTareaTrabajador.Rows[i]["HoraFinal"].ToString();

                        if (cboTrabajador.Text == tbOTTareaTrabajador.Rows[i]["Trabajador"] && fecha == Fechatabla &&
                             horai == Horaitabla && horaf == Horaftabla)
                        {
                            GlobalClass.ip.Mensaje("Error de Duplicidad", 2);
                            return;
                        }
                        else
                        {
                            row["Fecha"] = fecha;// Convert.ToDateTime(dtpFechaTarea.EditValue).ToString("dd/MM/yyyy");
                            row["HoraInicial"] = horai;// Convert.ToDateTime(txthoraini.EditValue).ToString("HH:mm");
                            row["HoraFinal"] = horaf;// Convert.ToDateTime(txthorafin.EditValue).ToString("HH:mm");
                            horas = (FechaFinal - FechaInicial);
                            row["HoraReal"] = Math.Round(horas.TotalHours, 2);  //Restar Hora
                            row["FlagActivo"] = true;
                            row["Nuevo"] = true;
                        }

                        /*
                        if (tbOTTareaTrabajador.Rows[0]["Trabajador"] != cboTrabajador.Text)
                        {
                            row["Fecha"] = fecha;// Convert.ToDateTime(dtpFechaTarea.EditValue).ToString("dd/MM/yyyy");
                            row["HoraInicial"] = horai;// Convert.ToDateTime(txthoraini.EditValue).ToString("HH:mm");
                            row["HoraFinal"] = horaf;// Convert.ToDateTime(txthorafin.EditValue).ToString("HH:mm");
                            horas = (FechaFinal - FechaInicial);
                            row["HoraReal"] = Math.Round(horas.TotalHours, 2);  //Restar Hora
                            row["FlagActivo"] = true;
                            row["Nuevo"] = true;
                        }
                         
                        if (cboTrabajador.Text != tbOTTareaTrabajador.Rows[i]["Trabajador"] && fecha != Fechatabla &&
                             horai != Horaitabla && horaf != Horaftabla)
                        {
                            row["Fecha"] = fecha;// Convert.ToDateTime(dtpFechaTarea.EditValue).ToString("dd/MM/yyyy");
                            row["HoraInicial"] = horai;// Convert.ToDateTime(txthoraini.EditValue).ToString("HH:mm");
                            row["HoraFinal"] = horaf;// Convert.ToDateTime(txthorafin.EditValue).ToString("HH:mm");
                            horas = (FechaFinal - FechaInicial);
                            row["HoraReal"] = Math.Round(horas.TotalHours, 2);  //Restar Hora
                            row["FlagActivo"] = true;
                            row["Nuevo"] = true;
                        }
                        else
                        {
                            GlobalClass.ip.Mensaje("Error de Duplicidad", 2);
                            //return;
                        }
                         * */

                    }
                }

                tbOTTareaTrabajador.Rows.Add(row);
                grvListarTrabajador.ItemsSource = tbOTTareaTrabajador.DefaultView;

                int idpadre = 0;
                for (int i = 0; i < tblOTCompTreeList.Rows.Count; i++)
                {
                    if (Convert.ToInt32(tblOTCompTreeList.Rows[i]["IdReal"]) == IdOTTarea && Convert.ToInt32(tblOTCompTreeList.Rows[i]["IdTipo"]) == 3 && idpadre == 0)
                    {
                        tblOTCompTreeList.Rows[i]["HorasReales"] = Convert.ToDouble(tbOTTareaTrabajador.Compute("sum(HoraReal)", "FlagActivo = True and IdOTTarea = " + IdOTTarea.ToString()));
                        idpadre = Convert.ToInt32(tblOTCompTreeList.Rows[i]["IdPadre"]);
                        i = 0;
                    }
                    if (Convert.ToInt32(tblOTCompTreeList.Rows[i]["Id"]) == idpadre && Convert.ToInt32(tblOTCompTreeList.Rows[i]["IdTipo"]) == 2)
                    {
                        tblOTCompTreeList.Rows[i]["ActividadRealizada"] = true;
                        break;
                    }
                }

                tblOTCompTreeList.DefaultView.Sort = "IdTipo desc";
                tblOTCompTreeList = tblOTCompTreeList.DefaultView.ToTable();
                for (int i = 0; i < tblOTCompTreeList.Rows.Count; i++)
                {
                    //if (Convert.ToInt32(tblOTCompTreeList.Rows[i]["idTipo"]) == 2)
                    //{
                    var x = tblOTCompTreeList.Compute("Sum(HorasEstimada)", "IdPadre = " + tblOTCompTreeList.Rows[i]["Id"].ToString());
                    if (x == DBNull.Value)
                    {
                    }
                    else
                    {
                        tblOTCompTreeList.Rows[i]["HorasEstimada"] = Convert.ToInt32(tblOTCompTreeList.Compute("Sum(HorasEstimada)", "IdPadre = " + tblOTCompTreeList.Rows[i]["Id"].ToString()));
                        tblOTCompTreeList.Rows[i]["HorasReales"] = Convert.ToInt32(tblOTCompTreeList.Compute("Sum(HorasReales)", "IdPadre = " + tblOTCompTreeList.Rows[i]["Id"].ToString()));
                    }
                    //}
                }


                //trlComponente.ItemsSource = tblOTCompTreeList.DefaultView;
                cboTrabajador.SelectedIndex = -1;
                dtpFechaTarea.EditValue = null;
                txthoraini.Clear();
                txthorafin.Clear();
                cboTrabajador.Focus();
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }


        }

        private void btnCancelarTrabajador_Click(object sender, RoutedEventArgs e)
        {
            trlComponente.ItemsSource = tblOTCompTreeList.DefaultView;
            stkTrabajadorTarea.Visibility = System.Windows.Visibility.Hidden;
            cboTrabajador.SelectedIndex = -1;
            dtpFechaTarea.EditValue = null;
            txthoraini.Clear();
            txthorafin.Clear();
        }
        private void trlComponente_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            try
            {
                if (trlComponente.VisibleRowCount > 0)
                {
                    int IdTipo = Convert.ToInt32(trlComponente.GetCellValue(trlViewComp.FocusedRowHandle, "IdTipo"));
                    int Id = Convert.ToInt32(trlComponente.GetCellValue(trlViewComp.FocusedRowHandle, "IdReal"));

                    if (IdTipo == 1)
                    {
                        gstrNroSerieSelec = tblOTCompDatos.Tables[0].Select("IdOTComp = " + Id)[0]["NroSerie"].ToString();

                    }

                    if (IdTipo == 3)
                    {
                        DataView dtvTarea = tbOTTareaTrabajador.DefaultView;
                        dtvTarea.RowFilter = "FlagActivo = True and  IdOTTarea = " + Id;
                        grvListarTrabajador.ItemsSource = dtvTarea;
                        grvHerrEspTarea.ItemsSource = null;
                        DtgRespuestosConsumibles.ItemsSource = null;
                    }
                    else if (IdTipo == 1)
                    {
                        DataView dtvHerra = tblHerrEspTarea.DefaultView;
                        dtvHerra.RowFilter = "IdOTComp = " + Id;
                        grvHerrEspTarea.ItemsSource = dtvHerra;

                        DataView dtvArticulo = tblArticuloTarea.DefaultView;
                        dtvArticulo.RowFilter = "IdOTComp = " + Id;
                        DtgRespuestosConsumibles.ItemsSource = dtvArticulo;
                    }
                    else
                    {
                        grvHerrEspTarea.ItemsSource = null;
                        DtgRespuestosConsumibles.ItemsSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void btnAceptarHerrEspTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboEstadoHerrEspTarea.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_HERR_ESTA"), 2);
                    return;
                }

                if (Convert.ToInt32(cboEstadoHerrEspTarea.EditValue) == 2 && txtNroDevolucion.Text.Trim() == "")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_HERR_DEVO"), 2);
                    txtNroDevolucion.Focus();
                    return;
                }

                int idotcomp = Convert.ToInt32(grvHerrEspTarea.GetCellValue(tblviewHerrEsp.FocusedRowHandle, "IdOTComp"));
                int idotcompactividad = Convert.ToInt32(grvHerrEspTarea.GetCellValue(tblviewHerrEsp.FocusedRowHandle, "IdOTCompActividad"));
                int idotherresp = Convert.ToInt32(grvHerrEspTarea.GetCellValue(tblviewHerrEsp.FocusedRowHandle, "IdOTHerramienta"));
                for (int i = 0; i < tblHerrEspTarea.Rows.Count; i++)
                {
                    if (Convert.ToInt32(tblHerrEspTarea.Rows[i]["IdOTComp"]) == idotcomp && Convert.ToInt32(tblHerrEspTarea.Rows[i]["IdOTCompActividad"]) == idotcompactividad && Convert.ToInt32(tblHerrEspTarea.Rows[i]["IdOTHerramienta"]) == idotherresp)
                    {
                        tblHerrEspTarea.Rows[i]["IdEstado"] = Convert.ToInt32(cboEstadoHerrEspTarea.EditValue);
                        tblHerrEspTarea.Rows[i]["Estado"] = Convert.ToString(cboEstadoHerrEspTarea.Text);
                        tblHerrEspTarea.Rows[i]["NroDevolucion"] = Convert.ToString(txtNroDevolucion.Text);
                        break;
                    }
                }
                stkHerrEspTarea.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void btnCancelarHerrEspTarea_Click(object sender, RoutedEventArgs e)
        {
            stkHerrEspTarea.Visibility = System.Windows.Visibility.Hidden;
        }
        private void btnAceptarArticuloTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (cboResponsableTarea.SelectedIndex == -1)
                //{
                //    GlobalClass.ip.Mensaje("Seleccionar responsable", 2);
                //    return;
                //}

                if (!gbolIsFrecExten)
                {
                    if (txtCantidadUtilizada.Text.Trim() == "" || Convert.ToInt32(txtCantidadUtilizada.Text) == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_HERR_CANT"), 2);
                        return;
                    }

                    if (txtNroSerie.Text.Trim() == "" && txtNroSerie.IsEnabled)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_HERR_NROS"), 2);
                        return;
                    }

                    if (Convert.ToDouble(txtFrecuencia.EditValue) <= 0 && txtFrecuencia.IsEnabled)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_HERR_FRDI"), 2);
                        return;
                    }

                    if (Convert.ToDouble(txtFrecuenciaTm.EditValue) <= 0 && txtFrecuenciaTm.IsEnabled)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_HERR_FRTI"), 2);
                        return;
                    }
                }
                else
                {
                    if (Convert.ToDouble(txtFrecuencia.EditValue) <= 0 && Convert.ToDouble(txtFrecuenciaTm.EditValue) <= 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_HERR_EXFR"), 2);
                        return;
                    }
                }


                int idotcomp = Convert.ToInt32(DtgRespuestosConsumibles.GetCellValue(tblViewRepCon.FocusedRowHandle, "IdOTComp"));
                int idotcompactividad = Convert.ToInt32(DtgRespuestosConsumibles.GetCellValue(tblViewRepCon.FocusedRowHandle, "IdOTCompActividad"));
                int idotarticulo = Convert.ToInt32(DtgRespuestosConsumibles.GetCellValue(tblViewRepCon.FocusedRowHandle, "IdOTArticulo"));

                for (int i = 0; i < tblArticuloTarea.Rows.Count; i++)
                {
                    if (Convert.ToInt32(tblArticuloTarea.Rows[i]["IdOTComp"]) == idotcomp && Convert.ToInt32(tblArticuloTarea.Rows[i]["IdOTCompActividad"]) == idotcompactividad && Convert.ToInt32(tblArticuloTarea.Rows[i]["IdOTArticulo"]) == idotarticulo)
                    {
                        if (Convert.ToInt32(txtCantidadUtilizada.Text) > Convert.ToInt32(tblArticuloTarea.Rows[i]["CantEnv"]))
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_RTAR_HERR_CANT"), 2);
                            return;
                        }
                        else
                        {
                            tblArticuloTarea.Rows[i]["CantUti"] = Convert.ToInt32(txtCantidadUtilizada.Text);
                            tblArticuloTarea.Rows[i]["CodResponsable"] = Convert.ToString(cboResponsableTarea.EditValue);
                            tblArticuloTarea.Rows[i]["Responsable"] = Convert.ToString(cboResponsableTarea.Text);
                            tblArticuloTarea.Rows[i]["NroSerie"] = Convert.ToString(txtNroSerie.Text);
                            tblArticuloTarea.Rows[i]["Frecuencia"] = Convert.ToDouble(txtFrecuencia.EditValue);
                            tblArticuloTarea.Rows[i]["FrecuenciaTie"] = Convert.ToDouble(txtFrecuenciaTm.EditValue) * gintValorTiempoDefecto;
                            tblArticuloTarea.Rows[i]["FlagExtendida"] = ((bool)chkFrecExtendida.IsChecked);
                            break;
                        }
                    }
                }
                stkArticuloTarea.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 2);
            }
        }
        private void btnCancelarArticuloTarea_Click(object sender, RoutedEventArgs e)
        {
            txtCantidadUtilizada.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtCantidadUtilizada_EditValueChanged);
            txtNroSerie.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtNroSerie_EditValueChanged);
            txtFrecuencia.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecuencia_EditValueChanged);
            txtFrecuenciaTm.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecuenciaTm_EditValueChanged);
            chkFrecExtendida.Checked -= new RoutedEventHandler(chkFrecExtendida_Checked);
            chkFrecExtendida.Unchecked -= new RoutedEventHandler(chkFrecExtendida_Unchecked);

            chkFrecExtendida.IsEnabled = true;
            chkFrecExtendida.IsChecked = false;
            txtNroSerie.EditValue = "";
            txtFrecuencia.EditValue = "0";
            txtFrecuenciaTm.EditValue = "0";
            txtCantidadUtilizada.EditValue = "0";
            txtCantidadUtilizada.MinValue = 1;
            txtCantidadUtilizada.IsEnabled = true;
            stkArticuloTarea.Visibility = System.Windows.Visibility.Hidden;

            txtCantidadUtilizada.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtCantidadUtilizada_EditValueChanged);
            txtNroSerie.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtNroSerie_EditValueChanged);
            txtFrecuencia.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecuencia_EditValueChanged);
            txtFrecuenciaTm.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecuenciaTm_EditValueChanged);
            chkFrecExtendida.Checked += new RoutedEventHandler(chkFrecExtendida_Checked);
            chkFrecExtendida.Unchecked += new RoutedEventHandler(chkFrecExtendida_Unchecked);
        }
        private void AbrirArticuloTarea_Click(object sender, RoutedEventArgs e)
        {
            txtNroSerie.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtNroSerie_EditValueChanged);
            if (DtgRespuestosConsumibles.VisibleRowCount == 0) return;
            //txtNroSerie.Text = "";
            if (tbOTTareaTrabajador.Rows.Count == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_REGI_TARE"), 2);
            }
            else
            {
                stkArticuloTarea.Visibility = System.Windows.Visibility.Visible;
            }
            txtNroSerie.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtNroSerie_EditValueChanged);
        }
        private void btnAbrirHerrEspTarea_Click(object sender, RoutedEventArgs e)
        {
            if (grvHerrEspTarea.VisibleRowCount == 0) return;

            if (tbOTTareaTrabajador.Rows.Count == 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_REGI_TARE"), 2);
            }
            else
            {
                cboEstadoHerrEspTarea.EditValue = 1;
                lblHerramienta.Content = grvHerrEspTarea.GetFocusedRowCellValue("Articulo").ToString();
                stkHerrEspTarea.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void grvHerrEspTarea_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            try
            {
                int idotcomp = Convert.ToInt32(grvHerrEspTarea.GetCellValue(tblviewHerrEsp.FocusedRowHandle, "IdOTComp"));
                int idotcompactividad = Convert.ToInt32(grvHerrEspTarea.GetCellValue(tblviewHerrEsp.FocusedRowHandle, "IdOTCompActividad"));
                int idotherresp = Convert.ToInt32(grvHerrEspTarea.GetCellValue(tblviewHerrEsp.FocusedRowHandle, "IdOTHerramienta"));
                for (int i = 0; i < tblHerrEspTarea.Rows.Count; i++)
                {
                    if (Convert.ToInt32(tblHerrEspTarea.Rows[i]["IdOTComp"]) == idotcomp && Convert.ToInt32(tblHerrEspTarea.Rows[i]["IdOTCompActividad"]) == idotcompactividad && Convert.ToInt32(tblHerrEspTarea.Rows[i]["IdOTHerramienta"]) == idotherresp)
                    {
                        lblHerramienta.Content = Convert.ToString(tblHerrEspTarea.Rows[i]["Articulo"]);

                        if (tblHerrEspTarea.Rows[i]["IdEstado"] != DBNull.Value)
                        {
                            cboEstadoHerrEspTarea.EditValue = Convert.ToInt32(tblHerrEspTarea.Rows[i]["IdEstado"]);
                        }
                        else
                        {
                            cboEstadoHerrEspTarea.SelectedIndex = -1;
                        }
                        if (tblHerrEspTarea.Rows[i]["NroDevolucion"] != DBNull.Value)
                        {
                            txtNroDevolucion.Text = Convert.ToString(tblHerrEspTarea.Rows[i]["NroDevolucion"]);
                        }
                        else
                        {
                            txtNroDevolucion.Text = "";
                        }
                        break;
                    }
                }
            }
            catch
            {

            }
        }
        private void DtgRespuestosConsumibles_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            try
            {
                txtCantidadUtilizada.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtCantidadUtilizada_EditValueChanged);
                txtNroSerie.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtNroSerie_EditValueChanged);
                txtFrecuencia.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecuencia_EditValueChanged);
                txtFrecuenciaTm.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecuenciaTm_EditValueChanged);
                chkFrecExtendida.Checked -= new RoutedEventHandler(chkFrecExtendida_Checked);
                chkFrecExtendida.Unchecked -= new RoutedEventHandler(chkFrecExtendida_Unchecked);

                int idotcomp = Convert.ToInt32(DtgRespuestosConsumibles.GetCellValue(tblViewRepCon.FocusedRowHandle, "IdOTComp"));
                int idotcompactividad = Convert.ToInt32(DtgRespuestosConsumibles.GetCellValue(tblViewRepCon.FocusedRowHandle, "IdOTCompActividad"));
                int idotarticulo = Convert.ToInt32(DtgRespuestosConsumibles.GetCellValue(tblViewRepCon.FocusedRowHandle, "IdOTArticulo"));
                //string Tipo = Convert.ToString(DtgRespuestosConsumibles.GetCellValue(tblViewRepCon.FocusedRowHandle, "Tipo"));
                for (int i = 0; i < tblArticuloTarea.Rows.Count; i++)
                {
                    if (Convert.ToInt32(tblArticuloTarea.Rows[i]["IdOTComp"]) == idotcomp && Convert.ToInt32(tblArticuloTarea.Rows[i]["IdOTCompActividad"]) == idotcompactividad && Convert.ToInt32(tblArticuloTarea.Rows[i]["IdOTArticulo"]) == idotarticulo)
                    {
                        lblDescripcion.Content = Convert.ToString(tblArticuloTarea.Rows[i]["Articulo"]);

                        if (tblArticuloTarea.Rows[i]["CantUti"] != DBNull.Value)
                        {
                            txtCantidadUtilizada.Text = Convert.ToString(tblArticuloTarea.Rows[i]["CantUti"]);
                        }
                        else
                        {
                            txtCantidadUtilizada.Text = "0";
                        }

                        if (tblArticuloTarea.Rows[i]["NroSerie"] != DBNull.Value)
                        {
                            txtNroSerie.Text = Convert.ToString(tblArticuloTarea.Rows[i]["NroSerie"]);
                        }
                        else
                        {
                            txtNroSerie.Text = "";
                        }

                        if (tblArticuloTarea.Rows[i]["Frecuencia"] != DBNull.Value)
                        {
                            txtFrecuencia.Text = Convert.ToString(tblArticuloTarea.Rows[i]["Frecuencia"]);
                            txtFrecuenciaTm.Text = (Convert.ToDouble(tblArticuloTarea.Rows[i]["FrecuenciaTie"]) / gintValorTiempoDefecto).ToString();
                        }
                        else
                        {
                            txtFrecuencia.EditValue = "0";
                            txtFrecuenciaTm.EditValue = "0";
                        }

                        chkFrecExtendida.IsChecked = Convert.ToBoolean(tblArticuloTarea.Rows[i]["FlagExtendida"]);

                        if (Convert.ToInt32(tblArticuloTarea.Rows[i]["IdTipoArticulo"]) == 4)
                        {
                            //Si es consumible mostrar Bloquear NroSerie y Frecuencia
                            txtNroSerie.IsEnabled = true;
                            txtFrecuencia.IsEnabled = true;
                            txtFrecuenciaTm.IsEnabled = true;
                            chkFrecExtendida.IsEnabled = true;
                        }
                        else
                        {
                            txtNroSerie.IsEnabled = false;
                            txtFrecuencia.IsEnabled = false;
                            txtFrecuenciaTm.IsEnabled = false;
                            chkFrecExtendida.IsEnabled = false;
                        }

                        break;

                    }
                }

                txtFrecuencia.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecuencia_EditValueChanged);
                txtFrecuenciaTm.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecuenciaTm_EditValueChanged);
                chkFrecExtendida.Checked += new RoutedEventHandler(chkFrecExtendida_Checked);
                chkFrecExtendida.Unchecked += new RoutedEventHandler(chkFrecExtendida_Unchecked);
                txtCantidadUtilizada.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtCantidadUtilizada_EditValueChanged);
                txtNroSerie.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtNroSerie_EditValueChanged);
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void txtCantidadUtilizada_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textboxSender = (TextBox)sender;
                var cursorPosition = textboxSender.SelectionStart;
                textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9]", "");
                textboxSender.SelectionStart = cursorPosition;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void LimpiarDatosRegistroTareas()
        {
            //trlComponente.ItemsSource = null;
            grvHerrEspTarea.ItemsSource = null;
            DtgRespuestosConsumibles.ItemsSource = null;
            dtpFechaCierreT.EditValue = DateTime.Now;
            //txtHoraFechaCierre.Text = "";
            TxTNumeroOT.Text = "";
            LblFechaLiberacion.Content = "";
            LblResponsable.Content = "";
            LblUnidadControl.Content = "";
            lblTipoOrden.Content = "";
        }
        private void btnRegistrarOTTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidaTipoAveria() == false) return;
                if (GrabarTarea() == true)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_RTAR"), 1);
                    LimpiarTareasRealizadas();
                    GlobalClass.ip.SeleccionarTab(tabListadoOT);
                    //tabControl1.SelectedIndex = 0; //Tab Javier
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }

        private bool GrabarTarea()
        {
            DataTable tblOTArticuloDetalles = new DataTable();
            tblOTArticuloDetalles.Columns.Add("IdOTArticulo", Type.GetType("System.Int32"));
            tblOTArticuloDetalles.Columns.Add("NroSerie", Type.GetType("System.String"));
            tblOTArticuloDetalles.Columns.Add("FrecuenciaDis", Type.GetType("System.Double"));
            tblOTArticuloDetalles.Columns.Add("FrecuenciaTie", Type.GetType("System.Double"));
            tblOTArticuloDetalles.Columns.Add("FlagExtendida", Type.GetType("System.Boolean"));
            tblOTArticuloDetalles.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

            bool blRpta = false;
            try
            {
                objE_OT = new E_OT();
                objE_OT.IdOT = IdOT;
                objE_OT.IdUsuario = gintIdUsuario;
                objE_OT.FechaModificacion = FechaModificacion;
                objE_OT.CodTipoAveria = Convert.ToInt32(cboTipoAveria.EditValue);

                if (dtpFechaCierreT.EditValue == null)
                {
                    objE_OT.FechaCierre = DateTime.MinValue;
                }
                else
                {
                    if (Convert.ToDateTime(LblFechaLiberacion.Content) > Convert.ToDateTime(dtpFechaCierreT.EditValue))
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_CERR_OT_FECH"), 2);
                        dtpFechaCierreT.Focus();
                        return blRpta; //Descomentar cuando la licencia de SAP este activa.
                    }
                    objE_OT.FechaCierre = Convert.ToDateTime(dtpFechaCierreT.EditValue);

                    int VerificarEstado = tblHerrEspTarea.Select("IdEstado <> 2").Length;
                    if (VerificarEstado > 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_HERR_DEVO_CERR"), 2);
                        return blRpta;
                    }
                }
                tblActividades.Rows.Clear();

                for (int i = 0; i < tblOTCompTreeList.Rows.Count; i++)
                {
                    if (Convert.ToInt32(tblOTCompTreeList.Rows[i]["IdTipo"]) == 2)
                    {
                        DataRow row = tblActividades.NewRow();
                        row["IdOTCompActividad"] = tblOTCompTreeList.Rows[i]["IdReal"];
                        row["IdOTComp"] = tblOTCompTreeList.Rows[i]["IdOTComp"];
                        row["IdPerfilComp"] = 0;
                        row["IdPerfilCompActividad"] = 0;
                        row["IdActividad"] = tblOTCompTreeList.Rows[i]["IdActividad"];
                        row["Actividad"] = 0;
                        row["IsChecked"] = tblOTCompTreeList.Rows[i]["ActividadRealizada"];
                        row["FlagUso"] = false;
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        tblActividades.Rows.Add(row);
                    }
                }
                int CantActPend = tblActividades.Select("IsChecked = false").Length;
                if (CantActPend > 0)
                {
                    var rpt = DevExpress.Xpf.Core.DXMessageBox.Show("¿Existe actividades pendientes, seguro de seguir grabando?", "Grabación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (rpt == MessageBoxResult.No)
                    {
                        return false;
                    }
                }


                tblHerrEsp.Rows.Clear();
                for (int i = 0; i < tblHerrEspTarea.Rows.Count; i++)
                {
                    DataRow row = tblHerrEsp.NewRow();
                    row["IdOTHerramienta"] = tblHerrEspTarea.Rows[i]["IdOTHerramienta"];
                    row["IdOTCompActividad"] = tblHerrEspTarea.Rows[i]["IdOTCompActividad"];
                    row["IdPerfilCompActividad"] = 0;
                    row["IdHerramienta"] = tblHerrEspTarea.Rows[i]["IdArticulo"];
                    row["Herramienta"] = tblHerrEspTarea.Rows[i]["Articulo"];
                    row["Cantidad"] = tblHerrEspTarea.Rows[i]["Cantidad"];
                    row["IdEstado"] = tblHerrEspTarea.Rows[i]["IdEstado"];
                    row["NroDevolucion"] = tblHerrEspTarea.Rows[i]["NroDevolucion"];
                    row["FlagActivo"] = true;
                    row["Nuevo"] = false;
                    tblHerrEsp.Rows.Add(row);
                }

                tblRepuesto.Rows.Clear();
                for (int i = 0; i < tblArticuloTarea.Rows.Count; i++)
                {
                    DataRow row = tblRepuesto.NewRow();
                    row["IdOTArticulo"] = tblArticuloTarea.Rows[i]["IdOTArticulo"];
                    row["IdOTCompActividad"] = tblArticuloTarea.Rows[i]["IdOTCompActividad"];
                    row["IdPerfilCompActividad"] = 0;
                    row["IdTipoArticulo"] = tblArticuloTarea.Rows[i]["IdTipoArticulo"];
                    row["IdArticulo"] = tblArticuloTarea.Rows[i]["IdArticulo"];
                    row["Articulo"] = tblArticuloTarea.Rows[i]["Articulo"];
                    row["CantSol"] = tblArticuloTarea.Rows[i]["CantSol"];
                    row["CantEnv"] = tblArticuloTarea.Rows[i]["CantEnv"];
                    row["CantUti"] = tblArticuloTarea.Rows[i]["CantUti"];
                    row["CostoArticulo"] = tblArticuloTarea.Rows[i]["CostoArticulo"];
                    row["Observacion"] = tblArticuloTarea.Rows[i]["Observacion"];
                    row["CodResponsable"] = tblArticuloTarea.Rows[i]["CodResponsable"];
                    row["NroSerie"] = tblArticuloTarea.Rows[i]["NroSerie"];
                    row["Frecuencia"] = tblArticuloTarea.Rows[i]["Frecuencia"];
                    row["FlagActivo"] = true;
                    row["Nuevo"] = false;
                    tblRepuesto.Rows.Add(row);

                    DataRow drDet = tblOTArticuloDetalles.NewRow();
                    drDet["IdOTArticulo"] = tblArticuloTarea.Rows[i]["IdOTArticulo"];
                    drDet["NroSerie"] = tblArticuloTarea.Rows[i]["NroSerie"];
                    drDet["FrecuenciaDis"] = tblArticuloTarea.Rows[i]["Frecuencia"];
                    drDet["FrecuenciaTie"] = tblArticuloTarea.Rows[i]["FrecuenciaTie"];
                    drDet["FlagExtendida"] = tblArticuloTarea.Rows[i]["FlagExtendida"];
                    drDet["Nuevo"] = false;
                    tblOTArticuloDetalles.Rows.Add(drDet);
                }

                for (int i = 0; i < tbOTTareaTrabajador.Rows.Count; i++)
                {
                    tbOTTareaTrabajador.Rows[i]["HoraInicial"] = Convert.ToDateTime(tbOTTareaTrabajador.Rows[i]["Fecha"]).ToString("yyyyMMdd") + " " + tbOTTareaTrabajador.Rows[i]["HoraInicial"];
                    tbOTTareaTrabajador.Rows[i]["HoraFinal"] = Convert.ToDateTime(tbOTTareaTrabajador.Rows[i]["Fecha"]).ToString("yyyyMMdd") + " " + tbOTTareaTrabajador.Rows[i]["HoraFinal"];
                    tbOTTareaTrabajador.Rows[i]["Fecha"] = Convert.ToDateTime(tbOTTareaTrabajador.Rows[i]["Fecha"]).ToString("yyyyMMdd");
                }

                int rpta = objB_OTComp.OTTarea_UpdateCascade(objE_OT, tblActividades, tbOTTareaTrabajador, tblHerrEsp, tblRepuesto, tblOTArticuloDetalles);
                if (rpta == 1)
                {
                    for (int i = 0; i < tbOTTareaTrabajador.Rows.Count; i++)
                    {
                        tbOTTareaTrabajador.Rows[i]["HoraInicial"] = tbOTTareaTrabajador.Rows[i]["HoraInicial"].ToString().Substring(9, 5);
                        tbOTTareaTrabajador.Rows[i]["HoraFinal"] = tbOTTareaTrabajador.Rows[i]["HoraFinal"].ToString().Substring(9, 5);
                        tbOTTareaTrabajador.Rows[i]["Fecha"] = tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(6, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(4, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(0, 4);
                    }
                }
                else if (rpta == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_MODI"), 2);
                    for (int i = 0; i < tbOTTareaTrabajador.Rows.Count; i++)
                    {
                        tbOTTareaTrabajador.Rows[i]["HoraInicial"] = tbOTTareaTrabajador.Rows[i]["HoraInicial"].ToString().Substring(9, 5);
                        tbOTTareaTrabajador.Rows[i]["HoraFinal"] = tbOTTareaTrabajador.Rows[i]["HoraFinal"].ToString().Substring(9, 5);
                        tbOTTareaTrabajador.Rows[i]["Fecha"] = tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(6, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(4, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(0, 4);
                    }
                    return false;
                }
                else if (rpta == 1205)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_CONC"), 2);
                    for (int i = 0; i < tbOTTareaTrabajador.Rows.Count; i++)
                    {
                        tbOTTareaTrabajador.Rows[i]["HoraInicial"] = tbOTTareaTrabajador.Rows[i]["HoraInicial"].ToString().Substring(9, 5);
                        tbOTTareaTrabajador.Rows[i]["HoraFinal"] = tbOTTareaTrabajador.Rows[i]["HoraFinal"].ToString().Substring(9, 5);
                        tbOTTareaTrabajador.Rows[i]["Fecha"] = tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(6, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(4, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(0, 4);
                    }
                    return false;
                }

                //if (rpta != 0)
                //{
                //    GlobalClass.ip.Mensaje("Error al grabar", 3);
                //    for (int i = 0; i < tbOTTareaTrabajador.Rows.Count; i++)
                //    {
                //        tbOTTareaTrabajador.Rows[i]["HoraInicial"] = tbOTTareaTrabajador.Rows[i]["HoraInicial"].ToString().Substring(9, 5);
                //        tbOTTareaTrabajador.Rows[i]["HoraFinal"] = tbOTTareaTrabajador.Rows[i]["HoraFinal"].ToString().Substring(9, 5);
                //        tbOTTareaTrabajador.Rows[i]["Fecha"] = tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(6, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(4, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(0, 4);
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < tbOTTareaTrabajador.Rows.Count; i++)
                //    {
                //        tbOTTareaTrabajador.Rows[i]["HoraInicial"] = tbOTTareaTrabajador.Rows[i]["HoraInicial"].ToString().Substring(9, 5);
                //        tbOTTareaTrabajador.Rows[i]["HoraFinal"] = tbOTTareaTrabajador.Rows[i]["HoraFinal"].ToString().Substring(9, 5);
                //        tbOTTareaTrabajador.Rows[i]["Fecha"] = tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(6, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(4, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(0, 4);
                //    }
                //    //LimpiarDatosRegistroTareas();
                //}

                ListarOT();
                blRpta = true;

                return blRpta;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                for (int i = 0; i < tbOTTareaTrabajador.Rows.Count; i++)
                {
                    tbOTTareaTrabajador.Rows[i]["HoraInicial"] = tbOTTareaTrabajador.Rows[i]["HoraInicial"].ToString().Substring(9, 5);
                    tbOTTareaTrabajador.Rows[i]["HoraFinal"] = tbOTTareaTrabajador.Rows[i]["HoraFinal"].ToString().Substring(9, 5);
                    tbOTTareaTrabajador.Rows[i]["Fecha"] = tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(6, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(4, 2) + "/" + tbOTTareaTrabajador.Rows[i]["Fecha"].ToString().Substring(0, 4);
                }
                return false;
            }


        }

        private void CancelarOTTarea_Click(object sender, RoutedEventArgs e)
        {
            ListarOT();
            LimpiarTareasRealizadas();
            GlobalClass.ip.SeleccionarTab(tabListadoOT);
            //tabControl1.SelectedIndex = 0; //Tab Javier
            //tabItem3.IsEnabled = false; //Tab Javier
            //tabItem9.IsEnabled = false; //Tab Javier
        }
        private void btnRICancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarDatosInformeProveedor();
            ListarOT();
            GlobalClass.ip.SeleccionarTab(tabListadoOT);
            //tabControl1.SelectedIndex = 0; //Tab Javier
        }

        private void cboRIProveedor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            RPTA = new InterfazMTTO.iSBO_BE.BERPTA();

            OPORList = InterfazMTTO.iSBO_BL.OrdenCompra_BL.ConsultaOrdenesCompra(cboRIProveedor.EditValue.ToString(), "Y", ref RPTA);

            lblRICosto.Content = "";
            cboRIORdenCompra.ItemsSource = null;
            if (RPTA.CodigoErrorUsuario == "000")
            {
                foreach (var prove in OCRDList.Where(i => i.CodigoProveedor == cboRIProveedor.EditValue.ToString()))
                {
                    lblRIRuc.Content = prove.RucProveedor;
                    break;
                }
                cboRIORdenCompra.ItemsSource = OPORList;
            }
            else
            {
                GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
            }
        }

        private void cboRIORdenCompra_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in OPORList.Where(i => i.NroOrden == Convert.ToInt32(cboRIORdenCompra.Text)))
                {
                    gdblCostoTotal = item.CostoTotal;
                    lblRICosto.Content = String.Format("{0} {1:###,###,##0.00}", item.Moneda, gdblCostoTotal);
                    break;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCargarDocumento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolNuevo == true && gbolEdicion == false)
                {
                    txbLinkCarga.Visibility = Visibility.Visible;
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|PDF Files (*.pdf)|*.pdf";
                    Nullable<bool> result = dlg.ShowDialog();
                    if (result == true)
                    {
                        fileName = dlg.SafeFileName;
                        string sourcePath = System.IO.Path.GetDirectoryName(dlg.FileName);
                        sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                        var fileInfo = new System.IO.FileInfo(sourceFile);
                        gdblFileSize = fileInfo.Length / 1024 / 1024;
                        lblRIUbicacion.Content = fileName;
                    }
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    var rpt = DevExpress.Xpf.Core.DXMessageBox.Show("Se borrará el archivo ya guardado\n¿Está seguro que desea subir otro documento?", "Carga de archivos", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (rpt == MessageBoxResult.Yes)
                    {
                        gbolNuevoDocumento = true;
                        txbLinkDescarga.Visibility = Visibility.Collapsed;
                        txbLinkCarga.Visibility = Visibility.Visible;

                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|PDF Files (*.pdf)|*.pdf";
                        Nullable<bool> result = dlg.ShowDialog();
                        if (result == true)
                        {
                            fileName = dlg.SafeFileName;
                            string sourcePath = System.IO.Path.GetDirectoryName(dlg.FileName);
                            sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                            var fileInfo = new System.IO.FileInfo(sourceFile);
                            gdblFileSize = fileInfo.Length / 1024 / 1024;
                            lblRIUbicacion.Content = fileName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LimpiarDatosInformeProveedor()
        {
            try
            {
                cboRIProveedor.SelectedIndexChanged -= new RoutedEventHandler(cboRIProveedor_SelectedIndexChanged);
                cboRIORdenCompra.SelectedIndexChanged -= new RoutedEventHandler(cboRIORdenCompra_SelectedIndexChanged);

                gbolNuevoDocumento = false;
                txtRICodOT.Text = "";
                txtRIComentarios.Text = "";
                DtpRIFechCierre.EditValue = null;
                trvRIComponente.ItemsSource = null;
                lblRIFecLiberacion.Content = "";
                lblRICodUC.Content = "";
                lblRICosto.Content = "";
                lblRIResponsable.Content = "";
                lblRIRuc.Content = "";
                lblRITipoOT.Content = "";
                lblRIUbicacion.Content = "";
                lblRIUbicacionDescarga.Content = "";
                cboRIProveedor.SelectedIndex = -1;
                cboRIORdenCompra.SelectedIndex = -1;
                //tbRegInfo.IsEnabled = false; //Tab Javier
                //btnCargarDoc.Visibility = Visibility.Visible;
                //btnDescDoc.Visibility = Visibility.Hidden;
                txbLinkCarga.Visibility = Visibility.Visible;
                txbLinkDescarga.Visibility = Visibility.Hidden;

                cboRIProveedor.Visibility = Visibility.Visible;
                cboRIORdenCompra.Visibility = Visibility.Visible;
                lblRIProveedor.Visibility = Visibility.Hidden;
                lblNroOrden.Visibility = Visibility.Hidden;


                treeListView2.AllowEditing = true;
                txtRIComentarios.IsReadOnly = false;
                DtpRIFechCierre.IsReadOnly = false;
                btnCargarDoc.IsEnabled = true;
                tblRIOTComp.Rows.Clear();
                //tabItem1.IsEnabled = true; //Tab Javier
                cboRIProveedor.SelectedIndexChanged += new RoutedEventHandler(cboRIProveedor_SelectedIndexChanged);
                cboRIORdenCompra.SelectedIndexChanged += new RoutedEventHandler(cboRIORdenCompra_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private bool ValidaCampoObligadoInformeProveedor()
        {
            bool bolRpta = false;
            try
            {
                //if (DtpRIFechCierre.EditValue == null)
                //{
                //    bolRpta = true;
                //    GlobalClass.ip.Mensaje("Seleccione una fecha de cierre correcta", 2);
                //    DtpRIFechCierre.Focus();
                //}
                //else if (DtpRIFechCierre.EditValue.ToString().Trim() == "")
                //{
                //    bolRpta = true;
                //    GlobalClass.ip.Mensaje("Seleccione una fecha de cierre correcta", 2);
                //    DtpRIFechCierre.Focus();
                //}

                if (cboRIProveedor.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_IPRO_PROV"), 2);
                    cboRIProveedor.Focus();
                }
                else if (cboRIORdenCompra.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_IPRO_ORDE"), 2);
                    cboRIORdenCompra.Focus();
                }
                else if (lblRIUbicacion.Content.ToString() == "")
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_IPRO_DOCU"), 2);
                    lblRIUbicacion.Focus();
                }
                return bolRpta;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                return bolRpta;
            }
        }
        //{
        //    bool bolRpta = false;
        //    try
        //    {
        //        if (DtpRIFechCierre.EditValue == null)
        //        {
        //            bolRpta = true;
        //            GlobalClass.ip.Mensaje("Seleccione una fecha de cierre correcta", 2);
        //            DtpRIFechCierre.Focus();
        //        }
        //        else if (cboRIProveedor.SelectedIndex == -1)
        //        {
        //            bolRpta = true;
        //            GlobalClass.ip.Mensaje("Seleccione un proveedor", 2);
        //            cboRIProveedor.Focus();
        //        }
        //        else if (cboRIORdenCompra.SelectedIndex == -1)
        //        {
        //            bolRpta = true;
        //            GlobalClass.ip.Mensaje("Seleccione una orden de compra", 2);
        //            cboRIORdenCompra.Focus();
        //        }
        //        else if (lblRIUbicacion.Text == "")
        //        {
        //            bolRpta = true;
        //            GlobalClass.ip.Mensaje("Es necesario adjuntar un documento", 2);
        //            lblRIUbicacion.Focus();
        //        }
        //        return bolRpta;
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobalClass.ip.Mensaje(ex.Message, 3);
        //        Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
        //        return bolRpta;
        //    }
        //}

        private void LlenarConsultaRegInforme()
        {
            try
            {

                LlenarDetallesRegInforme();
                if (tblOTInforme.Rows[0]["FechaCierre"] != DBNull.Value)
                {
                    DtpRIFechCierre.EditValue = Convert.ToDateTime(tblOTInforme.Rows[0]["FechaCierre"]);
                }
                cboRIProveedor.Visibility = Visibility.Hidden;
                cboRIORdenCompra.Visibility = Visibility.Hidden;
                lblRIProveedor.Visibility = Visibility.Visible;
                lblNroOrden.Visibility = Visibility.Visible;

                lblRIProveedor.Content = tblOTInforme.Rows[0]["RazonSocialProv"].ToString();
                lblNroOrden.Content = tblOTInforme.Rows[0]["NroOCProv"].ToString();
                lblRICosto.Content = tblOTInforme.Rows[0]["Costo"].ToString();
                lblRIRuc.Content = tblOTInforme.Rows[0]["RUCProv"].ToString();

                //cboRIProveedor.EditValue = tblOTInforme.Rows[0]["CodProveedor"].ToString();
                //cboRIORdenCompra.Text = tblOTInforme.Rows[0]["NroOCProv"].ToString();

                lblRIUbicacionDescarga.Content = tblOTInforme.Rows[0]["NombreFile"].ToString();
                txtRIComentarios.Text = tblOTInforme.Rows[0]["Observacion"].ToString();

                lblAuditoria_creacionRP.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblOTInforme.Rows[0]["UsuarioCreacion"].ToString(), tblOTInforme.Rows[0]["FechaCreacion"].ToString(), tblOTInforme.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacionRP.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblOTInforme.Rows[0]["UsuarioModificacion"].ToString(), tblOTInforme.Rows[0]["FechaModificacion"].ToString(), tblOTInforme.Rows[0]["HostModificacion"].ToString());

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void LlenarDetallesRegInforme()
        {
            try
            {
                tblRIOTComp.Rows.Clear();
                objE_OT.IdOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdOT"));

                DataTable tblDetRegInf = objB_OT.OT_Get(objE_OT);
                txtRICodOT.Text = tblDetRegInf.Rows[0]["CodOT"].ToString();
                lblRIFecLiberacion.Content = tblDetRegInf.Rows[0]["FechaLiber"].ToString();
                lblRIResponsable.Content = tblDetRegInf.Rows[0]["NombreResponsable"].ToString();
                lblRICodUC.Content = tblDetRegInf.Rows[0]["CodUC"].ToString();
                lblRITipoOT.Content = tblDetRegInf.Rows[0]["TipoOT"].ToString();

                DataRow row1 = tblRIOTComp.NewRow();
                row1["IdOTComp"] = 0;
                row1["IdUCComp"] = 0;
                row1["IdOT"] = 0;
                row1["IdPerfilComp"] = 0;
                row1["IdPerfilCompPadre"] = 1000;
                row1["PerfilComp"] = lblRICodUC.Content.ToString();
                row1["IdTipoDetalle"] = 0;
                row1["IdItem"] = 1000;
                row1["NroSerie"] = "";
                row1["CodigoSAP"] = "";
                row1["DescripcionSAP"] = "";
                row1["IdEstadoOTComp"] = 1;
                row1["FlagActivo"] = true;
                row1["Nuevo"] = false;
                row1["IsEnabled"] = false;
                tblRIOTComp.Rows.Add(row1);

                objE_OTComp.IdOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdOT"));
                objE_OTComp.CodUC = lblRICodUC.Content.ToString();
                objEUCComp.IdPerfil = Convert.ToInt32(tblDetRegInf.Rows[0]["IdPerfil"]);
                objEPerfilComp = new E_PerfilComp();
                objEPerfilComp.Idperfil = Convert.ToInt32(tblDetRegInf.Rows[0]["IdPerfil"]);
                objEPerfilComp.Idestadopc = 0;
                DataTable tblPerfilComponentesDatos = objBPerfilComp.PerfilComp_List(objEPerfilComp);//objBUCComp.PerfilComp_List(objEUCComp);
                DataTable tblCompDatos = objB_OTComp.OTComp_List(objE_OTComp);
                DataTable tblActDatos = objB_OT.OTActividad_Combo(objE_OT);

                int idpadre = 0;
                int CantExiste = 0;
                int CantExisteDatos = 0;
                int IdPerfilComp = Convert.ToInt32(tblCompDatos.Compute("MAX(IdPerfilComp)", ""));
                DataView dtvPerfilComp = new DataView();
                for (int i = 0; i < tblCompDatos.Rows.Count; i++)
                {
                    DataRow row = tblRIOTComp.NewRow();
                    row["IdOTComp"] = Convert.ToInt32(tblCompDatos.Rows[i]["IdOTComp"]);
                    row["IdUCComp"] = Convert.ToInt32(tblCompDatos.Rows[i]["IdUCComp"]);
                    row["IdOT"] = Convert.ToInt32(tblCompDatos.Rows[i]["IdOT"]);
                    row["IdPerfilComp"] = Convert.ToInt32(tblCompDatos.Rows[i]["IdPerfilComp"]); //IdPerfilComp;
                    row["IdPerfilCompPadre"] = Convert.ToInt32(tblCompDatos.Rows[i]["IdPerfilCompPadre"]);
                    row["PerfilComp"] = tblCompDatos.Rows[i]["PerfilComp"].ToString();
                    row["IdTipoDetalle"] = 0;// Convert.ToInt32(tblCompDatos.Rows[i]["IdTipoDetalle"]);
                    row["NroSerie"] = tblCompDatos.Rows[i]["NroSerie"].ToString();
                    row["CodigoSAP"] = tblCompDatos.Rows[i]["CodigoSAP"].ToString();
                    row["DescripcionSAP"] = tblCompDatos.Rows[i]["DescripcionSAP"].ToString();
                    row["IdEstadoOTComp"] = Convert.ToInt32(tblCompDatos.Rows[i]["IdEstadoOTComp"]);
                    row["FlagActivo"] = true;
                    row["Nuevo"] = false;
                    row["IsEnabled"] = false;
                    tblRIOTComp.Rows.Add(row);

                    foreach (DataRow drActiv in tblActDatos.Select("IdOTComp = " + Convert.ToInt32(tblCompDatos.Rows[i]["IdOTComp"]) + " AND FlagExterna = True"))
                    {
                        IdPerfilComp++;
                        row = tblRIOTComp.NewRow();
                        row["IdOTComp"] = Convert.ToInt32(drActiv["IdOTComp"]);
                        row["IdPerfilCompPadre"] = Convert.ToInt32(tblCompDatos.Rows[i]["IdPerfilComp"]); //IdPerfilComp;
                        //IdPerfilComp++;
                        row["IdPerfilComp"] = IdPerfilComp;//Convert.ToInt32(drActiv["IdOTCompActividad"]);
                        row["IdOTCompActividad"] = Convert.ToInt32(drActiv["IdOTCompActividad"]);
                        row["PerfilComp"] = drActiv["Actividad"].ToString();
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        row["IsChecked"] = Convert.ToBoolean(drActiv["FlagRealizado"]);
                        row["IsEnabled"] = true;
                        row["IsActividad"] = true;
                        tblRIOTComp.Rows.Add(row);
                    }
                    idpadre = Convert.ToInt32(tblCompDatos.Rows[i]["IdPerfilCompPadre"]);
                    CantExiste = tblCompDatos.Select("IdPerfilComp = " + idpadre.ToString()).Length;
                    CantExisteDatos = tblRIOTComp.Select("IdPerfilComp = " + idpadre.ToString()).Length;
                    while (idpadre != 0 && CantExiste == 0 && CantExisteDatos == 0)
                    {
                        dtvPerfilComp = new DataView();
                        dtvPerfilComp = tblPerfilComponentesDatos.DefaultView;
                        dtvPerfilComp.RowFilter = "IdPerfilComp = " + idpadre.ToString();
                        DataRow row2 = tblRIOTComp.NewRow();
                        row2["IdOTComp"] = 0;
                        row2["IdUCComp"] = 0;
                        row2["IdOT"] = 0;
                        row2["IdPerfilComp"] = Convert.ToInt32(dtvPerfilComp[0]["IdPerfilComp"]); //IdPerfilComp;
                        row2["IdPerfilCompPadre"] = Convert.ToInt32(dtvPerfilComp[0]["IdPerfilCompPadre"]);
                        row2["PerfilComp"] = dtvPerfilComp[0]["PerfilComp"].ToString();
                        row2["IdTipoDetalle"] = 0;
                        row2["IdItem"] = 0;
                        row2["NroSerie"] = "";
                        row2["CodigoSAP"] = dtvPerfilComp[0]["CodigoSAP"].ToString();
                        row2["DescripcionSAP"] = dtvPerfilComp[0]["DescripcionSAP"].ToString();
                        row2["IdEstadoOTComp"] = 0;
                        row2["FlagActivo"] = true;
                        row2["Nuevo"] = false;
                        row2["IsEnabled"] = false;
                        tblRIOTComp.Rows.Add(row2);
                        idpadre = Convert.ToInt32(dtvPerfilComp[0]["IdPerfilCompPadre"]);
                        CantExiste = tblCompDatos.Select("IdPerfilComp = " + idpadre.ToString()).Length;
                        CantExisteDatos = tblRIOTComp.Select("IdPerfilComp = " + idpadre.ToString()).Length;
                        //IdPerfilComp++;
                    }
                    CantExiste = 0;
                    idpadre = 0;
                }
                trvRIComponente.ItemsSource = tblRIOTComp;
                treeListView2.ExpandAllNodes();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnDescargarDocumento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog svg = new Microsoft.Win32.SaveFileDialog();
                svg.Filter = "All files (*.*)|*.*";
                svg.FileName = lblRIUbicacionDescarga.Content.ToString();
                svg.AddExtension = true;
                svg.ValidateNames = true;
                Nullable<bool> result = svg.ShowDialog();
                if (result == true)
                {
                    fileName = svg.SafeFileName;
                    string sourcePath = System.IO.Path.GetDirectoryName(svg.FileName);
                    sourceFile = System.IO.Path.Combine(sourcePath, fileName);

                    string pathString = System.IO.Path.Combine(targetPath, txtRICodOT.Text);
                    string destFile = System.IO.Path.Combine(pathString, tblOTInforme.Rows[0]["NombreFile"].ToString());
                    System.IO.File.Copy(destFile, sourceFile, true);
                    GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "MENS_DESC_ARCH"), svg.SafeFileName), 1);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void EstadoForm(Boolean stdNuevo, Boolean stdEditar, Boolean stdForzar)
        {
            try
            {
                if (stdForzar == true)
                {
                    gbolNuevo = stdNuevo; gbolEdicion = stdEditar;
                }
                else if (stdForzar == false)
                {
                    if (gbolNuevo == false)
                    {
                        gbolNuevo = stdNuevo; gbolEdicion = stdEditar;
                    }
                }

                if ((gbolNuevo == false) && (gbolEdicion == false))
                {
                    tabInformesOT.Header = "Consultar Informe Proveedor";
                    BtnRegistrarInformeP.Content = "Aceptar";
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tabInformesOT.Header = "Registrar Informe Proveedor";
                    BtnRegistrarInformeP.Content = "Registrar";
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tabInformesOT.Header = "Modificar Informe Proveedor";
                    BtnRegistrarInformeP.Content = "Modificar";
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private bool ValidarHora(string Hora)
        {
            bool Value = false;

            if (Hora != "")
            {
                if (Hora.ToString().Length == 5)
                {
                    if (Hora.ToString().Contains(':') == true)
                    {
                        string[] x = Hora.ToString().Split(':');
                        if (x.Count() == 2)
                        {
                            if (x[0] != "")
                            {
                                if (Convert.ToInt32(x[0]) < 0 && Convert.ToInt32(x[0]) > 24)
                                {
                                    GlobalClass.ip.Mensaje("Formato de Hora incorrecta (HH:mm)", 2);
                                    return true;
                                }
                            }
                            else
                            {
                                GlobalClass.ip.Mensaje("Formato de Hora incorrecta (HH:mm)", 2);
                                return true;
                            }
                            if (x[1] != "")
                            {
                                if (Convert.ToInt32(x[1]) < 0 && Convert.ToInt32(x[1]) > 59)
                                {
                                    GlobalClass.ip.Mensaje("Formato de minutos incorrecta (HH:mm)", 2);
                                    return true;
                                }
                            }
                            else
                            {
                                GlobalClass.ip.Mensaje("Formato de minutos incorrecta (HH:mm)", 2);
                                return true;
                            }
                        }
                        else
                        {
                            GlobalClass.ip.Mensaje("Formato de Hora incorrecta (HH:mm)", 2);
                            return true;
                        }

                    }
                    else
                    {
                        GlobalClass.ip.Mensaje("Formato de Hora incorrecta (HH:mm)", 2);
                        return true;
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje("Formato de Hora incorrecta (HH:mm)", 2);
                    return true;
                }
            }
            else
            {
                GlobalClass.ip.Mensaje("Ingresar Hora Correcta", 2);
                return true;
            }
            return Value;
        }
        private void LimpiarTareasRealizadas()
        {
            //trlComponente.ItemsSource = null;
            grvListarTrabajador.ItemsSource = null;
            grvHerrEspTarea.ItemsSource = null;
            DtgRespuestosConsumibles.ItemsSource = null;
            dtpFechaCierreT.EditValue = DateTime.Now;
            txtHorasTarea.Text = "";
            tblOTCompTreeList.Rows.Clear();
            tblArticuloTarea.Rows.Clear();
            tblHerrEspTarea.Rows.Clear();
            tbOTTareaTrabajador.Rows.Clear();
            tabItem4.IsEnabled = false;
            //tabItem9.IsEnabled = false; //Tab Javier
        }
        private void ActualizarEstadoOTTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (DataRow drArtiDet in tblArticuloTarea.Select("IdTipoArticulo = 4"))
                {
                    if (drArtiDet["NroSerie"].ToString().Trim() == "")
                    {
                        GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_CERR_NROS"), drArtiDet["Articulo"].ToString()), 2);
                        return;
                    }
                }

                if (dtpFechaCierreT.EditValue == null)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_RTAR_FECH_CIER"), 2);
                    dtpFechaCierreT.Focus();
                    return;
                }
                var rpt = DevExpress.Xpf.Core.DXMessageBox.Show(string.Format("Seguro de Cerrar la OT Nro: {0} ?", TxTNumeroOT.Text), "Cambio de Estado", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rpt == MessageBoxResult.Yes)
                {
                    //Grabar OT

                    if (GrabarTarea() == false)
                    {
                        return;
                    }

                    objE_OT = new E_OT();
                    objE_OT.IdOT = IdOT;
                    objE_OT.IdUsuario = gintIdUsuario;
                    int x = objB_OT.OTCompActividadEstado_Insert(objE_OT, tblActividades);

                    tblFrecuencias = new DataTable();
                    tblFrecuencias.Columns.Add("IdOTComp", Type.GetType("System.Int32"));
                    tblFrecuencias.Columns.Add("IdOTCompActividad", Type.GetType("System.Int32"));
                    tblFrecuencias.Columns.Add("IdOTArticulo", Type.GetType("System.Int32"));
                    tblFrecuencias.Columns.Add("IdTipoArticulo", Type.GetType("System.Int32"));
                    tblFrecuencias.Columns.Add("IdArticulo", Type.GetType("System.String"));
                    tblFrecuencias.Columns.Add("NroSerie", Type.GetType("System.String"));
                    tblFrecuencias.Columns.Add("Frecuencia", Type.GetType("System.Double"));
                    tblFrecuencias.Columns.Add("IdCiclo", Type.GetType("System.Double"));
                    tblFrecuencias.Columns.Add("FlagExtendido", Type.GetType("System.Boolean"));
                    tblFrecuencias.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                    foreach (DataRow drArtiDet in tblArticuloTarea.Select("IdTipoArticulo = 4"))
                    {
                        if (Convert.ToBoolean(drArtiDet["FlagExtendida"]))
                        {
                            if (Convert.ToBoolean(drArtiDet["FlagExtendida"]) && Convert.ToDouble(drArtiDet["Frecuencia"]) < 0)
                            {
                                DataRow drFrec = tblFrecuencias.NewRow();
                                drFrec["IdOTComp"] = drArtiDet["IdOTComp"];
                                drFrec["IdOTCompActividad"] = drArtiDet["IdOTCompActividad"];
                                drFrec["IdOTArticulo"] = drArtiDet["IdOTArticulo"];
                                drFrec["IdTipoArticulo"] = drArtiDet["IdTipoArticulo"];
                                drFrec["IdArticulo"] = drArtiDet["IdArticulo"];
                                drFrec["NroSerie"] = drArtiDet["NroSerie"];
                                drFrec["Frecuencia"] = drArtiDet["Frecuencia"];
                                drFrec["IdCiclo"] = 3;
                                drFrec["FlagExtendido"] = true;
                                drFrec["Nuevo"] = false;
                                tblFrecuencias.Rows.Add(drFrec);
                            }
                            else if (Convert.ToBoolean(drArtiDet["FlagExtendida"]) && Convert.ToDouble(drArtiDet["FrecuenciaTie"]) > 0)
                            {
                                DataRow drFrec = tblFrecuencias.NewRow();
                                drFrec["IdOTComp"] = drArtiDet["IdOTComp"];
                                drFrec["IdOTCompActividad"] = drArtiDet["IdOTCompActividad"];
                                drFrec["IdOTArticulo"] = drArtiDet["IdOTArticulo"];
                                drFrec["IdTipoArticulo"] = drArtiDet["IdTipoArticulo"];
                                drFrec["IdArticulo"] = drArtiDet["IdArticulo"];
                                drFrec["NroSerie"] = drArtiDet["NroSerie"];
                                drFrec["Frecuencia"] = drArtiDet["FrecuenciaTie"];
                                drFrec["IdCiclo"] = 4;
                                drFrec["FlagExtendido"] = true;
                                drFrec["Nuevo"] = false;
                                tblFrecuencias.Rows.Add(drFrec);
                            }

                        }
                        else
                        {
                            DataRow drFrec = tblFrecuencias.NewRow();
                            drFrec["IdOTComp"] = drArtiDet["IdOTComp"];
                            drFrec["IdOTCompActividad"] = drArtiDet["IdOTCompActividad"];
                            drFrec["IdOTArticulo"] = drArtiDet["IdOTArticulo"];
                            drFrec["IdTipoArticulo"] = drArtiDet["IdTipoArticulo"];
                            drFrec["IdArticulo"] = drArtiDet["IdArticulo"];
                            drFrec["NroSerie"] = drArtiDet["NroSerie"];
                            drFrec["Frecuencia"] = drArtiDet["Frecuencia"];
                            drFrec["IdCiclo"] = 3;
                            drFrec["FlagExtendido"] = false;
                            drFrec["Nuevo"] = false;
                            tblFrecuencias.Rows.Add(drFrec);

                            drFrec = tblFrecuencias.NewRow();
                            drFrec["IdOTComp"] = drArtiDet["IdOTComp"];
                            drFrec["IdOTCompActividad"] = drArtiDet["IdOTCompActividad"];
                            drFrec["IdOTArticulo"] = drArtiDet["IdOTArticulo"];
                            drFrec["IdTipoArticulo"] = drArtiDet["IdTipoArticulo"];
                            drFrec["IdArticulo"] = drArtiDet["IdArticulo"];
                            drFrec["NroSerie"] = drArtiDet["NroSerie"];
                            drFrec["Frecuencia"] = drArtiDet["FrecuenciaTie"];
                            drFrec["IdCiclo"] = 4;
                            drFrec["FlagExtendido"] = false;
                            drFrec["Nuevo"] = false;
                            tblFrecuencias.Rows.Add(drFrec);
                        }
                    }

                    //Grabar Item para Componente
                    objE_OT = new E_OT();
                    objE_OT.IdUsuario = gintIdUsuario;
                    int xcant = objB_OT.OT_UpdatebyItem(tblRepuesto, objE_OT, tblFrecuencias);

                    //Graba SAP
                    //Conectarse a SAP
                    objE_TablaMaestra.IdTabla = 43;
                    DataTable tblTipoOperacion = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
                    objE_TablaMaestra.IdTabla = 45;
                    DataTable CuentaContable = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
                    objE_TablaMaestra.IdTabla = 42;
                    DataTable tblAlmacen = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);

                    DataTable tbl = tblArticuloTarea.DefaultView.ToTable(true, "NroSolicitudTransferencia");
                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                        OIGE = new InterfazMTTO.iSBO_BE.BEOIGE();
                        OIGE.FechaSolicitud = Convert.ToDateTime(dtpFechaCierreT.EditValue);
                        OIGE.NroOrdenTrabajo = TxTNumeroOT.Text;
                        OIGE.NroSolicitudTransferencia = Convert.ToInt32(tbl.Rows[i]["NroSolicitudTransferencia"]);
                        InterfazMTTO.iSBO_BE.BEIGE1List IGE1List = new InterfazMTTO.iSBO_BE.BEIGE1List();
                        DataView dtvArticulo = tblArticuloTarea.DefaultView;
                        dtvArticulo.RowFilter = "NroSolicitudTransferencia = " + Convert.ToInt32(tbl.Rows[i]["NroSolicitudTransferencia"]);

                        for (int j = 0; j < dtvArticulo.Count; j++)
                        {
                            if (Convert.ToInt32(dtvArticulo[j]["CantUti"]) > 0)
                            {
                                IGE1 = new InterfazMTTO.iSBO_BE.BEIGE1();
                                IGE1.NroOrdenTrabajo = TxTNumeroOT.Text;
                                IGE1.NroLineaOrdenTrabajo = Convert.ToInt32(dtvArticulo[j]["IdOTArticulo"]);
                                IGE1.CodigoArticulo = Convert.ToString(dtvArticulo[j]["IdArticulo"]);
                                IGE1.AlmacenSalida = tblAlmacen.Rows[1]["Valor"].ToString();
                                IGE1.CantidadSalida = Convert.ToInt32(dtvArticulo[j]["CantUti"]);
                                IGE1.TipoOperacion = Convert.ToString(tblTipoOperacion.Rows[0]["Valor"]);
                                IGE1.CuentaContable = Convert.ToString(CuentaContable.Rows[0]["Valor"]);
                                IGE1List.Add(IGE1);
                            }
                        }

                        if (IGE1List.Count > 0)
                        {
                            string DocEntry = "";
                            InterfazMTTO.iSBO_BE.BEIGE1List IGE1List2 = new InterfazMTTO.iSBO_BE.BEIGE1List();
                            IGE1List2 = InterfazMTTO.iSBO_BL.SalidaMercancia_BL.RegistraSalidaMercancia(OIGE, IGE1List, ref RPTA, ref DocEntry);

                            if (RPTA.ResultadoRetorno != 0)
                            {
                                GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                                return;
                            }
                            else
                            {
                                //Actualizar datos OTArticulo Mercaderia
                                #region COSTO_ARTICULO_SALIDA
                                int TipoProceso = 2;
                                int iDocEntry = 0;
                                iDocEntry = Convert.ToInt32(DocEntry);
                                #endregion

                                tblOTArticuloSol.Rows.Clear();
                                for (int j = 0; j < IGE1List2.Count; j++)
                                {
                                    DataRow dr = tblOTArticuloSol.NewRow();
                                    dr["IdOTArticulo"] = IGE1List2[j].NroLineaOrdenTrabajo;
                                    dr["NroSalMercancia"] = IGE1List2[j].NroSalidaMercancia;
                                    dr["NroLinSalMercancia"] = IGE1List2[j].NroLineaSalidaMercancia;

                                    #region COSTO_ARTICULO_SALIDA
                                    RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                                    OITW_List = InterfazMTTO.iSBO_BL.Articulo_BL.ObtenerCostoArticulo(IGE1List2[j].CodigoArticulo.ToString(), TipoProceso, iDocEntry, ref RPTA);
                                    if (RPTA.ResultadoRetorno == 0)
                                    {

                                        dr["CostoArticulo"] = Convert.ToDouble(OITW_List[0].CostoArticulo);
                                    }
                                    else
                                    {
                                        dr["CostoArticulo"] = 0;
                                    }
                                    #endregion
                                    tblOTArticuloSol.Rows.Add(dr);
                                }
                                int z = objB_OT.OTArticulo_Update(2, tblOTArticuloSol);
                            }
                        }
                    }

                    OWTQ = new InterfazMTTO.iSBO_BE.BEOWTQ();
                    OWTQ.NroOrdenTrabajo = TxTNumeroOT.Text;
                    OWTQ.AlmacenEntrada = tblAlmacen.Rows[0]["Valor"].ToString(); //Alm Mantenimient
                    OWTQ.AlmacenSalida = tblAlmacen.Rows[1]["Valor"].ToString();//General
                    OWTQ.FechaSolicitud = Convert.ToDateTime(dtpFechaCierreT.EditValue);
                    WTQ1List = new InterfazMTTO.iSBO_BE.BEWTQ1List();

                    for (int i = 0; i < tblArticuloTarea.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(tblArticuloTarea.Rows[i]["CantEnv"]) > Convert.ToInt32(tblArticuloTarea.Rows[i]["CantUti"]))
                        {
                            InterfazMTTO.iSBO_BE.BEWTQ1 BEWTQ1 = new InterfazMTTO.iSBO_BE.BEWTQ1();
                            BEWTQ1.NroOrdenTrabajo = TxTNumeroOT.Text;
                            BEWTQ1.NroLinea = Convert.ToInt32(tblArticuloTarea.Rows[i]["IdOTArticulo"]);//De la BD
                            BEWTQ1.CodigoArticulo = Convert.ToString(tblArticuloTarea.Rows[i]["IdArticulo"]);
                            BEWTQ1.CantidadSolicitada = Convert.ToInt32(tblArticuloTarea.Rows[i]["CantEnv"]) - Convert.ToInt32(tblArticuloTarea.Rows[i]["CantUti"]);
                            BEWTQ1.TipoOperacion = tblTipoOperacion.Rows[0]["Valor"].ToString(); //Tabla Maestra  --> 12
                            WTQ1List.Add(BEWTQ1);
                        }
                    }

                    if (WTQ1List.Count > 0)
                    {
                        InterfazMTTO.iSBO_BE.BEWTQ1List WTQ1List2 = new InterfazMTTO.iSBO_BE.BEWTQ1List();
                        WTQ1List2 = InterfazMTTO.iSBO_BL.SolicitudTransferencia_BL.RegistraSolicitudTransferencia(OWTQ, WTQ1List, ref RPTA);
                        if (RPTA.ResultadoRetorno == 0)
                        {

                            tblOTArticuloSol.Rows.Clear();
                            //Actualizar datos OTArticulo
                            for (int i = 0; i < WTQ1List2.Count; i++)
                            {
                                DataRow dr = tblOTArticuloSol.NewRow();
                                dr["IdOTArticulo"] = WTQ1List2[i].NroLineaOT;
                                dr["NroSolDevolucion"] = WTQ1List2[i].NroSolicitudTransferencia;
                                dr["NroLinSolDevolucionr"] = WTQ1List2[i].NroLinea;
                                tblOTArticuloSol.Rows.Add(dr);
                            }
                            int z = objB_OT.OTArticulo_Update(3, tblOTArticuloSol);

                            LimpiarForm();
                            GlobalClass.ip.SeleccionarTab(tabListadoOT);
                            //tabControl1.SelectedIndex = 0; //Tab Javier
                            ListarOT();
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_RTAR_ESTA"), 1);
                            LimpiarTareasRealizadas();
                        }
                        else
                        {
                            GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                        }
                    }
                    //Actualiza Estado
                    objE_OT = new E_OT();
                    objE_OT.IdOT = IdOT;
                    objE_OT.IdEstadoOT = 5;
                    objE_OT.FechaCierre = Convert.ToDateTime(dtpFechaCierreT.EditValue);
                    objE_OT.IdUsuarioModificacion = Utilitarios.Utilitarios.gintIdUsuario;
                    objE_OT.IsRegProveedor = 0;
                    objE_OT.FechaModificacion = DateTime.Now;
                    int cant = objB_OT.OT_UpdateEstado(objE_OT);
                    if (cant == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_RTAR_CERR"), 1);
                        LimpiarTareasRealizadas();
                        GlobalClass.ip.SeleccionarTab(tabListadoOT);
                        //tabControl1.SelectedIndex = 0; //Tab Javier
                        ListarOT();
                    }
                    else if (cant == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (cant == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_CONC"), 2);
                        return;
                    }


                    # region ENVIAR_CORREO_USUARIOS_ALMACEN
                    string cuerpoEmail = "", asuntoEmail = "";
                    objE_OT = new E_OT();
                    objE_OT.IdOT = IdOT;
                    cuerpoEmail = objB_OTArticulo.BodyEmail(objE_OT);
                    asuntoEmail = "OT: " + objB_OTArticulo.SubjectEmail(objE_OT) + " - REPUESTOS NO UTILIZADOS";

                    DataTable tlbcorreo = new DataTable();
                    B_Correo objco = new B_Correo();
                    string usuario;
                    string servidor;
                    string password;
                    string puerto;
                    int Ltbl;

                    tlbcorreo = objco.Correo_List(); //Tabla de configuración del correo emisor

                    if (tlbcorreo.Rows.Count > 0)
                    {
                        usuario = tlbcorreo.Rows[0]["Correo"].ToString();
                        servidor = tlbcorreo.Rows[0]["Srv"].ToString();
                        password = tlbcorreo.Rows[0]["Pwd"].ToString();
                        puerto = tlbcorreo.Rows[0]["Puerto"].ToString();

                        B_Usuario objcor = new B_Usuario();
                        DataTable tlvcor = new DataTable(); //Listado de usuarios con el flag de gerencia operativa 
                        List<InterfazMTTO.iSBO_BE.BEOUSR> ListaUsuarioDepartment;
                        ListaUsuarioDepartment = InterfazMTTO.iSBO_BL.Usuario_BL.ListarUsuariosDepartment(13, ref RPTA);
                        string correos = "";

                        if (RPTA.ResultadoRetorno == 0)
                        {
                            tlvcor = Utilitarios.Utilitarios.ToDataTable(ListaUsuarioDepartment);
                            Ltbl = tlvcor.Rows.Count;

                            if (Ltbl > 0)
                            {
                                for (int i = 0; i < Ltbl; i++)
                                {
                                    if (tlvcor.Rows[i]["Correo"].ToString().Contains("@"))
                                    {
                                        correodest = tlvcor.Rows[i]["Correo"].ToString();

                                        if (i != Ltbl - 1)
                                        {
                                            correos = correos + correodest + ";";
                                        }
                                        else
                                        {
                                            correos = correos + correodest;
                                        }
                                    }
                                }

                                char[] delimitador = new char[] { ';' };
                                MailMessage message = new MailMessage();
                                foreach (string destinos in correos.Split(delimitador))
                                {
                                    message.To.Add(new MailAddress(destinos));
                                }

                                message.From = new MailAddress(usuario);
                                message.Body = cuerpoEmail;
                                message.Subject = asuntoEmail;
                                message.IsBodyHtml = true;
                                SmtpClient client = new SmtpClient(servidor, int.Parse(puerto));
                                client.EnableSsl = true;
                                client.Credentials = new System.Net.NetworkCredential(usuario, password);
                                client.Send(message);
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void txtHoraFechaCierre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //txtHoraFechaCierre.Text = Utilitarios.Utilitarios.SoloHora(txtHoraFechaCierre.Text);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void txthoraini_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txthoraini.Text = Utilitarios.Utilitarios.SoloHora(txthoraini.Text);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }
        private void txthorafin_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txthoraini.Text = Utilitarios.Utilitarios.SoloHora(txthoraini.Text);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }

        private void ListarDatosRegistraTarea()
        {
            LimpiarTareasRealizadas();
            //Listar Tarea
            //LimpiarForm();
            int ValorGrilla = Convert.ToInt32(dtgOT.GetCellDisplayText(tblvOT.FocusedRowHandle, "IdOT"));
            IdOT = ValorGrilla;

            //Listar Datos Cabecera
            objE_OT = new E_OT();
            objE_OTComp = new E_OTComp();
            objE_OT.IdOT = IdOT;
            objE_OTComp.IdOT = IdOT;
            DataTable tblOT = objB_OT.OT_Get(objE_OT);

            if (tblOT.Rows.Count > 0)
            {

        
                codTipoReq = tblOT.Rows[0]["CodTipoRequerimiento"].ToString() == "" ? 0 : Convert.ToInt32(tblOT.Rows[0]["CodTipoRequerimiento"].ToString());

                if(codTipoReq == (int)TipoRequerimientoEnum.Averia)
                {

                    #region RequerimientoCelsa
                    //cboTipoRequerimiento.SelectedIndexChanged -= new RoutedEventHandler(cboPrioridad_SelectedIndexChanged);
                    DataTable tbl = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=64", dtv_maestra);
                    tbl.DefaultView.RowFilter = "IdColumna <> 0";
                    cboTipoAveria.ItemsSource = tbl.DefaultView;
                    cboTipoAveria.DisplayMember = "Descripcion";
                    cboTipoAveria.ValueMember = "IdColumna";
                    cboTipoAveria.SelectedIndex = -1;
                    //cboPrioridad.SelectedIndexChanged += new RoutedEventHandler(cboPrioridad_SelectedIndexChanged);
                    #endregion

                    labelTipoAveria.Visibility = Visibility.Visible;
                    cboTipoAveria.Visibility = Visibility.Visible;
                }
                else
                {
                    labelTipoAveria.Visibility = Visibility.Hidden;
                    cboTipoAveria.Visibility = Visibility.Hidden;
                }


                TxTNumeroOT.Text = tblOT.Rows[0]["CodOT"].ToString();
                if (tblOT.Rows[0]["FechaLiber"] == DBNull.Value)
                {
                    LblFechaLiberacion.Content = "";
                }
                else
                {
                    LblFechaLiberacion.Content = Convert.ToDateTime(tblOT.Rows[0]["FechaLiber"]).ToString("dd/MM/yyyy HH:mm");
                }

                if (Convert.ToBoolean(tblOT.Rows[0]["FlagSinUC"]) == false)
                {
                    LblUnidadControl.Content = tblOT.Rows[0]["PlacaSerie"].ToString();
                }
                else
                {
                    LblUnidadControl.Content = tblOT.Rows[0]["Item"].ToString();
                }

                lblTipoOrden.Content = Convert.ToString(tblOT.Rows[0]["TipoOT"]);
                if (tblOT.Rows[0]["FechaCierre"] == DBNull.Value)
                {
                    dtpFechaCierreT.EditValue = null;
                }
                else
                {
                    dtpFechaCierreT.EditValue = Convert.ToDateTime(tblOT.Rows[0]["FechaCierre"]);
                }
                LblResponsable.Content = Convert.ToString(tblOT.Rows[0]["NombreResponsable"]);
                //txtHoraFechaCierre.Text = Convert.ToString(tblOT.Rows[0]["HoraCierre"]);
                CodResponsable = Convert.ToInt32(tblOT.Rows[0]["CodResponsable"]);
                IdTipoOrden = Convert.ToInt32(tblOT.Rows[0]["IdTipoOT"]);
            }

            //Listar TreeList || Revisar porque tienen que construir el datatable asociado al TreeList, en lugar de traerlo armado desde la BD.
            tblOTCompTreeList.Rows.Clear();
            tblOTCompDatos = objB_OTComp.OT_ListCascade(objE_OTComp);

            tblOTCompDetDatos = objB_OTComp.OT_ListCascadeDet(objE_OTComp);

            int cantfilas = tblOTCompDetDatos.Tables[2].Rows.Count;
            if (cantfilas != 0)
            {
                gbolExisteTarea = true;
                ActualizarEstadoOTTarea.IsEnabled = (Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdEstadoOT")) != 5);

                lblAuditoria_creacionRR.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblOT.Rows[0]["UsuarioCreacion"].ToString(), tblOT.Rows[0]["FechaCreacion"].ToString(), tblOT.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacionRR.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblOT.Rows[0]["UsuarioModificacion"].ToString(), tblOT.Rows[0]["FechaModificacion"].ToString(), tblOT.Rows[0]["HostModificacion"].ToString());

            }
            else
            {
                gbolExisteTarea = false;
                ActualizarEstadoOTTarea.IsEnabled = false;

                lblAuditoria_creacionRR.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                lblAuditoria_modificacionRR.Text = String.Format("Usuario: -- Fecha: -- Host: --");
            }

            //tabItem9.IsEnabled = (cantfilas != 0); //Tab Javier Consultas

            for (int i = 0; i < tblOTCompDatos.Tables[0].Rows.Count; i++)
            {
                DataRow row = tblOTCompTreeList.NewRow();
                row["Id"] = Convert.ToString(tblOTCompDatos.Tables[0].Rows[i]["Id"]);
                row["IdPadre"] = Convert.ToString(tblOTCompDatos.Tables[0].Rows[i]["IdPadre"]);
                row["Descripcion"] = Convert.ToString(tblOTCompDatos.Tables[0].Rows[i]["PerfilComp"]);
                row["DescripcionReal"] = Convert.ToString(tblOTCompDatos.Tables[0].Rows[i]["PerfilComp"]);
                row["HorasEstimada"] = 0;
                row["HorasReales"] = 0;
                row["IdTipo"] = 1;
                row["IdReal"] = Convert.ToInt32(tblOTCompDatos.Tables[0].Rows[i]["IdOTComp"]);
                row["IsEnabled"] = false;
                tblOTCompTreeList.Rows.Add(row);
            }

            for (int i = 0; i < tblOTCompDatos.Tables[1].Rows.Count; i++)
            {
                DataRow row = tblOTCompTreeList.NewRow();
                row["Id"] = Convert.ToString(tblOTCompDatos.Tables[1].Rows[i]["Id"]);
                row["IdPadre"] = Convert.ToString(tblOTCompDatos.Tables[1].Rows[i]["IdPadre"]);
                row["Descripcion"] = Convert.ToString(tblOTCompDatos.Tables[1].Rows[i]["Actividad"]);
                row["DescripcionReal"] = Convert.ToString(tblOTCompDatos.Tables[1].Rows[i]["Actividad"]);
                row["ActividadRealizada"] = Convert.ToBoolean(tblOTCompDatos.Tables[1].Rows[i]["FlagRealizado"]);
                row["HorasEstimada"] = 0;
                row["HorasReales"] = 0;
                row["IdTipo"] = 2;
                row["IdReal"] = Convert.ToInt32(tblOTCompDatos.Tables[1].Rows[i]["IdOTCompActividad"]);
                row["IsEnabled"] = false;
                row["IdOTComp"] = Convert.ToString(tblOTCompDatos.Tables[1].Rows[i]["IdOTComp"]);
                row["IdActividad"] = Convert.ToString(tblOTCompDatos.Tables[1].Rows[i]["IdActividad"]);
                tblOTCompTreeList.Rows.Add(row);
            }

            for (int i = 0; i < tblOTCompDatos.Tables[2].Rows.Count; i++)
            {
                DataRow row = tblOTCompTreeList.NewRow();
                row["Id"] = Convert.ToString(tblOTCompDatos.Tables[2].Rows[i]["Id"]);
                row["IdPadre"] = Convert.ToString(tblOTCompDatos.Tables[2].Rows[i]["IdPadre"]);
                row["Descripcion"] = Convert.ToString(tblOTCompDatos.Tables[2].Rows[i]["Tarea"]);
                row["DescripcionReal"] = Convert.ToString(tblOTCompDatos.Tables[2].Rows[i]["Tarea"]);
                row["HorasEstimada"] = Convert.ToString(tblOTCompDatos.Tables[2].Rows[i]["HorasEstimada"]); ;
                row["HorasReales"] = Convert.ToString(tblOTCompDatos.Tables[2].Rows[i]["HorasReal"]);
                row["IdTipo"] = 3;
                row["IdReal"] = Convert.ToInt32(tblOTCompDatos.Tables[2].Rows[i]["IdOTTarea"]);
                row["IsEnabled"] = false;
                tblOTCompTreeList.Rows.Add(row);
            }

            Double horasEstimadasAct;
            Double horasRealesAct;
            Double horasEstimadasCom;
            Double horasRealesCom;

            foreach (DataRow drComponente in tblOTCompTreeList.Select("IdPadre = 0"))
            {
                horasEstimadasCom = 0.00;
                horasRealesCom = 0.00;
                int IdComponente = Convert.ToInt32(drComponente["Id"]);
                foreach (DataRow drActividad in tblOTCompTreeList.Select("IdPadre = " + IdComponente.ToString()))
                {
                    horasEstimadasAct = 0.00;
                    horasRealesAct = 0.00;
                    foreach (DataRow drTarea in tblOTCompTreeList.Select("IdPadre = " + Convert.ToInt32(drActividad["Id"])))
                    {
                        horasEstimadasAct = horasEstimadasAct + Convert.ToDouble(drTarea["HorasEstimada"]);
                        horasRealesAct = horasRealesAct + Convert.ToDouble(drTarea["HorasReales"]);
                    }
                    drActividad["HorasEstimada"] = Math.Round(horasEstimadasAct, 2);
                    drActividad["HorasReales"] = Math.Round(horasRealesAct, 2);
                    horasEstimadasCom = horasEstimadasCom + Math.Round(horasEstimadasAct, 2);
                    horasRealesCom = horasRealesCom + Math.Round(horasRealesAct, 2);
                }
                drComponente["HorasEstimada"] = Math.Round(horasEstimadasCom, 2);
                drComponente["HorasReales"] = Math.Round(horasRealesCom, 2);
            }

            for (int i = 0; i < tblOTCompDetDatos.Tables[0].Rows.Count; i++)
            {
                DataRow row = tblHerrEspTarea.NewRow();
                row["IdOTComp"] = Convert.ToString(tblOTCompDetDatos.Tables[0].Rows[i]["IdOTComp"]);
                row["IdOTCompActividad"] = Convert.ToString(tblOTCompDetDatos.Tables[0].Rows[i]["IdOTCompActividad"]);
                row["IdOTHerramienta"] = Convert.ToString(tblOTCompDetDatos.Tables[0].Rows[i]["IdOTHerramienta"]);
                row["IdArticulo"] = Convert.ToString(tblOTCompDetDatos.Tables[0].Rows[i]["IdArticulo"]);
                row["Codigo"] = Convert.ToString(tblOTCompDetDatos.Tables[0].Rows[i]["CodHerramienta"]);
                row["Articulo"] = Convert.ToString(tblOTCompDetDatos.Tables[0].Rows[i]["Articulo"]);
                row["Cantidad"] = Convert.ToString(tblOTCompDetDatos.Tables[0].Rows[i]["Cantidad"]);
                row["IdEstado"] = Convert.ToString(tblOTCompDetDatos.Tables[0].Rows[i]["IdEstado"]);
                row["Estado"] = Convert.ToString(tblOTCompDetDatos.Tables[0].Rows[i]["Estado"]);
                row["NroDevolucion"] = Convert.ToString(tblOTCompDetDatos.Tables[0].Rows[i]["NroDevolucion"]);
                tblHerrEspTarea.Rows.Add(row);
            }

            if (tblOTCompDetDatos.Tables[1].Rows.Count != 0)
            {
                //Extraer Cant Entrega SAP
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                WTQ1List = InterfazMTTO.iSBO_BL.SolicitudTransferencia_BL.ObtenerTransferencia(TxTNumeroOT.Text, ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                    //return;
                }
            }
            for (int i = 0; i < tblOTCompDetDatos.Tables[1].Rows.Count; i++)
            {
                DataRow row = tblArticuloTarea.NewRow();
                row["IdOTComp"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["IdOTComp"]);
                row["IdOTCompActividad"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["IdOTCompActividad"]);
                row["IdOTArticulo"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["IdOTArticulo"]);
                row["IdTipoArticulo"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["IdTipoArticulo"]);
                row["IdArticulo"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["IdArticulo"]);
                row["Articulo"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["Articulo"]);
                row["NroSerie"] = tblOTCompDetDatos.Tables[1].Rows[i]["NroSerie"].ToString();

                row["Frecuencia"] = Convert.ToDouble(tblOTCompDetDatos.Tables[1].Rows[i]["FrecuenciaDis"]);
                row["FrecuenciaTie"] = Convert.ToDouble(tblOTCompDetDatos.Tables[1].Rows[i]["FrecuenciaTie"]);
                row["FlagExtendida"] = Convert.ToBoolean(tblOTCompDetDatos.Tables[1].Rows[i]["FlagExtendida"]);

                row["CantSol"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["CantSol"]);
                row["CantEnv"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["CantEnv"]);
                row["NroSolicitudTransferencia"] = 0;
                //row["CostoArticulo"] = 0;

                if (WTQ1List.Count == 0)
                {
                    #region COSTO_ARTICULO_CREAR_OT
                    //Obtener Costo Articulo SAP
                    //RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                    //OITW_List = InterfazMTTO.iSBO_BL.Articulo_BL.ObtenerCostoArticulo(Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["IdArticulo"]), ref RPTA);
                    //if (RPTA.ResultadoRetorno != 0)
                    //{
                    //    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                    //    //return;
                    //}

                    //row["CostoArticulo"] = Convert.ToDouble(OITW_List[0].CostoArticulo);
                    row["CostoArticulo"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["CostoArticulo"]);
                    #endregion
                }
                else
                {
                    for (int j = 0; j < WTQ1List.Count; j++)
                    {
                        if (Convert.ToInt32(tblOTCompDetDatos.Tables[1].Rows[i]["IdOTArticulo"]) == WTQ1List[j].NroLineaOT)
                        {
                            row["CantEnv"] = WTQ1List[j].CantidadTransferida;
                            row["NroSolicitudTransferencia"] = WTQ1List[j].NroSolicitudTransferencia;
                            row["CostoArticulo"] = WTQ1List[j].CostoUnitario;
                            break;
                        }
                    }
                }

                row["CantUti"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["CantUti"]);
                //row["CostoArticulo"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["CostoArticulo"]);
                row["Observacion"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["Observacion"]);

                if (Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["IdTipoArticulo"]) == "2")
                {
                    row["Tipo"] = "Repuesto";
                }
                else if (Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["IdTipoArticulo"]) == "3")
                {
                    row["Tipo"] = "Consumible";
                }
                else
                {
                    row["Tipo"] = "Componente";
                }
                row["CodResponsable"] = Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["CodResponsable"]); //Visualizar el responsable
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("R", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    for (int j = 0; j < OHEMlist.Count; j++)
                    {
                        if (tblOTCompDetDatos.Tables[1].Rows[i]["CodResponsable"].ToString() != "" && tblOTCompDetDatos.Tables[1].Rows[i]["CodResponsable"] != null)
                        {
                            if (Convert.ToString(OHEMlist[j].CodigoPersona) == Convert.ToString(tblOTCompDetDatos.Tables[1].Rows[i]["CodResponsable"]))
                            {
                                row["Responsable"] = OHEMlist[j].NombrePersona;
                                break;
                            }
                        }
                        else { break; }
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }


                tblArticuloTarea.Rows.Add(row);
            }

            for (int i = 0; i < tblOTCompDetDatos.Tables[2].Rows.Count; i++)
            {
                DataRow row = tbOTTareaTrabajador.NewRow();
                row["IdOTTareaDetalle"] = Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["IdOTTareaDetalle"]);
                row["IdOTTarea"] = Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["IdOTTarea"]);
                row["CodResponsable"] = Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["CodResponsable"]);
                //row["Trabajador"] = Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["IdOTComp"]);
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("R", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    for (int j = 0; j < OHEMlist.Count; j++)
                    {
                        if (Convert.ToString(OHEMlist[j].CodigoPersona) == Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["CodResponsable"]))
                        {
                            row["Trabajador"] = OHEMlist[j].NombrePersona;
                        }
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }
                row["IdOTTareaDetalle"] = Convert.ToInt32(tblOTCompDetDatos.Tables[2].Rows[i]["IdOTTareaDetalle"]);
                row["CostoHoraHombre"] = Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["CostoHoraHombre"]);
                row["Fecha"] = Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["Fecha"]);
                row["HoraInicial"] = Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["HoraInicial"]);
                row["HoraFinal"] = Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["HoraFinal"]);
                row["HoraReal"] = Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["HorasReal"]);
                row["FlagActivo"] = Convert.ToString(tblOTCompDetDatos.Tables[2].Rows[i]["FlagActivo"]);
                row["Nuevo"] = false;
                tbOTTareaTrabajador.Rows.Add(row);
            }


            tabItem4.IsEnabled = true;

            trlComponente.ItemsSource = tblOTCompTreeList.DefaultView;
        }
        private void BloquearControlRegistroTarea()
        {
            dtpFechaCierreT.IsEnabled = false;
            //Horas ??
            buttonEdit17.IsEnabled = false;
            buttonEdit18.IsEnabled = false;
            btnRegistrarOTTarea.IsEnabled = false;
            ActualizarEstadoOTTarea.IsEnabled = false;
            txtComentarioTarea.IsEnabled = false;
            cboTrabajador.IsEnabled = false;
            dtpFechaTarea.IsEnabled = false;
            txthoraini.IsEnabled = false;
            txthorafin.IsEnabled = false;
            btnAgregarTrabajador.IsEnabled = false;
        }
        private void DesBloquearControlRegistroTarea()
        {
            dtpFechaCierreT.IsEnabled = true;
            buttonEdit17.IsEnabled = true;
            buttonEdit18.IsEnabled = true;
            btnRegistrarOTTarea.IsEnabled = true;
            ActualizarEstadoOTTarea.IsEnabled = true;
            txtComentarioTarea.IsEnabled = true;
            cboTrabajador.IsEnabled = true;
            dtpFechaTarea.IsEnabled = true;
            txthoraini.IsEnabled = true;
            txthorafin.IsEnabled = true;
            btnAgregarTrabajador.IsEnabled = true;
        }

        private void btnAgregarAct_Click(object sender, RoutedEventArgs e)
        {
            //Aqui enviar a las tablas los seleccionados.
            TreeViewModelCompOT trm = (TreeViewModelCompOT)trvComp.SelectedItem;

            foreach (DataRow drActTemp in tblActividadestmp.Select("IsChecked = true"))
            {
                DataRow row = tblActividades.NewRow();
                if (Convert.ToInt32(drActTemp["IdOTCompActividad"]) != 0)
                {
                    row["IdOTCompActividad"] = Convert.ToInt32(drActTemp["IdOTCompActividad"]);//IdOTCompActividad;
                }
                else
                {
                    row["IdOTCompActividad"] = 0;
                    //if (chkSinUnidadControl.IsChecked == false)
                    //{
                    //    row["IdOTCompActividad"] = Convert.ToInt32(trm.IdOTComp);
                    //}
                    //else
                    //{
                    //    row["IdOTCompActividad"] = Convert.ToInt32(trm.IdOTComp);
                    //}
                }

                if (chkSinUnidadControl.IsChecked == false)
                {
                    row["IdOTComp"] = trm.IdOTComp;
                }
                else
                {
                    row["IdOTComp"] = Convert.ToInt32(trm.IdOTComp);
                }
                row["IdPerfilCompActividad"] = Convert.ToInt32(drActTemp["IdPerfilCompActividad"]);
                row["IdActividad"] = Convert.ToInt32(drActTemp["IdActividad"]);
                row["Actividad"] = drActTemp["Actividad"].ToString();
                if (Convert.ToInt32(CboOrden.EditValue) == 2)
                {
                    row["IsChecked"] = true;
                }
                else
                {
                    row["IsChecked"] = false;
                }
                row["FlagUso"] = Convert.ToInt32(drActTemp["FlagUso"]);
                row["FlagActivo"] = true;
                row["Nuevo"] = true;
                if (Convert.ToInt32(drActTemp["IdPerfilComp"]) != 0)
                {
                    row["IdPerfilComp"] = Convert.ToInt32(drActTemp["IdPerfilComp"]);
                }
                else
                {
                    if (chkSinUnidadControl.IsChecked == false)
                    {
                        row["IdPerfilComp"] = Convert.ToInt32(trm.IdMenu);
                    }
                    else
                    {
                        row["IdPerfilComp"] = 1;
                    }
                }
                tblActividades.Rows.Add(row);

                foreach (DataRow drTareasTmp in tblTareastmp.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActTemp["IdPerfilCompActividad"])))
                {
                    DataRow row1 = tblTareas.NewRow();
                    row1["IdOTTarea"] = 0;
                    row1["IdOTCompActividad"] = 0;
                    row1["IdPerfilCompActividad"] = Convert.ToDouble(drTareasTmp["IdPerfilCompActividad"]);
                    row1["IdTarea"] = Convert.ToDouble(drTareasTmp["IdTarea"]);
                    row1["CodResponsable"] = Convert.ToDouble(drTareasTmp["IdTarea"]);
                    row1["CostoHoraHombre"] = Convert.ToDouble(drTareasTmp["CostoHoraHombre"]);
                    row1["HorasEstimada"] = Convert.ToDouble(drTareasTmp["HorasEstimada"]);
                    row1["HorasReal"] = 0;
                    row1["OTTarea"] = drTareasTmp["OTTarea"].ToString();
                    row1["IdPerfilTarea"] = 0;
                    row1["IdEstadoOTT"] = 1;
                    row1["FlagAutomatico"] = true;
                    row1["FlagActivo"] = true;
                    row1["Nuevo"] = true;
                    tblTareas.Rows.Add(row1);
                }

                foreach (DataRow drHerraEsptmp in tblHerrEsptmp.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActTemp["IdPerfilCompActividad"])))
                {
                    DataRow row1 = tblHerrEsp.NewRow();
                    row1["IdOTHerramienta"] = 0;
                    row1["IdOTCompActividad"] = 0;
                    row1["IdPerfilCompActividad"] = Convert.ToInt32(drHerraEsptmp["IdPerfilCompActividad"]);
                    row1["IdHerramienta"] = Convert.ToInt32(drHerraEsptmp["IdHerramienta"]);
                    row1["Herramienta"] = Convert.ToString(drHerraEsptmp["Herramienta"]);
                    row1["Cantidad"] = Convert.ToInt32(drHerraEsptmp["Cantidad"]);
                    row1["FlagAutomatico"] = true;
                    row1["FlagActivo"] = true;
                    row1["Nuevo"] = true;
                    tblHerrEsp.Rows.Add(row1);
                }

                foreach (DataRow drConsuTmp in tblConsumibletmp.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActTemp["IdPerfilCompActividad"])))
                {
                    DataRow row1 = tblConsumible.NewRow();
                    row1["IdOTArticulo"] = 0;
                    row1["IdOTCompActividad"] = 0;
                    row1["IdPerfilCompActividad"] = Convert.ToInt32(drConsuTmp["IdPerfilCompActividad"]);
                    row1["IdArticulo"] = Convert.ToString(drConsuTmp["IdArticulo"]);
                    row1["Articulo"] = Convert.ToString(drConsuTmp["Articulo"]);
                    row1["CantSol"] = Convert.ToInt32(drConsuTmp["CantSol"]);
                    row1["IdTipoArticulo"] = Convert.ToInt32(drConsuTmp["IdTipoArticulo"]);
                    row1["FlagAutomatico"] = true;
                    row1["FlagActivo"] = true;
                    row1["Nuevo"] = true;
                    tblConsumible.Rows.Add(row1);
                }

                foreach (DataRow drRepTmp in tblRepuestotmp.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActTemp["IdPerfilCompActividad"])))
                {
                    DataRow row1 = tblRepuesto.NewRow();
                    row1["IdOTArticulo"] = 0;
                    row1["IdOTCompActividad"] = 0;
                    row1["IdPerfilCompActividad"] = Convert.ToInt32(drRepTmp["IdPerfilCompActividad"]);
                    row1["IdArticulo"] = Convert.ToString(drRepTmp["IdArticulo"]);
                    row1["Articulo"] = Convert.ToString(drRepTmp["Articulo"]);
                    row1["CantSol"] = Convert.ToInt32(drRepTmp["CantSol"]);
                    row1["FlagAutomatico"] = true;
                    row1["FlagActivo"] = true;
                    row1["Nuevo"] = true;
                    tblRepuesto.Rows.Add(row1);
                }
            }
            //for (int i = 0; i < tblActividadestmp.Rows.Count; i++)
            //{
            //    if (Convert.ToBoolean(tblActividadestmp.Rows[i]["IsChecked"]) == true)
            //    {
            //        DataRow row = tblActividades.NewRow();
            //        if (Convert.ToInt32(tblActividadestmp.Rows[i]["IdOTCompActividad"]) != 0)
            //        {
            //            row["IdOTCompActividad"] = Convert.ToInt32(tblActividadestmp.Rows[i]["IdOTCompActividad"]);//IdOTCompActividad;
            //        }
            //        else
            //        {
            //            row["IdOTCompActividad"] = 0;
            //            //if (chkSinUnidadControl.IsChecked == false)
            //            //{
            //            //    row["IdOTCompActividad"] = Convert.ToInt32(trm.IdOTComp);
            //            //}
            //            //else
            //            //{
            //            //    row["IdOTCompActividad"] = Convert.ToInt32(trm.IdOTComp);
            //            //}
            //        }

            //        if (chkSinUnidadControl.IsChecked == false)
            //        {
            //            row["IdOTComp"] = trm.IdOTComp;
            //        }
            //        else
            //        {
            //            row["IdOTComp"] = Convert.ToInt32(trm.IdOTComp);
            //        }
            //        row["IdPerfilCompActividad"] = Convert.ToInt32(tblActividadestmp.Rows[i]["IdPerfilCompActividad"]);
            //        row["IdActividad"] = Convert.ToInt32(tblActividadestmp.Rows[i]["IdActividad"]);
            //        row["Actividad"] = tblActividadestmp.Rows[i]["Actividad"].ToString();
            //        if (Convert.ToInt32(CboOrden.EditValue) == 2)
            //        {
            //            row["IsChecked"] = true;
            //        }
            //        else
            //        {
            //            row["IsChecked"] = false;
            //        }
            //        row["FlagUso"] = Convert.ToInt32(tblActividadestmp.Rows[i]["FlagUso"]);
            //        row["FlagActivo"] = true;
            //        row["Nuevo"] = true;
            //        if (Convert.ToInt32(tblActividadestmp.Rows[i]["IdPerfilComp"]) != 0)
            //        {
            //            row["IdPerfilComp"] = Convert.ToInt32(tblActividadestmp.Rows[i]["IdPerfilComp"]);
            //        }
            //        else
            //        {
            //            if (chkSinUnidadControl.IsChecked == false)
            //            {
            //                row["IdPerfilComp"] = Convert.ToInt32(trm.IdMenu);
            //            }
            //            else
            //            {
            //                row["IdPerfilComp"] = 1;
            //            }
            //        }
            //        tblActividades.Rows.Add(row);

            //        for (int j = 0; j < tblTareastmp.Rows.Count; j++)
            //        {
            //            if (Convert.ToInt32(tblActividadestmp.Rows[i]["IdPerfilCompActividad"]) == Convert.ToInt32(tblTareastmp.Rows[j]["IdPerfilCompActividad"]))
            //            {
            //                DataRow row1 = tblTareas.NewRow();
            //                row1["IdOTTarea"] = 0;
            //                row1["IdOTCompActividad"] = 0;
            //                row1["IdPerfilCompActividad"] = Convert.ToDouble(tblTareastmp.Rows[j]["IdPerfilCompActividad"]);
            //                row1["IdTarea"] = Convert.ToDouble(tblTareastmp.Rows[j]["IdTarea"]);
            //                row1["CodResponsable"] = Convert.ToDouble(tblTareastmp.Rows[j]["IdTarea"]);
            //                row1["CostoHoraHombre"] = Convert.ToDouble(tblTareastmp.Rows[j]["CostoHoraHombre"]);
            //                row1["HorasEstimada"] = Convert.ToDouble(tblTareastmp.Rows[j]["HorasEstimada"]);
            //                row1["HorasReal"] = 0;
            //                row1["OTTarea"] = tblTareastmp.Rows[j]["OTTarea"].ToString();
            //                row1["IdPerfilTarea"] = 0;
            //                row1["IdEstadoOTT"] = 1;
            //                row1["FlagAutomatico"] = true;
            //                row1["FlagActivo"] = true;
            //                row1["Nuevo"] = true;
            //                tblTareas.Rows.Add(row1);
            //            }
            //        }
            //        for (int j = 0; j < tblHerrEsptmp.Rows.Count; j++)
            //        {
            //            if (Convert.ToInt32(tblActividadestmp.Rows[i]["IdPerfilCompActividad"]) == Convert.ToInt32(tblHerrEsptmp.Rows[j]["IdPerfilCompActividad"]))
            //            {
            //                DataRow row1 = tblHerrEsp.NewRow();
            //                row1["IdOTHerramienta"] = 0;
            //                row1["IdOTCompActividad"] = 0;
            //                row1["IdPerfilCompActividad"] = Convert.ToInt32(tblHerrEsptmp.Rows[j]["IdPerfilCompActividad"]);
            //                row1["IdHerramienta"] = Convert.ToInt32(tblHerrEsptmp.Rows[j]["IdHerramienta"]);
            //                row1["Herramienta"] = Convert.ToString(tblHerrEsptmp.Rows[j]["Herramienta"]);
            //                row1["Cantidad"] = Convert.ToInt32(tblHerrEsptmp.Rows[j]["Cantidad"]);
            //                row1["FlagAutomatico"] = true;
            //                row1["FlagActivo"] = true;
            //                row1["Nuevo"] = true;
            //                tblHerrEsp.Rows.Add(row1);
            //            }
            //        }
            //        for (int j = 0; j < tblConsumibletmp.Rows.Count; j++)
            //        {
            //            if (Convert.ToInt32(tblActividadestmp.Rows[i]["IdPerfilCompActividad"]) == Convert.ToInt32(tblConsumibletmp.Rows[j]["IdPerfilCompActividad"]))
            //            {
            //                DataRow row1 = tblConsumible.NewRow();
            //                row1["IdOTArticulo"] = 0;
            //                row1["IdOTCompActividad"] = 0;
            //                row1["IdPerfilCompActividad"] = Convert.ToInt32(tblConsumibletmp.Rows[j]["IdPerfilCompActividad"]);
            //                row1["IdArticulo"] = Convert.ToString(tblConsumibletmp.Rows[j]["IdArticulo"]);
            //                row1["Articulo"] = Convert.ToString(tblConsumibletmp.Rows[j]["Articulo"]);
            //                row1["CantSol"] = Convert.ToInt32(tblConsumibletmp.Rows[j]["CantSol"]);
            //                row1["IdTipoArticulo"] = Convert.ToInt32(tblConsumibletmp.Rows[j]["IdTipoArticulo"]);
            //                row1["FlagAutomatico"] = true;
            //                row1["FlagActivo"] = true;
            //                row1["Nuevo"] = true;
            //                tblConsumible.Rows.Add(row1);
            //            }
            //        }

            //        for (int j = 0; j < tblRepuestotmp.Rows.Count; j++)
            //        {
            //            if (Convert.ToInt32(tblActividadestmp.Rows[i]["IdPerfilCompActividad"]) == Convert.ToInt32(tblRepuestotmp.Rows[j]["IdPerfilCompActividad"]))
            //            {
            //                DataRow row1 = tblRepuesto.NewRow();
            //                row1["IdOTArticulo"] = 0;
            //                row1["IdOTCompActividad"] = 0;
            //                row1["IdPerfilCompActividad"] = Convert.ToInt32(tblRepuestotmp.Rows[j]["IdPerfilCompActividad"]);
            //                row1["IdArticulo"] = Convert.ToString(tblRepuestotmp.Rows[j]["IdArticulo"]);
            //                row1["Articulo"] = Convert.ToString(tblRepuestotmp.Rows[j]["Articulo"]);
            //                row1["CantSol"] = Convert.ToInt32(tblRepuestotmp.Rows[j]["CantSol"]);
            //                row1["FlagAutomatico"] = true;
            //                row1["FlagActivo"] = true;
            //                row1["Nuevo"] = true;
            //                tblRepuesto.Rows.Add(row1);
            //            }
            //        }
            //    }
            //}

            stckAgregarActividad.Visibility = System.Windows.Visibility.Hidden;

            //Mostrar el seleccionado
            try
            {
                int idPerfilComp;
                int idotComp;
                if (chkSinUnidadControl.IsChecked == false)
                {
                    idPerfilComp = Convert.ToInt32(trm.IdMenu);
                    idotComp = Convert.ToInt32(trm.IdOTComp);
                }
                else
                {
                    idPerfilComp = Convert.ToInt32(trm.IdMenu);
                    idotComp = Convert.ToInt32(trm.IdOTComp);
                }

                if (IdOT == 0)
                {
                    int CantExisteDatos = tblActividades.Select("IdPerfilComp = " + idPerfilComp.ToString()).Length;
                    if (CantExisteDatos != 0)
                    {
                        DataView dtvActividades = tblActividades.DefaultView;
                        dtvActividades.RowFilter = "IdPerfilComp = " + idPerfilComp.ToString();
                        lstboxActividad.ItemsSource = dtvActividades;
                    }
                    else
                    {
                        CboPerfilAC.SelectedIndexChanged -= new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
                        CboPerfilAC.SelectedIndex = -1;
                        CboPerfilAC.SelectedIndexChanged += new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
                        lstboxActividad.ItemsSource = null;
                    }
                }
                else
                {
                    int CantExisteDatos = tblActividades.Select("IdOTComp = " + idotComp.ToString()).Length;
                    if (CantExisteDatos != 0)
                    {
                        DataView dtvActividades = tblActividades.DefaultView;
                        dtvActividades.RowFilter = "IdOTComp = " + idotComp.ToString();
                        lstboxActividad.ItemsSource = dtvActividades;
                    }
                    else
                    {
                        CboPerfilAC.SelectedIndexChanged -= new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
                        CboPerfilAC.SelectedIndex = -1;
                        CboPerfilAC.SelectedIndexChanged += new RoutedEventHandler(CboPerfilAC_SelectedIndexChanged);
                        lstboxActividad.ItemsSource = null;
                    }
                }
                grvTarea.ItemsSource = false;
                grvConsumible.ItemsSource = false;
                grvHerrpEsp.ItemsSource = false;
                grvRepuesto.ItemsSource = false;

                //Limpiar Seleccionado
                for (int i = 0; i < tblActividadestmp.Rows.Count; i++)
                {
                    tblActividadestmp.Rows[i]["IsChecked"] = false;
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }

        private void btnCancelarAct_Click(object sender, RoutedEventArgs e)
        {
            stckAgregarActividad.Visibility = System.Windows.Visibility.Hidden;
        }



        private void CboOrden_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivaDetalleOT();
                //Modificar tipo de act segun orden
                for (int i = 0; i < tblActividades.Rows.Count; i++)
                {
                    if (Convert.ToInt32(CboOrden.EditValue) == 1)
                    {
                        tblActividades.Rows[i]["IsChecked"] = false;
                    }
                    else if (Convert.ToInt32(CboOrden.EditValue) == 2)
                    {
                        tblActividades.Rows[i]["IsChecked"] = true;
                    }
                }
            }
            catch
            { }
        }

        private void ActivaDetalleOT()
        {
            if (CboOrden.SelectedIndex != -1 && dtpFechaProgram.EditValue != null
                && CboResponsable.SelectedIndex != -1 && (chkSinUnidadControl.IsChecked == true || CboUnidadControl.SelectedIndex != -1))
            {
                tabItem4.IsEnabled = true;
            }
            else
            {
                tabItem4.IsEnabled = false;
            }
        }

        private void lstboxActividad_KeyUp(object sender, KeyEventArgs e)
        {
            //Eliminar Actividades de la lista
            if (e.Key == Key.Delete)
            {
                int IdActividad = Convert.ToInt32((((sender as ListBox).SelectedItems[0]) as DataRowView)["IdActividad"]);
                int idPerfilComp = Convert.ToInt32((((sender as ListBox).SelectedItems[0]) as DataRowView)["IdPerfilComp"]);
                string Actividad = Convert.ToString((((sender as ListBox).SelectedItems[0]) as DataRowView)["Actividad"]);

                var rpt = DevExpress.Xpf.Core.DXMessageBox.Show(string.Format("Seguro de eliminar la actividad: {0} ?", Actividad), "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rpt == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < tblActividades.Rows.Count; i++)
                    {

                        if (Convert.ToInt32(tblActividades.Rows[i]["IdActividad"]) == IdActividad && Convert.ToInt32(tblActividades.Rows[i]["IdPerfilComp"]) == idPerfilComp)
                        {
                            if (Convert.ToInt32(tblActividades.Rows[i]["IdOTComp"]) == 0 || Convert.ToBoolean(tblActividades.Rows[i]["Nuevo"]))
                            {
                                int IdPerfilCompActividad = Convert.ToInt32((((sender as ListBox).SelectedItems[0]) as DataRowView)["IdPerfilCompActividad"]);
                                tblActividades.Rows.RemoveAt(i);


                                //Eliminar hijos
                                for (int j = 0; j < tblTareas.Rows.Count; j++)
                                {
                                    if (Convert.ToInt32(tblTareas.Rows[j]["IdPerfilCompActividad"]) == IdPerfilCompActividad)
                                    {
                                        tblTareas.Rows.RemoveAt(j);
                                    }
                                }

                                for (int j = 0; j < tblHerrEsp.Rows.Count; j++)
                                {
                                    if (Convert.ToInt32(tblHerrEsp.Rows[j]["IdPerfilCompActividad"]) == IdPerfilCompActividad)
                                    {
                                        tblHerrEsp.Rows.RemoveAt(j);
                                    }
                                }

                                for (int j = 0; j < tblConsumible.Rows.Count; j++)
                                {
                                    if (Convert.ToInt32(tblConsumible.Rows[j]["IdPerfilCompActividad"]) == IdPerfilCompActividad)
                                    {
                                        tblConsumible.Rows.RemoveAt(j);
                                    }
                                }

                                for (int j = 0; j < tblRepuesto.Rows.Count; j++)
                                {
                                    if (Convert.ToInt32(tblRepuesto.Rows[j]["IdPerfilCompActividad"]) == IdPerfilCompActividad)
                                    {
                                        tblRepuesto.Rows.RemoveAt(j);
                                    }
                                }

                                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "MENS_DELE_ACTI"), 1);
                                LblSelected.Content = "Seleccione actividad...";
                            }
                            else
                            {
                                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_DELE_ACTI"), 2);
                            }
                            break;
                        }

                    }

                }
            }
        }

        private void btnEliminarTarea_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dr = (grvTarea.SelectedItem) as DataRowView;
            if (Convert.ToBoolean(dr.Row["FlagAutomatico"]) == false)
            {
                for (int i = 0; i < tblTareas.Rows.Count; i++)
                {
                    if (IdOT == 0)
                    {
                        if (Convert.ToInt32(dr.Row["IdOTCompActividad"]) == Convert.ToInt32(tblTareas.Rows[i]["IdOTCompActividad"]) && Convert.ToInt32(dr.Row["IdTarea"]) == Convert.ToInt32(tblTareas.Rows[i]["IdTarea"]))
                        {
                            tblTareas.Rows.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dr.Row["IdPerfilCompActividad"]) == Convert.ToInt32(tblTareas.Rows[i]["IdPerfilCompActividad"]) && Convert.ToInt32(dr.Row["IdTarea"]) == Convert.ToInt32(tblTareas.Rows[i]["IdTarea"]))
                        {
                            tblTareas.Rows.RemoveAt(i);
                        }
                    }
                }
            }
            else
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_DELE_TARE_AUTO"), 2);
            }
        }

        private void btnEliminarHerrEsp_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dr = (grvHerrpEsp.SelectedItem) as DataRowView;
            if (Convert.ToBoolean(dr.Row["FlagAutomatico"]) == false)
            {
                for (int i = 0; i < tblHerrEsp.Rows.Count; i++)
                {
                    if (IdOT == 0)
                    {
                        if (Convert.ToInt32(dr.Row["IdOTCompActividad"]) == Convert.ToInt32(tblHerrEsp.Rows[i]["IdOTCompActividad"]) && Convert.ToInt32(dr.Row["IdHerramienta"]) == Convert.ToInt32(tblHerrEsp.Rows[i]["IdHerramienta"]))
                        {
                            tblHerrEsp.Rows.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dr.Row["IdPerfilCompActividad"]) == Convert.ToInt32(tblHerrEsp.Rows[i]["IdPerfilCompActividad"]) && Convert.ToInt32(dr.Row["IdHerramienta"]) == Convert.ToInt32(tblHerrEsp.Rows[i]["IdHerramienta"]))
                        {
                            tblHerrEsp.Rows.RemoveAt(i);
                        }
                    }
                }
            }
            else
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_DELE_HERR_AUTO"), 2);
            }
        }

        private void btnEliminarRep_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dr = (grvRepuesto.SelectedItem) as DataRowView;
            if (Convert.ToBoolean(dr.Row["FlagAutomatico"]) == false)
            {
                for (int i = 0; i < tblRepuesto.Rows.Count; i++)
                {
                    if (IdOT == 0)
                    {
                        if (Convert.ToInt32(dr.Row["IdOTCompActividad"]) == Convert.ToInt32(tblRepuesto.Rows[i]["IdOTCompActividad"]) && Convert.ToString(dr.Row["IdArticulo"]) == Convert.ToString(tblRepuesto.Rows[i]["IdArticulo"]))
                        {
                            tblRepuesto.Rows.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dr.Row["IdPerfilCompActividad"]) == Convert.ToInt32(tblHerrEsp.Rows[i]["IdPerfilCompActividad"]) && Convert.ToString(dr.Row["IdArticulo"]) == Convert.ToString(tblRepuesto.Rows[i]["IdArticulo"]))
                        {
                            tblRepuesto.Rows.RemoveAt(i);
                        }
                    }
                }
            }
            else
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_DELE_REPU_AUTO"), 2);
            }
        }

        private void btnEliminarCon_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dr = (grvConsumible.SelectedItem) as DataRowView;
            if (Convert.ToBoolean(dr.Row["FlagAutomatico"]) == false)
            {
                for (int i = 0; i < tblConsumible.Rows.Count; i++)
                {
                    if (IdOT == 0)
                    {
                        if (Convert.ToInt32(dr.Row["IdOTCompActividad"]) == Convert.ToInt32(tblConsumible.Rows[i]["IdOTCompActividad"]) && Convert.ToString(dr.Row["IdArticulo"]) == Convert.ToString(tblConsumible.Rows[i]["IdArticulo"]))
                        {
                            tblConsumible.Rows.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(dr.Row["IdPerfilCompActividad"]) == Convert.ToInt32(tblConsumible.Rows[i]["IdPerfilCompActividad"]) && Convert.ToString(dr.Row["IdArticulo"]) == Convert.ToString(tblConsumible.Rows[i]["IdArticulo"]))
                        {
                            tblConsumible.Rows.RemoveAt(i);
                        }
                    }
                }
            }
            else
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_DELE_CONS_AUTO"), 2);
            }
        }

        private void rbtListarOT_Checked(object sender, RoutedEventArgs e)
        {
            ListarOT();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e) //Marcado de todo
        {
            try
            {
                DataView tblDtgActividades = (DataView)grvActividades.ItemsSource;

                for (int i = 0; i < tblDtgActividades.Count; i++)
                {

                }
            }
            catch
            {

            }
        }

        private void cboHerrEsp_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                objE_Herramienta = new E_Herramienta();
                objE_Herramienta.IdHerramienta = Convert.ToInt32(cboHerrEsp.EditValue);
                objE_Herramienta.Cantidad = 0;
                //Valida cantidad maxima
                txtCantHerrEsp.Value = objHerramienta.Herramienta_GetCantItems(objE_Herramienta);
                txtCantHerrEsp.MaxValue = objHerramienta.Herramienta_GetCantItems(objE_Herramienta);
            }
            catch
            {
            }
        }

        private void dtgOT_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            try
            {

                int IdTipoOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdTipoOT"));

                if (IdTipoOT != 1 && IdTipoOT != 3)
                {
                    btnRegistrarOTTarea.IsEnabled = true;
                }
                else
                {
                    btnRegistrarOTTarea.IsEnabled = false;
                }

                if (IdTipoOT != 2 && IdTipoOT != 3)
                {
                    BtnRegistrarInforme.IsEnabled = true;
                }
                else
                {
                    BtnRegistrarInforme.IsEnabled = true;
                }

            }
            catch
            {
            }
        }

        private void ListarActividadesPendiente()
        {
            try
            {
                objE_OT = new E_OT();
                objE_OT.CodUC = Convert.ToString(CboUnidadControl.EditValue);
                DataSet tblActividadDatos = objB_OT.OTCompActividadEstado_Listar(objE_OT);
                for (int i = 0; i < tblActividadDatos.Tables[0].Rows.Count; i++)
                {
                    DataRow row = tblActividadestmp.NewRow();
                    row["IdOTCompActividad"] = Convert.ToInt32(tblActividadDatos.Tables[0].Rows[i]["IdOTCompActividad"]);
                    row["IdOTComp"] = Convert.ToInt32(tblActividadDatos.Tables[0].Rows[i]["IdPerfilComp"]);
                    row["IdPerfilCompActividad"] = IdPerfilCompMax;
                    row["IdActividad"] = Convert.ToInt32(tblActividadDatos.Tables[0].Rows[i]["IdActividad"]);
                    //Dar prioridad a los pendientes
                    for (int j = 0; j < tblActividadestmp.Rows.Count; j++)
                    {
                        if (Convert.ToInt32(tblActividadDatos.Tables[0].Rows[i]["IdActividad"]) == Convert.ToInt32(tblActividadestmp.Rows[j]["IdActividad"]) && (Convert.ToInt32(tblActividadDatos.Tables[0].Rows[i]["IdOTComp"]) == Convert.ToInt32(tblActividadestmp.Rows[j]["IdOTComp"]) || Convert.ToInt32(tblActividadestmp.Rows[j]["IdOTComp"]) == 0))
                        {
                            tblTareastmp.DefaultView.RowFilter = "IdPerfilCompActividad <> " + Convert.ToInt32(tblActividadestmp.Rows[j]["IdPerfilCompActividad"]);
                            tblTareastmp = tblTareastmp.DefaultView.ToTable();
                            tblHerrEsptmp.DefaultView.RowFilter = "IdPerfilCompActividad <> " + Convert.ToInt32(tblActividadestmp.Rows[j]["IdPerfilCompActividad"]);
                            tblHerrEsptmp = tblHerrEsptmp.DefaultView.ToTable();
                            tblConsumibletmp.DefaultView.RowFilter = "IdPerfilCompActividad <> " + Convert.ToInt32(tblActividadestmp.Rows[j]["IdPerfilCompActividad"]);
                            tblConsumibletmp = tblConsumibletmp.DefaultView.ToTable();
                            tblRepuestotmp.DefaultView.RowFilter = "IdPerfilCompActividad <> " + Convert.ToInt32(tblActividadestmp.Rows[j]["IdPerfilCompActividad"]);
                            tblRepuestotmp = tblRepuestotmp.DefaultView.ToTable();

                            tblActividadestmp.Rows.RemoveAt(j);
                            break;
                        }
                    }
                    row["Actividad"] = tblActividadDatos.Tables[0].Rows[i]["Actividad"].ToString();
                    row["IsChecked"] = false;
                    row["FlagUso"] = Convert.ToInt32(tblActividadDatos.Tables[0].Rows[i]["FlagUso"]);// Convert.ToBoolean(tblActividadDatos.Rows[i]["FlagExterna"]);
                    row["FlagActivo"] = true;
                    row["Nuevo"] = false;
                    row["IdPerfilComp"] = Convert.ToInt32(tblActividadDatos.Tables[0].Rows[i]["IdPerfilComp"]);
                    row["PerfilComp"] = "";
                    row["FlagPendiente"] = true;
                    tblActividadestmp.Rows.Add(row);
                    IdPerfilCompMax++;
                }

                for (int i = 0; i < tblActividadDatos.Tables[1].Rows.Count; i++)
                {
                    DataRow row = tblTareastmp.NewRow();
                    row["IdOTTarea"] = 0;// Convert.ToDouble(tblActividadDatos.Tables[1].Rows[i]["IdOTTarea"]);
                    row["IdOTCompActividad"] = Convert.ToDouble(tblActividadDatos.Tables[1].Rows[i]["IdOTCompActividad"]);
                    //Aqui obtener el valor de la tabla anterior
                    row["IdPerfilCompActividad"] = tblActividadestmp.Select("IdOTCompActividad = " + Convert.ToInt32(tblActividadDatos.Tables[1].Rows[i]["IdOTCompActividad"]), "")[0]["IdPerfilCompActividad"].ToString();
                    // Convert.ToDouble(tblTareaDatos.Rows[i]["IdPerfilCompActividad"]);
                    row["IdTarea"] = Convert.ToDouble(tblActividadDatos.Tables[1].Rows[i]["IdTarea"]);
                    row["CodResponsable"] = CboResponsable.EditValue;
                    row["CostoHoraHombre"] = 0;
                    row["HorasEstimada"] = Convert.ToDouble(tblActividadDatos.Tables[1].Rows[i]["HorasHombre"]);
                    row["HorasReal"] = 0;
                    row["OTTarea"] = tblActividadDatos.Tables[1].Rows[i]["Tarea"].ToString();
                    row["IdPerfilTarea"] = 0;
                    row["IdEstadoOTT"] = 1;
                    row["FlagActivo"] = true;
                    row["Nuevo"] = false;
                    tblTareastmp.Rows.Add(row);
                }

                //Hrr
                for (int i = 0; i < tblActividadDatos.Tables[2].Rows.Count; i++)
                {
                    DataRow row = tblHerrEsptmp.NewRow();
                    row["IdOTHerramienta"] = 0;
                    row["IdOTCompActividad"] = Convert.ToDouble(tblActividadDatos.Tables[2].Rows[i]["IdOTCompActividad"]);
                    row["IdPerfilCompActividad"] = tblActividadestmp.Select("IdOTCompActividad = " + Convert.ToDouble(tblActividadDatos.Tables[2].Rows[i]["IdOTCompActividad"]), "")[0]["IdPerfilCompActividad"].ToString();
                    row["IdHerramienta"] = Convert.ToInt32(tblActividadDatos.Tables[2].Rows[i]["IdHerramienta"]);
                    row["Herramienta"] = Convert.ToString(tblActividadDatos.Tables[2].Rows[i]["Herramienta"]);
                    row["Cantidad"] = Convert.ToInt32(tblActividadDatos.Tables[2].Rows[i]["Cantidad"]);
                    row["FlagActivo"] = true;
                    row["Nuevo"] = false;
                    tblHerrEsptmp.Rows.Add(row);

                }

                //Art
                for (int i = 0; i < tblActividadDatos.Tables[3].Rows.Count; i++)
                {
                    if (Convert.ToDouble(tblActividadDatos.Tables[3].Rows[i]["IdTipoArticulo"]) == 2)
                    {
                        DataRow row = tblRepuestotmp.NewRow();
                        row["IdOTArticulo"] = 0;
                        row["IdOTCompActividad"] = Convert.ToDouble(tblActividadDatos.Tables[3].Rows[i]["IdOTCompActividad"]);
                        row["IdPerfilCompActividad"] = tblActividadestmp.Select("IdOTCompActividad = " + Convert.ToDouble(tblActividadDatos.Tables[3].Rows[i]["IdOTCompActividad"]), "")[0]["IdPerfilCompActividad"].ToString();
                        row["IdArticulo"] = Convert.ToString(tblActividadDatos.Tables[3].Rows[i]["IdArticulo"]);
                        row["Articulo"] = Convert.ToString(tblActividadDatos.Tables[3].Rows[i]["DescripcionSAP"]);
                        row["CantSol"] = Convert.ToDouble(tblActividadDatos.Tables[3].Rows[i]["CantSol"]);
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        tblRepuestotmp.Rows.Add(row);
                    }
                    else if (Convert.ToDouble(tblActividadDatos.Tables[3].Rows[i]["IdTipoArticulo"]) == 3)
                    {
                        DataRow row = tblConsumibletmp.NewRow();
                        row["IdOTArticulo"] = 0;
                        row["IdOTCompActividad"] = Convert.ToDouble(tblActividadDatos.Tables[3].Rows[i]["IdOTCompActividad"]);
                        row["IdPerfilCompActividad"] = tblActividadestmp.Select("IdOTCompActividad = " + Convert.ToDouble(tblActividadDatos.Tables[3].Rows[i]["IdOTCompActividad"]), "")[0]["IdPerfilCompActividad"].ToString();
                        row["IdArticulo"] = Convert.ToString(tblActividadDatos.Tables[3].Rows[i]["IdArticulo"]);
                        row["Articulo"] = Convert.ToString(tblActividadDatos.Tables[3].Rows[i]["DescripcionSAP"]);
                        row["CantSol"] = Convert.ToDouble(tblActividadDatos.Tables[3].Rows[i]["CantSol"]);
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        tblConsumibletmp.Rows.Add(row);
                    }
                }

            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }


        private void btnEliminarTrabajador_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int IdOTTarea = Convert.ToInt32(trlComponente.GetCellValue(trlViewComp.FocusedRowHandle, "IdReal"));
                int IdOTTareaDetalle = Convert.ToInt32(grvListarTrabajador.GetCellDisplayText(tableView1.FocusedRowHandle, "IdOTTareaDetalle"));
                for (int i = 0; i < tbOTTareaTrabajador.Rows.Count; i++)
                {
                    if (Convert.ToInt32(tbOTTareaTrabajador.Rows[i]["IdOTTareaDetalle"]) == IdOTTareaDetalle)
                    {
                        if (Convert.ToBoolean(tbOTTareaTrabajador.Rows[i]["Nuevo"]) == true)
                        {
                            tbOTTareaTrabajador.Rows.RemoveAt(i);
                        }
                        else
                        {
                            tbOTTareaTrabajador.Rows[i]["FlagActivo"] = false;
                        }
                    }
                }

                int Id = Convert.ToInt32(trlComponente.GetCellValue(trlViewComp.FocusedRowHandle, "IdReal"));
                tbOTTareaTrabajador.DefaultView.RowFilter = "FlagActivo = True and IdOTTarea = " + Id.ToString();
                grvListarTrabajador.ItemsSource = tbOTTareaTrabajador.DefaultView;

                int idpadre = 0;
                for (int i = 0; i < tblOTCompTreeList.Rows.Count; i++)
                {
                    if (Convert.ToInt32(tblOTCompTreeList.Rows[i]["IdReal"]) == IdOTTarea && Convert.ToInt32(tblOTCompTreeList.Rows[i]["IdTipo"]) == 3 && idpadre == 0)
                    {
                        if (tbOTTareaTrabajador.Compute("sum(HoraReal)", "FlagActivo = True and IdOTTarea = " + IdOTTarea.ToString()) == DBNull.Value)
                        {
                            tblOTCompTreeList.Rows[i]["HorasReales"] = 0;
                        }
                        else
                        {
                            tblOTCompTreeList.Rows[i]["HorasReales"] = Convert.ToDouble(tbOTTareaTrabajador.Compute("sum(HoraReal)", "FlagActivo = True and IdOTTarea = " + IdOTTarea.ToString()));
                        }
                        idpadre = Convert.ToInt32(tblOTCompTreeList.Rows[i]["IdPadre"]);
                        i = 0;
                    }
                    if (Convert.ToInt32(tblOTCompTreeList.Rows[i]["Id"]) == idpadre && Convert.ToInt32(tblOTCompTreeList.Rows[i]["IdTipo"]) == 2)
                    {
                        tblOTCompTreeList.Rows[i]["ActividadRealizada"] = true;
                    }
                }

                tblOTCompTreeList.DefaultView.Sort = "IdTipo desc";
                tblOTCompTreeList = tblOTCompTreeList.DefaultView.ToTable();
                for (int i = 0; i < tblOTCompTreeList.Rows.Count; i++)
                {
                    //if (Convert.ToInt32(tblOTCompTreeList.Rows[i]["idTipo"]) == 2)
                    //{
                    var x = tblOTCompTreeList.Compute("Sum(HorasEstimada)", "IdPadre = " + tblOTCompTreeList.Rows[i]["Id"].ToString());
                    if (x == DBNull.Value)
                    {
                    }
                    else
                    {
                        tblOTCompTreeList.Rows[i]["HorasEstimada"] = Convert.ToInt32(tblOTCompTreeList.Compute("Sum(HorasEstimada)", "IdPadre = " + tblOTCompTreeList.Rows[i]["Id"].ToString()));
                        tblOTCompTreeList.Rows[i]["HorasReales"] = Convert.ToInt32(tblOTCompTreeList.Compute("Sum(HorasReales)", "IdPadre = " + tblOTCompTreeList.Rows[i]["Id"].ToString()));
                    }
                    //}
                }

                //trlComponente.ItemsSource = tblOTCompTreeList.DefaultView;
                cboTrabajador.SelectedIndex = -1;
                dtpFechaTarea.EditValue = null;
                txthoraini.Clear();
                txthorafin.Clear();
                cboTrabajador.Focus();
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }

        private void GrabarSolPendienteSAP(int idot, string nroot)
        {
            objE_TablaMaestra.IdTabla = 42;
            DataTable tblAlmacen = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);

            objE_TablaMaestra.IdTabla = 43;
            DataTable tblTipoOperacion = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);

            OWTQ = new InterfazMTTO.iSBO_BE.BEOWTQ();
            OWTQ.NroOrdenTrabajo = nroot;// LblNumeroDocumento.Text;
            OWTQ.AlmacenEntrada = tblAlmacen.Rows[1]["Valor"].ToString(); //Alm Mantenimient
            OWTQ.AlmacenSalida = tblAlmacen.Rows[0]["Valor"].ToString();//General
            OWTQ.FechaSolicitud = Convert.ToDateTime(dtpFechaProgram.EditValue);

            WTQ1List = new InterfazMTTO.iSBO_BE.BEWTQ1List();
            int cant = 0;

            cant = 0;

            objE_OT = new E_OT();
            objE_OT.IdOT = idot;//IdOT;
            DataTable tblDatos = objB_OT.OTArticulo_ListSolSAP(objE_OT);

            for (int i = 0; i < tblDatos.Rows.Count; i++)
            {

                InterfazMTTO.iSBO_BE.BEWTQ1 BEWTQ1 = new InterfazMTTO.iSBO_BE.BEWTQ1();
                BEWTQ1.NroOrdenTrabajo = nroot; // LblNumeroDocumento.Text;
                BEWTQ1.NroLinea = Convert.ToInt32(tblDatos.Rows[i]["IdOTArticulo"]);//De la BD
                BEWTQ1.CodigoArticulo = Convert.ToString(tblDatos.Rows[i]["IdArticulo"]);
                BEWTQ1.CantidadSolicitada = Convert.ToInt32(tblDatos.Rows[i]["CantSol"]);
                BEWTQ1.TipoOperacion = tblTipoOperacion.Rows[0]["Valor"].ToString(); //Tabla Maestra  --> 12
                WTQ1List.Add(BEWTQ1);
                cant++;

            }

            IdTipoOrden = Convert.ToInt32(CboOrden.EditValue);
            if (WTQ1List.Count > 0 && Convert.ToInt32(IdTipoOrden) != 2) //Revisar 
            {
                //InterfazMTTO.iSBO_BE.BEWTQ1List WTQ1List = new InterfazMTTO.iSBO_BE.BEWTQ1List();
                WTQ1List = InterfazMTTO.iSBO_BL.SolicitudTransferencia_BL.RegistraSolicitudTransferencia(OWTQ, WTQ1List, ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    tblOTArticuloSol.Rows.Clear();
                    //Actualizar datos OTArticulo
                    for (int i = 0; i < WTQ1List.Count; i++)
                    {
                        DataRow dr = tblOTArticuloSol.NewRow();
                        dr["IdOTArticulo"] = WTQ1List[i].NroLineaOT;
                        dr["NroSolTransfer"] = WTQ1List[i].NroSolicitudTransferencia;
                        dr["NroLinSolTransfer"] = WTQ1List[i].NroLinea;
                        tblOTArticuloSol.Rows.Add(dr);
                    }

                    int x = objB_OT.OTArticulo_Update(1, tblOTArticuloSol);
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_SOLI_SAP"), 1);
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }
            }
        }

        private void BloquearForm()
        {
            BtnGrabarOT.IsEnabled = false;
            btnAbrirActividad.IsEnabled = false;
            btnAbrirTarea.IsEnabled = false;
            btnAbrirHerrEsp.IsEnabled = false;
            btnRepuesto.IsEnabled = false;
            btnConsumible.IsEnabled = false;
        }
        private void DesbloquearForm()
        {
            BtnGrabarOT.IsEnabled = true;
            btnAbrirActividad.IsEnabled = true;
            btnAbrirTarea.IsEnabled = true;
            btnAbrirHerrEsp.IsEnabled = true;
            btnRepuesto.IsEnabled = true;
            btnConsumible.IsEnabled = true;
        }

        private void rbtCancelar_Checked(object sender, RoutedEventArgs e)
        {
            ListarOT();
        }

        private void btnSeleccionarSeries_Click(object sender, RoutedEventArgs e)
        {
            //RegistrarTreelistNodos(treeListView1.Nodes); XAVI

            tblNroSeriesAsignadas.Rows.Clear();
            foreach (DataRow drSeries in tblNroSerie.Select("IsChecked = True"))
            {
                int existecheckeado = tblNroSerie.Select("IsChecked = True AND NroSerie = '" + drSeries["NroSerie"].ToString() + "' AND IdOTCompActividad = " + Convert.ToInt32(drSeries["IdOTCompActividad"])).Length;
                if (existecheckeado > 1)
                {
                    GlobalClass.ip.Mensaje(String.Format("El NroSerie {0} de la Herramienta: {1}, se encuentra asignadas en diferentes Actividades", drSeries["NroSerie"].ToString(), drSeries["Herramienta"].ToString()), 2);
                    return;
                }

                DataRow drAddSeries = tblNroSeriesAsignadas.NewRow();
                drAddSeries["IdOT"] = Convert.ToInt32(drSeries["IdOT"]);
                drAddSeries["IdHerramientaItem"] = Convert.ToInt32(drSeries["IdHerramientaItem"]);
                drAddSeries["Nuevo"] = false;
                tblNroSeriesAsignadas.Rows.Add(drAddSeries);
            }

            if (gbolIsOTMod)
            {
                gbolIsRegNroSeries = true;
                int rpta = objB_OT.OTHerramientas_UpdateNroSeries(tblNroSeriesAsignadas, FechaModificacion, gintIdUsuario);
                if (rpta == 1)
                {
                    BtnGrabarOT_Click(sender, e);
                    stkHerramientasSeries.Visibility = Visibility.Collapsed;
                    gbolIsRegNroSeries = false;
                }
                else if (rpta == 0)
                {
                    gbolIsRegNroSeries = false;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_MODI"), 2);
                    return;
                }

                else if (rpta == 1205)
                {
                    gbolIsRegNroSeries = false;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_CONC"), 2);
                    return;
                }
            }
            else
            {
                gbolRegHerramientas = true;
                btnAceptarCambioEstado_Click(sender, e);
                stkHerramientasSeries.Visibility = Visibility.Collapsed;
                gbolRegHerramientas = false;
                //LimpiarTreelistNodos(treeListView1.Nodes);
            }
        }

        private void btnCancelarSeries_Click(object sender, RoutedEventArgs e)
        {
            stkHerramientasSeries.Visibility = Visibility.Collapsed;
            gbolRegHerramientas = false;
            //LimpiarTreelistNodos(treeListView1.Nodes); XAVI
        }

        private void treeListView1_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            //treeListView1.NodeCheckStateChanged -= new DevExpress.Xpf.Grid.TreeList.TreeListNodeEventHandler(treeListView1_NodeCheckStateChanged); XAVI
            //SetCheckedChilNodes(e.Node, e.Node.IsChecked);
            //SetCheckedParentNodes(e.Node, e.Node.IsChecked);
            //treeListView1.NodeCheckStateChanged += new DevExpress.Xpf.Grid.TreeList.TreeListNodeEventHandler(treeListView1_NodeCheckStateChanged);
        }

        private void SetCheckedChilNodes(TreeListNode nodo, bool? Checkado)
        {
            for (int i = 0; i < nodo.Nodes.Count; i++)
            {
                nodo.Nodes[i].IsChecked = Checkado;
                SetCheckedChilNodes(nodo.Nodes[i], Checkado);
            }
        }

        private void SetCheckedParentNodes(TreeListNode nodo, bool? Checkado)
        {
            //Si todos los hijos del Padre tienen la misma propiedad IsChecked (True/False),
            //entonces el padre tendrá lo tendra tambien, encaso contrario lo Seteamos con 'null'
            if (nodo.ParentNode == null) return;
            bool esTodoIgual = true;
            for (int i = 0; i < nodo.ParentNode.Nodes.Count; i++)
            {
                bool? state = nodo.ParentNode.Nodes[i].IsChecked;
                if (Checkado != state)
                {
                    esTodoIgual = false;
                    break;
                }
            }
            nodo.ParentNode.IsChecked = esTodoIgual ? Checkado : null;
            SetCheckedParentNodes(nodo.ParentNode, Checkado);
        }


        private void RegistrarTreelistNodos(TreeListNodeCollection nodos)
        {
            gstrIdHerramientas = "";
            foreach (TreeListNode nodo in nodos)
            {
                DataRowView DataNodo = (DataRowView)nodo.Content;
                string idmenu = DataNodo.Row["IdHerramienta"].ToString();
                int cant = Convert.ToInt32(tblHerramientaDatosCambioEstado.Compute("MAX(Cantidad)", "IdHerramienta = " + idmenu));
                string Herramienta = tblHerramientaDatosCambioEstado.Select("IdHerramienta = " + DataNodo.Row["IdHerramienta"])[0]["Herramienta"].ToString();
                int contHijos = 0;
                foreach (TreeListNode hijo in nodo.Nodes)
                {
                    if (hijo.IsChecked == true)
                    {
                        contHijos++;
                        DataRowView DataNodoHijo = (DataRowView)hijo.Content;
                        string idmenuHijo = DataNodoHijo.Row["IdHerramientaItem"].ToString();
                        if (idmenuHijo.Trim() != "")
                        {
                            gstrIdHerramientas += idmenuHijo + "|";
                        }
                    }
                }
                if (cant != contHijos)
                {
                    GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_CAMB_HERR_ASIG"), Herramienta, cant), 2);
                    return;
                }
            }
        }
        private void LimpiarTreelistNodos(TreeListNodeCollection nodos)
        {
            foreach (TreeListNode nodo in nodos)
            {
                nodo.IsChecked = false;
                LimpiarTreelistNodos(nodo.Nodes);
            }
        }

        private void cboRIProveedor_PopupOpening(object sender, DevExpress.Xpf.Editors.OpenPopupEventArgs e)
        {
            try
            {
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                OCRDList = InterfazMTTO.iSBO_BL.SocioNegocio_BL.ConsultaProveedor("Y", ref RPTA);

                if (RPTA.ResultadoRetorno == 0)
                {
                    cboRIProveedor.ItemsSource = OCRDList;
                    cboRIProveedor.ValueMember = "CodigoProveedor";
                    cboRIProveedor.DisplayMember = "DescripcionProveedor";
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }

        private void ActualizarEstadoOTRIP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int IdOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdOT"));
                if (DtpRIFechCierre.EditValue == null || DtpRIFechCierre.EditValue.ToString().Trim() == "")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_IPRO_FECH_CIER"), 2);
                    DtpRIFechCierre.Focus();
                    return;
                }
                if (Convert.ToDateTime(lblRIFecLiberacion.Content) > Convert.ToDateTime(DtpRIFechCierre.EditValue))
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_CERR_OT_FECH"), 2);
                    DtpRIFechCierre.Focus();
                    return;
                }
                var rpt = DevExpress.Xpf.Core.DXMessageBox.Show(string.Format("Seguro de Cerrar la OT Nro: {0} ?", txtRICodOT.Text), "Cambio de Estado", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rpt == MessageBoxResult.Yes)
                {
                    //Actualiza Estado
                    objE_OT = new E_OT();
                    objE_OT.IdOT = IdOT;
                    objE_OT.IdEstadoOT = 5;
                    objE_OT.FechaCierre = Convert.ToDateTime(DtpRIFechCierre.EditValue);
                    objE_OT.IdUsuarioModificacion = Utilitarios.Utilitarios.gintIdUsuario;
                    objE_OT.IsRegProveedor = 1;
                    objE_OT.FechaModificacion = FechaModificacion;

                    int cant = objB_OT.OT_UpdateEstado(objE_OT);
                    if (cant == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_RTAR_CERR"), 1);
                        LimpiarTareasRealizadas();
                        GlobalClass.ip.SeleccionarTab(tabListadoOT);
                        //tabControl1.SelectedIndex = 0; ////Tab Javier Consultas
                        ListarOT();
                    }
                    else if (cant == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (cant == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_CONC"), 2);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }

        }
        Boolean gbolIsFrecExten = false;
        private void chkFrecExtendida_Checked(object sender, RoutedEventArgs e)
        {
            int IdOTArticulo = Convert.ToInt32(DtgRespuestosConsumibles.GetCellValue(tblViewRepCon.FocusedRowHandle, "IdOTArticulo"));
            gbolIsFrecExten = true;
            txtCantidadUtilizada.IsEnabled = false;
            txtCantidadUtilizada.EditValue = 0;
            txtCantidadUtilizada.MinValue = 0;
            txtNroSerie.IsReadOnly = true;
            txtFrecuencia.IsEnabled = true;
            txtFrecuenciaTm.IsEnabled = true;

            foreach (DataRow drArtiTare in tblArticuloTarea.Select("IdOTArticulo = " + IdOTArticulo))
            {
                if ((bool)chkFrecExtendida.IsChecked == Convert.ToBoolean(drArtiTare["FlagExtendida"]))
                {
                    txtCantidadUtilizada.EditValue = Convert.ToDouble(drArtiTare["CantUti"]);
                    txtNroSerie.Text = drArtiTare["NroSerie"].ToString();
                    txtFrecuencia.EditValue = drArtiTare["Frecuencia"].ToString();
                    txtFrecuenciaTm.EditValue = (Convert.ToDouble(drArtiTare["FrecuenciaTie"]) / gintValorTiempoDefecto).ToString();
                }
                else
                {
                    txtNroSerie.Text = gstrNroSerieSelec;//DtgRespuestosConsumibles.GetFocusedRowCellValue("NroSerie").ToString();
                    txtCantidadUtilizada.EditValue = 0;
                    txtFrecuencia.EditValue = "0";
                    txtFrecuenciaTm.EditValue = "0";
                }
            }


        }

        private void chkFrecExtendida_Unchecked(object sender, RoutedEventArgs e)
        {
            int IdOTArticulo = Convert.ToInt32(DtgRespuestosConsumibles.GetCellValue(tblViewRepCon.FocusedRowHandle, "IdOTArticulo"));
            gbolIsFrecExten = false;
            txtCantidadUtilizada.IsEnabled = true;
            txtCantidadUtilizada.MinValue = 1;
            txtNroSerie.IsReadOnly = false;
            txtFrecuencia.IsEnabled = true;
            txtFrecuenciaTm.IsEnabled = true;

            foreach (DataRow drArtiTare in tblArticuloTarea.Select("IdOTArticulo = " + IdOTArticulo))
            {
                if ((bool)chkFrecExtendida.IsChecked == Convert.ToBoolean(drArtiTare["FlagExtendida"]))
                {
                    txtCantidadUtilizada.EditValue = Convert.ToDouble(drArtiTare["CantUti"]);
                    txtNroSerie.Text = drArtiTare["NroSerie"].ToString();
                    txtFrecuencia.EditValue = drArtiTare["Frecuencia"].ToString();
                    txtFrecuenciaTm.EditValue = (Convert.ToDouble(drArtiTare["FrecuenciaTie"]) / gintValorTiempoDefecto).ToString();
                }
                else
                {
                    txtCantidadUtilizada.EditValue = 1;
                    txtNroSerie.Text = "";
                    txtFrecuencia.EditValue = "0";
                    txtFrecuenciaTm.EditValue = "0";
                }
            }
        }

        private void txtFrecuencia_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (gbolIsFrecExten)
            {
                if (txtFrecuencia.EditValue.ToString() == "0")
                {
                    txtFrecuenciaTm.IsEnabled = true;
                }
                else
                {
                    txtFrecuenciaTm.IsEnabled = false;
                    txtFrecuenciaTm.EditValue = "0";
                }
            }
        }

        private void txtFrecuenciaTm_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (gbolIsFrecExten)
            {
                if (txtFrecuenciaTm.EditValue.ToString() == "0")
                {
                    txtFrecuencia.IsEnabled = true;
                }
                else
                {
                    txtFrecuencia.IsEnabled = false;
                    txtFrecuencia.EditValue = "0";
                }
            }
        }

        private void txtCantidadUtilizada_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }

        private void txtNroSerie_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }

        private void btnImprimirOT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgOT.VisibleRowCount == 0) { return; }
                var otId = Convert.ToInt32(dtgOT.GetCellDisplayText(tblvOT.FocusedRowHandle, "IdOT"));

                GlobalClass.GeneraImpresionOT((int)MenuEnum.OrdenTrabajo, otId);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgOT_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgOT.VisibleRowCount == 0) { return; }
                int IdEstadoOT = Convert.ToInt32(dtgOT.GetFocusedRowCellValue("IdEstadoOT"));

                if (IdEstadoOT == 4 || IdEstadoOT == 5)
                {
                    btnImprimirOT.Visibility = Visibility.Visible;
                }
                else
                {
                    btnImprimirOT.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                GlobalClass.GeneraImpresion(gintIdMenu, gintIdOT);
            }
            catch { }
        }

        private bool ValidaTipoCambio()
        {
            bool val = true;

            try
            {
                    //Obtener Tipo de Cambio
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                InterfazMTTO.iSBO_BE.BEORTT tipoCambio = null;
                tipoCambio = InterfazMTTO.iSBO_BL.TipoCambio_BL.ObtenerTipoCambioPorFecha(DateTime.Now, ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
                val = false;
                return val;
            }
            return val;
        }

        private bool ValidaTipoAveria()
        {
            bool val = true;

            try
            {
                if (codTipoReq == (int)TipoRequerimientoEnum.Averia)
                {
                    if (cboTipoAveria.SelectedIndex == -1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "OBLI_TIPO_REQUERIMIENTO"), 2);
                        val = false;
                        return val;
                    }
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
                val = false;
                return val;
            }
            return val;
        }


        private bool ValidaAlmacenArticulo(DataTable tableRepuesto, DataTable tablaConsumible)
        {
            bool val = true;

            try
            {

                IdTipoOrden = Convert.ToInt32(CboOrden.EditValue);
                if (Convert.ToInt32(IdTipoOrden) == 2) //Revisar 
                {
                    return true;
                }
                    string almacenEntrada = GetAmacenEntrada();
                string almacenSalida = GetAmacenSalida();

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                InterfazMTTO.iSBO_BE.BEOITWList listaOITW = new InterfazMTTO.iSBO_BE.BEOITWList();

                if (almacenEntrada == "" || almacenSalida == "")
                {
                    GlobalClass.ip.Mensaje("No se encontro un almacén de entrada y/o salida", 2);
                    return false;
                }

                if(tablaConsumible.Rows.Count>0) tableRepuesto.Merge(tablaConsumible);

                //Repuestos
                for (int i = 0; i < tblRepuesto.Rows.Count; i++)
                {
                    string articuloId = tblRepuesto.Rows[i]["IdArticulo"].ToString();

                    listaOITW = InterfazMTTO.iSBO_BL.Articulo_BL.ObtenerAlmacenEntradaSalidaArticulo(articuloId, almacenEntrada, almacenSalida, ref RPTA);
                    if (RPTA.ResultadoRetorno != 0)
                    {
                        val = false;
                        GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                        break;
                    }

                    if (listaOITW.Count == 1)
                    {

                        if (listaOITW[0].WhsCode == almacenEntrada)
                        {
                            //GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaOT, "GRAB_CONC"), 2);
                            GlobalClass.ip.Mensaje("Articulo " + listaOITW[0].WhsCode + " no tiene almacen de salida", 2);
                            val = false;
                            break;
                        }
                        else
                        {
                            GlobalClass.ip.Mensaje("Articulo " + listaOITW[0].WhsCode + " no tiene almacen de entrada", 2);
                            val = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
                val = false;
                return val;
            }
            return val;
        }

        private string GetAmacenEntrada()
        {
            try
            {
                objE_TablaMaestra.IdTabla = 42;
                DataTable tblAlmacen = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);

                if (tblAlmacen.Rows.Count > 0)
                    return tblAlmacen.Rows[0]["Valor"].ToString();
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
                return "";
            }
        }

        private string GetAmacenSalida()
        {
            try
            {
                objE_TablaMaestra.IdTabla = 42;
                DataTable tblAlmacen = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);

                if (tblAlmacen.Rows.Count > 0)
                    return tblAlmacen.Rows[1]["Valor"].ToString();//General
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                GlobalClass.ip.Mensaje(ex.Message, 3);
                return string.Empty;
            }
        }
    }
}