using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NerdLunch.Core.Interfaces;

namespace $safeprojectname$.Getters
{
    public class ConfigSettingsGetter : IConfigSettingsGetter
    {
        public string GetVersion()
        {
            string result = ConfigurationManager.AppSettings.Get("version");

            return result;
        }
    }
}
