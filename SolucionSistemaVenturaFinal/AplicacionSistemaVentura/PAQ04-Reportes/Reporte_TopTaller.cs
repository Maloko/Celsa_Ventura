using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Globalization;

namespace AplicacionSistemaVentura.PAQ04_Reportes
{
    public partial class Reporte_TopTaller : DevExpress.XtraReports.UI.XtraReport
    {
        public Reporte_TopTaller()
        {
            InitializeComponent();          
        }                

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((SubReporteOTProcesadas)((XRSubreport)sender).ReportSource).Parameters[0].Value = this.Parameters[0].Value;
            ((SubReporteOTProcesadas)((XRSubreport)sender).ReportSource).Parameters[1].Value = this.Parameters[1].Value;
            ((SubReporteOTProcesadas)((XRSubreport)sender).ReportSource).Parameters[2].Value = "-";
            
        }

        private void xrSubreport2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((SubReporteOTProcesadas)((XRSubreport)sender).ReportSource).Parameters[0].Value = this.Parameters[0].Value;
            ((SubReporteOTProcesadas)((XRSubreport)sender).ReportSource).Parameters[1].Value = this.Parameters[1].Value;
            ((SubReporteOTProcesadas)((XRSubreport)sender).ReportSource).Parameters[2].Value = "+";
        }

        private void xrSubreport4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((SubReporteActividades)((XRSubreport)sender).ReportSource).Parameters[0].Value = this.Parameters[0].Value;
            ((SubReporteActividades)((XRSubreport)sender).ReportSource).Parameters[1].Value = this.Parameters[1].Value;
            ((SubReporteActividades)((XRSubreport)sender).ReportSource).Parameters[2].Value = "+";            
        }

        private void xrSubreport3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((SubReporteActividades)((XRSubreport)sender).ReportSource).Parameters[0].Value = this.Parameters[0].Value;
            ((SubReporteActividades)((XRSubreport)sender).ReportSource).Parameters[1].Value = this.Parameters[1].Value;
            ((SubReporteActividades)((XRSubreport)sender).ReportSource).Parameters[2].Value = "-";     
        }

        private void xrSubreport5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((SubReporteUCCostoMtto)((XRSubreport)sender).ReportSource).Parameters[0].Value = this.Parameters[0].Value;
            ((SubReporteUCCostoMtto)((XRSubreport)sender).ReportSource).Parameters[1].Value = this.Parameters[1].Value;
            ((SubReporteUCCostoMtto)((XRSubreport)sender).ReportSource).Parameters[2].Value = "+";
        }

        private void xrSubreport6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((SubReporteUCCostoMtto)((XRSubreport)sender).ReportSource).Parameters[0].Value = this.Parameters[0].Value;
            ((SubReporteUCCostoMtto)((XRSubreport)sender).ReportSource).Parameters[1].Value = this.Parameters[1].Value;
            ((SubReporteUCCostoMtto)((XRSubreport)sender).ReportSource).Parameters[2].Value = "-";
        }

        private void xrSubreport8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {            
            ((SubReporteUCRecorrido)((XRSubreport)sender).ReportSource).Parameters[0].Value = "+";
        }

        private void xrSubreport7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((SubReporteUCRecorrido)((XRSubreport)sender).ReportSource).Parameters[0].Value = "-";
        }

      
    }
}
