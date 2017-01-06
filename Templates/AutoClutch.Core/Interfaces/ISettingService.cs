using AutoClutch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Interfaces
{
    public interface ISettingService : IService<$safeprojectname$.Models.setting>
    {
        string GetSettingValueBySettingKey(string settingKey);
    }
}
