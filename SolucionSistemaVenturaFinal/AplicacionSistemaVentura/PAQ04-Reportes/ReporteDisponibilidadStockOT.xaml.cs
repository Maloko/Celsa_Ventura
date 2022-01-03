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
    /// Interaction logic for ReporteListadoHojaInspeccion.xaml
    /// </summary>
    public partial class ReporteDisponibilidadStockOT : UserControl
    {

        InterfazPrincipal ip;
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();

        public ReporteDisponibilidadStockOT()
        {
            InitializeComponent();
        }

        public ReporteDisponibilidadStockOT(InterfazPrincipal p)
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
            String strConnString = ConfigurationManager.ConnectionStrings["BDVentura"].ConnectionString;
            int ResultadoRetorno = 0;

            using (SqlConnection con = new SqlConnection(strConnString))
            {

                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader = null;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "VS_SP_ReporteDisponibilidadStockOT";
                //Mensaje("", 0);
                gridControl1.ItemsSource = null;

                try
                {
                    if (String.IsNullOrEmpty(dateEdit1.Text))
                    {
                        GlobalClass.ip.Mensaje("Indicar una Fecha Inicial para la consulta", 3);
                    }

                    if (String.IsNullOrEmpty(dateEdit2.Text))
                    {
                        GlobalClass.ip.Mensaje("Indicar una Fecha Final para la consulta", 3);
                    }

                    if ((String.IsNullOrEmpty(dateEdit1.Text) == false) && (String.IsNullOrEmpty(dateEdit2.Text) == false))
                    {
                        cmd.Parameters.Add(new SqlParameter("@pFechaInicial", SqlDbType.VarChar));
                        cmd.Parameters["@pFechaInicial"].Value = dateEdit1.DateTime.ToShortDateString();
                        cmd.Parameters.Add(new SqlParameter("@pFechaFinal", SqlDbType.VarChar));
                        cmd.Parameters["@pFechaFinal"].Value = dateEdit2.DateTime.ToShortDateString();
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable("newTable");
                            dt.Columns.Add("Fecha", typeof(DateTime));
                            dt.Columns.Add("Hora", typeof(TimeSpan));
                            dt.Columns.Add("UC", typeof(String));
                            dt.Columns.Add("OT", typeof(String));
                            dt.Columns.Add("TipoArt", typeof(String));
                            dt.Columns.Add("Articulo", typeof(String));
                            dt.Columns.Add("CantidadSolicitada", typeof(Int32));
                            dt.Columns.Add("CantidadStock", typeof(Int32));

                            DataTable dta = new DataTable("newTable");
                            dta.Columns.Add("Información", typeof(String));

                            while (reader.Read())
                            {
                                ResultadoRetorno = reader.GetInt32(0);
                                if (ResultadoRetorno == 0)
                                {
                                    dt.Rows.Add(reader.GetDateTime(4), reader.GetTimeSpan(5), reader.GetString(6), reader.GetString(7), reader.GetString(8), reader.GetString(10), reader.GetInt32(11), reader.GetInt32(12));
                                }
                            }

                            if (ResultadoRetorno == -1)
                            {
                                dta.Rows.Add("No existen resultados para la consulta");
                                gridControl1.ItemsSource = dta;
                            }
                            else
                            {
                                gridControl1.ItemsSource = dt;
                                gridControl1.Columns["UC"].Header = "Unidad de Control";
                                gridControl1.Columns["OT"].Header = "O/T";
                                gridControl1.Columns["TipoArt"].Header = "Tipo Art.";
                                gridControl1.Columns["CantidadSolicitada"].Header = "Cant. Solicitada";
                                gridControl1.Columns["CantidadStock"].Header = "Cant. Stock";

                                gridControl1.GroupBy("Fecha");
                                gridControl1.GroupBy("Hora");
                                gridControl1.GroupBy("UC");
                                gridControl1.GroupBy("OT");
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
            saveFileDialog1.FileName = "Listado HR " + Fecha;
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
            if (String.IsNullOrEmpty(dateEdit1.Text))
            {
                GlobalClass.ip.Mensaje("Indicar una Fecha Inicial para la consulta", 3);
            }

            if (String.IsNullOrEmpty(dateEdit2.Text))
            {
                GlobalClass.ip.Mensaje("Indicar una Fecha Final para la consulta", 3);
            }

            if ((String.IsNullOrEmpty(dateEdit1.Text) == false) && (String.IsNullOrEmpty(dateEdit2.Text) == false))
            {

                Reporte_DisponibilidadStockOT DisponibilidadStockOT = new Reporte_DisponibilidadStockOT();

                DisponibilidadStockOT.Parameters[0].Value = dateEdit1.DateTime.ToShortDateString();
                DisponibilidadStockOT.Parameters[1].Value = dateEdit2.DateTime.ToShortDateString();
                DisponibilidadStockOT.Parameters[0].Visible = false;
                DisponibilidadStockOT.Parameters[1].Visible = false;
                DisponibilidadStockOT.RequestParameters = false;
                ReportPrintTool printTool = new ReportPrintTool(DisponibilidadStockOT);
                printTool.ShowPreviewDialog();
            }
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
