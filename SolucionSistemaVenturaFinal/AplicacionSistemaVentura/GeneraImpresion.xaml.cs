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
using System.Windows.Shapes;
using System.Data;
using Business;
using Entities;
using Utilitarios;

namespace AplicacionSistemaVentura
{
    /// <summary>
    /// Lógica de interacción para GeneraImpresion.xaml
    /// </summary>
    public partial class GeneraImpresion : Window
    {
        DataTable gtblFormatosImpresion = new DataTable();
        DataTable tblFormatosImpresion = new DataTable();
        ErrorHandler Error = new ErrorHandler();
        int IdFormatoImpresion = 0;
        string Param = string.Empty;

        public GeneraImpresion(DataTable dtblFormatosImpresion, string pParam)
        {
            tblFormatosImpresion = dtblFormatosImpresion;
            Param = pParam;
            InitializeComponent();
        }

        

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            string Ruta = string.Empty;
            Window XAML = GlobalClass.ip;

            DataTable tblFile = B_FormatoImpresion.FormatoImpresion_GetFile(IdFormatoImpresion);
            Ruta = GlobalClass.GrabaFormatoImpresion(tblFile);
            GlobalClass.ImprimirCR(Ruta, Param, XAML);

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gtblFormatosImpresion = new DataTable();
            gtblFormatosImpresion.Columns.Add("Id_FormatoImpresion", Type.GetType("System.Int32"));
            gtblFormatosImpresion.Columns.Add("NombreArchivo", Type.GetType("System.String"));
            LoadGridFormatosImpresion();
        }


        private void LoadGridFormatosImpresion()
        {
            try
            {
                gtblFormatosImpresion.Rows.Clear();                

                for (int i = 0; i < tblFormatosImpresion.Rows.Count; i++)
                {
                    DataRow Fila = gtblFormatosImpresion.NewRow();
                    Fila["Id_FormatoImpresion"] = Convert.ToInt32(tblFormatosImpresion.Rows[i]["Id_FormatoImpresion"]);
                    Fila["NombreArchivo"] = tblFormatosImpresion.Rows[i]["NombreArchivo"];
                    gtblFormatosImpresion.Rows.Add(Fila);
                }

                dtgFormatoImpresion.ItemsSource = gtblFormatosImpresion;
            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void dtgFormatoImpresion_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {            
            IdFormatoImpresion = Convert.ToInt32(dtgFormatoImpresion.GetFocusedRowCellValue("Id_FormatoImpresion").ToString());                        
        }
    }
}
