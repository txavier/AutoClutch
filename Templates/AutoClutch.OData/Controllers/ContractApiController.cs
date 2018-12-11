using AutoClutch.Core.Interfaces;
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
    [RoutePrefix("api/contract")]
    public class ContractApiController : ApiController
    {
        public IContractModifier _contractModifier;
        public IContractService _contractService;

        public ContractApiController(IContractModifier contractModifier, IContractService contractService)
        {
            _contractModifier = contractModifier;
            _contractService = contractService;
        }

        [HttpGet]
        [Route("GetContractAmounts(id={id},start={start},end={end})")]
        public ContractAmountData GetContractAmounts(int id, string start, string end)
        {
            var result = _contractService.getContractAmounts(id, start, end);

            return result;
        }

        [HttpPut]
        [Route("UpdateContract({id})")]
        public string UpdateContract(int id, Contract entity)
        {
            var currentUser = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();
            var result = _contractModifier.UpdateContract(id, currentUser, entity);

            return result;
        }

        [HttpPut]
        [Route("UpdateCTModHistory({id})")]
        public string UpdateCTModHistory(int id, CT_Mod_History entity)
        {
            var currentUser = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();
            var result = _contractModifier.UpdateCTModHistory(id, currentUser, entity);

            return result;
        }

        [HttpPost]
        [Route("CreateCTModHistory")]
        public string CreateCTModHistory(CT_Mod_History entity)
        {
            var currentUser = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();
            var result = _contractModifier.CreateCTModHistory(entity, currentUser);

            return result;
        }

        [HttpDelete]
        [Route("DeleteCTModHistory({id})")]
        public string DeleteCTModHistory(int id)
        {
            var currentUser = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();
            var result = _contractModifier.DeleteCTModHistory(id, currentUser);

            return result;
        }

        [HttpPost]
        [Route("CreateContractPay")]
        public string CreateContractPay(contract_pay entity)
        {
            var currentUser = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();
            var result = _contractModifier.CreateContractPay(entity, currentUser);

            return result;
        }

        [HttpDelete]
        [Route("DeleteContractPay")]
        public string DeleteContractPay(int id)
        {
            var currentUser = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();
            var result = _contractModifier.DeleteContractPay(id, currentUser);

            return result;
        }

        [HttpPut]
        [Route("UpdateContractPay({id})")]
        public string UpdateContractPay(int id, contract_pay entity)
        {
            var currentUser = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();
            var result = _contractModifier.UpdateContractPay(id, currentUser, entity);

            return result;
        }

    }
}
