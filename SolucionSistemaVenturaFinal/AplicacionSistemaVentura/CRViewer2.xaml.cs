using DevExpress.Xpf.Core;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace AplicacionSistemaVentura
{
    /// <summary>
    /// Interaction logic for CRViewer.xaml
    /// </summary>
    public partial class CRViewer2 : DXWindow
    {
        ReportDocument CRPrint = new ReportDocument();
        ParameterFields PFields = new ParameterFields();

        public CRViewer2(ReportDocument CR, ParameterFields PF)
        {
            CRPrint = CR;
            PFields = PF;
            InitializeComponent();
            DXWindow_Loaded();
        }

        private void DXWindow_Loaded()
        {

            for (int i = 0; i < PFields.Count; i++)
            {
                CRPrint.SetParameterValue(i, PFields[i].CurrentValues);
            }

            crystalReportsViewer1.ViewerCore.ReportSource = CRPrint;
        }
    }
}
