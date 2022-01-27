using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using Entities;
using Business;
using static Utilitarios.Utilitarios;
using Utilitarios.Enum;
using Utilitarios.Constantes;
#region REQUERIMIENTO_03_CELSA
using System.Net.Mail;
#endregion


namespace AplicacionSistemaVentura.PAQ03_Ejecucion
{
    /// <summary>
    /// Interaction logic for EjecGestRegInci.xaml
    /// </summary>
    public partial class EjecGestRegInci : UserControl
    {
        public EjecGestRegInci()
        {
            InitializeComponent();
            UserControl_Loaded();
        }
        string gstrEtiquetaRegistroIncidencias = "EjecGestRegInci";

        DataTable tblFechaServer = new DataTable();
        DataView dtvCiclo = new DataView();
        DataTable tblUCIncidenciaDet = new DataTable();
        Utilitarios.ErrorHandler Error = new Utilitarios.ErrorHandler();
        Utilitarios.DebugHandler Debug = new Utilitarios.DebugHandler();
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        E_UC objE_UC = new E_UC();
        B_UC objB_UC = new B_UC();
        E_Ciclo objE_Ciclo = new E_Ciclo();
        B_Ciclo objB_Ciclo = new B_Ciclo();
        E_UCIncidenciaDet objE_UCIncidenciaDet = new E_UCIncidenciaDet();
        B_UCIncidenciaDet objB_UCIncidenciaDet = new B_UCIncidenciaDet();

        DataView dtvMaestra = new DataView();
        DataView dtvUCIncidenciaDet = new DataView();
        InterfazMTTO.iSBO_BE.BEUDUC UDUC = new InterfazMTTO.iSBO_BE.BEUDUC();
        InterfazMTTO.iSBO_BE.BETUDUCList tucuclist = new InterfazMTTO.iSBO_BE.BETUDUCList();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        InterfazMTTO.iSBO_BE.BEOITMList BEOITMList = new InterfazMTTO.iSBO_BE.BEOITMList();
        InterfazMTTO.iSBO_BE.BEOITMList BEOITMListRep = new InterfazMTTO.iSBO_BE.BEOITMList();
        InterfazMTTO.iSBO_BE.BEOHEM BEOHEMList = new InterfazMTTO.iSBO_BE.BEOHEM();
        InterfazMTTO.iSBO_BE.BEOHEMList OHEMlist = new InterfazMTTO.iSBO_BE.BEOHEMList();
        InterfazMTTO.iSBO_BE.BEOHEMList OHRElist = new InterfazMTTO.iSBO_BE.BEOHEMList();

        B_ContadorDet objB_ContadorDet = new B_ContadorDet();
        E_ContadorDet objE_ContadorDet = new E_ContadorDet();


        int cicloPorDefecto = 0;

        #region REQUERIMIENTO_03_CELSA
        string correodest;
        #endregion

        private void ListarIncidencias()
        {
            cboNroOperacion.Visibility = Visibility.Collapsed;
            objE_ContadorDet.FechaIni = Convert.ToDateTime(mskFechaIni.EditValue);
            objE_ContadorDet.FechaFin = Convert.ToDateTime(mskFechaFin.EditValue);
            dtgContadorDet.ItemsSource = objB_ContadorDet.ContadorDet_ListByDate(objE_ContadorDet);
        }

        private void UserControl_Loaded()
        {
            try
            {
                btnModificar.Visibility = System.Windows.Visibility.Hidden;
                tblFechaServer = Utilitarios.Utilitarios.Fecha_Hora_Servidor();

                mskFechaFin.EditValue = Convert.ToDateTime(tblFechaServer.Rows[0]["FechaServer"].ToString() + " " + tblFechaServer.Rows[0]["HoraServer"].ToString());
                mskFechaIni.EditValue = ((DateTime)mskFechaFin.EditValue).AddDays(-20);
                ListarIncidencias();
                GlobalClass.ip.SeleccionarTab(tabLista);

                OHEMlist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("S", ref RPTA);
                OHRElist = InterfazMTTO.iSBO_BL.Empleado_BL.ListaEmpleado("R", ref RPTA);
                CboResponsable.ItemsSource = OHRElist;
                CboResponsable.DisplayMember = "NombrePersona";
                CboResponsable.ValueMember = "CodigoPersona";
                CboResponsable.SelectedIndex = -1;

                CboSolicitante.ItemsSource = OHEMlist;
                CboSolicitante.DisplayMember = "NombrePersona";
                CboSolicitante.ValueMember = "CodigoPersona";
                CboSolicitante.SelectedIndex = -1;

                cboUC.SelectedIndexChanged -= new RoutedEventHandler(cboUC_SelectedIndexChanged);
                objE_UC.IdPerfil = 0;
                objE_UC.IdUc = 0;
                cboUC.ItemsSource = objB_UC.UC_ComboByUC(objE_UC);//.B_UC_Combo(objE_UC);
                cboUC.DisplayMember = "PlacaSerie";
                cboUC.ValueMember = "CodUC";
                cboUC.SelectedIndex = -1;
                cboUC.SelectedIndexChanged += new RoutedEventHandler(cboUC_SelectedIndexChanged);

                objE_TablaMaestra.IdTabla = 0;
                dtvMaestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).DefaultView;

                cboOrigenRegistro.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=38", dtvMaestra);
                cboOrigenRegistro.DisplayMember = "Descripcion";
                cboOrigenRegistro.ValueMember = "IdColumna";
                cboOrigenRegistro.SelectedIndex = 0;
                cboOrigenRegistro.IsEnabled = false;

                cboEvento.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=47", dtvMaestra);
                cboEvento.DisplayMember = "Descripcion";
                cboEvento.ValueMember = "IdColumna";

                cboTipoOperacion.SelectedIndexChanged -= new RoutedEventHandler(cboTipoOperacion_SelectedIndexChanged);
                DataTable tbl = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=39", dtvMaestra);
                tbl.DefaultView.RowFilter = "IdColumna in (1,2)";
                cboTipoOperacion.ItemsSource = tbl.DefaultView;
                cboTipoOperacion.DisplayMember = "Descripcion";
                cboTipoOperacion.ValueMember = "IdColumna";
                cboTipoOperacion.SelectedIndex = -1;
                cboTipoOperacion.SelectedIndexChanged += new RoutedEventHandler(cboTipoOperacion_SelectedIndexChanged);

                txtContadorIni.EditValue = 0.0;

                btnBuscar_Click(null, null);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            ListarIncidencias();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LimpiarControles();
                GlobalClass.ip.SeleccionarTab(tabLista);
                EstadoForm(false, false, true);
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

                    tabDetalle.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "TAB1_CONS");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "BTNG_CONS");
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tabDetalle.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "TAB1_NUEV");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "BTNG_NUEV");
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tabDetalle.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "TAB1_EDIT");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "BTNG_EDIT");
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    if (ValidaCampoObligado() == true) return;
                    if (ValidaEstadoUnidadControl() == true) return;
                    //if (ValidaCorrecion() == true) return;

                    int IdTipoOperacion = Convert.ToInt32(cboTipoOperacion.EditValue);
                    string NroDocOperacion = "";
                    int IdDocCorregir = 0;
                    if (IdTipoOperacion == 2)
                    {
                        objE_ContadorDet.NroDocOperacion = cboNroOperacion.Text;
                        IdDocCorregir = Convert.ToInt32(cboNroOperacion.EditValue);
                        NroDocOperacion = cboNroOperacion.Text;
                    }
                    //DataTable tbl = (DataTable)dtgContadorDet.ItemsSource;
                    //for (int i = 0; i < tbl.Rows.Count; i++)
                    //{
                    //    string fechahorai = Convert.ToDateTime(txtFechaIni.EditValue).ToString();
                    //    string fechahoraf = Convert.ToDateTime(txtFechaFin.EditValue).ToString();
                    //    string uct = tbl.Rows[i]["PlacaSerie"].ToString();
                    //    string uc = cboUC.EditValue.ToString();
                    //    string fechai = tbl.Rows[i]["FechaIni"].ToString();
                    //    string fechaf = tbl.Rows[i]["FechaFin"].ToString();
                    //    if (uct != uc)
                    //    {
                    //    objE_ContadorDet.CodUc = cboUC.EditValue.ToString();
                    //    objE_ContadorDet.IdOrigenRegistro = Convert.ToInt32(cboOrigenRegistro.EditValue);
                    //    objE_ContadorDet.IdEvento = Convert.ToInt32(cboEvento.EditValue);
                    //    objE_ContadorDet.IdTipoOperacion = IdTipoOperacion;
                    //    objE_ContadorDet.NroDocOperacion = NroDocOperacion;
                    //    objE_ContadorDet.IdDocCorregir = IdDocCorregir;
                    //    objE_ContadorDet.FechaHoraIni = Convert.ToDateTime(txtFechaIni.EditValue);
                    //    objE_ContadorDet.FechaHoraFin = Convert.ToDateTime(txtFechaFin.EditValue);
                    //    objE_ContadorDet.ContadorIni = Convert.ToDouble(txtContadorIni.EditValue);
                    //    objE_ContadorDet.ContadorFin = Convert.ToDouble(txtContadorFin.EditValue);
                    //    objE_ContadorDet.CodSolicitante = "44";// CboSolicitante.EditValue.ToString();
                    //    objE_ContadorDet.CodResponsable = "105";// CboResponsable.EditValue.ToString();
                    //    objE_ContadorDet.Observacion = txtObservacion.Text;
                    //    objE_ContadorDet.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
                    //}

                    //if (uct == uc && fechahorai != fechai && fechahoraf != fechaf)
                    //{
                    objE_ContadorDet.CodUc = cboUC.EditValue.ToString();
                    objE_ContadorDet.IdOrigenRegistro = Convert.ToInt32(cboOrigenRegistro.EditValue);
                    objE_ContadorDet.IdEvento = Convert.ToInt32(cboEvento.EditValue);
                    objE_ContadorDet.IdTipoOperacion = IdTipoOperacion;
                    objE_ContadorDet.NroDocOperacion = NroDocOperacion;
                    objE_ContadorDet.IdDocCorregir = IdDocCorregir;
                    objE_ContadorDet.FechaHoraIni = Convert.ToDateTime(txtFechaIni.DisplayText);
                    objE_ContadorDet.FechaHoraFin = Convert.ToDateTime(txtFechaFin.DisplayText);
                    objE_ContadorDet.ContadorIni = Convert.ToDouble(txtContadorIni.EditValue);
                    objE_ContadorDet.ContadorFin = Convert.ToDouble(txtContadorFin.EditValue);
                    //objE_ContadorDet.CodSolicitante = "44";
                    //objE_ContadorDet.CodResponsable = "105";
                    objE_ContadorDet.CodSolicitante = CboSolicitante.EditValue.ToString();
                    objE_ContadorDet.CodResponsable = CboResponsable.EditValue.ToString();
                    objE_ContadorDet.Observacion = txtObservacion.Text;
                    objE_ContadorDet.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
                    //    }
                    //    else
                    //    {
                    //        GlobalClass.ip.Mensaje("Error de Duplicidad", 2);
                    //    }
                    //}


                    string DescError = String.Empty;
                    int resp = objB_ContadorDet.ContadorDet_UpdateProcess(objE_ContadorDet, out DescError);
                    if (resp > 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "GRAB_NUEV"), 1);
                        mskFechaFin.EditValue = Utilitarios.Utilitarios.FechaHora_Servidor();
                        ListarIncidencias();

                        #region REQUERIMIENTO_03_CELSA
                        //Enviar Correos si alguno de los semáforos está en amarillo o naranja 
                        B_UC objuc = new B_UC();
                        DataTable tlvu = new DataTable();
                        string vUC = cboUC.EditValue.ToString();
                        tlvu = objuc.ContadoresxUC_List(vUC, cicloPorDefecto);

                        if (tlvu.Rows.Count > 0)
                        {
                            for (int i = 0; i < tlvu.Rows.Count; i++)
                            {
                                decimal Vcontador = Convert.ToDecimal(tlvu.Rows[i]["Contador"]);
                                decimal VFcambio = Convert.ToDecimal(tlvu.Rows[i]["FrecuenciaCambio"]);
                                decimal VPor1 = Convert.ToDecimal(tlvu.Rows[i]["Porc01"]) / 100;
                                decimal VPor2 = Convert.ToDecimal(tlvu.Rows[i]["Porc02"]) / 100;
                                string PerfilComp = Convert.ToString(tlvu.Rows[i]["PerfilComp"]);
                                string color;

                                decimal vSemAmari = VFcambio - (VFcambio * VPor1);
                                decimal vSemNaran = VFcambio - (VFcambio * VPor2);

                                if (Vcontador >= vSemAmari && Vcontador < vSemNaran)
                                {
                                    color = "amarillo";
                                    Mailer("Semáforo amarillo: " + vUC, vUC, PerfilComp, VFcambio.ToString(), Vcontador.ToString(), Convert.ToString(tlvu.Rows[i]["Porc01"]), color);
                                }
                                if (Vcontador >= vSemNaran)
                                {
                                    color = "naranja";
                                    Mailer("Semáforo naranja: " + vUC, vUC, PerfilComp, VFcambio.ToString(), Vcontador.ToString(), Convert.ToString(tlvu.Rows[i]["Porc02"]), color);
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje(DescError, 2);
                        return;
                    }
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {

                }
                GlobalClass.ip.SeleccionarTab(tabLista);
                EstadoForm(false, false, true);
                LimpiarControles();
                ListarIncidencias();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgContadorDet_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgContadorDet.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "NroDocOperacion")
                    {
                        EstadoForm(false, false, true);
                        LlenarDatos();
                        GlobalClass.ip.SeleccionarTab(tabDetalle);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BloquearControles(Boolean Estado)
        {
            cboUC.SelectedIndexChanged -= new RoutedEventHandler(cboUC_SelectedIndexChanged);
            cboUC.IsReadOnly = Estado;
            cboOrigenRegistro.IsReadOnly = Estado;
            cboEvento.IsReadOnly = Estado;
            cboTipoOperacion.IsReadOnly = Estado;
            cboNroOperacion.IsEnabled = !Estado;
            CboSolicitante.IsReadOnly = Estado;
            CboResponsable.IsReadOnly = Estado;
            txtFechaIni.IsReadOnly = Estado;
            txtFechaFin.IsReadOnly = Estado;
            txtContadorIni.IsReadOnly = Estado;
            txtContadorFin.IsReadOnly = Estado;
            txtObservacion.IsReadOnly = Estado;
            cboUC.SelectedIndexChanged += new RoutedEventHandler(cboUC_SelectedIndexChanged);
        }
        private void LimpiarControles()
        {
            cboTipoOperacion.SelectedIndexChanged -= new RoutedEventHandler(cboTipoOperacion_SelectedIndexChanged);
            cboUC.SelectedIndexChanged -= new RoutedEventHandler(cboUC_SelectedIndexChanged);
            cboUC.SelectedIndex = -1;
            cboNroOperacion.SelectedIndex = -1;
            cboNroOperacion.ItemsSource = null;
            cboEvento.SelectedIndex = -1;
            cboTipoOperacion.SelectedIndex = -1;
            CboSolicitante.SelectedIndex = -1;
            CboResponsable.SelectedIndex = -1;
            txtFechaIni.EditValue = null;
            txtFechaFin.EditValue = null;
            txtContadorIni.Text = string.Empty;
            txtContadorFin.Text = string.Empty;
            txtObservacion.Text = string.Empty;
            txtCiclo.Text = string.Empty;
            cboNroOperacion.Visibility = Visibility.Collapsed;
            BloquearControles(false);
            cboUC.SelectedIndexChanged += new RoutedEventHandler(cboUC_SelectedIndexChanged);
            cboTipoOperacion.SelectedIndexChanged += new RoutedEventHandler(cboTipoOperacion_SelectedIndexChanged);
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarControles();
            EstadoForm(true, false, true);
            GlobalClass.ip.SeleccionarTab(tabDetalle);
        }

        private void LlenarDatos()
        {
            cboTipoOperacion.SelectedIndexChanged -= new RoutedEventHandler(cboTipoOperacion_SelectedIndexChanged);
            cboUC.SelectedIndexChanged -= new RoutedEventHandler(cboUC_SelectedIndexChanged);
            objE_ContadorDet.IdContadorDet = Convert.ToInt32(dtgContadorDet.GetFocusedRowCellValue("IdContadorDet"));
            DataTable tblContadorDet = objB_ContadorDet.ContadorDet_GetItem(objE_ContadorDet);

            cboUC.EditValue = tblContadorDet.Rows[0]["CodUc"].ToString();
            cboOrigenRegistro.EditValue = Convert.ToInt32(tblContadorDet.Rows[0]["IdOrigenRegistro"]);
            cboEvento.EditValue = Convert.ToInt32(tblContadorDet.Rows[0]["IdEvento"]);
            cboTipoOperacion.EditValue = Convert.ToInt32(tblContadorDet.Rows[0]["IdTipoOperacion"]);
            CboSolicitante.EditValue = Convert.ToInt32(tblContadorDet.Rows[0]["CodSolicitante"]);
            CboResponsable.EditValue = Convert.ToInt32(tblContadorDet.Rows[0]["CodResponsable"]);
            txtFechaIni.EditValue = Convert.ToDateTime(tblContadorDet.Rows[0]["FechaIni"]);
            txtFechaFin.EditValue = Convert.ToDateTime(tblContadorDet.Rows[0]["FechaFin"]);
            txtContadorIni.EditValue = tblContadorDet.Rows[0]["ContadorIni"];
            txtContadorFin.EditValue = tblContadorDet.Rows[0]["ContadorFin"];
            txtObservacion.Text = Utilitarios.Utilitarios.IIfNullBlank(tblContadorDet.Rows[0]["Observacion"]);
            lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblContadorDet.Rows[0]["UsuarioCreacion"].ToString(), tblContadorDet.Rows[0]["FechaCreacion"].ToString(), tblContadorDet.Rows[0]["HostCreacion"].ToString());
            cboTipoOperacion.SelectedIndexChanged += new RoutedEventHandler(cboTipoOperacion_SelectedIndexChanged);
            cboUC.SelectedIndexChanged += new RoutedEventHandler(cboUC_SelectedIndexChanged);
            EstadoForm(false, false, true);
            BloquearControles(true);
        }

        private bool ValidaCampoObligado()
        {
            bool bolRpta = false;
            try
            {
                if (cboUC.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "OBLI_UC"), 2);
                    cboUC.Focus();
                }
                else if (cboOrigenRegistro.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "OBLI_ORIG"), 2);
                    cboOrigenRegistro.Focus();
                }
                else if (cboEvento.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "OBLI_EVEN"), 2);
                    cboEvento.Focus();

                }
                else if (cboTipoOperacion.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "OBLI_TOPE"), 2);
                    cboTipoOperacion.Focus();
                }
                else if (cboTipoOperacion.SelectedIndex == 1 && cboNroOperacion.SelectedIndex == -1)//Correccion
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "OBLI_RINC"), 2);
                    cboNroOperacion.Focus();
                }
                else if (CboSolicitante.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "OBLI_SOLI"), 2);
                    CboSolicitante.Focus();
                }
                else if (CboResponsable.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "OBLI_RESP"), 2);
                    CboResponsable.Focus();
                }
                else if (txtContadorFin.Text == "" || txtContadorIni.Text == "")
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "OBLI_ACON"), 2);
                }
                else if (Convert.ToDouble(txtContadorFin.Text) < Convert.ToDouble(txtContadorIni.Text))
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "LOGI_FCON_MENO"), 2);
                }
                else if (txtFechaFin.Text == "" || txtFechaIni.Text == "")
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "OBLI_AFEC"), 2);
                }
                else if (Convert.ToDateTime(txtFechaFin.EditValue) < Convert.ToDateTime(txtFechaIni.DisplayText))
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "LOGI_FFEC_MENO"), 2);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            return bolRpta;
        }

        private void cboTipoOperacion_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            int IdColumna = Convert.ToInt32(cboTipoOperacion.EditValue);
            cboNroOperacion.Visibility = (IdColumna == 2) ? Visibility.Visible : Visibility.Collapsed;

            if (IdColumna == 2)
            {
                LimpiarContador();
                cboNroOperacion.Focus();
            }
            else
            {
                txtFechaIni.IsReadOnly = false;
                TraerUltimoContador();
            }
        }

        private void cboUC_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cboUC.EditValue == null) return;

            objE_ContadorDet.IdTipoOperacion = 1;
            objE_ContadorDet.CodUc = cboUC.EditValue.ToString();
            DataView dtv = objB_ContadorDet.ContadorDet_List(objE_ContadorDet).DefaultView;
            cboNroOperacion.ItemsSource = dtv;

            TraerCiclodeUnidadControl();
            TraerUltimoContador();

        }

        public void TraerUltimoContador()
        {
            try
            {
                //Traer la ultima incidencia registrada
                if (cboUC.EditValue == null) return;

                string DescError = string.Empty;
                objE_ContadorDet.CodUc = cboUC.EditValue.ToString();

                LimpiarContador();

                DataTable resultado = objB_ContadorDet.ContadorDet_GetLastRecord(objE_ContadorDet, out DescError);
                if (resultado.Rows.Count > 0)
                {
                    txtContadorIni.EditValue = resultado.Rows[0]["ContadorFin"].ToString();
                    txtFechaIni.EditValue = Convert.ToDateTime(resultado.Rows[0]["FechaFin"]).ToString("MM/dd/yyyy HH:mm");
                    txtFechaFin.Focus();
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public void TraerCiclodeUnidadControl()
        {
            try
            {
                //Traer ciclo de la unidad
                string DescError = string.Empty;

                B_Perfil bPerfil = new B_Perfil();
                E_UC eUnidadControl = new E_UC();
                E_Perfil ePerfil = new E_Perfil();

                eUnidadControl.CodUc = cboUC.EditValue.ToString();
                ePerfil = bPerfil.GetPerfilByCodUC(eUnidadControl);
                if (ePerfil != null)
                {
                    txtCiclo.Text = ePerfil.Ciclo;
                    cicloPorDefecto = ePerfil.IdCicloDefecto;

                    if (ePerfil.Ciclo == CicloConst.Horas)
                    {
                        txtContadorIni.IsEnabled = false;
                        txtContadorFin.IsEnabled = false;
                    }
                    else
                    {
                        txtContadorIni.IsEnabled = true;
                        txtContadorFin.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private bool ValidaEstadoUnidadControl()
        {
            bool rpta = false;
            try
            {

                E_UC objUnidadControl = new E_UC();
                objUnidadControl.CodUc = cboUC.EditValue.ToString();
                objUnidadControl = objB_UC.B_UC_GetItemByCodUC(objUnidadControl);
                if (objUnidadControl != null)
                {
                    if (objUnidadControl.IdEstadoUC == (int)EstadoUCEnum.Registrado)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "OBLI_UC_ACTIVO"), 2);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            return rpta;
        }

        public bool VerificarTieneContador()
        {
            try
            {
                //Verifica si ya tiene un registro de contador
                if (cboUC.EditValue == null) return false;

                string DescError = string.Empty;
                objE_ContadorDet.CodUc = cboUC.EditValue.ToString();

                DataTable resultado = objB_ContadorDet.ContadorDet_GetLastRecord(objE_ContadorDet, out DescError);
                if (resultado.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

            return false;
        }

        private void txtFechaIni_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                CalcuarTiempo();
                //txtObservacion.Focus();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtFechaFin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                CalcuarTiempo();
                txtObservacion.Focus();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public void CalcuarTiempo()
        {
            try
            {

                if (cboUC.EditValue == null) return;

                if (txtCiclo.Text == CicloConst.Horas && txtFechaIni.Text != "" && txtFechaFin.Text != "")
                {
                    decimal minutos, calculoMinutos = 0;
                    minutos = DateDiff(DateInterval.Minute, Convert.ToDateTime(txtFechaIni.EditValue), Convert.ToDateTime(txtFechaFin.EditValue));

                    bool rpta = VerificarTieneContador();

                    if (rpta == false)
                    {
                        calculoMinutos = (1 * minutos) / 60;
                        txtContadorIni.EditValue = 0;
                        txtContadorFin.EditValue = calculoMinutos;
                    }
                    else
                    {
                        calculoMinutos = (1 * minutos) / 60;
                        txtContadorFin.EditValue = Convert.ToDecimal(txtContadorIni.EditValue) + calculoMinutos;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void PART_GridControl_GotFocus(object sender, RoutedEventArgs e)
        {

            try
            {
                if (cboNroOperacion.ItemsSource != null)
                {
                    dynamic b = cboNroOperacion.ItemsSource;

                    if (b.Table.Rows.Count > 0)
                    {
                        var fechaIni = Convert.ToDateTime(b.Table.Rows[0][14]).ToString("MM/dd/yyyy HH:mm");
                        var contadorIni = b.Table.Rows[0][15];
                        var fechaFin = Convert.ToDateTime(b.Table.Rows[0][16]).ToString("MM/dd/yyyy HH:mm");
                        var contadorFin = b.Table.Rows[0][17];

                        cboNroOperacion.EditValue = b.Table.Rows[0][11];
                        cboNroOperacion.Text = b.Table.Rows[0][11];

                        txtFechaIni.EditValue = fechaIni;
                        txtContadorIni.EditValue = contadorIni;
                        txtFechaFin.EditValue = fechaFin;
                        txtContadorFin.EditValue = contadorFin;

                        txtFechaIni.IsReadOnly = false;
                        txtContadorFin.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public void LimpiarContador()
        {
            try
            {
                //Limpiar contadores
                txtContadorIni.EditValue = 0.0;
                txtContadorFin.EditValue = 0.0;
                txtFechaIni.EditValue = null;
                txtFechaFin.EditValue = null;
                txtFechaIni.EditValue = string.Empty;
                txtFechaFin.EditValue = string.Empty;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private bool ValidaCorrecion()
        {
            bool rpta = false;
            try
            {

                if(Convert.ToInt32(cboTipoOperacion.EditValue)==1) return rpta;

                string DescError = string.Empty;
                objE_ContadorDet.CodUc = cboUC.EditValue.ToString();

                DataTable resultado = objB_ContadorDet.ContadorDet_GetPenultimateRecord(objE_ContadorDet, out DescError);
                if (resultado.Rows.Count > 0)
                {
                    var contadorIni= resultado.Rows[0]["ContadorIni"].ToString();
                    var fechaIni= Convert.ToDateTime(resultado.Rows[0]["FechaIni"]).ToString("MM/dd/yyyy HH:mm");
                    var contadorFin = resultado.Rows[0]["ContadorFin"].ToString();
                    var fechaFin = Convert.ToDateTime(resultado.Rows[0]["FechaFin"]).ToString("MM/dd/yyyy HH:mm");

                    if (Convert.ToDouble(txtContadorFin.Text) < Convert.ToDouble(txtContadorIni.Text))
                    {
                        rpta = true;
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "LOGI_FCON_MENO"), 2);
                    }
                    else if (Convert.ToDateTime(txtFechaFin.EditValue) < Convert.ToDateTime(txtFechaIni.DisplayText))
                    {
                        rpta = true;
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRegistroIncidencias, "LOGI_FFEC_MENO"), 2);
                    }
                }

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            return rpta;
        }

        #region REQUERIMIENTO_03_CELSA
        private void Mailer(string titulo, string UC, string PerfilComp, string vfcambio, string vcontador, string vporc, string color)
        {
            try
            {
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
                    tlvcor = objcor.UsuarioCorreoG();
                    Ltbl = tlvcor.Rows.Count;
                    string correos = "";

                    if (Ltbl > 0)
                    {
                        for (int i = 0; i < Ltbl; i++)
                        {
                            correodest = tlvcor.Rows[i]["Email"].ToString();

                            if (correodest != " ")
                            {
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
                        string mensaje = "El semáforo del componente " + PerfilComp + ", proveniente de la Unidad de Control: " + UC + " se encuentra en " + color + "." + "\n";
                        mensaje = mensaje + "Valor del contador: " + vcontador + "." + "\n";
                        mensaje = mensaje + "Valor de la frecuencia de cambio:  " + vfcambio + "." + "\n";
                        mensaje = mensaje + "Porcentaje semáforo " + color + " : " + vporc + " %." + "\n";
                        message.Body = mensaje;
                        message.Subject = titulo;
                        message.IsBodyHtml = true;
                        SmtpClient client = new SmtpClient(servidor, int.Parse(puerto));
                        client.EnableSsl = true;
                        client.Credentials = new System.Net.NetworkCredential(usuario, password);
                        client.Send(message);


                    }
                }

            }
            catch (Exception ex)

            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        #endregion
    }
}
