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

        public TEntity Add(TEntity entity, string loggedInUserName = null, bool dontSave = false)
        {
            var result = _repository.Add(entity, loggedInUserName, dontSave: dontSave);

            return result;
        }

        public async Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null, bool dontSave = false)
        {
            var result = await _repository.AddAsync(entity, loggedInUserName, dontSave: dontSave);

            return result;
        }

        /// <summary>
        /// This mehod adds a range of entities to the database.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="dontSave"></param>
        /// <returns></returns>
        /// <remarks>Please note at this time auditing is not enabled for AddRange methods.</remarks>
        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, string loggedInUserName = null, bool dontSave = false)
        {
            var result = await _repository.AddRangeAsync(entities, loggedInUserName, dontSave: dontSave);

            return result;
        }

        /// <summary>
        /// This mehod adds a range of entities to the database.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="dontSave"></param>
        /// <returns></returns>
        /// <remarks>Please note at this time auditing is not enabled for AddRange methods.</remarks>
        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, string loggedInUserName = null, bool dontSave = false)
        {
            var result = _repository.AddRange(entities, loggedInUserName, dontSave: dontSave);

            return result;
        }

        public TEntity Update(TEntity entity, string loggedInUserName = null, bool dontSave = false)
        {
            var result = _repository.Update(entity, loggedInUserName, dontSave: dontSave);

            return result;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, string loggedInUserName = null, bool dontSave = false)
        {
            var result = await _repository.UpdateAsync(entity, loggedInUserName, dontSave: dontSave);

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
        public TEntity AddOrUpdate(TEntity entity, string loggedInUserName = null, bool dontSave = false)
        {
            var idObject = _repository.GetEntityIdObject(entity);

            if((int)idObject == 0)
            {
                var newEntity = _repository.Add(entity, loggedInUserName, dontSave: dontSave);

                return newEntity;
            }
            else
            {
                var updatedEntity = _repository.Update(entity, loggedInUserName, dontSave: dontSave);

                return updatedEntity;
            }
        }

        public TEntity Delete(int id, string loggedInUserName = null, bool dontSave = false)
        {
            var result = _repository.Delete(id, loggedInUserName, dontSave: dontSave);

            return result;
        }

        public TEntity Delete(TEntity entity, string loggedInUserName = null, bool dontSave = false)
        {
            var result = _repository.Delete(entity, loggedInUserName, dontSave: dontSave);

            return result;
        }

        public async Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool dontSave = false)
        {
            var result = await _repository.DeleteAsync(id, loggedInUserName, dontSave: dontSave);

            return result;
        }

        /// <summary>
        /// This method returns data from the database.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties">This will tell entity framework to return the specified properties from the database.</param>
        /// <param name="distinctBy">
        /// /// This is the distinct by parameter that you can pass a lamdba function to for evaluation.
        /// http://pranayamr.blogspot.com/2013/01/distinctby-in-linq.html
        /// </param>
        /// <param name="searchParameters">
        /// If you have a enumerable list of strings and wish to use them to query
        /// data your data with instead of using the fluent i.e. (i => i.name == "facility2") you can query with
        /// a string of searchParameters that follow the form "name:facility2", "state:state3".
        /// </param>
        /// <returns></returns>
        public IEnumerable<TEntity> Get(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IEnumerable<TEntity>> distinctBy = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            Func<IEnumerable<TEntity>, IEnumerable<TEntity>> maxBy = null, 
            int? skip = null, 
            int? take = null, 
            string includeProperties = "",
            IEnumerable<string> searchParameters = null,
            bool lazyLoadingEnabled = true, 
            bool proxyCreationEnabled = true)
        {
            var result = _repository.Get(
                filter: filter,
                distinctBy: distinctBy,
                orderBy: orderBy,
                maxBy: maxBy,
                skip: skip,
                take: take,
                includeProperties: includeProperties,
                searchParameters: searchParameters,
                lazyLoadingEnabled: lazyLoadingEnabled,
                proxyCreationEnabled: proxyCreationEnabled);

            return result;
        }

        public int GetCount(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, 
            IEnumerable<TEntity>> distinctBy = null, 
            Func<IQueryable<TEntity>, 
            IOrderedQueryable<TEntity>> orderBy = null, 
            Func<IEnumerable<TEntity>, 
            IEnumerable<TEntity>> maxBy = null, 
            string includeProperties = "",
            IEnumerable<string> searchParameters = null)
        {
            var result = _repository.GetCount(
                filter: filter,
                distinctBy: distinctBy,
                orderBy: orderBy,
                maxBy: maxBy,
                includeProperties: includeProperties,
                searchParameters: searchParameters);

            return result;
        }

        public int SaveChanges(string loggedInUserName = null)
        {
            var result = _repository.SaveChanges(loggedInUserName);

            return result;
        }

        public async Task<int> SaveChangesAsync(string loggedInUserName = null)
        {
            var result = await _repository.SaveChangesAsync(loggedInUserName);

            return result;
        }

        /// <summary>
        /// This method gets the property names of a generic object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<string> GetEntityPropertyNames(TEntity entity)
        {
            var result = _repository.GetEntityPropertyNames(entity);

            return result;
        }

    }
}
