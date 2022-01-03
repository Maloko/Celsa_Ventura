using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Net.Mail;

namespace Utilitarios
{
    public class Utilitarios
    {
        public static string gstrUsuarioSAP;
        public static string gstrPasswordSAP;

        public static Boolean IsEML;
        public static Boolean IsDB;
        public static Boolean IsSAP;

        public static DateTime gdttFecha;
        public static DateTime gdttHora;
        public static object MDI;
        public static object Toolbar;
        public static object FormPrincipal;
        public static DataTable tblMaestra;
        public static string gstrUsuario;
        public static int gintIdRol;
        public static int gintIdUsuario;
        public static string NomUsuario;
        public static string FormatoDecimal = "#,###,###,###,###,##0.00";
        public static int gintIdNeumaticoDeMovimiento = 0;
        public static string gstrServidorSBO = "";
        public static string gstrLicenciaServidorSBO = "";
        public static string gstrBaseDatosSBO = "";
        public static IniParser parser;
        public static readonly string PasswordHash = "P@@Sw0rd";
        public static readonly string SaltKey = "S@LT&KEY";
        public static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        //Listar Combos

        public DataView ListarCombo(int IdTabla)
        {
            DataView dtv = new DataView();
            dtv = tblMaestra.DefaultView;
            dtv.RowFilter = "IdTabla=" + IdTabla.ToString();
            return dtv;
        }

        public static DataTable ListarCombo_TablaMaestra(string filtro, DataView dtv_maestra)
        {
            dtv_maestra.RowFilter = filtro;
            DataTable tbl = dtv_maestra.ToTable();
            return tbl;
        }

        public static bool IsNumeric(string s)
        {
            int intOutput;
            if (Int32.TryParse(s, out intOutput))
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public static string SoloNumeroDecimal(string letra)
        {
            if (letra != ".")
            {
                string cadena = "0123456789.";
                for (int i = 0; i < letra.Length; i++)
                {
                    string lt = letra.Substring(i, 1);
                    if (!ValidaCantPuntos(letra))
                    {
                        if (!cadena.Contains(lt))
                            letra = letra.Replace(lt, "");
                    }
                    else
                    {
                        letra = letra.Remove(letra.Length - 1);
                    }

                }
            }
            else
            {
                letra = "0.";
            }

            return letra;
        }

        public static bool ValidaCantPuntos(string cadena)
        {
            int cpuntos = 0;
            bool rpt = false;

            for (int i = 0; i < cadena.Length; i++)
            {
                if (cadena.Substring(i, 1) == ".")
                {
                    cpuntos++;
                }
            }
            if (cpuntos == 2)
            {
                rpt = true;
            }

            return rpt;
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
        public static string IIfBlankZero(string strCadena)
        {
            if (strCadena == "")
            {
                strCadena = "0";
            }
            return strCadena;
        }

        public static string IIfNullBlank(object obj)
        {
            if (obj == null)
                return "";
            else
                return obj.ToString();
        }

        public static DateTime FechaHora_Servidor()
        {
            DateTime fecha = Convert.ToDateTime(Fecha_Hora_Servidor().Rows[0]["FechaServer"].ToString() + " " + Fecha_Hora_Servidor().Rows[0]["HoraServer"].ToString());
            return fecha;
        }

        public static DataTable Fecha_Hora_Servidor()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlDataAdapter da = new SqlDataAdapter("Servidor_FechaHora", cx);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static DataTable ListarTablasDisponibles()
        {
            DataTable tbl = new DataTable();
            using (SqlConnection cx = Conexion.ObtenerConexion())
            {
                cx.Open();
                SqlDataAdapter da = new SqlDataAdapter("CargasMasivas_GetTables", cx);
                da.Fill(tbl);
                cx.Close();
            }
            return tbl;
        }

        public static string SoloDecimal(string letra)
        {
            string cadena = "0123456789.";
            for (int i = 0; i < letra.Length; i++)
            {
                string lt = letra.Substring(i, 1);
                if (!cadena.Contains(lt))
                    letra = letra.Replace(lt, "");
            }
            return letra;
        }

        public static string SoloHora(string letra)
        {
            if (letra != ":")
            {
                string cadena = "0123456789:";
                for (int i = 0; i < letra.Length; i++)
                {
                    string lt = letra.Substring(i, 1);
                    if (!ValidaCantPuntos(letra))
                    {
                        if (!cadena.Contains(lt))
                            letra = letra.Replace(lt, "");
                    }
                    else
                    {
                        letra = letra.Remove(letra.Length - 1);
                    }

                }
            }
            else
            {
                letra = "00:";
            }

            return letra;
        }

        public static string NumeroChar2(int numero)
        {
            if (numero.ToString().Length == 1)
            {
                return "0" + numero.ToString();
            }
            else if (numero.ToString().Length == 2)
            {
                return numero.ToString();
            }
            else
            {
                return numero.ToString();
            }

        }
        public static object IIfDBNull(object obj, object rpt)
        {

            if (DBNull.Value.Equals(obj))
                return rpt;
            else
                return obj;
        }

        public static object IIfBlank(string cadena, object rpt)
        {
            if (cadena.Trim() == "")
                return rpt;
            else
                return cadena;

        }

        public static bool IsDecimal(string s)
        {
            decimal decOutput;
            if (Decimal.TryParse(s, out decOutput))
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public static string SubStringSeccion(string Cadena, char Inicio, char Fin)
        {
            if (!Cadena.Contains(Inicio) || !Cadena.Contains(Fin)) return Cadena;
            int index1 = Cadena.IndexOf(Inicio) + 1;
            string cadena2 = Cadena.Substring(index1);
            return cadena2.Remove(cadena2.Length - 1);
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        /// <summary>  
        /// Compara dos DataTable y obten la diferencia de Registro, sin importar el orden 
        /// </summary>
        public static DataTable Datatable_Diferencias(DataTable FirstDataTable, DataTable SecondDataTable)
        {
            //Create Empty Table  
            DataTable ResultDataTable = new DataTable("ResultDataTable");

            //use a Dataset to make use of a DataRelation object  
            using (DataSet ds = new DataSet())
            {
                //Add tables  
                ds.Tables.AddRange(new DataTable[] { FirstDataTable.Copy(), SecondDataTable.Copy() });

                //Get Columns for DataRelation  
                DataColumn[] firstColumns = new DataColumn[ds.Tables[0].Columns.Count];
                for (int i = 0; i < firstColumns.Length; i++)
                {
                    firstColumns[i] = ds.Tables[0].Columns[i];
                }

                DataColumn[] secondColumns = new DataColumn[ds.Tables[1].Columns.Count];
                for (int i = 0; i < secondColumns.Length; i++)
                {
                    secondColumns[i] = ds.Tables[1].Columns[i];
                }

                //Create DataRelation  
                DataRelation r1 = new DataRelation(string.Empty, firstColumns, secondColumns, false);
                ds.Relations.Add(r1);

                DataRelation r2 = new DataRelation(string.Empty, secondColumns, firstColumns, false);
                ds.Relations.Add(r2);

                //Create columns for return table  
                for (int i = 0; i < FirstDataTable.Columns.Count; i++)
                {
                    ResultDataTable.Columns.Add(FirstDataTable.Columns[i].ColumnName, FirstDataTable.Columns[i].DataType);
                }

                //If FirstDataTable Row not in SecondDataTable, Add to ResultDataTable.  
                ResultDataTable.BeginLoadData();
                foreach (DataRow parentrow in ds.Tables[0].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r1);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }

                //If SecondDataTable Row not in FirstDataTable, Add to ResultDataTable.  
                foreach (DataRow parentrow in ds.Tables[1].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r2);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }
                ResultDataTable.EndLoadData();
            }

            return ResultDataTable;
        }


        public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            long rs = 0; TimeSpan diff = date2.Subtract(date1);
            switch (interval)
            {
                case DateInterval.Day:
                case DateInterval.DayOfYear:
                    rs = (long)diff.TotalDays;
                    break;
                case DateInterval.Hour:
                    rs = (long)diff.TotalHours;
                    break;
                case DateInterval.Minute:
                    rs = (long)diff.TotalMinutes;
                    break;
                case DateInterval.Month:
                    rs = (date2.Month - date1.Month) + (12 * Utilitarios.DateDiff(DateInterval.Year, date1, date2));
                    break;
                case DateInterval.Quarter:
                    rs = (long)Math.Ceiling((double)(Utilitarios.DateDiff(DateInterval.Month, date1, date2) / 3.0));
                    break;
                case DateInterval.Second:
                    rs = (long)diff.TotalSeconds;
                    break;
                case DateInterval.Weekday:
                case DateInterval.WeekOfYear:
                    rs = (long)(diff.TotalDays / 7);
                    break;
                case DateInterval.Year:
                    rs = date2.Year - date1.Year;
                    break;
            }//switch   
            return rs;
        }//DateDiff


        public enum DateInterval
        {
            Day,
            DayOfYear,
            Hour,
            Minute,
            Month,
            Quarter,
            Second,
            Weekday,
            WeekOfYear,
            Year
        }
    }
}

