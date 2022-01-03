using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace AplicacionSistemaVentura.PAQ04_Reportes
{
    public partial class SubReporteUCRecorrido : DevExpress.XtraReports.UI.XtraReport
    {
        public SubReporteUCRecorrido()
        {
            InitializeComponent();
        }

        private void SubReporteUCRecorrido_DataSourceDemanded(object sender, EventArgs e)
        {
            sqlDataSource1.Queries[0].Parameters[0].Value = this.Parameters[0].Value;
            sqlDataSource1.Fill();
            this.DataSource = sqlDataSource1;
            this.RequestParameters = false;
        }        
    }
}
