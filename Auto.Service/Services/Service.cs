using AutoClutch.Auto.Repo.Interfaces;
using AutoClutch.Auto.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClutch.Auto.Service.Services
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> _repository;

        public Service(IRepository<TEntity> repository)
        {
            this._repository = repository;
        }

        public async Task<TEntity> FindAsync(object entityId)
        {
            return await _repository.FindAsync(entityId);
        }

        public TEntity Find(object entityId)
        {
            return _repository.Find(entityId);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var result = _repository.GetAll().ToList();

            return result;
        }

        public TEntity Add(TEntity entity, bool dontSave = false)
        {
            var result = _repository.Add(entity, dontSave: dontSave);

            return result;
        }

        public async Task<TEntity> AddAsync(TEntity entity, bool dontSave = false)
        {
            var result = await _repository.AddAsync(entity, dontSave: dontSave);

            return result;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, bool dontSave = false)
        {
            var result = await _repository.AddRangeAsync(entities, dontSave: dontSave);

            return result;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, bool dontSave = false)
        {
            var result = _repository.AddRange(entities, dontSave: dontSave);

            return result;
        }

        public TEntity Update(TEntity entity, bool dontSave = false)
        {
            var result = _repository.Update(entity, dontSave: dontSave);

            return result;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool dontSave = false)
        {
            var result = await _repository.UpdateAsync(entity, dontSave: dontSave);

            return result;
        }

        /// <summary>
        /// This method adds this entity to the database if the key is 0 but updates
        /// the object if the key is 1.  This method assumes your primary key is an 
        /// integer.
        /// </summary>
        /// <param name="entity">This is the entity that must have an integer key.</param>
        /// <param name="dontSave"></param>
        /// <returns></returns>
        public TEntity AddOrUpdate(TEntity entity, bool dontSave = false)
        {
            var idObject = _repository.GetEntityIdObject(entity);

            if((int)idObject == 0)
            {
                var newEntity = _repository.Add(entity, dontSave: dontSave);

                return newEntity;
            }
            else
            {
                var updatedEntity = _repository.Update(entity, dontSave: dontSave);

                return updatedEntity;
            }
        }

        public TEntity Delete(int id, bool dontSave = false)
        {
            var result = _repository.Delete(id, dontSave: dontSave);

            return result;
        }

        public TEntity Delete(TEntity entity, bool dontSave = false)
        {
            var result = _repository.Delete(entity, dontSave: dontSave);

            return result;
        }

        public async Task<TEntity> DeleteAsync(int id, bool dontSave = false)
        {
            var result = await _repository.DeleteAsync(id, dontSave: dontSave);

            return result;
        }

        public IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IEnumerable<TEntity>> distinctBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IEnumerable<TEntity>, IEnumerable<TEntity>> maxBy = null, int? skip = null, int? take = null, string includeProperties = "")
        {
            var result = _repository.Get(
                filter: filter,
                distinctBy: distinctBy,
                orderBy: orderBy,
                maxBy: maxBy,
                includeProperties: includeProperties);

            return result;
        }

        public int GetCount(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, 
            IEnumerable<TEntity>> distinctBy = null, 
            Func<IQueryable<TEntity>, 
            IOrderedQueryable<TEntity>> orderBy = null, 
            Func<IEnumerable<TEntity>, 
            IEnumerable<TEntity>> maxBy = null, 
            string includeProperties = "")
        {
            var result = _repository.GetCount(
                filter: filter,
                distinctBy: distinctBy,
                orderBy: orderBy,
                maxBy: maxBy,
                includeProperties: includeProperties);

            return result;
        }

        public int SaveChanges()
        {
            var result = _repository.SaveChanges();

            return result;
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _repository.SaveChangesAsync();

            return result;
        }

    }
}
