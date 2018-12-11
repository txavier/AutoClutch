using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OTPS.Core.Interfaces;
using OTPS.Core.Objects;

namespace $safeprojectname$.Controllers

{


    [RoutePrefix("api/Mod_History")]
    public class CT_Mod_HistoryTotalController : ApiController
    {
        private ICT_Mod_HistoryTotalService _CT_ModHistorySearchBudObjService;
        private ICT_Mod_HistoryTotalService _CT_ModHistorySearchBudObjServicePSR;

        public CT_Mod_HistoryTotalController(ICT_Mod_HistoryTotalService CTModHistorySearchBugObjService, ICT_Mod_HistoryTotalService CTModHistorySearchBugObjServicePSR)
        {
            _CT_ModHistorySearchBudObjService = CTModHistorySearchBugObjService;
            _CT_ModHistorySearchBudObjServicePSR = CTModHistorySearchBugObjServicePSR;
        }


        //CT_Total_Po

        [HttpGet]
        [Route("CMHBudObj({object_code})")]

        public List<CT_TotalPo> CTModHistorySearchBugObj(string object_code)
        {
            var result = _CT_ModHistorySearchBudObjService.CTModHistorySearchBudObj(object_code);


            return result;

        }

        //CT_Open_Req
        [HttpGet]
        [Route("CMHBudObjPSR({object_code})")]

        public List<CT_OpenReq> CTModHistorySearchBugObjPSR(string object_code)
        {
            var result = _CT_ModHistorySearchBudObjService.CTModHistorySearchBudObjPSR(object_code);


            return result;

        }
        //CT_Total_po_Line
        [HttpGet]
        [Route("CMHBudObjLine({object_line})")]

        public List<CT_TotalPoLine> CTModHistorySearchBugObjLine(string object_line)
        {
            var result = _CT_ModHistorySearchBudObjService.CTModHistorySearchBudObjLine(object_line);


            return result;

        }
        //CT_Open_req_line
        [HttpGet]
        [Route("CMHBudObjPSRLine({object_line})")]

        public List<CT_OpenReqLine> CTModHistorySearchBugObjPSRLine(string object_line)
        {
            var result = _CT_ModHistorySearchBudObjService.CTModHistorySearchBudObjPSRLine(object_line);


            return result;

        }

    }





}
