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
using System.IO;
//using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using Entities;
using Business;
using DevExpress.Xpf.Grid;
using Microsoft.Office.Interop;

namespace AplicacionSistemaVentura.PAQ05_Utilitarios
{
    /// <summary>
    /// Interaction logic for UtilCargaMasivaN.xaml
    /// </summary>
    public partial class UtilCargaMasivaN : UserControl
    {
        public UtilCargaMasivaN()
        {
            InitializeComponent();
            UserControl_Loaded();
        }


        private class clsModelo
        {
            public int IdColumna { get; set; }
            public string Descripcion { get; set; }
        }
        object mising = Missing.Value;
        ErrorHandler objError = new ErrorHandler();
        double gdblTamanioMax = 0;
        B_Ciclo objB_Ciclo = new B_Ciclo();
        E_CargaMasiva objE_CargaMasiva = new E_CargaMasiva();
        B_CargaMasiva objB_CargaMasiva = new B_CargaMasiva();
        E_Neumatico objE_Neumatico = new E_Neumatico();
        B_Neumatico objB_Neumatico = new B_Neumatico();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        
        InterfazMTTO.iSBO_BE.BEOITMList glstCodigoSAP;
        IList<clsModelo> glstDisenio, glstTipoBanda, glstCiclo;
        DataTable tblNeumatico, gtblNeumaticoCiclo;
        bool gbolExiteError = false;
        byte[] ExcelNeumatico;
        private void UserControl_Loaded()
        {
            try
            {
                lblUbicacion.Visibility = System.Windows.Visibility.Collapsed;
                objE_TablaMaestra.IdTabla = 0;
                DataView tvwTablaMaestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).DefaultView;
                glstCiclo = CargarClases(objB_Ciclo.Ciclo_Combo(), "IdCiclo", "Ciclo");
                glstDisenio = CargarClases(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 10", tvwTablaMaestra), "IdColumna", "Descripcion");
                glstTipoBanda = CargarClases(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 11", tvwTablaMaestra), "IdColumna", "Descripcion");

                string gstrAlmaSali = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=42 and IdColumna=2", tvwTablaMaestra).Rows[0]["Valor"].ToString();
                InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                glstCodigoSAP = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("N", false, gstrAlmaSali, ref RPTA);


                DataTable tblParam = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1000", tvwTablaMaestra);
                gdblTamanioMax = Convert.ToDouble(tblParam.Rows[4]["Valor"]);

                objE_CargaMasiva.IdCargaMasiva = 1;
                DataTable CargaMasiva = objB_CargaMasiva.CargaMasiva_GetItem(objE_CargaMasiva);
                if (CargaMasiva.Rows[0]["ArchivoCarga"].ToString() != "")
                    ExcelNeumatico = CargaMasiva.Rows[0]["ArchivoCarga"] as byte[];
                else
                    GlobalClass.ip.Mensaje("No se encontro el Archivo Neumatico.", 2);

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private IList<clsModelo> CargarClases(DataTable tbl, string Id, string Descripcion)
        {
            List<clsModelo> lst = new List<clsModelo>();
            foreach (DataRow f in tbl.Rows)
            {
                lst.Add(new clsModelo()
                {
                    IdColumna = Convert.ToInt32(f[Id]),
                    Descripcion = f[Descripcion].ToString()
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

                bool ExisteN = false, ExisteNC = false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet w in workBook.Worksheets)
                {
                    if (w.Name == "Neumatico") ExisteN = true;
                    if (w.Name == "Neumatico_Ciclo") ExisteNC = true;
                }
                if (!ExisteN)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja Neumatico.", 2);
                    lblRuta.Content = null;
                    lblUbicacion.Visibility = System.Windows.Visibility.Collapsed;
                    return;
                }
                if (!ExisteNC)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja Neumatico_Ciclo.", 2);
                    lblRuta.Content = null;
                    lblUbicacion.Visibility = System.Windows.Visibility.Collapsed;
                    return;
                }

                #region dataNeumatico
                Microsoft.Office.Interop.Excel.Worksheet HojaNeumatico = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Neumatico"];
                Microsoft.Office.Interop.Excel.Range RangeNeumatico = HojaNeumatico.UsedRange;

                tblNeumatico = new DataTable();
                tblNeumatico.Columns.Add("IdNeumatico");
                tblNeumatico.Columns.Add("NroSerie");
                tblNeumatico.Columns.Add("Disenio");
                tblNeumatico.Columns.Add("TipoBanda");
                tblNeumatico.Columns.Add("CodigoSAP");//CodigoSAP
                tblNeumatico.Columns.Add("DescripcionSAP");
                tblNeumatico.Columns.Add("IdEstadoN");
                tblNeumatico.Columns.Add("FlagActivo");
                tblNeumatico.Columns.Add("Nuevo");
                tblNeumatico.Columns.Add("Mensaje");
                tblNeumatico.Columns.Add("Color");
                for (int irow = 2; irow <= RangeNeumatico.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = tblNeumatico.NewRow();
                    for (int icolumn = 1; icolumn <= RangeNeumatico.Columns.Count; icolumn++)
                    {
                        object Val = (RangeNeumatico.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (tblNeumatico.Rows.Count + 1 != tblNeumatico.Rows.Count)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en Neumatico.", 2);
                        break;
                    }
                    else
                    {
                        dr["IdNeumatico"] = tblNeumatico.Rows.Count + 1;
                        dr["FlagActivo"] = 1;
                        dr["IdEstadoN"] = 1;
                        dr["Nuevo"] = 1;
                        tblNeumatico.Rows.Add(dr);
                    }
                }
                #endregion
                #region data Ciclo Neumatico
                Microsoft.Office.Interop.Excel.Worksheet HojaCiclo = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Neumatico_Ciclo"];
                Microsoft.Office.Interop.Excel.Range Rangeciclo = HojaCiclo.UsedRange;

                gtblNeumaticoCiclo = new DataTable();
                gtblNeumaticoCiclo.Columns.Add("IdNeumaticoCiclo");
                gtblNeumaticoCiclo.Columns.Add("NroSerie"); // IdNeumatico
                gtblNeumaticoCiclo.Columns.Add("Ciclo");
                gtblNeumaticoCiclo.Columns.Add("Contador");
                gtblNeumaticoCiclo.Columns.Add("FrecuenciaCambio");
                gtblNeumaticoCiclo.Columns.Add("FrecuenciaExtendida");
                gtblNeumaticoCiclo.Columns.Add("FlagCicloPrincipal");
                gtblNeumaticoCiclo.Columns.Add("IdEstado");
                gtblNeumaticoCiclo.Columns.Add("FlagActivo");
                gtblNeumaticoCiclo.Columns.Add("Nuevo");
                gtblNeumaticoCiclo.Columns.Add("Mensaje");
                gtblNeumaticoCiclo.Columns.Add("Color");
                for (int irow = 2; irow <= Rangeciclo.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblNeumaticoCiclo.NewRow();
                    for (int icolumn = 1; icolumn <= Rangeciclo.Columns.Count; icolumn++)
                    {
                        object Val = (Rangeciclo.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (gtblNeumaticoCiclo.Rows.Count + 1 != Rangeciclo.Rows.Count)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en Neumatico_Ciclo.", 2);
                        break;
                    }
                    else
                    {
                        dr["IdNeumaticoCiclo"] = gtblNeumaticoCiclo.Rows.Count + 1;
                        dr["FlagActivo"] = 1;
                        dr["IdEstado"] = 1;
                        dr["Nuevo"] = 1;
                        gtblNeumaticoCiclo.Rows.Add(dr);
                    }
                }
                #endregion


                #region Neumatico
                int cantErrorNeum = 0;
                foreach (DataRow f in tblNeumatico.Rows)
                {
                    string msgInterno = "";
                    for (int iColumna = 1; iColumna < 5; iColumna++)// De NroSerie a Tipobanda
                    {
                        string NombreColumna = tblNeumatico.Columns[iColumna].ColumnName;
                        object Valor = f[iColumna];
                        if (Valor.ToString() == "" && !(new[] { "Disenio", "TipoBanda" }.Contains(NombreColumna)))
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "NroSerie":
                                    if ((tblNeumatico.Select("NroSerie = '" + Valor.ToString() + "'").Length > 1))
                                        msgInterno += string.Format("La Columna <{0}> admite solo valores unicos.\r\n", NombreColumna);
                                    else if (gtblNeumaticoCiclo.Select("NroSerie = '" + Valor + "'").Length != 4)
                                        msgInterno += "Se debe registro 4 Ciclo por cada Neumatico.\r\n";
                                    break;
                                case "Disenio":
                                    if (Valor.ToString() != "" && glstDisenio.Where(x => x.Descripcion == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    break;
                                case "TipoBanda":
                                    if (Valor.ToString() != "" && glstTipoBanda.Where(x => x.Descripcion == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    break;
                                case "CodigoSAP":
                                    if (glstCodigoSAP.Where(x => x.CodigoArticulo == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de la Columna <CódigoSAP>.\r\n", Valor);
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    cantErrorNeum += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                #region Neumatico Ciclo

                int cantErrorNeumCiclo = 0;
                foreach (DataRow f in gtblNeumaticoCiclo.Rows)
                {
                    bool EsNroSerie = false;
                    string msgInterno = "";
                    for (int iColumna = 1; iColumna < 6; iColumna++)// De NroSerie a FrecuenciaExtendida
                    {
                        string NombreColumna = gtblNeumaticoCiclo.Columns[iColumna].ColumnName;
                        object Valor = f[iColumna];
                        if (Valor.ToString() == "")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "NroSerie":
                                    if ((tblNeumatico.Select("NroSerie = '" + Valor + "'").Length == 0))//Cambio 04.12
                                        msgInterno += string.Format("El Valor '{0}' de la Columna <{1}> no exitir en Neumatico.\r\n", Valor, NombreColumna);
                                    else EsNroSerie = true;
                                    break;
                                case "Ciclo":
                                    if (glstCiclo.Where(x => x.Descripcion == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    if (EsNroSerie)                                    
                                        if (gtblNeumaticoCiclo.Select("NroSerie ='" + f["NroSerie"] + "' AND Ciclo = '" + Valor + "'").Length > 1)
                                            msgInterno += "El Ciclo es unico por Neumatico..\r\n";                                    
                                    break;
                                case "Contador":
                                    if (!Utilitarios.Utilitarios.IsDecimal(Valor.ToString()))
                                        msgInterno += string.Format("El formato del Valor '{0}' de <Contador> no es valido.\r\n", Valor);
                                    else if (Convert.ToDecimal(Valor) < 0)
                                        msgInterno += string.Format("la Columna <{0}> no admite valores negativos.\r\n", NombreColumna);
                                    break;
                                case "FrecuenciaCambio":
                                    if (!Utilitarios.Utilitarios.IsDecimal(Valor.ToString()))
                                        msgInterno += string.Format("El formato del Valor '{0}' de <Contador> no es valido.\r\n", Valor);
                                    else if (Convert.ToDecimal(Valor) < 0)
                                        msgInterno += string.Format("la Columna <{0}> no admite valores negativos.\r\n", NombreColumna);
                                    break;
                                case "FrecuenciaExtendida":
                                    if (!Utilitarios.Utilitarios.IsDecimal(Valor.ToString()))
                                        msgInterno += string.Format("El formato del Valor '{0}' de <Contador> no es valido.\r\n", Valor);
                                    else if (Convert.ToDecimal(Valor) < 0)
                                        msgInterno += string.Format("la Columna <{0}> no admite valores negativos.\r\n", NombreColumna);
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    cantErrorNeumCiclo += msgInterno.Split('\n').Length -1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion

                dtgEncabezado.ItemsSource = tblNeumatico;
                dtgDetalle.ItemsSource = gtblNeumaticoCiclo;
                lblTotalErrores.Content = (cantErrorNeum + cantErrorNeumCiclo).ToString();
                workBook.Close(true, mising, mising);
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
            objE_Neumatico.IdEstadoN = 0;
            DataTable tbl = objB_Neumatico.Neumatico_List(objE_Neumatico);
            foreach (DataRow f in tblNeumatico.Rows)
            {
                if (tbl.Select("NroSerie = '" + f["NroSerie"] + "'").Length > 0)
                {
                    GlobalClass.ip.Mensaje(string.Format("La Serie '{0}' del Neumatico ya existe.", f["NroSerie"]), 2);
                    return;
                }
            }

            foreach (DataRow f in tblNeumatico.Rows)
            {
                if (glstDisenio.Where(l => l.Descripcion == f["Disenio"].ToString()).Count() > 0)
                    f["Disenio"] = glstDisenio.Where(l => l.Descripcion == f["Disenio"].ToString()).First().IdColumna;
                if (glstTipoBanda.Where(l => l.Descripcion == f["TipoBanda"].ToString()).Count() > 0)
                    f["TipoBanda"] = glstTipoBanda.Where(l => l.Descripcion == f["TipoBanda"].ToString()).First().IdColumna;
                f["DescripcionSAP"] = glstCodigoSAP.Where(x => x.CodigoArticulo == f["CodigoSAP"].ToString()).First().DescripcionArticulo;
            }
            foreach (DataRow f in gtblNeumaticoCiclo.Rows)
            {
                f["NroSerie"] = tblNeumatico.Select("NroSerie = '" + f["NroSerie"] + "'")[0]["IdNeumatico"];
                f["Ciclo"] = glstCiclo.Where(l => l.Descripcion == f["Ciclo"].ToString()).First().IdColumna;
                f["FlagCicloPrincipal"] = (2 == Convert.ToInt32(f["Ciclo"])) ? 1 : 0;
            }
            try
            {
                tblNeumatico.Columns.Remove("Color");
                tblNeumatico.Columns.Remove("Mensaje");
                gtblNeumaticoCiclo.Columns.Remove("Color");
                gtblNeumaticoCiclo.Columns.Remove("Mensaje");
                objE_Neumatico.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                if (objB_Neumatico.Neumatico_InsertMasivo(objE_Neumatico, tblNeumatico, gtblNeumaticoCiclo) == 0)
                {
                    GlobalClass.ip.Mensaje("Registro Masivo Satisfactorio", 1);
                    gbolExiteError = false;
                    dtgEncabezado.ItemsSource = null;
                    dtgDetalle.ItemsSource = null;
                    tblNeumatico.Dispose();
                    gtblNeumaticoCiclo.Dispose();
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                tblNeumatico.Columns.Add("Color");
                tblNeumatico.Columns.Add("Mensaje");
                gtblNeumaticoCiclo.Columns.Add("Color");
                gtblNeumaticoCiclo.Columns.Add("Mensaje");
            }
            finally
            {
                //tblNeumatico.Rows.Clear();
                //tblNeumatico.Columns.Add("Color");
                //tblNeumatico.Columns.Add("Mensaje");
            }
        }

        private void dtgPLANTILLA_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if ((sender as TableView).Grid.IsVisible)
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
                    System.IO.File.WriteAllBytes(@Ruta, ExcelNeumatico);
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(@Ruta, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
                    Microsoft.Office.Interop.Excel.Worksheet _HojaDisenio = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Diseño"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaTipoBanda = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["TipoBanda"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaCiclo = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Ciclo"];

                    Microsoft.Office.Interop.Excel.Worksheet _HojaNeumatico = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Neumatico"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaNeumatico_ciclo = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Neumatico_Ciclo"];
                    
                    int irow = 1;
                    foreach (clsModelo cls in glstDisenio)
                    {
                        irow++;
                        _HojaDisenio.Cells[irow, 1] = cls.IdColumna;
                        _HojaDisenio.Cells[irow, 2] = "'" + cls.Descripcion;
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstTipoBanda)
                    {
                        irow++;
                        _HojaTipoBanda.Cells[irow, 1] = cls.IdColumna;
                        _HojaTipoBanda.Cells[irow, 2] = "'" + cls.Descripcion;
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstCiclo)
                    {
                        irow++;
                        _HojaCiclo.Cells[irow, 1] = cls.IdColumna;
                        _HojaCiclo.Cells[irow, 2] = "'" + cls.Descripcion;
                    }
                    CargarListaDesplegable(_HojaNeumatico, "B:B", "Diseño");
                    CargarListaDesplegable(_HojaNeumatico,  "C:C", "TipoBanda");
                    CargarListaDesplegable(_HojaNeumatico_ciclo, "B:B","Ciclo");
                    _HojaNeumatico.Activate();
                    workBook.Save();
                    workBook.Close(true, mising, mising);
                    app.Quit();

                    releaseObject(app);
                    releaseObject(workBook);
                    releaseObject(_HojaCiclo);
                    releaseObject(_HojaDisenio);
                    releaseObject(_HojaNeumatico);
                    releaseObject(_HojaNeumatico_ciclo);
                    releaseObject(_HojaTipoBanda);
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

        private void CargarListaDesplegable(Microsoft.Office.Interop.Excel.Worksheet _Hoja, string CoordenadaColumna,string hojaDato)
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
