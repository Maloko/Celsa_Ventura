using System;

namespace Data
{
    public class D_Settings
    {
        public static String GetValueSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        }
    }
}
