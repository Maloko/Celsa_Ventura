using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Docking;
using System.Configuration;
using System.Data;
using Utilitarios;
using Business;
using Entities;
using System.Threading;
using System.Windows.Threading;
using System.Data.SqlClient;
using System.IO;
using System.ComponentModel;

namespace AplicacionSistemaVentura
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class InterfazPrincipal : DXWindow
    {
        public InterfazPrincipal()
        {
            InitializeComponent();
        }

        string gstrEtiquetaInterfazPrincipal = "InterfazPrincipal";

        BackgroundWorker HiloComprobarConexion = new BackgroundWorker();
        BitmapImage imgOnline = new BitmapImage(new Uri("pack://application:,,,/Image/online.png"));
        BitmapImage imgOffline = new BitmapImage(new Uri("pack://application:,,,/Image/offline.png"));
        //BitmapImage ImagenFondo = new BitmapImage(new Uri("pack://application:,,,/Image/FondoPantalla/camion.png"));

        BitmapImage gimgEML;
        BitmapImage gimgDB;
        BitmapImage gimgSAP;
        Boolean gbolVulneracion = false;
        Boolean gbolManipulacion = false;

        ErrorHandler objError = new ErrorHandler();
        B_Conexion objB_Conexion = new B_Conexion();
        B_Alertas objB_Alertas = new B_Alertas();
        E_Alertas objE_Alertas = new E_Alertas();
        DataTable tblFechHoraServ = new DataTable();

        double HeightScreen = System.Windows.SystemParameters.PrimaryScreenHeight;
        double WidthScreen = System.Windows.SystemParameters.PrimaryScreenWidth;
        int gintTickConexion = 0;
        public int gintTickMensaje = 0;
        string gstrServidorSBO = "";
        string gstrLicenciaServidorSBO = "";
        string gstrBaseDatosSBO = "";
        string gstrRol = "";
        int gintIdRol = 0;
        double gdblTiempoAlertas = 0;
        int gintTiempoSegundosAlertas = 0;
        //Abrir conexión SAP
        //Conexion a SAP
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        InterfazMTTO.iSBO_BE.BEOUSR BEOUSR = new InterfazMTTO.iSBO_BE.BEOUSR();
        InterfazMTTO.iSBO_BE.BEOUSR BEOUSR_Rpta = new InterfazMTTO.iSBO_BE.BEOUSR();
        InterfazMTTO.iSBO_BE.BEPCSAP BEPCSAP = new InterfazMTTO.iSBO_BE.BEPCSAP();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();

        E_Empresa objE_Empresa = new E_Empresa();
        B_Empresa objB_Empresa = new B_Empresa();
        E_Usuario objE_Usuario = new E_Usuario();
        B_Usuario objB_Usuario = new B_Usuario();
        E_Rol objE_Rol = new E_Rol();
        B_Rol objB_Rol = new B_Rol();

        DataTable gtblMenuOld = new DataTable();
        DataTable tblMaestra = new DataTable();
        SqlConnectionStringBuilder builder;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        List<string> gstrRolMenu_List0 = new List<string>();

        B_Settings objB_Settings = new B_Settings();

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalClass.ip = this;
                GlobalClass.CultureInfo = new System.Globalization.CultureInfo("es-PE", false).NumberFormat;

                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-PE");
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                
                HiloComprobarConexion.WorkerSupportsCancellation = true;
                HiloComprobarConexion.DoWork += new DoWorkEventHandler(HiloComprobarConexion_DoWork);
                HiloComprobarConexion.RunWorkerCompleted += new RunWorkerCompletedEventHandler(HiloComprobarConexion_RunWorkerCompleted);

                imgEML.Visibility = (Utilitarios.Utilitarios.gintIdRol == 2) ? Visibility.Visible : Visibility.Collapsed;

                tblFechHoraServ = Utilitarios.Utilitarios.Fecha_Hora_Servidor();
                Utilitarios.Utilitarios.gdttFecha = Utilitarios.Utilitarios.FechaHora_Servidor();//Convert.ToDateTime(tblFechHoraServ.Rows[0]["FechaServer"]);
                Utilitarios.Utilitarios.gdttHora = Utilitarios.Utilitarios.FechaHora_Servidor();//Convert.ToDateTime(tblFechHoraServ.Rows[0]["HoraServer"]);
                lblFechaHora.Text = Utilitarios.Utilitarios.gdttFecha.ToShortDateString() + " - " + Utilitarios.Utilitarios.gdttHora.ToShortTimeString();
                imgEML.Source = (Utilitarios.Utilitarios.IsEML) ? imgOnline : imgOffline;
                imgDB.Source = (Utilitarios.Utilitarios.IsDB) ? imgOnline : imgOffline;
                imgSAP.Source = (Utilitarios.Utilitarios.IsSAP) ? imgOnline : imgOffline;

                builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["BDVentura"].ToString());
                //Ocultando Reporte de Indicadores
                NBIndicadores.IsVisible = false;
                TBIndicadores.IsVisible = false;

                
                objE_TablaMaestra.IdTabla = 1000;
                tblMaestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);

                objE_Usuario.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
                DataRow FRol = objB_Rol.Rol_GetItemByUsuario(objE_Usuario).Rows[0];
                gintIdRol = Convert.ToInt32(FRol["Idrol"]);
                gstrRol = FRol["Rol"].ToString();
                lblRol.Text = gstrRol;
                lblUsuario.Text = Utilitarios.Utilitarios.gstrUsuario;
                if (Utilitarios.Utilitarios.gintIdUsuario == 1)
                    imgUser.Source = new BitmapImage(new Uri("pack://application:,,,/Image/Iconos/user_Manager.png"));
                if (gintIdRol == 1)
                    imgRol.Source = new BitmapImage(new Uri("pack://application:,,,/Image/Iconos/Role_Manager.png"));

                ActualizarControlAcceso();
                gtblMenuOld = objB_Rol.Menu_List();

                gstrServidorSBO = tblMaestra.Rows[1]["Valor"].ToString();
                gstrLicenciaServidorSBO = tblMaestra.Rows[6]["Valor"].ToString();
                gstrBaseDatosSBO = tblMaestra.Rows[0]["Valor"].ToString();

                //Actualizar hora cada seg.

                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();

                imgDB.ToolTip = builder.DataSource + "-->" + builder.InitialCatalog;
                imgSAP.ToolTip = gstrServidorSBO + "-->" + gstrBaseDatosSBO;

                VerificarAlertas();
                OcultarReportes();
            }
            catch (Exception ex)
            {
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void OcultarReportes() 
        {
            if (objB_Settings.GetValueSetting("TipoServidorBD").Equals("dst_HANADB"))//dst_MSSQL2012
            {
                Paquete04.IsVisible = false;
            }
        }

        private void DefinirHijo(string FormularioXAML, string Titulo, double Ancho, double Alto)
        {
            try
            {
                if (ControlAcceso(FormularioXAML)) return;
                DocumentPanel DocPanel = new DocumentPanel();
                DocPanel.Content = new Uri(FormularioXAML, UriKind.Relative);
                DocPanel.Caption = Titulo;
                DocumentPanel.SetMDISize(DocPanel, new Size(Ancho, Alto));
                mdiContenedor.Add(DocPanel);
                DocPanel.IsActive = true;
            }
            catch (Exception ex)
            {
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void AbrirDetalleTablaMestra(string FormularioXAML, string Titulo, double Ancho, double Alto)
        {
            DocumentPanel DocPanel = new DocumentPanel();
            DocPanel.Content = new Uri(FormularioXAML, UriKind.Relative);
            DocPanel.Caption = Titulo;
            DocumentPanel.SetMDISize(DocPanel, new Size(Ancho, Alto));
            mdiContenedor.Add(DocPanel);
            DocPanel.IsActive = true;
        }

        private bool ControlAcceso(string URLFrm)
        {
            bool rpt = true;
            string msg = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaInterfazPrincipal, "VALI_PERM");
            foreach (string frm in gstrRolMenu_List0)
            {
                if (URLFrm.Contains(frm) && frm != "")
                {
                    rpt = false;
                    break;
                }
            }
            if (rpt) Mensaje(msg, 2);
            return rpt;
        }

        private void ActualizarControlAcceso()
        {
            objE_Rol.IdRol = gintIdRol;
            DataTable tblMenurol = objB_Rol.Rol_Menu_List(objE_Rol);
            gstrRolMenu_List0 = new List<string>();
            foreach (DataRow fila in tblMenurol.Select("FlagActivo = 'True' AND IdTipo IN (3,4,5,6)"))
            {
                gstrRolMenu_List0.Add(fila["Formulario"].ToString());
            }
        }

        private bool ActualizacionMenu()
        {
            bool rpt = false;
            DataTable tblMenuNew = objB_Rol.Menu_List();
            DataTable tbl = Utilitarios.Utilitarios.Datatable_Diferencias(gtblMenuOld, tblMenuNew);
            if (tbl.Rows.Count > 0)
            {
                rpt = true;
                gtblMenuOld = new DataTable();
                gtblMenuOld = tblMenuNew;
            }
            return rpt;
        }

        private void VerificarAlertas()
        {
            try
            {
                objE_TablaMaestra.IdTabla = 1000;
                int TipoFrecuencia = Convert.ToInt32(B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).Select("IdColumna = 10")[0]["Valor"]);

                objE_TablaMaestra.IdTabla = 61;
                int SegundosPorTipo = Convert.ToInt32(B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).Select("IdColumna = " + TipoFrecuencia)[0]["Valor"]);

                objE_TablaMaestra.IdTabla = 60;
                gdblTiempoAlertas = Convert.ToDouble(B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).Select("IdColumna = 1")[0]["Valor"]);

                gdblTiempoAlertas = gdblTiempoAlertas * SegundosPorTipo;

                objE_Alertas.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
                DataTable tblAlertas = objB_Alertas.Alertas_GetItems(objE_Alertas);
                int nuevasAlertas = tblAlertas.Select("FlagLeido = 0").Length;
                if (nuevasAlertas > 0)
                {
                    if (ValidaUserControlAbierto(GlobalClass.FrmAlerta))
                    {
                        CerraChild(GlobalClass.FrmAlerta);
                        AbrirControlAlertas();
                    }
                    else
                    {
                        AbrirControlAlertas();
                    }
                }
            }
            catch (Exception ex)
            {
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            gintTickConexion += 1;
            gintTickMensaje += 1;
            gintTiempoSegundosAlertas += 1;

            if (gintTickConexion == 21)
            {
                if (HiloComprobarConexion.IsBusy != true)
                {
                    HiloComprobarConexion.RunWorkerAsync();
                }
                gintTickConexion = 0;
            }
            if (gintTickMensaje == 6)
            {
                lblError.Text = string.Empty;
                bdMensajes.Background = new SolidColorBrush(Colors.Transparent);
                gintTickMensaje = 0;
            }
            if (gintTiempoSegundosAlertas >= gdblTiempoAlertas) //gintTiempoAlertas
            {
                gintTiempoSegundosAlertas = 0;
                VerificarAlertas();
            }
        }

        public void HiloComprobarConexion_DoWork(object sender, DoWorkEventArgs e)
        {
            //ComprobarConexion();

            //Comprobar la conexión de la aplicación de correos
            if (Utilitarios.Utilitarios.gintIdRol == 2)
            {
                try
                {
                    string server = tblMaestra.Select("IdColumna = 11")[0]["Valor"].ToString();
                    System.Net.Sockets.TcpClient socketForServer = new System.Net.Sockets.TcpClient(server, 55950);
                    gimgEML = imgOnline;
                }
                catch
                {
                    gimgEML = imgOffline;
                }
            }


            //Comprobar la conexión de la base de datos MMTO
            try
            {
                SqlConnection cx = objB_Conexion.ObtenerConexion();
                cx.Open();
                gimgDB = imgOnline;
                //Cargando tabla maestra
                objE_TablaMaestra.IdTabla = 0;
                Utilitarios.Utilitarios.tblMaestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);

                tblFechHoraServ = Utilitarios.Utilitarios.Fecha_Hora_Servidor();

                Utilitarios.Utilitarios.gdttFecha = Convert.ToDateTime(tblFechHoraServ.Rows[0]["FechaServer"]);
                Utilitarios.Utilitarios.gdttHora = Convert.ToDateTime(tblFechHoraServ.Rows[0]["HoraServer"]);

                gbolVulneracion = ComprobarCantidadLicencia();

                if (ActualizacionMenu())
                {
                    gbolManipulacion = true;
                }

                cx.Close();
                cx.Dispose();
                cx = null;
            }
            catch
            {
                gimgDB = imgOffline;
            }

            //Comprobar la conexión de SAP
            try
            {
                BEOUSR = null;
                BEPCSAP = null;
                BEOUSR_Rpta = null;
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                GC.Collect();

                if (InterfazMTTO.iSBO_BL.Usuario_BL.ValidaEstadoConexion(ref RPTA))
                    gimgSAP = imgOnline;
                else
                {
                    gimgSAP = imgOffline;
                    //BEOUSR.RespuestaValidacion = InterfazMTTO.iSBO_BL.Usuario_BL.ValidaUsuario(BEOUSR, BEPCSAP, ref RPTA);

                    BEOUSR = new InterfazMTTO.iSBO_BE.BEOUSR();
                    BEOUSR_Rpta = new InterfazMTTO.iSBO_BE.BEOUSR();
                    BEPCSAP = new InterfazMTTO.iSBO_BE.BEPCSAP();
                    RPTA = new InterfazMTTO.iSBO_BE.BERPTA();


                    BEOUSR.CodigoUsuario = Utilitarios.Utilitarios.gstrUsuarioSAP;// ConfigurationManager.AppSettings["CodigoUsuario"];
                    BEOUSR.Clave = Utilitarios.Utilitarios.gstrPasswordSAP;// ConfigurationManager.AppSettings["Clave"];
                    BEPCSAP.Servidor = gstrServidorSBO;// ConfigurationManager.AppSettings["Servidor"];1
                    BEPCSAP.LicenciaServidor = gstrLicenciaServidorSBO;// ConfigurationManager.AppSettings["LicenciaServidor"];6
                    BEPCSAP.BaseDatos = gstrBaseDatosSBO;// ConfigurationManager.AppSettings["BaseDatos"];0
                    BEPCSAP.BDUser = ConfigurationManager.AppSettings["BDUser"];
                    BEPCSAP.BDPass = ConfigurationManager.AppSettings["BDPass"];
                    BEPCSAP.TipoServidorBD = ConfigurationManager.AppSettings["TipoServidorBD"];


                    BEOUSR_Rpta = InterfazMTTO.iSBO_BL.Usuario_BL.ValidaUsuario(BEOUSR, BEPCSAP, ref RPTA);
                    BEOUSR.RespuestaValidacion = BEOUSR_Rpta.RespuestaValidacion;
                    if (RPTA.ResultadoRetorno != 0)
                    {
                        Mensaje(RPTA.DescripcionErrorUsuario, 3);
                    }
                }
            }
            catch
            {
                gimgSAP = imgOffline;
            }
        }

        public void HiloComprobarConexion_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblFechaHora.Text = lblFechaHora.Text = Utilitarios.Utilitarios.gdttFecha.ToShortDateString() + " - " + Utilitarios.Utilitarios.gdttHora.ToShortTimeString();
            imgEML.Source = gimgEML;
            imgDB.Source = gimgDB;
            imgSAP.Source = gimgSAP;
            if (gbolVulneracion)
            {
                BloquearAplicacion();
                System.Threading.Thread.Sleep(5000);
            }
            else
            {
                this.IsEnabled = true;
            }

            if (gbolManipulacion)
            {
                Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaInterfazPrincipal, "MENS_MENU_MANI"), 2);
            }
        }

        public void BloquearAplicacion()
        {
            this.IsEnabled = false;
            Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaInterfazPrincipal, "MENS_SEGU_VULN"), 3);
        }
        ///<summary>
        ///<para>El Primer Parametro sera Enfocado</para>
        ///<para>El Resto de parametro no seran deshabilitados</para>
        ///</summary>
        public void SeleccionarTab(params TabItem[] tabs)
        {

            tabs[0].IsSelected = true;
            TabControl OtrosTabs = (TabControl)tabs[0].Parent;
            foreach (TabItem item in OtrosTabs.Items)
            {
                item.IsEnabled = (tabs.Contains(item));
            }
        }

        public void CerraChild(UserControl hijo)
        {
            #region REQUERIMIENTO_03
            //mdiContenedor.Remove((DocumentPanel)hijo.Parent);
            try
            {
                dynamic parent = mdiContenedor.GetItems();
                for (int i = 0; i < mdiContenedor.GetItems().Length; i++)
                {
                    if (parent[i].Control.ToString() == hijo.ToString())
                    {
                        mdiContenedor.Remove(parent[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "";
                mensaje = ex.ToString();
            }
            #endregion
        }

        public void ActivarPanel(DependencyObject obj)
        {
            Button tb = obj as Button;
            if (tb != null)
                tb.IsEnabled = true;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj as DependencyObject); i++)
                ActivarPanel(VisualTreeHelper.GetChild(obj, i));
        }

        public void VentanaEmergente_Visibilidad(object sender)
        {
            FrameworkElement Ventana = (FrameworkElement)sender;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(Ventana.Parent); i++)
            {
                var control = VisualTreeHelper.GetChild(Ventana.Parent, i);
                if (control != Ventana)
                    (control as FrameworkElement).IsEnabled = (Ventana.IsVisible) ? false : true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            App.Current.Shutdown();
        }

        public void Mensaje(string strMensaje, int intTipo)
        {
            try
            {
                lblError.Text = strMensaje;
                if (intTipo == 1) //Ejecución satisfactoria
                {
                    bdMensajes.Background = ColorDeFondoDegradado(1);
                    lblError.Foreground = System.Windows.Media.Brushes.Black;
                }
                else if (intTipo == 2) //Advertencia de validación
                {
                    bdMensajes.Background = ColorDeFondoDegradado(2);
                    lblError.Foreground = System.Windows.Media.Brushes.Black;
                }
                else if (intTipo == 3) //Mensaje de error
                {
                    bdMensajes.Background = ColorDeFondoDegradado(3);
                    lblError.Foreground = System.Windows.Media.Brushes.White;
                }
                gintTickMensaje = 0;
            }
            catch (Exception ex)
            {
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
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

        private Boolean ValidaUserControlAbierto(UserControl hijo)
        {
            if (hijo == null) return false;
            return mdiContenedor.Items.Contains((DocumentPanel)hijo.Parent);
        }

        private bool ComprobarCantidadLicencia()
        {
            bool rpt = false;
            int CantidadTotal = 0, CantidadUsuada = 0;
            try
            {
                objE_Empresa.IdEmpresa = 1;
                DataTable tblEmpresa = objB_Empresa.Empresa_GetItem(objE_Empresa);
                if (!(tblEmpresa.Rows[0]["Licencia"].ToString() == ""))
                {
                    byte[] LicenciaFile = tblEmpresa.Rows[0]["Licencia"] as byte[];
                    Stream stm = new MemoryStream(LicenciaFile);
                    string[] parametros = GlobalClass.CargarParametrosLicencia(new StreamReader(stm));
                    CantidadTotal = Convert.ToInt32(parametros[2]);
                    objE_Usuario.FlagActivo = 2;
                    DataTable tbl = objB_Usuario.Usuario_List(objE_Usuario);
                    CantidadUsuada = Convert.ToInt32(tbl.Compute("Count(IdUsuario)", "Licenciado = 'TRUE' AND IdUsuario <> 1 AND FlagActivo = 1"));
                }
                if (CantidadUsuada > CantidadTotal) rpt = true;
            }
            catch (Exception ex)
            {
                Mensaje(ex.Message, 3);
            }
            return rpt;
        }


        #region METODOS DE INVOCACION NUEVA VENTANA
        public void AbrirGestionActividad()
        {
            DefinirHijo(@"PAQ01-Definicion\GestActividad.xaml", Utilitarios.Utilitarios.parser.GetSetting("GestActividad", "FORM_CAPT"), 640, 590);
        }
        public void AbrirGestionHerramientaEspecial()
        {
            DefinirHijo(@"PAQ01-Definicion\GestHerramientaEspecial.xaml", Utilitarios.Utilitarios.parser.GetSetting("GestHerramientaEspecial", "FORM_CAPT"), 990, 560);
        }
        public void AbrirGestMoviNeumatico()
        {
            DefinirHijo(@"PAQ01-Definicion\GestMoviNeumatico.xaml", Utilitarios.Utilitarios.parser.GetSetting("GestMoviNeumatico", "FORM_CAPT"), 1210, 580);
        }
        public void AbrirGestionNeumatico()
        {
            DefinirHijo(@"PAQ01-Definicion\GestNeumatico.xaml", Utilitarios.Utilitarios.parser.GetSetting("GestNeumatico", "FORM_CAPT"), 810, 500);
        }
        public void AbrirGestionPerfilNeumatico()
        {
            DefinirHijo(@"PAQ01-Definicion\GestPerfilNeumatico.xaml", Utilitarios.Utilitarios.parser.GetSetting("GestPerfilNeumatico", "FORM_CAPT"), 1020, 570);
        }
        public void AbrirGestionPerfilUnidadControl()
        {
            DefinirHijo(@"PAQ01-Definicion\GestPerfilUC.xaml", Utilitarios.Utilitarios.parser.GetSetting("GestPerfilUC", "FORM_CAPT"), 1035, 625);
        }
        public void AbrirGestionTarea()
        {
            DefinirHijo(@"PAQ01-Definicion\GestTarea.xaml", Utilitarios.Utilitarios.parser.GetSetting("GestTarea", "FORM_CAPT"), 790, 620);
        }
        public void AbrirUnidadControl()
        {
            DefinirHijo(@"PAQ01-Definicion\GestUnidadControl.xaml", Utilitarios.Utilitarios.parser.GetSetting("GestUnidadControl", "FORM_CAPT"), 1035, 625);
        }
        //Paquete 02 - Planificacion
        public void AbrirPlanMantenimiento()
        {
            DefinirHijo(@"PAQ02-Planificacion\PlanGestionMantenimiento.xaml", Utilitarios.Utilitarios.parser.GetSetting("PlanGestionMantenimiento", "FORM_CAPT"), 1035, 570);
        }
        public void AbrirPlanPrioridadMantenimiento()
        {
            DefinirHijo(@"PAQ02-Planificacion\PlanPriorizaMantenimiento.xaml", Utilitarios.Utilitarios.parser.GetSetting("PlanPriorizaMantenimiento", "FORM_CAPT"), 450, 550);
        }
        public void AbrirPlanUC()
        {
            DefinirHijo(@"PAQ02-Planificacion\PlanProgramacionUC.xaml", Utilitarios.Utilitarios.parser.GetSetting("PlanProgramacionUC", "FORM_CAPT"), 1035, 600);
        }
        //Paquete 3

        public void AbrirControlNeumatico()
        {
            DefinirHijo(@"PAQ03-Ejecucion\EjecControlNeumatico.xaml", Utilitarios.Utilitarios.parser.GetSetting("EjecControlNeumatico", "FORM_CAPT"), 1035, 625);
        }
        public void AbrirRegistroOT()
        {
            DefinirHijo(@"PAQ03-Ejecucion\EjecGestionOTDato.xaml", "Permisos de Acceso", 1035, 625);
        }
        public void AbrirRegistroTR()
        {
            DefinirHijo(@"PAQ03-Ejecucion\EjecRegistroTR.xaml", "Permisos de Acceso", 1035, 625);
        }
        public void AbrirGestionHojaInspeccion()
        {
            DefinirHijo(@"PAQ03-Ejecucion\EjecGestHojaInspec.xaml", Utilitarios.Utilitarios.parser.GetSetting("EjecGestHojaInspec", "FORM_CAPT"), 1000, 625);
        }
        public void AbrirGestionHojaRequerimiento()
        {
            DefinirHijo(@"PAQ03-Ejecucion\EjecGestHojaRequer.xaml", Utilitarios.Utilitarios.parser.GetSetting("EjecGestHojaRequer", "FORM_CAPT"), 880, 590);
        }
        public void AbrirGestionTranferenciaParte()
        {
            DefinirHijo(@"PAQ03-Ejecucion\EjecGestTransferParte.xaml", Utilitarios.Utilitarios.parser.GetSetting("EjecGestTransferParte", "FORM_CAPT"), 1035, 600);
        }

        public void AbrirGestionRegistroIncidencia()
        {
            DefinirHijo(@"PAQ03-Ejecucion\EjecGestRegInci.xaml", Utilitarios.Utilitarios.parser.GetSetting("EjecGestRegInci", "FORM_CAPT"), 965, 530);
        }
        public void AbrirGestionOrdendeTrabajo()
        {
            DefinirHijo(@"PAQ03-Ejecucion\EjecGestionOT.xaml", Utilitarios.Utilitarios.parser.GetSetting("EjecGestionOT", "FORM_CAPT"), 1035, 625);
        }

        public void AbrirOT()
        {
            DefinirHijo(@"PAQ03-Ejecucion\EjecGestionOT.xaml", Utilitarios.Utilitarios.parser.GetSetting("EjecGestionOT", "FORM_CAPT"), 1035, 625);
        }
        private void AbrirConfiguracionTP()
        {
            DefinirHijo(@"PAQ05-Utilitarios\UtilConfiguracionTP.xaml", "Configuración de Tabla de Parámetros", 875, 530);
        }
        public void AbrirDetalleTablaMaestra(int IdTabla)
        {
            GlobalClass.IdTabla = IdTabla;
            AbrirDetalleTablaMestra(@"PAQ05-Utilitarios\UtilConfiguracionDetalleTM.xaml", "Gestión Detalle de Tabla Maestra", 840, 520);
        }
        //Paquete 04 - Reportes
        public void ReporteTransferenciaParte()
        {
            DefinirHijo(@"PAQ04-Reportes\ReporteTransferenciaPartes.xaml", "Reporte Transferencia de Partes", 1035, 625);
        }
        public void ReporteListadoHojaRequerimiento()
        {
            DefinirHijo(@"PAQ04-Reportes\ReporteListadoHojaRequerimiento.xaml", "Listado de Hoja de Requerimientos", 1035, 625);
        }
        public void ReporteListadoHojaInspeccion()
        {
            DefinirHijo(@"PAQ04-Reportes\ReporteListadoHojaInspeccion.xaml", "Listado de Hoja de Inspección", 1035, 625);
        }
        public void ReporteTopTaller()
        {
            DefinirHijo(@"PAQ04-Reportes\ReporteTopTaller.xaml", "Reporte Top de Taller", 1035, 625);
        }
        public void ReporteMonitoreoLineaOT()
        {
            DefinirHijo(@"PAQ04-Reportes\ReporteMonitoreoLineaOT.xaml", "Reporte Monitoreo en Linea O/T", 1035, 625);
        }
        public void ReporteDisponibilidadStockOT()
        {
            DefinirHijo(@"PAQ04-Reportes\ReporteDisponibilidadStockOT.xaml", "Reporte Disponibilidad de Stock O/T", 1035, 625);
        }
        public void ReporteComparativo()
        {
            DefinirHijo(@"PAQ04-Reportes\ReporteComparativo.xaml", "Reporte Comparativo Estimado vs. Real", 1035, 625);
        }
        public void ReporteTrabajoMecanico()
        {
            DefinirHijo(@"PAQ04-Reportes\ReporteTrabajoMecanico.xaml", "Reporte Trabajo Realizado por Mecanico", 1035, 625);
        }
        public void ReporteCostoMantenimiento()
        {
            DefinirHijo(@"PAQ04-Reportes\ReporteCostoMantenimiento.xaml", "Reporte Costo de Mantenimientos", 1035, 625);
        }
        public void ReporteSchedulerProgramacionOT()
        {
            DefinirHijo(@"PAQ04-Reportes\SchedulerProgramacionOT.xaml", "Reporte Programación de O/T", 1035, 625);
        }
        public void Indicadores()
        {
            DefinirHijo(@"PAQ04-Reportes\Indicadores.xaml", "Tablero de Control (Indicadores)", 1035, 625);
        }
        public void AbrirSeguConfigAcceso()
        {
            DefinirHijo(@"PAQ06-Seguridad\SeguConfigAcceso.xaml", "Gestión de Rol y Acceso", 780, 500);
        }

        public void AbrirCargaMasiva()
        {
            DefinirHijo(@"PAQ05-Utilitarios\UtilCargaMasiva.xaml", "Carga Masiva de Datos", 850, 600);
        }

        private void AbrirCargaMasivaPN()
        {
            DefinirHijo(@"PAQ05-Utilitarios\UtilCargaMasivaPN.xaml", "Carga Masiva de Perfil Neumatico", 730, 510);
        }
        private void AbrirCargaMasivaN()
        {
            DefinirHijo(@"PAQ05-Utilitarios\UtilCargaMasivaN.xaml", "Carga Masiva de Neumatico", 730, 510);
        }

        public void AbrirControlAlertas()
        {
            DefinirHijo(@"PAQ04-Reportes\ControlAlertas.xaml", "Control de Alertas", 700, 480);
        }
        public void AbrirUsuarioAsinacionLicencia()
        {
            DefinirHijo(@"PAQ06-Seguridad\SeguUsuarioAsignaLicencia.xaml", "Usuario y Asignación de Licencias", 750, 490);
        }
        public void UtilFormatosImpresion()
        {
            DefinirHijo(@"PAQ05-Utilitarios\FormatosImpresion.xaml", "Formatos de Impresión", 1035, 623);
        }

        #endregion

        #region Navigate_Click
        private void NBGestActividad_Click(object sender, EventArgs e)
        {
            AbrirGestionActividad();
        }
        private void NbGestTarea_Click(object sender, EventArgs e)
        {
            AbrirGestionTarea();
        }
        private void NBGestHerramientaEspecial_Click(object sender, EventArgs e)
        {
            AbrirGestionHerramientaEspecial();
        }
        private void NBGestPerfilUC_Click(object sender, EventArgs e)
        {
            AbrirGestionPerfilUnidadControl();
        }
        private void NBGestPerfilNeumatico_Click(object sender, EventArgs e)
        {
            AbrirGestionPerfilNeumatico();
        }
        private void NBGestNeumatico_Click(object sender, EventArgs e)
        {
            AbrirGestionNeumatico();
        }
        private void NBGestUnidadControl_Click(object sender, EventArgs e)
        {
            AbrirUnidadControl();
        }
        private void NBGestMoviNeumaticos_Click(object sender, EventArgs e)
        {
            AbrirGestMoviNeumatico();
        }
        private void NBPlanGestionMantenimiento_Click(object sender, EventArgs e)
        {
            AbrirPlanMantenimiento();
        }
        private void NBPlanPriozaMantenimiento_Click(object sender, EventArgs e)
        {
            AbrirPlanPrioridadMantenimiento();
        }
        private void NBPlanProgramacionUC_Click(object sender, EventArgs e)
        {
            AbrirPlanUC();
        }
        private void NBEjecGestionOT_Click(object sender, EventArgs e)
        {
            AbrirOT();
        }
        private void NBEjecGestTransferParte_Click(object sender, EventArgs e)
        {
            AbrirGestionTranferenciaParte();
        }
        private void NBEjecGestHojaRequer_Click(object sender, EventArgs e)
        {
            AbrirGestionHojaRequerimiento();
        }
        private void NBEjecGestHojaInspec_Click(object sender, EventArgs e)
        {
            AbrirGestionHojaInspeccion();
        }
        private void NBEjecGestRegInci_Click(object sender, EventArgs e)
        {
            AbrirGestionRegistroIncidencia();
        }
        private void NBEjecControlNeumatico_Click(object sender, EventArgs e)
        {
            AbrirControlNeumatico();
        }
        private void NBUtilConfiguracionTP_Click(object sender, EventArgs e)
        {
            AbrirConfiguracionTP();
        }
        //Paquete04 - Reportes
        private void NBReporteTransferenciaParte_Click(object sender, EventArgs e)
        {
            ReporteTransferenciaParte();
        }
        private void NBReporteListadoHojaRequerimiento_Click(object sender, EventArgs e)
        {
            ReporteListadoHojaRequerimiento();
        }
        private void NBReporteListadoHojaInspeccion_Click(object sender, EventArgs e)
        {
            ReporteListadoHojaInspeccion();
        }
        private void NBReporteTopTaller_Click(object sender, EventArgs e)
        {
            ReporteTopTaller();
        }
        private void NBReporteMonitoreoLineaOT_Click(object sender, EventArgs e)
        {
            ReporteMonitoreoLineaOT();
        }
        private void NBReporteDisponibilidadStockOT_Click(object sender, EventArgs e)
        {
            ReporteDisponibilidadStockOT();
        }
        private void NBReporteComparativo_Click(object sender, EventArgs e)
        {
            ReporteComparativo();
        }
        private void NBReporteTrabajoMecanico_Click(object sender, EventArgs e)
        {
            ReporteTrabajoMecanico();
        }
        private void NBReporteCostoMantenimiento_Click(object sender, EventArgs e)
        {
            ReporteCostoMantenimiento();
        }
        private void NBReporteSchedulerProgramacionOT_Click(object sender, EventArgs e)
        {
            ReporteSchedulerProgramacionOT();
        }
        private void NBIndicadores_Click(object sender, EventArgs e)
        {
            Indicadores();
        }
        private void NBSeguConfigAcceso_Click(object sender, EventArgs e)
        {
            AbrirSeguConfigAcceso();
        }

        //Paquete04 - Reportes
        private void ReporteTransferenciaParte_Click(object sender, ItemClickEventArgs e)
        {
            ReporteTransferenciaParte();
        }
        private void ReporteListadoHojaRequerimiento_Click(object sender, ItemClickEventArgs e)
        {
            ReporteListadoHojaRequerimiento();
        }
        private void ReporteListadoHojaInspeccion_Click(object sender, ItemClickEventArgs e)
        {
            ReporteListadoHojaInspeccion();
        }
        private void ReporteTopTaller_Click(object sender, ItemClickEventArgs e)
        {
            ReporteTopTaller();
        }
        private void ReporteMonitoreoLineaOT_Click(object sender, ItemClickEventArgs e)
        {
            ReporteMonitoreoLineaOT();
        }
        private void ReporteDisponibilidadStockOT_Click(object sender, ItemClickEventArgs e)
        {
            ReporteDisponibilidadStockOT();
        }
        private void ReporteComparativo_Click(object sender, ItemClickEventArgs e)
        {
            ReporteComparativo();
        }
        private void ReporteTrabajoMecanico_Click(object sender, ItemClickEventArgs e)
        {
            ReporteTrabajoMecanico();
        }
        private void ReporteCostoMantenimiento_Click(object sender, ItemClickEventArgs e)
        {
            ReporteCostoMantenimiento();
        }
        private void ReporteSchedulerProgramacionOT_Click(object sender, ItemClickEventArgs e)
        {
            ReporteSchedulerProgramacionOT();
        }
        private void Indicadores_Click(object sender, ItemClickEventArgs e)
        {
            Indicadores();
        }
        private void NBUtilCargaMasiva_Click(object sender, EventArgs e)
        {
            AbrirCargaMasiva();
        }

        private void NBSeguUsuarioAsignaLicencia_Click(object sender, EventArgs e)
        {
            AbrirUsuarioAsinacionLicencia();
        }

        private void NBUtilFormatosImpresion_Click(object sender, EventArgs e)
        {
            UtilFormatosImpresion();
        }

        private void NBUtilCargaMasivaPN_Click(object sender, EventArgs e)
        {
            AbrirCargaMasivaPN();
        }

        private void NBUtilCargaMasivaN_Click(object sender, EventArgs e)
        {
            AbrirCargaMasivaN();
        }
        #endregion

        #region ToolBar
        private void GestActividad_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestionActividad();
        }
        private void GestTarea_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestionTarea();
        }
        private void GestHerramientaEspecial_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestionHerramientaEspecial();
        }
        private void GestPerfilUC_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestionPerfilUnidadControl();
        }
        private void GestPerfilNeumatico_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestionPerfilNeumatico();
        }
        private void GestUnidadControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirUnidadControl();
        }
        private void GestNeumatico_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestionNeumatico();
        }

        private void GestMovimiNeumatico_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestMoviNeumatico();
        }

        private void PlanGestionMantenimiento_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirPlanMantenimiento();
        }
        private void PlanPriozaMantenimiento_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirPlanPrioridadMantenimiento();
        }
        private void PlanProgramacionUC_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirPlanUC();
        }
        private void EjecGestionOT_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirOT();
        }
        private void EjecGestTransferParte_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestionTranferenciaParte();
        }
        private void EjecGestHojaRequer_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestionHojaRequerimiento();
        }
        private void EjecGestHojaInspec_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestionHojaInspeccion();
        }
        private void EjecGestRegInci_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirGestionRegistroIncidencia();
        }
        private void EjecControlNeumatico_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirControlNeumatico();
        }
        private void SeguConfigAcceso_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirSeguConfigAcceso();
        }

        private void SeguAsignaLicencia_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirUsuarioAsinacionLicencia();
        }
        private void TBUtilCargaMasiva_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirCargaMasiva();
        }
        private void TBUtilFormatosImpresion_ItemClick(object sender, ItemClickEventArgs e)
        {
            UtilFormatosImpresion();
        }
        private void TBAlertas_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirControlAlertas();
        }

        #endregion

        private void TBUtilConfiguracionTP_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirConfiguracionTP();
        }
    }
}
