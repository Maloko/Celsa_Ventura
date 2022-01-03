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
using DevExpress.Xpf.Editors;
using System.Data;
using Entities;
using Business;
using System.Configuration;
using System.Data.SqlClient;
using DevExpress.XtraPivotGrid;
using DevExpress.PivotGrid;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Core;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using DevExpress.XtraReports.UI;
using System.Collections;

namespace AplicacionSistemaVentura.PAQ04_Reportes
{
    /// <summary>
    /// Interaction logic for ReporteTopTaller.xaml
    /// </summary>
    public partial class Indicadores : UserControl
    {

        InterfazPrincipal ip;
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();

        public Indicadores()
        {
            InitializeComponent();
        }

        public Indicadores(InterfazPrincipal p)
            : this()
        {
            ip = p;
        }

        void OnFocus(object sender, RoutedEventArgs e)
        {
            (sender as Control).Background = System.Windows.Media.Brushes.Gold;
        }

        private void OutFocus(object sender, RoutedEventArgs e)
        {
            (sender as Control).Background = System.Windows.Media.Brushes.White;
        }


        E_TablaMaestra objTablaMaestra = new E_TablaMaestra();


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(dpAño.Text))
            {
                Mensaje("Indicar una Fecha Inicial para la consulta", 3);
            }

            if (String.IsNullOrEmpty(dpAño.Text))
            {
                Mensaje("Indicar una Fecha Final para la consulta", 3);
            }

            if ((String.IsNullOrEmpty(dpAño.Text) == false) && (String.IsNullOrEmpty(dpAño.Text) == false))
            {
                Reporte_Indicadores Indicadores = new Reporte_Indicadores();

                Indicadores.Parameters[0].Value = FlagTiempo;
                Indicadores.Parameters[1].Value = cboSemestre.EditValue;
                Indicadores.Parameters[2].Value = dpAño.DateTime.Year;
                if (ismensual)
                {
                    Indicadores.Parameters[3].Value = cboSemestre.EditValue;
                }
                else
                {
                    Indicadores.Parameters[3].Value = 0;
                }
                

                Indicadores.Parameters[0].Visible = false;
                Indicadores.Parameters[1].Visible = false;
                Indicadores.Parameters[2].Visible = false;
                Indicadores.Parameters[3].Visible = false;
                Indicadores.RequestParameters = false;
                ReportPrintTool printTool = new ReportPrintTool(Indicadores);
                printTool.ShowPreviewDialog();
            }
        }

        private void Mensaje(string strMensaje, int intTipo)
        {
            try
            {
                ip.lblError.Text = strMensaje;
                if (intTipo == 0) //Sin mensaje
                {
                    ip.lblError.Background = System.Windows.Media.Brushes.LightGray;
                    ip.lblError.Foreground = System.Windows.Media.Brushes.Black;
                }
                else if (intTipo == 1) //Ejecución satisfactoria
                {
                    ip.lblError.Background = System.Windows.Media.Brushes.LightGreen;
                    ip.lblError.Foreground = System.Windows.Media.Brushes.Black;
                }
                else if (intTipo == 2) //Advertencia de validación
                {
                    ip.lblError.Background = System.Windows.Media.Brushes.LightBlue;
                    ip.lblError.Foreground = System.Windows.Media.Brushes.Black;
                }
                else if (intTipo == 3) //Mensaje de error
                {
                    ip.lblError.Background = System.Windows.Media.Brushes.IndianRed;
                    ip.lblError.Foreground = System.Windows.Media.Brushes.White;
                }
            }
            catch (Exception ex)
            {
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        class ClsCombo
        {
            public string id { get; set; }
            public string text { get; set; }
        }
        class ClsMeses
        {
            public int id { get; set; }
            public string text { get; set; }
        }

        string FlagTiempo;
        Boolean ismensual = false;
        private IList<ClsCombo> ComboPerfilComp(int tipo)
        {
            List<ClsCombo> ListCombo = new List<ClsCombo>();
            if (tipo == 1)
            {
                ListCombo.Add(new ClsCombo() { id = "T1", text = "1er. Trimestre" });
                ListCombo.Add(new ClsCombo() { id = "T2", text = "2do. Trimestre" });
                ListCombo.Add(new ClsCombo() { id = "T3", text = "3er. Trimestre" });
                ListCombo.Add(new ClsCombo() { id = "T4", text = "4t0. Trimestre" });
            }
            else
            {
                ListCombo.Add(new ClsCombo() { id = "S1", text = "1er. Semestre" });
                ListCombo.Add(new ClsCombo() { id = "S2", text = "2do. Semestre" });

            }
            return ListCombo;
        }

        private IList<ClsMeses> ComboMeses()
        {
            List<ClsMeses> ListCombo = new List<ClsMeses>();
            string[] names = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;
            for (int i = 0; i < names.Length - 1; i++)
            {
                ListCombo.Add(new ClsMeses() { id = i + 1, text = names[i].ToString() });
            }
            return ListCombo;
        }
        
        private void rbnAnual_Checked(object sender, RoutedEventArgs e)
        {
            FlagTiempo = "A";
            lblsemestre.Visibility = Visibility.Hidden;
            cboSemestre.Visibility = Visibility.Hidden;
        }

        private void rbnSemestral_Checked(object sender, RoutedEventArgs e)
        {
            FlagTiempo = "S";
            cboSemestre.ItemsSource = ComboPerfilComp(0);
            cboSemestre.DisplayMember = "text";
            cboSemestre.ValueMember = "id";
            lblsemestre.Visibility = Visibility.Visible;
            cboSemestre.Visibility = Visibility.Visible;
        }

        private void rbnTrimestral_Checked(object sender, RoutedEventArgs e)
        {
            FlagTiempo = "T";
            cboSemestre.ItemsSource = ComboPerfilComp(1);
            cboSemestre.DisplayMember = "text";
            cboSemestre.ValueMember = "id";
            lblsemestre.Visibility = Visibility.Visible;
            cboSemestre.Visibility = Visibility.Visible;
        }

        private void rbnMensual_Checked(object sender, RoutedEventArgs e)
        {
            ismensual = true;
            FlagTiempo = "M";
            cboSemestre.ItemsSource = ComboMeses();
            cboSemestre.DisplayMember = "text";
            cboSemestre.ValueMember = "id";
            lblsemestre.Visibility = Visibility.Visible;
            cboSemestre.Visibility = Visibility.Visible;
        }
    }
}
