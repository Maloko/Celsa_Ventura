using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Globalization;

namespace AplicacionSistemaVentura.PAQ04_Reportes
{
    public partial class Reporte_MonitoreoLineaOT : DevExpress.XtraReports.UI.XtraReport
    {
        public Reporte_MonitoreoLineaOT()
        {
            InitializeComponent();
        }

        private void Reporte_MonitoreoLineaOT_DataSourceDemanded(object sender, EventArgs e)
        {
            sqlDataSource1.Fill();
            this.DataSource = sqlDataSource1;
        }


      

    }
}
