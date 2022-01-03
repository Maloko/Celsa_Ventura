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

using Business;
using Entities;
using Utilitarios;

namespace AplicacionSistemaVentura.PAQ02_Planificacion
{
    /// <summary>
    /// Interaction logic for PlanPriorizaMantenimiento.xaml
    /// </summary>
    public partial class PlanPriorizaMantenimiento : UserControl
    {
        int gintusuario = 1;
        B_Perfil B_perfil = new B_Perfil();
        E_Perfil objPerfil = new E_Perfil();
        B_PM B_PM = new B_PM();
        E_PM objPM = new E_PM();
        E_TablaMaestra objTablaMaestra = new E_TablaMaestra();
        ErrorHandler objError = new ErrorHandler();
        DataTable gtblPM = new DataTable();
        DataTable tblpm = new DataTable();
        DataView dtv_TablaMaestra = new DataView();
        DataTable tblPMTemp = new DataTable();

        string gstrEtiquetaPriorizaMantenimiento = "PlanPriorizaMantenimiento";

        public class clsPM
        {
            public int IdPM { get; set; }
            public string PM { get; set; }
            public int IdPrioridad { get; set; }
            public object cboFuentePrioridad { get; set; }
            public int IdEstado { get; set; }
            public object cboFuenteEstado { get; set; }
        }

        public class ClsCombobox
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }
        private IList<ClsCombobox> CargarcboEstado()
        {
            List<ClsCombobox> ListCombo = new List<ClsCombobox>();
            DataTable tbl = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=24", dtv_TablaMaestra);
            for (int j = 0; j < tbl.Rows.Count; j++)
            {
                ListCombo.Add(new ClsCombobox()
                {
                    Id = Convert.ToInt32(tbl.Rows[j]["IdColumna"]),
                    Text = tbl.Rows[j]["Descripcion"].ToString()
                });
            }
            return ListCombo;
        }
        private IList<ClsCombobox> CargarcboPrioridad()
        {
            List<ClsCombobox> ListCombo = new List<ClsCombobox>();
            DataTable tbl = Utilitarios.Utilitarios.ListarCombo_TablaMaestra("IdTabla=30", dtv_TablaMaestra);
            for (int j = 0; j < tbl.Rows.Count; j++)
            {
                ListCombo.Add(new ClsCombobox()
                {
                    Id = Convert.ToInt32(tbl.Rows[j]["IdColumna"]),
                    Text = tbl.Rows[j]["Descripcion"].ToString()
                });
            }
            return ListCombo;
        }

        public PlanPriorizaMantenimiento()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        private void UserControl_Loaded()
        {
            try
            {
                objTablaMaestra.IdTabla = 0;
                dtv_TablaMaestra = B_TablaMaestra.TablaMaestra_Combo(objTablaMaestra).DefaultView;

                gtblPM = new DataTable();
                gtblPM.Columns.Add("IdPM", Type.GetType("System.Int32"));
                gtblPM.Columns.Add("PM", Type.GetType("System.String"));
                gtblPM.Columns.Add("Prioridad", Type.GetType("System.Int32"));
                gtblPM.Columns.Add("Frecuencia", Type.GetType("System.Double"));
                gtblPM.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                gtblPM.Columns.Add("cboFuentePrioridad", Type.GetType("System.Object"));
                gtblPM.Columns.Add("Nuevo", Type.GetType("System.Int32"));
                gtblPM.Columns.Add("FlagCambio", Type.GetType("System.Boolean"));

                tblpm = new DataTable();
                tblpm.Columns.Add("IdPM", Type.GetType("System.Int32"));
                tblpm.Columns.Add("Prioridad", Type.GetType("System.Int32"));
                tblpm.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                tblpm.Columns.Add("Nuevo", Type.GetType("System.Int32"));

                BtnAceptar.Content = Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPriorizaMantenimiento, "BTNG_GRAB");
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboPerfil_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                gtblPM.Rows.Clear();
                objPM.IdPerfil = Convert.ToInt32(cboPerfil.EditValue);
                DataTable tblPM = B_PM.PM_ListByPerfil(objPM);
                for (int i = 0; i < tblPM.Rows.Count; i++)
                {
                    DataRow fila = gtblPM.NewRow();
                    fila["IdPM"] = Convert.ToInt32(tblPM.Rows[i]["IdPM"]);
                    fila["PM"] = tblPM.Rows[i]["PM"];
                    fila["Prioridad"] = Convert.ToInt32(tblPM.Rows[i]["Prioridad"]);
                    fila["Frecuencia"] = Convert.ToDouble(tblPM.Rows[i]["Frecuencia"]);
                    fila["FlagActivo"] = true;
                    fila["cboFuentePrioridad"] = CargarcboPrioridad();
                    fila["Nuevo"] = 0;
                    fila["FlagCambio"] = false;
                    gtblPM.Rows.Add(fila);
                }
                tblPMTemp = gtblPM.Copy();
                gtblPM.DefaultView.Sort = "Prioridad Asc,Frecuencia Asc";
                dtgListadoPM.ItemsSource = gtblPM.DefaultView;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (DataRow drPM in gtblPM.Select("FlagCambio = true"))
                {
                    tblpm.Rows.Clear();
                    objPM.IdPM = Convert.ToInt32(drPM["IdPM"]);
                    DataTable tblPM_GetBeforeChange = B_PM.PM_GetBeforeChange(objPM);
                    if (Convert.ToInt32(tblPM_GetBeforeChange.Rows[0]["Contador"]) != 0)
                    {
                        GlobalClass.ip.Mensaje(String.Format(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPriorizaMantenimiento, "LOGI_PRIO"), drPM["PM"].ToString()), 2);
                        return;
                    }
                    DataRow dr = tblpm.NewRow();
                    dr["IdPM"] = Convert.ToInt32(drPM["IdPM"]);
                    dr["Prioridad"] = Convert.ToInt32(drPM["Prioridad"]);
                    dr["FlagActivo"] = true;
                    dr["Nuevo"] = 0;
                    tblpm.Rows.Add(dr);
                }
                
                objPM.IdPM = 0;
                objPM.IdUsuarioModificacion = gintusuario;
                if (dtgListadoPM.VisibleRowCount > 0)
                {
                    B_PM.PM_UpdateCascadePrioridad(objPM, tblpm);
                }
                gtblPM.Rows.Clear();
                tblPMTemp.Rows.Clear();
                cboPerfil.SelectedIndex = -1;
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaPriorizaMantenimiento, "GRAB_PLAN"), 1);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboPrioridad_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgListadoPM.ItemsSource = null;
                dtgListadoPM.ItemsSource = gtblPM.DefaultView;
                foreach (DataRow drPM in gtblPM.Select())
                {
                    int IdPM = Convert.ToInt32(drPM["IdPM"]);
                    foreach (DataRow drPMTemp in tblPMTemp.Select("IdPM = " + IdPM))
                    {
                        if (Convert.ToInt32(drPM["Prioridad"]) != Convert.ToInt32(drPMTemp["Prioridad"]))
                        {
                            drPM["FlagCambio"] = true;
                        }
                        else
                        {
                            drPM["FlagCambio"] = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboPerfil_PopupOpening(object sender, DevExpress.Xpf.Editors.OpenPopupEventArgs e)
        {
            try
            {
                cboPerfil.ItemsSource = null;
                cboPerfil.ItemsSource = B_perfil.Perfil_ComboWithPM();
                cboPerfil.DisplayMember = "Perfil";
                cboPerfil.ValueMember = "IdPerfil";
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
    }
}
