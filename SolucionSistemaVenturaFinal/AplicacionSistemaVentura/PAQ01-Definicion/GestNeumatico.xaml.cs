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
using DevExpress.Xpf.Editors.Controls;
using System.Data;
using Business;
using Entities;
using InterfazMTTO;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;

namespace AplicacionSistemaVentura.PAQ01_Definicion
{
    public partial class GestNeumatico : UserControl
    {
        int gintIdAlmacen = 1000;
        string gstrCodigoArticulo = "";
        int gintNroLineaOT = 0;
        string gstrAlmaSali = "";
        int gintCantidadSalida = 1;
        string gstrCuenCont = "";
        string gstrTipoOperacion = "";

        string gstrDescripcionSAP = "";
        string gstrCodigoSAP = "";
        string gstrRowCicloEditando = "";
        int gintIdUC = 0;
        int gintIdNeumatico = 0;
        int gintIdEstadoN = 0;
        
        int gintValorTiempoDefecto = 0;
        int gintTiempoDefecto = 0;
        string gstrCicloDefecto = "";
        DateTime fechamodificacion;

        Boolean bolNuevo = false; Boolean bolEdicion = false;
        E_Perfil objE_perfil = new E_Perfil();
        E_Neumatico objNeumatico = new E_Neumatico();
        E_TablaMaestra objTablaMestra = new E_TablaMaestra();
        E_UCComp_Ciclo objUCComp_Ciclo = new E_UCComp_Ciclo();
        B_Neumatico objB_Neumatico = new B_Neumatico();
        B_UCComp_Ciclo objB_UCComp_Ciclo = new B_UCComp_Ciclo();
        E_UCComp_Ciclo objE_UCComp_Ciclo = new E_UCComp_Ciclo();
        B_Ciclo objB_Ciclo = new B_Ciclo();
        E_Ciclo objE_Ciclo = new E_Ciclo();
        B_Neumatico_Ciclo objB_Neumatico_Ciclo = new B_Neumatico_Ciclo();
        E_Neumatico_Ciclo objE_Neumatico_Ciclo = new E_Neumatico_Ciclo();

        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();
        DataTable tblCiclos = new DataTable();
        DataView dtv_maestra;
        InterfazMTTO.iSBO_BE.BEUDUC UDUC = new InterfazMTTO.iSBO_BE.BEUDUC();
        InterfazMTTO.iSBO_BE.BETUDUCList tucuclist = new InterfazMTTO.iSBO_BE.BETUDUCList();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        InterfazMTTO.iSBO_BE.BEOITMList BEOITMList = new InterfazMTTO.iSBO_BE.BEOITMList();

        int gintIdMenu = 0;
        string gstrEtiquetaNeumatico = "GestNeumatico";

        public GestNeumatico()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        public void LecturaDesdeMovimientoNeumatico()
        {
            
            EstadoForm(false, true, false);
            GlobalClass.ip.SeleccionarTab(tabDetalle);
            LlenarDatos(Utilitarios.Utilitarios.gintIdNeumaticoDeMovimiento);
            this.IsEnabled = false;
            
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                gridTab1.IsEnabled = true;
                LimpiarControles();
                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tabLista);
                //sbAuditoria.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboCiclo.SelectedIndex == -1) { return; }
                int irow = Convert.ToInt32(gstrRowCicloEditando);
                tblCiclos.Rows[irow]["IdCiclo"] = Convert.ToInt32(cboCiclo.EditValue);
                tblCiclos.Rows[irow]["Ciclo"] = cboCiclo.Text;
                tblCiclos.Rows[irow]["FrecuenciaCambio"] = txtFrecuenciaCambio.DisplayText;
                tblCiclos.Rows[irow]["Contador"] = txtContador.DisplayText;
                tblCiclos.Rows[irow]["FrecuenciaExtendida"] = txtFrecuenciaExtendida.DisplayText;
                tblCiclos.Rows[irow]["FlagCicloPrincipal"] = ((bool)chkCicloPrincipal.IsChecked);
                tblCiclos.Rows[irow]["IdEstadoCiclo"] = cboEstadoCiclo.EditValue;
                tblCiclos.Rows[irow]["Estado"] = cboEstadoCiclo.Text;
                //dtgCiclo.ItemsSource = tblCiclos;
                LimpiarPopUp();
                stpVentanaCiclo.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void CambiarformatoNumerico(int IdCiclo)
        {//Si es Reencauche recive tipo 1
            string MaskEntero = "#,###,###,###,###,##0;";
            string MaskDecimal = "#,###,###,###,###,##0.00;";
            bool Reencauche = (IdCiclo == 1);
            txtContador.Mask = (Reencauche) ? MaskEntero : MaskDecimal;
            txtFrecuenciaCambio.Mask = (Reencauche) ? MaskEntero : MaskDecimal;
            txtFrecuenciaExtendida.Mask = (Reencauche) ? MaskEntero : MaskDecimal;
        }

        private void btnAbrirVentanaCiclo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgCiclo.VisibleRowCount < 1) return;
                
                CambiarformatoNumerico(Convert.ToInt32(dtgCiclo.GetFocusedRowCellValue("IdCiclo")));
                
                chkCicloPrincipal.IsEnabled = false;

                EstadoForm(false, true, false);
                int Irow = dtgCiclo.GetSelectedRowHandles()[0];

                //gstrIdFilaCiclo = Irow.ToString();
                gstrRowCicloEditando = Irow.ToString();
                cboCiclo.EditValue = Convert.ToInt32(dtgCiclo.GetCellValue(Irow, "IdCiclo"));
                txtContador.Text = dtgCiclo.GetCellValue(Irow, "Contador").ToString();
                txtFrecuenciaCambio.Text = dtgCiclo.GetCellValue(Irow, "FrecuenciaCambio").ToString();
                txtFrecuenciaExtendida.Text = dtgCiclo.GetCellValue(Irow, "FrecuenciaExtendida").ToString();
                bool flagPrincipal = (bool)dtgCiclo.GetCellValue(Irow, "FlagCicloPrincipal");
                chkCicloPrincipal.IsChecked = (flagPrincipal) ? true : false;
                cboEstadoCiclo.EditValue = Convert.ToInt32(dtgCiclo.GetCellValue(Irow, "IdEstadoCiclo"));

                cboCiclo.IsEnabled = false;
                stpVentanaCiclo.Visibility = Visibility.Visible;
                txtContador.Focus();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        
        private void btnCancelarVentana_Click(object sender, RoutedEventArgs e)
        {
            stpVentanaCiclo.Visibility = Visibility.Hidden;
            LimpiarPopUp();
        }

        private void BtnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int NroSM = 0;
                int NroLineaSM = 0;
                if (ValidaLogicaNegocio(txtNroSerie.Text)) return;

                foreach (DataRow drCiclos in tblCiclos.Select("IdCiclo = 4"))
                {
                    drCiclos["FrecuenciaCambio"] = Convert.ToDouble(drCiclos["FrecuenciaCambio"]) * gintValorTiempoDefecto;
                    drCiclos["Contador"] = Convert.ToDouble(drCiclos["Contador"]) * gintValorTiempoDefecto;
                    drCiclos["FrecuenciaExtendida"] = Convert.ToDouble(drCiclos["FrecuenciaExtendida"]) * gintValorTiempoDefecto;
                }

                if ((bolNuevo == true) && (bolEdicion == false))
                {
                    if (ValidaCampoObligado()) return;


                    if (chkNoStock.IsChecked == false)
                    {
                        string sDocEntry = "";
                        InterfazMTTO.iSBO_BE.BEOIGE objOIGE = new InterfazMTTO.iSBO_BE.BEOIGE();
                        objOIGE.FechaSolicitud = DateTime.Now;
                        objOIGE.NroOrdenTrabajo = "";
                        objOIGE.NroSolicitudTransferencia = 0;
                        InterfazMTTO.iSBO_BE.BEIGE1List LBEIGE1 = new InterfazMTTO.iSBO_BE.BEIGE1List();
                        InterfazMTTO.iSBO_BE.BEIGE1List LBEIGE2 = new InterfazMTTO.iSBO_BE.BEIGE1List();
                        InterfazMTTO.iSBO_BE.BEIGE1 objBEIGE1 = new InterfazMTTO.iSBO_BE.BEIGE1();

                        objBEIGE1.CodigoArticulo = gstrCodigoArticulo;
                        objBEIGE1.NroLineaOrdenTrabajo = gintNroLineaOT;
                        objBEIGE1.AlmacenSalida = gstrAlmaSali;
                        objBEIGE1.CantidadSalida = gintCantidadSalida;
                        objBEIGE1.CuentaContable = gstrCuenCont;
                        objBEIGE1.TipoOperacion = gstrTipoOperacion;
                        LBEIGE1.Add(objBEIGE1);
                        LBEIGE2 = InterfazMTTO.iSBO_BL.SalidaMercancia_BL.RegistraSalidaMercancia(objOIGE, LBEIGE1, ref RPTA, ref sDocEntry);

                        if (RPTA.ResultadoRetorno == 0)
                        {
                            NroSM = LBEIGE2[0].NroSalidaMercancia;
                            NroLineaSM = LBEIGE2[0].NroLineaSalidaMercancia;
                        }
                        else
                        {
                            GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                            //return;
                        }

                    }

                    objNeumatico.IdNeumatico = 0;
                    objNeumatico.NroSerie = txtNroSerie.Text;
                    objNeumatico.CodigoSAP = cboTipoNeumatico.EditValue.ToString();
                    objNeumatico.DescripcionSAP = cboTipoNeumatico.Text;
                    objNeumatico.IdAlmacen = gintIdAlmacen;
                    objNeumatico.IdUC = 0;
                    objNeumatico.IdDisenio = Convert.ToInt32(cboDisenio.EditValue);
                    objNeumatico.IdTipoBanda = Convert.ToInt32(cboTipoBanda.EditValue);
                    objNeumatico.FechaAlta = Convert.ToDateTime(lblFeAl.Content);
                    objNeumatico.FechaBaja = "19000101";
                    objNeumatico.Observacion = txtComen.Text;
                    objNeumatico.IdEstadoN = Convert.ToInt32(cboEstado.EditValue);
                    objNeumatico.FlagActivo = 1;
                    objNeumatico.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                    objNeumatico.NroSalidaMercancia = NroSM;
                    objNeumatico.NroLineaSalidaMercancia = NroLineaSM;
                    objNeumatico.FechaModificacion = DateTime.Now;//****************************
                    tblCiclos.Columns.Remove("Estado");
                    int rpta = objB_Neumatico.Neumatico_UpdateCascade(objNeumatico, tblCiclos);
                    if (rpta != 0)// Ok
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "GRAB_NUEV1"), 1);
                    }
                    else if (rpta == 0)// ya fue modificado
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)// PK
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "GRAB_CONC"), 2);
                        return;
                    }

                    LimpiarControles();
                    ListarNeumatico();
                    EstadoForm(false, false, true);
                    GlobalClass.ip.SeleccionarTab(tabLista);
                    gintIdUC = 0;
                    gintIdNeumatico = 0;
                    gintIdAlmacen = 1000;
                }
                else if ((bolNuevo == false) && (bolEdicion == true))
                {
                    gintIdAlmacen = 0;
                    objNeumatico.IdNeumatico = gintIdNeumatico;
                    objNeumatico.NroSerie = txtNroSerie.Text;
                    objNeumatico.CodigoSAP = gstrCodigoSAP;
                    objNeumatico.DescripcionSAP = gstrDescripcionSAP;
                    objNeumatico.IdAlmacen = gintIdAlmacen;
                    objNeumatico.IdUC = 0;
                    objNeumatico.IdDisenio = Convert.ToInt32(cboDisenio.EditValue);
                    objNeumatico.IdTipoBanda = Convert.ToInt32(cboTipoBanda.EditValue);
                    objNeumatico.FechaAlta = Convert.ToDateTime(lblFeAl.Content);
                    objNeumatico.FechaBaja = string.IsNullOrEmpty(lblFeBa.Content.ToString()) ? "19000101" : lblFeBa.Content.ToString();
                    objNeumatico.Observacion = txtComen.Text;
                    objNeumatico.IdEstadoN = Convert.ToInt32(cboEstado.EditValue);
                    objNeumatico.FlagActivo = 1;
                    objNeumatico.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                    objNeumatico.FechaModificacion = fechamodificacion;//*********************
                    tblCiclos.Columns.Remove("Estado");
                    int rpta = objB_Neumatico.Neumatico_UpdateCascade(objNeumatico, tblCiclos);
                    if (rpta == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "GRAB_EDIT1"), 1);
                    }
                    else if (rpta == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "GRAB_CONC"), 2);
                        return;
                    }

                    LimpiarControles();
                    ListarNeumatico();
                    EstadoForm(false, false, true);
                    GlobalClass.ip.SeleccionarTab(tabLista);
                    
                    gintIdUC = 0;
                    gintIdNeumatico = 0;
                    gintIdAlmacen = 1000;
                }
                gridTab1.IsEnabled = true;
                tabControl1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            finally
            {
                GlobalClass.Columna_AddIFnotExits(tblCiclos, "Estado");
            }
        }
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgNeumatico.VisibleRowCount == 0) { return; }
                //5: idEstado de Baja
                if (Convert.ToInt32(dtgNeumatico.GetFocusedRowCellValue("IdEstadoN")) == 5)
                {
                    gridTab1.IsEnabled = false;
                }
                
                
                EstadoForm(false, true, false);
                GlobalClass.ip.SeleccionarTab(tabDetalle);
                LlenarDatos(Convert.ToInt32(dtgNeumatico.GetFocusedRowCellValue("IdNeumatico")));
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tblFechaHoraServer = Utilitarios.Utilitarios.Fecha_Hora_Servidor();
                LimpiarControles();
                cboEstado.EditValue = 1;
                
                txtNroSerie.Focus();
                lblFeAl.Content = tblFechaHoraServer.Rows[0]["FechaServer"].ToString() + " " + tblFechaHoraServer.Rows[0]["HoraServer"].ToString();
                lblFeBa.Content = "";
                cboEstado.SelectedIndex = 0;

                #region Cargar Grilla Ciclo
                DataTable tbl = objB_Ciclo.Ciclo_Combo();
                tblCiclos.Rows.Clear();
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    DataRow drCiclo = tblCiclos.NewRow();
                    drCiclo["IdNeumaticoCiclo"] = 0;
                    drCiclo["IdNeumatico"] = 0;
                    drCiclo["IdCiclo"] = tbl.Rows[i]["IdCiclo"].ToString();
                    drCiclo["Ciclo"] = tbl.Rows[i]["Ciclo"].ToString();
                    drCiclo["FrecuenciaCambio"] = (Convert.ToInt32(drCiclo["IdCiclo"]) == 1) ? "0" : "0.00";
                    drCiclo["Contador"] = (Convert.ToInt32(drCiclo["IdCiclo"]) == 1) ? "0" : "0.00";
                    drCiclo["FrecuenciaExtendida"] = (Convert.ToInt32(drCiclo["IdCiclo"]) == 1) ? "0" : "0.00";
                    drCiclo["FlagCicloPrincipal"] = (tbl.Rows[i]["Ciclo"].ToString() == "Tamaño de Banda") ? true : false;
                    drCiclo["IdEstadoCiclo"] = 1;
                    drCiclo["Estado"] = 1;
                    drCiclo["FlagActivo"] = true;
                    drCiclo["Nuevo"] = true;
                    tblCiclos.Rows.Add(drCiclo);
                }
                dtgCiclo.ItemsSource = tblCiclos;
                #endregion

                EstadoForm(true, false, true);
                GlobalClass.ip.SeleccionarTab(tabDetalle);
                btnDarBaja.IsEnabled = false;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboDisenio_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
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
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void cboTipoBanda_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void dtFechaAlta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void dtFechaBaja_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgNeumatico_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgNeumatico.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "NroSerie")
                    {
                        e.Handled = true;
                        stpVentanaTipoNeumatico.Visibility = Visibility.Hidden;
                        if (Convert.ToInt32(dtgNeumatico.GetFocusedRowCellValue("IdEstadoN")) == 5)
                        {
                            gridTab1.IsEnabled = false;
                        }
                        txtTipoNeu.Text = cboTipoNeumatico.Text;
                        cboTipoNeumatico.EditValue = gstrCodigoSAP;
                        gstrDescripcionSAP = cboTipoNeumatico.Text;
                        tblCiclos.Clear();
                                             
                        EstadoForm(false, false, true);
                        GlobalClass.ip.SeleccionarTab(tabDetalle);
                        LlenarDatos(Convert.ToInt32(dtgNeumatico.GetFocusedRowCellValue("IdNeumatico")));   
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void EstadoForm(Boolean stdNuevo, Boolean stdEditar, Boolean stdForzar)
        {
            try
            {
                if (stdForzar == true)
                {
                    bolNuevo = stdNuevo; bolEdicion = stdEditar;
                }
                else if (stdForzar == false)
                {
                    if (bolNuevo == false)
                    {
                        bolNuevo = stdNuevo; bolEdicion = stdEditar;
                    }
                }

                if ((bolNuevo == false) && (bolEdicion == false))
                {
                    tabDetalle.Header = "Consultar Neumático";
                    btnGrabar.Content = "Aceptar";
                    btnDarBaja.IsEnabled = false;
                }
                else if ((bolNuevo == true) && (bolEdicion == false))
                {
                    tabDetalle.Header = "Nuevo Neumático";
                    btnGrabar.Content = "Crear";                    
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((bolNuevo == false) && (bolEdicion == true))
                {
                    tabDetalle.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "TAB1_EDIT1");
                    btnGrabar.Content = "Actualizar";
                    btnDarBaja.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void LimpiarControles()
        {
            btnDarBaja.IsEnabled = true;
            btnAbrirVentanaTipo.IsEnabled = true;
            txtNroSerie.Text = string.Empty;
            cboEstado.EditValue = -1;
            cboDisenio.SelectedIndex = -1;
            cboTipoBanda.SelectedIndex = -1;
            txtComen.Text = string.Empty;
            lblFeAl.Content = DateTime.Now;
            lblFeBa.Content = DateTime.Now.Date;
            chkNoStock.Visibility = Visibility.Visible;
            ReadOnlyControls(gridTab1, false);
            lblIdNeumatico.Content = null;
            tblCiclos.Rows.Clear();
            dtgCiclo.ItemsSource = null;
            gintIdEstadoN = 0;
            txtEje.Clear();
            txtPosicion.Clear();
            txtTipoNeu.Clear();
            txtComen.Clear();
            txtContador.Clear();
            txtNroSerie.Clear();
            txtPosicion.Clear();
            txtUniControl.Clear();

        }
        private void LlenarDatos(int IdNeumatico)
        {
            try
            {
                gintIdNeumatico = IdNeumatico;
                gridTab1.IsEnabled = true;
                objNeumatico.IdNeumatico = IdNeumatico;
                DataTable tbl = B_Neumatico.Neumatico_GetItem(objNeumatico);
                lblIdNeumatico.Content = IdNeumatico;
                LimpiarControles();
                chkNoStock.Visibility = Visibility.Hidden;
                
                gstrDescripcionSAP = tbl.Rows[0]["DescripcionSAP"].ToString();
                txtTipoNeu.Text = gstrDescripcionSAP;
                gstrCodigoSAP = tbl.Rows[0]["CodigoSAP"].ToString();
                txtNroSerie.Text = tbl.Rows[0]["NroSerie"].ToString();
                int IdEstadoN = Convert.ToInt32(Utilitarios.Utilitarios.IIfDBNull(tbl.Rows[0]["IdEstadoN"], -1));
                gintIdEstadoN = IdEstadoN;
                cboEstado.EditValue = IdEstadoN;

                btnAbrirVentanaTipo.IsEnabled  = false;
                txtNroSerie.IsReadOnly = (IdEstadoN == 1) ? false : true;
                ReadOnlyControls(gridTab1, (IdEstadoN == 1) ? false: true);
                btnCancelar.IsEnabled = true;

                lblFeAl.Content = Convert.ToDateTime(tbl.Rows[0]["FechaAlta"]).ToString("dd/MM/yyyy HH:mm");
                lblFeBa.Content = DBNull.Value.Equals(tbl.Rows[0]["FechaBaja"]) ? string.Empty : Convert.ToDateTime(tbl.Rows[0]["FechaBaja"]).ToString("dd/MM/yyyy HH:mm");
                

                cboDisenio.EditValue = Convert.ToInt32(Utilitarios.Utilitarios.IIfDBNull(tbl.Rows[0]["IdDisenio"], 0));
                cboTipoBanda.EditValue = Convert.ToInt32(Utilitarios.Utilitarios.IIfDBNull(tbl.Rows[0]["IdTipoBanda"], 0));

                txtComen.Text = tbl.Rows[0]["Observacion"].ToString();
                txtUniControl.Text = tbl.Rows[0]["PlacaSerie"].ToString();
                string eje =string.Empty;
                if (!DBNull.Value.Equals(tbl.Rows[0]["IdEje"]))
                    eje = "E" + Utilitarios.Utilitarios.NumeroChar2(Convert.ToInt32(tbl.Rows[0]["IdEje"]));
                txtEje.Text = eje; 
                txtPosicion.Text = tbl.Rows[0]["IdPosicionCliente"].ToString();
                gintIdUC = Convert.ToInt32(Utilitarios.Utilitarios.IIfDBNull(tbl.Rows[0]["IdUC"], 0));

                objE_Neumatico_Ciclo.IdNeumatico = Convert.ToInt32(tbl.Rows[0]["IdNeumatico"]);
                DataTable TablaDeConsultaDeNeumaticoDeCiclos = objB_Neumatico_Ciclo.NeumaticoCiclo_List(objE_Neumatico_Ciclo);

                tblCiclos.Rows.Clear();
                for (int i = 0; i < TablaDeConsultaDeNeumaticoDeCiclos.Rows.Count; i++)
                {
                    DataRow rowCiclo = tblCiclos.NewRow();
                    rowCiclo["IdNeumaticoCiclo"] = Convert.ToInt32(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["IdNeumaticoCiclo"]);
                    rowCiclo["IdNeumatico"] = Convert.ToInt32(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["IdNeumatico"]);
                    rowCiclo["IdCiclo"] = Convert.ToInt32(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["IdCiclo"]);

                    rowCiclo["Ciclo"] = (Convert.ToInt32(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["IdCiclo"]) == 4) 
                        ?gstrCicloDefecto : TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["Ciclo"].ToString();
                    rowCiclo["FrecuenciaCambio"] = (Convert.ToInt32(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["IdCiclo"]) == 4) 
                        ?Convert.ToDouble( Convert.ToDouble(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["FrecuenciaCambio"]) / gintValorTiempoDefecto).ToString(Utilitarios.Utilitarios.FormatoDecimal) 
                        : Convert.ToDouble(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["FrecuenciaCambio"]).ToString(Utilitarios.Utilitarios.FormatoDecimal);
                    rowCiclo["Contador"] = (Convert.ToInt32(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["IdCiclo"]) == 4)
                        ? Convert.ToDouble(Convert.ToDouble(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["Contador"]) / gintValorTiempoDefecto).ToString(Utilitarios.Utilitarios.FormatoDecimal)
                        : Convert.ToDouble(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["Contador"]).ToString(Utilitarios.Utilitarios.FormatoDecimal);
                    rowCiclo["FrecuenciaExtendida"] = (Convert.ToInt32(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["IdCiclo"]) == 4)
                        ? Convert.ToDouble(Convert.ToDouble(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["FrecuenciaExtendida"]) / gintValorTiempoDefecto).ToString(Utilitarios.Utilitarios.FormatoDecimal)
                        : Convert.ToDouble(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["FrecuenciaExtendida"]).ToString(Utilitarios.Utilitarios.FormatoDecimal);
                    
                    rowCiclo["FlagCicloPrincipal"] = Convert.ToBoolean(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["FlagCicloPrincipal"]);
                    rowCiclo["IdEstadoCiclo"] = Convert.ToInt32(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["IdEstadoCiclo"]);
                    rowCiclo["Estado"] = TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["Estado"].ToString();
                    rowCiclo["FlagActivo"] = Convert.ToBoolean(TablaDeConsultaDeNeumaticoDeCiclos.Rows[i]["FlagActivo"]);
                    rowCiclo["Nuevo"] = false;
                    tblCiclos.Rows.Add(rowCiclo);
                }
                dtgCiclo.ItemsSource = tblCiclos;
                fechamodificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tbl.Rows[0]["UsuarioCreacion"].ToString(), tbl.Rows[0]["FechaCreacion"].ToString(), tbl.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", Utilitarios.Utilitarios.IIfNullBlank(tbl.Rows[0]["UsuarioModificacion"]),
                    Utilitarios.Utilitarios.IIfNullBlank(tbl.Rows[0]["FechaModificacion"]), Utilitarios.Utilitarios.IIfNullBlank(tbl.Rows[0]["HostModificacion"]));

                if (gintIdEstadoN == 5) btnDarBaja.IsEnabled = false;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarNeumatico()
        {
            if ((bool)rdbTodos.IsChecked)
                objNeumatico.IdEstadoN = 0;
            else if ((bool)rdbAlta.IsChecked)
                objNeumatico.IdEstadoN = 1;
            else if ((bool)rdbAsignado.IsChecked)
                objNeumatico.IdEstadoN = 2;
            else if ((bool)rdbRetiro.IsChecked)
                objNeumatico.IdEstadoN = 3;
            else if ((bool)rdbReparado.IsChecked)
                objNeumatico.IdEstadoN = 4;
            else if ((bool)rdbBaja.IsChecked)
                objNeumatico.IdEstadoN = 5;

            dtgNeumatico.ItemsSource = objB_Neumatico.Neumatico_List(objNeumatico);
        }

        public int ValidaTablaMaestra(DataView dv, string Filtro)
        {
            int Filas = 0;
            Filas = dv.Count;

            if (dv.Count == 0)
            {
                GlobalClass.ip.Mensaje("La tabla con Codigo : " + Filtro + " no se encuentra en la Base de Datos", 2);
            }

            return Filas;
        }

        private void UserControl_Loaded()
        {
            try
            {               
                chkNoStock.IsChecked = true;
                tblCiclos.Columns.Clear();
                tblCiclos.Columns.Add("IdNeumaticoCiclo", Type.GetType("System.Int32"));
                tblCiclos.Columns.Add("IdNeumatico", Type.GetType("System.Int32"));
                tblCiclos.Columns.Add("IdCiclo", Type.GetType("System.Int32"));
                tblCiclos.Columns.Add("Ciclo", Type.GetType("System.String"));
                tblCiclos.Columns.Add("FrecuenciaCambio", Type.GetType("System.Decimal"));
                tblCiclos.Columns.Add("Contador", Type.GetType("System.Decimal"));
                tblCiclos.Columns.Add("FrecuenciaExtendida", Type.GetType("System.Decimal"));
                tblCiclos.Columns.Add("FlagCicloPrincipal", Type.GetType("System.Boolean"));
                tblCiclos.Columns.Add("IdEstadoCiclo", Type.GetType("System.Int32"));
                tblCiclos.Columns.Add("Estado", Type.GetType("System.String"));
                tblCiclos.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblCiclos.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                
                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tabLista);

                txtUniControl.IsReadOnly = true;
                txtPosicion.IsReadOnly = true;
                txtEje.IsReadOnly = true;
                stpVentanaTipoNeumatico.Visibility = Visibility.Hidden;                
                txtNroSerie.MaxLength = 50;
                txtContador.MaxLength = 18;
                txtFrecuenciaCambio.MaxLength = 18;
                txtFrecuenciaExtendida.MaxLength = 18;
                

                objTablaMestra.IdTabla = 0;
                dtv_maestra = B_TablaMaestra.TablaMaestra_Combo(objTablaMestra).DefaultView;

                gintTiempoDefecto = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1000", dtv_maestra).Rows[7]["Valor"]);
                gintValorTiempoDefecto = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 58", dtv_maestra).Select("IdColumna = " + gintTiempoDefecto)[0][2]);
                gstrCicloDefecto = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 58", dtv_maestra).Select("IdColumna = " + gintTiempoDefecto)[0][3].ToString();

                cboDisenio.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=10", dtv_maestra);
                cboDisenio.DisplayMember = "Descripcion";
                cboDisenio.ValueMember = "IdColumna";
                if (ValidaTablaMaestra(dtv_maestra, "10") == 0) { return; }

                cboTipoBanda.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=11", dtv_maestra);
                cboTipoBanda.DisplayMember = "Descripcion";
                cboTipoBanda.ValueMember = "IdColumna";
                if (ValidaTablaMaestra(dtv_maestra, "11") == 0) { return; }

                cboEstado.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=2", dtv_maestra);
                cboEstado.DisplayMember = "Descripcion";
                cboEstado.ValueMember = "IdColumna";
                if (ValidaTablaMaestra(dtv_maestra, "2") == 0) { return; }

                objTablaMestra.IdTabla = 44;
                cboEstadoCiclo.ItemsSource = B_TablaMaestra.TablaMaestra_Combo(objTablaMestra);
                cboEstadoCiclo.DisplayMember = "Descripcion";
                cboEstadoCiclo.ValueMember = "IdColumna";

                cboCiclo.ItemsSource = objB_Ciclo.Ciclo_Combo();
                cboCiclo.DisplayMember = "Ciclo";
                cboCiclo.ValueMember = "IdCiclo";

                rdbAlta.IsChecked = true;//Esto lista los neumaticos

                //Variables para las dll
                gstrCodigoArticulo = gstrCodigoSAP;
                gstrAlmaSali = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=42 and IdColumna=2", dtv_maestra).Rows[0]["Valor"].ToString(); //TablaMaestra
                gstrCuenCont = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=45 and IdColumna=1", dtv_maestra).Rows[0]["Valor"].ToString(); //TablaMaestra
                gstrTipoOperacion = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=43 and IdColumna=1", dtv_maestra).Rows[0]["Valor"].ToString();//TablaMaestra

                BEOITMList = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("N", ref RPTA);

                if (RPTA.ResultadoRetorno == 0)
                {
                    cboTipoNeumatico.ItemsSource = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("N", false, gstrAlmaSali, ref RPTA);
                    cboTipoNeumatico.DisplayMember = "DescripcionArticulo";
                    cboTipoNeumatico.ValueMember = "CodigoArticulo";
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario.ToString(), 2);
                }

                #region VisualizacionBotonImprimir
                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                if (!VisualizaBotonImprimirDetalle)
                {
                    btnImprimir.Visibility = System.Windows.Visibility.Hidden;
                }
                #endregion

                if (Utilitarios.Utilitarios.gintIdNeumaticoDeMovimiento != 0)
                {
                    LecturaDesdeMovimientoNeumatico();
                    return;
                }


            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private bool ValidaCampoObligado()
        {
            bool bolRpta = false;
            try
            {
                if (txtTipoNeu.Text == "")
                {
                    bolRpta = true;
                    txtTipoNeu.Focus();
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "OBLI_TPN"), 2);
                }
                else if (txtNroSerie.Text == "")
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "OBLI_NSE"), 2);
                    txtNroSerie.Focus();
                }
                //else if (Utilitarios.Utilitarios.IsNumeric(txtNroSerie.Text))
                //{
                //    bolRpta = true;
                //    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "VAL_NSERNONUM"), 2);
                //    txtNroSerie.Focus();
                //}
                else if (cboEstado.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "OBLI_EST"), 2);
                    cboEstado.Focus();
                }
                return bolRpta;
            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                return bolRpta;
            }
        }

        private void btnAbrirVentanaTipo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stpVentanaTipoNeumatico.Visibility = Visibility.Visible;
                cboTipoNeumatico.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnAceptarTipoNeumatico_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTipoNeu.Text = cboTipoNeumatico.Text;
                gstrCodigoSAP = cboTipoNeumatico.EditValue.ToString();
                gstrDescripcionSAP = cboTipoNeumatico.Text;
                gstrCodigoArticulo = cboTipoNeumatico.EditValue.ToString();
                stpVentanaTipoNeumatico.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCancelarTipoNeumatico_Click(object sender, RoutedEventArgs e)
        {
            stpVentanaTipoNeumatico.Visibility = Visibility.Collapsed;
        }

        private void cboEstadoCiclo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtPLANTILLA_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LimpiarPopUp()
        {
            cboCiclo.IsEnabled = true;
            txtContador.Text = "";
            txtFrecuenciaCambio.Text = "";
            cboCiclo.SelectedIndex = -1;
            txtFrecuenciaExtendida.Text = "";
            chkCicloPrincipal.IsChecked = false;
        }

        private void rdbBaja_Checked(object sender, RoutedEventArgs e)
        {
            ListarNeumatico();
        }

        private void rdbReparado_Checked(object sender, RoutedEventArgs e)
        {
            ListarNeumatico();
        }

        private void rdbRetiro_Checked(object sender, RoutedEventArgs e)
        {
            ListarNeumatico();
        }

        private void rdbAsignado_Checked(object sender, RoutedEventArgs e)
        {
            ListarNeumatico();
        }

        private void rdbAlta_Checked(object sender, RoutedEventArgs e)
        {
            ListarNeumatico();
        }

        private void rdbTodos_Checked(object sender, RoutedEventArgs e)
        {
            ListarNeumatico();
        }

        private bool ValidaLogicaNegocio(string nroSerie)
        {
            bool bolRpta = false;
            try
            {
                if ((bolNuevo == true) && (bolEdicion == false))
                {
                    objNeumatico.IdNeumatico = 0;
                }
                else if ((bolNuevo == false) && (bolEdicion == true))
                {
                    objNeumatico.IdNeumatico = Convert.ToInt32(dtgNeumatico.GetFocusedRowCellValue("IdNeumatico"));
                }
                objNeumatico.NroSerie = nroSerie;
                DataTable tblNeumatico = objB_Neumatico.Neumatico_GetItemBySerie(objNeumatico);
                if (tblNeumatico.Rows.Count > 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "LOGI_DUPL_SER"), 2);
                    txtNroSerie.Focus();
                }
                return bolRpta;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                return bolRpta;
            }
        }

        private void chkNoStock_Click(object sender, RoutedEventArgs e)
        {
            txtTipoNeu.Clear();
            bool NoStock = ((bool)chkNoStock.IsChecked) ? false : true;
            cboTipoNeumatico.ItemsSource = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("N", NoStock, gstrAlmaSali, ref RPTA);

            if (RPTA.ResultadoRetorno == 0)
            {
                //cboTipoNeumatico.ItemsSource = X;
                cboTipoNeumatico.DisplayMember = "DescripcionArticulo";
                cboTipoNeumatico.ValueMember = "CodigoArticulo";
            }
            else
            {
                GlobalClass.ip.Mensaje(RPTA.ToString(), 2);
            }
        }
        private void btnDarBaja_Click(object sender, RoutedEventArgs e)
        {

            if (2 == gintIdEstadoN)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "LOGI_BAJA_ASIG"), 2);
                return;
            }
            stpVentanaBaja.Visibility = Visibility.Visible;
        }

        private void btnAceptarVentanaBaja_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(mskFecha.Text))
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "OBLI_FECH_BAJA"), 2);
                return;
            }
            objNeumatico.IdNeumatico = gintIdNeumatico;
            objNeumatico.FechaBaja = Convert.ToDateTime(mskFecha.EditValue).ToString("yyyyMMdd HH:mm");
            objNeumatico.IdUsuarioModificacion = Utilitarios.Utilitarios.gintIdUsuario;
            int n = B_Neumatico.Neumatico_UpdateBaja(objNeumatico);
            if (n > 0)
            {
                LimpiarControles();
                ListarNeumatico();
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "GRAB_BAJ"), 1);
                mskFecha.EditValue = null;
                stpVentanaBaja.Visibility = Visibility.Collapsed;
                tabControl1.SelectedIndex = 0;
            }
            else
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaNeumatico, "GRAB_BAJA_ERRO"), 2);
            }
        }

        private void btnCancelarVentanaBaja_Click(object sender, RoutedEventArgs e)
        {
            mskFecha.EditValue = null;
            stpVentanaBaja.Visibility = Visibility.Collapsed;
        }

        private void ReadOnlyControls(FrameworkElement contenedor, bool valor)
        {
            foreach (var c in LogicalTreeHelper.GetChildren(contenedor))
            {
                var hijo = c as FrameworkElement;
                if (c is TextBox ) (c as TextBox).IsReadOnly = valor;
                if (c is TextEdit) (c as TextEdit).IsReadOnly = valor;
                if (c is ComboBox) (c as ComboBox).IsReadOnly = valor;
                if (c is ComboBoxEdit) (c as ComboBoxEdit).IsReadOnly = valor;
                if (c is Button) (c as Button).IsEnabled = !valor;
                if (c is ButtonEdit) (c as ButtonEdit).IsEnabled = !valor;
                if (c is GridControl) (c as GridControl).View.IsEnabled = !valor;                

                if (c is GroupBox || c is DockPanel || c is Grid || c is StackPanel || c is TabControl)
                    ReadOnlyControls(hijo, valor);

            }
            cboEstado.IsEnabled = false;
            txtPosicion.IsReadOnly=true;
            txtEje.IsReadOnly=true;
            txtUniControl.IsReadOnly=true;
        }

        private void PLANTILLA_VentanaEmergente_IsVisible(object sender, DependencyPropertyChangedEventArgs e)
        {
            GlobalClass.ip.VentanaEmergente_Visibilidad(sender);
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int IdEstado = 0;
                if ((bool)rdbTodos.IsChecked)
                    IdEstado = 0;
                else if ((bool)rdbAlta.IsChecked)
                    IdEstado = 1;
                else if ((bool)rdbAsignado.IsChecked)
                    IdEstado = 2;
                else if ((bool)rdbRetiro.IsChecked)
                    IdEstado = 3;
                else if ((bool)rdbReparado.IsChecked)
                    IdEstado = 4;
                else if ((bool)rdbBaja.IsChecked)
                    IdEstado = 5;
                GlobalClass.GeneraImpresion(gintIdMenu, IdEstado);
            }
            catch { }
        }
    }
}