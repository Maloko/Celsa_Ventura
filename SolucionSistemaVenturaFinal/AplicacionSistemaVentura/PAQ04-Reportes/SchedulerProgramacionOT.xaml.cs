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
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Collections;
using DevExpress.Xpf.Printing;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Reporting;
using DevExpress.Xpf.Scheduler.Reporting;
using DevExpress.Xpf.Scheduler;


namespace AplicacionSistemaVentura.PAQ04_Reportes
{
    /// <summary>
    /// Interaction logic for SchedulerProgramacionOT.xaml
    /// </summary>
    public partial class SchedulerProgramacionOT : UserControl
    {

        InterfazPrincipal ip;
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();
        SchedulerPrintingSettings printingSettings = new SchedulerPrintingSettings();


        public SchedulerProgramacionOT()
        {
            InitializeComponent();                        
        }
        
        public SchedulerProgramacionOT(InterfazPrincipal p) : this()
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
        

        private void dateNavigator1_SelectedDatesChanged(object sender, EventArgs e)
        {
            String strConnString = ConfigurationManager.ConnectionStrings["BDVentura"].ConnectionString;
            DateTime StartDate = dateNavigator1.SelectedDates[0].Date;
            DateTime EndDate = dateNavigator1.SelectedDates[4].Date;

            DataTable dtAppointments = new DataTable();
            dtAppointments.Columns.AddRange(
            new DataColumn[] { new DataColumn("UC", typeof(string)), new DataColumn("FechaInicialEstimada", typeof(DateTime)), new DataColumn("FechaFinalEstimada", typeof(DateTime)), new DataColumn("TipoOT", typeof(Int32)) });

            using (SqlConnection con = new SqlConnection(strConnString))
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader = null;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "VS_SP_ReporteProgramacionOTSemanal";
                //Mensaje("", 0);

                try
                {
                    cmd.Parameters.Add(new SqlParameter("@pFechaInicial", SqlDbType.VarChar));
                    cmd.Parameters["@pFechaInicial"].Value = StartDate;
                    cmd.Parameters.Add(new SqlParameter("@pFechaFinal", SqlDbType.VarChar));
                    cmd.Parameters["@pFechaFinal"].Value = EndDate;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int ResultadoRetorno = reader.GetInt32(0);

                            if (ResultadoRetorno == -1)
                            {
                                DataTable dt = new DataTable("newTable");
                                dt.Columns.Add("Información", typeof(String));
                                dt.Rows.Add("No existen resultados para la consulta");
                                break;
                            }
                            else
                            {
                                dtAppointments.Rows.Add(new object[] { reader.GetString(6), reader.GetDateTime(14), reader.GetDateTime(15), reader.GetInt32(16) });
                                
                                schedulerControl1.Storage.AppointmentStorage.DataSource = dtAppointments;   
                                DevExpress.Xpf.Scheduler.AppointmentLabel LabelPreventivo = new DevExpress.Xpf.Scheduler.AppointmentLabel(Colors.LightGreen, "1");
                                DevExpress.Xpf.Scheduler.AppointmentLabel LabelCorrectivo = new DevExpress.Xpf.Scheduler.AppointmentLabel(Colors.PowderBlue, "2");
                                DevExpress.Xpf.Scheduler.AppointmentLabel LabelManual = new DevExpress.Xpf.Scheduler.AppointmentLabel(Colors.Violet, "3");

                                schedulerControl1.Storage.AppointmentStorage.Labels.Add(LabelPreventivo);
                                schedulerControl1.Storage.AppointmentStorage.Labels.Add(LabelCorrectivo);
                                schedulerControl1.Storage.AppointmentStorage.Labels.Add(LabelManual);                                
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


        private void button1_Click(object sender, RoutedEventArgs e)
        {            
            
            DateTime DateStart = dateNavigator1.SelectedDates[0].Date;
            DateTime DateEnd = dateNavigator1.SelectedDates[6].Date.AddDays(1);

            printAdapter.TimeInterval = new TimeInterval(dateNavigator1.SelectedDates[0].Date, dateNavigator1.SelectedDates[6].Date.AddDays(1));            

            XtraSchedulerReport1 XSR1 = new XtraSchedulerReport1();
            XSR1.SchedulerAdapter = printAdapter.SchedulerAdapter;

            XSR1.FechaInicio = DateStart.ToString();
            XSR1.FechaFin = DateEnd.ToString();
            PrintHelper.ShowPrintPreviewDialog(ip, XSR1);       
        }

      
                          
    }
}
