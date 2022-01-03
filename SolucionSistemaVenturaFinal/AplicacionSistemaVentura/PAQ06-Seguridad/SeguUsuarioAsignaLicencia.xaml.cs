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

using Utilitarios;
using Business;
using Entities;
using System.Data;

using System.Security.Cryptography;
using System.IO;

namespace AplicacionSistemaVentura.PAQ06_Seguridad
{
    /// <summary>
    /// Interaction logic for SeguUsuario.xaml
    /// </summary>
    public partial class SeguUsuario : UserControl
    {
        public SeguUsuario()
        {
            InitializeComponent();
        }
        E_Empresa objE_Empresa = new E_Empresa();
        E_Usuario objE_Usuario = new E_Usuario();
        B_Usuario objB_Usuario = new B_Usuario();
        B_Rol objB_Rol = new B_Rol();
        B_Empresa objB_Empresa = new B_Empresa();
        ErrorHandler objError = new ErrorHandler();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        List<InterfazMTTO.iSBO_BE.BEOUSR> ListaUsuario;
        string gstrEtiquetaUsuario = "SeguUsuarioAsignaLicencia";
        int CantidadTotal = 0, CantidadDisponible = 0;
        string RUC, RazonSocial;
        bool gbolLicenciaCargada = false, gbolLicenciadoOld = false;
        DateTime FechaModificacion;
        DataView dtvUsuarios = new DataView();
        string gstrIdUsuario = string.Empty;
        string gstrfiltro = string.Empty;

        private void btnCargarLicencia_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Archivo de Licencia (*.lic)|*.lic";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    string fileName = dlg.SafeFileName;
                    string sourcePath = System.IO.Path.GetDirectoryName(dlg.FileName);
                    string RutaCompleta = System.IO.Path.Combine(sourcePath, fileName);

                    StreamReader file = new StreamReader(@RutaCompleta);

                    string[] parametros = GlobalClass.CargarParametrosLicencia(file);
                    RUC = parametros[0];
                    RazonSocial = parametros[1];
                    CantidadTotal = Convert.ToInt32(parametros[2]);

                    if (InterfazMTTO.iSBO_BL.Usuario_BL.ValidaSociedad(RUC, RazonSocial, ref RPTA))
                    {
                        byte[] ArchivoLic;
                        using (var stream = new FileStream(@RutaCompleta, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                ArchivoLic = reader.ReadBytes((int)stream.Length);
                            }
                        }

                        objE_Empresa.RUC = RUC;
                        objE_Empresa.Empresa = RazonSocial;
                        objE_Empresa.Licencia = ArchivoLic;

                        string msg = "";
                        if (0 < objB_Empresa.Empresa_CargarLicencia(objE_Empresa))
                        {
                            ActualizarCantidadDisponible();
                            gbolLicenciaCargada = true;
                            msg += "Carga de archivo de licencia Correctamente";
                            //if (0 < objB_Empresa.Empresa_AutoUpdate())
                            //    msg += ", Registro empresa correcta";
                            //else
                            //    msg += ". Registro empresa errada";
                        }
                        else
                            GlobalClass.ip.Mensaje("Carga de archivo de licencia Errada", 3);
                        if (msg.Length > 0)
                            GlobalClass.ip.Mensaje(msg, (msg.Contains(".")) ? 3 : 1);
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje("Empresa no Licenciada SAP", 2);
                    }
                }
                catch (Exception ex)
                {
                    GlobalClass.ip.Mensaje("Error al cargar la Licencia. " + ex.Message, 3);
                    gbolLicenciaCargada = false;
                    CantidadTotal = 0;
                    lblTotal.Content = null;
                    lblDisponible.Content = null;
                }
            }
        }

        private void ActualizarCantidadDisponible()
        {
            int CantidadUsuada = 0;
            objE_Usuario.FlagActivo = 2;
            DataTable tbl = objB_Usuario.Usuario_List(objE_Usuario);
            CantidadUsuada = Convert.ToInt32(tbl.Compute("Count(IdUsuario)", "Licenciado = 'TRUE' AND IdUsuario <> 1 AND FlagActivo = 1"));

            CantidadDisponible = CantidadTotal - CantidadUsuada;
            lblTotal.Content = CantidadTotal;
            lblDisponible.Content = CantidadDisponible;
            if (CantidadDisponible < 0)
                GlobalClass.ip.BloquearAplicacion();
        }

        private void ListarComboUsuarios(bool estado)
        {
            DataTable tblUsuariosReg = (DataTable)dtgListado.ItemsSource;

            for (int i = 0; i < tblUsuariosReg.Rows.Count; i++)
            {
                gstrIdUsuario += "'" + tblUsuariosReg.Rows[i]["Codigo"].ToString() + "',";
            }
            if (gstrIdUsuario.Length > 0)
            {
                gstrIdUsuario = gstrIdUsuario.Substring(0, gstrIdUsuario.Length - 1);
                gstrfiltro = "CodigoUsuario NOT IN (" + gstrIdUsuario + ")";
            }

            ListaUsuario = InterfazMTTO.iSBO_BL.Usuario_BL.ListarUsuarios(ref RPTA);
            if (RPTA.ResultadoRetorno == 0)
            {
                dtvUsuarios = Utilitarios.Utilitarios.ToDataTable(ListaUsuario).DefaultView;
                dtvUsuarios.RowFilter = (estado) ? "" : gstrfiltro;
                cboUsuario.ItemsSource = dtvUsuarios;
                cboUsuario.DisplayMember = "CodigoUsuario";
                cboUsuario.ValueMember = "CodigoUsuario";
            }
            else
            {
                GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rdbActivo.IsChecked = true;

            cboRol.ItemsSource = objB_Rol.Rol_Combo();
            cboRol.DisplayMember = "Rol";
            cboRol.ValueMember = "IdRol";

            tabItem1.Header = "Matrícula y Asignación de Licencias";
            EstadoForm(false, false, true);
            GlobalClass.ip.SeleccionarTab(tabItem1);
            rdbFActivo.IsChecked = true;


            try
            {
                objE_Empresa.IdEmpresa = 1;
                DataTable tblEmpresa = objB_Empresa.Empresa_GetItem(objE_Empresa);
                if (!(tblEmpresa.Rows[0]["Licencia"].ToString() == ""))
                {
                    byte[] LicenciaFile = tblEmpresa.Rows[0]["Licencia"] as byte[];
                    Stream stm = new MemoryStream(LicenciaFile);
                    string[] parametros = GlobalClass.CargarParametrosLicencia(new StreamReader(stm));
                    RUC = parametros[0];
                    RazonSocial = parametros[1];
                    CantidadTotal = Convert.ToInt32(parametros[2]);

                    ActualizarCantidadDisponible();
                    gbolLicenciaCargada = true;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje("Error Licencia de BD. " + ex.Message, 3);
            }
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgListado.VisibleRowCount == 0) { return; }
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            if (dep is TextBlock)
            {
                if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "Codigo")
                {
                    EstadoForm(false, false, true);
                    GlobalClass.ip.SeleccionarTab(tabItem2);
                    ListarComboUsuarios(true);
                    LlenarDatos();
                }
            }
        }

        private void btnMatricular_Click(object sender, RoutedEventArgs e)
        {
            ListarComboUsuarios(false);
            rdbActivo.IsEnabled = false;
            rdbInactivo.IsEnabled = false;
            GlobalClass.ip.SeleccionarTab(tabItem2);
            EstadoForm(true, false, true);
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            ListarComboUsuarios(true);
            EstadoForm(false, true, true);
            GlobalClass.ip.SeleccionarTab(tabItem2);
            LlenarDatos();
        }

        private bool ValidarCampos()
        {
            bool rpt = false;
            string usuarios = "";
            if (cboUsuario.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje("Seleccione un Usuario", 2);
                cboUsuario.Focus();
                rpt = true;
            }
            else if (cboRol.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje("Seleccione un Rol", 2);
                cboRol.Focus();
                rpt = true;
            }
            else if ((bool)chkLicencia.IsChecked && chkLicencia.IsEnabled && (bool)rdbActivo.IsChecked)
            {
                if (!gbolLicenciaCargada)
                {
                    GlobalClass.ip.Mensaje("Cargue el archivo de para asignar Licencia", 2);
                    btnCargarLicencia.Focus();
                    rpt = true;
                }
                else if (CantidadDisponible == 0)
                {
                    GlobalClass.ip.Mensaje("La Cantidad de Licencia ya llego a su limite", 2);
                    rpt = true;
                }
            }
            else if (gbolEdicion && gbolLicenciadoOld && (bool)rdbActivo.IsChecked)
            {
                if (CantidadDisponible == 0)
                {
                    GlobalClass.ip.Mensaje("La Cantidad de Licencia ya llego a su limite", 2);
                    rpt = true;
                }
            }
            else if ((gbolNuevo == true && gbolEdicion == false))
            {
                objE_Usuario.FlagActivo = 2;
                DataTable tblUsuariosMatriculados = objB_Usuario.Usuario_List(objE_Usuario);
                foreach (DataRow f in tblUsuariosMatriculados.Rows)
                    usuarios += f["Codigo"].ToString() + ",";
                if (usuarios.Contains(cboUsuario.EditValue.ToString()))
                {
                    GlobalClass.ip.Mensaje(string.Format("El Usuario {0} ya fue Matriculado", cboUsuario.Text), 2);
                    cboUsuario.Focus();
                    rpt = true;
                }
            }

            return rpt;
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos()) return;
            if (gbolNuevo == true && gbolEdicion == false)
            {
                objE_Usuario = new E_Usuario()
                {
                    IdUsuario = 0,
                    Codigo = lblCodigo.Content.ToString(),
                    Apellidos = lblApellido.Content.ToString(),
                    Nombres = lblNombre.Content.ToString(),
                    Usuario = cboUsuario.Text,
                    IdRol = Convert.ToInt32(cboRol.EditValue),
                    Email = txtCorreo.Text,
                    Licenciado = (bool)chkLicencia.IsChecked,
                    FlagManager = false,
                    FlagActivo = ((bool)rdbActivo.IsChecked) ? 1 : 0,
                    IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario
                };
                if (objE_Usuario.IdRol == 1)
                {
                    GlobalClass.ip.Mensaje("El rol 'Manager' es reservado.", 2);
                    return;
                }
                objE_Usuario.FechaModificacion = DateTime.Now;
                int nresp = objB_Usuario.Usuario_Insert(objE_Usuario);
                if (nresp == 1)//********************
                {
                    EstadoForm(false, false, true);
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUsuario, "GRAB_NUEV"), 1);
                }
                else if (nresp == 1205)//*****************
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUsuario, "GRAB_CONC"), 2);
                    return;
                }
            }
            else if (gbolNuevo == false && gbolEdicion == true)
            {
                objE_Usuario = new E_Usuario()
                {
                    IdUsuario = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdUsuario")),
                    Codigo = lblCodigo.Content.ToString(),
                    Apellidos = lblApellido.Content.ToString(),
                    Nombres = lblNombre.Content.ToString(),
                    Usuario = cboUsuario.Text,
                    IdRol = Convert.ToInt32(cboRol.EditValue),
                    Email = txtCorreo.Text,
                    Licenciado = (bool)chkLicencia.IsChecked,
                    FlagManager = false,
                    FlagActivo = ((bool)rdbActivo.IsChecked) ? 1 : 0,
                    IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario
                };
                if (objE_Usuario.IdUsuario != 1 && objE_Usuario.IdRol == 1)
                {
                    GlobalClass.ip.Mensaje("El rol 'Manager' es reservado.", 2);
                    return;
                }
                objE_Usuario.FechaModificacion = (dtgListado.GetFocusedRowCellValue("FechaModificacion") == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(dtgListado.GetFocusedRowCellValue("FechaModificacion"));
                int nresp = objB_Usuario.Usuario_Update(objE_Usuario);
                if (nresp == 1)
                {
                    EstadoForm(false, false, true);
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUsuario, "GRAB_EDIT"), 1);
                }
                else if (nresp == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUsuario, "LOGI_MODI"), 2);
                    return;
                }
                else if (nresp == 1205)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUsuario, "GRAB_CONC"), 2);
                    return;
                }
            }
            if (gbolLicenciaCargada)
                ActualizarCantidadDisponible();

            LimpiarControles();
            ListarUsuarios();
            EstadoForm(false, false, true);
            GlobalClass.ip.SeleccionarTab(tabItem1);
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            EstadoForm(false, false, true);
            LimpiarControles();
            GlobalClass.ip.SeleccionarTab(tabItem1);
        }

        private void rdbPLANTILLAfiltro_Checked(object sender, RoutedEventArgs e)
        {
            ListarUsuarios();
        }

        private void ListarUsuarios()
        {
            if ((bool)rdbTodos.IsChecked)
                objE_Usuario.FlagActivo = 2;
            if ((bool)rdbFActivo.IsChecked)
                objE_Usuario.FlagActivo = 1;
            if ((bool)rdbFInactivo.IsChecked)
                objE_Usuario.FlagActivo = 0;
            dtgListado.ItemsSource = objB_Usuario.Usuario_List(objE_Usuario);
        }

        private void cboUsuario_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            string cod = cboUsuario.EditValue.ToString();
            InterfazMTTO.iSBO_BE.BEOUSR ClsUsuario = ListaUsuario.Find(x => x.CodigoUsuario == cod);
            lblCodigo.Content = ClsUsuario.CodigoUsuario;
            lblNombre.Content = ClsUsuario.Nombres;
            lblApellido.Content = ClsUsuario.Apellidos;
            if (ClsUsuario.Correo == "-")
            {
                txtCorreo.Text = string.Empty;
                txtCorreo.IsReadOnly = false;
            }
        }

        private void LlenarDatos()
        {
            cboRol.SelectedIndexChanged -= new RoutedEventHandler(cboRol_SelectedIndexChanged);
            cboUsuario.SelectedIndexChanged -= new RoutedEventHandler(cboUsuario_SelectedIndexChanged);
            objE_Usuario.IdUsuario = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdUsuario"));
            objE_Usuario.Usuario = string.Empty;
            DataTable tbl = objB_Usuario.Usuario_GetItem(objE_Usuario);
            if (tbl.Rows.Count == 0) return;
            DataRow FUsuario = tbl.Rows[0];
            cboUsuario.EditValue = FUsuario["Codigo"].ToString();
            cboUsuario.IsEnabled = false;
            lblCodigo.Content = FUsuario["Codigo"];
            lblApellido.Content = FUsuario["Apellidos"];
            lblNombre.Content = FUsuario["Nombres"];
            txtCorreo.Text = FUsuario["Email"].ToString();
            cboRol.EditValue = Convert.ToInt32(FUsuario["IdRol"]);
            bool licenciado = Convert.ToBoolean(FUsuario["Licenciado"]);
            chkLicencia.IsChecked = licenciado;
            chkLicencia.IsEnabled = !licenciado;
            bool Estado = Convert.ToBoolean(FUsuario["FlagActivo"]);
            if (Estado)
                rdbActivo.IsChecked = true;
            else
                rdbInactivo.IsChecked = true;
            gbolLicenciadoOld = (licenciado && !Estado);

            if (Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdUsuario")) == 1)
            {
                chkLicencia.IsEnabled = false;
                rdbActivo.IsEnabled = false;
                rdbInactivo.IsEnabled = false;
                cboRol.IsEnabled = false;
            }
            FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
            cboUsuario.SelectedIndexChanged += new RoutedEventHandler(cboUsuario_SelectedIndexChanged);
            cboRol.SelectedIndexChanged += new RoutedEventHandler(cboRol_SelectedIndexChanged);
            lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", FUsuario["UsuarioCreacion"].ToString(), FUsuario["FechaCreacion"].ToString(), FUsuario["HostCreacion"].ToString());
            lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", FUsuario["UsuarioModificacion"].ToString(), FUsuario["FechaModificacion"].ToString(), FUsuario["HostModificacion"].ToString());
        }


        private void LimpiarControles()
        {
            cboUsuario.SelectedIndexChanged -= new RoutedEventHandler(cboUsuario_SelectedIndexChanged);
            cboRol.SelectedIndexChanged -= new RoutedEventHandler(cboRol_SelectedIndexChanged);
            cboUsuario.SelectedIndex = -1;
            lblCodigo.Content = string.Empty;
            lblNombre.Content = string.Empty;
            lblApellido.Content = string.Empty;
            txtCorreo.Text = string.Empty;
            cboRol.SelectedIndex = -1;
            rdbActivo.IsChecked = true;
            chkLicencia.IsChecked = false;
            txtCorreo.IsReadOnly = false;
            cboUsuario.IsEnabled = true;
            chkLicencia.IsEnabled = true;
            rdbActivo.IsEnabled = true;
            rdbInactivo.IsEnabled = true;
            cboRol.IsEnabled = true;
            gbolLicenciadoOld = false;
            cboUsuario.SelectedIndexChanged += new RoutedEventHandler(cboUsuario_SelectedIndexChanged);
            cboRol.SelectedIndexChanged += new RoutedEventHandler(cboRol_SelectedIndexChanged);
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
                    tabItem2.Header = "Detalle de Matricula de Usuario";
                    btnAceptar.Content = "Aceptar";
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tabItem2.Header = "Nueva Matrícula de Usuario";
                    btnAceptar.Content = "Matricular";
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tabItem2.Header = "Edición Matrícula de Usuario";
                    btnAceptar.Content = "Actualizar";
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void chkPLANTILLA_Click(object sender, RoutedEventArgs e)
        {
            EstadoForm(false, true, false);
        }

        private void cboRol_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            EstadoForm(false, true, false);
        }

        private void txtCorreo_KeyUp(object sender, KeyEventArgs e)
        {
            EstadoForm(false, true, false);
        }
    }
}
