using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

[assembly: CLSCompliant(true)]

namespace AutoClutch.Core.Interfaces
{
    public interface IEFService<TEntity> : IService<TEntity>, IDisposable
     where TEntity : class
    {
        bool AutoDetectChangesEnabled { get; set; }
        IEnumerable<Error> Errors { get; set; }
        bool LazyLoadingEnabled { get; set; }
        bool ProxyCreationEnabled { get; set; }
        bool EnsureTransactionsForFunctionsAndCommands { get; set; }
        bool ValidateOnSaveEnabled { get; set; }

        TEntity Add(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        TEntity AddOrUpdate(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        TEntity Delete(int id, string loggedInUserName = null, bool softDelete = false, bool dontSave = false);
        Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool softDelete = false, bool dontSave = false);
        void Dispose();
        bool Exists(object entityIdObject, bool? includeSoftDeleted = default(bool?));
        Task<bool> ExistsAsync(object entityIdObject, bool? includeSoftDeleted = default(bool?));
        TEntity Find(object entityId, bool? lazyLoadingEnabled, bool? proxyCreationEnabled, bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = default(bool?));
        Task<TEntity> FindAsync(object entityId, bool? lazyLoadingEnabled, bool? proxyCreationEnabled, bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = default(bool?));
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string filterString = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> distinctBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string orderByString = null, Func<IEnumerable<TEntity>, IQueryable<TEntity>> maxBy = null, int? skip = default(int?), int? take = default(int?), string includeProperties = "", bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = default(bool?));
        IEnumerable<TEntity> GetAll(bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = default(bool?));
        IEnumerable<Error> GetAnyAvailableValidationErrors();
        bool IsValid(TEntity entity, string loggedInUserName = null);
        int GetCount(Expression<Func<TEntity, bool>> filter = null, string filterString = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> distinctBy = null, Func<IEnumerable<TEntity>, IQueryable<TEntity>> maxBy = null, bool? includeSoftDeleted = default(bool?));
        object GetEntityIdObject(TEntity entity, bool? includeSoftDeleted = default(bool?));
        string GetEntityKeyName(TEntity entity);
        IEnumerable<string> GetEntityPropertyNames(TEntity entity, bool? includeSoftDeleted = default(bool?));
        IQueryable<TEntity> Queryable(bool? includeSoftDeleted = default(bool?));
        int SaveChanges(string loggedInUserName = null);
        Task<int> SaveChangesAsync(string loggedInUserName = null);
        TEntity Update(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        Task<TEntity> UpdateAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
    }
}