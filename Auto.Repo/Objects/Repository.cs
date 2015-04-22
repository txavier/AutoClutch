using Auto.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using AutoClutch.Auto.Repo.Interfaces;

namespace AutoClutch.Auto.Repo.Objects
{
    public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;

        private readonly DbSet<TEntity> _dbSet;
        
        public Repository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;

            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// This method gets a single entity by its primary key.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public TEntity Find(object entityId)
        {
            var result = _dbSet.Find(entityId);

            return result;
        }

        public async Task<TEntity> FindAsync(object entityId)
        {
            var result = await _dbSet.FindAsync(entityId);

            return result;
        }

        private IQueryable<TEntity> GetAllAsQueryable()
        {
            return _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            var result = GetAllAsQueryable().ToList();

            return result;
        }

        /// <summary>
        /// This method returns data from the database.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="distinctBy">
        /// This is the distinct by parameter that you can pass a lamdba function to for evaluation.
        /// http://pranayamr.blogspot.com/2013/01/distinctby-in-linq.html
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
        /// http://www.codeproject.com/Articles/535374/DistinctBy-in-Linq-Find-Distinct-object-by-Propert
        /// </remarks>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IEnumerable<TEntity>> distinctBy = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IEnumerable<TEntity>, IEnumerable<TEntity>> maxBy = null,
            int? skip = null, int? take = null, string includeProperties = "",
            bool lazyLoadingEnabled = true, bool proxyCreationEnabled = true)
        {
            _context.Configuration.LazyLoadingEnabled = lazyLoadingEnabled;

            _context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;

            if (skip.HasValue && orderBy == null)
            {
                throw new ArgumentNullException("orderby");
            }

            skip = skip ?? 0;

            take = take ?? Int32.MaxValue;

            IEnumerable<TEntity> resultEnumerable = GetQuery(filter, distinctBy, orderBy, maxBy, includeProperties);

            resultEnumerable = resultEnumerable.Skip(skip.Value).Take(take.Value);

            List<TEntity> resultList = new List<TEntity>();

            resultList = resultEnumerable.ToList();

            return resultList;
        }

        private IEnumerable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IEnumerable<TEntity>> distinctBy,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            Func<IEnumerable<TEntity>, IEnumerable<TEntity>> maxBy,
            string includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            IEnumerable<TEntity> resultEnumerable = null;

            // Implements distinct and orderby.
            if (orderBy != null && distinctBy != null)
            {
                var result = distinctBy(orderBy(query));

                resultEnumerable = result;
            }
            else if (orderBy != null)
            {
                var result = orderBy(query);

                resultEnumerable = result;
            }
            else if (distinctBy != null)
            {
                var result = maxBy == null ? distinctBy(query) : maxBy(distinctBy(query));

                resultEnumerable = result;
            }
            else
            {
                var result = maxBy == null ? query : maxBy(query);

                resultEnumerable = result;
            }

            return resultEnumerable;
        }


        public virtual int GetCount(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IEnumerable<TEntity>> distinctBy = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IEnumerable<TEntity>, IEnumerable<TEntity>> maxBy = null,
            string includeProperties = "")
        {
            var result = GetQuery(filter, distinctBy, orderBy, maxBy, includeProperties).Count();

            return result;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, bool dontSave = false)
        {
            _dbSet.AddRange(entities);

            if (!dontSave)
            {
                await SaveChangesAsync();
            }

            return entities;
        }

        public async Task<TEntity> AddAsync(TEntity entity, bool dontSave = false)
        {
            _dbSet.Add(entity);

            if (!dontSave)
            {
                await SaveChangesAsync();
            }

            return entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, bool dontSave = false)
        {
            _dbSet.AddRange(entities);

            if (!dontSave)
            {
                SaveChanges();
            }

            return entities;
        }

        public TEntity Add(TEntity entity, bool dontSave = false)
        {
            _dbSet.Add(entity);

            if (!dontSave)
            {
                SaveChanges();
            }

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool dontSave = false)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _context.Entry(entity).State = EntityState.Modified;

            if (!dontSave)
            {
                await SaveChangesAsync();
            }

            return entity;
        }

        /// <summary>
        /// This method gets the name of the generic objects primary key.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>http://michaelmairegger.wordpress.com/2013/03/30/find-primary-keys-from-entities-from-dbcontext/</remarks>
        public string GetEntityKeyName(TEntity entity)
        {
            var result = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<TEntity>().EntitySet.ElementType.
                KeyMembers.Select(k => k.Name).ToArray().FirstOrDefault();

            return result;
        }

        /// <summary>
        /// This method returns the value of the primary key of this entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public object GetEntityIdObject(TEntity entity)
        {
            Type type = typeof(TEntity);

            var keyName = GetEntityKeyName(entity);

            var result = type.GetProperty(keyName).GetValue(entity, null);

            return result;
        }

        public TEntity Update(TEntity entity, bool dontSave = false)
        {
            // Get the value of the primary key.
            var id = GetEntityIdObject(entity);

            // Get the orginal object from the database.
            TEntity baseEntity = Find(id);

            // Using ValueInjector to inject the updated values into the context connected entity.
            baseEntity.InjectFrom(entity);

            if (!dontSave)
            {
                SaveChanges();
            }

            return entity;
        }

        public TEntity Delete(TEntity entity, bool dontSave = false)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);

            if (!dontSave)
            {
                SaveChanges();
            }

            return entity;
        }

        public TEntity Delete(int id, bool dontSave = false)
        {
            var entity = _dbSet.Find(id);

            var result = Delete(entity, dontSave: dontSave);

            return entity;
        }

        public async Task<TEntity> DeleteAsync(int id, bool dontSave = false)
        {
            var entity = await _dbSet.FindAsync(id);

            var result = await DeleteAsync(entity, dontSave: dontSave);

            return result;
        }

        private async Task<TEntity> DeleteAsync(TEntity entity, bool dontSave = false)
        {
            if (entity == null)
            {
                return entity;
            }

            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);

            if (!dontSave)
            {
                await SaveChangesAsync();
            }

            return entity;
        }

        /// <summary>
        /// Wrapper for SaveChanges adding the Validation Messages to the generated exception
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>http://stackoverflow.com/questions/10219864/ef-code-first-how-do-i-see-entityvalidationerrors-property-from-the-nuget-pac</remarks>
        public int SaveChanges()
        {
            try
            {
                var saveChangesInt = _context.SaveChanges();

                return saveChangesInt;
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                var saveChangesInt = await _context.SaveChangesAsync();

                return saveChangesInt;
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }

        /// <summary>
        /// This method gets the property names of a generic object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IEnumerable<string> GetEntityPropertyNames(TEntity entity)
        {
            var result = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<TEntity>().EntitySet.ElementType.Members.Select(i => i.Name);

            return result;
        }

        /// <summary>
        /// This method sets the entities property to a value by its string property name.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void SetEntityValueByPropertyName(TEntity entity, string propertyName, object value)
        {
            Type type = typeof(TEntity);

            type.GetProperty(propertyName).SetValue(entity, value);
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

    }
}
