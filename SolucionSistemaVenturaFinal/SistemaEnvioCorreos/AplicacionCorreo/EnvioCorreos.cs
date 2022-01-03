using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Entities;
using Business;
using Utilitarios;
using System.IO;
using System.Configuration;

namespace AplicacionCorreo
{
    public partial class EnvioCorreos : Form
    {
        BackgroundWorker HiloEnvio = new BackgroundWorker();
        BackgroundWorker HiloRenvio = new BackgroundWorker();

        B_Alertas objB_Alertas = new B_Alertas();
        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        E_TablaMaestra E_TablaMaestra = new E_TablaMaestra();
        E_Alertas E_Alertas = new E_Alertas();
        Utilitarios.ErrorHandler Error = new ErrorHandler();

        int gintIdUsuario = 1;
        //int gintMaxRegistros = 0;       //Seteado Desde la Tabla de Parametros
        //double gintTiempoEnvio = 0;     //SeteadoDesde la Tabla de Parametros
        //double gintTiempoReenvio = 0;   //Seteado Desde la Tabla de Parametros
        //string gstrSubject = "";        //Seteado Desde la Tabla de Parametros
        //Boolean IsZip = false;          //Seteado Desde la Tabla de Parametros
        Boolean ErrorEnvio = false;
        Boolean ErrorReenvio = false;
        Boolean ForceExit = false;

        string gstrAlertaEnviada = "";
        string gstrEmailDestinos = "";
        string gstrIdUsuariosDestino = "";
        string gstrErrorEnvio = "";
        string gstrErrorReenvio = "";
        string gstrTipoFiltro = "";

        string gstrSMTPHost = "";
        int gintSMTPPort = 0;
        string gstrSMTPEmail = "";
        string gstrSMTPPassword = "";

        string gstrNombreArchivoAdjunto = "";

        DataSet Alertas = new DataSet();
        DataTable tblAlertasLog = new DataTable();
        DataTable tblReenvioAlertas = new DataTable();
        DataTable tblAlertasApp = new DataTable();
        DataTable tblUsuarios = new DataTable();
        int gintIndexRow = 1;
        int gintIndexRowApp = 1;
        int gintSegundosTimerEnvio = 0;
        int gintSegundosTimerReenvio = 0;
        byte[] gbyteDetalleAlerta;
        System.Net.Sockets.TcpListener tcpListener;

        public EnvioCorreos()
        {
            tcpListener = new System.Net.Sockets.TcpListener(IPAddress.Parse("127.0.0.1"), 55950);
            tcpListener.Start();
            InitializeComponent();
        }

        private void EnvioCorreos_Load(object sender, EventArgs e)
        {
            try
            {
                timer.Enabled = false;
                NTIcon.Visible = false;
                Utilitarios.Utilitarios.IsNewLoad = false;
                if (ConfigurationManager.AppSettings["IsNew"].ToString() == "0")
                {
                    Utilitarios.Utilitarios.IsNewLoad = true;
                    Configuracion config = new Configuracion();
                    config.ShowDialog();
                }

                gstrSMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"].ToString();
                gstrSMTPPassword = ConfigurationManager.AppSettings["SMTPPassword"].ToString();
                gstrSMTPHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                gintSMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);

                gstrNombreArchivoAdjunto = ConfigurationManager.AppSettings["NombreArchivoAdjunto"].ToString();

                tblAlertasLog.Columns.Add("IdAlertaEnvioLog", Type.GetType("System.Int32"));
                tblAlertasLog.Columns.Add("NombreAlerta", Type.GetType("System.String"));
                tblAlertasLog.Columns.Add("Correos", Type.GetType("System.String"));
                tblAlertasLog.Columns.Add("Archivo", Type.GetType("System.Byte[]"));
                tblAlertasLog.Columns.Add("Subject", Type.GetType("System.String"));
                tblAlertasLog.Columns.Add("html", Type.GetType("System.String"));
                tblAlertasLog.Columns.Add("FlagArchivo", Type.GetType("System.Boolean"));
                tblAlertasLog.Columns.Add("FlagEnvio", Type.GetType("System.Boolean"));
                tblAlertasLog.Columns.Add("FlagReenvio", Type.GetType("System.Boolean"));
                tblAlertasLog.Columns.Add("CantReenvio", Type.GetType("System.Int32"));
                tblAlertasLog.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                tblAlertasApp.Columns.Add("IdAlerta", Type.GetType("System.Int32"));
                tblAlertasApp.Columns.Add("Alerta", Type.GetType("System.String"));
                tblAlertasApp.Columns.Add("IdUsuario", Type.GetType("System.Int32"));
                tblAlertasApp.Columns.Add("FechaAlerta", Type.GetType("System.DateTime"));
                tblAlertasApp.Columns.Add("DetalleAlerta", Type.GetType("System.Byte[]"));
                tblAlertasApp.Columns.Add("FlagLeido", Type.GetType("System.Boolean"));
                tblAlertasApp.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblAlertasApp.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                //Declarando Hilos para el envío
                HiloEnvio.WorkerReportsProgress = true;
                HiloEnvio.WorkerSupportsCancellation = true;

                HiloEnvio.DoWork += new DoWorkEventHandler(HiloEnvio_DoWork);
                HiloEnvio.RunWorkerCompleted += new RunWorkerCompletedEventHandler(HiloEnvio_RunWorkerCompleted);
                HiloEnvio.ProgressChanged += new ProgressChangedEventHandler(HiloEnvio_ProgressChanged);

                //Declarando Hilos para el reenvío
                HiloRenvio.WorkerReportsProgress = true;
                HiloRenvio.WorkerSupportsCancellation = true;

                HiloRenvio.DoWork += new DoWorkEventHandler(HiloRenvio_DoWork);
                HiloRenvio.RunWorkerCompleted += new RunWorkerCompletedEventHandler(HiloRenvio_RunWorkerCompleted);
                HiloRenvio.ProgressChanged += new ProgressChangedEventHandler(HiloRenvio_ProgressChanged);

                E_TablaMaestra.IdTabla = 1000;
                gstrTipoFiltro = objB_TablaMaestra.TablaMaestra_Combo(E_TablaMaestra).Select("IdColumna = 9")[0]["Valor"].ToString();

                CargarDatosMaestros();
                EnviarAlertas();
                ReenviarAlertas();
                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public void CargarDatosMaestros()
        {
            try
            {
                E_TablaMaestra.IdTabla = 1000;
                int TipoFrecuencia = Convert.ToInt32(objB_TablaMaestra.TablaMaestra_Combo(E_TablaMaestra).Select("IdColumna = 10")[0]["Valor"]);

                E_TablaMaestra.IdTabla = 61;
                Utilitarios.Utilitarios.SegundosPorTipo = Convert.ToInt32(objB_TablaMaestra.TablaMaestra_Combo(E_TablaMaestra).Select("IdColumna = " + TipoFrecuencia)[0]["Valor"]);

                E_TablaMaestra.IdTabla = 60;
                Utilitarios.Utilitarios.gintMaxRegistros = Convert.ToInt32(objB_TablaMaestra.TablaMaestra_Combo(E_TablaMaestra).Select("IdColumna = 4")[0]["Valor"]);
                Utilitarios.Utilitarios.gintTiempoEnvio = Convert.ToDouble(objB_TablaMaestra.TablaMaestra_Combo(E_TablaMaestra).Select("IdColumna = 1")[0]["Valor"]);
                Utilitarios.Utilitarios.gintTiempoReenvio = Convert.ToDouble(objB_TablaMaestra.TablaMaestra_Combo(E_TablaMaestra).Select("IdColumna = 2")[0]["Valor"]);
                Utilitarios.Utilitarios.gstrSubject = objB_TablaMaestra.TablaMaestra_Combo(E_TablaMaestra).Select("IdColumna = 6")[0]["Valor"].ToString();
                Utilitarios.Utilitarios.IsZip = Convert.ToBoolean(Convert.ToInt32(objB_TablaMaestra.TablaMaestra_Combo(E_TablaMaestra).Select("IdColumna = 5")[0]["Valor"]));
                Utilitarios.Utilitarios.ComprimirZip = Utilitarios.Utilitarios.IsZip;
                Utilitarios.Utilitarios.gintTiempoEnvio = Utilitarios.Utilitarios.gintTiempoEnvio * Utilitarios.Utilitarios.SegundosPorTipo;
                Utilitarios.Utilitarios.gintTiempoReenvio = Utilitarios.Utilitarios.gintTiempoReenvio * Utilitarios.Utilitarios.SegundosPorTipo;
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        public void EnviarAlertas()
        {
            try
            {
                Alertas = new DataSet();
                Alertas = objB_Alertas.Alerta_Call();
                Boolean ExistenAlertas = false;

                for (int i = 0; i < Alertas.Tables.Count; i++)
                {
                    if (Alertas.Tables[i].Rows.Count > 0)
                    {
                        ExistenAlertas = true;
                        break;
                    }
                }

                if (ExistenAlertas)
                {
                    tblUsuarios = objB_Alertas.Usuario_ListByFilterType();
                    gstrEmailDestinos = "";
                    gstrIdUsuariosDestino = "";
                    foreach (DataRow drUser in tblUsuarios.Select())
                    {
                        gstrEmailDestinos += drUser["Email"].ToString() + ";";
                        gstrIdUsuariosDestino += drUser["IdUsuario"].ToString() + "|";
                    }

                    if (gstrEmailDestinos.Length > 0)
                    {
                        if (HiloEnvio.IsBusy != true)
                        {
                            HiloEnvio.RunWorkerAsync();
                        }
                    }
                    else
                    {
                        lstLogCorreos.Items.Add(String.Format("[{0}] No hay Email(s) Disponibles para realizar el envío", DateTime.Now));
                    }
                }
                else
                {
                    lstLogCorreos.Items.Add(String.Format("[{0}] No hay alertas ", DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public void ReenviarAlertas()
        {
            try
            {
                tblReenvioAlertas = new DataTable();
                tblReenvioAlertas = objB_Alertas.Alertas_Envio_Log_GetReenvios();
                tblReenvioAlertas.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                for (int i = 0; i < tblReenvioAlertas.Rows.Count; i++)
                {
                    tblReenvioAlertas.Rows[i]["Nuevo"] = false;
                }
                if (tblReenvioAlertas.Rows.Count > 0)
                {
                    if (HiloRenvio.IsBusy != true)
                    {
                        HiloRenvio.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public string FormatMultipleEmailAddresses(string emailAddresses)
        {
            var delimiters = new[] { ',', ';' };
            var addresses = emailAddresses.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(",", addresses);
        }

        public void SendAutomatedEmail(string htmlString, DataSet ds)
        {
            DataRow dr = tblAlertasLog.NewRow();
            dr["IdAlertaEnvioLog"] = gintIndexRow;
            dr["NombreAlerta"] = gstrAlertaEnviada;
            dr["Correos"] = gstrEmailDestinos.Substring(0, gstrEmailDestinos.Length - 1);
            dr["Subject"] = Utilitarios.Utilitarios.gstrSubject;
            dr["html"] = htmlString;

            Utilitarios.Utilitarios.curTempFileName = Application.StartupPath;
            MailMessage message = new MailMessage();
            message.From = new MailAddress(gstrSMTPEmail);
            message.To.Add(FormatMultipleEmailAddresses(gstrEmailDestinos));
            message.IsBodyHtml = true;
            message.Body = htmlString;

            if (ds.Tables.Count > 0)
            {
                dr["Archivo"] = Utilitarios.Utilitarios.ConvertDataSetToExcelAndReturnByte(ds);
                dr["FlagArchivo"] = true;
                Attachment Attchmnt = new Attachment(new MemoryStream(Utilitarios.Utilitarios.ConvertDataSetToExcelAndReturnByte(ds)), gstrNombreArchivoAdjunto + ".xls");
                message.Attachments.Add(Attchmnt);
            }
            else
            {
                dr["FlagArchivo"] = false;
            }

            message.Subject = Utilitarios.Utilitarios.gstrSubject;

            //Set up SMTP client
            SmtpClient client = new SmtpClient();
            client.Host = gstrSMTPHost;
            client.Port = gintSMTPPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(gstrSMTPEmail, gstrSMTPPassword);
            client.EnableSsl = true;
            dr["CantReenvio"] = 0;
            dr["Nuevo"] = true;
            tblAlertasLog.Rows.Add(dr);
            gintIndexRow++;
            client.Send(message);
        }

        public void ReSendAutomatedEmail(string EmailDestinos, string Subject, string htmlString, byte[] Archivo)
        {
            Utilitarios.Utilitarios.curTempFileName = Application.StartupPath;
            MailMessage message = new MailMessage();
            message.From = new MailAddress(gstrSMTPEmail);
            message.To.Add(FormatMultipleEmailAddresses(EmailDestinos));
            message.IsBodyHtml = true;
            message.Body = htmlString;

            if (Archivo != null)
            {
                Attachment Attchmnt = new Attachment(new MemoryStream(Archivo), gstrNombreArchivoAdjunto + ".xls");
                message.Attachments.Add(Attchmnt);
            }

            message.Subject = Subject;

            //Set up SMTP client
            SmtpClient client = new SmtpClient();
            client.Host = gstrSMTPHost;
            client.Port = gintSMTPPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(gstrSMTPEmail, gstrSMTPPassword);
            client.EnableSsl = true;
            client.Send(message);
        }

        public void HiloEnvio_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet AlertasCorreo = new DataSet();
            DataSet AlertasAdjuntas = new DataSet();
            for (int i = 0; i < Alertas.Tables.Count; i++)
            {
                AlertasCorreo = new DataSet();
                AlertasAdjuntas = new DataSet();
                DataRow[] Fila = Alertas.Tables[i].AsEnumerable().Take(1).ToArray();
                if (Alertas.Tables[i].Rows.Count > 0 && Alertas.Tables[i].Rows.Count <= Utilitarios.Utilitarios.gintMaxRegistros)
                {
                    Alertas.Tables[i].TableName = Fila[0]["NombreAlerta"].ToString();
                    AlertasCorreo.Tables.Add(Alertas.Tables[i].Copy());
                }
                else if (Alertas.Tables[i].Rows.Count > Utilitarios.Utilitarios.gintMaxRegistros)
                {
                    Alertas.Tables[i].TableName = Fila[0]["NombreAlerta"].ToString();
                    AlertasAdjuntas.Tables.Add(Alertas.Tables[i].Copy());
                }


                if (Alertas.Tables[i].Rows.Count > 0)
                {
                    DataSet dsAlertasAPP = new DataSet();
                    dsAlertasAPP.Tables.Add(Alertas.Tables[i].Copy());
                    gbyteDetalleAlerta = null;
                    gbyteDetalleAlerta = Utilitarios.Utilitarios.ConvertDataTableToByteArray(dsAlertasAPP);

                    if (gstrTipoFiltro == "F")
                    {
                        int IdMenu = Convert.ToInt32(Alertas.Tables[i].Rows[0]["IdMenu"]);
                        gstrEmailDestinos = "";
                        gstrIdUsuariosDestino = "";
                        foreach (DataRow drUsuarios in tblUsuarios.Select("IdMenu =" + IdMenu))
                        {
                            gstrEmailDestinos += drUsuarios["Email"].ToString() + ";";
                            gstrIdUsuariosDestino += drUsuarios["IdUsuario"].ToString() + "|";
                        }
                    }

                    string[] Usuarios = gstrIdUsuariosDestino.Substring(0, gstrIdUsuariosDestino.Length - 1).Split('|');
                    foreach (string strUser in Usuarios)
                    {
                        DataRow dr = tblAlertasApp.NewRow();
                        dr["IdAlerta"] = gintIndexRowApp;
                        dr["Alerta"] = Alertas.Tables[i].TableName;
                        dr["IdUsuario"] = Convert.ToInt32(strUser);
                        dr["FechaAlerta"] = DateTime.Now;
                        dr["DetalleAlerta"] = gbyteDetalleAlerta;
                        dr["FlagActivo"] = true;
                        dr["FlagLeido"] = false;
                        dr["Nuevo"] = true;
                        tblAlertasApp.Rows.Add(dr);
                        gintIndexRowApp++;
                    }

                    gstrAlertaEnviada = Fila[0]["NombreAlerta"].ToString();
                    string htmlString = Utilitarios.Utilitarios.GetHtmlTable(AlertasCorreo, AlertasAdjuntas);
                    try
                    {
                        SendAutomatedEmail(htmlString, AlertasAdjuntas);
                    }
                    catch (Exception ex)
                    {
                        gstrErrorEnvio = ex.Message;
                        ErrorEnvio = true;
                    }

                    HiloEnvio.ReportProgress(gintIndexRow - 1);
                }
            }
        }

        public void HiloEnvio_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))
            {
                tsMsg.ForeColor = Color.Red;
                tsMsg.Text = e.Error.Message;
            }
            else
            {
                tsMsg.Text = String.Format("Último envío: {0}", DateTime.Now);
                E_Alertas.IdUsuarioCreacion = gintIdUsuario;
                try
                {
                    DataTable tbl = tblAlertasApp;
                    try
                    {
                        int rpta = objB_Alertas.Alertas_Envio_Log_UpdateCascade(E_Alertas, tblAlertasLog);
                        if (rpta == 0)
                        {
                            lstLogCorreos.Items.Add(String.Format("[{0}][Envío] Se guardaron los Logs en la Base de Datos satisfactoriamente", DateTime.Now));
                        }
                    }
                    catch (Exception ex)
                    {
                        lstLogCorreos.Items.Add(String.Format("[{0}][Envío] Error al guardar los Logs de alertas en la Base de Datos", DateTime.Now));
                        Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                    }

                    try
                    {
                        int rpta2 = objB_Alertas.Alertas_UpdateCascade(E_Alertas, tblAlertasApp);
                        if (rpta2 == 0)
                        {
                            lstLogCorreos.Items.Add(String.Format("[{0}][SistemaMMTO] Se guardaron las alertas en la Base de Datos satisfactoriamente", DateTime.Now));
                        }
                    }
                    catch (Exception ex)
                    {
                        lstLogCorreos.Items.Add(String.Format("[{0}][SistemaMMTO] Error al guardar las alertas en la Base de Datos", DateTime.Now));
                        Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                    }

                    tblAlertasLog.Rows.Clear();
                    tblAlertasApp.Rows.Clear();
                    gintIndexRow = 1;
                    gintIndexRowApp = 1;
                }
                catch (Exception ex)
                {
                    Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                }

            }
            GC.Collect();
        }

        public void HiloEnvio_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int rowIndex = tblAlertasLog.Rows.IndexOf(tblAlertasLog.Select("IdAlertaEnvioLog = " + e.ProgressPercentage)[0]);

            if (ErrorEnvio)
            {
                tblAlertasLog.Rows[rowIndex]["FlagEnvio"] = false;
                tblAlertasLog.Rows[rowIndex]["FlagReenvio"] = false;
                lstLogCorreos.Items.Add(String.Format("[{0}][Envío] {1}", DateTime.Now, gstrErrorEnvio));
                gstrErrorEnvio = "";
                ErrorEnvio = false;
            }
            else
            {
                tblAlertasLog.Rows[rowIndex]["FlagEnvio"] = true;
                tblAlertasLog.Rows[rowIndex]["FlagReenvio"] = false;
                lstLogCorreos.Items.Add(String.Format("[{0}][Envío] {1} ha sido enviada", DateTime.Now, tblAlertasLog.Rows[rowIndex]["NombreAlerta"].ToString()));
            }
        }

        public void HiloRenvio_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < tblReenvioAlertas.Rows.Count; i++)
            {
                byte[] Archivo;
                if (Convert.ToBoolean(tblReenvioAlertas.Rows[i]["FlagArchivo"])) { Archivo = (byte[])tblReenvioAlertas.Rows[i]["Archivo"]; } else { Archivo = null; }
                try
                {
                    ReSendAutomatedEmail(tblReenvioAlertas.Rows[i]["Correos"].ToString(), tblReenvioAlertas.Rows[i]["Subject"].ToString(), tblReenvioAlertas.Rows[i]["html"].ToString(), Archivo);
                }
                catch (Exception ex)
                {
                    gstrErrorReenvio = ex.Message;
                    ErrorReenvio = true;
                }
                HiloRenvio.ReportProgress(Convert.ToInt32(tblReenvioAlertas.Rows[i]["IdAlertaEnvioLog"]));
            }
        }

        public void HiloRenvio_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Error == null))
            {
                tsMsg.ForeColor = Color.Red;
                tsMsg.Text = e.Error.Message;
            }
            else
            {
                tsMsg.Text = String.Format("Último reenvío: {0}", DateTime.Now);
                E_Alertas.IdUsuarioCreacion = gintIdUsuario;
                try
                {
                    int rpta = objB_Alertas.Alertas_Envio_Log_UpdateCascade(E_Alertas, tblReenvioAlertas);
                    if (rpta == 0)
                    {
                        lstLogCorreos.Items.Add(String.Format("[{0}][Reenvío] Se guardaron los Logs en la Base de Datos satisfactoriamente", DateTime.Now));
                    }
                }
                catch (Exception ex)
                {
                    lstLogCorreos.Items.Add(String.Format("[{0}][Reenvío] Error al guardar los Logs en la Base de Datos", DateTime.Now));
                    Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                }

            }
            GC.Collect();
        }

        public void HiloRenvio_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int rowIndex = tblReenvioAlertas.Rows.IndexOf(tblReenvioAlertas.Select("IdAlertaEnvioLog = " + e.ProgressPercentage)[0]);
            if (ErrorReenvio)
            {
                tblReenvioAlertas.Rows[rowIndex]["FlagEnvio"] = false;
                tblReenvioAlertas.Rows[rowIndex]["FlagReenvio"] = false;
                tblReenvioAlertas.Rows[rowIndex]["CantReenvio"] = Convert.ToInt32(tblReenvioAlertas.Rows[rowIndex]["CantReenvio"]) + 1;
                lstLogCorreos.Items.Add(String.Format("[{0}][Reenvío] {1}", DateTime.Now, gstrErrorReenvio));
                gstrErrorReenvio = "";
                ErrorReenvio = false;
            }
            else
            {
                tblReenvioAlertas.Rows[rowIndex]["FlagEnvio"] = true;
                tblReenvioAlertas.Rows[rowIndex]["FlagReenvio"] = true;
                tblReenvioAlertas.Rows[rowIndex]["CantReenvio"] = Convert.ToInt32(tblReenvioAlertas.Rows[rowIndex]["CantReenvio"]) + 1;
                lstLogCorreos.Items.Add(String.Format("[{0}][Reenvío] {1} ha sido Reenviada", DateTime.Now, tblReenvioAlertas.Rows[rowIndex]["NombreAlerta"].ToString()));
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                gintSegundosTimerEnvio++;
                if (gintSegundosTimerEnvio >= Utilitarios.Utilitarios.gintTiempoEnvio) //gintTiempoEnvio
                {
                    gintSegundosTimerEnvio = 0; EnviarAlertas();
                }

                gintSegundosTimerReenvio++;
                if (gintSegundosTimerReenvio >= Utilitarios.Utilitarios.gintTiempoReenvio) //gintTiempoReenvio
                {
                    gintSegundosTimerReenvio = 0; ReenviarAlertas();
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void mbtnConfig_Click(object sender, EventArgs e)
        {
            Utilitarios.Utilitarios.IsNewLoad = false;
            Configuracion config = new Configuracion();
            config.ShowDialog();
        }

        private void mbtnCerrar_Click(object sender, EventArgs e)
        {
            ForceExit = true;
            Application.Exit();
        }

        private void EnvioCorreos_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ForceExit)
            {
                NTIcon.Visible = true;
                this.Hide();
                e.Cancel = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NTIcon.Visible = false;
            this.Show();
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ForceExit = true;
            Application.Exit();
        }

        private void mbtnOcultar_Click(object sender, EventArgs e)
        {
            NTIcon.Visible = true;
            this.Hide();
        }

        private void reiniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ForceExit = true;
            Application.Restart();
        }
    }
}
