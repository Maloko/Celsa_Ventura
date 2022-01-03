using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Xsl;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Drawing;
using System.Configuration;

namespace Utilitarios
{
    public class Utilitarios
    {
        public static string curTempFileName;
        public static Boolean ComprimirZip;
        public static string gstrUsuario;
        public static Boolean IsNewLoad;

        public static int SegundosPorTipo;
        public static int gintMaxRegistros;
        public static double gintTiempoEnvio;
        public static double gintTiempoReenvio;
        public static string gstrSubject;
        public static Boolean IsZip;

        public static byte[] ConvertDataTableToByteArray(DataSet tblOriginal)
        {
            byte[] binaryDataResult = null;
            MemoryStream memStream = new MemoryStream();
            IFormatter brFormatter = new BinaryFormatter();

            tblOriginal.RemotingFormat = SerializationFormat.Binary;
            brFormatter.Serialize(memStream, tblOriginal);
            binaryDataResult = memStream.ToArray();

            memStream.Close();
            memStream.Dispose();
            return binaryDataResult;
        }

        public static DataSet ConvertByteArrayToDataTable(byte[] ByteDs)
        {
            DataSet tempDataTable = new DataSet();
            using (MemoryStream stream = new MemoryStream(ByteDs))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                tempDataTable = (DataSet)bformatter.Deserialize(stream);
            }
            return tempDataTable;
        }

        public static byte[] ConvertDataSetToExcelAndReturnByte(DataSet ds)
        {
            Excel.Range rango;
            Excel.Application excel = new Excel.Application();
            Excel.Workbook libro = null;
            Excel.Worksheet hoja = null;
            object misValue = System.Reflection.Missing.Value;

            libro = excel.Workbooks.Add(misValue);

            for (int dset = 0; dset < ds.Tables.Count; dset++)
            {
                int Filas = 0;
                int CantFilas = 0;
                hoja = (Excel.Worksheet)libro.Worksheets.Add();
                hoja.Name = (ds.Tables[dset].TableName.Length > 30) ? SoloAlfanumerico(ds.Tables[dset].TableName.Substring(0, 30)) : SoloAlfanumerico(ds.Tables[dset].TableName);

                //Se Ingresa la Cabeceras de la Hoja
                CrearCabeceras(ds.Tables[dset], ref hoja, ref Filas);

                //Insertamos los datos
                for (int i = 0; i < ds.Tables[dset].Rows.Count; i++)
                {
                    for (int j = 2; j < ds.Tables[dset].Columns.Count; j++)
                    {
                        hoja.Cells[Filas + i, j + 1] = ds.Tables[dset].Rows[i][j].ToString();
                    }
                    CantFilas = ds.Tables[dset].Rows.Count;
                    hoja.Range[hoja.Cells[Filas + i, 2], hoja.Cells[CantFilas, ds.Tables[dset].Columns.Count]].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
                rango = hoja.Rows[CantFilas + Filas];
                rango.EntireColumn.AutoFit();
            }
            ((Excel.Worksheet)excel.ActiveWorkbook.Sheets["Hoja1"]).Delete();   //Borramos la hoja que crea en el libro por defecto
            libro.SaveAs(curTempFileName + "\\Alertas.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            libro.Close(true, misValue, misValue);
            excel.Quit();
            releaseObject(excel);
            releaseObject(libro);
            releaseObject(hoja);
            MemoryStream ms = new MemoryStream();
            
            using (FileStream file = new FileStream(curTempFileName + "\\Alertas.xls", FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);
                ms.Close();
                ms.Dispose();
                file.Close();
                file.Dispose();
                File.Delete(file.Name);

                Process[] Processes;
                Processes = System.Diagnostics.
                Process.GetProcessesByName("EXCEL");
                foreach (Process p in Processes) { if (p.MainWindowTitle.Trim() == "") p.Kill(); }
                return bytes;
            }
        }

        public static void CrearCabeceras(DataTable tblAlertas, ref Excel.Worksheet hoja, ref int CantFilas)
        {
            Excel.Range rango;
            CantFilas = 2;
            //** Montamos el título en la línea 1 **
            hoja.Cells[CantFilas, 2] = tblAlertas.TableName;
            hoja.Range[hoja.Cells[CantFilas, 2], hoja.Cells[CantFilas, tblAlertas.Columns.Count]].Merge();
            hoja.Range[hoja.Cells[CantFilas, 2], hoja.Cells[CantFilas, tblAlertas.Columns.Count]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            hoja.Range[hoja.Cells[CantFilas, 2], hoja.Cells[CantFilas, tblAlertas.Columns.Count]].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            hoja.Range[hoja.Cells[CantFilas, 2], hoja.Cells[CantFilas, tblAlertas.Columns.Count]].Interior.ColorIndex = 33;
            CantFilas++;

            for (int j = 2; j < tblAlertas.Columns.Count; j++)
            {
                //** Montamos las cabeceras en la línea 2 **
                hoja.Cells[CantFilas, j + 1] = tblAlertas.Columns[j].ColumnName;
            }

            //Centramos los textos
            rango = hoja.Rows[CantFilas];
            hoja.Range[hoja.Cells[CantFilas, 2], hoja.Cells[CantFilas, tblAlertas.Columns.Count]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            hoja.Range[hoja.Cells[CantFilas, 2], hoja.Cells[CantFilas, tblAlertas.Columns.Count]].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            hoja.Range[hoja.Cells[CantFilas, 2], hoja.Cells[CantFilas, tblAlertas.Columns.Count]].Interior.ColorIndex = 34;
            rango.AutoFit();
            CantFilas++;
        }

        public static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception) { obj = null; }
            finally { GC.Collect(); }
        }

        public static string GetHtmlTable(DataSet tblAlertas, DataSet tblAlertasAdjuntas)
        {
            string CuerpoTotalCorreo = "";

            for (int dscoun = 0; dscoun < tblAlertas.Tables.Count; dscoun++)
            {
                string messageBody = "<font style=\"text-shadow: 2px 2px 1px #F2F2F2; font-size: " + ConfigurationManager.AppSettings["FontSize"] + "px; font-family: " + ConfigurationManager.AppSettings["TipoLetraAlerta"] + "; color: #000;\">" + tblAlertas.Tables[dscoun].TableName + ":</font><br><br>";
                string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center; box-shadow: 5px 5px 10px #888888;\" >";
                string htmlTableEnd = "</table>";
                //string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
                string htmlHeaderRowStart = "<tr style=\"background-color:" + ConfigurationManager.AppSettings["ColorHeader"] + "; color:" + ConfigurationManager.AppSettings["ColorTextoHeader"] + "; font-size: " + ConfigurationManager.AppSettings["FontSizeTabla"] + "px; font-family: " + ConfigurationManager.AppSettings["TipoLetraTabla"] + "; font-weight: " + ConfigurationManager.AppSettings["FontWeightTabla"] + ";\">";
                string htmlHeaderRowEnd = "</tr>";
                string htmlTrStart = "<tr style=\"color:" + ConfigurationManager.AppSettings["ColorTextoFilas"] + "; font-size: " + ConfigurationManager.AppSettings["FontSizeTabla"] + "px; font-family: " + ConfigurationManager.AppSettings["TipoLetraTabla"] + "; font-weight: " + ConfigurationManager.AppSettings["FontWeightTabla"] + ";\">";
                string htmlTrEnd = "</tr>";
                string htmlTdStart = "<td style=\" border-color:" + ConfigurationManager.AppSettings["ColorBordes"] + "; border-style:solid; border-width:thin; padding: 5px;\">";
                string htmlTdEnd = "</td>";

                messageBody += htmlTableStart;
                messageBody += htmlHeaderRowStart;
                for (int i = 2; i < tblAlertas.Tables[dscoun].Columns.Count; i++)
                {
                    messageBody += htmlTdStart + tblAlertas.Tables[dscoun].Columns[i].ColumnName + htmlTdEnd;
                }

                messageBody += htmlHeaderRowEnd;
                foreach (DataRow Row in tblAlertas.Tables[dscoun].Rows)
                {
                    messageBody = messageBody + htmlTrStart;
                    for (int i = 2; i < Row.ItemArray.Length; i++)
                    {
                        messageBody = messageBody + htmlTdStart + Row[i] + htmlTdEnd;
                    }
                    messageBody = messageBody + htmlTrEnd;
                }
                messageBody = messageBody + htmlTableEnd;

                CuerpoTotalCorreo += messageBody + "<br><br>";

            }
            if (tblAlertas.Tables.Count == 0)
            {
                string messageBody = "<font style=\"text-shadow: 2px 2px 1px #F2F2F2; font-size: " + ConfigurationManager.AppSettings["FontSize"] + "px; font-family: " + ConfigurationManager.AppSettings["TipoLetraAlerta"] + "; color: #000;\"> Se adjuntó la " + tblAlertasAdjuntas.Tables[0].TableName + "</font><br><br>";
                CuerpoTotalCorreo = messageBody;
                //tblAlertasAdjuntas
            }
            return CuerpoTotalCorreo;
        }

        public static string SoloAlfanumerico(string letra)
        {
            string cadena = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789áéíóú, ";
            for (int i = 0; i < letra.Length; i++)
            {
                string lt = letra.Substring(i, 1);
                if (!cadena.Contains(lt))
                    letra = letra.Replace(lt, "");
            }
            return letra;
        }
        public string NullableTrim(string s)
        {
            if (s == null)
            {
                return "";
            }
            else
            {
                return s.Trim();
            }
        }
        public static string ConvertirRGB(Color Color)
        {
            string RGB = "#";

            byte[] rgbcolor = { Color.R, Color.G, Color.B };
            foreach (var c in rgbcolor)
            {
                string cr = c.ToString("X");
                if (cr.Length == 1)
                {
                    cr = "0" + cr;
                }
                RGB += cr;
            }
            return RGB;
        }
    }
}
