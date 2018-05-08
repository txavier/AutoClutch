using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
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
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        private IContractService _contractService;
        private IMetricService _metricService;
        private IReceivingReportService _receivingReportService;

        public DashboardController(IMetricService metricService, IContractService contractService, IReceivingReportService receivingReportService)
        {
            _metricService = metricService;

            _contractService = contractService;

            _receivingReportService = receivingReportService;
        }

        [HttpGet]
        public IHttpActionResult Get(string name, int? loggedInUserId = null)
        {
            switch(name)
            {
                case "contractTotalsPerSection":
                    {
                        var result = _metricService.QueryContractTotalsPerSection();

                        return Ok(result);
                    }
                case "contractsPerEngineer":
                    {
                        var result = _metricService.QueryContractsPerEngineerViewModelsByYear(new int[] { DateTime.Now.Year - 1, DateTime.Now.Year });

                        return Ok(result);
                    }
                case "workOrdersInCurrentSectionPerEngineer":
                    {
                        var result = _metricService.QueryWorkOrdersInCurrentSectionPerEngineer(User.Identity.Name.Split("\\".ToCharArray()).Last());

                        return Ok(result);
                    }
                case "workOrdersInCurrentSectionPerContractByEngineer":
                    {
                        var result = _metricService.QueryWorkOrdersInCurrentSectionPerContractByEngineer(loggedInUserId);

                        return Ok(result);
                    }
                case "expiringContractsCount":
                    {
                        var result = _contractService.QueryExpiringContractCountBySectionOfLoggedInUser(User.Identity.Name.Split("\\".ToCharArray()).Last(), loggedInUserId.HasValue);

                        return Ok(result);
                    }
                case "lowFundContractsCount":
                    {
                        var result = _contractService.QueryLowFundContractCountBySectionOfLoggedInUser(User.Identity.Name.Split("\\".ToCharArray()).Last(), loggedInUserId.HasValue);

                        return Ok(result);
                    }
                case "expiringContracts":
                    {
                        var result = _contractService.QueryExpiringContractBySectionOfLoggedInUser(User.Identity.Name.Split("\\".ToCharArray()).Last(), loggedInUserId.HasValue);

                        return Ok(result);
                    }
                case "lowFundContracts":
                    {
                        var result = _contractService.QueryLowFundContractBySectionOfLoggedInUser(User.Identity.Name.Split("\\".ToCharArray()).Last(), loggedInUserId.HasValue);

                        return Ok(result);
                    }
                case "missingSpec":
                    {
                        var result = _contractService.QueryMissingSpecBySectionOfLoggedInUser(User.Identity.Name.Split("\\".ToCharArray()).Last(), loggedInUserId.HasValue);

                        return Ok(result);
                    }
                case "searchProcurementReceivingReportsCount":
                    {
                        if(!loggedInUserId.HasValue)
                        {
                            return BadRequest("Unable to continue getting the receiving report count, the userId has not been found");
                        }

                        var result = _receivingReportService.GetOpenReceivingReportsCount(loggedInUserId.Value);

                        return Ok(result);
                    }
                default:
                    {
                        break;
                    }
            }

            return NotFound();
        }

    }
}
