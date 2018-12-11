using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OTPS.Core.Interfaces;
using OTPS.Core.Models;

namespace $safeprojectname$.Controllers
{   
    [RoutePrefix("api/BudgetAdd")]
    public class BudgetAddController : ApiController
    {

        IBudgetAddService _BudgetAddService;



        public BudgetAddController(IBudgetAddService BudgetAddService)
        {
            _BudgetAddService = BudgetAddService;
        }

        [HttpPost]
        [Route("BudgetAdd")]
        public IHttpActionResult BudgetAdd(sp_object entity)
        {
           _BudgetAddService.BudgetAdd(entity);

            return Ok();

        }


    }
}
