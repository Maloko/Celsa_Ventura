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

using Business;
using Entities;
using Utilitarios;
using System.Data;
namespace AplicacionSistemaVentura.PAQ05_Utilitarios
{
    /// <summary>
    /// Interaction logic for UtilConfiguracionDetalleTM.xaml
    /// </summary>
    public partial class UtilConfiguracionDetalleTM : UserControl
    {
        int gintIdTabla = 0;
        ErrorHandler ObjError = new ErrorHandler();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        int gintIdUsuario = 1;

        string gstrUsuario = "Admi";
        public UtilConfiguracionDetalleTM()
        {
            gintIdTabla = GlobalClass.IdTabla;
            InitializeComponent();
            UserControl_Load();
        }
        private void UserControl_Load()
        {
            objE_TablaMaestra.IdTabla = gintIdTabla;
            DataTable tbl = objB_TablaMaestra.TablaMaestra_GetItem(objE_TablaMaestra);
            lblTablaPadre.Content = tbl.Rows[0]["IdTabla"] + " - " + tbl.Rows[0]["Descripcion"].ToString();
            ListarTablaMaestra();
        }

        private void ListarTablaMaestra()
        {
            objE_TablaMaestra.IdTabla = gintIdTabla;
            objE_TablaMaestra.IdColumna = 0;
            objE_TablaMaestra.FlagActivo = 2;
            dtgListado.ItemsSource = objB_TablaMaestra.TablaMaestra_List(objE_TablaMaestra);
        }
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            
            tabDato.IsEnabled = true;
            tabDato.IsSelected = true;
            tabLista.IsEnabled = false;
            LimpiarControles();
            txtCodigo.Text = "Nuevo Código";
            rdActivo.IsEnabled = false;
            rdInactivo.IsEnabled = false;
            sbAuditoria.Visibility = Visibility.Visible;
            EstadoForm(true, false, true);
            txtDescripcion.Focus();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgListado.VisibleRowCount == 0) { return; }
            tabDato.IsEnabled = true;
            tabDato.IsSelected = true;
            tabLista.IsEnabled = false;
            sbAuditoria.Visibility = Visibility.Visible;
            EstadoForm(false, true, true);

            LlenarDatos(Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdColumna")));
        }

        private void dtgListado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgListado.VisibleRowCount == 0) { return; }
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            if (dep is TextBlock)
            {
                if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "IdColumna")
                {
                    tabDato.IsEnabled = true;
                    tabDato.IsSelected = true;
                    tabLista.IsEnabled = false;
                    sbAuditoria.Visibility = Visibility.Visible;
                    EstadoForm(false, false, true);
                    LlenarDatos(Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdColumna")));
                }
            }
        }

        private void LlenarDatos(int IdColumna)
        {
            try
            {
                objE_TablaMaestra.IdTabla = gintIdTabla;
                objE_TablaMaestra.IdColumna = IdColumna;
                DataTable tblTablaMaestra = objB_TablaMaestra.TablaMaestra_GetItem(objE_TablaMaestra);
                txtCodigo.Text = tblTablaMaestra.Rows[0]["IdColumna"].ToString();
                txtValor.Text = tblTablaMaestra.Rows[0]["Valor"].ToString();
                txtDescripcion.Text = tblTablaMaestra.Rows[0]["Descripcion"].ToString();
                if ((bool)tblTablaMaestra.Rows[0]["FlagActivo"])
                    rdActivo.IsChecked = true;
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
            if (txtDescripcion.Text == "") { msg = "La Campo Descripción no puede estar Vacía"; txtDescripcion.Focus(); valida = true; }
            if (txtValor.Text == "") { msg = "El Campo Valor no puede estar Vacía"; txtDescripcion.Focus(); valida = true; }
            GlobalClass.ip.Mensaje(msg, 2);
            return valida;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCamposGrabar()) return;
            try
            {
                objE_TablaMaestra.Descripcion = txtDescripcion.Text;
                objE_TablaMaestra.IdTabla = gintIdTabla;                
                objE_TablaMaestra.Valor = txtValor.Text;
                objE_TablaMaestra.FlagActivo = ((bool)rdActivo.IsChecked) ? 1 : 0;

                if (gbolNuevo == true && gbolEdicion == false)
                {
                    objE_TablaMaestra.IdUsuarioCreacion = gintIdUsuario;
                    objE_TablaMaestra.IdColumna = 0;
                    objB_TablaMaestra.TablaMaestra_Insert(objE_TablaMaestra);

                    GlobalClass.ip.Mensaje("Creación Satisfactoria", 1);
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    objE_TablaMaestra.IdColumna = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdColumna"));
                    objE_TablaMaestra.IdUsuarioModificacion = gintIdUsuario;
                    objB_TablaMaestra.TablaMaestra_Update(objE_TablaMaestra);

                    GlobalClass.ip.Mensaje("Edición Satisfactoria", 1);
                }
                tabLista.IsEnabled = true;
                tabLista.IsSelected = true;
                tabDato.IsEnabled = false;
                rdActivo.IsEnabled = true;
                rdInactivo.IsEnabled = true;
                sbAuditoria.Visibility = Visibility.Collapsed;
                EstadoForm(false, false, true);
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
            tabLista.IsEnabled = true;
            tabLista.IsSelected = true;
            tabDato.IsEnabled = false;
            rdActivo.IsEnabled = true;
            rdInactivo.IsEnabled = true;
            sbAuditoria.Visibility = Visibility.Collapsed;
            EstadoForm(false, false, true);
            LimpiarControles();
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
            txtValor.Text = "";
            rdActivo.IsChecked = true;
        }

        private void txtCampo_KeyUp(object sender, KeyEventArgs e)
        {
            EstadoForm(false, true, false);
        }
    }
}
