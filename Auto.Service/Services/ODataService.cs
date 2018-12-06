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
    }
}