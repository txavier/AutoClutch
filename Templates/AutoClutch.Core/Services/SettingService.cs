using AutoClutch.Repo.Interfaces;
using $safeprojectname$.Interfaces;
using $safeprojectname$.Models;
using $safeprojectname$.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClutch.Core;

namespace $safeprojectname$.Services
{
    public class SettingService : Service<setting>, ISettingService
    {
        private readonly IRepository<setting> _settingRepository;

        public SettingService(IRepository<setting> settingRepository)
            : base(settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public setting GetSettingBySettingKey(string settingKey)
        {
            var result = base.Get(filter: i => i.settingKey == settingKey).SingleOrDefault();

            return result;
        }

        public string GetSettingValueBySettingKey(string settingKey)
        {
            //if (settingKey == "version")
            //{
            //    var version = _systemSettingsService.GetProductVersion();

            //    return version;
            //}

            var result = GetSettingBySettingKey(settingKey);

            var value = result.settingValue;

            return value;
        }

    }
}
