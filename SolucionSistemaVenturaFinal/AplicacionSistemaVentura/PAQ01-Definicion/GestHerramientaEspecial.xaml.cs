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
using System.Collections.ObjectModel;
using System.Data;
using DevExpress.Xpf.Editors.Controls;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Entities;
using Business;
using Utilitarios;
using System.Text.RegularExpressions;

namespace AplicacionSistemaVentura.PAQ01_Definicion
{
    /// <summary>
    /// Interaction logic for GestHerramientaEspecial.xaml
    /// </summary>
    public partial class GestHerramientaEspecial : UserControl
    {
        E_Herramienta objHerramienta = new E_Herramienta();
        E_HerramientaItem objHerramientaItem = new E_HerramientaItem();
        E_TablaMaestra objTablaMaestra = new E_TablaMaestra();
        Boolean bolNuevo = false; Boolean bolEdicion = false;
        DataTable tblSeries = new DataTable();
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();
        Utilitarios.DebugHandler Debug = new Utilitarios.DebugHandler();
        string gstrIDSerieEditando = "";
        int gintIdMenu = 0;
        DateTime fechamodificacion;
        string gstrEtiquetaHerramientaEspecial = "GestHerramientaEspecial";

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            //tabControl1.SelectedIndex = 0;
            GlobalClass.ip.SeleccionarTab(tabItem1);
            EstadoForm(false, false, true);
            sbAuditoria.Visibility = Visibility.Hidden;
            LimpiarControles();
        }

        private void btnEliminarHerramientaItems_Click(object sender, RoutedEventArgs e)
        {
            string NroSerie = dtgSeries.GetFocusedRowCellValue("NroSerie").ToString();
            var rpt = DevExpress.Xpf.Core.DXMessageBox.Show(string.Format("Seguro de Eliminar registro {0} ?", NroSerie), "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (rpt == MessageBoxResult.Yes)
            {
                EstadoForm(false, true, false);
                tblSeries.Rows.RemoveAt(dtgSeries.GetSelectedRowHandles()[0]);
                dtgSeries.ItemsSource = tblSeries;
                txtCantidad.Text = dtgSeries.VisibleRowCount.ToString();
            }

        }
        private void btnEditarHerramientaItems_Click(object sender, RoutedEventArgs e)
        {
            if (dtgSeries.VisibleRowCount < 1) return;
            txtSerie.Focus();
            txtSerie.Text = string.Empty;
            cboEstadoHerramientaItem.EditValue = 1;
            stkHerramientaitem.Visibility = Visibility.Visible;
            
            int Irow = dtgSeries.GetSelectedRowHandles()[0];
            gstrIDSerieEditando = Irow.ToString();
            txtSerie.Text = dtgSeries.GetCellValue(Irow, "NroSerie").ToString();
            cboEstadoHerramientaItem.EditValue = Convert.ToInt32(dtgSeries.GetCellValue(Irow, "IdEstadoDisponible"));
        }

        private void BtnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (bolNuevo == true && bolEdicion == false)
                {
                    if (ValidaCampoObligado() == true) { return; }
                    if (ValidaLogicaNegocio() == true) { return; }

                    objHerramienta.IdHerramienta = 0;
                    objHerramienta.CodHerramienta = "";
                    objHerramienta.Herramienta = txtDescripcion.Text.Trim();
                    objHerramienta.Observacion = txtObservacion.Text;
                    objHerramienta.IdEstadoH = Convert.ToInt32(cboEstado.EditValue);
                    objHerramienta.FlagActivo = 1;
                    objHerramienta.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                    objHerramienta.FechaModificacion = DateTime.Now;
                    tblSeries.Columns.Remove("Estado");
                    tblSeries.Columns.Remove("Editable");
                    tblSeries.Columns.Remove("Eliminable");
                    tblSeries.Columns.Remove("CodOT");
                    int rpta = B_Herramienta.Herramienta_UpdateCascade(objHerramienta, tblSeries);
                    if (rpta == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "GRAB_NUEV"), 1);
                    }
                    else if (rpta != 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "GRAB_CONC"), 2);
                        return;
                    }
                    
                }
                else if (bolNuevo == false && bolEdicion == true)
                {
                    if (ValidaCampoObligado() == true) { return; }
                    if (ValidaLogicaNegocio() == true) { return; }

                    objHerramienta.IdHerramienta = Convert.ToInt32(lblIdHerramienta.Content);
                    objHerramienta.CodHerramienta = txtCodigo.Text;
                    objHerramienta.Herramienta = txtDescripcion.Text.Trim();
                    objHerramienta.Observacion = txtObservacion.Text;
                    objHerramienta.IdEstadoH = Convert.ToInt32(cboEstado.EditValue);
                    objHerramienta.FlagActivo = 1;
                    objHerramienta.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                    objHerramienta.FechaModificacion = fechamodificacion;
                    tblSeries.Columns.Remove("Estado");
                    tblSeries.Columns.Remove("Editable");
                    tblSeries.Columns.Remove("Eliminable");
                    tblSeries.Columns.Remove("CodOT");
                    int rpta = B_Herramienta.Herramienta_UpdateCascade(objHerramienta, tblSeries);
                    if (rpta == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "GRAB_EDIT"), 1);
                    }
                    else if (rpta == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "GRAB_CONC"), 2);
                        return;
                    }
                    
                }

                EstadoForm(false, false, true);
                //tabControl1.SelectedIndex = 0;
                GlobalClass.ip.SeleccionarTab(tabItem1);
                ListarHerramienta();
                LimpiarControles();
                sbAuditoria.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            finally
            {
                GlobalClass.Columna_AddIFnotExits(tblSeries, "Estado");
                GlobalClass.Columna_AddIFnotExits(tblSeries, "Editable", Type.GetType("System.Boolean"));
                GlobalClass.Columna_AddIFnotExits(tblSeries, "Eliminable", Type.GetType("System.Boolean"));
                GlobalClass.Columna_AddIFnotExits(tblSeries, "CodOT");
            }
        }

        private void btnHerramientaItemAbrir(object sender, RoutedEventArgs e)
        {
            
            txtSerie.Text = string.Empty;            
            gstrIDSerieEditando = "";
            cboEstadoHerramientaItem.EditValue = 1;
            stkHerramientaitem.Visibility = Visibility.Visible;
            txtSerie.Focus();
            //cboEstadoHerramientaItem.EditValue = 1;
            EstadoForm(false, true, false);
        }

        private void btnHerramientaItemAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (txtSerie.Text == string.Empty)
            {
                GlobalClass.ip.Mensaje("Ingrese la Serie", 2);
                return;
            }
            string NroSerieNew = txtSerie.Text;
            bool bolExiste = false;
            for (int i = 0; i < dtgSeries.VisibleRowCount; i++)
            {
                if (NroSerieNew == dtgSeries.GetCellValue(i, "NroSerie").ToString())
                    if (gstrIDSerieEditando != "")
                    {
                        if (i != dtgSeries.GetSelectedRowHandles()[0])
                            bolExiste = true;
                    }
                    else bolExiste = true;
            }
            if (bolExiste)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "LOGI_DUPL_SER"), 2);
                return;
            }


            if (gstrIDSerieEditando != "")
            {
                int IRowEditing = Convert.ToInt32(gstrIDSerieEditando);
                tblSeries.Rows[IRowEditing]["NroSerie"] = txtSerie.Text;
                tblSeries.Rows[IRowEditing]["Estado"] = cboEstadoHerramientaItem.Text;
                tblSeries.Rows[IRowEditing]["IdEstadoDisponible"] = Convert.ToInt32(cboEstadoHerramientaItem.EditValue);
                dtgSeries.ItemsSource = tblSeries;
            }
            else
            {
                DataRow Fila = tblSeries.NewRow();
                Fila["NroSerie"] = txtSerie.Text;
                Fila["Estado"] = cboEstadoHerramientaItem.Text;
                Fila["IdEstadoDisponible"] = Convert.ToInt32(cboEstadoHerramientaItem.EditValue);
                Fila["FlagActivo"] = 1;
                Fila["IdHerramienta"] = Utilitarios.Utilitarios.IIfBlankZero(Utilitarios.Utilitarios.IIfNullBlank(lblIdHerramienta.Content));
                Fila["IdHerramientaItem"] = 0;
                Fila["Nuevo"] = 1;
                Fila["Editable"] = true;
                Fila["Eliminable"] = true;
                tblSeries.Rows.Add(Fila);
                dtgSeries.ItemsSource = tblSeries;
            }
            EstadoForm(false, true, false);
            txtCantidad.Text = dtgSeries.VisibleRowCount.ToString();
            stkHerramientaitem.Visibility = Visibility.Hidden;
        }

        private void btnHerramientaItemCancelar_Click(object sender, RoutedEventArgs e)
        {
            stkHerramientaitem.Visibility = Visibility.Hidden;
        }


        private void BtnModificarHerramienta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgHerrEspe.VisibleRowCount == 0) { return; }
                LimpiarControles();
                //tabControl1.SelectedIndex = 1;
                tabItem2.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "TAB1_EDIT");
                LlenarDatos();
                txtDescripcion.Focus();
                //tabControl1.SelectedIndex = 1;
                GlobalClass.ip.SeleccionarTab(tabItem2);
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnRegistrarHerramienta_Click(object sender, RoutedEventArgs e)
        {
            bolNuevo = true; bolEdicion = false;
            LimpiarControles();
            //tabControl1.SelectedIndex = 1;
            tabItem2.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "TAB1_NUEV");
            txtCodigo.Text = "Nuevo Código";
            cboEstado.EditValue = 1;
            cboEstado.IsEnabled = false;
            GlobalClass.ip.SeleccionarTab(tabItem2);
            EstadoForm(true, false, false);
            this.txtDescripcion.Focus();
            lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
            lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
            LimpiarControles();
        }

        private void cboEstado_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            bolNuevo = false;
            bolEdicion = true;
            EstadoForm(false, true, false);
        }

        private void LlenarDatos()
        {
            try
            {
                LimpiarControles();
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                txtDescripcion.EditValueChanged -= new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);
                txtObservacion.EditValueChanged -= new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);

                objHerramienta.IdHerramienta = Convert.ToInt32(dtgHerrEspe.GetFocusedRowCellValue("IdHerramienta").ToString());
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                DataTable tbl = B_Herramienta.Herramienta_GetItem(objHerramienta);
                lblIdHerramienta.Content = tbl.Rows[0]["IdHerramienta"].ToString();
                txtCodigo.Text = tbl.Rows[0]["CodHerramienta"].ToString();
                txtDescripcion.Text = tbl.Rows[0]["Herramienta"].ToString();
                txtCantidad.Text = tbl.Rows[0]["Cantidad"].ToString();
                cboEstado.EditValue = Convert.ToInt32(tbl.Rows[0]["IdEstadoH"]);
                
                if (Convert.ToInt32(tbl.Rows[0]["IdEstadoH"]) == 2)
                {
                    txtDescripcion.IsReadOnly = true;
                    txtObservacion.IsReadOnly = true;
                    buttonEdit3.IsEnabled = false;
                    dtgSeries.IsEnabled = false;
                }
                else
                {
                    txtDescripcion.IsReadOnly = false;
                    txtObservacion.IsReadOnly = false;
                    buttonEdit3.IsEnabled = true;
                    dtgSeries.IsEnabled = true;
                }

                txtObservacion.Text = tbl.Rows[0]["Observacion"].ToString();
                cboEstado.IsEnabled = true;
                lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tbl.Rows[0]["UsuarioCreacion"].ToString(), tbl.Rows[0]["FechaCreacion"].ToString(), tbl.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tbl.Rows[0]["UsuarioModificacion"].ToString(), tbl.Rows[0]["FechaModificacion"].ToString(), tbl.Rows[0]["HostModificacion"].ToString());
                objHerramientaItem.IdHerramienta = Convert.ToInt32(dtgHerrEspe.GetFocusedRowCellValue("IdHerramienta").ToString());
                DataTable tblDetalle = B_HerramientaItem.Herramientaitem_List(objHerramientaItem);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                for (int i = 0; i < tblDetalle.Rows.Count; i++)
                {
                    DataRow fila = tblSeries.NewRow();
                    fila["NroSerie"] = tblDetalle.Rows[i]["NroSerie"];
                    fila["IdEstadoDisponible"] = tblDetalle.Rows[i]["IdEstadoDisponible"];
                    fila["Estado"] = tblDetalle.Rows[i]["Estado"];
                    fila["FlagActivo"] = tblDetalle.Rows[i]["FlagActivo"];
                    fila["IdHerramienta"] = tblDetalle.Rows[i]["IdHerramienta"];
                    fila["IdHerramientaItem"] = tblDetalle.Rows[i]["IdHerramientaItem"];
                    fila["CodOT"] = tblDetalle.Rows[i]["CodOT"];
                    fila["Nuevo"] = 0;
                    fila["Editable"] = !(Convert.ToInt32(fila["IdEstadoDisponible"]) == 4);//Si esta ASignado, No editar
                    fila["Eliminable"] = false;
                    
                    tblSeries.Rows.Add(fila);
                }
                dtgSeries.ItemsSource = tblSeries;
                fechamodificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                //tabControl1.SelectedIndex = 1;
                txtObservacion.EditValueChanged += new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);
                txtDescripcion.EditValueChanged += new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgHerrEspe_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgHerrEspe.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodHerramienta")
                    {
                        e.Handled = true;
                        GlobalClass.ip.SeleccionarTab(tabItem2);
                        EstadoForm(false, false, true);
                        LlenarDatos();
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
                    tabItem2.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "TAB1_CONS");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "BTNG_CONS");
                    //tabItem1.IsEnabled = true;
                    //GlobalClass.ip.SeleccionarTab(tabItem1);
                    sbAuditoria.Visibility = Visibility.Visible;
                }
                else if ((bolNuevo == true) && (bolEdicion == false))
                {
                    tabItem2.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "TAB1_NUEV");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "BTNG_NUEV");
                    //tabItem1.IsEnabled = false;
                    //GlobalClass.ip.SeleccionarTab(tabItem2);
                    sbAuditoria.Visibility = Visibility.Visible;
                }
                else if ((bolNuevo == false) && (bolEdicion == true))
                {
                    tabItem2.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "TAB1_EDIT");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "BTNG_EDIT");
                    //tabItem1.IsEnabled = false;
                    //GlobalClass.ip.SeleccionarTab(tabItem2);
                    sbAuditoria.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public GestHerramientaEspecial()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        private void LimpiarControles()
        {
            cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
            txtObservacion.EditValueChanged -= new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);
            txtDescripcion.EditValueChanged -= new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);

            txtCodigo.Text = "Nuevo Codigo";
            lblIdHerramienta.Content = null;
            txtCantidad.Text = "0";
            txtDescripcion.Text = "";
            txtObservacion.Text = "";
            dtgSeries.ItemsSource = null;
            tblSeries.Rows.Clear();

            txtDescripcion.IsReadOnly = false;
            txtObservacion.IsReadOnly = false;
            buttonEdit3.IsEnabled = true;
            dtgSeries.IsEnabled = true;

            txtDescripcion.EditValueChanged += new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);
            txtObservacion.EditValueChanged += new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);
            cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
        }

        void LimpiarControlesDeGrilla(Grid grilla)
        {
            try
            {
                //NO FUNCIONABA POR EL GRID QUE ESTA EN EL TAB Y LOS TEXTBOX
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
                {
                    var control = VisualTreeHelper.GetChild(grilla, i);

                    if (control is TextBox)
                    {
                        (control as TextBox).Text = string.Empty;
                    }
                    else if (control is ComboBoxEdit)
                    {
                        (control as ComboBoxEdit).SelectedIndex = -1;
                    }
                }
                //lblMsg.Content = string.Empty;
            }
            catch (Exception ex)
            {
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarHerramienta()
        {
            if ((bool)rdbTodos.IsChecked)
                objHerramienta.IdEstadoH = 0;
            else if ((bool)rdbActivo.IsChecked)
                objHerramienta.IdEstadoH = 1;
            else if ((bool)rdbInactivo.IsChecked)
                objHerramienta.IdEstadoH = 2;
            try
            {
                dtgHerrEspe.ItemsSource = B_Herramienta.Herramienta_List(objHerramienta);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        
        private void rdbActivo_Checked(object sender, RoutedEventArgs e)
        {
            ListarHerramienta();
        }

        private void rdbInactivo_Checked(object sender, RoutedEventArgs e)
        {
            ListarHerramienta();
        }

        private void rdbTodos_Checked(object sender, RoutedEventArgs e)
        {
            ListarHerramienta();
        }

        private void UserControl_Loaded()
        {
            try
            {
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                
                tabItem1.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "TAB0_CAPT");
                dtgHerrEspe.AutoGenerateColumns = AutoGenerateColumnsMode.None;
                dtgSeries.AutoGenerateColumns = AutoGenerateColumnsMode.None;

                txtSerie.MaxLength = 50;
                tblSeries.Columns.Clear();
                tblSeries.Columns.Add("IdHerramientaItem");
                tblSeries.Columns.Add("IdHerramienta");
                tblSeries.Columns.Add("NroSerie");
                tblSeries.Columns.Add("IdEstadoDisponible");
                tblSeries.Columns.Add("FlagActivo");
                tblSeries.Columns.Add("Nuevo");
                tblSeries.Columns.Add("Estado");
                tblSeries.Columns.Add("CodOT");
                tblSeries.Columns.Add("Editable", Type.GetType("System.Boolean"));
                tblSeries.Columns.Add("Eliminable", Type.GetType("System.Boolean"));

                objTablaMaestra.IdTabla = 8;//Tabla FlagActivo
                cboEstado.ItemsSource =B_TablaMaestra.TablaMaestra_Combo(objTablaMaestra);
                cboEstado.DisplayMember = "Descripcion";
                cboEstado.ValueMember = "IdColumna";
                cboEstado.IsTextEditable = false;

                objTablaMaestra.IdTabla = 9;                
                DataView dtvCboEstado = B_TablaMaestra.TablaMaestra_Combo(objTablaMaestra).DefaultView;
                dtvCboEstado.RowFilter = "IdColumna <> 4";//NO MOSTRAMOS EL ESTADO Asignado;
                cboEstadoHerramientaItem.ItemsSource = dtvCboEstado;
                cboEstadoHerramientaItem.DisplayMember = "Descripcion";
                cboEstadoHerramientaItem.ValueMember = "IdColumna";
                cboEstadoHerramientaItem.IsTextEditable = false;
                lblIdHerramienta.Visibility = Visibility.Hidden;
                txtCodigo.IsEnabled = false;
                rdbActivo.IsChecked = true;

                GlobalClass.ip.SeleccionarTab(tabItem1);

                #region VisualizacionBotonImprimir
                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                if (!VisualizaBotonImprimirDetalle)
                {
                    btnImprimir.Visibility = System.Windows.Visibility.Hidden;
                }
                #endregion

                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
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
            if (txtDescripcion.Text.Trim() == "")
            {
                bolRpta = true;
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "OBLI_DESC"), 2);
                txtDescripcion.Focus();
            }
            else if (Utilitarios.Utilitarios.IsNumeric(txtDescripcion.Text.Trim()))
            {
                bolRpta = true;
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "OBLI_DESCNUM"), 2);
                txtDescripcion.Focus();
            }
            else if (cboEstado.SelectedIndex == -1)
            {
                bolRpta = true;
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "OBLI_ESTA"), 2);
                cboEstado.Focus();
            }
            else if (txtCodigo.Text.Trim() == "")
            {
                bolRpta = true;
                txtCodigo.Focus();
            }
            else if (!Utilitarios.Utilitarios.IsNumeric(txtCantidad.Text))
            {
                bolRpta = true;
                txtCantidad.Focus();
            }
            else if (txtCantidad.Text.Trim() == "")
            {
                bolRpta = true;
                txtCantidad.Focus();
            }
            return bolRpta;
        }

        private bool ValidaLogicaNegocio()
        {
            bool bolRpta = false;
            try
            {
                foreach (DataRow f in tblSeries.Select("IdEstadoDisponible <> 4"))
                {
                    objHerramientaItem.IdHerramientaItem = Convert.ToInt32(f["IdHerramientaItem"]);
                    DataRow FilaBD = B_HerramientaItem.Herramientaitem_GetItem(objHerramientaItem).Rows[0];
                    if (Convert.ToInt32(FilaBD["IdEstadoDisponible"]) == 4)
                    {
                        GlobalClass.ip.Mensaje(string.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "LOGI_SERI_ASIG"), FilaBD["NroSerie"]), 2);
                        bolRpta = true;
                        break;
                    }
                }

                if ((bolNuevo == true) && (bolEdicion == false))
                {
                    objHerramienta.IdHerramienta = 0;
                }
                else if ((bolNuevo == false) && (bolEdicion == true))
                {
                    objHerramienta.IdHerramienta = Convert.ToInt32(dtgHerrEspe.GetFocusedRowCellValue("IdHerramienta").ToString());
                }
                objHerramienta.Herramienta = txtDescripcion.Text.Trim();
                DataTable tblConsulta = B_Herramienta.Herramienta_GetItemByDesc(objHerramienta);
                if (tblConsulta.Rows.Count > 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "LOGI_DUPL"), 2);
                    txtDescripcion.Focus();
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

        private bool ValidaSerieHerramienta()
        {
            bool bolRpta = false;
            try
            {
                objHerramientaItem.IdHerramienta = Convert.ToInt32(txtCodigo.Text);
                objHerramientaItem.NroSerie = txtSerie.Text;
                DataTable tblConsulta = B_HerramientaItem.HerramientaItem_GetItemByDesc(objHerramientaItem);

                if (tblConsulta.Rows.Count > 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaHerramientaEspecial, "LOGI_DUPL_SER"), 2);
                    txtSerie.Focus();
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
        
        private void stkHerramientaitem_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GlobalClass.ip.VentanaEmergente_Visibilidad(sender);
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //int IdEstado = 0;
                //if ((bool)rdbTodos.IsChecked)
                //    IdEstado = 0;
                //else if ((bool)rdbActivo.IsChecked)
                //    IdEstado = 1;
                //else if ((bool)rdbInactivo.IsChecked)
                //    IdEstado = 2;
                //GlobalClass.GeneraImpresion(gintIdMenu, IdEstado);
            }
            catch { }
        }

        private void txtPLANTILLA_EditValueChanged(object sender, EditValueChangedEventArgs e)
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
    }
}
