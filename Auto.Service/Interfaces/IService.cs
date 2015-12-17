using AutoClutch.Auto.Repo.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

[assembly: CLSCompliant(true)]

namespace AutoClutch.Auto.Service.Interfaces
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
        TEntity Delete(TEntity entity, string loggedInUserName = null, bool dontSave = false);
        TEntity Delete(int id, string loggedInUserName = null, bool dontSave = false);
        Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool dontSave = false);
        void Dispose();
        bool Exists(object entityIdObject);
        TEntity Find(object entityId, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true);
        Task<TEntity> FindAsync(object entityId, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string filterString = null, Func<IQueryable<TEntity>, IEnumerable<TEntity>> distinctBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string orderByString = null, Func<IEnumerable<TEntity>, IEnumerable<TEntity>> maxBy = null, int? skip = default(int?), int? take = default(int?), string includeProperties = "", bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true);
        IEnumerable<TEntity> GetAll(bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true);
        IEnumerable<Error> GetAnyAvailableValidationErrors();
        int GetCount(Expression<Func<TEntity, bool>> filter = null, string filterString = null, Func<IQueryable<TEntity>, IEnumerable<TEntity>> distinctBy = null, Func<IEnumerable<TEntity>, IEnumerable<TEntity>> maxBy = null);
        object GetEntityIdObject(TEntity entity);
        IEnumerable<string> GetEntityPropertyNames(TEntity entity);
        int SaveChanges(string loggedInUserName = null);
        Task<int> SaveChangesAsync(string loggedInUserName = null);
        TEntity Update(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        Task<TEntity> UpdateAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        IQueryable<TEntity> Queryable();
    }
}
