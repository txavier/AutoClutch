using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Data.Entity;
using System.Web.Http.Controllers;
using System.Net.Http;

namespace AutoClutch.Controller
{
    public class ODataApiController<TEntity> : ODataController
        where TEntity : class, new()
    {
        private IService<TEntity> _service;

        private ILogService<TEntity> _logService;

        public ODataApiController(IService<TEntity> service, ILogService<TEntity> logService)
        {
            _service = service;

            _logService = logService;
        }

        public ODataApiController(IService<TEntity> service)
        {
            _service = service;
        }

        [HttpGet]
        [EnableQuery]
        public virtual IHttpActionResult Get()
        {
            // If the user included a calculated value in his $filter or $select
            // query options then the entire table has to be brought into memory
            // to be calculated properly using ToList() before being returned to 
            // the user.
            if (NotMappedPropertiesInQueryOptions(Request).Any())
            {
                // This is very slow.
                var result = _service.Queryable().ToList();

                return Ok(result);
            }

            var queryable = _service.Queryable();

            return Ok(queryable);
        }

        private static IEnumerable<KeyValuePair<string, string>> NotMappedPropertiesInQueryOptions(HttpRequestMessage request)
        {
            // As of 5/17/2017 unable to use Delegate Decompiler because it fails 
            // when a computed property uses recursion.
            // Use DelegateDecompiler to be able to query on computed properties.
            //var queryable = _service.Queryable().Decompile();

            var notMappedProperties = (new TEntity()).GetType().GetProperties()
                .Where(i => i.CustomAttributes.Any(j => j.AttributeType.Name.Equals("NotMappedAttribute")))
                .Select(j => j.Name);

            // If there aren't any mapped properties then we can stop here.
            if (!notMappedProperties.Any())
            {
                return new List<KeyValuePair<string, string>>();
            }

            // If $select has a not mapped attribute then to .toList().
            var queryNameValuePairs = request.GetQueryNameValuePairs();

            var concernedQuerys = queryNameValuePairs.Where(i => i.Key.Equals("$select") || i.Key.Equals("$filter") || i.Key.Equals("$orderby"));
            // If there is no select, orderby or filter query options then we can stop here.
            if (!concernedQuerys.Any())
            {
                return new List<KeyValuePair<string, string>>();
            }

            var resultSet = concernedQuerys.Where(i => notMappedProperties.Intersect(i.Value.Split(", ".ToCharArray())).Any());

            return resultSet;
        }
        //[EnableQuery]
        //public SingleResult<TEntity> Get([FromODataUri] int key)
        //{
        //    IQueryable<TEntity> result = db.Products.Where(p => p.Id == key);
        //    return SingleResult.Create(result);
        //}
        [HttpGet]
        [EnableQuery]
        public virtual async Task<IHttpActionResult> Get([FromODataUri] int key)
        {
            var result = await _service.FindAsync(key);

            return Ok(result);
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Post(TEntity entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _service.AddAsync(entity, User.Identity.Name?.Split("\\".ToCharArray()).LastOrDefault());

                if (_service.Errors.Any())
                {
                    return RetrieveErrorResult(_service.Errors);
                }

                // If a logging service has been injected then use it.
                if (_logService != null)
                {
                    await _logService.InfoAsync(entity, _service.GetEntityIdObject(entity).ToString(), EventType.Added, loggedInUserName: User?.Identity?.Name, useToString: true);
                }

                return Created(result);
            }
            catch (Exception ex)
            {
                _logService?.Info(ex);

                return InternalServerError(ex);
            }
        }

        [HttpPatch]
        public virtual async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<TEntity> entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var entityFromDatabase = await _service.FindAsync(key);

                if (entityFromDatabase == null)
                {
                    return NotFound();
                }

                entity.Patch(entityFromDatabase);

                try
                {
                    var update = await _service.UpdateAsync(entityFromDatabase, User.Identity.Name?.Split("\\".ToCharArray()).LastOrDefault());

                    if (_service.Errors.Any())
                    {
                        return RetrieveErrorResult(_service.Errors);
                    }

                    // If a logging service has been injected then use it.
                    if (_logService != null)
                    {
                        await _logService.InfoAsync(update, key.ToString(), EventType.Modified, loggedInUserName: User?.Identity?.Name, useToString: true);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntityExists(key))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Updated(entityFromDatabase);
            }
            catch (Exception ex)
            {
                _logService?.Info(ex);

                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public virtual async Task<IHttpActionResult> Put([FromODataUri] int key, TEntity update)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (key != (int)_service.GetEntityIdObject(update))
                {
                    return BadRequest();
                }

                try
                {
                    update = await _service.UpdateAsync(update, User.Identity.Name?.Split("\\".ToCharArray()).LastOrDefault());

                    if (_service.Errors.Any())
                    {
                        return RetrieveErrorResult(_service.Errors);
                    }

                    // If a logging service has been injected then use it.
                    if (_logService != null)
                    {
                        await _logService.InfoAsync(update, key.ToString(), EventType.Modified, loggedInUserName: User?.Identity?.Name, useToString: true);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntityExists(key))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Updated(update);
            }
            catch (Exception ex)
            {
                _logService?.Info(ex);

                return InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key, bool? softDelete = null)
        {
            try
            {
                var entity = await _service.FindAsync(key);

                if (entity == null)
                {
                    return NotFound();
                }

                await _service.DeleteAsync(key, User.Identity.Name?.Split("\\".ToCharArray()).LastOrDefault(), softDelete ?? false);

                if (_service.Errors.Any())
                {
                    return RetrieveErrorResult(_service.Errors);
                }

                // If a logging service has been injected then use it.
                if (_logService != null)
                {
                    await _logService.InfoAsync(entity, key.ToString(), (softDelete ?? false) ? EventType.SoftDeleted : EventType.Deleted, loggedInUserName: User?.Identity?.Name?.Split("\\".ToCharArray()).LastOrDefault(), useToString: true);
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logService?.Info(ex);

                return InternalServerError(ex);
            }
        }

        private bool EntityExists(int key)
        {
            var result = _service.Find(key) != null;

            return result;
        }

        [Route("RetrieveErrorResult")]
        /// <summary>
        /// http://www.codeproject.com/Articles/825274/ASP-NET-Web-Api-Unwrapping-HTTP-Error-Results-and
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        protected IHttpActionResult RetrieveErrorResult(IEnumerable<Error> errors)
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

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();

            base.Dispose(disposing);
        }
    }
}