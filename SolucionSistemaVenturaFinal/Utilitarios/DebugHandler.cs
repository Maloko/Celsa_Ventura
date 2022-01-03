using System;
using System.Xml;
using System.IO;
using System.Data;
using System.Configuration;

namespace Utilitarios
{
    public class DebugHandler
    {
        string NombreArchivo;
        string Ruta = ConfigurationManager.AppSettings["Ruta"];
        int FlagDebug = Convert.ToInt32(ConfigurationManager.AppSettings["FlagDebug"]);
        DataTable tbl = new DataTable();
        public void EscribirDebug(string Metodo,string Parametros)
        {
            Utilitarios objUtil = new Utilitarios();
            if (FlagDebug == 0) 
            {
                return;
            }

            NombreArchivo = ConfigurationManager.AppSettings["NombreDebug"];
            NombreArchivo = NombreArchivo.Substring(0, NombreArchivo.Length - 4) + "_" + DateTime.Now.Year.ToString() + 
                (100 + DateTime.Now.Month).ToString().Substring(1) +
                (100 + DateTime.Now.Day).ToString().Substring(1)  +".XML"; //Se añade la fecha al nombre del archivo

            ValidarArchivo();
            tbl = new DataTable();
            try
            {
                tbl.ReadXml(Ruta + NombreArchivo);
            }
            catch
            {
                XmlDocument ErrorXML = new XmlDocument();
                ErrorXML.LoadXml(Esquema);
                ErrorXML.Save(Ruta + NombreArchivo);
                tbl.ReadXml(Ruta + NombreArchivo);
            }

            DataRow row;
            row = tbl.NewRow();
            row["Metodo"] = Metodo;
            row["Parametros"] = Parametros;
            row["FechaHora"] = DateTime.Now;
            row["NombreUsuario"] = objUtil.NullableTrim(Utilitarios.gstrUsuario);
            row["HostName"] = System.Environment.MachineName;
            row["IPAddress"] = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1];
            tbl.Rows.Add(row);
            tbl.TableName = "Table1";
            tbl.WriteXml(Ruta + NombreArchivo, XmlWriteMode.WriteSchema, false);
        }

        void ValidarArchivo()
        {
            string RutaCompleta = System.IO.Path.Combine(Ruta, NombreArchivo);

            if (!System.IO.File.Exists(RutaCompleta))
            {
                FileStream fs = new FileStream(RutaCompleta, FileMode.OpenOrCreate);
                StreamWriter str = new StreamWriter(fs);
                str.Flush();
                str.Close();
                fs.Close();
            }
        }

        string Esquema = "<?xml version='1.0' standalone='yes'?>" +
                                "<NewDataSet>" +
                                 " <xs:schema id='NewDataSet' xmlns='' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:msdata='urn:schemas-microsoft-com:xml-msdata'>" +
                                 "   <xs:element name='NewDataSet' msdata:IsDataSet='true' msdata:MainDataTable='Table1' msdata:UseCurrentLocale='true'>" +
                                 "     <xs:complexType>" +
                                 "       <xs:choice minOccurs='0' maxOccurs='unbounded'>" +
                                 "         <xs:element name='Table1'>" +
                                 "           <xs:complexType>" +
                                 "             <xs:sequence>" +
                                 "               <xs:element name='Metodo' type='xs:string' minOccurs='0' />" +
                                 "               <xs:element name='Parametros' type='xs:string' minOccurs='0' />" +
                                 "               <xs:element name='FechaHora' type='xs:dateTime' minOccurs='0' />" +
                                 "               <xs:element name='NombreUsuario' type='xs:string' minOccurs='0' />" +
                                 "               <xs:element name='HostName' type='xs:string' minOccurs='0' />" +
                                 "               <xs:element name='IPAddress' type='xs:string' minOccurs='0' />" +
                                 "             </xs:sequence>" +
                                 "           </xs:complexType>" +
                                 "         </xs:element>" +
                                 "       </xs:choice>" +
                                 "     </xs:complexType>" +
                                 "   </xs:element>" +
                                 " </xs:schema>" +
                                 " </NewDataSet>";
    }


}
