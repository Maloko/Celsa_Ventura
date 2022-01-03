using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace AplicacionSistemaVentura.PAQ04_Reportes
{
    public partial class SubReporteOTProcesadas : DevExpress.XtraReports.UI.XtraReport
    {
        public SubReporteOTProcesadas()
        {
            InitializeComponent();
        }

        private void SubReporteOTProcesadas_DataSourceDemanded(object sender, EventArgs e)
        {
            sqlDataSource1.Queries[0].Parameters[0].Value = this.Parameters[0].Value;
            sqlDataSource1.Queries[0].Parameters[1].Value = this.Parameters[1].Value;
            sqlDataSource1.Queries[0].Parameters[2].Value = this.Parameters[2].Value;
            sqlDataSource1.Fill();
            this.DataSource = sqlDataSource1;
            this.RequestParameters = false;
        }

    }
}
