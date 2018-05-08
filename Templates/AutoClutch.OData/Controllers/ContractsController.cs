 using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Objects;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/contracts")]
    public class ContractsController : BaseApiController<contract>
    {
        private IContractService _contractService;

        private ILogService<contract> _logService;

        public ContractsController(IContractService contractService, ILogService<contract> logService)
            : base(contractService, logService)
        {
            _contractService = contractService;

            _logService = logService;
        }

        [Route("count")]
        [HttpGet]
        public IHttpActionResult Count(string q = null)
        {
            var result = base.BaseCount(q);

            return Ok(result);
        }

        //// http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api#requirements
        //// For sorting http://jasonwatmore.com/post/2014/07/16/Dynamic-LINQ-Using-strings-to-sort-by-properties-and-child-object-properties.aspx
        //[HttpGet]
        //public IHttpActionResult GetTest(
        //    string sort = null,
        //    string expand = null,
        //    string fields = null,
        //    string q = null,
        //    int page = 1,
        //    int perPage = Int32.MaxValue,
        //    string search = null,
        //    string searchFields = null,
        //    bool count = false)
        //{
        //    try
        //    {
        //        var skip = (page - 1) * perPage;

        //        // If sort is descending then change the '-' sign which represents the descending order
        //        // to the word 'descending' which is used by AutoService.
        //        sort = (!string.IsNullOrWhiteSpace(sort) && sort.Contains('-')) ? sort.TrimStart("-".ToCharArray()) + " descending" : sort;

        //        if (search != null)
        //        {
        //            // contractNumber.Contains("20151427556") and section.sectionId=1
        //            q = GetDynamicQuery(search, searchFields, q, typeof(contract));
        //        }

        //        if (count)
        //        {
        //            var countResult = _contractService.Queryable().Where(q ?? "1 = 1").Count();

        //            return Ok(countResult);
        //        }

        //        var result = _contractService.Get(
        //            skip: skip,
        //            take: perPage,
        //            includeProperties: expand,
        //            filterString: q,
        //            orderByString: sort,
        //            lazyLoadingEnabled: false,
        //            proxyCreationEnabled: false,
        //            loggedInUserName: User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault());

        //        //var result = fields == null ? _service.Queryable().Where(q ?? "1 = 1").OrderBy(sort).Skip(skip).Take(perPage)
        //        //    : _service.Queryable().Where(q ?? "1 = 1").OrderBy(sort).Include("engineerContracts.engineer").Select("new(" + fields + ")").Skip(skip).Take(perPage);

        //        // Get any errors.
        //        var errors = _contractService.Errors;

        //        if (errors.Any())
        //        {
        //            return RetrieveErrorResult(errors);
        //        }

        //        if (fields != null)
        //        {
        //            if (expand != null)
        //            {
        //                var expandedObjects = expand.Split(new char[] { ',' });

        //                List<string> resultingObjects = new List<string>();

        //                // Filter out all of the . object navigational properties.
        //                // We cannot select on these but this is ok since they are
        //                // already included from the previous result set.
        //                foreach (var expandedObject in expandedObjects)
        //                {
        //                    if (expandedObject.Split(new char[] { '.' }).Count() > 1)
        //                    {
        //                        resultingObjects.Add(expandedObject.Split(new char[] { '.' }).First());
        //                    }
        //                    else
        //                    {
        //                        resultingObjects.Add(expandedObject);
        //                    }
        //                }

        //                var selectedResult = result.Select("new(" + fields + ")", expandedObjects);

        //                return Ok(selectedResult);
        //            }
        //            else
        //            {
        //                var selectedResult = result.Select("new(" + fields + ")");

        //                return Ok(selectedResult);
        //            }

        //        }
        //        else
        //        {
        //            List<string> resultingObjects = new List<string>();

        //            if (expand != null)
        //            {
        //                var expandedObjects = expand.Split(new char[] { ',' });

        //                // Filter out all of the . object navigational properties.
        //                // We cannot select on these but this is ok since they are
        //                // already included from the previous result set.
        //                foreach (var expandedObject in expandedObjects)
        //                {
        //                    if (expandedObject.Split(new char[] { '.' }).Count() > 1)
        //                    {
        //                        resultingObjects.Add(expandedObject.Split(new char[] { '.' }).First());
        //                    }
        //                    else
        //                    {
        //                        resultingObjects.Add(expandedObject);
        //                    }
        //                }
        //            }

        //            var expandedObjectsString = expand == null ? string.Empty : "," + resultingObjects.Aggregate((current, next) => current + ", " + next);

        //            // If there arent any items then return the empty set.
        //            if (!result.Any())
        //            {
        //                return Ok(result);
        //            }

        //            // Add all of this objects base properties if there were no fields specified.
        //            fields = result.First().GetType().GetProperties().Select(i => i.Name).Aggregate((current, next) => current + ", " + next) + expandedObjectsString;

        //            // Get distinct fields so that there is no overlap.
        //            fields = fields.Split(",".ToCharArray()).Distinct().Aggregate((current, next) => current + ", " + next);

        //            var selectedResult = result.Select("new(" + fields + ")");

        //            return Ok(selectedResult);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorSignal.FromCurrentContext().Raise(ex);

        //        return InternalServerError(ex);
        //    }
        //}

        [Route("getContractId")]
        [HttpGet]
        public IHttpActionResult GetContractId(string contractNumber)
        {
            var result = _contractService.Queryable().Where(i => i.contractNumber == contractNumber);

            if(result.Count() > 1)
            {
                return base.RetrieveErrorResult(new List<AutoClutch.Core.Objects.Error> { new AutoClutch.Core.Objects.Error { Description = "There are more than 1 " + contractNumber + " contracts in the database." } });
            }
            else if(!result.Any())
            {
                return Ok();
            }

            return Ok(result.FirstOrDefault().contractId);
        }

        [Route("getInitialContract")]
        [HttpGet]
        public IHttpActionResult GetInitialContract(string sectionName)
        {
            var result = _contractService.GetInitialContract(sectionName);

            return Ok(result);
        }

        [Route("getInitialRenewalContract")]
        [HttpGet]
        public IHttpActionResult GetInitialRenewalContract(string originalContractNumber)
        {
            var result = _contractService.GetInitialRenewalContract(originalContractNumber);

            return Ok(result);
        }

        [Route("addRenewalContract")]
        [HttpPost]
        public IHttpActionResult AddRenewalContract(contract contract)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _contractService.AddRenewalContract(contract, loggedInUserName: User.Identity.Name.Split("\\".ToCharArray()).Last(), lazyLoadingEnabled: false, proxyCreationEnabled: false);

                // Get any errors.
                var errors = _contractService.Errors;

                if (errors.Any())
                {
                    return RetrieveErrorResult(errors);
                }

                if (_logService != null)
                {
                    _logService.Info(contract, contract.contractId, EventType.Added, entityName: contract.contractNumber, loggedInUserName: User.Identity.Name.Split("\\".ToCharArray()).Last());
                }

                // Null is passed because the entity coming back from the service layer is 
                // filled with proxies and stuff that causes havok for the JSON deserializer.
                // Until this is fixed the return value should remain something that does 
                // not cause the deserializer to take a significantly long time.
                return Ok(new { contract.contractId, contract.contractNumber });
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);

                return InternalServerError(ex);
            }
        }

    }
}
