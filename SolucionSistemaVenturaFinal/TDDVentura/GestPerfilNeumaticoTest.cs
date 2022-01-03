using AplicacionSistemaVentura.PAQ01_Definicion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AplicacionSistemaVentura;
using System.Data;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Input;
using DevExpress.Xpf.Grid;

namespace TDDVentura
{
    
    
    /// <summary>
    ///This is a test class for GestPerfilNeumaticoTest and is intended
    ///to contain all GestPerfilNeumaticoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GestPerfilNeumaticoTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GestPerfilNeumatico Constructor
        ///</summary>
        [TestMethod()]
        public void GestPerfilNeumaticoConstructorTest()
        {
            GestPerfilNeumatico target = new GestPerfilNeumatico();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GestPerfilNeumatico Constructor
        ///</summary>
        [TestMethod()]
        public void GestPerfilNeumaticoConstructorTest1()
        {
            InterfazPrincipal p = null; // TODO: Initialize to an appropriate value
            GestPerfilNeumatico target = new GestPerfilNeumatico(p);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ComboEnGrilla
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void ComboEnGrillaTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            DataTable tblDetalleEjeresult = null; // TODO: Initialize to an appropriate value
            target.ComboEnGrilla(tblDetalleEjeresult);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DetallePerfilNeumatico
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void DetallePerfilNeumaticoTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            target.DetallePerfilNeumatico();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for EstadoEdicion
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void EstadoEdicionTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            target.EstadoEdicion();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for EstadoForm
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void EstadoFormTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            target.EstadoForm();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for InitializeComponent
        ///</summary>
        [TestMethod()]
        public void InitializeComponentTest()
        {
            GestPerfilNeumatico target = new GestPerfilNeumatico(); // TODO: Initialize to an appropriate value
            target.InitializeComponent();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LimpiarControles
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void LimpiarControlesTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            target.LimpiarControles();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LimpiarGrid
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void LimpiarGridTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            Grid grilla = null; // TODO: Initialize to an appropriate value
            target.LimpiarGrid(grilla);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ListarPerfilNeumatico
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void ListarPerfilNeumaticoTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            target.ListarPerfilNeumatico();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Mensaje
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void MensajeTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            string strMensaje = string.Empty; // TODO: Initialize to an appropriate value
            int intTipo = 0; // TODO: Initialize to an appropriate value
            target.Mensaje(strMensaje, intTipo);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OnFocus
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void OnFocusTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OnFocus(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OutFocus
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void OutFocusTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.OutFocus(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for PintarEje
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void PintarEjeTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            int intNroEjes = 0; // TODO: Initialize to an appropriate value
            target.PintarEje(intNroEjes);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for PintarLlantaRepuesto
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void PintarLlantaRepuestoTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            int intLR = 0; // TODO: Initialize to an appropriate value
            target.PintarLlantaRepuesto(intLR);
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for PintarNeumatico
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void PintarNeumaticoTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            string strEje = string.Empty; // TODO: Initialize to an appropriate value
            int Llantas = 0; // TODO: Initialize to an appropriate value
            target.PintarNeumatico(strEje, Llantas);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for System.Windows.Markup.IComponentConnector.Connect
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void ConnectTest()
        {
            IComponentConnector target = new GestPerfilNeumatico(); // TODO: Initialize to an appropriate value
            int connectionId = 0; // TODO: Initialize to an appropriate value
            object target1 = null; // TODO: Initialize to an appropriate value
            target.Connect(connectionId, target1);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for System.Windows.Markup.IStyleConnector.Connect
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void ConnectTest1()
        {
            IStyleConnector target = new GestPerfilNeumatico(); // TODO: Initialize to an appropriate value
            int connectionId = 0; // TODO: Initialize to an appropriate value
            object target1 = null; // TODO: Initialize to an appropriate value
            target.Connect(connectionId, target1);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UserControl_Loaded
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void UserControl_LoadedTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.UserControl_Loaded(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ValidaCampoObligado
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void ValidaCampoObligadoTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ValidaCampoObligado();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ValidaLogicaNegocio
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void ValidaLogicaNegocioTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ValidaLogicaNegocio();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for btnCancelar_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void btnCancelar_ClickTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.btnCancelar_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for btnEditar_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void btnEditar_ClickTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.btnEditar_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for btnGrabar_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void btnGrabar_ClickTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.btnGrabar_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for btnNuevo_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void btnNuevo_ClickTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.btnNuevo_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for cboLlanRepu_SelectedIndexChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void cboLlanRepu_SelectedIndexChangedTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.cboLlanRepu_SelectedIndexChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for cboNroEje_SelectedIndexChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void cboNroEje_SelectedIndexChangedTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.cboNroEje_SelectedIndexChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for cboNroLlanta_SelectedIndexChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void cboNroLlanta_SelectedIndexChangedTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.cboNroLlanta_SelectedIndexChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for dtgPerfNeum_MouseDoubleClick
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void dtgPerfNeum_MouseDoubleClickTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            MouseButtonEventArgs e = null; // TODO: Initialize to an appropriate value
            target.dtgPerfNeum_MouseDoubleClick(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for tbvPerfNeumEje_ShowingEditor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void tbvPerfNeumEje_ShowingEditorTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            ShowingEditorEventArgs e = null; // TODO: Initialize to an appropriate value
            target.tbvPerfNeumEje_ShowingEditor(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for txtDesc_KeyUp
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AplicacionSistemaVentura.exe")]
        public void txtDesc_KeyUpTest()
        {
            GestPerfilNeumatico_Accessor target = new GestPerfilNeumatico_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            KeyEventArgs e = null; // TODO: Initialize to an appropriate value
            target.txtDesc_KeyUp(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
