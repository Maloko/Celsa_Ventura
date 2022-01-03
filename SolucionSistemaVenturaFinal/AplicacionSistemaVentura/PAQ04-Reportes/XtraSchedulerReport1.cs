using System;

namespace AplicacionSistemaVentura.PAQ04_Reportes
{
    public partial class XtraSchedulerReport1 : DevExpress.XtraScheduler.Reporting.XtraSchedulerReport
    {
        public String FechaInicio;
        public String FechaFin;

        public XtraSchedulerReport1()
        {
            InitializeComponent();

        }


     

        private void XtraSchedulerReport1_DataSourceDemanded(object sender, EventArgs e)
        {
            xrLabel41.Text = Convert.ToDateTime(SchedulerAdapter.TimeInterval.Start).Date.ToShortDateString();
            xrLabel43.Text = Convert.ToDateTime(SchedulerAdapter.TimeInterval.End).Date.ToShortDateString();
        }

    
    }
}
