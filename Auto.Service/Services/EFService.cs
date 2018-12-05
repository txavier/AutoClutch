﻿using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Objects;
using AutoClutch.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace AutoClutch.Core
{
    public class EFService<TEntity> : IEFService<TEntity> where TEntity : class, new()
    {
        private readonly IEFRepository<TEntity> _repository;

        public IEnumerable<Error> Errors { get; set; }

        private bool disposedValue = false; // To detect redundant calls

        public IValidation<TEntity> Validation { get; set; }

        public bool ProxyCreationEnabled
        {
            get { return _repository.ProxyCreationEnabled; }
            set { _repository.ProxyCreationEnabled = value; }
        }

        public bool LazyLoadingEnabled
        {
            get { return _repository.LazyLoadingEnabled; }
            set { _repository.LazyLoadingEnabled = value; }
        }

        public bool AutoDetectChangesEnabled
        {
            get { return _repository.AutoDetectChangesEnabled; }
            set { _repository.AutoDetectChangesEnabled = value; }
        }

        public bool EnsureTransactionsForFunctionsAndCommands
        {
            get { return _repository.EnsureTransactionsForFunctionsAndCommands; }
            set { _repository.EnsureTransactionsForFunctionsAndCommands = value; }
        }

        public bool ValidateOnSaveEnabled
        {
            get { return _repository.ValidateOnSaveEnabled; }
            set { _repository.ValidateOnSaveEnabled = value; }
        }

        public EFService(IEFRepository<TEntity> repository)
        {
            this._repository = repository;

            Errors = new List<Error>();
        }

        [Obsolete("Find(object, null, null, null, null) is deprecated, please use FindAsync(entityId, includeSoftDeleted) instead.")]
        public virtual async Task<TEntity> FindAsync(object entityId, bool? lazyLoadingEnabled, bool? proxyCreationEnabled, bool autoDetectChangesEnabled = true,
            bool? includeSoftDeleted = null)
        {
            lazyLoadingEnabled = lazyLoadingEnabled ?? true;
            proxyCreationEnabled = proxyCreationEnabled ?? true;

            var entity = await _repository.FindAsync(entityId, lazyLoadingEnabled: lazyLoadingEnabled.Value, proxyCreationEnabled: proxyCreationEnabled.Value, autoDetectChangesEnabled: autoDetectChangesEnabled);

            // If this is an entity with an interface ISoftdeletable and it is 
            // set to deleted then dont indicate that this object exists unless
            // we are checking for deleted objects also.
            if (IsIncludeSoftDeleted(includeSoftDeleted, entity))
            {
                return null;
            }

            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="lazyLoadingEnabled">The default value for this is true.</param>
        /// <param name="proxyCreationEnabled">The default value for this is true.</param>
        /// <param name="autoDetectChangesEnabled"></param>
        /// <param name="includeSoftDeleted"></param>
        /// <returns></returns>
        [Obsolete("Find(object, null, null, null, null) is deprecated, please use Find(entityId, includeSoftDeleted) instead.")]
        public virtual TEntity Find(object entityId, bool? lazyLoadingEnabled, bool? proxyCreationEnabled, bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = null)
        {
            lazyLoadingEnabled = lazyLoadingEnabled ?? true;
            proxyCreationEnabled = proxyCreationEnabled ?? true;

            var entity = _repository.Find(entityId, lazyLoadingEnabled: lazyLoadingEnabled.Value, proxyCreationEnabled: proxyCreationEnabled.Value, autoDetectChangesEnabled: autoDetectChangesEnabled);

            // If this is an entity with an interface ISoftdeletable and it is 
            // set to deleted then dont indicate that this object exists unless
            // we are checking for deleted objects also.
            if (IsIncludeSoftDeleted(includeSoftDeleted, entity))
            {
                return null;
            }

            return entity;
        }

        public virtual IEnumerable<TEntity> GetAll(bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = null)
        {
            var result = _repository.GetAll(lazyLoadingEnabled: lazyLoadingEnabled, proxyCreationEnabled: proxyCreationEnabled, autoDetectChangesEnabled: autoDetectChangesEnabled);

            // If this is an entity with an interface ISoftdeletable and it is 
            // set to deleted then dont return this object unless
            // we are checking for deleted objects also.
            if (IsIncludeSoftDeleted(includeSoftDeleted))
            {
                result = result.Where(i => !((ISoftDeletable)result).IsDeleted);
            }

            return result;
        }

        public virtual TEntity Add(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false)
        {
            if (!IsValid(entity, loggedInUserName))
            {
                return null;
            }

            var result = _repository.Add(entity, loggedInUserName, lazyLoadingEnabled: lazyLoadingEnabled, proxyCreationEnabled: proxyCreationEnabled, autoDetectChangesEnabled: autoDetectChangesEnabled, dontSave: dontSave);

            ConcatRepositoryErrors();

            return result;
        }

        private void ConcatRepositoryErrors()
        {
            if (Errors.Count() < 5)
            {
                Errors = Errors.Concat(_repository.Errors);
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false)
        {
            if (!IsValid(entity, loggedInUserName))
            {
                return null;
            }

            var result = await _repository.AddAsync(entity, loggedInUserName, lazyLoadingEnabled: lazyLoadingEnabled, proxyCreationEnabled: proxyCreationEnabled, autoDetectChangesEnabled: autoDetectChangesEnabled, dontSave: dontSave);

            ConcatRepositoryErrors();

            return result;
        }

        /// <summary>
        /// This mehod adds a range of entities to the database.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="dontSave"></param>
        /// <returns></returns>
        /// <remarks>Please note at this time auditing is not enabled for AddRange methods.</remarks>
        public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false)
        {
            var validEntities = new List<TEntity>();

            foreach (var entity in entities)
            {
                if (!IsValid(entity, loggedInUserName))
                {
                    return null;
                }
                else
                {
                    validEntities.Add(entity);
                }
            }

            var result = _repository.AddRange(validEntities, loggedInUserName, lazyLoadingEnabled: lazyLoadingEnabled, proxyCreationEnabled: proxyCreationEnabled, autoDetectChangesEnabled: autoDetectChangesEnabled, dontSave: dontSave);

            ConcatRepositoryErrors();

            return result;
        }

        public virtual TEntity Update(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false)
        {
            if (!IsValid(entity, loggedInUserName))
            {
                ((List<Error>)Errors).AddRange(Validation.Errors);

                return null;
            }

            var result = _repository.Update(entity, loggedInUserName, lazyLoadingEnabled: lazyLoadingEnabled, proxyCreationEnabled: proxyCreationEnabled, autoDetectChangesEnabled: autoDetectChangesEnabled, dontSave: dontSave);

            ConcatRepositoryErrors();

            return result;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false)
        {
            if (!IsValid(entity, loggedInUserName))
            {
                ((List<Error>)Errors).AddRange(Validation.Errors);

                return null;
            }

            var result = await _repository.UpdateAsync(entity, loggedInUserName, lazyLoadingEnabled: lazyLoadingEnabled, proxyCreationEnabled: proxyCreationEnabled, autoDetectChangesEnabled: autoDetectChangesEnabled, dontSave: dontSave);

            ConcatRepositoryErrors();

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
        public virtual TEntity AddOrUpdate(TEntity entity, string loggedInUserName = null, bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true, bool autoDetectChangesEnabled = true, bool dontSave = false)
        {
            if (!IsValid(entity, loggedInUserName))
            {
                return null;
            }

            var idObject = _repository.GetEntityIdObject(entity);

            if ((int)idObject == 0)
            {
                var newEntity = _repository.Add(entity, loggedInUserName, lazyLoadingEnabled: lazyLoadingEnabled, proxyCreationEnabled: proxyCreationEnabled, autoDetectChangesEnabled: autoDetectChangesEnabled, dontSave: dontSave);

                ConcatRepositoryErrors();

                return newEntity;
            }
            else
            {
                var updatedEntity = _repository.Update(entity, loggedInUserName, lazyLoadingEnabled: lazyLoadingEnabled, proxyCreationEnabled: proxyCreationEnabled, autoDetectChangesEnabled: autoDetectChangesEnabled, dontSave: dontSave);

                ConcatRepositoryErrors();

                return updatedEntity;
            }
        }

        public virtual TEntity Delete(int id, string loggedInUserName = null, bool softDelete = false, bool dontSave = false)
        {
            if (softDelete)
            {
                var entity = Find(id);

                if (entity is ISoftDeletable)
                {
                    (entity as ISoftDeletable).IsDeleted = true;

                    var softDeleteResult = Update(entity, loggedInUserName: loggedInUserName, dontSave: dontSave);

                    return softDeleteResult;
                }
            }

            var result = _repository.Delete(id, loggedInUserName, dontSave: dontSave);

            ConcatRepositoryErrors();

            return result;
        }

        public virtual async Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool softDelete = false, bool dontSave = false)
        {
            if (softDelete)
            {
                var entity = await FindAsync(id);

                if (entity is ISoftDeletable)
                {
                    (entity as ISoftDeletable).IsDeleted = true;

                    var softDeleteResult = await UpdateAsync(entity, loggedInUserName, dontSave: dontSave);

                    return softDeleteResult;
                }
            }

            var result = await _repository.DeleteAsync(id, loggedInUserName, dontSave: dontSave);

            ConcatRepositoryErrors();

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
        public virtual IEnumerable<TEntity> Get(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null,
            string filterString = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> distinctBy = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string orderByString = null,
            Func<IEnumerable<TEntity>, IQueryable<TEntity>> maxBy = null,
            int? skip = null,
            int? take = null,
            string includeProperties = "",
            bool lazyLoadingEnabled = true,
            bool proxyCreationEnabled = true,
            bool autoDetectChangesEnabled = true, bool? includeSoftDeleted = null)
        {
            // If this is an entity with an interface ISoftdeletable and it is 
            // set to deleted then dont return this object unless
            // we are checking for deleted objects also.
            if (IsIncludeSoftDeleted(includeSoftDeleted))
            {
                if (string.IsNullOrWhiteSpace(filterString))
                {
                    filterString += "IsDeleted=false";
                }
                else
                {
                    filterString += " AND IsDeleted=false";
                }
            }

            var result = _repository.Get(
                filter: filter,
                filterString: filterString,
                distinctBy: distinctBy,
                orderBy: orderBy,
                orderByString: orderByString,
                maxBy: maxBy,
                skip: skip,
                take: take,
                includeProperties: includeProperties,
                lazyLoadingEnabled: lazyLoadingEnabled,
                proxyCreationEnabled: proxyCreationEnabled,
                autoDetectChanges: autoDetectChangesEnabled);

            return result;
        }

        public virtual int GetCount(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null,
            string filterString = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> distinctBy = null,
            Func<IEnumerable<TEntity>, IQueryable<TEntity>> maxBy = null, bool? includeSoftDeleted = null)
        {
            // If this is an entity with an interface ISoftdeletable and it is 
            // set to deleted then dont return this object unless
            // we are checking for deleted objects also.
            if (IsIncludeSoftDeleted(includeSoftDeleted))
            {
                filterString += " AND IsDeleted=false";
            }

            var result = _repository.GetCount(
                filter: filter,
                filterString: filterString,
                distinctBy: distinctBy,
                maxBy: maxBy);

            return result;
        }

        /// <summary>
        /// If the return value is false then check the 'Errors' property.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid(TEntity entity, string loggedInUserName = null)
        {
            // If ValidateOnSaveEnabled is set to false then we are going to 
            // skip validation.
            if(!ValidateOnSaveEnabled)
            {
                return true;
            }

            var valid = (Validation?.IsValid(entity, loggedInUserName, this) ?? true);

            if(!valid && (Validation != null))
            {
                ((List<Error>)Errors).AddRange(Validation.Errors);
            }

            return valid;
        }

        public virtual int SaveChanges(string loggedInUserName = null)
        {
            var result = _repository.SaveChanges(loggedInUserName);

            return result;
        }

        public virtual async Task<int> SaveChangesAsync(string loggedInUserName = null)
        {
            var result = await _repository.SaveChangesAsync(loggedInUserName);

            return result;
        }

        public virtual bool Exists(object entityIdObject, bool? includeSoftDeleted = null)
        {
            if (entityIdObject == null)
            {
                return false;
            }

            var found = false;

            var result = Find(entityIdObject, includeSoftDeleted: includeSoftDeleted);

            if (result != null)
            {
                found = true;
            }

            return found;
        }

        public virtual async Task<bool> ExistsAsync(object entityIdObject, bool? includeSoftDeleted = null)
        {
            if (entityIdObject == null)
            {
                return false;
            }

            var found = false;

            var result = await FindAsync(entityIdObject, includeSoftDeleted: includeSoftDeleted);

            if (result != null)
            {
                found = true;
            }

            return found;
        }
        /// <summary>
        /// This method gets the property names of a generic object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<string> GetEntityPropertyNames(TEntity entity, bool? includeSoftDeleted = null)
        {
            // If this is an entity with an interface ISoftdeletable and it is 
            // set to deleted then dont return this object unless
            // we are checking for deleted objects also.
            if (IsIncludeSoftDeleted(includeSoftDeleted, entity))
            {
                return new List<string>();
            }

            var result = _repository.GetEntityPropertyNames(entity);

            return result;
        }

        public static bool IsIncludeSoftDeleted(bool? includeSoftDeleted)
        {
            var result = (!(includeSoftDeleted ?? false) && (new TEntity() is ISoftDeletable));

            return result;
        }


        public static bool IsIncludeSoftDeleted(bool? includeSoftDeleted, TEntity entity, out ISoftDeletable softDeletableEntity)
        {
            var result = (!(includeSoftDeleted ?? false) && (entity is ISoftDeletable));

            if (result)
            {
                softDeletableEntity = entity as ISoftDeletable;

                result = result && softDeletableEntity.IsDeleted;
            }
            else
            {
                softDeletableEntity = null;
            }

            return result;
        }

        /// <summary>
        /// This method is just like the method of the same name with one parameter.  However, if you use this one 
        /// and supply the entity that you are inspecting the result will be faster than that of the said method 
        /// because the first method has to new up a TEntity to determine if it is ISoftDeletable.
        /// </summary>
        /// <param name="includeSoftDeleted"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool IsIncludeSoftDeleted(bool? includeSoftDeleted, TEntity entity)
        {
            ISoftDeletable softDeletableEntity;

            var result = IsIncludeSoftDeleted(includeSoftDeleted, entity, out softDeletableEntity);

            return result;
        }

        public virtual object GetEntityIdObject(TEntity entity, bool? includeSoftDeleted = null)
        {
            // If this is an entity with an interface ISoftdeletable and it is 
            // set to deleted then dont return this object unless
            // we are checking for deleted objects also.
            if (IsIncludeSoftDeleted(includeSoftDeleted, entity))
            {
                return null;
            }

            var idObject = _repository.GetEntityIdObject(entity);

            return idObject;
        }

        public IEnumerable<Error> GetAnyAvailableValidationErrors()
        {
            ((List<Error>)Errors).AddRange(_repository.GetAnyAvailableValidationErrors());

            return Errors;
        }

        public virtual IQueryable<TEntity> Queryable(bool? includeSoftDeleted = null)
        {
            var result = _repository.Queryable();

            // If this is an entity with an interface ISoftdeletable and it is 
            // set to deleted then dont return this object unless
            // we are checking for deleted objects also.
            if (IsIncludeSoftDeleted(includeSoftDeleted) && result.Any())
            {
                result = result.Where("IsDeleted=false");
            }

            return result;
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects).
                    _repository.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override a finalizer below.
                // Set large fields to null.
                Errors = null;

                disposedValue = true;
            }
        }

        // Dispose(bool disposing) above has code to free unmanaged resources.
        ~EFService()
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

        public virtual string GetEntityKeyName(TEntity entity)
        {
            var result = _repository.GetEntityKeyName(entity);

            return result;
        }

        public TEntity Add(TEntity entity, string loggedInUserName = null)
        {
            var result = Add(entity, loggedInUserName, true);

            return result;
        }

        public async Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null)
        {
            var result = await AddAsync(entity, loggedInUserName, true);

            return result;
        }

        public TEntity Delete(int id, string loggedInUserName = null, bool softDelete = false)
        {
            var result = Delete(id, loggedInUserName, softDelete, false);

            return result;
        }

        public async Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool softDelete = false)
        {
            var result = await DeleteAsync(id, loggedInUserName, softDelete, false);

            return result;
        }

        public TEntity Update(TEntity entity, string loggedInUserName = null)
        {
            var result = Update(entity, loggedInUserName: loggedInUserName, lazyLoadingEnabled: true);

            return result;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, string loggedInUserName = null)
        {
            var result = await UpdateAsync(entity, loggedInUserName: loggedInUserName, lazyLoadingEnabled: true);

            return result;
        }

        public TEntity Find(object entityId, bool? includeSoftDeleted = null)
        {
            var result = Find(entityId, null, null, includeSoftDeleted: includeSoftDeleted);

            return result;
        }

        public async Task<TEntity> FindAsync(object entityId, bool? includeSoftDeleted = null)
        {
            var result = await FindAsync(entityId, null, null, includeSoftDeleted: includeSoftDeleted);

            return result;
        }
    }
}
