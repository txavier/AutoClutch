using System;
namespace AutoClutch.Auto.Service.Interfaces
{
    public interface IService<TEntity>
     where TEntity : class
    {
        System.Collections.Generic.IEnumerable<Auto.Service.Objects.Error> Errors { get; set; }
        TEntity Add(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool dontSave = false);
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<TEntity>> AddRangeAsync(System.Collections.Generic.IEnumerable<TEntity> entities, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool dontSave = false);
        System.Threading.Tasks.Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool dontSave = false);
        System.Collections.Generic.IEnumerable<TEntity> AddRange(System.Collections.Generic.IEnumerable<TEntity> entities, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool dontSave = false);
        TEntity AddOrUpdate(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool dontSave = false);
        TEntity Delete(int id, string loggedInUserName = null, bool dontSave = false);
        TEntity Delete(TEntity entity, string loggedInUserName = null, bool dontSave = false);
        System.Threading.Tasks.Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool dontSave = false);
        System.Collections.Generic.IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, string filterString = null, Func<System.Linq.IQueryable<TEntity>, System.Collections.Generic.IEnumerable<TEntity>> distinctBy = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, string orderByString = null, Func<System.Collections.Generic.IEnumerable<TEntity>, System.Collections.Generic.IEnumerable<TEntity>> maxBy = null, int? skip = null, int? take = null, string includeProperties = "", bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true);
        System.Collections.Generic.IEnumerable<TEntity> GetAll(bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true);
        int GetCount(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, string filterString = null, Func<System.Linq.IQueryable<TEntity>, System.Collections.Generic.IEnumerable<TEntity>> distinctBy = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, string orderByString = null, Func<System.Collections.Generic.IEnumerable<TEntity>, System.Collections.Generic.IEnumerable<TEntity>> maxBy = null, string includeProperties = "");
        TEntity Find(object entityId, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true);
        System.Threading.Tasks.Task<TEntity> FindAsync(object entityId, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true);
        int SaveChanges(string loggedInUserName = null);
        System.Threading.Tasks.Task<int> SaveChangesAsync(string loggedInUserName = null);
        TEntity Update(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool dontSave = false);
        System.Threading.Tasks.Task<TEntity> UpdateAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool dontSave = false);
        System.Collections.Generic.IEnumerable<string> GetEntityPropertyNames(TEntity entity);
        object GetEntityIdObject(TEntity entity);
        bool Exists(object entityIdObject);
    }
}
