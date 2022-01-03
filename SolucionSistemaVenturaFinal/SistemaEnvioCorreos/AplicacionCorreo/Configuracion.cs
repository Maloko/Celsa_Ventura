using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Utilitarios;
using Business;
using Entities;

namespace AplicacionCorreo
{
    public partial class Configuracion : Form
    {
        Utilitarios.ErrorHandler Error = new ErrorHandler();
        string ColorHeader = "";
        string ColorTextoHeader = "";
        string ColorBordes = "";
        string ColorTextoFilas = "";
        string TipoLetraAlerta = "";
        string TipoLetraTabla = "";
        int FontSizeAlerta = 0;
        int FontSizeTabla = 0;
        int gintIdUsuario = 1;
        string FontWeightAlerta = "";
        string FontWeightTabla = "";
        Boolean gbolConexionFail = false;
        DataTable tblMaestra = new DataTable();

        B_TablaMaestra objB_TablaMaestra = new B_TablaMaestra();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        //System.Data.SqlClient.SqlConnectionStringBuilder DatosConexion = new System.Data.SqlClient.SqlConnectionStringBuilder();

        public Configuracion()
        {
            InitializeComponent();
        }

        private void Configuracion_Load(object sender, EventArgs e)
        {
            try
            {
                this.AcceptButton = btnGrabar;
                this.CancelButton = btnCancelar;

                chkIniWin.CheckedChanged -= new EventHandler(chkIniWin_CheckedChanged);
                chkIniWin.Checked = (ConfigurationManager.AppSettings["FlagInicioWin"].ToString() == "1");
                chkIniWin.CheckedChanged += new EventHandler(chkIniWin_CheckedChanged);

                //DatosConexion.ConnectionString = ConfigurationManager.ConnectionStrings["BDVentura"].ConnectionString;
                objE_TablaMaestra.IdTabla = 60;
                tblMaestra = objB_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
                tblMaestra.Columns.Remove("FlagActivo");
                tblMaestra.Columns.Remove("IdColumnaPadre");
                tblMaestra.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                objE_TablaMaestra.IdTabla = 1000;
                int TipoParametro = Convert.ToInt32(objB_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).Rows[9]["Valor"]);

                objE_TablaMaestra.IdTabla = 61;
                lblTiempo01.Text = objB_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).Select("IdColumna = " + TipoParametro)[0]["Descripcion"].ToString();
                lblTiempo02.Text = objB_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra).Select("IdColumna = " + TipoParametro)[0]["Descripcion"].ToString();

                txtFrecEnv.Text = tblMaestra.Rows[0]["Valor"].ToString();
                txtFrecReenv.Text = tblMaestra.Rows[1]["Valor"].ToString();
                txtCantReenv.Text = tblMaestra.Rows[2]["Valor"].ToString();
                txtMaxReg.Text = tblMaestra.Rows[3]["Valor"].ToString();
                chkZip.Checked = Convert.ToBoolean(Convert.ToInt32(tblMaestra.Rows[4]["Valor"]));
                txtSubject.Text = tblMaestra.Rows[5]["Valor"].ToString();

                txtServerSMTP.Text = ConfigurationManager.AppSettings["SMTPHost"];
                txtPuertoSMTP.Text = ConfigurationManager.AppSettings["SMTPPort"];
                txtUsuarioSMTP.Text = ConfigurationManager.AppSettings["SMTPEmail"];
                txtPassSMTP.Text = ConfigurationManager.AppSettings["SMTPPassword"];

                txtDBServer.Text = ConfigurationManager.AppSettings["SERVER"];
                txtDBName.Text = ConfigurationManager.AppSettings["BD"];
                txtDBUserID.Text = ConfigurationManager.AppSettings["USER"];
                txtDBPassword.Text = ConfigurationManager.AppSettings["PWD"];
                BloquearControles(false);
                gbolConexionFail = false;
                CargarControles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nPorfavor configure los datos de conexión y guarde los cambios");
                BloquearControles(true);
                gbolConexionFail = true;
                txtDBServer.Text = ConfigurationManager.AppSettings["SERVER"];
                txtDBName.Text = ConfigurationManager.AppSettings["BD"];
                txtDBUserID.Text = ConfigurationManager.AppSettings["USER"];
                txtDBPassword.Text = ConfigurationManager.AppSettings["PWD"];

                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public void BloquearControles(Boolean Estado)
        {
            tpPlantilla.Enabled = !Estado;
            txtColorHeader.Enabled = !Estado;
            txtColorTextoHeader.Enabled = !Estado;
            txtColorBordes.Enabled = !Estado;
            txtColorTextoFilas.Enabled = !Estado;
            txtTipoLetraAlerta.Enabled = !Estado;
            txtTipoLetraTabla.Enabled = !Estado;

            lblTiempo01.Enabled = !Estado;
            lblTiempo02.Enabled = !Estado;

            txtFrecEnv.Enabled = !Estado;
            txtFrecReenv.Enabled = !Estado;
            txtCantReenv.Enabled = !Estado;
            txtMaxReg.Enabled = !Estado;
            chkZip.Enabled = !Estado;
            txtSubject.Enabled = !Estado;

            txtServerSMTP.Enabled = !Estado;
            txtPuertoSMTP.Enabled = !Estado;
            txtUsuarioSMTP.Enabled = !Estado;
            txtPassSMTP.Enabled = !Estado;

            txtDBServer.Enabled = Estado;
            txtDBName.Enabled = Estado;
            txtDBUserID.Enabled = Estado;
            txtDBPassword.Enabled = Estado;
        }
        public void CargarControles()
        {
            txtColorHeader.Text = ConfigurationManager.AppSettings["ColorHeader"];
            txtColorTextoHeader.Text = ConfigurationManager.AppSettings["ColorTextoHeader"];
            txtColorBordes.Text = ConfigurationManager.AppSettings["ColorBordes"];
            txtColorTextoFilas.Text = ConfigurationManager.AppSettings["ColorTextoFilas"];
            txtTipoLetraAlerta.Text = ConfigurationManager.AppSettings["TipoLetraAlerta"];
            txtTipoLetraTabla.Text = ConfigurationManager.AppSettings["TipoLetraTabla"];
            FontSizeAlerta = Convert.ToInt32(ConfigurationManager.AppSettings["FontSize"]);
            FontSizeTabla = Convert.ToInt32(ConfigurationManager.AppSettings["FontSizeTabla"]);
            FontWeightAlerta = ConfigurationManager.AppSettings["FontWeightAlerta"];
            FontWeightTabla = ConfigurationManager.AppSettings["FontWeightTabla"];

            LlenarHtml();
        }
        public void LlenarHtml()
        {
            ColorHeader = txtColorHeader.Text;
            ColorTextoHeader = txtColorTextoHeader.Text;
            ColorBordes = txtColorBordes.Text;
            ColorTextoFilas = txtColorTextoFilas.Text;
            TipoLetraAlerta = txtTipoLetraAlerta.Text;
            TipoLetraTabla = txtTipoLetraTabla.Text;

            webBrowser1.DocumentText = "<html>" +
                                            "<body>" +
                                                "<font style=\"text-shadow: 2px 2px 1px #F2F2F2; font-size: " + FontSizeAlerta + "px; font-family: " + TipoLetraAlerta + "; color: #000;\">Alertas Prueba:</font><br><br>" +
                                                "<table style=\"border-collapse:collapse; text-align:center; box-shadow: 5px 5px 10px #888888;\" >" +
                                                    "<tr style=\"background-color:" + ColorHeader + "; color:" + ColorTextoHeader + "; font-size: " + FontSizeTabla + "px; font-family: " + TipoLetraTabla + "; font-weight: " + FontWeightTabla + ";\">" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Columna 01</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Columna 02</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Columna 03</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Columna 04</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Columna 05</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Columna 06</td>" +
                                                    "</tr>" +
                                                    "<tr style=\"color:" + ColorTextoFilas + "; font-size: " + FontSizeTabla + "px; font-family: " + TipoLetraTabla + "; font-weight: " + FontWeightTabla + ";\">" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 01</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 01</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 01</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 01</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 01</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 01</td>" +
                                                    "</tr>" +
                                                    "<tr style=\"color:" + ColorTextoFilas + "; font-size: " + FontSizeTabla + "px; font-family: " + TipoLetraTabla + "; font-weight: " + FontWeightTabla + ";\">" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 02</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 02</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 02</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 02</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 02</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 02</td>" +
                                                    "</tr>" +
                                                    "<tr style=\"color:" + ColorTextoFilas + "; font-size: " + FontSizeTabla + "px; font-family: " + TipoLetraTabla + "; font-weight: " + FontWeightTabla + ";\">" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 03</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 03</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 03</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 03</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 03</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 03</td>" +
                                                    "</tr>" +
                                                    "<tr style=\"color:" + ColorTextoFilas + "; font-size: " + FontSizeTabla + "px; font-family: " + TipoLetraTabla + "; font-weight: " + FontWeightTabla + ";\">" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 04</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 04</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 04</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 04</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 04</td>" +
                                                        "<td style=\"border-color:" + ColorBordes + "; border-style:solid; border-width:thin; padding: 5px;\">Fila 04</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</body>" +
                                       "</html>";
        }

        private void txtColorHeader_Click(object sender, EventArgs e)
        {
            try
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtColorHeader.Text = Utilitarios.Utilitarios.ConvertirRGB(colorDialog1.Color);
                    LlenarHtml();
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtColorTextoHeader_Click(object sender, EventArgs e)
        {
            try
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtColorTextoHeader.Text = Utilitarios.Utilitarios.ConvertirRGB(colorDialog1.Color);
                    LlenarHtml();
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtColorTextoFilas_Click(object sender, EventArgs e)
        {
            try
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtColorTextoFilas.Text = Utilitarios.Utilitarios.ConvertirRGB(colorDialog1.Color);
                    LlenarHtml();
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtColorBordes_Click(object sender, EventArgs e)
        {
            try
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtColorBordes.Text = Utilitarios.Utilitarios.ConvertirRGB(colorDialog1.Color);
                    LlenarHtml();
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtTipoLetraGeneral_Click(object sender, EventArgs e)
        {
            try
            {
                if (fdAlertas.ShowDialog() == DialogResult.OK)
                {
                    txtTipoLetraAlerta.Text = fdAlertas.Font.FontFamily.Name;
                    FontSizeAlerta = Convert.ToInt32(fdAlertas.Font.Size);
                    FontWeightAlerta = fdAlertas.Font.Style.ToString();
                    LlenarHtml();
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtTipoLetraTabla_Click(object sender, EventArgs e)
        {
            try
            {
                if (fdTablas.ShowDialog() == DialogResult.OK)
                {
                    txtTipoLetraTabla.Text = fdTablas.Font.FontFamily.Name;
                    FontSizeTabla = Convert.ToInt32(fdTablas.Font.Size);
                    FontWeightTabla = fdTablas.Font.Style.ToString();
                    LlenarHtml();
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CargarControles();
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!gbolConexionFail)
                {
                    Utilitarios.Utilitarios.gintMaxRegistros = Convert.ToInt32(txtMaxReg.Text);
                    Utilitarios.Utilitarios.gintTiempoEnvio = Convert.ToDouble(txtFrecEnv.Text) * Utilitarios.Utilitarios.SegundosPorTipo;
                    Utilitarios.Utilitarios.gintTiempoReenvio = Convert.ToDouble(txtFrecReenv.Text) * Utilitarios.Utilitarios.SegundosPorTipo;
                    Utilitarios.Utilitarios.gstrSubject = txtSubject.Text;
                    Utilitarios.Utilitarios.IsZip = (chkZip.Checked) ? true : false;

                    foreach (DataRow drTablaMaesta in tblMaestra.Select())
                    {
                        drTablaMaesta["Nuevo"] = false;
                        switch (drTablaMaesta["IdColumna"].ToString())
                        {
                            case "1": drTablaMaesta["Valor"] = txtFrecEnv.Text;
                                break;
                            case "2": drTablaMaesta["Valor"] = txtFrecReenv.Text;
                                break;
                            case "3": drTablaMaesta["Valor"] = Convert.ToInt32(txtCantReenv.Text);
                                break;
                            case "4": drTablaMaesta["Valor"] = Convert.ToInt32(txtMaxReg.Text);
                                break;
                            case "5": drTablaMaesta["Valor"] = (chkZip.Checked) ? 1 : 0;
                                break;
                            case "6": drTablaMaesta["Valor"] = txtSubject.Text;
                                break;
                        }
                    }

                    objE_TablaMaestra.IdUsuarioCreacion = gintIdUsuario;
                    int rpt = objB_TablaMaestra.TablaMaestra_UpdateMasivo(objE_TablaMaestra, tblMaestra);
                    if (rpt > 0)
                    {
                        Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        configuration.AppSettings.Settings["SMTPHost"].Value = txtServerSMTP.Text;
                        configuration.AppSettings.Settings["SMTPPort"].Value = txtPuertoSMTP.Text;
                        configuration.AppSettings.Settings["SMTPEmail"].Value = txtUsuarioSMTP.Text;
                        configuration.AppSettings.Settings["SMTPPassword"].Value = txtPassSMTP.Text;
                        configuration.AppSettings.Settings["IsNew"].Value = "1";
                        configuration.AppSettings.Settings["FlagInicioWin"].Value = (chkIniWin.Checked) ? "1" : "0";

                        configuration.AppSettings.Settings["ColorHeader"].Value = txtColorHeader.Text;
                        configuration.AppSettings.Settings["ColorTextoHeader"].Value = txtColorTextoHeader.Text;
                        configuration.AppSettings.Settings["ColorBordes"].Value = txtColorBordes.Text;
                        configuration.AppSettings.Settings["ColorTextoFilas"].Value = txtColorTextoFilas.Text;
                        configuration.AppSettings.Settings["TipoLetraAlerta"].Value = txtTipoLetraAlerta.Text;
                        configuration.AppSettings.Settings["TipoLetraTabla"].Value = txtTipoLetraTabla.Text;
                        configuration.AppSettings.Settings["FontSize"].Value = FontSizeAlerta.ToString();
                        configuration.AppSettings.Settings["FontSizeTabla"].Value = FontSizeTabla.ToString();
                        configuration.AppSettings.Settings["FontWeightAlerta"].Value = FontWeightAlerta;
                        configuration.AppSettings.Settings["FontWeightTabla"].Value = FontWeightTabla;
                        configuration.Save(ConfigurationSaveMode.Full, true);
                        ConfigurationManager.RefreshSection("appSettings");

                        MessageBox.Show("Se guardaron los datos satisfactoriamente", "Sistema de Envío Correos");
                        Utilitarios.Utilitarios.IsNewLoad = false;
                        this.Close();
                    }
                }
                else
                {
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    configuration.AppSettings.Settings["SERVER"].Value = txtDBServer.Text;
                    configuration.AppSettings.Settings["BD"].Value = txtDBName.Text;
                    configuration.AppSettings.Settings["USER"].Value = txtDBUserID.Text;
                    configuration.AppSettings.Settings["PWD"].Value = txtDBPassword.Text;
                    configuration.Save(ConfigurationSaveMode.Full, true);
                    ConfigurationManager.RefreshSection("appSettings");
                    Configuracion_Load(null, null);
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPLANTILLA_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (txt.Tag.ToString() == "A")
            {
                e.Handled = SoloAlfaNumerico(e);
            }
            else
            {
                if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
                {
                    e.Handled = true;
                    return;
                }


                if (e.KeyChar == 46)
                {
                    if (txt.Tag.ToString() == "D")
                    {
                        if (txt.Text.IndexOf(e.KeyChar) != -1)
                            e.Handled = true;
                    }
                    else if (txt.Tag.ToString() == "N")
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
        }

        public Boolean SoloAlfaNumerico(KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || Char.IsUpper(e.KeyChar) || Char.IsLower(e.KeyChar) || Char.IsSeparator(e.KeyChar) || Char.IsLetter(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 46 || e.KeyChar == 44)
            { return false; }
            else { return true; }
        }

        private void Configuracion_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Utilitarios.Utilitarios.IsNewLoad)
            {
                Application.Exit();
            }
        }

        private void chkIniWin_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkIniWin.Checked)
                {
                    var path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path, true);
                    key.SetValue("VenturaCorreos", Application.ExecutablePath.ToString());
                }
                else
                {
                    var path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path, true);
                    key.DeleteValue("VenturaCorreos", false);
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
    }
}
