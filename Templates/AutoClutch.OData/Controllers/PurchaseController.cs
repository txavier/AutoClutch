using OTPS.Core.Interfaces;
using OTPS.Core.Models;
using OTPS.Core.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/purchase")]
    public class PurchaseController : ApiController
    {
        public IReqListService _reqListService;
        public IReqListModifier _reqListModifier;

        public PurchaseController(IReqListService reqListService, IReqListModifier reqListModifier)
        {
            _reqListService = reqListService;
            _reqListModifier = reqListModifier;
        }

        [HttpGet]
        [Route("GetPurchaseByRepCat({Rep_Cat})")]
        public List<PurchaseCenterData> GetPurchaseByRepCat(string Rep_Cat)
        {
            var result = _reqListService.GetPurchaseByRepCat(Rep_Cat);

            return result;
        }

        [HttpGet]
        [Route("GetPurchase()")]
        public List<PurchaseCenterData> GetPurchase()
        {
            var result = _reqListService.GetPurchaseByRepCat("%");

            return result;
        }

        [HttpGet]
        [Route("GetRepCatByReqKey({Req_Key})")]
        public List<PurchaseRepCatData> GetRepCatByReqKey(string Req_Key)
        {
            var result = _reqListService.GetRepCatByReqKey(Req_Key);

            return result;
        }

        [HttpGet]
        [Route("GetModBatch({Req_Key})")]
        public List<ModiBatchData> GetModBatch(string Req_Key)
        {
            var result = _reqListService.GetModiBatchData(Req_Key);

            return result;
        }

        [HttpPut]
        [Route("UpdatePurchase({Req_Key})")]
        public IHttpActionResult UpdatePurchase(ReqList entity, string Req_Key)
        {
            try
            {
                var loggedInUserName = User.Identity.Name?.Split("\\".ToCharArray()).LastOrDefault();

                var result = _reqListModifier.UpdatePurchase(entity, Req_Key, loggedInUserName);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return base.InternalServerError(ex);
            }
        }

        [HttpHead]
        [Route("CancelPurchase({Req_Key})")]
        public string CancelPurchase(string Req_Key)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            var result = _reqListModifier.CancelPurchase(Req_Key, loggedInUserName);

            return result;
        }

        [HttpPost]
        [Route("AddPurchase({Req_No})")]
        public IHttpActionResult AddPurchase(ReqList entity, string Req_No)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var loggedInUserName = User.Identity.Name?.Split("\\".ToCharArray()).LastOrDefault();
                
                var result = _reqListModifier.AddPurchase(entity, Req_No, loggedInUserName);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return base.InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("AddRepCat({Req_Key},{Type_Ord})")]
        public string AddRepCat(Batch_RepCat entity, string Req_Key, string Type_Ord)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            var result = _reqListModifier.AddRepCat(entity, Req_Key, Type_Ord, loggedInUserName);

            return result;
        }

        [HttpPut]
        [Route("UpdateRepCat({Req_Key},{Type_Ord},{Remark})")]
        public string UpdateRepCat(Batch_RepCat entity, string Req_Key, string Type_Ord, string Remark)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            var result = _reqListModifier.UpdateRepCat(entity, Req_Key, Type_Ord, Remark, loggedInUserName);
            return result;
        }

        [HttpDelete]
        [Route("DeleteRepCat({Req_Key},{Rep_Cat},{id})")]
        public string DeleteRepCat(string Req_Key, string Rep_Cat, int id)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            var result = _reqListModifier.DeleteRepCat(Req_Key, Rep_Cat, id, loggedInUserName);
            return result;
        }
    }
}
