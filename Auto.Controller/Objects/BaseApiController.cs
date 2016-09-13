using AutoClutch.Auto.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.ComponentModel;
using AutoClutch.Auto.Core.Objects;
using Auto.Controller.Interfaces;
using AutoClutch.Auto.Log.Interfaces;

namespace Auto.Controller.Objects
{
    public class BaseApiController<TEntity> : ApiController
        where TEntity : class
    {
        private IService<TEntity> _service;

        private ILogService<TEntity> _logService;

        private IAutoClutchAuthorizationService _autoClutchAuthorizationService;

        public BaseApiController(IService<TEntity> service,
            ILogService<TEntity> logService = null,
            IAutoClutchAuthorizationService autoClutchAuthorizationService = null)
        {
            _service = service;

            _logService = logService;

            _autoClutchAuthorizationService = autoClutchAuthorizationService;
        }

        public BaseApiController(IService<TEntity> service)
        {
            _service = service;
        }

        [Route("RetrieveErrorResult")]
        /// <summary>
        /// http://www.codeproject.com/Articles/825274/ASP-NET-Web-Api-Unwrapping-HTTP-Error-Results-and
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        protected IHttpActionResult RetrieveErrorResult(IEnumerable<AutoClutch.Auto.Repo.Objects.Error> errors)
        {
            if (errors != null && errors.Any())
            {
                foreach (var error in errors.Select(i => i.Description).Distinct())
                {
                    ModelState.AddModelError("", error);
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, 
                    // so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }
            return null;
        }

        // http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api#requirements
        // For sorting http://jasonwatmore.com/post/2014/07/16/Dynamic-LINQ-Using-strings-to-sort-by-properties-and-child-object-properties.aspx
        // For sorting by child collections http://stackoverflow.com/questions/35496303/sorting-dynamic-linq-by-collection-property
        [HttpGet]
        public virtual IHttpActionResult Get(
            string sort = null,
            string expand = null,
            string fields = null,
            string q = null,
            int page = 1,
            int perPage = Int32.MaxValue,
            string search = null,
            string searchFields = null,
            bool count = false)
        {
            try
            {
                if (_autoClutchAuthorizationService != null &&
                    !_autoClutchAuthorizationService.IsAuthorized(User, ControllerContext.Request.Method.Method, ControllerContext.Request.RequestUri.AbsoluteUri))
                {
                    return Unauthorized();
                }

                var skip = (page - 1) * perPage;

                // If sort is descending then change the '-' sign which represents the descending order
                // to the word 'descending' which is used by AutoService.
                sort = (!string.IsNullOrWhiteSpace(sort) && sort.Contains('-')) ? sort.TrimStart("-".ToCharArray()) + " descending" : sort;

                if (search != null)
                {
                    // contractNumber.Contains("20151427556") and section.sectionId=1
                    q = GetDynamicQuery(search, searchFields, q, typeof(TEntity));
                }

                if (count)
                {
                    var countResult = _service.Queryable().Where(q ?? "1 = 1").Count();

                    return Ok(countResult);
                }

                _service.LazyLoadingEnabled = false;

                _service.ProxyCreationEnabled = false;

                var result = _service.Get(
                    skip: skip,
                    take: perPage,
                    includeProperties: expand,
                    filterString: q,
                    orderByString: sort);

                //var result = fields == null ? _service.Queryable().Where(q ?? "1 = 1").OrderBy(sort).Skip(skip).Take(perPage)
                //    : _service.Queryable().Where(q ?? "1 = 1").OrderBy(sort).Include("engineerContracts.engineer").Select("new(" + fields + ")").Skip(skip).Take(perPage);

                // Get any errors.
                var errors = _service.Errors;

                if (errors.Any())
                {
                    return RetrieveErrorResult(errors);
                }

                if (fields != null)
                {
                    if (expand != null)
                    {
                        var expandedObjects = expand.Split(new char[] { ',' });

                        List<string> resultingObjects = new List<string>();

                        // Filter out all of the . object navigational properties.
                        // We cannot select on these but this is ok since they are
                        // already included from the previous result set.
                        foreach (var expandedObject in expandedObjects)
                        {
                            if (expandedObject.Split(new char[] { '.' }).Count() > 1)
                            {
                                resultingObjects.Add(expandedObject.Split(new char[] { '.' }).First());
                            }
                            else
                            {
                                resultingObjects.Add(expandedObject);
                            }
                        }

                        // The injected authorization service may filter the requested fields here.
                        if (_autoClutchAuthorizationService != null)
                        {
                            fields = _autoClutchAuthorizationService.GetFilteredFields(User, ControllerContext.Request.Method.Method, ControllerContext.Request.RequestUri.AbsoluteUri, fields.Split(",".ToCharArray()))
                                .Aggregate((current, next) => current + "," + next);
                        }

                        var selectedResult1 = result.Select("new(" + fields + ")", expandedObjects);

                        return Ok(selectedResult1);
                    }
                    else
                    {
                        // The injected authorization service may filter the requested fields here.
                        if (_autoClutchAuthorizationService != null)
                        {
                            fields = _autoClutchAuthorizationService.GetFilteredFields(User, ControllerContext.Request.Method.Method, ControllerContext.Request.RequestUri.AbsoluteUri, fields.Split(",".ToCharArray()))
                                .Aggregate((current, next) => current + "," + next);
                        }

                        var selectedResult2 = result.Select("new(" + fields + ")");

                        return Ok(selectedResult2);
                    }

                }
                else
                {
                    List<string> resultingObjects = new List<string>();

                    if (expand != null)
                    {
                        var expandedObjects = expand.Split(new char[] { ',' });

                        // Filter out all of the . object navigational properties.
                        // We cannot select on these but this is ok since they are
                        // already included from the previous result set.
                        foreach (var expandedObject in expandedObjects)
                        {
                            if (expandedObject.Split(new char[] { '.' }).Count() > 1)
                            {
                                resultingObjects.Add(expandedObject.Split(new char[] { '.' }).First());
                            }
                            else
                            {
                                resultingObjects.Add(expandedObject);
                            }
                        }
                    }

                    var expandedObjectsString = expand == null ? string.Empty : "," + resultingObjects.Aggregate((current, next) => current + "," + next);

                    // If there arent any items then return the empty set.
                    if (!result.Any())
                    {
                        return Ok(result);
                    }

                    // Add all of this objects base properties if there were no fields specified.
                    fields = result.First().GetType().GetProperties().Select(i => i.Name).Aggregate((current, next) => current + ", " + next) + expandedObjectsString;

                    // Get distinct fields so that there is no overlap.
                    fields = fields.Split(",".ToCharArray()).Distinct().Aggregate((current, next) => current + "," + next);

                    try
                    {
                        // The injected authorization service may filter the requested fields here.
                        if (_autoClutchAuthorizationService != null)
                        {
                            fields = _autoClutchAuthorizationService.GetFilteredFields(User, ControllerContext.Request.Method.Method, ControllerContext.Request.RequestUri.AbsoluteUri, fields.Split(",".ToCharArray()))
                                .Aggregate((current, next) => current + "," + next);
                        }

                        var selectedResult3 = result.Select("new(" + fields + ")");

                        return Ok(selectedResult3);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new ArgumentException("Argument exception caught.  The fileds value is fields: " + fields, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.Info(ex);

                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// contractNumber.Contains("20151427556") and section.sectionId=1
        /// "ContractNumber=\"Test-Contract2\" AND ((contractNumber.Contains(\"1236-BIO\") OR contractDescription.Contains(\"1236-BIO\") OR (contractNumber.Contains(\"201-hey\") OR contractDescription.Contains(\"201-hey\"))", result);
        /// 
        /// </summary>
        public static string GetDynamicQuery(string fullTextSearchParameters, string fullTextSearchFields = null, string currentQuery = null, Type type = null)
        {
            //Contract.Requires<ArgumentNullException>(fullTextSearchParameters != null, "The text search parameters are needed in order to get all of this objects properties.");

            if (fullTextSearchFields == null && type != null)
            {
                fullTextSearchFields = GetEntityStringProperties(type).Aggregate((current, next) => current + "," + next);
            }

            bool appendToPreviousQuery = false;

            if (currentQuery == null)
            {
                appendToPreviousQuery = false;

                currentQuery = "";
            }
            else
            {
                appendToPreviousQuery = true;

                currentQuery = currentQuery + " AND (";
            }

            int fieldCounter = 0;

            foreach (var parameter in fullTextSearchParameters.Split(",".ToCharArray()))
            {
                if (fieldCounter != 0)
                {
                    currentQuery += " OR ";
                }

                int parameterCounter = 0;

                foreach (var field in fullTextSearchFields.Split(",".ToCharArray()))
                {
                    if (parameterCounter != 0)
                    {
                        currentQuery += " OR ";
                    }
                    else
                    {
                        currentQuery += "(";
                    }

                    currentQuery += field + ".Contains(\"" + parameter + "\")";

                    parameterCounter++;
                }

                if (fullTextSearchFields.Any())
                {
                    currentQuery += ")";
                }

                fieldCounter++;
            }

            if (appendToPreviousQuery)
            {
                currentQuery += ")";
            }

            return currentQuery;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/1447308/enumerating-through-an-objects-properties-string-in-c-sharp
        /// http://stackoverflow.com/questions/2281972/how-to-get-a-list-of-properties-with-a-given-attribute
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetEntityStringProperties(Type type)
        {
            var result = type
                .GetProperties()
                .Where(pi => pi.PropertyType == typeof(string) && pi.GetGetMethod() != null && !Attribute.IsDefined(pi, typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute)))
                .Select(pi => pi.Name);

            return result;
        }

        [Route("BaseCount")]
        protected int BaseCount(string q = null, string search = null, string searchFields = null)
        {
            try
            {
                if (_autoClutchAuthorizationService != null &&
                    !_autoClutchAuthorizationService.IsAuthorized(User, ControllerContext.Request.Method.Method, ControllerContext.Request.RequestUri.AbsoluteUri))
                {
                    return -1;
                }

                if (search != null)
                {
                    q = GetDynamicQuery(search, searchFields, q, typeof(TEntity));
                }

                //var result = _service.GetCount(filterString: q);

                var result = _service.Queryable().Where(q ?? "1 = 1").Count();

                return result;
            }
            catch (Exception ex)
            {
                _logService.Info(ex);
            }

            return -1;
        }

        [HttpGet]
        public virtual async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                _service.LazyLoadingEnabled = false;

                _service.ProxyCreationEnabled = false;

                var result = await _service.FindAsync(id);

                if (result == null)
                {
                    return NotFound();
                }

                // Get any errors.
                var errors = _service.Errors;

                if (errors.Any())
                {
                    return RetrieveErrorResult(errors);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                if (_logService != null)
                {
                    _logService.Info(ex);
                }

                return InternalServerError(ex);
            }
        }

        // PUT: api/plantUsers/5
        [HttpPut]
        public virtual async Task<IHttpActionResult> Put(int id, TEntity entity)
        {
            try
            {
                if (_autoClutchAuthorizationService != null &&
                    !_autoClutchAuthorizationService.IsAuthorized(User, ControllerContext.Request.Method.Method, ControllerContext.Request.RequestUri.AbsoluteUri))
                {
                    return Unauthorized();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != (int)_service.GetEntityIdObject(entity))
                {
                    return BadRequest();
                }

                var errors1 = _service.GetAnyAvailableValidationErrors();

                var loggedInUserName = _autoClutchAuthorizationService?.GetLoggedInUserName(User);

                _service.LazyLoadingEnabled = false;

                _service.ProxyCreationEnabled = false;

                await _service.UpdateAsync(entity, loggedInUserName);

                // Get any errors.
                var errors = _service.Errors;

                if (errors.Any())
                {
                    return RetrieveErrorResult(errors);
                }

                // If a logging service has been injected then use it.
                if (_logService != null)
                {
                    await _logService.InfoAsync(entity, (int)_service.GetEntityIdObject(entity), EventType.Modified, loggedInUserName: loggedInUserName, useToString: true);
                }

                //return StatusCode(HttpStatusCode.NoContent);
                return Ok(entity);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
            {
                if (!_service.Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                if (_logService != null)
                {
                    _logService.Info(ex);
                }

                return InternalServerError(ex);
            }
        }

        // POST: api/plantUsers
        [HttpPost]
        public virtual async Task<IHttpActionResult> Post(TEntity entity)
        {
            try
            {
                if (_autoClutchAuthorizationService != null &&
                    !_autoClutchAuthorizationService.IsAuthorized(User, ControllerContext.Request.Method.Method, ControllerContext.Request.RequestUri.AbsoluteUri))
                {
                    return Unauthorized();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var loggedInUserName = _autoClutchAuthorizationService?.GetLoggedInUserName(User);

                _service.LazyLoadingEnabled = false;

                _service.ProxyCreationEnabled = false;

                await _service.AddAsync(entity, loggedInUserName: loggedInUserName);

                // Get any errors.
                var errors = _service.Errors;

                if (errors.Any())
                {
                    return RetrieveErrorResult(errors);
                }

                // If a logging service has been injected then use it.
                if (_logService != null)
                {
                    await _logService.InfoAsync(entity, (int)_service.GetEntityIdObject(entity), EventType.Added, loggedInUserName: loggedInUserName, useToString: true);
                }

                // Null is passed because the entity coming back from the service layer is 
                // filled with proxies and stuff that causes havok for the JSON deserializer.
                // Until this is fixed the return value should remain something that does 
                // not cause the deserializer to take a significantly long time.
                return CreatedAtRoute("DefaultApi", new { id = (int)_service.GetEntityIdObject(entity) }, _service.GetEntityIdObject(entity));
            }
            catch (Exception ex)
            {
                if (_logService != null)
                {
                    _logService.Info(ex);
                }

                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>In order to fix help page generation try the following links.
        /// http://stackoverflow.com/questions/17163213/asp-net-web-api-help-page-how-to-show-generic-return-type-in-documentation
        /// http://aspnetwebstack.codeplex.com/workitem/1480
        /// </remarks>
        [HttpDelete]
        public virtual async Task<IHttpActionResult> Delete(int id, bool? softDelete = null)
        {
            try
            {
                if (_autoClutchAuthorizationService != null &&
                    !_autoClutchAuthorizationService.IsAuthorized(User, ControllerContext.Request.Method.Method, ControllerContext.Request.RequestUri.AbsoluteUri))
                {
                    return Unauthorized();
                }

                var entity = await _service.FindAsync(id);

                if (entity == null)
                {
                    return NotFound();
                }

                var loggedInUserName = _autoClutchAuthorizationService?.GetLoggedInUserName(User);

                // The below line has been commented out because the softdelete is not working.
                //var result = await _service.DeleteAsync(id, loggedInUserName: loggedInUserName, softDelete: (softDelete ?? false));

                var result = await _service.DeleteAsync(id, loggedInUserName: loggedInUserName);

                // If a logging service has been injected then use it.
                if (_logService != null)
                {
                    await _logService.InfoAsync(entity, (int)_service.GetEntityIdObject(entity), (softDelete ?? false) ? EventType.SoftDeleted : EventType.Deleted, loggedInUserName: loggedInUserName, useToString: true);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                if (_logService != null)
                {
                    _logService.Info(ex);
                }

                return InternalServerError(ex);
            }
        }

        [Route("Dispose")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
