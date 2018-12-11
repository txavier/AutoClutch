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

    [RoutePrefix("api/contractSummary")]
    public class ContractSummaryController : ApiController
    {
        public IContractSummaryService _contractSummaryService;

        public ContractSummaryController(IContractSummaryService contractSummaryService)
        {
            _contractSummaryService = contractSummaryService;
        }

        [HttpGet]
        [Route("GetRegContract")]
        public List<ContractSummaryData> GetRegContract(string Reg_No, string start, string end)
        {
            var result = _contractSummaryService.GetRegContract(Reg_No,start,end);

            return result;
        }


        [HttpGet]
        [Route("GetPinContract")]
        public List<ContractSummaryData> GetPinContract(string Pin, string start, string end)
        {
            var result = _contractSummaryService.GetPinContract(Pin, start, end);

            return result;
        }

        [HttpGet]
        [Route("GetProjIDContract")]
        public List<ContractSummaryData> GetProjIDContract(string ProjID, string start, string end)
        {
            var result = _contractSummaryService.GetProjIDContract(ProjID, start, end);

            return result;
        }

        [HttpGet]
        [Route("GetObjectCodeContract")]
        public List<ContractSummaryData> GetObjectCodeContract(string ObjectCode, string start, string end)
        {
            var result = _contractSummaryService.GetObjectCodeContract(ObjectCode, start, end);

            return result;
        }

        [HttpGet]
        [Route("GetContractPayment({ProjNo},{ObjectCode},{Fstart},{Fend},{PStart},{PEnd})")]
        public List<ContractSummaryPay> GetContractPayment(string ProjNo,string ObjectCode, string FStart, string FEnd,string PStart,string PEnd)
        {
            if (ProjNo.Equals("null"))
            {
                ProjNo = null;
            }
            if (ObjectCode.Equals("null"))
            {
                ObjectCode = null;
            }

            var result = _contractSummaryService.GetContractPayment(ProjNo,ObjectCode, FStart, FEnd, PStart, PEnd);

            return result;
        }
    }
}