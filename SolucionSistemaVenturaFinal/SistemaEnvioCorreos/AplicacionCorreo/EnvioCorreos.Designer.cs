namespace AplicacionCorreo
{
    partial class EnvioCorreos
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnvioCorreos));
            this.lstLogCorreos = new System.Windows.Forms.ListBox();
            this.MenuForm = new System.Windows.Forms.MenuStrip();
            this.sistemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mbtnOcultar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mbtnCerrar = new System.Windows.Forms.ToolStripMenuItem();
            this.sistemaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mbtnLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.mbtnConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.BarraForm = new System.Windows.Forms.StatusStrip();
            this.tsMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.NTIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.MenuIcono = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cerrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reiniciarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuForm.SuspendLayout();
            this.BarraForm.SuspendLayout();
            this.MenuIcono.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstLogCorreos
            // 
            this.lstLogCorreos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLogCorreos.FormattingEnabled = true;
            this.lstLogCorreos.Location = new System.Drawing.Point(12, 33);
            this.lstLogCorreos.Name = "lstLogCorreos";
            this.lstLogCorreos.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lstLogCorreos.Size = new System.Drawing.Size(760, 498);
            this.lstLogCorreos.TabIndex = 0;
            // 
            // MenuForm
            // 
            this.MenuForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sistemaToolStripMenuItem,
            this.sistemaToolStripMenuItem1});
            this.MenuForm.Location = new System.Drawing.Point(0, 0);
            this.MenuForm.Name = "MenuForm";
            this.MenuForm.Size = new System.Drawing.Size(784, 24);
            this.MenuForm.TabIndex = 1;
            this.MenuForm.Text = "msMenu";
            // 
            // sistemaToolStripMenuItem
            // 
            this.sistemaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mbtnOcultar,
            this.toolStripSeparator1,
            this.mbtnCerrar});
            this.sistemaToolStripMenuItem.Name = "sistemaToolStripMenuItem";
            this.sistemaToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.sistemaToolStripMenuItem.Text = "&Inicio";
            // 
            // mbtnOcultar
            // 
            this.mbtnOcultar.Name = "mbtnOcultar";
            this.mbtnOcultar.Size = new System.Drawing.Size(152, 22);
            this.mbtnOcultar.Text = "Oc&ultar";
            this.mbtnOcultar.Click += new System.EventHandler(this.mbtnOcultar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // mbtnCerrar
            // 
            this.mbtnCerrar.Name = "mbtnCerrar";
            this.mbtnCerrar.Size = new System.Drawing.Size(152, 22);
            this.mbtnCerrar.Text = "&Cerrar";
            this.mbtnCerrar.Click += new System.EventHandler(this.mbtnCerrar_Click);
            // 
            // sistemaToolStripMenuItem1
            // 
            this.sistemaToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mbtnLogs,
            this.mbtnConfig});
            this.sistemaToolStripMenuItem1.Name = "sistemaToolStripMenuItem1";
            this.sistemaToolStripMenuItem1.Size = new System.Drawing.Size(60, 20);
            this.sistemaToolStripMenuItem1.Text = "&Sistema";
            // 
            // mbtnLogs
            // 
            this.mbtnLogs.Name = "mbtnLogs";
            this.mbtnLogs.Size = new System.Drawing.Size(150, 22);
            this.mbtnLogs.Text = "&Ver Logs";
            this.mbtnLogs.Visible = false;
            // 
            // mbtnConfig
            // 
            this.mbtnConfig.Name = "mbtnConfig";
            this.mbtnConfig.Size = new System.Drawing.Size(150, 22);
            this.mbtnConfig.Text = "C&onfiguración";
            this.mbtnConfig.Click += new System.EventHandler(this.mbtnConfig_Click);
            // 
            // BarraForm
            // 
            this.BarraForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMsg});
            this.BarraForm.Location = new System.Drawing.Point(0, 544);
            this.BarraForm.Name = "BarraForm";
            this.BarraForm.Size = new System.Drawing.Size(784, 22);
            this.BarraForm.TabIndex = 2;
            this.BarraForm.Text = "stsEstatus";
            // 
            // tsMsg
            // 
            this.tsMsg.Name = "tsMsg";
            this.tsMsg.Size = new System.Drawing.Size(0, 17);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // NTIcon
            // 
            this.NTIcon.ContextMenuStrip = this.MenuIcono;
            this.NTIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NTIcon.Icon")));
            this.NTIcon.Text = "Sistema de envios de correos";
            this.NTIcon.Visible = true;
            this.NTIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MenuIcono
            // 
            this.MenuIcono.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reiniciarToolStripMenuItem,
            this.cerrarToolStripMenuItem});
            this.MenuIcono.Name = "MenuIcono";
            this.MenuIcono.Size = new System.Drawing.Size(153, 70);
            // 
            // cerrarToolStripMenuItem
            // 
            this.cerrarToolStripMenuItem.Name = "cerrarToolStripMenuItem";
            this.cerrarToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.cerrarToolStripMenuItem.Text = "Cerrar";
            this.cerrarToolStripMenuItem.Click += new System.EventHandler(this.cerrarToolStripMenuItem_Click);
            // 
            // reiniciarToolStripMenuItem
            // 
            this.reiniciarToolStripMenuItem.Name = "reiniciarToolStripMenuItem";
            this.reiniciarToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.reiniciarToolStripMenuItem.Text = "Reiniciar";
            this.reiniciarToolStripMenuItem.Click += new System.EventHandler(this.reiniciarToolStripMenuItem_Click);
            // 
            // EnvioCorreos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 566);
            this.Controls.Add(this.BarraForm);
            this.Controls.Add(this.lstLogCorreos);
            this.Controls.Add(this.MenuForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuForm;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "EnvioCorreos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de envios de correos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EnvioCorreos_FormClosing);
            this.Load += new System.EventHandler(this.EnvioCorreos_Load);
            this.MenuForm.ResumeLayout(false);
            this.MenuForm.PerformLayout();
            this.BarraForm.ResumeLayout(false);
            this.BarraForm.PerformLayout();
            this.MenuIcono.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstLogCorreos;
        private System.Windows.Forms.MenuStrip MenuForm;
        private System.Windows.Forms.ToolStripMenuItem sistemaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mbtnCerrar;
        private System.Windows.Forms.StatusStrip BarraForm;
        private System.Windows.Forms.ToolStripStatusLabel tsMsg;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem mbtnOcultar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem sistemaToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mbtnLogs;
        private System.Windows.Forms.ToolStripMenuItem mbtnConfig;
        private System.Windows.Forms.NotifyIcon NTIcon;
        private System.Windows.Forms.ContextMenuStrip MenuIcono;
        private System.Windows.Forms.ToolStripMenuItem cerrarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reiniciarToolStripMenuItem;
    }
}

