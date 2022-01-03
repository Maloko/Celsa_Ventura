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
using System.Windows.Shapes;
using System.ComponentModel;
using DevExpress.Xpf.Core;
using System.Windows.Threading;
using System.Threading;

namespace AplicacionSistemaVentura
{
    /// <summary>
    /// Lógica de interacción para Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        InterfazPrincipal ip = new InterfazPrincipal();
        int gintTick_Mostrar = 0;

        public Loading()
        {
            InitializeComponent();
            Window_Loaded();
        }
        private void Window_Loaded()
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            gintTick_Mostrar += 1;
            if (gintTick_Mostrar == 3)
            {
                ip.Show();
                this.Close();
                timer.Stop();
            }
        }
    }
}
