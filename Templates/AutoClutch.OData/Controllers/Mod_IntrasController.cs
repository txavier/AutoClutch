using AutoClutch.Controller;
using AutoClutch.Core.Interfaces;
using OTPS.Core.Interfaces;
using OTPS.Core.Models;
using OTPS.Core.Objects;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace $safeprojectname$.Controllers
{
    public class Mod_IntrasController : ODataApiController<Mod_Intra>
    {
        public IService<Mod_Intra> _Mod_IntraService { get; set; }
        public IBudgetNewModService _budgetNewModService;
        public IBudgetModModifier _budgetModifierService;

        public Mod_IntrasController(IService<Mod_Intra> Mod_IntraService, ILogService<Mod_Intra> logService, IBudgetNewModService budgetNewModService, IBudgetModModifier budgetModifierService)
            : base(Mod_IntraService, logService)
        {
            _Mod_IntraService = Mod_IntraService;
            _budgetModifierService = budgetModifierService;
            _budgetNewModService = budgetNewModService;
        }

        [HttpPost]
        [ODataRoute("CreateModIntra")]
        public List<string> CreateModIntra(ODataActionParameters parameters)
        {
            BudgetNewModData result = _budgetNewModService.CreateModIntra((int)parameters["id"], (string)parameters["Line"], (decimal)parameters["Mod_Amt"]);
            List<string> resultList = new List<String>();
            resultList.Add(result.message);
            resultList.Add(result.messageType);
            return resultList;
        }

        [HttpDelete]
        [ODataRoute("DeleteModIntra(key={key},item={item})")]
        public IHttpActionResult DeleteModIntra([FromODataUri] int key, [FromODataUri] string item)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            _budgetModifierService.DeleteModIntra(key, item, loggedInUserName);

            return Ok();
        }

        [HttpDelete]
        [EnableQuery]
        [ODataRoute("DeleteIntraByKey(key={key})")]
        public IHttpActionResult DeleteIntraByKey([FromODataUri] int key)
        {
            var intraList = _Mod_IntraService.Queryable().Where(i => i.Tranfer_Key == key).ToList();
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            foreach (var singleCase in intraList)
            {
                _budgetModifierService.DeleteModIntra(key, singleCase.Item_key, loggedInUserName);
            }

            return Ok();
        }

        [HttpPut]
        [ODataRoute("UpdateModIntra")]
        public IHttpActionResult UpdateModIntra(ODataActionParameters parameters)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            _budgetModifierService.UpdateModIntra((int)parameters["key"], (string)parameters["item"], (string)parameters["newItem"], (decimal)parameters["newAmt"], loggedInUserName);

            return Ok();
        }

        [HttpPatch]
        [ODataRoute("SubmitModIntra(key={key},remark={remark})")]
        public IHttpActionResult SubmitModIntra([FromODataUri] int key, [FromODataUri] string remark)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            _budgetModifierService.SubmitModIntra(key, remark, loggedInUserName);

            return Ok();
        }
    }
}
