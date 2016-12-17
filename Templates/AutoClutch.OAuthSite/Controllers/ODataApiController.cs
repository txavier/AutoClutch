using AutoClutch.Auto.Log.Interfaces;
using AutoClutch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;

namespace $safeprojectname$.Controllers
{
    public class ODataApiController<TEntity> : ODataController
        where TEntity : class
    {
        private IService<TEntity> _service;

        private ILogService<TEntity> _logService;

        public ODataApiController(IService<TEntity> service)
        {
            _service = service;
        }

        public ODataApiController(IService<TEntity> service, ApplicationUserManager userManager)
        {
            _service = service;
        }

        public ODataApiController(IService<TEntity> service, ApplicationUserManager userManager, ILogService<TEntity> logService = null)
        {
            _service = service;

            _logService = logService;
        }

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

        [EnableQuery]
        public virtual TEntity Get([FromODataUri] int key)
        {
            var result = _service.Find(key);

            return result;
        }

        [System.Web.Http.HttpPost]
        public virtual async Task<IHttpActionResult> Post(TEntity entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.AddAsync(entity, User.Identity.Name.Split("\\".ToCharArray()).FirstOrDefault());

            return Created(result);
        }

        [HttpPatch]
        public virtual async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<TEntity> entity)
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
                await _service.UpdateAsync(entityFromDatabase, User.Identity.Name.Split("\\".ToCharArray()).FirstOrDefault());
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

        [HttpPut]
        public virtual async Task<IHttpActionResult> Put([FromODataUri] int key, TEntity update)
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
                await _service.UpdateAsync(update, User.Identity.Name.Split("\\".ToCharArray()).FirstOrDefault());
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

        private bool EntityExists(int key)
        {
            var result = _service.Exists(key);

            return result;
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var entity = await _service.FindAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(key, User.Identity.Name.Split("\\".ToCharArray()).FirstOrDefault());

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();

            base.Dispose(disposing);
        }
    }
}