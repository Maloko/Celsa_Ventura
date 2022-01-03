using DevExpress.Xpf.Core;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace AplicacionSistemaVentura
{
    /// <summary>
    /// Interaction logic for CRViewer.xaml
    /// </summary>
    public partial class CRViewer : DXWindow
    {
        ReportDocument CRPrint = new ReportDocument();
        ParameterFields PFields = new ParameterFields();

        public CRViewer(ReportDocument CR, ParameterFields PF)
        {
            CRPrint = CR;
            PFields = PF;
            InitializeComponent();
            DXWindow_Loaded();
        }

        private void DXWindow_Loaded()
        {
            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();

            crParameterFieldDefinitions = CRPrint.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions[0];
            crParameterValues = crParameterFieldDefinition.CurrentValues;

            crParameterValues.Clear();            
            crParameterFieldDefinition.ApplyCurrentValues(PFields[0].CurrentValues);
                       
            crystalReportsViewer1.ViewerCore.ReportSource = CRPrint;      
        }
    }
}
