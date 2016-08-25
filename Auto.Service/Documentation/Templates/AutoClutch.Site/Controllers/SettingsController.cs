using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.Core.Objects;
using $safeprojectname$.Core.Services;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/settings")]
    public class settingsController : ApiController
    {
        private readonly ISettingService _settingService;

        public settingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public IHttpActionResult Get()
        {
            var result = _settingService.Get(lazyLoadingEnabled: false, proxyCreationEnabled: false);

            return Ok(result);
        }

        [Route("{parameter}")]
        [HttpGet]
        public IHttpActionResult Get(string parameter)
        {
            int settingId;

            bool isId = int.TryParse(parameter, out settingId);

            var result = _settingService.Get(
                filter: i => isId ? i.settingId == settingId : i.settingKey == parameter,
                lazyLoadingEnabled: false,
                proxyCreationEnabled: false).SingleOrDefault();

            return Ok(result);
        }

        [Route("search")]
        [HttpGet]
        public IHttpActionResult Search([FromUri]SearchCriteria searchCriteria)
        {
            var result = _settingService.Search(searchCriteria, lazyLoadingEnabled: false, proxyCreationEnabled: false);

            return Ok(result);
        }

        [Route("search/count")]
        [HttpGet]
        public IHttpActionResult SearchCount([FromUri]SearchCriteria searchCriteria)
        {
            var result = _settingService.SearchCount(searchCriteria);

            return Ok(result);
        }

        public IHttpActionResult Post(setting setting)
        {
            _settingService.AddOrUpdate(setting, loggedInUserName: User.Identity.Name.Split('\\').LastOrDefault(), dontSave: false);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _settingService.Delete(id);

            return Ok();
        }
    }
}
