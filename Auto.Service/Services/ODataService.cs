using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Objects;
using AutoClutch.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoClutch.Core
{
    public class ODataService<TEntity> : IODataService<TEntity> where TEntity : class, new()
    {
        public IEnumerable<Error> Errors { get; set; }

        private IODataRepository<TEntity> _repository;

        public ODataService(IODataRepository<TEntity> repository)
        {
            Errors = new List<Error>();

            this._repository = repository;
        }

        public virtual IQueryable<TEntity> Queryable()
        {
            var result = _repository.Queryable();

            return result;
        }

        public async Task<TEntity> UpdateAsync(string id, TEntity entity, string loggedInUserName)
        {
            var result = await _repository.UpdateAsync(id, entity, loggedInUserName);

            ((List<Error>)Errors).AddRange(_repository.Errors);

            return result;
        }

        public async Task<TEntity> AddAsync(TEntity entity, string loggedInUserName)
        {
            var result = await _repository.AddAsync(entity, loggedInUserName);

            ((List<Error>)Errors).AddRange(_repository.Errors);

            return result;
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // The below line has been commented out because we are going to let our dependency 
                    // injector handle the lifecycle of our repository objects.

                    // Dispose managed state (managed objects).
                    //_repository.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override a finalizer below.
                // Set large fields to null.

                disposedValue = true;
            }
        }

        // Dispose(bool disposing) above has code to free unmanaged resources.
        ~ODataService()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        #endregion

        public TEntity Add(TEntity entity, string loggedInUserName = null)
        {
            entity = AddAsync(entity, loggedInUserName).Result;

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loggedInUserName"></param>
        /// <param name="softDelete">Softdelete is ignored in this context.</param>
        /// <returns></returns>
        public TEntity Delete(int id, string loggedInUserName = null, bool softDelete = false)
        {
            var entity = Find(id, softDelete);

            var result = _repository.DeleteAsync(id.ToString(), loggedInUserName).Result;

            return entity;
        }

        public async Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool softDelete = false)
        {
            var entity = Find(id, softDelete);

            var result = await _repository.DeleteAsync(id.ToString(), loggedInUserName);

            return entity;
        }

        /// <summary>
        /// Returns an IQueryable of this set.
        /// </summary>
        /// <param name="includeSoftDeleted">IncludeSoftDeleted is ignored in this context.</param>
        /// <returns></returns>
        public IQueryable<TEntity> Queryable(bool? includeSoftDeleted = null)
        {
            var result = _repository.Queryable();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="includeSoftDeleted">IncludeSoftDeleted is ignored in this context.</param>
        /// <returns></returns>
        public TEntity Find(object entityId, bool? includeSoftDeleted = null)
        {
            var result = _repository.Find(entityId.ToString());

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="includeSoftDeleted">IncludeSoftDeleted is ignored in this context.</param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync(object entityId, bool? includeSoftDeleted = null)
        {
            var result = await _repository.FindAsync(entityId.ToString());

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="includeSoftDeleted">IncludeSoftDeleted is ignored in this context.</param>
        /// <returns></returns>
        public object GetEntityIdObject(TEntity entity, bool? includeSoftDeleted = null)
        {
            Errors.Concat(new List<Error> { new Error { Description = "Unable to retrieve the entityId object in the OData context.", Property = "entity" } });

            return null;
        }

        public TEntity Update(object entityId, TEntity entity, string loggedInUserName = null)
        {
            entity = _repository.Update(entityId.ToString(), entity, loggedInUserName);

            return entity;
        }

        public async Task<TEntity> UpdateAsync(object entityId, TEntity entity, string loggedInUserName = null)
        {
            entity = await UpdateAsync(entityId.ToString(), entity, loggedInUserName);

            return entity;
        }
    }
}