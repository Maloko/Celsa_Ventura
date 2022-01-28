using System;
using System.Windows;
using System.Windows.Media;
using Business;
using Entities;
using System.Windows.Threading;
using System.Data;
using DevExpress.Xpf.Core;
using System.Configuration;
using System.IO;
using System.ComponentModel;
using Utilitarios;

namespace AplicacionSistemaVentura.PAQ06_Seguridad
{
    /// <summary>
    /// Interaction logic for Segulogin.xaml
    /// </summary>
    public partial class Segulogin : DXWindow
    {
        public Segulogin()
        {
            InitializeComponent();
        }
        string gstrEtiquetaLogin = "Segulogin";
        int gintTick_Mensaje = 0;
        int gintTick_Cerrar = 10;
        int Contador = 0;
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        E_Usuario objE_Usuario = new E_Usuario();
        B_Usuario objB_Usuario = new B_Usuario();
        DispatcherTimer timer = new DispatcherTimer();
        InterfazMTTO.iSBO_BE.BEOUSR BEOUSR;
        InterfazMTTO.iSBO_BE.BEPCSAP BEPCSAP;
        InterfazMTTO.iSBO_BE.BEOUSR BEOUSR_Rpta = new InterfazMTTO.iSBO_BE.BEOUSR();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        DataTable gtblUsuario = new DataTable();
        private BackgroundWorker bgwork = new BackgroundWorker();
        private bool gbolIngresarSistema = false;
        private string gstrMensaje = "";
        string gstrUsuario, gstrClave;

        DataTable tblMaestra = new DataTable();

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //txtPassword.Password = "1234";
            //txtUsuario.Text = "MANAGER";

            bgwork.WorkerSupportsCancellation = true;
            bgwork.DoWork += worker_DoWork;
            bgwork.RunWorkerCompleted += worker_RunWorkerCompleted;

            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            txtUsuario.Focus();
            try
            {
                Utilitarios.Utilitarios.parser = new IniParser(@System.Configuration.ConfigurationManager.AppSettings["RutaMSG"]);
                objE_TablaMaestra.IdTabla = 1000;
                tblMaestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);

                Utilitarios.Utilitarios.gstrServidorSBO = tblMaestra.Rows[1]["Valor"].ToString();
                Utilitarios.Utilitarios.gstrLicenciaServidorSBO = tblMaestra.Rows[6]["Valor"].ToString();
                Utilitarios.Utilitarios.gstrBaseDatosSBO = tblMaestra.Rows[0]["Valor"].ToString();

            
                //btnIngresar_Click(null,null);
            }
            catch (Exception ex)
            {
                Mensaje(ex.Message, 3);
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            gintTick_Mensaje += 1;
            gintTick_Cerrar += 1;
            if (gintTick_Mensaje == 3)
            {
                lblMensaje.Text = string.Empty;
                bdMensajes.Background = new SolidColorBrush(Colors.Transparent);
            }
            if (gintTick_Cerrar == 5)
            {
                App.Current.Shutdown();
            }
        }
        E_Empresa objE_Empresa = new E_Empresa();
        B_Empresa objB_Empresa = new B_Empresa();

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtUsuario.IsEnabled = false;
                txtPassword.IsEnabled = false;
                btnIngresar.IsEnabled = false;
                gstrUsuario = txtUsuario.Text;
                gstrClave = txtPassword.Password;
                if (ValidarCampos())
                {
                    txtUsuario.IsEnabled = true;
                    txtPassword.IsEnabled = true;
                    btnIngresar.IsEnabled = true;
                    return;
                }
                if (ValidarLogicaNegocio(gstrUsuario))
                {
                    txtUsuario.IsEnabled = true;
                    txtPassword.IsEnabled = true;
                    btnIngresar.IsEnabled = true;
                    return;
                }
                gbolIngresarSistema = true;
                gstrMensaje = "";
                bgwork.RunWorkerAsync();
                imgLoading.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { Mensaje("No hay Conexion. " + ex.Message, 3); }
        }

        private bool ValidarCampos()
        {
            bool rpt = false;
            if (txtUsuario.Text.Trim() == string.Empty) { Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "OBLI_USUA"), 2); rpt = true; }
            else if (txtPassword.Password.Trim() == string.Empty) { Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "OBLI_PASS"), 2); rpt = true; }
            return rpt;
        }

        private bool ValidarLogicaNegocio(string Usuario)
        {
            bool rpt = false;
            objE_Usuario.Usuario = Usuario;

            gtblUsuario = objB_Usuario.Usuario_GetItem(objE_Usuario);
            if (gtblUsuario.Rows.Count == 0)
            {
                Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "VALI_USUA"), 2);
                rpt = true;
            }
            else
            {
                if (!Convert.ToBoolean(gtblUsuario.Rows[0]["FlagActivo"]))
                {
                    Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "VALI_USUA_ACTI"), 2);
                    rpt = true;
                }
                else
                {
                    DataTable tbl = objB_Usuario.UsuarioBloqueo_GetItem(objE_Usuario);
                    if (tbl.Rows.Count > 0)
                        if (Convert.ToInt32(tbl.Rows[0]["EsBloqueado"]) == 1)
                        {
                            Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "VALI_USUA_ESTA"), 2);
                            rpt = true;
                        }
                }
            }

            return rpt;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.bgwork.CancellationPending)
            {
                e.Cancel = true;
            }
            else
            {
                string Manager = "MANAGER";
                Manager = gstrUsuario.ToUpper();
                if (!Convert.ToBoolean(gtblUsuario.Rows[0]["Licenciado"]) && gstrUsuario.ToUpper() != Manager)
                {
                    gstrMensaje = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "MENS_USUA_LICE");
                    gbolIngresarSistema = false;
                }
                else
                {
                    objE_Empresa.IdEmpresa = 1;
                    DataTable tblEmpresa = objB_Empresa.Empresa_GetItem(objE_Empresa);
                    if (tblEmpresa.Rows[0]["Licencia"].ToString() == "" && gstrUsuario.ToUpper() != Manager)
                    {
                        gstrMensaje = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "MENS_EMPR_LICE");
                        gbolIngresarSistema = false;
                    }
                    else
                    {

                        BEOUSR = new InterfazMTTO.iSBO_BE.BEOUSR()
                        {
                            CodigoUsuario = gstrUsuario,
                            Clave = gstrClave
                        };
                        BEPCSAP = new InterfazMTTO.iSBO_BE.BEPCSAP()
                        {
                            Servidor = Utilitarios.Utilitarios.gstrServidorSBO,
                            LicenciaServidor = Utilitarios.Utilitarios.gstrLicenciaServidorSBO,
                            BaseDatos = Utilitarios.Utilitarios.gstrBaseDatosSBO,
                            BDUser = ConfigurationManager.AppSettings["BDUser"],
                            BDPass = ConfigurationManager.AppSettings["BDPass"],
                            TipoServidorBD = ConfigurationManager.AppSettings["TipoServidorBD"]
                        };

                        BEOUSR_Rpta = InterfazMTTO.iSBO_BL.Usuario_BL.ValidaUsuario(BEOUSR, BEPCSAP, ref RPTA);
                        gstrMensaje = Utilitarios.Utilitarios.SubStringSeccion(RPTA.DescripcionErrorUsuario, '(', ')');
                        if (RPTA.ResultadoRetorno == 0)//Datos Correctos
                        {
                            BEOUSR.RespuestaValidacion = BEOUSR_Rpta.RespuestaValidacion;
                            if (!(tblEmpresa.Rows[0]["Licencia"].ToString() == ""))
                            {
                                byte[] LicenciaFile = tblEmpresa.Rows[0]["Licencia"] as byte[];
                                Stream stm = new MemoryStream(LicenciaFile);
                                string[] parametros = GlobalClass.CargarParametrosLicencia(new StreamReader(stm));
                                string RUC = parametros[0];
                                string RazonSocial = parametros[1];
                                int CantidadTotal = Convert.ToInt32(parametros[2]);

                                if (!InterfazMTTO.iSBO_BL.Usuario_BL.ValidaSociedad(RUC, RazonSocial, ref RPTA))
                                {
                                    gstrMensaje = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "MENS_EMPR_LICE_SAP");
                                    gbolIngresarSistema = false;
                                }
                            }
                        }
                        else if (RPTA.ResultadoRetorno == -2)
                        {
                            if (gstrMensaje.Contains("100000048") && gstrUsuario.ToUpper() != "MANAGER")//No se valida Licencia al Manager
                                gstrMensaje = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "MENS_USUA_LICE_SAP");
                            if (gstrMensaje.Contains("-107"))
                                gstrMensaje = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "MENS_USUA_DATA_SAP");
                            if (gstrMensaje.Contains("-131"))
                                gstrMensaje = "Falló la conexión con el servicio de licencias.\n\r La dirección del servicio de licencias no es válida,\n\r o el servicio de lincencias no funciona (o no responde)";

                            gbolIngresarSistema = false;
                            Contador += 1;
                            if (Contador == 3)
                            {
                                objE_Usuario.Usuario = gstrUsuario;
                                objB_Usuario.UsuarioBloqueo_Insert(objE_Usuario);
                                gstrMensaje = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaLogin, "MENS_USUA_BLOC");
                                gintTick_Cerrar = 0;
                            }
                        }
                    }
                }
                try
                {
                    string server = tblMaestra.Select("IdColumna = 11")[0]["Valor"].ToString();
                    System.Net.Sockets.TcpClient socketForServer = new System.Net.Sockets.TcpClient(server, 5432);
                    Utilitarios.Utilitarios.IsEML = true;
                }
                catch
                {
                    Utilitarios.Utilitarios.IsEML = false;
                }
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            imgLoading.Visibility = Visibility.Hidden;
            if (gbolIngresarSistema)
            {
                Utilitarios.Utilitarios.IsDB = true;
                Utilitarios.Utilitarios.IsSAP = true;
                AbrirLoading(txtUsuario.Text, txtPassword.Password);
            }
            else
            {
                btnIngresar.IsEnabled = true;
                txtUsuario.IsEnabled = true;
                txtPassword.IsEnabled = true;
                Mensaje(gstrMensaje, 3);
            }
        }

        private void AbrirLoading(string Usuario, string Clave)
        {
            //ConfigurationManager.AppSettings["CodigoUsuario"] = Usuario;
            //ConfigurationManager.AppSettings["Clave"] = Clave;
            Utilitarios.Utilitarios.gstrUsuarioSAP = Usuario;
            Utilitarios.Utilitarios.gstrPasswordSAP = Clave;
            Utilitarios.Utilitarios.gintIdRol = Convert.ToInt32(gtblUsuario.Rows[0]["IdRol"]);
            Utilitarios.Utilitarios.gintIdUsuario = Convert.ToInt32(gtblUsuario.Rows[0]["IdUsuario"]);
            Utilitarios.Utilitarios.gstrUsuario = gtblUsuario.Rows[0]["Usuario"].ToString();

            Contador = 0;
            BEOUSR = null;
            BEPCSAP = null;
            RPTA = null;
            Loading load = new Loading();
            load.Show();
            timer.Stop();
            this.Close();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            bgwork.CancelAsync();
            btnSalir.IsEnabled = false;
            this.Close();
        }

        public void Mensaje(string strMensaje, int intTipo)
        {
            lblMensaje.Text = strMensaje;
            if (intTipo == 1) //Ejecución satisfactoria
            {
                bdMensajes.Background = ColorDeFondoDegradado(1);
                lblMensaje.Foreground = System.Windows.Media.Brushes.Black;
            }
            else if (intTipo == 2) //Advertencia de validación
            {
                bdMensajes.Background = ColorDeFondoDegradado(2);
                lblMensaje.Foreground = System.Windows.Media.Brushes.Black;
            }
            else if (intTipo == 3) //Mensaje de error
            {
                bdMensajes.Background = ColorDeFondoDegradado(3);
                lblMensaje.Foreground = System.Windows.Media.Brushes.White;
            }
            gintTick_Mensaje = 0;
        }

        private LinearGradientBrush ColorDeFondoDegradado(int color)
        {
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
            myLinearGradientBrush.StartPoint = new Point(0.5, 0);
            myLinearGradientBrush.EndPoint = new Point(0.5, 1);
            if (color == 1)//Verde
            {
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 115, 248, 156), 0));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 39, 219, 95), 0.968));
            }
            else if (color == 2)//Celeste
            {
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 160, 211, 255), 0));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 111, 188, 255), 0.968));
            }
            else if (color == 3)//Rojo
            {
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 255, 108, 108), 0));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 255, 52, 52), 0.968));
            }
            return myLinearGradientBrush;
        }
    }
}
