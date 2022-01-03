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
using DevExpress.Xpf.Core;
using Entities;
using Business;
using Utilitarios;
using System.Data;

namespace AplicacionSistemaVentura.PAQ06_Seguridad
{
    /// <summary>
    /// Interaction logic for SeguConfigAcceso.xaml
    /// </summary>
    public partial class SeguConfigAcceso : UserControl
    {
        public SeguConfigAcceso()
        {
            InitializeComponent();
        }
        B_Rol objB_Rol = new B_Rol();
        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        E_Rol objE_Rol = new E_Rol();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        ErrorHandler objError = new ErrorHandler();
        DataTable gtblMenuRol_List = new DataTable();
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        Boolean gbolConsulta = false;
        string gstrMenus2Insert = string.Empty, gstrMenus2Update = string.Empty;
        string gstrEtiquetaRol = "SeguConfigAcceso";
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                objE_TablaMaestra.IdTabla = 1;
                cboEstado.ItemsSource = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
                cboEstado.DisplayMember = "Descripcion";
                cboEstado.ValueMember = "Valor";

                rdbTodos.IsChecked = true;
                treeListControl1.ItemsSource = objB_Rol.Menu_List();
                EstadoForm(false, false, true);
                tabListado.Header = "Gestión Rol y Accesos";
                GlobalClass.ip.SeleccionarTab(tabListado);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgListado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgListado.VisibleRowCount == 0) { return; }
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            if (dep is TextBlock)
            {
                if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "IdRol")
                {
                    e.Handled = true;
                    EstadoForm(false, false, true);
                    LlenarDatos(Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdRol")));
                    gbolConsulta = true;
                }
            }
        }

        private void rdbTodos_Checked(object sender, RoutedEventArgs e)
        {
            ListarRoles();
        }
        private void rdbActivo_Checked(object sender, RoutedEventArgs e)
        {
            ListarRoles();
        }
        private void rdbInactivo_Checked(object sender, RoutedEventArgs e)
        {
            ListarRoles();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgListado.VisibleRowCount == 0) { return; }
            EstadoForm(false, true, true);
            LlenarDatos(Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdRol")));
            txtRol.Focus();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            cboEstado.EditValue = "1";
            cboEstado.IsEnabled = false;
            EstadoForm(true, false, true);
            GlobalClass.ip.SeleccionarTab(tabDatos);
            LimpiarControles();
        }

        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCamposGrabar()) return;

            try
            {
                string msg = string.Empty;
                objE_Rol.IdRol = 0;
                objE_Rol.Rol = txtRol.Text;
                objE_Rol.FlagActivo = Convert.ToInt32(cboEstado.EditValue);

                if (gbolNuevo == true && gbolEdicion == false)
                {

                    objE_Rol.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                    int IdNewRol = objB_Rol.Rol_Insert(objE_Rol);
                    if (IdNewRol != 0)
                    {
                        msg += "Creación Satisfactoria. ";
                        objE_Rol.IdRol = IdNewRol;
                        RegistrarTreelistNodos(treeListView1.Nodes);
                        if (gstrMenus2Insert != "")
                        {
                            objE_Rol.IdMenus2Insert = gstrMenus2Insert;
                            objB_Rol.Rol_Menu_InsertMasivo(objE_Rol);
                            msg += "Asignación de Menu Satisfactoria";
                        }
                    }
                    else if (IdNewRol == 1205)//*****************
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRol, "GRAB_CONC"), 2);
                        return;
                    }
                    GlobalClass.ip.Mensaje(msg, 1);
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    objE_Rol.IdRol = Convert.ToInt32(txtIdRol.Text);
                    objE_Rol.IdUsuarioModificacion = Utilitarios.Utilitarios.gintIdUsuario;
                    objE_Rol.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                    objE_Rol.FechaModificacion = (dtgListado.GetFocusedRowCellValue("FechaModificacion") == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dtgListado.GetFocusedRowCellValue("FechaModificacion"));
                    int nresp = objB_Rol.Rol_Update(objE_Rol);
                    if (nresp == 1)
                    {
                        msg += "Edición Satisfactoria. ";

                        RegistrarTreelistNodos(treeListView1.Nodes);
                        if (gstrMenus2Insert != "")
                        {
                            objE_Rol.IdMenus2Insert = gstrMenus2Insert;
                            objB_Rol.Rol_Menu_InsertMasivo(objE_Rol);
                        }
                        if (gstrMenus2Update != "")
                        {
                            objE_Rol.IdMenus2Update = gstrMenus2Update;
                            objB_Rol.Rol_Menu_UpdateMasivo(objE_Rol);
                        }

                        if (gstrMenus2Update != "" || gstrMenus2Insert != "")
                        {
                            msg += "Asignación de Menu Satisfactoria";
                        }
                        GlobalClass.ip.Mensaje(msg, 1);
                    }
                    else if (nresp == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRol, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (nresp == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaRol, "GRAB_CONC"), 2);
                        return;
                    }
                }

                EstadoForm(false, false, true);
                LimpiarControles();
                GlobalClass.ip.SeleccionarTab(tabListado);
                ListarRoles();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarRoles()
        {
            try
            {
                if ((bool)rdbTodos.IsChecked)
                    objE_Rol.FlagActivo = 2;
                else if ((bool)rdbActivo.IsChecked)
                    objE_Rol.FlagActivo = 1;
                else if ((bool)rdbInactivo.IsChecked)
                    objE_Rol.FlagActivo = 0;

                dtgListado.ItemsSource = objB_Rol.Rol_List(objE_Rol);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LimpiarControles()
        {
            txtIdRol.Text = "Nuevo Código";
            txtRol.Text = string.Empty;
            txtRol.IsReadOnly = false;
            gstrMenus2Insert = string.Empty;
            gstrMenus2Update = string.Empty;
            gtblMenuRol_List.Rows.Clear();
            cboEstado.IsEnabled = true;
            gbolConsulta = false;//Siempre antes de LimpiarTreeView
            LimpiarTreelistNodos(treeListView1.Nodes);

        }

        private bool ValidarCamposGrabar()
        {
            bool valida = false;
            string msg = string.Empty;
            if (txtRol.Text == "") { msg += "Ingrese la descripcion del Rol."; txtRol.Focus(); valida = true; }
            if (msg != "")
                GlobalClass.ip.Mensaje(msg, 2);
            return valida;
        }

        private void LlenarDatos(int IdRol)
        {
            try
            {
                objE_Rol.IdRol = IdRol;
                DataTable tbl = objB_Rol.Rol_Get(objE_Rol);

                txtIdRol.Text = tbl.Rows[0]["IdRol"].ToString();
                txtRol.Text = tbl.Rows[0]["Rol"].ToString();
                cboEstado.EditValue = ((bool)tbl.Rows[0]["FlagActivo"]) ? "1" : "0";
                objE_Rol.IdRol = IdRol;
                gtblMenuRol_List = objB_Rol.Rol_Menu_List(objE_Rol);

                if (txtIdRol.Text.Equals("1") || txtIdRol.Text.Equals("2") || txtIdRol.Text.Equals("3") || txtIdRol.Text.Equals("4"))
                {
                    txtRol.IsReadOnly = true;
                }

                if (gtblMenuRol_List.Rows.Count > 0)
                    CargarTreeListNodos(treeListView1.Nodes);

                lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tbl.Rows[0]["UsuarioCreacion"], tbl.Rows[0]["FechaCreacion"], tbl.Rows[0]["HostCreacion"]);
                lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tbl.Rows[0]["UsuarioModificacion"], tbl.Rows[0]["FechaModificacion"], tbl.Rows[0]["HostModificacion"]);
                GlobalClass.ip.SeleccionarTab(tabDatos);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void txtRol_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox tx = (TextBox)sender;
            if (!tx.IsReadOnly)
            {
                EstadoForm(false, true, false);
            }
        }


        private void CargarTreeListNodos(TreeListNodeCollection nodos)
        {
            foreach (TreeListNode nodo in nodos)
            {
                DataRowView DataNodo = (DataRowView)nodo.Content;
                int idmenu = Convert.ToInt32(DataNodo.Row["IdMenu"]);

                nodo.IsChecked = (PerteneceAlRol(idmenu, 1));

                CargarTreeListNodos(nodo.Nodes);
            }
        }

        private bool PerteneceAlRol(int IdMenu, int IdEstado)
        {
            //Idestado 2: conparar con todos, Para Registrar
            //IdEstado 1: comparar con Activos, Para Listar Menu x Rol

            bool Coindicen = false;
            if (IdEstado == 2)
            {
                if (gtblMenuRol_List.Select("IdMenu = " + IdMenu.ToString()).Length > 0)
                    Coindicen = true;
            }
            else if (IdEstado == 1)
            {
                if (gtblMenuRol_List.Select("IdMenu = " + IdMenu.ToString() + " and FlagActivo = 'True'").Length > 0)
                    Coindicen = true;
            }
            return Coindicen;
        }

        private void treeListView1_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            treeListView1.NodeCheckStateChanged -= new DevExpress.Xpf.Grid.TreeList.TreeListNodeEventHandler(treeListView1_NodeCheckStateChanged);
            if (gbolConsulta)
                EstadoForm(false, true, false);
            SetCheckedChilNodes(e.Node, e.Node.IsChecked);
            SetCheckedParentNodes(e.Node, e.Node.IsChecked);
            treeListView1.NodeCheckStateChanged += new DevExpress.Xpf.Grid.TreeList.TreeListNodeEventHandler(treeListView1_NodeCheckStateChanged);
        }

        private void SetCheckedChilNodes(TreeListNode nodo, bool? Checkado)
        {
            for (int i = 0; i < nodo.Nodes.Count; i++)
            {
                nodo.Nodes[i].IsChecked = Checkado;
                SetCheckedChilNodes(nodo.Nodes[i], Checkado);
            }
        }

        private void SetCheckedParentNodes(TreeListNode nodo, bool? Checkado)
        {
            //Si todos los hijos del Padre tienen la misma propiedad IsChecked (True/False),
            //entonces el padre tendrá lo tendra tambien, encaso contrario lo Seteamos con 'null'
            if (nodo.ParentNode == null) return;
            bool esTodoIgual = true;
            for (int i = 0; i < nodo.ParentNode.Nodes.Count; i++)
            {
                bool? state = nodo.ParentNode.Nodes[i].IsChecked;
                if (Checkado != state)
                {
                    esTodoIgual = false;
                    break;
                }
            }
            nodo.ParentNode.IsChecked = esTodoIgual ? Checkado : null;
            SetCheckedParentNodes(nodo.ParentNode, Checkado);
        }

        private void RegistrarTreelistNodos(TreeListNodeCollection nodos)
        {
            foreach (TreeListNode nodo in nodos)
            {
                DataRowView DataNodo = (DataRowView)nodo.Content;
                int idmenu = Convert.ToInt32(DataNodo.Row["IdMenu"]);
                if (gbolNuevo == false && gbolEdicion == true)//Para edicion
                {
                    if (PerteneceAlRol(idmenu, 2))
                    {
                        bool? FlagActivo = Convert.ToBoolean(gtblMenuRol_List.Select("IdMenu = " + idmenu.ToString())[0]["FlagActivo"]);
                        if (FlagActivo != nodo.IsChecked)
                        {
                            gstrMenus2Update += idmenu + ",";
                        }
                    }
                    else
                    {
                        if (nodo.IsChecked == true)
                        {
                            gstrMenus2Insert += idmenu + ",";
                        }
                    }
                }
                else if (gbolNuevo == true && gbolEdicion == false)//Para Nuevo
                {
                    if (nodo.IsChecked == true)
                    {
                        gstrMenus2Insert += idmenu + ",";
                    }
                }

                RegistrarTreelistNodos(nodo.Nodes);
            }
        }

        private void LimpiarTreelistNodos(TreeListNodeCollection nodos)
        {
            foreach (TreeListNode nodo in nodos)
            {
                nodo.IsChecked = false;
                LimpiarTreelistNodos(nodo.Nodes);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            EstadoForm(false, false, true);
            GlobalClass.ip.SeleccionarTab(tabListado);
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
                    tabDatos.Header = "Consulta Rol y Accesos";
                    btnGrabar.Content = "Aceptar";
                    tabListado.IsEnabled = true;
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tabDatos.Header = "Nuevo Rol y Accesos";
                    btnGrabar.Content = "Crear";
                    tabListado.IsEnabled = false;
                    lblAuditoria_creacion.Text = String.Format("[Creación] Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("[Modificación] Usuario: -- Fecha: -- Host: --");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tabDatos.Header = "Edición Rol y Accesos";
                    btnGrabar.Content = "Actualizar";
                    tabListado.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
    }
}
