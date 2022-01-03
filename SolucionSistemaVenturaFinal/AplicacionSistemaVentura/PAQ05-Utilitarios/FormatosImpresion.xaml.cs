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
using CrystalDecisions.CrystalReports.Engine;
using Business;
using Entities;
using Utilitarios;
using DevExpress.Xpf.Grid;
using System.IO;


namespace AplicacionSistemaVentura.PAQ05_Utilitarios
{
    /// <summary>
    /// Interaction logic for FormatosImpresion.xaml
    /// </summary>
    public partial class FormatosImpresion : UserControl
    {
        B_Menu B_Menu = new B_Menu();
        E_Menu E_Menu = new E_Menu();
        MenuPadre MenuP = new MenuPadre();
        MenuHijo MenuH = new MenuHijo();
        B_FormatoImpresion B_FormatosImpresion = new B_FormatoImpresion();
        E_FormatoImpresion E_FormatosImpresion = new E_FormatoImpresion();
        E_TablaMaestra objTablaMaestra = new E_TablaMaestra();
        ErrorHandler objError = new ErrorHandler();
        DataTable gtblMenuPadre = new DataTable();
        DataTable gtblMenuHijo = new DataTable();
        DataTable gtblFormatosImpresion = new DataTable();
        DataView dtv_TablaMaestra = new DataView();
        string sourcePath;
        string FileName;
        string SourceFile;
        int IdMenu = -1;

        public FormatosImpresion()
        {
            InitializeComponent();
            UserControl_Loaded();                                   
        }

        public class MenuPadre
        {
            public String Menu { get; set; }
            public int IdMenu { get; set; }    
        }

        public class MenuHijo
        {
            public String Menu { get; set; }
            public int IdMenu { get; set; }    
        }

        private TreeListNode CreateRootNode(object dataObject)
        {
            TreeListNode rootNode = new TreeListNode(dataObject);                               
            treeListView1.Nodes.Add(rootNode);                  
            return rootNode;
        }

        private TreeListNode CreateChildNode(TreeListNode parentNode, object dataObject)
        {
            TreeListNode childNode = new TreeListNode(dataObject);                       
            parentNode.Nodes.Add(childNode);                        
            return childNode;
        }

        private void UserControl_Loaded()
        {
            try
            {                   
                gtblMenuPadre = new DataTable();
                gtblMenuPadre.Columns.Add("ResultadoRetorno", Type.GetType("System.Int32"));
                gtblMenuPadre.Columns.Add("IdMenuPadre", Type.GetType("System.Int32"));
                gtblMenuPadre.Columns.Add("MenuPadre", Type.GetType("System.String"));



                gtblFormatosImpresion = new DataTable();                
                gtblFormatosImpresion.Columns.Add("Id_FormatoImpresion", Type.GetType("System.Int32"));
                gtblFormatosImpresion.Columns.Add("NombreArchivo", Type.GetType("System.String"));

                LoadMenuOpciones();                

            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void LoadMenuOpciones()
        {
            try
            {
                int TipoLecturaPadre = 1;                
                DataTable tblMenuPadre = B_Menu.Menu_ListaOpciones(TipoLecturaPadre);
                treeListControl1.ItemsSource = tblMenuPadre;                                       
            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnCargarDocumento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                
                dlg.Filter = "Crystal Report Files (*.rpt)|*.rpt";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    FileName = dlg.SafeFileName;
                    sourcePath = System.IO.Path.GetDirectoryName(dlg.FileName);
                    SourceFile = System.IO.Path.Combine(sourcePath, FileName);
                    textEdit2.Text = FileName;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnEliminarFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                E_FormatoImpresion e_FormatoImpresion = new E_FormatoImpresion();
                object Item = dtgFormatoImpresion.SelectedItem;
                DataRowView Node = (DataRowView)Item;
                e_FormatoImpresion.IdFormatoImpresion = Convert.ToInt32(Node.Row[0].ToString());
                e_FormatoImpresion.Flagactivo = 0;
                e_FormatoImpresion.Idusuariomodificacion = 1;
                e_FormatoImpresion.NombreArchivo = (Node.Row[1].ToString());
                int Result = B_FormatoImpresion.FormatoImpresion_Update(e_FormatoImpresion);

                if (Result != 1)
                {
                    GlobalClass.ip.Mensaje("No se elimino el formato", 3);
                    UserControl_Loaded();
                }
                else
                {
                    GlobalClass.ip.Mensaje("Se elimino correctamente el formato", 1);
                }

            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void btnDownloadFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {               
                object Item =  dtgFormatoImpresion.SelectedItem;
                DataRowView Node = (DataRowView)Item;                
                int IdFormatoImpresion = Convert.ToInt32(Node.Row[0].ToString());
                DataTable tblFile = B_FormatoImpresion.FormatoImpresion_GetFile(IdFormatoImpresion);

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = tblFile.Rows[0]["NombreArchivo"].ToString();                
                dlg.Filter = "Crystal Report Files (*.rpt)|*.rpt";
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    byte[] objData;
                    objData = (byte[])tblFile.Rows[0]["File"];
                    string sourcePathSave = System.IO.Path.GetDirectoryName(dlg.FileName);
                    string SourceFileSave = System.IO.Path.Combine(sourcePathSave, dlg.FileName);
                    FileStream objFileStream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write);
                    objFileStream.Write(objData, 0, objData.Length);
                    objFileStream.Close();
                    GlobalClass.ip.Mensaje("Archivo Descargado", 1);
                }
            }

            catch (Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }
        }

        private void btnImportar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int Result = -1;

                FileStream fs = new FileStream(SourceFile, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                br.Close();
                fs.Close();

                if (string.IsNullOrEmpty(Convert.ToString(IdMenu)))
                {
                    GlobalClass.ip.Mensaje("Seleccione una opción del menú a registrar", 1);
                }
                else
                {
                    E_FormatosImpresion.IdMenu = IdMenu;
                    E_FormatosImpresion.NombreArchivo = textEdit1.Text;
                    E_FormatosImpresion.Flagactivo = 1;
                    E_FormatosImpresion.Idusuariocreacion = 1;
                    E_FormatosImpresion.File = bytes;

                    Result = B_FormatosImpresion.FormatoImpresion_Insert(E_FormatosImpresion);

                    if (Result != -1)
                    {
                        GlobalClass.ip.Mensaje("Se cargo correctamente el formato", 1);
                        textEdit1.Text = "";
                        textEdit2.Text = "";
                    }
                    else
                    {
                        GlobalClass.ip.Mensaje("Se generó un error al momento de registrar el formato", 3);
                    }
                }
                
            }

            catch(Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");         
            }
        }   
       
        private void LoadGridFormatosImpresion(int IdMenu)
        { 
            try
            {
                gtblFormatosImpresion.Rows.Clear();
                DataTable tblFormatosImpresion = B_FormatoImpresion.FormatoImpresion_GetItem(IdMenu);

                for (int i = 0; i < tblFormatosImpresion.Rows.Count; i++)
                {
                    DataRow Fila = gtblFormatosImpresion.NewRow();                    
                    Fila["Id_FormatoImpresion"] = Convert.ToInt32(tblFormatosImpresion.Rows[i]["Id_FormatoImpresion"]);
                    Fila["NombreArchivo"] = tblFormatosImpresion.Rows[i]["NombreArchivo"];
                    gtblFormatosImpresion.Rows.Add(Fila);                    
                }

                dtgFormatoImpresion.ItemsSource = gtblFormatosImpresion;
            }

            catch(Exception ex)
            {
                GlobalClass.ip.Mensaje(ex.Message, 3);
                objError.EscribirError(ex.Data.ToString(), ex.Message, ex.Source, ex.StackTrace, ex.TargetSite.ToString(), "", "", "");
            }

        }

        private void treeListView1_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            treeListView1.NodeCheckStateChanged -= new DevExpress.Xpf.Grid.TreeList.TreeListNodeEventHandler(treeListView1_NodeCheckStateChanged);
            SetCheckedChilNodes(e.Node, e.Node.IsChecked);
            SetCheckedParentNodes(e.Node, e.Node.IsChecked);
            treeListView1.NodeCheckStateChanged += new DevExpress.Xpf.Grid.TreeList.TreeListNodeEventHandler(treeListView1_NodeCheckStateChanged);
        }

        private void SetCheckedChilNodes(TreeListNode nodo, bool? Checkado)
        {
            for (int i = 0; i < nodo.Nodes.Count; i++)
            {
                nodo.Nodes[i].IsChecked = Checkado;
                SetCheckedChilNodes(nodo.Nodes[i], Checkado);
            }
        }

        private void SetCheckedParentNodes(TreeListNode nodo, bool? Checkado)
        {
            //Si todos los hijos del Padre tienen la misma propiedad IsChecked (True/False),
            //entonces el padre tendrá lo tendra tambien, encaso contrario lo Seteamos con 'null'
            if (nodo.ParentNode == null) return;
            bool esTodoIgual = true;
            for (int i = 0; i < nodo.ParentNode.Nodes.Count; i++)
            {
                bool? state = nodo.ParentNode.Nodes[i].IsChecked;
                if (Checkado != state)
                {
                    esTodoIgual = false;
                    break;
                }
            }
            nodo.ParentNode.IsChecked = esTodoIgual ? Checkado : null;
            SetCheckedParentNodes(nodo.ParentNode, Checkado);
        }
            
        private void treeListControl1_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            string NodeName = string.Empty;
            
            object Row = treeListView1.FocusedNode.Content;
            DataRowView Node = (DataRowView)Row;
            NodeName = Node.Row[2].ToString();
            IdMenu = Convert.ToInt32(Node.Row[1].ToString());                    
            label4.Content = NodeName;
            LoadGridFormatosImpresion(IdMenu);
        }

    }
}
