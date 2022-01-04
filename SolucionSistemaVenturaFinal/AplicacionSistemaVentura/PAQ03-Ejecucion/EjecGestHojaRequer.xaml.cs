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
using Utilitarios;
using Entities;
using Business;

namespace AplicacionSistemaVentura.PAQ03_Ejecucion
{
    /// <summary>
    /// Interaction logic for EjecGestHojaRequer.xaml
    /// </summary>
    public partial class EjecGestHojaRequer : UserControl
    {
        public EjecGestHojaRequer()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        InterfazMTTO.iSBO_BE.BEOHEMList lstSolicitanteSAP = new InterfazMTTO.iSBO_BE.BEOHEMList();
        Utilitarios.ErrorHandler Error = new Utilitarios.ErrorHandler();

        B_Perfil objB_Perfil = new B_Perfil();
        B_PerfilComp objB_Perfilcomp = new B_PerfilComp();
        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        B_UCComp objB_UCComp = new B_UCComp();
        B_UC objB_UC = new B_UC();
        B_HR objB_HR = new B_HR();

        E_Perfil objE_Perfil = new E_Perfil();
        E_PerfilComp objE_Perfilcomp = new E_PerfilComp();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        E_UCComp objE_UCComp = new E_UCComp();
        E_TablaMaestra objTM = new E_TablaMaestra();
        E_UC objE_UC = new E_UC();
        E_HR objE_HR = new E_HR();

        DateTime FechaModificacion;

        Boolean gbolNuevo = false, gbolEdicion = false;
        int gintIdPerfil = 0;
        int gintIdHR = 0;
        DataTable tblUCList = new DataTable();
        DataTable HRComp = new DataTable();
        DataTable tblPFComponentes = new DataTable();
        DataView dtvMaestra = new DataView();
        DataView dtvEstadosHR = new DataView();

        string gstrEtiquetaHojaRequerimiento = "EjecGestHojaRequer";

        int gintIdMenu = 0;

        private void UserControl_Loaded()
        {
            try
            {
                txtInfoComplementaria.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtInfoComplementaria_EditValueChanged);
                txtObservacionFinal.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtObservacionFinal_EditValueChanged);
                dtpFecha.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpFecha_EditValueChanged);
                cboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
                cboSolicitante.SelectedIndexChanged -= new RoutedEventHandler(cboSolicitante_SelectedIndexChanged);
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                chkInspeccion.Unchecked -= new RoutedEventHandler(chkInspeccion_Unchecked);
                chkInspeccion.Checked -= new RoutedEventHandler(chkInspeccion_Checked);

                GlobalClass.ip.SeleccionarTab(tbListaHR);
                objTM.IdTabla = 0;
                dtvMaestra = B_TablaMaestra.TablaMaestra_Combo(objTM).DefaultView;

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                lstSolicitanteSAP = new InterfazMTTO.iSBO_BE.BEOHEMList();

                objE_UC.IdPerfil = 0;
                tblUCList = new DataTable();
                tblUCList = objB_UC.B_UC_Combo(objE_UC);
                cboUnidadControl.ItemsSource = tblUCList;
                cboUnidadControl.DisplayMember = "PlacaSerie";
                cboUnidadControl.ValueMember = "IdUC";

                lstSolicitanteSAP = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("S", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    cboSolicitante.ItemsSource = lstSolicitanteSAP;
                    cboSolicitante.DisplayMember = "NombrePersona";
                    cboSolicitante.ValueMember = "CodigoPersona";
                    cboSolicitante.SelectedIndex = -1;
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }

                dtvEstadosHR = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 15", dtvMaestra).DefaultView;
                dtvEstadosHR.RowFilter = "IdColumna <> 4";
                cboEstado.ItemsSource = dtvEstadosHR;
                cboEstado.DisplayMember = "Descripcion";
                cboEstado.ValueMember = "Valor";
                cboEstado.SelectedIndex = 0;

                rbnAbierto.IsChecked = true;
                ListarHR();

                #region VisualizacionBotonImprimir
                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                if (!VisualizaBotonImprimirDetalle)
                {
                    btnImprimir.Visibility = System.Windows.Visibility.Hidden;
                }
                #endregion

                txtInfoComplementaria.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtInfoComplementaria_EditValueChanged);
                txtObservacionFinal.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtObservacionFinal_EditValueChanged);
                dtpFecha.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpFecha_EditValueChanged);
                cboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
                cboSolicitante.SelectedIndexChanged += new RoutedEventHandler(cboSolicitante_SelectedIndexChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                chkInspeccion.Unchecked += new RoutedEventHandler(chkInspeccion_Unchecked);
                chkInspeccion.Checked += new RoutedEventHandler(chkInspeccion_Checked);

       
                #region RequerimientoCelsa
                cboPrioridad.SelectedIndexChanged -= new RoutedEventHandler(cboPrioridad_SelectedIndexChanged);
                DataTable tbl = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=62", dtvMaestra);
                tbl.DefaultView.RowFilter = "IdColumna <> 0";
                cboPrioridad.ItemsSource = tbl.DefaultView;
                cboPrioridad.DisplayMember = "Descripcion";
                cboPrioridad.ValueMember = "IdColumna";
                cboPrioridad.SelectedIndex = -1;
                cboPrioridad.SelectedIndexChanged += new RoutedEventHandler(cboPrioridad_SelectedIndexChanged);

                cboTipoRequerimiento.SelectedIndexChanged -= new RoutedEventHandler(cboTipoRequerimiento_SelectedIndexChanged);
                DataTable tblTipoReq = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=63", dtvMaestra);
                tblTipoReq.DefaultView.RowFilter = "IdColumna <> 0";
                cboTipoRequerimiento.ItemsSource = tblTipoReq.DefaultView;
                cboTipoRequerimiento.DisplayMember = "Descripcion";
                cboTipoRequerimiento.ValueMember = "IdColumna";
                cboTipoRequerimiento.SelectedIndex = -1;
                cboTipoRequerimiento.SelectedIndexChanged += new RoutedEventHandler(cboTipoRequerimiento_SelectedIndexChanged);


                #endregion

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarHR()
        {
            if ((bool)rbnTodos.IsChecked)
            {
                objE_HR.IdEstadoHR = 0;
            }
            else if ((bool)rbnAbierto.IsChecked)
            {
                objE_HR.IdEstadoHR = 1;
            }
            else if ((bool)rbnAtendido.IsChecked)
            {
                objE_HR.IdEstadoHR = 4;
            }
            else if ((bool)rbnCancelado.IsChecked)
            {
                objE_HR.IdEstadoHR = 2;
            }
            else if ((bool)rbnCerrado.IsChecked)
            {
                objE_HR.IdEstadoHR = 3;
            }
            dtgHR.ItemsSource = objB_HR.HR_List(objE_HR);
        }

        private void trvComp_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (trvComp.ItemsSource != null)
                {
                    txtInfoComplementaria.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtInfoComplementaria_EditValueChanged);
                    chkInspeccion.Unchecked -= new RoutedEventHandler(chkInspeccion_Unchecked);
                    chkInspeccion.Checked -= new RoutedEventHandler(chkInspeccion_Checked);

                    TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                    if (trm != null)
                    {

                        DataRow[] drComponente = tblPFComponentes.Select("IdPerfilComp = " + trm.IdMenu);
                        txtInfoComplementaria.Text = drComponente[0]["Observacion"].ToString();
                        chkInspeccion.IsChecked = Convert.ToBoolean(drComponente[0]["FlagSolicitaInspeccion"]);
                        grbDetalles.IsEnabled = true;
                        if (trm.IdMenuPadre == 1000) { grbDetalles.IsEnabled = false; }
                    }

                    txtInfoComplementaria.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtInfoComplementaria_EditValueChanged);
                    chkInspeccion.Unchecked += new RoutedEventHandler(chkInspeccion_Unchecked);
                    chkInspeccion.Checked += new RoutedEventHandler(chkInspeccion_Checked);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalClass.ip.SeleccionarTab(tbListaHR);
                LimpiarControles();
                ListarHR();
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
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "OBLI_UC"), 2);
                    cboUnidadControl.Focus();
                }
                else if (dtpFecha.EditValue == null)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "OBLI_FECH_REGI"), 2);
                    dtpFecha.Focus();
                }
                else if (DateTime.Compare(Convert.ToDateTime(dtpFecha.EditValue), FechaServidor) > 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "OBLI_FECH_MAYO"), 2);
                    dtpFecha.Focus();
                }
                else if (cboSolicitante.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "OBLI_SOLI"), 2);
                    cboSolicitante.Focus();
                }

                #region "CELSA"
                else if (cboTipoRequerimiento.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "OBLI_TIPR"), 2);
                    cboTipoRequerimiento.Focus();
                }
                else if (cboPrioridad.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "OBLI_PRIO"), 2);
                    cboPrioridad.Focus();
                }
                #endregion 

                return bolRpta;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                return bolRpta;
            }
        }

        private bool ValidaLogicaNegocio()
        {
            bool bolRpta = false;
            try
            {
                if (gbolNuevo == true && gbolEdicion == false)
                {
                    objE_HR.IdHR = 0;
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    objE_HR.IdHR = Convert.ToInt32(dtgHR.GetFocusedRowCellValue("IdHR").ToString());
                }

                objE_HR.FechaHR = Convert.ToDateTime(dtpFecha.EditValue);
                objE_HR.IdUC = Convert.ToInt32(cboUnidadControl.EditValue);
                DataTable tblConsulta = objB_HR.HRComp_BeforeCreate(objE_HR, HRComp);
                if (Convert.ToInt32(tblConsulta.Rows[0]["Cantidad"]) > 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "LOGI_COMP_DUPL"), 2);
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

        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidaCampoObligado()) { return; }
                HRComp = new DataTable();
                HRComp.Columns.Add("IdHRComp", Type.GetType("System.Int32"));
                HRComp.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
                HRComp.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                HRComp.Columns.Add("IdHR", Type.GetType("System.Int32"));
                HRComp.Columns.Add("FlagSolicitaInspeccion", Type.GetType("System.String"));
                HRComp.Columns.Add("Observacion", Type.GetType("System.String"));
                HRComp.Columns.Add("IdEstadoHRComp", Type.GetType("System.Int32"));
                HRComp.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                HRComp.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                int indexhr = 1;
                foreach (DataRow drHRComp in tblPFComponentes.Select("Nuevo = True AND (FlagSolicitaInspeccion = true OR Observacion <> '')"))
                {
                    DataRow dr = HRComp.NewRow();
                    dr["IdHRComp"] = indexhr;
                    dr["IdUCComp"] = Convert.ToInt32(drHRComp["IdUCComp"]);
                    dr["IdPerfilComp"] = Convert.ToInt32(drHRComp["IdPerfilComp"]);
                    dr["IdHR"] = (gbolIsDetalles) ? gintIdHR : 0;
                    dr["FlagSolicitaInspeccion"] = Convert.ToBoolean(drHRComp["FlagSolicitaInspeccion"]);
                    dr["Observacion"] = drHRComp["Observacion"].ToString();
                    dr["IdEstadoHRComp"] = 1;
                    dr["FlagActivo"] = true;
                    dr["Nuevo"] = true;
                    HRComp.Rows.Add(dr);
                    indexhr++;
                }

                foreach (DataRow drHRComp in tblPFComponentes.Select("Nuevo = false AND IdPerfilComp <> 0"))
                {
                    DataRow dr = HRComp.NewRow();
                    dr["IdHRComp"] = Convert.ToInt32(drHRComp["IdHRComp"]);
                    dr["IdUCComp"] = Convert.ToInt32(drHRComp["IdUCComp"]);
                    dr["IdPerfilComp"] = Convert.ToInt32(drHRComp["IdPerfilComp"]);
                    dr["IdHR"] = gintIdHR;
                    dr["FlagSolicitaInspeccion"] = Convert.ToBoolean(drHRComp["FlagSolicitaInspeccion"]);
                    dr["Observacion"] = drHRComp["Observacion"].ToString();
                    dr["IdEstadoHRComp"] = 1;
                    dr["FlagActivo"] = true;
                    dr["Nuevo"] = false;
                    HRComp.Rows.Add(dr);
                }


                if (gbolNuevo == true && gbolEdicion == false)
                {
                    if (ValidaLogicaNegocio()) { return; }
                    if (HRComp.Rows.Count == 0 && txtObservacionFinal.Text.Trim() == "")
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "LOGI_SOLI_ASIG"), 2);
                        return;
                    }

                    objE_HR = new E_HR();
                    objE_HR.IdHR = 0;
                    objE_HR.CodHR = "";
                    objE_HR.IdUC = Convert.ToInt32(cboUnidadControl.EditValue);
                    objE_HR.FechaHR = Convert.ToDateTime(dtpFecha.EditValue);
                    objE_HR.CodSolicitanteSAP = cboSolicitante.EditValue.ToString();
                    objE_HR.NombreSolicitanteSAP = cboSolicitante.Text;
                    objE_HR.IdEstadoHR = Convert.ToInt32(cboEstado.EditValue);
                    objE_HR.Observacion = txtObservacionFinal.Text;
                    objE_HR.FlagActivo = 1;
                    objE_HR.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
                    objE_HR.FechaModificacion = DateTime.Now;

                    #region CELSA
                    objE_HR.CodPrioridad = Convert.ToInt32(cboPrioridad.EditValue);
                    objE_HR.CodTipoRequerimiento = Convert.ToInt32(cboTipoRequerimiento.EditValue); ;
                    #endregion
                    int resp = objB_HR.HR_UpdateCascade(objE_HR, HRComp);
                    if (resp == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "GRAB_NUEV"), 1);
                    }
                    else if (resp == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (resp == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "GRAB_CONC"), 2);
                        return;
                    }

                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    if (ValidaLogicaNegocio()) { return; }
                    if ((HRComp.Rows.Count == 0 || HRComp.Select("FlagSolicitaInspeccion = True").Length == 0) && txtObservacionFinal.Text.Trim() == "")
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "LOGI_SOLI_ASIG"), 2);
                        return;
                    }

                    objE_HR = new E_HR();
                    objE_HR.IdHR = gintIdHR;
                    objE_HR.CodHR = "";
                    objE_HR.IdUC = Convert.ToInt32(cboUnidadControl.EditValue);
                    objE_HR.FechaHR = Convert.ToDateTime(dtpFecha.EditValue);
                    objE_HR.CodSolicitanteSAP = cboSolicitante.EditValue.ToString();
                    objE_HR.NombreSolicitanteSAP = cboSolicitante.Text;
                    objE_HR.IdEstadoHR = Convert.ToInt32(cboEstado.EditValue);
                    objE_HR.Observacion = txtObservacionFinal.Text;
                    objE_HR.FlagActivo = 1;
                    objE_HR.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
                    objE_HR.FechaModificacion = FechaModificacion;

                    #region "CELSA"
                    objE_HR.CodPrioridad= Convert.ToInt32(cboPrioridad.EditValue);
                    objE_HR.CodTipoRequerimiento = Convert.ToInt32(cboTipoRequerimiento.EditValue);
                    #endregion

                    int resp = objB_HR.HR_UpdateCascade(objE_HR, HRComp);
                    if (resp == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "GRAB_EDIT"), 1);
                    }
                    else if (resp == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (resp == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "GRAB_CONC"), 2);
                        return;
                    }
                }
                LimpiarControles();
                ListarHR();
                GlobalClass.ip.SeleccionarTab(tbListaHR);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tblUCList.Rows.Count == 0) { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "OBLI_CANT_UC"), 2); return; }
                if (lstSolicitanteSAP.Count == 0) { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "OBLI_CANT_SOLI"), 2); return; }
                dtvEstadosHR.RowFilter = "IdColumna <> 4";
                GlobalClass.ip.SeleccionarTab(tbDetalleHR);
                EstadoForm(true, false, true);
                cboEstado.IsEnabled = false;
                cboUnidadControl.IsEnabled = true;
                grbDetalles.IsEnabled = false;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgHR.VisibleRowCount == 0) { return; }
                GlobalClass.ip.SeleccionarTab(tbDetalleHR);
                gintIdHR = Convert.ToInt32(dtgHR.GetFocusedRowCellValue("IdHR"));
                LlenarDetallesHR();
                EstadoForm(false, true, true);
                cboEstado.IsEnabled = true;
                dtpFecha.IsReadOnly = true;
                cboUnidadControl.IsEnabled = false;
                grbDetalles.IsEnabled = false;
                if (Convert.ToInt32(cboEstado.EditValue) == 3 || Convert.ToInt32(cboEstado.EditValue) == 4)
                {
                    dtvEstadosHR.RowFilter = "";
                    cboEstado.IsEnabled = false;
                }
                else
                {
                    dtvEstadosHR.RowFilter = "IdColumna <> 4";
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BloquearControlesPorEstado(Boolean Valor)
        {
            cboSolicitante.IsEnabled = !Valor;
            chkInspeccion.IsEnabled = !Valor;
            dtpFecha.IsReadOnly = Valor;
            txtObservacionFinal.IsReadOnly = Valor;
            txtInfoComplementaria.IsReadOnly = Valor;
        }

        private void LimpiarControles()
        {
            try
            {
                txtInfoComplementaria.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtInfoComplementaria_EditValueChanged);
                txtObservacionFinal.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtObservacionFinal_EditValueChanged);
                dtpFecha.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpFecha_EditValueChanged);
                cboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
                cboSolicitante.SelectedIndexChanged -= new RoutedEventHandler(cboSolicitante_SelectedIndexChanged);
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                chkInspeccion.Unchecked -= new RoutedEventHandler(chkInspeccion_Unchecked);
                chkInspeccion.Checked -= new RoutedEventHandler(chkInspeccion_Checked);

                txtcodigo.Text = "Nuevo Código";
                txtInfoComplementaria.Text = "";
                txtObservacionFinal.Text = "";
                dtpFecha.EditValue = null;
                cboUnidadControl.SelectedIndex = -1;
                cboPerfil.SelectedIndex = -1;
                cboSolicitante.SelectedIndex = -1;
                cboEstado.SelectedIndex = 0;
                chkInspeccion.IsChecked = false;
                cboPerfil.ItemsSource = null;
                dtpFecha.IsReadOnly = false;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                trvComp.ItemsSource = null;
                gbolIsDetalles = false;
                BloquearControlesPorEstado(false);
                EstadoForm(false, false, true);

                lblHI.Visibility = Visibility.Collapsed;
                lblCodigoHI.Visibility = Visibility.Collapsed;

                txtInfoComplementaria.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtInfoComplementaria_EditValueChanged);
                txtObservacionFinal.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtObservacionFinal_EditValueChanged);
                dtpFecha.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpFecha_EditValueChanged);
                cboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(cboUnidadControl_SelectedIndexChanged);
                cboSolicitante.SelectedIndexChanged += new RoutedEventHandler(cboSolicitante_SelectedIndexChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                chkInspeccion.Unchecked += new RoutedEventHandler(chkInspeccion_Unchecked);
                chkInspeccion.Checked += new RoutedEventHandler(chkInspeccion_Checked);

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }


        private void dtpFecha_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
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

        private void cboSolicitante_SelectedIndexChanged(object sender, RoutedEventArgs e)
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

        private void chkInspeccion_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    DataRow[] drComponente = tblPFComponentes.Select("IdPerfilComp = " + trm.IdMenu);
                    drComponente[0]["FlagSolicitaInspeccion"] = (bool)chkInspeccion.IsChecked;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtInfoComplementaria_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    DataRow[] drComponente = tblPFComponentes.Select("IdPerfilComp = " + trm.IdMenu);
                    drComponente[0]["Observacion"] = txtInfoComplementaria.Text;
                    chkInspeccion.IsChecked = !(txtInfoComplementaria.Text.Trim() == "");
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtObservacionFinal_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
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
                    tbDetalleHR.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "TAB1_CONS");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "BTNG_CONS");
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tbDetalleHR.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "TAB1_NUEV");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "BTNG_NUEV");
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tbDetalleHR.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "TAB1_EDIT");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHojaRequerimiento, "BTNG_EDIT");
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgHR_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgHR.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodHR")
                    {
                        gintIdHR = Convert.ToInt32(dtgHR.GetFocusedRowCellValue("IdHR"));
                        LlenarDetallesHR();
                        GlobalClass.ip.SeleccionarTab(tbDetalleHR);
                        EstadoForm(false, false, true);
                        cboEstado.IsEnabled = true;
                        cboUnidadControl.IsEnabled = false;
                        dtpFecha.IsReadOnly = true;
                        grbDetalles.IsEnabled = false;

                        if (Convert.ToInt32(cboEstado.EditValue) == 3 || Convert.ToInt32(cboEstado.EditValue) == 4)
                        {
                            lblHI.Visibility = (lblCodigoHI.Content.ToString().Length > 0) ? Visibility.Visible : Visibility.Collapsed;
                            lblCodigoHI.Visibility = (lblCodigoHI.Content.ToString().Length > 0) ? Visibility.Visible : Visibility.Collapsed;
                            dtvEstadosHR.RowFilter = "";
                            cboEstado.IsEnabled = false;
                        }
                        else
                        {
                            lblHI.Visibility = Visibility.Collapsed;
                            lblCodigoHI.Visibility = Visibility.Collapsed;
                            dtvEstadosHR.RowFilter = "IdColumna <> 4";
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

        Boolean gbolIsDetalles = false;

        private void LlenarDetallesHR()
        {
            try
            {
                dtpFecha.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpFecha_EditValueChanged);
                cboSolicitante.SelectedIndexChanged -= new RoutedEventHandler(cboSolicitante_SelectedIndexChanged);
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                txtObservacionFinal.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtObservacionFinal_EditValueChanged);
                cboPrioridad.SelectedIndexChanged -= new RoutedEventHandler(cboPrioridad_SelectedIndexChanged);
                cboTipoRequerimiento.SelectedIndexChanged -= new RoutedEventHandler(cboTipoRequerimiento_SelectedIndexChanged);

                gbolIsDetalles = true;
                objE_HR.IdHR = gintIdHR;
                DataTable tblHRDet = objB_HR.HR_GetItem(objE_HR);
                txtcodigo.Text = tblHRDet.Rows[0]["CodHR"].ToString();
                dtpFecha.EditValue = Convert.ToDateTime(tblHRDet.Rows[0]["FechaHR"]);
                cboEstado.EditValue = tblHRDet.Rows[0]["IdEstadoHR"].ToString();
                cboSolicitante.EditValue = Convert.ToInt32(tblHRDet.Rows[0]["CodSolicitanteSAP"]);
                cboUnidadControl.EditValue = Convert.ToInt32(tblHRDet.Rows[0]["IdUC"]);
                txtObservacionFinal.Text = tblHRDet.Rows[0]["Observacion"].ToString();
                lblCodigoHI.Content = tblHRDet.Rows[0]["CodHI"].ToString();

                #region Celsa

                if (tblHRDet.Rows[0]["CodPrioridad"].ToString()!="" && tblHRDet.Rows[0]["CodTipoRequerimiento"].ToString() != "")
                {
                    cboPrioridad.EditValue = Convert.ToInt32(tblHRDet.Rows[0]["CodPrioridad"]);
                    cboTipoRequerimiento.EditValue = Convert.ToInt32(tblHRDet.Rows[0]["CodTipoRequerimiento"]);
                }
                #endregion

                lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblHRDet.Rows[0]["UsuarioCreacion"].ToString(), tblHRDet.Rows[0]["FechaCreacion"].ToString(), tblHRDet.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblHRDet.Rows[0]["UsuarioModificacion"].ToString(), tblHRDet.Rows[0]["FechaModificacion"].ToString(), tblHRDet.Rows[0]["HostModificacion"].ToString());

                if (Convert.ToInt32(cboEstado.EditValue) != 1)
                {
                    BloquearControlesPorEstado(true);
                }
                FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                dtpFecha.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpFecha_EditValueChanged);
                cboSolicitante.SelectedIndexChanged += new RoutedEventHandler(cboSolicitante_SelectedIndexChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                txtObservacionFinal.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtObservacionFinal_EditValueChanged);

                #region Celsa
                cboPrioridad.SelectedIndexChanged += new RoutedEventHandler(cboPrioridad_SelectedIndexChanged);
                cboTipoRequerimiento.SelectedIndexChanged += new RoutedEventHandler(cboTipoRequerimiento_SelectedIndexChanged);
                #endregion
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboUnidadControl_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                gintIdPerfil = Convert.ToInt32(tblUCList.Select("IdUC = " + cboUnidadControl.EditValue.ToString())[0]["IdPerfil"]);
                cboPerfil.ItemsSource = objB_Perfil.Perfil_List();
                cboPerfil.DisplayMember = "Perfil";
                cboPerfil.ValueMember = "IdPerfil";
                cboPerfil.EditValue = gintIdPerfil;

                CargarArbolComponentes(gintIdPerfil);

                grbDetalles.IsEnabled = false;
                txtInfoComplementaria.Text = "";
                chkInspeccion.IsChecked = false;
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
            tblPFComponentes = objB_Perfilcomp.PerfilComp_List(objE_Perfilcomp);
            DataColumn IdHRComp = new DataColumn() { ColumnName = "IdHRComp", DefaultValue = 0 };
            DataColumn IdUComp = new DataColumn() { ColumnName = "IdUCComp", DefaultValue = 0 };
            DataColumn dcNuevo = new DataColumn() { ColumnName = "Nuevo", DefaultValue = true };
            DataColumn dcNroSerie = new DataColumn() { ColumnName = "NroSerie", DefaultValue = "" };
            DataColumn dcFlagSolicitaInspeccion = new DataColumn() { ColumnName = "FlagSolicitaInspeccion", DefaultValue = false };
            DataColumn dcObservacion = new DataColumn() { ColumnName = "Observacion", DefaultValue = "" };
            tblPFComponentes.Columns.Add(IdHRComp);
            tblPFComponentes.Columns.Add(IdUComp);
            tblPFComponentes.Columns.Add(dcNuevo);
            tblPFComponentes.Columns.Add(dcNroSerie);
            tblPFComponentes.Columns.Add(dcFlagSolicitaInspeccion);
            tblPFComponentes.Columns.Add(dcObservacion);


            DataRow row = tblPFComponentes.NewRow();
            row["IdPerfilCompPadre"] = 1000;
            row["IdPerfilComp"] = 0;
            row["PerfilComp"] = cboPerfil.Text;
            row["Nuevo"] = false;
            row["Nivel"] = 1;
            row["NroSerie"] = "";
            row["FlagNeumatico"] = 0;
            tblPFComponentes.Rows.Add(row);

            objE_UCComp.IdUC = Convert.ToInt32(cboUnidadControl.EditValue);
            DataTable UCComp = objB_UCComp.UCComp_List(objE_UCComp);
            foreach (DataRow drUCComp in UCComp.Rows)
            {
                DataRow[] drPFComp = tblPFComponentes.Select("IdPerfilComp = " + drUCComp["IdPerfilComp"].ToString());
                drPFComp[0]["IdUCComp"] = Convert.ToInt32(drUCComp["IdUCComp"]);
                drPFComp[0]["NroSerie"] = drUCComp["NroSerie"].ToString();
            }

            if (gbolIsDetalles)
            {
                objE_HR.IdHR = gintIdHR;
                DataTable HRCompDet = objB_HR.HRComp_List(objE_HR);
                foreach (DataRow drHRComp in HRCompDet.Rows)
                {
                    DataRow[] drPFComp = tblPFComponentes.Select("IdPerfilComp = " + drHRComp["IdPerfilComp"].ToString());
                    Boolean FlagAndObser = (Convert.ToBoolean(drHRComp["FlagSolicitaInspeccion"]) && drHRComp["Observacion"].ToString() != "");
                    drPFComp[0]["IdHRComp"] = Convert.ToInt32(drHRComp["IdHRComp"]);
                    drPFComp[0]["FlagSolicitaInspeccion"] = drHRComp["FlagSolicitaInspeccion"].ToString();
                    drPFComp[0]["Observacion"] = drHRComp["Observacion"].ToString();
                    drPFComp[0]["PerfilComp"] = (FlagAndObser) ? "* " + drPFComp[0]["PerfilComp"].ToString() : drPFComp[0]["PerfilComp"].ToString();
                    drPFComp[0]["Nuevo"] = false;
                }
            }
            tblPFComponentes.DefaultView.RowFilter = "FlagNeumatico = 0";
            tblPFComponentes = tblPFComponentes.DefaultView.ToTable(true);

            Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblPFComponentes;
            trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(1000, null);
        }

        private void chkInspeccion_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    DataRow[] drComponente = tblPFComponentes.Select("IdPerfilComp = " + trm.IdMenu);
                    drComponente[0]["FlagSolicitaInspeccion"] = (bool)chkInspeccion.IsChecked;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void rbnPLANTILLA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListarHR();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }


        #region Celsa
        private void cboPrioridad_SelectedIndexChanged(object sender, RoutedEventArgs e)
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

        private void cboTipoRequerimiento_SelectedIndexChanged(object sender, RoutedEventArgs e)
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
        #endregion

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalClass.GeneraImpresion(gintIdMenu, gintIdHR);
            }
            catch { }
        }
    }
}
