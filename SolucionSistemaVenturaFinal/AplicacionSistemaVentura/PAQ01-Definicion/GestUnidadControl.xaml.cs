using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Editors;
using System.Data;
using Utilitarios;
using Entities;
using Business;
using InterfazMTTO;
using Utilitarios.Enum;

namespace AplicacionSistemaVentura.PAQ01_Definicion
{
    /// <summary>
    /// Interaction logic for GestUnidadControl.xaml
    /// </summary>
    /// 

    public partial class GestUnidadControl : UserControl
    {
        string gstrUsuario = Utilitarios.Utilitarios.gstrUsuario;
        Boolean gbolNuevo = false; Boolean gbolEdicion = false;
        B_UC objUC = new B_UC();
        B_Perfil objPerfil = new B_Perfil();
        B_PerfilComp objPerfilComp = new B_PerfilComp();
        B_UCComp objUCComp = new B_UCComp();
        B_UCComp_Ciclo objUCCompCiclo = new B_UCComp_Ciclo();
        DataTable tblUC, tblPerfil, tblPerfilComponentes, tblPerfilCompCiclo, tblPerfilCompCicloDatos, tblPerfilComponentesDatos, tblUCComponentes, tblItem_Ciclo;
        E_TablaMaestra objTablaMaestra = new E_TablaMaestra();
        E_UC objEUC = new E_UC();
        E_UCComp objE_UCComp = new E_UCComp();
        E_Perfil objE_Perfil = new E_Perfil();
        E_TablaMaestra objTablaMestra = new E_TablaMaestra();
        InterfazMTTO.iSBO_BE.BEUDUC UDUC = new InterfazMTTO.iSBO_BE.BEUDUC();
        InterfazMTTO.iSBO_BE.BEUDUCList tucuclist = new InterfazMTTO.iSBO_BE.BEUDUCList();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        InterfazMTTO.iSBO_BE.BEOITMList BEOITMList = new InterfazMTTO.iSBO_BE.BEOITMList();
        int gintusuario = Utilitarios.Utilitarios.gintIdUsuario;
        int gintCantDocumentos = 0;
        Utilitarios.ErrorHandler ObjError = new Utilitarios.ErrorHandler();
        Utilitarios.Utilitarios objUtil = new Utilitarios.Utilitarios();
        DataView dtv_maestra = new DataView();
        E_PerfilComp objE_PerfilComp = new E_PerfilComp();
        DataTable gtblUCComp = new DataTable();
        DataTable tblPerfilComponentestmp = new DataTable();

        int gintValorTiempoDefecto = 0;
        int gintTiempoDefecto = 0;
        string gstrCicloDefecto = "";

        int gIdEstadoUC = 0;
        Boolean gbolFlagInactivo = false;
        DateTime FechaModificacion;
        string gstrEtiquetaUnidadControl = "GestUnidadControl";

        public class ClsPerfilComp
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        private IList<ClsPerfilComp> ComboPerfilComp(int IdPerfil)
        {
            List<ClsPerfilComp> ListCombo = new List<ClsPerfilComp>();
            objE_PerfilComp.Idperfil = IdPerfil;
            DataTable tblFinal = new DataTable();
            DataRow[] drFinal = objPerfilComp.PerfilComp_ListWithNoParent(objE_PerfilComp).Select("FlagNeumatico <> 1");
            if (drFinal.Length != 0)
            {
                tblFinal = drFinal.CopyToDataTable();
            }

            for (int j = 0; j < tblFinal.Rows.Count; j++)
            {
                ListCombo.Add(new ClsPerfilComp()
                {
                    Id = Convert.ToInt32(tblFinal.Rows[j]["IdPerfilComp"]),
                    Text = tblFinal.Rows[j]["PerfilComp"].ToString()
                });
            }
            return ListCombo;
        }

        public GestUnidadControl()
        {
            InitializeComponent();
            UserControl_Loaded();
        }


        private void CrearTablasPerfilComponentes()
        {
            try
            {
                //Inicializar Tabla Componentes
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

                //Inicializar Tabla Ciclo
                tblPerfilCompCiclo = new DataTable();
                tblPerfilCompCiclo.Columns.Add("IdUCCompCiclo", Type.GetType("System.Int32"));
                tblPerfilCompCiclo.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
                tblPerfilCompCiclo.Columns.Add("IdPerfilCompCiclo", Type.GetType("System.Int32"));
                tblPerfilCompCiclo.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblPerfilCompCiclo.Columns.Add("IdCiclo", Type.GetType("System.Int32"));
                tblPerfilCompCiclo.Columns.Add("Ciclo", Type.GetType("System.String"));
                tblPerfilCompCiclo.Columns.Add("Contador", Type.GetType("System.Double"));
                tblPerfilCompCiclo.Columns.Add("FrecuenciaExtendida", Type.GetType("System.Double"));
                tblPerfilCompCiclo.Columns.Add("FrecuenciaCambio", Type.GetType("System.Double"));
                tblPerfilCompCiclo.Columns.Add("PorcAmarillo", Type.GetType("System.Double"));
                tblPerfilCompCiclo.Columns.Add("PorcNaranja", Type.GetType("System.Double"));
                tblPerfilCompCiclo.Columns.Add("FlagCicloPrincipal", Type.GetType("System.Boolean"));
                tblPerfilCompCiclo.Columns.Add("IdEstadoCiclo", Type.GetType("System.Int32"));
                tblPerfilCompCiclo.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblPerfilCompCiclo.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void CrearTablasUCCOmponentes()
        {
            try
            {
                //Inicializar Tabla UC Componentes
                tblUCComponentes = new DataTable();
                tblUCComponentes.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
                tblUCComponentes.Columns.Add("IdPerfilComp", Type.GetType("System.Int32"));
                tblUCComponentes.Columns.Add("IdUC", Type.GetType("System.Int32"));
                tblUCComponentes.Columns.Add("IdTipoDetalle", Type.GetType("System.Int32"));
                tblUCComponentes.Columns.Add("IdItem", Type.GetType("System.Int32"));
                tblUCComponentes.Columns.Add("NroSerie", Type.GetType("System.String"));
                tblUCComponentes.Columns.Add("CodigoSAP", Type.GetType("System.String"));
                tblUCComponentes.Columns.Add("DescripcionSAP", Type.GetType("System.String"));
                tblUCComponentes.Columns.Add("IdEstadoUCComp", Type.GetType("System.Int32"));
                tblUCComponentes.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblUCComponentes.Columns.Add("Nuevo", Type.GetType("System.Boolean"));

                //Inicializar Tabla UC Componentes Ciclo
                tblItem_Ciclo = new DataTable();
                tblItem_Ciclo.Columns.Add("IdItemCiclo", Type.GetType("System.Int32"));
                tblItem_Ciclo.Columns.Add("IdItem", Type.GetType("System.Int32"));
                tblItem_Ciclo.Columns.Add("IdPerfilCompCiclo", Type.GetType("System.Int32"));
                tblItem_Ciclo.Columns.Add("FrecuenciaCambio", Type.GetType("System.Double"));
                tblItem_Ciclo.Columns.Add("Contador", Type.GetType("System.Double"));
                tblItem_Ciclo.Columns.Add("FrecuenciaExtendida", Type.GetType("System.Double"));
                tblItem_Ciclo.Columns.Add("PorcAmarillo", Type.GetType("System.Double"));
                tblItem_Ciclo.Columns.Add("PorcNaranja", Type.GetType("System.Double"));
                tblItem_Ciclo.Columns.Add("FlagCicloPrincipal", Type.GetType("System.Boolean"));
                tblItem_Ciclo.Columns.Add("IdEstadoCiclo", Type.GetType("System.Int32"));
                tblItem_Ciclo.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblItem_Ciclo.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void ListarUnidadControl()
        {
            try
            {
                //Listar Grilla Principal
                tblUC = new DataTable();
                tblUC.Columns.Add("IsChecked", Type.GetType("System.Boolean"));
                tblUC.Columns.Add("IdUC", Type.GetType("System.Int32"));
                tblUC.Columns.Add("CodInterno", Type.GetType("System.String"));
                tblUC.Columns.Add("CodigoUC", Type.GetType("System.String"));
                tblUC.Columns.Add("IdPerfil", Type.GetType("System.Int32"));
                tblUC.Columns.Add("IdPerfilNeumatico", Type.GetType("System.Int32"));
                tblUC.Columns.Add("PlacaSerie", Type.GetType("System.String"));
                tblUC.Columns.Add("Marca", Type.GetType("System.String"));
                tblUC.Columns.Add("Modelo", Type.GetType("System.String"));
                tblUC.Columns.Add("Anio", Type.GetType("System.String"));
                tblUC.Columns.Add("Familia", Type.GetType("System.String"));
                tblUC.Columns.Add("SubFamilia", Type.GetType("System.String"));
                tblUC.Columns.Add("Perfil", Type.GetType("System.String"));
                tblUC.Columns.Add("IdEstadoUC", Type.GetType("System.Int32"));
                tblUC.Columns.Add("Estado", Type.GetType("System.String"));


                DataTable tblUCDatos = new DataTable();
                string CodigosUC = "";
                tblUCDatos = objUC.UC_List(objEUC);

                if (tblUCDatos.Rows.Count != 0)
                {
                    for (int i = 0; i < tblUCDatos.Rows.Count; i++)
                    {
                        CodigosUC = CodigosUC + tblUCDatos.Rows[i]["CodUC"].ToString() + '|';
                    }

                    tucuclist = InterfazMTTO.iSBO_BL.UnidadControl_BL.ListaUnidadControl(CodigosUC, ref RPTA);
                    if (RPTA.ResultadoRetorno != 0)
                    {
                        GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                    }

                    for (int i = 0; i < tblUCDatos.Rows.Count; i++)
                    {
                        DataRow row;
                        row = tblUC.NewRow();
                        row["IsChecked"] = false;
                        row["IdUC"] = tblUCDatos.Rows[i]["IdUC"].ToString();
                        row["CodInterno"] = tblUCDatos.Rows[i]["CodInterno"].ToString();
                        row["CodigoUC"] = tblUCDatos.Rows[i]["CodUC"].ToString();
                        row["IdPerfil"] = tblUCDatos.Rows[i]["IdPerfil"].ToString();
                        row["IdPerfilNeumatico"] = tblUCDatos.Rows[i]["IdPerfilNeumatico"].ToString();
                        row["PlacaSerie"] = tblUCDatos.Rows[i]["PlacaSerie"].ToString();
                        row["Perfil"] = tblUCDatos.Rows[i]["Perfil"].ToString();
                        row["IdEstadoUC"] = tblUCDatos.Rows[i]["IdEstadoUC"].ToString();
                        row["Estado"] = tblUCDatos.Rows[i]["Estado"].ToString();
                        foreach (var drTucu in tucuclist.Where(tc => tc.CodigoUnidadControl == Convert.ToString(tblUCDatos.Rows[i]["CodUC"])))
                        {
                            row["Marca"] = drTucu.Marca;
                            row["Modelo"] = drTucu.Modelo;
                            row["Anio"] = drTucu.Anho;
                            row["Familia"] = drTucu.Familia;
                            row["SubFamilia"] = drTucu.SubFamilia;
                        }
                        tblUC.Rows.Add(row);
                    }
                }
                dtgUnidContr.ItemsSource = tblUC;
                //Listar Combo Perfil
                tblPerfil = objPerfil.Perfil_Combo();
                cboPerfil.ItemsSource = tblPerfil;
                cboPerfil.DisplayMember = "Perfil";
                cboPerfil.ValueMember = "IdPerfil";

                cboEstadoUC.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=19", dtv_maestra);
                cboEstadoUC.DisplayMember = "Descripcion";
                cboEstadoUC.ValueMember = "Valor";

                cboEstaNuev.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=19", dtv_maestra);
                cboEstaNuev.DisplayMember = "Descripcion";
                cboEstaNuev.ValueMember = "Valor";

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void UserControl_Loaded()
        {
            try
            {


                fechaInicio.SelectedDate = DateTime.Now.Date;

                labelContadorAutomatico.Visibility = Visibility.Hidden;
                checkContadorAutomatico.Visibility = Visibility.Hidden;
                fechaInicio.Visibility = Visibility.Hidden;
                labelFechaInicio.Visibility = Visibility.Hidden;
                fechaInicio.Visibility = Visibility.Hidden;



                GlobalClass.ControlSubMenu(this.GetType().Name, gridTabLista);
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged -= new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged -= new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
                trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                cboEstadoUC.SelectedIndexChanged -= new RoutedEventHandler(cboEstadoUC_SelectedIndexChanged);
                objTablaMestra.IdTabla = 0;
                dtv_maestra = B_TablaMaestra.TablaMaestra_Combo(objTablaMestra).DefaultView;
                rdbActivo.IsChecked = true;
                cboEstadoUC.EditValue = "1";
                objEUC.IdEstadoUC = 1;
                //ListarUnidadControl();
                CrearTablasPerfilComponentes();
                CrearTablasUCCOmponentes();

                //Listar Combo Estado
                cboEstado.ItemsSource = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1", dtv_maestra);
                cboEstado.DisplayMember = "Descripcion";
                cboEstado.ValueMember = "IdColumna";

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

                gtblUCComp.Columns.Clear();
                gtblUCComp.Columns.Add("IdUCComp", Type.GetType("System.Int32"));
                gtblUCComp.Columns.Add("IdPerfilCompOld", Type.GetType("System.Int32"));
                gtblUCComp.Columns.Add("PerfilCompOld", Type.GetType("System.String"));
                gtblUCComp.Columns.Add("IdPerfilCompNew", Type.GetType("System.Int32"));
                gtblUCComp.Columns.Add("DetailSource", Type.GetType("System.Object"));
                gtblUCComp.Columns.Add("Nuevo", Type.GetType("System.Int32"));

                DataTable tblGerarquiaProducto = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=52", dtv_maestra);
                labelFamilia.Content = tblGerarquiaProducto.Rows[0]["Descripcion"];
                labelSubFamilia.Content = tblGerarquiaProducto.Rows[1]["Descripcion"];
                labelLineaN.Content = tblGerarquiaProducto.Rows[2]["Descripcion"];

                gintTiempoDefecto = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 1000", dtv_maestra).Rows[7]["Valor"]);
                gintValorTiempoDefecto = Convert.ToInt32(Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 58", dtv_maestra).Select("IdColumna = " + gintTiempoDefecto)[0][2]);
                gstrCicloDefecto = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla = 58", dtv_maestra).Select("IdColumna = " + gintTiempoDefecto)[0][3].ToString();

                cboEstadoUC.SelectedIndexChanged += new RoutedEventHandler(cboEstadoUC_SelectedIndexChanged);
                trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged += new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged += new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private bool PrecondicionCambioPerfil()
        {
            bool rpt = false;
            if (dtgUnidContr.VisibleRowCount == 0) { rpt = true; GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_SELE_UC"), 2); }
            else if (1 != Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdEstadoUC"))) { rpt = true; GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_ESTA_UC"), 2); }
            else if (objPerfil.Perfil_Combo().Rows.Count < 2) { rpt = true; GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_PERF_CANT"), 2); }
            else
            {
                string IdUC = dtgUnidContr.GetFocusedRowCellValue("IdUC").ToString();
                DataTable tblCant = objUC.UC_GetBeforeChange(IdUC);
                if (Convert.ToInt32(tblCant.Rows[0]["Contador"]) != 0)
                {
                    rpt = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_DOCU_UC"), 2);
                }

            }
            return rpt;
        }
        private void BtnCambioPerfil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgUnidContr.VisibleRowCount == 0) { return; }
                if (PrecondicionCambioPerfil()) return;

                string CodUC = dtgUnidContr.GetFocusedRowCellValue("CodigoUC").ToString();
                int IdPerfilSelected = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdPerfil"));
                int IdPerfilNeumaticoSelected = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdPerfilNeumatico"));
                tucuclist = InterfazMTTO.iSBO_BL.UnidadControl_BL.ListaUnidadControl(CodUC, ref RPTA);

                if (RPTA.CodigoErrorUsuario == "000")
                {
                    lblTipoUC.Content = tucuclist[0].DescripcionTipoUnidadControl;
                }
                else
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }

                lblUC.Content = CodUC;


                objE_Perfil.Idperfil = IdPerfilSelected;
                DataTable tblPerfilSelected = objPerfil.Perfil_GetItem(objE_Perfil);

                lblPerfOrig.Content = tblPerfilSelected.Rows[0]["Perfil"];
                string IdTipoUnidad = tblPerfilSelected.Rows[0]["IdTipoUnidad"].ToString();


                objE_UCComp.IdUC = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdUC"));
                objE_UCComp.IdPerfil = IdPerfilSelected;
                DataTable tblOriginal = objUCComp.UCComp_List(objE_UCComp);

                gtblUCComp.Rows.Clear();

                for (int i = 0; i < tblOriginal.Rows.Count; i++)
                {
                    DataRow fila = gtblUCComp.NewRow();
                    fila["IdUCComp"] = Convert.ToInt32(tblOriginal.Rows[i]["IdUCComp"]);
                    fila["IdPerfilCompOld"] = Convert.ToInt32(tblOriginal.Rows[i]["IdPerfilComp"]);
                    fila["PerfilCompOld"] = tblOriginal.Rows[i]["PerfilComp"].ToString();
                    fila["IdPerfilCompNew"] = 0;
                    fila["DetailSource"] = null;
                    fila["Nuevo"] = 0;
                    gtblUCComp.Rows.Add(fila);
                }

                dtgUCComp.ItemsSource = gtblUCComp;

                cboPerfFina.SelectedIndexChanged -= new RoutedEventHandler(cboPerfFina_SelectedIndexChanged);
                DataView dtvPerfilCombo = objPerfil.Perfil_Combo().DefaultView;
                dtvPerfilCombo.RowFilter = "IdTipoUnidad = '" + IdTipoUnidad + "' and " +
                    "IdPerfil <> " + IdPerfilSelected.ToString() + " and IdPerfilNeumatico = " + IdPerfilNeumaticoSelected.ToString();

                string IdPerfilesDisable = "0,";
                for (int i = 0; i < dtvPerfilCombo.Count; i++)
                {
                    int IdPerfil = Convert.ToInt32(dtvPerfilCombo[i]["IdPerfil"]);
                    int CantComp = ComboPerfilComp(IdPerfil).Count;
                    if (CantComp < dtgUCComp.VisibleRowCount)
                        IdPerfilesDisable += IdPerfil.ToString() + ",";
                }
                //No mostrar Si la cantidad de componentes Nuevos son menor a Componente Antiguo.
                dtvPerfilCombo.RowFilter = "IdTipoUnidad = '" + IdTipoUnidad + "' and " +
                    "IdPerfil <> " + IdPerfilSelected.ToString() + "and IdPerfilNeumatico = " + IdPerfilNeumaticoSelected.ToString() +
                        "and IdPerfil NOT IN (" + IdPerfilesDisable + ")";

                cboPerfFina.ItemsSource = dtvPerfilCombo;
                cboPerfFina.DisplayMember = "Perfil";
                cboPerfFina.ValueMember = "IdPerfil";
                cboPerfFina.SelectedIndex = -1;
                cboPerfFina.SelectedIndexChanged += new RoutedEventHandler(cboPerfFina_SelectedIndexChanged);
                if (dtvPerfilCombo.Count == 0)
                {
                    LimparDatosCambioPerfil();
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_COND_PF"), 2);
                    return;
                }
                tabItem6.IsEnabled = true;
                tabControl1.SelectedIndex = 3;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboPerfFina_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                objE_UCComp.IdUC = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdUC"));
                objE_UCComp.IdPerfil = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdPerfil"));

                int IdPerfilNuevo = Convert.ToInt32(cboPerfFina.EditValue);
                IList<ClsPerfilComp> ComboComp = ComboPerfilComp(IdPerfilNuevo);

                for (int i = 0; i < gtblUCComp.Rows.Count; i++)
                {
                    gtblUCComp.Rows[i]["DetailSource"] = ComboComp;
                }
                dtgUCComp.ItemsSource = gtblUCComp;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void BtnGrabarCambioPerfil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboPerfFina.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_SELE_PF"), 2);
                    cboPerfFina.Focus();
                    return;
                }

                foreach (DataRow dr in gtblUCComp.Select("IdPerfilCompNew = 0"))
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_ESTR_PF"), 2);
                    return;
                }

                int fila = ValidarIdCompRepetido();
                if (fila != -1)
                {
                    int can = 0; string PerfilCompNew = string.Empty;
                    int IdPerfilCompNew = Convert.ToInt32(dtgUCComp.GetCellValue(fila, "IdPerfilCompNew"));
                    List<ClsPerfilComp> ListCombo = (List<ClsPerfilComp>)gtblUCComp.Rows[0]["DetailSource"];
                    for (int i = 0; i < gtblUCComp.Rows.Count; i++)
                        if (IdPerfilCompNew == Convert.ToInt32(gtblUCComp.Rows[i]["IdPerfilCompNew"]))
                            can += 1;
                    PerfilCompNew = ListCombo.Find(x => x.Id == IdPerfilCompNew).Text;

                    GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_ESTR_REPI")
                                    , PerfilCompNew, can), 2);
                    return;
                }
                objE_UCComp.IdUC = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdUC"));
                objE_UCComp.IdPerfil = Convert.ToInt32(cboPerfFina.EditValue);
                objE_UCComp.IdUsuarioCreacion = gintusuario;
                objE_UCComp.FechaModificacion = FechaModificacion;

                gtblUCComp.Columns.Remove("PerfilCompOld");
                gtblUCComp.Columns.Remove("DetailSource");

                int rpta = objUCComp.UCComp_UpdateCascade(objE_UCComp, gtblUCComp);
                if (rpta == 1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "GRAB_CAMB"), 1);
                    LimparDatosCambioPerfil();
                    tabItem6.IsEnabled = false;
                    tabControl1.SelectedIndex = 0;
                    ListarUnidadControl();
                }
                else if (rpta == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_MODI"), 2);
                    return;
                }
                else if (rpta == 1205)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "GRAB_CONC"), 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            finally
            {
                gtblUCComp.Columns.Add("PerfilCompOld", Type.GetType("System.String"));
                gtblUCComp.Columns.Add("DetailSource", Type.GetType("System.Object"));
            }
        }
        private int ValidarIdCompRepetido()
        {
            int id = -1;
            int IdPerfilCompOld = 0;
            for (int i = dtgUCComp.VisibleRowCount - 1; i >= 0; i--)
            {
                IdPerfilCompOld = Convert.ToInt32(dtgUCComp.GetCellValue(i, "IdPerfilCompNew"));
                for (int j = i - 1; j >= 0; j--)
                {
                    if (IdPerfilCompOld == Convert.ToInt32(dtgUCComp.GetCellValue(j, "IdPerfilCompNew")))
                    {
                        id = i;
                    }
                }
            }
            return id;
        }
        private void btnCancelarCambioPerfil_Click(object sender, RoutedEventArgs e)
        {
            LimparDatosCambioPerfil();
            tabItem6.IsEnabled = false;
            tabControl1.SelectedIndex = 0;
        }
        private void LimparDatosCambioPerfil()
        {
            cboPerfFina.SelectedIndexChanged -= new RoutedEventHandler(cboPerfFina_SelectedIndexChanged);
            lblPerfOrig.Content = string.Empty;
            lblUC.Content = string.Empty;
            lblTipoUC.Content = string.Empty;
            cboPerfFina.SelectedIndex = -1;
            dtgUCComp.ItemsSource = null;
            gtblUCComp.Rows.Clear();
            cboPerfFina.SelectedIndexChanged += new RoutedEventHandler(cboPerfFina_SelectedIndexChanged);
        }


        private void BtnComponenetes_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnCancelar1_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void BtnACtualizarPerfil_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(true, false, true);
                LimpiarDatoscontroles();
                cboEstadoUC.EditValue = "2";
                cboEstadoUC.IsEnabled = false;
                CrearTablasPerfilComponentes();
                CrearTablasUCCOmponentes();
                tabControl1.SelectedIndex = 1;
                tcUC.SelectedIndex = 0;
                tbDatosCompUC.IsEnabled = false;
                cboPerfil.IsEnabled = true;
                chkTodaEstr.IsEnabled = false;
                tbIDatosUC.IsEnabled = true;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnMatrUnid_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void ButtonInfo_Click_2(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void btnModiUnid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgUnidContr.VisibleRowCount == 0) { return; }
                gIdEstadoUC = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdEstadoUC"));

                cboPerfil.SelectedIndexChanged += new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged += new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);

                EstadoForm(false, true, true);
                cboPerfil.EditValue = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdPerfil").ToString());
                cboUnidCont.EditValue = dtgUnidContr.GetFocusedRowCellValue("CodigoUC").ToString();
                cboEstadoUC.EditValue = dtgUnidContr.GetFocusedRowCellValue("IdEstadoUC").ToString();
                cboPerfil.IsEnabled = false;
                cboUnidCont.IsEnabled = false;
                tbIDatosUC.IsEnabled = true;
                chkTodaEstr.IsEnabled = true;
                LlenarDatosPerfilModificar();
                tabControl1.SelectedIndex = 1;
                cboPerfil.SelectedIndexChanged -= new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged -= new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCambEstado_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgUnidContr.VisibleRowCount == 0) { return; }
                DataTable tblUCEstado = new DataTable();
                tblUCEstado.Columns.Add("IdUC", Type.GetType("System.Int32"));
                tblUCEstado.Columns.Add("CodInterno", Type.GetType("System.String"));
                tblUCEstado.Columns.Add("CodigoUC", Type.GetType("System.String"));
                tblUCEstado.Columns.Add("PlacaSerie", Type.GetType("System.String"));
                tblUCEstado.Columns.Add("IdEstadoUC", Type.GetType("System.Int32"));
                tblUCEstado.Columns.Add("Estado", Type.GetType("System.String"));
                string estado = "";
                bool EstadoDif = false;
                foreach (DataRow drUCList in tblUC.Select("IsChecked = true"))
                {
                    estado = drUCList["Estado"].ToString();
                    DataRow row;
                    row = tblUCEstado.NewRow();
                    row["IdUC"] = Convert.ToInt32(drUCList["IdUC"]);
                    row["CodInterno"] = Convert.ToString(drUCList["CodInterno"]);
                    row["CodigoUC"] = Convert.ToString(drUCList["CodigoUC"]);
                    row["PlacaSerie"] = Convert.ToString(drUCList["PlacaSerie"]);
                    row["IdEstadoUC"] = Convert.ToString(drUCList["IdEstadoUC"]);
                    row["Estado"] = Convert.ToString(drUCList["Estado"]);
                    tblUCEstado.Rows.Add(row);
                }
                foreach (DataRow drUCEst in tblUCEstado.Select("Estado <> '" + estado + "'"))
                {
                    EstadoDif = true;
                    break;
                }

                if (EstadoDif)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_ESTA_UC"), 2);
                    return;
                }
                else if (tblUCEstado.Rows.Count == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_CANT_UC"), 2);
                    return;
                }
                FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                tabItem4.IsEnabled = true;
                dtgUCSele.ItemsSource = tblUCEstado;
                tabControl1.SelectedIndex = 2;
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

                if ((gbolNuevo == false) && (gbolEdicion == false))
                {
                    tbIDatosUC.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "TAB1_CONS");
                    btnAcepComp.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "BTNG_CONS");
                }
                else if ((gbolNuevo == true) && (gbolEdicion == false))
                {
                    tbIDatosUC.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "TAB1_NUEV");
                    btnAcepComp.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "BTNG_NUEV");
                    lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: -- Host: --", gstrUsuario);
                    lblAuditoria_modificacion.Text = String.Format("Usuario: -- Fecha: -- Host: --");
                }
                else if ((gbolNuevo == false) && (gbolEdicion == true))
                {
                    tbIDatosUC.Header = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "TAB1_EDIT");
                    btnAcepComp.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "BTNG_EDIT");
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnGrabarDatos_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void btnCancelar_Click_1(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void Btncomponentes_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void cboPerfil_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                cboUnidCont.SelectedIndexChanged -= new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
                lblPlacaSerie.Content = "";
                lblMarca.Content = "";
                lblFami.Content = "";
                lblProp.Content = "";
                lblTipoUni.Content = "";
                lblMode.Content = "";
                lblSubFam.Content = "";
                lblConfi.Content = "";
                lblAnio.Content = "";
                lblLinea.Content = "";
                lblTipoUni.Content = "";
                lblTipoProp.Content = "";
                DataTable tblUCTodos = new DataTable();
                objEUC.IdEstadoUC = 0;
                tblUCTodos = objUC.UC_List(objEUC);

                int IdPerfil = Convert.ToInt32(cboPerfil.EditValue);
                string IdTipoUnidad = "";

                #region "Planes de Mantenimiento=>,"Cargar Ciclo por Defecto de perfil seleccionado"
                B_Ciclo objCiclos = new B_Ciclo();
                E_Perfil E_Perfil = new E_Perfil();
                E_Perfil.Idperfil = IdPerfil;
                DataView dtvciclos = objCiclos.Ciclo_ComboByPerfil(E_Perfil).DefaultView;
                dtvciclos.RowFilter = "IdTipoCiclo = 2";
                CboCiclo.ItemsSource = dtvciclos;
                CboCiclo.DisplayMember = "Ciclo";
                CboCiclo.ValueMember = "IdCiclo";

                DataTable tblPerfil = new DataTable();
                tblPerfil = objPerfil.Perfil_GetItem(E_Perfil);
                CboCiclo.EditValue = Convert.ToInt32(tblPerfil.Rows[0]["IdCicloDefecto"]);

                int idCicloDefec= Convert.ToInt32(tblPerfil.Rows[0]["IdCicloDefecto"]);



                if(idCicloDefec==(int)CicloEnum.Dias)
                {

                    labelContadorAutomatico.Visibility = Visibility.Visible;
                    checkContadorAutomatico.Visibility = Visibility.Visible;
                    fechaInicio.Visibility = Visibility.Visible;
                    labelFechaInicio.Visibility = Visibility.Visible;
                    fechaInicio.Visibility = Visibility.Visible;


                    checkContadorAutomatico.IsEnabled = true;
                    checkContadorAutomatico.IsChecked = true;


                }
                else
                {
                    labelContadorAutomatico.Visibility = Visibility.Hidden;
                    checkContadorAutomatico.Visibility = Visibility.Hidden;

                    labelFechaInicio.Visibility = Visibility.Hidden;
                    fechaInicio.Visibility = Visibility.Hidden;

                    checkContadorAutomatico.IsChecked = false;
                    
                 
                }
                #endregion

                foreach (DataRow drPerfil in tblPerfil.Select("IdPerfil = " + IdPerfil))
                {
                    IdTipoUnidad = drPerfil["IdTipoUnidad"].ToString();
                }

                RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
                tucuclist = InterfazMTTO.iSBO_BL.UnidadControl_BL.ListaUnidadControlxTipo(IdTipoUnidad, ref RPTA);

                if (RPTA.CodigoErrorUsuario != "000")
                {
                    if (RPTA.CodigoErrorUsuario == "993")
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_EXIS_UC"), 2);
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                    }
                }

                DataTable tblUC_Combo = new DataTable();
                tblUC_Combo.Columns.Add("PlacaSerieUnidadControl");
                tblUC_Combo.Columns.Add("CodigoUnidadControl");
                DataRow row;
                bool existe = false;

                if ((gbolNuevo == false && gbolEdicion == true) || (gbolNuevo == false && gbolEdicion == false))
                {
                    for (int j = 0; j < tucuclist.Count; j++)
                    {
                        row = tblUC_Combo.NewRow();
                        row["PlacaSerieUnidadControl"] = tucuclist[j].PlacaSerieUnidadControl;
                        row["CodigoUnidadControl"] = tucuclist[j].CodigoUnidadControl;
                        tblUC_Combo.Rows.Add(row);
                    }
                }
                else if (gbolNuevo == true && gbolEdicion == false)
                {
                    for (int j = 0; j < tucuclist.Count; j++)
                    {
                        for (int ex = 0; ex < tblUCTodos.Rows.Count; ex++)
                        {
                            if (tucuclist[j].CodigoUnidadControl == tblUCTodos.Rows[ex]["CodUC"].ToString())
                            {
                                existe = true;
                                break;
                            }
                        }
                        if (!existe)
                        {
                            row = tblUC_Combo.NewRow();
                            row["PlacaSerieUnidadControl"] = tucuclist[j].PlacaSerieUnidadControl;
                            row["CodigoUnidadControl"] = tucuclist[j].CodigoUnidadControl;
                            tblUC_Combo.Rows.Add(row);

                        }
                        existe = false;
                    }
                }
                cboUnidCont.ItemsSource = tblUC_Combo;
                cboUnidCont.DisplayMember = "PlacaSerieUnidadControl";
                cboUnidCont.ValueMember = "CodigoUnidadControl";
                cboUnidCont.IsEnabled = true;
                cboUnidCont.EditValue = -1;
                if (cboUnidCont.EditValue.ToString() == "-1") { tbDatosCompUC.IsEnabled = false; }
                else { tbDatosCompUC.IsEnabled = true; }
                cboUnidCont.SelectedIndexChanged += new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
            }
            catch (Exception ex)
            {

                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LimpiarDatoscontroles()
        {
            try
            {
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged -= new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged -= new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
                trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                chkTodaEstr.Checked -= new RoutedEventHandler(chkTodaEstr_Checked);
                chkTodaEstr.Unchecked -= new RoutedEventHandler(chkTodaEstr_Unchecked);
                dtgCiclo.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(dtgCiclo_SelectedItemChanged);
                txtCodiSAP.EditValueChanged -= new EditValueChangedEventHandler(txtCodiSAP_EditValueChanged);
                txtNserie.EditValueChanged -= new EditValueChangedEventHandler(txtNserie_EditValueChanged);
                txtobservacion.EditValueChanged -= new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);

                rbnComp.IsChecked = false;
                rbntitu.IsChecked = false;
                ChkComponenteExistente.IsChecked = false;
                chkTodaEstr.IsChecked = false;

                cboPerfil.ItemsSource = null;
                cboUnidCont.ItemsSource = null;

                ListarUnidadControl();
                cboPerfil.EditValue = -1;
                lblPlacaSerie.Content = "";
                lblMarca.Content = "";
                lblFami.Content = "";
                lblProp.Content = "";
                lblTipoUni.Content = "";
                lblMode.Content = "";
                lblSubFam.Content = "";
                lblConfi.Content = "";
                lblAnio.Content = "";
                lblLinea.Content = "";
                lblTipoUni.Content = "";
                lblTipoProp.Content = "";
                lblContadorAcumulado.Content = String.Format("{0:###,###,##0.00}", 0.00);
                groupBox2.Header = "Información del componente";

                txtobservacion.Text = "";
                lblUnidCont.Content = "";
                lblPerfUCComp.Content = "";
                dtgCiclo.ItemsSource = null;
                txtCodiSAP.Text = "";
                txtNserie.Text = "";
                lblDesc.Content = "";
                tcUC.SelectedIndex = 0;
                tabControl1.SelectedIndex = 0;
                gintCantDocumentos = 0;
                trvComp.ItemsSource = null;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                gbolFlagInactivo = false;
                tabItem4.IsEnabled = false;
                tabItem6.IsEnabled = false;
                stkPanelComponenteSAP.Visibility = Visibility.Hidden;
                CrearTablasPerfilComponentes();
                CrearTablasUCCOmponentes();
                tbDatosCompUC.IsEnabled = true;
                txtobservacion.IsReadOnly = false;
                groupBox2.IsEnabled = true;
                dtgCiclo.IsEnabled = true;
                DesactivarEstadoInactivo(false);

                txtobservacion.EditValueChanged += new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);
                txtNserie.EditValueChanged += new EditValueChangedEventHandler(txtNserie_EditValueChanged);
                txtCodiSAP.EditValueChanged += new EditValueChangedEventHandler(txtCodiSAP_EditValueChanged);
                dtgCiclo.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(dtgCiclo_SelectedItemChanged);
                chkTodaEstr.Checked += new RoutedEventHandler(chkTodaEstr_Checked);
                chkTodaEstr.Unchecked += new RoutedEventHandler(chkTodaEstr_Unchecked);
                trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged += new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged += new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LlenarDatosPerfilNuevo()
        {
            try
            {
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged -= new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged -= new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
                trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);

                CrearTablasUCCOmponentes();
                CrearTablasPerfilComponentes();
                //Listar Componentes
                tblPerfilComponentes.Rows.Clear();
                tblPerfilComponentesDatos = new DataTable();
                objE_PerfilComp.Idperfil = Convert.ToInt32(cboPerfil.EditValue);
                objE_PerfilComp.Idestadopc = 0;
                DataView PerfilCompDatos = objPerfilComp.PerfilComp_List(objE_PerfilComp).DefaultView;
                PerfilCompDatos.RowFilter = "FlagNeumatico = 0";
                tblPerfilComponentesDatos = PerfilCompDatos.ToTable();


                DataRow row1;
                row1 = tblPerfilComponentes.NewRow();
                row1["IdPerfilCompPadre"] = 1000;
                row1["IdPerfilComp"] = 0;
                row1["Nivel"] = 1;
                row1["PerfilComp"] = lblUnidCont.Content;
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
                    row["IdTipoDetalle"] = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdTipoDetalle"]);
                    row["IdItem"] = 0;
                    row["NroSerie"] = "";
                    row["CodigoSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["CodigoSAP"]);
                    row["DescripcionSAP"] = Convert.ToString(tblPerfilComponentesDatos.Rows[i]["DescripcionSAP"]);
                    row["IdEstadoUCComp"] = Convert.ToBoolean(tblPerfilComponentesDatos.Rows[i]["IdEstadoPC"]);
                    row["FlagActivo"] = true;
                    row["Nuevo"] = true;
                    tblPerfilComponentes.Rows.Add(row);
                }
                tblPerfilComponentestmp = tblPerfilComponentes.Copy();
                trvComp.ItemsSource = null;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblPerfilComponentes;
                trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(1000, null);

                //Listar Ciclos
                E_UCComp_Ciclo objE_UCComp_Ciclo = new E_UCComp_Ciclo();
                objE_UCComp_Ciclo.IdPerfil = Convert.ToInt32(cboPerfil.EditValue);
                tblPerfilCompCicloDatos = new DataTable();
                tblPerfilCompCicloDatos = objUCCompCiclo.PerfilComp_Ciclo_List(objE_UCComp_Ciclo);
                for (int i = 0; i < tblPerfilCompCicloDatos.Rows.Count; i++)
                {
                    DataRow row;
                    row = tblPerfilCompCiclo.NewRow();

                    row["IdUCCompCiclo"] = 0;
                    row["IdUCComp"] = i + 1;
                    row["IdPerfilCompCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilCompCiclo"]);
                    row["IdPerfilComp"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilComp"]);
                    row["IdCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]);
                    row["Ciclo"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? gstrCicloDefecto : tblPerfilCompCicloDatos.Rows[i]["Ciclo"].ToString();
                    row["Contador"] = 0;
                    row["FrecuenciaExtendida"] = 0;
                    row["PorcAmarillo"] = 20.0;
                    row["PorcNaranja"] = 10.0;
                    row["FrecuenciaCambio"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaCambio"]) / gintValorTiempoDefecto : Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaCambio"]);
                    row["FlagCicloPrincipal"] = false;
                    row["IdEstadoCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdEstadoPCC"]);
                    row["FlagActivo"] = true;
                    row["Nuevo"] = true;
                    tblPerfilCompCiclo.Rows.Add(row);
                }
                lblUnidCont.Content = cboUnidCont.Text;
                lblPerfUCComp.Content = cboPerfil.Text;
                CrearTablasUCCOmponentes();

                trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged += new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged += new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LlenarDatosPerfilModificarChecked()
        {
            try
            {
                LimpiarDatosPerfilMod();
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged -= new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged -= new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
                trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);

                CrearTablasPerfilComponentes();
                objE_UCComp.IdPerfil = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdPerfil"));
                objE_UCComp.IdUC = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdUC"));
                tblUCComponentes = objUCComp.UCComp_List(objE_UCComp);
                tblUCComponentes.DefaultView.Sort = "IdPerfilComp asc";
                tblUCComponentes = tblUCComponentes.DefaultView.ToTable(true);

                objE_PerfilComp.Idperfil = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdPerfil"));
                objE_PerfilComp.Idestadopc = 0;
                //--------------------------------------------------------------
                //Listar Componentes
                tblPerfilComponentes.Rows.Clear();
                tblPerfilComponentesDatos = new DataTable();
                tblPerfilComponentesDatos = objPerfilComp.PerfilComp_List(objE_PerfilComp);
                tblPerfilComponentesDatos.DefaultView.Sort = "IdPerfilComp asc";
                tblPerfilComponentesDatos.DefaultView.RowFilter = "FlagNeumatico = 0";
                tblPerfilComponentesDatos = tblPerfilComponentesDatos.DefaultView.ToTable(true);

                DataRow row1;
                row1 = tblPerfilComponentes.NewRow();
                row1["IdPerfilCompPadre"] = 1000;
                row1["IdPerfilComp"] = 0;
                row1["Nivel"] = 1;
                row1["PerfilComp"] = lblUnidCont.Content;
                row1["NroSerie"] = "";
                row1["CodigoSAP"] = "";
                row1["DescripcionSAP"] = "";
                row1["Nuevo"] = true;
                row1["IdEstadoUCComp"] = 1;
                row1["IdTipoDetalle"] = 1;
                tblPerfilComponentes.Rows.Add(row1);

                int ucindex = 0;
                int IdPerfilCompUC = 0;
                for (int i = 0; i < tblPerfilComponentesDatos.Rows.Count; i++)
                {
                    int IdPerfilComp = Convert.ToInt32(tblPerfilComponentesDatos.Rows[i]["IdPerfilComp"]);
                    if (ucindex < tblUCComponentes.Rows.Count) IdPerfilCompUC = Convert.ToInt32(tblUCComponentes.Rows[ucindex]["IdPerfilComp"]);

                    DataRow row;
                    row = tblPerfilComponentes.NewRow();
                    if (IdPerfilCompUC == IdPerfilComp)
                    {
                        row["IdPerfilCompPadre"] = Convert.ToInt32(tblUCComponentes.Rows[ucindex]["IdPerfilCompPadre"]);
                        row["IdUCComp"] = Convert.ToInt32(tblUCComponentes.Rows[ucindex]["IdUCComp"]);
                        row["IdPerfilComp"] = Convert.ToInt32(tblUCComponentes.Rows[ucindex]["IdPerfilComp"]);
                        row["Nivel"] = Convert.ToInt32(tblUCComponentes.Rows[ucindex]["Nivel"]);
                        row["PerfilComp"] = Convert.ToString(tblUCComponentes.Rows[ucindex]["PerfilComp"]);
                        row["IdUC"] = Convert.ToInt32(tblUCComponentes.Rows[ucindex]["IdUC"]);
                        row["IdTipoDetalle"] = Convert.ToInt32(tblUCComponentes.Rows[ucindex]["IdTipoDetalle"]);
                        row["IdItem"] = Convert.ToInt32(tblUCComponentes.Rows[ucindex]["IdItem"]);
                        row["NroSerie"] = Convert.ToString(tblUCComponentes.Rows[ucindex]["NroSerie"]);
                        row["CodigoSAP"] = Convert.ToString(tblUCComponentes.Rows[ucindex]["CodigoSAP"]);
                        row["DescripcionSAP"] = Convert.ToString(tblUCComponentes.Rows[ucindex]["DescripcionSAP"]);
                        row["IdEstadoUCComp"] = Convert.ToInt32(tblUCComponentes.Rows[ucindex]["IdEstadoUCComp"]);
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        tblPerfilComponentes.Rows.Add(row);
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
                        row["IdEstadoUCComp"] = 1;
                        row["FlagActivo"] = true;
                        row["Nuevo"] = true;
                        tblPerfilComponentes.Rows.Add(row);
                    }
                }
                trvComp.ItemsSource = null;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();
                Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblPerfilComponentes;
                trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(1000, null);

                //Listar Ciclos
                E_UCComp_Ciclo objE_UCComp_Ciclo = new E_UCComp_Ciclo();
                DataTable tblPerfilCompCicloDatos = objUCCompCiclo.Item_Ciclo_List(objE_UCComp);
                for (int i = 0; i < tblPerfilCompCicloDatos.Rows.Count; i++)
                {
                    DataRow row;
                    row = tblPerfilCompCiclo.NewRow();

                    row["IdUCCompCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdItemCiclo"]);
                    row["IdUCComp"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdItem"]);
                    row["IdPerfilCompCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilCompCiclo"]);
                    row["IdPerfilComp"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilComp"]);
                    row["IdCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]);

                    row["Ciclo"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? gstrCicloDefecto : tblPerfilCompCicloDatos.Rows[i]["Ciclo"].ToString();
                    row["Contador"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["Contador"]) / gintValorTiempoDefecto : Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["Contador"]);
                    row["FrecuenciaExtendida"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaExtendida"]) / gintValorTiempoDefecto : Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaExtendida"]);
                    row["FrecuenciaCambio"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaCambio"]) / gintValorTiempoDefecto : Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaCambio"]);

                    row["PorcAmarillo"] = Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["PorcAmarillo"]);
                    row["PorcNaranja"] = Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["PorcNaranja"]);
                    row["FlagCicloPrincipal"] = Convert.ToBoolean(tblPerfilCompCicloDatos.Rows[i]["FlagCicloPrincipal"]);
                    row["IdEstadoCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdEstadoPCC"]);
                    row["FlagActivo"] = true;
                    row["Nuevo"] = false;
                    tblPerfilCompCiclo.Rows.Add(row);
                }

                //Listar Ciclos
                int IdUCCompCiclo = 1;
                int IdUCComp = 0;
                objE_UCComp_Ciclo.IdPerfil = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdPerfil"));
                tblPerfilCompCicloDatos = objUCCompCiclo.PerfilComp_Ciclo_List(objE_UCComp_Ciclo);

                #region REQUERIMIENTO_01
                DataTable tblPerfil = new DataTable();
                objE_Perfil.Idperfil = Convert.ToInt32(objE_UCComp.IdPerfil);
                tblPerfil = objPerfil.Perfil_GetItem(objE_Perfil);
                int valorCiclo = Convert.ToInt32(tblPerfil.Rows[0]["IdCicloDefecto"]);
                #endregion

                for (int i = 0; i < tblPerfilCompCicloDatos.Rows.Count; i++)
                {
                    int cantexiste = tblPerfilCompCiclo.Select("IdPerfilCompCiclo = " + Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilCompCiclo"])).Length;
                    if (cantexiste == 0)
                    {
                        DataRow row;
                        row = tblPerfilCompCiclo.NewRow();
                        row["IdUCCompCiclo"] = IdUCCompCiclo;
                        row["IdUCComp"] = IdUCComp;
                        row["IdPerfilCompCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilCompCiclo"]);
                        row["IdPerfilComp"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilComp"]);
                        row["IdCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]);
                        row["Ciclo"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? gstrCicloDefecto : tblPerfilCompCicloDatos.Rows[i]["Ciclo"].ToString();
                        row["Contador"] = 0;
                        row["FrecuenciaExtendida"] = 0;
                        row["PorcAmarillo"] = 20.0;
                        row["PorcNaranja"] = 10.0;
                        row["FrecuenciaCambio"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaCambio"]) / gintValorTiempoDefecto : Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaCambio"]);
                        #region REQUERIMIENTO_01
                        //row["FlagCicloPrincipal"] = false;
                        if (valorCiclo == Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]))
                        {
                            row["FlagCicloPrincipal"] = true;
                        }
                        else
                        {
                            row["FlagCicloPrincipal"] = false;
                        }
                        #endregion
                        row["IdEstadoCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdEstadoPCC"]);
                        row["FlagActivo"] = true;
                        row["Nuevo"] = true;
                        tblPerfilCompCiclo.Rows.Add(row);
                        IdUCCompCiclo++;
                    }
                }
                CrearTablasUCCOmponentes();

                tblPerfilComponentestmp = tblPerfilComponentes.Copy();

                trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged += new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged += new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LimpiarDatosPerfilMod()
        {
            dtgCiclo.PreviewKeyDown -= new KeyEventHandler(dtgCiclo_PreviewKeyDown);
            txtCodiSAP.EditValueChanged -= new EditValueChangedEventHandler(txtCodiSAP_EditValueChanged);
            txtNserie.EditValueChanged -= new EditValueChangedEventHandler(txtNserie_EditValueChanged);

            ChkComponenteExistente.IsChecked = false;
            rbntitu.IsChecked = false;
            rbnComp.IsChecked = false;
            dtgCiclo.ItemsSource = null;
            txtCodiSAP.Text = "";
            txtNserie.Text = "";
            lblDesc.Content = "";
            cboEstado.IsEnabled = false;
            btnAbrirSAP.IsEnabled = false;
            //txtNserie.IsEnabled = false;
            //txtCodiSAP.IsEnabled = false;
            lblDesc.IsEnabled = false;
            label31.IsEnabled = false;
            label29.IsEnabled = false;
            label28.IsEnabled = false;
            label27.IsEnabled = false;
            dtgCiclo.IsEnabled = false;

            txtNserie.EditValueChanged += new EditValueChangedEventHandler(txtNserie_EditValueChanged);
            txtCodiSAP.EditValueChanged += new EditValueChangedEventHandler(txtCodiSAP_EditValueChanged);
            dtgCiclo.PreviewKeyDown += new KeyEventHandler(dtgCiclo_PreviewKeyDown);
        }

        private void LlenarDatosPerfilModificar()
        {
            try
            {
                LimpiarDatosPerfilMod();
                trvComp.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged -= new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged -= new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
                txtobservacion.EditValueChanged -= new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);

                trvComp.ItemsSource = null;
                Utilitarios.TreeViewModel.LimpiarDatosTreeview();

                chkTodaEstr.IsChecked = false;
                objE_UCComp.IdPerfil = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdPerfil").ToString());
                objE_UCComp.IdUC = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdUC").ToString());
                objEUC.IdUc = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdUC").ToString());
                DataTable tblUC = objUC.B_UC_GetItem(objEUC);
                txtobservacion.Text = tblUC.Rows[0]["Observacion"].ToString();

                int IdCicloDefecto = Convert.ToInt32(tblUC.Rows[0]["IdCicloDefecto"]);
                if (IdCicloDefecto == 4)
                {
                    lblContadorAcumulado.Content = String.Format("{0:###,###,##0.00}", Convert.ToDouble(tblUC.Rows[0]["ContadorAcum"]) / gintValorTiempoDefecto);
                }
                else
                {
                    lblContadorAcumulado.Content = String.Format("{0:###,###,##0.00}", Convert.ToDouble(tblUC.Rows[0]["ContadorAcum"]));
                }

                lblAuditoria_creacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblUC.Rows[0]["UsuarioCreacion"].ToString(), tblUC.Rows[0]["FechaCreacion"].ToString(), tblUC.Rows[0]["HostCreacion"].ToString());
                lblAuditoria_modificacion.Text = String.Format("Usuario: {0} Fecha: {1} Host: {2}", tblUC.Rows[0]["UsuarioModificacion"].ToString(), tblUC.Rows[0]["FechaModificacion"].ToString(), tblUC.Rows[0]["HostModificacion"].ToString());


                CrearTablasPerfilComponentes();
                tblUCComponentes = objUCComp.UCComp_List(objE_UCComp);
                objE_PerfilComp.Idperfil = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdPerfil").ToString());
                objE_PerfilComp.Idestadopc = 0;
                tblPerfilComponentes.Rows.Clear();
                tblPerfilComponentesDatos = new DataTable();
                tblPerfilComponentesDatos = objPerfilComp.PerfilComp_List(objE_PerfilComp);
                tblUCComponentes.DefaultView.Sort = "IdPerfilComp asc";
                tblUCComponentes = tblUCComponentes.DefaultView.ToTable(true);
                tblPerfilComponentesDatos.DefaultView.Sort = "IdPerfilComp asc";
                tblPerfilComponentesDatos.DefaultView.RowFilter = "FlagNeumatico = 0";
                tblPerfilComponentesDatos = tblPerfilComponentesDatos.DefaultView.ToTable(true);
                DataRow row1;
                row1 = tblPerfilComponentes.NewRow();
                row1["IdPerfilCompPadre"] = 1000;
                row1["IdPerfilComp"] = 0;
                row1["Nivel"] = 1;
                row1["PerfilComp"] = lblUnidCont.Content;
                row1["NroSerie"] = "";
                row1["CodigoSAP"] = "";
                row1["DescripcionSAP"] = "";
                row1["Nuevo"] = true;
                row1["IdEstadoUCComp"] = 1;
                row1["IdTipoDetalle"] = 1;
                tblPerfilComponentes.Rows.Add(row1);


                int idpadre = 0;
                int CantExiste = 0;
                int CantExisteDatos = 0;
                DataView dtvPerfilComp = new DataView();
                for (int i = 0; i < tblUCComponentes.Rows.Count; i++)
                {
                    if (Convert.ToString(tblUCComponentes.Rows[i]["NroSerie"]) != "")
                    {
                        DataRow row;
                        row = tblPerfilComponentes.NewRow();
                        row["IdPerfilCompPadre"] = Convert.ToInt32(tblUCComponentes.Rows[i]["IdPerfilCompPadre"]);
                        row["IdUCComp"] = Convert.ToInt32(tblUCComponentes.Rows[i]["IdUCComp"]);
                        row["IdPerfilComp"] = Convert.ToInt32(tblUCComponentes.Rows[i]["IdPerfilComp"]);
                        row["Nivel"] = Convert.ToInt32(tblUCComponentes.Rows[i]["Nivel"]);
                        row["PerfilComp"] = Convert.ToString(tblUCComponentes.Rows[i]["PerfilComp"]);
                        row["IdUC"] = Convert.ToInt32(tblUCComponentes.Rows[i]["IdUC"]);
                        row["IdTipoDetalle"] = Convert.ToInt32(tblUCComponentes.Rows[i]["IdTipoDetalle"]);
                        row["IdItem"] = Convert.ToInt32(tblUCComponentes.Rows[i]["IdItem"]);
                        row["NroSerie"] = Convert.ToString(tblUCComponentes.Rows[i]["NroSerie"]);
                        row["CodigoSAP"] = Convert.ToString(tblUCComponentes.Rows[i]["CodigoSAP"]);
                        row["DescripcionSAP"] = Convert.ToString(tblUCComponentes.Rows[i]["DescripcionSAP"]);
                        row["IdEstadoUCComp"] = Convert.ToInt32(tblUCComponentes.Rows[i]["IdEstadoUCComp"]);
                        row["FlagActivo"] = true;
                        row["Nuevo"] = false;
                        tblPerfilComponentes.Rows.Add(row);

                        idpadre = Convert.ToInt32(tblUCComponentes.Rows[i]["IdPerfilCompPadre"]);
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
                            row2["IdTipoDetalle"] = Convert.ToInt32(dtvPerfilComp[0]["IdTipoDetalle"]);
                            row2["IdItem"] = 0;
                            row2["NroSerie"] = "";
                            row2["CodigoSAP"] = Convert.ToString(dtvPerfilComp[0]["CodigoSAP"]);
                            row2["DescripcionSAP"] = Convert.ToString(dtvPerfilComp[0]["DescripcionSAP"]);
                            row2["IdEstadoUCComp"] = Convert.ToBoolean(dtvPerfilComp[0]["IdEstadoPC"]);
                            row2["FlagActivo"] = true;
                            row2["Nuevo"] = false;
                            tblPerfilComponentes.Rows.Add(row2);
                            idpadre = Convert.ToInt32(dtvPerfilComp[0]["IdPerfilCompPadre"]);
                            CantExiste = tblPerfilComponentes.Select("IdPerfilComp = " + idpadre.ToString()).Length;
                            CantExisteDatos = tblPerfilComponentes.Select("IdPerfilComp = " + idpadre.ToString()).Length;
                        }
                        CantExiste = 0;
                        idpadre = 0;
                    }
                }

                tblPerfilComponentes.DefaultView.Sort = "IdPerfilComp asc";
                tblPerfilComponentes = tblPerfilComponentes.DefaultView.ToTable(true);
                Utilitarios.TreeViewModel.tblListarPerfilComponentes = tblPerfilComponentes;
                trvComp.ItemsSource = Utilitarios.TreeViewModel.CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(1000, null);

                //Listar Ciclos
                E_UCComp_Ciclo objE_UCComp_Ciclo = new E_UCComp_Ciclo();
                DataTable tblPerfilCompCicloDatos = objUCCompCiclo.Item_Ciclo_List(objE_UCComp);

                for (int i = 0; i < tblPerfilCompCicloDatos.Rows.Count; i++)
                {
                    DataRow row;
                    row = tblPerfilCompCiclo.NewRow();

                    row["IdUCCompCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdItemCiclo"]);
                    row["IdUCComp"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdItem"]);
                    row["IdPerfilCompCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilCompCiclo"]);
                    row["IdPerfilComp"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdPerfilComp"]);
                    row["IdCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]);

                    row["Ciclo"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? gstrCicloDefecto : tblPerfilCompCicloDatos.Rows[i]["Ciclo"].ToString();
                    row["Contador"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["Contador"]) / gintValorTiempoDefecto : Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["Contador"]);
                    row["FrecuenciaExtendida"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaExtendida"]) / gintValorTiempoDefecto : Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaExtendida"]);
                    row["FrecuenciaCambio"] = (Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdCiclo"]) == 4) ? Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaCambio"]) / gintValorTiempoDefecto : Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["FrecuenciaCambio"]);

                    row["PorcAmarillo"] = Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["PorcAmarillo"]);
                    row["PorcNaranja"] = Convert.ToDouble(tblPerfilCompCicloDatos.Rows[i]["PorcNaranja"]);
                    row["FlagCicloPrincipal"] = Convert.ToBoolean(tblPerfilCompCicloDatos.Rows[i]["FlagCicloPrincipal"]);
                    row["IdEstadoCiclo"] = Convert.ToInt32(tblPerfilCompCicloDatos.Rows[i]["IdEstadoPCC"]);
                    row["FlagActivo"] = true;
                    row["Nuevo"] = false;
                    tblPerfilCompCiclo.Rows.Add(row);
                }
                lblUnidCont.Content = cboUnidCont.Text;
                lblPerfUCComp.Content = cboPerfil.Text;
                CrearTablasUCCOmponentes();
                tblPerfilComponentestmp = tblPerfilComponentes.Copy();
                FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                txtobservacion.EditValueChanged += new EditValueChangedEventHandler(txtPLANTILLA_EditValueChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged += new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged += new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
                trvComp.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(trvComp_SelectedItemChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboUnidCont_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                string CodUC = cboUnidCont.EditValue.ToString();
                tucuclist = InterfazMTTO.iSBO_BL.UnidadControl_BL.ListaUnidadControl(CodUC, ref RPTA);
                if (RPTA.CodigoErrorUsuario != "000")
                {
                    GlobalClass.ip.Mensaje(RPTA.DescripcionErrorUsuario, 3);
                }
                lblUnidCont.Content = cboUnidCont.Text;
                lblPlacaSerie.Content = tucuclist[0].PlacaSerieUnidadControl;
                lblMarca.Content = tucuclist[0].Marca;
                lblProp.Content = tucuclist[0].Propietario;
                lblTipoUni.Content = tucuclist[0].DescripcionTipoUnidadControl;
                lblConfi.Content = tucuclist[0].Configuracion;
                lblAnio.Content = tucuclist[0].Anho;
                lblTipoProp.Content = tucuclist[0].TipoPropiedad;
                lblMode.Content = tucuclist[0].Modelo;
                lblFami.Content = tucuclist[0].Familia;
                lblSubFam.Content = tucuclist[0].SubFamilia;
                lblLinea.Content = tucuclist[0].LineaNegocio;

                if (gbolNuevo == true && gbolEdicion == false)
                {
                    LlenarDatosPerfilNuevo();
                }
                tbDatosCompUC.IsEnabled = true;
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
                cboEstado.SelectedIndexChanged -= new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged -= new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged -= new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
                dtgCiclo.SelectedItemChanged -= new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(dtgCiclo_SelectedItemChanged);
                txtCodiSAP.EditValueChanged -= new EditValueChangedEventHandler(txtCodiSAP_EditValueChanged);
                txtNserie.EditValueChanged -= new EditValueChangedEventHandler(txtNserie_EditValueChanged);
                //Listar Ciclos Filtro
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    btnAbrirSAP.IsEnabled = true;
                    cboEstado.IsEnabled = true;
                    txtNserie.IsReadOnly = false;
                    int idPerfilComp = Convert.ToInt32(trm.IdMenu);
                    int idPadre = Convert.ToInt32(trm.IdMenuPadre);
                    string Padre = "";
                    DataView dtvCiclos = tblPerfilCompCiclo.DefaultView;
                    dtvCiclos.RowFilter = "idPerfilComp = " + idPerfilComp;
                    dtgCiclo.ItemsSource = dtvCiclos;
                    foreach (DataRow item in tblPerfilComponentes.Select("IdPerfilComp = " + trm.IdMenuPadre))
                    {
                        Padre = item["PerfilComp"].ToString();
                    }
                    //Listar Campos Componentes
                    if (idPadre != 1000)
                    {
                        lblPadre.Content = String.Format("Padre: {0} | Nivel {1}", Padre, trm.Nivel.ToString());
                    }
                    else
                    {
                        btnAbrirSAP.IsEnabled = false;
                        lblPadre.Content = String.Format("Padre: {0} | Nivel{1}", trm.Name, trm.Nivel.ToString());
                    }


                    DataView dtvComp = tblPerfilComponentes.DefaultView; ;
                    dtvComp.RowFilter = "idPerfilComp = " + idPerfilComp;

                    lblDesc.Content = dtvComp[0]["PerfilComp"].ToString();
                    txtCodiSAP.Text = dtvComp[0]["CodigoSAP"].ToString();

                    if (dtvComp[0]["DescripcionSAP"].ToString() == "")
                    {
                        txtCodiSAP.ToolTip = null;
                    }
                    else
                    {
                        txtCodiSAP.ToolTip = dtvComp[0]["DescripcionSAP"].ToString();
                    }


                    txtNserie.Text = dtvComp[0]["NroSerie"].ToString();
                    cboEstado.EditValue = Convert.ToInt32(dtvComp[0]["IdEstadoUCComp"].ToString());

                    if (txtNserie.Text.Length > 0) { ChkComponenteExistente.IsChecked = true; }
                    else { ChkComponenteExistente.IsChecked = false; }

                    if (Convert.ToInt32(dtvComp[0]["IdTipoDetalle"]) == 0)
                    {
                        foreach (DataRow drPfComp in tblPerfilComponentes.Select("IdPerfilCompPadre = " + idPerfilComp))
                        {
                            dtvComp[0]["IdTipoDetalle"] = 1;
                        }
                    }

                    if (Convert.ToInt32(dtvComp[0]["IdTipoDetalle"]) == 1)
                    {
                        if (!gbolFlagInactivo)
                        {
                            rbntitu.IsChecked = true;
                            cboEstado.IsEnabled = false;
                            btnAbrirSAP.IsEnabled = false;
                            txtNserie.IsEnabled = false;
                            txtCodiSAP.IsEnabled = false;
                            lblDesc.IsEnabled = false;
                            label31.IsEnabled = false;
                            label29.IsEnabled = false;
                            label28.IsEnabled = false;
                            label27.IsEnabled = false;
                            dtgCiclo.IsEnabled = false;
                        }
                    }
                    else
                    {
                        if (!gbolFlagInactivo)
                        {
                            rbnComp.IsChecked = true;
                            //cboEstado.IsEnabled = true;
                            //btnAbrirSAP.IsEnabled = true;
                            txtNserie.IsEnabled = true;
                            txtCodiSAP.IsEnabled = true;
                            lblDesc.IsEnabled = true;
                            label31.IsEnabled = true;
                            label29.IsEnabled = true;
                            label28.IsEnabled = true;
                            label27.IsEnabled = true;
                            dtgCiclo.IsEnabled = true;
                        }
                    }

                    int IdUCComp = 0;
                    string PerfilComp = "";
                    string NroSerie = "";
                    foreach (DataRow drUCComp in tblPerfilComponentes.Select("idPerfilComp = " + idPerfilComp + " AND IdPerfilCompPadre <> 1000"))
                    {
                        IdUCComp = Convert.ToInt32(drUCComp["IdUCComp"]);
                        PerfilComp = drUCComp["PerfilComp"].ToString();
                        NroSerie = drUCComp["NroSerie"].ToString();
                    }
                    if (!gbolFlagInactivo)
                    {
                        objE_UCComp.IdUCComp = IdUCComp;
                        DataTable tblUCCompValida = objUCComp.UCComp_GetBeforeChange(objE_UCComp);
                        if (Convert.ToInt32(tblUCCompValida.Rows[0]["Contador"]) != 0 || gintCantDocumentos != 0)
                        {
                            if (NroSerie != "")
                            {
                                //groupBox2.IsEnabled = false;
                                txtNserie.IsReadOnly = true;
                                cboEstado.IsEnabled = false;
                                btnAbrirSAP.IsEnabled = false;
                                groupBox2.Header = String.Format("Información del componente: {0} | La unidad de control está asociado a un documento activo", PerfilComp);

                                dtgCiclo.Columns["Contador"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                                dtgCiclo.Columns["FrecuenciaCambio"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                                dtgCiclo.Columns["PorcAmarillo"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                                dtgCiclo.Columns["PorcNaranja"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                                #region REQUERIMIENTO_01
                                dtgCiclo.Columns["FlagCicloPrincipal"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                                #endregion
                            }
                            else
                            {
                                //groupBox2.IsEnabled = true;
                                //txtNserie.IsReadOnly = false;
                                //cboEstado.IsEnabled = true;
                                //btnAbrirSAP.IsEnabled = true;
                                groupBox2.Header = String.Format("Información del componente: {0}", PerfilComp);

                                dtgCiclo.Columns["Contador"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                                dtgCiclo.Columns["FrecuenciaCambio"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                                dtgCiclo.Columns["PorcAmarillo"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                                dtgCiclo.Columns["PorcNaranja"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                                #region REQUERIMIENTO_01
                                dtgCiclo.Columns["FlagCicloPrincipal"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                                #endregion
                            }
                        }
                        else
                        {
                            //groupBox2.IsEnabled = true;
                            //txtNserie.IsReadOnly = false;
                            //cboEstado.IsEnabled = true;
                            //btnAbrirSAP.IsEnabled = true;
                            groupBox2.Header = String.Format("Información del componente: {0}", PerfilComp);

                            dtgCiclo.Columns["Contador"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                            dtgCiclo.Columns["FrecuenciaCambio"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                            dtgCiclo.Columns["PorcAmarillo"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                            dtgCiclo.Columns["PorcNaranja"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                            #region REQUERIMIENTO_01
                            dtgCiclo.Columns["FlagCicloPrincipal"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                            #endregion
                        }
                    }
                }
                txtNserie.EditValueChanged += new EditValueChangedEventHandler(txtNserie_EditValueChanged);
                txtCodiSAP.EditValueChanged += new EditValueChangedEventHandler(txtCodiSAP_EditValueChanged);
                dtgCiclo.SelectedItemChanged += new DevExpress.Xpf.Grid.SelectedItemChangedEventHandler(dtgCiclo_SelectedItemChanged);
                cboEstado.SelectedIndexChanged += new RoutedEventHandler(cboEstado_SelectedIndexChanged);
                cboPerfil.SelectedIndexChanged += new RoutedEventHandler(cboPerfil_SelectedIndexChanged);
                cboUnidCont.SelectedIndexChanged += new RoutedEventHandler(cboUnidCont_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAbrirSAP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarBotonDefecto(false);
                btnAgregarComp.IsDefault = true;
                btnCancelComp.IsCancel = true;
                stkPanelComponenteSAP.Visibility = System.Windows.Visibility.Visible;
                cboCompSAP.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnAgregarComp_Click(object sender, RoutedEventArgs e)
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
                        CambiarBotonDefecto(true);
                        btnAgregarComp.IsDefault = false;
                        btnCancelComp.IsCancel = false;
                        txtCodiSAP.Text = cboCompSAP.EditValue.ToString();
                        txtCodiSAP.ToolTip = cboCompSAP.Text;
                        stkPanelComponenteSAP.Visibility = System.Windows.Visibility.Hidden;
                        cboCompSAP.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }
        void CambiarBotonDefecto(bool estado)
        {
            btnAcepComp.IsDefault = estado;
            btnCancComp.IsCancel = estado;
        }
        private void btnCancelComp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarBotonDefecto(true);
                btnAgregarComp.IsDefault = false;
                btnCancelComp.IsCancel = false;
                stkPanelComponenteSAP.Visibility = System.Windows.Visibility.Hidden;
                cboCompSAP.SelectedIndex = -1;
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
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    int idPerfilComp = Convert.ToInt32(trm.IdMenu);
                    foreach (DataRow drPfComp in tblPerfilComponentes.Select("IdPerfilComp = " + idPerfilComp))
                    {
                        drPfComp["IdEstadoUCComp"] = cboEstado.EditValue;
                    }
                }
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgUnidContr_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }

        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidaCampoObligado() == true) { return; }
                tblUCComponentes.Rows.Clear();
                tblItem_Ciclo.Rows.Clear();
                int cod = 1;
                string Componente = "";

                if (gbolNuevo != false || gbolEdicion != false)
                {
                    foreach (DataRow drPfCiclo in tblPerfilCompCiclo.Select("Contador > 0 AND (FrecuenciaCambio = 0 OR FrecuenciaCambio IS NULL)"))
                    {
                        foreach (DataRow drPfComp in tblPerfilComponentes.Select("IdPerfilComp = " + Convert.ToInt32(drPfCiclo["IdPerfilComp"])))
                        {
                            Componente = drPfComp["PerfilComp"].ToString();
                        }
                        GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_CICL_FREC"), drPfCiclo["Ciclo"].ToString(), Componente), 2);
                        return;
                    }

                    foreach (DataRow drPfCompTMP in tblPerfilComponentestmp.Select("CodigoSAP = ''"))
                    {
                        if (tblPerfilComponentes.Select("NroSerie = '' AND CodigoSAP <> '' AND IdPerfilComp = " + Convert.ToInt32(drPfCompTMP["IdPerfilComp"])).Length > 0)
                        {
                            GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_CSAP_COMP"), drPfCompTMP["PerfilComp"].ToString()), 2);
                            return;
                        }
                    }
                }
                if (gbolNuevo == true && gbolEdicion == false)
                {
                    objEUC.IdUc = 0;
                    objEUC.CodUc = cboUnidCont.EditValue.ToString();
                    objEUC.PlacaSerie = lblPlacaSerie.Content.ToString();
                    objEUC.IdTipoUnidad = tucuclist[0].CodigoTipoUnidadControl;
                    objEUC.ContadorAcum = 0;
                    objEUC.IdPerfil = Convert.ToInt32(cboPerfil.EditValue);
                    objEUC.Observacion = txtobservacion.Text;
                    objEUC.IdUsuarioCreacion = gintusuario;
                    objEUC.IdEstadoUC = Convert.ToInt32(cboEstadoUC.EditValue);
                    objEUC.FechaModificacion = DateTime.Now;
                    objEUC.ConContadorAutomatico = checkContadorAutomatico.IsChecked== true ? true : false;
                    objEUC.FechaInicioUso = fechaInicio.SelectedDate.Value;
                    objEUC.FechaUltimoControl = objEUC.FechaInicioUso;

                    foreach (DataRow drPfComp in tblPerfilComponentes.Select("NroSerie <> ''"))
                    {
                        DataRow row1;
                        row1 = tblUCComponentes.NewRow();
                        row1["IdUCComp"] = cod;
                        row1["IdPerfilComp"] = Convert.ToInt32(drPfComp["IdPerfilComp"]);
                        row1["IdUC"] = Convert.ToInt32(drPfComp["IdUC"]);
                        row1["IdTipoDetalle"] = Convert.ToInt32(drPfComp["IdTipoDetalle"]);
                        row1["IdItem"] = cod;//Convert.ToInt32(drPfComp["IdItem"]);
                        row1["NroSerie"] = Convert.ToString(drPfComp["NroSerie"]);
                        row1["CodigoSAP"] = Convert.ToString(drPfComp["CodigoSAP"]);
                        row1["DescripcionSAP"] = Convert.ToString(drPfComp["DescripcionSAP"]);
                        row1["IdEstadoUCComp"] = Convert.ToInt32(drPfComp["IdEstadoUCComp"]);
                        row1["FlagActivo"] = true;
                        row1["Nuevo"] = true;
                        tblUCComponentes.Rows.Add(row1);

                        foreach (DataRow drPfCompCi in tblPerfilCompCiclo.Select("IdPerfilComp = " + Convert.ToInt32(drPfComp["IdPerfilComp"])))
                        {
                            DataRow row;
                            row = tblItem_Ciclo.NewRow();
                            row["IdItemCiclo"] = Convert.ToInt32(drPfCompCi["IdUCCompCiclo"]);
                            row["IdItem"] = cod;
                            row["IdPerfilCompCiclo"] = Convert.ToInt32(drPfCompCi["IdPerfilCompCiclo"]);

                            row["FrecuenciaCambio"] = (Convert.ToInt32(drPfCompCi["IdCiclo"]) == 4) ? Convert.ToDouble(drPfCompCi["FrecuenciaCambio"]) * gintValorTiempoDefecto : Convert.ToDouble(drPfCompCi["FrecuenciaCambio"]);
                            row["Contador"] = (Convert.ToInt32(drPfCompCi["IdCiclo"]) == 4) ? Convert.ToDouble(drPfCompCi["Contador"]) * gintValorTiempoDefecto : Convert.ToDouble(drPfCompCi["Contador"]);
                            row["FrecuenciaExtendida"] = (Convert.ToInt32(drPfCompCi["IdCiclo"]) == 4) ? Convert.ToDouble(drPfCompCi["FrecuenciaExtendida"]) * gintValorTiempoDefecto : Convert.ToDouble(drPfCompCi["FrecuenciaExtendida"]);

                            row["PorcAmarillo"] = Convert.ToDouble(drPfCompCi["PorcAmarillo"]);
                            row["PorcNaranja"] = Convert.ToDouble(drPfCompCi["PorcNaranja"]);
                            row["FlagCicloPrincipal"] = Convert.ToBoolean(drPfCompCi["FlagCicloPrincipal"]);
                            row["IdEstadoCiclo"] = Convert.ToInt32(drPfCompCi["IdEstadoCiclo"]);
                            row["FlagActivo"] = true;
                            row["Nuevo"] = true;
                            tblItem_Ciclo.Rows.Add(row);
                        }
                        cod++;
                    }
                    cod = 1;

                    foreach (DataRow drUCComp in tblUCComponentes.Select("NroSerie <> '' AND CodigoSAP = ''"))
                    {
                        foreach (DataRow drPfComp in tblPerfilComponentes.Select("IdPerfilComp = " + Convert.ToInt32(drUCComp["IdPerfilComp"])))
                        {
                            GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_CODI_SAP"), drPfComp["PerfilComp"].ToString()), 2);
                            return;
                        }
                    }

                    if (tblUCComponentes.Rows.Count == 0)
                    {
                        objEUC.IdEstadoUC = 2;
                    }
                    int rpt = objUC.UC_InsertMasivo(objEUC, tblUCComponentes, tblItem_Ciclo);
                    if (rpt == 1)
                    {
                        EstadoForm(false, false, true);
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "GRAB_NUEV"), 1);
                    }
                    else if (rpt == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpt == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "GRAB_CONC"), 2);
                        return;
                    }
                }
                else if (gbolNuevo == false && gbolEdicion == true)
                {
                    objEUC.IdUc = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdUC"));
                    objEUC.CodUc = cboUnidCont.EditValue.ToString();
                    objEUC.PlacaSerie = lblPlacaSerie.Content.ToString();
                    objEUC.IdTipoUnidad = tucuclist[0].CodigoTipoUnidadControl;
                    objEUC.ContadorAcum = 0;
                    objEUC.IdPerfil = Convert.ToInt32(cboPerfil.EditValue);
                    objEUC.Observacion = txtobservacion.Text;
                    objEUC.IdUsuarioCreacion = gintusuario;
                    objEUC.IdEstadoUC = Convert.ToInt32(cboEstadoUC.EditValue);
                    objEUC.FechaModificacion = FechaModificacion;

                    foreach (DataRow drPfComp in tblPerfilComponentes.Select("Nuevo = false")) //NroSerie <> '' AND Nuevo = false
                    {
                        DataRow row1;
                        row1 = tblUCComponentes.NewRow();
                        row1["IdUCComp"] = Convert.ToInt32(drPfComp["IdUCComp"]);
                        row1["IdPerfilComp"] = Convert.ToInt32(drPfComp["IdPerfilComp"]);
                        row1["IdUC"] = Convert.ToInt32(drPfComp["IdUC"]);
                        row1["IdTipoDetalle"] = Convert.ToInt32(drPfComp["IdTipoDetalle"]);
                        row1["IdItem"] = Convert.ToInt32(drPfComp["IdItem"]);
                        row1["NroSerie"] = Convert.ToString(drPfComp["NroSerie"]);
                        row1["CodigoSAP"] = Convert.ToString(drPfComp["CodigoSAP"]);
                        row1["DescripcionSAP"] = Convert.ToString(drPfComp["DescripcionSAP"]);
                        row1["IdEstadoUCComp"] = Convert.ToInt32(drPfComp["IdEstadoUCComp"]);
                        row1["FlagActivo"] = Convert.ToBoolean(drPfComp["FlagActivo"]);
                        row1["Nuevo"] = false;
                        tblUCComponentes.Rows.Add(row1);

                        foreach (DataRow drPfCiclo in tblPerfilCompCiclo.Select("IdPerfilComp = " + Convert.ToInt32(drPfComp["IdPerfilComp"])))
                        {
                            DataRow row;
                            row = tblItem_Ciclo.NewRow();
                            row["IdItemCiclo"] = Convert.ToInt32(drPfCiclo["IdUCCompCiclo"]);
                            row["IdItem"] = Convert.ToInt32(drPfComp["IdItem"]);
                            row["IdPerfilCompCiclo"] = Convert.ToInt32(drPfCiclo["IdPerfilCompCiclo"]);

                            row["FrecuenciaCambio"] = (Convert.ToInt32(drPfCiclo["IdCiclo"]) == 4) ? Convert.ToDouble(drPfCiclo["FrecuenciaCambio"]) * gintValorTiempoDefecto : Convert.ToDouble(drPfCiclo["FrecuenciaCambio"]);
                            row["Contador"] = (Convert.ToInt32(drPfCiclo["IdCiclo"]) == 4) ? Convert.ToDouble(drPfCiclo["Contador"]) * gintValorTiempoDefecto : Convert.ToDouble(drPfCiclo["Contador"]);
                            row["FrecuenciaExtendida"] = (Convert.ToInt32(drPfCiclo["IdCiclo"]) == 4) ? Convert.ToDouble(drPfCiclo["FrecuenciaExtendida"]) * gintValorTiempoDefecto : Convert.ToDouble(drPfCiclo["FrecuenciaExtendida"]);

                            row["PorcAmarillo"] = Convert.ToDouble(drPfCiclo["PorcAmarillo"]);
                            row["PorcNaranja"] = Convert.ToDouble(drPfCiclo["PorcNaranja"]);
                            row["FlagCicloPrincipal"] = Convert.ToBoolean(drPfCiclo["FlagCicloPrincipal"]);
                            row["IdEstadoCiclo"] = Convert.ToInt32(drPfCiclo["IdEstadoCiclo"]);
                            row["FlagActivo"] = Convert.ToBoolean(drPfCiclo["FlagActivo"]);
                            row["Nuevo"] = false;
                            tblItem_Ciclo.Rows.Add(row);
                        }
                    }

                    foreach (DataRow drPfComp in tblPerfilComponentes.Select("NroSerie <> '' AND Nuevo = true"))
                    {
                        DataRow row1;
                        row1 = tblUCComponentes.NewRow();
                        row1["IdUCComp"] = cod;
                        row1["IdPerfilComp"] = Convert.ToInt32(drPfComp["IdPerfilComp"]);
                        row1["IdUC"] = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdUC"));
                        row1["IdTipoDetalle"] = Convert.ToInt32(drPfComp["IdTipoDetalle"]);
                        row1["IdItem"] = cod;
                        row1["NroSerie"] = Convert.ToString(drPfComp["NroSerie"]);
                        row1["CodigoSAP"] = Convert.ToString(drPfComp["CodigoSAP"]);
                        row1["DescripcionSAP"] = Convert.ToString(drPfComp["DescripcionSAP"]);
                        row1["IdEstadoUCComp"] = Convert.ToInt32(drPfComp["IdEstadoUCComp"]);
                        row1["FlagActivo"] = Convert.ToBoolean(drPfComp["FlagActivo"]);
                        row1["Nuevo"] = true;
                        tblUCComponentes.Rows.Add(row1);
                        foreach (DataRow drPfCiclo in tblPerfilCompCiclo.Select("IdPerfilComp = " + Convert.ToInt32(drPfComp["IdPerfilComp"])))
                        {
                            DataRow row;
                            row = tblItem_Ciclo.NewRow();
                            row["IdItemCiclo"] = 0;
                            row["IdItem"] = cod;
                            row["IdPerfilCompCiclo"] = Convert.ToInt32(drPfCiclo["IdPerfilCompCiclo"]);

                            row["FrecuenciaCambio"] = (Convert.ToInt32(drPfCiclo["IdCiclo"]) == 4) ? Convert.ToDouble(drPfCiclo["FrecuenciaCambio"]) * gintValorTiempoDefecto : Convert.ToDouble(drPfCiclo["FrecuenciaCambio"]);
                            row["Contador"] = (Convert.ToInt32(drPfCiclo["IdCiclo"]) == 4) ? Convert.ToDouble(drPfCiclo["Contador"]) * gintValorTiempoDefecto : Convert.ToDouble(drPfCiclo["Contador"]);
                            row["FrecuenciaExtendida"] = (Convert.ToInt32(drPfCiclo["IdCiclo"]) == 4) ? Convert.ToDouble(drPfCiclo["FrecuenciaExtendida"]) * gintValorTiempoDefecto : Convert.ToDouble(drPfCiclo["FrecuenciaExtendida"]);

                            row["PorcAmarillo"] = Convert.ToDouble(drPfCiclo["PorcAmarillo"]);
                            row["PorcNaranja"] = Convert.ToDouble(drPfCiclo["PorcNaranja"]);
                            row["FlagCicloPrincipal"] = Convert.ToBoolean(drPfCiclo["FlagCicloPrincipal"]);
                            row["IdEstadoCiclo"] = Convert.ToInt32(drPfCiclo["IdEstadoCiclo"]);
                            row["FlagActivo"] = Convert.ToBoolean(drPfCiclo["FlagActivo"]);
                            row["Nuevo"] = true;
                            tblItem_Ciclo.Rows.Add(row);
                        }
                        cod++;
                    }
                    cod = 1;

                    if (tblUCComponentes.Rows.Count == 0 && Convert.ToInt32(cboEstadoUC.EditValue) == 1)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_CANT_COMP"), 2);
                        return;
                    }

                    foreach (DataRow drUCComp in tblUCComponentes.Select("NroSerie <> '' AND CodigoSAP = ''"))
                    {
                        foreach (DataRow drPfComp in tblPerfilComponentes.Select("IdPerfilComp = " + Convert.ToInt32(drUCComp["IdPerfilComp"])))
                        {
                            GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_CODI_SAP"), drPfComp["PerfilComp"].ToString()), 2);
                            return;
                        }
                    }

                    foreach (DataRow drUCComp in tblUCComponentes.Select("NroSerie = ''"))
                    {
                        if (Convert.ToBoolean(drUCComp["Nuevo"]))
                        {
                            drUCComp["Nuevo"] = false;
                            drUCComp["IdUCComp"] = 0;
                        }
                        else
                        {
                            drUCComp["FlagActivo"] = false;
                        }
                        foreach (DataRow drUCCompCic in tblItem_Ciclo.Select("IdItem = " + Convert.ToInt32(drUCComp["IdUCComp"])))
                        {
                            drUCCompCic["FlagActivo"] = false;
                        }
                    }

                    if (tblUCComponentes.Select("FlagActivo = true").Length == 0)
                    {
                        objEUC.IdEstadoUC = 2;
                    }

                    int rpt = objUC.UC_InsertMasivo(objEUC, tblUCComponentes, tblItem_Ciclo);
                    if (rpt == 1)
                    {
                        EstadoForm(false, false, true);
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "GRAB_EDIT"), 1);
                    }
                    else if (rpt == 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_MODI"), 2);
                        return;
                    }
                    else if (rpt == 1205)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "GRAB_CONC"), 2);
                        return;
                    }
                }
                objEUC.IdEstadoUC = 0;
                tbIDatosUC.IsEnabled = false;
                rdbTodos.IsChecked = true;
                LimpiarDatoscontroles();
                ListarUnidadControl();
                tabControl1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void chkTodaEstr_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                LlenarDatosPerfilModificarChecked();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void chkTodaEstr_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                LlenarDatosPerfilModificar();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancComp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EstadoForm(false, false, true);
                objEUC.IdEstadoUC = 1;
                tbIDatosUC.IsEnabled = false;
                rdbActivo.IsChecked = true;
                LimpiarDatoscontroles();
                CrearTablasPerfilComponentes();
                CrearTablasUCCOmponentes();
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
            int contador = 0;
            try
            {
                if (cboPerfil.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_PERF"), 2);
                    cboPerfil.Focus();
                }
                else if (cboEstadoUC.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_ESTA"), 2);
                    cboEstadoUC.Focus();
                }
                else if (cboUnidCont.SelectedIndex == -1)
                {
                    bolRpta = true;
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_UC"), 2);
                    cboUnidCont.Focus();
                }
                #region REQUERIMIENTO_01
                else if (dtgCiclo.ItemsSource != null)
                {
                    DataTable tblciclo = tblPerfilCompCiclo;
                    foreach (DataRow drPfCiclo in tblPerfilCompCiclo.Select("FlagCicloPrincipal IS NOT NULL"))
                    {
                        if (drPfCiclo["FlagCicloPrincipal"].Equals(true))
                        {
                            contador = contador + 1;
                        }
                    }
                    if (contador == 0)
                    {
                        bolRpta = true;
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_CICLO"), 2);
                    }
                }
                #endregion
                return bolRpta;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
                return bolRpta;
            }
        }

        private void dtgUnidContr_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (dtgUnidContr.VisibleRowCount == 0) { return; }
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                if (dep is TextBlock)
                {
                    if (Utilitarios.Utilitarios.IIfNullBlank((dep as TextBlock).Tag) == "CodigoUC")
                    {
                        cboEstadoUC.SelectedIndexChanged -= new RoutedEventHandler(cboEstadoUC_SelectedIndexChanged);

                        gIdEstadoUC = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdEstadoUC"));
                        string IdUC = dtgUnidContr.GetFocusedRowCellValue("IdUC").ToString();
                        e.Handled = true;
                        EstadoForm(false, false, true);
                        cboPerfil.EditValue = Convert.ToInt32(dtgUnidContr.GetFocusedRowCellValue("IdPerfil").ToString());
                        cboUnidCont.EditValue = dtgUnidContr.GetFocusedRowCellValue("CodigoUC").ToString();
                        cboEstadoUC.EditValue = dtgUnidContr.GetFocusedRowCellValue("IdEstadoUC").ToString();
                        if (dtgUnidContr.GetFocusedRowCellValue("IdEstadoUC").ToString() == "4")
                        {
                            gbolFlagInactivo = true;
                            DesactivarEstadoInactivo(true);
                        }
                        else
                        {
                            gbolFlagInactivo = false;
                            DesactivarEstadoInactivo(false);
                        }

                        DataTable tblCant = objUC.UC_GetBeforeChange(IdUC);
                        gintCantDocumentos = Convert.ToInt32(tblCant.Rows[0]["Contador"]);

                        tbIDatosUC.IsEnabled = true;
                        cboPerfil.IsEnabled = false;
                        cboUnidCont.IsEnabled = false;
                        chkTodaEstr.IsEnabled = true;
                        LlenarDatosPerfilModificar();
                        tabControl1.SelectedIndex = 1;
                        cboEstadoUC.SelectedIndexChanged += new RoutedEventHandler(cboEstadoUC_SelectedIndexChanged);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void DesactivarEstadoInactivo(bool estado)
        {
            txtobservacion.IsReadOnly = estado;
            txtNserie.IsReadOnly = estado;
            cboEstado.IsEnabled = !estado;
            btnAbrirSAP.IsEnabled = !estado;
            dtgCiclo.IsEnabled = !estado;
        }
        private void rbnUnidadControlCheked()
        {
            try
            {
                if ((bool)rdbTodos.IsChecked)
                    objEUC.IdEstadoUC = 0;
                else if ((bool)rdbActivo.IsChecked)
                    objEUC.IdEstadoUC = 1;
                else if ((bool)rdbRegistrada.IsChecked)
                    objEUC.IdEstadoUC = 2;
                else if ((bool)rdbBaja.IsChecked)
                    objEUC.IdEstadoUC = 3;
                else if ((bool)rdbInactivo.IsChecked)
                    objEUC.IdEstadoUC = 4;
                ListarUnidadControl();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }



        private void rdbTodos_Checked(object sender, RoutedEventArgs e)
        {
            rbnUnidadControlCheked();
        }

        private void rdbActivo_Checked(object sender, RoutedEventArgs e)
        {
            rbnUnidadControlCheked();
        }

        private void rdbRegistrada_Checked(object sender, RoutedEventArgs e)
        {
            rbnUnidadControlCheked();
        }

        private void rdbBaja_Checked(object sender, RoutedEventArgs e)
        {
            rbnUnidadControlCheked();
        }

        private void rdbInactivo_Checked(object sender, RoutedEventArgs e)
        {
            rbnUnidadControlCheked();
        }

        private void btnActu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboEstaNuev.SelectedIndex == -1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "OBLI_ESTA"), 2);
                    cboEstaNuev.Focus();
                    return;
                }
                objEUC.Observacion = txtComenCambEsta.Text;
                objEUC.IdEstadoUC = Convert.ToInt32(cboEstaNuev.EditValue); ;
                objEUC.IdUsuarioCreacion = gintusuario;
                objEUC.FechaModificacion = FechaModificacion;

                DataTable tblUCCambio = (DataTable)dtgUCSele.ItemsSource;
                int IdEstadoUC = Convert.ToInt32(tblUCCambio.Rows[0]["IdEstadoUC"]);
                int NewIdEstadoUC = Convert.ToInt32(cboEstaNuev.EditValue);
                if (IdEstadoUC == 2 && NewIdEstadoUC == 1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_CAMB_ESTA1"), 2);
                    return;
                }
                else if (IdEstadoUC == 1 && NewIdEstadoUC == 2)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_CAMB_ESTA2"), 2);
                    return;
                }
                DataTable tblUCEstado = new DataTable();
                tblUCEstado.Columns.Add("IdUC", Type.GetType("System.Int32"));
                tblUCEstado.Columns.Add("IdEstadoOrigen", Type.GetType("System.Int32"));
                tblUCEstado.Columns.Add("IdEstadoDestino", Type.GetType("System.Int32"));
                tblUCEstado.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblUCEstado.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                DataRow dr;
                for (int i = 0; i < tblUCCambio.Rows.Count; i++)
                {
                    dr = tblUCEstado.NewRow();
                    dr["IdUC"] = Convert.ToInt32(tblUCCambio.Rows[i]["IdUC"]);
                    dr["IdEstadoOrigen"] = Convert.ToInt32(tblUCCambio.Rows[i]["IdEstadoUC"]);
                    dr["IdEstadoDestino"] = Convert.ToInt32(cboEstaNuev.EditValue);
                    dr["FlagActivo"] = true;
                    dr["Nuevo"] = true;
                    tblUCEstado.Rows.Add(dr);
                }

                string IdUc = "";
                foreach (DataRow drUCEst in tblUCEstado.Select())
                {
                    IdUc += drUCEst["IdUC"].ToString() + ", ";
                }

                if (IdUc != "")
                {
                    IdUc = IdUc.Remove(IdUc.Length - 2);
                    DataTable tblCant = objUC.UC_GetBeforeChange(IdUc);
                    if (Convert.ToInt32(tblCant.Rows[0]["Contador"]) != 0)
                    {
                        GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_CAMB_ASIG"), 2);
                        return;
                    }
                }

                int rpta = objUC.UCCambioEstado_UpdateCascade(objEUC, tblUCEstado);
                if (rpta == 1)
                {
                    LimpiarDatosCambioEstado();
                    ListarUnidadControl();
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "GRAB_ESTA"), 1);
                    tabControl1.SelectedIndex = 0;
                }
                else if (rpta == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "LOGI_MODI"), 2);
                    return;
                }
                else if (rpta == 1205)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaUnidadControl, "GRAB_CONC"), 2);
                    return;
                }

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCancelarUC_Click(object sender, RoutedEventArgs e)
        {
            LimpiarDatosCambioEstado();
            ListarUnidadControl();
            tabControl1.SelectedIndex = 0;
        }

        private void LimpiarDatosCambioEstado()
        {
            tabItem4.IsEnabled = false;
            rdbActivo.IsChecked = true;
            objEUC.IdEstadoUC = 1;
            dtgUCSele.ItemsSource = null;
            txtComenCambEsta.Text = "";
            cboEstaNuev.SelectedIndex = -1;
        }

        private void cboEstadoUC_SelectedIndexChanged(object sender, RoutedEventArgs e)
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

        private void dtgCiclo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            EstadoForm(false, true, false);
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
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
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
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void dtgCiclo_IsMouseCaptureWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (dtgCiclo.ItemsSource != null)
            {
                if (dtgCiclo.CurrentColumn.FieldName == "FlagCicloPrincipal")
                {
                    if ((bool)e.NewValue == true)
                    {
                        int rowHandle, idPerfil = 0;
                        int id = Convert.ToInt32(dtgCiclo.GetFocusedRowCellValue("IdPerfilCompCiclo"));

                        for (int i = 0; i < dtgCiclo.VisibleRowCount; i++)
                        {
                            rowHandle = dtgCiclo.GetRowHandleByVisibleIndex(i);
                            idPerfil = Convert.ToInt32(dtgCiclo.GetCellValue(rowHandle, "IdPerfilCompCiclo"));
                            if (id != idPerfil)
                            {
                                dtgCiclo.SetCellValue(rowHandle, "FlagCicloPrincipal", false);
                                dtgCiclo.RefreshRow(rowHandle);
                            }
                        }
                    }
                }
            }
        }

        void ValidarCamposNullEnGrillas()
        {
            try
            {
                if (dtgCiclo.ItemsSource != null)
                {
                    DataTable tblciclo = tblPerfilCompCiclo;
                    foreach (DataRow drPfCiclo in tblPerfilCompCiclo.Select("FrecuenciaCambio IS NULL OR Contador IS NULL OR PorcAmarillo IS NULL OR PorcNaranja IS NULL"))
                    {
                        if (drPfCiclo["FrecuenciaCambio"].Equals(DBNull.Value))
                        {
                            drPfCiclo["FrecuenciaCambio"] = 0.00;
                        }
                        if (drPfCiclo["Contador"].Equals(DBNull.Value))
                        {
                            drPfCiclo["Contador"] = 0.00;
                        }
                        if (drPfCiclo["PorcAmarillo"].Equals(DBNull.Value))
                        {
                            drPfCiclo["PorcAmarillo"] = 20.0;
                        }
                        if (drPfCiclo["PorcNaranja"].Equals(DBNull.Value))
                        {
                            drPfCiclo["PorcNaranja"] = 10.0;
                        }

                    }

                    //foreach (DataRow drPfCiclo in tblPerfilCompCiclo.Select("Contador > 0 AND (FrecuenciaCambio = 0 OR FrecuenciaCambio IS NULL)"))
                    //{
                    //    GlobalClass.ip.Mensaje(String.Format("El ciclo: {0}, necesita tener una frecuencia de cambio", drPfCiclo["Ciclo"].ToString()), 2);
                    //}
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        void ValidarCheckCiclo()
        {
            try
            {
                int contador = 0;
                if (dtgCiclo.ItemsSource != null)
                {
                    DataTable tblciclo = tblPerfilCompCiclo;
                    foreach (DataRow drPfCiclo in tblPerfilCompCiclo.Select("FlagCicloPrincipal IS NOT NULL"))
                    {
                        if (drPfCiclo["FlagCicloPrincipal"].Equals(true))
                        {
                            contador = contador + 1;
                        }
                        else
                        {

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

        private void txtCodiSAP_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                if (txtNserie.Text != "" && txtCodiSAP.Text != "")
                {
                    ChkComponenteExistente.IsChecked = true;
                }
                else
                {
                    ChkComponenteExistente.IsChecked = false;
                }
                EstadoForm(false, true, false);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtNserie_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                EstadoForm(false, true, false);
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                int ex = 0;
                if (trm != null)
                {
                    int idPerfilComp = Convert.ToInt32(trm.IdMenu);

                    for (int i = 0; i < tblPerfilComponentes.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(tblPerfilComponentes.Rows[i]["IdPerfilComp"]) == idPerfilComp)
                        {
                            tblPerfilComponentes.Rows[i]["NroSerie"] = txtNserie.Text;
                        }
                        if (tblPerfilComponentes.Rows[i]["NroSerie"].ToString() != "")
                        {
                            ex++;
                        }
                    }

                    if (txtNserie.Text != "" && txtCodiSAP.Text != "")
                    {
                        ChkComponenteExistente.IsChecked = true;
                    }
                    else
                    {
                        ChkComponenteExistente.IsChecked = false;
                    }
                }

                if (ex != 0)
                {
                    cboEstadoUC.EditValue = "1";
                }
                else
                {
                    cboEstadoUC.EditValue = "2";
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void txtPLANTILLA_EditValueChanged(object sender, EditValueChangedEventArgs e)
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

        private void stkPanelComponenteSAP_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GlobalClass.ip.VentanaEmergente_Visibilidad(sender);
        }

        private void tblViewCiclo_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            try
            {
                string Componente = "";
                TreeViewModel trm = (TreeViewModel)trvComp.SelectedItem;
                if (trm != null)
                {
                    Componente = trm.Name;
                }
                DataRowView dr = (DataRowView)e.Row;
                string Ciclo = dr["Ciclo"].ToString();
                int IdCiclo = Convert.ToInt32(dr["IdCiclo"]);
                int PorcAmarillo = Convert.ToInt32(dr["PorcAmarillo"]);
                int PorcNaranja = Convert.ToInt32(dr["PorcNaranja"]);
                double Contador = Convert.ToDouble(dr["Contador"]);
                double FrecuenciaCambio = Convert.ToDouble(dr["FrecuenciaCambio"]);

                if (Contador.ToString().Length > 15)
                {
                    e.IsValid = false;
                    e.ErrorContent = "El contador excede la cantidad máxima";
                }
                else if (((FrecuenciaCambio * gintValorTiempoDefecto).ToString().Length > 17 || (FrecuenciaCambio * gintValorTiempoDefecto).ToString().Contains("E")) && IdCiclo == 4)
                {
                    e.IsValid = false;
                    e.ErrorContent = "La Frecuencia de cambio excede la cantidad máxima de";
                }
                else if (PorcNaranja > PorcAmarillo)
                {
                    e.IsValid = false;
                    e.ErrorContent = "El límite naranja no puede ser mayor al límite amarillo";
                }
                else if (PorcNaranja > 100 || PorcNaranja < 0)
                {
                    e.IsValid = false;
                    e.ErrorContent = "El rango del límite naranja es de 0 a 100";
                }
                else if (PorcAmarillo > 100 || PorcAmarillo < 0)
                {
                    e.IsValid = false;
                    e.ErrorContent = "El rango del límite amarillo es de 0 a 100";
                }
                else if (Contador > 0 && FrecuenciaCambio <= 0)
                {
                    e.IsValid = false;
                    e.ErrorContent = String.Format("El ciclo: {0} necesita tener una frecuencia de cambio", Ciclo);
                }
                else if (FrecuenciaCambio > 0 && txtNserie.Text.Trim() == "")
                {
                    e.IsValid = false;
                    e.ErrorContent = String.Format("El componente: {0} necesita tener un número de serie", Componente);
                    txtNserie.Focus();
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                ObjError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
    }
}
