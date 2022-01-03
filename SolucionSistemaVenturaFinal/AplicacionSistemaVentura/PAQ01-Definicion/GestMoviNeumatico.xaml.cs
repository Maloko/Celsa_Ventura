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
using Entities;
using Business;
using System.Collections.ObjectModel;
using System.Collections;
using InterfazMTTO;
using Utilitarios;
using System.Windows.Threading;
using System.Threading;

namespace AplicacionSistemaVentura.PAQ01_Definicion
{
    /// <summary>
    /// Interaction logic for GestMoviNeumatico.xaml
    /// </summary>
    public partial class GestMoviNeumatico : UserControl
    {
        ObservableCollection<string> ArraySeries = new ObservableCollection<string>();
        
        public GestMoviNeumatico()
        {
            InitializeComponent();
            UserControl_Loaded();
        }
        string strProcedencia;
        string gstrNroSerieDrop;
        string gstrNroSerieDrag;
        string gstrMovimiento=string.Empty ;
        int gintCantMovi = 1;
        //int gintIdNeumaticoDBaja = 0;
        int gintIdNeumaticoTransferDet = 0;
        int gintIdUcDestinoPrevio = -1;
        int gintIdUcOrigenPrevio = -1;
        int RespuestaMensaje = 0;
        int gintIdAlmacen;
        int gintIdTrash;
        bool Detener = false;
        string gstrEtiquetaMovimientoNeumatico = "GestMoviNeumatico";

        ErrorHandler objError = new ErrorHandler();
                
        Image imgDrag = new Image();
        Image imgDrop = new Image();
        DataTable gtblNeumaticoTransferDet = new DataTable();
        DataTable gtblDesecho = new DataTable();
        DataTable gtblcboDestino = new DataTable();
        DataTable gtblUcCombo = new DataTable();
        DataTable gtblListSerie = new DataTable();
        DataTable gtblNeimaticoCicloList = new DataTable();
        int gintNuevaSerieAgregada = -1;

        DataTable TempEstrucOrigen = new DataTable();
        DataTable TempEstrucDestino = new DataTable();
        DataTable gtblPosicionesOrigen = new DataTable();
        DataTable gtblPosicionesDestino = new DataTable();
        DataTable gtblPosicionesClient;

        E_UC objE_UC = new E_UC();
        B_UC objB_UC = new B_UC();
        B_Neumatico objB_Neumatico = new B_Neumatico();
        E_Neumatico objE_Neumatico = new E_Neumatico();
        E_Neumatico_Transfer objE_Neumatico_Transfer = new E_Neumatico_Transfer();
        B_Neumatico_Transfer objB_Neumatico_Transfer = new B_Neumatico_Transfer();
        E_PerfilNeumatico objE_PerfilNeumatico = new E_PerfilNeumatico();
        B_PerfilNeumatico objB_PerfilNeumatico = new B_PerfilNeumatico();
        E_PerfilNeumaticoEje objE_PerfilNeumaticoEje = new E_PerfilNeumaticoEje();
        E_Neumatico_Ciclo objE_NeumaticoCiclo = new E_Neumatico_Ciclo();
        B_Neumatico_Ciclo objB_NeumaticoCiclo = new B_Neumatico_Ciclo();
        E_TablaMaestra objE_TablaMaestra = new E_TablaMaestra();
        
        InterfazMTTO.iSBO_BE.BEUDUC UDUC = new InterfazMTTO.iSBO_BE.BEUDUC();
        InterfazMTTO.iSBO_BE.BETUDUCList tucuclist = new InterfazMTTO.iSBO_BE.BETUDUCList();
        InterfazMTTO.iSBO_BE.BERPTA RPTA = new InterfazMTTO.iSBO_BE.BERPTA();
        InterfazMTTO.iSBO_BE.BEOITMList BEOITMList = new InterfazMTTO.iSBO_BE.BEOITMList();

        BitmapImage imgLlantaNormal = new BitmapImage(new Uri("pack://application:,,,/Image/camion-llanta.png"));
        BitmapImage imgLlantaRepuesto = new BitmapImage(new Uri("pack://application:,,,/Image/LlantaRepuesto.png"));
        BitmapImage imgLlantaNormal_disable = new BitmapImage(new Uri("pack://application:,,,/Image/camion-llanta-disable.png"));
        BitmapImage imgLlantaRepuesto_disable = new BitmapImage(new Uri("pack://application:,,,/Image/LlantaRepuesto-disable.png"));
        BitmapImage imgLlantaNormal_enable = new BitmapImage(new Uri("pack://application:,,,/Image/camion-llanta-enable.png"));
        BitmapImage imgLlantaRepuesto_enable = new BitmapImage(new Uri("pack://application:,,,/Image/LlantaRepuesto-enable.png"));
        BitmapImage imgEje_blank = new BitmapImage(new Uri("pack://application:,,,/Image/camion-eje-blank.png"));
        BitmapImage imgEje = new BitmapImage(new Uri("pack://application:,,,/Image/camion-eje.png"));
        BitmapImage imgTrash = new BitmapImage(new Uri("pack://application:,,,/Image/Trash.png"));
        BitmapImage imgTrash_Full = new BitmapImage(new Uri("pack://application:,,,/Image/Trash_Full.png"));

        int gintIdMenu = 0;
        DateTime FechaModificacion;

        private void UserControl_Loaded()
        {
            try
            {
                cboUCOrigen.SelectedIndexChanged -= new RoutedEventHandler(cboUCOrigen_SelectedIndexChanged);
                cboUCDestino.SelectedIndexChanged -= new RoutedEventHandler(cboUCDestino_SelectedIndexChanged);
                FechaModificacion = Utilitarios.Utilitarios.FechaHora_Servidor();
                //gtblNeumaticoTransferDet.Columns.Clear();
                gtblNeumaticoTransferDet.Columns.Add("IdNeumaticoTransferDet", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdNeumaticoTransfer", Type.GetType("System.Int32"));                
                gtblNeumaticoTransferDet.Columns.Add("IdNeumatico", Type.GetType("System.Int32"));                
                gtblNeumaticoTransferDet.Columns.Add("IdAlmacenSalida", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdUCSalida", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdEjeSalida", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdPosicionSalida", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdPerfilNeumaticoSalida", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdPosicionSalidaCliente", Type.GetType("System.Int32"));

                gtblNeumaticoTransferDet.Columns.Add("IdAlmacenEntrada", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdUCEntrada", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdEjeEntrada", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdPosicionEntrada", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdPerfilNeumaticoEntrada", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("IdPosicionEntradaCliente", Type.GetType("System.Int32"));

                gtblNeumaticoTransferDet.Columns.Add("FechaMovi", Type.GetType("System.String"));
                gtblNeumaticoTransferDet.Columns.Add("Movimiento", Type.GetType("System.String"));
                gtblNeumaticoTransferDet.Columns.Add("Observacion", Type.GetType("System.String"));
                gtblNeumaticoTransferDet.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                gtblNeumaticoTransferDet.Columns.Add("Nuevo", Type.GetType("System.Boolean"));
                                
                gtblNeumaticoTransferDet.Columns.Add("CantMovimiento", Type.GetType("System.Int32"));
                gtblNeumaticoTransferDet.Columns.Add("NroSerie", Type.GetType("System.String"));
                gtblNeumaticoTransferDet.Columns.Add("OrigenNombre", Type.GetType("System.String"));
                gtblNeumaticoTransferDet.Columns.Add("DestinoNombre", Type.GetType("System.String"));
                gtblDesecho.Columns.Clear();
                gtblDesecho.Columns.Add("IdNeumatico", Type.GetType("System.Int32"));
                gtblDesecho.Columns.Add("FechaBaja", Type.GetType("System.String"));
                gtblDesecho.Columns.Add("IdEstado", Type.GetType("System.Int32"));                
                gtblDesecho.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                gtblDesecho.Columns.Add("Nuevo", Type.GetType("System.Boolean"));


                gtblListSerie.Columns.Clear();
                gtblListSerie.Columns.Add("IdNeumatico", Type.GetType("System.Int32"));
                gtblListSerie.Columns.Add("NroSerie", Type.GetType("System.String"));
                gtblListSerie.Columns.Add("FlagActivo", Type.GetType("System.Boolean"));
                gtblListSerie.Columns.Add("DeAlmacen", Type.GetType("System.Int32"));
                CargarListSeries();

                objE_UC.IdPerfil = 0;
                gtblUcCombo = objB_UC.B_UC_ComboWithPN(objE_UC);
                cboUCOrigen.ItemsSource = objB_UC.B_UC_ComboWithPN(objE_UC);
                cboUCOrigen.DisplayMember = "CodUC";
                cboUCOrigen.ValueMember = "IdUC";

                cboUCDestino.ItemsSource = objB_UC.B_UC_ComboWithPN(objE_UC);
                cboUCDestino.DisplayMember = "CodUC";
                cboUCDestino.ValueMember = "IdUC";
                cboUCDestino.IsEnabled = false;

                objE_NeumaticoCiclo.IdNeumatico = 0;
                gtblNeimaticoCicloList = objB_NeumaticoCiclo.NeumaticoCiclo_List(objE_NeumaticoCiclo);

                objE_TablaMaestra.IdTabla = 46;
                DataTable tblAlmacenes = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
                if (tblAlmacenes.Rows.Count > 1)
                {
                    gintIdAlmacen = Convert.ToInt32(tblAlmacenes.Rows[0]["Valor"]);
                    gintIdTrash = Convert.ToInt32(tblAlmacenes.Rows[1]["Valor"]);
                }

                
                cboUCOrigen.SelectedIndexChanged += new RoutedEventHandler(cboUCOrigen_SelectedIndexChanged);
                cboUCDestino.SelectedIndexChanged += new RoutedEventHandler(cboUCDestino_SelectedIndexChanged);

                lstSerie.AllowDrop = true;
                DeshabilitarImagenes("O");
                DeshabilitarImagenes("D");

                #region VisualizacionBotonImprimir
                bool VisualizaBotonImprimirDetalle = GlobalClass.ExisteFormatoImpresion(this.GetType().Name, ref gintIdMenu);
                if (!VisualizaBotonImprimirDetalle)
                {
                    btnImprimir.Visibility = System.Windows.Visibility.Hidden;
                }
                #endregion
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void lstSerie_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox ListOrigen = (ListBox)sender;
            object data = GetDataFromListBox(ListOrigen, e.GetPosition(ListOrigen));
            strProcedencia = "L";
            if (data != null)
            {
                lstSerie.AllowDrop = false;
                imgEliminar.AllowDrop = true;
                DragDrop.DoDragDrop(ListOrigen, data, DragDropEffects.Move);
                lstSerie.AllowDrop = true;
                imgEliminar.AllowDrop = false;
            }
        }
        
        private void AddTransferenciaData(int IdNeumatico,string NroSerie, int IdAlmacenSalida, int IdUCSalida, int IdEjeSalida, int IdPosicionSalida,
            int IdAlmacenEntrada, int IdUCEntrada, int IdEjeEntrada, int IdPosicionEntrada,string Movimiento)
        {
            try
            {
                int IdPerfilNeumaticoSalida = 0, IdPerfilNeumaticoEntrada = 0;
                if (IdUCSalida != 0)
                    IdPerfilNeumaticoSalida = Convert.ToInt32(gtblUcCombo.Select("IdUC =" + IdUCSalida)[0]["IdPerfilNeumatico"]);
                if (IdUCEntrada != 0)
                    IdPerfilNeumaticoEntrada = Convert.ToInt32(gtblUcCombo.Select("IdUC =" + IdUCEntrada)[0]["IdPerfilNeumatico"]);

                if ((bool)rdReemplazar.IsChecked && IdAlmacenEntrada == gintIdAlmacen)//En caso se Reemplazar
                    Movimiento = "Retiro";
                else if (IdAlmacenEntrada != gintIdTrash)
                    Movimiento = gstrMovimiento;
                
                gintIdNeumaticoTransferDet++;
                DataRow rowsNeumaticoTransferDet = gtblNeumaticoTransferDet.NewRow();
                rowsNeumaticoTransferDet["IdNeumaticoTransferDet"] = gintIdNeumaticoTransferDet;
                rowsNeumaticoTransferDet["IdNeumaticoTransfer"] = 0;
                rowsNeumaticoTransferDet["IdNeumatico"] = IdNeumatico;

                rowsNeumaticoTransferDet["IdAlmacenSalida"] = IdAlmacenSalida;
                rowsNeumaticoTransferDet["IdUCSalida"] = IdUCSalida;
                rowsNeumaticoTransferDet["IdEjeSalida"] = IdEjeSalida;  //Dependiendo
                rowsNeumaticoTransferDet["IdPosicionSalida"] = IdPosicionSalida;
                rowsNeumaticoTransferDet["IdPerfilNeumaticoSalida"] = IdPerfilNeumaticoSalida;
                rowsNeumaticoTransferDet["IdPosicionSalidaCliente"] = 0;
                rowsNeumaticoTransferDet["IdAlmacenEntrada"] = IdAlmacenEntrada;
                rowsNeumaticoTransferDet["IdUCEntrada"] = IdUCEntrada;
                rowsNeumaticoTransferDet["IdEjeEntrada"] = IdEjeEntrada;//Dependiendo
                rowsNeumaticoTransferDet["IdPosicionEntrada"] = IdPosicionEntrada;
                rowsNeumaticoTransferDet["IdPerfilNeumaticoEntrada"] = IdPerfilNeumaticoEntrada;
                rowsNeumaticoTransferDet["IdPosicionEntradaCliente"] = 0;
                rowsNeumaticoTransferDet["FlagActivo"] = true;
                rowsNeumaticoTransferDet["Nuevo"] = true;
                
                rowsNeumaticoTransferDet["CantMovimiento"] = gintCantMovi;
                rowsNeumaticoTransferDet["NroSerie"] = NroSerie;
                rowsNeumaticoTransferDet["OrigenNombre"] = imgDrag.Name;
                rowsNeumaticoTransferDet["DestinoNombre"] = imgDrop.Name;
                rowsNeumaticoTransferDet["FechaMovi"] = Convert.ToDateTime(mskFecha.EditValue).ToString("yyyyMMdd HH:mm");
                rowsNeumaticoTransferDet["Observacion"] = txtObservacion.Text;
                rowsNeumaticoTransferDet["Movimiento"] = Movimiento;
                gtblNeumaticoTransferDet.Rows.Add(rowsNeumaticoTransferDet);

                MensajeBitacora(NroSerie, imgDrag.Name, imgDrop.Name,
                    IdAlmacenSalida, IdUCSalida, IdEjeSalida, IdPosicionSalida,
                    IdAlmacenEntrada, IdUCEntrada, IdEjeEntrada, IdPosicionEntrada,
                    Movimiento);
                
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void MensajeBitacora(string NroSerie,string OrigenNombre,string DestinoNombre,int IdAlmacenOrigen,int IdUCOrigen, int IdEjeOrigen,int IdPosicionOrigen, int IdAlmacenDestino,int IdUCDestino, int IdEjeDestino,int IdPosicionDestino,string Movimiento)
        {

            string Desde = string.Empty, Hacia = string.Empty, UCOrigen = string.Empty, UCDestino = string.Empty, EjeOrigen = string.Empty, EjeDestino = string.Empty;
            
            if (IdUCOrigen != 0)
                UCOrigen = gtblUcCombo.Select("IdUC =" + IdUCOrigen.ToString())[0]["CodUC"].ToString();
            if (IdUCDestino != 0)
                UCDestino = gtblUcCombo.Select("IdUC =" + IdUCDestino.ToString())[0]["CodUC"].ToString();

            EjeOrigen = (IdEjeOrigen == 99) ? "REP" : "E" + Utilitarios.Utilitarios.NumeroChar2(IdEjeOrigen);
            EjeDestino = (IdEjeDestino == 99) ? "REP" : "E" + Utilitarios.Utilitarios.NumeroChar2(IdEjeDestino);

            if (IdAlmacenDestino == gintIdTrash)
            {
                Desde = "Almacén";
                Hacia = "Papelera";
            }
            else if (IdAlmacenOrigen == 0 && IdAlmacenDestino == 0)
            {
                Desde = UCOrigen + " Eje: " + EjeOrigen + " Pos.: " + GetPosicionCliente(IdEjeOrigen , IdPosicionOrigen,OrigenNombre.Substring(3,1)); 
                Hacia = UCDestino + " Eje: " + EjeDestino + " Pos.: " + GetPosicionCliente(IdEjeDestino, IdPosicionDestino, DestinoNombre.Substring(3, 1));
            }
            else if (IdAlmacenOrigen == gintIdAlmacen && IdAlmacenDestino == 0)
            {
                Desde = "Almacén";
                Hacia = UCDestino + " Eje: " + EjeDestino + " Pos.: " + GetPosicionCliente( IdEjeDestino,IdPosicionDestino, DestinoNombre.Substring(3, 1));
            }
            else if (IdAlmacenOrigen == 0 && IdAlmacenDestino == gintIdAlmacen)
            {
                Desde = UCOrigen + " Eje: " + EjeOrigen + " Pos.: " + GetPosicionCliente( IdEjeOrigen, IdPosicionOrigen, OrigenNombre.Substring(3, 1));
                Hacia = "Almacén";
            }
            TextBlock tb = new TextBlock();
            tb.Inlines.Add(Movimiento);//Falta Estador
            tb.Inlines.Add(new Run(" | ") { FontWeight = FontWeights.Bold });
            tb.Inlines.Add(NroSerie);
            tb.Inlines.Add(new Run(" | ") { FontWeight = FontWeights.Bold });
            tb.Inlines.Add(Desde);
            tb.Inlines.Add(new Run(" | ") { FontWeight = FontWeights.Bold });
            tb.Inlines.Add(Hacia);
            lstMovi.Items.Add(tb);
            lstMovi.ScrollIntoView(lstMovi.Items[lstMovi.Items.Count - 1]);
        }
        
        private void Imagen_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                imgDrag = (Image)sender;
                strProcedencia = imgDrag.Name.Substring(3, 1).ToString();
                imgDrag.AllowDrop = false;
                DragDrop.DoDragDrop(imgDrag, "NOLIST", DragDropEffects.Move);
                imgDrag.AllowDrop = true;

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void lstSerie_Drop(object sender, DragEventArgs e)
        {
            if (imgDrag.Source != imgLlantaNormal && imgDrag.Source != imgLlantaRepuesto) return;
            if (MensajeEmergente_Respuesta(false,true)== 0) return;
            
            gintCantMovi = 1;
            gstrMovimiento = "Retiro";
            int IdEjeDrag = 0;
            if (imgDrag.Source == imgLlantaRepuesto || imgDrag.Source == imgLlantaRepuesto_enable)
                IdEjeDrag = 99;
            else
                IdEjeDrag = Convert.ToInt32(imgDrag.Name.Substring(5, 2));
            int IdPosicionDrag = Convert.ToInt32(imgDrag.Name.Substring(8, 2));
            int IdNeumaticoDrag = Convert.ToInt32(imgDrag.Tag.ToString().Split('|')[0]);
            gstrNroSerieDrag = imgDrag.Tag.ToString().Split('|')[1];

            int IdUCDrag = 0;
            if (imgDrag.Name.Substring(3, 1) == "O")
                IdUCDrag = Convert.ToInt32(cboUCOrigen.EditValue);
            else if (imgDrag.Name.Substring(3, 1) == "D")
                IdUCDrag = Convert.ToInt32(cboUCDestino.EditValue);

            if (strProcedencia == "L") return;
            AddTransferenciaData(IdNeumaticoDrag, gstrNroSerieDrag,
                    0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                    gintIdAlmacen, 0, 0, 0,
                    gstrMovimiento);

            if (gtblListSerie.Select("NroSerie = " + "'" + gstrNroSerieDrag + "'").Length == 1)
            {
                for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                {
                    if (IdNeumaticoDrag == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                    {
                        gtblListSerie.Rows[i]["FlagActivo"] = true;
                        gintNuevaSerieAgregada = i;
                    }
                }
            }
            else
            {
                DataRow F = gtblListSerie.NewRow();
                F["IdNeumatico"] = IdNeumaticoDrag;
                F["NroSerie"] = gstrNroSerieDrag;
                F["FlagActivo"] = true;
                gintNuevaSerieAgregada = gtblListSerie.Rows.Count;
                gtblListSerie.Rows.Add(F);
            }
            ActualizarListaSeries();
            LimpiarImagenDrag();
            LimpiarMensajeEmegente();
        }
        
        private void ActualizarListaSeries()
        {
            ArraySeries.Clear();
            gtblListSerie.DefaultView.RowFilter = "FlagActivo = true";
            for (int i = 0; i < gtblListSerie.DefaultView.Count; i++)
            {
                ArraySeries.Add(gtblListSerie.DefaultView[i]["NroSerie"].ToString());
            }
            lstSerie.ItemsSource = ArraySeries;
            lstSerie.SelectedIndex = gintNuevaSerieAgregada;
            lblCantidad.Content = lstSerie.Items.Count;
        }

        private void Imagenes_Drop(object sender, DragEventArgs e)
        {
            if (strProcedencia == "L")
                strProcedencia = "L";
            else if (imgDrag.Source != imgLlantaNormal && imgDrag.Source != imgLlantaRepuesto) return;
            //if ((imgDrag.Source != imgLlantaNormal || imgDrag.Source != imgLlantaRepuesto) && strProcedencia != "L") { return; }
            gintCantMovi = 1;
            string NroSerieList = e.Data.GetData(typeof(string)).ToString();
            gstrNroSerieDrag = NroSerieList;
            imgDrop = (sender) as Image;

            int IdEjeDrop = 0;
            if (imgDrop.Source == imgLlantaRepuesto || imgDrop.Source == imgLlantaRepuesto_enable)
                IdEjeDrop = 99;
            else
                IdEjeDrop = Convert.ToInt32(imgDrop.Name.Substring(5, 2));
            int IdPosicionDrop = Convert.ToInt32(imgDrop.Name.Substring(8, 2));

            int IdUCDrop = 0;
            if (imgDrop.Name.Substring(3, 1) == "O")
                IdUCDrop = Convert.ToInt32(cboUCOrigen.EditValue);
            else if (imgDrop.Name.Substring(3, 1) == "D")
                IdUCDrop = Convert.ToInt32(cboUCDestino.EditValue);

            
            if (strProcedencia == "L")
            {
                gintCantMovi = 1;
                gstrMovimiento = "Asignación";
                int IdNeumaticoList = 0;
                                
                if (imgDrop.Source == imgLlantaNormal_enable || imgDrop.Source == imgLlantaRepuesto_enable)
                {
                    if (MensajeEmergente_Respuesta(false, false) == 0) return;
                }
                else if (imgDrop.Source == imgLlantaNormal || imgDrop.Source == imgLlantaRepuesto)
                {
                    if (MensajeEmergente_Respuesta(false, true) == 0) return;
                    gintCantMovi = 2;
                    //++++++++++Copiando Esto para Rotar en los neumaticos
                    int IdNeumaticoDrop = Convert.ToInt32(imgDrop.Tag.ToString().Split('|')[0]);
                    gstrNroSerieDrop = imgDrop.Tag.ToString().Split('|')[1];
                    
                    if (gtblListSerie.Select("NroSerie = " + "'" + gstrNroSerieDrop + "'").Length == 1)
                    {
                        for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                        {
                            if (IdNeumaticoDrop == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                            {
                                gtblListSerie.Rows[i]["FlagActivo"] = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        DataRow F = gtblListSerie.NewRow();
                        F["IdNeumatico"] = IdNeumaticoDrop;
                        F["NroSerie"] = gstrNroSerieDrop;
                        F["FlagActivo"] = true;
                        gtblListSerie.Rows.Add(F);
                    }
                    ActualizarListaSeries();
                    //++++++++++
                    imgDrag.Name = imgDrop.Name;
                    AddTransferenciaData(IdNeumaticoDrop, gstrNroSerieDrop,
                        0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                        gintIdAlmacen, 0, 0, 0,
                        gstrMovimiento);
                }

                for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                {
                    if (NroSerieList == gtblListSerie.Rows[i]["NroSerie"].ToString())
                    {
                        IdNeumaticoList = Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]);
                        if ("1" == Utilitarios.Utilitarios.IIfBlankZero(gtblListSerie.Rows[i]["DeAlmacen"].ToString()))
                            gtblListSerie.Rows[i]["FlagActivo"] = false;
                        else
                            gtblListSerie.Rows.RemoveAt(i);
                        break;
                    }
                }
                ActualizarListaSeries();

                AddTransferenciaData(IdNeumaticoList, NroSerieList,
                        gintIdAlmacen, 0, 0, 0,
                        0, IdUCDrop, IdEjeDrop, IdPosicionDrop, gstrMovimiento);

                imgDrop.Tag = IdNeumaticoList + "|" + NroSerieList;
                imgDrop.ToolTip = MensajeToolTip(imgDrop.Tag.ToString());

                if (imgDrop.Name.Substring(4, 1) == "E")
                    imgDrop.Source = imgLlantaNormal;
                else if (imgDrop.Name.Substring(4, 1) == "L")
                    imgDrop.Source = imgLlantaRepuesto;

            }
            if (strProcedencia == "L")
            {
                LimpiarMensajeEmegente();
                return;
            }

            int IdEjeDrag = 0, IdPosicionDrag = 0, IdNeumaticoDrag = 0, IdUCDrag = 0;
            if (NroSerieList == "NOLIST")
            {
                if (imgDrag.Source == imgLlantaRepuesto || imgDrag.Source == imgLlantaRepuesto_enable)
                    IdEjeDrag = 99;
                else
                    IdEjeDrag = Convert.ToInt32(imgDrag.Name.Substring(5, 2));
                IdPosicionDrag = Convert.ToInt32(imgDrag.Name.Substring(8, 2));
                IdNeumaticoDrag = Convert.ToInt32(imgDrag.Tag.ToString().Split('|')[0]);
                gstrNroSerieDrag = imgDrag.Tag.ToString().Split('|')[1];

                if (imgDrag.Name.Substring(3, 1) == "O")
                    IdUCDrag = Convert.ToInt32(cboUCOrigen.EditValue);
                else if (imgDrag.Name.Substring(3, 1) == "D")
                    IdUCDrag = Convert.ToInt32(cboUCDestino.EditValue);
            }
            
            if (imgDrop.Source == imgLlantaNormal_enable || imgDrop.Source == imgLlantaRepuesto_enable)
            {
                if (strProcedencia == imgDrop.Name.Substring(3, 1))//si el del midmo UC
                    gstrMovimiento = "Rotación";
                else
                    gstrMovimiento = "Traslado";

                //De: Llanta Normal Origen o Llanta Repuesto Origen 
                if (strProcedencia == "O")
                {
                    #region
                    //Hacia: Llanta Normal Origen o Llanta Respuesto Origen
                    if (imgDrop.Name.Substring(3, 1) == "O")
                    {
                        if (MensajeEmergente_Respuesta(false, false) == 0) return;

                        AddTransferenciaData(IdNeumaticoDrag, gstrNroSerieDrag,
                            0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                            0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                            gstrMovimiento);

                    }
                    //Hacia: Llanta Normal Destino o Llanta Repuesto Destino
                    else if (imgDrop.Name.Substring(3, 1) == "D")
                    {
                        if (MensajeEmergente_Respuesta(false,true) == 0) return;

                        AddTransferenciaData(IdNeumaticoDrag, gstrNroSerieDrag,
                            0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                            0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                            gstrMovimiento);
                    }

                    #endregion
                }
                //De: Llanta Normal Destino o llanta repuesto destino
                else if (strProcedencia == "D")
                {
                    #region
                    //Hacia: Llanta Normal Origen o Llanta Respuesto Origen
                    if (imgDrop.Name.Substring(3, 1) =="O")
                    {
                        if (MensajeEmergente_Respuesta(false,true) == 0) return;

                        AddTransferenciaData(IdNeumaticoDrag, gstrNroSerieDrag,
                            0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                            0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                            gstrMovimiento);
                    }
                    //Hacia: Llanta Normal Destino
                    else if (imgDrop.Name.Substring(3, 1) == "D")
                    {
                        if (MensajeEmergente_Respuesta(false, false) == 0) return;

                        AddTransferenciaData(IdNeumaticoDrag, gstrNroSerieDrag,
                            0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                            0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                            gstrMovimiento);
                    }

                    #endregion
                }

                imgDrop.Tag = imgDrag.Tag;
                imgDrop.ToolTip = MensajeToolTip(imgDrop.Tag.ToString());
                if (imgDrop.Name.Substring(4, 1) == "E")
                    imgDrop.Source = imgLlantaNormal;
                else if (imgDrop.Name.Substring(4, 1) == "L")
                    imgDrop.Source = imgLlantaRepuesto;
                LimpiarImagenDrag();                    
                

            }
            //intercambio de neumaticos
            else
            {              
                gintCantMovi = 2;
                int IdNeumaticoDrop = Convert.ToInt32(imgDrop.Tag.ToString().Split('|')[0]);
                gstrNroSerieDrop = imgDrop.Tag.ToString().Split('|')[1];

                object etiquetaDrag = null;
                if (strProcedencia == imgDrop.Name.Substring(3, 1))
                    gstrMovimiento = "Rotación";
                else
                    gstrMovimiento = "Traslado";
                
                //De: Llanta Normal Origen o llanta normal repuesto
                if (strProcedencia == "O")
                {
                    #region 
                    //Hacia: Llanta Normal Origen o Llanta Respuesto Origen
                    if (imgDrop.Name.Substring(3, 1) =="O")
                    {
                        if (MensajeEmergente_Respuesta(true,true) == 0) return;
                                                
                        AddTransferenciaData(IdNeumaticoDrop, gstrNroSerieDrop,
                            0,IdUCDrop, IdEjeDrop, IdPosicionDrop,
                            gintIdAlmacen, 0, 0, 0,
                        gstrMovimiento);

                        AddTransferenciaData(IdNeumaticoDrag, gstrNroSerieDrag,
                            0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                            0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                        gstrMovimiento);

                        if ((bool)rdRotar.IsChecked)
                        {
                            gintCantMovi = 3;
                            AddTransferenciaData(IdNeumaticoDrop, gstrNroSerieDrop,
                                gintIdAlmacen, 0, 0, 0,
                                0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                        gstrMovimiento);

                            etiquetaDrag = imgDrag.Tag;
                            imgDrag.Tag = imgDrop.Tag;
                            imgDrag.ToolTip = MensajeToolTip(imgDrag.Tag.ToString());
                            imgDrop.Tag = etiquetaDrag;
                            imgDrop.ToolTip = MensajeToolTip(imgDrop.Tag.ToString());
                        }
                        else
                        {
                            #region Agregando a Lista Series

                            if (gtblListSerie.Select("NroSerie = " + "'" + gstrNroSerieDrop + "'").Length == 1)
                            {
                                for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                                {
                                    if (IdNeumaticoDrop == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                                    {
                                        gtblListSerie.Rows[i]["FlagActivo"] = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                DataRow F = gtblListSerie.NewRow();
                                F["IdNeumatico"] = IdNeumaticoDrop;
                                F["NroSerie"] = gstrNroSerieDrop;
                                F["FlagActivo"] = true;
                                gtblListSerie.Rows.Add(F);
                            }
                            ActualizarListaSeries();
                            #endregion

                            imgDrop.Tag = imgDrag.Tag;
                            imgDrop.ToolTip = MensajeToolTip(imgDrop.Tag.ToString());
                            LimpiarImagenDrag();
                        }
                        
                    }
                    //Hacia:Llanta Normal o Llanta Repuesto Destino
                    else if (imgDrop.Name.Substring(3, 1) == "D")
                    {
                        if (MensajeEmergente_Respuesta(true,true) == 0) return;

                        AddTransferenciaData(IdNeumaticoDrop, gstrNroSerieDrop,
                            0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                            gintIdAlmacen, 0, 0, 0,
                        gstrMovimiento);

                        AddTransferenciaData(
                            IdNeumaticoDrag, gstrNroSerieDrag,
                            0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                            0,IdUCDrop, IdEjeDrop, IdPosicionDrop,
                        gstrMovimiento);

                        if ((bool)rdRotar.IsChecked)
                        {
                            gintCantMovi = 3;
                            AddTransferenciaData(IdNeumaticoDrop, gstrNroSerieDrop,
                                gintIdAlmacen, 0, 0, 0,
                                0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                        gstrMovimiento);

                            etiquetaDrag = imgDrag.Tag;
                            imgDrag.Tag = imgDrop.Tag;
                            imgDrag.ToolTip = MensajeToolTip(imgDrag.Tag.ToString());
                            imgDrop.Tag = etiquetaDrag;
                            imgDrop.ToolTip = MensajeToolTip(imgDrop.Tag.ToString());
                        }
                        else
                        {
                            #region Agregando a Lista Series

                            //int IdUCDrop = 0;
                            //if (imgDrop.Name.Substring(3, 1) == "O")
                            //    IdUCDrop = Convert.ToInt32(cboUCOrigen.EditValue);
                            //else if (imgDrop.Name.Substring(3, 1) == "D")
                            //    IdUCDrop = Convert.ToInt32(cboUCDestino.EditValue);

                            if (gtblListSerie.Select("NroSerie = " + "'" + gstrNroSerieDrop + "'").Length == 1)
                            {
                                for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                                {
                                    if (IdNeumaticoDrop == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                                    {
                                        gtblListSerie.Rows[i]["FlagActivo"] = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                DataRow F = gtblListSerie.NewRow();
                                F["IdNeumatico"] = IdNeumaticoDrop;
                                F["NroSerie"] = gstrNroSerieDrop;
                                F["FlagActivo"] = true;
                                gtblListSerie.Rows.Add(F);
                            }
                            ActualizarListaSeries();
                            #endregion

                            imgDrop.Tag = imgDrag.Tag;
                            imgDrop.ToolTip = MensajeToolTip(imgDrop.Tag.ToString());
                            LimpiarImagenDrag();
                        }
                    }
                    #endregion
                }
                //De: Llanta Normal Destino o  Llanta Repuesto Destino
                else if (strProcedencia == "D" )
                {
                    #region
                    //Hacia: Llanta Normal Origen o Llanta Respuesto Origen
                    if (imgDrop.Name.Substring(3, 1) == "O")
                    {
                        if (MensajeEmergente_Respuesta(true,true) == 0) return;
                        
                        AddTransferenciaData(IdNeumaticoDrop, gstrNroSerieDrop,
                            0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                            gintIdAlmacen, 0, 0, 0,
                        gstrMovimiento);

                        AddTransferenciaData(IdNeumaticoDrag, gstrNroSerieDrag,
                            0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                            0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                        gstrMovimiento);

                        if ((bool)rdRotar.IsChecked)
                        {
                            gintCantMovi = 3;
                            AddTransferenciaData(IdNeumaticoDrop, gstrNroSerieDrop,
                                gintIdAlmacen, 0, 0, 0,
                                0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                        gstrMovimiento);

                            etiquetaDrag = imgDrag.Tag;
                            imgDrag.Tag = imgDrop.Tag;
                            imgDrag.ToolTip = MensajeToolTip(imgDrag.Tag.ToString());
                            imgDrop.Tag = etiquetaDrag;
                            imgDrop.ToolTip = MensajeToolTip(imgDrop.Tag.ToString());
                        }
                        else
                        {
                            #region Agregando a Lista Series
                            
                            if (gtblListSerie.Select("NroSerie = " + "'" + gstrNroSerieDrop + "'").Length == 1)
                            {
                                for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                                {
                                    if (IdNeumaticoDrop == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                                    {
                                        gtblListSerie.Rows[i]["FlagActivo"] = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                DataRow F = gtblListSerie.NewRow();
                                F["IdNeumatico"] = IdNeumaticoDrop;
                                F["NroSerie"] = gstrNroSerieDrop;
                                F["FlagActivo"] = true;
                                gtblListSerie.Rows.Add(F);
                            }
                            ActualizarListaSeries();
                            #endregion

                            imgDrop.Tag = imgDrag.Tag;
                            imgDrop.ToolTip = MensajeToolTip(imgDrop.Tag.ToString());
                            LimpiarImagenDrag();
                        }
                    }
                    //Hacia: Llanta Normal Destino o Llanta Repuesto Destino
                    else if (imgDrop.Name.Substring(3, 1) == "D")
                    {
                        if (MensajeEmergente_Respuesta(true,true) == 0) return;

                        AddTransferenciaData(IdNeumaticoDrop, gstrNroSerieDrop,
                            0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                            gintIdAlmacen, 0, 0, 0,
                        gstrMovimiento);

                        AddTransferenciaData(IdNeumaticoDrag, gstrNroSerieDrag,
                            0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                            0, IdUCDrop, IdEjeDrop, IdPosicionDrop,
                        gstrMovimiento);
                        if ((bool)rdRotar.IsChecked)
                        {
                            gintCantMovi = 3;
                            AddTransferenciaData(IdNeumaticoDrop, gstrNroSerieDrop,
                                gintIdAlmacen, 0, 0, 0,
                                0, IdUCDrag, IdEjeDrag, IdPosicionDrag,
                        gstrMovimiento);

                            etiquetaDrag = imgDrag.Tag;
                            imgDrag.Tag = imgDrop.Tag;
                            imgDrag.ToolTip = MensajeToolTip(imgDrag.Tag.ToString());
                            imgDrop.Tag = etiquetaDrag;
                            imgDrop.ToolTip = MensajeToolTip(imgDrop.Tag.ToString());
                        }
                        else
                        {
                            #region Agregando a Lista Series

                            if (gtblListSerie.Select("NroSerie = " + "'" + gstrNroSerieDrop + "'").Length == 1)
                            {
                                for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                                {
                                    if (IdNeumaticoDrop == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                                    {
                                        gtblListSerie.Rows[i]["FlagActivo"] = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                DataRow F = gtblListSerie.NewRow();
                                F["IdNeumatico"] = IdNeumaticoDrop;
                                F["NroSerie"] = gstrNroSerieDrop;
                                F["FlagActivo"] = true;
                                gtblListSerie.Rows.Add(F);
                            }
                            ActualizarListaSeries();
                            #endregion

                            imgDrop.Tag = imgDrag.Tag;
                            imgDrop.ToolTip = MensajeToolTip(imgDrop.Tag.ToString());
                            LimpiarImagenDrag();
                        }
                    } 
                    #endregion
                }
            }
            LimpiarMensajeEmegente();
        }

        private static object GetDataFromListBox(ListBox source, Point point)
        {
            try
            {
                UIElement element = source.InputHitTest(point) as UIElement;

                if (element != null)
                {
                    object data = DependencyProperty.UnsetValue;
                    while (data == DependencyProperty.UnsetValue)
                    {
                        data = source.ItemContainerGenerator.ItemFromContainer(element);

                        if (data == DependencyProperty.UnsetValue)
                        {
                            element = VisualTreeHelper.GetParent(element) as UIElement;
                        }

                        if (element == source)
                        {
                            return null;
                        }
                    }

                    if (data != DependencyProperty.UnsetValue)
                    {
                        return data;
                    }
                }

                
            }
            catch { }
            return null;
        }
                
        private void CargarListSeries()
        {
            try
            {
                DataTable tblNeumaticos = objB_Neumatico.B_Neumatico_Combo();
                gtblListSerie.Rows.Clear();
                for (int i = 0; i < tblNeumaticos.Rows.Count; i++)
                {
                    DataRow F = gtblListSerie.NewRow();
                    F["IdNeumatico"] = Convert.ToInt32(tblNeumaticos.Rows[i]["IdNeumatico"]);
                    F["NroSerie"] = tblNeumaticos.Rows[i]["NroSerie"];
                    F["FlagActivo"] = true;
                    F["DeAlmacen"] = 1;
                    gtblListSerie.Rows.Add(F);
                }
                ActualizarListaSeries();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void InicializarGrillaImagen(Grid grilla)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
            {
                var control = VisualTreeHelper.GetChild(grilla, i);
                if (control is Image)
                {
                    (control as Image).AllowDrop = false;
                    (control as Image).Tag = null;
                    (control as Image).ToolTip = null;
                    if (grilla == gridEjesOrigen || grilla == gridEjesDestino)
                        (control as Image).Source = imgEje_blank;
                    else if (grilla == gridLlantasNormalOrigen || grilla == gridLlantaNormalDestino)
                        (control as Image).Source = imgLlantaNormal_disable;
                    else if (grilla == gridLlantaRepuestoOrigen || grilla == gridLlantaRepuestoDestino)
                        (control as Image).Source = imgLlantaRepuesto_disable;
                }
            }
        }

        private void DeshabilitarImagenes(string procedencia)
        {
            if (procedencia == "O")
            {
                imgTimonOrigen.Visibility = Visibility.Hidden;
                InicializarGrillaImagen(gridLlantasNormalOrigen);
                InicializarGrillaImagen(gridLlantaRepuestoOrigen);
                InicializarGrillaImagen(gridEjesOrigen);
            }
            else if (procedencia == "D")
            {
                imgTimonDestino.Visibility = Visibility.Hidden;
                InicializarGrillaImagen(gridLlantaNormalDestino);
                InicializarGrillaImagen(gridLlantaRepuestoDestino);
                InicializarGrillaImagen(gridEjesDestino);
            }
        }
        
        private void LimpiarImagenDrag()
        {
            try
            {
                if (imgDrag.Source == imgLlantaNormal)
                    imgDrag.Source = imgLlantaNormal_enable;
                else if (imgDrag.Source == imgLlantaRepuesto)
                    imgDrag.Source = imgLlantaRepuesto_enable;
                imgDrag.Tag = null;
                imgDrag.ToolTip = null;
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private bool IsAutorizadoBaja()
        {
            bool rpt = false;
            objE_TablaMaestra.IdTabla = 1000;
            DataTable tblParametroSistema = B_TablaMaestra.TablaMaestra_Combo(objE_TablaMaestra);
            if (1 == Convert.ToInt32(tblParametroSistema.Rows[3]["Valor"]))
                rpt = true;
            else if (0 == Convert.ToInt32(tblParametroSistema.Rows[3]["Valor"]))
                rpt= false;

            return rpt;
        }

        private bool ValidarFechaBaja(string NroSerie)
        {
            if (MensajeEmergente_Respuesta(false, false) == 0) return true;
            
            int IdNeumatico=0;
            for (int i = 0; i < gtblListSerie.Rows.Count; i++)
            {
                if (NroSerie == gtblListSerie.Rows[i]["NroSerie"].ToString())
                {
                    IdNeumatico = Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]);
                    break;
                }
            }
            bool VolverAbrirMensaje = false;
            string FechaMovi = gtblNeumaticoTransferDet.Compute("Max(FechaMovi)", "IdNeumatico = " + IdNeumatico).ToString();
            FechaMovi = (FechaMovi == "") ? "1/1/0001" :
                FechaMovi.Substring(0, 4) + "/" + FechaMovi.Substring(4, 2) + "/" + FechaMovi.Substring(6, 2) + FechaMovi.Substring(8, 6);

            if (Convert.ToDateTime(mskFecha.EditValue) <= DateTime.Parse(FechaMovi))
            { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "LOGI_FECH_BAJA"), 2); mskFecha.Focus(); VolverAbrirMensaje = true; }
            else
            {
                objE_Neumatico.IdNeumatico = IdNeumatico;
                DataTable tblFechaMovi = objB_Neumatico.Neumatico_UltimoMovimiento(objE_Neumatico);
                if (tblFechaMovi.Rows.Count > 0)
                    FechaMovi = tblFechaMovi.Rows[0]["FehaMovi"].ToString();

                if (Convert.ToDateTime(mskFecha.EditValue) <= Convert.ToDateTime(FechaMovi))
                { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "LOGI_FECH_BAJA"), 2); mskFecha.Focus(); VolverAbrirMensaje = true; }

            }
            if (VolverAbrirMensaje)
            {
                return ValidarFechaBaja(NroSerie);
                    
                //mskFecha.EditValue = DateTime.Parse(FechaMovi);
            }
            return false;

        }
        private void imgEliminar_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (strProcedencia != "L") return;
                if (IsAutorizadoBaja())
                {
                    GlobalClass.ip.Mensaje( "La Autorizacion de Baja de Neumatico esta Desactivada",2);
                    return;
                }
                //if (MensajeEmergente_Respuesta(false, false) == 0) return;

                object data = e.Data.GetData(typeof(string));
                string NroSerieDList= data.ToString();
                int IdNeumaticoDList=0;

                if (ValidarFechaBaja(NroSerieDList)) return;


                for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                {
                    if (NroSerieDList == gtblListSerie.Rows[i]["NroSerie"].ToString())
                    {
                        IdNeumaticoDList = Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]);
                        if ("1" == Utilitarios.Utilitarios.IIfBlankZero(gtblListSerie.Rows[i]["DeAlmacen"].ToString()))
                            gtblListSerie.Rows[i]["FlagActivo"] = false;
                        else
                            gtblListSerie.Rows.RemoveAt(i);
                        break;
                    }
                }
                
                ActualizarListaSeries();

                DataRow fila = gtblDesecho.NewRow();
                fila["IdNeumatico"] = IdNeumaticoDList;
                fila["FechaBaja"] = Convert.ToDateTime(mskFecha.EditValue).ToString("yyyyMMdd HH:mm");
                fila["IdEstado"] = 5;
                fila["FlagActivo"] = true;
                fila["Nuevo"] = true;
                gtblDesecho.Rows.Add(fila);
                ActualizarImagenTrash();

                AddTransferenciaData(IdNeumaticoDList, NroSerieDList,
                    gintIdAlmacen, 0, 0, 0,
                    gintIdTrash, 0, 0, 0,
                    "Baja");
                LimpiarMensajeEmegente();
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void chkTras_Click(object sender, RoutedEventArgs e)
        {
            if (lstMovi.Items.Count > 0)
            {
                GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "LOGI_MOVI_PEND"), 2);
                chkTras.IsChecked = !(bool)chkTras.IsChecked ? true : false;
                return;
            }
            if ((bool)chkTras.IsChecked)
            {
                cboUCDestino.IsEnabled = true;
                cboUCDestino.SelectedIndex = -1;
                DeshabilitarImagenes("D");
                //lstSerie.IsEnabled = false;
            }
            else
            {
                cboUCDestino.IsEnabled = false;
                cboUCDestino.SelectedIndex = -1;
                DeshabilitarImagenes("D");
                //lstSerie.IsEnabled = true;
            }

        }
        
        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstMovi.Items.Count == 0)
                {
                    GlobalClass.ip.Mensaje("No se encontro movimientos", 2);
                    return;
                }

                objE_Neumatico_Transfer.IdNeumaticoTransfer = 0;
                objE_Neumatico_Transfer.CodNeumaticoTransfer = "";
                objE_Neumatico_Transfer.FlagActivo = true;
                objE_Neumatico_Transfer.IdUsuario = 1;
                objE_Neumatico_Transfer.FechaModificacion = FechaModificacion;
                
                gtblNeumaticoTransferDet.Columns.Remove("CantMovimiento");
                gtblNeumaticoTransferDet.Columns.Remove("NroSerie");
                gtblNeumaticoTransferDet.Columns.Remove("OrigenNombre");
                gtblNeumaticoTransferDet.Columns.Remove("DestinoNombre");

                int rpta = objB_Neumatico_Transfer.Neumatico_InsertMasivo(objE_Neumatico_Transfer, gtblNeumaticoTransferDet, gtblDesecho);
                if (rpta == 1)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "GRAB_NEUM"), 1);
                }
                else if (rpta == 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "LOGI_MODI"), 2);
                    return;
                }
                else if (rpta == 1205)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "GRAB_CONC"), 2);
                    return;
                }

                lstMovi.Items.Clear();
                CargarListSeries();
                gtblNeumaticoTransferDet.Rows.Clear();

                //IMPRIMIR AL GUARDAR
                int UC = 0;
                UC = Convert.ToInt32(cboUCOrigen.EditValue);
                if (UC > 0)
                {
                    GlobalClass.GeneraImpresion(gintIdMenu, UC);
                }
                //IMPRIMIR AL GUARDAR

                cboUCOrigen.SelectedIndex = -1;
                cboUCDestino.SelectedIndex = -1;
                DeshabilitarImagenes("D");
                DeshabilitarImagenes("O");
                imgEliminar.Source = imgTrash;

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
            finally
            {
                GlobalClass.Columna_AddIFnotExits(gtblNeumaticoTransferDet, "CantMovimiento", Type.GetType("System.Boolean"));
                GlobalClass.Columna_AddIFnotExits(gtblNeumaticoTransferDet, "NroSerie", Type.GetType("System.String"));
                GlobalClass.Columna_AddIFnotExits(gtblNeumaticoTransferDet, "OrigenNombre", Type.GetType("System.String"));
                GlobalClass.Columna_AddIFnotExits(gtblNeumaticoTransferDet, "DestinoNombre", Type.GetType("System.String"));
            }
        }

        private void btnCancNeum_Click(object sender, RoutedEventArgs e)
        {
            if (lstMovi.Items.Count > 0)
            {
                var resultado = DevExpress.Xpf.Core.DXMessageBox.Show("Tiene Movimientos Pendientes por grabar.\r\n¿Seguro que desea Salir?", "Mensaje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado != MessageBoxResult.Yes) return;
            }
            GlobalClass.ip.CerraChild(this);
        }
        
        private void MostrarLlantaRepuesto(int intLR, string procedencia)
        {
            try
            {
                string imagenes = string.Empty;
                for (int i = 1; i <= intLR; i++)
                    imagenes = imagenes + "img" + procedencia + "LR00" + Utilitarios.Utilitarios.NumeroChar2(i);

                if (procedencia == "O")
                {
                    imgTimonOrigen.Visibility = System.Windows.Visibility.Visible;
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gridLlantaRepuestoOrigen); i++)
                    {
                        var control = VisualTreeHelper.GetChild(gridLlantaRepuestoOrigen, i);
                        if (control is Image)
                            if (imagenes.Contains((control as Image).Name))
                            {
                                (control as Image).AllowDrop = true;
                                (control as Image).Source = imgLlantaRepuesto_enable;
                                //(control as Image).Visibility = Visibility.Visible;
                            }
                    }
                }
                if (procedencia == "D")
                {
                    imgTimonDestino.Visibility = System.Windows.Visibility.Visible;
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gridLlantaRepuestoDestino); i++)
                    {
                        var control = VisualTreeHelper.GetChild(gridLlantaRepuestoDestino, i);
                        if (control is Image)
                            if (imagenes.Contains((control as Image).Name))
                            {
                                (control as Image).AllowDrop = true;
                                (control as Image).Source = imgLlantaRepuesto_enable;
                                //(control as Image).Visibility = Visibility.Visible;
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

        private void MostrarLlantaNormal(string strEje, int Llantas,string procedencia)
        {
            try
            {
                string imagenes="";
                for (int j = 1; j <= Llantas; j++)
                {
                    imagenes=imagenes+","+"img" + procedencia + strEje + "L" + Utilitarios.Utilitarios.NumeroChar2(j);                
                }
                
                if (procedencia == "O")
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gridLlantasNormalOrigen); i++)
                    {
                        for (int j = 1; j <= Llantas; j++)
                        {
                            var control = VisualTreeHelper.GetChild(gridLlantasNormalOrigen, i);
                            if (control is Image)
                                if (imagenes.Contains((control as Image).Name))
                                {
                                    (control as Image).AllowDrop = true;
                                    (control as Image).Source = imgLlantaNormal_enable;
                                    //(control as Image).Visibility = Visibility.Visible;
                                }
                        }

                    }
                }
                else if (procedencia == "D")
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gridLlantaNormalDestino); i++)
                    {
                        for (int j = 1; j <= Llantas; j++)
                        {
                            var control = VisualTreeHelper.GetChild(gridLlantaNormalDestino, i);
                            if (control is Image)
                                if (imagenes.Contains((control as Image).Name))
                                {
                                    (control as Image).AllowDrop = true;
                                    (control as Image).Source = imgLlantaNormal_enable;
                                    //(control as Image).Visibility = Visibility.Visible;
                                }
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

        private void MostrarEjes(int Nroeje, string procedencia)
        {
            string imagenes = string.Empty;
            for (int i = 1; i <= Nroeje; i++)
                imagenes = imagenes + "imgEje" + procedencia + Utilitarios.Utilitarios.NumeroChar2(i);

            if (procedencia == "O")
            {
                imgTimonOrigen.Visibility = System.Windows.Visibility.Visible;
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gridEjesOrigen); i++)
                {
                    var control = VisualTreeHelper.GetChild(gridEjesOrigen, i);
                    if (control is Image)
                        if (imagenes.Contains((control as Image).Name))
                        {
                            //(control as Image).Visibility = Visibility.Visible;
                            (control as Image).Source = imgEje;
                        }
                }
            }
            else if (procedencia == "D")
            {
                imgTimonDestino.Visibility = System.Windows.Visibility.Visible;
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(gridEjesDestino); i++)
                {
                    var control = VisualTreeHelper.GetChild(gridEjesDestino, i);
                    if (control is Image)
                        if (imagenes.Contains((control as Image).Name))
                        {
                            //(control as Image).Visibility = Visibility.Visible;
                            (control as Image).Source = imgEje;
                        }
                }
            }
                
        }
                
        private void PintarLlantaOcupada(int IdEje, int IdPosicion,int IdNeumatico,string NroSerie, string Procedencia)
        {
            string NombreImagen = string.Empty;
            Grid grillaRepuesto = null, grillaNormal = null;
            if (Procedencia.ToUpper() == "O")
            {
                grillaRepuesto = gridLlantaRepuestoOrigen;
                grillaNormal = gridLlantasNormalOrigen;
            }
            else if (Procedencia.ToUpper() == "D")
            {
                grillaRepuesto = gridLlantaRepuestoDestino;
                grillaNormal = gridLlantaNormalDestino;
            }
            if (grillaNormal == null || grillaRepuesto == null) return;

            if (IdEje == 99)
            {
                NombreImagen = "img" + Procedencia.ToUpper() + "LR00" + Utilitarios.Utilitarios.NumeroChar2(IdPosicion);
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grillaRepuesto); i++)
                {
                    var control = VisualTreeHelper.GetChild(grillaRepuesto, i);
                    if (control is Image)
                        if ((control as Image).Name == NombreImagen)
                        {
                            (control as Image).Source = imgLlantaRepuesto;
                            (control as Image).Tag = IdNeumatico + "|" + NroSerie;
                            (control as Image).ToolTip = MensajeToolTip((control as Image).Tag.ToString());
                            break;
                        }
                }
            }
            else
            {
                NombreImagen = "img" + Procedencia + "E" + Utilitarios.Utilitarios.NumeroChar2(IdEje) + "L" + Utilitarios.Utilitarios.NumeroChar2(IdPosicion);
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grillaNormal); i++)
                {
                    var control = VisualTreeHelper.GetChild(grillaNormal, i);
                    if (control is Image)
                        if ((control as Image).Name == NombreImagen)
                        {
                            (control as Image).Source = imgLlantaNormal;
                            (control as Image).Tag = IdNeumatico + "|" + NroSerie;
                            (control as Image).ToolTip = MensajeToolTip((control as Image).Tag.ToString());
                            break;
                        }
                }            
            }            
        }
        
        private void cboUCOrigen_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboUCOrigen.SelectedIndex == -1) return;
                if (lstMovi.Items.Count > 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "LOGI_MOVI_PEND"), 2);
                    cboUCOrigen.EditValue = gintIdUcOrigenPrevio;
                    return;
                }
                gintIdUcOrigenPrevio = Convert.ToInt32(cboUCOrigen.EditValue);

                DeshabilitarImagenes("O");
                objE_UC.IdUc = Convert.ToInt32(cboUCOrigen.EditValue);
                DataTable tblUC = objB_UC.B_UC_GetItem(objE_UC);

                int IdPerfilNeumatico = Convert.ToInt32(tblUC.Rows[0]["IdPerfilNeumatico"].ToString());
                                
                objE_PerfilNeumaticoEje.IdPerfilNeumatico = IdPerfilNeumatico;
                DataTable tblPerfilNeumaticoEjes = B_PerfilNeumaticoEje.PerfilNeumaticoEje_List(objE_PerfilNeumaticoEje);

                TempEstrucOrigen.Rows.Clear();
                for (int i = 0; i < tblPerfilNeumaticoEjes.Rows.Count; i++)
                {
                    if (tblPerfilNeumaticoEjes.Rows[i]["NroLlantas"].ToString() != "")
                    {
                        MostrarLlantaNormal(tblPerfilNeumaticoEjes.Rows[i]["Eje"].ToString(), Convert.ToInt32(tblPerfilNeumaticoEjes.Rows[i]["NroLlantas"]), "O");
                        EstructurarPerfilNeumatico(tblPerfilNeumaticoEjes.Rows[i]["Eje"].ToString(), Convert.ToInt32(tblPerfilNeumaticoEjes.Rows[i]["NroLlantas"]),"O");
                    }
                }

                objE_PerfilNeumatico.IdPerfilNeumatico = IdPerfilNeumatico;
                DataTable tblPerfilNeumatico = B_PerfilNeumatico.PerfilNeumatico_GetItem(objE_PerfilNeumatico);
                MostrarEjes(Convert.ToInt32(tblPerfilNeumatico.Rows[0]["NroEjes"]), "O");
                MostrarLlantaRepuesto(Convert.ToInt32(tblPerfilNeumatico.Rows[0]["NroLlantaRepuesto"]), "O");
                
                if (Convert.ToInt32(tblPerfilNeumatico.Rows[0]["NroLlantaRepuesto"]) > 0)
                    EstructurarPerfilNeumatico("E99", Convert.ToInt32(tblPerfilNeumatico.Rows[0]["NroLlantaRepuesto"]),"O");

                OrdenarPosicionCliente("O");
                
                objE_UC.IdUc = Convert.ToInt32(cboUCOrigen.EditValue);
                DataTable tblNeumaticos = B_UC.B_Neumatico_ListByUC(objE_UC);

                for (int i = 0; i < tblNeumaticos.Rows.Count; i++)
                {
                    PintarLlantaOcupada(
                        Convert.ToInt32(tblNeumaticos.Rows[i]["IdEje"].ToString()),
                        Convert.ToInt32(tblNeumaticos.Rows[i]["IdPosicion"].ToString()),
                        Convert.ToInt32(tblNeumaticos.Rows[i]["IdNeumatico"].ToString()),
                        tblNeumaticos.Rows[i]["NroSerie"].ToString(),
                        "O");
                }

                cboUCDestino.SelectedIndexChanged -= new RoutedEventHandler(cboUCDestino_SelectedIndexChanged);
                gtblcboDestino = objB_UC.B_UC_ComboWithPN(objE_UC);
                gtblcboDestino.DefaultView.RowFilter = "IdUC <> " + cboUCOrigen.EditValue.ToString();
                cboUCDestino.ItemsSource = gtblcboDestino.DefaultView;
                cboUCDestino.DisplayMember = "CodUC";
                cboUCDestino.ValueMember = "IdUC";
                cboUCDestino.SelectedIndex = -1;
                cboUCDestino.SelectedIndexChanged += new RoutedEventHandler(cboUCDestino_SelectedIndexChanged);
                
                gtblNeumaticoTransferDet.Rows.Clear();
                CargarListSeries();
                DeshabilitarImagenes("D");
                lstMovi.Items.Clear(); 
                
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void cboUCDestino_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (cboUCDestino.SelectedIndex == -1) return;
                if (lstMovi.Items.Count > 0)
                {
                    GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "LOGI_MOVI_PEND"), 2);
                    cboUCDestino.EditValue = gintIdUcDestinoPrevio;
                    return;
                }
                gintIdUcDestinoPrevio = Convert.ToInt32(cboUCDestino.EditValue);
                DeshabilitarImagenes("D");
                objE_UC.IdUc = Convert.ToInt32(cboUCDestino.EditValue);
                DataTable tblUC = objB_UC.B_UC_GetItem(objE_UC);

                int IdPerfilNeumatico = Convert.ToInt32(tblUC.Rows[0]["IdPerfilNeumatico"].ToString());
                                
                objE_PerfilNeumaticoEje.IdPerfilNeumatico = IdPerfilNeumatico;
                DataTable tblPerfilNeumaticoEjes = B_PerfilNeumaticoEje.PerfilNeumaticoEje_List(objE_PerfilNeumaticoEje);

                TempEstrucDestino.Rows.Clear();
                for (int i = 0; i < tblPerfilNeumaticoEjes.Rows.Count; i++)
                {
                    if (tblPerfilNeumaticoEjes.Rows[i]["NroLlantas"].ToString() != "")
                    {
                        MostrarLlantaNormal(tblPerfilNeumaticoEjes.Rows[i]["Eje"].ToString(), Convert.ToInt32(tblPerfilNeumaticoEjes.Rows[i]["NroLlantas"].ToString()),"D");
                        EstructurarPerfilNeumatico(tblPerfilNeumaticoEjes.Rows[i]["Eje"].ToString(), Convert.ToInt32(tblPerfilNeumaticoEjes.Rows[i]["NroLlantas"]), "D");
                    }
                }

                objE_PerfilNeumatico.IdPerfilNeumatico = IdPerfilNeumatico;
                DataTable tblPerfilNeumatico = B_PerfilNeumatico.PerfilNeumatico_GetItem(objE_PerfilNeumatico);

                MostrarEjes(Convert.ToInt32(tblPerfilNeumatico.Rows[0]["NroEjes"]), "D");
                MostrarLlantaRepuesto(Convert.ToInt32(tblPerfilNeumatico.Rows[0]["NroLlantaRepuesto"]), "D");
                
                if (Convert.ToInt32(tblPerfilNeumatico.Rows[0]["NroLlantaRepuesto"]) > 0)
                    EstructurarPerfilNeumatico("E99", Convert.ToInt32(tblPerfilNeumatico.Rows[0]["NroLlantaRepuesto"]), "D");
                
                OrdenarPosicionCliente("D");

                objE_UC.IdUc = Convert.ToInt32(cboUCDestino.EditValue);
                DataTable tblNeumaticos = B_UC.B_Neumatico_ListByUC(objE_UC);

                for (int i = 0; i < tblNeumaticos.Rows.Count; i++)
                {
                    PintarLlantaOcupada(
                        Convert.ToInt32(tblNeumaticos.Rows[i]["IdEje"].ToString()),
                        Convert.ToInt32(tblNeumaticos.Rows[i]["IdPosicion"].ToString()),
                        Convert.ToInt32(tblNeumaticos.Rows[i]["IdNeumatico"].ToString()),
                        tblNeumaticos.Rows[i]["NroSerie"].ToString(),
                        "D");
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void PLANTILLA_VentanaEmergente_IsVisible(object sender, DependencyPropertyChangedEventArgs e)
        {
            GlobalClass.ip.VentanaEmergente_Visibilidad(sender);
        }
        
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (gtblNeumaticoTransferDet.Rows.Count == 0) return;
            int iLast = gtblNeumaticoTransferDet.Rows.Count - 1;
            int IdNeumatico = Convert.ToInt32(gtblNeumaticoTransferDet.Rows[iLast]["IdNeumatico"]);
            string NroSerie = gtblNeumaticoTransferDet.Rows[iLast]["NroSerie"].ToString();
            string NombreOrigen = gtblNeumaticoTransferDet.Rows[iLast]["OrigenNombre"].ToString();
            string NombreDestino = gtblNeumaticoTransferDet.Rows[iLast]["DestinoNombre"].ToString();

            int IdAlmacenSalida = Convert.ToInt32(gtblNeumaticoTransferDet.Rows[iLast]["IdAlmacenSalida"]);
            int IdAlmacenEntrada = Convert.ToInt32(gtblNeumaticoTransferDet.Rows[iLast]["IdAlmacenEntrada"]);

            int CantMovimiento= Convert.ToInt32( gtblNeumaticoTransferDet.Rows[iLast]["CantMovimiento"]);


            if (CantMovimiento==3)
            {
                int IdNeumaticoOrigen = Convert.ToInt32(gtblNeumaticoTransferDet.Rows[iLast-1]["IdNeumatico"]);
                string NroSerieOrigen = gtblNeumaticoTransferDet.Rows[iLast-1]["NroSerie"].ToString();
                int IdNeumaticoDestino = Convert.ToInt32(gtblNeumaticoTransferDet.Rows[iLast]["IdNeumatico"]);
                string NroSerieDestino = gtblNeumaticoTransferDet.Rows[iLast]["NroSerie"].ToString();
                string procedencia = NombreOrigen.Substring(3, 2);
                Grid grilla = new Grid();
                if (procedencia == "OE")
                    grilla = gridLlantasNormalOrigen;
                else if (procedencia == "OL")
                    grilla = gridLlantaRepuestoOrigen;
                else if (procedencia == "DE")
                    grilla = gridLlantaNormalDestino;
                else if (procedencia == "DL")
                    grilla = gridLlantaRepuestoDestino;

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
                {
                    var control = VisualTreeHelper.GetChild(grilla, i);
                    if (control is Image)
                        if ((control as Image).Name == NombreOrigen)
                        {
                            if (procedencia == "OE" || procedencia == "DE")
                                (control as Image).Source = imgLlantaNormal;
                            else if (procedencia == "OL" || procedencia == "DL")
                                (control as Image).Source = imgLlantaRepuesto;

                            (control as Image).Tag = IdNeumaticoOrigen + "|" + NroSerieOrigen;
                            (control as Image).ToolTip = MensajeToolTip((control as Image).Tag.ToString());
                            break;
                        }
                }

                string SiglaDestino = NombreDestino.Substring(3, 2);
                if (SiglaDestino == "OE")
                    grilla = gridLlantasNormalOrigen;
                else if (SiglaDestino == "OL")
                    grilla = gridLlantaRepuestoOrigen;
                else if (SiglaDestino == "DE")
                    grilla = gridLlantaNormalDestino;
                else if (SiglaDestino == "DL")
                    grilla = gridLlantaRepuestoDestino;
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
                {
                    var control = VisualTreeHelper.GetChild(grilla, i);
                    if (control is Image)
                        if ((control as Image).Name == NombreDestino)
                        {
                            if (SiglaDestino == "OE" || SiglaDestino == "DE")
                                (control as Image).Source = imgLlantaNormal;
                            else if (SiglaDestino == "OL" || SiglaDestino == "DL")
                                (control as Image).Source = imgLlantaRepuesto;

                            (control as Image).Tag = IdNeumaticoDestino + "|" + NroSerieDestino;
                            (control as Image).ToolTip = MensajeToolTip((control as Image).Tag.ToString());
                            break;
                        }
                }
            }
            else if (CantMovimiento == 2)
            {
                Grid grilla = null;
                int IdNeumaticoOrigen = Convert.ToInt32(gtblNeumaticoTransferDet.Rows[iLast]["IdNeumatico"]);
                string NroSerieOrigen = gtblNeumaticoTransferDet.Rows[iLast]["NroSerie"].ToString();
                int IdNeumaticoDestino = Convert.ToInt32(gtblNeumaticoTransferDet.Rows[iLast - 1]["IdNeumatico"]);
                string NroSerieDestino = gtblNeumaticoTransferDet.Rows[iLast - 1]["NroSerie"].ToString();

                if (IdAlmacenSalida == gintIdAlmacen)//Si viene del almacen
                {
                    string SiglaDestino = NombreDestino.Substring(3, 2);
                    if (SiglaDestino == "OE")
                        grilla = gridLlantasNormalOrigen;
                    else if (SiglaDestino == "OL")
                        grilla = gridLlantaRepuestoOrigen;
                    else if (SiglaDestino == "DE")
                        grilla = gridLlantaNormalDestino;
                    else if (SiglaDestino == "DL")
                        grilla = gridLlantaRepuestoDestino;
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
                    {
                        var control = VisualTreeHelper.GetChild(grilla, i);
                        if (control is Image)
                            if ((control as Image).Name == NombreDestino)
                            {
                                if (SiglaDestino == "OE" || SiglaDestino == "DE")
                                    (control as Image).Source = imgLlantaNormal;
                                else if (SiglaDestino == "OL" || SiglaDestino == "DL")
                                    (control as Image).Source = imgLlantaRepuesto;

                                (control as Image).Tag = IdNeumaticoDestino + "|" + NroSerieDestino;
                                (control as Image).ToolTip = MensajeToolTip((control as Image).Tag.ToString());
                                break;
                            }
                    }
                    //remover de la lista
                    for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                    {
                        if (IdNeumaticoDestino == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                        {
                            if ("1" == Utilitarios.Utilitarios.IIfBlankZero(gtblListSerie.Rows[i]["DeAlmacen"].ToString()))
                                gtblListSerie.Rows[i]["FlagActivo"] = false;
                            else
                                gtblListSerie.Rows.RemoveAt(i);
                            break;
                        }
                    }
                    ActualizarListaSeries();
                    //agregar a la lista
                    if (gtblListSerie.Select("NroSerie = " + "'" + NroSerieOrigen + "'").Length == 1)
                    {
                        for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                        {
                            if (IdNeumaticoOrigen == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                            {
                                gtblListSerie.Rows[i]["FlagActivo"] = true;
                                gintNuevaSerieAgregada = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        DataRow F = gtblListSerie.NewRow();
                        F["IdNeumatico"] = IdNeumaticoOrigen;
                        F["NroSerie"] = NroSerieOrigen;
                        F["FlagActivo"] = true;
                        gintNuevaSerieAgregada = gtblListSerie.Rows.Count;
                        gtblListSerie.Rows.Add(F);
                    }
                    ActualizarListaSeries();
                }
                else if (IdAlmacenSalida == 0)// si no viene del almacen
                {
                    string SiglaOrigen = NombreOrigen.Substring(3, 2);
                    if (SiglaOrigen == "OE")
                        grilla = gridLlantasNormalOrigen;
                    else if (SiglaOrigen == "OL")
                        grilla = gridLlantaRepuestoOrigen;
                    else if (SiglaOrigen == "DE")
                        grilla = gridLlantaNormalDestino;
                    else if (SiglaOrigen == "DL")
                        grilla = gridLlantaRepuestoDestino;
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
                    {
                        var control = VisualTreeHelper.GetChild(grilla, i);
                        if (control is Image)
                            if ((control as Image).Name == NombreOrigen)
                            {
                                if (SiglaOrigen == "OE" || SiglaOrigen == "DE")
                                    (control as Image).Source = imgLlantaNormal;
                                else if (SiglaOrigen == "OL" || SiglaOrigen == "DL")
                                    (control as Image).Source = imgLlantaRepuesto;

                                (control as Image).Tag = IdNeumaticoOrigen + "|" + NroSerieOrigen;
                                (control as Image).ToolTip = MensajeToolTip((control as Image).Tag.ToString());
                                break;
                            }
                    }

                    string SiglaDestino = NombreDestino.Substring(3, 2);
                    if (SiglaDestino == "OE")
                        grilla = gridLlantasNormalOrigen;
                    else if (SiglaDestino == "OL")
                        grilla = gridLlantaRepuestoOrigen;
                    else if (SiglaDestino == "DE")
                        grilla = gridLlantaNormalDestino;
                    else if (SiglaDestino == "DL")
                        grilla = gridLlantaRepuestoDestino;
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
                    {
                        var control = VisualTreeHelper.GetChild(grilla, i);
                        if (control is Image)
                            if ((control as Image).Name == NombreDestino)
                            {
                                //no establecer img, ya tiene la correcta
                                (control as Image).Tag = IdNeumaticoDestino + "|" + NroSerieDestino;
                                (control as Image).ToolTip = MensajeToolTip((control as Image).Tag.ToString());
                                break;
                            }
                    }
                    //Remover de la lista
                    for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                    {
                        if (IdNeumaticoDestino == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                        {
                            if ("1" == Utilitarios.Utilitarios.IIfBlankZero(gtblListSerie.Rows[i]["DeAlmacen"].ToString()))
                                gtblListSerie.Rows[i]["FlagActivo"] = false;
                            else
                                gtblListSerie.Rows.RemoveAt(i);
                            break;
                        }
                    }
                    ActualizarListaSeries();
                }

            }
            else if (IdAlmacenEntrada == 0 && IdAlmacenSalida == 0)
            {
                string procedencia = NombreOrigen.Substring(3, 2);
                Grid grilla = new Grid();
                if (procedencia == "OE")
                    grilla = gridLlantasNormalOrigen;
                else if (procedencia == "OL")
                    grilla = gridLlantaRepuestoOrigen;
                else if (procedencia == "DE")
                    grilla = gridLlantaNormalDestino;
                else if (procedencia == "DL")
                    grilla = gridLlantaRepuestoDestino;

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
                {
                    var control = VisualTreeHelper.GetChild(grilla, i);
                    if (control is Image)
                        if ((control as Image).Name == NombreOrigen)
                        {
                            if (procedencia == "OE" || procedencia == "DE")
                                (control as Image).Source = imgLlantaNormal;
                            else if (procedencia == "OL" || procedencia == "DL")
                                (control as Image).Source = imgLlantaRepuesto;

                            (control as Image).Tag = IdNeumatico + "|" + NroSerie;
                            (control as Image).ToolTip = MensajeToolTip((control as Image).Tag.ToString());
                            break;
                        }
                }

                string SiglaDestino = NombreDestino.Substring(3, 2);
                if (SiglaDestino == "OE")
                    grilla = gridLlantasNormalOrigen;
                else if (SiglaDestino == "OL")
                    grilla = gridLlantaRepuestoOrigen;
                else if (SiglaDestino == "DE")
                    grilla = gridLlantaNormalDestino;
                else if (SiglaDestino == "DL")
                    grilla = gridLlantaRepuestoDestino;
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
                {
                    var control = VisualTreeHelper.GetChild(grilla, i);
                    if (control is Image)
                        if ((control as Image).Name == NombreDestino)
                        {
                            if (SiglaDestino == "OE" || SiglaDestino == "DE")
                                (control as Image).Source = imgLlantaNormal_enable;
                            else if (SiglaDestino == "OL" || SiglaDestino == "DL")
                                (control as Image).Source = imgLlantaRepuesto_enable;

                            (control as Image).Tag = null;
                            (control as Image).ToolTip = null;
                            break;
                        }
                }
            }
            else
            {
                #region De Numatico Hacia almacen
                if (IdAlmacenEntrada == gintIdAlmacen)
                {
                    Grid grilla = null;

                    for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                    {
                        if (IdNeumatico == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                        {
                            if ("1" == Utilitarios.Utilitarios.IIfBlankZero(gtblListSerie.Rows[i]["DeAlmacen"].ToString()))
                                gtblListSerie.Rows[i]["FlagActivo"] = false;
                            else
                                gtblListSerie.Rows.RemoveAt(i);
                            break;
                        }
                    }
                    ActualizarListaSeries();

                    string procedencia = NombreOrigen.Substring(3, 2);
                    if (procedencia == "OE")
                        grilla = gridLlantasNormalOrigen;
                    else if (procedencia == "OL")
                        grilla = gridLlantaRepuestoOrigen;
                    else if (procedencia == "DE")
                        grilla = gridLlantaNormalDestino;
                    else if (procedencia == "DL")
                        grilla = gridLlantaRepuestoDestino;

                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
                    {
                        var control = VisualTreeHelper.GetChild(grilla, i);
                        if (control is Image)
                            if ((control as Image).Name == NombreOrigen)
                            {
                                if (procedencia == "OE" || procedencia == "DE")
                                    (control as Image).Source = imgLlantaNormal;
                                else if (procedencia == "OL" || procedencia == "DL")
                                    (control as Image).Source = imgLlantaRepuesto;
                                (control as Image).Tag = IdNeumatico + "|" + NroSerie;
                                (control as Image).ToolTip = MensajeToolTip((control as Image).Tag.ToString());
                                break;
                            }
                    }
                }
                #endregion

                #region De almacen, hacia un neumatico
                else if (IdAlmacenSalida == gintIdAlmacen && IdAlmacenEntrada != gintIdTrash)
                {
                    string procedencia = NombreDestino.Substring(3, 2);
                    Grid grilla = null;

                    if (gtblListSerie.Select("NroSerie = " + "'" + NroSerie + "'").Length == 1)
                    {
                        for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                        {
                            if (IdNeumatico == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                            {
                                gtblListSerie.Rows[i]["FlagActivo"] = true;
                                gintNuevaSerieAgregada = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        DataRow F = gtblListSerie.NewRow();
                        F["IdNeumatico"] = IdNeumatico;
                        F["NroSerie"] = NroSerie;
                        F["FlagActivo"] = true;
                        gintNuevaSerieAgregada = gtblListSerie.Rows.Count;
                        gtblListSerie.Rows.Add(F);
                    }
                    ActualizarListaSeries();

                    if (procedencia == "OE")
                        grilla = gridLlantasNormalOrigen;
                    else if (procedencia == "OL")
                        grilla = gridLlantaRepuestoOrigen;
                    else if (procedencia == "DE")
                        grilla = gridLlantaNormalDestino;
                    else if (procedencia == "DL")
                        grilla = gridLlantaRepuestoDestino;

                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(grilla); i++)
                    {
                        var control = VisualTreeHelper.GetChild(grilla, i);
                        if (control is Image)
                            if ((control as Image).Name == NombreDestino)
                            {
                                if (procedencia == "OE" || procedencia == "DE")
                                    (control as Image).Source = imgLlantaNormal_enable;
                                else if (procedencia == "OL" || procedencia == "DL")
                                    (control as Image).Source = imgLlantaRepuesto_enable;
                                (control as Image).Tag = null;
                                (control as Image).ToolTip = null;
                                break;
                            }
                    }
                }
                #endregion
                
                #region HACIA EL TACHO
                else if (IdAlmacenSalida == gintIdAlmacen && IdAlmacenEntrada == gintIdTrash)
                {
                    if (gtblListSerie.Select("NroSerie = " + "'" + NroSerie + "'").Length == 1)
                    {
                        for (int i = 0; i < gtblListSerie.Rows.Count; i++)
                        {
                            if (IdNeumatico == Convert.ToInt32(gtblListSerie.Rows[i]["IdNeumatico"]))
                            {
                                gtblListSerie.Rows[i]["FlagActivo"] = true;
                                gintNuevaSerieAgregada = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        DataRow F = gtblListSerie.NewRow();
                        F["IdNeumatico"] = IdNeumatico;
                        F["NroSerie"] = NroSerie;
                        F["FlagActivo"] = true;
                        gintNuevaSerieAgregada = gtblListSerie.Rows.Count;
                        gtblListSerie.Rows.Add(F);
                    }
                    ActualizarListaSeries();

                    for (int i = 0; i < gtblDesecho.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(gtblDesecho.Rows[i]["IdNeumatico"]) == IdNeumatico)
                        {
                            gtblDesecho.Rows.RemoveAt(i);
                            break;
                        }
                    }
                    ActualizarImagenTrash();

                }
                #endregion
            }

            for (int i = 1; i <= CantMovimiento; i++)
            {
                lstMovi.Items.RemoveAt(lstMovi.Items.Count - 1);
                gtblNeumaticoTransferDet.Rows.RemoveAt(gtblNeumaticoTransferDet.Rows.Count - 1);
            }
            
            
            
        }
                
        private ToolTip MensajeToolTip(string Etiqueta)
        {
            string IdNeumatico = Etiqueta.Split('|')[0];
            string NroSerie = Etiqueta.Split('|')[1];

            
            DataRow[] FRecauche = gtblNeimaticoCicloList.Select("IdNeumatico =" + IdNeumatico + " and IdCiclo = 1");
            DataRow[] FTamanioBanda = gtblNeimaticoCicloList.Select("IdNeumatico = " + IdNeumatico + " and IdCiclo = 2");
            string Contador1=string.Empty,Frcambio1=string.Empty;
            string Contador2=string.Empty,Frcambio2=string.Empty;
            if (FRecauche.Length > 0)
            {
                Contador1 = FRecauche[0]["Contador"].ToString();
                Frcambio1 = FRecauche[0]["FrecuenciaCambio"].ToString();
            }
            if (FTamanioBanda.Length > 0)
            {
                Contador2 = FTamanioBanda[0]["Contador"].ToString();
                Frcambio2 = FTamanioBanda[0]["FrecuenciaCambio"].ToString();
            }

            
            
            TextBlock txtNroSerie = new TextBlock();
            txtNroSerie.FontFamily = new FontFamily("Tahoma");
            txtNroSerie.Foreground = Brushes.Black;
            txtNroSerie.HorizontalAlignment = HorizontalAlignment.Center;
            txtNroSerie.FontSize = 11;
            txtNroSerie.Inlines.Add(new Run("NroSerie: ") { FontWeight = FontWeights.Bold });
            txtNroSerie.Inlines.Add(NroSerie);
            
            TextBlock txtRecauche = new TextBlock();
            txtRecauche.Foreground = Brushes.Black;
            txtRecauche.Inlines.Add(new Run("Recauche:\n") { FontWeight = FontWeights.Bold } );
            txtRecauche.Inlines.Add("Contador: " + Contador1 + "\n");
            txtRecauche.Inlines.Add("Frec. Cambio: " + Frcambio1);
            
            TextBlock txtTamanioBanda = new TextBlock();
            txtTamanioBanda.Foreground = Brushes.Black;
            txtTamanioBanda.Inlines.Add(new Run("Tamaño Banda:\n") { FontWeight = FontWeights.Bold } );
            txtTamanioBanda.Inlines.Add("Contador: " + Contador2 + "\n");
            txtTamanioBanda.Inlines.Add("Frec. Cambio: " + Frcambio2);
            

            Border borde1 = new Border()
            {
                Background = (new BrushConverter().ConvertFrom("#FF5AA6CB") as Brush),
                BorderBrush = (new BrushConverter().ConvertFrom("#FF5AA6CB") as Brush),
                BorderThickness = new Thickness(2, 2, 2, 2),
                Child = txtNroSerie
            };

            Border borde2 = new Border()
            {
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = (new BrushConverter().ConvertFrom("#FF5AA6CB") as Brush),
                BorderThickness = new Thickness(2, 0, 2, 0),
                Child = txtRecauche
            };

            Border borde3 = new Border()
            {
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = (new BrushConverter().ConvertFrom("#FF5AA6CB") as Brush),
                BorderThickness = new Thickness(2, 0, 2, 2),
                Child = txtTamanioBanda
            };

            StackPanel stk = new StackPanel();
            stk.Margin = new Thickness(-10, -5, -10, -5);
            stk.Orientation = Orientation.Vertical;
            stk.Children.Add(borde1);
            if (FRecauche.Length > 0)
                stk.Children.Add(borde2);
            if (FTamanioBanda.Length > 0)
                stk.Children.Add(borde3);

            ToolTip tt = new System.Windows.Controls.ToolTip()
            {
                Opacity = 1,                
                Content = stk
            };
            return tt;
        }

        private void Imagenes_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                if (((Image)sender).Source == imgLlantaNormal || ((Image)sender).Source == imgLlantaRepuesto)
                {
                    Utilitarios.Utilitarios.gintIdNeumaticoDeMovimiento=Convert.ToInt32(((Image)sender).Tag.ToString().Split('|')[0]);
                    GlobalClass.ip.AbrirGestionNeumatico();
                    Utilitarios.Utilitarios.gintIdNeumaticoDeMovimiento = 0;
                }
            }
        }

        private int MensajeEmergente_Respuesta(bool MostrarAcciones, bool MostrarComentario)
        {
            Detener = false;
            if (MostrarAcciones)
            {
                stkAcciones.Visibility = Visibility.Visible;
                if (imgDrag.Name.Substring(3, 1) == imgDrop.Name.Substring(3, 1))
                    rdRotar.Content = "Rotar";
                else
                    rdRotar.Content = "Trasladar/Intercambiar";
            }
            else
                stkAcciones.Visibility = Visibility.Collapsed;
            if (MostrarComentario)
                stkObservacion.Visibility = Visibility.Visible;
            else
                stkObservacion.Visibility = Visibility.Collapsed;

            VentanaEmergente.Visibility = Visibility.Visible;
            mskFecha.Focus();
            //LimpiarMensajeEmegente();
            EmpezarEspera();
            return RespuestaMensaje;
        }
        private void LimpiarMensajeEmegente()
        {
            txtObservacion.Text = string.Empty;
            mskFecha.EditValue = string.Empty;
            rdReemplazar.IsChecked = false;
            rdRotar.IsChecked = false;
        }
        private bool ValidarVentanaEmergente()
        {
            bool rpt = false;
            if (mskFecha.EditValue.ToString() == "")
            { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "OBLI_FECH_MOVI"), 2); mskFecha.Focus(); rpt = true; }
            else if (stkObservacion.IsVisible && txtObservacion.Text == "")
            { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "OBLI_OBSE_MOVI"), 2); txtObservacion.Focus(); rpt = true; }
            else if (stkAcciones.IsVisible && rdReemplazar.IsChecked == false && rdRotar.IsChecked == false)
            { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "OBLI_ACCI_MOVI"), 2); grbAcciones.Focus(); rpt = true; }
            if (rpt) return rpt;
            DateTime Hoy = Convert.ToDateTime(Utilitarios.Utilitarios.Fecha_Hora_Servidor().Rows[0]["FechaServer"]);
            if (Convert.ToDateTime(mskFecha.EditValue) >= Hoy.AddDays(1))
            { GlobalClass.ip.Mensaje(Utilitarios.Utilitarios.parser.GetSetting(gstrEtiquetaMovimientoNeumatico, "LOGI_FECH_SERV"), 2); mskFecha.Focus(); rpt = true; }
            
            return rpt;
        }
        
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarVentanaEmergente()) return;
            RespuestaMensaje = 1;
            Detener = true;
            VentanaEmergente.Visibility = Visibility.Collapsed;
            stkAcciones.Visibility = Visibility.Collapsed;
            stkObservacion.Visibility = Visibility.Collapsed;
            cboUCDestino.IsEnabled = ((bool)chkTras.IsChecked);
        }
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            RespuestaMensaje = 0;
            Detener = true;
            LimpiarMensajeEmegente();
            VentanaEmergente.Visibility = Visibility.Collapsed;
            stkAcciones.Visibility = Visibility.Collapsed;
            stkObservacion.Visibility = Visibility.Collapsed;
            cboUCDestino.IsEnabled = ((bool)chkTras.IsChecked);
            
        }
        public void EmpezarEspera()
        {
            while (true)
            {
                var frame = new DispatcherFrame();
                new Thread((ThreadStart)(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    frame.Continue = false;
                })).Start();
                Dispatcher.PushFrame(frame);
                if (Detener) break;
            }
        }

        #region PROCEDIMIENTOS PARA OBTENER LA POSICION PARA DEL CLIENTE
        private void IffNoColumnaCrear(string columnName, DataTable table)
        {
            DataColumnCollection columns = table.Columns;
            if (!columns.Contains(columnName))
                table.Columns.Add(columnName);

        }
        private void EstructurarPerfilNeumatico(string IdEje, int Nrollantas, string Procedencia)
        {
            if (Procedencia == "O")
            {
                IffNoColumnaCrear("Eje", TempEstrucOrigen);
                IffNoColumnaCrear("NroLlantas", TempEstrucOrigen);

                DataRow f = TempEstrucOrigen.NewRow();
                f["Eje"] = IdEje;
                f["NroLlantas"] = Nrollantas;
                TempEstrucOrigen.Rows.Add(f);
            }
            else if (Procedencia == "D")
            {
                IffNoColumnaCrear("Eje", TempEstrucDestino);
                IffNoColumnaCrear("NroLlantas", TempEstrucDestino);

                DataRow f = TempEstrucDestino.NewRow();
                f["Eje"] = IdEje;
                f["NroLlantas"] = Nrollantas;
                TempEstrucDestino.Rows.Add(f);
            }
        }
        private void NuevaFilaDPosicion(int IdEje, int IdPosicion)
        {
            DataRow f = gtblPosicionesClient.NewRow();
            f["IdEje"] = IdEje;
            f["IdPosicion"] = IdPosicion;
            gtblPosicionesClient.Rows.Add(f);
        }
        private void OrdenarPosicionCliente(string Procedencia)
        {
            string xeje;
            int xnrollantas;

            gtblPosicionesClient = new DataTable();
            DataColumn clm = new DataColumn();
            clm.AutoIncrementSeed = 1;
            clm.AutoIncrementStep = 1;
            clm.AutoIncrement = true;
            clm.ColumnName = "Id";
            gtblPosicionesClient.Columns.Add(clm);
            gtblPosicionesClient.Columns.Add("IdEje");
            gtblPosicionesClient.Columns.Add("IdPosicion");

            DataTable TempEstructura = new DataTable();
            if (Procedencia == "O")
                TempEstructura = TempEstrucOrigen;
            else if (Procedencia == "D")
                TempEstructura = TempEstrucDestino;

            xeje = TempEstructura.Compute("Min(Eje)", "").ToString();
            while (xeje != "")
            {
                xnrollantas = Convert.ToInt32(TempEstructura.Select("Eje = '" + xeje + "'")[0]["Nrollantas"]);
                if (xeje.Substring(1, 2) != "99")
                {
                    for (int i = 11; i >= 1; i -= 2)
                    {
                        if (i <= xnrollantas)
                            NuevaFilaDPosicion(Convert.ToInt32(xeje.Substring(1, 2)), i);
                    }
                    for (int i = 2; i <= 12; i += 2)
                    {
                        if (i <= xnrollantas)
                            NuevaFilaDPosicion(Convert.ToInt32(xeje.Substring(1, 2)), i);
                    }
                }
                else
                {
                    for (int i = 12; i >= 2; i -= 2)
                    {
                        if (i <= xnrollantas)
                            NuevaFilaDPosicion(Convert.ToInt32(xeje.Substring(1, 2)), i);
                    }
                    for (int i = 1; i <= 11; i += 2)
                    {
                        if (i <= xnrollantas)
                            NuevaFilaDPosicion(Convert.ToInt32(xeje.Substring(1, 2)), i);
                    }
                }
                xeje = TempEstructura.Compute("Min(Eje)", "Eje > '" + xeje + "'").ToString();
            }
            if (Procedencia == "O")
                gtblPosicionesOrigen = gtblPosicionesClient;
            else if (Procedencia == "D")
                gtblPosicionesDestino = gtblPosicionesClient;
        }
        private string GetPosicionCliente(int IdEjeFind, int IdPosicionFind, string Procedencia)
        {
            string Posicion = string.Empty;
            if (Procedencia == "O")
                Posicion = gtblPosicionesOrigen.Select("IdEje = " + IdEjeFind + " And IdPosicion = " + IdPosicionFind)[0]["Id"].ToString();
            else if (Procedencia == "D")
                Posicion = gtblPosicionesDestino.Select("IdEje = " + IdEjeFind + " And IdPosicion = " + IdPosicionFind)[0]["Id"].ToString();
            return Posicion;
        }
        #endregion

        private void ActualizarImagenTrash()
        {
            if (gtblDesecho.Rows.Count == 0)
                imgEliminar.Source = imgTrash;
            else
                imgEliminar.Source = imgTrash_Full;
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //int IdNeumaticoTransfer = 0;
                //GlobalClass.GeneraImpresion(gintIdMenu, IdNeumaticoTransfer);

                int UC = 0;
                UC = Convert.ToInt32(cboUCOrigen.EditValue);
                if (UC > 0)
                {
                    GlobalClass.GeneraImpresion(gintIdMenu, UC);
                }
            }
            catch { }
        }
    }
}

