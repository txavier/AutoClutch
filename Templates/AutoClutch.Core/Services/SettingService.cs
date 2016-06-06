using AutoClutch.Auto.Repo.Interfaces;
using AutoClutch.Auto.Service.Services;
using $safeprojectname$.Interfaces;
using $safeprojectname$.Models;
using $safeprojectname$.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var result = Get(filter: i => i.settingKey == settingKey).SingleOrDefault();

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

        public IEnumerable<setting> Search(SearchCriteria searchCriteria, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true)
        {
            if (searchCriteria == null)
            {
                return null;
            }

            bool isSearchCriteriaSet = searchCriteria != null;

            searchCriteria.includeProperties = searchCriteria.includeProperties ?? "";

            searchCriteria.orderBy = searchCriteria.orderBy ?? "";

            var result = Get(
               filter: i => isSearchCriteriaSet || searchCriteria.searchText == null ? true : ((i.settingKey).Contains(searchCriteria.searchText) || searchCriteria.searchText.Contains(i.settingKey)),
               orderBy: j => searchCriteria.orderBy == "name" ? j.OrderBy(k => k.settingKey) : j.OrderBy(k => k.settingKey),
               skip: ((searchCriteria.currentPage - 1) ?? 1) * (searchCriteria.itemsPerPage ?? int.MaxValue),
               take: (searchCriteria.itemsPerPage ?? int.MaxValue),
               includeProperties: searchCriteria.includeProperties,
               lazyLoadingEnabled: lazyLoadingEnabled,
               proxyCreationEnabled: proxyCreationEnabled);

            return result;
        }

        public int SearchCount(SearchCriteria searchCriteria)
        {
            bool isSearchCriteriaSet = searchCriteria != null;

            var result = GetCount(
               filter: i => isSearchCriteriaSet || searchCriteria.searchText == null ? true : (i.settingKey).Contains(searchCriteria.searchText) || searchCriteria.searchText.Contains(i.settingKey));

            return result;
        }

    }
}
