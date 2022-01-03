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
using System.Data;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Editors;

using Utilitarios;
//using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;
using Entities;
using Business;
using DevExpress.Xpf.Grid;

namespace AplicacionSistemaVentura.PAQ05_Utilitarios
{
    /// <summary>
    /// Interaction logic for UtilCargaMasivaUC.xaml
    /// </summary>
    public partial class UtilCargaMasivaUC : UserControl
    {
        public UtilCargaMasivaUC()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        private class clsModelo
        {
            public int Id { get; set; }
            public string Valor { get; set; }
        }

        ErrorHandler objError = new ErrorHandler();
        double gdblTamanioMax = 0;
        E_UC objE_UC = new E_UC();
        B_UC objB_UC = new B_UC();
        E_Perfil objE_Perfil = new E_Perfil();
        B_Perfil objB_Perfil = new B_Perfil();
        E_PerfilComp objE_PerfilComp = new E_PerfilComp();
        B_PerfilComp objB_PerfilComp = new B_PerfilComp();
        object mising = Missing.Value;
        B_CargaMasiva objB_CargaMasiva = new B_CargaMasiva();
        E_CargaMasiva objE_CargaMasiva = new E_CargaMasiva();
        B_PerfilComp_Ciclo objB_PerfilCompCiclo = new B_PerfilComp_Ciclo();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        B_Ciclo objB_Ciclo = new B_Ciclo();
        IList<clsModelo> glstEstado, glstCiclo;
        InterfazMTTO.iSBO_BE.BEOITMList glstArticulos = new InterfazMTTO.iSBO_BE.BEOITMList();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        DataTable gtblUC, gtblUCComp, gtblPerfil, gtblPerfilComp,gtblItemCiclo,gtblPerfilCompCiclo;
        DataSet gtblUCxTipo = new DataSet();
        bool gbolExiteError = false;
        byte[] ExcelUnidadControl;
        private void UserControl_Loaded()
        {
            try
            {
                lblUbicacion.Visibility = System.Windows.Visibility.Collapsed;
                objE_TablaMaestra.IdTabla = 0;
                DataView tvwTablaMaestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).DefaultView;
                glstEstado = CargarClases(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1", tvwTablaMaestra), "IdColumna", "Descripcion");

                glstCiclo = CargarClases(objB_Ciclo.Ciclo_Combo(), "IdCiclo", "Ciclo");
                gtblPerfil = objB_Perfil.Perfil_Combo(); //CodPerfil - IdPerfil - Perfil - IdTipoUnidad - IdPerfilNeumatico

                gtblPerfilComp = objB_PerfilComp.PerfilComp_Combo();//IdPerfil - Perfil -  IdPerfilComp - CodPerfilComp - PerfilComp - IdTipoDetalle

                objE_Perfil.Idperfil = 0;
                gtblPerfilCompCiclo = objB_PerfilCompCiclo.PerfilComp_Ciclo_List(objE_Perfil);//IdPerfilCompCiclo -IdPerfilComp - IdCiclo  / PerfilComp - Ciclo / IdPerfil - Perfil
                CargarData_TipoUnidad("C");
                CargarData_TipoUnidad("CB");
                CargarData_TipoUnidad("G");
                CargarData_TipoUnidad("M");
                CargarData_TipoUnidad("A");
                CargarData_TipoUnidad("V");

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                glstArticulos = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("P", ref RPTA);


                DataTable tblParam = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1000", tvwTablaMaestra);
                gdblTamanioMax = Convert.ToDouble(tblParam.Rows[4]["Valor"]);

                objE_CargaMasiva.IdCargaMasiva = 4;
                DataTable CargaMasiva = objB_CargaMasiva.CargaMasiva_GetItem(objE_CargaMasiva);
                if (CargaMasiva.Rows[0]["ArchivoCarga"].ToString() != "")
                    ExcelUnidadControl = CargaMasiva.Rows[0]["ArchivoCarga"] as byte[];
                else
                    GlobalClass.ip.Mensaje("No se encontro el Archivo Neumatico.", 2);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void CargarData_TipoUnidad(string tipo)
        {
            DataTable tbl = new DataTable(tipo);
            tbl.Columns.Add("PlacaSerieUnidadControl");
            tbl.Columns.Add("CodigoUnidadControl");

            RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
            InterfazMTTO.iSBO_BE.BEUDUCList tucuclist = InterfazMTTO.iSBO_BL.UnidadControl_BL.ListaUnidadControlxTipo(tipo, ref RPTA);
            for (int j = 0; j < tucuclist.Count; j++)
            {
                DataRow row = tbl.NewRow();
                row["PlacaSerieUnidadControl"] = tucuclist[j].PlacaSerieUnidadControl;
                row["CodigoUnidadControl"] = tucuclist[j].CodigoUnidadControl;
                tbl.Rows.Add(row);
            }
            gtblUCxTipo.Tables.Add(tbl);
        }

        private IList<clsModelo> CargarClases(DataTable tbl, string Id, string Valor)
        {
            List<clsModelo> lst = new List<clsModelo>();
            foreach (DataRow f in tbl.Rows)
            {
                lst.Add(new clsModelo()
                {
                    Id = Convert.ToInt32(f[Id]),
                    Valor = f[Valor].ToString()
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
                    if (w.Name == "UC") ExistePN = true;
                    if (w.Name == "UCComp") ExistePNEje = true;
                    if (w.Name == "ItemCiclo") ExistePNEje = true;
                }
                if (!ExistePN)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja UC.", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                if (!ExistePNEje)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja UCComp", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                if (!ExistePNEje)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja ItemCiclo", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }

                #region data UC
                Microsoft.Office.Interop.Excel.Worksheet HojaUC = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["UC"];
                Microsoft.Office.Interop.Excel.Range RangeUC = HojaUC.UsedRange;
                
                gtblUC = new DataTable();
                gtblUC.Columns.Add("IdUC");
                gtblUC.Columns.Add("PlacaSerie");
                gtblUC.Columns.Add("Perfil");//IdPerfil
                gtblUC.Columns.Add("ContadorAcumulado");
                gtblUC.Columns.Add("IdEstadoUC");
                gtblUC.Columns.Add("FlagActivo");
                gtblUC.Columns.Add("CodUC");
                gtblUC.Columns.Add("Nuevo");
                gtblUC.Columns.Add("Mensaje");
                gtblUC.Columns.Add("Color");

                for (int irow = 2; irow <= RangeUC.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblUC.NewRow();
                    for (int icolumn = 1; icolumn <= RangeUC.Columns.Count; icolumn++)
                    {
                        object Val = (RangeUC.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (gtblUC.Rows.Count != gtblUC.Rows.Count)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en UC.", 2);
                        break;
                    }
                    else
                    {
                        dr["IdUC"] = gtblUC.Rows.Count + 1;
                        dr["IdEstadoUC"] = 1;
                        dr["FlagActivo"] = 1;
                        dr["Nuevo"] = 1;
                        gtblUC.Rows.Add(dr);
                    }
                }
                #endregion
                #region data UCComp
                Microsoft.Office.Interop.Excel.Worksheet HojaUCComp = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["UCComp"];
                Microsoft.Office.Interop.Excel.Range RangeUCComp = HojaUCComp.UsedRange;
                gtblUCComp = new DataTable();
                gtblUCComp.Columns.Add("Id");
                gtblUCComp.Columns.Add("PlacaSerie"); //IdUC
                gtblUCComp.Columns.Add("PerfilComp");// IdPerfilComp
                gtblUCComp.Columns.Add("NroSerie");
                gtblUCComp.Columns.Add("CodigoSAP");
                gtblUCComp.Columns.Add("Estado");
                gtblUCComp.Columns.Add("FlagActivo");
                gtblUCComp.Columns.Add("DescripcionSAP");
                gtblUCComp.Columns.Add("Nuevo");
                gtblUCComp.Columns.Add("Mensaje");
                gtblUCComp.Columns.Add("Color");
                gtblUCComp.Columns.Add("PerfilCompAux");
                gtblUCComp.Columns.Add("PlacaSerieAux");
                for (int irow = 2; irow <= RangeUCComp.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblUCComp.NewRow();
                    for (int icolumn = 1; icolumn <= RangeUCComp.Columns.Count; icolumn++)
                    {
                        object Val = (RangeUCComp.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (gtblUCComp.Rows.Count + 1 != RangeUCComp.Rows.Count)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en UCComp.", 2);
                        break;
                    }
                    else
                    {
                        dr["Id"] = gtblUCComp.Rows.Count + 1;
                        dr["Nuevo"] = 1;
                        dr["FlagActivo"] = 1;
                        gtblUCComp.Rows.Add(dr);
                    }
                }
                #endregion
                #region data Item_Ciclo
                Microsoft.Office.Interop.Excel.Worksheet HojaItemCiclo = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["ItemCiclo"];
                Microsoft.Office.Interop.Excel.Range RangeItemCiclo = HojaItemCiclo.UsedRange;
                gtblItemCiclo = new DataTable();
                gtblItemCiclo.Columns.Add("IdItemCiclo");
                gtblItemCiclo.Columns.Add("PlacaSerie"); //eliminara   }  para obtener
                gtblItemCiclo.Columns.Add("PerfilComp"); // eliminara  }  el IdItem
                gtblItemCiclo.Columns.Add("Ciclo"); //IdPerfilCompCiclo <- Ciclo + IdPerfilComp
                gtblItemCiclo.Columns.Add("Contador");
                gtblItemCiclo.Columns.Add("FrecuenciaCambio");                
                gtblItemCiclo.Columns.Add("Porc1");
                gtblItemCiclo.Columns.Add("Porc2");
                gtblItemCiclo.Columns.Add("IdEstadoCiclo");
                gtblItemCiclo.Columns.Add("FlagActivo");
                gtblItemCiclo.Columns.Add("IdItem");
                gtblItemCiclo.Columns.Add("FrecuenciaExtendida");
                gtblItemCiclo.Columns.Add("FlagCicloPrincipal");
                gtblItemCiclo.Columns.Add("Nuevo");
                gtblItemCiclo.Columns.Add("Mensaje");
                gtblItemCiclo.Columns.Add("Color");
               
                for (int irow = 2; irow <= RangeItemCiclo.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblItemCiclo.NewRow();
                    for (int icolumn = 1; icolumn <= RangeItemCiclo.Columns.Count; icolumn++)
                    {
                        object Val = (RangeItemCiclo.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (gtblItemCiclo.Rows.Count + 1 != RangeItemCiclo.Rows.Count)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en ItemCiclo.", 2);
                        break;
                    }
                    else
                    {
                        dr["IdItemCiclo"] = gtblItemCiclo.Rows.Count + 1;
                        dr["Nuevo"] = 1;
                        dr["IdEstadoCiclo"] = 1;
                        dr["FrecuenciaExtendida"] = 0;
                        dr["FlagCicloPrincipal"] = 0;
                        dr["FlagActivo"] = 1;
                        gtblItemCiclo.Rows.Add(dr);
                    }
                }
                #endregion


                #region UC
                int CantErrorUC = 0;
                foreach (DataRow f in gtblUC.Rows)
                {
                    string msgInterno = "";
                    for (int iColumna = 1; iColumna < 4; iColumna++)// De CodPerfNeum a IdEstadoPN
                    {
                        string NombreColumna = gtblUC.Columns[iColumna].ColumnName;
                        object valorError = f[iColumna];
                        if (valorError.ToString() == "")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "PlacaSerie":
                                    if (gtblUC.Select("PlacaSerie = '" + valorError + "'").Length < 1)
                                        msgInterno += "La PlacaSerie debe ser valor unico.\r\n";
                                    else if (gtblPerfil.Select("Perfil = '" + f["Perfil"] + "'").Length > 0)
                                    {
                                        string tipoUnidad = gtblPerfil.Select("Perfil = '" + f["Perfil"] + "'")[0]["IdTipoUnidad"].ToString();
                                        if (gtblUCxTipo.Tables[tipoUnidad].Select("PlacaSerieUnidadControl = '" + valorError + "'").Length == 0)
                                            msgInterno += "La PlacaSerie no pertenece al mimo tipo de unidad que es perfil";
                                        else if (gtblUCxTipo.Tables[tipoUnidad].Select("PlacaSerieUnidadControl = '" + valorError + "'").Length > 1)
                                            msgInterno += string.Format("La Columna <{0}> admite solo valores unicos.\r\n", NombreColumna);
                                    }
                                    break;
                                case "Perfil":
                                    if (gtblPerfil.Select("Perfil = '" + valorError + "'").Length == 0)
                                        msgInterno += string.Format("El valor '{0}' de la Columna <{1}> no exite o fue anulada.\r\n", valorError, NombreColumna);                                    
                                    break;
                                case "ContadorAcumulado":
                                    if (!Utilitarios.Utilitarios.IsDecimal(valorError.ToString()))
                                        msgInterno += string.Format("El formato del Valor '{0}' de <ContadorAcumulado> no es valido.\r\n", valorError);
                                    else if (Convert.ToDecimal(valorError) < 0)
                                        msgInterno += string.Format("la Columna <{0}> no admite valores negativos.\r\n", NombreColumna);
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorUC += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                #region UCComp
                int CantErrorUCComp = 0;
                foreach (DataRow f in gtblUCComp.Rows)
                {
                    string msgInterno = "";
                    bool IsUC=false;
                    for (int iColumna = 1; iColumna < 6; iColumna++)// De CodPerfNeum a NroLlantas
                    {
                        string NombreColumna = gtblUCComp.Columns[iColumna].ColumnName;
                        object valorError = f[iColumna];
                        if (valorError.ToString() == "")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "PlacaSerie":
                                    if (gtblUC.Select("PlacaSerie = '" + valorError + "'").Length == 0)
                                        msgInterno += "La PlacaSerie no esta registrado en hoja UC.\r\n";
                                    else IsUC=true;
                                    break;
                                case "PerfilComp":
                                    if (IsUC)
                                    {
                                        string perfil = gtblUC.Select("PlacaSerie = '" + f["PlacaSerie"] + "'")[0]["Perfil"].ToString();
                                        int CantCompXperfil=gtblPerfilComp.Select("Perfil = '" + perfil + "' AND perfilComp = '" + valorError + "'").Length;
                                        if (CantCompXperfil == 0)
                                            msgInterno += string.Format("El Componente no pertenece al Pefil {0}.\r\n", perfil);
                                        else if (CantCompXperfil > 1)
                                            msgInterno += "El componente es unico por cada perfil.\r\n";
                                        else if (1 == Convert.ToInt32(gtblPerfilComp.Select("Perfil = '" + perfil + "' AND perfilComp = '" + valorError + "'")[0]["IdtipoDetalle"]))
                                            msgInterno += "El componente de tipo 'Título' no puede ser registrado.\r\n";
                                        else if (gtblItemCiclo.Select("PlacaSerie = '" + f["PlacaSerie"] + "' AND PerfilComp = '" + valorError + "'").Length != 2)
                                            msgInterno += string.Format("Se debe asignar 2 ciclo para el componente '{0}'", valorError);//cambio 04.12
                                    }
                                    break;
                                case "NroSerie":
                                    break;
                                case "CodigoSAP":
                                    if (valorError.ToString() == "")
                                    {
                                        if (f["NroSerie"].ToString() != "")
                                            msgInterno += "CodigoSAP en blanco, entonces el NroSerie debe estar en blanco.\r\n";
                                    }
                                    else
                                    {
                                        if (f["NroSerie"].ToString() == "")
                                            msgInterno += "El NroSerie no debe estar en blanco.\r\n";
                                        if (glstArticulos.Where(x => x.CodigoArticulo == valorError.ToString()).Count() == 0)
                                            msgInterno += string.Format("No se puedo encontrar el Articulo {0} en SAP.\r\n", valorError);
                                    }
                                    break;
                                case "Estado":
                                    if (glstEstado.Where(x => x.Valor == valorError.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", valorError, NombreColumna);
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorUCComp += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                #region Item_Ciclo
                int CantErrorUCCompCiclo = 0;
                foreach (DataRow f in gtblItemCiclo.Rows)
                {
                    string msgInterno = "",CodSAP = "";
                    object perfil=null;
                    for (int iColumna = 1; iColumna < 8; iColumna++)// De PlacaSerie a Porc2
                    {
                        string NombreColumna = gtblItemCiclo.Columns[iColumna].ColumnName;
                        object valorError = f[iColumna];
                        if (valorError.ToString() == "")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "PlacaSerie":
                                    if (gtblUC.Select("PlacaSerie = '" + valorError + "'").Length == 0)
                                        msgInterno += "La PlacaSerie no esta registrado en hoja UC.\r\n";                                    
                                    break;
                                case "PerfilComp":
                                    if (gtblUCComp.Select("PlacaSerie = '" + f["PlacaSerie"] + "' AND PerfilComp = '" + f["PerfilComp"] + "'").Length == 0)
                                        msgInterno += "El valor conjunto <PlacaSerie,PerfilComp> no se encontro en la hoja UCComp.\r\n";
                                    else if (gtblItemCiclo.Select("PlacaSerie = '" + f["PlacaSerie"] + "' AND PerfilComp = '" + f["PerfilComp"] + "'").Length != 2)
                                        msgInterno += "El valor conjunto <PlacaSerie,PerfilComp> debe tener 2 Ciclos.\r\n";
                                    else
                                    {
                                        perfil = gtblUC.Select("PlacaSerie ='" + f["PlacaSerie"] + "'");//[0]["Perfil"];
                                        if (gtblPerfilCompCiclo.Select("Perfil = '" + perfil + "'  AND PerfilComp = '" + valorError + "'").Length == 0)
                                            msgInterno += string.Format("El Componente del Perfil '{0}', no exite o no tiene ciclos en el Sistema.\r\n", perfil);
                                        CodSAP = gtblUCComp.Select("PlacaSerie ='" + f["PlacaSerie"] + "' AND PerfilComp = '" + valorError + "'")[0]["CodigoSAP"].ToString();
                                    }
                                    break;
                                case "Ciclo":
                                    if (glstCiclo.Where(x => x.Valor == valorError.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", valorError, NombreColumna);
                                    else if (new[] { 1, 2 }.Contains(glstCiclo.Where(x => x.Valor == valorError.ToString()).First().Id))
                                        msgInterno += "No se puede registrar un componente tipo Neumatico.\r\n";
                                    else
                                    {
                                        if (gtblItemCiclo.Select("PlacaSerie = '" + f["PlacaSerie"] + "' AND PerfilComp = '" + f["PerfilComp"] + "' AND Ciclo = '" + valorError + "'").Length > 1)
                                            msgInterno += "El Ciclo es unico por cada valor conjunto <PlacaSerie,PerfilComp>.\r\n";
                                        if (gtblPerfilCompCiclo.Select("Perfil = '" + perfil + "' AND PerfilComp = '" + f["PerfilComp"] + "' AND Ciclo = '" + valorError + "'").Length == 0)
                                            msgInterno += "El Tipo de Ciclo no pertenece al componente.\r\n";
                                    }
                                    break;
                                case "Contador":
                                     if (!Utilitarios.Utilitarios.IsDecimal(valorError.ToString()))
                                         msgInterno += string.Format("El formato del Valor '{0}' de <Contador> no es valido.\r\n", valorError);
                                     if (CodSAP.Length==0 && Convert.ToInt32(valorError) != 0)
                                         msgInterno += "CodigoSAP en blanco, entonces la <Contador> debe tener valor 0 (cero).\r\n";
                                     else if (Convert.ToDecimal(valorError) < 0)
                                         msgInterno += string.Format("la Columna <{0}> no admite valores negativos.\r\n", NombreColumna);
                                    break;
                                case "FrecuenciaCambio":
                                     if (!Utilitarios.Utilitarios.IsDecimal(valorError.ToString()))
                                        msgInterno += string.Format("El formato del Valor '{0}' de <ContadorAcumulado> no es valido.\r\n", valorError);
                                     
                                     if (Convert.ToInt32(valorError) != 0)
                                     {
                                         if (CodSAP.Length == 0)
                                             msgInterno += "CodigoSAP en blanco, entonces la <FrecuenciaCambio> debe tener valor 0 (cero).\r\n";
                                     }
                                     else
                                     {
                                         if (Utilitarios.Utilitarios.IsDecimal(f["Contador"].ToString()))
                                             if (Convert.ToDecimal(f["Contador"]) != 0)
                                                 msgInterno += "FrecuenciaCambio = 0, entonces el Contador debe tener valor 0 (cero).\r\n";
                                     }
                                    break;
                                case "Porc1":
                                    if (!Utilitarios.Utilitarios.IsDecimal(valorError.ToString()))
                                        msgInterno += string.Format("El formato del Valor '{0}' de <Límite Amarillo> no es valido.\r\n", valorError);
                                    else if (Convert.ToDecimal(valorError) < 0)
                                        msgInterno += string.Format("la Columna <{0}> no admite valores negativos.\r\n", NombreColumna);
                                    else if (Convert.ToDecimal(valorError) >100)
                                        msgInterno += "El valor de <Límite Amarillo> es invalido.\r\n";
                                    else if (Utilitarios.Utilitarios.IsDecimal(f["Porc2"].ToString()))
                                        if (Convert.ToDecimal(f["Porc2"]) >= Convert.ToDecimal(valorError))
                                            msgInterno += "El <Límite Amarillo> debe ser mayor al <Límite Naranja>.\r\n";
                                    break;
                                case "Porc2":
                                    if (!Utilitarios.Utilitarios.IsDecimal(valorError.ToString()))
                                        msgInterno += string.Format("El formato del Valor '{0}' de <Límite Naranja> no es valido.\r\n", valorError);
                                    else if (Convert.ToDecimal(valorError) < 0)
                                        msgInterno += string.Format("la Columna <{0}> no admite valores negativos.\r\n", NombreColumna);
                                    else if (Convert.ToDecimal(valorError) > 100)
                                        msgInterno += "El valor de <Límite Naranja> es invalido.\r\n";
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorUCCompCiclo += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion

                dtgEncabezado.ItemsSource = gtblUC;
                dtgDetalle.ItemsSource = gtblUCComp;
                dtgItemCiclo.ItemsSource = gtblItemCiclo;
                lblTotalErrores.Content = (CantErrorUC + CantErrorUCComp + CantErrorUCCompCiclo).ToString();
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

        private bool ValidacionNegocio()
        {
            bool rpta = false;
            objE_UC.IdPerfil=0;
            DataTable tblUCList = objB_UC.B_UC_Combo(objE_UC);
            foreach (DataRow f in gtblUC.Rows)
            {
                if (tblUCList.Select("PlacaSerie = '" + f["PlacaSerie"] + "'").Length > 0)
                {
                    GlobalClass.ip.Mensaje(string.Format("La unidad de control '{0}' ya esta registrado", f["PlacaSerie"]), 3);
                    rpta = true;
                    break;
                }
            }
            tblUCList.Dispose();
            return rpta;
        }
        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolExiteError)
                {
                    GlobalClass.ip.Mensaje("Tiene errores pendientes, no puede continuar.", 2);
                    return;
                }
                if (ValidacionNegocio()) return;
                //cambiamos los valores de Valor por IdColumna
                foreach (DataRow f in gtblUC.Rows)
                {
                    string TipoUnidad = gtblPerfil.Select("Perfil = '" + f["Perfil"] + "'")[0]["IdTipoUnidad"].ToString();
                    f["CodUC"] = gtblUCxTipo.Tables[TipoUnidad].Select("PlacaSerieUnidadControl = '" + f["PlacaSerie"] + "'")[0]["CodigoUnidadControl"];
                    f["Perfil"] = gtblPerfil.Select("Perfil = '" + f["Perfil"] + "'")[0]["IdPerfil"];
                }
                foreach (DataRow f in gtblUCComp.Rows)
                {
                    f["PlacaSerieAux"] = f["PlacaSerie"];
                    f["PlacaSerie"] = gtblUC.Select("PlacaSerie = '" + f["PlacaSerie"] + "'")[0]["IdUC"];
                    string idperfil = gtblUC.Select("IdUC = '" + f["PlacaSerie"] + "'")[0]["Perfil"].ToString();
                    f["PerfilCompAux"] = f["PerfilComp"];
                    f["PerfilComp"] = gtblPerfilComp.Select("IdPerfil = " + idperfil + " AND PerfilComp = '" + f["PerfilComp"] + "'")[0]["IdPerfilComp"];
                    f["DescripcionSAP"] = glstArticulos.Where(x => x.CodigoArticulo == f["CodigoSAP"].ToString()).First().DescripcionArticulo;
                    f["Estado"] = glstEstado.Where(x => x.Valor == f["Estado"].ToString()).First().Id;
                }
                foreach (DataRow f in gtblItemCiclo.Rows)
                {
                    f["IdItem"] = gtblUCComp.Select("PlacaSerieAux = '" + f["PlacaSerie"] + "' AND PerfilCompAux = '" + f["PerfilComp"] + "'")[0]["Id"];
                    object IdPerfilComp = gtblUCComp.Select("PlacaSerieAux = '" + f["PlacaSerie"] + "' AND PerfilCompAux = '" + f["PerfilComp"] + "'")[0]["PerfilComp"];
                    int IdCiclo = glstCiclo.Where(x => x.Valor == f["Ciclo"].ToString()).First().Id;
                    f["Ciclo"] = gtblPerfilCompCiclo.Select("IdPerfilComp = " + IdPerfilComp + " AND IdCiclo = " + IdCiclo)[0]["IdPerfilCompCiclo"];
                }


                try
                {
                    gtblUC.Columns.Remove("Color");
                    gtblUC.Columns.Remove("Mensaje");

                    gtblUCComp.Columns.Remove("PerfilCompAux");
                    gtblUCComp.Columns.Remove("PlacaSerieAux");
                    gtblUCComp.Columns.Remove("Color");
                    gtblUCComp.Columns.Remove("Mensaje");

                    gtblItemCiclo.Columns.Remove("PlacaSerie");
                    gtblItemCiclo.Columns.Remove("PerfilComp");
                    gtblItemCiclo.Columns.Remove("Color");
                    gtblItemCiclo.Columns.Remove("Mensaje");

                    objE_UC.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                    if (objB_UC.UC_CargaMasiva(objE_UC, gtblUC, gtblUCComp,gtblItemCiclo) == 0)
                    {
                        gbolExiteError = false;
                        GlobalClass.ip.Mensaje("Registro Masivo Satisfactorio", 1);
                        dtgEncabezado.ItemsSource = null;
                        dtgDetalle.ItemsSource = null;
                        dtgItemCiclo.ItemsSource = null;
                        gtblUC.Dispose();
                        gtblUCComp.Dispose();
                        gtblItemCiclo.Dispose();
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje("Registro Masivo errado", 3);
                    }
                }
                catch (Exception ex)
                {
                    GlobalClass.ip.Mensaje(ex.Message, 3);
                    objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                    gtblUC.Columns.Add("Color");
                    gtblUC.Columns.Add("Mensaje");
                    gtblUCComp.Columns.Add("Color");
                    gtblUCComp.Columns.Add("Mensaje");
                    gtblItemCiclo.Columns.Add("PlacaSerie");
                    gtblItemCiclo.Columns.Add("PerfilComp");
                    gtblItemCiclo.Columns.Add("Color");
                    gtblItemCiclo.Columns.Add("Mensaje");
                }
                finally
                {
                    //gtblUC.Rows.Clear();
                    //gtblUCComp.Rows.Clear();
                    //gtblUC.Columns.Add("Color");
                    //gtblUC.Columns.Add("Mensaje");
                    //gtblUCComp = TempPerfilNeumaticoEje.Clone();
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
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
                    System.IO.File.WriteAllBytes(@Ruta, ExcelUnidadControl);
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(@Ruta, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
                    Microsoft.Office.Interop.Excel.Worksheet _HojaEstado = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Estado"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaCiclo = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Ciclo"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfil = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Perfil"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilComp = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilComp"];


                    Microsoft.Office.Interop.Excel.Worksheet _HojaUC = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["UC"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaUCComp = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["UCComp"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaItemCiclo = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["ItemCiclo"];
                    
                    int irow = 1;                    
                    foreach (clsModelo cls in glstEstado)
                    {
                        irow++;
                        _HojaEstado.Cells[irow, 1] = cls.Id;
                        _HojaEstado.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstCiclo)
                    {
                        irow++;
                        _HojaCiclo.Cells[irow, 1] = cls.Id;
                        _HojaCiclo.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (DataRow f in gtblPerfil.Rows)
                    {
                        irow++;
                        _HojaPerfil.Cells[irow, 1] = f["CodPerfil"];
                        _HojaPerfil.Cells[irow, 2] = "'" + f["Perfil"];
                    }
                    irow = 1;
                    foreach (DataRow f in gtblPerfilComp.Rows)
                    {
                        irow++;                        
                        _HojaPerfilComp.Cells[irow, 1] = "'" + f["CodPerfilComp"];
                        _HojaPerfilComp.Cells[irow, 2] = "'" + f["PerfilComp"];
                        _HojaPerfilComp.Cells[irow, 3] = "'" + f["Perfil"];
                    }

                    CargarListaDesplegable(_HojaUC, "B:B", "Perfil");
                    CargarListaDesplegable(_HojaUCComp, "B:B", "PerfilComp");
                    CargarListaDesplegable(_HojaUCComp, "E:E", "Estado");
                    CargarListaDesplegable(_HojaItemCiclo, "B:B", "PerfilComp");
                    CargarListaDesplegable(_HojaItemCiclo, "C:C", "Ciclo");
                    _HojaUC.Activate();
                    workBook.Save();
                    workBook.Close(true, mising, mising);
                    app.Quit();

                    releaseObject(app);
                    releaseObject(workBook);
                    releaseObject(_HojaCiclo);
                    releaseObject(_HojaEstado);
                    releaseObject(_HojaItemCiclo);
                    releaseObject(_HojaPerfil);
                    releaseObject(_HojaPerfilComp);
                    releaseObject(_HojaUC);
                    releaseObject(_HojaUCComp);
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
