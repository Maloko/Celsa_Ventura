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

using Utilitarios;
using Entities;
using Business;

namespace AplicacionSistemaVentura.PAQ03_Ejecucion
{
    /// <summary>
    /// Interaction logic for EjecGestTransferParte.xaml
    /// </summary>
    public partial class EjecGestTransferParte : UserControl
    {
        int gintIdUsuario = 1;//Se Seteara del Login
        ErrorHandler ObjError = new ErrorHandler();
        DebugHandler objDebug = new DebugHandler();
        E_UCCompTransfer objUCCompTransfer = new E_UCCompTransfer();
        E_TablaMaestra objTablaMaestra = new E_TablaMaestra();
        B_UCComp objBUCComp = new B_UCComp();
        B_PerfilComp objPerfilComp = new B_PerfilComp();
        E_PerfilComp objE_PerfilComp = new E_PerfilComp();
        E_UCComp objEUCComp = new E_UCComp();
        E_UC objUC = new E_UC();
        E_TP ObjETP = new E_TP();
        B_TP objBTP = new B_TP();
        B_UC bUC = new B_UC();
        Boolean gbolNuevo = false; Boolean gbolEdicion = false; Boolean gbolDevolver = false;
        DataView dtv_maestra = new DataView();
        DataTable tblUCComboDestino = new DataTable();
        DataTable tblUCCompOrigen = new DataTable();
        DataTable tblUCCompDestino = new DataTable();
        DataTable tblDetalleTP = new DataTable();
        DataTable tblComboOrigen = new DataTable();

        DateTime FechaModificacion;

        int gintEstado = 0;
        int gintIdHijoOrigen = 0;
        string gstrHijoOrigen = "";
        int gintIdPadreOrigen = 0;

        int gintIdHijoDestino = 0;
        string gstrHijoDestino = "";
        int gintIdPadreDestino = 0;
        int gintIdPerfilCompOrigen = 0, gintIdPerfilCompPadreOrigen = 0, gintIdUCOrigen = 0, gintIdPerfilCompDestino = 0, gintIdPerfilCompPadreDestino = 0, gintIdUCDestino = 0, gintIdPerfilComp = 0, gintIdUCComp = 0;
        int gintCantComp = 0;
        int gintDiasAdd = 0;
        string gstrSerialOrigen = "";
        string gstrSerialDestino = "";
        string gstrPerfilOrigen = "";
        string gstrPerfilDestino = "";
        string gstrEtiquetaTransferenciaPartes = "EjecGestTransferParte";

        int gintIdUCCompTransfer = 0, gintIdMenu = 0;

        public EjecGestTransferParte()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        void OnFocus(object sender, RoutedEventArgs e)
        {
            (sender as Control).Background = System.Windows.Media.Brushes.LightYellow;
        }

        private void OutFocus(object sender, RoutedEventArgs e)
        {
            (sender as Control).Background = System.Windows.Media.Brushes.White;
        }

        private void IniciarTablasTemp()
        {
            tblUCCompOrigen = new DataTable();
            tblUCCompOrigen.Columns.Add("IdUC", Type.GetType("System.Int32"));
            tblUCCompOrigen.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
            tblUCCompOrigen.Columns.Add("IdPerfil", Type.GetType("System.Int32"));
            tblUCCompOrigen.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
            tblUCCompOrigen.Columns.Add("IdPerfilCompPadre", Type.GetType("System.Int32"));
            tblUCCompOrigen.Columns.Add("Nivel", Type.GetType("System.Int32"));
            tblUCCompOrigen.Columns.Add("PerfilComp", Type.GetType("System.String"));
            tblUCCompOrigen.Columns.Add("IdTipoDetalle", Type.GetType("System.Int32"));
            tblUCCompOrigen.Columns.Add("IdItem", Type.GetType("System.Int32"));
            tblUCCompOrigen.Columns.Add("NroSerie", Type.GetType("System.String"));
            tblUCCompOrigen.Columns.Add("CodigoSAP", Type.GetType("System.String"));
            tblUCCompOrigen.Columns.Add("DescripcionSAP", Type.GetType("System.String"));
            tblUCCompOrigen.Columns.Add("IdEstadoUCComp", Type.GetType("System.Int32"));
            tblUCCompOrigen.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
            tblUCCompOrigen.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

            tblUCCompDestino = new DataTable();
            tblUCCompDestino.Columns.Add("IdUC", Type.GetType("System.Int32"));
            tblUCCompDestino.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
            tblUCCompDestino.Columns.Add("IdPerfil", Type.GetType("System.Int32"));
            tblUCCompDestino.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
            tblUCCompDestino.Columns.Add("IdPerfilCompPadre", Type.GetType("System.Int32"));
            tblUCCompDestino.Columns.Add("Nivel", Type.GetType("System.Int32"));
            tblUCCompDestino.Columns.Add("PerfilComp", Type.GetType("System.String"));
            tblUCCompDestino.Columns.Add("IdTipoDetalle", Type.GetType("System.Int32"));
            tblUCCompDestino.Columns.Add("IdItem", Type.GetType("System.Int32"));
            tblUCCompDestino.Columns.Add("NroSerie", Type.GetType("System.String"));
            tblUCCompDestino.Columns.Add("CodigoSAP", Type.GetType("System.String"));
            tblUCCompDestino.Columns.Add("DescripcionSAP", Type.GetType("System.String"));
            tblUCCompDestino.Columns.Add("IdEstadoUCComp", Type.GetType("System.Int32"));
            tblUCCompDestino.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
            tblUCCompDestino.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
        }

        private void UserControl_Loaded()
        {
            try
            {
                IniciarTablasTemp();
                objTablaMaestra.IdTabla = 0;
                dtv_maestra = B_TablaMaestra.TablaMaestra_Combo(objTablaMaestra).DefaultView;
                gintDiasAdd = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=12", dtv_maestra).Rows[0]["Valor"]);
                CboTipoTransfer.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=13", dtv_maestra);
                CboTipoTransfer.DisplayMember = "Descripcion";
                CboTipoTransfer.ValueMember = "IdColumna";

                objUC.IdUc = 0;
                tblComboOrigen = bUC.UC_ComboByUC(objUC);

                CboControlOrigen.ItemsSource = tblComboOrigen;
                CboControlOrigen.DisplayMember = "PlacaSerie";
                CboControlOrigen.ValueMember = "IdUC";
                rbnProgramado.IsChecked = true;
                ListarTransferencias();

                #region VisualizacionBotonImprimir
                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                if (!VisualizaBotonImprimirDetalle)
                {
                    btnImprimir.Visibility = System.Windows.Visibility.Hidden;
                }
                #endregion
                GlobalClass.ip.SeleccionarTab(tbListaTrans);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarTransferencias()
        {
            try
            {
                if ((bool)rbnTodos.IsChecked)
                    objUCCompTransfer.IdEstadoTransfer = 0;
                else if ((bool)rbnProgramado.IsChecked)
                    objUCCompTransfer.IdEstadoTransfer = 1;
                else if ((bool)rbnReprogramado.IsChecked)
                    objUCCompTransfer.IdEstadoTransfer = 4;
                else if ((bool)rbnDevuelto.IsChecked)
                    objUCCompTransfer.IdEstadoTransfer = 3;
                else if ((bool)rbnCerrado.IsChecked)
                    objUCCompTransfer.IdEstadoTransfer = 2;

                dtgListado.ItemsSource = B_UCCompTransfer.UCCompTransfer_List(objUCCompTransfer);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnNuevo(object sender, RoutedEventArgs e)
        {
            if (tblComboOrigen.Rows.Count <= 1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_CANT_UC"), 2);
                return;
            }
            FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
            EstadoForm(true, false, true);
            GlobalClass.ip.SeleccionarTab(tbDetalleTrans);
            lblCodigo.Content = "Nuevo Código";
        }

        private void LimpiarControles()
        {
            try
            {
                trvCompOrigen.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvCompOrigen_SelectedItemChanged);
                trvCompDestino.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvCompDestino_SelectedItemChanged);
                CboControlOrigen.SelectedIndexChanged -= new RoutedEventHandler(CboControlOrigen_SelectedIndexChanged);
                CboControlDestino.SelectedIndexChanged -= new RoutedEventHandler(CboControlDestino_SelectedIndexChanged);
                CboTipoTransfer.SelectedIndexChanged -= new RoutedEventHandler(CboTipoTransfer_SelectedIndexChanged);
                dtpfechaInicio.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(FechaInicio_EditValueChanged);
                dtpfechaDevolucion.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpfechaDevolucion_EditValueChanged);
                TxTComentarios.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(TxTComentarios_EditValueChanged);

                trvCompDestino.ItemsSource = null;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();

                trvCompOrigen.ItemsSource = null;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();

                lblCodigo.Content = "Nuevo Código";
                CboControlDestino.SelectedIndex = -1;
                CboControlOrigen.SelectedIndex = -1;
                CboTipoTransfer.SelectedIndex = -1;
                dtpfechaInicio.EditValue = null;//DateTime.Now.ToShortDateString();
                dtpfechaDevolucion.EditValue = null; //DateTime.Now.ToShortDateString();
                LbLEstado.Content = "";
                TxTComentarios.Text = "";
                LblComponenteDestino.Content = "";
                LbLComponenteOrigen.Content = "";
                dtpfechaDevolucion.IsEnabled = true;
                dtpfechaInicio.IsEnabled = true;

                trvCompOrigen.Focusable = true;
                trvCompDestino.Focusable = true;

                lstvBitacora.Items.Clear();
                TxTComentarios.IsReadOnly = false;
                btnCambioComp.IsEnabled = true;
                CboControlDestino.IsEnabled = true;
                CboControlOrigen.IsEnabled = true;
                CboTipoTransfer.IsEnabled = true;
                gintCantComp = 0;
                skTreeblock01.Visibility = Visibility.Hidden;
                skTreeblock02.Visibility = Visibility.Hidden;

                dtpfechaDevolucion.Visibility = Visibility.Visible;
                lblFechaDevol.Visibility = Visibility.Visible;
                gbolDevolver = false;
                GlobalClass.ip.SeleccionarTab(tbListaTrans);
                TxTComentarios.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(TxTComentarios_EditValueChanged);
                dtpfechaDevolucion.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpfechaDevolucion_EditValueChanged);
                dtpfechaInicio.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(FechaInicio_EditValueChanged);
                trvCompOrigen.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvCompOrigen_SelectedItemChanged);
                trvCompDestino.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvCompDestino_SelectedItemChanged);
                CboControlOrigen.SelectedIndexChanged += new RoutedEventHandler(CboControlOrigen_SelectedIndexChanged);
                CboControlDestino.SelectedIndexChanged += new RoutedEventHandler(CboControlDestino_SelectedIndexChanged);
                CboTipoTransfer.SelectedIndexChanged += new RoutedEventHandler(CboTipoTransfer_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }


        private void CboControlDestino_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                LblComponenteDestino.Content = "";
                tblUCCompDestino.Rows.Clear();
                trvCompDestino.ItemsSource = null;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();

                objEUCComp.IdPerfil = 0;
                objEUCComp.IdUC = Convert.ToInt32(CboControlDestino.EditValue);
                DataTable tblCompDatos = objBUCComp.UCComp_List(objEUCComp);

                objE_PerfilComp.Idperfil = Convert.ToInt32(tblUCComboDestino.Rows[0]["IdPerfil"]);
                objE_PerfilComp.Idestadopc = 0;

                DataView PerfilCompDatos = objPerfilComp.PerfilComp_List(objE_PerfilComp).DefaultView;
                PerfilCompDatos.RowFilter = "FlagNeumatico = 0";
                DataTable tblPerfilComponentesDatos = PerfilCompDatos.ToTable();

                tblCompDatos.DefaultView.Sort = "IdPerfilComp asc";
                tblCompDatos = tblCompDatos.DefaultView.ToTable(true);
                tblPerfilComponentesDatos.DefaultView.Sort = "IdPerfilComp asc";
                tblPerfilComponentesDatos = tblPerfilComponentesDatos.DefaultView.ToTable(true);

                DataRow row1;
                row1 = tblUCCompDestino.NewRow();
                row1["IdPerfilCompPadre"] = 1000;
                row1["IdPerfilComp"] = 0;
                row1["Nivel"] = 1;
                row1["PerfilComp"] = CboControlDestino.Text;
                row1["NroSerie"] = "";
                row1["CodigoSAP"] = "";
                row1["DescripcionSAP"] = "";
                row1["Nuevo"] = true;
                row1["IdEstadoUCComp"] = 1;
                row1["IdTipoDetalle"] = 1;
                tblUCCompDestino.Rows.Add(row1);

                int ucindex = 0;
                int IdPerfilCompUC = 0;
                for (int i = 0; i < tblPerfilComponentesDatos.Rows.Count; i++)
                {
                    int IdPerfilComp = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilComp"]);
                    if (ucindex < tblCompDatos.Rows.Count) IdPerfilCompUC = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilComp"]);

                    DataRow row;
                    row = tblUCCompDestino.NewRow();
                    if (IdPerfilCompUC == IdPerfilComp)
                    {
                        row["IdPerfilCompPadre"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilCompPadre"]);
                        row["IdUCComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdUCComp"]);
                        row["IdPerfil"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfil"]);
                        row["IdPerfilComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilComp"]);
                        row["Nivel"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["Nivel"]);
                        row["PerfilComp"] = Convert.ToString(tblCompDatos.Rows[ucindex]["PerfilComp"]);
                        row["IdUC"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdUC"]);
                        row["IdTipoDetalle"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdTipoDetalle"]);
                        row["IdItem"] = 0;
                        row["NroSerie"] = Convert.ToString(tblCompDatos.Rows[ucindex]["NroSerie"]);
                        row["CodigoSAP"] = Convert.ToString(tblCompDatos.Rows[ucindex]["CodigoSAP"]);
                        row["DescripcionSAP"] = Convert.ToString(tblCompDatos.Rows[ucindex]["DescripcionSAP"]);
                        row["IdEstadoUCComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdEstadoUCComp"]);
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        tblUCCompDestino.Rows.Add(row);
                        ucindex++;
                    }
                    else
                    {
                        row["IdPerfilCompPadre"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilCompPadre"]);
                        row["IdUCComp"] = 0;
                        row["IdPerfilComp"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilComp"]);
                        row["Nivel"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["Nivel"]);
                        row["PerfilComp"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["PerfilComp"]);
                        row["IdUC"] = 0;
                        row["IdTipoDetalle"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdTipoDetalle"]);
                        row["IdItem"] = 0;
                        row["NroSerie"] = "";
                        row["CodigoSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["CodigoSAP"]);
                        row["DescripcionSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["DescripcionSAP"]);
                        row["IdEstadoUCComp"] = 0;
                        row["FlagActivo"] = true;
                        row["Nuevo"] = true;
                        tblUCCompDestino.Rows.Add(row);
                    }
                }

                Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblUCCompDestino;
                trvCompDestino.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(1000, null);

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void CboControlOrigen_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                trvCompDestino.ItemsSource = null;
                LbLComponenteOrigen.Content = "";
                LblComponenteDestino.Content = "";
                tblUCCompOrigen.Rows.Clear();
                trvCompOrigen.ItemsSource = null;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();

                objUC.IdUc = Convert.ToInt32(CboControlOrigen.EditValue);
                tblUCComboDestino = bUC.UC_ComboByUC(objUC);


                DataView dtvDestino = tblUCComboDestino.DefaultView;
                dtvDestino.RowFilter = "IdUC <>" + CboControlOrigen.EditValue.ToString();
                CboControlDestino.ItemsSource = dtvDestino;
                CboControlDestino.DisplayMember = "PlacaSerie";
                CboControlDestino.ValueMember = "IdUC";

                objEUCComp.IdPerfil = 0;
                objEUCComp.IdUC = Convert.ToInt32(CboControlOrigen.EditValue);
                DataTable tblCompDatos = objBUCComp.UCComp_List(objEUCComp);

                //if (tblCompDatos.Rows.Count != 0 || (!gbolEdicion && !gbolNuevo))
                //{
                objE_PerfilComp.Idperfil = Convert.ToInt32(tblUCComboDestino.Rows[0]["IdPerfil"]);
                objE_PerfilComp.Idestadopc = 0;

                DataView PerfilCompDatos = objPerfilComp.PerfilComp_List(objE_PerfilComp).DefaultView;
                PerfilCompDatos.RowFilter = "FlagNeumatico = 0";
                DataTable tblPerfilComponentesDatos = PerfilCompDatos.ToTable();

                tblCompDatos.DefaultView.Sort = "IdPerfilComp asc";
                tblCompDatos = tblCompDatos.DefaultView.ToTable(true);

                tblPerfilComponentesDatos.DefaultView.Sort = "IdPerfilComp asc";
                tblPerfilComponentesDatos = tblPerfilComponentesDatos.DefaultView.ToTable(true);

                DataRow row1;
                row1 = tblUCCompOrigen.NewRow();
                row1["IdPerfilCompPadre"] = 1000;
                row1["IdPerfilComp"] = 0;
                row1["Nivel"] = 1;
                row1["PerfilComp"] = CboControlOrigen.Text;
                row1["NroSerie"] = "";
                row1["CodigoSAP"] = "";
                row1["DescripcionSAP"] = "";
                row1["Nuevo"] = true;
                row1["IdEstadoUCComp"] = 1;
                row1["IdTipoDetalle"] = 1;
                tblUCCompOrigen.Rows.Add(row1);

                int ucindex = 0;
                int IdPerfilCompUC = 0;
                for (int i = 0; i < tblPerfilComponentesDatos.Rows.Count; i++)
                {
                    int IdPerfilComp = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilComp"]);
                    if (ucindex < tblCompDatos.Rows.Count) IdPerfilCompUC = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilComp"]);

                    DataRow row;
                    row = tblUCCompOrigen.NewRow();
                    if (IdPerfilCompUC == IdPerfilComp)
                    {
                        if (tblCompDatos.Rows[ucindex]["NroSerie"].ToString() != "")
                        {
                            row["IdPerfilCompPadre"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilCompPadre"]);
                            row["IdUCComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdUCComp"]);
                            row["IdPerfil"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfil"]);
                            row["IdPerfilComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilComp"]);
                            row["Nivel"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["Nivel"]);
                            row["PerfilComp"] = Convert.ToString(tblCompDatos.Rows[ucindex]["PerfilComp"]);
                            row["IdUC"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdUC"]);
                            row["IdTipoDetalle"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdTipoDetalle"]);
                            row["IdItem"] = 0;
                            row["NroSerie"] = Convert.ToString(tblCompDatos.Rows[ucindex]["NroSerie"]);
                            row["CodigoSAP"] = Convert.ToString(tblCompDatos.Rows[ucindex]["CodigoSAP"]);
                            row["DescripcionSAP"] = Convert.ToString(tblCompDatos.Rows[ucindex]["DescripcionSAP"]);
                            row["IdEstadoUCComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdEstadoUCComp"]);
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                        }
                        else
                        {
                            row["IdPerfilCompPadre"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilCompPadre"]);
                            row["IdUCComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdUCComp"]);
                            row["IdPerfil"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfil"]);
                            row["IdPerfilComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdPerfilComp"]);
                            row["Nivel"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["Nivel"]);
                            row["PerfilComp"] = Convert.ToString(tblCompDatos.Rows[ucindex]["PerfilComp"]);
                            row["IdUC"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdUC"]);
                            row["IdTipoDetalle"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdTipoDetalle"]);
                            row["IdItem"] = 0;
                            row["NroSerie"] = Convert.ToString(tblCompDatos.Rows[ucindex]["NroSerie"]);
                            row["CodigoSAP"] = Convert.ToString(tblCompDatos.Rows[ucindex]["CodigoSAP"]);
                            row["DescripcionSAP"] = Convert.ToString(tblCompDatos.Rows[ucindex]["DescripcionSAP"]);
                            row["IdEstadoUCComp"] = Convert.ToInt32(tblCompDatos.Rows[ucindex]["IdEstadoUCComp"]);
                            row["FlagActivo"] = true;
                            row["Nuevo"] = false;
                        }

                        tblUCCompOrigen.Rows.Add(row);
                        ucindex++;
                    }
                    else
                    {
                        row["IdPerfilCompPadre"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilCompPadre"]);
                        row["IdUCComp"] = 0;
                        row["IdPerfilComp"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilComp"]);
                        row["Nivel"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["Nivel"]);
                        row["PerfilComp"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["PerfilComp"]);
                        row["IdUC"] = 0;
                        row["IdTipoDetalle"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdTipoDetalle"]);
                        row["IdItem"] = 0;
                        row["NroSerie"] = "";
                        row["CodigoSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["CodigoSAP"]);
                        row["DescripcionSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["DescripcionSAP"]);
                        row["IdEstadoUCComp"] = 0;
                        row["FlagActivo"] = true;
                        row["Nuevo"] = true;
                        tblUCCompOrigen.Rows.Add(row);
                    }
                }
                Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblUCCompOrigen;
                int CantSeries = tblUCCompOrigen.Select("NroSerie <> '' ").Length;
                if (CantSeries != 0)
                {
                    trvCompOrigen.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(1000, null);
                }
                else
                {
                    trvCompOrigen.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponente(1000, null);
                }


            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidaCampoObligado() == true) { return; }
                if (ValidaLogicaNegocio() == true) { return; }



                if (gbolNuevo == true && gbolEdicion == false)
                {
                    if (gintCantComp == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_REAL_TRAN"), 2);
                        btnCambioComp.Focus();
                        return;
                    }
                    string bitacora = "";
                    foreach (var item in lstvBitacora.Items)
                    {
                        bitacora += item.ToString() + "|";
                    }
                    ObjETP.IdPerfilCompOrigen = gintIdPerfilCompOrigen;
                    ObjETP.IdPerfilCompPadreOrigen = gintIdPerfilCompPadreOrigen;
                    ObjETP.IdUCOrigen = gintIdUCOrigen;

                    ObjETP.IdPerfilCompDestino = gintIdPerfilCompDestino;
                    ObjETP.IdPerfilCompPadreDestino = gintIdPerfilCompPadreDestino;
                    ObjETP.IdUCDestino = gintIdUCDestino;

                    ObjETP.IdPerfilComp = gintIdPerfilComp;
                    ObjETP.IdUCComp = gintIdUCComp;
                    ObjETP.IdTipoTransfer = Convert.ToInt32(CboTipoTransfer.EditValue);
                    ObjETP.FechaTransfer = Convert.ToDateTime(dtpfechaInicio.EditValue);
                    ObjETP.FechaDevolucion = Convert.ToDateTime(dtpfechaDevolucion.EditValue);
                    ObjETP.Observacion = TxTComentarios.Text;
                    ObjETP.Bitacora = bitacora.Remove(bitacora.Length - 1);
                    ObjETP.IdEstadoTransfer = gintEstado;
                    ObjETP.IdUsuario = gintIdUsuario;
                    ObjETP.FechaModificacion = FechaModificacion;

                    int rpta = objBTP.UCCompTransfer_UpdateCascade(ObjETP);
                    if (rpta == 1)
                    {
                        gintCantComp = 0;
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "GRAB_NUEV"), 1);
                    }
                    else if (rpta == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "GRAB_CONC"), 2);
                        return;
                    }
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    DateTime fecDevol = Convert.ToDateTime(dtgListado.GetFocusedRowCellValue("FechaDevolucion"));
                    int compare = DateTime.Compare(Convert.ToDateTime(dtpfechaDevolucion.EditValue), fecDevol);
                    if (compare != 0)
                    {
                        ObjETP.IdEstadoTransfer = 4;
                    }
                    else
                    {
                        ObjETP.IdEstadoTransfer = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdEstadoTransfer"));
                    }

                    if (Convert.ToInt32(CboTipoTransfer.EditValue) == 2)
                    {
                        if (TxTComentarios.Text == "")
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_COME"), 2);
                            TxTComentarios.Focus();
                            return;
                        }

                        var dialogResult = DevExpress.Xpf.Core.DXMessageBox.Show("¿Esta seguro que desea cambiar el tipo de transferencia a definitiva?", "Sistema", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (dialogResult == MessageBoxResult.No)
                        {
                            return;
                        }

                        ObjETP.IdEstadoTransfer = 2;
                    }
                    ObjETP.IdUCCompTransfer = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdUCCompTransfer"));
                    ObjETP.IdTipoTransfer = Convert.ToInt32(CboTipoTransfer.EditValue);
                    ObjETP.FechaTransfer = Convert.ToDateTime(dtpfechaInicio.EditValue);
                    ObjETP.FechaDevolucion = Convert.ToDateTime(dtpfechaDevolucion.EditValue);
                    ObjETP.Observacion = TxTComentarios.Text;
                    ObjETP.IdUsuario = gintIdUsuario;
                    ObjETP.FechaModificacion = FechaModificacion;

                    int rpta = objBTP.UCCompTransfer_Update(ObjETP);
                    if (rpta == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "GRAB_EDIT"), 1);
                    }
                    else if (rpta == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "GRAB_CONC"), 2);
                        return;
                    }
                }
                else if (gbolNuevo == false && gbolEdicion == false && gbolDevolver == true)
                {
                    if (TxTComentarios.Text == "")
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_COME"), 2);
                        TxTComentarios.Focus();
                        return;
                    }
                    ObjETP.IdUCCompTransfer = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdUCCompTransfer"));
                    ObjETP.IdTipoTransfer = Convert.ToInt32(CboTipoTransfer.EditValue);
                    ObjETP.FechaTransfer = Convert.ToDateTime(dtpfechaInicio.EditValue);
                    ObjETP.FechaDevolucion = Convert.ToDateTime(dtpfechaDevolucion.EditValue);
                    ObjETP.Observacion = TxTComentarios.Text;
                    ObjETP.IdEstadoTransfer = 3;
                    ObjETP.IdUsuario = gintIdUsuario;
                    ObjETP.FechaModificacion = FechaModificacion;

                    int rpta = objBTP.UCCompTransfer_Delete(ObjETP);
                    if (rpta == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "GRAB_DEVO"), 1);
                    }
                    else if (rpta == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "GRAB_CONC"), 2);
                        return;
                    }
                }
                LimpiarControles();
                ListarTransferencias();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int IdEstadoTransfer = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdEstadoTransfer"));
                if (IdEstadoTransfer != 1 && IdEstadoTransfer != 4)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "LOGI_ESTA_MODI"), 2);
                    return;
                }
                if (dtgListado.VisibleRowCount == 0) { return; }
                trvCompOrigen.Focusable = false;
                trvCompDestino.Focusable = false;
                btnCambioComp.IsEnabled = false;
                CboControlDestino.IsEnabled = false;
                CboControlOrigen.IsEnabled = false;
                dtpfechaInicio.IsEnabled = false;
                DetallesTranferParte();
                EstadoForm(false, true, true);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void EstadoForm(Boolean stdNuevo, Boolean stdEditar, Boolean stdForzar)
        {
            try
            {
                if (stdForzar == true)
                {
                    gbolNuevo = stdNuevo; gbolEdicion = stdEditar;
                }
                else if (stdForzar == false)
                {
                    if (gbolNuevo == false)
                    {
                        gbolNuevo = stdNuevo; gbolEdicion = stdEditar;
                    }
                }
                if ((gbolNuevo == false) && (gbolEdicion == false) && (gbolDevolver == true))
                {
                    tbDetalleTrans.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "TAB1_DEVO");
                    BtnRegistrarPrestamo.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "BTNG_DEVO");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == false))
                {
                    tbDetalleTrans.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "TAB1_CONS");
                    BtnRegistrarPrestamo.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "BTNG_CONS");
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tbDetalleTrans.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "TAB1_NUEV");
                    BtnRegistrarPrestamo.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "BTNG_NUEV");
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tbDetalleTrans.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "TAB1_EDIT");
                    BtnRegistrarPrestamo.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "BTNG_EDIT");
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnDevolucion(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgListado.VisibleRowCount == 0) { return; }
                int IdEstadoTransfer;
                try { IdEstadoTransfer = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdEstadoTransfer")); }
                catch { return; }

                if (IdEstadoTransfer == 1 || IdEstadoTransfer == 4)
                {
                    gbolDevolver = true;
                    EstadoForm(false, false, true);
                    dtpfechaInicio.IsEnabled = false;
                    trvCompOrigen.Focusable = false;
                    trvCompDestino.Focusable = false;
                    btnCambioComp.IsEnabled = false;
                    CboControlDestino.IsEnabled = false;
                    CboControlOrigen.IsEnabled = false;
                    CboTipoTransfer.IsEnabled = false;

                    DetallesTranferParteDevolicion();
                }
                else
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "LOGI_ESTA_DEVO"), 2);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void btnCambioComp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gstrSerialOrigen == "")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_COMP_ORIG"), 2);
                    return;
                }
                else if (gstrSerialDestino != "")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_COMP_DEST"), 2);
                    return;
                }

                if (gintCantComp == 0)
                {
                    if (gintIdHijoOrigen != 0 && gintIdHijoDestino != 0)
                    {
                        ObjETP.IdPerfilCompOrigen = gintIdHijoOrigen;
                        ObjETP.IdUCOrigen = Convert.ToInt32(CboControlOrigen.EditValue);
                        DataTable tblUCCompTransfer = objBTP.UCCompTransfer_BeforeChange(ObjETP);
                        
                        if (tblUCCompTransfer.Rows.Count > 0)
                        {
                            GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "LOGI_COMP_NUEV"), tblUCCompTransfer.Rows[0]["PerfilCompDestino"].ToString(), tblUCCompTransfer.Rows[0]["PlacaSerieOrigen"].ToString()), 2);
                            return;
                        }

                        if (!gstrHijoOrigen.Equals(gstrHijoDestino))
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_COMP_EQUA"), 2);
                            return;
                        }

                        trvCompDestino.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvCompDestino_SelectedItemChanged);
                        trvCompOrigen.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvCompOrigen_SelectedItemChanged);

                        int OriIndex = 0, DesIndex = 0;
                        int IdTipoDetalleOrigen = 0;
                        int IdTipoDetalleDestino = 0;
                        foreach (DataRow drUCCompOrigen in tblUCCompOrigen.Select("IdPerfilComp =" + gintIdHijoOrigen))
                        {
                            OriIndex = tblUCCompOrigen.Rows.IndexOf(drUCCompOrigen);
                            gintIdPerfilCompOrigen = Convert.ToInt32(drUCCompOrigen["IdPerfilComp"]);
                            gintIdPerfilCompPadreOrigen = Convert.ToInt32(drUCCompOrigen["IdPerfilCompPadre"]);
                            gintIdUCOrigen = Convert.ToInt32(CboControlOrigen.EditValue);
                            gstrSerialOrigen = drUCCompOrigen["NroSerie"].ToString();
                            gintIdPerfilComp = Convert.ToInt32(drUCCompOrigen["IdPerfilComp"]);
                            gintIdUCComp = Convert.ToInt32(drUCCompOrigen["IdUCComp"]);
                            IdTipoDetalleOrigen = Convert.ToInt32(drUCCompOrigen["IdTipoDetalle"]);
                        }

                        foreach (DataRow drUCCompDestino in tblUCCompDestino.Select("IdPerfilComp =" + gintIdHijoDestino))
                        {
                            //DataView dtvDestino = tblUCCompDestino.DefaultView;
                            //dtvDestino.RowFilter = "IdUC <> 0";
                            DesIndex = tblUCCompDestino.Rows.IndexOf(drUCCompDestino);
                            gintIdPerfilCompDestino = Convert.ToInt32(drUCCompDestino["IdPerfilComp"]);
                            gintIdPerfilCompPadreDestino = Convert.ToInt32(drUCCompDestino["IdPerfilCompPadre"]);
                            gintIdUCDestino = Convert.ToInt32(CboControlDestino.EditValue);
                            gstrSerialDestino = drUCCompDestino["NroSerie"].ToString();
                            IdTipoDetalleDestino = Convert.ToInt32(drUCCompDestino["IdTipoDetalle"]);
                        }

                        if (IdTipoDetalleOrigen == 1 || IdTipoDetalleDestino == 1)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "LOGI_COMP_TIPO"), 2);
                            trvCompDestino.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvCompDestino_SelectedItemChanged);
                            trvCompOrigen.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvCompOrigen_SelectedItemChanged);
                            return;
                        }
                        //for (int i = 0; i < tblUCCompOrigen.Rows.Count; i++)
                        //{
                        //    if (Convert.ToInt32(tblUCCompOrigen.Rows[i]["IdPerfilComp"]) == gintIdHijoOrigen)
                        //    {
                        //        OriIndex = i;
                        //        gintIdPerfilCompOrigen = Convert.ToInt32(tblUCCompOrigen.Rows[i]["IdPerfilComp"]);
                        //        gintIdPerfilCompPadreOrigen = Convert.ToInt32(tblUCCompOrigen.Rows[i]["IdPerfilCompPadre"]);
                        //        gintIdUCOrigen = Convert.ToInt32(CboControlOrigen.EditValue);
                        //        gstrSerialOrigen = tblUCCompOrigen.Rows[i]["NroSerie"].ToString();
                        //        gintIdPerfilComp = Convert.ToInt32(tblUCCompOrigen.Rows[i]["IdPerfilComp"]);
                        //        gintIdUCComp = Convert.ToInt32(tblUCCompOrigen.Rows[i]["IdUCComp"]);
                        //    }
                        //}

                        //for (int i = 0; i < tblUCCompDestino.Rows.Count; i++)
                        //{
                        //    if (Convert.ToInt32(tblUCCompDestino.Rows[i]["IdPerfilComp"]) == gintIdHijoDestino)
                        //    {
                        //        DataView dtvDestino = tblUCCompDestino.DefaultView;
                        //        dtvDestino.RowFilter = "IdUC <> 0";
                        //        DesIndex = i;
                        //        gintIdPerfilCompDestino = Convert.ToInt32(tblUCCompDestino.Rows[i]["IdPerfilComp"]);
                        //        gintIdPerfilCompPadreDestino = Convert.ToInt32(tblUCCompDestino.Rows[i]["IdPerfilCompPadre"]);
                        //        gintIdUCDestino = Convert.ToInt32(CboControlDestino.EditValue);
                        //        gstrSerialDestino = tblUCCompDestino.Rows[i]["NroSerie"].ToString();
                        //    }
                        //}

                        var dialogResult = DevExpress.Xpf.Core.DXMessageBox.Show("¿Esta seguro de hacer el intercambio?", "Sistema", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            gintCantComp++;
                            gstrPerfilOrigen = tblUCCompOrigen.Rows[OriIndex]["PerfilComp"].ToString();
                            gstrPerfilDestino = tblUCCompDestino.Rows[DesIndex]["PerfilComp"].ToString();

                            tblUCCompOrigen.Rows[OriIndex]["PerfilComp"] = tblUCCompOrigen.Rows[OriIndex]["PerfilComp"].ToString();
                            tblUCCompDestino.Rows[DesIndex]["PerfilComp"] = tblUCCompDestino.Rows[DesIndex]["PerfilComp"];

                            tblUCCompOrigen.Rows[OriIndex]["NroSerie"] = "";
                            tblUCCompDestino.Rows[DesIndex]["NroSerie"] = "Nuevo";

                            lstvBitacora.Items.Add(String.Format("[Origen] UC: {0} - Componente: {1}", CboControlOrigen.Text, tblUCCompOrigen.Rows[OriIndex]["PerfilComp"].ToString()));
                            lstvBitacora.Items.Add(String.Format("[Destino] UC: {0} - Componente: {1}", CboControlDestino.Text, tblUCCompDestino.Rows[DesIndex]["PerfilComp"].ToString()));

                            trvCompOrigen.ItemsSource = null;
                            Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                            Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblUCCompOrigen;
                            trvCompOrigen.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(1000, null);

                            trvCompDestino.ItemsSource = null;
                            Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                            Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblUCCompDestino;
                            trvCompDestino.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(1000, null);
                        }

                        trvCompDestino.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvCompDestino_SelectedItemChanged);
                        trvCompOrigen.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvCompOrigen_SelectedItemChanged);
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_COMP_SELE"), 2);
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "LOGI_COMP_CANT"), 2);
                }
                
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void trvCompOrigen_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (tblUCCompOrigen.Rows.Count != 0)
                {
                    TreeViewModel trmOrigen = (TreeViewModel)trvCompOrigen.SelectedItem;
                    if (trmOrigen != null)
                    {
                        gstrHijoOrigen = trmOrigen.Name;
                        gintIdHijoOrigen = trmOrigen.IdMenu;
                        gintIdPadreOrigen = trmOrigen.IdMenuPadre;
                        string principal = "";
                        string Titulo = "";
                        string Componente = "";
                        int Padre = 0;

                        foreach (DataRow drUCCompOrigen in tblUCCompOrigen.Select("IdPerfilComp = " + gintIdHijoOrigen))
                        {
                            Componente = drUCCompOrigen["PerfilComp"].ToString();
                            gstrSerialOrigen = drUCCompOrigen["NroSerie"].ToString();
                        }

                        foreach (DataRow drUCCompOrigen in tblUCCompOrigen.Select("IdPerfilComp = " + gintIdPadreOrigen))
                        {
                            Titulo = drUCCompOrigen["PerfilComp"].ToString() + " - ";
                            Padre = Convert.ToInt32(drUCCompOrigen["IdPerfilCompPadre"]);
                        }

                        foreach (DataRow drUCCompOrigen in tblUCCompOrigen.Select("IdPerfilComp = " + Padre))
                        {
                            principal = drUCCompOrigen["PerfilComp"].ToString() + " - ";
                        }
                        if (gintIdHijoOrigen != 0)
                        {
                            LbLComponenteOrigen.Content = String.Format("{0}{1}{2}", principal, Titulo, Componente);
                        }
                        else
                        {
                            LbLComponenteOrigen.Content = String.Format("{0}", Componente);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void trvCompDestino_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (tblUCCompDestino.Rows.Count != 0)
                {
                    TreeViewModel trmDestino = (TreeViewModel)trvCompDestino.SelectedItem;
                    if (trmDestino != null)
                    {
                        gstrHijoDestino = trmDestino.Name;
                        gintIdHijoDestino = trmDestino.IdMenu;
                        gintIdPadreDestino = trmDestino.IdMenuPadre;
                        string principal = "";
                        string Titulo = "";
                        string Componente = "";
                        int Padre = 0;

                        foreach (DataRow drUCCompDestino in tblUCCompDestino.Select("IdPerfilComp = " + gintIdHijoDestino))
                        {
                            Componente = drUCCompDestino["PerfilComp"].ToString();
                            gstrSerialDestino = drUCCompDestino["NroSerie"].ToString();
                        }

                        foreach (DataRow drUCCompDestino in tblUCCompDestino.Select("IdPerfilComp = " + gintIdPadreDestino))
                        {
                            Titulo = drUCCompDestino["PerfilComp"].ToString() + " - ";
                            Padre = Convert.ToInt32(drUCCompDestino["IdPerfilCompPadre"]);
                        }

                        foreach (DataRow drUCCompDestino in tblUCCompDestino.Select("IdPerfilComp = " + Padre))
                        {
                            principal = drUCCompDestino["PerfilComp"].ToString() + " - ";
                        }

                        if (gintIdHijoDestino != 0)
                        {
                            LblComponenteDestino.Content = String.Format("{0}{1}{2}", principal, Titulo, Componente);
                        }
                        else
                        {
                            LblComponenteDestino.Content = String.Format("{0}", Componente);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void CboTipoTransfer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                gintEstado = Convert.ToInt32(CboTipoTransfer.EditValue);
                if (gintEstado == 2)
                {
                    dtpfechaDevolucion.Visibility = Visibility.Hidden;
                    lblFechaDevol.Visibility = Visibility.Hidden;
                }
                else
                {
                    dtpfechaDevolucion.Visibility = Visibility.Visible;
                    lblFechaDevol.Visibility = Visibility.Visible;
                }
                CambiarEstadoTranPartes();
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void CambiarEstadoTranPartes()
        {
            try
            {
                DataView dtvEstados = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=36", dtv_maestra).DefaultView;
                dtvEstados.RowFilter = "Valor = " + gintEstado;
                if (gintEstado == 2)
                {
                    LbLEstado.Content = "";
                }
                else
                {
                    LbLEstado.Content = dtvEstados[0]["Descripcion"].ToString();
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gbolDevolver = false;
                EstadoForm(false, false, true);
                LimpiarControles();
                ListarTransferencias();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private bool ValidaLogicaNegocio()
        {
            bool bolRpta = false;
            try
            {

                return bolRpta;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                return bolRpta;
            }
        }

        private bool ValidaCampoObligado()
        {
            bool bolRpta = false;
            try
            {
                if (CboTipoTransfer.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_TIPO"), 2);
                    CboTipoTransfer.Focus();
                }
                else if (dtpfechaInicio.EditValue == null)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_FECH_TRAN"), 2);
                    dtpfechaInicio.Focus();
                }
                else if (dtpfechaDevolucion.EditValue == null)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "OBLI_FECH_DEVO"), 2);
                    dtpfechaDevolucion.Focus();
                }
                else if (DateTime.Compare(Convert.ToDateTime(dtpfechaDevolucion.EditValue), Convert.ToDateTime(dtpfechaInicio.EditValue)) < 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaTransferenciaPartes, "LOGI_FECH_DEVO"), 2);
                    dtpfechaDevolucion.Focus();
                }
                return bolRpta;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                return bolRpta;
            }
        }

        private void dtgListado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgListado.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodUCCompTransfer")
                    {
                        e.Handled = true;
                        EstadoForm(false, false, true);
                        DetallesTranferParte();
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void DetallesTranferParte()
        {
            try
            {
                GlobalClass.ip.SeleccionarTab(tbDetalleTrans);
                EstadoForm(false, false, true);
                btnCambioComp.IsEnabled = false;
                if (Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdEstadoTransfer")) == 2 || Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdEstadoTransfer")) == 3)
                {
                    dtpfechaDevolucion.IsEnabled = false;
                    dtpfechaInicio.IsEnabled = false;
                    trvCompOrigen.Focusable = false;
                    trvCompDestino.Focusable = false;
                    btnCambioComp.IsEnabled = false;
                    CboTipoTransfer.IsEnabled = false;
                    TxTComentarios.IsReadOnly = true;
                }
                dtpfechaInicio.IsEnabled = false;
                CboControlDestino.IsEnabled = false;
                CboControlOrigen.IsEnabled = false;
                skTreeblock01.Visibility = Visibility.Visible;
                skTreeblock02.Visibility = Visibility.Visible;

                CboTipoTransfer.SelectedIndexChanged -= new RoutedEventHandler(CboTipoTransfer_SelectedIndexChanged);
                dtpfechaInicio.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(FechaInicio_EditValueChanged);
                dtpfechaDevolucion.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpfechaDevolucion_EditValueChanged);
                TxTComentarios.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(TxTComentarios_EditValueChanged);

                gintIdUCCompTransfer = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdUCCompTransfer"));
                ObjETP.IdUCCompTransfer = gintIdUCCompTransfer;
                tblDetalleTP = objBTP.UCCompTransfer_GetItem(ObjETP);

                lblCodigo.Content = tblDetalleTP.Rows[0]["CodUCCompTransfer"].ToString();
                CboTipoTransfer.EditValue = Convert.ToInt32(tblDetalleTP.Rows[0]["IdTipoTransfer"]);
                dtpfechaInicio.EditValue = Convert.ToDateTime(tblDetalleTP.Rows[0]["FechaTransfer"]);
                if (Convert.ToInt32(CboTipoTransfer.EditValue) != 2)
                {
                    dtpfechaDevolucion.EditValue = Convert.ToDateTime(tblDetalleTP.Rows[0]["FechaDevolucion"]);
                    dtpfechaDevolucion.Visibility = Visibility.Visible;
                    lblFechaDevol.Visibility = Visibility.Visible;
                }
                else
                {
                    dtpfechaDevolucion.Visibility = Visibility.Hidden;
                    lblFechaDevol.Visibility = Visibility.Hidden;
                }
                string[] bitacora = tblDetalleTP.Rows[0]["Bitacora"].ToString().Split('|');
                foreach (string item in bitacora)
                {
                    lstvBitacora.Items.Add(item);
                }
                TxTComentarios.Text = tblDetalleTP.Rows[0]["Observacion"].ToString();
                CboControlOrigen.EditValue = Convert.ToInt32(tblDetalleTP.Rows[0]["IdUCOrigen"]);
                CboControlDestino.EditValue = Convert.ToInt32(tblDetalleTP.Rows[0]["IdUCDestino"]);
                LbLEstado.Content = tblDetalleTP.Rows[0]["Estado"].ToString();

                lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblDetalleTP.Rows[0]["UsuarioCreacion"].ToString(), tblDetalleTP.Rows[0]["FechaCreacion"].ToString(), tblDetalleTP.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblDetalleTP.Rows[0]["UsuarioModificacion"].ToString(), tblDetalleTP.Rows[0]["FechaModificacion"].ToString(), tblDetalleTP.Rows[0]["HostModificacion"].ToString());

                FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                TxTComentarios.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(TxTComentarios_EditValueChanged);
                dtpfechaDevolucion.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpfechaDevolucion_EditValueChanged);
                dtpfechaInicio.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(FechaInicio_EditValueChanged);
                CboTipoTransfer.SelectedIndexChanged += new RoutedEventHandler(CboTipoTransfer_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void DetallesTranferParteDevolicion()
        {
            try
            {
                btnCambioComp.IsEnabled = false;
                GlobalClass.ip.SeleccionarTab(tbDetalleTrans);
                EstadoForm(false, false, true);
                if (Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdEstadoTransfer")) == 2 || Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdEstadoTransfer")) == 3)
                {
                    dtpfechaDevolucion.IsEnabled = false;
                    dtpfechaInicio.IsEnabled = false;
                    trvCompOrigen.Focusable = false;
                    trvCompDestino.Focusable = false;
                    btnCambioComp.IsEnabled = false;
                    CboTipoTransfer.IsEnabled = false;
                }
                CboControlDestino.IsEnabled = false;
                CboControlOrigen.IsEnabled = false;
                skTreeblock01.Visibility = Visibility.Visible;
                skTreeblock02.Visibility = Visibility.Visible;

                CboTipoTransfer.SelectedIndexChanged -= new RoutedEventHandler(CboTipoTransfer_SelectedIndexChanged);
                dtpfechaInicio.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(FechaInicio_EditValueChanged);
                dtpfechaDevolucion.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpfechaDevolucion_EditValueChanged);
                TxTComentarios.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(TxTComentarios_EditValueChanged);

                ObjETP.IdUCCompTransfer = Convert.ToInt32(dtgListado.GetFocusedRowCellValue("IdUCCompTransfer"));
                tblDetalleTP = objBTP.UCCompTransfer_GetItem(ObjETP);

                lblCodigo.Content = tblDetalleTP.Rows[0]["CodUCCompTransfer"].ToString();
                CboTipoTransfer.EditValue = Convert.ToInt32(tblDetalleTP.Rows[0]["IdTipoTransfer"]);
                dtpfechaInicio.EditValue = Convert.ToDateTime(tblDetalleTP.Rows[0]["FechaTransfer"].ToString());
                dtpfechaDevolucion.EditValue = Convert.ToDateTime(tblDetalleTP.Rows[0]["FechaDevolucion"].ToString());
                string[] bitacora = tblDetalleTP.Rows[0]["Bitacora"].ToString().Split('|');
                foreach (string item in bitacora)
                {
                    lstvBitacora.Items.Add(item);
                }
                TxTComentarios.Text = tblDetalleTP.Rows[0]["Observacion"].ToString();
                CboControlOrigen.EditValue = Convert.ToInt32(tblDetalleTP.Rows[0]["IdUCDestino"]);
                CboControlDestino.EditValue = Convert.ToInt32(tblDetalleTP.Rows[0]["IdUCOrigen"]);
                LbLEstado.Content = tblDetalleTP.Rows[0]["Estado"].ToString();

                FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();

                dtpfechaDevolucion.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(dtpfechaDevolucion_EditValueChanged);
                TxTComentarios.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(TxTComentarios_EditValueChanged);
                dtpfechaInicio.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(FechaInicio_EditValueChanged);
                CboTipoTransfer.SelectedIndexChanged += new RoutedEventHandler(CboTipoTransfer_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void FechaInicio_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                if (!gbolNuevo)
                {
                    gintEstado = 4;
                    CambiarEstadoTranPartes();
                }
                EstadoForm(false, true, false);
                dtpfechaDevolucion.EditValue = Convert.ToDateTime(dtpfechaInicio.EditValue).AddDays(gintDiasAdd);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void rbnTodos_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ListarTransferencias();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void rbnProgramado_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ListarTransferencias();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void rbnReprogramado_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ListarTransferencias();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void rbnDevuelto_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ListarTransferencias();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void rbnCerrado_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ListarTransferencias();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtpfechaDevolucion_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                if (!gbolDevolver)
                {
                    EstadoForm(false, true, false);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void TxTComentarios_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                if (!gbolDevolver)
                {
                    EstadoForm(false, true, false);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalClass.GeneraImpresion(gintIdMenu, gintIdUCCompTransfer);
            }
            catch { }
        }
    }
}
