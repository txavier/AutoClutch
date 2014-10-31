using System;
using System.Collections.Generic;
namespace AutoClutch.Auto.Repo.Interfaces
{
    public interface IRepository<TEntity>
     where TEntity : class
    {
        TEntity Add(TEntity entity, bool dontSave = false);
        System.Threading.Tasks.Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, bool dontSave = false);
        System.Threading.Tasks.Task<TEntity> AddAsync(TEntity entity, bool dontSave = false);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, bool dontSave = false);
        object GetEntityIdObject(TEntity entity);

        TEntity Delete(int id, bool dontSave = false);
        TEntity Delete(TEntity entity, bool dontSave = false);
        System.Threading.Tasks.Task<TEntity> DeleteAsync(int id, bool dontSave = false);
        System.Collections.Generic.IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<System.Linq.IQueryable<TEntity>, System.Collections.Generic.IEnumerable<TEntity>> distinctBy = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, Func<System.Collections.Generic.IEnumerable<TEntity>, System.Collections.Generic.IEnumerable<TEntity>> maxBy = null, int? skip = null, int? take = null, string includeProperties = "");
        IEnumerable<TEntity> GetAll();
        int GetCount(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<System.Linq.IQueryable<TEntity>, System.Collections.Generic.IEnumerable<TEntity>> distinctBy = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, Func<System.Collections.Generic.IEnumerable<TEntity>, System.Collections.Generic.IEnumerable<TEntity>> maxBy = null, string includeProperties = "");
        TEntity Find(object entityId);
        System.Threading.Tasks.Task<TEntity> FindAsync(object entityId);
        int SaveChanges();
        System.Threading.Tasks.Task<int> SaveChangesAsync();
        TEntity Update(TEntity entity, bool dontSave = false);
        System.Threading.Tasks.Task<TEntity> UpdateAsync(TEntity entity, bool dontSave = false);
    }
}
