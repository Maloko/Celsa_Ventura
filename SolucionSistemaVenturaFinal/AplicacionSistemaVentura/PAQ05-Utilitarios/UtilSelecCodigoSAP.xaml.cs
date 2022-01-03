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
using DevExpress.Xpf.Editors.Settings;
using System.Data;

namespace AplicacionSistemaVentura.PAQ05_Utilitarios
{
    /// <summary>
    /// Interaction logic for UtilSelecCodigoSAP.xaml
    /// </summary>
    public partial class UtilSelecCodigoSAP : UserControl
    {
        public UtilSelecCodigoSAP()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            DataTable GrillaDato = new DataTable();

            GrillaDato.Columns.Add("Codigo");

            GrillaDato.Columns.Add("Descripcion");

            DataRow row1;

            row1 = GrillaDato.NewRow();

            row1["Codigo"] = "SAP01";

            row1["Descripcion"] = "ASDASDASDASDASD";

            GrillaDato.Rows.Add(row1);


            row1 = GrillaDato.NewRow();

            row1["Codigo"] = "SAP02";

            row1["Descripcion"] = "fffffffffffffff";

            GrillaDato.Rows.Add(row1);


            gridControl1.ItemsSource = GrillaDato;
        }
    }
}
