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
using DevExpress.Xpf.Editors.Settings;
using Entities;
using Business;
using Utilitarios;
using DevExpress.Xpf.Grid;
using System.Reflection;
using System.Diagnostics;

namespace AplicacionSistemaVentura.PAQ05_Utilitarios
{
    /// <summary>
    /// Interaction logic for UtilCargaMasivaPUC.xaml
    /// </summary>
    public partial class UtilCargaMasivaPUC : UserControl
    {
        public UtilCargaMasivaPUC()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        private class clsModelo
        {
            public int Id { get; set; }
            public string Cod { get; set; }
            public string Valor { get; set; }
        }


        ErrorHandler objError = new ErrorHandler();
        double gdblTamanioMax = 0;
        B_Perfil objB_Perfil = new B_Perfil();
        E_Perfil objE_Perfil = new E_Perfil();
        B_Tarea objB_Tarea = new B_Tarea();
        E_Tarea objE_Tarea = new E_Tarea();
        object mising = Missing.Value;
        B_Herramienta objB_Herramienta=new B_Herramienta();

        E_CargaMasiva objE_CargaMasiva = new E_CargaMasiva();
        B_CargaMasiva objB_CargaMasiva = new B_CargaMasiva();
        B_PerfilNeumatico objB_PerfilNeumatico = new B_PerfilNeumatico();
        E_PerfilNeumatico objE_PerfilNeumatico = new E_PerfilNeumatico();
        B_Actividad objB_Actividad = new B_Actividad();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        IList<clsModelo> glstCiclo, glstEstado,glstPerfilNeumatico,glstFlag,glstTipoDetalle,glstActividad,glstTipoArticulo;
        DataTable gtblPerfilUC, gtblPerfilComp, gtblPCActividad,gtblPCCiclo,gtblPCATarea,gtblPCADetalle;
        DataTable gtblTareaByAct = new DataTable();// IdActividad - Actividad - IdTarea - CodTarea - Tarea
        DataTable gtblHerramienta = new DataTable();//CodHerramienta, IdHerramienta, Herramienta, Cantidad(para su maximo valor)
        InterfazMTTO.iSBO_BE.BETUDUCList glstSAPTipoUnidad;
        InterfazMTTO.iSBO_BE.BEOITMList glstSAPRepuesto,glstSAPConsumible;
        byte[] ExcelPerfilUC;
        B_Ciclo objB_Ciclo = new B_Ciclo();
        bool gbolExiteError = false;
        
        private void UserControl_Loaded()
        {
            try
            {
                lblUbicacion.Visibility = System.Windows.Visibility.Collapsed;
                objE_TablaMaestra.IdTabla = 0;
                DataView tvwTablaMaestra = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).DefaultView;

                glstCiclo = CargarClases(objB_Ciclo.Ciclo_Combo(), "IdCiclo", "Ciclo");
                glstEstado = CargarClases(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1", tvwTablaMaestra), "IdColumna", "Descripcion");
                objE_PerfilNeumatico.FlagActivo = 1;
                DataTable tblPN = B_PerfilNeumatico.PerfilNeumatico_List(objE_PerfilNeumatico);
                tblPN.DefaultView.RowFilter = "IdEstadoPN = 1";
                glstPerfilNeumatico = CargarClases(tblPN.DefaultView.ToTable(), "IdPerfilNeumatico", "CodPerfilNeumatico", "PerfilNeumatico");
                glstActividad = CargarClases(objB_Actividad.Actividad_Combo(), "IdActividad", "CodActividad", "Actividad");

                List<clsModelo> lst = new List<clsModelo>();
                lst.Add(new clsModelo() { Id = 1, Valor = "SI" });
                lst.Add(new clsModelo() { Id = 0, Valor = "NO" });
                glstFlag = lst;
                lst = new List<clsModelo>();
                lst.Add(new clsModelo() { Id = 1, Valor = "Título" });
                lst.Add(new clsModelo() { Id = 2, Valor = "Componente" });
                glstTipoDetalle = lst;
                lst = new List<clsModelo>();
                lst.Add(new clsModelo() { Id = 1, Valor = "Herramienta" });
                lst.Add(new clsModelo() { Id = 2, Valor = "Repuesto" });
                lst.Add(new clsModelo() { Id = 3, Valor = "Consumible" });
                glstTipoArticulo = lst;


                objE_Tarea.IdActividad = 0;
                objE_Tarea.Actividad = "";
                gtblTareaByAct = objB_Tarea.Tarea_ComboByAct(objE_Tarea);


                DataTable tblParam = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1000", tvwTablaMaestra);
                gdblTamanioMax = Convert.ToDouble(tblParam.Rows[4]["Valor"]);

                InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                glstSAPTipoUnidad = InterfazMTTO.iSBO_BL.UnidadControl_BL.ListaTipoUnidadControl(ref RPTA);

                if (RPTA.ResultadoRetorno != 0)
                    GlobalClass.ip.Mensaje("Carga TipoUnidad: " + RPTA.DescripcionErrorUsuario, 3);

                glstSAPRepuesto = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("R", ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                    GlobalClass.ip.Mensaje("Carga Repuesto: " + RPTA.DescripcionErrorUsuario, 3);

                glstSAPConsumible = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("C", ref RPTA);
                if (RPTA.ResultadoRetorno != 0)
                    GlobalClass.ip.Mensaje("Carga Consumible: " + RPTA.DescripcionErrorUsuario, 3);

                gtblHerramienta = objB_Herramienta.Herramienta_Combo();


                objE_CargaMasiva.IdCargaMasiva = 3;
                DataTable CargaMasiva = objB_CargaMasiva.CargaMasiva_GetItem(objE_CargaMasiva);
                if (CargaMasiva.Rows[0]["ArchivoCarga"].ToString() != "")
                    ExcelPerfilUC = CargaMasiva.Rows[0]["ArchivoCarga"] as byte[];
                else
                    GlobalClass.ip.Mensaje("No se encontro el Archivo Perfil Unidad Control.", 2);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private IList<clsModelo> CargarClases(DataTable tbl,string Id,string Valor)
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
        private IList<clsModelo> CargarClases(DataTable tbl, string Id,string Codigo, string Valor)
        {
            List<clsModelo> lst = new List<clsModelo>();
            foreach (DataRow f in tbl.Rows)
            {
                lst.Add(new clsModelo()
                {
                    Id = Convert.ToInt32(f[Id]),
                    Cod=f[Codigo].ToString(),
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
                    gbolExiteError = false;
                    CargarExcel(@sourceFile);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void NivelarComponentes(string CodCompPadre,int NivelPadre)
        {
            foreach (DataRow f in gtblPerfilComp.Select("IdPerfilCompPadre = '" + CodCompPadre + "'"))
            {                
                f["Nivel"] = 1 + NivelPadre;
                if(f["CodPerfilComp"].ToString() != f["IdPerfilCompPadre"].ToString())
                    NivelarComponentes(f["CodPerfilComp"].ToString(), Convert.ToInt32(f["Nivel"]));
            }
        }

        public void CargarExcel(String path)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
               
                #region VALIDACION DE EXISTENCIA DE HOJAS
                bool IsPerfilUC = false, IsPerfilComp = false, IsPerfilComp_Ciclo = false, IsPerfilComp_Actividad = false, IsPerfilTarea=false,
                    IsPerfilDetalle_Herramienta = false, IsPerfilDetalle_Repuesto = false, IsPerfilDetalle_Consumible=false;
                foreach (Microsoft.Office.Interop.Excel.Worksheet w in workBook.Worksheets)
                {
                    if (w.Name == "PerfilUC") IsPerfilUC = true;
                    else if (w.Name == "PerfilComp") IsPerfilComp = true;
                    else if (w.Name == "PerfilComp_Ciclo") IsPerfilComp_Ciclo = true;
                    else if (w.Name == "PerfilComp_Actividad") IsPerfilComp_Actividad = true;
                    else if (w.Name == "PerfilTarea") IsPerfilTarea = true;
                    else if (w.Name == "PerfilDetalle_Herramienta") IsPerfilDetalle_Herramienta = true;
                    else if (w.Name == "PerfilDetalle_Repuesto") IsPerfilDetalle_Repuesto = true;
                    else if (w.Name == "PerfilDetalle_Consumible") IsPerfilDetalle_Consumible = true;

                }
                if (!IsPerfilUC)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja PerfilUC.", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                if (!IsPerfilComp)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja PerfilComp", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                if (!IsPerfilComp_Ciclo)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja PerfilComp_Ciclo", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                if (!IsPerfilComp_Actividad)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja PerfilComp_Actividad", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                if (!IsPerfilTarea)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja PerfilTarea", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                if (!IsPerfilDetalle_Herramienta)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja PerfilDetalle_Herramienta", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                if (!IsPerfilDetalle_Repuesto)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja PerfilDetalle_Repuesto", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                if (!IsPerfilDetalle_Consumible)
                {
                    GlobalClass.ip.Mensaje("No existe la hoja PerfilDetalle_Consumible", 2);
                    lblRuta.Content = null; lblUbicacion.Visibility = System.Windows.Visibility.Collapsed; return;
                }
                #endregion
                
                #region data PerfilUC
                Microsoft.Office.Interop.Excel.Worksheet HojaPerfilUC = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilUC"];
                Microsoft.Office.Interop.Excel.Range RangePerfiluc = HojaPerfilUC.UsedRange;
                gtblPerfilUC = new DataTable();
                gtblPerfilUC.Columns.Add("IdPerfil");
                gtblPerfilUC.Columns.Add("CodPerfil");//No se toma en cuenta
                gtblPerfilUC.Columns.Add("Perfil");
                gtblPerfilUC.Columns.Add("IdTipoUnidad");
                gtblPerfilUC.Columns.Add("IdCicloDefecto");
                gtblPerfilUC.Columns.Add("IdPerfilNeumatico");//PerfilNeumatico
                gtblPerfilUC.Columns.Add("IdEstadoP"); //1,2
                gtblPerfilUC.Columns.Add("FlagActivo");
                gtblPerfilUC.Columns.Add("Nuevo");
                gtblPerfilUC.Columns.Add("Mensaje");
                gtblPerfilUC.Columns.Add("Color");
                for (int irow = 2; irow <= RangePerfiluc.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblPerfilUC.NewRow();
                    for (int icolumn = 1; icolumn <= RangePerfiluc.Columns.Count; icolumn++)
                    {
                        object Val = (RangePerfiluc.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 2] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (RangePerfiluc.Rows.Count != gtblPerfilUC.Rows.Count + 1)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en PerfilUC.", 2);
                        break;
                    }
                    else
                    {
                        dr["IdPerfil"] = gtblPerfilUC.Rows.Count + 1;
                        dr["FlagActivo"] = 1;
                        dr["Nuevo"] = 1;
                        gtblPerfilUC.Rows.Add(dr);
                    }
                }
                #endregion
                #region data Perfil Componente
                Microsoft.Office.Interop.Excel.Worksheet HojaPerfilComp = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilComp"];
                Microsoft.Office.Interop.Excel.Range RangePerfilComp = HojaPerfilComp.UsedRange;
                gtblPerfilComp = new DataTable();
                gtblPerfilComp.Columns.Add("IdPerfilComp");
                gtblPerfilComp.Columns.Add("CodPerfilComp");
                gtblPerfilComp.Columns.Add("PerfilComp");
                gtblPerfilComp.Columns.Add("IdPerfilCompPadre");//CodPerfilcompPadre
                gtblPerfilComp.Columns.Add("IdPerfil");//Perfil
                gtblPerfilComp.Columns.Add("IdTipoDetalle");
                gtblPerfilComp.Columns.Add("FlagNeumatico");
                gtblPerfilComp.Columns.Add("IdEstadoPC");
                gtblPerfilComp.Columns.Add("FlagActivo");
                gtblPerfilComp.Columns.Add("Nivel");
                gtblPerfilComp.Columns.Add("Nuevo");
                gtblPerfilComp.Columns.Add("Mensaje");
                gtblPerfilComp.Columns.Add("Color");
                for (int irow = 2; irow <= RangePerfilComp.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblPerfilComp.NewRow();
                    for (int icolumn = 1; icolumn <= RangePerfilComp.Columns.Count; icolumn++)
                    {
                        object Val = (RangePerfilComp.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        if (icolumn != 1 && Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (RangePerfilComp.Rows.Count != gtblPerfilComp.Rows.Count + 1)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en PerfilComp.", 2);
                        break;
                    }
                    else
                    {
                        dr["IdPerfilComp"] = gtblPerfilComp.Rows.Count + 1;
                        dr["FlagActivo"] = 1;
                        dr["Nuevo"] = 1;
                        gtblPerfilComp.Rows.Add(dr);
                    }
                }
                //Insetar los niveles
                foreach (DataRow f in gtblPerfilComp.Select("IdPerfilCompPadre is NULL"))
                {
                    f["Nivel"] = 2;
                    NivelarComponentes(f["CodPerfilComp"].ToString(),Convert.ToInt32( f["Nivel"]));
                }
                #endregion
                #region data Componente Ciclo
                Microsoft.Office.Interop.Excel.Worksheet hojaPCCiclo = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilComp_Ciclo"];
                Microsoft.Office.Interop.Excel.Range RengePCCiclo = hojaPCCiclo.UsedRange;
                gtblPCCiclo = new DataTable();
                gtblPCCiclo.Columns.Add("IdPerfilCompCiclo");
                gtblPCCiclo.Columns.Add("IdPerfilComp");//CodPerfilComp
                gtblPCCiclo.Columns.Add("IdCiclo");//CodCiclo
                gtblPCCiclo.Columns.Add("FrecuenciaCambio");
                gtblPCCiclo.Columns.Add("IdEstadoPCC");
                gtblPCCiclo.Columns.Add("FlagActivo");
                gtblPCCiclo.Columns.Add("Nuevo");
                gtblPCCiclo.Columns.Add("Mensaje");
                gtblPCCiclo.Columns.Add("Color");
                for (int irow = 2; irow <= RengePCCiclo.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblPCCiclo.NewRow();
                    for (int icolumn = 1; icolumn <= RengePCCiclo.Columns.Count; icolumn++)
                    {
                        object Val = (RengePCCiclo.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (RengePCCiclo.Rows.Count != gtblPCCiclo.Rows.Count + 1)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en PerfilComp_Ciclo.", 2);
                        break;
                    }
                    else
                    {
                        dr["IdPerfilCompCiclo"] = gtblPCCiclo.Rows.Count + 1;
                        dr["FlagActivo"] = 1;
                        dr["Nuevo"] = 1;
                        gtblPCCiclo.Rows.Add(dr);
                    }
                }
                #endregion
                #region data Componente Actividad
                Microsoft.Office.Interop.Excel.Worksheet HojaPCActividad = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilComp_Actividad"];
                Microsoft.Office.Interop.Excel.Range RangePCActividad = HojaPCActividad.UsedRange;
                gtblPCActividad = new DataTable();
                gtblPCActividad.Columns.Add("IdPerfilCompActividad");
                gtblPCActividad.Columns.Add("IdPerfilComp");//CodPerfilComp
                gtblPCActividad.Columns.Add("IdActividad");//CodActividad
                gtblPCActividad.Columns.Add("FlagUso");
                gtblPCActividad.Columns.Add("IsActivo");
                gtblPCActividad.Columns.Add("FlagActivo");
                gtblPCActividad.Columns.Add("Nuevo");
                gtblPCActividad.Columns.Add("Mensaje");
                gtblPCActividad.Columns.Add("Color");
                for (int irow = 2; irow <= RangePCActividad.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblPCActividad.NewRow();
                    for (int icolumn = 1; icolumn <= RangePCActividad.Columns.Count; icolumn++)
                    {
                        object Val = (RangePCActividad.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (RangePCActividad.Rows.Count != gtblPCActividad.Rows.Count + 1)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en PerfilComp_Actividad.", 2);
                        break;
                    }
                    else
                    {
                        dr["IdPerfilCompActividad"] = gtblPCActividad.Rows.Count + 1;
                        dr["FlagActivo"] = 1;
                        dr["Nuevo"] = 1;
                        gtblPCActividad.Rows.Add(dr);
                    }
                }
                //if(gtblPCActividad.Rows.Count>0)
                //    gtblPCActividad = gtblPCActividad.AsEnumerable().OrderBy(r => r["IdPerfilCompActividad"]).CopyToDataTable();
                #endregion                                
                #region data Componente Actividad Tarea
                Microsoft.Office.Interop.Excel.Worksheet hojaPCATarea = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilTarea"];
                Microsoft.Office.Interop.Excel.Range RengePCATarea = hojaPCATarea.UsedRange;
                gtblPCATarea = new DataTable();
                gtblPCATarea.Columns.Add("IdPerfilTarea");
                gtblPCATarea.Columns.Add("CodPerfilComp");//IdPerfilCompActividad luego
                gtblPCATarea.Columns.Add("Actividad");//delete luego
                gtblPCATarea.Columns.Add("IdTarea");//Tarea
                gtblPCATarea.Columns.Add("HorasHombre");
                gtblPCATarea.Columns.Add("IdEstadoPT");
                gtblPCATarea.Columns.Add("FlagActivo");
                gtblPCATarea.Columns.Add("Nuevo");
                gtblPCATarea.Columns.Add("Mensaje");
                gtblPCATarea.Columns.Add("Color");
                gtblPCATarea.Columns.Add("IdPerfilCompActividadAux");//IdPerfilCompActividad, temporalmente
                for (int irow = 2; irow <= RengePCATarea.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblPCATarea.NewRow();
                    for (int icolumn = 1; icolumn <= RengePCATarea.Columns.Count; icolumn++)
                    {
                        object Val = (RengePCATarea.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 1] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        if (RengePCATarea.Rows.Count != gtblPCATarea.Rows.Count + 1)
                            GlobalClass.ip.Mensaje("Se encontro fila vacia en PerfilTarea.", 2);
                        break;
                    }
                    else
                    {
                        if (gtblPCActividad.Select("IdPerfilComp = '" + dr["CodPerfilComp"] + "' AND IdActividad = '" + dr["Actividad"] + "'").Length > 0)
                            dr["IdPerfilCompActividadAux"] = gtblPCActividad.Select("IdPerfilComp = '" + dr["CodPerfilComp"] + "' AND IdActividad = '" + dr["Actividad"] + "'")[0]["IdPerfilCompActividad"];
                        else
                            dr["IdPerfilCompActividadAux"] = 0;
                        dr["IdPerfilTarea"] = gtblPCATarea.Rows.Count + 1;
                        dr["FlagActivo"] = 1;
                        dr["Nuevo"] = 1;
                        gtblPCATarea.Rows.Add(dr);
                    }
                }
                #endregion
                #region data Componente Actividad Detalles
                
                gtblPCADetalle = new DataTable();
                gtblPCADetalle.Columns.Add("IdPerfilDetalle");
                gtblPCADetalle.Columns.Add("IdPerfilCompActividad");
                gtblPCADetalle.Columns.Add("IdTipoArticulo");
                gtblPCADetalle.Columns.Add("CodPerfilComp"); // delete luego
                gtblPCADetalle.Columns.Add("Actividad");//delete luego
                gtblPCADetalle.Columns.Add("IdArticulo");//Articulo, Solo si es Herrmaienta cambiar a IdArticulo
                gtblPCADetalle.Columns.Add("Cantidad");
                gtblPCADetalle.Columns.Add("FlagActivo");
                gtblPCADetalle.Columns.Add("Nuevo");
                gtblPCADetalle.Columns.Add("Mensaje");
                gtblPCADetalle.Columns.Add("Color");

                #region DETALLE HERRAMIENTA ESPECIAL
                Microsoft.Office.Interop.Excel.Worksheet hojaPCADetalle1 = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilDetalle_Herramienta"];
                Microsoft.Office.Interop.Excel.Range RangePCADetalle1 = hojaPCADetalle1.UsedRange;
                for (int irow = 2; irow <= RangePCADetalle1.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblPCADetalle.NewRow();
                    for (int icolumn = 1; icolumn <= RangePCADetalle1.Columns.Count; icolumn++)
                    {
                        object Val = (RangePCADetalle1.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 3] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        break;
                    }
                    else
                    {
                        if (gtblPCActividad.Select("IdPerfilComp = '" + dr["CodPerfilComp"] + "' AND IdActividad = '" + dr["Actividad"] + "'").Length > 0)
                            dr["IdPerfilCompActividad"] = gtblPCActividad.Select("IdPerfilComp = '" + dr["CodPerfilComp"] + "' AND IdActividad = '" + dr["Actividad"] + "'")[0]["IdPerfilCompActividad"];
                        else
                            dr["IdPerfilCompActividad"] = 0;
                        dr["IdPerfilDetalle"] = gtblPCADetalle.Rows.Count + 1;
                        dr["IdTipoArticulo"] = "Herramienta"; 
                        dr["FlagActivo"] = 1;
                        dr["Nuevo"] = 1;
                        gtblPCADetalle.Rows.Add(dr);
                    }
                }
                #endregion
                #region DETALLE REPUESTO
                Microsoft.Office.Interop.Excel.Worksheet hojaPCADetalle2 = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilDetalle_Repuesto"];
                Microsoft.Office.Interop.Excel.Range RangePCADetalle2 = hojaPCADetalle2.UsedRange;
                for (int irow = 2; irow <= RangePCADetalle2.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblPCADetalle.NewRow();
                    for (int icolumn = 1; icolumn <= RangePCADetalle2.Columns.Count; icolumn++)
                    {
                        object Val = (RangePCADetalle2.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 3] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        break;
                    }
                    else
                    {
                        if (gtblPCActividad.Select("IdPerfilComp = '" + dr["CodPerfilComp"] + "' AND IdActividad = '" + dr["Actividad"] + "'").Length > 0)
                            dr["IdPerfilCompActividad"] = gtblPCActividad.Select("IdPerfilComp = '" + dr["CodPerfilComp"] + "' AND IdActividad = '" + dr["Actividad"] + "'")[0]["IdPerfilCompActividad"];
                        else
                            dr["IdPerfilCompActividad"] = 0;
                        dr["IdPerfilDetalle"] = gtblPCADetalle.Rows.Count + 1;
                        dr["IdTipoArticulo"] = "Repuesto"; 
                        dr["FlagActivo"] = 1;
                        dr["Nuevo"] = 1;
                        gtblPCADetalle.Rows.Add(dr);
                    }
                }
                #endregion
                #region DETALLE CONSUMIBLE
                Microsoft.Office.Interop.Excel.Worksheet hojaPCADetalle3 = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilDetalle_Consumible"];
                Microsoft.Office.Interop.Excel.Range RangePCADetalle3 = hojaPCADetalle3.UsedRange;
                for (int irow = 2; irow <= RangePCADetalle3.Rows.Count; irow++)
                {
                    bool IsFilaVacia = true;
                    DataRow dr = gtblPCADetalle.NewRow();
                    for (int icolumn = 1; icolumn <= RangePCADetalle3.Columns.Count; icolumn++)
                    {
                        object Val = (RangePCADetalle3.Cells[irow, icolumn] as Microsoft.Office.Interop.Excel.Range).Value2;
                        dr[(icolumn - 1) + 3] = Val;
                        if (Val != null) IsFilaVacia = false;
                    }
                    if (IsFilaVacia)
                    {
                        break;
                    }
                    else
                    {
                        if (gtblPCActividad.Select("IdPerfilComp = '" + dr["CodPerfilComp"] + "' AND IdActividad = '" + dr["Actividad"] + "'").Length > 0)
                            dr["IdPerfilCompActividad"] = gtblPCActividad.Select("IdPerfilComp = '" + dr["CodPerfilComp"] + "' AND IdActividad = '" + dr["Actividad"] + "'")[0]["IdPerfilCompActividad"];
                        else
                            dr["IdPerfilCompActividad"] = 0;
                        dr["IdPerfilDetalle"] = gtblPCADetalle.Rows.Count + 1;
                        dr["IdTipoArticulo"] = "Consumible"; 
                        dr["FlagActivo"] = 1;
                        dr["Nuevo"] = 1;
                        gtblPCADetalle.Rows.Add(dr);
                    }
                }
                #endregion
                #endregion

                #region PefilUC
                int CantErrorPFUC = 0;
                foreach (DataRow f in gtblPerfilUC.Rows)
                {
                    string msgInterno = "";
                    for (int iColumna = 2; iColumna < 7; iColumna++)// De CodPerfil a IdEstadoP
                    {
                        string NombreColumna = gtblPerfilUC.Columns[iColumna].ColumnName;
                        object Valor = f[iColumna];
                        if (Valor.ToString().Trim()=="" && NombreColumna != "IdPerfilNeumatico")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "Perfil":
                                    if ((gtblPerfilUC.Select("Perfil = '" + Valor + "'").Length > 1))
                                        msgInterno += string.Format("La Columna <{0}> admite solo valores unicos.\r\n", NombreColumna);
                                    if (gtblPerfilComp.Select("IdPerfil = '" + Valor + "'").Length == 0)
                                        msgInterno += "El Perfil de Unidad de Control debe tener Componentes.\r\n";
                                    break;
                                case "IdTipoUnidad":
                                    if (glstSAPTipoUnidad.Where(x => x.CodigoTipoUnidadControl == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    break;
                                case "IdCicloDefecto":
                                    if (glstCiclo.Where(x => x.Valor == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    else if (!(new[] { 3, 4 }.Contains(glstCiclo.Where(x => x.Valor == Valor.ToString()).First().Id)))
                                        msgInterno += "Solo esta Permitido el tipo de Ciclo 'Kilometraje' u 'Horas'.\r\n";
                                    break;
                                case "IdPerfilNeumatico":
                                    if (Valor.ToString() != "")
                                    {
                                        if (glstPerfilNeumatico.Where(x => x.Valor == Valor.ToString()).Count() == 0)
                                            msgInterno += string.Format("El Perfil Neumatico '{0}' no existe o fue anulada.\r\n", Valor);
                                        if (gtblPerfilComp.Select("FlagNeumatico = 'SI' AND IdPerfil = '" + f["Perfil"] + "'").Length != 2)
                                            msgInterno += "No se encontró componentes con FlagNeumatico = 'SI' correctamente.\r\n";
                                    }
                                    else
                                    {
                                        if (gtblPerfilComp.Select("FlagNeumatico = 'SI' AND IdPerfil = '" + f["Perfil"] + "'").Length > 0)
                                            msgInterno += "Ningun componente debe tener FlagNeumatico = 'SI'.\r\n";
                                    }
                                    break;
                                case "IdEstadoP":
                                    if (glstEstado.Where(x => x.Valor == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorPFUC += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                #region PerfilComp
                int CantErrorPFUComp = 0;
                foreach (DataRow f in gtblPerfilComp.Rows)
                {
                    bool IsPerfilCompPadre = true;//Si el valor del PerfilCompPadre es Correcto
                    bool IsCodPerfilComp = false;
                    string msgInterno = "";
                    for (int iColumna = 1; iColumna < 8; iColumna++)// De CodPerfil a IdEstadoP
                    {
                        string NombreColumna = gtblPerfilComp.Columns[iColumna].ColumnName;
                        object Valor = f[iColumna];
                        if (Valor.ToString().Trim() == "" && NombreColumna != "IdPerfilCompPadre")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "CodPerfilComp":
                                    if (!Valor.ToString().Contains("COMP"))
                                        msgInterno += "El formato de IdComponente es invalido.\r\n";
                                    else if ((gtblPerfilComp.Select("CodPerfilComp = '" + Valor + "'").Length > 1))
                                        msgInterno += string.Format("La Columna <{0}> admite solo valores unicos.\r\n", NombreColumna);
                                    else IsCodPerfilComp = true;
                                    break;
                                case "PerfilComp":
                                    if ((gtblPerfilComp.Select("PerfilComp = '" + Valor + "' AND IdPerfil = '" + f["IdPerfil"].ToString() + "' AND Nivel = '" + f["Nivel"] + "'").Length > 1))
                                        msgInterno += string.Format("La Columna <{0}> admite solo valores unicos en el mismo nivel.\r\n", NombreColumna);
                                    break;
                                case "IdPerfilCompPadre":
                                    if (Valor.ToString() != "")
                                        if (gtblPerfilComp.Select("CodPerfilComp='" + Valor + "' AND IdPerfil = '" + f["IdPerfil"].ToString() + "'").Length == 0)
                                        {
                                            IsPerfilCompPadre = false;
                                            msgInterno += string.Format("El valor '{0}' de <PerfilCompPadre> no existe en la Columna <CodPerfilComp>.\r\n", Valor);
                                        }
                                        else if (f["CodPerfilComp"] == Valor)
                                            msgInterno += string.Format("El Valor '{0}' de PerfilCompPadre no puede ser igual al valor de CodPerfilComp.\r\n", Valor);
                                        else
                                        {
                                            string CodCompGrandPa = gtblPerfilComp.Select("CodPerfilComp = '" + Valor + "'")[0]["IdPerfilCompPadre"].ToString();
                                            if (CodCompGrandPa == f["CodPerfilComp"].ToString())
                                                msgInterno = "Relacion de Componente con sub-componente invalido.\r\n";
                                        }
                                    break;
                                case "IdPerfil":
                                    if (gtblPerfilUC.Select("Perfil = '" + Valor + "'").Length == 0)
                                        msgInterno += string.Format("El Valor '{0}' de la Columna <{1}> no exitir en la hoja PerfilUC.\r\n", Valor, NombreColumna);
                                    break;
                                case "IdTipoDetalle":

                                    if (glstTipoDetalle.Where(x => x.Valor == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    else if(IsCodPerfilComp)
                                    {
                                        int IdTipoDetalle = glstTipoDetalle.Where(x => x.Valor == Valor.ToString()).First().Id;
                                        if (IdTipoDetalle == 1)//tipo Titulo
                                        {
                                            if (gtblPerfilComp.Select("IdPerfilCompPadre = '" + f["CodPerfilComp"] + "'").Length == 0)
                                                msgInterno += string.Format("El Componente '{0}' de tipo 'Título' debe tener al menos un sub-componente.\r\n", f["CodPerfilComp"]);
                                            if (gtblPCActividad.Select("IdPerfilComp = '" + f["CodPerfilComp"] + "'").Length > 0)
                                                msgInterno += "El Componente de tipo 'Título' no puede tener Actividades relacionadas.\r\n";
                                            if (gtblPCCiclo.Select("IdPerfilComp = '" + f["CodPerfilComp"] + "'").Length > 0)
                                                msgInterno += "El Componente de tipo 'Título' no puede tener Ciclos relacionados.\r\n";
                                        }
                                        else if (gtblPCCiclo.Select("IdPerfilComp = '" + f["CodPerfilComp"] + "'").Length != 2)//tipo componente
                                            msgInterno += string.Format("El Componente '{0}' debe tener 2 ciclo relacionados", f["CodPerfilComp"]);

                                    }
                                    break;
                                case "FlagNeumatico":
                                    if (glstFlag.Where(x => x.Valor == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    else
                                    {
                                        if (IsPerfilCompPadre)                                            
                                            if (Valor.ToString() == "SI")
                                                if (f["IdPerfilCompPadre"].ToString() == "")
                                                {
                                                    if (gtblPerfilComp.Select("IdPerfilCompPadre is NULL AND FlagNeumatico = 'SI' AND IdPerfil = '" + f["IdPerfil"].ToString() + "'").Length > 1)
                                                        msgInterno += "Solo un Componente con un sub-componente puede tener el 'FlagNeumatico' Activo.\r\n";

                                                    if (gtblPerfilComp.Select("IdPerfilCompPadre = '" + f["CodPerfilComp"].ToString() + "' AND IdPerfil = '" + f["IdPerfil"].ToString() + "'").Length != 1)
                                                        msgInterno += string.Format("El Componente '{0}' solo debe tener 1 Sub-Componente.\r\n", f["CodPerfilComp"]);
                                                    else
                                                    {
                                                        DataRow Fsub = gtblPerfilComp.Select("IdPerfilCompPadre = '" + f["CodPerfilComp"].ToString() + "' AND IdPerfil = '" + f["IdPerfil"].ToString() + "'")[0];
                                                        if (Fsub["FlagNeumatico"].ToString() == "NO")
                                                            msgInterno += string.Format("El Sub-Componente '{0}' debe tener el 'FlagNeumatico' Activo.\r\n", Fsub["CodPerfilComp"]);
                                                    }
                                                    if (1 != glstTipoDetalle.Where(x => x.Valor == f["IdTipoDetalle"].ToString()).First().Id)
                                                        msgInterno += "Este Componente debe ser de tipo 'Título'.\r\n";
                                                }
                                                else
                                                {
                                                    if (gtblPerfilComp.Select("CodPerfilComp = '" + f["IdPerfilCompPadre"] + "'  AND IdPerfil = '" + f["IdPerfil"].ToString() + "'")[0]["IdPerfilCompPadre"].ToString() != "")
                                                        msgInterno += "El Componente no Puede tener FlagNeumatico Activo en este nivel.\r\n";
                                                    if (gtblPerfilComp.Select("IdPerfilCompPadre = '" + f["CodPerfilComp"] + "'").Length > 0)
                                                        msgInterno += string.Format("Este sub-componete '{0}', con FlagNeumatico Activo, no puede tener sub-componentes", f["CodPerfilComp"]);
                                                    if (2 != glstTipoDetalle.Where(x => x.Valor == f["IdTipoDetalle"].ToString()).First().Id)
                                                        msgInterno += "Este Componente debe ser de tipo 'Componente'.\r\n";
                                                }
                                        
                                    }
                                    break;
                                case "IdEstadoPC":
                                    if (glstEstado.Where(x => x.Valor == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorPFUComp += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                #region PERFIL COMPONENTE CICLO
                int CantErrorPFUCompCiclo = 0;
                foreach (DataRow f in gtblPCCiclo.Rows)
                {
                    bool isIdPerfilComp = false;
                    string msgInterno = "";
                    for (int iColumna = 1; iColumna < 5; iColumna++)// De CodPerfil a IdEstadoP
                    {
                        string NombreColumna = gtblPCCiclo.Columns[iColumna].ColumnName;
                        object Valor = f[iColumna];
                        if (Valor.ToString().Trim() == "")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "IdPerfilComp":
                                    if ((gtblPerfilComp.Select("CodPerfilComp = '" + Valor + "'").Length == 0))
                                        msgInterno += string.Format("El Valor '{0}' de la Columna <{1}> no exitir en Perfilcomp.\r\n", Valor, NombreColumna);
                                    else
                                    {
                                        isIdPerfilComp = true;                                        
                                    }
                                    break;
                                case "IdCiclo":
                                    if (glstCiclo.Where(x => x.Valor == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <Ciclo>.\r\n", Valor);
                                    else
                                    {
                                        if (isIdPerfilComp)
                                            if (gtblPCCiclo.Select("IdPerfilComp = '" + f["IdPerfilComp"] + "' AND IdCiclo = '" + f["IdCiclo"] + "'").Length > 1)
                                                msgInterno += "El Ciclo es unico por cada Componente.\r\n";
                                            if (gtblPerfilComp.Select("CodPerfilComp = '" + f["IdPerfilComp"] + "'")[0]["FlagNeumatico"].ToString() == "NO")
                                            {
                                                if (!(new[] { 3, 4 }.Contains(glstCiclo.Where(x => x.Valor == Valor.ToString()).First().Id)))
                                                    msgInterno += "Componente con FlagNeumatico, Solo es permitido el tipo de Ciclo 'Kilometraje' u 'Horas'.\r\n";
                                                else if (gtblPCCiclo.Select("IdPerfilComp = '" + f["IdPerfilComp"] + "'").Length != 2)
                                                    msgInterno += "El Componente debe tener 2 ciclos, 'Kilometraje' y 'Horas'.\r\n";
                                                    
                                            }
                                            else
                                            {
                                                if (!(new[] { 1, 2 }.Contains(glstCiclo.Where(x => x.Valor == Valor.ToString()).First().Id)))
                                                    msgInterno += "Componente sin FlagNeumatico, Solo es Permitido el tipo de Ciclo 'Reencauches' o 'Tamaño de Banda'.\r\n";
                                                else if (gtblPCCiclo.Select("IdPerfilComp = '" + f["IdPerfilComp"] + "'").Length != 2)
                                                    msgInterno += "El Componente debe tener 2 ciclos, 'Reencauches' y 'Tamaño de Banda.\r\n";
                                            }
                                    }
                                    break;
                                case "FrecuenciaCambio":
                                    if (!Utilitarios.Utilitarios.IsDecimal(Valor.ToString()))
                                        msgInterno += string.Format("El valor '{0}' no es tipo Decimal.\r\n", Valor);
                                    else if (Convert.ToDecimal(Valor) < 0)
                                        msgInterno += string.Format("la Columna <{0}> no admite valores negativos.\r\n", NombreColumna);
                                    break;
                                case "IdEstadoPCC":
                                    if (glstEstado.Where(x => x.Valor == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <Estado>.\r\n", Valor);
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorPFUCompCiclo += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                #region PERFIL COMPONENTE ACTIVIDAD
                int CantErrorPFUCompActi = 0;
                //int rowindex = 0;
                foreach (DataRow f in gtblPCActividad.Rows)
                {
                    //rowindex++;
                    bool isIdPerfilComp = false;
                    string msgInterno = "";
                    for (int iColumna = 0; iColumna < 4; iColumna++)// De IdPerfilCompActividad CodPerfil a IdEstadoP
                    {
                        string NombreColumna = gtblPCActividad.Columns[iColumna].ColumnName;
                        object Valor = f[iColumna];
                        if (Valor.ToString().Trim() == "")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "IdPerfilCompActividad":
                                    //if (!Utilitarios.Utilitarios.IsNumeric(Valor.ToString()))
                                    //    msgInterno += "El valor de <IdPerfilCompActividad> solo aceptar números.\r\n";
                                    //else if (Convert.ToInt32(Valor) != rowindex)
                                    //    msgInterno += "El valor de <IdPerfilCompActividad> es invalido.\r\n";
                                    break;
                                case "IdPerfilComp":
                                    if ((gtblPerfilComp.Select("CodPerfilComp = '" + Valor + "'").Length == 0))
                                        msgInterno += string.Format("El Valor '{0}' de la Columna <{1}> no exitir en Perfilcomp.\r\n", Valor, NombreColumna);
                                    else
                                    {
                                        isIdPerfilComp = true;
                                    }
                                    break;
                                case "IdActividad":
                                    if(glstActividad.Where(x=>x.Valor==Valor.ToString()).Count()==0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    else
                                    {
                                        if (isIdPerfilComp)
                                            if (gtblPCActividad.Select("IdPerfilComp = '" + f["IdPerfilComp"] + "' AND IdActividad = '" + f["IdActividad"] + "'").Length > 1)
                                                msgInterno += "La Actividad es una unica por cada Componente.\r\n";
                                    }
                                    break;
                                case "FlagUso":
                                    if (glstFlag.Where(x => x.Valor == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    else
                                    {
                                        if (isIdPerfilComp && Valor.ToString()=="SI")
                                            if (gtblPCActividad.Select("FlagUso = 'SI' AND IdPerfilComp = '" + f["IdPerfilComp"] + "'").Length > 1)
                                                msgInterno += string.Format("Solo una Actividad del Componente '{0}' puede tener FlagUso 'SI'.\r\n", f["IdPerfilComp"]);
                                    }
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorPFUCompActi += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                #region PERFIL COMPONENTE ACTIVIDAD TAREA
                int CantErrorPFUCompActiTarea = 0;
                foreach (DataRow f in gtblPCATarea.Rows)
                {
                    //rowindex++;
                    //string Actividad = "";
                    int IdActividad = 0;
                    string msgInterno = "";
                    for (int iColumna = 1; iColumna < 6; iColumna++)// De IdPerfilCompActividad a IdEstadoPT
                    {
                        string NombreColumna = gtblPCATarea.Columns[iColumna].ColumnName;
                        object Valor = f[iColumna];
                        if (Valor.ToString().Trim() == "")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "CodPerfilComp":

                                    if ((gtblPCActividad.Select("IdPerfilComp = '" + f["CodPerfilComp"] + "' AND IdActividad = '" + f["Actividad"] + "'").Length == 0))
                                        msgInterno += string.Format("El Valor conjunto '{0} y {1}' no exitir en Perfilcomp_Actividad.\r\n", f["CodPerfilComp"], f["Actividad"]);
                                    else if (glstActividad.Where(x => x.Valor == f["Actividad"].ToString()).Count() > 0)
                                            IdActividad = glstActividad.Where(x => x.Valor == f["Actividad"].ToString()).First().Id;
                                    
                                    break;
                                case "IdTarea":
                                    if (gtblPCATarea.Select("IdPerfilCompActividadAux ='" + f["IdPerfilCompActividadAux"] + "' AND IdTarea = '" + Valor + "'").Length > 1)
                                        msgInterno += "La Tarea es unica por cada conjunto <CodPerfilComp,Actividad>.\r\n";
                                    else
                                    {
                                        if (gtblTareaByAct.Select("Actividad = '" + f["Actividad"] + "' AND Tarea = '" + Valor + "'").Length == 0)
                                            msgInterno += string.Format("La Tarea '{0}' fue anulada o no pertenece a la Actividad <{1}>.\r\n", Valor, f["Actividad"]);
                                    }
                                    break;
                                case "HorasHombre":
                                    if (!Utilitarios.Utilitarios.IsDecimal(Valor.ToString()))
                                        msgInterno += string.Format("El formato del Valor {0} de <HorasHombre> no es valido.\r\n", Valor);
                                    else if (Convert.ToDecimal(Valor) < 0)
                                        msgInterno += string.Format("la Columna <{0}> no admite valores negativos.\r\n", NombreColumna);
                                    break;
                                case "IdEstadoPT":
                                    if (glstEstado.Where(x => x.Valor == Valor.ToString()).Count() == 0)
                                        msgInterno += string.Format("El valor '{0}' esta fuera de rango de valores de la Columna <{1}>.\r\n", Valor, NombreColumna);
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorPFUCompActiTarea += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                #region PERFIL COMPONENTE ACTIVIDAD DETALLE
                int CantErrorPFUCompActiDeta = 0;
                foreach (DataRow f in gtblPCADetalle.Rows)
                {
                    //rowindex++;
                    //string Actividad = "";
                    int IdActividad = 0;
                    string msgInterno = "";
                    for (int iColumna = 3; iColumna < 7; iColumna++)// De CodPerfilComp a Cantidad
                    {
                        string NombreColumna = gtblPCADetalle.Columns[iColumna].ColumnName;
                        object Valor = f[iColumna];
                        if (Valor.ToString().Trim() == "")
                            msgInterno += string.Format("La Columna <{0}> no admite valor en nulos.\r\n", NombreColumna);
                        else
                        {
                            switch (NombreColumna)
                            {
                                case "CodPerfilComp":

                                    if (0==Convert.ToInt32( f["IdPerfilCompActividad"]))
                                        msgInterno += string.Format("El Valor conjunto '{0} y {1}' no exitir en Perfilcomp_Actividad.\r\n", f["CodPerfilComp"], f["Actividad"]);
                                    else
                                    {
                                        if (glstActividad.Where(x => x.Valor == f["Actividad"].ToString()).Count() > 0)
                                            IdActividad = glstActividad.Where(x => x.Valor == f["Actividad"].ToString()).First().Id;
                                    }
                                    break;
                                case "IdArticulo":
                                    switch (f["IdTipoArticulo"].ToString())
                                    {
                                        case "Herramienta":
                                            if (gtblHerramienta.Select("Herramienta = '" + Valor + "'").Length == 0)
                                                msgInterno += string.Format("El Valor '{0}' de <Herramienta> no existe o fue anulada.\r\n", Valor);
                                            else if (Utilitarios.Utilitarios.IsNumeric(f["Cantidad"].ToString()))
                                            {
                                                int CantMax=Convert.ToInt32( gtblHerramienta.Select("Herramienta = '" + Valor + "'")[0]["Cantidad"]);
                                                if (Convert.ToInt32(f["Cantidad"]) > CantMax)
                                                    msgInterno += string.Format("La Cantidad Maxima para la Herramienta '{0}' es '{1}'.\r\n", Valor,CantMax);
                                            }

                                            break;
                                        case "Repuesto":
                                            if(glstSAPRepuesto.Where(x=>x.CodigoArticulo==Valor.ToString()).Count()==0)
                                                msgInterno += string.Format("El Valor '{0}' esta fuera de rango de Repuestos.\r\n", Valor);
                                            break;
                                        case "Consumible":
                                            if (glstSAPConsumible.Where(x => x.CodigoArticulo == Valor.ToString()).Count() == 0)
                                                msgInterno += string.Format("El Valor '{0}' esta fuera de rango de Consumibles.\r\n", Valor);
                                            break;
                                    }
                                    if (gtblPCADetalle.Select("IdPerfilCompActividad ='" + f["IdPerfilCompActividad"] + "' AND IdArticulo = '" + Valor + "' AND IdTipoArticulo ='" + f["IdTipoArticulo"] + "'").Length > 1)
                                        msgInterno += "El Articulo es unico por cada conjunto <CodPerfilComp,Actividad>.\r\n";                                    
                                    break;
                                case "Cantidad":
                                    if (!Utilitarios.Utilitarios.IsNumeric(Valor.ToString()))
                                        msgInterno += string.Format("El formato del Valor {0} de <Cantidad> no es valido.\r\n", Valor);
                                    else if (Convert.ToDecimal(Valor) < 0)
                                        msgInterno += string.Format("la Columna <{0}> no admite valores negativos.\r\n", NombreColumna);
                                    break;
                            }
                        }
                    }
                    if (msgInterno.Length > 0) gbolExiteError = true;
                    CantErrorPFUCompActiDeta += msgInterno.Split('\n').Length - 1;
                    f["Mensaje"] = msgInterno;
                    f["Color"] = (msgInterno == "") ? "Transparent" : "Red";
                }
                #endregion
                
                dtgEncabezado.ItemsSource = gtblPerfilUC;
                dtgDetalle.ItemsSource = gtblPerfilComp;
                dtgPerfilCompActividad.ItemsSource = gtblPCActividad;
                dtgPerfilCompCiclo.ItemsSource = gtblPCCiclo;
                dtgPerfilTarea.ItemsSource = gtblPCATarea;
                dtgPerfilDetalle.ItemsSource = gtblPCADetalle;

                lblTotalErrores.Content = (CantErrorPFUC + CantErrorPFUComp + CantErrorPFUCompCiclo + CantErrorPFUCompActi + CantErrorPFUCompActiTarea + CantErrorPFUCompActiDeta).ToString();

                dtgPerfilDetalle.GroupBy("IdTipoArticulo");
                workBook.Close(false, Missing.Value, Missing.Value);
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
            DataTable tblPN = B_PerfilNeumatico.PerfilNeumatico_List(objE_PerfilNeumatico);
            tblPN.DefaultView.RowFilter = "IdEstadoPN = 1";
            glstPerfilNeumatico = CargarClases(tblPN.DefaultView.ToTable(), "IdPerfilNeumatico", "CodPerfilNeumatico","PerfilNeumatico");
            
            DataTable tblPerfiles = objB_Perfil.Perfil_List();
            //cambiamos los valores de Valor por Id
            int irow = 0;
            foreach (DataRow f in gtblPerfilUC.Rows)
            {
                irow++;                
                if (tblPerfiles.Select("Perfil = '" + f["Perfil"] + "'").Length > 0)
                {
                    dtgEncabezado.View.FocusedRowHandle = irow;
                    GlobalClass.ip.Mensaje(string.Format("El Perfil '{0}' ya existe.", f["Perfil"]), 2);
                    return;
                }
                if (f["IdPerfilNeumatico"].ToString() != "")
                {
                    if (glstPerfilNeumatico.Where(x => x.Valor == f["IdPerfilNeumatico"].ToString()).Count() == 0)
                    {
                        dtgEncabezado.View.FocusedRowHandle = irow;
                        GlobalClass.ip.Mensaje(string.Format("El Perfil Neumatico '{0}' no existe.\r\n", f["IdPerfilNeumatico"]), 2);
                        return;
                    }
                }
                else
                {
                    f["IdPerfilNeumatico"] = 0;// DBNull.Value;
                }
                f["IdCicloDefecto"] = glstCiclo.Where(l => l.Valor == f["IdCicloDefecto"].ToString()).First().Id;
                //f["IdPerfilNeumatico"] = glstPerfilNeumatico.Where(l => l.Valor == f["IdPerfilNeumatico"].ToString()).First().Id;
                f["IdEstadoP"] = glstEstado.Where(l => l.Valor == f["IdEstadoP"].ToString()).First().Id;                
            }
            irow = 0;
            foreach (DataRow f in gtblPerfilComp.Rows)
            {
                irow++;
                if (f["IdPerfilCompPadre"].ToString() != "")
                    f["IdPerfilCompPadre"] = gtblPerfilComp.Select("CodPerfilComp = '" + f["IdPerfilCompPadre"].ToString() + "'")[0]["IdPerfilComp"];
                f["IdPerfil"] = gtblPerfilUC.Select("Perfil = '" + f["IdPerfil"].ToString() + "'")[0]["IdPerfil"];
                f["IdTipoDetalle"] = glstTipoDetalle.Where(x => x.Valor == f["IdTipoDetalle"].ToString()).First().Id;
                f["FlagNeumatico"] = glstFlag.Where(x => x.Valor == f["FlagNeumatico"].ToString()).First().Id;
                f["IdEstadoPC"] = glstEstado.Where(x => x.Valor == f["IdEstadoPC"].ToString()).First().Id;
            }

            irow = 0;
            foreach (DataRow f in gtblPCCiclo.Rows)
            {
                f["IdPerfilComp"] = gtblPerfilComp.Select("CodPerfilComp = '" + f["IdPerfilComp"] + "'")[0]["IdPerfilComp"];
                f["IdCiclo"] = glstCiclo.Where(x => x.Valor == f["IdCiclo"].ToString()).First().Id;
                f["IdEstadoPCC"] = glstEstado.Where(x => x.Valor == f["IdEstadoPCC"].ToString()).First().Id;
            }

            irow = 0;
            foreach (DataRow f in gtblPCActividad.Rows)
            {
                irow++;
                f["IdPerfilComp"] = gtblPerfilComp.Select("CodPerfilComp = '" + f["IdPerfilComp"] + "'")[0]["IdPerfilComp"];
                f["IdActividad"] = glstActividad.Where(x => x.Valor == f["IdActividad"].ToString()).First().Id;
                f["FlagUso"] = glstFlag.Where(x => x.Valor == f["FlagUso"].ToString()).First().Id;
                f["IsActivo"] = 0;
                if (gtblPCATarea.Select("IdPerfilCompActividadAux = '" + f["IdPerfilCompActividad"] + "'").Length > 0)
                    f["IsActivo"] = 1;
                if(gtblPCADetalle.Select("IdPerfilCompActividad = '"+ f["IdPerfilCompActividad"] + "'").Length > 0)
                    f["IsActivo"] = 1;                
            }

            irow = 0;
            foreach (DataRow f in gtblPCATarea.Rows)
            {
                irow++;
                f["CodPerfilComp"] = f["IdPerfilCompActividadAux"];
                f["IdEstadoPT"] = glstEstado.Where(x => x.Valor == f["IdEstadoPT"].ToString()).First().Id;
                f["IdTarea"] = gtblTareaByAct.Select("Tarea = '" + f["IdTarea"] + "'")[0]["IdTarea"];
            }

            irow = 0;
            foreach (DataRow f in gtblPCADetalle.Rows)
            {
                irow++;
                switch (f["IdTipoArticulo"].ToString())
                {
                    case "Herramienta":
                        f["IdArticulo"] = gtblHerramienta.Select("Herramienta = '" + f["IdArticulo"] + "'")[0]["IdHerramienta"];
                        break;
                }
                f["IdTipoArticulo"] = glstTipoArticulo.Where(x => x.Valor == f["IdTipoArticulo"].ToString()).First().Id;
            }


            try
            {
                gtblPerfilUC.Columns.Remove("Color");
                gtblPerfilUC.Columns.Remove("Mensaje");

                gtblPerfilComp.Columns.Remove("Color");
                gtblPerfilComp.Columns.Remove("Mensaje");

                gtblPCCiclo.Columns.Remove("Color");
                gtblPCCiclo.Columns.Remove("Mensaje");

                gtblPCActividad.Columns.Remove("Color");
                gtblPCActividad.Columns.Remove("Mensaje");

                gtblPCATarea.Columns.Remove("Color");
                gtblPCATarea.Columns.Remove("Mensaje");                
                gtblPCATarea.Columns.Remove("IdPerfilCompActividadAux");
                gtblPCATarea.Columns.Remove("Actividad");

                gtblPCADetalle.Columns.Remove("CodPerfilComp");
                gtblPCADetalle.Columns.Remove("Actividad");
                gtblPCADetalle.Columns.Remove("Color");
                gtblPCADetalle.Columns.Remove("Mensaje");  

                objE_Perfil.Idusuariocreacion=Utilitarios.Utilitarios.gintIdUsuario;
                if (objB_Perfil.Perfil_CargaMasiva(objE_Perfil, gtblPerfilUC, gtblPerfilComp, gtblPCCiclo, gtblPCActividad, gtblPCATarea,gtblPCADetalle) == 0)
                {
                    GlobalClass.ip.Mensaje("Registro Masivo Satisfactorio", 1);
                    gbolExiteError = true;
                    dtgEncabezado.ItemsSource = null;
                    dtgDetalle.ItemsSource = null;
                    dtgPerfilCompActividad.ItemsSource = null;
                    dtgPerfilCompCiclo.ItemsSource = null;
                    dtgPerfilTarea.ItemsSource = null;
                    dtgPerfilDetalle.ItemsSource = null;
                    gtblPerfilUC.Dispose();
                    gtblPerfilComp.Dispose();
                    gtblPCATarea.Dispose();
                    gtblPCActividad.Dispose();
                    gtblPCATarea.Dispose();
                    gtblPCADetalle.Dispose();
                }
                else
                {
                    GlobalClass.ip.Mensaje("Registro Masivo Fallido, vuelva a cargar", 3);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            finally
            {
                //gtblPerfilUC.Rows.Clear();
                //gtblPerfilComp.Rows.Clear();
                //gtblPerfilUC.Columns.Add("Color");
                //gtblPerfilUC.Columns.Add("Mensaje");
                //gtblPerfilComp = gtblPerfilComp_Clon.Clone();
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
                    System.IO.File.WriteAllBytes(@Ruta, ExcelPerfilUC);

                    #region Seleccion de Hojas
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(@Ruta, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
                    Microsoft.Office.Interop.Excel.Worksheet _HojaCiclo = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Ciclo"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaEstado = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["EstadoP"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaFlag = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Flag"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaTipoDetalle = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["TipoDetalle"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaTipoUnidad = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["TipoUnidad"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilNeumatico = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilNeumatico"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaActividad = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Actividad"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaTarea = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Tarea"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaHerramienta = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["Herramienta"];

                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilUC = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilUC"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilComp = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilComp"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilComp_Ciclo = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilComp_Ciclo"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilComp_Actividad = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilComp_Actividad"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilTarea = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilTarea"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilDetalle_Herramienta = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilDetalle_Herramienta"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilDetalle_Repuesto = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilDetalle_Repuesto"];
                    Microsoft.Office.Interop.Excel.Worksheet _HojaPerfilDetalle_Consumible = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets["PerfilDetalle_Consumible"];
                    #endregion                                        

                    #region carga de Lista Desplegable
                    int irow = 1;
                    foreach (clsModelo cls in glstCiclo)
                    {
                        irow++;
                        _HojaCiclo.Cells[irow, 1] = cls.Id;
                        _HojaCiclo.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstEstado)
                    {
                        irow++;
                        _HojaEstado.Cells[irow, 1] = cls.Id;
                        _HojaEstado.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstFlag)
                    {
                        irow++;
                        _HojaFlag.Cells[irow, 1] = cls.Id;
                        _HojaFlag.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (DataRow f in gtblTareaByAct.Rows)
                    {
                        irow++;
                        _HojaTarea.Cells[irow, 1] = f["CodTarea"].ToString();
                        _HojaTarea.Cells[irow, 2] = "'" + f["Tarea"].ToString();
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstTipoDetalle)
                    {
                        irow++;
                        _HojaTipoDetalle.Cells[irow, 1] = cls.Id;
                        _HojaTipoDetalle.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (InterfazMTTO.iSBO_BE.BETUDUC cls in glstSAPTipoUnidad)
                    {
                        irow++;
                        _HojaTipoUnidad.Cells[irow, 1] = cls.CodigoTipoUnidadControl;
                        _HojaTipoUnidad.Cells[irow, 2] = "'" + cls.CodigoTipoUnidadControl;
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstPerfilNeumatico)
                    {
                        irow++;
                        _HojaPerfilNeumatico.Cells[irow, 1] = cls.Cod;
                        _HojaPerfilNeumatico.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (clsModelo cls in glstActividad)
                    {
                        irow++;
                        _HojaActividad.Cells[irow, 1] = cls.Cod;
                        _HojaActividad.Cells[irow, 2] = "'" + cls.Valor;
                    }
                    irow = 1;
                    foreach (DataRow f in gtblHerramienta.Rows)
                    {
                        irow++;
                        _HojaHerramienta.Cells[irow, 1] = f["CodHerramienta"].ToString();
                        _HojaHerramienta.Cells[irow, 2] = "'" + f["Herramienta"].ToString();
                    }

                    for (int i = 2; i < 100; i++)
                        _HojaPerfilComp.Cells[i, 1] = "COMP" + new string('0', 5 - (i-1).ToString().Length) + (i-1);
                    
                    #endregion

                    CargarListaDesplegable(_HojaPerfilUC, "B:B", "TipoUnidad");
                    CargarListaDesplegable(_HojaPerfilUC, "C:C", "Ciclo");
                    CargarListaDesplegable(_HojaPerfilUC, "D:D", "PerfilNeumatico");
                    CargarListaDesplegable(_HojaPerfilUC, "E:E", "EstadoP");

                    CargarListaDesplegable(_HojaPerfilComp, "C:C", "PerfilComp","A");
                    CargarListaDesplegable(_HojaPerfilComp, "E:E", "TipoDetalle");
                    CargarListaDesplegable(_HojaPerfilComp, "F:F", "Flag");
                    CargarListaDesplegable(_HojaPerfilComp, "G:G", "EstadoP");

                    CargarListaDesplegable(_HojaPerfilComp_Ciclo, "B:B", "Ciclo");
                    CargarListaDesplegable(_HojaPerfilComp_Ciclo, "D:D", "EstadoP");

                    CargarListaDesplegable(_HojaPerfilComp_Actividad, "B:B", "Actividad");
                    CargarListaDesplegable(_HojaPerfilComp_Actividad,  "C:C","Flag");

                    CargarListaDesplegable(_HojaPerfilTarea, "C:C", "Tarea");
                    CargarListaDesplegable(_HojaPerfilTarea, "E:E", "EstadoP");

                    CargarListaDesplegable(_HojaPerfilDetalle_Herramienta, "C:C", "Herramienta");
                    _HojaPerfilUC.Activate();
                    workBook.Save();
                    workBook.Close(true, mising, mising);
                    app.Quit();
 
                    #region LimpiarMemoria
                    releaseObject(app);
                    releaseObject(workBook);
                    releaseObject(_HojaCiclo);
                    releaseObject(_HojaEstado);
                    releaseObject(_HojaFlag);
                    releaseObject(_HojaTipoDetalle);
                    releaseObject(_HojaTipoUnidad);
                    releaseObject(_HojaPerfilNeumatico);
                    releaseObject(_HojaActividad);
                    releaseObject(_HojaTarea);
                    releaseObject(_HojaHerramienta);
                    releaseObject(_HojaPerfilUC);
                    releaseObject(_HojaPerfilComp);
                    releaseObject(_HojaPerfilComp_Ciclo);
                    releaseObject(_HojaPerfilComp_Actividad);
                    releaseObject(_HojaPerfilTarea);
                    releaseObject(_HojaPerfilDetalle_Herramienta);
                    releaseObject(_HojaPerfilDetalle_Repuesto);
                    releaseObject(_HojaPerfilDetalle_Consumible);                    
                    #endregion
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

        private void CargarListaDesplegable(Microsoft.Office.Interop.Excel.Worksheet _Hoja, string ColumnaDestino,string hojaDato, string ColumnaDatos)
        {
            string formula = string.Format("='{0}'!${1}$2:${1}$10000", hojaDato,ColumnaDatos);
            _Hoja.get_Range(ColumnaDestino, mising).Validation.Delete();
            _Hoja.get_Range(ColumnaDestino, mising).Validation.Add(
                Microsoft.Office.Interop.Excel.XlDVType.xlValidateList,
                Microsoft.Office.Interop.Excel.XlDVAlertStyle.xlValidAlertInformation,
                Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlBetween, formula, mising);
            _Hoja.get_Range(ColumnaDestino, mising).Validation.InCellDropdown = true;
        }
        private void CargarListaDesplegable(Microsoft.Office.Interop.Excel.Worksheet _Hoja, string ColumnaDestino, string hojaDato)
        {
            string formula = string.Format("='{0}'!$B$2:$B$10000", hojaDato);
            _Hoja.get_Range(ColumnaDestino, mising).Validation.Delete();
            _Hoja.get_Range(ColumnaDestino, mising).Validation.Add(
                Microsoft.Office.Interop.Excel.XlDVType.xlValidateList,
                Microsoft.Office.Interop.Excel.XlDVAlertStyle.xlValidAlertInformation,
                Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlBetween, formula, mising);
            _Hoja.get_Range(ColumnaDestino, mising).Validation.InCellDropdown = true;
        }

        private void PLANTILLAtabControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TabControl tc = (sender as TabControl);
            if (tc.IsVisible)
                (tc.Items[0] as TabItem).IsSelected = true;
        }

        

        
    

       
    }
}
