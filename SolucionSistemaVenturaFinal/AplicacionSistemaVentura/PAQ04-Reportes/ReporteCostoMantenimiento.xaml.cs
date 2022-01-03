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
    public partial class ReporteCostoMantenimiento : UserControl
    {

        InterfazPrincipal ip;
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();

        public ReporteCostoMantenimiento()
        {
            InitializeComponent();
        }

        public ReporteCostoMantenimiento(InterfazPrincipal p)
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
                cmd.CommandText = "VS_SP_ReporteCostoMantenimiento";
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
                            dt.Columns.Add("Familia", typeof(String));
                            dt.Columns.Add("SubFamilia", typeof(String));
                            dt.Columns.Add("U.C.", typeof(String));
                            dt.Columns.Add("Preventivo", typeof(Decimal));
                            dt.Columns.Add("Correctivo", typeof(Decimal));
                            dt.Columns.Add("Manual", typeof(Decimal));                            

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
                                    dt.Rows.Add(reader.GetString(3), reader.GetString(5), reader.GetString(7), reader.GetDecimal(8), reader.GetDecimal(9), reader.GetDecimal(10));
                                    string FamiliaD = reader.GetString(4);
                                    string SFamiliaD = reader.GetString(6);
                                    dt.Columns[0].ColumnName = FamiliaD;
                                    dt.Columns[1].ColumnName = SFamiliaD;
                                    gridControl1.ItemsSource = dt;                                    
                                    gridControl1.GroupBy(FamiliaD);
                                    gridControl1.GroupBy(SFamiliaD);
                                    gridControl1.ExpandAllGroups();
                                    view.ShowGroupFooters = true;                                  
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
            saveFileDialog1.FileName = "CostoMantenimiento  " + Fecha;
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
                Reporte_CostoMantenimiento CostoMantenimiento = new Reporte_CostoMantenimiento();

                CostoMantenimiento.Parameters[0].Value = dateEdit2.DateTime.ToShortDateString();
                CostoMantenimiento.Parameters[1].Value = dateEdit1.DateTime.ToShortDateString();                
                CostoMantenimiento.Parameters[0].Visible = false;
                CostoMantenimiento.Parameters[1].Visible = false;
                CostoMantenimiento.RequestParameters = false;
                ReportPrintTool printTool = new ReportPrintTool(CostoMantenimiento);
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
