using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;

namespace AutoClutch.Controller
{
    public class ODataApiController<TEntity> : ODataController
        where TEntity : class
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
        public virtual IQueryable<TEntity> Get()
        {
            var queryable = _service.Queryable();

            return queryable;
        }

        //[EnableQuery]
        //public SingleResult<TEntity> Get([FromODataUri] int key)
        //{
        //    IQueryable<TEntity> result = db.Products.Where(p => p.Id == key);
        //    return SingleResult.Create(result);
        //}
        [HttpGet]
        [EnableQuery]
        public virtual TEntity Get([FromODataUri] int key)
        {
            var result = _service.Find(key);

            return result;
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

                var result = await _service.AddAsync(entity, User.Identity.Name?.Split("\\".ToCharArray()).FirstOrDefault());

                if (_service.Errors.Any())
                {
                    return RetrieveErrorResult(_service.Errors);
                }

                // If a logging service has been injected then use it.
                if (_logService != null)
                {
                    await _logService.InfoAsync(entity, (int)_service.GetEntityIdObject(entity), EventType.Added, loggedInUserName: User?.Identity?.Name, useToString: true);
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
                    await _service.UpdateAsync(entityFromDatabase, User.Identity.Name?.Split("\\".ToCharArray()).FirstOrDefault());

                    if (_service.Errors.Any())
                    {
                        return RetrieveErrorResult(_service.Errors);
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
                    update = await _service.UpdateAsync(update, User.Identity.Name?.Split("\\".ToCharArray()).FirstOrDefault());

                    if (_service.Errors.Any())
                    {
                        return RetrieveErrorResult(_service.Errors);
                    }

                    // If a logging service has been injected then use it.
                    if (_logService != null)
                    {
                        await _logService.InfoAsync(update, (int)_service.GetEntityIdObject(update), EventType.Modified, loggedInUserName: User?.Identity?.Name, useToString: true);
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

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                var entity = await _service.FindAsync(key);

                if (entity == null)
                {
                    return NotFound();
                }

                await _service.DeleteAsync(key, User.Identity.Name?.Split("\\".ToCharArray()).FirstOrDefault());

                if (_service.Errors.Any())
                {
                    return RetrieveErrorResult(_service.Errors);
                }

                // If a logging service has been injected then use it.
                if (_logService != null)
                {
                    await _logService.InfoAsync(entity, (int)_service.GetEntityIdObject(entity), EventType.Deleted, loggedInUserName: User?.Identity?.Name, useToString: true);
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
            var result = _service.Exists(key);

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
