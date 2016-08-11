using AutoClutch.Auto.Repo.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

[assembly: CLSCompliant(true)]

namespace AutoClutch.Auto.Core.Interfaces
{
    public interface IService<TEntity> : IDisposable
     where TEntity : class
    {
        bool LazyLoadingEnabled { get; set; }
        bool ProxyCreationEnabled { get; set; }
        IEnumerable<Error> Errors { get; set; }
        TEntity Add(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        TEntity AddOrUpdate(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        TEntity Delete(int id, string loggedInUserName = null, bool dontSave = false, bool softDelete = false);
        Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool dontSave = false, bool softDelete = false);
        void Dispose();
        bool Exists(object entityIdObject, bool? includeSoftDeleted = null);
        Task<bool> ExistsAsync(object entityIdObject, bool? includeSoftDeleted = null);
        TEntity Find(object entityId, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = null);
        Task<TEntity> FindAsync(object entityId, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = null);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string filterString = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> distinctBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string orderByString = null, Func<IEnumerable<TEntity>, IQueryable<TEntity>> maxBy = null, int? skip = default(int?), int? take = default(int?), string includeProperties = "", bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = null);
        IEnumerable<TEntity> GetAll(bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = null);
        IEnumerable<Error> GetAnyAvailableValidationErrors();
        int GetCount(Expression<Func<TEntity, bool>> filter = null, string filterString = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> distinctBy = null, Func<IEnumerable<TEntity>, IQueryable<TEntity>> maxBy = null, bool? includeSoftDeleted = null);
        object GetEntityIdObject(TEntity entity, bool? includeSoftDeleted = null);
        IEnumerable<string> GetEntityPropertyNames(TEntity entity, bool? includeSoftDeleted = null);
        int SaveChanges(string loggedInUserName = null);
        Task<int> SaveChangesAsync(string loggedInUserName = null);
        TEntity Update(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        Task<TEntity> UpdateAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        IQueryable<TEntity> Queryable(bool? includeSoftDeleted = null);
        string GetEntityKeyName(TEntity entity);
    }
}
