using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;


namespace Utilitarios
{

    public class TreeViewModel : INotifyPropertyChanged
    {
        TreeViewModel(string name,int id, int IdPadre,int nivel, bool Nuevo)
        {
            Name = name;
            IdMenu = id;
            IdMenuPadre = IdPadre;
            Nivel = nivel;
            BoolNuevo = Nuevo;
            Children = new List<TreeViewModel>();
        }

        #region Properties

        public string Name { get; private set; }
        public int IdMenu { get; private set; }
        public int IdMenuPadre { get; private set; }
        public int Nivel { get; private set; }
        public bool BoolNuevo { get; private set; }
        public List<TreeViewModel> Children { get; private set; }
        public bool IsInitiallySelected { get; private set; }
        

        bool? _isChecked = false;
        bool? _isEnabled = false;
        TreeViewModel _parent;
        string _fontweight = "Normal";


        #region IsChecked

        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked) return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue) Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent && _parent != null) _parent.VerifyCheckedState();

            NotifyPropertyChanged("IsChecked");
        }

        void VerifyCheckedState()
        {
            bool? state = null;

            for (int i = 0; i < Children.Count; ++i)
            {
                bool? current = Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }

            SetIsChecked(state, false, true);
        }

        #endregion

        #region IsEnabled
        
        public bool? IsEnabled
        {
            get { return _isEnabled; }
            set { SetIsEnabled(value, true, true); }
        }

        void SetIsEnabled(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isEnabled) return;

            _isEnabled = value;

            if (updateChildren && _isEnabled.HasValue) Children.ForEach(c => c.SetIsEnabled(_isEnabled, true, false));

            if (updateParent && _parent != null) _parent.VerifyEnabledState();

            NotifyPropertyChanged("IsEnabled");
        }

        void VerifyEnabledState()
        {
            bool? state = null;

            for (int i = 0; i < Children.Count; ++i)
            {
                bool? current = Children[i].IsEnabled;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }

            SetIsEnabled(state, false, true);
        }

        #endregion

        #region FontWeight

        public string FontWeight
        {
            get { return _fontweight; }
            set { SetFontWeight(value, true, true); }
        }

        void SetFontWeight(string value, bool updateChildren, bool updateParent)
        {
            if (value == "") return;

            _fontweight = value;

            //if (updateChildren) Children.ForEach(c => c.SetFontWeight(_fontweight, true, false));

            //if (updateParent && _parent != null) _parent.VerifyFontWeight();

            NotifyPropertyChanged("FontWeight");
        }

        void VerifyFontWeight()
        {
            string state = null;

            for (int i = 0; i < Children.Count; ++i)
            {
                string current = Children[i].FontWeight;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            SetFontWeight(state, false, true);
        }

        #endregion

        #endregion
                
        void Initialize()
        {
            foreach (TreeViewModel child in Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }

        //Cambie a publicas para eliminar sus datos a partir de la 2da ejecucion
        static List<TreeViewModel> treeView = new List<TreeViewModel>();
        static TreeViewModel treePadre;

        public static DataTable tblListarPerfilComponentes;
        
        

        
        public static void LimpiarDatosTreeview()
        {
            treePadre = null;
            treeView.Clear();
        }
        
        public static List<TreeViewModel> CargarDatosTreeViewPerfilComponente(int indicePadre, TreeViewModel nodePadre)
        {
           
            DataView dataViewHijos = new DataView(tblListarPerfilComponentes);
            dataViewHijos.RowFilter = tblListarPerfilComponentes.Columns["IdPerfilCompPadre"].ColumnName + " = " + indicePadre;
            
            foreach (DataRowView dataRowCurrent in dataViewHijos)
            {
                TreeViewModel nuevoNodo = new TreeViewModel(dataRowCurrent["PerfilComp"].ToString().Trim(),
                    Convert.ToInt32(dataRowCurrent["IdPerfilComp"]), Convert.ToInt32(dataRowCurrent["IdPerfilCompPadre"]), Convert.ToInt32(dataRowCurrent["Nivel"]), Convert.ToBoolean(dataRowCurrent["Nuevo"]));
                
                if (nodePadre == null)
                {
                    treeView.Add(nuevoNodo);
                    treePadre = nuevoNodo;
                }
                else
                {
                    nodePadre.Children.Add(nuevoNodo);
                }
                CargarDatosTreeViewPerfilComponente(Int32.Parse(dataRowCurrent["IdPerfilComp"].ToString()), nuevoNodo);
            }
            treePadre.Initialize();
            return treeView;
        }


        public static List<TreeViewModel> CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(int indicePadre, TreeViewModel nodePadre)
        {

            DataView dataViewHijos = new DataView(tblListarPerfilComponentes);
            dataViewHijos.RowFilter = tblListarPerfilComponentes.Columns["IdPerfilCompPadre"].ColumnName + " = " + indicePadre;
            string nroserie = "";
            int IdPerfilComp = 0;
            foreach (DataRowView dataRowCurrent in dataViewHijos)
            {
                TreeViewModel nuevoNodo = new TreeViewModel(dataRowCurrent["PerfilComp"].ToString().Trim(),
                    Convert.ToInt32(dataRowCurrent["IdPerfilComp"]), Convert.ToInt32(dataRowCurrent["IdPerfilCompPadre"]), Convert.ToInt32(dataRowCurrent["Nivel"]), Convert.ToBoolean(dataRowCurrent["Nuevo"]));
                nroserie = dataRowCurrent["NroSerie"].ToString();
                IdPerfilComp = Convert.ToInt32(dataRowCurrent["IdPerfilComp"]);
                if (nodePadre == null)
                {
                    treeView.Add(nuevoNodo);
                    treePadre = nuevoNodo;
                }
                else
                {
                    if (nroserie != "" && nuevoNodo.IdMenu == IdPerfilComp)
                    {
                        nuevoNodo.FontWeight = "Bold";
                    }
                    nodePadre.Children.Add(nuevoNodo);

                }
                CargarDatosTreeViewPerfilComponenteConSerieEnNegrita(Int32.Parse(dataRowCurrent["IdPerfilComp"].ToString()), nuevoNodo);
            }
            treePadre.Initialize();
            return treeView;
        }
        

        #region INotifyPropertyChanged Members

        void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


    }

    public class TreeViewModelCompOT : INotifyPropertyChanged
    {
        TreeViewModelCompOT(string name, int id, int idotc, int IdPadre, bool Nuevo)
        {
            Name = name;
            IdMenu = id;
            IdOTComp = idotc;
            IdMenuPadre = IdPadre;
            BoolNuevo = Nuevo;
            Children = new List<TreeViewModelCompOT>();
        }



        #region Properties

        public string Name { get; private set; }
        public int IdMenu { get; private set; }
        public int IdOTComp { get; private set; }
        public int IdMenuPadre { get; private set; }
        public bool BoolNuevo { get; private set; }
        public bool boolChecked { get; private set; }
        public List<TreeViewModelCompOT> Children { get; private set; }
        public bool IsInitiallySelected { get; private set; }
        public static DataTable tblListarPerfilComponentes;

        bool? _isChecked = false;
        TreeViewModelCompOT _parent;
        string _fontweight = "Normal";

        #region IsChecked

        public string FontWeight
        {
            get { return _fontweight; }
            set { SetFontWeight(value, true, true); }
        }

        void SetFontWeight(string value, bool updateChildren, bool updateParent)
        {
            if (value == "") return;
            _fontweight = value;
            NotifyPropertyChanged("FontWeight");
        }

        void VerifyFontWeight()
        {
            string state = null;

            for (int i = 0; i < Children.Count; ++i)
            {
                string current = Children[i].FontWeight;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            SetFontWeight(state, false, true);
        }

        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked) return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue) Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent && _parent != null) _parent.VerifyCheckedState();

            NotifyPropertyChanged("IsChecked");
        }

        void VerifyCheckedState()
        {
            bool? state = null;

            for (int i = 0; i < Children.Count; ++i)
            {
                bool? current = Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }

            SetIsChecked(state, false, true);
        }

        #endregion
        #region FontWeight

        public string FontWeight2
        {
            get { return _fontweight; }
            set { SetFontWeight2(value, true, true); }
        }

        void SetFontWeight2(string value, bool updateChildren, bool updateParent)
        {
            if (value == "") return;

            _fontweight = value;

            //if (updateChildren) Children.ForEach(c => c.SetFontWeight(_fontweight, true, false));

            //if (updateParent && _parent != null) _parent.VerifyFontWeight();

            NotifyPropertyChanged("FontWeight");
        }

        void VerifyFontWeight2()
        {
            string state = null;

            for (int i = 0; i < Children.Count; ++i)
            {
                string current = Children[i].FontWeight2;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            SetFontWeight2(state, false, true);
        }

        #endregion
        #endregion

        void Initialize()
        {
            foreach (TreeViewModelCompOT child in Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }

        static List<TreeViewModelCompOT> treeView = new List<TreeViewModelCompOT>();
        static TreeViewModelCompOT treePadre;

        public static void LimpiarDatosTreeview()
        {
            treePadre = null;
            treeView.Clear();
        }

        public static List<TreeViewModelCompOT> CargarDatosTreeViewPerfilComponente(int indicePadre, TreeViewModelCompOT nodePadre)
        {

            DataView dataViewHijos = new DataView(tblListarPerfilComponentes);
            dataViewHijos.RowFilter = tblListarPerfilComponentes.Columns["IdPerfilCompPadre"].ColumnName + " = " + indicePadre;
            string nroserie = "";
            int IdPerfilComp = 0;
            foreach (DataRowView dataRowCurrent in dataViewHijos)
            {
                TreeViewModelCompOT nuevoNodo = new TreeViewModelCompOT(dataRowCurrent["PerfilComp"].ToString().Trim(),
                    Convert.ToInt32(dataRowCurrent["IdPerfilComp"]), Convert.ToInt32(dataRowCurrent["IdOTComp"]), Convert.ToInt32(dataRowCurrent["IdPerfilCompPadre"]), Convert.ToBoolean(dataRowCurrent["Nuevo"]));

                nroserie = dataRowCurrent["NroSerie"].ToString();
                IdPerfilComp = Convert.ToInt32(dataRowCurrent["IdPerfilComp"]); 
                if (nodePadre == null)
                {
                    treeView.Add(nuevoNodo);
                    treePadre = nuevoNodo;
                }
                else
                {
                    if (nroserie != "" && nuevoNodo.IdMenu == IdPerfilComp)
                    {
                        nuevoNodo.FontWeight2 = "Bold";
                    }
                    nodePadre.Children.Add(nuevoNodo);
                }
                CargarDatosTreeViewPerfilComponente(Int32.Parse(dataRowCurrent["IdPerfilComp"].ToString()), nuevoNodo);
            }
            treePadre.Initialize();
            return treeView;
        }




        #region INotifyPropertyChanged Members

        void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
