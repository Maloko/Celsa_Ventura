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

namespace AplicacionSistemaVentura.PAQ04_Reportes
{
    /// <summary>
    /// Interaction logic for ReporteMonitoreoLineaOT.xaml
    /// </summary>
    public partial class ReporteMonitoreoLineaOT : UserControl
    {

        InterfazPrincipal ip;
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();

        public ReporteMonitoreoLineaOT()
        {
            InitializeComponent();
        }

        public ReporteMonitoreoLineaOT(InterfazPrincipal p)
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
            String strConnString = ConfigurationManager.ConnectionStrings["BDVentura"].ConnectionString;

            using (SqlConnection con = new SqlConnection(strConnString))
            {

                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader = null;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "VS_SP_ReporteMonitoreoLineaOT";
                //Mensaje("", 0);
                gridControl1.ItemsSource = null;

                try
                {
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {

                        DataTable dt = new DataTable("newTable");
                        dt.Columns.Add("FechaLiberacion", typeof(String));
                        dt.Columns.Add("UC", typeof(String));
                        dt.Columns.Add("OT", typeof(String));
                        dt.Columns.Add("Estado", typeof(String));
                        dt.Columns.Add("Horas Estimadas", typeof(Decimal));
                        dt.Columns.Add("Horas Reales", typeof(Decimal));
                        dt.Columns.Add("Horas Canceladas", typeof(Decimal));                        

                        while (reader.Read())
                        {
                            int ResultadoRetorno = reader.GetInt32(0);

                            if (ResultadoRetorno == -1)
                            {
                                DataTable dta = new DataTable("newTable");
                                dta.Columns.Add("Información", typeof(String));
                                dta.Rows.Add("No existen resultados para la consulta");
                                gridControl1.ItemsSource = dta;
                                break;
                            }
                            else
                            {
                                dt.Rows.Add(reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetDecimal(6), reader.GetDecimal(7), reader.GetDecimal(8));
                                gridControl1.ItemsSource = dt;                                
                                gridControl1.Columns["FechaLiberacion"].Header = "Fecha de Liberación";
                                gridControl1.Columns["UC"].Header = "Unidad de Control";
                                gridControl1.Columns["OT"].Header = "# O/T";                                                                
                                gridControl1.ExpandAllGroups();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
       

        private void PreviewGrid(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            PrintableControlLink link = new PrintableControlLink(gridControl1.View as IPrintableControl);
            link.Landscape = true;
            string Fecha = Regex.Replace(System.DateTime.Now.ToShortDateString(), @"[^\w\.@-]", "");
            saveFileDialog1.Filter = "Archivo PDF|*.pdf|Archivo Excel|*.xls";
            saveFileDialog1.Title = "Guardar como";
            saveFileDialog1.FileName = "MonitoreoOT " + Fecha;
            saveFileDialog1.ShowDialog();

            switch (saveFileDialog1.FilterIndex)
            {
                case 1:
                    link.ExportToPdf(saveFileDialog1.FileName);
                    break;
                case 2:
                    link.ExportToXls(saveFileDialog1.FileName);
                    break;
            }            
        }
       

        private void button2_Click(object sender, RoutedEventArgs e)
        {                                
                Reporte_MonitoreoLineaOT MonitoreoLineaOT = new Reporte_MonitoreoLineaOT();

                MonitoreoLineaOT.RequestParameters = false;            
                ReportPrintTool printTool = new ReportPrintTool(MonitoreoLineaOT);
                printTool.ShowPreviewDialog();            
        }

        //private void Mensaje(string strMensaje, int intTipo)
        //{
        //    try
        //    {
        //        ip.lblError.Text = strMensaje;
        //        if (intTipo == 0) //Sin mensaje
        //        {
        //            ip.lblError.Background = System.Windows.Media.Brushes.LightGray;
        //            ip.lblError.Foreground = System.Windows.Media.Brushes.Black;
        //        }
        //        else if (intTipo == 1) //Ejecución satisfactoria
        //        {
        //            ip.lblError.Background = System.Windows.Media.Brushes.LightGreen;
        //            ip.lblError.Foreground = System.Windows.Media.Brushes.Black;
        //        }
        //        else if (intTipo == 2) //Advertencia de validación
        //        {
        //            ip.lblError.Background = System.Windows.Media.Brushes.LightBlue;
        //            ip.lblError.Foreground = System.Windows.Media.Brushes.Black;
        //        }
        //        else if (intTipo == 3) //Mensaje de error
        //        {
        //            ip.lblError.Background = System.Windows.Media.Brushes.IndianRed;
        //            ip.lblError.Foreground = System.Windows.Media.Brushes.White;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
        //    }
        //}
                          
    }
}
