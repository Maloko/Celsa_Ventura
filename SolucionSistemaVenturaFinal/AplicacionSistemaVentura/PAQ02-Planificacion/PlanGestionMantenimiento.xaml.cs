using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using Entities;
using Business;
using Utilitarios;

namespace AplicacionSistemaVentura.PAQ02_Planificacion
{
    /// <summary>
    /// Interaction logic for PlanGestionMantenimiento.xaml
    /// </summary>
    public partial class PlanGestionMantenimiento : UserControl
    {

        E_PM E_PM = new E_PM();
        E_TablaMaestra E_TabMaes = new E_TablaMaestra();
        E_PerfilComp E_PComp = new E_PerfilComp();
        E_Perfil E_Perfil = new E_Perfil();
        B_PM objPM = new B_PM();
        B_Perfil objPerfil = new B_Perfil();
        B_PerfilComp objPComp = new B_PerfilComp();
        B_PerfilComp_Ciclo objPCiclo = new B_PerfilComp_Ciclo();
        B_PerfilComp_Actividad objPerActi = new B_PerfilComp_Actividad();
        B_TablaMaestra objTabMaes = new B_TablaMaestra();
        B_Ciclo objCiclos = new B_Ciclo();
        Utilitarios.Utilitarios objUtil = new Utilitarios.Utilitarios();
        DataView dtvMaestra = new DataView();
        DataTable tblPerfilComponentes = new DataTable();
        DataTable tblPerfilCompActividad = new DataTable();
        DataTable tblPerfilCompActividadtmp = new DataTable();
        DataTable tblPMFrecuencias = new DataTable();
        DataTable tblPMComp = new DataTable();
        DataTable tblMPComp_Actividad = new DataTable();

        Boolean gbolFlagInactivo = false;
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        int gintCount = 1;
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();
        int gintIDFrec = 0;
        string OldEstado = "";

        int gintValorTiempoDefecto = 0;
        int gintTiempoDefecto = 0;
        DateTime FechaModificacion;
        string gstrEtiquetaPlanMantenimiento = "PlanGestionMantenimiento";

        public PlanGestionMantenimiento()
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

        private void IniciarTablasTemporales()
        {
            tblPerfilComponentes = new DataTable();
            tblPerfilComponentes.Columns.Add("IdPerfilCompPadre", Type.GetType("System.Int32"));
            tblPerfilComponentes.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
            tblPerfilComponentes.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
            tblPerfilComponentes.Columns.Add("Nivel", Type.GetType("System.Int32"));
            tblPerfilComponentes.Columns.Add("PerfilComp", Type.GetType("System.String"));
            tblPerfilComponentes.Columns.Add("IdUC", Type.GetType("System.Int32"));
            tblPerfilComponentes.Columns.Add("IdTipoDetalle", Type.GetType("System.Int32"));
            tblPerfilComponentes.Columns.Add("IdItem", Type.GetType("System.Int32"));
            tblPerfilComponentes.Columns.Add("NroSerie", Type.GetType("System.String"));
            tblPerfilComponentes.Columns.Add("CodigoSAP", Type.GetType("System.String"));
            tblPerfilComponentes.Columns.Add("DescripcionSAP", Type.GetType("System.String"));
            tblPerfilComponentes.Columns.Add("IdEstadoUCComp", Type.GetType("System.Int32"));
            tblPerfilComponentes.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
            tblPerfilComponentes.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

            tblPerfilCompActividad = new DataTable();
            tblPerfilCompActividad.Columns.Add("IsChecked", Type.GetType("System.Boolean"));
            tblPerfilCompActividad.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
            tblPerfilCompActividad.Columns.Add("IdPMComp", Type.GetType("System.Int32"));
            tblPerfilCompActividad.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
            tblPerfilCompActividad.Columns.Add("Actividad", Type.GetType("System.String"));
            tblPerfilCompActividad.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
            tblPerfilCompActividad.Columns.Add("IdActividad", Type.GetType("System.Int32"));
            tblPerfilCompActividad.Columns.Add("Activo", Type.GetType("System.Boolean"));
            tblPerfilCompActividad.Columns.Add("Uso", Type.GetType("System.Boolean"));
        }

        private void IniciarTablasPM()
        {
            tblPMComp = new DataTable();
            tblPMComp.Columns.Add("IdPMComp", Type.GetType("System.Int32"));
            tblPMComp.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
            tblPMComp.Columns.Add("IdPM", Type.GetType("System.Int32"));
            tblPMComp.Columns.Add("IdEstadoPMC", Type.GetType("System.Int32"));
            tblPMComp.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
            tblPMComp.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

            tblMPComp_Actividad = new DataTable();
            tblMPComp_Actividad.Columns.Add("IdPMCompActividad", Type.GetType("System.Int32"));
            tblMPComp_Actividad.Columns.Add("IdPMComp", Type.GetType("System.Int32"));
            tblMPComp_Actividad.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
            tblMPComp_Actividad.Columns.Add("IdPerfilCompActividad", Type.GetType("System.Int32"));
            tblMPComp_Actividad.Columns.Add("IdEstadoPMA", Type.GetType("System.Int32"));
            tblMPComp_Actividad.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
            tblMPComp_Actividad.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

            tblPMFrecuencias = new DataTable();
            tblPMFrecuencias.Columns.Add("IdPMCompFrecuencia", Type.GetType("System.Int32"));
            tblPMFrecuencias.Columns.Add("IdPM", Type.GetType("System.Int32"));
            tblPMFrecuencias.Columns.Add("Frecuencia", Type.GetType("System.Double"));
            tblPMFrecuencias.Columns.Add("IdEstadoPMF", Type.GetType("System.Int32"));
            tblPMFrecuencias.Columns.Add("EstadoF", Type.GetType("System.String"));
            tblPMFrecuencias.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
            tblPMFrecuencias.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
        }

        private void ListarPMantenimiento()
        {
            E_PM.FlagActivo = true;
            dtgListPM.ItemsSource = B_PM.PM_List(E_PM);
        }

        private void UserControl_Loaded()
        {
            trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
            CboPerfil.SelectedIndexChanged -= new RoutedEventHandler(CboPerfil_SelectedIndexChanged);

            IniciarTablasTemporales();
            IniciarTablasPM();

            txtFrecuencia.MaxLength = 18;
            txtDescripcion.MaxLength = 100;

            E_TabMaes.IdTabla = 0;
            dtvMaestra = B_TablaMaestra.TablaMaestra_Combo(E_TabMaes).DefaultView;
            ListarPMantenimiento();
            CboPerfil.ItemsSource = objPerfil.Perfil_Combo();
            CboPerfil.DisplayMember = "Perfil";
            CboPerfil.ValueMember = "IdPerfil";

            CboEstado.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 24", dtvMaestra);
            CboEstado.DisplayMember = "Descripcion";
            CboEstado.ValueMember = "Valor";

            CboTipoOTDefecto.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 20", dtvMaestra);
            CboTipoOTDefecto.DisplayMember = "Descripcion";
            CboTipoOTDefecto.ValueMember = "Valor";

            cboEstFrec.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 24", dtvMaestra);
            cboEstFrec.DisplayMember = "Descripcion";
            cboEstFrec.ValueMember = "Valor";

            gintTiempoDefecto = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1000", dtvMaestra).Rows[7]["Valor"]);
            gintValorTiempoDefecto = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 58", dtvMaestra).Select("IdColumna = " + gintTiempoDefecto)[0][2]);

            EstadoForm(false, false, true);
            GlobalClass.ip.SeleccionarTab(tbListaPM);
            trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
            CboPerfil.SelectedIndexChanged += new RoutedEventHandler(CboPerfil_SelectedIndexChanged);

        }

        private void BtnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                E_PM.PM = txtDescripcion.Text;
                E_PM.IdPerfil = Convert.ToInt32(CboPerfil.EditValue);
                E_PM.IdCiclo = Convert.ToInt32(CboCiclo.EditValue);
                E_PM.Porc01 = Convert.ToInt32(spLimiteAmarillo.Value);
                E_PM.Porc02 = Convert.ToInt32(spLimiteNaranja.Value);
                E_PM.IdTipoOTDefecto = Convert.ToInt32(CboTipoOTDefecto.EditValue);
                E_PM.IdEstadoPM = Convert.ToInt32(CboEstado.EditValue);
                E_PM.Prioridad = 1;
                E_PM.FlagActivo = true;
                E_PM.IdUsuarioCreacion = 1;

                int activo = tblPMFrecuencias.Select("IdEstadoPMF = 1").Length;
                if (activo != 1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "LOGI_FREC_ACTI"), 2);
                    return;
                }

                if (Convert.ToInt32(CboCiclo.EditValue) == 4)
                {
                    for (int i = 0; i < tblPMFrecuencias.Rows.Count; i++)
                    {
                        tblPMFrecuencias.Rows[i]["Frecuencia"] = Convert.ToDouble(tblPMFrecuencias.Rows[i]["Frecuencia"]) * gintValorTiempoDefecto;
                        tblPMFrecuencias.Rows[i]["EstadoF"] = (Convert.ToInt32(tblPMFrecuencias.Rows[i]["IdEstadoPMF"]) == 1) ? "Activo" : "Inactivo";
                    }
                }


                int IdPMCompActividad = 1;
                int IdPmComp = 1;
                if (gbolNuevo == true && gbolEdicion == false)
                {
                    if (ValidaCampoObligado() == true) { return; }
                    E_PM.IdPM = 0;

                    foreach (DataRow drPFCompActiv in tblPerfilCompActividad.Select("IsChecked = true"))
                    {
                        DataRow dr;
                        dr = tblMPComp_Actividad.NewRow();
                        dr["IdPMCompActividad"] = IdPMCompActividad;
                        dr["IdPMComp"] = 0;
                        dr["IdPerfilComp"] = Convert.ToInt32(drPFCompActiv["IdPerfilComp"]); ;
                        dr["IdPerfilCompActividad"] = Convert.ToInt32(drPFCompActiv["IdPerfilCompActividad"]);
                        dr["IdEstadoPMA"] = Convert.ToInt32(drPFCompActiv["IsChecked"]);
                        dr["FlagActivo"] = true;
                        dr["Nuevo"] = true;
                        tblMPComp_Actividad.Rows.Add(dr);
                        IdPMCompActividad++;
                    }

                    for (int i = 0; i < tblMPComp_Actividad.Rows.Count; i++)
                    {
                        int IdPerfilComp = Convert.ToInt32(tblMPComp_Actividad.Rows[i]["IdPerfilComp"]);
                        foreach (DataRow drPFComp in tblPerfilComponentes.Select("IdPerfilComp = " + IdPerfilComp))
                        {
                            int existe = tblPMComp.Select("IdPerfilComp = " + IdPerfilComp).Length;
                            if (existe == 0)
                            {
                                DataRow dr = tblPMComp.NewRow();
                                dr["IdPMComp"] = IdPmComp;
                                dr["IdPerfilComp"] = drPFComp["IdPerfilComp"];
                                dr["IdPM"] = 0;
                                dr["IdEstadoPMC"] = 1;
                                dr["FlagActivo"] = true;
                                dr["Nuevo"] = true;
                                tblPMComp.Rows.Add(dr);
                                IdPmComp++;
                            }
                        }
                    }
                    for (int i = 0; i < tblPMComp.Rows.Count; i++)
                    {
                        for (int a = 0; a < tblMPComp_Actividad.Rows.Count; a++)
                        {
                            if (Convert.ToInt32(tblPMComp.Rows[i]["IdPerfilComp"]) == Convert.ToInt32(tblMPComp_Actividad.Rows[a]["IdPerfilComp"]))
                            {
                                tblMPComp_Actividad.Rows[a]["IdPMComp"] = Convert.ToInt32(tblPMComp.Rows[i]["IdPMComp"]);
                            }
                        }
                    }

                    if (tblMPComp_Actividad.Select("IdEstadoPMA <> 0").Length == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "LOGI_ACTI_CANT"), 2);
                        tblMPComp_Actividad.Rows.Clear();
                        return;
                    }

                    else if (tblPMFrecuencias.Rows.Count == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "LOGI_FREC_CANT"), 2);
                        return;
                    }

                    tblPMFrecuencias.Columns.Remove("EstadoF");
                    tblMPComp_Actividad.Columns.Remove("IdPerfilComp");
                    E_PM.FechaModificacion = DateTime.Now;
                    int nresp = objPM.Perfil_InsertMasivo(E_PM, tblPMComp, tblMPComp_Actividad, tblPMFrecuencias);
                    if (nresp == 1)
                    {
                        EstadoForm(false, false, true);
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "GRAB_NUEV"), 1);
                    }
                    else if (nresp == 0)
                    {
                        GlobalClass.Columna_AddIFnotExits(tblPMFrecuencias, "EstadoF", Type.GetType("System.String"));
                        GlobalClass.Columna_AddIFnotExits(tblMPComp_Actividad, "IdPerfilComp", Type.GetType("System.Int32"));
                        if (Convert.ToInt32(CboCiclo.EditValue) == 4)
                        {
                            for (int i = 0; i < tblPMFrecuencias.Rows.Count; i++)
                            {
                                tblPMFrecuencias.Rows[i]["Frecuencia"] = Convert.ToDouble(tblPMFrecuencias.Rows[i]["Frecuencia"]) / gintValorTiempoDefecto;
                                tblPMFrecuencias.Rows[i]["EstadoF"] = (Convert.ToInt32(tblPMFrecuencias.Rows[i]["IdEstadoPMF"]) == 1) ? "Activo" : "Inactivo";
                            }
                        }
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (nresp == 1205)
                    {
                        GlobalClass.Columna_AddIFnotExits(tblPMFrecuencias, "EstadoF", Type.GetType("System.String"));
                        GlobalClass.Columna_AddIFnotExits(tblMPComp_Actividad, "IdPerfilComp", Type.GetType("System.Int32"));
                        if (Convert.ToInt32(CboCiclo.EditValue) == 4)
                        {
                            for (int i = 0; i < tblPMFrecuencias.Rows.Count; i++)
                            {
                                tblPMFrecuencias.Rows[i]["Frecuencia"] = Convert.ToDouble(tblPMFrecuencias.Rows[i]["Frecuencia"]) / gintValorTiempoDefecto;
                                tblPMFrecuencias.Rows[i]["EstadoF"] = (Convert.ToInt32(tblPMFrecuencias.Rows[i]["IdEstadoPMF"]) == 1) ? "Activo" : "Inactivo";
                            }
                        }
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "GRAB_CONC"), 2);
                        return;
                    }
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    if (ValidaCampoObligado() == true) { return; }
                    E_PM.IdPM = Convert.ToInt32(dtgListPM.GetFocusedRowCellValue("IdPM"));

                    foreach (DataRow drPFCompActiv in tblPerfilCompActividad.Select("IsChecked = true AND IdPMComp = IdPerfilComp"))
                    {
                        DataRow dr;
                        dr = tblMPComp_Actividad.NewRow();
                        dr["IdPMCompActividad"] = IdPMCompActividad;
                        if (Convert.ToBoolean(drPFCompActiv["Nuevo"]))
                        {
                            foreach (DataRow drPFComp in tblPerfilComponentes.Select("IdPerfilComp = " + drPFCompActiv["IdPerfilComp"]))
                            {
                                int existe = tblPMComp.Select("IdPerfilComp = " + drPFCompActiv["IdPerfilComp"]).Length;
                                if (existe == 0)
                                {
                                    DataRow row = tblPMComp.NewRow();
                                    row["IdPMComp"] = IdPmComp;
                                    dr["IdPMComp"] = IdPmComp;
                                    row["IdPerfilComp"] = drPFComp["IdPerfilComp"];
                                    row["IdPM"] = 0;
                                    row["IdEstadoPMC"] = 1;
                                    row["FlagActivo"] = true;
                                    row["Nuevo"] = true;
                                    tblPMComp.Rows.Add(row);
                                    IdPmComp++;
                                }
                                else
                                {
                                    dr["IdPMComp"] = Convert.ToInt32(tblPMComp.Select("IdPerfilComp = " + drPFCompActiv["IdPerfilComp"])[0]["IdPMComp"]);
                                }
                            }

                        }
                        else
                        {
                            dr["IdPMComp"] = Convert.ToInt32(drPFCompActiv["IdPerfilComp"]);
                        }

                        dr["IdPerfilComp"] = Convert.ToInt32(drPFCompActiv["IdPerfilComp"]);
                        dr["IdPerfilCompActividad"] = Convert.ToInt32(drPFCompActiv["IdPerfilCompActividad"]);
                        dr["IdEstadoPMA"] = Convert.ToInt32(drPFCompActiv["IsChecked"]);
                        dr["FlagActivo"] = true;
                        dr["Nuevo"] = true;
                        tblMPComp_Actividad.Rows.Add(dr);
                        IdPMCompActividad++;

                    }

                    for (int i = 0; i < tblPerfilCompActividad.Rows.Count; i++)
                    {
                        for (int a = 0; a < tblMPComp_Actividad.Rows.Count; a++)
                        {
                            if (Convert.ToInt32(tblPerfilCompActividad.Rows[i]["IdPerfilCompActividad"]) == Convert.ToInt32(tblMPComp_Actividad.Rows[a]["IdPerfilCompActividad"])
                                && Convert.ToInt32(tblPerfilCompActividad.Rows[i]["IdPMComp"]) == Convert.ToInt32(tblMPComp_Actividad.Rows[a]["IdPMComp"]))
                            {
                                tblMPComp_Actividad.Rows[a]["IdEstadoPMA"] = Convert.ToInt32(tblPerfilCompActividad.Rows[i]["IsChecked"]);
                            }
                        }
                    }

                    if (tblMPComp_Actividad.Select("IdEstadoPMA <> 0").Length == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "LOGI_ACTI_CANT"), 2);
                        return;
                    }

                    tblPMFrecuencias.Columns.Remove("EstadoF");
                    tblMPComp_Actividad.Columns.Remove("IdPerfilComp");
                    E_PM.FechaModificacion = FechaModificacion;
                    int nresp = objPM.Perfil_InsertMasivo(E_PM, tblPMComp, tblMPComp_Actividad, tblPMFrecuencias);
                    if (nresp == 1)
                    {
                        EstadoForm(false, false, true);
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "GRAB_EDIT"), 1);
                    }
                    else if (nresp == 0)
                    {
                        GlobalClass.Columna_AddIFnotExits(tblPMFrecuencias, "EstadoF", Type.GetType("System.String"));
                        GlobalClass.Columna_AddIFnotExits(tblMPComp_Actividad, "IdPerfilComp", Type.GetType("System.Int32"));
                        if (Convert.ToInt32(CboCiclo.EditValue) == 4)
                        {
                            for (int i = 0; i < tblPMFrecuencias.Rows.Count; i++)
                            {
                                tblPMFrecuencias.Rows[i]["Frecuencia"] = Convert.ToDouble(tblPMFrecuencias.Rows[i]["Frecuencia"]) / gintValorTiempoDefecto;
                                tblPMFrecuencias.Rows[i]["EstadoF"] = (Convert.ToInt32(tblPMFrecuencias.Rows[i]["IdEstadoPMF"]) == 1) ? "Activo" : "Inactivo";
                            }
                        }
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (nresp == 1205)
                    {
                        GlobalClass.Columna_AddIFnotExits(tblPMFrecuencias, "EstadoF", Type.GetType("System.String"));
                        GlobalClass.Columna_AddIFnotExits(tblMPComp_Actividad, "IdPerfilComp", Type.GetType("System.Int32"));
                        if (Convert.ToInt32(CboCiclo.EditValue) == 4)
                        {
                            for (int i = 0; i < tblPMFrecuencias.Rows.Count; i++)
                            {
                                tblPMFrecuencias.Rows[i]["Frecuencia"] = Convert.ToDouble(tblPMFrecuencias.Rows[i]["Frecuencia"]) / gintValorTiempoDefecto;
                                tblPMFrecuencias.Rows[i]["EstadoF"] = (Convert.ToInt32(tblPMFrecuencias.Rows[i]["IdEstadoPMF"]) == 1) ? "Activo" : "Inactivo";
                            }
                        }
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "GRAB_CONC"), 2);
                        return;
                    }
                }
                ListarPMantenimiento();
                LimpiarControlesDatos();
                tabControl1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            finally
            {
                GlobalClass.Columna_AddIFnotExits(tblPMFrecuencias, "EstadoF", Type.GetType("System.String"));
                GlobalClass.Columna_AddIFnotExits(tblMPComp_Actividad, "IdPerfilComp", Type.GetType("System.Int32"));
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarControlesDatos();
            ListarPMantenimiento();
            tabControl1.SelectedIndex = 0;
        }

        private void BtnRegistrarPMantenimiento_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.ip.SeleccionarTab(tbDetallesPM);
            EstadoForm(true, false, true);
            TxTCodigo.Text = "Nuevo Código";
            CboEstado.SelectedIndexChanged -= new RoutedEventHandler(CboEstado_SelectedIndexChanged);
            CboEstado.SelectedIndex = 0;
            CboEstado.SelectedIndexChanged += new RoutedEventHandler(CboEstado_SelectedIndexChanged);
            CboEstado.IsEnabled = false;
            CboPerfil.IsEnabled = true;
            tabControl1.SelectedIndex = 1;
        }

        private void BtnModificarPMantenimiento_Click(object sender, RoutedEventArgs e)
        {
            if (dtgListPM.VisibleRowCount == 0) { return; }
            EstadoForm(false, true, true);
            DetallesPMantenimiendo();
            tabControl1.SelectedIndex = 1;
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
                    tbDetallesPM.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "TAB1_CONS");
                    BtnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "BTNG_CONS");
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tbDetallesPM.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "TAB1_NUEV");
                    BtnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "BTNG_NUEV");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tbDetallesPM.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "TAB1_EDIT");
                    BtnGrabar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "BTNG_EDIT");
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void DetallesPMantenimiendo()
        {
            trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
            CboEstado.SelectedIndexChanged -= new RoutedEventHandler(CboEstado_SelectedIndexChanged);
            CboCiclo.SelectedIndexChanged -= new RoutedEventHandler(CboCiclo_SelectedIndexChanged);
            CboTipoOTDefecto.SelectedIndexChanged -= new RoutedEventHandler(CboTipoOTDefecto_SelectedIndexChanged);
            spLimiteAmarillo.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(spLimiteAmarillo_EditValueChanged);
            spLimiteNaranja.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(spLimiteNaranja_EditValueChanged);
            txtDescripcion.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtDescripcion_EditValueChanged);

            GlobalClass.ip.SeleccionarTab(tbDetallesPM);

            E_PM.IdPM = Convert.ToInt32(dtgListPM.GetFocusedRowCellValue("IdPM"));
            DataTable tblPM_GetItem = objPM.PM_GetItem(E_PM);
            TxTCodigo.Text = tblPM_GetItem.Rows[0]["CodPM"].ToString();
            E_Perfil.Idperfil = Convert.ToInt32(tblPM_GetItem.Rows[0]["IdPerfil"]);
            CboPerfil.EditValue = Convert.ToInt32(tblPM_GetItem.Rows[0]["IdPerfil"]);
            CboCiclo.EditValue = Convert.ToInt32(tblPM_GetItem.Rows[0]["IdCiclo"]);
            CboTipoOTDefecto.EditValue = tblPM_GetItem.Rows[0]["IdTipoOTDefecto"].ToString();
            CboEstado.EditValue = tblPM_GetItem.Rows[0]["IdEstadoPM"].ToString();

            if (Convert.ToInt32(tblPM_GetItem.Rows[0]["IdEstadoPM"]) == 2)
            {
                //gbActividades.IsEnabled = false;
                //gbFrecuencias.IsEnabled = false;
                spLimiteNaranja.IsReadOnly = true;
                spLimiteAmarillo.IsReadOnly = true;
                txtDescripcion.IsReadOnly = true;
                CboPerfil.IsEnabled = false;
                CboEstado.IsEnabled = false;
                CboCiclo.IsEnabled = false;
                CboTipoOTDefecto.IsEnabled = false;
                gbolFlagInactivo = true;
            }
            else
            {
                //gbActividades.IsEnabled = true;
                //gbFrecuencias.IsEnabled = true;
                spLimiteNaranja.IsReadOnly = false;
                spLimiteAmarillo.IsReadOnly = false;
                txtDescripcion.IsReadOnly = false;
                CboPerfil.IsEnabled = false;
                CboEstado.IsEnabled = true;
                CboCiclo.IsEnabled = true;
                CboTipoOTDefecto.IsEnabled = true;

            }

            OldEstado = tblPM_GetItem.Rows[0]["IdEstadoPM"].ToString();
            txtDescripcion.Text = tblPM_GetItem.Rows[0]["PM"].ToString();
            spLimiteAmarillo.Value = Convert.ToInt32(tblPM_GetItem.Rows[0]["Porc01"]);
            spLimiteNaranja.Value = Convert.ToInt32(tblPM_GetItem.Rows[0]["Porc02"]);
            CboPerfil.IsEnabled = false;
            CboEstado.IsEnabled = true;
            DataTable tblFrec = objPM.PMComp_Frecuencia_List(E_PM);
            DataTable tblActi = objPM.PMComp_Actividad_List(E_PM);
            DataTable tblComp = objPM.PMComp_List(E_PM);
            DataTable tblPerfilActividadDatos = objPerActi.PerfilComp_Actividad_List(E_Perfil);

            for (int i = 0; i < tblFrec.Rows.Count; i++)
            {
                DataRow dr = tblPMFrecuencias.NewRow();
                dr["IdPMCompFrecuencia"] = Convert.ToInt32(tblFrec.Rows[i]["IdPMCompFrecuencia"]);
                dr["IdPM"] = Convert.ToInt32(tblFrec.Rows[i]["IdPM"]);
                dr["Frecuencia"] = (Convert.ToInt32(CboCiclo.EditValue) == 4) ? Convert.ToDouble(tblFrec.Rows[i]["Frecuencia"]) / gintValorTiempoDefecto : Convert.ToDouble(tblFrec.Rows[i]["Frecuencia"]);
                dr["IdEstadoPMF"] = Convert.ToInt32(tblFrec.Rows[i]["IdEstadoPMF"]);
                dr["EstadoF"] = tblFrec.Rows[i]["EstadoPMF"].ToString();
                dr["FlagActivo"] = true;
                dr["Nuevo"] = false;
                tblPMFrecuencias.Rows.Add(dr);
            }

            for (int i = 0; i < tblActi.Rows.Count; i++)
            {
                DataRow dr = tblMPComp_Actividad.NewRow();
                dr["IdPMCompActividad"] = Convert.ToInt32(tblActi.Rows[i]["IdPMCompActividad"]);
                dr["IdPMComp"] = Convert.ToInt32(tblActi.Rows[i]["IdPMComp"]);
                dr["IdPerfilCompActividad"] = tblActi.Rows[i]["IdPerfilCompActividad"];
                dr["IdEstadoPMA"] = Convert.ToInt32(tblActi.Rows[i]["IdEstadoPMA"]);
                dr["FlagActivo"] = true;
                dr["Nuevo"] = false;
                tblMPComp_Actividad.Rows.Add(dr);
            }

            for (int i = 0; i < tblComp.Rows.Count; i++)
            {
                DataRow dr = tblPMComp.NewRow();
                dr["IdPMComp"] = Convert.ToInt32(tblComp.Rows[i]["IdPMComp"]);
                dr["IdPerfilComp"] = Convert.ToInt32(tblComp.Rows[i]["IdPerfilComp"]);
                dr["IdPM"] = tblComp.Rows[i]["IdPM"];
                dr["IdEstadoPMC"] = Convert.ToInt32(tblComp.Rows[i]["IdEstadoPMC"]);
                dr["FlagActivo"] = true;
                dr["Nuevo"] = false;
                tblPMComp.Rows.Add(dr);
            }
            dtgFrec.ItemsSource = tblPMFrecuencias;

            tblPerfilCompActividad.Rows.Clear();
            E_PM.IdPM = Convert.ToInt32(dtgListPM.GetFocusedRowCellValue("IdPM"));
            DataTable tblPMActi = objPM.PMComp_Actividad_List(E_PM);
            string IdPerfilCompActividad = "";
            for (int i = 0; i < tblPMActi.Rows.Count; i++)
            {
                DataRow row;
                row = tblPerfilCompActividad.NewRow();
                row["IsChecked"] = Convert.ToBoolean(tblPMActi.Rows[i]["IdEstadoPMA"]);
                row["IdPerfilCompActividad"] = Convert.ToString(tblPMActi.Rows[i]["IdPerfilCompActividad"]);
                row["Actividad"] = Convert.ToString(tblPMActi.Rows[i]["Actividad"]);
                row["IdPMComp"] = Convert.ToInt32(tblPMActi.Rows[i]["IdPMComp"]);
                row["IdPerfilComp"] = Convert.ToInt32(tblPMActi.Rows[i]["IdPerfilComp"]);
                row["Nuevo"] = false;
                row["IdActividad"] = Convert.ToInt32(tblPMActi.Rows[i]["IdActividad"]);
                row["Activo"] = Convert.ToInt32(tblPMActi.Rows[i]["FlagActivo"]);

                foreach (DataRow drPFCActi in tblPerfilActividadDatos.Select("IdPerfilCompActividad = " + Convert.ToInt32(tblPMActi.Rows[i]["IdPerfilCompActividad"])))
                {
                    row["Uso"] = Convert.ToBoolean(drPFCActi["FlagUso"]);
                }

                tblPerfilCompActividad.Rows.Add(row);
                IdPerfilCompActividad += tblPMActi.Rows[i]["IdPerfilCompActividad"] + ",";
            }

            //Verificando si hay nuevas actividades para el PMComp
            for (int i = 0; i < tblComp.Rows.Count; i++)
            {
                string IdPerfilCompActividadtmp = IdPerfilCompActividad.Remove(IdPerfilCompActividad.Length - 1);
                foreach (DataRow drPFCompAct in tblPerfilActividadDatos.Select("IsActivo = true AND IdPerfilCompActividad NOT IN (" + IdPerfilCompActividadtmp + ") AND IdPerfilComp = " + Convert.ToInt32(tblComp.Rows[i]["IdPerfilComp"])))
                {
                    DataRow row;
                    row = tblPerfilCompActividad.NewRow();
                    row["IsChecked"] = false;
                    row["IdPerfilCompActividad"] = Convert.ToString(drPFCompAct["IdPerfilCompActividad"]);
                    row["Actividad"] = Convert.ToString(drPFCompAct["Actividad"]);
                    row["IdPMComp"] = Convert.ToInt32(tblComp.Rows[i]["IdPMComp"]);
                    row["IdPerfilComp"] = Convert.ToInt32(tblComp.Rows[i]["IdPerfilComp"]);
                    row["Nuevo"] = false;
                    row["IdActividad"] = Convert.ToInt32(drPFCompAct["IdActividad"]);
                    row["Activo"] = Convert.ToInt32(drPFCompAct["FlagActivo"]);
                    row["Uso"] = Convert.ToBoolean(drPFCompAct["FlagUso"]);
                    tblPerfilCompActividad.Rows.Add(row);
                    IdPerfilCompActividad += drPFCompAct["IdPerfilCompActividad"] + ",";
                }
            }

            //Agregando Actividades del los componentes nuevos en el perfil uc
            IdPerfilCompActividad = IdPerfilCompActividad.Remove(IdPerfilCompActividad.Length - 1);
            foreach (DataRow drPFCompAct in tblPerfilActividadDatos.Select("IsActivo = true AND IdPerfilCompActividad NOT IN (" + IdPerfilCompActividad + ")"))
            {
                DataRow row;
                row = tblPerfilCompActividad.NewRow();
                row["IsChecked"] = false;
                row["IdPerfilCompActividad"] = Convert.ToString(drPFCompAct["IdPerfilCompActividad"]);
                row["Actividad"] = Convert.ToString(drPFCompAct["Actividad"]);
                row["IdPMComp"] = Convert.ToInt32(drPFCompAct["IdPerfilComp"]);
                row["IdPerfilComp"] = Convert.ToInt32(drPFCompAct["IdPerfilComp"]);
                row["Nuevo"] = true;
                row["IdActividad"] = Convert.ToInt32(drPFCompAct["IdActividad"]);
                row["Activo"] = Convert.ToInt32(drPFCompAct["FlagActivo"]);
                row["Uso"] = Convert.ToBoolean(drPFCompAct["FlagUso"]);
                tblPerfilCompActividad.Rows.Add(row);
            }
            FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
            tblPerfilCompActividadtmp = tblPerfilCompActividad.Copy();
            CboCiclo.SelectedIndexChanged += new RoutedEventHandler(CboCiclo_SelectedIndexChanged);
            CboTipoOTDefecto.SelectedIndexChanged += new RoutedEventHandler(CboTipoOTDefecto_SelectedIndexChanged);
            spLimiteAmarillo.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(spLimiteAmarillo_EditValueChanged);
            spLimiteNaranja.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(spLimiteNaranja_EditValueChanged);
            txtDescripcion.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtDescripcion_EditValueChanged);
            CboEstado.SelectedIndexChanged += new RoutedEventHandler(CboEstado_SelectedIndexChanged);

            trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
        }

        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void LimpiarControlesDatos()
        {
            try
            {
                trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                CboPerfil.SelectedIndexChanged -= new RoutedEventHandler(CboPerfil_SelectedIndexChanged);
                CboEstado.SelectedIndexChanged -= new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                CboCiclo.SelectedIndexChanged -= new RoutedEventHandler(CboCiclo_SelectedIndexChanged);
                CboTipoOTDefecto.SelectedIndexChanged -= new RoutedEventHandler(CboTipoOTDefecto_SelectedIndexChanged);
                spLimiteAmarillo.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(spLimiteAmarillo_EditValueChanged);
                spLimiteNaranja.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(spLimiteNaranja_EditValueChanged);
                txtDescripcion.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtDescripcion_EditValueChanged);

                trvComp.ItemsSource = null;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();

                CboPerfil.SelectedIndex = -1;
                txtDescripcion.Text = "";

                CboCiclo.EditValue = -1;
                CboEstado.EditValue = -1;
                cboEstFrec.EditValue = -1;
                spLimiteAmarillo.Value = 1;
                spLimiteNaranja.Value = 1;
                CboTipoOTDefecto.EditValue = -1;
                TxTCodigo.Text = "Nuevo Código";
                dtgActi.ItemsSource = null;
                dtgFrec.ItemsSource = null;

                gintCount = 1;

                IniciarTablasPM();
                IniciarTablasTemporales();
                gbActividades.IsEnabled = true;
                gbFrecuencias.IsEnabled = true;

                spLimiteNaranja.IsReadOnly = false;
                spLimiteAmarillo.IsReadOnly = false;

                txtDescripcion.IsReadOnly = false;
                CboPerfil.IsEnabled = false;
                CboEstado.IsEnabled = true;
                CboCiclo.IsEnabled = true;
                CboTipoOTDefecto.IsEnabled = true;
                gbolFlagInactivo = false;
                EstadoForm(false, false, true);
                GlobalClass.ip.SeleccionarTab(tbListaPM);
                CboCiclo.SelectedIndexChanged += new RoutedEventHandler(CboCiclo_SelectedIndexChanged);
                CboTipoOTDefecto.SelectedIndexChanged += new RoutedEventHandler(CboTipoOTDefecto_SelectedIndexChanged);
                spLimiteAmarillo.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(spLimiteAmarillo_EditValueChanged);
                spLimiteNaranja.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(spLimiteNaranja_EditValueChanged);
                txtDescripcion.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtDescripcion_EditValueChanged);
                CboEstado.SelectedIndexChanged += new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                CboPerfil.SelectedIndexChanged += new RoutedEventHandler(CboPerfil_SelectedIndexChanged);
                trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LlenarTrvComponentes(int Idperfil)
        {
            try
            {
                trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                CboPerfil.SelectedIndexChanged -= new RoutedEventHandler(CboPerfil_SelectedIndexChanged);

                DataTable tblPerfilComponentesDatos = new DataTable();

                if (gbolNuevo == true && gbolEdicion == false)
                {
                    E_PComp.Idperfil = Idperfil;
                    E_PComp.Idestadopc = 0;
                    //tblPerfilComponentesDatos = objPComp.PerfilComp_List(E_PComp);

                    DataView PerfilCompDatos = objPComp.PerfilComp_List(E_PComp).DefaultView;
                    PerfilCompDatos.RowFilter = "FlagNeumatico = 0";
                    tblPerfilComponentesDatos = PerfilCompDatos.ToTable();

                    IniciarTablasTemporales();
                    DataRow row1;
                    row1 = tblPerfilComponentes.NewRow();
                    row1["IdPerfilCompPadre"] = 1000;
                    row1["IdPerfilComp"] = 0;
                    row1["Nivel"] = 1;
                    row1["PerfilComp"] = CboPerfil.Text;
                    row1["NroSerie"] = "";
                    row1["CodigoSAP"] = "";
                    row1["DescripcionSAP"] = "";
                    row1["Nuevo"] = true;
                    row1["IdEstadoUCComp"] = 1;
                    row1["IdTipoDetalle"] = 1;
                    tblPerfilComponentes.Rows.Add(row1);

                    for (int i = 0; i < tblPerfilComponentesDatos.Rows.Count; i++)
                    {
                        DataRow row;
                        row = tblPerfilComponentes.NewRow();
                        row["IdPerfilCompPadre"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilCompPadre"]);
                        row["IdUCComp"] = 0;
                        row["IdPerfilComp"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilComp"]);
                        row["Nivel"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["Nivel"]);
                        row["PerfilComp"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["PerfilComp"]);
                        row["IdUC"] = 0;
                        row["IdTipoDetalle"] = 0;
                        row["IdItem"] = 0;
                        row["NroSerie"] = "";
                        row["CodigoSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["CodigoSAP"]);
                        row["DescripcionSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["DescripcionSAP"]);
                        row["IdEstadoUCComp"] = Convert.ToBoolean(tblPerfilComponentesDatos.Rows[i]["IdEstadoPC"]);
                        row["FlagActivo"] = true;
                        row["Nuevo"] = true;
                        tblPerfilComponentes.Rows.Add(row);
                    }
                }
                else if ((gbolNuevo == false && gbolEdicion == true) || (gbolNuevo == false && gbolEdicion == false))
                {
                    E_PComp.Idperfil = Idperfil;
                    E_PComp.Idestadopc = 0;
                    DataView PerfilCompDatos = objPComp.PerfilComp_List(E_PComp).DefaultView;
                    PerfilCompDatos.RowFilter = "FlagNeumatico = 0";
                    tblPerfilComponentesDatos = PerfilCompDatos.ToTable();

                    IniciarTablasTemporales();
                    DataRow row1;
                    row1 = tblPerfilComponentes.NewRow();
                    row1["IdPerfilCompPadre"] = 1000;
                    row1["IdPerfilComp"] = 0;
                    row1["Nivel"] = 1;
                    row1["PerfilComp"] = CboPerfil.Text;
                    row1["NroSerie"] = "";
                    row1["CodigoSAP"] = "";
                    row1["DescripcionSAP"] = "";
                    row1["Nuevo"] = true;
                    row1["IdEstadoUCComp"] = 1;
                    row1["IdTipoDetalle"] = 1;
                    tblPerfilComponentes.Rows.Add(row1);

                    E_PM.IdPM = Convert.ToInt32(dtgListPM.GetFocusedRowCellValue("IdPM"));
                    DataTable tblPMComp = objPM.PMComp_List(E_PM);

                    int idpadre = 0;
                    int CantExiste = 0;
                    int CantExisteDatos = 0;
                    DataView dtvPerfilComp = new DataView();
                    string IdPerfilComp = "";
                    for (int i = 0; i < tblPMComp.Rows.Count; i++)
                    {
                        DataRow row;
                        row = tblPerfilComponentes.NewRow();
                        row["IdPerfilCompPadre"] = Convert.ToInt32(tblPMComp.Rows[i]["IdPerfilCompPadre"]);
                        row["IdUCComp"] = Convert.ToInt32(tblPMComp.Rows[i]["IdPMComp"]);
                        row["IdPerfilComp"] = Convert.ToInt32(tblPMComp.Rows[i]["IdPerfilComp"]);
                        row["Nivel"] = Convert.ToInt32(tblPMComp.Rows[i]["Nivel"]);
                        row["PerfilComp"] = Convert.ToString(tblPMComp.Rows[i]["PerfilComp"]);
                        row["IdUC"] = 0;
                        row["IdTipoDetalle"] = 0;
                        row["IdItem"] = 0;
                        row["NroSerie"] = "";
                        row["CodigoSAP"] = "";
                        row["DescripcionSAP"] = "";
                        row["IdEstadoUCComp"] = Convert.ToBoolean(tblPMComp.Rows[i]["IdEstadoPMC"]);
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        tblPerfilComponentes.Rows.Add(row);

                        IdPerfilComp += tblPMComp.Rows[i]["IdPerfilComp"] + ",";
                        int IdPMComp = Convert.ToInt32(tblPMComp.Rows[i]["IdPMComp"]);
                        idpadre = Convert.ToInt32(tblPMComp.Rows[i]["IdPerfilCompPadre"]);
                        CantExiste = tblPerfilComponentes.Select("IdPerfilComp = " + idpadre.ToString()).Length;
                        CantExisteDatos = tblPerfilComponentes.Select("IdPerfilComp = " + idpadre.ToString()).Length;
                        while (idpadre != 0 && CantExiste == 0 && CantExisteDatos == 0)
                        {
                            dtvPerfilComp = new DataView();
                            dtvPerfilComp = tblPerfilComponentesDatos.DefaultView;
                            dtvPerfilComp.RowFilter = "IdPerfilComp = " + idpadre.ToString();
                            DataRow row2;
                            row2 = tblPerfilComponentes.NewRow();
                            row2["IdPerfilCompPadre"] = Convert.ToInt32(dtvPerfilComp[0]["IdPerfilCompPadre"]);
                            row2["IdUCComp"] = 0;
                            row2["IdPerfilComp"] = Convert.ToInt32(dtvPerfilComp[0]["IdPerfilComp"]);
                            row2["Nivel"] = Convert.ToInt32(dtvPerfilComp[0]["Nivel"]);
                            row2["PerfilComp"] = Convert.ToString(dtvPerfilComp[0]["PerfilComp"]);
                            row2["IdUC"] = 0;
                            row2["IdTipoDetalle"] = 0;
                            row2["IdItem"] = 0;
                            row2["NroSerie"] = "";
                            row2["CodigoSAP"] = Convert.ToString(dtvPerfilComp[0]["CodigoSAP"]);
                            row2["DescripcionSAP"] = Convert.ToString(dtvPerfilComp[0]["DescripcionSAP"]);
                            row2["IdEstadoUCComp"] = Convert.ToBoolean(dtvPerfilComp[0]["IdEstadoPC"]);
                            row2["FlagActivo"] = true;
                            row2["Nuevo"] = true;
                            tblPerfilComponentes.Rows.Add(row2);
                            IdPerfilComp += dtvPerfilComp[0]["IdPerfilComp"] + ",";
                            idpadre = Convert.ToInt32(dtvPerfilComp[0]["IdPerfilCompPadre"]);
                            CantExiste = tblPerfilComponentes.Select("IdPerfilComp = " + idpadre.ToString()).Length;
                            CantExisteDatos = tblPerfilComponentes.Select("IdPerfilComp = " + idpadre.ToString()).Length;
                        }
                        CantExiste = 0;
                        idpadre = 0;
                    }
                    IdPerfilComp = IdPerfilComp.Remove(IdPerfilComp.Length - 1);

                    foreach (DataRow drPFComp in tblPerfilComponentesDatos.Select("IdPerfilComp NOT IN (" + IdPerfilComp + ")"))
                    {
                        DataRow row;
                        row = tblPerfilComponentes.NewRow();
                        row["IdPerfilCompPadre"] = Convert.ToInt32(drPFComp["IdPerfilCompPadre"]);
                        row["IdUCComp"] = 0;
                        row["IdPerfilComp"] = Convert.ToInt32(drPFComp["IdPerfilComp"]);
                        row["Nivel"] = Convert.ToInt32(drPFComp["Nivel"]);
                        row["PerfilComp"] = Convert.ToString(drPFComp["PerfilComp"]);
                        row["IdUC"] = 0;
                        row["IdTipoDetalle"] = Convert.ToInt32(drPFComp["IdTipoDetalle"]);
                        row["IdItem"] = 0;
                        row["NroSerie"] = "";
                        row["CodigoSAP"] = Convert.ToString(drPFComp["CodigoSAP"]);
                        row["DescripcionSAP"] = Convert.ToString(drPFComp["DescripcionSAP"]);
                        row["IdEstadoUCComp"] = Convert.ToString(drPFComp["IdEstadoPC"]);
                        row["FlagActivo"] = Convert.ToBoolean(drPFComp["IdFlagActivo"]);
                        row["Nuevo"] = true;
                        tblPerfilComponentes.Rows.Add(row);
                    }
                }
                DataView dtvPerfilComponentes = new DataView(tblPerfilComponentes);
                dtvPerfilComponentes.Sort = "IdPerfilComp asc";
                tblPerfilComponentes = dtvPerfilComponentes.ToTable();
                trvComp.ItemsSource = null;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblPerfilComponentes;
                trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponente(1000, null);

                trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                CboPerfil.SelectedIndexChanged += new RoutedEventHandler(CboPerfil_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void CboPerfil_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CboPerfil.SelectedIndex != -1)
                {
                    dtgActi.ItemsSource = null;
                    trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                    IniciarTablasTemporales();

                    CboCiclo.SelectedIndex = -1;
                    E_Perfil.Idperfil = Convert.ToInt32(CboPerfil.EditValue);

                    DataView dtvciclos = objCiclos.Ciclo_ComboByPerfil(E_Perfil).DefaultView;
                    dtvciclos.RowFilter = "IdTipoCiclo = 2";
                    CboCiclo.ItemsSource = dtvciclos;
                    CboCiclo.DisplayMember = "Ciclo";
                    CboCiclo.ValueMember = "IdCiclo";

                    #region "Planes de Mantenimiento=>,"Cargar Ciclo por Defecto de perfil seleccionado"
                    DataTable tblPerfil = new DataTable();
                    tblPerfil = objPerfil.Perfil_GetItem(E_Perfil);
                    CboCiclo.EditValue = Convert.ToInt32(tblPerfil.Rows[0]["IdCicloDefecto"]);
                    #endregion

                    LlenarTrvComponentes(Convert.ToInt32(CboPerfil.EditValue));

                    DataTable tblPerfilActividadDatos = objPerActi.PerfilComp_Actividad_List(E_Perfil);

                    foreach (DataRow drActividades in tblPerfilActividadDatos.Select("IsActivo = true"))
                    {
                        DataRow row;
                        row = tblPerfilCompActividad.NewRow();
                        row["IsChecked"] = false;
                        row["IdPerfilCompActividad"] = Convert.ToString(drActividades["IdPerfilCompActividad"]);
                        row["Actividad"] = Convert.ToString(drActividades["Actividad"]);
                        row["IdPMComp"] = Convert.ToInt32(drActividades["IdPerfilComp"]);
                        row["IdPerfilComp"] = Convert.ToInt32(drActividades["IdPerfilComp"]);
                        row["Nuevo"] = true;
                        row["IdActividad"] = Convert.ToInt32(drActividades["IdActividad"]);
                        row["Activo"] = Convert.ToInt32(drActividades["FlagActivo"]);
                        row["Uso"] = Convert.ToInt32(drActividades["FlagUso"]);
                        tblPerfilCompActividad.Rows.Add(row);
                    }
                    //for (int i = 0; i < tblPerfilActividadDatos.Rows.Count; i++)
                    //{
                    //    DataRow row;
                    //    row = tblPerfilCompActividad.NewRow();
                    //    row["IsChecked"] = false;
                    //    row["IdPerfilCompActividad"] = Convert.ToString(tblPerfilActividadDatos.Rows[i]["IdPerfilCompActividad"]);
                    //    row["Actividad"] = Convert.ToString(tblPerfilActividadDatos.Rows[i]["Actividad"]);
                    //    row["IdPMComp"] = Convert.ToInt32(tblPerfilActividadDatos.Rows[i]["IdPerfilComp"]);
                    //    row["IdPerfilComp"] = Convert.ToInt32(tblPerfilActividadDatos.Rows[i]["IdPerfilComp"]);
                    //    row["Nuevo"] = true;
                    //    row["IdActividad"] = Convert.ToInt32(tblPerfilActividadDatos.Rows[i]["IdActividad"]);
                    //    row["Activo"] = Convert.ToInt32(tblPerfilActividadDatos.Rows[i]["FlagActivo"]);
                    //    row["Uso"] = Convert.ToInt32(tblPerfilActividadDatos.Rows[i]["FlagUso"]);
                    //    tblPerfilCompActividad.Rows.Add(row);
                    //}
                    tblPerfilCompActividadtmp = tblPerfilCompActividad.Copy();
                    trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void trvComp_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (trvComp.ItemsSource != null)
                {
                    TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                    if (trm != null)
                    {
                        int idPerfilComp = Convert.ToInt32(trm.IdMenu);

                        if (idPerfilComp != 0)
                        {
                            foreach (DataRow drPfComp in tblPerfilComponentes.Select("IdPerfilComp = " + idPerfilComp))
                            {
                                if (Convert.ToInt32(drPfComp["IdUCComp"]) != 0)
                                {
                                    idPerfilComp = Convert.ToInt32(drPfComp["IdUCComp"]);
                                }
                                else
                                {
                                    idPerfilComp = Convert.ToInt32(drPfComp["IdPerfilComp"]);
                                }
                                break;
                            }
                            DataView dtvPerfilCompActividad = new DataView(tblPerfilCompActividad);
                            dtvPerfilCompActividad.RowFilter = "IdPMComp = " + idPerfilComp + " AND Activo = true AND Uso = false AND Nuevo = " + trm.BoolNuevo;
                            dtgActi.ItemsSource = dtvPerfilCompActividad;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void btnAbrirFrecuencia_Click(object sender, RoutedEventArgs e)
        {
            txtFrecEdit.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecEdit_EditValueChanged);
            cboEstFrec.SelectedIndexChanged -= new RoutedEventHandler(cboEstFrec_SelectedIndexChanged);

            cboEstFrec.EditValue = "1";
            txtFrecuencia.EditValue = 0.00;
            CambiarBotonDefecto(false);
            btnAgregarFrec.IsDefault = true;
            btnCancelarFrec.IsCancel = true;
            stkAbrirFrecuencia.Visibility = Visibility.Visible;

            txtFrecEdit.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecEdit_EditValueChanged);
            cboEstFrec.SelectedIndexChanged += new RoutedEventHandler(cboEstFrec_SelectedIndexChanged);
        }

        private void btnAgregarFrec_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtFrecuencia.Text == string.Empty)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_FREC"), 2);
                    txtFrecuencia.Focus();
                    return;
                }
                else if (Convert.ToDouble(txtFrecuencia.Text) == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_FREC"), 2);
                    txtFrecuencia.Focus();
                    return;
                }
                DataRow Fila = tblPMFrecuencias.NewRow();
                Fila["IdPMCompFrecuencia"] = gintCount;
                Fila["IdPM"] = 0;
                Fila["Frecuencia"] = Convert.ToDouble(txtFrecuencia.Text);
                Fila["IdEstadoPMF"] = 1;
                Fila["EstadoF"] = "Activo";
                Fila["FlagActivo"] = true;
                Fila["Nuevo"] = true;

                for (int i = 0; i < tblPMFrecuencias.Rows.Count; i++)
                {
                    tblPMFrecuencias.Rows[i]["IdEstadoPMF"] = 2;
                    tblPMFrecuencias.Rows[i]["EstadoF"] = "Inactivo";
                }
                tblPMFrecuencias.Rows.Add(Fila);
                dtgFrec.ItemsSource = tblPMFrecuencias;
                txtFrecuencia.Text = "";
                CambiarBotonDefecto(true);
                btnAgregarFrec.IsDefault = false;
                btnCancelarFrec.IsCancel = false;
                stkAbrirFrecuencia.Visibility = Visibility.Hidden;
                gintCount++;
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelarFrec_Click(object sender, RoutedEventArgs e)
        {
            CambiarBotonDefecto(true);
            btnAgregarFrec.IsDefault = false;
            btnCancelarFrec.IsCancel = false;
            stkAbrirFrecuencia.Visibility = Visibility.Hidden;
        }

        private void btnEditarFrec_Click(object sender, RoutedEventArgs e)
        {
            if (gbolFlagInactivo)
            {
                return;
            }
            txtFrecEdit.EditValueChanged -= new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecEdit_EditValueChanged);
            cboEstFrec.SelectedIndexChanged -= new RoutedEventHandler(cboEstFrec_SelectedIndexChanged);

            if (dtgFrec.VisibleRowCount < 1) return;
            txtFrecEdit.Focus();
            CambiarBotonDefecto(false);
            btnEditarFrecu.IsDefault = true;
            btnCancelarFrecu.IsCancel = true;
            stkEditFrec.Visibility = Visibility.Visible;

            gintIDFrec = dtgFrec.GetSelectedRowHandles()[0];
            txtFrecEdit.Text = Convert.ToDouble(dtgFrec.GetFocusedRowCellValue("Frecuencia")).ToString("###,###,##0.00");
            cboEstFrec.EditValue = dtgFrec.GetFocusedRowCellValue("IdEstadoPMF").ToString();

            txtFrecEdit.EditValueChanged += new DevExpress.Xpf.Editors.EditValueChangedEventHandler(txtFrecEdit_EditValueChanged);
            cboEstFrec.SelectedIndexChanged += new RoutedEventHandler(cboEstFrec_SelectedIndexChanged);

        }

        private void dtgListPM_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgListPM.VisibleRowCount == 0) { return; }
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            if (dep is TextBlock)
            {
                if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodPM")
                {
                    e.Handled = true;
                    EstadoForm(false, false, true);
                    DetallesPMantenimiendo();
                    tabControl1.SelectedIndex = 1;
                }
            }
        }

        private bool ValidaCampoObligado()
        {
            bool bolRpta = false;
            try
            {
                if (txtDescripcion.Text.Trim() == "")
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_DESC"), 2);
                    txtDescripcion.Focus();
                }
                else if (CboPerfil.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_PERF"), 2);
                    CboPerfil.Focus();
                }
                else if (CboCiclo.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_CICL"), 2);
                    CboCiclo.Focus();
                }
                else if (CboEstado.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_ESTA"), 2);
                    CboEstado.Focus();
                }
                else if (CboTipoOTDefecto.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_TIPO_OT"), 2);
                    CboTipoOTDefecto.Focus();
                }
                else if (spLimiteAmarillo.Value <= 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_AMAR"), 2);
                    spLimiteAmarillo.Focus();
                }
                else if (spLimiteNaranja.Value <= 0)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_NARA"), 2);
                    spLimiteNaranja.Focus();
                }
                else if (spLimiteAmarillo.Value > 100)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_CANT_AMAR"), 2);
                    spLimiteAmarillo.Focus();
                }
                else if (spLimiteNaranja.Value > 100)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "OBLI_CANT_NARA"), 2);
                    spLimiteNaranja.Focus();
                }
                else if (Convert.ToInt32(spLimiteNaranja.EditValue) >= Convert.ToInt32(spLimiteAmarillo.EditValue))
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "LOGI_INDI"), 2);
                    spLimiteAmarillo.Focus();
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
        void CambiarBotonDefecto(bool estado)
        {
            BtnGrabar.IsDefault = estado;
            BtnCancelar.IsCancel = estado;
        }
        private void btnCancelarFrecu_Click(object sender, RoutedEventArgs e)
        {
            CambiarBotonDefecto(true);
            btnEditarFrecu.IsDefault = false;
            btnCancelarFrecu.IsCancel = false;
            stkEditFrec.Visibility = Visibility.Hidden;
        }

        private void btnEditarFrecu_Click(object sender, RoutedEventArgs e)
        {
            dtgFrec.ItemsSource = null;
            tblPMFrecuencias.Rows[gintIDFrec]["Frecuencia"] = Convert.ToDouble(txtFrecEdit.Text);
            tblPMFrecuencias.Rows[gintIDFrec]["IdEstadoPMF"] = Convert.ToInt32(cboEstFrec.EditValue);
            tblPMFrecuencias.Rows[gintIDFrec]["EstadoF"] = "Activo";

            if (Convert.ToInt32(tblPMFrecuencias.Rows[gintIDFrec]["IdEstadoPMF"]) == 1)
            {
                for (int i = 0; i < tblPMFrecuencias.Rows.Count; i++)
                {
                    if (i != gintIDFrec)
                    {
                        tblPMFrecuencias.Rows[i]["IdEstadoPMF"] = 2;
                        tblPMFrecuencias.Rows[i]["EstadoF"] = "Inactivo";
                    }
                }
            }
            else if (Convert.ToInt32(tblPMFrecuencias.Rows[gintIDFrec]["IdEstadoPMF"]) == 2)
            {
                tblPMFrecuencias.Rows[gintIDFrec]["IdEstadoPMF"] = 2;
                tblPMFrecuencias.Rows[gintIDFrec]["EstadoF"] = "Inactivo";
            }
            txtFrecEdit.Text = "";
            cboEstFrec.EditValue = "-1";
            dtgFrec.ItemsSource = tblPMFrecuencias;
            CambiarBotonDefecto(true);
            btnEditarFrecu.IsDefault = false;
            btnCancelarFrecu.IsCancel = false;
            stkEditFrec.Visibility = Visibility.Hidden;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            int IdPerfilCompActividad = Convert.ToInt32(dtgActi.GetFocusedRowCellValue("IdPerfilCompActividad"));
            if (gbolFlagInactivo)
            {
                foreach (DataRow drPfAcTmp in tblPerfilCompActividadtmp.Select("IdPerfilCompActividad = " + IdPerfilCompActividad))
                {
                    bool Estado = Convert.ToBoolean(drPfAcTmp["IsChecked"]);
                    foreach (DataRow drAct in tblPerfilCompActividad.Select("IdPerfilCompActividad = " + IdPerfilCompActividad))
                    {
                        drAct["IsChecked"] = Estado;
                    }
                }
                return;
            }

            foreach (DataRow drPfAcTmp in tblPerfilCompActividadtmp.Select("IdPerfilCompActividad = " + IdPerfilCompActividad + " AND IsChecked = true"))
            {
                bool Estado = Convert.ToBoolean(drPfAcTmp["IsChecked"]);
                foreach (DataRow drAct in tblPerfilCompActividad.Select("IdPerfilCompActividad = " + IdPerfilCompActividad))
                {
                    E_PM.IdPM = Convert.ToInt32(dtgListPM.GetFocusedRowCellValue("IdPM"));
                    E_PM.IdPMComp = Convert.ToInt32(drAct["IdPerfilComp"]);
                    E_PM.IdPMActividad = Convert.ToInt32(drAct["IdActividad"]);
                    DataTable tblPM_GetBeforeChange = objPM.PMComp_Actividad_GetBeforeChange(E_PM);
                    if (Convert.ToInt32(tblPM_GetBeforeChange.Rows[0]["Contador"]) != 0)
                    {
                        drAct["IsChecked"] = true;
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "LOGI_ACTI_ASIG"), 2);
                        return;
                    }
                }
            }
            EstadoForm(false, true, false);
        }

        private void CboEstado_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                E_PM.IdPM = Convert.ToInt32(dtgListPM.GetFocusedRowCellValue("IdPM"));
                DataTable tblPM_GetBeforeChange = objPM.PM_GetBeforeChange(E_PM);
                if (Convert.ToInt32(tblPM_GetBeforeChange.Rows[0]["Contador"]) != 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPlanMantenimiento, "LOGI_PLAN_ASIG"), 2);
                    CboEstado.SelectedIndexChanged -= new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                    CboEstado.EditValue = OldEstado;
                    CboEstado.SelectedIndexChanged += new RoutedEventHandler(CboEstado_SelectedIndexChanged);
                    return;
                }
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void CboCiclo_SelectedIndexChanged(object sender, RoutedEventArgs e)
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

        private void CboTipoOTDefecto_SelectedIndexChanged(object sender, RoutedEventArgs e)
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

        private void spLimiteAmarillo_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
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

        private void spLimiteNaranja_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
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

        private void txtDescripcion_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
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

        private void txtFrecEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
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

        private void cboEstFrec_SelectedIndexChanged(object sender, RoutedEventArgs e)
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
    }
}
