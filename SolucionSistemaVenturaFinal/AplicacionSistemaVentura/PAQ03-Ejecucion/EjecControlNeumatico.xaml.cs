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
    /// Interaction logic for EjecControlNeumatico.xaml
    /// </summary>
    public partial class EjecControlNeumatico : UserControl
    {
        Utilitarios.ErrorHandler Error = new Utilitarios.ErrorHandler();
        Utilitarios.DebugHandler Debug = new Utilitarios.DebugHandler();
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        int IdNC = 0;
        E_Neumatico_Control objE_Neumatico_Control = new E_Neumatico_Control();
        B_Neumatico_Control objB_Neumatico_Control = new B_Neumatico_Control();
        B_Ciclo objB_Ciclo = new B_Ciclo();
        B_UC objB_UC = new B_UC();
        E_UC objE_UC = new E_UC();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        E_Neumatico_ControlDet objE_Neumatico_ControlDet = new E_Neumatico_ControlDet();
        B_Neumatico_ControlDet objB_Neumatico_ControlDet = new B_Neumatico_ControlDet();
        E_Perfil objE_Perfil = new E_Perfil();
        DataView dtvMaestra = new DataView();
        DataTable tblNCDet = new DataTable();
        DateTime FechaModificacion;
        string gstrEtiquetaControlNeumatico = "EjecControlNeumatico";
        int gintIdMenu = 0;

        public EjecControlNeumatico()
        {            
            InitializeComponent();
        }
        
        private void ListarCN()
        {
            objE_Neumatico_Control.IdEstadoNC = 0;
            dtgCN.ItemsSource = objB_Neumatico_Control.Neumatico_Control_List(objE_Neumatico_Control).DefaultView;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);

                ListarCN();
                
                txtComen.MaxLength = 200;
                txtComen.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tabLista);

                objE_UC.IdPerfil = 0;
                CboUnidadControl.ItemsSource = objB_UC.B_UC_ComboWithPN(objE_UC);
                CboUnidadControl.DisplayMember = "PlacaSerie";
                CboUnidadControl.ValueMember = "IdUC";
                CboUnidadControl.SelectedIndex = -1;

                objE_TablaMaestra.IdTabla = 0;
                dtvMaestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).DefaultView;
                CboEstado.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=31", dtvMaestra);
                CboEstado.DisplayMember = "Descripcion";
                CboEstado.ValueMember = "IdColumna";
                CboEstado.SelectedIndex = -1;

                tblNCDet.Columns.Add("IdNCD", Type.GetType("System.Int32"));
                tblNCDet.Columns.Add("IdNC", Type.GetType("System.Int32"));
                tblNCDet.Columns.Add("IdEje", Type.GetType("System.Int32"));                
                tblNCDet.Columns.Add("IdPosicionCliente", Type.GetType("System.Int32"));
                tblNCDet.Columns.Add("TipoNeumatico", Type.GetType("System.String"));
                tblNCDet.Columns.Add("IdNeumatico", Type.GetType("System.Int32"));
                tblNCDet.Columns.Add("NroSerie", Type.GetType("System.String"));
                tblNCDet.Columns.Add("IdCiclo", Type.GetType("System.Int32"));
                tblNCDet.Columns.Add("Ciclo", Type.GetType("System.String"));                
                tblNCDet.Columns.Add("Contador", Type.GetType("System.Double"));//          Estan invertidor
                tblNCDet.Columns.Add("ContadorAnterior", Type.GetType("System.Double"));//  a proposito, para el registro
                tblNCDet.Columns.Add("FlagReencauche", Type.GetType("System.Boolean"));
                tblNCDet.Columns.Add("Observacion", Type.GetType("System.String"));
                tblNCDet.Columns.Add("IdEstadoNCD", Type.GetType("System.Int32"));
                tblNCDet.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblNCDet.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                tblNCDet.Columns.Add("Eje", Type.GetType("System.String"));
                tblNCDet.Columns.Add("ContadorReencauche", Type.GetType("System.Int32"));
                tblNCDet.Columns.Add("ContadorAnterior2", Type.GetType("System.Double"));

                #region VisualizacionBotonImprimir
                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                if (!VisualizaBotonImprimirDetalle)
                {
                    btnImprimir.Visibility = System.Windows.Visibility.Hidden;
                }
                #endregion

                CboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);                
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            CboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);
            LimpiarControles();
            dtgDetNeum.Columns["Accion"].Visible = true;            
            LbLNroDocumento.Content = "Nuevo Codigo";
            CboUnidadControl.IsEnabled = true;
            mskFecha.IsEnabled = true;
            CboEstado.SelectedIndex = 0;
            CboEstado.IsEnabled = false;
            FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
            EstadoForm(true, false, true);
            GlobalClass.ip.SeleccionarTab(tabDetalle);
            CboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);
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

                    tabDetalle.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "TAB1_CONS1");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "BTNG_CONS1");
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tabDetalle.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "TAB1_NUEV1");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "BTNG_NUEV1");
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tabDetalle.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "TAB1_EDIT1");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "BTNG_EDIT1");
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgCN_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgCN.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodNC")
                    {
                        e.Handled = true;
                        CboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);
                        IdNC = Convert.ToInt32(dtgCN.GetFocusedRowCellValue("IdNC").ToString());
                        objE_Neumatico_Control.IdNC = IdNC;

                        DataTable tblNeumaticoControl = objB_Neumatico_Control.NeumaticoControl_GetItem(objE_Neumatico_Control);
                        LbLNroDocumento.Content = tblNeumaticoControl.Rows[0]["CodNC"].ToString();
                        CboUnidadControl.EditValue = Convert.ToInt32(tblNeumaticoControl.Rows[0]["IdUC"]);
                        CboEstado.EditValue = Convert.ToInt32(tblNeumaticoControl.Rows[0]["IdEstadoNC"]);
                        mskFecha.EditValue = Convert.ToDateTime(tblNeumaticoControl.Rows[0]["FechaControl"]);
                        ListarDetalleNeumatico(IdNC, true);

                        lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblNeumaticoControl.Rows[0]["UsuarioCreacion"], tblNeumaticoControl.Rows[0]["FechaCreacion"], tblNeumaticoControl.Rows[0]["HostCreacion"]);
                        lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblNeumaticoControl.Rows[0]["UsuarioModificacion"], tblNeumaticoControl.Rows[0]["FechaModificacion"], tblNeumaticoControl.Rows[0]["HostModificacion"]);
                        

                        EstadoForm(false, false, true);
                        GlobalClass.ip.SeleccionarTab(tabDetalle);
                        
                        mskFecha.IsEnabled = false;
                        CboUnidadControl.IsEnabled = false;
                        FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                        dtgDetNeum.Columns["Accion"].Visible = false;
                        CboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);
                    }
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
                if (ValidaCampoObligado() == true) { return; }
                if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    //cuando el Contador nuevo es 0
                    foreach (DataRow f in tblNCDet.Rows)
                    {
                        if (Convert.ToInt32(f["ContadorAnterior"]) == 0)
                        {
                            f["ContadorAnterior"] = f["Contador"];
                            f["Contador"] = f["ContadorAnterior2"];
                        }
                    }

                    Utilitarios.Utilitarios.gintIdUsuario = 1;
                    objE_Neumatico_Control.IdNC = 0;
                    objE_Neumatico_Control.CodNC = "";
                    objE_Neumatico_Control.IdUC = Convert.ToInt32(CboUnidadControl.EditValue);
                    objE_Neumatico_Control.IdEstadoNC = Convert.ToInt32(CboEstado.EditValue);
                    objE_Neumatico_Control.FlagActivo = true;
                    objE_Neumatico_Control.FechaControl = Convert.ToDateTime(mskFecha.EditValue);
                    objE_Neumatico_Control.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
                    objE_Neumatico_Control.FechaModificacion = FechaModificacion;
                    tblNCDet.Columns.Remove("Eje");
                    tblNCDet.Columns.Remove("ContadorReencauche");
                    tblNCDet.Columns.Remove("ContadorAnterior2");

                    int rpta = objB_Neumatico_Control.NeumaticoControl_UpdateCascada(objE_Neumatico_Control, tblNCDet);
                    if (rpta == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "GRAB_NUEV1"), 1);
                    }
                    else if (rpta == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "GRAB_CONC"), 2);
                        return;
                    }
                }
                LimpiarControles();
                ListarCN();
                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tabLista);

                dtgDetNeum.Columns["Accion"].Visible = true;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            finally
            {
                GlobalClass.Columna_AddIFnotExits(tblNCDet, "Eje", Type.GetType("System.String"));
                GlobalClass.Columna_AddIFnotExits(tblNCDet, "ContadorReencauche", Type.GetType("System.Int32"));
                GlobalClass.Columna_AddIFnotExits(tblNCDet, "ContadorAnterior2", Type.GetType("System.Double"));
                tblNCDet.DefaultView.RowFilter = "";
            }
        }

        private void LimpiarControles()
        {
            dtgDetNeum.ItemsSource = null;
            CboEstado.SelectedIndex = -1;
            CboUnidadControl.SelectedIndexChanged -= new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);
            CboUnidadControl.SelectedIndex = -1;
            CboUnidadControl.SelectedIndexChanged += new RoutedEventHandler(CboUnidadControl_SelectedIndexChanged);
            mskFecha.Text = "";
            LbLNroDocumento.Content = "Nuevo Codigo";
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LimpiarControles();
                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tabLista);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void CboUnidadControl_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                ListarDetalleNeumatico(Convert.ToInt32(CboUnidadControl.EditValue),false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAceptarPopUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (ValidaCampoObligadoPopup() == true) return;

                int IdNeumatico = Convert.ToInt32(dtgDetNeum.GetFocusedRowCellValue("IdNeumatico"));

                for (int i = 0; i < tblNCDet.Rows.Count; i++)
                {
                    if (IdNeumatico == Convert.ToInt32(tblNCDet.Rows[i]["IdNeumatico"]))
                    {
                        tblNCDet.Rows[i]["ContadorAnterior"] = Convert.ToDouble(txtContNuev.Text);
                        tblNCDet.Rows[i]["Observacion"] = txtComen.Text;
                        tblNCDet.Rows[i]["FlagReencauche"] = ((bool)chkReencauche.IsChecked) ? true : false;
                        tblNCDet.Rows[i]["Nuevo"] = true;
                        break;
                    }
                }
                dtgDetNeum.ItemsSource = tblNCDet;
                dcpEdicion.Visibility = Visibility.Collapsed;
                LimpiarPopUp();

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelarPopUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LimpiarPopUp();
                dcpEdicion.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAbrirPopUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dcpEdicion.Visibility = Visibility.Visible;
                LimpiarPopUp();
                chkReencauche.IsChecked = (bool)dtgDetNeum.GetFocusedRowCellValue("FlagReencauche");
                lblPosi.Content = Convert.ToInt32(dtgDetNeum.GetFocusedRowCellValue("IdPosicionCliente").ToString());
                lblCont.Content = Convert.ToDouble(dtgDetNeum.GetFocusedRowCellValue("Contador").ToString());
                txtComen.Text = dtgDetNeum.GetFocusedRowCellValue("Observacion").ToString();
                txtContNuev.Text = dtgDetNeum.GetFocusedRowCellValue("ContadorAnterior").ToString();
                txtContNuev.Focus();
                lblCiclo.Content = dtgDetNeum.GetFocusedRowCellValue("Ciclo").ToString();

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LimpiarPopUp()
        {
            txtComen.Clear();
            lblPosi.Content = "";
            chkReencauche.IsChecked = false;
        }
        
        private void ListarDetalleNeumatico(int Id,bool Consulta)
        {
            DataTable tbl = new DataTable();
            if (Consulta)
            {
                objE_Neumatico_ControlDet.IdNC = Id;
                tbl=objB_Neumatico_ControlDet.NeumaticoControlDet_List(objE_Neumatico_ControlDet);
                //Invertimos los valores para el usuario
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var aux = tbl.Rows[i]["Contador"];
                    tbl.Rows[i]["Contador"] = tbl.Rows[i]["ContadorAnterior"];
                    tbl.Rows[i]["ContadorAnterior"] = aux;
                }
                dtgDetNeum.ItemsSource = tbl;
            }
            else
            {
                objE_Neumatico_ControlDet.IdUC = Id;
                tbl = objB_Neumatico_ControlDet.NeumaticoControlDet_ListByUC(objE_Neumatico_ControlDet);

                tblNCDet.Rows.Clear();
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    DataRow rowNCDet = tblNCDet.NewRow();
                    rowNCDet["IdNCD"] = 0;
                    rowNCDet["IdNC"] = Convert.ToInt32(tbl.Rows[i]["IdNC"].ToString());
                    rowNCDet["IdEje"] = tbl.Rows[i]["IdEje"];
                    rowNCDet["Eje"] = tbl.Rows[i]["Eje"];
                    rowNCDet["IdPosicionCliente"] = Convert.ToInt32(tbl.Rows[i]["IdPosicionCliente"].ToString());
                    rowNCDet["IdNeumatico"] = Convert.ToInt32(tbl.Rows[i]["IdNeumatico"].ToString());
                    rowNCDet["TipoNeumatico"] = tbl.Rows[i]["TipoNeumatico"].ToString();
                    rowNCDet["NroSerie"] = tbl.Rows[i]["NroSerie"].ToString();
                    rowNCDet["IdCiclo"] = Convert.ToInt32(tbl.Rows[i]["IdCiclo"]);
                    rowNCDet["Ciclo"] = tbl.Rows[i]["Ciclo"].ToString();
                    rowNCDet["Contador"] = Convert.ToDouble(tbl.Rows[i]["Contador"]);
                    rowNCDet["ContadorAnterior"] = (Consulta) ? Convert.ToDouble(tbl.Rows[i]["ContadorAnterior"]) : 0;
                    rowNCDet["FlagReencauche"] = false;
                    rowNCDet["ContadorReencauche"] = Convert.ToInt32(tbl.Rows[i]["ContadorReencauche"]);
                    rowNCDet["Observacion"] = string.Empty;
                    rowNCDet["IdEstadoNCD"] = Convert.ToInt32(tbl.Rows[i]["IdEstadoNCD"].ToString());
                    rowNCDet["FlagActivo"] = Convert.ToBoolean(tbl.Rows[i]["FlagActivo"].ToString());
                    rowNCDet["Nuevo"] = true;
                    rowNCDet["ContadorAnterior2"] = Convert.ToDouble(tbl.Rows[i]["ContadorAnterior"]);
                    tblNCDet.Rows.Add(rowNCDet);
                }
                dtgDetNeum.ItemsSource = tblNCDet;
            }
        }
        
        private bool ValidaCampoObligado()
        {
            bool bolRpta = false;
            try
            {
                if (CboUnidadControl.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "OBLI_UC"), 2);
                    CboUnidadControl.Focus();
                }
                else if (dtgDetNeum.VisibleRowCount <= 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "OBLI_NEUM_ASIG"), 2);
                }
                if (mskFecha.EditValue.ToString() == "")
                {
                    bolRpta = true;
                    mskFecha.Focus();
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "OBLI_FECH"), 2);
                }
                else
                {
                    DateTime Hoy = Convert.ToDateTime(Utilitarios.Utilitarios.Fecha_Hora_Servidor().Rows[0]["FechaServer"]);
                    if (Convert.ToDateTime(mskFecha.EditValue) >= Hoy.AddDays(1))
                    {
                        bolRpta = true;
                        mskFecha.Focus();
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "LOGI_FECH"), 2);                        
                    }

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

        private bool ValidaCampoObligadoPopup()
        {
            bool bolRpta = false;
            Double Contador = Convert.ToDouble(lblCont.Content);

            try
            {
                if ((bool)chkReencauche.IsChecked)
                {
                    if (Convert.ToDouble(txtContNuev.Text) < Contador)
                    {
                        bolRpta = true;
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "LOGI_CONT_MENO"), 2);
                        txtContNuev.Focus();
                    }                    
                }
                else
                {                    
                    if (Convert.ToDouble(txtContNuev.Text) > Contador)
                    {
                        bolRpta = true;
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "LOGI_CONT_MAYO"), 2);
                        txtContNuev.Focus();
                    }
                }
                if (Convert.ToDouble(txtContNuev.Text) == 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaControlNeumatico, "OBLI_CONT"), 2);
                    txtContNuev.Focus();
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                
            }
            return bolRpta;
        }

        private void PLANTILLA_VentanaEmergente_IsVisible(object sender, DependencyPropertyChangedEventArgs e)
        {
            GlobalClass.ip.VentanaEmergente_Visibilidad(sender);
        }


        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalClass.GeneraImpresion(gintIdMenu, IdNC);
            }
            catch { }
        }
    }
}
