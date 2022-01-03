namespace AplicacionSistemaVentura.PAQ04_Reportes
{
    partial class XtraSchedulerReport1
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraSchedulerReport1));
            this.dayViewTimeCells1 = new DevExpress.XtraScheduler.Reporting.DayViewTimeCells();
            this.horizontalDateHeaders1 = new DevExpress.XtraScheduler.Reporting.HorizontalDateHeaders();
            this.reportDayView1 = new DevExpress.XtraScheduler.Reporting.ReportDayView();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.dayViewTimeRuler1 = new DevExpress.XtraScheduler.Reporting.DayViewTimeRuler();
            this.personMasterDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.reportDayView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.personMasterDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // dayViewTimeCells1
            // 
            this.dayViewTimeCells1.AppointmentDisplayOptions.EndTimeVisibility = DevExpress.XtraScheduler.AppointmentTimeVisibility.Never;
            this.dayViewTimeCells1.AppointmentDisplayOptions.ShowRecurrence = false;
            this.dayViewTimeCells1.AppointmentDisplayOptions.ShowReminder = false;
            this.dayViewTimeCells1.AppointmentDisplayOptions.SnapToCellsMode = DevExpress.XtraScheduler.AppointmentSnapToCellsMode.Never;
            this.dayViewTimeCells1.AppointmentDisplayOptions.StartTimeVisibility = DevExpress.XtraScheduler.AppointmentTimeVisibility.Never;
            this.dayViewTimeCells1.AppointmentDisplayOptions.StatusDisplayType = DevExpress.XtraScheduler.AppointmentStatusDisplayType.Never;
            this.dayViewTimeCells1.ExtraCells.Visible = false;
            this.dayViewTimeCells1.HorizontalHeaders = this.horizontalDateHeaders1;
            this.dayViewTimeCells1.LocationFloat = new DevExpress.Utils.PointFloat(72.99998F, 35.00001F);
            this.dayViewTimeCells1.Name = "dayViewTimeCells1";
            this.dayViewTimeCells1.ShowAllAppointmentsAtTimeCells = true;
            this.dayViewTimeCells1.ShowAllDayArea = false;
            this.dayViewTimeCells1.ShowWorkTimeOnly = true;
            this.dayViewTimeCells1.SizeF = new System.Drawing.SizeF(886F, 31.58334F);
            this.dayViewTimeCells1.TimeScale = System.TimeSpan.Parse("01:00:00");
            this.dayViewTimeCells1.VerticalLayoutType = DevExpress.XtraScheduler.Reporting.ControlContentLayoutType.Tile;
            this.dayViewTimeCells1.View = this.reportDayView1;
            this.dayViewTimeCells1.VisibleTimeSnapMode = false;
            // 
            // horizontalDateHeaders1
            // 
            this.horizontalDateHeaders1.LocationFloat = new DevExpress.Utils.PointFloat(72.99998F, 10.00001F);
            this.horizontalDateHeaders1.Name = "horizontalDateHeaders1";
            this.horizontalDateHeaders1.SizeF = new System.Drawing.SizeF(886F, 25F);
            this.horizontalDateHeaders1.View = this.reportDayView1;
            // 
            // reportDayView1
            // 
            this.reportDayView1.Appearance.TimeRulerHourLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.reportDayView1.Appearance.TimeRulerHourLine.Options.UseBackColor = true;
            this.reportDayView1.Appearance.TimeRulerLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.reportDayView1.Appearance.TimeRulerLine.Options.UseBackColor = true;
            this.reportDayView1.VisibleDayCount = 12;
            this.reportDayView1.VisibleResourceCount = 2;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.dayViewTimeRuler1,
            this.dayViewTimeCells1,
            this.horizontalDateHeaders1,
            this.xrLabel2,
            this.xrLabel4,
            this.xrLabel5,
            this.xrLabel6,
            this.xrLabel7,
            this.xrLabel8,
            this.xrLabel9});
            this.Detail.HeightF = 153.125F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // dayViewTimeRuler1
            // 
            this.dayViewTimeRuler1.Corners.Top = 25;
            this.dayViewTimeRuler1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.dayViewTimeRuler1.Name = "dayViewTimeRuler1";
            this.dayViewTimeRuler1.SizeF = new System.Drawing.SizeF(72.99998F, 56.58332F);
            this.dayViewTimeRuler1.TimeCells = this.dayViewTimeCells1;
            this.dayViewTimeRuler1.View = this.reportDayView1;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel42,
            this.xrLabel43,
            this.xrLabel40,
            this.xrLabel41,
            this.xrPageInfo1,
            this.xrLabel3,
            this.xrLabel28,
            this.xrPageInfo2,
            this.xrLabel1,
            this.xrPictureBox1});
            this.ReportHeader.HeightF = 100F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel42
            // 
            this.xrLabel42.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(499.4351F, 60.95835F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(30.73248F, 23F);
            this.xrLabel42.StylePriority.UseFont = false;
            this.xrLabel42.StylePriority.UseTextAlignment = false;
            this.xrLabel42.Text = "Al:";
            this.xrLabel42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel43
            // 
            this.xrLabel43.AutoWidth = true;
            this.xrLabel43.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(530.1678F, 60.95835F);
            this.xrLabel43.Name = "xrLabel43";
            this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel43.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel43.StylePriority.UseFont = false;
            // 
            // xrLabel40
            // 
            this.xrLabel40.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(363.8323F, 60.95835F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(35.60263F, 23F);
            this.xrLabel40.StylePriority.UseFont = false;
            this.xrLabel40.StylePriority.UseTextAlignment = false;
            this.xrLabel40.Text = "Del:";
            this.xrLabel40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel41
            // 
            this.xrLabel41.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(399.4351F, 60.95835F);
            this.xrLabel41.Name = "xrLabel41";
            this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel41.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel41.StylePriority.UseFont = false;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.Format = "{0:dd/MM/yyyy hh:mm tt}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(846.3088F, 17.3437F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(122.6911F, 21.05733F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(746.3088F, 17.3437F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 21.05733F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Fecha y Hora:";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel28
            // 
            this.xrLabel28.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(876.2994F, 38.40102F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(48.95831F, 20.91667F);
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.Text = "Pagina";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(925.2576F, 38.40102F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(43.74237F, 21.05733F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(116.6666F, 38.54167F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(759.6328F, 20.91668F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Programación de OT Semanal";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(116.6667F, 73.95834F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // ReportFooter
            // 
            this.ReportFooter.HeightF = 0F;
            this.ReportFooter.KeepTogether = true;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(30.20833F, 139.5833F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(163.5417F, 13.54166F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "O/T Programación Manual";
            // 
            // xrLabel8
            // 
            this.xrLabel8.BackColor = System.Drawing.Color.Violet;
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 139.5833F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(14.58333F, 13.54166F);
            this.xrLabel8.StylePriority.UseBackColor = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(30.20833F, 126.0417F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(163.5417F, 13.54166F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "O/T Programación Correctiva";
            // 
            // xrLabel6
            // 
            this.xrLabel6.BackColor = System.Drawing.Color.PowderBlue;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 126.0417F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(14.58333F, 13.54166F);
            this.xrLabel6.StylePriority.UseBackColor = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(30.20833F, 112.5F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(163.5417F, 13.54166F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "O/T Programación Preventiva";
            // 
            // xrLabel4
            // 
            this.xrLabel4.BackColor = System.Drawing.Color.LightGreen;
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 112.5F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(14.58333F, 13.54166F);
            this.xrLabel4.StylePriority.UseBackColor = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 87.5F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(72.99998F, 18.75F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Leyenda";
            // 
            // XtraSchedulerReport1
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.ReportHeader,
            this.ReportFooter});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 45, 100);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Version = "14.1";
            this.Views.AddRange(new DevExpress.XtraScheduler.Reporting.ReportViewBase[] {
            this.reportDayView1});
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.XtraSchedulerReport1_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.reportDayView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.personMasterDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraScheduler.Reporting.DayViewTimeCells dayViewTimeCells1;
        private DevExpress.XtraScheduler.Reporting.HorizontalDateHeaders horizontalDateHeaders1;
        private DevExpress.XtraScheduler.Reporting.ReportDayView reportDayView1;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraScheduler.Reporting.DayViewTimeRuler dayViewTimeRuler1;
        private System.Windows.Forms.BindingSource personMasterDataBindingSource;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel28;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel42;
        private DevExpress.XtraReports.UI.XRLabel xrLabel43;
        private DevExpress.XtraReports.UI.XRLabel xrLabel40;
        private DevExpress.XtraReports.UI.XRLabel xrLabel41;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;







    }
}
