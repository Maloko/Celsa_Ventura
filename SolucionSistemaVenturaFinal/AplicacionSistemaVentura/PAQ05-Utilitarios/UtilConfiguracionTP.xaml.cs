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

using DevExpress.Xpf.Grid;
using Business;
using Entities;
using System.Data;
using Utilitarios;

namespace AplicacionSistemaVentura.PAQ05_Utilitarios
{
    /// <summary>
    /// Interaction logic for UtilConfiguracionTP.xaml
    /// </summary>
    public partial class UtilConfiguracionTP : UserControl
    {
        ErrorHandler ObjError = new ErrorHandler();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        int gintIdUsuario = 1;
        DataTable gtblParametros = new DataTable();
        
        string gstrUsuario = "Admi";
        
        public UtilConfiguracionTP()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ListarParametrosSistema();
            ListarTablaMaestra();
            GlobalClass.ip.SeleccionarTab(tabItem0, tabLista);
        }

        private void ListarParametrosSistema()
        {
            try
            {
                gtblParametros = new DataTable();
                objE_TablaMaestra.IdTabla = 1000;
                objE_TablaMaestra.IdColumna = 0;
                objE_TablaMaestra.FlagActivo = 1;
                gtblParametros = objB_TablaMaestra.TablaMaestra_List(objE_TablaMaestra);
                gtblParametros.Columns["IdColumnaPadre"].DefaultValue = 0;
                objE_TablaMaestra.IdTabla = 1000;
                objE_TablaMaestra.IdColumna = 0;
                DataRow RowTabla = objB_TablaMaestra.TablaMaestra_GetItem(objE_TablaMaestra).Rows[0];
                
                gtblParametros.ImportRow(RowTabla);
                DataColumn clm = new DataColumn("Nuevo", Type.GetType("System.Boolean"));
                clm.DefaultValue = true;
                gtblParametros.Columns.Add(clm);
                trvParametros.ItemsSource = gtblParametros;
                TreeListView1.ExpandAllNodes();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarTablaMaestra()
        {
            objE_TablaMaestra.IdTabla = 0;
            objE_TablaMaestra.IdColumna = 0;
            objE_TablaMaestra.FlagActivo = 2;
            DataView dtv=objB_TablaMaestra.TablaMaestra_List(objE_TablaMaestra).DefaultView;
            dtv.RowFilter = "IdTabla < 1000";
            dtgListado.ItemsSource = dtv;
        }
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            txtCodigo.Text = "Nuevo Código";
            rdAcivo.IsEnabled = false;
            rdInactivo.IsEnabled = false;
            //sbAuditoria.Visibility = Visibility.Visible;
            EstadoForm(true, false, true);
            GlobalClass.ip.SeleccionarTab(tabDato, tabItem0);
            txtDescripcion.Focus();
            
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgListado.VisibleRowCount == 0) { return; }
            //sbAuditoria.Visibility = Visibility.Visible;
            EstadoForm(false, true, true);
            GlobalClass.ip.SeleccionarTab(tabDato, tabItem0);

            LlenarDatos(Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdTabla")));
        }

        private void dtgListado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgListado.VisibleRowCount == 0) { return; }
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            if (dep is TextBlock)
            {
                if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "IdTabla")
                {
                    GlobalClass.ip.SeleccionarTab(tabDato, tabItem0);
                    //sbAuditoria.Visibility = Visibility.Visible;
                    EstadoForm(false, false, true);
                    LlenarDatos(Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdTabla")));
                }
            }
        }

        private void LlenarDatos(int IdTabla)
        {
            try
            {
                objE_TablaMaestra.IdTabla = IdTabla;
                objE_TablaMaestra.IdColumna = 0;
                DataTable tblTablaMaestra = objB_TablaMaestra.TablaMaestra_GetItem(objE_TablaMaestra);
                txtCodigo.Text = tblTablaMaestra.Rows[0]["IdTabla"].ToString();
                txtDescripcion.Text = tblTablaMaestra.Rows[0]["Descripcion"].ToString();
                if ((bool)tblTablaMaestra.Rows[0]["FlagActivo"])
                    rdAcivo.IsChecked = true;
                else
                    rdInactivo.IsChecked = true;

                lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblTablaMaestra.Rows[0]["UsuarioCreacion"].ToString(), tblTablaMaestra.Rows[0]["FechaCreacion"].ToString(), tblTablaMaestra.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblTablaMaestra.Rows[0]["UsuarioModificacion"].ToString(), tblTablaMaestra.Rows[0]["FechaModificacion"].ToString(), tblTablaMaestra.Rows[0]["HostModificacion"].ToString());
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private bool ValidarCamposGrabar()
        {
            var valida = false;
            string msg = string.Empty;
            if (txtDescripcion.Text=="") { msg = "La Descripción no puede estar Vacía"; txtDescripcion.Focus(); valida = true; }
            if (valida)
                GlobalClass.ip.Mensaje(msg, 2);
            return valida;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {            
            if (ValidarCamposGrabar()) return;
            try
            {
                objE_TablaMaestra.Descripcion = txtDescripcion.Text;
                objE_TablaMaestra.IdColumna = 0;
                objE_TablaMaestra.Valor = "";
                objE_TablaMaestra.FlagActivo = ((bool)rdAcivo.IsChecked) ? 1 : 0;

                if (gbolNuevo == true && gbolEdicion == false)
                {
                    objE_TablaMaestra.IdUsuarioCreacion = gintIdUsuario;
                    objE_TablaMaestra.IdTabla = 0;
                    objB_TablaMaestra.TablaMaestra_Insert(objE_TablaMaestra);

                    GlobalClass.ip.Mensaje("Creación Satisfactoria", 1);
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    objE_TablaMaestra.IdTabla = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdTabla"));
                    objE_TablaMaestra.IdUsuarioModificacion = gintIdUsuario;
                    objB_TablaMaestra.TablaMaestra_Update(objE_TablaMaestra);

                    GlobalClass.ip.Mensaje("Edición Satisfactoria", 1);
                }
                GlobalClass.ip.SeleccionarTab(tabLista, tabItem0);
                rdAcivo.IsEnabled = true;
                rdInactivo.IsEnabled = true;
                //sbAuditoria.Visibility = Visibility.Collapsed;
                EstadoForm(false, false, true);
                LimpiarControles();
                ListarTablaMaestra();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.ip.SeleccionarTab(tabLista, tabItem0);
            rdAcivo.IsEnabled = true;
            rdInactivo.IsEnabled = true;
            //sbAuditoria.Visibility = Visibility.Collapsed;
            EstadoForm(false, false, true);
            LimpiarControles();
        }

        private void btnDetalle_Click(object sender, RoutedEventArgs e)
        {
            if(dtgListado.VisibleRowCount>0)
                GlobalClass.ip.AbrirDetalleTablaMaestra(Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdTabla")));
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
                    tabDato.Header = "Consulta";
                    btnAceptar.Content = "Aceptar";
                    
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tabDato.Header = "Nueva";
                    btnAceptar.Content = "Crear";
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tabDato.Header = "Edicion";
                    btnAceptar.Content = "Actualizar";
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
            txtDescripcion.Text = "";
            txtCodigo.Text = "";
            rdAcivo.IsChecked = true;
        }

        private void txtDescripcion_KeyUp(object sender, KeyEventArgs e)
        {
            EstadoForm(false, true, false);
        }

        private void btnActualizarParametros_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tbl = new DataTable();
                tbl.Columns.Add("IdTabla", Type.GetType("System.Int32"));
                tbl.Columns.Add("IdColumna", Type.GetType("System.Int32"));
                tbl.Columns.Add("Valor", Type.GetType("System.String"));
                tbl.Columns.Add("Descripcion", Type.GetType("System.String"));
                tbl.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                foreach (DataRow FParametro in gtblParametros.Select("Nuevo = 'False'"))
                {
                    tbl.ImportRow(FParametro);
                }
                objE_TablaMaestra.IdUsuarioModificacion = gintIdUsuario;
                objB_TablaMaestra.TablaMaestra_UpdateMasivo(objE_TablaMaestra, tbl);
                ListarParametrosSistema();
                GlobalClass.ip.Mensaje("Edición  de Parametros  Satisfactoria", 1);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }


        private void TreeListView1_CellValueChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListCellValueChangedEventArgs e)
        {
            DataRowView f = (DataRowView)e.Row;
            f["Nuevo"] = false;
        }

        

        
            
    }
}
