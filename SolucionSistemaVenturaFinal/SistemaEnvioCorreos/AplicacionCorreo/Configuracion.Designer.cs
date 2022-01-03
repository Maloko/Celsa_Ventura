namespace AplicacionCorreo
{
    partial class Configuracion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuracion));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtColorBordes = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtColorTextoFilas = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTipoLetraTabla = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtColorTextoHeader = new System.Windows.Forms.TextBox();
            this.txtColorHeader = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTipoLetraAlerta = new System.Windows.Forms.TextBox();
            this.tcConfiguracion = new System.Windows.Forms.TabControl();
            this.tbParametros = new System.Windows.Forms.TabPage();
            this.chkIniWin = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtDBPassword = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtDBUserID = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtDBServer = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtPuertoSMTP = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtPassSMTP = new System.Windows.Forms.TextBox();
            this.txtUsuarioSMTP = new System.Windows.Forms.TextBox();
            this.txtServerSMTP = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtCantReenv = new System.Windows.Forms.TextBox();
            this.txtFrecReenv = new System.Windows.Forms.TextBox();
            this.txtFrecEnv = new System.Windows.Forms.TextBox();
            this.lblTiempo02 = new System.Windows.Forms.Label();
            this.lblTiempo01 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtMaxReg = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkZip = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tpPlantilla = new System.Windows.Forms.TabPage();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.fdAlertas = new System.Windows.Forms.FontDialog();
            this.fdTablas = new System.Windows.Forms.FontDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tcConfiguracion.SuspendLayout();
            this.tbParametros.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tpPlantilla.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 248);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(564, 235);
            this.webBrowser1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtColorBordes);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.txtTipoLetraTabla);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(15, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(561, 178);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuración de la Tabla";
            // 
            // txtColorBordes
            // 
            this.txtColorBordes.Location = new System.Drawing.Point(121, 45);
            this.txtColorBordes.Name = "txtColorBordes";
            this.txtColorBordes.ReadOnly = true;
            this.txtColorBordes.Size = new System.Drawing.Size(130, 20);
            this.txtColorBordes.TabIndex = 5;
            this.txtColorBordes.Click += new System.EventHandler(this.txtColorBordes_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtColorTextoFilas);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(282, 77);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(261, 84);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Detalles";
            // 
            // txtColorTextoFilas
            // 
            this.txtColorTextoFilas.Location = new System.Drawing.Point(96, 19);
            this.txtColorTextoFilas.Name = "txtColorTextoFilas";
            this.txtColorTextoFilas.ReadOnly = true;
            this.txtColorTextoFilas.Size = new System.Drawing.Size(130, 20);
            this.txtColorTextoFilas.TabIndex = 5;
            this.txtColorTextoFilas.Click += new System.EventHandler(this.txtColorTextoFilas_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Color de Texto:";
            // 
            // txtTipoLetraTabla
            // 
            this.txtTipoLetraTabla.Location = new System.Drawing.Point(121, 19);
            this.txtTipoLetraTabla.Name = "txtTipoLetraTabla";
            this.txtTipoLetraTabla.ReadOnly = true;
            this.txtTipoLetraTabla.Size = new System.Drawing.Size(179, 20);
            this.txtTipoLetraTabla.TabIndex = 5;
            this.txtTipoLetraTabla.Click += new System.EventHandler(this.txtTipoLetraTabla_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtColorTextoHeader);
            this.groupBox2.Controls.Add(this.txtColorHeader);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(15, 77);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 84);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cabecera";
            // 
            // txtColorTextoHeader
            // 
            this.txtColorTextoHeader.Location = new System.Drawing.Point(106, 45);
            this.txtColorTextoHeader.Name = "txtColorTextoHeader";
            this.txtColorTextoHeader.ReadOnly = true;
            this.txtColorTextoHeader.Size = new System.Drawing.Size(130, 20);
            this.txtColorTextoHeader.TabIndex = 5;
            this.txtColorTextoHeader.Click += new System.EventHandler(this.txtColorTextoHeader_Click);
            // 
            // txtColorHeader
            // 
            this.txtColorHeader.Location = new System.Drawing.Point(106, 19);
            this.txtColorHeader.Name = "txtColorHeader";
            this.txtColorHeader.ReadOnly = true;
            this.txtColorHeader.Size = new System.Drawing.Size(130, 20);
            this.txtColorHeader.TabIndex = 5;
            this.txtColorHeader.Click += new System.EventHandler(this.txtColorHeader_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Color de Fondo:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Color de Texto:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Color de bordes:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(57, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Tipo Letra:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Tipo Letra de la Alerta:";
            // 
            // txtTipoLetraAlerta
            // 
            this.txtTipoLetraAlerta.Location = new System.Drawing.Point(136, 20);
            this.txtTipoLetraAlerta.Name = "txtTipoLetraAlerta";
            this.txtTipoLetraAlerta.ReadOnly = true;
            this.txtTipoLetraAlerta.Size = new System.Drawing.Size(179, 20);
            this.txtTipoLetraAlerta.TabIndex = 5;
            this.txtTipoLetraAlerta.Click += new System.EventHandler(this.txtTipoLetraGeneral_Click);
            // 
            // tcConfiguracion
            // 
            this.tcConfiguracion.Controls.Add(this.tbParametros);
            this.tcConfiguracion.Controls.Add(this.tpPlantilla);
            this.tcConfiguracion.Location = new System.Drawing.Point(12, 12);
            this.tcConfiguracion.Name = "tcConfiguracion";
            this.tcConfiguracion.SelectedIndex = 0;
            this.tcConfiguracion.Size = new System.Drawing.Size(602, 531);
            this.tcConfiguracion.TabIndex = 7;
            // 
            // tbParametros
            // 
            this.tbParametros.Controls.Add(this.chkIniWin);
            this.tbParametros.Controls.Add(this.groupBox7);
            this.tbParametros.Controls.Add(this.groupBox5);
            this.tbParametros.Controls.Add(this.groupBox4);
            this.tbParametros.Location = new System.Drawing.Point(4, 22);
            this.tbParametros.Name = "tbParametros";
            this.tbParametros.Padding = new System.Windows.Forms.Padding(3);
            this.tbParametros.Size = new System.Drawing.Size(594, 505);
            this.tbParametros.TabIndex = 0;
            this.tbParametros.Text = "Configuración de Correos";
            this.tbParametros.UseVisualStyleBackColor = true;
            // 
            // chkIniWin
            // 
            this.chkIniWin.AutoSize = true;
            this.chkIniWin.Location = new System.Drawing.Point(16, 12);
            this.chkIniWin.Name = "chkIniWin";
            this.chkIniWin.Size = new System.Drawing.Size(122, 17);
            this.chkIniWin.TabIndex = 2;
            this.chkIniWin.Text = "Iniciar con Windows";
            this.chkIniWin.UseVisualStyleBackColor = true;
            this.chkIniWin.CheckedChanged += new System.EventHandler(this.chkIniWin_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txtDBName);
            this.groupBox7.Controls.Add(this.label18);
            this.groupBox7.Controls.Add(this.txtDBPassword);
            this.groupBox7.Controls.Add(this.label20);
            this.groupBox7.Controls.Add(this.txtDBUserID);
            this.groupBox7.Controls.Add(this.label19);
            this.groupBox7.Controls.Add(this.txtDBServer);
            this.groupBox7.Controls.Add(this.label17);
            this.groupBox7.Location = new System.Drawing.Point(16, 292);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(564, 87);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Configuración Base de Datos";
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(352, 22);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(180, 20);
            this.txtDBName.TabIndex = 9;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(266, 25);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(80, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Base de Datos:";
            // 
            // txtDBPassword
            // 
            this.txtDBPassword.Location = new System.Drawing.Point(352, 48);
            this.txtDBPassword.Name = "txtDBPassword";
            this.txtDBPassword.Size = new System.Drawing.Size(180, 20);
            this.txtDBPassword.TabIndex = 10;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(282, 51);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(64, 13);
            this.label20.TabIndex = 0;
            this.label20.Text = "Contraseña:";
            // 
            // txtDBUserID
            // 
            this.txtDBUserID.Location = new System.Drawing.Point(68, 48);
            this.txtDBUserID.Name = "txtDBUserID";
            this.txtDBUserID.Size = new System.Drawing.Size(180, 20);
            this.txtDBUserID.TabIndex = 8;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(16, 51);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(46, 13);
            this.label19.TabIndex = 0;
            this.label19.Text = "Usuario:";
            // 
            // txtDBServer
            // 
            this.txtDBServer.Location = new System.Drawing.Point(68, 22);
            this.txtDBServer.Name = "txtDBServer";
            this.txtDBServer.Size = new System.Drawing.Size(180, 20);
            this.txtDBServer.TabIndex = 7;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(13, 25);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(49, 13);
            this.label17.TabIndex = 0;
            this.label17.Text = "Servidor:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtPuertoSMTP);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.label16);
            this.groupBox5.Controls.Add(this.txtPassSMTP);
            this.groupBox5.Controls.Add(this.txtUsuarioSMTP);
            this.groupBox5.Controls.Add(this.txtServerSMTP);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Location = new System.Drawing.Point(16, 401);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(564, 84);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Configuración SMTP";
            // 
            // txtPuertoSMTP
            // 
            this.txtPuertoSMTP.Location = new System.Drawing.Point(352, 22);
            this.txtPuertoSMTP.MaxLength = 6;
            this.txtPuertoSMTP.Name = "txtPuertoSMTP";
            this.txtPuertoSMTP.Size = new System.Drawing.Size(50, 20);
            this.txtPuertoSMTP.TabIndex = 13;
            this.txtPuertoSMTP.Tag = "N";
            this.txtPuertoSMTP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPuertoSMTP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPLANTILLA_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(282, 51);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Contraseña:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 52);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(46, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "Usuario:";
            // 
            // txtPassSMTP
            // 
            this.txtPassSMTP.Location = new System.Drawing.Point(352, 48);
            this.txtPassSMTP.Name = "txtPassSMTP";
            this.txtPassSMTP.PasswordChar = '*';
            this.txtPassSMTP.Size = new System.Drawing.Size(180, 20);
            this.txtPassSMTP.TabIndex = 14;
            // 
            // txtUsuarioSMTP
            // 
            this.txtUsuarioSMTP.Location = new System.Drawing.Point(68, 49);
            this.txtUsuarioSMTP.Name = "txtUsuarioSMTP";
            this.txtUsuarioSMTP.Size = new System.Drawing.Size(180, 20);
            this.txtUsuarioSMTP.TabIndex = 12;
            // 
            // txtServerSMTP
            // 
            this.txtServerSMTP.Location = new System.Drawing.Point(68, 23);
            this.txtServerSMTP.Name = "txtServerSMTP";
            this.txtServerSMTP.Size = new System.Drawing.Size(180, 20);
            this.txtServerSMTP.TabIndex = 11;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(305, 25);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Puerto:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Servidor:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtCantReenv);
            this.groupBox4.Controls.Add(this.txtFrecReenv);
            this.groupBox4.Controls.Add(this.txtFrecEnv);
            this.groupBox4.Controls.Add(this.lblTiempo02);
            this.groupBox4.Controls.Add(this.lblTiempo01);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.txtSubject);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Location = new System.Drawing.Point(16, 35);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(564, 229);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Configuración de Envío / Reenvío";
            // 
            // txtCantReenv
            // 
            this.txtCantReenv.Location = new System.Drawing.Point(142, 101);
            this.txtCantReenv.MaxLength = 5;
            this.txtCantReenv.Name = "txtCantReenv";
            this.txtCantReenv.Size = new System.Drawing.Size(50, 20);
            this.txtCantReenv.TabIndex = 4;
            this.txtCantReenv.Tag = "N";
            this.txtCantReenv.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCantReenv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPLANTILLA_KeyPress);
            // 
            // txtFrecReenv
            // 
            this.txtFrecReenv.Location = new System.Drawing.Point(142, 75);
            this.txtFrecReenv.MaxLength = 5;
            this.txtFrecReenv.Name = "txtFrecReenv";
            this.txtFrecReenv.Size = new System.Drawing.Size(50, 20);
            this.txtFrecReenv.TabIndex = 3;
            this.txtFrecReenv.Tag = "D";
            this.txtFrecReenv.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFrecReenv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPLANTILLA_KeyPress);
            // 
            // txtFrecEnv
            // 
            this.txtFrecEnv.Location = new System.Drawing.Point(142, 49);
            this.txtFrecEnv.MaxLength = 5;
            this.txtFrecEnv.Name = "txtFrecEnv";
            this.txtFrecEnv.Size = new System.Drawing.Size(50, 20);
            this.txtFrecEnv.TabIndex = 2;
            this.txtFrecEnv.Tag = "D";
            this.txtFrecEnv.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFrecEnv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPLANTILLA_KeyPress);
            // 
            // lblTiempo02
            // 
            this.lblTiempo02.AutoSize = true;
            this.lblTiempo02.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTiempo02.Location = new System.Drawing.Point(198, 78);
            this.lblTiempo02.Name = "lblTiempo02";
            this.lblTiempo02.Size = new System.Drawing.Size(48, 13);
            this.lblTiempo02.TabIndex = 4;
            this.lblTiempo02.Text = "Tiempo";
            // 
            // lblTiempo01
            // 
            this.lblTiempo01.AutoSize = true;
            this.lblTiempo01.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTiempo01.Location = new System.Drawing.Point(198, 52);
            this.lblTiempo01.Name = "lblTiempo01";
            this.lblTiempo01.Size = new System.Drawing.Size(48, 13);
            this.lblTiempo01.TabIndex = 4;
            this.lblTiempo01.Text = "Tiempo";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Intentos de Reenvío:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Frecuencia de Reenvío:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Frecuencia de Envío:";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(142, 23);
            this.txtSubject.MaxLength = 100;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(390, 20);
            this.txtSubject.TabIndex = 1;
            this.txtSubject.Tag = "A";
            this.txtSubject.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPLANTILLA_KeyPress);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtMaxReg);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.chkZip);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Location = new System.Drawing.Point(16, 140);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(532, 74);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Archivos Adjuntos";
            // 
            // txtMaxReg
            // 
            this.txtMaxReg.Location = new System.Drawing.Point(163, 32);
            this.txtMaxReg.MaxLength = 5;
            this.txtMaxReg.Name = "txtMaxReg";
            this.txtMaxReg.Size = new System.Drawing.Size(50, 20);
            this.txtMaxReg.TabIndex = 5;
            this.txtMaxReg.Tag = "N";
            this.txtMaxReg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMaxReg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPLANTILLA_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(233, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Comprimido en Zip:";
            this.label11.Visible = false;
            // 
            // chkZip
            // 
            this.chkZip.AutoSize = true;
            this.chkZip.Location = new System.Drawing.Point(336, 35);
            this.chkZip.Name = "chkZip";
            this.chkZip.Size = new System.Drawing.Size(15, 14);
            this.chkZip.TabIndex = 6;
            this.chkZip.UseVisualStyleBackColor = true;
            this.chkZip.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(151, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Máximo de Registro por Alerta:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(90, 26);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Subject:";
            // 
            // tpPlantilla
            // 
            this.tpPlantilla.Controls.Add(this.btnReset);
            this.tpPlantilla.Controls.Add(this.webBrowser1);
            this.tpPlantilla.Controls.Add(this.txtTipoLetraAlerta);
            this.tpPlantilla.Controls.Add(this.groupBox1);
            this.tpPlantilla.Controls.Add(this.label5);
            this.tpPlantilla.Location = new System.Drawing.Point(4, 22);
            this.tpPlantilla.Name = "tpPlantilla";
            this.tpPlantilla.Padding = new System.Windows.Forms.Padding(3);
            this.tpPlantilla.Size = new System.Drawing.Size(594, 505);
            this.tpPlantilla.TabIndex = 1;
            this.tpPlantilla.Text = "Configuración de  Plantilla";
            this.tpPlantilla.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Image = global::AplicacionCorreo.Properties.Resources.reset;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.Location = new System.Drawing.Point(475, 9);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(101, 40);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Restaurar";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Image = global::AplicacionCorreo.Properties.Resources.Cancelar;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.Location = new System.Drawing.Point(327, 551);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(101, 40);
            this.btnCancelar.TabIndex = 16;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnGrabar
            // 
            this.btnGrabar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGrabar.Image = global::AplicacionCorreo.Properties.Resources.Guardar;
            this.btnGrabar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGrabar.Location = new System.Drawing.Point(199, 551);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(101, 40);
            this.btnGrabar.TabIndex = 15;
            this.btnGrabar.Text = "Guardar";
            this.btnGrabar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGrabar.UseVisualStyleBackColor = true;
            this.btnGrabar.Click += new System.EventHandler(this.btnGrabar_Click);
            // 
            // Configuracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 609);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGrabar);
            this.Controls.Add(this.tcConfiguracion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(641, 643);
            this.MinimumSize = new System.Drawing.Size(641, 643);
            this.Name = "Configuracion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuración General";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Configuracion_FormClosing);
            this.Load += new System.EventHandler(this.Configuracion_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tcConfiguracion.ResumeLayout(false);
            this.tbParametros.ResumeLayout(false);
            this.tbParametros.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tpPlantilla.ResumeLayout(false);
            this.tpPlantilla.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtColorHeader;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtColorBordes;
        private System.Windows.Forms.TextBox txtColorTextoHeader;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TextBox txtTipoLetraAlerta;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtColorTextoFilas;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tcConfiguracion;
        private System.Windows.Forms.TabPage tbParametros;
        private System.Windows.Forms.TabPage tpPlantilla;
        private System.Windows.Forms.FontDialog fdAlertas;
        private System.Windows.Forms.TextBox txtTipoLetraTabla;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.FontDialog fdTablas;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblTiempo02;
        private System.Windows.Forms.Label lblTiempo01;
        private System.Windows.Forms.CheckBox chkZip;
        private System.Windows.Forms.Button btnGrabar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtPassSMTP;
        private System.Windows.Forms.TextBox txtUsuarioSMTP;
        private System.Windows.Forms.TextBox txtServerSMTP;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtDBPassword;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtDBUserID;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtDBServer;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtCantReenv;
        private System.Windows.Forms.TextBox txtFrecReenv;
        private System.Windows.Forms.TextBox txtFrecEnv;
        private System.Windows.Forms.TextBox txtPuertoSMTP;
        private System.Windows.Forms.TextBox txtMaxReg;
        private System.Windows.Forms.CheckBox chkIniWin;
    }
}