using OTPS.Core.Interfaces;
using OTPS.Core.Models;
using OTPS.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/budgetmodificationhistory")]
    public class BudgetModHistoryController : ApiController
    {
        IBudgetModHistoryService _budgetModHistoryService;
        IBudgetModUpdater _budgetModUpdater;

        public BudgetModHistoryController(IBudgetModHistoryService budgetModHistoryService, IBudgetModUpdater budgetModUpdater)
        {
            _budgetModHistoryService = budgetModHistoryService;
            _budgetModUpdater = budgetModUpdater;
        }

        [HttpGet]
        [Route("getBudgetObjectModification({Transfer_Key})")]
        public List<BudgetObjectModificationData> GetBudgetObjectModification(int Transfer_Key)
        {
            var result = _budgetModHistoryService.GetBudgetObjectModificationData(Transfer_Key);

            return result;
        }

        [HttpGet]
        [Route("getLineModification({Transfer_Key})")]
        public List<LineModificationData> GetLineModification(int Transfer_Key)
        {
            var result = _budgetModHistoryService.GetLineModificationData(Transfer_Key);

            return result;
        }

        //import BudgetModHistoryData obj
        [HttpPut]
        [Route("updateBudgetModificationHistory({transfer_key},{remark})")]
        public IHttpActionResult UpdateLineModification(int transfer_key, string remark, List<BudgetModHistoryData> data)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            _budgetModUpdater.UpdateBudgetMod(transfer_key, remark, data, loggedInUserName);

            return Ok();
        }
    }
}