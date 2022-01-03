using System;
using Data;

namespace Business
{
    public class B_Settings
    {
        public string GetValueSetting(String key)
        {
            return D_Settings.GetValueSetting(key);
        }
    }
}
