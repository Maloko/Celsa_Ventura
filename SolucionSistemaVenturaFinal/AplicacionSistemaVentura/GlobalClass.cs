using System;
using System.Text;
using System.Globalization;
using DevExpress.Mvvm;
using System.Windows.Data;
using Utilitarios;
using Business;
using Entities;
using System.Data;
using System.IO;
using System.Configuration;
using System.Windows;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace AplicacionSistemaVentura
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ruta = "/AplicacionSistemaVentura;component/Image/Semaforo/" + value + ".png";
            return ruta;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ruta = "/AplicacionSistemaVentura;component/Image/Semaforo/" + value + ".png";
            return ruta;
        }
    }

    public class EmailImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = "";
            bool valor = (Boolean)value;
            switch (valor)
            {
                case false: color = "/AplicacionSistemaVentura;component/Image/email-nuevo.png"; break;
                case true: color = "/AplicacionSistemaVentura;component/Image/email_visto.png"; break;
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = "";
            bool valor = (Boolean)value;
            switch (valor)
            {
                case false: color = "/AplicacionSistemaVentura;component/Image/email-nuevo.png"; break;
                case true: color = "/AplicacionSistemaVentura;component/Image/email_visto.png"; break;
            }
            return color;
        }
    }

    public class EmailForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = "";
            bool valor = (Boolean)value;
            switch (valor)
            {
                case false: color = "Red"; break;
                case true: color = "Black"; break;
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { string color = "Black"; return color; }
    }

    public class EmailFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = "";
            bool valor = (Boolean)value;
            switch (valor)
            {
                case false: color = "Bold"; break;
                case true: color = "Normal"; break;
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { string color = "Normal"; return color; }
    }

    [Serializable]
    public class DataTiposOT
    {
        public TiposOT[] TipoOT { get { return TipoOTData.TipoOT; } }
    }

    public class TipoOTData : BindableBase
    {
        public static readonly TiposOT[] TipoOT = new TiposOT[] {
            new TiposOT() { Id = 1, Name = "Interna"},
            new TiposOT() { Id = 2, Name = "Externa"},
            new TiposOT() { Id = 3, Name = "Mixta" },
        };
    }

    public class TiposOT
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    class GlobalClass
    {
        public static InterfazPrincipal ip { get; set; }
        public static int IdTabla { get; set; }
        public static NumberFormatInfo CultureInfo { get; set; }
        public static DataTable tblAlertasAPP { get; set; }
        public static PAQ04_Reportes.ControlAlertas FrmAlerta {get;set;}
        public static bool ExisteFormatoImpresion(string NombreMenu, ref int IdMenu)
        {
            bool Resultado = false;
            ErrorHandler Error = new ErrorHandler();

            try
            {
                IdMenu = B_Menu.Menu_GetByFormulario(NombreMenu);

                DataTable tblFormatosImpresion = B_FormatoImpresion.FormatoImpresion_GetItem(IdMenu);

                if (tblFormatosImpresion.Rows.Count > 0)
                {
                    Resultado = true;
                }

            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

            return Resultado;
        }

        public static DataTable GetFormatoImpresion(int IdMenu)
        {
            DataTable tblFormatosImpresion = new DataTable();
            ErrorHandler Error = new ErrorHandler();

            try
            {
                tblFormatosImpresion = B_FormatoImpresion.FormatoImpresion_GetItem(IdMenu);
            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

            return tblFormatosImpresion;
        }

        public static string GrabaFormatoImpresion(DataTable tblFile)
        {
            string Ruta = string.Empty;
            ErrorHandler Error = new ErrorHandler();

            try
            {
                byte[] objData;
                objData = (byte[])tblFile.Rows[0]["File"];
                string sourcePathSave = ConfigurationManager.AppSettings["Ruta"];
                string SourceFileSave = System.IO.Path.Combine(sourcePathSave, tblFile.Rows[0]["NombreArchivo"].ToString() + ".rpt");
                FileStream objFileStream = new FileStream(SourceFileSave, FileMode.Create, FileAccess.Write);
                objFileStream.Write(objData, 0, objData.Length);
                objFileStream.Close();
                Ruta = SourceFileSave;
            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

            return Ruta;
        }

        public static void GeneraImpresion(int IdMenu, int pParam)
        {
            Window XAML = GlobalClass.ip;
            string Ruta = string.Empty;
            ErrorHandler Error = new ErrorHandler();
            string Param = Convert.ToString(pParam);

            try
            {
                DataTable FormatosImpresion = GetFormatoImpresion(IdMenu);

                if (FormatosImpresion.Rows.Count == 0)
                {
                    GlobalClass.ip.Mensaje("No existen formatos de impresión disponibles", 3);
                }
                else if (FormatosImpresion.Rows.Count == 1)
                {
                    DataTable tblFile = B_FormatoImpresion.FormatoImpresion_GetFile(Convert.ToInt32(FormatosImpresion.Rows[0]["Id_FormatoImpresion"].ToString()));
                    Ruta = GrabaFormatoImpresion(tblFile);
                    ImprimirCR(Ruta, Param, XAML);

                }
                else if (FormatosImpresion.Rows.Count > 1)
                {
                    GeneraImpresion XAMLGeneraImpresion = new GeneraImpresion(FormatosImpresion, Param);
                    XAMLGeneraImpresion.Owner = XAML;
                    AplicarEfecto(XAML);
                    XAMLGeneraImpresion.ShowDialog();

                    bool? resultado = XAMLGeneraImpresion.DialogResult;


                    if (resultado != null)
                    {
                        if (resultado == false)
                        {
                            QuitarEfecto(XAML);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private static void AplicarEfecto(Window win)
        {
            var objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = 5;
            win.Effect = objBlur;
        }

        private static void QuitarEfecto(Window win)
        {
            win.Effect = null;
        }

        public static void ImprimirCR(string Ruta, string Param, Window XAML)
        {

            ErrorHandler Error = new ErrorHandler();
            try
            {
                ReportDocument cryRpt = new ReportDocument();
                ParameterField param1 = new ParameterField();
                ParameterDiscreteValue discreteValue1 = new ParameterDiscreteValue();
                ParameterFields paramFiels1 = new ParameterFields();
                B_Conexion b_Conexion = new B_Conexion();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["BDVentura"].ToString());

                
                param1 = new ParameterField();
                param1.ParameterFieldName = "Param";

                discreteValue1.Value = Param;
                param1.CurrentValues.Add(discreteValue1);

                paramFiels1.Add(param1);

                cryRpt.Load(Ruta);
                cryRpt.SetDatabaseLogon(builder.UserID, builder.Password, builder.DataSource, builder.InitialCatalog);

                CRViewer CRViewer = new CRViewer(cryRpt, paramFiels1);
                CRViewer.Owner = XAML;
                CRViewer.ShowDialog();
            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        public static string[] CargarParametrosLicencia(StreamReader Archivo)
        {
            string line, lineas = "";
            while ((line = Archivo.ReadLine()) != null)
            {
                lineas += Decrypt(line) + "|";
            }
            Archivo.Close();
            string[] parametros = lineas.Split('|');
            if (parametros.Length != 4) parametros = new string[] { "", "", "0", "" };
            return parametros;
        }

        public static string Decrypt(string encryptedText)
        {
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(Utilitarios.Utilitarios.PasswordHash, Encoding.ASCII.GetBytes(Utilitarios.Utilitarios.SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(Utilitarios.Utilitarios.VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            catch
            {
                return "";
            }
        }

        public static void Columna_AddIFnotExits(DataTable tbl, string clm)
        {
            if (!tbl.Columns.Contains(clm))
                tbl.Columns.Add(clm);
        }
        public static void Columna_AddIFnotExits(DataTable tbl, string clm,Type tipo)
        {
            if (!tbl.Columns.Contains(clm))
                tbl.Columns.Add(clm,tipo);
        }

        public static void ControlSubMenu(string frm, Grid grilla)
        {
            E_Rol objE_Rol=new E_Rol();            
            B_Rol objB_rol=new B_Rol();
            objE_Rol.IdRol=Utilitarios.Utilitarios.gintIdRol;
            DataTable tblTab = objB_rol.Rol_Menu_List(objE_Rol);
            foreach (DataRow fila in tblTab.Select("FlagActivo = 'False' AND IdTipo IN (4,5) AND Formulario = '"+frm+"'"))
            {
                object btn = grilla.FindName(fila["Objeto"].ToString());
                if (btn is FrameworkElement)
                    (btn as FrameworkElement).IsEnabled = false;
            }
        }

        public static void GeneraImpresionOT(int IdMenu, int pParam)
        {
            Window XAML = GlobalClass.ip;
            string Ruta = string.Empty;
            ErrorHandler Error = new ErrorHandler();
            string Param = Convert.ToString(pParam);

            try
            {
                DataTable FormatosImpresion = GetFormatoImpresion(IdMenu);

                if (FormatosImpresion.Rows.Count == 0)
                {
                    GlobalClass.ip.Mensaje("No existen formatos de impresión disponibles", 3);
                }
                else if (FormatosImpresion.Rows.Count == 1)
                {
                    DataTable tblFile = B_FormatoImpresion.FormatoImpresion_GetFile(Convert.ToInt32(FormatosImpresion.Rows[0]["Id_FormatoImpresion"].ToString()));
                    Ruta = GrabaFormatoImpresion(tblFile);
                    ImprimirCROT(Ruta, Param, XAML);

                }
            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public static void ImprimirCROT(string Ruta, string Param, Window XAML)
        {

            ErrorHandler Error = new ErrorHandler();
            try
            {
                ReportDocument cryRpt = new ReportDocument();
                ParameterField param1 = new ParameterField();
                ParameterDiscreteValue discreteValue1 = new ParameterDiscreteValue();
                ParameterFields paramFiels1 = new ParameterFields();
                B_Conexion b_Conexion = new B_Conexion();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["BDVentura"].ToString());

                param1 = new ParameterField();
                param1.ParameterFieldName = "Param";

                discreteValue1.Value = Param;
                param1.CurrentValues.Add(discreteValue1);
                paramFiels1.Add(param1);
                param1.CurrentValues.Add(discreteValue1);
                paramFiels1.Add(param1);
                param1.CurrentValues.Add(discreteValue1);
                paramFiels1.Add(param1);
                param1.CurrentValues.Add(discreteValue1);
                paramFiels1.Add(param1);

                cryRpt.Load(Ruta);
                cryRpt.SetDatabaseLogon(builder.UserID, builder.Password, builder.DataSource, builder.InitialCatalog);

                CRViewer2 CRViewer = new CRViewer2(cryRpt, paramFiels1);
                CRViewer.Owner = XAML;
                CRViewer.ShowDialog();
            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
    }

    public class GifImage : Image
    {
        private bool _isInitialized;
        private GifBitmapDecoder _gifDecoder;
        private Int32Animation _animation;

        public int FrameIndex
        {
            get { return (int)GetValue(FrameIndexProperty); }
            set { SetValue(FrameIndexProperty, value); }
        }

        private void Initialize()
        {
            _gifDecoder = new GifBitmapDecoder(new Uri("pack://application:,,," + this.GifSource), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            _animation = new Int32Animation(0, _gifDecoder.Frames.Count - 1, new Duration(new TimeSpan(0, 0, 0, _gifDecoder.Frames.Count / 10, (int)((_gifDecoder.Frames.Count / 10.0 - _gifDecoder.Frames.Count / 10) * 1000))));
            _animation.RepeatBehavior = RepeatBehavior.Forever;
            this.Source = _gifDecoder.Frames[0];

            _isInitialized = true;
        }

        static GifImage()
        {
            VisibilityProperty.OverrideMetadata(typeof(GifImage),
                new FrameworkPropertyMetadata(VisibilityPropertyChanged));
        }

        private static void VisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Visibility)e.NewValue == Visibility.Visible)
            {
                ((GifImage)sender).StartAnimation();
            }
            else
            {
                ((GifImage)sender).StopAnimation();
            }
        }

        public static readonly DependencyProperty FrameIndexProperty =
            DependencyProperty.Register("FrameIndex", typeof(int), typeof(GifImage), new UIPropertyMetadata(0, new PropertyChangedCallback(ChangingFrameIndex)));

        static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
        {
            var gifImage = obj as GifImage;
            gifImage.Source = gifImage._gifDecoder.Frames[(int)ev.NewValue];
        }

        /// <summary>
        /// Defines whether the animation starts on it's own
        /// </summary>
        public bool AutoStart
        {
            get { return (bool)GetValue(AutoStartProperty); }
            set { SetValue(AutoStartProperty, value); }
        }

        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register("AutoStart", typeof(bool), typeof(GifImage), new UIPropertyMetadata(false, AutoStartPropertyChanged));

        private static void AutoStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                (sender as GifImage).StartAnimation();
        }

        public string GifSource
        {
            get { return (string)GetValue(GifSourceProperty); }
            set { SetValue(GifSourceProperty, value); }
        }

        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.Register("GifSource", typeof(string), typeof(GifImage), new UIPropertyMetadata(string.Empty, GifSourcePropertyChanged));

        private static void GifSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as GifImage).Initialize();
        }

        /// <summary>
        /// Starts the animation
        /// </summary>
        public void StartAnimation()
        {
            if (!_isInitialized)
                this.Initialize();

            BeginAnimation(FrameIndexProperty, _animation);
        }

        /// <summary>
        /// Stops the animation
        /// </summary>
        public void StopAnimation()
        {
            BeginAnimation(FrameIndexProperty, null);
        }



    }
}
