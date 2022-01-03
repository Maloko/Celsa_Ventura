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
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Editors;
using System.Data;
using System.Collections.ObjectModel;
using System.IO;
using Entities;
using Business;
using Utilitarios;
using System.Text.RegularExpressions;

namespace AplicacionSistemaVentura.PAQ01_Definicion
{
    public partial class GestPerfilNeumatico : UserControl
    {
        string gstrEtiquetaPerfilNeumatico = "GestPerfilNeumatico";
        int CantidadUC = 0;
        DataTable gtblPerfilNeumaticoEje = new DataTable();

        IList<ClsNrollantas> gIlstComboLlantas;
        Boolean bolNuevo = false; Boolean bolEdicion = false;
        E_PerfilNeumatico objPerfilNeumatico = new E_PerfilNeumatico();
        E_PerfilNeumaticoEje objPerfilNeumaticoEje = new E_PerfilNeumaticoEje();
        E_TablaMaestra objTablaMaestra = new E_TablaMaestra();
        B_PerfilNeumatico objBPerfilNeumatico = new B_PerfilNeumatico();
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();
        Utilitarios.DebugHandler Debug = new Utilitarios.DebugHandler();
        int gintIdMenu = 0, gintIdPerilNeumatico = 0;
        DateTime fechamodificacion;
        public class Neumatico
        {
            //En esta clase se declara los campos de la grilla
            public string Eje { get; set; }
            public string NroLlantas { get; set; }
            public ObservableCollection<string> Llantas { get; set; } //Una colleccion que sirve para llenar los combos
            public Neumatico() { Llantas = new ObservableCollection<string>(); } // Una variable para referenciar el campo a mostrar de los combos
        }

        public class ClsNrollantas
        {
            public string Id { get; set; }
            public string Text { get; set; }
        }
        private IList<ClsNrollantas> DataComboLlantas()
        {
            List<ClsNrollantas> ListCombo = new List<ClsNrollantas>();
            objTablaMaestra.IdTabla = 5;
            DataTable tblFinal= B_TablaMaestra.TablaMaestra_Combo(objTablaMaestra);
            for (int j = 0; j < tblFinal.Rows.Count; j++)
            {
                ListCombo.Add(new ClsNrollantas()
                {
                    Id = tblFinal.Rows[j]["Valor"].ToString(),
                    Text = tblFinal.Rows[j]["Valor"].ToString()
                });
            }
            return ListCombo;
        }

        public GestPerfilNeumatico()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        private void UserControl_Loaded()
        {
            try
            {
                cboLlanRepu.SelectedIndexChanged -= new RoutedEventHandler(cboLlanRepu_SelectedIndexChanged);
                cboNroEje.SelectedIndexChanged -= new RoutedEventHandler(cboNroEje_SelectedIndexChanged);
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                                
                tabLista.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "TAB0_CAPT");
                ListarPerfilNeumatico();
                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tabLista);

                E_TablaMaestra ObjETablaMaestra = new E_TablaMaestra();
                ObjETablaMaestra.IdTabla = 0;
                DataView tvwTablaMaestra = B_TablaMaestra.TablaMaestra_Combo(ObjETablaMaestra).DefaultView;

                cboLlanRepu.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 7", tvwTablaMaestra);
                cboLlanRepu.DisplayMember = "Valor";
                cboLlanRepu.ValueMember = "Valor";
                cboLlanRepu.IsTextEditable = false;

                cboEstado.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 4", tvwTablaMaestra);
                cboEstado.DisplayMember = "Descripcion";
                cboEstado.ValueMember = "IdColumna";
                cboEstado.IsTextEditable = false;

                cboNroEje.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 6", tvwTablaMaestra);
                cboNroEje.DisplayMember = "Valor";
                cboNroEje.ValueMember = "Valor";
                cboNroEje.IsTextEditable = false;

                gtblPerfilNeumaticoEje.Columns.Clear();
                gtblPerfilNeumaticoEje.Columns.Add("IdPerfilNeumaticoEje", Type.GetType("System.Int32"));
                gtblPerfilNeumaticoEje.Columns.Add("IdPerfilNeumatico", Type.GetType("System.Int32"));
                gtblPerfilNeumaticoEje.Columns.Add("Eje", Type.GetType("System.String"));
                gtblPerfilNeumaticoEje.Columns.Add("NroLlantas", Type.GetType("System.String"));
                gtblPerfilNeumaticoEje.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                gtblPerfilNeumaticoEje.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                gtblPerfilNeumaticoEje.Columns.Add("DetailSource", Type.GetType("System.Object"));

                gIlstComboLlantas = DataComboLlantas();

                #region VisualizacionBotonImprimir
                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                if (!VisualizaBotonImprimirDetalle)
                {
                    btnImprimir.Visibility = System.Windows.Visibility.Hidden;
                }
                #endregion

                cboNroEje.SelectedIndexChanged += new RoutedEventHandler(cboNroEje_SelectedIndexChanged);
                cboLlanRepu.SelectedIndexChanged += new RoutedEventHandler(cboLlanRepu_SelectedIndexChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
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
                LimpiarControles();
                LimpiarImagenes();
                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tabLista);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgPerfNeum.VisibleRowCount == 0) { return; }
                DetallePerfilNeumatico();
                EstadoForm(false, true, false);
                GlobalClass.ip.SeleccionarTab(tabDetalle);
                txtDesc.Focus();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CantidadUC != 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "LOGI_EDIT_ASIG"), 2);
                    return;
                }
                if (ValidaCampoObligado()) return;
                if (ValidaLogicaNegocio()) return;

                if ((bolNuevo == true) && (bolEdicion == false))
                {
                    objPerfilNeumatico.IdPerfilNeumatico = 0;
                    objPerfilNeumatico.CodPerfilNeumatico = "";
                    objPerfilNeumatico.PerfilNeumatico = txtDesc.Text.Trim();
                    objPerfilNeumatico.NroEjes = Convert.ToInt32(cboNroEje.EditValue);
                    objPerfilNeumatico.NroLlantaRepuesto = Convert.ToInt32(cboLlanRepu.EditValue);
                    objPerfilNeumatico.Observacion = txtComen.Text.Trim();
                    objPerfilNeumatico.IdEstadoPN = Convert.ToInt32(cboEstado.EditValue);
                    objPerfilNeumatico.FlagActivo = 1;
                    objPerfilNeumatico.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                    objPerfilNeumatico.FechaModificacion = DateTime.Now;
                    gtblPerfilNeumaticoEje.Columns.Remove("DetailSource");
                    int rpta= objBPerfilNeumatico.PerfilNeumatico_UpdateCascade(objPerfilNeumatico, gtblPerfilNeumaticoEje);
                    if (rpta == 1)// Ok
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "GRAB_NUEV"), 1);
                    }
                    else if (rpta == 0)// ya fue modificado
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)// PK
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "GRAB_CONC"), 2);
                        return;
                    }
                    EstadoForm(false, false, true);
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "GRAB_NUEV"), 1);

                }
                else if ((bolNuevo == false) && (bolEdicion == true))
                {
                    objPerfilNeumatico.IdPerfilNeumatico = Convert.ToInt32(dtgPerfNeum.GetFocusedRowCellValue("IdPerfilNeumatico"));
                    objPerfilNeumatico.CodPerfilNeumatico = txtCodigoPN.Text.Trim();
                    objPerfilNeumatico.PerfilNeumatico = txtDesc.Text.Trim();
                    objPerfilNeumatico.NroEjes = Convert.ToInt32(cboNroEje.EditValue);
                    objPerfilNeumatico.NroLlantaRepuesto = Convert.ToInt32(cboLlanRepu.EditValue);
                    objPerfilNeumatico.Observacion = txtComen.Text.Trim();
                    objPerfilNeumatico.IdEstadoPN = Convert.ToInt32(cboEstado.EditValue);
                    objPerfilNeumatico.FlagActivo = 1;
                    objPerfilNeumatico.FechaModificacion = fechamodificacion;
                    objPerfilNeumatico.IdUsuarioCreacion = Utilitarios.Utilitarios.gintIdUsuario;
                    gtblPerfilNeumaticoEje.Columns.Remove("DetailSource");
                    int rpta = objBPerfilNeumatico.PerfilNeumatico_UpdateCascade(objPerfilNeumatico, gtblPerfilNeumaticoEje);
                    if (rpta == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "GRAB_EDIT"), 1);
                    }
                    else if (rpta == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpta == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "GRAB_CONC"), 2);
                        return;
                    }
                    EstadoForm(false, false, true);
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "GRAB_EDIT"), 1);
                }

                LimpiarControles();
                LimpiarImagenes();
                ListarPerfilNeumatico();
                GlobalClass.ip.SeleccionarTab(tabLista);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            finally
            {
                GlobalClass.Columna_AddIFnotExits(gtblPerfilNeumaticoEje, "DetailSource", Type.GetType("System.Object"));
            }
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LimpiarControles();
                LimpiarImagenes();
                txtCodigoPN.Text = "Nuevo Código";
                cboLlanRepu.SelectedIndex = -1;
                cboNroEje.SelectedIndex = -1;
                cboEstado.SelectedIndex = 0; //Activo por defecto
                cboEstado.IsEnabled = false;
                EstadoForm(true, false, false);
                GlobalClass.ip.SeleccionarTab(tabDetalle);
                txtDesc.Focus();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboEstado_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboNroLlanta_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {                
                ComboBoxEdit cboNroLlanta = (ComboBoxEdit)sender;
                if (!cboNroLlanta.IsEditorActive) return;//Si no fue hecho por el click
                int IdSelected =Convert.ToInt32( dtgPerfNeumEje.GetCellValue( dtgPerfNeumEje.GetSelectedRowHandles()[0],"IdPerfilNeumaticoEje"));
                for (int i = 0; i < gtblPerfilNeumaticoEje.Rows.Count; i++)
                {
                    if (IdSelected == Convert.ToInt32(gtblPerfilNeumaticoEje.Rows[i]["IdPerfilNeumaticoEje"]))
                    {
                        gtblPerfilNeumaticoEje.Rows[i]["NroLlantas"] = Convert.ToInt32(cboNroLlanta.EditValue);
                        break;
                    }
                }
                string idEje = dtgPerfNeumEje.GetFocusedRowCellValue("Eje").ToString();
                PintarNeumatico(idEje, Convert.ToInt32(cboNroLlanta.EditValue));
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboNroEje_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                int IdPerfilNeumatico = 0;
                if (bolEdicion)
                    IdPerfilNeumatico = Convert.ToInt32(dtgPerfNeum.GetFocusedRowCellValue("IdPerfilNeumatico"));

                LimpiarImagenesDeGrilla(gridEjes);
                int CantEjes = Convert.ToInt32(cboNroEje.Text);
                PintarEje(CantEjes);

                //Si la agregamos nuevas filas de detalle
                if (dtgPerfNeumEje.VisibleRowCount < CantEjes)
                {
                    for (int i = dtgPerfNeumEje.VisibleRowCount; i < CantEjes; i++)
                    {
                        DataRow F = gtblPerfilNeumaticoEje.NewRow();
                        string MaxId = Utilitarios.Utilitarios.IIfBlankZero(Utilitarios.Utilitarios.IIfNullBlank(gtblPerfilNeumaticoEje.Compute("Max(IdPerfilNeumaticoEje)", "")));
                        F["IdPerfilNeumaticoEje"] = 1 + Convert.ToInt32(MaxId);
                        F["IdPerfilNeumatico"] = IdPerfilNeumatico;
                        F["Eje"] = "E" + Utilitarios.Utilitarios.NumeroChar2(i+1);
                        F["NroLlantas"] = 2;
                        //Solo el 1er eje(i=1) tiene 1 llanta 
                        F["DetailSource"] = (i != 0) ? gIlstComboLlantas.Where(x => x.Id != "1") : gIlstComboLlantas;
                        F["FlagActivo"] = true;
                        F["Nuevo"] = true;
                        gtblPerfilNeumaticoEje.Rows.Add(F);
                    }
                }
                else
                {
                    for (int i = CantEjes; i < gtblPerfilNeumaticoEje.Rows.Count; i++)
                    {
                        if ((bool)gtblPerfilNeumaticoEje.Rows[i]["Nuevo"] == false)
                            gtblPerfilNeumaticoEje.Rows[i]["FlagActivo"] = false;
                        else
                        {
                            gtblPerfilNeumaticoEje.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                }
                LimpiarImagenesDeGrilla(gridNeumaticos);
                gtblPerfilNeumaticoEje.DefaultView.RowFilter = "FlagActivo = True";
                dtgPerfNeumEje.ItemsSource = gtblPerfilNeumaticoEje.DefaultView;
                for (int i = 0; i < gtblPerfilNeumaticoEje.DefaultView.Count; i++)
                {
                    string IdEje=gtblPerfilNeumaticoEje.DefaultView[i]["Eje"].ToString();
                    int CantLlantas=Convert.ToInt32(gtblPerfilNeumaticoEje.DefaultView[i]["NroLlantas"]);
                    PintarNeumatico(IdEje, CantLlantas);
                }
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboLlanRepu_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                LimpiarImagenesDeGrilla(gridRepuestos);
                PintarLlantaRepuesto(Convert.ToInt32(cboLlanRepu.EditValue));
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        
        private void DetallePerfilNeumatico()        
        {
            try
            {
                RemoverEventoControles();
                gintIdPerilNeumatico = Convert.ToInt32(dtgPerfNeum.GetFocusedRowCellValue("IdPerfilNeumatico").ToString());
                objPerfilNeumatico.IdPerfilNeumatico = gintIdPerilNeumatico;
                CantidadUC = 0;
                CantidadUC = Convert.ToInt32(dtgPerfNeum.GetFocusedRowCellValue("CantUC"));

                DataTable tblConsulta = B_PerfilNeumatico.PerfilNeumatico_GetItem(objPerfilNeumatico);
                if (tblConsulta.Rows.Count > 0)
                {                                     
                    txtCodigoPN.Text = tblConsulta.Rows[0]["CodPerfilNeumatico"].ToString();
                    txtDesc.Text = tblConsulta.Rows[0]["PerfilNeumatico"].ToString();
                    txtComen.Text = tblConsulta.Rows[0]["Observacion"].ToString();
                    cboNroEje.EditValue = tblConsulta.Rows[0]["NroEjes"].ToString();
                    cboLlanRepu.EditValue = tblConsulta.Rows[0]["NroLlantaRepuesto"].ToString();
                    cboEstado.EditValue = Convert.ToInt32(tblConsulta.Rows[0]["IdEstadoPN"]);
                    cboEstado.IsEnabled = true;
                    fechamodificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                    if (Convert.ToInt32(tblConsulta.Rows[0]["IdEstadoPN"]) == 1)
                    {
                        txtDesc.IsReadOnly = false;
                        cboNroEje.IsEnabled = true;
                        cboLlanRepu.IsEnabled = true;
                        txtComen.IsReadOnly = false;
                        dtgPerfNeumEje.IsEnabled = true;
                    }
                    else
                    {
                        txtDesc.IsReadOnly = true;
                        cboNroEje.IsEnabled = false;
                        cboLlanRepu.IsEnabled = false;
                        txtComen.IsReadOnly = true;
                        dtgPerfNeumEje.IsEnabled = false;
                    }

                    LimpiarImagenes();
                    PintarEje(Convert.ToInt32(cboNroEje.EditValue));
                    PintarLlantaRepuesto(Convert.ToInt32(tblConsulta.Rows[0]["NroLlantaRepuesto"].ToString()));

                    objPerfilNeumaticoEje.IdPerfilNeumatico = objPerfilNeumatico.IdPerfilNeumatico;
                    DataTable tblPerfilNeumaticoEjes = B_PerfilNeumaticoEje.PerfilNeumaticoEje_List(objPerfilNeumaticoEje);
                    gtblPerfilNeumaticoEje.Rows.Clear();
                    for (int i = 0; i < tblPerfilNeumaticoEjes.Rows.Count; i++)
                    {
                        DataRow F = gtblPerfilNeumaticoEje.NewRow();
                        F["IdPerfilNeumaticoEje"] = tblPerfilNeumaticoEjes.Rows[i]["IdPerfilNeumaticoEje"];
                        F["IdPerfilNeumatico"] = tblPerfilNeumaticoEjes.Rows[i]["IdPerfilNeumatico"];
                        F["Eje"] = tblPerfilNeumaticoEjes.Rows[i]["Eje"];
                        F["NroLlantas"] = tblPerfilNeumaticoEjes.Rows[i]["NroLlantas"];
                        F["FlagActivo"] = tblPerfilNeumaticoEjes.Rows[i]["FlagActivo"];
                        F["Nuevo"] = false;
                        // solo el 1er eje puede tener 1 llanta
                        F["DetailSource"] = (i != 0) ? gIlstComboLlantas.Where(x => x.Id != "1") : gIlstComboLlantas;
                        gtblPerfilNeumaticoEje.Rows.Add(F);

                        string IdEje = tblPerfilNeumaticoEjes.Rows[i]["Eje"].ToString();
                        int Llantas = Convert.ToInt32(Utilitarios.Utilitarios.IIfBlankZero(tblPerfilNeumaticoEjes.Rows[i]["NroLlantas"].ToString()));
                        PintarNeumatico(IdEje, Llantas);  
                    }

                    gtblPerfilNeumaticoEje.DefaultView.RowFilter = "FlagActivo = True";
                    dtgPerfNeumEje.ItemsSource = gtblPerfilNeumaticoEje.DefaultView;
                    

                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblConsulta.Rows[0]["UsuarioCreacion"].ToString(), tblConsulta.Rows[0]["FechaCreacion"].ToString(), tblConsulta.Rows[0]["HostCreacion"].ToString());
                    lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblConsulta.Rows[0]["UsuarioModificacion"].ToString(), tblConsulta.Rows[0]["FechaModificacion"].ToString(), tblConsulta.Rows[0]["HostModificacion"].ToString());
                    
                }
                
                RegresarEventoControles();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgPerfNeum_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgPerfNeum.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodPerfilNeumatico")
                    {
                        e.Handled = true;
                        EstadoForm(false, false, true);
                        GlobalClass.ip.SeleccionarTab(tabDetalle);
                        DetallePerfilNeumatico();
                    }
                }
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
                    bolNuevo = stdNuevo; bolEdicion = stdEditar;
                }
                else if (stdForzar == false)
                {
                    if (bolNuevo == false)
                    {
                        bolNuevo = stdNuevo; bolEdicion = stdEditar;
                    }
                }
                if ((bolNuevo == false) && (bolEdicion == false))
                {
                    tabDetalle.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "TAB1_CONS");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "BTNG_CONS");                                        
                }
                else if ((bolNuevo == true) && (bolEdicion == false))
                {
                    tabDetalle.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "TAB1_NUEV");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "BTNG_NUEV");
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", Utilitarios.Utilitarios.gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((bolNuevo == false) && (bolEdicion == true))
                {
                    tabDetalle.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "TAB1_EDIT");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "BTNG_EDIT");                    
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LimpiarControles()
        {
            try
            {
                RemoverEventoControles();
                txtCodigoPN.Text = "";
                txtDesc.Text = "";
                txtComen.Text = "";
                cboNroEje.SelectedIndex = -1;
                cboLlanRepu.SelectedIndex = -1;
                cboEstado.SelectedIndex = -1;
                dtgPerfNeumEje.ItemsSource = null;
                txtDesc.IsReadOnly = false;
                cboNroEje.IsEnabled = true;
                cboLlanRepu.IsEnabled = true;
                txtComen.IsReadOnly = false;
                dtgPerfNeumEje.IsEnabled = true;
                gtblPerfilNeumaticoEje.Rows.Clear();
                RegresarEventoControles();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LimpiarImagenesDeGrilla(Grid grilla)
        {            
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
            {
                var control = VisualTreeHelper.GetChild(grilla, i);
                if (control is Image)
                    (control as Image).Visibility = Visibility.Hidden;
            }
        }

        private void LimpiarImagenes()
        {
            try
            {
                imgTimon.Visibility = Visibility.Hidden;
                LimpiarImagenesDeGrilla(gridEjes);
                LimpiarImagenesDeGrilla(gridNeumaticos);
                LimpiarImagenesDeGrilla(gridRepuestos);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarPerfilNeumatico()
        {
            try
            {                
                objPerfilNeumatico.FlagActivo = 1;
                dtgPerfNeum.ItemsSource = B_PerfilNeumatico.PerfilNeumatico_List(objPerfilNeumatico);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void PintarEje(int intNroEjes)
        {
            try
            {
                imgTimon.Visibility = (intNroEjes >= 1) ? Visibility.Visible : Visibility.Hidden;
                string Imagenes = string.Empty;
                for (int i = 1; i <= intNroEjes; i++)
                    Imagenes += "imgEje" + i.ToString() + ",";
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gridEjes); i++)
                {
                    var control = VisualTreeHelper.GetChild(gridEjes, i);
                    if (control is Image)
                        if (Imagenes.Contains((control as Image).Name))
                            (control as Image).Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void PintarNeumatico(string strEje, int Llantas)
        {
            try
            {
                //limpiamos losNeumaticos del Eje
                string ImagenesEje = string.Empty;
                for (int i = 1; i <= 12; i++)
                    ImagenesEje += "img" + strEje + "L" + Utilitarios.Utilitarios.NumeroChar2(i) + ",";
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gridNeumaticos); i++)
                {
                    var control = VisualTreeHelper.GetChild(gridNeumaticos, i);
                    if (control is Image)
                        if (ImagenesEje.Contains((control as Image).Name))
                            (control as Image).Visibility = Visibility.Hidden;
                }

                string Imagenes = string.Empty;
                for (int i = 1; i <= Llantas; i++)
                    Imagenes += "img" + strEje + "L" + Utilitarios.Utilitarios.NumeroChar2(i) + ",";

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gridNeumaticos); i++)
                {
                    var control = VisualTreeHelper.GetChild(gridNeumaticos, i);
                    if (control is Image)
                        if (Imagenes.Contains((control as Image).Name))
                            (control as Image).Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void PintarLlantaRepuesto(int intLR)
        {
            try
            {
                string Imagenes = string.Empty;
                for (int i = 1; i <= intLR; i++)
                    Imagenes += "imgLR" + i.ToString() + ",";
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gridRepuestos); i++)
                {
                    var control = VisualTreeHelper.GetChild(gridRepuestos, i);
                    if (control is Image)
                        if (Imagenes.Contains((control as Image).Name))
                            (control as Image).Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
                
        private void txtComen_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtDesc_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var textboxSender = (TextBox)sender;
                var cursorPosition = textboxSender.SelectionStart;
                textboxSender.Text = Regex.Replace(textboxSender.Text, "[^a-zA-Z0-9áéíóú ]", "");
                textboxSender.SelectionStart = cursorPosition;
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
                
        private bool ValidaCampoObligado()
        {
            bool bolRpta = false;
            try
            {
                if (txtDesc.Text.Trim() == "")
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "OBLI_DESC"), 2);
                    txtDesc.Focus();
                }
                else if (Utilitarios.Utilitarios.IsNumeric(txtDesc.Text))
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "OBLI_DESCNUM"), 2);
                    txtDesc.Focus();
                }
                else if (cboNroEje.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "OBLI_EJE"), 2);
                    cboNroEje.Focus();
                }
                else if (cboEstado.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "OBLI_ESTA"), 2);
                    cboEstado.Focus();
                }
                else if (cboLlanRepu.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "OBLI_LLAN"), 2);
                    cboLlanRepu.Focus();
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

        private bool ValidaLogicaNegocio()
        {
            bool bolRpta = false;
            try
            {
                if ((bolNuevo == true) && (bolEdicion == false))
                {
                    objPerfilNeumatico.IdPerfilNeumatico = 0;
                }
                else if ((bolNuevo == false) && (bolEdicion == true))
                {
                    objPerfilNeumatico.IdPerfilNeumatico = Convert.ToInt32(dtgPerfNeum.GetFocusedRowCellValue("IdPerfilNeumatico").ToString());
                }
                objPerfilNeumatico.PerfilNeumatico = txtDesc.Text.Trim();
                DataTable tblConsulta = B_PerfilNeumatico.PerfilNeumatico_GetItemByDesc(objPerfilNeumatico);
                if (tblConsulta.Rows.Count > 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilNeumatico, "LOGI_DUPL"), 2);
                    txtDesc.Focus();
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

        private void txtPlantilla_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void RemoverEventoControles()
        {
            cboLlanRepu.SelectedIndexChanged -= new RoutedEventHandler(cboLlanRepu_SelectedIndexChanged); //Esto si funciona bien
            cboNroEje.SelectedIndexChanged -= new RoutedEventHandler(cboNroEje_SelectedIndexChanged);
            cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
            txtComen.EditValueChanged -= new EditValueChangedEventHandler(txtPlantilla_EditValueChanged);
            txtDesc.EditValueChanged -= new EditValueChangedEventHandler(txtPlantilla_EditValueChanged);

        }

        private void RegresarEventoControles()
        {
            txtComen.EditValueChanged += new EditValueChangedEventHandler(txtPlantilla_EditValueChanged);
            txtDesc.EditValueChanged += new EditValueChangedEventHandler(txtPlantilla_EditValueChanged);
            cboLlanRepu.SelectedIndexChanged += new RoutedEventHandler(cboLlanRepu_SelectedIndexChanged);
            cboNroEje.SelectedIndexChanged += new RoutedEventHandler(cboNroEje_SelectedIndexChanged);
            cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalClass.GeneraImpresion(gintIdMenu, gintIdPerilNeumatico);
            }
            catch { }
        }

    }
}