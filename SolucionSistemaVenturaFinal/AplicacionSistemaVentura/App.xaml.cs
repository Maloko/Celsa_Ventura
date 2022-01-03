using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;


namespace AplicacionSistemaVentura
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            base.OnStartup(e);
            EcriptarAppConfig();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            DevExpress.Xpf.Core.DXMessageBox.Show(e.ExceptionObject.ToString());
        }
        void EcriptarAppConfig()
        {
            EncriptarSecciones("appSettings");
            EncriptarSecciones("connectionStrings");
        }
        void EncriptarSecciones(string NombreSeccion)
        {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection section = conf.GetSection(NombreSeccion);
            if (section != null)
            {
                if (!section.IsReadOnly())
                {
                    if (!section.SectionInformation.IsProtected)
                    {
                        section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                        section.SectionInformation.ForceSave = true;
                        conf.Save(ConfigurationSaveMode.Full);
                    }
                }
            }
        }
    }
}