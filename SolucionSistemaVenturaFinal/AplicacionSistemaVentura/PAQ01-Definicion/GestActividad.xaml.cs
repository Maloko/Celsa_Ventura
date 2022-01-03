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
using DevExpress.Utils;
using DevExpress.XtraGrid;

namespace AplicacionSistemaVentura.PAQ01_Definicion
{
    /// <summary>
    /// Interaction logic for GestActividad.xaml
    /// </summary>
    public partial class GestActividad : UserControl
    {
        //int gintIdUsuario = 1;//Se Seteara del Login
        //string gstrUsuario = "Admi";//Se seteara del login
        E_Actividad objActividad = new E_Actividad();
        E_TablaMaestra objTablaMaestra = new E_TablaMaestra();
        B_Actividad objB_Actividad = new B_Actividad();
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();
        int IdMenuDetalle = 0;

        string gstrEtiquetaActividad = "GestActividad";

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, false, true);
                LimpiarControles();
                e.Handled = false;
                GlobalClass.ip.SeleccionarTab(tabListado);
                //tcAvtividad.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnGrabar_Click(object sender, RoutedEventArgs e)
        {
            objActividad.CodActividad = txtCodigo.Text;
            objActividad.Actividad = txtDescripcion.Text;
            objActividad.IdEstadoActividad = Convert.ToInt32(cboEstado.EditValue);
            objActividad.FlagActivo = 1;
            try
            {
                if (gbolNuevo == true && gbolEdicion == false)
                {
                    if (ValidaCampoObligado() == true) { return; }
                    if (ValidaLogicaNegocio() == true) { return; }
                    objActividad.IdActividad = 0;
                    objActividad.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                    int nresp = objB_Actividad.Actividad_Insert(objActividad);
                    if (nresp == 1)
                    {
                        EstadoForm(false, false, true);
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "GRAB_NUEV"), 1);
                    }
                    else if (nresp == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "GRAB_CONC"), 2);
                        return;
                    }
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    if (ValidaEnOperacion()) return;
                    if (ValidaCampoObligado() == true) { return; }
                    if (ValidaLogicaNegocio() == true) { return; }

                    objActividad.IdActividad = Convert.ToInt32(lblIdActividad.Content);
                    objActividad.IdUsuarioModificacion = Utilitarios.Utilitarios.gintIdUsuario;
                    objActividad.FechaModificacion = (dtgActividad.GetFocusedRowCellValue("FechaModificacion") == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dtgActividad.GetFocusedRowCellValue("FechaModificacion"));
                    int nresp = objB_Actividad.Actividad_Update(objActividad);
                    if (nresp == 1)
                    {
                        EstadoForm(false, false, true);
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "GRAB_EDIT"), 1);
                    }
                    else if (nresp == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (nresp == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "GRAB_CONC"), 2);
                        return;
                    }
                }
                LimpiarControles();
                GlobalClass.ip.SeleccionarTab(tabListado);
                //tcAvtividad.SelectedIndex = 0;
                ListarActividades();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgActividad.VisibleRowCount == 0) { return; }
            EstadoForm(false, true, true);
            GlobalClass.ip.SeleccionarTab(tabDatos);
            LlenarDatos();
        }

        private void BtnRegistrarActividad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LimpiarControles();
                GlobalClass.ip.SeleccionarTab(tabDatos);
                //tcAvtividad.SelectedIndex = 1;
                EstadoForm(true, false, true);
                txtDescripcion.Focus();
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

        private void LlenarDatos()
        {
            try
            {
                objActividad.IdActividad = Convert.ToInt32(dtgActividad.GetFocusedRowCellValue("IdActividad"));
                //tcAvtividad.SelectedIndex = 1;
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                txtDescripcion.EditValueChanged -= new EditValueChangedEventHandler(txtDescripcion_EditValueChanged);
                dtgActividad.AutoGenerateColumns = DevExpress.Xpf.Grid.AutoGenerateColumnsMode.None;

                DataTable tbl = objB_Actividad.Actividad_GetItem(objActividad);
                lblIdActividad.Content = tbl.Rows[0]["IdActividad"];
                txtCodigo.Text = tbl.Rows[0]["CodActividad"].ToString();
                txtDescripcion.Text = tbl.Rows[0]["Actividad"].ToString();
                cboEstado.EditValue = Convert.ToInt32(tbl.Rows[0]["IdEstadoActividad"]);
                txtDescripcion.IsReadOnly = (Convert.ToInt32(tbl.Rows[0]["IdEstadoActividad"]) == 2) ? true : false;
                lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tbl.Rows[0]["UsuarioCreacion"].ToString(), tbl.Rows[0]["FechaCreacion"].ToString(), tbl.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tbl.Rows[0]["UsuarioModificacion"].ToString(), tbl.Rows[0]["FechaModificacion"].ToString(), tbl.Rows[0]["HostModificacion"].ToString());

                cboEstado.IsEnabled = true;

                sbAuditoria.Visibility = Visibility.Visible;
                txtDescripcion.EditValueChanged += new EditValueChangedEventHandler(txtDescripcion_EditValueChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgActividad_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgActividad.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodActividad")
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
                    tabDatos.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "TAB1_CONS");
                    btnGraDato.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "BTNG_CONS");
                    //GlobalClass.ip.SeleccionarTab(tabListado);
                    //tabListado.IsEnabled = true;
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tabDatos.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "TAB1_NUEV");
                    btnGraDato.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "BTNG_NUEV");
                    //tabListado.IsEnabled = false;
                    //GlobalClass.ip.SeleccionarTab(tabDatos);
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tabDatos.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "TAB1_EDIT");
                    btnGraDato.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "BTNG_EDIT");
                    //tabListado.IsEnabled = false;
                    //GlobalClass.ip.SeleccionarTab(tabDatos);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public GestActividad()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        private void LimpiarControles()
        {
            try
            {
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                txtCodigo.Text = "Nuevo Código";
                txtDescripcion.Text = "";
                cboEstado.EditValue = 1;
                cboEstado.IsEnabled = false;
                txtDescripcion.IsReadOnly = false;
                sbAuditoria.Visibility = Visibility.Hidden;
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarActividades()
        {
            try
            {
                if ((bool)rdbTodos.IsChecked)
                    objActividad.IdEstadoActividad = 0;
                else if ((bool)rdbActivo.IsChecked)
                    objActividad.IdEstadoActividad = 1;
                else if ((bool)rdbInactivo.IsChecked)
                    objActividad.IdEstadoActividad = 2;

                dtgActividad.ItemsSource = objB_Actividad.Actividad_List(objActividad);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void rdbTodos_Checked(object sender, RoutedEventArgs e)
        {
            ListarActividades();
        }

        private void rdbActivo_Checked(object sender, RoutedEventArgs e)
        {
            ListarActividades();
        }

        private void rdbInactivo_Checked(object sender, RoutedEventArgs e)
        {
            ListarActividades();
        }

        private void UserControl_Loaded()
        {
            try
            {
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                dtgActividad.AutoGenerateColumns = DevExpress.Xpf.Grid.AutoGenerateColumnsMode.None;

                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tabListado);
                txtCodigo.Text = "Nuevo Código";
                objTablaMaestra.IdTabla = 3;
                cboEstado.ItemsSource = B_TablaMaestra.TablaMaestra_Combo(objTablaMaestra);
                cboEstado.DisplayMember = "Descripcion";
                cboEstado.ValueMember = "IdColumna";
                cboEstado.IsTextEditable = false;
                rdbActivo.IsChecked = true;
                tabListado.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "TAB0_CAPT");
                sbAuditoria.Visibility = Visibility.Hidden;

                #region VisualizacionBotonImprimir

                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref IdMenuDetalle);


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
            try
            {
                if (txtDescripcion.Text.Trim() == "")
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "OBLI_DESC"), 2);
                    txtDescripcion.Focus();
                }
                else if (Utilitarios.Utilitarios.IsNumeric(txtDescripcion.Text))
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "OBLI_DESCNUM"), 2);
                    txtDescripcion.Focus();
                }
                else if (cboEstado.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "OBLI_ESTA"), 2);
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

        private bool ValidaEnOperacion()
        {
            bool rpt = false;
            try
            {
                objActividad.IdActividad = Convert.ToInt32(dtgActividad.GetFocusedRowCellValue("IdActividad").ToString());
                int CantOperacion = objB_Actividad.Actividad_GetBeforeChange(objActividad);
                if (CantOperacion > 0)
                {
                    rpt = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "LOGI_DOC_OPE"), 2);
                }
                return rpt;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                return rpt;
            }
        }

        private bool ValidaLogicaNegocio()
        {
            bool bolRpta = false;
            try
            {
                if (gbolNuevo == true && gbolEdicion == false)
                {
                    objActividad.IdActividad = 0;
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    objActividad.IdActividad = Convert.ToInt32(dtgActividad.GetFocusedRowCellValue("IdActividad").ToString());
                }
                objActividad.Actividad = txtDescripcion.Text.Trim();
                DataTable tblConsulta = objB_Actividad.Actividad_GetItemByDesc(objActividad);
                if (tblConsulta.Rows.Count > 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaActividad, "LOGI_DUPL"), 2);
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
                //GlobalClass.GeneraImpresion(IdMenuDetalle, IdEstado);
            }

            catch { }
        }
    }
}