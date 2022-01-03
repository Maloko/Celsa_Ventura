using System;
using System.ComponentModel;
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
using DevExpress.Xpf.Editors.Controls;
using System.Data;
using Entities;
using InterfazMTTO;
using Business;
using Utilitarios;
using System.Text.RegularExpressions;

namespace AplicacionSistemaVentura.PAQ01_Definicion
{
    public partial class GestPerfilUC : UserControl
    {
        public GestPerfilUC()
        {
            //prueb
            InitializeComponent();
            UserControl_Loaded();
        }


        #region
        E_PerfilTarea objEPerfilTarea = new E_PerfilTarea();
        E_Perfil objEPerfil = new E_Perfil();
        E_Tarea objETarea = new E_Tarea();
        E_PerfilComp objEPerfilComp = new E_PerfilComp();
        B_Perfil objPerfil = new B_Perfil();
        B_PerfilComp objPerfilComp = new B_PerfilComp();
        B_PerfilNeumatico objPerfilNeumatico = new B_PerfilNeumatico();
        B_PerfilComp_Actividad objPerfilCompActividad = new B_PerfilComp_Actividad();
        B_PerfilDetalle objPerfilDetalle = new B_PerfilDetalle();
        B_PerfilTarea objPerfilTarea = new B_PerfilTarea();
        B_Actividad objActividad = new B_Actividad();
        B_Herramienta objHerramienta = new B_Herramienta();
        B_PerfilComp_Ciclo objPerfilCompCiclo = new B_PerfilComp_Ciclo();
        B_Ciclo objCiclo = new B_Ciclo();
        B_Tarea objTarea = new B_Tarea();
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        Boolean gbolFlagInactivo = false;
        Boolean gbolFlagEditarDetAct = false;
        Utilitarios.Utilitarios ObjUtil = new Utilitarios.Utilitarios();
        Utilitarios.ErrorHandler Error = new Utilitarios.ErrorHandler();
        Utilitarios.DebugHandler Debug = new Utilitarios.DebugHandler();

        InterfazMTTO.iSBO_BE.BEUDUC UDUC = new InterfazMTTO.iSBO_BE.BEUDUC();
        InterfazMTTO.iSBO_BE.BETUDUCList tucuclist = new InterfazMTTO.iSBO_BE.BETUDUCList();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        InterfazMTTO.iSBO_BE.BEOITMList BEOITMList = new InterfazMTTO.iSBO_BE.BEOITMList();
        InterfazMTTO.iSBO_BE.BEOITMList BEOITMGetItem = new InterfazMTTO.iSBO_BE.BEOITMList();

        DataTable tblPerfilCompActividad = new DataTable();
        DataTable tblListadoCiclos = new DataTable();
        DataTable tblPerfilComponentes;
        DataTable tblPerfilTarea;
        DataTable tblPerfilDetalleHerrEsp;
        DataTable tblPerfilDetalleRepuesto;
        DataTable tblPerfilDetalleConsumible;
        DataTable tblPerfilCompCiclo;
        DataTable tblCiclosExistentes;
        DataTable tblActividades;
        DataTable tblPerfilNeumatico;

        DataView dtvPerfilNeumatico = new DataView();
        DataView dtvListadoCiclos = new DataView();
        DataView tblTipoCiclo = new DataView();
        B_TablaMaestra objTablaMaestra = new B_TablaMaestra();
        E_TablaMaestra objTM = new E_TablaMaestra();
        DataView dtvMaestra = new DataView();
        DataRow gdrActividad;
        DataRow gdrDetActividad;

        int IdPerfil;
        int tipo;
        int IdPerfilNew = 1;
        int IdPerfilCompCicloNew = 1;
        int gintIdPerfilComp = 0;
        int gintNivelPerfilComp = 0;
        string gstrUsuario = Utilitarios.Utilitarios.gstrUsuario;
        int gintValorTiempoDefecto = 0;
        int gintTiempoDefecto = 0;
        string gstrCicloDefecto = "";

        string gstrEtiquetaPerfilUC = "GestPerfilUC";

        int gintIdMenu = 0, gintIdPerilUC = 0;

        Boolean gbolAllActividades = true;
        DateTime FechaModificacion;
        private void OnFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                (sender as Control).Background = System.Windows.Media.Brushes.LightYellow;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void OutFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                (sender as Control).Background = System.Windows.Media.Brushes.White;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void AbrirCompSAP_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarBotones(false, true)) { return; }
                cboCompSAP.SelectedIndexChanged -= new RoutedEventHandler(cboCompSAP_SelectedIndexChanged);
                cboCompSAP.SelectedIndex = -1;
                cboCompSAP.SelectedIndexChanged += new RoutedEventHandler(cboCompSAP_SelectedIndexChanged);
                stkPanelComponenteSAP.Visibility = System.Windows.Visibility.Visible;
                CambiarBotonDefecto(false);
                btnAgregarComp.IsDefault = true;
                btnCancelComp.IsCancel = true;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void AbrirConsumibles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarBotones(true, false)) { return; }
                DataTable tblConsumiblesExistentes = (DataTable)dtgCons.ItemsSource;

                DataTable tblConsumibles = new DataTable();
                tblConsumibles.Columns.Add("Articulo");
                tblConsumibles.Columns.Add("IdArticulo");
                string IdCodigoArticulo = "";
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                BEOITMList = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("C", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    for (int i = 0; i < BEOITMList.Count; i++)
                    {
                        DataRow dr;
                        dr = tblConsumibles.NewRow();
                        dr["Articulo"] = BEOITMList[i].DescripcionArticulo;
                        dr["IdArticulo"] = BEOITMList[i].CodigoArticulo;
                        tblConsumibles.Rows.Add(dr);
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }

                for (int i = 0; i < tblConsumiblesExistentes.Rows.Count; i++)
                {
                    IdCodigoArticulo += tblConsumiblesExistentes.Rows[i]["IdArticulo"].ToString() + "', '";
                }

                if (tblConsumiblesExistentes.Rows.Count != 0)
                {
                    IdCodigoArticulo = IdCodigoArticulo.Remove(IdCodigoArticulo.Length - 4);
                    string filtro = "IdArticulo  NOT IN ('" + IdCodigoArticulo + "')";

                    tblConsumibles.DefaultView.RowFilter = filtro;
                    tblConsumibles = tblConsumibles.DefaultView.ToTable(true);
                    cboConsumibles.ItemsSource = null;
                }
                cboConsumibles.ItemsSource = tblConsumibles;
                cboConsumibles.DisplayMember = "Articulo";
                cboConsumibles.ValueMember = "IdArticulo";

                lblTituloConsumibles.Text = "Agregar Nuevo Consumible";
                lblConsumibles.Visibility = Visibility.Collapsed;
                cboConsumibles.Visibility = Visibility.Visible;
                CambiarBotonDefecto(false);
                btnAceptarConsumibles.IsDefault = true;
                btnCancelarConsumibles.IsCancel = true;
                stkPanelConsumibles.Visibility = System.Windows.Visibility.Visible;
                spCantidadConsumible.Value = 1;
                cboConsumibles.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void AbrirHerramientaEspecial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarBotones(true, false)) { return; }
                DataTable tblComboHerramienta = objHerramienta.Herramienta_Combo();
                DataTable tblHerramientaExistente = (DataTable)dtgHerrEspe.ItemsSource;

                string IdHerramienta = "";
                for (int i = 0; i < tblHerramientaExistente.Rows.Count; i++)
                {
                    IdHerramienta += tblHerramientaExistente.Rows[i]["IdArticulo"].ToString() + ",";
                }
                if (tblHerramientaExistente.Rows.Count != 0)
                {
                    tblComboHerramienta.DefaultView.RowFilter = "IdHerramienta NOT IN (" + IdHerramienta + ")";
                    tblComboHerramienta = tblComboHerramienta.DefaultView.ToTable(true);
                }
                cboHerramientaEspecial.ItemsSource = tblComboHerramienta;
                cboHerramientaEspecial.DisplayMember = "Herramienta";
                cboHerramientaEspecial.ValueMember = "IdHerramienta";
                spCantidadHerramienta.Value = 1;
                spCantidadHerramienta.MinValue = 1;

                lblTituloHerramientas.Text = "Agregar Nueva Herramienta Especial";
                CambiarBotonDefecto(false);
                btnAceptarHerramientaEspecial.IsDefault = true;
                btnCancelarHerramientaEspecial.IsCancel = true;
                lblHerramienta.Visibility = Visibility.Collapsed;
                cboHerramientaEspecial.Visibility = Visibility.Visible;
                stkPanelHerramientas.Visibility = System.Windows.Visibility.Visible;
                cboHerramientaEspecial.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void AbrirRepuesto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarBotones(true, false)) { return; }
                DataTable tblRepuestoExistentes = (DataTable)dtgRepu.ItemsSource;

                DataTable tblRepuesto = new DataTable();
                tblRepuesto.Columns.Add("Articulo");
                tblRepuesto.Columns.Add("IdArticulo");
                string IdCodigoArticulo = "";
                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                BEOITMList = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("R", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    for (int i = 0; i < BEOITMList.Count; i++)
                    {
                        DataRow dr;
                        dr = tblRepuesto.NewRow();
                        dr["Articulo"] = BEOITMList[i].DescripcionArticulo;
                        dr["IdArticulo"] = BEOITMList[i].CodigoArticulo;
                        tblRepuesto.Rows.Add(dr);
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }

                for (int i = 0; i < tblRepuestoExistentes.Rows.Count; i++)
                {
                    IdCodigoArticulo += tblRepuestoExistentes.Rows[i]["IdArticulo"].ToString() + "', '";
                }

                if (tblRepuestoExistentes.Rows.Count != 0)
                {
                    IdCodigoArticulo = IdCodigoArticulo.Remove(IdCodigoArticulo.Length - 4);
                    string filtro = "IdArticulo  NOT IN ('" + IdCodigoArticulo + "')";
                    tblRepuesto.DefaultView.RowFilter = filtro;
                    tblRepuesto = tblRepuesto.DefaultView.ToTable(true);
                    cboRepuesto.ItemsSource = null;
                }
                cboRepuesto.ItemsSource = tblRepuesto;
                cboRepuesto.DisplayMember = "Articulo";
                cboRepuesto.ValueMember = "IdArticulo";

                lblTituloRepuestos.Text = "Agregar Nuevo Repuesto";
                lblRepuesto.Visibility = Visibility.Collapsed;
                cboRepuesto.Visibility = Visibility.Visible;
                CambiarBotonDefecto(false);
                btnAceptarRepusto.IsDefault = true;
                btnCancelarRepuesto.IsCancel = true;
                stkPanelRepuestos.Visibility = System.Windows.Visibility.Visible;
                spCantidadRepuesto.Value = 1;
                cboRepuesto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void AbrirTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarBotones(true, false)) { return; }
                objETarea.IdActividad = 0;
                objETarea.Actividad = ((System.Data.DataRowView)lstActi.Items[lstActi.SelectedIndex]).Row["Actividad"].ToString();
                cboTarea.ItemsSource = objTarea.Tarea_ComboByAct(objETarea);
                cboTarea.DisplayMember = "Tarea";
                cboTarea.ValueMember = "IdTarea";

                DataTable tblComboTareas = (DataTable)cboTarea.ItemsSource;
                DataTable tblTareasExistentes = (DataTable)dtgTarea.ItemsSource;

                cboEstadoTarea.EditValue = 1;
                string IdTarea = "";
                for (int i = 0; i < tblTareasExistentes.Rows.Count; i++)
                {
                    IdTarea += tblTareasExistentes.Rows[i]["IdTarea"].ToString() + ",";
                }

                if (tblTareasExistentes.Rows.Count != 0)
                {
                    tblComboTareas.DefaultView.RowFilter = "IdTarea NOT IN (" + IdTarea + ")";
                    tblComboTareas = tblComboTareas.DefaultView.ToTable(true);
                    cboTarea.ItemsSource = null;

                    cboTarea.ItemsSource = tblComboTareas;
                    cboTarea.DisplayMember = "Tarea";
                    cboTarea.ValueMember = "IdTarea";
                }
                lblTituloTareas.Text = "Agregar Nueva Tarea";
                lblTarea.Visibility = Visibility.Collapsed;
                cboTarea.Visibility = Visibility.Visible;
                CambiarBotonDefecto(false);
                btnAgregarTarea.IsDefault = true;
                btnCancelarTarea.IsCancel = true;
                stkPanelTareas.Visibility = System.Windows.Visibility.Visible;
                txtHorasHombre.Clear();
                cboTarea.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        void CambiarBotonDefecto(bool estado)
        {
            btnGrabar.IsDefault = estado;
            btnCancelar.IsCancel = estado;
        }
        private void btnActividadAbrir(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarBotones(false, true)) { return; }

                DataTable tblActividad_Grid = new DataTable();
                tblActividad_Grid.Columns.Add("IsChecked", Type.GetType("System.Boolean"));
                tblActividad_Grid.Columns.Add("Actividad", Type.GetType("System.String"));
                tblActividad_Grid.Columns.Add("IdActividad", Type.GetType("System.Int32"));

                for (int i = 0; i < tblActividades.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = tblActividad_Grid.NewRow();
                    dr["IsChecked"] = false;
                    dr["Actividad"] = tblActividades.Rows[i]["Actividad"].ToString();
                    dr["IdActividad"] = Convert.ToInt32(tblActividades.Rows[i]["IdActividad"]);
                    tblActividad_Grid.Rows.Add(dr);
                }

                DataView dtvActividadesExistente = (DataView)lstActi.ItemsSource;

                string IdActividad = "";
                for (int i = 0; i < dtvActividadesExistente.Count; i++)
                {
                    IdActividad += dtvActividadesExistente[i]["IdActividad"].ToString() + ",";
                }

                if (dtvActividadesExistente.Count != 0)
                {
                    tblActividad_Grid.DefaultView.RowFilter = "IdActividad NOT IN (" + IdActividad + ")";
                    tblActividad_Grid = tblActividad_Grid.DefaultView.ToTable(true);
                }

                CambiarBotonDefecto(false);
                btnAgregarActividad.IsDefault = true;
                btnCancelarActividad.IsCancel = true;
                dtgActividades.ItemsSource = tblActividad_Grid;
                stkActividad.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnAgregarActividad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstActi.SelectionChanged -= new SelectionChangedEventHandler(lstActi_SelectionChanged);
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    DataTable tblDtgActividades = (DataTable)dtgActividades.ItemsSource;

                    foreach (DataRow drAct in tblDtgActividades.Select("IsChecked = true"))
                    {
                        DataRow row;
                        row = tblPerfilCompActividad.NewRow();
                        row["Actividad"] = drAct["Actividad"].ToString();
                        row["IdPerfilComp"] = Convert.ToInt32(trm.IdMenu);
                        int IdPerfilComp = 0;
                        if (tblPerfilCompActividad.Rows.Count == 0)
                        {
                            IdPerfilComp = 1;
                        }
                        else
                        {
                            IdPerfilComp = Convert.ToInt32(tblPerfilCompActividad.Compute("max(IdPerfilCompActividad)", "")) + 1;
                        }

                        row["IdPerfilCompActividad"] = IdPerfilComp;
                        row["Nuevo"] = true;
                        row["IdActividad"] = Convert.ToInt32(drAct["IdActividad"]);
                        row["Activo"] = false;
                        row["Uso"] = false;
                        row["FlagActivo"] = true;
                        tblPerfilCompActividad.Rows.Add(row);
                    }
                    gbolAllActividades = true;
                    //DataView dtvActividades = new DataView(tblPerfilCompActividad);
                    tblPerfilCompActividad.DefaultView.RowFilter = "IdPerfilComp = " + trm.IdMenu;
                    lstActi.ItemsSource = tblPerfilCompActividad.DefaultView;
                    lstActi.DisplayMemberPath = "Actividad";
                    lstActi.SelectedValuePath = "IdPerfilCompActividad";
                    CambiarBotonDefecto(true);
                    btnAgregarActividad.IsDefault = false;
                    btnCancelarActividad.IsCancel = false;
                    stkActividad.Visibility = System.Windows.Visibility.Hidden;
                    EstadoForm(false, true, false);
                }
                lstActi.SelectionChanged += new SelectionChangedEventHandler(lstActi_SelectionChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        void LlenarNuevosCiclos()
        {
            dtvListadoCiclos.RowFilter = "IdCiclo IN (3,4)";
            for (int i = 0; i < dtvListadoCiclos.Count; i++)
            {
                DataRow row2;
                row2 = tblPerfilCompCiclo.NewRow();
                row2["IdPerfilCompCiclo"] = IdPerfilCompCicloNew;
                row2["IdPerfilComp"] = IdPerfilNew;
                row2["Descripcion"] = dtvListadoCiclos[i]["TipoCiclo"].ToString();
                row2["IdCiclo"] = Convert.ToInt32(dtvListadoCiclos[i]["IdCiclo"]);
                row2["Ciclo"] = Convert.ToString(dtvListadoCiclos[i]["Ciclo"]);
                row2["FrecuenciaCambio"] = 0;
                row2["IdEstadoPCC"] = 1;
                row2["Estado"] = "Activo";
                row2["FlagActivo"] = true;
                row2["Nuevo"] = true;
                tblPerfilCompCiclo.Rows.Add(row2);
                IdPerfilCompCicloNew++;
            }
        }
        private void btnAgregarComponente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtComoponente.Text.Trim() == "")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_DESC_COMP"), 2);
                    txtComoponente.Focus();
                    return;
                }
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    foreach (DataRow drExiste in tblPerfilComponentes.Select("PerfilComp = '" + txtComoponente.Text + "' AND Nivel = " + trm.Nivel))
                    {
                        GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_DUPL_COMP"), trm.Nivel), 2);
                        txtComoponente.Focus();
                        return;
                    }
                    trvComp.ItemsSource = null;
                    Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                    int IdPadre = Convert.ToInt32(trm.IdMenuPadre);
                    DataRow row;
                    row = tblPerfilComponentes.NewRow();
                    row["IdPerfilCompPadre"] = IdPadre;
                    row["IdPerfilComp"] = IdPerfilNew;
                    if (IdPadre == 0)
                    {
                        row["IdTipoDetalle"] = 1;
                    }
                    else
                    {
                        row["IdTipoDetalle"] = 2;
                    }
                    row["PerfilComp"] = txtComoponente.Text;
                    row["Nuevo"] = true;

                    if (trm.IdMenuPadre == 0) { row["NuevoPadre"] = false; }
                    else { row["NuevoPadre"] = trm.BoolNuevo; }

                    row["CodigoSAP"] = "";
                    row["DescripcionSAP"] = "";
                    row["Nivel"] = trm.Nivel;
                    row["Estado"] = 1;
                    row["FlagNeumatico"] = false;
                    row["FlagActivo"] = true;
                    tblPerfilComponentes.Rows.Add(row);

                    if (IdPadre != 0)
                    {
                        LlenarNuevosCiclos();
                    }

                    DataView dtvComp = tblPerfilComponentes.DefaultView;
                    dtvComp.RowFilter = "FlagActivo = true";

                    Utilitarios.TreeViewModel.tblListarPerfilComponentes = dtvComp.ToTable();
                    trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponente(1000, null);
                    IdPerfilNew++;
                    txtComoponente.Clear();
                    stkPanelComponente.Visibility = System.Windows.Visibility.Hidden;
                    CambiarBotonDefecto(true);
                    btnAceptarComponente.IsDefault = false;
                    btnCancelarComponente.IsCancel = false;
                    EstadoForm(false, true, false);
                }
                else
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnAgregarComponenteHijo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtComponenteHijo.Text.Trim() == "")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_DESC_COMP"), 2);
                    txtComponenteHijo.Focus();
                    return;
                }
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    foreach (DataRow drExiste in tblPerfilComponentes.Select("PerfilComp = '" + txtComponenteHijo.Text + "' AND Nivel = " + (trm.Nivel + 1)))
                    {
                        GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_DUPL_COMP"), (trm.Nivel + 1)), 2);
                        txtComponenteHijo.Focus();
                        return;
                    }
                    trvComp.ItemsSource = null;
                    Utilitarios.TreeViewModel.LimpiarDatosTreeview();

                    int IdPadre = Convert.ToInt32(trm.IdMenu);
                    DataRow row;
                    row = tblPerfilComponentes.NewRow();
                    row["IdPerfilCompPadre"] = IdPadre;
                    row["IdPerfilComp"] = IdPerfilNew;
                    row["PerfilComp"] = txtComponenteHijo.Text;
                    row["CodigoSAP"] = "";
                    row["DescripcionSAP"] = "";
                    row["Nuevo"] = true;
                    row["NuevoPadre"] = trm.BoolNuevo;
                    row["FlagNeumatico"] = false;
                    row["Nivel"] = trm.Nivel + 1;
                    row["Estado"] = 1;
                    row["FlagActivo"] = true;
                    if (IdPadre == 0)
                    {
                        row["IdTipoDetalle"] = 1;
                    }
                    else
                    {
                        row["IdTipoDetalle"] = 2;
                    }
                    tblPerfilComponentes.Rows.Add(row);

                    DataView dtvComp = tblPerfilComponentes.DefaultView;
                    dtvComp.RowFilter = "FlagActivo = true";

                    Utilitarios.TreeViewModel.tblListarPerfilComponentes = dtvComp.ToTable();

                    trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponente(1000, null);

                    if (IdPadre != 0)
                    {
                        LlenarNuevosCiclos();
                    }
                    IdPerfilNew++;
                    txtComponenteHijo.Clear();
                    stkPanelComponenteHijo.Visibility = System.Windows.Visibility.Hidden;
                    CambiarBotonDefecto(true);
                    btnAceptarComponenteHijo.IsDefault = false;
                    btnCancelarComponenteHijo.IsCancel = false;
                    EstadoForm(false, true, false);
                }
                else
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnAgregarConsumible_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboConsumibles.SelectedIndex == -1 && !gbolFlagEditarDetAct)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_CONS"), 2);
                    cboConsumibles.Focus();
                    return;
                }
                if (spCantidadConsumible.Value == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_CANT_CONS"), 2);
                    spCantidadConsumible.Focus();
                    return;
                }
                try
                {
                    Convert.ToInt32(spCantidadConsumible.Value);
                }
                catch
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_CANT_CONS"), 2);
                    spCantidadConsumible.Focus();
                }

                if (!gbolFlagEditarDetAct)
                {
                    DataRow row;
                    row = tblPerfilDetalleConsumible.NewRow();
                    if (tblPerfilDetalleConsumible.Rows.Count == 0) { row["IdPerfilDetalle"] = 1; }
                    else { row["IdPerfilDetalle"] = Convert.ToInt32(tblPerfilDetalleConsumible.Compute("max(IdPerfilDetalle)", "")) + 1; }
                    row["IdPerfilCompActividad"] = Convert.ToInt32(lstActi.SelectedValue);
                    row["IdArticulo"] = Convert.ToString(cboConsumibles.EditValue);
                    row["Articulo"] = Convert.ToString(cboConsumibles.Text);
                    row["IdTipoArticulo"] = 3;
                    row["Nuevo"] = true;
                    row["Cantidad"] = Convert.ToInt32(spCantidadConsumible.Value);
                    row["FlagActivo"] = true;
                    tblPerfilDetalleConsumible.Rows.Add(row);
                }
                else
                {
                    gdrDetActividad["Cantidad"] = Convert.ToInt32(spCantidadConsumible.Value);
                }

                gbolFlagEditarDetAct = false;
                CambiarBotonDefecto(true);
                btnAceptarConsumibles.IsDefault = false;
                btnCancelarConsumibles.IsCancel = false;
                dtgCons.ItemsSource = null;
                DataView tdvPerfilDetalle = tblPerfilDetalleConsumible.DefaultView;
                tdvPerfilDetalle.RowFilter = "IdPerfilCompActividad = " + Convert.ToInt32(lstActi.SelectedValue) + " and IdTipoArticulo = 3";
                dtgCons.ItemsSource = FiltroDetallesComponentes(Convert.ToInt32(lstActi.SelectedValue), tdvPerfilDetalle.ToTable());
                VerificarActividadActiva();
                stkPanelConsumibles.Visibility = System.Windows.Visibility.Hidden;
                cboConsumibles.SelectedIndex = -1;
                spCantidadConsumible.Value = 1;
                EstadoForm(false, true, false);

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnAgregarHerramientaEspecial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboHerramientaEspecial.SelectedIndex == -1 && !gbolFlagEditarDetAct)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_HERR"), 2);
                    cboHerramientaEspecial.Focus();
                    return;
                }
                if (spCantidadHerramienta.Value == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_CANT_HERR"), 2);
                    spCantidadHerramienta.Focus();
                    return;
                }
                try
                {
                    Convert.ToDouble(spCantidadHerramienta.Text);
                }
                catch
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_CANT_HERR"), 2);
                    spCantidadHerramienta.Focus();
                }
                if (!gbolFlagEditarDetAct)
                {
                    DataRow row;
                    row = tblPerfilDetalleHerrEsp.NewRow();
                    if (tblPerfilDetalleHerrEsp.Rows.Count == 0) { row["IdPerfilDetalle"] = 1; }
                    else { row["IdPerfilDetalle"] = Convert.ToInt32(tblPerfilDetalleHerrEsp.Compute("max(IdPerfilDetalle)", "")) + 1; }
                    row["IdPerfilCompActividad"] = Convert.ToInt32(lstActi.SelectedValue);
                    row["IdArticulo"] = Convert.ToString(cboHerramientaEspecial.EditValue);
                    row["Articulo"] = Convert.ToString(cboHerramientaEspecial.Text);
                    row["IdTipoArticulo"] = 1;
                    row["Nuevo"] = true;
                    row["Cantidad"] = Convert.ToInt32(spCantidadHerramienta.Value);
                    row["FlagActivo"] = true;
                    tblPerfilDetalleHerrEsp.Rows.Add(row);
                }
                else
                {
                    gdrDetActividad["Cantidad"] = Convert.ToInt32(spCantidadHerramienta.Value);
                }
                gbolFlagEditarDetAct = false;
                CambiarBotonDefecto(true);
                btnAceptarHerramientaEspecial.IsDefault = false;
                btnCancelarHerramientaEspecial.IsCancel = false;
                dtgHerrEspe.ItemsSource = null;
                DataView tdvPerfilDetalle = new DataView(tblPerfilDetalleHerrEsp);
                tdvPerfilDetalle.RowFilter = "IdPerfilCompActividad = " + Convert.ToInt32(lstActi.SelectedValue);
                dtgHerrEspe.ItemsSource = FiltroDetallesComponentes(Convert.ToInt32(lstActi.SelectedValue), tdvPerfilDetalle.ToTable());
                VerificarActividadActiva();
                stkPanelHerramientas.Visibility = System.Windows.Visibility.Hidden;
                cboHerramientaEspecial.SelectedIndex = -1;
                spCantidadHerramienta.Value = 1;
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnAgregarRepuesto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboRepuesto.SelectedIndex == -1 && !gbolFlagEditarDetAct)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_REPU"), 2);
                    return;
                }
                if (spCantidadRepuesto.Value == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_CANT_REPU"), 2);
                    return;
                }
                try
                {
                    Convert.ToDouble(spCantidadRepuesto.Value);
                }
                catch
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_CANT_REPU"), 2);
                }
                if (!gbolFlagEditarDetAct)
                {
                    DataRow row;
                    row = tblPerfilDetalleRepuesto.NewRow();
                    if (tblPerfilDetalleRepuesto.Rows.Count == 0) { row["IdPerfilDetalle"] = 1; }
                    else { row["IdPerfilDetalle"] = Convert.ToInt32(tblPerfilDetalleRepuesto.Compute("max(IdPerfilDetalle)", "")) + 1; }
                    row["IdPerfilCompActividad"] = Convert.ToInt32(lstActi.SelectedValue);
                    row["IdArticulo"] = Convert.ToString(cboRepuesto.EditValue);
                    row["Articulo"] = Convert.ToString(cboRepuesto.Text);
                    row["IdTipoArticulo"] = 2;
                    row["Nuevo"] = true;
                    row["Cantidad"] = Convert.ToInt32(spCantidadRepuesto.Value);
                    row["FlagActivo"] = true;
                    tblPerfilDetalleRepuesto.Rows.Add(row);
                }
                else
                {
                    gdrDetActividad["Cantidad"] = Convert.ToInt32(spCantidadRepuesto.Value);
                }

                gbolFlagEditarDetAct = false;
                CambiarBotonDefecto(true);
                btnAceptarRepusto.IsDefault = false;
                btnCancelarRepuesto.IsCancel = false;
                dtgRepu.ItemsSource = null;
                DataView tdvPerfilDetalle = tblPerfilDetalleRepuesto.DefaultView;
                tdvPerfilDetalle.RowFilter = "IdPerfilCompActividad = " + Convert.ToInt32(lstActi.SelectedValue);
                dtgRepu.ItemsSource = FiltroDetallesComponentes(Convert.ToInt32(lstActi.SelectedValue), tdvPerfilDetalle.ToTable());
                VerificarActividadActiva();
                stkPanelRepuestos.Visibility = System.Windows.Visibility.Hidden;
                cboRepuesto.SelectedIndex = -1;
                spCantidadRepuesto.Value = 1;
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnAgregarTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboTarea.SelectedIndex == -1 && !gbolFlagEditarDetAct)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_TARE"), 2);
                    cboTarea.Focus();
                    return;
                }
                else if (txtHorasHombre.Text == "")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_HORA_TARE"), 2);
                    txtHorasHombre.Focus();
                    return;
                }
                else if (txtHorasHombre.Text == "00:00")
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_HORA_TARE"), 2);
                    txtHorasHombre.Focus();
                    return;
                }

                if (cboEstadoTarea.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_ESTA_TARE"), 2);
                    cboEstadoTarea.Focus();
                    return;
                }
                double hrest = Convert.ToDouble(txtHorasHombre.Text.Substring(11, 2)) + (Convert.ToDouble(txtHorasHombre.Text.Substring(14, 2)) / 60);

                if (!gbolFlagEditarDetAct)
                {
                    DataRow row;
                    row = tblPerfilTarea.NewRow();
                    if (tblPerfilTarea.Rows.Count == 0) { row["IdPerfilTarea"] = 1; }
                    else { row["IdPerfilTarea"] = Convert.ToInt32(tblPerfilTarea.Compute("max(IdPerfilTarea)", "")) + 1; }
                    row["IdPerfilCompActividad"] = Convert.ToInt32(lstActi.SelectedValue);
                    row["IdTarea"] = cboTarea.EditValue;
                    row["Tarea"] = cboTarea.Text;
                    row["HorasHombre"] = hrest;
                    row["Nuevo"] = true;
                    row["IdEstado"] = Convert.ToInt32(cboEstadoTarea.EditValue);
                    row["Estado"] = Convert.ToString(cboEstadoTarea.Text);
                    row["FlagActivo"] = true;
                    tblPerfilTarea.Rows.Add(row);
                }
                else
                {
                    gdrDetActividad["HorasHombre"] = hrest;
                    gdrDetActividad["IdEstado"] = Convert.ToInt32(cboEstadoTarea.EditValue);
                    gdrDetActividad["Estado"] = Convert.ToString(cboEstadoTarea.Text);
                }

                gbolFlagEditarDetAct = false;
                dtgTarea.ItemsSource = null;
                CambiarBotonDefecto(true);
                btnAgregarTarea.IsDefault = false;
                btnCancelarTarea.IsCancel = false;
                dtgTarea.ItemsSource = FiltroDetallesComponentes(Convert.ToInt32(lstActi.SelectedValue), tblPerfilTarea);
                VerificarActividadActiva();
                stkPanelTareas.Visibility = System.Windows.Visibility.Hidden;
                txtHorasHombre.Clear();
                cboTarea.SelectedIndex = -1;
                cboEstadoTarea.SelectedIndex = -1;
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tabControl1.SelectedIndex = 0;
                LimpiarControles();
                IdPerfil = 0;
                EstadoForm(false, false, true);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCancelarActividad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarBotonDefecto(true);
                btnAgregarActividad.IsDefault = false;
                btnCancelarActividad.IsCancel = false;
                gbolAllActividades = true;
                dtgActividades.RefreshData();
                dtgActividades.ItemsSource = null;
                stkActividad.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {

                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelarComp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarBotonDefecto(true);
                btnAgregarComp.IsDefault = false;
                btnCancelComp.IsCancel = false;
                cboCompSAP.SelectedIndexChanged -= new RoutedEventHandler(cboCompSAP_SelectedIndexChanged);
                cboCompSAP.SelectedIndex = -1;
                cboCompSAP.SelectedIndexChanged += new RoutedEventHandler(cboCompSAP_SelectedIndexChanged);
                stkPanelComponenteSAP.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCancelarComponente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarBotonDefecto(true);
                btnAceptarComponente.IsDefault = false;
                btnCancelarComponente.IsCancel = false;
                stkPanelComponente.Visibility = System.Windows.Visibility.Hidden;
                txtComoponente.Clear();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCancelarComponenteHijo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarBotonDefecto(true);
                btnAceptarComponenteHijo.IsDefault = false;
                btnCancelarComponenteHijo.IsCancel = false;
                stkPanelComponenteHijo.Visibility = System.Windows.Visibility.Hidden;
                txtComponenteHijo.Clear();
            }
            catch (Exception ex)
            {

                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCancelarConsumible_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarBotonDefecto(true);
                btnAceptarConsumibles.IsDefault = false;
                btnCancelarConsumibles.IsCancel = false;
                stkPanelConsumibles.Visibility = System.Windows.Visibility.Hidden;
                spCantidadConsumible.Value = 1;
                cboConsumibles.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCancelarHerramientaEspecial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarBotonDefecto(true);
                btnAceptarHerramientaEspecial.IsDefault = false;
                btnCancelarHerramientaEspecial.IsCancel = false;
                stkPanelHerramientas.Visibility = System.Windows.Visibility.Hidden;
                cboHerramientaEspecial.SelectedIndex = -1;
                spCantidadHerramienta.Value = 1;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCancelarRepuesto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarBotonDefecto(true);
                btnAceptarRepusto.IsDefault = false;
                btnCancelarRepuesto.IsCancel = false;
                stkPanelRepuestos.Visibility = System.Windows.Visibility.Hidden;
                spCantidadRepuesto.Value = 1;
                cboRepuesto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCancelarTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarBotonDefecto(true);
                btnAgregarTarea.IsDefault = false;
                btnCancelarTarea.IsCancel = false;
                stkPanelTareas.Visibility = System.Windows.Visibility.Hidden;
                txtHorasHombre.Clear();
                cboTarea.SelectedIndex = -1;
                cboEstadoTarea.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnCompAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboCompSAP.SelectedIndex != -1)
                {
                    TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                    if (trm != null)
                    {
                        int idPerfilComp = trm.IdMenu;

                        foreach (DataRow drPfComp in tblPerfilComponentes.Select("IdPerfilComp = " + idPerfilComp))
                        {
                            drPfComp["CodigoSAP"] = cboCompSAP.EditValue;
                            drPfComp["DescripcionSAP"] = cboCompSAP.Text;
                        }

                        lblCodiSAP.Text = cboCompSAP.EditValue.ToString();
                        lblCodiSAP.ToolTip = cboCompSAP.Text;
                        stkPanelComponenteSAP.Visibility = System.Windows.Visibility.Hidden;
                        cboCompSAP.SelectedIndexChanged -= new RoutedEventHandler(cboCompSAP_SelectedIndexChanged);
                        cboCompSAP.SelectedIndex = -1;
                        cboCompSAP.SelectedIndexChanged += new RoutedEventHandler(cboCompSAP_SelectedIndexChanged);
                        CambiarBotonDefecto(true);
                        btnAgregarComp.IsDefault = false;
                        btnCancelComp.IsCancel = false;
                        EstadoForm(false, true, false);
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarPerfilUnidadControl()
        {
            dtgPUC.ItemsSource = null;
            DataTable tblListaPerfil = new DataTable();
            tblListaPerfil = objPerfil.Perfil_List();
            tblListaPerfil.Columns.Add("TipoUnidad");

            foreach (DataRow drCompLis in tblListaPerfil.Select("TipoUnidad IS NULL OR TipoUnidad = ''"))
            {
                string IdTipoUnidad = drCompLis["IdTipoUnidad"].ToString();
                foreach (var item in tucuclist.Where(i => i.CodigoTipoUnidadControl == IdTipoUnidad))
                {
                    drCompLis["TipoUnidad"] = item.DescripcionTipoUnidadControl;
                }
            }

            dtgPUC.ItemsSource = tblListaPerfil;
        }
        private void BtnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarCampos()) { return; }

                E_PerfilComp objEPerfilComp = new E_PerfilComp();
                E_PerfilComp_Actividad objEPerfilComp_Actividad = new E_PerfilComp_Actividad();
                E_PerfilTarea objEPerfilTarea = new E_PerfilTarea();
                E_PerfilDetalle objEPerfilDetalle = new E_PerfilDetalle();
                E_PerfilComp_Ciclo objEPerfilCompCiclo = new E_PerfilComp_Ciclo();

                if (tblPerfilComponentes.Rows.Count <= 1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_COMP_PERF"), 2);
                    return;
                }
                foreach (DataRow drPFComp in tblPerfilComponentes.Select("IdTipoDetalle = 1"))
                {
                    int tienehijos = tblPerfilComponentes.Select("FlagActivo = 1 AND IdPerfilCompPadre = " + Convert.ToInt32(drPFComp["IdPerfilComp"])).Length;
                    if (tienehijos == 0)
                    {
                        GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_COMP"), drPFComp["PerfilComp"].ToString()), 2);
                        return;
                    }
                }
                if (tipo == 1)
                {
                    objEPerfil.Idperfil = 0;
                }
                else
                {
                    objEPerfil.Idperfil = Convert.ToInt32(dtgPUC.GetCellDisplayText(tblvPerfil.FocusedRowHandle, "IdPerfil"));
                }

                objEPerfil.Perfil = txtDescr.Text;
                objEPerfil.Idtipounidad = cboTipoUnid.EditValue.ToString();
                if (cboPerfNeum.EditValue != null)
                {
                    objEPerfil.Idperfilneumatico = Convert.ToInt32(cboPerfNeum.EditValue);
                }
                else
                {
                    objEPerfil.Idperfilneumatico = 0;
                }

                if (rbnActi.IsChecked == true)
                {
                    objEPerfil.Idestadop = 1;
                }
                else
                {
                    objEPerfil.Idestadop = 2;
                }

                objEPerfil.Flagactivo = true;
                objEPerfil.Idusuariocreacion = Utilitarios.Utilitarios.gintIdUsuario;
                objEPerfil.Idusuariomodificacion = Utilitarios.Utilitarios.gintIdUsuario;
                objEPerfil.IdCicloDefecto = Convert.ToInt32(cboCicloPerfil.EditValue);

                if (gintTiempoDefecto != 1)
                {
                    foreach (DataRow drCiclosTiempo in tblPerfilCompCiclo.Select("IdCiclo = 4"))
                    {
                        drCiclosTiempo["FrecuenciaCambio"] = Convert.ToDouble(drCiclosTiempo["FrecuenciaCambio"]) * gintValorTiempoDefecto;
                    }
                }

                try
                {
                    if (tipo == 1)
                    {
                        objEPerfil.Fechamodificacion = DateTime.Now;
                        if (ValidaLogicaNegocio() == true) { return; }
                        int rpta = objPerfil.Perfil_InsertMasivo(objEPerfil, tblPerfilComponentes, tblPerfilCompActividad, tblPerfilCompCiclo, tblPerfilTarea, tblPerfilDetalleHerrEsp, tblPerfilDetalleRepuesto, tblPerfilDetalleConsumible);
                        if (rpta == 1)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "GRAB_NUEV"), 1);
                        }
                        else if (rpta == 0)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_MODI"), 2);
                            return;
                        }
                        else if (rpta == 1205)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "GRAB_CONC"), 2);
                            return;
                        }
                    }
                    else if (tipo == 2)
                    {
                        if (ValidaLogicaNegocio() == true) { return; }
                        int IdPerfilCompNew = 1;
                        foreach (DataRow drComp in tblPerfilComponentes.Select("Nuevo = true"))
                        {
                            int IdPerfilComp = Convert.ToInt32(drComp["IdPerfilComp"]);
                            drComp["IdPerfilComp"] = IdPerfilCompNew;
                            foreach (DataRow drCompNew in tblPerfilComponentes.Select("IdPerfilCompPadre = " + IdPerfilComp + " AND Nuevo = true")) { drCompNew["IdPerfilCompPadre"] = IdPerfilCompNew; }
                            foreach (DataRow drAct in tblPerfilCompActividad.Select("IdPerfilComp = " + IdPerfilComp + " AND Nuevo = true")) { drAct["IdPerfilComp"] = IdPerfilCompNew; }
                            foreach (DataRow drCiclo in tblPerfilCompCiclo.Select("IdPerfilComp = " + IdPerfilComp + " AND Nuevo = true")) { drCiclo["IdPerfilComp"] = IdPerfilCompNew; }
                            IdPerfilCompNew++;
                        }
                        objEPerfil.Fechamodificacion = FechaModificacion;
                        int rpta = objPerfil.Perfil_InsertMasivo(objEPerfil, tblPerfilComponentes, tblPerfilCompActividad, tblPerfilCompCiclo, tblPerfilTarea, tblPerfilDetalleHerrEsp, tblPerfilDetalleRepuesto, tblPerfilDetalleConsumible);
                        if (rpta == 1)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "GRAB_EDIT"), 1);
                        }
                        else if (rpta == 0)
                        {
                            if (gintTiempoDefecto != 1)
                            {
                                foreach (DataRow drCiclosTiempo in tblPerfilCompCiclo.Select("IdCiclo = 4"))
                                {
                                    drCiclosTiempo["FrecuenciaCambio"] = Convert.ToDouble(drCiclosTiempo["FrecuenciaCambio"]) / gintValorTiempoDefecto;
                                }
                            }
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_MODI"), 2);
                            return;
                        }
                        else if (rpta == 1205)
                        {
                            if (gintTiempoDefecto != 1)
                            {
                                foreach (DataRow drCiclosTiempo in tblPerfilCompCiclo.Select("IdCiclo = 4"))
                                {
                                    drCiclosTiempo["FrecuenciaCambio"] = Convert.ToDouble(drCiclosTiempo["FrecuenciaCambio"]) / gintValorTiempoDefecto;
                                }
                            }
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "GRAB_CONC"), 2);
                            return;
                        }
                    }
                    LimpiarControles();
                    ListarPerfilUnidadControl();
                    IdPerfil = 0;
                    tabControl1.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                    GlobalClass.ip.Mensaje(ex.Message, 3);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void BtnModificarPerfil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgPUC.VisibleRowCount == 0) { return; }
                EstadoForm(false, true, true);
                tblPerfilCompActividad.Clear();
                tblPerfilComponentes.Clear();
                tblPerfilTarea.Clear();
                tblPerfilDetalleHerrEsp.Clear();
                tblPerfilDetalleRepuesto.Clear();
                tblPerfilDetalleConsumible.Clear();
                tblPerfilCompCiclo.Clear();
                tipo = 2;
                LlenarDataPerfil();
                tabItem2.IsEnabled = true;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnRaizNivel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    if (trm.IdMenuPadre == 1000)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_COMP_DIFE"), 2);
                        return;
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                    return;
                }
                CambiarBotonDefecto(false);
                btnAceptarComponente.IsDefault = true;
                btnCancelarComponente.IsCancel = true;
                txtComoponente.Focus();
                stkPanelComponente.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {

                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void btnRaizSubo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm == null)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                    return;
                }
                CambiarBotonDefecto(false);
                btnAceptarComponenteHijo.IsDefault = true;
                btnCancelarComponenteHijo.IsCancel = true;
                txtComponenteHijo.Focus();
                stkPanelComponenteHijo.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnRegistrarPerfil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                rbnActi.Checked -= new RoutedEventHandler(rbnActi_Checked);
                rbnInac.Checked -= new RoutedEventHandler(rbnInac_Checked);

                EstadoForm(true, false, true);
                dtvPerfilNeumatico.RowFilter = "IdEstadoPN = 1";
                LimpiarControles();
                txtCodiPUC.Text = "PFNuevo";
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();

                DataRow row;
                row = tblPerfilComponentes.NewRow();
                row["IdPerfilCompPadre"] = 1000;
                row["IdPerfilComp"] = 0;
                row["IdTipoDetalle"] = 0;
                row["PerfilComp"] = txtDescr.Text;
                row["Nuevo"] = false;
                row["NuevoPadre"] = false;
                row["CodigoSAP"] = "";
                row["DescripcionSAP"] = "";
                row["FlagNeumatico"] = false;
                row["FlagActivo"] = true;
                row["Nivel"] = 1;
                row["Estado"] = 1;
                tblPerfilComponentes.Rows.Add(row);

                Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblPerfilComponentes;
                trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponente(1000, null);

                tipo = 1;
                tabControl1.SelectedIndex = 1;
                rbnActi.IsChecked = true;
                rbnInac.IsEnabled = true;
                rbnActi.IsEnabled = true;
                tabItem2.IsEnabled = true;
                rbnActi.Checked += new RoutedEventHandler(rbnActi_Checked);
                rbnInac.Checked += new RoutedEventHandler(rbnInac_Checked);

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void CboEstado_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (trvComp.ItemsSource != null)
                {
                    TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                    if (trm != null)
                    {
                        int idPerfilComp = Convert.ToInt32(trm.IdMenu);
                        int estado = 0;
                        foreach (DataRow drPerComp in tblPerfilComponentes.Select("IdPerfilComp = " + idPerfilComp))
                        {
                            estado = Convert.ToInt32(drPerComp["Estado"]);
                        }

                        objEPerfilComp.Idperfilcomp = idPerfilComp;
                        DataTable tblCont = objPerfilComp.PerfilComp_GetBeforeDel(objEPerfilComp);

                        if (Convert.ToInt32(tblCont.Rows[0]["Contador"]) != 0)
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_ESTA_COMP_ASIG"), 2);
                            CboEstado.EditValue = estado;
                            return;
                        }

                        foreach (DataRow drPerComp in tblPerfilComponentes.Select("IdPerfilComp = " + idPerfilComp))
                        {
                            drPerComp["Estado"] = CboEstado.EditValue;
                        }

                        EstadoForm(false, true, false);
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void chkActivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idPerfilCompActividad = Convert.ToInt32(lstActi.SelectedValue);
                foreach (DataRow drPfAct in tblPerfilCompActividad.Select("idPerfilCompActividad = " + idPerfilCompActividad))
                {
                    if (chkActivo.IsChecked == true)
                    {
                        drPfAct["Activo"] = true;
                    }
                    else
                    {
                        drPfAct["Activo"] = false;
                    }
                }
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void chkComp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idPerfilCompActividad = Convert.ToInt32(lstActi.SelectedValue);
                foreach (DataRow drPfAct in tblPerfilCompActividad.Select("idPerfilCompActividad = " + idPerfilCompActividad))
                {
                    if (chkComp.IsChecked == true)
                    {
                        drPfAct["Uso"] = true;
                    }
                    else
                    {
                        drPfAct["Uso"] = false;
                    }
                }
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        void LimpiarControles()
        {
            try
            {
                CboEstado.SelectedIndexChanged -= new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                cboPerfNeum.SelectedIndexChanged -= new RoutedEventHandler(cboPerfNeum_SelectedIndexChanged);
                cboCicloPerfil.SelectedIndexChanged -= new RoutedEventHandler(cboCicloPerfil_SelectedIndexChanged);
                trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                cboTipoUnid.SelectedIndexChanged -= new RoutedEventHandler(cboTipoUnid_SelectedIndexChanged);
                rbnActi.Checked -= new RoutedEventHandler(rbnActi_Checked);
                rbnInac.Checked -= new RoutedEventHandler(rbnInac_Checked);

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(this.Contenedor); i++)
                {
                    TextBox txt = VisualTreeHelper.GetChild(this.Contenedor, i) as TextBox;
                    if (txt != null)
                    {
                        txt.Clear();
                    }
                }

                txtDescr.Text = "";
                trvComp.ItemsSource = null;
                lstActi.ItemsSource = null;

                lblPadre.Content = "";
                lblCodiSAP.Text = "";
                lblCodiSAP.ToolTip = null;
                txtCodiPUC.Text = "";

                cboCicloPerfil.SelectedIndex = -1;
                cboCompSAP.SelectedIndex = -1;
                cboConsumibles.SelectedIndex = -1;
                CboEstado.EditValue = -1;
                cboEstadoTarea.SelectedIndex = -1;
                cboHerramientaEspecial.SelectedIndex = -1;
                cboPerfNeum.SelectedIndex = -1;
                cboRepuesto.SelectedIndex = -1;
                cboTipoUnid.SelectedIndex = -1;
                dtgTarea.ItemsSource = null;
                dtgCiclo.ItemsSource = null;
                dtgCons.ItemsSource = null;
                dtgHerrEspe.ItemsSource = null;
                dtgRepu.ItemsSource = null;

                chkActivo.IsChecked = false;
                chkComp.IsChecked = false;

                chkActivo.IsEnabled = false;
                chkComp.IsEnabled = false;

                rbnActi.IsChecked = false;
                rbnInac.IsChecked = false;
                rbnActi.IsEnabled = true;
                rbnInac.IsEnabled = true;
                cboPerfNeum.IsReadOnly = false;
                cboPerfNeum.ToolTip = null;
                groupBox1.Header = "Información del componente";

                stkActividad.Visibility = Visibility.Hidden;
                stkPanelComponenteSAP.Visibility = Visibility.Hidden;
                stkPanelConsumibles.Visibility = Visibility.Hidden;
                stkPanelHerramientas.Visibility = Visibility.Hidden;
                stkPanelRepuestos.Visibility = Visibility.Hidden;
                stkPanelTareas.Visibility = Visibility.Hidden;

                tabControl2.SelectedIndex = 0;

                if (tblPerfilComponentes.Rows.Count > 0)
                {
                    tblPerfilComponentes.Rows.Clear();
                }
                if (tblPerfilTarea.Rows.Count > 0)
                {
                    tblPerfilTarea.Rows.Clear();
                }
                if (tblPerfilDetalleConsumible.Rows.Count > 0)
                {
                    tblPerfilDetalleConsumible.Rows.Clear();
                }
                if (tblPerfilDetalleHerrEsp.Rows.Count > 0)
                {
                    tblPerfilDetalleHerrEsp.Rows.Clear();
                }
                if (tblPerfilDetalleRepuesto.Rows.Count > 0)
                {
                    tblPerfilDetalleRepuesto.Rows.Clear();
                }
                if (tblPerfilCompActividad.Rows.Count > 0)
                {
                    tblPerfilCompActividad.Rows.Clear();
                }
                tabItem1.IsEnabled = true;
                tabItem2.IsEnabled = false;
                IdPerfilNew = 1;
                label001.IsEnabled = true;
                label7.IsEnabled = true;
                lblCodiSAP.IsEnabled = true;
                btnbuscarsap.IsEnabled = true;
                CboEstado.IsEnabled = true;
                groupBox3.IsEnabled = true;
                dtgCiclo.IsEnabled = true;
                rbnTitulo.IsEnabled = true;
                rbnComponente.IsEnabled = true;
                rbnTitulo.IsChecked = false;
                rbnComponente.IsChecked = false;


                groupBox1.IsEnabled = true;
                cboPerfNeum.IsEnabled = true;
                trvComp.IsEnabled = true;
                btnRaizSubo1.IsEnabled = true;
                buttonEdit1.IsEnabled = true;
                cboCicloPerfil.IsEnabled = true;
                cboTipoUnid.IsEnabled = true;
                txtDescr.IsReadOnly = false;

                gbolFlagInactivo = false;
                CambiarEstadoInactivo(false);
                groupBox1.IsEnabled = false;
                tblPerfilComponentes.Rows.Clear();
                tblPerfilCompActividad.Rows.Clear();
                tblPerfilCompCiclo.Rows.Clear();
                tblPerfilTarea.Rows.Clear();
                tblPerfilDetalleHerrEsp.Rows.Clear();
                tblPerfilDetalleRepuesto.Rows.Clear();
                tblPerfilDetalleConsumible.Rows.Clear();

                rbnActi.Checked += new RoutedEventHandler(rbnActi_Checked);
                rbnInac.Checked += new RoutedEventHandler(rbnInac_Checked);
                cboTipoUnid.SelectedIndexChanged += new RoutedEventHandler(cboTipoUnid_SelectedIndexChanged);
                trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                CboEstado.SelectedIndexChanged += new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                cboPerfNeum.SelectedIndexChanged += new RoutedEventHandler(cboPerfNeum_SelectedIndexChanged);
                cboCicloPerfil.SelectedIndexChanged += new RoutedEventHandler(cboCicloPerfil_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void lstActi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm == null) { return; }
                dtgTarea.ItemsSource = null;
                dtgHerrEspe.ItemsSource = null;
                dtgRepu.ItemsSource = null;
                dtgCons.ItemsSource = null;
                int IdPerfilCompActividad = Convert.ToInt32(lstActi.SelectedValue);
                int Activo = 0;
                chkActivo.IsChecked = false;
                chkComp.IsChecked = false;
                chkComp.IsEnabled = false;

                foreach (DataRow drPfCActiv in tblPerfilCompActividad.Select("IdPerfilComp = " + trm.IdMenu + " AND Uso = true"))
                {
                    Activo = 1; break;
                }
                foreach (DataRow drActiv in tblPerfilCompActividad.Select("IdPerfilCompActividad = " + IdPerfilCompActividad))
                {
                    gdrActividad = drActiv;
                    if (Convert.ToBoolean(drActiv["Activo"]) == true) { chkActivo.IsChecked = true; }
                    else { chkActivo.IsChecked = false; }
                    if (Convert.ToBoolean(drActiv["Uso"]) == true) { chkComp.IsChecked = true; if (!gbolFlagInactivo) { chkComp.IsEnabled = true; } }
                    else { chkComp.IsChecked = false; }
                }

                if (Activo == 0) { if (!gbolFlagInactivo) { chkComp.IsEnabled = true; } }

                dtgTarea.ItemsSource = FiltroDetallesComponentes(IdPerfilCompActividad, tblPerfilTarea);
                dtgHerrEspe.ItemsSource = FiltroDetallesComponentes(IdPerfilCompActividad, tblPerfilDetalleHerrEsp);
                dtgRepu.ItemsSource = FiltroDetallesComponentes(IdPerfilCompActividad, tblPerfilDetalleRepuesto);
                dtgCons.ItemsSource = FiltroDetallesComponentes(IdPerfilCompActividad, tblPerfilDetalleConsumible);
                VerificarActividadActiva();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private DataTable FiltroDetallesComponentes(int IdPerfilCompActividad, DataTable tblDetalle)
        {
            DataTable tblRetorno = new DataTable();
            DataRow[] Rows = tblDetalle.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND FlagActivo = true");
            if (Rows.Count() > 0) { tblRetorno = Rows.CopyToDataTable(); }
            return tblRetorno;
        }

        private void VerificarActividadActiva()
        {
            try
            {
                if (dtgTarea.VisibleRowCount == 0 && dtgHerrEspe.VisibleRowCount == 0 && dtgRepu.VisibleRowCount == 0 && dtgCons.VisibleRowCount == 0)
                {
                    chkActivo.IsChecked = false;
                    gdrActividad["Activo"] = false;
                }
                else
                {
                    chkActivo.IsChecked = true;
                    gdrActividad["Activo"] = true;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        void CambiarEstadoInactivo(bool Estado)
        {
            txtDescr.IsReadOnly = Estado;
            cboTipoUnid.IsEnabled = !Estado;
            cboCicloPerfil.IsEnabled = !Estado;
            buttonEdit1.IsEnabled = !Estado;
            btnRaizSubo1.IsEnabled = !Estado;
            btnActividadAbrir1.IsEnabled = !Estado;
            chkComp.IsEnabled = !Estado;
            chkActivo.IsEnabled = !Estado;
            gbTipoDetalle.IsEnabled = !Estado;
            lblCodiSAP.IsEnabled = !Estado;
            btnbuscarsap.IsEnabled = !Estado;
            CboEstado.IsEnabled = !Estado;
            cboPerfNeum.IsEnabled = !Estado;
            dtgCiclo.IsEnabled = !Estado;
            btnTareaSel.IsEnabled = !Estado;

            btnRaizNivel.IsEnabled = !Estado;
            btnRaizSubo.IsEnabled = !Estado;
            btnSelHE1.IsEnabled = !Estado;
            btnSelRepu1.IsEnabled = !Estado;
            btnSelCons1.IsEnabled = !Estado;

            menuAddComp.IsEnabled = !Estado;
            menuAddTitulo.IsEnabled = !Estado;
            menuCambiarDesc.IsEnabled = !Estado;
            menuDelSelec.IsEnabled = !Estado;
        }
        void LlenarDataPerfil()
        {
            try
            {
                GlobalClass.ip.SeleccionarTab(tabItem2);
                cboPerfNeum.SelectedIndexChanged -= new RoutedEventHandler(cboPerfNeum_SelectedIndexChanged);
                CboEstado.SelectedIndexChanged -= new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                cboTipoUnid.SelectedIndexChanged -= new RoutedEventHandler(cboTipoUnid_SelectedIndexChanged);
                cboCicloPerfil.SelectedIndexChanged -= new RoutedEventHandler(cboCicloPerfil_SelectedIndexChanged);
                rbnActi.Checked -= new RoutedEventHandler(rbnActi_Checked);
                rbnInac.Checked -= new RoutedEventHandler(rbnInac_Checked);

                var ValorGrilla = dtgPUC.GetCellDisplayText(tblvPerfil.FocusedRowHandle, "IdPerfil");
                IdPerfil = Convert.ToInt32(ValorGrilla);

                if (ValorGrilla != null || ValorGrilla == "")
                {
                    gintIdPerilUC = IdPerfil;
                    E_Perfil objE_Perfil = new E_Perfil();
                    E_PerfilComp objE_PerfilComp = new E_PerfilComp();
                    DataTable tblPerfil = new DataTable();
                    DataTable tblPerfilComponentesDatos = new DataTable();

                    objE_Perfil.Idperfil = Convert.ToInt32(ValorGrilla);
                    objE_PerfilComp.Idperfil = Convert.ToInt32(ValorGrilla);
                    objE_PerfilComp.Idestadopc = 1;
                    tblPerfil = objPerfil.Perfil_GetItem(objE_Perfil);

                    txtCodiPUC.Text = tblPerfil.Rows[0]["CodPerfil"].ToString();
                    txtDescr.Text = tblPerfil.Rows[0]["Perfil"].ToString();
                    cboTipoUnid.EditValue = Convert.ToString(tblPerfil.Rows[0]["IdTipoUnidad"]);
                    cboPerfNeum.EditValue = Convert.ToInt32(tblPerfil.Rows[0]["IdPerfilNeumatico"]);
                    cboCicloPerfil.EditValue = Convert.ToInt32(tblPerfil.Rows[0]["IdCicloDefecto"]);

                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblPerfil.Rows[0]["UsuarioCreacion"].ToString(), tblPerfil.Rows[0]["FechaCreacion"].ToString(), tblPerfil.Rows[0]["HostCreacion"].ToString());
                    lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblPerfil.Rows[0]["UsuarioModificacion"].ToString(), tblPerfil.Rows[0]["FechaModificacion"].ToString(), tblPerfil.Rows[0]["HostModificacion"].ToString());


                    if (Convert.ToInt32(tblPerfil.Rows[0]["IdEstadoP"].ToString()) == 1)
                    {
                        rbnActi.IsChecked = true;
                        rbnInac.IsChecked = false;
                        menuAddTitulo.IsEnabled = true;
                        menuAddComp.IsEnabled = true;
                        menuCambiarDesc.IsEnabled = true;
                        menuDelSelec.IsEnabled = true;
                    }
                    else
                    {
                        rbnActi.IsChecked = false;
                        rbnInac.IsChecked = true;
                        gbolFlagInactivo = true;
                        CambiarEstadoInactivo(true);
                    }

                    tabControl1.SelectedIndex = 1;
                    Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                    tblPerfilComponentes.Rows.Clear();
                    tblPerfilComponentesDatos = objPerfilComp.PerfilComp_List(objE_PerfilComp);
                    DataRow row1;
                    row1 = tblPerfilComponentes.NewRow();
                    row1["IdPerfilCompPadre"] = 1000;
                    row1["IdPerfilComp"] = 0;
                    row1["IdTipoDetalle"] = 0;
                    row1["PerfilComp"] = txtDescr.Text;
                    row1["Nuevo"] = false;
                    row1["NuevoPadre"] = false;
                    row1["CodigoSAP"] = "";
                    row1["DescripcionSAP"] = "";
                    row1["Nivel"] = 1;
                    row1["Estado"] = 1;
                    row1["FlagNeumatico"] = false;
                    row1["FlagActivo"] = true;
                    tblPerfilComponentes.Rows.Add(row1);

                    for (int i = 0; i < tblPerfilComponentesDatos.Rows.Count; i++)
                    {
                        DataRow row;
                        row = tblPerfilComponentes.NewRow();
                        row["IdPerfilCompPadre"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilCompPadre"]);
                        row["IdPerfilComp"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilComp"]);
                        row["IdTipoDetalle"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdTipoDetalle"]);
                        row["PerfilComp"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["PerfilComp"]);
                        row["Nuevo"] = false;
                        row["NuevoPadre"] = false;
                        row["CodigoSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["CodigoSAP"]);
                        row["DescripcionSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["DescripcionSAP"]);
                        row["Nivel"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["Nivel"]);
                        row["Estado"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["IdEstadoPC"]);
                        row["FlagNeumatico"] = Convert.ToBoolean(tblPerfilComponentesDatos.Rows[i]["FlagNeumatico"]);
                        row["FlagActivo"] = Convert.ToBoolean(tblPerfilComponentesDatos.Rows[i]["IdFlagActivo"]);
                        tblPerfilComponentes.Rows.Add(row);
                    }

                    DataView dtvComp = tblPerfilComponentes.DefaultView;
                    dtvComp.RowFilter = "FlagActivo = true";

                    Utilitarios.TreeViewModel.tblListarPerfilComponentes = dtvComp.ToTable();
                    trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponente(1000, null);

                    DataTable tblPerfilActividadDatos = objPerfilCompActividad.PerfilComp_Actividad_List(objE_Perfil);

                    for (int i = 0; i < tblPerfilActividadDatos.Rows.Count; i++)
                    {
                        DataRow row;
                        row = tblPerfilCompActividad.NewRow();
                        row["Actividad"] = Convert.ToString(tblPerfilActividadDatos.Rows[i]["Actividad"]);
                        row["IdPerfilComp"] = Convert.ToInt32(tblPerfilActividadDatos.Rows[i]["IdPerfilComp"]);
                        row["IdPerfilCompActividad"] = Convert.ToInt32(tblPerfilActividadDatos.Rows[i]["IdPerfilCompActividad"]);
                        row["Nuevo"] = false;
                        row["IdActividad"] = Convert.ToInt32(tblPerfilActividadDatos.Rows[i]["IdActividad"]);
                        row["Activo"] = Convert.ToInt32(tblPerfilActividadDatos.Rows[i]["IsActivo"]);
                        row["Uso"] = Convert.ToInt32(tblPerfilActividadDatos.Rows[i]["FlagUso"]);
                        row["FlagActivo"] = Convert.ToBoolean(tblPerfilActividadDatos.Rows[i]["FlagActivo"]);
                        tblPerfilCompActividad.Rows.Add(row);
                    }

                    objEPerfilTarea.IdPerfil = Convert.ToInt32(ValorGrilla);
                    DataTable tblPerfilTareaDatos = objPerfilTarea.PerfilTarea_List(objEPerfilTarea);

                    for (int i = 0; i < tblPerfilTareaDatos.Rows.Count; i++)
                    {
                        DataRow row;
                        row = tblPerfilTarea.NewRow();
                        row["IdPerfilTarea"] = Convert.ToInt32(tblPerfilTareaDatos.Rows[i]["IdPerfilTarea"]);
                        row["IdPerfilCompActividad"] = Convert.ToInt32(tblPerfilTareaDatos.Rows[i]["IdPerfilCompActividad"]);
                        row["IdTarea"] = Convert.ToString(tblPerfilTareaDatos.Rows[i]["IdTarea"]);
                        row["Tarea"] = Convert.ToString(tblPerfilTareaDatos.Rows[i]["Tarea"]);
                        row["HorasHombre"] = Convert.ToDouble(tblPerfilTareaDatos.Rows[i]["HorasHombre"]);
                        row["Nuevo"] = false;
                        row["IdEstado"] = Convert.ToInt32(tblPerfilTareaDatos.Rows[i]["IdEstadoPT"]);
                        row["Estado"] = Convert.ToString(tblPerfilTareaDatos.Rows[i]["EstadoPT"]);
                        row["FlagActivo"] = true;
                        tblPerfilTarea.Rows.Add(row);
                    }

                    DataTable tblPerfilDetalleDatos = objPerfilDetalle.PerfilDetalle_List(objE_Perfil);

                    RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                    InterfazMTTO.iSBO_BE.BEOITMList BEOITMListRep = new InterfazMTTO.iSBO_BE.BEOITMList();
                    BEOITMListRep = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("R", ref RPTA);
                    if (RPTA.ResultadoRetorno != 0)
                    {
                        GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                    }

                    RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                    InterfazMTTO.iSBO_BE.BEOITMList BEOITMListCon = new InterfazMTTO.iSBO_BE.BEOITMList();
                    BEOITMListCon = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("C", ref RPTA);
                    if (RPTA.ResultadoRetorno != 0)
                    {
                        GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                    }

                    for (int i = 0; i < tblPerfilDetalleDatos.Rows.Count; i++)
                    {
                        if (Convert.ToString(tblPerfilDetalleDatos.Rows[i]["IdTipoArticulo"]) == "1")
                        {
                            DataRow row;
                            row = tblPerfilDetalleHerrEsp.NewRow();
                            row["IdPerfilDetalle"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["IdPerfilDetalle"]);
                            row["IdPerfilCompActividad"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["IdPerfilCompActividad"]);
                            row["IdArticulo"] = Convert.ToString(tblPerfilDetalleDatos.Rows[i]["IdArticulo"]);
                            row["Articulo"] = Convert.ToString(tblPerfilDetalleDatos.Rows[i]["Articulo"]);
                            row["IdTipoArticulo"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["IdTipoArticulo"]);
                            row["Nuevo"] = false;
                            row["Cantidad"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["Cantidad"]);
                            row["FlagActivo"] = Convert.ToBoolean(tblPerfilDetalleDatos.Rows[i]["FlagActivo"]);
                            tblPerfilDetalleHerrEsp.Rows.Add(row);
                        }
                        else if (Convert.ToString(tblPerfilDetalleDatos.Rows[i]["IdTipoArticulo"]) == "2")
                        {
                            DataRow row;
                            row = tblPerfilDetalleRepuesto.NewRow();
                            row["IdPerfilDetalle"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["IdPerfilDetalle"]);
                            row["IdPerfilCompActividad"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["IdPerfilCompActividad"]);
                            row["IdArticulo"] = Convert.ToString(tblPerfilDetalleDatos.Rows[i]["IdArticulo"]);
                            row["Articulo"] = "";
                            for (int j = 0; j < BEOITMListRep.Count; j++)
                            {
                                if (Convert.ToString(tblPerfilDetalleDatos.Rows[i]["IdArticulo"]) == BEOITMListRep[j].CodigoArticulo)
                                {
                                    row["Articulo"] = BEOITMListRep[j].DescripcionArticulo;
                                }
                            }
                            row["IdTipoArticulo"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["IdTipoArticulo"]);
                            row["Nuevo"] = false;
                            row["Cantidad"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["Cantidad"]);
                            row["FlagActivo"] = Convert.ToBoolean(tblPerfilDetalleDatos.Rows[i]["FlagActivo"]);
                            tblPerfilDetalleRepuesto.Rows.Add(row);
                        }
                        else if (Convert.ToString(tblPerfilDetalleDatos.Rows[i]["IdTipoArticulo"]) == "3")
                        {
                            DataRow row;
                            row = tblPerfilDetalleConsumible.NewRow();
                            row["IdPerfilDetalle"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["IdPerfilDetalle"]);
                            row["IdPerfilCompActividad"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["IdPerfilCompActividad"]);
                            row["IdArticulo"] = Convert.ToString(tblPerfilDetalleDatos.Rows[i]["IdArticulo"]);
                            row["Articulo"] = "";
                            for (int j = 0; j < BEOITMListCon.Count; j++)
                            {
                                if (Convert.ToString(tblPerfilDetalleDatos.Rows[i]["IdArticulo"]) == BEOITMListCon[j].CodigoArticulo)
                                {
                                    row["Articulo"] = BEOITMListCon[j].DescripcionArticulo;
                                }
                            }
                            row["IdTipoArticulo"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["IdTipoArticulo"]);
                            row["Nuevo"] = false;
                            row["Cantidad"] = Convert.ToInt32(tblPerfilDetalleDatos.Rows[i]["Cantidad"]);
                            row["FlagActivo"] = Convert.ToBoolean(tblPerfilDetalleDatos.Rows[i]["FlagActivo"]);
                            tblPerfilDetalleConsumible.Rows.Add(row);
                        }

                    }

                    DataTable tblPerfilCompCicloDatos = objPerfilCompCiclo.PerfilComp_Ciclo_List(objE_Perfil);
                    for (int i = 0; i < tblPerfilCompCicloDatos.Rows.Count; i++)
                    {
                        DataRow row;
                        row = tblPerfilCompCiclo.NewRow();
                        row["IdPerfilCompCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilCompCiclo"]);
                        row["IdPerfilComp"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilComp"]);
                        row["Descripcion"] = Convert.ToString(tblPerfilCompCicloDatos.Rows[i]["TipoCiclo"]);
                        row["IdCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]);
                        if (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4)
                        {
                            row["Ciclo"] = gstrCicloDefecto;
                            row["FrecuenciaCambio"] = Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaCambio"]) / gintValorTiempoDefecto;
                        }
                        else
                        {
                            row["Ciclo"] = Convert.ToString(tblPerfilCompCicloDatos.Rows[i]["Ciclo"]);
                            row["FrecuenciaCambio"] = Convert.ToString(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaCambio"]);
                        }
                        row["IdEstadoPCC"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdEstadoPCC"]);
                        row["Estado"] = Convert.ToString(tblPerfilCompCicloDatos.Rows[i]["Estado"]);
                        row["FlagActivo"] = Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FlagActivo"]);
                        row["Nuevo"] = false;
                        tblPerfilCompCiclo.Rows.Add(row);
                    }
                }

                IdPerfilNew = Convert.ToInt32(tblPerfilComponentes.Compute("MAX(IdPerfilComp)", string.Empty)) + 1;
                FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                rbnActi.Checked += new RoutedEventHandler(rbnActi_Checked);
                rbnInac.Checked += new RoutedEventHandler(rbnInac_Checked);
                cboTipoUnid.SelectedIndexChanged += new RoutedEventHandler(cboTipoUnid_SelectedIndexChanged);
                CboEstado.SelectedIndexChanged += new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                cboPerfNeum.SelectedIndexChanged += new RoutedEventHandler(cboPerfNeum_SelectedIndexChanged);
                cboCicloPerfil.SelectedIndexChanged += new RoutedEventHandler(cboCicloPerfil_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void trvComp_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (trvComp.ItemsSource != null)
                {
                    int idPerfilComp = 0;
                    lstActi.SelectionChanged -= new SelectionChangedEventHandler(lstActi_SelectionChanged);
                    CboEstado.SelectedIndexChanged -= new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                    rbnTitulo.Checked -= new RoutedEventHandler(rbnTitulo_Checked);
                    rbnComponente.Checked -= new RoutedEventHandler(rbnComponente_Checked);
                    CboEstado.SelectedIndexChanged -= new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                    groupBox1.IsEnabled = true;
                    TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                    if (trm != null)
                    {
                        idPerfilComp = Convert.ToInt32(trm.IdMenu);
                        int idPadre = trm.IdMenuPadre;
                        int IdPerfilComp = trm.IdMenu;
                        string Padre = "";
                        bool FlagNeumatico = false;
                        btnRaizSubo1.IsEnabled = true;
                        buttonEdit1.IsEnabled = true;

                        if (idPadre == 1000)
                        {
                            if (!gbolFlagInactivo)
                            {
                                rbnTitulo.IsChecked = false;
                                rbnComponente.IsChecked = false;
                                rbnTitulo.IsEnabled = false;
                                rbnComponente.IsEnabled = false;
                                label001.IsEnabled = false;
                                label7.IsEnabled = false;
                                lblCodiSAP.IsEnabled = false;
                                btnbuscarsap.IsEnabled = false;
                                CboEstado.IsEnabled = false;
                                groupBox3.IsEnabled = false;
                                dtgCiclo.IsEnabled = false;
                            }
                            menuCambiarDesc.IsEnabled = false;
                            menuDelSelec.IsEnabled = false;
                            groupBox1.Header = "Información del componente";
                        }
                        else
                        {
                            if (!gbolFlagInactivo)
                            {
                                rbnTitulo.IsEnabled = true;
                                rbnComponente.IsEnabled = true;
                                label001.IsEnabled = true;
                                label7.IsEnabled = true;
                                lblCodiSAP.IsEnabled = true;
                                btnbuscarsap.IsEnabled = true;
                                CboEstado.IsEnabled = true;
                                groupBox3.IsEnabled = true;
                                dtgCiclo.IsEnabled = true;
                            }
                            groupBox1.Header = "Información del componente: " + trm.Name;
                        }

                        while (idPadre != 1000)
                        {
                            for (int i = 0; i < tblPerfilComponentes.Rows.Count; i++)
                            {
                                if (Convert.ToInt32(tblPerfilComponentes.Rows[i]["IdPerfilComp"]) == idPadre)
                                {
                                    if (Padre == "")
                                    {
                                        Padre = Convert.ToString(tblPerfilComponentes.Rows[i]["PerfilComp"]);
                                    }
                                    idPadre = Convert.ToInt32(tblPerfilComponentes.Rows[i]["IdPerfilCompPadre"]);
                                }

                                if (Convert.ToInt32(tblPerfilComponentes.Rows[i]["IdPerfilComp"]) == IdPerfilComp)
                                {
                                    if (Convert.ToInt32(tblPerfilComponentes.Rows[i]["IdTipoDetalle"]) == 1)
                                    {
                                        if (!gbolFlagInactivo)
                                        {
                                            rbnTitulo.IsChecked = true;
                                            label001.IsEnabled = false;
                                            label7.IsEnabled = false;
                                            lblCodiSAP.IsEnabled = false;
                                            btnbuscarsap.IsEnabled = false;
                                            CboEstado.IsEnabled = false;
                                            groupBox3.IsEnabled = false;
                                            dtgCiclo.IsEnabled = false;
                                        }
                                    }
                                    else if (Convert.ToInt32(tblPerfilComponentes.Rows[i]["IdTipoDetalle"]) == 2)
                                    {
                                        if (!gbolFlagInactivo)
                                        {
                                            rbnComponente.IsChecked = true;
                                            label001.IsEnabled = true;
                                            label7.IsEnabled = true;
                                            lblCodiSAP.IsEnabled = true;
                                            btnbuscarsap.IsEnabled = true;
                                            CboEstado.IsEnabled = true;
                                            groupBox3.IsEnabled = true;
                                            dtgCiclo.IsEnabled = true;
                                        }
                                    }
                                    else
                                    {
                                        if (!gbolFlagInactivo)
                                        {
                                            rbnTitulo.IsChecked = false;
                                            rbnComponente.IsChecked = false;
                                            label001.IsEnabled = true;
                                            label7.IsEnabled = true;
                                            lblCodiSAP.IsEnabled = true;
                                            btnbuscarsap.IsEnabled = true;
                                            CboEstado.IsEnabled = true;
                                            groupBox3.IsEnabled = true;
                                            dtgCiclo.IsEnabled = true;
                                        }
                                    }
                                    if (Convert.ToBoolean(tblPerfilComponentes.Rows[i]["FlagNeumatico"]))
                                    {
                                        FlagNeumatico = true;

                                    }
                                }
                            }
                        }
                        if (FlagNeumatico)
                        {
                            btnRaizSubo1.IsEnabled = false;
                            buttonEdit1.IsEnabled = false;
                        }

                        if (trm.IdMenuPadre != 1000)
                        {
                            lblPadre.Content = String.Format("Padre: {0} | Nivel {1}", Padre, trm.Nivel.ToString());
                        }
                        else
                        {
                            lblPadre.Content = String.Format("Padre: {0} | Nivel {1}", trm.Name, trm.Nivel.ToString());
                        }

                        if (lblPadre.Content.ToString().Length >= 44)
                        {
                            lblPadre.ToolTip = lblPadre.Content.ToString();
                            lblPadre.Content = lblPadre.Content.ToString().Substring(0, 41) + "...";
                        }

                        for (int i = 0; i < tblPerfilComponentes.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(tblPerfilComponentes.Rows[i]["IdPerfilComp"]) == idPerfilComp)
                            {
                                lblCodiSAP.Text = tblPerfilComponentes.Rows[i]["CodigoSAP"].ToString();

                                if (tblPerfilComponentes.Rows[i]["DescripcionSAP"].ToString() == "")
                                {
                                    lblCodiSAP.ToolTip = null;
                                }
                                else
                                {
                                    lblCodiSAP.ToolTip = tblPerfilComponentes.Rows[i]["DescripcionSAP"].ToString();
                                }

                                if (Convert.ToString(tblPerfilComponentes.Rows[i]["Estado"]) == "" || tblPerfilComponentes.Rows[i]["Estado"] == DBNull.Value)
                                {
                                    CboEstado.EditValue = 1;
                                }
                                else
                                {
                                    CboEstado.EditValue = Convert.ToInt32(tblPerfilComponentes.Rows[i]["Estado"]);
                                }
                            }
                        }

                        foreach (DataRow drPfComp in tblPerfilComponentes.Select("IdPerfilComp = " + IdPerfilComp))
                        {
                            if (rbnTitulo.IsChecked == true)
                            {
                                drPfComp["IdTipoDetalle"] = 1;
                            }
                            else if (rbnComponente.IsChecked == true)
                            {
                                drPfComp["IdTipoDetalle"] = 2;
                            }
                        }

                        //DataView dtvActividad = new DataView(tblPerfilCompActividad);
                        tblPerfilCompActividad.DefaultView.RowFilter = "IdPerfilComp = " + idPerfilComp + " AND FlagActivo = true";
                        lstActi.ItemsSource = tblPerfilCompActividad.DefaultView;
                        lstActi.DisplayMemberPath = "Actividad";
                        lstActi.SelectedValuePath = "IdPerfilCompActividad";

                        DataView dtvCiclos = new DataView(tblPerfilCompCiclo);
                        dtvCiclos.RowFilter = "IdPerfilComp = " + idPerfilComp + " AND FlagActivo = true";
                        dtgCiclo.ItemsSource = dtvCiclos;
                        tblCiclosExistentes = dtvCiclos.ToTable();

                        tblActividades = objActividad.Actividad_Combo();

                        chkActivo.IsChecked = false;
                        chkComp.IsChecked = false;
                        chkActivo.IsEnabled = false;
                        chkComp.IsEnabled = false;

                        dtgTarea.ItemsSource = null;
                        dtgHerrEspe.ItemsSource = null;
                        dtgRepu.ItemsSource = null;
                        dtgCons.ItemsSource = null;
                        tabControl2.SelectedIndex = 0;

                        lstActi.SelectionChanged += new SelectionChangedEventHandler(lstActi_SelectionChanged);
                        CboEstado.SelectedIndexChanged += new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                        rbnTitulo.Checked += new RoutedEventHandler(rbnTitulo_Checked);
                        rbnComponente.Checked += new RoutedEventHandler(rbnComponente_Checked);
                        CboEstado.SelectedIndexChanged += new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtValidacion_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var textboxSender = (TextBox)sender;
                var cursorPosition = textboxSender.SelectionStart;
                textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9]", "");
                textboxSender.SelectionStart = cursorPosition;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void UserControl_Loaded()
        {
            try
            {
                CboEstado.SelectedIndexChanged -= new RoutedEventHandler(CboEstado_SelectedIndexChanged);

                txtComoponente.MaxLength = 100;
                txtHorasHombre.MaxLength = 18;

                objTM.IdTabla = 0;
                tblTipoCiclo = B_TablaMaestra.TablaMaestra_Combo(objTM).DefaultView;

                txtDescr.MaxLength = 50;
                tblPerfilNeumatico = new DataTable();
                tblPerfilNeumatico = B_PerfilNeumatico.PerfilNeumatico_Combo();
                dtvPerfilNeumatico = new DataView(tblPerfilNeumatico);
                dtvPerfilNeumatico.RowFilter = "IdEstadoPN = 1";
                cboPerfNeum.ItemsSource = dtvPerfilNeumatico;
                cboPerfNeum.DisplayMember = "PerfilNeumatico";
                cboPerfNeum.ValueMember = "IdPerfilNeumatico";

                E_Ciclo objECiclo = new E_Ciclo();
                objECiclo.Idtipociclo = 0;
                tblListadoCiclos = objCiclo.Ciclo_List(objECiclo);
                dtvListadoCiclos = new DataView(tblListadoCiclos);
                DataView dtvCiclosCombo = new DataView(tblListadoCiclos);
                dtvCiclosCombo.RowFilter = "IdCiclo IN (3,4)";
                cboCicloPerfil.ItemsSource = dtvCiclosCombo;
                cboCicloPerfil.DisplayMember = "Ciclo";
                cboCicloPerfil.ValueMember = "IdCiclo";

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                tucuclist = InterfazMTTO.iSBO_BL.UnidadControl_BL.ListaTipoUnidadControl(ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    cboTipoUnid.ItemsSource = tucuclist;
                    cboTipoUnid.DisplayMember = "DescripcionTipoUnidadControl";
                    cboTipoUnid.ValueMember = "CodigoTipoUnidadControl";
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                BEOITMList = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("C", ref RPTA);
                if (RPTA.ResultadoRetorno == 0)
                {
                    cboConsumibles.ItemsSource = BEOITMList;
                    cboConsumibles.DisplayMember = "DescripcionArticulo";
                    cboConsumibles.ValueMember = "CodigoArticulo";
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                BEOITMList = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos("P", ref RPTA);

                if (RPTA.ResultadoRetorno == 0)
                {
                    cboCompSAP.ItemsSource = BEOITMList;
                    cboCompSAP.DisplayMember = "DescripcionArticulo";
                    cboCompSAP.ValueMember = "CodigoArticulo";
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }

                ListarPerfilUnidadControl();


                tblPerfilComponentes = new DataTable();
                tblPerfilComponentes.Columns.Add("IdPerfilCompPadre", Type.GetType("System.Int32"));
                tblPerfilComponentes.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblPerfilComponentes.Columns.Add("IdTipoDetalle", Type.GetType("System.Int32"));
                tblPerfilComponentes.Columns.Add("Nivel", Type.GetType("System.Int32"));
                tblPerfilComponentes.Columns.Add("PerfilComp", Type.GetType("System.String"));
                tblPerfilComponentes.Columns.Add("CodigoSAP", Type.GetType("System.String"));
                tblPerfilComponentes.Columns.Add("DescripcionSAP", Type.GetType("System.String"));
                tblPerfilComponentes.Columns.Add("Estado", Type.GetType("System.Int32"));
                tblPerfilComponentes.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblPerfilComponentes.Columns.Add("FlagNeumatico", Type.GetType("System.Boolean"));
                tblPerfilComponentes.Columns.Add("NuevoPadre", Type.GetType("System.Boolean"));
                tblPerfilComponentes.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                tblPerfilCompActividad = new DataTable();
                tblPerfilCompActividad.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblPerfilCompActividad.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblPerfilCompActividad.Columns.Add("Actividad", Type.GetType("System.String"));
                tblPerfilCompActividad.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                tblPerfilCompActividad.Columns.Add("IdActividad", Type.GetType("System.Int32"));
                tblPerfilCompActividad.Columns.Add("Uso", Type.GetType("System.Boolean"));
                tblPerfilCompActividad.Columns.Add("Activo", Type.GetType("System.Boolean"));
                tblPerfilCompActividad.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));

                tblPerfilTarea = new DataTable();
                tblPerfilTarea.Columns.Add("IdPerfilTarea", Type.GetType("System.Int32"));
                tblPerfilTarea.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblPerfilTarea.Columns.Add("IdTarea", Type.GetType("System.String"));
                tblPerfilTarea.Columns.Add("Tarea", Type.GetType("System.String"));
                tblPerfilTarea.Columns.Add("HorasHombre", Type.GetType("System.Decimal"));
                tblPerfilTarea.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                tblPerfilTarea.Columns.Add("IdEstado", Type.GetType("System.Int32"));
                tblPerfilTarea.Columns.Add("Estado", Type.GetType("System.String"));
                tblPerfilTarea.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));

                tblPerfilDetalleHerrEsp = new DataTable();
                tblPerfilDetalleHerrEsp.Columns.Add("IdPerfilDetalle", Type.GetType("System.Int32"));
                tblPerfilDetalleHerrEsp.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblPerfilDetalleHerrEsp.Columns.Add("IdTipoArticulo", Type.GetType("System.Int32"));
                tblPerfilDetalleHerrEsp.Columns.Add("IdArticulo", Type.GetType("System.String"));
                tblPerfilDetalleHerrEsp.Columns.Add("Articulo", Type.GetType("System.String"));
                tblPerfilDetalleHerrEsp.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                tblPerfilDetalleHerrEsp.Columns.Add("Cantidad", Type.GetType("System.Int32"));
                tblPerfilDetalleHerrEsp.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));

                tblPerfilDetalleConsumible = new DataTable();
                tblPerfilDetalleConsumible.Columns.Add("IdPerfilDetalle", Type.GetType("System.Int32"));
                tblPerfilDetalleConsumible.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblPerfilDetalleConsumible.Columns.Add("IdTipoArticulo", Type.GetType("System.Int32"));
                tblPerfilDetalleConsumible.Columns.Add("IdArticulo", Type.GetType("System.String"));
                tblPerfilDetalleConsumible.Columns.Add("Articulo", Type.GetType("System.String"));
                tblPerfilDetalleConsumible.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                tblPerfilDetalleConsumible.Columns.Add("Cantidad", Type.GetType("System.Int32"));
                tblPerfilDetalleConsumible.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));

                tblPerfilDetalleRepuesto = new DataTable();
                tblPerfilDetalleRepuesto.Columns.Add("IdPerfilDetalle", Type.GetType("System.Int32"));
                tblPerfilDetalleRepuesto.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
                tblPerfilDetalleRepuesto.Columns.Add("IdTipoArticulo", Type.GetType("System.Int32"));
                tblPerfilDetalleRepuesto.Columns.Add("IdArticulo", Type.GetType("System.String"));
                tblPerfilDetalleRepuesto.Columns.Add("Articulo", Type.GetType("System.String"));
                tblPerfilDetalleRepuesto.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                tblPerfilDetalleRepuesto.Columns.Add("Cantidad", Type.GetType("System.Int32"));
                tblPerfilDetalleRepuesto.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));

                tblPerfilCompCiclo = new DataTable();
                tblPerfilCompCiclo.Columns.Add("IdPerfilCompCiclo", Type.GetType("System.Int32"));
                tblPerfilCompCiclo.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblPerfilCompCiclo.Columns.Add("Descripcion", Type.GetType("System.String"));
                tblPerfilCompCiclo.Columns.Add("IdCiclo", Type.GetType("System.Int32"));
                tblPerfilCompCiclo.Columns.Add("Ciclo", Type.GetType("System.String"));
                tblPerfilCompCiclo.Columns.Add("FrecuenciaCambio", Type.GetType("System.Double"));
                tblPerfilCompCiclo.Columns.Add("IdEstadoPCC", Type.GetType("System.Int32"));
                tblPerfilCompCiclo.Columns.Add("Estado", Type.GetType("System.String"));
                tblPerfilCompCiclo.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblPerfilCompCiclo.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                tblActividades = objActividad.Actividad_Combo();

                CboEstado.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 22", tblTipoCiclo);
                CboEstado.DisplayMember = "Descripcion";
                CboEstado.ValueMember = "IdColumna";

                cboEstadoTarea.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 23", tblTipoCiclo);
                cboEstadoTarea.DisplayMember = "Descripcion";
                cboEstadoTarea.ValueMember = "IdColumna";


                gintTiempoDefecto = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1000", tblTipoCiclo).Rows[7]["Valor"]);
                gintValorTiempoDefecto = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 58", tblTipoCiclo).Select("IdColumna = " + gintTiempoDefecto)[0][2]);
                gstrCicloDefecto = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 58", tblTipoCiclo).Select("IdColumna = " + gintTiempoDefecto)[0][3].ToString();

                spCantidadConsumible.MaxValue = 999;
                spCantidadRepuesto.MaxValue = 999;

                groupBox1.IsEnabled = false;
                tabItem2.IsEnabled = false;
                objTM.IdTabla = 0;
                dtvMaestra = B_TablaMaestra.TablaMaestra_Combo(objTM).DefaultView;
                spCantidadHerramienta.MinValue = 1;
                spCantidadRepuesto.MinValue = 1;
                spCantidadConsumible.MinValue = 1;

                #region VisualizacionBotonImprimir
                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                if (!VisualizaBotonImprimirDetalle)
                {
                    btnImprimir.Visibility = System.Windows.Visibility.Hidden;
                }
                #endregion

                CboEstado.SelectedIndexChanged += new RoutedEventHandler(CboEstado_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        void EliminarComponentesNuevos(int IdPerfilcomp)
        {
            try
            {
                int Nuevo = 0;
                foreach (DataRow drComp in tblPerfilComponentes.Select("(IdPerfilComp = " + IdPerfilcomp + " OR IdPerfilCompPadre = " + IdPerfilcomp + ") AND Nuevo = true"))
                {
                    drComp.Delete();
                    EliminarActividadesNuevas(IdPerfilcomp);
                    foreach (DataRow drCic in tblPerfilCompCiclo.Select("IdPerfilComp = " + IdPerfilcomp + " AND Nuevo = true"))
                    {
                        drCic.Delete();
                    }

                    Nuevo++;
                }
                if (Nuevo != 0)
                {
                    TrvComp_Refresh();
                    return;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        void EliminarComponentesExistentes(int IdPerfilcomp)
        {
            try
            {
                foreach (DataRow drComp in tblPerfilComponentes.Select("(IdPerfilComp = " + IdPerfilcomp + " OR IdPerfilCompPadre = " + IdPerfilcomp + ") AND Nuevo = false"))
                {
                    drComp["FlagActivo"] = false;
                    EliminarActividadesExistentes(IdPerfilcomp);
                    foreach (DataRow drCic in tblPerfilCompCiclo.Select("IdPerfilComp = " + IdPerfilcomp + " AND Nuevo = false"))
                    {
                        drCic["FlagActivo"] = false;
                    }
                }
                TrvComp_Refresh();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        void EliminarActividadesExistentes(int IdPerfilcomp)
        {
            try
            {
                foreach (DataRow drAct in tblPerfilCompActividad.Select("IdPerfilComp = " + IdPerfilcomp + " AND Nuevo = false"))
                {
                    int IdPerfilCompActividad = Convert.ToInt32(drAct["IdPerfilCompActividad"]);
                    drAct["FlagActivo"] = false;
                    //Borrado Lógico de Tareas
                    foreach (DataRow drTar in tblPerfilTarea.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND Nuevo = false"))
                    {
                        drTar["FlagActivo"] = false;
                    }
                    //Borrado Lógico de Herramientas
                    foreach (DataRow drHer in tblPerfilDetalleHerrEsp.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND Nuevo = false"))
                    {
                        drHer["FlagActivo"] = false;
                    }
                    //Borrado Lógico de Repuestos
                    foreach (DataRow drRep in tblPerfilDetalleRepuesto.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND Nuevo = false"))
                    {
                        drRep["FlagActivo"] = false;
                    }
                    //Borrado Lógico de Consumibles
                    foreach (DataRow drCon in tblPerfilDetalleConsumible.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND Nuevo = false"))
                    {
                        drCon["FlagActivo"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        void EliminarActividadesNuevas(int IdPerfilcomp)
        {
            try
            {
                foreach (DataRow drAct in tblPerfilCompActividad.Select("IdPerfilComp = " + IdPerfilcomp + " AND Nuevo = true"))
                {
                    int IdPerfilCompActividad = Convert.ToInt32(drAct["IdPerfilCompActividad"]);
                    drAct.Delete();
                    //Borrado Físico de Tareas
                    foreach (DataRow drTar in tblPerfilTarea.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND Nuevo = true"))
                    {
                        drTar.Delete();
                    }
                    //Borrado Físico de Herramientas
                    foreach (DataRow drHer in tblPerfilDetalleHerrEsp.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND Nuevo = true"))
                    {
                        drHer.Delete();
                    }
                    //Borrado Físico de Repuestos
                    foreach (DataRow drRep in tblPerfilDetalleRepuesto.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND Nuevo = true"))
                    {
                        drRep.Delete();
                    }
                    //Borrado Físico de Consumibles
                    foreach (DataRow drCon in tblPerfilDetalleConsumible.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND Nuevo = true"))
                    {
                        drCon.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void trvComp_KeyDown(object sender, KeyEventArgs e)
        {
            int IdEstado = Convert.ToInt32(dtgPUC.GetFocusedRowCellValue("IdEstadoP"));
            if (IdEstado == 2) { return; }
            if (e.Key == Key.Delete)
            {
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    if (trm.IdMenuPadre == 1000) { return; }
                    int canHijos = 0;
                    int IdPerfilcomp = trm.IdMenu;
                    var rpt = DevExpress.Xpf.Core.DXMessageBox.Show("¿Está seguro que desea eliminarlo?", "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (rpt == MessageBoxResult.Yes)
                    {
                        foreach (DataRow drCantHijos in tblPerfilComponentes.Select("IdPerfilCompPadre = " + IdPerfilcomp + " AND FlagActivo = true")) { canHijos++; }

                        if (canHijos != 0)
                        {
                            var rpta = DevExpress.Xpf.Core.DXMessageBox.Show("Se eliminara todo su contenido del item seleccionado \n¿Desea continuar?", "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (rpta == MessageBoxResult.No) { return; }
                        }

                        EliminarComponentesNuevos(IdPerfilcomp);

                        objEPerfilComp.Idperfilcomp = trm.IdMenu;
                        DataTable tblCont = objPerfilComp.PerfilComp_GetBeforeDel(objEPerfilComp);

                        if (Convert.ToInt32(tblCont.Rows[0]["Contador"]) == 0)
                        {
                            EliminarComponentesExistentes(IdPerfilcomp);
                        }
                        else
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_DELE_COMP_ASIG"), 2);
                        }
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                    return;
                }
            }
        }

        private void txtCantidadConsumible_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                EstadoForm(false, true, false);
                var textboxSender = (TextBox)sender;

                string cadena = textboxSender.Text;

                if (textboxSender.Text.Trim() == ".")
                {
                    textboxSender.Text = "0.";
                    textboxSender.SelectionStart = 2;
                }

                if (textboxSender.Text == " ")
                {
                    textboxSender.Text = "";
                }

                e.Handled = Utilitarios.Utilitarios.ValidaCantPuntos(textboxSender.Text);

                if (e.Handled)
                {
                    textboxSender.Text = cadena.Remove(textboxSender.Text.Length - 1, 1);
                    textboxSender.SelectionStart = cadena.Length;
                    return;
                }


                textboxSender.Text = textboxSender.Text.Replace(" ", "");
                textboxSender.SelectionStart = textboxSender.Text.Length;

                string strFilterRegexPattern = "[^0-9.]";
                textboxSender.Text = Regex.Replace(textboxSender.Text, strFilterRegexPattern, "");

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtCantidadHerramienta_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textboxSender = (TextBox)sender;
                var cursorPosition = textboxSender.SelectionStart;
                textboxSender.Text = Regex.Replace(textboxSender.Text, "[^0-9]", "");
                textboxSender.SelectionStart = cursorPosition;
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtComponenteHijo_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtComponenteHijo.Text = Utilitarios.Utilitarios.SoloAlfanumerico(txtComponenteHijo.Text);
            txtComponenteHijo.SelectionStart = txtComponenteHijo.Text.Length;

            txtcambiardesc.Text = Utilitarios.Utilitarios.SoloAlfanumerico(txtcambiardesc.Text);
            txtcambiardesc.SelectionStart = txtcambiardesc.Text.Length;
            EstadoForm(false, true, false);
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

                if ((gbolNuevo == false) && (gbolEdicion == false))
                {

                    tabItem2.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "TAB1_CONS");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "BTNG_CONS");
                    tabItem1.IsEnabled = true;
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tabItem2.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "TAB1_NUEV");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "BTNG_NUEV");
                    tabItem1.IsEnabled = false;
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                    tipo = 1;
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tabItem2.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "TAB1_EDIT");
                    btnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "BTNG_EDIT");
                    tabItem1.IsEnabled = false;
                    tipo = 2;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgPUC_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgPUC.VisibleRowCount == 0) { return; }
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            if (dep is TextBlock)
            {
                if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodPerfil")
                {
                    e.Handled = true;
                    EstadoForm(false, false, true);
                    dtvPerfilNeumatico.RowFilter = "";
                    tblPerfilCompActividad.Clear();
                    tblPerfilComponentes.Clear();
                    tblPerfilTarea.Clear();
                    tblPerfilDetalleHerrEsp.Clear();
                    tblPerfilDetalleRepuesto.Clear();
                    tblPerfilDetalleConsumible.Clear();
                    tblPerfilCompCiclo.Clear();
                    tipo = 3;
                    LlenarDataPerfil();
                    tabItem2.IsEnabled = true;
                }
            }
        }

        private bool ValidarBotones(bool acti, bool comple)
        {
            bool abrir = true;
            if (acti)
            {
                if (lstActi.Items.Count <= 0) { abrir = false; GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_ADD_ACTI"), 2); }
                else if (lstActi.SelectedIndex == -1) { abrir = false; GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_ACTI"), 2); }

            }
            else if (comple)
            {
                if (trvComp.Items.Count == 0) { abrir = false; GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_ADD_COMP"), 2); }
                else if (trvComp.SelectedItem == null) { abrir = false; GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2); }

            }
            return abrir;
        }
        private bool ValidarCampos()
        {
            bool permitir = false;
            if (txtDescr.Text.Trim() == "")
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_DESC"), 2);
                txtDescr.Focus();
            }
            else if (cboTipoUnid.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_TIPO"), 2);
                cboTipoUnid.Focus();
            }
            else if (cboCicloPerfil.SelectedIndex == -1)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_CICL"), 2);
                cboCicloPerfil.Focus();
            }
            else if (rbnActi.IsChecked == false && rbnInac.IsChecked == false)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_ESTA"), 2);
                rbnActi.Focus();
            }
            else
            {
                permitir = true;
            }
            return permitir;
        }

        private void cboPerfNeum_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                bool exneuma = false;
                foreach (DataRow drCompNeu in tblPerfilComponentes.Select("FlagNeumatico = true")) { exneuma = true; }

                if (!exneuma)
                {
                    trvComp.ItemsSource = null;
                    Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                    DataRow row;
                    row = tblPerfilComponentes.NewRow();
                    row["IdPerfilCompPadre"] = 0;
                    row["IdPerfilComp"] = IdPerfilNew;
                    row["IdTipoDetalle"] = 1;
                    row["PerfilComp"] = "Neumáticos";
                    row["Nuevo"] = true;
                    row["NuevoPadre"] = false;
                    row["CodigoSAP"] = "";
                    row["DescripcionSAP"] = "";
                    row["Nivel"] = 2;
                    row["Estado"] = 1;
                    row["FlagNeumatico"] = true;
                    row["FlagActivo"] = true;
                    tblPerfilComponentes.Rows.Add(row);

                    row = tblPerfilComponentes.NewRow();
                    row["IdPerfilCompPadre"] = IdPerfilNew;
                    IdPerfilNew++;
                    row["IdPerfilComp"] = IdPerfilNew;
                    row["IdTipoDetalle"] = 2;
                    row["PerfilComp"] = "Llantas";
                    row["Nuevo"] = true;
                    row["NuevoPadre"] = true;
                    row["CodigoSAP"] = "";
                    row["DescripcionSAP"] = "";
                    row["Nivel"] = 3;
                    row["Estado"] = 1;
                    row["FlagNeumatico"] = true;
                    row["FlagActivo"] = true;
                    tblPerfilComponentes.Rows.Add(row);

                    dtvListadoCiclos.RowFilter = "IdCiclo IN (1,2)";
                    for (int i = 0; i < dtvListadoCiclos.Count; i++)
                    {
                        DataRow row2;
                        row2 = tblPerfilCompCiclo.NewRow();
                        row2["IdPerfilCompCiclo"] = IdPerfilCompCicloNew;
                        row2["IdPerfilComp"] = IdPerfilNew;
                        row2["Descripcion"] = dtvListadoCiclos[i]["TipoCiclo"].ToString();
                        row2["IdCiclo"] = Convert.ToInt32(dtvListadoCiclos[i]["IdCiclo"]);
                        row2["Ciclo"] = Convert.ToString(dtvListadoCiclos[i]["Ciclo"]);
                        row2["FrecuenciaCambio"] = 0;
                        row2["IdEstadoPCC"] = 1;
                        row2["Estado"] = "Activo";
                        row2["FlagActivo"] = true;
                        row2["Nuevo"] = true;
                        tblPerfilCompCiclo.Rows.Add(row2);
                        IdPerfilCompCicloNew++;
                    }
                    IdPerfilNew++;

                    DataView dtvComp = new DataView(tblPerfilComponentes);
                    dtvComp.RowFilter = "FlagActivo = true";

                    Utilitarios.TreeViewModel.tblListarPerfilComponentes = dtvComp.ToTable();
                    trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponente(1000, null);
                }
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboHerramientaEspecial_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            E_Herramienta objE_Herramienta = new E_Herramienta();
            objE_Herramienta.IdHerramienta = Convert.ToInt32(cboHerramientaEspecial.EditValue);
            spCantidadHerramienta.Value = 1;
            spCantidadHerramienta.MaxValue = objHerramienta.Herramienta_GetCantItems(objE_Herramienta);
        }

        private void btnEliminarConsumibles_Click(object sender, RoutedEventArgs e)
        {
            if (gbolFlagInactivo) { return; }
            var rpt = DevExpress.Xpf.Core.DXMessageBox.Show("¿Está seguro que desea eliminar el consumible?", "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rpt == MessageBoxResult.Yes)
            {
                EstadoForm(false, true, false);
                int IdPerfilDetalle = Convert.ToInt32(dtgCons.GetFocusedRowCellValue("IdPerfilDetalle"));
                int IdPerfilCompActividad = Convert.ToInt32(dtgCons.GetFocusedRowCellValue("IdPerfilCompActividad"));

                foreach (DataRow drPfDet in tblPerfilDetalleConsumible.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND IdPerfilDetalle = " + IdPerfilDetalle))
                {
                    if (Convert.ToBoolean(drPfDet["Nuevo"]) == true)
                    {
                        drPfDet.Delete();
                        break;
                    }
                    else
                    {
                        drPfDet["FlagActivo"] = false;
                        break;
                    }
                }
                dtgCons.ItemsSource = FiltroDetallesComponentes(IdPerfilCompActividad, tblPerfilDetalleConsumible);
                VerificarActividadActiva();
            }
        }

        private void btnEliminarRepuesto_Click(object sender, RoutedEventArgs e)
        {
            if (gbolFlagInactivo) { return; }
            var rpt = DevExpress.Xpf.Core.DXMessageBox.Show("¿Está seguro que desea eliminar el repuesto?", "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rpt == MessageBoxResult.Yes)
            {
                EstadoForm(false, true, false);
                int IdPerfilDetalle = Convert.ToInt32(dtgRepu.GetFocusedRowCellValue("IdPerfilDetalle"));
                int IdPerfilCompActividad = Convert.ToInt32(dtgRepu.GetFocusedRowCellValue("IdPerfilCompActividad"));

                foreach (DataRow drPfDet in tblPerfilDetalleRepuesto.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND IdPerfilDetalle = " + IdPerfilDetalle))
                {
                    if (Convert.ToBoolean(drPfDet["Nuevo"]) == true)
                    {
                        drPfDet.Delete();
                        break;
                    }
                    else
                    {
                        drPfDet["FlagActivo"] = false;
                        break;
                    }
                }
                dtgRepu.ItemsSource = FiltroDetallesComponentes(IdPerfilCompActividad, tblPerfilDetalleRepuesto);
                VerificarActividadActiva();
            }
        }

        private void btnEliminarHerramienta_Click(object sender, RoutedEventArgs e)
        {
            if (gbolFlagInactivo) { return; }
            var rpt = DevExpress.Xpf.Core.DXMessageBox.Show("¿Está seguro que desea eliminar la herramienta?", "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rpt == MessageBoxResult.Yes)
            {
                EstadoForm(false, true, false);
                int IdPerfilDetalle = Convert.ToInt32(dtgHerrEspe.GetFocusedRowCellValue("IdPerfilDetalle"));
                int IdPerfilCompActividad = Convert.ToInt32(dtgHerrEspe.GetFocusedRowCellValue("IdPerfilCompActividad"));

                foreach (DataRow drPfDet in tblPerfilDetalleHerrEsp.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND IdPerfilDetalle = " + IdPerfilDetalle))
                {
                    if (Convert.ToBoolean(drPfDet["Nuevo"]) == true)
                    {
                        drPfDet.Delete();
                        break;
                    }
                    else
                    {
                        drPfDet["FlagActivo"] = false;
                        break;
                    }
                }
                dtgHerrEspe.ItemsSource = FiltroDetallesComponentes(IdPerfilCompActividad, tblPerfilDetalleHerrEsp);
                VerificarActividadActiva();
            }
        }

        private void btnEliminarTarea_Click(object sender, RoutedEventArgs e)
        {
            if (gbolFlagInactivo) { return; }
            var rpt = DevExpress.Xpf.Core.DXMessageBox.Show("¿Está seguro que desea eliminar la tarea?", "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rpt == MessageBoxResult.Yes)
            {
                EstadoForm(false, true, false);
                int IdTarea = Convert.ToInt32(dtgTarea.GetFocusedRowCellValue("IdTarea"));
                int IdPerfilCompActividad = Convert.ToInt32(dtgTarea.GetFocusedRowCellValue("IdPerfilCompActividad"));

                foreach (DataRow drPfDet in tblPerfilTarea.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND IdTarea = " + IdTarea))
                {
                    if (Convert.ToBoolean(drPfDet["Nuevo"]) == true)
                    {
                        drPfDet.Delete();
                        break;
                    }
                    else
                    {
                        drPfDet["FlagActivo"] = false;
                        break;
                    }
                }
                dtgTarea.ItemsSource = FiltroDetallesComponentes(IdPerfilCompActividad, tblPerfilTarea);
                VerificarActividadActiva();
            }
        }

        private void rbnTitulo_Checked(object sender, RoutedEventArgs e)
        {
            TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
            if (trm != null)
            {
                int idPerfilComp = Convert.ToInt32(trm.IdMenu);
                objEPerfilComp.Idperfilcomp = idPerfilComp;
                DataTable tblCont = objPerfilComp.PerfilComp_GetBeforeDel(objEPerfilComp);

                bool nuevo = false;
                foreach (DataRow drComp in tblPerfilComponentes.Select("IdPerfilComp = " + idPerfilComp + " AND Nuevo = true"))
                {
                    nuevo = true;
                }

                if (Convert.ToInt32(tblCont.Rows[0]["Contador"]) != 0)
                {
                    if (!nuevo)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_TIPO_COMP"), 2);
                        rbnComponente.Checked -= new RoutedEventHandler(rbnComponente_Checked);
                        rbnComponente.IsChecked = true;
                        rbnComponente.Checked += new RoutedEventHandler(rbnComponente_Checked);
                        return;
                    }
                }

                foreach (DataRow drCiclos in tblPerfilCompCiclo.Select("IdPerfilComp = " + idPerfilComp + " AND Nuevo = true"))
                {
                    drCiclos.Delete();
                }

                foreach (DataRow drCiclos in tblPerfilCompCiclo.Select("IdPerfilComp = " + idPerfilComp + " AND Nuevo = false"))
                {
                    drCiclos["FlagActivo"] = false;
                }

                foreach (DataRow drComp in tblPerfilComponentes.Select("IdPerfilComp = " + idPerfilComp))
                {
                    drComp["IdTipoDetalle"] = 1;
                }
                EstadoForm(false, true, false);
            }
            else
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                rbnTitulo.IsChecked = false;
                return;
            }
            label001.IsEnabled = false;
            label7.IsEnabled = false;
            lblCodiSAP.IsEnabled = false;
            btnbuscarsap.IsEnabled = false;
            CboEstado.IsEnabled = false;
            groupBox3.IsEnabled = false;
            dtgCiclo.IsEnabled = false;
        }

        private void rbnComponente_Checked(object sender, RoutedEventArgs e)
        {
            TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
            if (trm != null)
            {
                int idPerfilComp = Convert.ToInt32(trm.IdMenu);
                objEPerfilComp.Idperfilcomp = idPerfilComp;
                DataTable tblCont = objPerfilComp.PerfilComp_GetBeforeDel(objEPerfilComp);
                bool nuevo = false;
                foreach (DataRow drComp in tblPerfilComponentes.Select("IdPerfilComp = " + idPerfilComp + " AND Nuevo = true"))
                {
                    nuevo = true;
                }
                if (Convert.ToInt32(tblCont.Rows[0]["Contador"]) != 0)
                {
                    if (!nuevo)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_TIPO_COMP"), 2);
                        rbnTitulo.Checked -= new RoutedEventHandler(rbnTitulo_Checked);
                        rbnTitulo.IsChecked = true;
                        rbnTitulo.Checked += new RoutedEventHandler(rbnTitulo_Checked);
                        return;
                    }
                }
                bool IsNeumatico = false;
                foreach (DataRow drComp in tblPerfilComponentes.Select("IdPerfilComp = " + idPerfilComp))
                {
                    drComp["IdTipoDetalle"] = 2;
                    if (Convert.ToBoolean(drComp["FlagNeumatico"]))
                    {
                        IsNeumatico = true;
                    }
                }

                foreach (DataRow drCiclos in tblPerfilCompCiclo.Select("IdPerfilComp = " + idPerfilComp + " AND Nuevo = false AND FlagActivo = false"))
                {
                    drCiclos["FlagActivo"] = true;
                }

                int exciclos = 0;
                foreach (DataRow drCiclos in tblPerfilCompCiclo.Select("IdPerfilComp = " + idPerfilComp))
                {
                    exciclos++;
                }

                if (trm.IdMenu != 0 && exciclos == 0)
                {
                    dtvListadoCiclos.RowFilter = "IdCiclo IN (3,4)";
                    if (IsNeumatico)
                    {
                        dtvListadoCiclos.RowFilter = "IdCiclo IN (1,2)";
                    }

                    for (int i = 0; i < dtvListadoCiclos.Count; i++)
                    {
                        DataRow row2;
                        row2 = tblPerfilCompCiclo.NewRow();
                        row2["IdPerfilCompCiclo"] = IdPerfilCompCicloNew;
                        row2["IdPerfilComp"] = idPerfilComp;
                        row2["Descripcion"] = dtvListadoCiclos[i]["TipoCiclo"].ToString();
                        row2["IdCiclo"] = Convert.ToInt32(dtvListadoCiclos[i]["IdCiclo"]);
                        row2["Ciclo"] = Convert.ToString(dtvListadoCiclos[i]["Ciclo"]);
                        row2["FrecuenciaCambio"] = 0;
                        row2["IdEstadoPCC"] = 1;
                        row2["Estado"] = "Activo";
                        row2["FlagActivo"] = true;
                        row2["Nuevo"] = true;
                        tblPerfilCompCiclo.Rows.Add(row2);
                        IdPerfilCompCicloNew++;
                    }
                }
                EstadoForm(false, true, false);
            }
            else
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                rbnComponente.IsChecked = false;
                return;
            }
            label001.IsEnabled = true;
            label7.IsEnabled = true;
            lblCodiSAP.IsEnabled = true;
            btnbuscarsap.IsEnabled = true;
            CboEstado.IsEnabled = true;
            groupBox3.IsEnabled = true;
            dtgCiclo.IsEnabled = true;
        }

        private bool ValidaLogicaNegocio()
        {
            bool bolRpta = false;
            try
            {
                if (gbolNuevo == true && gbolEdicion == false)
                {
                    objEPerfil.Idperfil = 0;
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    objEPerfil.Idperfil = Convert.ToInt32(dtgPUC.GetFocusedRowCellValue("IdPerfil").ToString());
                }
                objEPerfil.Perfil = txtDescr.Text.Trim();
                DataTable tblConsulta = objPerfil.Perfil_GetItemByDesc(objEPerfil);
                if (tblConsulta.Rows.Count > 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_DUPL"), 2);
                    txtDescr.Focus();
                }
                return bolRpta;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                return bolRpta;
            }
        }

        private void menuAddComp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm == null)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                    return;
                }
                CambiarBotonDefecto(false);
                btnAceptarComponenteHijo.IsDefault = true;
                btnCancelarComponenteHijo.IsCancel = true;
                stkPanelComponenteHijo.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void menuAddTitulo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    if (trm.IdMenuPadre == 1000)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_COMP_DIFE"), 2);
                        return;
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                    return;
                }
                CambiarBotonDefecto(false);
                btnAceptarComponente.IsDefault = true;
                btnCancelarComponente.IsCancel = true;
                stkPanelComponente.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {

                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void menuDelSelec_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    int canHijos = 0;
                    int IdPerfilcomp = trm.IdMenu;
                    var rpt = DevExpress.Xpf.Core.DXMessageBox.Show("¿Está seguro que desea eliminarlo?", "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (rpt == MessageBoxResult.Yes)
                    {
                        foreach (DataRow drCantHijos in tblPerfilComponentes.Select("IdPerfilCompPadre = " + IdPerfilcomp + " AND FlagActivo = true")) { canHijos++; }

                        if (canHijos != 0)
                        {
                            var rpta = DevExpress.Xpf.Core.DXMessageBox.Show("Se eliminara todo su contenido del item seleccionado \n¿Desea continuar?", "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (rpta == MessageBoxResult.No) { return; }
                        }

                        EliminarComponentesNuevos(IdPerfilcomp);

                        objEPerfilComp.Idperfilcomp = trm.IdMenu;
                        DataTable tblCont = objPerfilComp.PerfilComp_GetBeforeDel(objEPerfilComp);

                        if (Convert.ToInt32(tblCont.Rows[0]["Contador"]) == 0)
                        {
                            EliminarComponentesExistentes(IdPerfilcomp);
                        }
                        else
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_DELE_COMP_ASIG"), 2);
                        }
                    }
                }
                else
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        void TrvComp_Refresh()
        {
            tblPerfilComponentes.AcceptChanges();
            DataView dtvPerfilComponentes = new DataView(tblPerfilComponentes);
            dtvPerfilComponentes.RowFilter = "FlagActivo = true";
            trvComp.ItemsSource = null;
            Utilitarios.TreeViewModel.LimpiarDatosTreeview();
            Utilitarios.TreeViewModel.tblListarPerfilComponentes = dtvPerfilComponentes.ToTable();
            trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponente(1000, null);
            EstadoForm(false, true, false);
        }

        private void menuCambiarDesc_Click(object sender, RoutedEventArgs e)
        {
            TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
            if (trm != null)
            {
                gintIdPerfilComp = trm.IdMenu;
                gintNivelPerfilComp = trm.Nivel;
                txtcambiardesc.Text = trm.Name;
                CambiarBotonDefecto(false);
                btnCambiarDescripcion.IsDefault = true;
                btnCancelarCambiarDescripcion.IsCancel = true;
                txtcambiardesc.Focus();
                stkCambiarDescripcion.Visibility = Visibility.Visible;
            }
            else
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_SELE_COMP"), 2);
                return;
            }
        }

        private void btnCambiarDescripcion_Click(object sender, RoutedEventArgs e)
        {
            if (txtcambiardesc.Text.Trim() == "")
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "OBLI_DESC_COMP"), 2);
                txtComoponente.Focus();
                return;
            }
            foreach (DataRow drExiste in tblPerfilComponentes.Select("PerfilComp = '" + txtComponenteHijo.Text + "' AND Nivel = " + gintNivelPerfilComp))
            {
                GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_DUPL_COMP"), gintNivelPerfilComp), 2);
                txtComponenteHijo.Focus();
                return;
            }

            foreach (DataRow drPfComp in tblPerfilComponentes.Select("IdPerfilComp =" + gintIdPerfilComp))
            {
                drPfComp["PerfilComp"] = txtcambiardesc.Text;
            }

            trvComp.ItemsSource = null;
            Utilitarios.TreeViewModel.LimpiarDatosTreeview();
            Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblPerfilComponentes;
            trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponente(1000, null);
            txtcambiardesc.Text = "";
            CambiarBotonDefecto(true);
            btnCambiarDescripcion.IsDefault = false;
            btnCancelarCambiarDescripcion.IsCancel = false;
            stkCambiarDescripcion.Visibility = Visibility.Hidden;
        }

        private void btnCancelarCambiarDescripcion_Click(object sender, RoutedEventArgs e)
        {
            CambiarBotonDefecto(true);
            btnCambiarDescripcion.IsDefault = false;
            btnCancelarCambiarDescripcion.IsCancel = false;
            txtcambiardesc.Text = "";
            stkCambiarDescripcion.Visibility = Visibility.Hidden;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            DataTable tblActividad_Grid = (DataTable)dtgActividades.ItemsSource;
            if (gbolAllActividades)
            {
                for (int i = 0; i < tblActividad_Grid.Rows.Count; i++)
                {
                    tblActividad_Grid.Rows[i]["IsChecked"] = gbolAllActividades;
                }
                gbolAllActividades = false;
            }
            else if (!gbolAllActividades)
            {
                for (int i = 0; i < tblActividad_Grid.Rows.Count; i++)
                {
                    tblActividad_Grid.Rows[i]["IsChecked"] = gbolAllActividades;
                }
                gbolAllActividades = true;
            }
            dtgActividades.ItemsSource = tblActividad_Grid;
        }

        private void txtDescr_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtDescr.IsReadOnly)
            {
                return;
            }
            EstadoForm(false, true, false);
            tblPerfilComponentes.Rows[0]["PerfilComp"] = txtDescr.Text;
            trvComp.ItemsSource = null;
            Utilitarios.TreeViewModel.LimpiarDatosTreeview();
            Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblPerfilComponentes;
            trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponente(1000, null);
        }

        private void rbnInac_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rbnActi.Checked -= new RoutedEventHandler(rbnActi_Checked);
                EstadoForm(false, true, false);
                rbnActi.Checked += new RoutedEventHandler(rbnActi_Checked);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void rbnActi_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgPLANTILLA_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (dtgCiclo.CurrentColumn.FieldName == "FrecuenciaCambio")
                {
                    EstadoForm(false, true, false);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void cboCicloPerfil_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboCompSAP_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable tblAlmacenes = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 42", dtvMaestra);
                BEOITMGetItem = InterfazMTTO.iSBO_BL.Articulo_BL.ListarArticulos(cboCompSAP.EditValue.ToString(), tblAlmacenes.Rows[0]["Valor"].ToString(), ref RPTA);
                if(BEOITMGetItem.Count == 0)
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 2);
                }
                else
                {
                    if (RPTA.CodigoErrorUsuario == "000")
                    {
                        if (BEOITMGetItem[0].Bloqueado != "N")
                        {
                            GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "ALER_COMP_BLOC"), 2);
                            Error.EscribirError("SAP", Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "ALER_COMP_BLOC"), "Seleccion de CódigoSAP", "cboCompSAP_SelectedIndexChanged", "cboCompSAP_SelectedIndexChanged", "", "", "");
                        }
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                    }
                }              
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboTipoUnid_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboPerfNeum_PopupOpening(object sender, DevExpress.Xpf.Editors.OpenPopupEventArgs e)
        {
            if (cboPerfNeum.EditValue != null)
            {
                string PerfilNeumatico = cboPerfNeum.EditValue.ToString();
                objEPerfilComp.Idperfil = Convert.ToInt32(dtgPUC.GetCellDisplayText(tblvPerfil.FocusedRowHandle, "IdPerfil"));
                DataTable tblCont = objPerfilComp.PerfilComp_GetBeforeChange(objEPerfilComp, 0);
                if (Convert.ToInt32(tblCont.Rows[0]["Contador"]) != 0)
                {
                    dtvPerfilNeumatico.RowFilter = "IdPerfilNeumatico = " + PerfilNeumatico;
                    cboPerfNeum.ToolTip = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_PFNM_ASIG");
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPerfilUC, "LOGI_PFNM_ASIG"), 2);
                }
                else
                {

                    bool existe = false;
                    foreach (DataRow drVerifica in dtvPerfilNeumatico.ToTable().Select(""))
                    {
                        existe = true;
                        break;
                    }
                    if (existe)
                    {
                        dtvPerfilNeumatico.RowFilter = "";
                    }
                    else
                    {
                        dtvPerfilNeumatico.RowFilter = "IdEstadoPN = 1";
                    }
                }
            }
            else
            {
                dtvPerfilNeumatico.RowFilter = "IdEstadoPN = 1";
            }

        }

        void ValidarCamposNullEnGrillas()
        {
            try
            {
                if (dtgCiclo.ItemsSource != null)
                {
                    DataTable tblciclo = tblPerfilCompCiclo;
                    foreach (DataRow drPfCiclo in tblPerfilCompCiclo.Select("FrecuenciaCambio IS NULL"))
                    {
                        if (drPfCiclo["FrecuenciaCambio"].Equals(DBNull.Value))
                        {
                            drPfCiclo["FrecuenciaCambio"] = 0.00;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgCiclo_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            try
            {
                ValidarCamposNullEnGrillas();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgCiclo_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidarCamposNullEnGrillas();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEditarTarea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                gbolFlagEditarDetAct = true;
                int IdTarea = Convert.ToInt32(dtgTarea.GetFocusedRowCellValue("IdTarea"));
                int IdPerfilCompActividad = Convert.ToInt32(dtgTarea.GetFocusedRowCellValue("IdPerfilCompActividad"));
                foreach (DataRow drPfDet in tblPerfilTarea.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND IdTarea = " + IdTarea))
                {
                    gdrDetActividad = drPfDet;
                }
                lblTituloTareas.Text = "Modificar Tarea";
                lblTarea.Content = gdrDetActividad["Tarea"].ToString();
                TimeSpan ts = TimeSpan.FromHours(Decimal.ToDouble(Convert.ToDecimal(gdrDetActividad["HorasHombre"])));
                string timeFormat = String.Format("{0:00}:{1:00}", ts.Hours.ToString(), ts.Minutes.ToString());
                txtHorasHombre.Text = timeFormat;
                cboEstadoTarea.EditValue = Convert.ToInt32(gdrDetActividad["IdEstado"]);
                CambiarBotonDefecto(false);
                btnAgregarTarea.IsDefault = true;
                btnCancelarTarea.IsCancel = true;
                cboTarea.Visibility = Visibility.Collapsed;
                lblTarea.Visibility = Visibility.Visible;
                stkPanelTareas.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEditarHerramienta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                E_Herramienta objE_Herramienta = new E_Herramienta();

                gbolFlagEditarDetAct = true;
                int IdPerfilDetalle = Convert.ToInt32(dtgHerrEspe.GetFocusedRowCellValue("IdPerfilDetalle"));
                int IdPerfilCompActividad = Convert.ToInt32(dtgHerrEspe.GetFocusedRowCellValue("IdPerfilCompActividad"));

                foreach (DataRow drPfDet in tblPerfilDetalleHerrEsp.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND IdPerfilDetalle = " + IdPerfilDetalle))
                {
                    gdrDetActividad = drPfDet;
                }
                objE_Herramienta.IdHerramienta = Convert.ToInt32(gdrDetActividad["IdArticulo"]);
                lblTituloHerramientas.Text = "Modificar Herramienta Especial";
                lblHerramienta.Content = gdrDetActividad["Articulo"].ToString();
                spCantidadHerramienta.Value = Convert.ToInt32(gdrDetActividad["Cantidad"]);
                spCantidadHerramienta.MaxValue = objHerramienta.Herramienta_GetCantItems(objE_Herramienta);
                CambiarBotonDefecto(false);
                btnAceptarHerramientaEspecial.IsDefault = true;
                btnCancelarHerramientaEspecial.IsCancel = true;
                cboHerramientaEspecial.Visibility = Visibility.Collapsed;
                lblHerramienta.Visibility = Visibility.Visible;
                stkPanelHerramientas.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEditarRepuesto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                gbolFlagEditarDetAct = true;
                int IdPerfilDetalle = Convert.ToInt32(dtgRepu.GetFocusedRowCellValue("IdPerfilDetalle"));
                int IdPerfilCompActividad = Convert.ToInt32(dtgRepu.GetFocusedRowCellValue("IdPerfilCompActividad"));

                foreach (DataRow drPfDet in tblPerfilDetalleRepuesto.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND IdPerfilDetalle = " + IdPerfilDetalle))
                {
                    gdrDetActividad = drPfDet;
                }
                lblTituloRepuestos.Text = "Modificar Repuesto";
                lblRepuesto.Content = gdrDetActividad["Articulo"].ToString();
                spCantidadRepuesto.Value = Convert.ToInt32(gdrDetActividad["Cantidad"]);
                CambiarBotonDefecto(false);
                btnAceptarRepusto.IsDefault = true;
                btnCancelarRepuesto.IsCancel = true;
                cboRepuesto.Visibility = Visibility.Collapsed;
                lblRepuesto.Visibility = Visibility.Visible;
                stkPanelRepuestos.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEditarConsumibles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gbolFlagInactivo) { return; }
                gbolFlagEditarDetAct = true;
                int IdPerfilDetalle = Convert.ToInt32(dtgCons.GetFocusedRowCellValue("IdPerfilDetalle"));
                int IdPerfilCompActividad = Convert.ToInt32(dtgCons.GetFocusedRowCellValue("IdPerfilCompActividad"));

                foreach (DataRow drPfDet in tblPerfilDetalleConsumible.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND IdPerfilDetalle = " + IdPerfilDetalle))
                {
                    gdrDetActividad = drPfDet;
                }
                lblTituloHerramientas.Text = "Modificar Consumible";
                lblConsumibles.Content = gdrDetActividad["Articulo"].ToString();
                spCantidadConsumible.Value = Convert.ToInt32(gdrDetActividad["Cantidad"]);
                CambiarBotonDefecto(false);
                btnAceptarConsumibles.IsDefault = true;
                btnCancelarConsumibles.IsCancel = true;
                cboConsumibles.Visibility = Visibility.Collapsed;
                lblConsumibles.Visibility = Visibility.Visible;
                stkPanelConsumibles.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtPLANTILLA_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void tableView3_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)e.Row;
                int FrecuenciaCambio = Convert.ToInt32(dr["FrecuenciaCambio"]);
                int IdCiclo = Convert.ToInt32(dr["IdCiclo"]);
                if (((FrecuenciaCambio * gintValorTiempoDefecto).ToString().Length > 17 || (FrecuenciaCambio * gintValorTiempoDefecto).ToString().Contains("E")) && IdCiclo == 4)
                {
                    e.IsValid = false;
                    e.ErrorContent = "La frecuencia de cambio excede la cantidad máxima en nuestra base de datos";
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void lstbActividad_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lstActi.SelectionChanged -= new SelectionChangedEventHandler(lstActi_SelectionChanged);

                DataRowView drActiExis = (DataRowView)lstActi.SelectedItem;
                if (e.Key != Key.Delete) { return; }
                var rpt = DevExpress.Xpf.Core.DXMessageBox.Show(string.Format("¿Seguro de eliminar la actividad: {0} ?", drActiExis["Actividad"].ToString()), "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rpt == MessageBoxResult.No) { return; }
                //tblPerfilCompActividad, tblPerfilTarea, tblPerfilDetalleHerrEsp, tblPerfilDetalleRepuesto, tblPerfilDetalleConsumible
                foreach (DataRow drActividad in tblPerfilCompActividad.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActiExis["IdPerfilCompActividad"])))
                {
                    foreach (DataRow drTareaDatos in tblPerfilTarea.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActividad["IdPerfilCompActividad"])))
                    {
                        if (Convert.ToBoolean(drTareaDatos["Nuevo"])) { drTareaDatos.Delete(); } else { drTareaDatos["FlagActivo"] = false; }
                    }
                    foreach (DataRow drDetalleDatos in tblPerfilDetalleHerrEsp.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActividad["IdPerfilCompActividad"])))
                    {
                        if (Convert.ToBoolean(drDetalleDatos["Nuevo"])) { drDetalleDatos.Delete(); } else { drDetalleDatos["FlagActivo"] = false; }
                    }

                    foreach (DataRow drDetalleDatos in tblPerfilDetalleRepuesto.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActividad["IdPerfilCompActividad"])))
                    {
                        if (Convert.ToBoolean(drDetalleDatos["Nuevo"])) { drDetalleDatos.Delete(); } else { drDetalleDatos["FlagActivo"] = false; }
                    }

                    foreach (DataRow drDetalleDatos in tblPerfilDetalleConsumible.Select("IdPerfilCompActividad = " + Convert.ToInt32(drActividad["IdPerfilCompActividad"])))
                    {
                        if (Convert.ToBoolean(drDetalleDatos["Nuevo"])) { drDetalleDatos.Delete(); } else { drDetalleDatos["FlagActivo"] = false; }
                    }

                    if (Convert.ToBoolean(drActividad["Nuevo"])) { drActividad.Delete(); } else { drActividad["FlagActivo"] = false; }
                }

                dtgTarea.ItemsSource = null;
                dtgHerrEspe.ItemsSource = null;
                dtgRepu.ItemsSource = null;
                dtgCons.ItemsSource = null;
                EstadoForm(false, true, false);
                lstActi.SelectionChanged += new SelectionChangedEventHandler(lstActi_SelectionChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalClass.GeneraImpresion(gintIdMenu, gintIdPerilUC);
            }
            catch { }
        }
    }
}
        #endregion