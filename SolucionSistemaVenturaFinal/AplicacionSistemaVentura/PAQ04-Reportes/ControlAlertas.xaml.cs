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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Bars;
using Entities;
using Business;

namespace AplicacionSistemaVentura.PAQ04_Reportes
{
    /// <summary>
    /// Interaction logic for ControlAlertas.xaml
    /// </summary>
    public partial class ControlAlertas : UserControl
    {
        E_Alertas objE_Alertas = new E_Alertas();
        B_Alertas objB_Alertas = new B_Alertas();

        DataView dtvAlertas = new DataView();
        DataTable tblTitulosAlertas = new DataTable();
        Utilitarios.ErrorHandler Error = new Utilitarios.ErrorHandler();



        public ControlAlertas()
        {
            InitializeComponent();
            UserControl_Loaded();
        }

        private void UserControl_Loaded()
        {
            try
            {
                GlobalClass.FrmAlerta = this;
                //ds = Utilitarios.Utilitarios.ListarAlertas();

                //tblTitulosAlertas.Columns.Add("IdAlerta");
                //tblTitulosAlertas.Columns.Add("Tipo");
                //tblTitulosAlertas.Columns.Add("IdEstado");
                //tblTitulosAlertas.Columns.Add("FechaEnvio");
                //tblTitulosAlertas.Columns.Add("FlagActivo");
                InterfazPrincipal frm = new InterfazPrincipal();

                ListarAlertas();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public void ListarAlertas()
        {
            objE_Alertas.IdUsuario = Utilitarios.Utilitarios.gintIdUsuario;
            GlobalClass.tblAlertasAPP = objB_Alertas.Alertas_GetItems(objE_Alertas);
            dtvAlertas = GlobalClass.tblAlertasAPP.DefaultView;
            dtvAlertas.RowFilter = "FlagActivo = 1";
            dtgAlertas.ItemsSource = dtvAlertas;
        }

        private byte[] ConvertDataSetToByteArray(DataTable tbldetalles)
        {
            byte[] binaryDataResult = null;
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, tbldetalles);
            binaryDataResult = stream.GetBuffer();
            return binaryDataResult;
        }

        private void dtgAlertas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int rowHandle = viewAlertas.GetRowHandleByMouseEventArgs(e);
                if (rowHandle == GridControl.InvalidRowHandle) return;
                dtgAlertas.Height = 200;
                dtgAlertas.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                dtgAlertaDet.Visibility = Visibility.Visible;

                byte[] AlertasAPP = (byte[])dtgAlertas.GetFocusedRowCellValue("DetalleAlerta");
                DataSet tblDetalleAlertas = Utilitarios.Utilitarios.ConvertByteArrayToDataTable(AlertasAPP);
                dtgAlertaDet.ItemsSource = tblDetalleAlertas.Tables[0];

                dtgAlertaDet.Columns["NombreAlerta"].Visible = false;

                if (Convert.ToInt32(dtgAlertas.GetFocusedRowCellValue("FlagLeido")) != 1)
                {
                    foreach (DataRow Fila in GlobalClass.tblAlertasAPP.Select("IdAlerta = " + Convert.ToInt32(dtgAlertas.GetFocusedRowCellValue("IdAlerta"))))
                    {
                        Fila["FlagLeido"] = 1;
                    }
                    ActualizarAlertas();
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void viewAlertas_ShowGridMenu(object sender, DevExpress.Xpf.Grid.GridMenuEventArgs e)
        {
            try
            {
                if (e.MenuType != GridMenuType.RowCell) return;
                e.Customizations.Add(new RemoveBarItemAndLinkAction()
                {
                    ItemName = DefaultColumnMenuItemNames.ColumnChooser
                });
                BarButtonItem bi = new BarButtonItem();
                bi.Name = "menuBorrarItem";
                bi.Content = "Eliminar Mensaje";
                bi.ItemClick += new ItemClickEventHandler(customItem_ItemClick);
                e.Customizations.Add(bi);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }
        private void customItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var rpt = DevExpress.Xpf.Core.DXMessageBox.Show("¿Está seguro que desea eliminarlo?", "Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (rpt == MessageBoxResult.Yes)
                {
                    foreach (DataRow Fila in GlobalClass.tblAlertasAPP.Select("IdAlerta = " + Convert.ToInt32(dtgAlertas.GetFocusedRowCellValue("IdAlerta"))))
                    {
                        Fila["FlagActivo"] = 0;
                    }
                    ActualizarAlertas();
                    dtgAlertaDet.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        public void ActualizarAlertas()
        {
            try
            {
                DataTable tblAlertasApp = ((DataView)dtgAlertas.ItemsSource).ToTable();
                DataColumn nuevo = new DataColumn()
                {
                    ColumnName = "Nuevo",
                    DefaultValue = false
                };
                tblAlertasApp.Columns.Add(nuevo);

                int rpta = objB_Alertas.Alertas_UpdateCascade(objE_Alertas, tblAlertasApp);
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                Error.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

    }
}
