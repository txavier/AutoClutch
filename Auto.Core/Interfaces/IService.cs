using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutoClutch.Core.Interfaces
{
    public interface IService<TEntity> : IDisposable
     where TEntity : class
    {
        IEnumerable<Error> Errors { get; set; }

        TEntity Add(TEntity entity, string loggedInUserName = null);
        Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null);
        TEntity Delete(int id, string loggedInUserName = null, bool softDelete = false);
        Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool softDelete = false);
        void Dispose();
        IQueryable<TEntity> Queryable(bool? includeSoftDeleted = default(bool?));
        TEntity Update(TEntity entity, string loggedInUserName = null);
        Task<TEntity> UpdateAsync(TEntity entity, string loggedInUserName = null);
        TEntity Find(object entityId, bool? includeSoftDeleted = default(bool?));
        Task<TEntity> FindAsync(object entityId, bool? includeSoftDeleted = default(bool?));
        object GetEntityIdObject(TEntity entity, bool? includeSoftDeleted = default(bool?));
    }
}