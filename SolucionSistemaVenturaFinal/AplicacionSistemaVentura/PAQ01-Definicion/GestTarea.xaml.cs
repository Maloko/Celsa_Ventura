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
using DevExpress.Xpf.Editors.Controls;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Editors;
using Entities;
using Business;
using Utilitarios;
using System.Text.RegularExpressions;

namespace AplicacionSistemaVentura.PAQ01_Definicion
{
    /// <summary>
    /// Interaction logic for GestTarea.xaml
    /// </summary>
    public partial class GestTarea : UserControl
    {
        int gintIdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
        string gstrUsuario = Utilitarios.Utilitarios.gstrUsuario;
        E_Tarea objTarea = new E_Tarea();
        E_TablaMaestra objTablaMaestra = new E_TablaMaestra();
        E_Actividad objActividad = new E_Actividad();
        B_Actividad objB_Actividad = new B_Actividad();
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();
        Utilitarios.DebugHandler Debug = new Utilitarios.DebugHandler();
        
        SolidColorBrush gsbcolorFocus = System.Windows.Media.Brushes.LightYellow;
        SolidColorBrush gsbcolorNoFocus = System.Windows.Media.Brushes.White;
        int gintIdMenu = 0;

        string gstrEtiquetaTarea = "GestTarea";

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tabListado);
                //tabControl1.SelectedIndex = 0;
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnGrabar_Click(object sender, RoutedEventArgs e)
        {
            objTarea.CodTarea = txtCodigo.Text;
            objTarea.Tarea = txtDescripcion.Text;
            objTarea.IdActividad = Convert.ToInt32(cboActividad.EditValue);
            objTarea.IdEstadoT = Convert.ToInt32(cboEstado.EditValue);
            objTarea.FlagActivo = 1;

            try
            {
                if (gbolNuevo == true && gbolEdicion == false)
                {
                    if (ValidaCampoObligado() == true) { return; }
                    if (ValidaLogicaNegocio() == true) { return; }
                    objTarea.IdTarea = 0;
                    objTarea.IdUsuarioCreacion = gintIdUsuario;
                    int iderror = B_Tarea.Tarea_Insert(objTarea);
                    if (iderror == 1)//********************
                    {
                        EstadoForm(false, false, true);
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "GRAB_NUEV"), 1);
                    }
                    else if (iderror == 1205)//*****************
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "GRAB_CONC"), 2);
                        return;
                    }
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    if (ValidaCampoObligado() == true) { return; }
                    if (ValidaLogicaNegocio() == true) { return; }
                    objTarea.IdTarea = Convert.ToInt32(lblIdTarea.Content);
                    objTarea.IdUsuarioModificacion = gintIdUsuario;
                    objTarea.FechaModificacion = Convert.ToDateTime(Utilitarios.Utilitarios.IIfDBNull(dtgTarea.GetFocusedRowCellValue("FechaModificacion"), DateTime.Now));
                    int iderror = B_Tarea.Tarea_Update(objTarea);
                    if (iderror == 1)
                    {
                        EstadoForm(false, false, true);
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "GRAB_EDIT"), 1);
                    }
                    else if (iderror == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (iderror == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "GRAB_CONC"), 2);
                        return;
                    }

                }
                LimpiarCampos();
                GlobalClass.ip.SeleccionarTab(tabListado);
                ListarTarea();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgTarea.VisibleRowCount == 0) { return; }
            EstadoForm(false, true, false);
            GlobalClass.ip.SeleccionarTab(tabDatos);
            LlenarDatos();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(true, false, false);
                //tabControl1.SelectedIndex = 1;
                GlobalClass.ip.SeleccionarTab(tabDatos);
                LimpiarCampos();
                cboEstado.IsEnabled = false;
                sbAuditoria.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void cboEstado_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            EstadoForm(false, true, false);
        }

        private void cboActividad_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            EstadoForm(false, true, false);
        }

        private void LlenarDatos()
        {
            try
            {
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboActividad.SelectedIndexChanged -= new RoutedEventHandler(cboActividad_SelectedIndexChanged);
                txtDescripcion.EditValueChanged -= new EditValueChangedEventHandler(txtDescripcion_EditValueChanged);
                objTarea.IdTarea = Convert.ToInt32(dtgTarea.GetFocusedRowCellValue("IdTarea"));
                DataTable tbl = B_Tarea.Tarea_GetItem(objTarea);
                lblIdTarea.Content = tbl.Rows[0]["IdTarea"];
                txtCodigo.Text = tbl.Rows[0]["CodTarea"].ToString();
                txtDescripcion.Text = tbl.Rows[0]["Tarea"].ToString();
                cboEstado.EditValue = Convert.ToInt32(tbl.Rows[0]["IdEstadoT"]);

                if (Convert.ToInt32(tbl.Rows[0]["IdEstadoT"]) == 2)
                {
                    txtDescripcion.IsReadOnly = true;
                    cboActividad.IsEnabled = false;
                }
                else
                {
                    txtDescripcion.IsReadOnly = false;
                    cboActividad.IsEnabled = true;
                }

                cboActividad.EditValue = Convert.ToInt32(tbl.Rows[0]["IdActividad"]);

                lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tbl.Rows[0]["UsuarioCreacion"].ToString(), tbl.Rows[0]["FechaCreacion"].ToString(), tbl.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tbl.Rows[0]["UsuarioModificacion"].ToString(), tbl.Rows[0]["FechaModificacion"].ToString(), tbl.Rows[0]["HostModificacion"].ToString());

                cboEstado.IsEnabled = true;
                //tabControl1.SelectedIndex = 1;
                sbAuditoria.Visibility = Visibility.Visible;
                txtDescripcion.EditValueChanged += new EditValueChangedEventHandler(txtDescripcion_EditValueChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboActividad.SelectedIndexChanged += new RoutedEventHandler(cboActividad_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgTarea_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgTarea.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodTarea")
                    {
                        e.Handled = true;
                        EstadoForm(false, false, true);
                        GlobalClass.ip.SeleccionarTab(tabDatos);
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
                    tabDatos.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "TAB1_CONS");
                    btnGraDato.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "BTNG_CONS");
                    //GlobalClass.ip.SeleccionarTab(tabListado);
                    //tabListado.IsEnabled = true;
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tabDatos.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "TAB1_NUEV");
                    btnGraDato.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "BTNG_NUEV");
                    //GlobalClass.ip.SeleccionarTab(tabDatos);
                    //tabListado.IsEnabled = false;
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tabDatos.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "TAB1_EDIT");
                    btnGraDato.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "BTNG_EDIT");
                    //GlobalClass.ip.SeleccionarTab(tabDatos);
                    //tabListado.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public GestTarea()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        private void LimpiarCampos()
        {
            cboActividad.SelectedIndexChanged -= new RoutedEventHandler(cboActividad_SelectedIndexChanged);
            cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
            txtDescripcion.EditValueChanged -= new EditValueChangedEventHandler(txtDescripcion_EditValueChanged);

            txtCodigo.Text = "Nuevo Código";
            txtDescripcion.Text = "";
            cboActividad.EditValue = 0;
            cboEstado.EditValue = 1;
            sbAuditoria.Visibility = Visibility.Hidden;
            txtDescripcion.IsReadOnly = false;
            cboActividad.IsEnabled = true;

            txtDescripcion.EditValueChanged += new EditValueChangedEventHandler(txtDescripcion_EditValueChanged);
            cboActividad.SelectedIndexChanged += new RoutedEventHandler(cboActividad_SelectedIndexChanged);
            cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
        }

        private void LimpiarControlesDeGrilla(Grid grilla)
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

        private void ListarTarea()
        {
            if ((bool)rdbTodos.IsChecked)
                objTarea.IdEstadoT = 0;
            else if ((bool)rdbActivo.IsChecked)
                objTarea.IdEstadoT = 1;
            else if ((bool)rdbInactivo.IsChecked)
                objTarea.IdEstadoT = 2;
            try
            {
                dtgTarea.AutoGenerateColumns = DevExpress.Xpf.Grid.AutoGenerateColumnsMode.None;
                dtgTarea.ItemsSource = B_Tarea.Tarea_List(objTarea);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        
        private void rdbActivo_Checked(object sender, RoutedEventArgs e)
        {
            ListarTarea();
        }

        private void rdbInactivo_Checked(object sender, RoutedEventArgs e)
        {
            ListarTarea();
        }

        private void rdbTodos_Checked(object sender, RoutedEventArgs e)
        {
            ListarTarea();
        }
        private void UserControl_Loaded()
        {
            try
            {
                cboActividad.SelectedIndexChanged -= new RoutedEventHandler(cboActividad_SelectedIndexChanged);
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);

                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tabListado);

                cboEstado.IsTextEditable = false;

                objTablaMaestra.IdTabla = 3;//Tabla Estado de Tarea
                cboEstado.ItemsSource = B_TablaMaestra.TablaMaestra_Combo(objTablaMaestra);
                cboEstado.DisplayMember = "Descripcion";
                cboEstado.ValueMember = "IdColumna";
                rdbActivo.IsChecked = true;

                cboActividad.ItemsSource = objB_Actividad.Actividad_Combo();
                cboActividad.DisplayMember = "Actividad";
                cboActividad.ValueMember = "IdActividad";

                tabListado.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "TAB0_CAPT");
                sbAuditoria.Visibility = Visibility.Hidden;
                
                #region VisualizacionBotonImprimir
                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                if (!VisualizaBotonImprimirDetalle)
                {
                    btnImprimir.Visibility = System.Windows.Visibility.Hidden;
                }
                #endregion

                cboActividad.SelectedIndexChanged += new RoutedEventHandler(cboActividad_SelectedIndexChanged);
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
            try
            {
                if (txtDescripcion.Text.Trim() == "")
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "OBLI_DESC"), 2);
                    txtDescripcion.Focus();
                }
                else if (cboEstado.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "OBLI_ESTA"), 2);
                    cboEstado.Focus();
                }
                else if (cboActividad.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "OBLI_ACTI"), 2);
                    cboActividad.Focus();
                }else if(Utilitarios.Utilitarios.IsNumeric(txtDescripcion.Text))
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "OBLI_DESCNUM"), 2);
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

        private bool ValidaLogicaNegocio()
        {
            bool bolRpta = false;
            try
            {
                if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    objTarea.IdTarea = 0;
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    objTarea.IdTarea = Convert.ToInt32(dtgTarea.GetFocusedRowCellValue("IdTarea").ToString());
                }
                objTarea.Tarea = txtDescripcion.Text.Trim();
                DataTable tblConsulta = B_Tarea.Tarea_GetItemByDesc(objTarea);
                if (tblConsulta.Rows.Count > 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTarea, "LOGI_DUPL"), 2);
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

        private void txtDescripcion_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            EstadoForm(false, true, false);
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

    }
}
