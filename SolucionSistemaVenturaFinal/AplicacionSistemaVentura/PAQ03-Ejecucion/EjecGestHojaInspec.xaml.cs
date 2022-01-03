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
using System.Data;
using Entities;
using Business;
using Utilitarios;

namespace AplicacionSistemaVentura.PAQ03_Ejecucion
{
    /// <summary>
    /// Interaction logic for EjecGestHojaInspec.xaml
    /// </summary>
    public partial class EjecGestHojaInspec : UserControl
    {
        #region DefincionDatos

        B_PerfilComp objB_Perfilcomp = new B_PerfilComp();
        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        B_UC objB_UC = new B_UC();
        B_Perfil objB_Perfil = new B_Perfil();
        B_Actividad objB_Actividad = new B_Actividad();
        B_PM objB_PM = new B_PM();
        B_HR objB_HR = new B_HR();
        B_HI objB_HI = new B_HI();
        B_UCComp objB_UCComp = new B_UCComp();
        B_ContadorDet objB_ContadorDet = new B_ContadorDet();
        B_PerfilTarea objB_PerfilTarea = new B_PerfilTarea();

        E_PerfilTarea objE_PerfilTarea = new E_PerfilTarea();
        E_Actividad objE_Actividad = new E_Actividad();
        E_ContadorDet objE_ContadorDet = new E_ContadorDet();
        E_UCComp objE_UCComp = new E_UCComp();
        E_HI objE_HI = new E_HI();
        E_HR objE_HR = new E_HR();
        E_PM objE_PM = new E_PM();
        E_Perfil objE_Perfil = new E_Perfil();
        E_PerfilComp objE_Perfilcomp = new E_PerfilComp();
        E_UC objE_UC = new E_UC();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();

        Utilitarios.ErrorHandler Error = new Utilitarios.ErrorHandler();

        
        DataView dtvACtividades = new DataView();
        DataView dtv_maestra = new DataView();
        DataTable tblComboHR = new DataTable();
        DataTable tblUCList = new DataTable();
        DataTable tblHRDetalle = new DataTable();
        DataTable tblPerfilComp = new DataTable();
        DataTable tblDetalleHI = new DataTable();
        InterfazMTTO.iSBO_BE.BEOHEMList lstEmpleadosSAP = new InterfazMTTO.iSBO_BE.BEOHEMList();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();

        InterfazMTTO.iSBO_BE.BEOITMList lstRepuestosSAP = new InterfazMTTO.iSBO_BE.BEOITMList();
        InterfazMTTO.iSBO_BE.BEOITMList lstConsumiblesSAP = new InterfazMTTO.iSBO_BE.BEOITMList();

        int gintIdPerfil = 0;
        Boolean gbolAllActividades = true;
        Boolean gbolNuevo = false, gbolEdicion = false;
        Boolean gbolIsDetalles = false;
        Boolean gbolFlagRequiereOT = false;
        Boolean gbolFlagInactivo = false;
        Boolean gbolIsConsulta = false;

        DateTime FechaModificacion;

        int gintIdHIComp = 1;
        int gintIdHICompActividad = 1;
        int gintIdHIDetalle = 1;
        int gintIdHITarea = 1;
        int gintIdHIHorasDetalle = 1;
        int gintIdMenu = 0;

        int gintIdPerfilComp = 0;
        int gintIdHI = 0;
        DataTable tblTareasDatos = new DataTable();
        DataTable tblDetallesDatos = new DataTable();
        DataTable tblComboHerramienta = new DataTable();
        DataView dtvEstadosHI = new DataView();
        //Tablas para el guardado masivo
        DataTable tblHIComp = new DataTable();
        DataTable tblHIHorasDetalle = new DataTable();
        DataTable tblHIComp_Actividad = new DataTable();
        DataTable tblHITarea = new DataTable();
        DataTable tblHIDetalle = new DataTable();
        #endregion

        string gstrEtiquetaHojaInspeccion = "EjecGestHojaInspec";

        public EjecGestHojaInspec()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        private void UserControl_Loaded()
        {
            try
            {
                
                cboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);

                GlobalClass.ip.SeleccionarTab(tbListadoHI);
                objE_UC.IdPerfil = 0;
                tblUCList = objB_UC.B_UC_Combo(objE_UC);

                objE_TablaMaestra.IdTabla = 0;
                dtv_maestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).DefaultView;

                cboUnidadControl.ItemsSource = tblUCList;
                cboUnidadControl.DisplayMember = "PlacaSerie";
                cboUnidadControl.ValueMember = "IdUC";

                dtvEstadosHI = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 16", dtv_maestra).DefaultView;
                dtvEstadosHI.RowFilter = "IdColumna <> 4";
                cboEstado.ItemsSource = dtvEstadosHI;
                cboEstado.DisplayMember = "Descripcion";
                cboEstado.ValueMember = "IdColumna";

                lstEmpleadosSAP = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("R", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    cboResponsable.ItemsSource = lstEmpleadosSAP;
                    cboResponsable.DisplayMember = "NombrePersona";
                    cboResponsable.ValueMember = "CodigoPersona";

                    cboTrabajador.ItemsSource = lstEmpleadosSAP;
                    cboTrabajador.DisplayMember = "NombrePersona";
                    cboTrabajador.ValueMember = "CodigoPersona";
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                lstRepuestosSAP = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("R", ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                lstConsumiblesSAP = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("C", ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }

                tblHIComp.Columns.Add("IdHIComp", Type.GetType("System.Int32"));
                tblHIComp.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
                tblHIComp.Columns.Add("IdHRComp", Type.GetType("System.Int32"));
                tblHIComp.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblHIComp.Columns.Add("IdHI", Type.GetType("System.Int32"));
                tblHIComp.Columns.Add("FlagSolicitaInspeccion", Type.GetType("System.Boolean"));
                tblHIComp.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblHIComp.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                tblHIHorasDetalle.Columns.Add("IdHIHorasDetalle", Type.GetType("System.Int32"));
                tblHIHorasDetalle.Columns.Add("IdHI", Type.GetType("System.Int32"));
                tblHIHorasDetalle.Columns.Add("CodResponsable", Type.GetType("System.String"));
                tblHIHorasDetalle.Columns.Add("Responsable", Type.GetType("System.String"));
                tblHIHorasDetalle.Columns.Add("CostoHoraHombre", Type.GetType("System.Double"));
                tblHIHorasDetalle.Columns.Add("FechaInicial", Type.GetType("System.DateTime"));
                tblHIHorasDetalle.Columns.Add("FechaFinal", Type.GetType("System.DateTime"));
                tblHIHorasDetalle.Columns.Add("HorasReal", Type.GetType("System.Double"));
                tblHIHorasDetalle.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblHIHorasDetalle.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                tblHIComp_Actividad.Columns.Add("IdHICompActividad", Type.GetType("System.Int32"));
                tblHIComp_Actividad.Columns.Add("IdHIComp", Type.GetType("System.Int32"));
                tblHIComp_Actividad.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblHIComp_Actividad.Columns.Add("IdActividad", Type.GetType("System.Int32"));
                tblHIComp_Actividad.Columns.Add("Actividad", Type.GetType("System.String"));
                tblHIComp_Actividad.Columns.Add("FlagUso", Type.GetType("System.Boolean"));
                tblHIComp_Actividad.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblHIComp_Actividad.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                tblHITarea.Columns.Add("IdHITarea", Type.GetType("System.Int32"));
                tblHITarea.Columns.Add("IdHICompActividad", Type.GetType("System.Int32"));
                tblHITarea.Columns.Add("IdTarea", Type.GetType("System.Int32"));
                tblHITarea.Columns.Add("Tarea", Type.GetType("System.String"));
                tblHITarea.Columns.Add("HorasHombre", Type.GetType("System.Double"));
                tblHITarea.Columns.Add("FlagAutomatico", Type.GetType("System.Boolean"));
                tblHITarea.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblHITarea.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                tblHIDetalle.Columns.Add("IdHIDetalle", Type.GetType("System.Int32"));
                tblHIDetalle.Columns.Add("IdHICompActividad", Type.GetType("System.Int32"));
                tblHIDetalle.Columns.Add("IdTipoArticulo", Type.GetType("System.Int32"));
                tblHIDetalle.Columns.Add("IdArticulo", Type.GetType("System.String"));
                tblHIDetalle.Columns.Add("Articulo", Type.GetType("System.String"));
                tblHIDetalle.Columns.Add("Cantidad", Type.GetType("System.Int32"));
                tblHIDetalle.Columns.Add("FlagAutomatico", Type.GetType("System.Boolean"));
                tblHIDetalle.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblHIDetalle.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                rbnAbierto.IsChecked = true;
                ListarHI();

                //lstbActividad.ItemsSource = tblHIComp_Actividad.DefaultView;
                lstbActividad.DisplayMember = "Actividad";
                lstbActividad.ValueMember = "IdHICompActividad";

                grbActividades.IsEnabled = false;
                LlenarComboHR();

                #region VisualizacionBotonImprimir
                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                if (!VisualizaBotonImprimirDetalle)
                {
                    btnImprimir.Visibility = System.Windows.Visibility.Hidden;
                }
                #endregion
                cboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarHI()
        {
            if ((bool)rbnTodos.IsChecked)
            {
                objE_HI.IdEstadoHI = 0;
            }
            else if ((bool)rbnAbierto.IsChecked)
            {
                objE_HI.IdEstadoHI = 1;
            }
            else if ((bool)rbnAtendido.IsChecked)
            {
                objE_HI.IdEstadoHI = 4;
            }
            else if ((bool)rbnCancelado.IsChecked)
            {
                objE_HI.IdEstadoHI = 2;
            }
            else if ((bool)rbnCerrado.IsChecked)
            {
                objE_HI.IdEstadoHI = 3;
            }
            dtgHI.ItemsSource = objB_HI.HI_List(objE_HI);
        }

        private void BtnRegistrarInspeccion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tblUCList.Rows.Count == 0) { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_CANT_UC"), 2); return; }
                if (lstEmpleadosSAP.Count == 0) { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_CANT_SOLI"), 2); return; }

                LbLHojaInspeccion.Content = "Nuevo Código";
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                grbActividades.IsEnabled = false;
                gbolFlagRequiereOT = false;
                cboEstado.EditValue = 1;
                cboEstado.IsEnabled = false;
                GlobalClass.ip.SeleccionarTab(tbDetalleHI);
                EstadoForm(true, false, true);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnModificarInspeccion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgHI.VisibleRowCount == 0) { return; }
                gintIdHI = Convert.ToInt32(dtgHI.GetFocusedRowCellValue("IdHI"));
                LlenarDetallesHI();
                EstadoForm(false, true, true);
                GlobalClass.ip.SeleccionarTab(tbDetalleHI);
                EstadoForm(false, true, true);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalClass.ip.SeleccionarTab(tbListadoHI);
                LimpiarControles();
                LimpiarCambioDeUC();
                ListarHI();
                EstadoForm(false, false, true);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LimpiarControles()
        {
            cboHR.SelectedIndexChanged -= new RoutedEventHandler(cboHR_SelectedIndexChanged);
            cboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
            cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
            trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
            rbnPlanMantenimiento.Checked -= new RoutedEventHandler(rbnPlanMantenimiento_Checked);
            rbnPerfil.Checked -= new RoutedEventHandler(rbnPerfil_Checked);
            rbnManual.Checked -= new RoutedEventHandler(rbnManual_Checked);
            cboResponsable.SelectedIndexChanged -= new RoutedEventHandler(cboResponsable_SelectedIndexChanged);
            txtComentarios.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtComentarios_EditValueChanged);
            lstbActividad.SelectedIndexChanged -= new RoutedEventHandler(lstbActividad_SelectedIndexChanged);
            dtpFechaHI.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpFechaHI_EditValueChanged);

            cboEstado.IsEnabled = true;
            cboHR.EditValue = null;
            cboUnidadControl.EditValue = null;
            cboUnidadControl.IsEnabled = true;
            cboPerfil.EditValue = null;
            cboEstado.EditValue = "1";
            cboResponsable.EditValue = null;

            dtpFechaHI.EditValue = null;
            chkProgramado.IsChecked = false;
            lblNroOT.Content = "";
            chkReqOT.IsChecked = false;

            Utilitarios.TreeViewModel.LimpiarDatosTreeview();
            trvComp.ItemsSource = null;

            rbnPlanMantenimiento.IsChecked = false;
            cboPlanMantenimiento.EditValue = null;
            cboPlanMantenimiento.ItemsSource = null;
            rbnPerfil.IsChecked = false;
            rbnManual.IsChecked = false;

            lstbActividad.EditValue = null;
            lstbActividad.ItemsSource = null;

            dtgTareas.ItemsSource = null;
            dtgHerramientas.ItemsSource = null;
            dtgRepuestos.ItemsSource = null;
            dtgConsumibles.ItemsSource = null;
            dtgActividades.ItemsSource = null;
            dtgListarTrabajador.ItemsSource = null;

            lblCantHoras.Content = "0";

            dtpFechaInicio.EditValue = null;
            dtpFechaFinal.EditValue = null;
            txtKilomInicial.Text = "0";
            txtKilomFinal.Text = "0";
            txtComentarios.Text = "";

            gintIdHICompActividad = 1;
            gintIdHIDetalle = 1;
            gintIdHITarea = 1;
            gintIdPerfil = 0;

            gbolIsDetalles = false;
            gbolFlagRequiereOT = false;
            cboHR.IsEnabled = true;
            cboUnidadControl.IsEnabled = true;
            dtpFechaHI.IsEnabled = true;
            dtpFechaInicio.IsEnabled = true;
            dtpFechaFinal.IsEnabled = true;
            txtKilomInicial.IsEnabled = true;
            txtKilomFinal.IsEnabled = true;

            grbActividades.IsEnabled = false;
            BloquearControlesPorEstado(false);

            tblHIComp_Actividad.Rows.Clear();
            tblHIComp.Rows.Clear();
            tblHITarea.Rows.Clear();
            tblHIHorasDetalle.Rows.Clear();
            tblHIDetalle.Rows.Clear();

            dtpFechaHI.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpFechaHI_EditValueChanged);
            lstbActividad.SelectedIndexChanged += new RoutedEventHandler(lstbActividad_SelectedIndexChanged);
            cboHR.SelectedIndexChanged += new RoutedEventHandler(cboHR_SelectedIndexChanged);
            cboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
            cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
            trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
            rbnPlanMantenimiento.Checked += new RoutedEventHandler(rbnPlanMantenimiento_Checked);
            rbnPerfil.Checked += new RoutedEventHandler(rbnPerfil_Checked);
            rbnManual.Checked += new RoutedEventHandler(rbnManual_Checked);
            cboResponsable.SelectedIndexChanged += new RoutedEventHandler(cboResponsable_SelectedIndexChanged);
            txtComentarios.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtComentarios_EditValueChanged);
        }

        private void BtnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidaCampoObligado()) { return; }

                if ((bool)chkReqOT.IsChecked && tblHIComp_Actividad.Select("FlagActivo = True").Length == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_AGRE_COMP_ACTI"), 2);
                    return;
                }

                if ((bool)chkReqOT.IsChecked && tblHITarea.Select("FlagActivo = True").Length == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_TARE_ASIG"), 2);
                    return;
                }

                int IdHIComp = 0;
                int IdHICompActividad = 1;
                foreach (DataRow drActividad in tblHIComp_Actividad.Select("Nuevo = true"))
                {
                    int IdPerfilComp = Convert.ToInt32(drActividad["IdPerfilComp"]);
                    if (tblHIComp.Select("IdPerfilComp = " + IdPerfilComp).Length == 0)
                    {
                        DataRow[] drPFComp = tblPerfilComp.Select("IdPerfilComp = " + IdPerfilComp);

                        DataRow drComp = tblHIComp.NewRow();
                        drComp["IdHIComp"] = gintIdHIComp;
                        drComp["IdUCComp"] = Convert.ToInt32(drPFComp[0]["IdUCComp"]);
                        drComp["IdHRComp"] = Convert.ToInt32(drPFComp[0]["IdHRComp"]);
                        drComp["IdPerfilComp"] = IdPerfilComp;
                        drComp["IdHI"] = 0;
                        drComp["FlagSolicitaInspeccion"] = Convert.ToBoolean(drPFComp[0]["FlagSolicitaInspeccion"]);
                        drComp["FlagActivo"] = true;
                        drComp["Nuevo"] = true;
                        tblHIComp.Rows.Add(drComp);
                        IdHIComp = gintIdHIComp;
                        gintIdHIComp++;
                    }
                    else
                    {
                        IdHIComp = Convert.ToInt32(tblHIComp.Select("IdPerfilComp = " + IdPerfilComp)[0]["IdHIComp"]);
                    }

                    foreach (DataRow drTarea in tblHITarea.Select("IdHICompActividad = " + Convert.ToInt32(drActividad["IdHICompActividad"])))
                    {
                        drTarea["IdHICompActividad"] = IdHICompActividad;
                    }
                    foreach (DataRow drDetalle in tblHIDetalle.Select("IdHICompActividad = " + Convert.ToInt32(drActividad["IdHICompActividad"])))
                    {
                        drDetalle["IdHICompActividad"] = IdHICompActividad;
                    }
                    drActividad["IdHICompActividad"] = IdHICompActividad;
                    drActividad["IdHIComp"] = IdHIComp;
                    IdHICompActividad++;
                }



                if (gbolNuevo == true && gbolEdicion == false)
                {
                    objE_ContadorDet.CodUc = tblUCList.Select("IdUC = " + cboUnidadControl.EditValue.ToString())[0]["CodUC"].ToString();
                    objE_ContadorDet.IdOrigenRegistro = 1;
                    objE_ContadorDet.IdEvento = 1;
                    objE_ContadorDet.IdTipoOperacion = 1;
                    objE_ContadorDet.NroDocOperacion = "";
                    objE_ContadorDet.IdDocCorregir = 0;
                    objE_ContadorDet.FechaHoraIni = Convert.ToDateTime(dtpFechaInicio.EditValue);
                    objE_ContadorDet.FechaHoraFin = Convert.ToDateTime(dtpFechaFinal.EditValue);
                    objE_ContadorDet.ContadorIni = Convert.ToDouble(txtKilomInicial.EditValue);
                    objE_ContadorDet.ContadorFin = Convert.ToDouble(txtKilomFinal.EditValue);
                    objE_ContadorDet.CodSolicitante = (cboHR.EditValue == null) ? "" : tblHRDetalle.Rows[0]["CodSolicitanteSAP"].ToString();
                    objE_ContadorDet.CodResponsable = cboResponsable.EditValue.ToString();
                    objE_ContadorDet.Observacion = txtComentarios.Text;
                    objE_ContadorDet.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;

                    string DescError = "";
                    int RPTACO = objB_ContadorDet.ContadorDet_UpdateProcess(objE_ContadorDet, out DescError);
                    if (RPTACO > 0)
                    {
                        objE_HI.IdHI = 0;
                        objE_HI.CodHI = "";
                        objE_HI.IdUC = Convert.ToInt32(cboUnidadControl.EditValue);
                        objE_HI.FechaInspeccion = Convert.ToDateTime(dtpFechaHI.EditValue);
                        objE_HI.IdOT = 0;
                        objE_HI.IdHR = (cboHR.EditValue != null) ? Convert.ToInt32(cboHR.EditValue) : 0;
                        objE_HI.FlagRequiereOT = (bool)chkReqOT.IsChecked;
                        objE_HI.FlagProgramado = false;
                        objE_HI.CodResponsableSAP = cboResponsable.EditValue.ToString();
                        objE_HI.NombreResponsableSAP = cboResponsable.Text;
                        objE_HI.FechaInicial = Convert.ToDateTime(dtpFechaInicio.EditValue);
                        objE_HI.FechaFinal = Convert.ToDateTime(dtpFechaFinal.EditValue);
                        objE_HI.KmInicial = Convert.ToDouble(txtKilomInicial.EditValue);
                        objE_HI.KmFinal = Convert.ToDouble(txtKilomFinal.EditValue);
                        objE_HI.Observacion = txtComentarios.Text;
                        objE_HI.IdEstadoHI = Convert.ToInt32(cboEstado.EditValue);
                        objE_HI.FlagActivo = true;
                        objE_HI.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
                        objE_HI.FechaModificacion = DateTime.Now;

                        int RPTAHI = objB_HI.HI_UpdateCascade(objE_HI, tblHIComp, tblHIComp_Actividad, tblHITarea, tblHIDetalle, tblHIHorasDetalle);
                        if (RPTAHI == 1)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "GRAB_NUEV"), 1);
                        }
                        else if (RPTAHI == 2)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_MODI_HRAS"), 2);
                            return;
                        }
                        else if (RPTAHI == 0)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_MODI"), 2);
                            return;
                        }
                        else if (RPTAHI == 1205)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "GRAB_CONC"), 2);
                            return;
                        }
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje(DescError, 2);
                        return;
                    }
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    foreach (DataRow drComp in tblHIComp.Rows)
                    {
                        if (tblHIComp_Actividad.Select("FlagActivo = True AND IdHIComp = " + Convert.ToInt32(drComp["IdHIComp"])).Length == 0)
                        {
                            drComp["FlagActivo"] = false;
                        }
                    }

                    objE_HI.IdHI = gintIdHI;
                    objE_HI.CodHI = "";
                    objE_HI.IdUC = Convert.ToInt32(cboUnidadControl.EditValue);
                    objE_HI.FechaInspeccion = Convert.ToDateTime(dtpFechaHI.EditValue);
                    objE_HI.IdOT = 0;
                    objE_HI.IdHR = (cboHR.EditValue != null) ? Convert.ToInt32(cboHR.EditValue) : 0;
                    objE_HI.FlagRequiereOT = (bool)chkReqOT.IsChecked;
                    objE_HI.FlagProgramado = false;
                    objE_HI.CodResponsableSAP = cboResponsable.EditValue.ToString();
                    objE_HI.NombreResponsableSAP = cboResponsable.Text;
                    objE_HI.FechaInicial = Convert.ToDateTime(dtpFechaInicio.EditValue);
                    objE_HI.FechaFinal = Convert.ToDateTime(dtpFechaFinal.EditValue);
                    objE_HI.KmInicial = Convert.ToDouble(txtKilomInicial.EditValue);
                    objE_HI.KmFinal = Convert.ToDouble(txtKilomFinal.EditValue);
                    objE_HI.Observacion = txtComentarios.Text;
                    objE_HI.IdEstadoHI = Convert.ToInt32(cboEstado.EditValue);
                    objE_HI.FlagActivo = true;
                    objE_HI.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
                    objE_HI.FechaModificacion = FechaModificacion;

                    int RPTAHI = objB_HI.HI_UpdateCascade(objE_HI, tblHIComp, tblHIComp_Actividad, tblHITarea, tblHIDetalle, tblHIHorasDetalle);
                    if (RPTAHI == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "GRAB_EDIT"), 1);
                    }
                    else if (RPTAHI == 2)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_MODI_HRAS"), 2);
                        return;
                    }
                    else if (RPTAHI == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (RPTAHI == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "GRAB_CONC"), 2);
                        return;
                    }
                }
                LimpiarCambioDeUC();
                LimpiarControles();
                ListarHI();
                GlobalClass.ip.SeleccionarTab(tbListadoHI);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAbrirActividades_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                if (!gbolFlagRequiereOT) { return; }
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm == null || trm.IdMenuPadre == 1000)
                { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_SELE_COMP"), 2); return; }

                if (!((bool)rbnPlanMantenimiento.IsChecked) && !((bool)rbnPerfil.IsChecked) && !((bool)rbnManual.IsChecked))
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_FILT_ACTI"), 2);
                    return;
                }

                if ((bool)rbnPlanMantenimiento.IsChecked && cboPlanMantenimiento.EditValue == null)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_SELE_PLAN"), 2);
                    return;
                }

                string codigos = "";

                DataView dtvActividadReg = (DataView)lstbActividad.ItemsSource;
                if (dtvActividadReg != null)
                {
                    foreach (DataRow drActReg in dtvActividadReg.ToTable().Select("IdPerfilComp = " + trm.IdMenu))
                    {
                        codigos += drActReg["IdActividad"].ToString() + ",";
                    }
                    if (codigos.Length != 0)
                    {
                        if ((bool)rbnManual.IsChecked)
                        {
                            dtvACtividades.RowFilter = "IdActividad NOT IN (" + codigos.Substring(0, codigos.Length - 1) + ")";
                        }
                        else
                        {
                            dtvACtividades.RowFilter = "IdPerfilComp =" + trm.IdMenu + "AND IdActividad NOT IN (" + codigos.Substring(0, codigos.Length - 1) + ")";
                        }
                    }
                    else
                    {
                        if ((bool)rbnManual.IsChecked)
                        {
                            dtvACtividades.RowFilter = "";
                        }
                        else
                        {
                            dtvACtividades.RowFilter = "IdPerfilComp =" + trm.IdMenu;
                        }
                    }
                }
                else
                {
                    if ((bool)rbnManual.IsChecked)
                    {
                        dtvACtividades.RowFilter = "";
                    }
                    else
                    {
                        dtvACtividades.RowFilter = "IdPerfilComp =" + trm.IdMenu;
                    }
                }
                gintIdPerfilComp = trm.IdMenu;
                dtgActividades.ItemsSource = dtvACtividades;
                stkActividad.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAbrirHorasDedicadas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stkTiempoDedicado.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAbrirTareas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                if (!gbolFlagRequiereOT) { return; }
                if (lstbActividad.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_SELE_ACTI"), 2);
                    return;
                }
                //DevExpress.Xpf.Core.DXMessageBox.Show(((DataRowView)lstbActividad.SelectedItem).Row["Actividad"].ToString());
                E_Tarea objE_Tarea = new E_Tarea();
                B_Tarea objB_Tarea = new B_Tarea();

                objE_Tarea.IdActividad = 0;
                objE_Tarea.Actividad = ((DataRowView)lstbActividad.SelectedItem).Row["Actividad"].ToString();
                cboTarea.ItemsSource = objB_Tarea.Tarea_ComboByAct(objE_Tarea);
                cboTarea.DisplayMember = "Tarea";
                cboTarea.ValueMember = "IdTarea";

                DataTable tblComboTareas = (DataTable)cboTarea.ItemsSource;
                DataTable tblTareasExistentes = (DataTable)dtgTareas.ItemsSource;

                string IdTarea = "";
                for (int i = 0; i < tblTareasExistentes.Rows.Count; i++)
                {
                    IdTarea += tblTareasExistentes.Rows[i]["IdTarea"].ToString() + ",";
                }

                if (tblTareasExistentes.Rows.Count != 0)
                {
                    tblComboTareas.DefaultView.RowFilter = "IdTarea NOT IN (" + IdTarea + ")";
                    tblComboTareas = tblComboTareas.DefaultView.ToTable(true);
                    cboTarea.ItemsSource = null;

                    cboTarea.ItemsSource = tblComboTareas;
                    cboTarea.DisplayMember = "Tarea";
                    cboTarea.ValueMember = "IdTarea";
                }
                stkPanelTareas.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAbrirHerramientas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                if (!gbolFlagRequiereOT) { return; }
                if (lstbActividad.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_SELE_ACTI"), 2);
                    return;
                }
                B_Herramienta objB_Herramienta = new B_Herramienta();

                tblComboHerramienta = objB_Herramienta.Herramienta_Combo();
                DataTable tblHerramientaExistente = (DataTable)dtgHerramientas.ItemsSource;

                string IdHerramienta = "";
                for (int i = 0; i < tblHerramientaExistente.Rows.Count; i++)
                {
                    IdHerramienta += tblHerramientaExistente.Rows[i]["IdArticulo"].ToString() + ",";
                }
                if (tblHerramientaExistente.Rows.Count != 0)
                {
                    tblComboHerramienta.DefaultView.RowFilter = "IdHerramienta NOT IN (" + IdHerramienta + ")";
                    tblComboHerramienta = tblComboHerramienta.DefaultView.ToTable(true);
                }
                cboHerramientaEspecial.ItemsSource = tblComboHerramienta;
                cboHerramientaEspecial.DisplayMember = "Herramienta";
                cboHerramientaEspecial.ValueMember = "IdHerramienta";

                spCantidadHerramienta.Value = 1;
                spCantidadHerramienta.MinValue = 1;

                stkPanelHerramientas.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAbrirRepuestos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                if (!gbolFlagRequiereOT) { return; }
                if (lstbActividad.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_SELE_ACTI"), 2);
                    return;
                }
                DataTable tblRepuestoExistentes = (DataTable)dtgRepuestos.ItemsSource;

                DataTable tblRepuesto = new DataTable();
                tblRepuesto.Columns.Add("Articulo");
                tblRepuesto.Columns.Add("IdArticulo");
                string IdCodigoArticulo = "";
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                lstRepuestosSAP = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("R", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    for (int i = 0; i < lstRepuestosSAP.Count; i++)
                    {
                        DataRow dr;
                        dr = tblRepuesto.NewRow();
                        dr["Articulo"] = lstRepuestosSAP[i].DescripcionArticulo;
                        dr["IdArticulo"] = lstRepuestosSAP[i].CodigoArticulo;
                        tblRepuesto.Rows.Add(dr);
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }

                for (int i = 0; i < tblRepuestoExistentes.Rows.Count; i++)
                {
                    IdCodigoArticulo += tblRepuestoExistentes.Rows[i]["IdArticulo"].ToString() + "', '";
                }

                if (tblRepuestoExistentes.Rows.Count != 0)
                {
                    IdCodigoArticulo = IdCodigoArticulo.Remove(IdCodigoArticulo.Length - 4);
                    string filtro = "IdArticulo  NOT IN ('" + IdCodigoArticulo + "')";
                    tblRepuesto.DefaultView.RowFilter = filtro;
                    tblRepuesto = tblRepuesto.DefaultView.ToTable(true);
                    cboRepuesto.ItemsSource = null;
                }
                cboRepuesto.ItemsSource = tblRepuesto;
                cboRepuesto.DisplayMember = "Articulo";
                cboRepuesto.ValueMember = "IdArticulo";

                stkPanelRepuestos.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAbrirConsumibles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                if (!gbolFlagRequiereOT) { return; }
                if (lstbActividad.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_SELE_ACTI"), 2);
                    return;
                }
                DataTable tblConsumiblesExistentes = (DataTable)dtgConsumibles.ItemsSource;

                DataTable tblConsumibles = new DataTable();
                tblConsumibles.Columns.Add("Articulo");
                tblConsumibles.Columns.Add("IdArticulo");
                string IdCodigoArticulo = "";
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                lstConsumiblesSAP = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("C", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    for (int i = 0; i < lstConsumiblesSAP.Count; i++)
                    {
                        DataRow dr;
                        dr = tblConsumibles.NewRow();
                        dr["Articulo"] = lstConsumiblesSAP[i].DescripcionArticulo;
                        dr["IdArticulo"] = lstConsumiblesSAP[i].CodigoArticulo;
                        tblConsumibles.Rows.Add(dr);
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }

                for (int i = 0; i < tblConsumiblesExistentes.Rows.Count; i++)
                {
                    IdCodigoArticulo += tblConsumiblesExistentes.Rows[i]["IdArticulo"].ToString() + "', '";
                }

                if (tblConsumiblesExistentes.Rows.Count != 0)
                {
                    IdCodigoArticulo = IdCodigoArticulo.Remove(IdCodigoArticulo.Length - 4);
                    string filtro = "IdArticulo  NOT IN ('" + IdCodigoArticulo + "')";

                    tblConsumibles.DefaultView.RowFilter = filtro;
                    tblConsumibles = tblConsumibles.DefaultView.ToTable(true);
                    cboConsumibles.ItemsSource = null;
                }
                cboConsumibles.ItemsSource = tblConsumibles;
                cboConsumibles.DisplayMember = "Articulo";
                cboConsumibles.ValueMember = "IdArticulo";

                stkPanelConsumibles.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void trvComp_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (trvComp.ItemsSource != null)
                {
                    TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                    if (trm != null)
                    {
                        lstbActividad.SelectedIndex = -1;
                        grbActividades.Header = (trm.IdMenuPadre == 1000) ? "Actividades - Componente" : "Actividades - Componente: " + trm.Name.Replace("* ", "").Replace(" ✓", "");
                        tblHIComp_Actividad.DefaultView.RowFilter = "FlagActivo = True AND IdPerfilComp = " + trm.IdMenu;
                        lstbActividad.ItemsSource = tblHIComp_Actividad.DefaultView;
                        dtgConsumibles.ItemsSource = null;
                        dtgHerramientas.ItemsSource = null;
                        dtgRepuestos.ItemsSource = null;
                        dtgTareas.ItemsSource = null;

                        grbActividades.IsEnabled = (trm.IdMenuPadre != 1000 && gbolFlagRequiereOT && !gbolFlagInactivo);
                    }

                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void CargarArbolComponentes(int IdPerfil)
        {
            Utilitarios.TreeViewModel.LimpiarDatosTreeview();
            trvComp.ItemsSource = null;

            objE_Perfilcomp.Idperfil = IdPerfil;
            objE_Perfilcomp.Idestadopc = 0;

            DataView dtvPerfilComp = new DataView(objB_Perfilcomp.PerfilComp_List(objE_Perfilcomp));
            dtvPerfilComp.RowFilter = "FlagNeumatico = 0";
            tblPerfilComp = dtvPerfilComp.ToTable();//objB_Perfilcomp.PerfilComp_List(objE_Perfilcomp);
            DataColumn dcNuevo = new DataColumn() { ColumnName = "Nuevo", DefaultValue = true };
            DataColumn dcNroSerie = new DataColumn() { ColumnName = "NroSerie", DefaultValue = "" };
            DataColumn dcIdUComp = new DataColumn() { ColumnName = "IdUCComp", DefaultValue = 0 };
            DataColumn dcFlagSolicitaInspeccion = new DataColumn() { ColumnName = "FlagSolicitaInspeccion", DefaultValue = false };
            DataColumn dcIdHRComp = new DataColumn() { ColumnName = "IdHRComp", DefaultValue = 0 };
            tblPerfilComp.Columns.Add(dcNuevo);
            tblPerfilComp.Columns.Add(dcNroSerie);
            tblPerfilComp.Columns.Add(dcIdUComp);
            tblPerfilComp.Columns.Add(dcFlagSolicitaInspeccion);
            tblPerfilComp.Columns.Add(dcIdHRComp);


            DataRow row = tblPerfilComp.NewRow();
            row["IdPerfilCompPadre"] = 1000;
            row["IdPerfilComp"] = 0;
            row["PerfilComp"] = cboPerfil.Text;
            row["Nuevo"] = false;
            row["Nivel"] = 1;
            tblPerfilComp.Rows.Add(row);

            objE_UCComp.IdUC = Convert.ToInt32(cboUnidadControl.EditValue);
            DataTable UCComp = objB_UCComp.UCComp_List(objE_UCComp);
            foreach (DataRow drUCComp in UCComp.Rows)
            {
                DataRow[] drPFComp = tblPerfilComp.Select("IdPerfilComp = " + drUCComp["IdPerfilComp"].ToString());
                if (drPFComp.Length > 0)
                {
                    drPFComp[0]["IdUCComp"] = Convert.ToInt32(drUCComp["IdUCComp"]);
                    drPFComp[0]["NroSerie"] = drUCComp["NroSerie"].ToString();
                }
            }
            if (cboHR.EditValue != null)
            {
                objE_HR.IdHR = Convert.ToInt32(cboHR.EditValue);
                DataTable HRCompDet = objB_HR.HRComp_List(objE_HR);
                foreach (DataRow drHRComp in HRCompDet.Rows)
                {
                    DataRow[] drPFComp = tblPerfilComp.Select("IdPerfilComp = " + drHRComp["IdPerfilComp"].ToString());
                    if (drPFComp.Length > 0)
                    {
                        drPFComp[0]["IdHRComp"] = Convert.ToInt32(drHRComp["IdHRComp"]);
                        drPFComp[0]["FlagSolicitaInspeccion"] = drHRComp["FlagSolicitaInspeccion"].ToString();
                        drPFComp[0]["PerfilComp"] = "* " + drPFComp[0]["PerfilComp"].ToString();
                    }
                }
            }

            if (gbolIsDetalles)
            {
                objE_HI.IdHI = gintIdHI;
                DataTable HICompDet = objB_HI.HIComp_List(objE_HI);
                DataTable tblHIHorasDetalletmp = objB_HI.HIHorasDetalle_List(objE_HI);
                DataTable tblHIComp_Actividadtmp = objB_HI.HIComp_Actividad_List(objE_HI);
                DataTable tblHIDetalletmp = objB_HI.HIDetalle_List(objE_HI);
                DataTable tblHITareatmp = objB_HI.HITarea_List(objE_HI);

                //Actualizando Nombres | IdTipoArticulo = 2 -> Repuestos
                foreach (DataRow drDetallesRep in tblHIDetalletmp.Select("IdTipoArticulo = 2"))
                {

                    foreach (var drArt in lstRepuestosSAP.Where(emp => emp.CodigoArticulo == drDetallesRep["IdArticulo"].ToString()))
                    {
                        drDetallesRep["Articulo"] = drArt.DescripcionArticulo;
                        break;
                    }
                }
                //Actualizando Nombres | IdTipoArticulo = 3 -> Consumibles
                foreach (DataRow drDetallesCon in tblHIDetalletmp.Select("IdTipoArticulo = 3"))
                {
                    foreach (var drArt in lstConsumiblesSAP.Where(emp => emp.CodigoArticulo == drDetallesCon["IdArticulo"].ToString()))
                    {
                        drDetallesCon["Articulo"] = drArt.DescripcionArticulo;
                        break;
                    }
                }

                foreach (DataRow drHorasDet in tblHIHorasDetalletmp.Rows)
                {
                    DataRow drHoras = tblHIHorasDetalle.NewRow();
                    drHoras["IdHIHorasDetalle"] = Convert.ToInt32(drHorasDet["IdHIHorasDetalle"]);
                    drHoras["IdHI"] = Convert.ToInt32(drHorasDet["IdHI"]);
                    drHoras["CodResponsable"] = drHorasDet["CodResponsable"].ToString();
                    drHoras["Responsable"] = drHorasDet["Responsable"].ToString();
                    drHoras["CostoHoraHombre"] = Convert.ToDouble(drHorasDet["CostoHoraHombre"]);
                    drHoras["FechaInicial"] = Convert.ToDateTime(drHorasDet["FechaInicial"]);
                    drHoras["FechaFinal"] = Convert.ToDateTime(drHorasDet["FechaFinal"]);
                    drHoras["HorasReal"] = Convert.ToDouble(drHorasDet["HorasReal"]);
                    drHoras["FlagActivo"] = Convert.ToBoolean(drHorasDet["FlagActivo"]);
                    drHoras["Nuevo"] = false;
                    tblHIHorasDetalle.Rows.Add(drHoras);
                }

                foreach (DataRow drCompActividadDet in tblHIComp_Actividadtmp.Rows)
                {
                    DataRow drCompActividad = tblHIComp_Actividad.NewRow();
                    drCompActividad["IdHICompActividad"] = Convert.ToInt32(drCompActividadDet["IdHICompActividad"]);
                    drCompActividad["IdHIComp"] = Convert.ToInt32(drCompActividadDet["IdHIComp"]);
                    drCompActividad["IdPerfilComp"] = Convert.ToInt32(drCompActividadDet["IdPerfilComp"]);
                    drCompActividad["IdActividad"] = Convert.ToInt32(drCompActividadDet["IdActividad"]);
                    drCompActividad["Actividad"] = drCompActividadDet["Actividad"].ToString();
                    drCompActividad["FlagUso"] = Convert.ToBoolean(drCompActividadDet["FlagUso"]);
                    drCompActividad["FlagActivo"] = Convert.ToBoolean(drCompActividadDet["FlagActivo"]);
                    drCompActividad["Nuevo"] = false;
                    tblHIComp_Actividad.Rows.Add(drCompActividad);
                }

                foreach (DataRow drDetalleDet in tblHIDetalletmp.Rows)
                {
                    DataRow drDetalle = tblHIDetalle.NewRow();
                    drDetalle["IdHIDetalle"] = Convert.ToInt32(drDetalleDet["IdHIDetalle"]);
                    drDetalle["IdHICompActividad"] = Convert.ToInt32(drDetalleDet["IdHICompActividad"]);
                    drDetalle["IdTipoArticulo"] = Convert.ToInt32(drDetalleDet["IdTipoArticulo"]);
                    drDetalle["IdArticulo"] = drDetalleDet["IdArticulo"].ToString();
                    drDetalle["Articulo"] = drDetalleDet["Articulo"].ToString();
                    drDetalle["Cantidad"] = Convert.ToDouble(drDetalleDet["Cantidad"]);
                    drDetalle["FlagAutomatico"] = Convert.ToBoolean(drDetalleDet["FlagAutomatico"]);
                    drDetalle["FlagActivo"] = Convert.ToBoolean(drDetalleDet["FlagActivo"]);
                    drDetalle["Nuevo"] = false;
                    tblHIDetalle.Rows.Add(drDetalle);
                }

                foreach (DataRow drTareaDet in tblHITareatmp.Rows)
                {
                    DataRow drTarea = tblHITarea.NewRow();
                    drTarea["IdHITarea"] = Convert.ToInt32(drTareaDet["IdHITarea"]);
                    drTarea["IdHICompActividad"] = Convert.ToInt32(drTareaDet["IdHICompActividad"]);
                    drTarea["IdTarea"] = Convert.ToInt32(drTareaDet["IdTarea"]);
                    drTarea["Tarea"] = drTareaDet["Tarea"].ToString();
                    drTarea["HorasHombre"] = Convert.ToDouble(drTareaDet["HorasHombre"]);
                    drTarea["FlagAutomatico"] = Convert.ToBoolean(drTareaDet["FlagAutomatico"]);
                    drTarea["FlagActivo"] = Convert.ToBoolean(drTareaDet["FlagActivo"]);
                    drTarea["Nuevo"] = false;
                    tblHITarea.Rows.Add(drTarea);
                }

                dtgListarTrabajador.ItemsSource = tblHIHorasDetalle;

                foreach (DataRow drHIComp in HICompDet.Rows)
                {
                    DataRow[] drPFComp = tblPerfilComp.Select("IdPerfilComp = " + drHIComp["IdPerfilComp"].ToString());
                    if (drPFComp.Length > 0)
                    {
                        drPFComp[0]["IdHRComp"] = Convert.ToInt32(drHIComp["IdHRComp"]);
                        drPFComp[0]["FlagSolicitaInspeccion"] = drHIComp["FlagSolicitaInspeccion"].ToString();
                        drPFComp[0]["PerfilComp"] = drPFComp[0]["PerfilComp"].ToString() + " ✓";
                    }
                    
                    DataRow drNewHIComp = tblHIComp.NewRow();
                    drNewHIComp["IdHIComp"] = Convert.ToInt32(drHIComp["IdHIComp"]);
                    drNewHIComp["IdUCComp"] = Convert.ToInt32(drHIComp["IdUCComp"]);
                    drNewHIComp["IdHRComp"] = Convert.ToInt32(drHIComp["IdHRComp"]);
                    drNewHIComp["IdPerfilComp"] = Convert.ToInt32(drHIComp["IdPerfilComp"]);
                    drNewHIComp["IdHI"] = Convert.ToInt32(drHIComp["IdHI"]);
                    drNewHIComp["FlagSolicitaInspeccion"] = Convert.ToBoolean(drHIComp["FlagSolicitaInspeccion"]);
                    drNewHIComp["FlagActivo"] = true;
                    drNewHIComp["Nuevo"] = false;
                    tblHIComp.Rows.Add(drNewHIComp);
                }

                if (tblHIHorasDetalle.Rows.Count > 0)
                {
                    lblCantHoras.Content = tblHIHorasDetalle.Compute("SUM(HorasReal)", "").ToString();
                }
            }
            if (tblHIComp_Actividad.Rows.Count != 0)
            {
                gintIdHICompActividad = Convert.ToInt32(tblHIComp_Actividad.Compute("MAX(IdHICompActividad)", "")) + 1;
            }

            Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblPerfilComp;
            trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(1000, null);
        }

        private void cboUnidadControl_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gintIdPerfil == 0)
                {
                    gintIdPerfil = Convert.ToInt32(tblUCList.Select("IdUC = " + cboUnidadControl.EditValue.ToString())[0]["IdPerfil"]);
                    cboPerfil.ItemsSource = objB_Perfil.Perfil_List();
                    cboPerfil.DisplayMember = "Perfil";
                    cboPerfil.ValueMember = "IdPerfil";
                    cboPerfil.EditValue = gintIdPerfil;
                    LimpiarCambioDeUC();
                    CargarArbolComponentes(gintIdPerfil);
                    cboPlanMantenimiento.ItemsSource = null;
                    rbnPlanMantenimiento.IsChecked = false;
                    rbnPerfil.IsChecked = false;
                    rbnManual.IsChecked = false;
                }
                else
                {

                    if (tblHIComp.Rows.Count > 0 || tblHIComp_Actividad.Rows.Count > 0 || tblHIDetalle.Rows.Count > 0 || tblHIHorasDetalle.Rows.Count > 0 || tblHITarea.Rows.Count > 0)
                    {
                        var rs = DevExpress.Xpf.Core.DXMessageBox.Show("Al cambiar de unidad de control, se perderán los datos ingresados.\n¿Desea continuar?", "Sistema MMTO", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (rs == MessageBoxResult.No) { return; }
                        gintIdPerfil = 0;
                        LimpiarCambioDeUC();
                        cboUnidadControl_SelectedIndexChanged(sender, e);
                    }
                    else
                    {
                        gintIdPerfil = 0;
                        LimpiarCambioDeUC();
                        cboUnidadControl_SelectedIndexChanged(sender, e);
                    }
                }
                LlenarComboHR();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LimpiarCambioDeUC()
        {
            tblHIComp_Actividad.Rows.Clear();
            tblHIComp.Rows.Clear();
            tblHITarea.Rows.Clear();
            tblHIHorasDetalle.Rows.Clear();
            tblHIDetalle.Rows.Clear();

            gintIdHIComp = 1;
            gintIdHICompActividad = 1;
            gintIdHIDetalle = 1;
            gintIdHITarea = 1;
            gintIdHIHorasDetalle = 1;
            chkReqOT.IsChecked = false;
            dtvACtividades = new DataView();
            dtgActividades.ItemsSource = null;

            dtgActividades.ItemsSource = null;
            dtgConsumibles.ItemsSource = null;
            dtgHerramientas.ItemsSource = null;
            dtgRepuestos.ItemsSource = null;
            dtgTareas.ItemsSource = null;
            dtgListarTrabajador.ItemsSource = null;

        }
        private void rbnPlanMantenimiento_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                dtvACtividades = new DataView();
                cboPlanMantenimiento.IsEnabled = true;
                objE_PM = new E_PM();
                objE_PM.IdPerfil = gintIdPerfil;
                cboPlanMantenimiento.ItemsSource = objB_PM.PM_CombobyPerfil(objE_PM);
                cboPlanMantenimiento.DisplayMember = "PM";
                cboPlanMantenimiento.ValueMember = "IdPM";
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void rbnPerfil_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                cboPlanMantenimiento.SelectedIndexChanged -= new RoutedEventHandler(cboPlanMantenimiento_SelectedIndexChanged);
                cboPlanMantenimiento.EditValue = null;
                cboPlanMantenimiento.ItemsSource = null;
                cboPlanMantenimiento.IsEnabled = false;

                dtvACtividades = new DataView();
                tblTareasDatos = new DataTable();
                tblDetallesDatos = new DataTable();

                objE_Perfilcomp = new E_PerfilComp();
                objE_Perfilcomp.Idperfil = gintIdPerfil;
                objE_PerfilTarea.IdPerfil = gintIdPerfil;
                DataTable tblActividadDatos = objB_Perfilcomp.Actividad_ComboByPerfil(objE_Perfilcomp);
                tblTareasDatos = objB_PerfilTarea.PerfilTarea_List(objE_PerfilTarea);
                tblDetallesDatos = objB_Perfilcomp.PerfilDetalle_ComboByPerfil(objE_Perfilcomp);

                DataColumn IsChecked = new DataColumn()
                {
                    ColumnName = "IsChecked",
                    DefaultValue = false
                };

                tblActividadDatos.Columns.Add(IsChecked);
                dtvACtividades = tblActividadDatos.DefaultView;

                //Actualizando Nombres | IdTipoArticulo = 2 -> Repuestos
                foreach (DataRow drDetallesRep in tblDetallesDatos.Select("IdTipoArticulo = 2"))
                {

                    foreach (var drArt in lstRepuestosSAP.Where(emp => emp.CodigoArticulo == drDetallesRep["IdArticulo"].ToString()))
                    {
                        drDetallesRep["Articulo"] = drArt.DescripcionArticulo;
                        break;
                    }
                }
                //Actualizando Nombres | IdTipoArticulo = 3 -> Consumibles
                foreach (DataRow drDetallesCon in tblDetallesDatos.Select("IdTipoArticulo = 3"))
                {
                    foreach (var drArt in lstConsumiblesSAP.Where(emp => emp.CodigoArticulo == drDetallesCon["IdArticulo"].ToString()))
                    {
                        drDetallesCon["Articulo"] = drArt.DescripcionArticulo;
                        break;
                    }
                }

                cboPlanMantenimiento.SelectedIndexChanged += new RoutedEventHandler(cboPlanMantenimiento_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void rbnManual_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tblActividadDatos = objB_Actividad.Actividad_Combo();
                DataColumn IsChecked = new DataColumn()
                {
                    ColumnName = "IsChecked",
                    DefaultValue = false
                };

                tblActividadDatos.Columns.Add(IsChecked);
                dtvACtividades = tblActividadDatos.DefaultView;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboPlanMantenimiento_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                objE_PM = new E_PM();
                objE_PM.IdPM = Convert.ToInt32(cboPlanMantenimiento.EditValue);
                DataTable tblActividades = objB_PM.PMComp_Actividad_List(objE_PM);
                tblActividades.DefaultView.RowFilter = "IdEstadoPMA = 1";
                tblActividades = tblActividades.DefaultView.ToTable(true);

                DataColumn IsChecked = new DataColumn()
                {
                    ColumnName = "IsChecked",
                    DefaultValue = false
                };
                tblActividades.Columns.Add(IsChecked);
                dtvACtividades = tblActividades.DefaultView;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (gbolAllActividades)
                {
                    for (int i = 0; i < dtvACtividades.Count; i++)
                    {
                        dtvACtividades[i]["IsChecked"] = gbolAllActividades;
                    }
                    gbolAllActividades = false;
                }
                else if (!gbolAllActividades)
                {
                    for (int i = 0; i < dtvACtividades.Count; i++)
                    {
                        dtvACtividades[i]["IsChecked"] = gbolAllActividades;
                    }
                    gbolAllActividades = true;
                }

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAgregarActividad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gbolAllActividades = true;
                foreach (DataRow drActividad in dtvACtividades.ToTable().Select("IsChecked = true"))
                {
                    DataRow dr = tblHIComp_Actividad.NewRow();
                    dr["IdHICompActividad"] = gintIdHICompActividad;
                    dr["IdHIComp"] = 0;
                    dr["IdPerfilComp"] = gintIdPerfilComp;
                    dr["IdActividad"] = Convert.ToInt32(drActividad["IdActividad"]);
                    dr["Actividad"] = drActividad["Actividad"].ToString();
                    dr["FlagUso"] = ((bool)rbnPerfil.IsChecked) ? Convert.ToBoolean(drActividad["FlagUso"]) : false;
                    dr["FlagActivo"] = true;
                    dr["Nuevo"] = true;
                    tblHIComp_Actividad.Rows.Add(dr);

                    if ((bool)rbnPerfil.IsChecked)
                    {
                        foreach (DataRow drTareaDatos in tblTareasDatos.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActividad["IdPerfilCompActividad"])))
                        {
                            DataRow drTarea = tblHITarea.NewRow();
                            drTarea["IdHITarea"] = gintIdHITarea;
                            drTarea["IdHICompActividad"] = gintIdHICompActividad;
                            drTarea["IdTarea"] = Convert.ToInt32(drTareaDatos["IdTarea"]);
                            drTarea["Tarea"] = drTareaDatos["Tarea"].ToString();
                            drTarea["HorasHombre"] = Convert.ToDouble(drTareaDatos["HorasHombre"]);
                            drTarea["FlagAutomatico"] = true;
                            drTarea["FlagActivo"] = true;
                            drTarea["Nuevo"] = true;
                            tblHITarea.Rows.Add(drTarea);
                            gintIdHITarea++;
                        }
                        foreach (DataRow drDetalleDatos in tblDetallesDatos.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActividad["IdPerfilCompActividad"])))
                        {
                            DataRow drDetalle = tblHIDetalle.NewRow();
                            drDetalle["IdHIDetalle"] = gintIdHIDetalle;
                            drDetalle["IdHICompActividad"] = gintIdHICompActividad;
                            drDetalle["IdTipoArticulo"] = Convert.ToInt32(drDetalleDatos["IdTipoArticulo"]);
                            drDetalle["IdArticulo"] = drDetalleDatos["IdArticulo"].ToString();
                            drDetalle["Articulo"] = drDetalleDatos["Articulo"].ToString();
                            drDetalle["Cantidad"] = Convert.ToInt32(drDetalleDatos["Cantidad"]);
                            drDetalle["FlagAutomatico"] = true;
                            drDetalle["FlagActivo"] = true;
                            drDetalle["Nuevo"] = true;
                            tblHIDetalle.Rows.Add(drDetalle);
                            gintIdHIDetalle++;
                        }
                    }

                    gintIdHICompActividad++;
                    drActividad["IsChecked"] = false;
                }
                EstadoForm(false, true, false);
                lstbActividad.ItemsSource = tblHIComp_Actividad.DefaultView;
                stkActividad.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelarActividad_Click(object sender, RoutedEventArgs e)
        {
            gbolAllActividades = true;
            stkActividad.Visibility = Visibility.Collapsed;
        }
        private void cboHR_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {

                trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);

                if (cboHR.EditValue != null)
                {
                    if ((tblHIComp.Rows.Count > 0 || tblHIComp_Actividad.Rows.Count > 0 || tblHIDetalle.Rows.Count > 0 || tblHIHorasDetalle.Rows.Count > 0 || tblHITarea.Rows.Count > 0) && gintIdPerfil != 0 && objE_HR.IdHR != Convert.ToInt32(cboHR.EditValue))
                    {
                        var rs = DevExpress.Xpf.Core.DXMessageBox.Show("Al asginar una hoja de requerimiento, se perderán los datos ingresados.\n¿Desea continuar?", "Sistema MMTO", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (rs == MessageBoxResult.No)
                        {
                            cboHR.EditValue = null;
                            return;
                        }
                        gintIdPerfil = 0;
                    }
                    objE_HR = new E_HR();
                    objE_HR.IdHR = Convert.ToInt32(cboHR.EditValue);
                    tblHRDetalle = objB_HR.HR_GetItem(objE_HR);
                    cboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
                    cboUnidadControl.EditValue = null;
                    cboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
                    cboUnidadControl.EditValue = Convert.ToInt32(tblHRDetalle.Rows[0]["IdUC"]);
                    cboUnidadControl.IsEnabled = false;
                    txtComentarios.Text = (gbolIsDetalles) ? tblDetalleHI.Rows[0]["Observacion"].ToString() : tblHRDetalle.Rows[0]["Observacion"].ToString();
                }
                else
                {
                    if (tblHIComp.Rows.Count > 0 || tblHIComp_Actividad.Rows.Count > 0 || tblHIDetalle.Rows.Count > 0 || tblHIHorasDetalle.Rows.Count > 0 || tblHITarea.Rows.Count > 0)
                    {
                        MessageBoxResult rs = DevExpress.Xpf.Core.DXMessageBox.Show("Al quitar de hoja de requerimiento, se perderán los datos ingresados\n¿Desea continuar?", "Sistema MMTO", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (rs == MessageBoxResult.No)
                        {
                            cboHR.EditValue = objE_HR.IdHR;
                            return;
                        }
                        objE_HR = new E_HR();
                        cboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
                        cboUnidadControl.EditValue = null;
                        cboPerfil.EditValue = null;
                        cboUnidadControl.IsEnabled = true;
                        LimpiarCambioDeUC();
                        txtComentarios.Text = "";
                        Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                        trvComp.ItemsSource = null;
                        cboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
                    }
                    else
                    {
                        objE_HR = new E_HR();
                        cboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
                        cboUnidadControl.EditValue = null;
                        cboPerfil.EditValue = null;
                        cboUnidadControl.IsEnabled = true;
                        LimpiarCambioDeUC();
                        txtComentarios.Text = "";
                        Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                        trvComp.ItemsSource = null;
                        cboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
                    }
                }
                trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);

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
                    tbDetalleHI.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "TAB1_CONS");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "BTNG_CONS");
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tbDetalleHI.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "TAB1_NUEV");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "BTNG_NUEV");
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tbDetalleHI.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "TAB1_EDIT");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "BTNG_EDIT");
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboEstado_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboResponsable_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtComentarios_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void chkReqOT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm == null) { gbolFlagRequiereOT = (bool)chkReqOT.IsChecked; return; }

                if (!(bool)chkReqOT.IsChecked)
                {
                    if (tblHIComp_Actividad.Rows.Count > 0 || tblHIDetalle.Rows.Count > 0 || tblHITarea.Rows.Count > 0 && gbolFlagRequiereOT)
                    {
                        chkReqOT.IsChecked = true;
                        var rs = DevExpress.Xpf.Core.DXMessageBox.Show("Al remover el check de requiere O/T, se borrarán los datos ingresados de todos los componentes.\n¿Desea continuar?", "Sistema MMTO", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (rs == MessageBoxResult.No) { gbolFlagRequiereOT = true; return; }

                        if (gbolIsDetalles)
                        {
                            foreach (DataRow drActivida in tblHIComp_Actividad.Select("Nuevo = false")) { drActivida["FlagActivo"] = false; }
                            foreach (DataRow drDetalle in tblHIDetalle.Select("Nuevo = false")) { drDetalle["FlagActivo"] = false; }
                            foreach (DataRow drTarea in tblHITarea.Select("Nuevo = false")) { drTarea["FlagActivo"] = false; }

                            foreach (DataRow drActivida in tblHIComp_Actividad.Select("Nuevo = true")) { drActivida.Delete(); }
                            foreach (DataRow drDetalle in tblHIDetalle.Select("Nuevo = true")) { drDetalle.Delete(); }
                            foreach (DataRow drTarea in tblHITarea.Select("Nuevo = true")) { drTarea.Delete(); }
                        }
                        else
                        {
                            tblHIComp_Actividad.Rows.Clear();
                            tblHIDetalle.Rows.Clear();
                            tblHITarea.Rows.Clear();
                        }

                        rbnPerfil.IsChecked = false;
                        rbnPlanMantenimiento.IsChecked = false;
                        rbnManual.IsChecked = false;
                        gbolFlagRequiereOT = false;
                    }
                    grbActividades.IsEnabled = false;
                    gbolFlagRequiereOT = false;
                    chkReqOT.IsChecked = false;
                }
                else
                {
                    grbActividades.IsEnabled = (trm.IdMenuPadre != 1000);
                    gbolFlagRequiereOT = true;
                }

                EstadoForm(false, true, false);

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAgregarTrabajador_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboTrabajador.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_SELE_TRAB"), 2);
                    return;
                }
                if (dtpFechaTarea.EditValue == null)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_TRAB_FECH"), 2);
                    return;
                }

                DateTime FechaInicial = Convert.ToDateTime(Convert.ToDateTime(dtpFechaTarea.EditValue).ToString("dd/MM/yyyy") + " " + Convert.ToDateTime(txthoraini.EditValue).ToString("HH:mm"));
                DateTime FechaFinal = Convert.ToDateTime(Convert.ToDateTime(dtpFechaTarea.EditValue).ToString("dd/MM/yyyy") + " " + Convert.ToDateTime(txthorafin.EditValue).ToString("HH:mm"));
                TimeSpan horas = (FechaFinal - FechaInicial);

                if (DateTime.Compare(FechaInicial, Convert.ToDateTime(dtpFechaHI.EditValue)) < 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_TRAB_FECH"), 2);
                    return;
                }

                if (FechaFinal < FechaInicial)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_TRAB_HORA"), 2);
                    return;
                }
                //Validar si existe un trabajador en dentro de las horas
                foreach (DataRow drHorasDet in tblHIHorasDetalle.Select("CodResponsable = " + cboTrabajador.EditValue.ToString()))
                {
                    DateTime dt = Convert.ToDateTime(drHorasDet["FechaInicial"].ToString());
                    DateTime dt2 = Convert.ToDateTime(drHorasDet["FechaFinal"].ToString());
                    if ((FechaInicial >= dt && FechaInicial < dt2))
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_TRAB_RANG"), 2);
                        return;
                    }
                    if (FechaFinal > dt && FechaFinal < dt2)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_TRAB_RANG"), 2);
                        return;
                    }
                    if ((FechaInicial < dt && FechaFinal > dt))
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_TRAB_RANG"), 2);
                        return;
                    }
                }

                DataRow drHorasDetalle = tblHIHorasDetalle.NewRow();
                drHorasDetalle["IdHIHorasDetalle"] = gintIdHIHorasDetalle;
                drHorasDetalle["IdHI"] = 0;
                drHorasDetalle["CodResponsable"] = cboTrabajador.EditValue.ToString();
                drHorasDetalle["Responsable"] = cboTrabajador.Text;
                foreach (var Empleado in lstEmpleadosSAP.Where(emp => emp.CodigoPersona == Convert.ToInt32(cboTrabajador.EditValue)))
                {
                    drHorasDetalle["CostoHoraHombre"] = Empleado.CostoHoraHombre;
                }
                drHorasDetalle["FechaInicial"] = FechaInicial;
                drHorasDetalle["FechaFinal"] = FechaFinal;
                drHorasDetalle["HorasReal"] = Math.Round(horas.TotalHours, 2);
                drHorasDetalle["FlagActivo"] = true;
                drHorasDetalle["Nuevo"] = true;
                tblHIHorasDetalle.Rows.Add(drHorasDetalle);
                gintIdHIHorasDetalle++;

                lblCantHoras.Content = tblHIHorasDetalle.Compute("SUM(HorasReal)", "").ToString();

                cboTrabajador.EditValue = null;
                dtpFechaTarea.EditValue = null;
                txthoraini.EditValue = "00:00";
                txthorafin.EditValue = "00:00";
                EstadoForm(false, true, false);

                tblHIHorasDetalle.DefaultView.RowFilter = "FlagActivo = 1";
                dtgListarTrabajador.ItemsSource = tblHIHorasDetalle.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelarTrabajador_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cboTrabajador.EditValue = null;
                dtpFechaTarea.EditValue = null;
                txthoraini.EditValue = "00:00";
                txthorafin.EditValue = "00:00";
                stkTiempoDedicado.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEliminarTrabajador_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                if (!gbolFlagRequiereOT) { return; }
                DataRowView dr = (dtgListarTrabajador.SelectedItem) as DataRowView;

                foreach (DataRow drTareaDelete in tblHIHorasDetalle.Select("IdHIHorasDetalle =" + Convert.ToInt32(dr.Row["IdHIHorasDetalle"])))
                {
                    if (Convert.ToBoolean(drTareaDelete["Nuevo"]))
                    {
                        drTareaDelete.Delete();
                    }
                    else
                    {
                        drTareaDelete["FlagActivo"] = false;
                    }
                }

                lblCantHoras.Content = (tblHIHorasDetalle.Rows.Count > 0) ? tblHIHorasDetalle.Compute("SUM(HorasReal)", "").ToString() : "0";
                tblHIHorasDetalle.DefaultView.RowFilter = "FlagActivo = 1";
                dtgListarTrabajador.ItemsSource = tblHIHorasDetalle.DefaultView.ToTable();
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void lstbActividad_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                int IdHICompActividad = Convert.ToInt32(lstbActividad.EditValue);

                tblHITarea.DefaultView.RowFilter = "FlagActivo = True AND IdHICompActividad = " + IdHICompActividad;
                dtgTareas.ItemsSource = tblHITarea.DefaultView.ToTable();

                DataView dtvHerramientas = tblHIDetalle.Copy().DefaultView;
                DataView dtvConsumibles = tblHIDetalle.Copy().DefaultView;
                DataView dtvRepuestos = tblHIDetalle.Copy().DefaultView;

                dtvHerramientas.RowFilter = "FlagActivo = True AND IdTipoArticulo = 1 AND IdHICompActividad = " + IdHICompActividad;
                dtvConsumibles.RowFilter = "FlagActivo = True AND IdTipoArticulo = 3 AND IdHICompActividad = " + IdHICompActividad;
                dtvRepuestos.RowFilter = "FlagActivo = True AND IdTipoArticulo = 2 AND IdHICompActividad = " + IdHICompActividad;

                dtgHerramientas.ItemsSource = dtvHerramientas.ToTable();
                dtgConsumibles.ItemsSource = dtvConsumibles.ToTable();
                dtgRepuestos.ItemsSource = dtvRepuestos.ToTable();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEliminarTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                if (!gbolFlagRequiereOT) { return; }
                DataRowView dr = (dtgTareas.SelectedItem) as DataRowView;
                if (Convert.ToBoolean(dr.Row["FlagAutomatico"])) { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_DELE_TARE_AUTO"), 2); return; }
                foreach (DataRow drTareaDelete in tblHITarea.Select("IdHITarea =" + Convert.ToInt32(dr.Row["IdHITarea"])))
                {
                    if (Convert.ToBoolean(drTareaDelete["Nuevo"]))
                    {
                        drTareaDelete.Delete();
                    }
                    else
                    {
                        drTareaDelete["FlagActivo"] = false;
                    }
                }
                int IdHICompActividad = Convert.ToInt32(lstbActividad.EditValue);
                tblHITarea.DefaultView.RowFilter = "FlagActivo = True AND IdHICompActividad = " + IdHICompActividad;
                dtgTareas.ItemsSource = tblHITarea.DefaultView.ToTable();
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEliminarHerrEsp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                if (!gbolFlagRequiereOT) { return; }
                DataRowView dr = (dtgHerramientas.SelectedItem) as DataRowView;
                if (Convert.ToBoolean(dr.Row["FlagAutomatico"])) { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_DELE_HERR_AUTO"), 2); return; }
                foreach (DataRow drHerramientaDelete in tblHIDetalle.Select("IdHIDetalle =" + Convert.ToInt32(dr.Row["IdHIDetalle"])))
                {
                    if (Convert.ToBoolean(drHerramientaDelete["Nuevo"]))
                    {
                        drHerramientaDelete.Delete();
                    }
                    else
                    {
                        drHerramientaDelete["FlagActivo"] = false;
                    }
                }
                DataView dtvHerramientas = tblHIDetalle.Copy().DefaultView;
                dtvHerramientas.RowFilter = "FlagActivo = True AND IdTipoArticulo = 1 AND IdHICompActividad = " + Convert.ToInt32(lstbActividad.EditValue);
                dtgHerramientas.ItemsSource = dtvHerramientas.ToTable();
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEliminarRep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                if (!gbolFlagRequiereOT) { return; }
                DataRowView dr = (dtgRepuestos.SelectedItem) as DataRowView;
                if (Convert.ToBoolean(dr.Row["FlagAutomatico"])) { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_DELE_REPU_AUTO"), 2); return; }
                foreach (DataRow drRepuestoDelete in tblHIDetalle.Select("IdHIDetalle =" + Convert.ToInt32(dr.Row["IdHIDetalle"])))
                {
                    if (Convert.ToBoolean(drRepuestoDelete["Nuevo"]))
                    {
                        drRepuestoDelete.Delete();
                    }
                    else
                    {
                        drRepuestoDelete["FlagActivo"] = false;
                    }
                }
                DataView dtvRepuestos = tblHIDetalle.Copy().DefaultView;
                dtvRepuestos.RowFilter = "FlagActivo = True AND IdTipoArticulo = 2 AND IdHICompActividad = " + Convert.ToInt32(lstbActividad.EditValue);
                dtgRepuestos.ItemsSource = dtvRepuestos.ToTable();
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEliminarCon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                if (!gbolFlagRequiereOT) { return; }
                DataRowView dr = (dtgConsumibles.SelectedItem) as DataRowView;
                if (Convert.ToBoolean(dr.Row["FlagAutomatico"])) { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "LOGI_DELE_CONS_AUTO"), 2); return; }
                foreach (DataRow drConsumibleDelete in tblHIDetalle.Select("IdHIDetalle =" + Convert.ToInt32(dr.Row["IdHIDetalle"])))
                {
                    if (Convert.ToBoolean(drConsumibleDelete["Nuevo"]))
                    {
                        drConsumibleDelete.Delete();
                    }
                    else
                    {
                        drConsumibleDelete["FlagActivo"] = false;
                    }
                }
                DataView dtvConsumibles = tblHIDetalle.Copy().DefaultView;
                dtvConsumibles.RowFilter = "FlagActivo = True AND IdTipoArticulo = 3 AND IdHICompActividad = " + Convert.ToInt32(lstbActividad.EditValue);
                dtgConsumibles.ItemsSource = dtvConsumibles.ToTable();
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAgregarTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Agregar Tareas
                if (cboTarea.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_TARE_SELE"), 2);
                    cboTarea.Focus();
                    return;
                }
                else if (txtHorasHombre.Text == "")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_TARE_HORA"), 2);
                    txtHorasHombre.Focus();
                    return;
                }
                else if (!txtHorasHombre.Text.Contains(":"))
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_TARE_HORA"), 2);
                    txtHorasHombre.Focus();
                    return;
                }
                else if (txtHorasHombre.Text.Split(':')[1].Trim() == "")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_TARE_HORA"), 2);
                    txtHorasHombre.Focus();
                    return;
                }
                else if (txtHorasHombre.Text == "00:00")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_TARE_HORA"), 2);
                    txtHorasHombre.Focus();
                    return;
                }
                string[] Horas = txtHorasHombre.Text.Split(':');
                Horas[1] = (Horas[1].Length == 1) ? Horas[1] + "0" : Horas[1];
                double hrest = Convert.ToDouble(Horas[0]) + (Convert.ToDouble(Horas[1]) / 60);

                DataRow drTarea = tblHITarea.NewRow();
                drTarea["IdHITarea"] = gintIdHITarea;
                drTarea["IdHICompActividad"] = Convert.ToInt32(lstbActividad.EditValue);
                drTarea["IdTarea"] = Convert.ToInt32(cboTarea.EditValue);
                drTarea["Tarea"] = cboTarea.Text;
                drTarea["HorasHombre"] = hrest;
                drTarea["FlagAutomatico"] = false;
                drTarea["FlagActivo"] = true;
                drTarea["Nuevo"] = true;
                tblHITarea.Rows.Add(drTarea);
                gintIdHITarea++;

                tblHITarea.DefaultView.RowFilter = "IdHICompActividad = " + Convert.ToInt32(lstbActividad.EditValue);
                dtgTareas.ItemsSource = tblHITarea.DefaultView.ToTable();

                txtHorasHombre.EditValue = "00:00";
                cboTarea.SelectedIndex = -1;
                EstadoForm(false, true, false);
                stkPanelTareas.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelarTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Cancelar Tareas
                txtHorasHombre.EditValue = "00:00";
                cboTarea.SelectedIndex = -1;
                stkPanelTareas.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAceptarHerramientaEspecial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cboHerramientaEspecial.SelectedIndexChanged -= new RoutedEventHandler(cboHerramientaEspecial_SelectedIndexChanged);
                //Agregar Herramienta
                if (cboHerramientaEspecial.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_HERR_SELE"), 2);
                    cboHerramientaEspecial.Focus();
                    return;
                }
                if (spCantidadHerramienta.Value == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_HERR_CANT"), 2);
                    spCantidadHerramienta.Focus();
                    return;
                }

                DataRow drHerramienta = tblHIDetalle.NewRow();
                drHerramienta["IdHIDetalle"] = gintIdHIDetalle;
                drHerramienta["IdHICompActividad"] = Convert.ToInt32(lstbActividad.EditValue);
                drHerramienta["IdTipoArticulo"] = 1;
                drHerramienta["IdArticulo"] = cboHerramientaEspecial.EditValue.ToString();
                drHerramienta["Articulo"] = cboHerramientaEspecial.Text;
                drHerramienta["Cantidad"] = Convert.ToInt32(spCantidadHerramienta.Value);
                drHerramienta["FlagAutomatico"] = false;
                drHerramienta["FlagActivo"] = true;
                drHerramienta["Nuevo"] = true;
                tblHIDetalle.Rows.Add(drHerramienta);
                gintIdHIDetalle++;

                DataView dtvHerramientas = tblHIDetalle.Copy().DefaultView;
                dtvHerramientas.RowFilter = "IdTipoArticulo = 1 AND IdHICompActividad = " + Convert.ToInt32(lstbActividad.EditValue);
                dtgHerramientas.ItemsSource = dtvHerramientas.ToTable();

                EstadoForm(false, true, false);
                cboHerramientaEspecial.SelectedIndex = -1;
                spCantidadHerramienta.Value = 1;
                stkPanelHerramientas.Visibility = Visibility.Collapsed;
                cboHerramientaEspecial.SelectedIndexChanged += new RoutedEventHandler(cboHerramientaEspecial_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelarHerramientaEspecial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cboHerramientaEspecial.SelectedIndexChanged -= new RoutedEventHandler(cboHerramientaEspecial_SelectedIndexChanged);
                //Cerrar Herramienta
                cboHerramientaEspecial.SelectedIndex = -1;
                spCantidadHerramienta.Value = 1;
                stkPanelHerramientas.Visibility = Visibility.Collapsed;
                cboHerramientaEspecial.SelectedIndexChanged += new RoutedEventHandler(cboHerramientaEspecial_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAceptarRepusto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Agregar Repuesto
                if (cboRepuesto.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_REPU_SELE"), 2);
                    return;
                }
                if (spCantidadRepuesto.Value == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_REPU_CANT"), 2);
                    return;
                }

                DataRow drRepuesto = tblHIDetalle.NewRow();
                drRepuesto["IdHIDetalle"] = gintIdHIDetalle;
                drRepuesto["IdHICompActividad"] = Convert.ToInt32(lstbActividad.EditValue);
                drRepuesto["IdTipoArticulo"] = 2;
                drRepuesto["IdArticulo"] = cboRepuesto.EditValue.ToString();
                drRepuesto["Articulo"] = cboRepuesto.Text;
                drRepuesto["Cantidad"] = Convert.ToInt32(spCantidadRepuesto.Value);
                drRepuesto["FlagAutomatico"] = false;
                drRepuesto["FlagActivo"] = true;
                drRepuesto["Nuevo"] = true;
                tblHIDetalle.Rows.Add(drRepuesto);
                gintIdHIDetalle++;

                DataView dtvRepuestos = tblHIDetalle.Copy().DefaultView;
                dtvRepuestos.RowFilter = "IdTipoArticulo = 2 AND IdHICompActividad = " + Convert.ToInt32(lstbActividad.EditValue);
                dtgRepuestos.ItemsSource = dtvRepuestos.ToTable();

                spCantidadRepuesto.Value = 1;
                cboRepuesto.SelectedIndex = -1;
                EstadoForm(false, true, false);
                stkPanelRepuestos.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelarRepuesto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Cancelar Repuesto

                spCantidadRepuesto.Value = 1;
                cboRepuesto.SelectedIndex = -1;
                stkPanelRepuestos.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAceptarConsumibles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Agregar Consumible
                if (cboConsumibles.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_CONS_SELE"), 2);
                    cboConsumibles.Focus();
                    return;
                }
                if (spCantidadConsumible.Value == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_CONS_CANT"), 2);
                    spCantidadConsumible.Focus();
                    return;
                }

                DataRow drConsumible = tblHIDetalle.NewRow();
                drConsumible["IdHIDetalle"] = gintIdHIDetalle;
                drConsumible["IdHICompActividad"] = Convert.ToInt32(lstbActividad.EditValue);
                drConsumible["IdTipoArticulo"] = 3;
                drConsumible["IdArticulo"] = cboConsumibles.EditValue.ToString();
                drConsumible["Articulo"] = cboConsumibles.Text;
                drConsumible["Cantidad"] = Convert.ToInt32(spCantidadConsumible.Value);
                drConsumible["FlagAutomatico"] = false;
                drConsumible["FlagActivo"] = true;
                drConsumible["Nuevo"] = true;
                tblHIDetalle.Rows.Add(drConsumible);
                gintIdHIDetalle++;

                DataView dtvConsumibles = tblHIDetalle.Copy().DefaultView;
                dtvConsumibles.RowFilter = "IdTipoArticulo = 3 AND IdHICompActividad = " + Convert.ToInt32(lstbActividad.EditValue);
                dtgConsumibles.ItemsSource = dtvConsumibles.ToTable();

                EstadoForm(false, true, false);
                cboConsumibles.SelectedIndex = -1;
                spCantidadConsumible.Value = 1;
                stkPanelConsumibles.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelarConsumibles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Cancelar Consumible
                cboConsumibles.SelectedIndex = -1;
                spCantidadConsumible.Value = 1;
                stkPanelConsumibles.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboHerramientaEspecial_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                spCantidadHerramienta.Value = 1;
                spCantidadHerramienta.MaxValue = Convert.ToInt32(tblComboHerramienta.Select("IdHerramienta = " + Convert.ToInt32(cboHerramientaEspecial.EditValue))[0]["Cantidad"]);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private bool ValidaCampoObligado()
        {
            bool bolRpta = false;
            try
            {
                DateTime FechaServidor = Convert.ToDateTime(Utilitarios.Utilitarios.Fecha_Hora_Servidor().Rows[0]["FechaServer"].ToString() + " " + Utilitarios.Utilitarios.Fecha_Hora_Servidor().Rows[0]["HoraServer"].ToString());

                if (cboUnidadControl.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_UC"), 2);
                    cboUnidadControl.Focus();
                }
                else if (dtpFechaHI.EditValue == null)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_FECH_REGI"), 2);
                    dtpFechaHI.Focus();
                }
                else if (DateTime.Compare(Convert.ToDateTime(dtpFechaHI.EditValue), FechaServidor) > 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_FECH_MAYO"), 2);
                    dtpFechaHI.Focus();
                }
                else if (cboHR.EditValue != null)
                {
                    if (DateTime.Compare(Convert.ToDateTime(tblHRDetalle.Rows[0]["FechaHR"]), Convert.ToDateTime(dtpFechaHI.EditValue)) > 0)
                    {
                        bolRpta = true;
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_FECH_MENO"), 2);
                        dtpFechaHI.Focus();
                    }
                }
                else if (cboResponsable.EditValue == null)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_SOLI"), 2);
                    cboResponsable.Focus();
                }
                else if (cboResponsable.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_SOLI"), 2);
                    cboResponsable.Focus();
                }
                else if (dtpFechaInicio.EditValue == null)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_FECH_INIC"), 2);
                    dtpFechaInicio.Focus();
                }
                else if (dtpFechaFinal.EditValue == null)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_FECH_FINA"), 2);
                    dtpFechaFinal.Focus();
                }
                else if (Convert.ToDouble(txtKilomInicial.EditValue) <= 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_CONT_INIC"), 2);
                    txtKilomInicial.Focus();
                }
                else if (Convert.ToDouble(txtKilomFinal.EditValue) <= 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaInspeccion, "OBLI_CONT_FINA"), 2);
                    txtKilomFinal.Focus();
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

        private void dtgHI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgHI.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodHI")
                    {
                        gintIdHI = Convert.ToInt32(dtgHI.GetFocusedRowCellValue("IdHI"));
                        LlenarDetallesHI();
                        GlobalClass.ip.SeleccionarTab(tbDetalleHI);
                        EstadoForm(false, false, true);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        
        private void LlenarDetallesHI()
        {
            try
            {
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboResponsable.SelectedIndexChanged -= new RoutedEventHandler(cboResponsable_SelectedIndexChanged);
                txtComentarios.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtComentarios_EditValueChanged);
                dtpFechaHI.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpFechaHI_EditValueChanged);

                gbolIsDetalles = true;
                objE_HI.IdHI = gintIdHI;
                tblDetalleHI = objB_HI.HI_GetItem(objE_HI);
                LbLHojaInspeccion.Content = tblDetalleHI.Rows[0]["CodHI"].ToString();
                cboEstado.EditValue = Convert.ToInt32(tblDetalleHI.Rows[0]["IdEstadoHI"]);
                dtpFechaHI.EditValue = Convert.ToDateTime(tblDetalleHI.Rows[0]["FechaInspeccion"]);
                chkProgramado.IsChecked = Convert.ToBoolean(tblDetalleHI.Rows[0]["FlagProgramado"]);
                lblNroOT.Content = tblDetalleHI.Rows[0]["CodOT"].ToString();
                chkReqOT.IsChecked = Convert.ToBoolean(tblDetalleHI.Rows[0]["FlagRequiereOT"]);
                cboResponsable.EditValue = Convert.ToInt32(tblDetalleHI.Rows[0]["CodResponsableSAP"]);
                dtpFechaInicio.EditValue = Convert.ToDateTime(tblDetalleHI.Rows[0]["FechaInicial"]);
                dtpFechaFinal.EditValue = Convert.ToDateTime(tblDetalleHI.Rows[0]["FechaFinal"]);
                txtKilomInicial.EditValue = Convert.ToDouble(tblDetalleHI.Rows[0]["KmInicial"]);
                txtKilomFinal.EditValue = Convert.ToDouble(tblDetalleHI.Rows[0]["KmFinal"]);
                txtComentarios.EditValue = tblDetalleHI.Rows[0]["Observacion"].ToString();

                lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblDetalleHI.Rows[0]["UsuarioCreacion"].ToString(), tblDetalleHI.Rows[0]["FechaCreacion"].ToString(), tblDetalleHI.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblDetalleHI.Rows[0]["UsuarioModificacion"].ToString(), tblDetalleHI.Rows[0]["FechaModificacion"].ToString(), tblDetalleHI.Rows[0]["HostModificacion"].ToString());

                if (Convert.ToInt32(tblDetalleHI.Rows[0]["IdHR"]) == 0)
                {
                    cboUnidadControl.EditValue = Convert.ToInt32(tblDetalleHI.Rows[0]["IdUC"]);
                }
                else
                {
                    objE_HR.IdEstadoHR = 0;
                    cboHR.ItemsSource = objB_HR.HR_Combo(objE_HR);
                    cboHR.EditValue = Convert.ToInt32(tblDetalleHI.Rows[0]["IdHR"]);
                }

                cboHR.IsEnabled = false;
                cboUnidadControl.IsEnabled = false;
                dtpFechaHI.IsReadOnly = true;
                dtpFechaInicio.IsReadOnly = true;
                dtpFechaFinal.IsReadOnly = true;
                txtKilomInicial.IsReadOnly = true;
                txtKilomFinal.IsReadOnly = true;

                if ((bool)chkReqOT.IsChecked)
                {
                    //grbActividades.IsEnabled = true;
                    gbolFlagRequiereOT = true;
                }
                else
                {
                    //grbActividades.IsEnabled = false;
                    gbolFlagRequiereOT = false;
                }

                if (Convert.ToInt32(cboEstado.EditValue) != 1)
                {
                    BloquearControlesPorEstado(true);
                }

                if (Convert.ToInt32(cboEstado.EditValue) == 3 || Convert.ToInt32(cboEstado.EditValue) == 4)
                {
                    dtvEstadosHI.RowFilter = "";
                    cboEstado.IsEnabled = false;
                }
                else
                {
                    dtvEstadosHI.RowFilter = "IdColumna <> 4";
                }
                FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                dtpFechaHI.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpFechaHI_EditValueChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboResponsable.SelectedIndexChanged += new RoutedEventHandler(cboResponsable_SelectedIndexChanged);
                txtComentarios.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtComentarios_EditValueChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BloquearControlesPorEstado(Boolean Valor)
        {
            cboHR.IsEnabled = !Valor;
            cboUnidadControl.IsEnabled = !Valor;
            dtpFechaHI.IsReadOnly = Valor;
            chkReqOT.IsEnabled = !Valor;
            cboResponsable.IsEnabled = !Valor;
            dtpFechaInicio.IsReadOnly = Valor;
            dtpFechaFinal.IsReadOnly = Valor;
            txtKilomInicial.IsReadOnly = Valor;
            txtKilomFinal.IsReadOnly = Valor;
            txtComentarios.IsReadOnly = Valor;
            cboTrabajador.IsEnabled = !Valor;
            dtpFechaTarea.IsReadOnly = Valor;
            txthoraini.IsReadOnly = Valor;
            txthorafin.IsReadOnly = Valor;
            gbolFlagInactivo = Valor;
            btnAgregarTrabajador.IsEnabled = !Valor;
        }

        private void rbnPLANTILLA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListarHI();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void lstbActividad_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataRowView drActiExis = (DataRowView)lstbActividad.SelectedItem;
                if (e.Key != Key.Delete) { return; }
                var rpt = DevExpress.Xpf.Core.DXMessageBox.Show(string.Format("¿Seguro de eliminar la actividad: {0} ?", drActiExis["Actividad"].ToString()), "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rpt == MessageBoxResult.No) { return; }

                foreach (DataRow drActividad in tblHIComp_Actividad.Select("IdHICompActividad = " + Convert.ToInt32(drActiExis["IdHICompActividad"])))
                {
                    foreach (DataRow drTareaDatos in tblHITarea.Select("IdHICompActividad = " + Convert.ToInt32(drActiExis["IdHICompActividad"])))
                    {
                        if (Convert.ToBoolean(drTareaDatos["Nuevo"])) { drTareaDatos.Delete(); } else { drTareaDatos["FlagActivo"] = false; }
                    }
                    foreach (DataRow drDetalleDatos in tblHIDetalle.Select("IdHICompActividad = " + Convert.ToInt32(drActiExis["IdHICompActividad"])))
                    {
                        if (Convert.ToBoolean(drDetalleDatos["Nuevo"])) { drDetalleDatos.Delete(); } else { drDetalleDatos["FlagActivo"] = false; }
                    }

                    if (Convert.ToBoolean(drActividad["Nuevo"])) { drActividad.Delete(); } else { drActividad["FlagActivo"] = false; }
                }

                dtgConsumibles.ItemsSource = null;
                dtgHerramientas.ItemsSource = null;
                dtgRepuestos.ItemsSource = null;
                dtgTareas.ItemsSource = null;
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        

        private void LlenarComboHR()
        {
            try
            {
                if (gbolIsDetalles)
                {
                    objE_HR.IdEstadoHR = 0;
                    objE_HR.IdUC = Convert.ToInt32(cboUnidadControl.EditValue);
                    objE_HR.FechaHR = Convert.ToDateTime("01/01/1900");
                }
                else
                {
                    objE_HR.IdEstadoHR = 1;
                    objE_HR.IdUC = (cboUnidadControl.EditValue != null) ? Convert.ToInt32(cboUnidadControl.EditValue) : 0;
                    objE_HR.FechaHR = (dtpFechaHI.EditValue != null) ? Convert.ToDateTime(dtpFechaHI.EditValue) : Convert.ToDateTime("01/01/1900");
                }
                tblComboHR = objB_HR.HR_ComboByFilters(objE_HR);
                cboHR.ItemsSource = tblComboHR;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void cboHR_PopupOpening(object sender, DevExpress.Xpf.Editors.OpenPopupEventArgs e)
        {
            try
            {
                if (tblComboHR.Rows.Count <= 0)
                {
                    cboUnidadControl.IsEnabled = true;
                }
                else if (cboHR.EditValue != null)
                {
                    cboUnidadControl.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtpFechaHI_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                LlenarComboHR();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalClass.GeneraImpresion(gintIdMenu, gintIdHI);
            }
            catch { }
        }

    }


}
