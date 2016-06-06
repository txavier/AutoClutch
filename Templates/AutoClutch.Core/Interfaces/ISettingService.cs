using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Interfaces
{
    public interface ISettingService : AutoClutch.Auto.Service.Interfaces.IService<$safeprojectname$.Models.setting>
    {
        string GetSettingValueBySettingKey(string settingKey);

        System.Collections.Generic.IEnumerable<$safeprojectname$.Models.setting> Search($safeprojectname$.Objects.SearchCriteria searchCriteria, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true);

        int SearchCount($safeprojectname$.Objects.SearchCriteria searchCriteria);
    }
}
