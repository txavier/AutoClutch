using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OTPS.Core.Interfaces;

namespace $safeprojectname$.Controllers
{

    [RoutePrefix("api/BudgetDelete")]
    public class BudgetDeleteController : ApiController
    {

        IBudgetDeleteService _BudgetDeleteService;


        public BudgetDeleteController(IBudgetDeleteService BudgetDeleteService)
        {
            _BudgetDeleteService = BudgetDeleteService;
        }


        [HttpDelete]
        [Route("BudgetDelete(Key={Key})")]

         public void BudgetDelete(string key)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            _BudgetDeleteService.BudgetDelete(key, loggedInUserName);
        }
    }
}
