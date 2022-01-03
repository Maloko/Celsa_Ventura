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
    /// Interaction logic for ReporteProgramacionOT.xaml
    /// </summary>
    public partial class ReporteTrabajoMecanico : UserControl
    {

        InterfazPrincipal ip;
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();

        public ReporteTrabajoMecanico()
        {
            InitializeComponent();
        }

        public ReporteTrabajoMecanico(InterfazPrincipal p)
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

            using (SqlConnection con = new SqlConnection(strConnString))         
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader = null;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "VS_SP_ReporteTrabajoMecanico";
                //Mensaje("", 0);
                gridControl1.ItemsSource = null;

                try
                {
                    if (String.IsNullOrEmpty(dateEdit2.Text))
                    {
                        GlobalClass.ip.Mensaje("Indicar una Fecha Inicial para la consulta", 3);
                    }

                    if (String.IsNullOrEmpty(dateEdit1.Text))
                    {
                        GlobalClass.ip.Mensaje("Indicar una Fecha Final para la consulta", 3);
                    }

                    if ((String.IsNullOrEmpty(dateEdit2.Text) == false) && (String.IsNullOrEmpty(dateEdit1.Text) == false))
                    {
                        cmd.Parameters.Add(new SqlParameter("@pFechaInicial", SqlDbType.VarChar));
                        cmd.Parameters["@pFechaInicial"].Value = dateEdit2.DateTime.ToShortDateString(); 
                        cmd.Parameters.Add(new SqlParameter("@pFechaFinal", SqlDbType.VarChar));
                        cmd.Parameters["@pFechaFinal"].Value = dateEdit1.DateTime.ToShortDateString();                        
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable("newTable");
                            dt.Columns.Add("Responsable", typeof(String));
                            dt.Columns.Add("Fecha", typeof(DateTime));
                            dt.Columns.Add("Hora Inicio", typeof(TimeSpan));
                            dt.Columns.Add("Hora Fin", typeof(TimeSpan));
                            dt.Columns.Add("O/T", typeof(String));
                            dt.Columns.Add("Actividad", typeof(String));
                            dt.Columns.Add("Tarea", typeof(String));
                            dt.Columns.Add("Total Horas", typeof(TimeSpan));                            

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
                                    dt.Rows.Add(reader.GetString(4), reader.GetDateTime(5), reader.GetTimeSpan(6), reader.GetTimeSpan(7), reader.GetString(8), reader.GetString(9), reader.GetString(10),reader.GetTimeSpan(11));
                                    gridControl1.ItemsSource = dt;                                    
                                    gridControl1.GroupBy("Responsable");                                                                                                           
                                    gridControl1.ExpandAllGroups();                                                                        
                                }      
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
                    cmd.Dispose();
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
            saveFileDialog1.FileName = "TrabajoMecanico  " + Fecha;
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
            if (String.IsNullOrEmpty(dateEdit2.Text))
            {
                GlobalClass.ip.Mensaje("Indicar una Fecha Inicial para la consulta", 3);
            }

            if (String.IsNullOrEmpty(dateEdit1.Text))
            {
                GlobalClass.ip.Mensaje("Indicar una Fecha Final para la consulta", 3);
            }

            if ((String.IsNullOrEmpty(dateEdit2.Text) == false) && (String.IsNullOrEmpty(dateEdit1.Text) == false))
            {
                Reporte_TrabajoMecanico TrabajoMecanico = new Reporte_TrabajoMecanico();

                TrabajoMecanico.Parameters[0].Value = dateEdit2.DateTime.ToShortDateString(); 
                TrabajoMecanico.Parameters[1].Value = dateEdit1.DateTime.ToShortDateString();
                TrabajoMecanico.Parameters[0].Visible = false;
                TrabajoMecanico.Parameters[1].Visible = false;
                TrabajoMecanico.RequestParameters = false;
                ReportPrintTool printTool = new ReportPrintTool(TrabajoMecanico);
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
