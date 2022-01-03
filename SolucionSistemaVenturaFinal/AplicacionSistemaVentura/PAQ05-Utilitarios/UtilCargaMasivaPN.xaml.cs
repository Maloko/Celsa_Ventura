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

using Utilitarios;
//using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using Entities;
using Business;
using DevExpress.Xpf.Grid;

namespace AplicacionSistemaVentura.PAQ05_Utilitarios
{
    /// <summary>
    /// Interaction logic for UtilCargaMasivaPN.xaml
    /// </summary>
    public partial class UtilCargaMasivaPN : UserControl
    {
        public UtilCargaMasivaPN()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        private class clsModelo
        {
            public int IdColumna { get; set; }
            public string Valor { get; set; }
        }
        object mising = Missing.Value;
        ErrorHandler objError = new ErrorHandler();        
        double gdblTamanioMax = 0;
        B_CargaMasiva objB_CargaMasiva = new B_CargaMasiva();
        E_CargaMasiva objE_CargaMasiva = new E_CargaMasiva();
        B_PerfilNeumatico objB_HojaPerfilNeumatico = new B_PerfilNeumatico();
        E_PerfilNeumatico objE_HojaPerfilNeumatico = new E_PerfilNeumatico();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        IList<clsModelo> glstLlantaRepuesto;
        IList<clsModelo> glstFlagActivo;
        IList<clsModelo> glstLlantas;
        IList<clsModelo> glstEje;
        DataTable tblPerfilNeumatico, tblPerfilNeumaticoEje;
        bool gbolExiteError = false;
        byte[] ExcelPerfilNeumatico;
        private void UserControl_Loaded()
        {
            try
            {
                lblUbicacion.Visibility = System.Windows.Visibility.Collapsed;
                objE_TablaMaestra.IdTabla = 0;
                DataView tvwTablaMaestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).DefaultView;
                glstLlantaRepuesto = CargarClases(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 7", tvwTablaMaestra));
                glstFlagActivo = CargarClases(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 4", tvwTablaMaestra));
                glstLlantas = CargarClases(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 5", tvwTablaMaestra));
                glstEje = CargarClases(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 6", tvwTablaMaestra));
                DataTable tblParam = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1000", tvwTablaMaestra);
                gdblTamanioMax = Convert.ToDouble(tblParam.Rows[4]["Valor"]);

                objE_CargaMasiva.IdCargaMasiva = 2;
                DataTable CargaMasiva = objB_CargaMasiva.CargaMasiva_GetItem(objE_CargaMasiva);
                if (CargaMasiva.Rows[0]["ArchivoCarga"].ToString() != "")
                    ExcelPerfilNeumatico = CargaMasiva.Rows[0]["ArchivoCarga"] as byte[];
                else
                    GlobalClass.ip.Mensaje("No se encontro el Archivo Perfil Neumatico.", 2);

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private IList<clsModelo> CargarClases(DataTable tbl)
        {
            List<clsModelo> lst = new List<clsModelo>();
            foreach (DataRow f in tbl.Rows)
            {
                lst.Add(new clsModelo()
                {
                    IdColumna = Convert.ToInt32(f["IdColumna"]),
                    Valor = f["Valor"].ToString()
                });
            }
            return lst;
        }

        private void btnAbrirExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "Archivo de Excel (*.xlsx; *.xls)|*.xlsx; *.xls";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    gbolExiteError = false;
                    string fileName = dlg.SafeFileName;
                    string sourcePath = System.IO.Path.GetDirectoryName(dlg.FileName);
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                    var fileInfo = new System.IO.FileInfo(sourceFile);
                    double volumenMB = fileInfo.Length / 1024 / 1024;
                    if (volumenMB > gdblTamanioMax)
                    {
                        GlobalClass.ip.Mensaje(string.Format("El maximo tamaño en MB de archivos es {0}.", gdblTamanioMax), 2);
                        return;
                    }
                    lblRuta.Content = sourceFile;
                    lblUbicacion.Visibility = System.Windows.Visibility.Visible;
                    CargarExcel(@sourceFile);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public void CargarExcel(String path)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                bool ExistePN = false, ExistePNEje = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet w in workBook.Worksheets)
                {
                    if (w.Name == "PerfilNeumatico") ExistePN = true;
                    if (w.Name == "PerfilNeumaticoEje") ExistePNEje = true;
                }
                if (!ExistePN)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja PerfilNeumatico.", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                if (!ExistePNEje)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja PerfilNeumaticoEje", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                Microsoft.Office.Interop.Excel.Worksheet HojaPerfilNeumatico = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilNeumatico"];
                Microsoft.Office.Interop.Excel.Worksheet HojaPerfilNeumaticoEje = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilNeumaticoEje"];
                Microsoft.Office.Interop.Excel.Range RangePerfilNeumatico = HojaPerfilNeumatico.UsedRange;
                Microsoft.Office.Interop.Excel.Range RangePerfilNeumaticoEje = HojaPerfilNeumaticoEje.UsedRange;
                #region data PerfilNeumatico
                tblPerfilNeumatico = new DataTable();
                tblPerfilNeumatico.Columns.Add("IdPerfNeum");
                tblPerfilNeumatico.Columns.Add("PerfNeum");
                tblPerfilNeumatico.Columns.Add("NroEjes");
                tblPerfilNeumatico.Columns.Add("NroLlanRepu");
                tblPerfilNeumatico.Columns.Add("IdEstadoPN");
                tblPerfilNeumatico.Columns.Add("FlagActivo");
                tblPerfilNeumatico.Columns.Add("Nuevo");
                tblPerfilNeumatico.Columns.Add("Mensaje");
                tblPerfilNeumatico.Columns.Add("Color");
                for (int irow = 2; irow <= RangePerfilNeumatico.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = tblPerfilNeumatico.NewRow();
                    for (int icolumn = 1; icolumn <= RangePerfilNeumatico.Columns.Count; icolumn++)
                    {
                        object Val = (RangePerfilNeumatico.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (tblPerfilNeumatico.Rows.Count != tblPerfilNeumatico.Rows.Count)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en PerfilNeumatico.", 2);
                        break;
                    }
                    else
                    {
                        dr["IdPerfNeum"] = tblPerfilNeumatico.Rows.Count + 1;
                        dr["FlagActivo"] = 1;
                        dr["Nuevo"] = 1;
                        tblPerfilNeumatico.Rows.Add(dr);
                    }
                }
                #endregion
                #region data PerfilNeumatico Eje
                tblPerfilNeumaticoEje = new DataTable();
                tblPerfilNeumaticoEje.Columns.Add("IdPerfNeumEje");
                tblPerfilNeumaticoEje.Columns.Add("PerfNeum"); // IdPerfilNeum
                tblPerfilNeumaticoEje.Columns.Add("Eje");
                tblPerfilNeumaticoEje.Columns.Add("NroLlantas");
                tblPerfilNeumaticoEje.Columns.Add("FlagActivo");
                tblPerfilNeumaticoEje.Columns.Add("Nuevo");
                tblPerfilNeumaticoEje.Columns.Add("Mensaje");
                tblPerfilNeumaticoEje.Columns.Add("Color");
                for (int irow = 2; irow <= RangePerfilNeumaticoEje.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = tblPerfilNeumaticoEje.NewRow();
                    for (int icolumn = 1; icolumn <= RangePerfilNeumaticoEje.Columns.Count; icolumn++)
                    {
                        object Val = (RangePerfilNeumaticoEje.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (tblPerfilNeumaticoEje.Rows.Count != tblPerfilNeumaticoEje.Rows.Count)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en PerfilNeumaticoEje.", 2);
                        break;
                    }
                    else
                    {
                        dr["IdPerfNeumEje"] = tblPerfilNeumaticoEje.Rows.Count + 1;
                        dr["Nuevo"] = 1;
                        dr["FlagActivo"] = 1;
                        tblPerfilNeumaticoEje.Rows.Add(dr);
                    }
                }
                #endregion

                #region PefilNeumatico
                int CantErrorPFNeum = 0;
                foreach (DataRow f in tblPerfilNeumatico.Rows)
                {
                    string msgInterno = "";
                    for (int iColumna = 1; iColumna < 5; iColumna++)// De PerfNeum a IdEstadoPN
                    {
                        string NombreColumna = tblPerfilNeumatico.Columns[iColumna].ColumnName;
                        object valorError = f[iColumna];
                        if (valorError.ToString() == "")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else                        
                        {
                            switch (NombreColumna)
                            {
                                case "PerfNeum":
                                    if ((tblPerfilNeumatico.Select("PerfNeum = '" + valorError.ToString() + "'").Length > 1))
                                        msgInterno += string.Format("La Columna <{0}> admite solo valores unicos.\r\n", NombreColumna);
                                    else if (tblPerfilNeumaticoEje.Select("PerfNeum='" + valorError + "'").Length == 0)//cambio 04.12
                                        msgInterno += "No se encontro el registro de los ejes en <PerfilNeumaticoEje>.\r\n";
                                    break;
                                case "NroEjes":
                                    if (!Utilitarios.Utilitarios.IsNumeric(valorError.ToString()))
                                        msgInterno += string.Format("La Columna <{0}> admite solo valor numerico.\r\n", NombreColumna);
                                    else if (glstEje.Where(x => x.Valor == valorError.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", valorError, NombreColumna);
                                    else if (tblPerfilNeumaticoEje.Select("PerfNeum='" + f["PerfNeum"] + "'").Length != Convert.ToInt32(valorError))//cambio 04.12
                                        msgInterno += string.Format("La cantidad de ejes en <PerfilNeumaticoEje> no coinciden a la cantidad establecida.\r\n");
                                    break;
                                case "NroLlanRepu":
                                    if (!Utilitarios.Utilitarios.IsNumeric(valorError.ToString()))
                                        msgInterno += string.Format("La Columna <{0}> admite solo valor numerico.\r\n", NombreColumna);
                                    if (glstLlantaRepuesto.Where(x => x.Valor == valorError.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", valorError, NombreColumna);
                                    break;
                                case "IdEstadoPN":
                                    if (glstFlagActivo.Where(x => x.Valor == valorError.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", valorError, NombreColumna);
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorPFNeum += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                #region PefilNeumaticoEje
                int CantErrorPFNeumEje = 0;
                foreach (DataRow f in tblPerfilNeumaticoEje.Rows)
                {
                    string msgInterno = "";
                    string PerfNeum = "";
                    bool ExistePerfNeum = false;
                    int Eje = 0;
                    
                    for (int iColumna = 1; iColumna < 4; iColumna++)// De CodPerfNeum a NroLlantas
                    {
                        string NombreColumna = tblPerfilNeumaticoEje.Columns[iColumna].ColumnName;
                        object valorError = f[iColumna];
                        if (valorError.ToString() == "")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "PerfNeum":
                                    PerfNeum = valorError.ToString();
                                    if ((tblPerfilNeumatico.Select("PerfNeum = '" + PerfNeum + "'").Length == 0))
                                        msgInterno += string.Format("El Valor '{0}' de la Columna <{1}> no exitir en PerfilNeumatico.\r\n", valorError, NombreColumna);
                                    else
                                        ExistePerfNeum = true;
                                    break;
                                case "Eje":
                                    Eje = Convert.ToInt32(valorError);
                                    if (!Utilitarios.Utilitarios.IsNumeric(valorError.ToString()))
                                        msgInterno += string.Format("La Columna <{0}> admite solo valor numerico.\r\n", NombreColumna);
                                    if (glstEje.Where(x => x.Valor == valorError.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", valorError, NombreColumna);
                                    if (ExistePerfNeum)
                                    {                                        
                                        if (tblPerfilNeumaticoEje.Select("PerfNeum = '" + PerfNeum + "' AND Eje = " + Eje).Length > 1)
                                            msgInterno += string.Format("El PerfNeum '{0}' ya tine el Eje de número '{1}'.\r\n", PerfNeum, Eje);
                                        if (Eje > tblPerfilNeumaticoEje.Select("Eje <= " + Eje + " AND PerfNeum = '" + PerfNeum + "'").Length)
                                        {
                                            string Ejes = string.Empty;
                                            for (int i = 1; i < Eje; i++) Ejes += i + ",";
                                            Ejes = Ejes.Remove(Ejes.Length - 1);
                                            msgInterno += string.Format("Deben existir eje(s) '{0}' para registrar el Eje '{1}'.\r\n", Ejes, Eje);
                                        }
                                    }
                                    break;
                                case "NroLlantas":
                                    if (!Utilitarios.Utilitarios.IsNumeric(valorError.ToString()))
                                        msgInterno += string.Format("La Columna <{0}> admite solo valor numerico.\r\n", NombreColumna);
                                    if (glstLlantas.Where(x => x.Valor == valorError.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", valorError, NombreColumna);
                                    if (Eje != 1 && Convert.ToInt32(valorError) == 1)
                                        msgInterno += "Solo el Eje 1 puede tener una llanta";
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorPFNeumEje += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                
                dtgEncabezado.ItemsSource = tblPerfilNeumatico;
                dtgDetalle.ItemsSource = tblPerfilNeumaticoEje;
                lblTotalErrores.Content = (CantErrorPFNeum + CantErrorPFNeumEje).ToString();
                workBook.Close(true, Missing.Value, Missing.Value);
                app.Quit();
                Process[] Processes = Process.GetProcessesByName("EXCEL");
                foreach (Process p in Processes) { if (p.MainWindowTitle.Trim() == "") p.Kill(); }

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (gbolExiteError)
            {
                GlobalClass.ip.Mensaje("Tiene errores pendientes, no puede continuar.", 2);
                return;
            }
            objE_HojaPerfilNeumatico.FlagActivo=-1;
            DataTable tbl = B_PerfilNeumatico.PerfilNeumatico_List(objE_HojaPerfilNeumatico);
            foreach(DataRow f in tblPerfilNeumatico.Rows){                
                if (tbl.Select("PerfilNeumatico = '" + f["PerfNeum"] + "'").Length > 0)
                {
                    GlobalClass.ip.Mensaje(string.Format("El PerfilNeumatico {0} ya existe.", f["PerfNeum"]), 2);
                    return;
                }
            }

            //cambiamos los valores de Valor por IdColumna
            foreach (DataRow f in tblPerfilNeumatico.Rows)
            {
                f["NroEjes"]=glstEje.Where(l => l.Valor == f["NroEjes"].ToString()).First().IdColumna;
                f["NroLlanRepu"] = glstLlantaRepuesto.Where(l => l.Valor == f["NroLlanRepu"].ToString()).First().IdColumna;
                f["IdEstadoPN"] = glstFlagActivo.Where(l => l.Valor == f["IdEstadoPN"].ToString()).First().IdColumna;
                //f["FlagActivo"] = (Convert.ToInt32(f["IdEstadoPN"]) == 1) ? 1 : 0;
            }
            foreach (DataRow f in tblPerfilNeumaticoEje.Rows)
            {
                f["PerfNeum"] = tblPerfilNeumatico.Select("PerfNeum = '" + f["PerfNeum"] + "'")[0]["IdPerfNeum"];
                f["Eje"] = "E" + Utilitarios.Utilitarios.NumeroChar2(Convert.ToInt32(f["Eje"]));
            }

            //tblPerfilNeumaticoEje = tblPerfilNeumaticoEje.AsEnumerable().OrderBy(r => r["CodPerfNeum"]).ThenBy(r => r["Eje"]).CopyToDataTable();
            
            try
            {
                tblPerfilNeumatico.Columns.Remove("Color");
                tblPerfilNeumatico.Columns.Remove("Mensaje");
                tblPerfilNeumaticoEje.Columns.Remove("Color");
                tblPerfilNeumaticoEje.Columns.Remove("Mensaje");
                objE_HojaPerfilNeumatico.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                if (objB_HojaPerfilNeumatico.PerfilNeumatico_InsertMasivo(objE_HojaPerfilNeumatico, tblPerfilNeumatico, tblPerfilNeumaticoEje) == 0)
                {
                    gbolExiteError = false;
                    GlobalClass.ip.Mensaje("Registro Masivo Satisfactorio", 1);
                    dtgEncabezado.ItemsSource = null;
                    dtgDetalle.ItemsSource = null;
                    tblPerfilNeumatico.Dispose();
                    tblPerfilNeumaticoEje.Dispose();
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            finally
            {
                //tblPerfilNeumatico.Rows.Clear();
                //tblPerfilNeumaticoEje.Rows.Clear();
                //tblPerfilNeumatico.Columns.Add("Color");
                //tblPerfilNeumatico.Columns.Add("Mensaje");
                //tblPerfilNeumaticoEje = TempPerfilNeumaticoEje.Clone();
            }
        }

        private void dtgPLANTILLA_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if((sender as TableView).Grid.IsVisible)
                    txtMensaje.Text = (sender as TableView).Grid.GetFocusedRowCellValue("Mensaje").ToString();
            }
            catch { txtMensaje.Text = ""; }
        }

        private void dtgPLANTILLA_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                txtMensaje.Text = (sender as TableView).Grid.GetFocusedRowCellValue("Mensaje").ToString();
            }
            catch { txtMensaje.Text = ""; }
        }

        private void btnDescargar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.Filter = "Libro de Excel (*.xlsx)|*.xlsx | Libro de Excel 97-2003 (*.xls)| *.xls";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    string Ruta = dlg.FileName;
                    System.IO.File.WriteAllBytes(@Ruta, ExcelPerfilNeumatico);
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(@Ruta, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
                    Microsoft.Office.Interop.Excel.Worksheet _HojaFlagActivo = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["FlagActivo"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaEje = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Eje"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaNroLlantasPorEje = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["NroLlantasPorEje"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaNroLlantaRepu = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["NroLlantaRepu"];

                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilNeumatico = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilNeumatico"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilNeumaticoEje = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilNeumaticoEje"];


                    int irow = 1;
                    foreach (clsModelo cls in glstFlagActivo)
                    {
                        irow++;
                        _HojaFlagActivo.Cells[irow, 1] = cls.IdColumna;
                        _HojaFlagActivo.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstEje)
                    {
                        irow++;
                        _HojaEje.Cells[irow, 1] = cls.IdColumna;
                        _HojaEje.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstLlantas)
                    {
                        irow++;
                        _HojaNroLlantasPorEje.Cells[irow, 1] = cls.IdColumna;
                        _HojaNroLlantasPorEje.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstLlantaRepuesto)
                    {
                        irow++;
                        _HojaNroLlantaRepu.Cells[irow, 1] = cls.IdColumna;
                        _HojaNroLlantaRepu.Cells[irow, 2] = "'" + cls.Valor;
                    }

                    CargarListaDesplegable(_HojaPerfilNeumatico, "B:B", "Eje");
                    CargarListaDesplegable(_HojaPerfilNeumatico, "C:C", "NroLlantaRepu");
                    CargarListaDesplegable(_HojaPerfilNeumatico, "D:D", "FlagActivo");
                    CargarListaDesplegable(_HojaPerfilNeumaticoEje, "B:B", "Eje");
                    CargarListaDesplegable(_HojaPerfilNeumaticoEje, "C:C", "NroLlantasPorEje");
                    _HojaPerfilNeumatico.Activate();
                    workBook.Save();
                    workBook.Close(true, mising, mising);
                    app.Quit();

                    releaseObject(app);
                    releaseObject(workBook);
                    releaseObject(_HojaFlagActivo);
                    releaseObject(_HojaEje);
                    releaseObject(_HojaNroLlantaRepu);
                    releaseObject(_HojaNroLlantasPorEje);
                    releaseObject(_HojaPerfilNeumatico);
                    releaseObject(_HojaPerfilNeumaticoEje);
                    GlobalClass.ip.Mensaje("Descarga de Plantilla satisfactoria", 1);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            finally
            {

                Process[] Processes = Process.GetProcessesByName("EXCEL");
                foreach (Process p in Processes) { if (p.MainWindowTitle.Trim() == "") p.Kill(); }
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void CargarListaDesplegable(Microsoft.Office.Interop.Excel.Worksheet _Hoja, string CoordenadaColumna, string hojaDato)
        {
            string formula = string.Format("='{0}'!$B$2:$B$10000", hojaDato);
            _Hoja.get_Range(CoordenadaColumna, mising).Validation.Delete();
            _Hoja.get_Range(CoordenadaColumna, mising).Validation.Add(
                Microsoft.Office.Interop.Excel.XlDVType.xlValidateList,
                Microsoft.Office.Interop.Excel.XlDVAlertStyle.xlValidAlertInformation,
                Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlBetween, formula, mising);
            _Hoja.get_Range(CoordenadaColumna, mising).Validation.InCellDropdown = true;
        }
    
    }
}
