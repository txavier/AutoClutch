using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrackerEnabledDbContext.Common.Models;

[assembly: CLSCompliant(true)]

namespace AutoClutch.Repo.Interfaces
{
    public interface IRepository<TEntity> : IDisposable
     where TEntity : class
    {
        bool AutoDetectChangesEnabled { get; set; }
        IEnumerable<Error> Errors { get; set; }
        bool LazyLoadingEnabled { get; set; }
        bool ProxyCreationEnabled { get; set; }
        bool EnsureTransactionsForFunctionsAndCommands { get; set; }
        bool ValidateOnSaveEnabled { get; set; }
        string RegexMatchPrimaryKeyIdPattern { get; set; }

        TEntity Add(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false);
        TEntity Delete(TEntity entity, string loggedInUserName = null, bool dontSave = false);
        TEntity Delete(int id, string loggedInUserName = null, bool dontSave = false);
        Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool dontSave = false);
        void Dispose();
        IEnumerable<AuditLog> EntityAuditLog(object entityId);
        bool Exists(TEntity entity);
        bool Exists(object entityIdObject);
        TEntity Find(object entityId, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true);
        Task<TEntity> FindAsync(object entityId, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string filterString = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> distinctBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string orderByString = null, Func<IEnumerable<TEntity>, IQueryable<TEntity>> maxBy = null, int? skip = default(int?), int? take = default(int?), string includeProperties = "", bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChanges = true);
        IEnumerable<TEntity> GetAll(bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true);
        IEnumerable<Error> GetAnyAvailableValidationErrors();
        int GetCount(Expression<Func<TEntity, bool>> filter = null, string filterString = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> distinctBy = null, Func<IEnumerable<TEntity>, IQueryable<TEntity>> maxBy = null);
        object GetEntityIdObject(TEntity entity);
        string GetEntityKeyName(TEntity entity);
        IEnumerable<string> GetEntityPropertyNames(TEntity entity);
        IQueryable<TEntity> Queryable();
        int SaveChanges(string loggedInUserName = null);
        Task<int> SaveChangesAsync(string loggedInUserName = null);
        void SetEntityValueByPropertyName(TEntity entity, string propertyName, object value);
        TEntity Update(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false, string regexMatchPrimaryKeyIdPattern = null);
        Task<TEntity> UpdateAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false, string regexMatchPrimaryKeyIdPattern = null);
    }
}