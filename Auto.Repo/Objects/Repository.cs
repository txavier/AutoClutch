using Auto.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using AutoClutch.Auto.Repo.Interfaces;
using System.Text.RegularExpressions;
using System.Linq.Dynamic;

namespace AutoClutch.Auto.Repo.Objects
{
    public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
    {
        private readonly TrackerEnabledDbContext.TrackerContext _context;

        private readonly DbSet<TEntity> _dbSet;

        public string RegexMatchPrimaryKeyIdPattern { get; set; }

        public Repository(TrackerEnabledDbContext.TrackerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;

            _dbSet = context.Set<TEntity>();

            RegexMatchPrimaryKeyIdPattern = null;
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
        /// <remarks>
        /// http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
        /// http://www.codeproject.com/Articles/535374/DistinctBy-in-Linq-Find-Distinct-object-by-Propert
        /// </remarks>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            string searchParameters = null,
            Func<IQueryable<TEntity>, IEnumerable<TEntity>> distinctBy = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string orderByString = null,
            Func<IEnumerable<TEntity>, IEnumerable<TEntity>> maxBy = null,
            int? skip = null, 
            int? take = null, 
            string includeProperties = "",
            bool lazyLoadingEnabled = true, 
            bool proxyCreationEnabled = true)
        {
            _context.Configuration.LazyLoadingEnabled = lazyLoadingEnabled;

            _context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;

            skip = skip ?? 0;

            take = take ?? Int32.MaxValue;

            IEnumerable<TEntity> resultEnumerable = GetQuery(filter, searchParameters, distinctBy, orderBy, orderByString, maxBy, includeProperties);

            if(resultEnumerable.Any())
            {
                return new List<TEntity>();
            }

            // Skip and take require an ordered enumerable.  So if no orderby was passed and
            // a skip value is given, order by the primary key.
            if (skip.HasValue && orderBy == null && orderByString == null)
            {
                var entityKeyName = GetEntityKeyName(resultEnumerable.First());

                resultEnumerable.OrderBy(entityKeyName);
            }

            resultEnumerable = resultEnumerable.Skip(skip.Value).Take(take.Value);

            List<TEntity> resultList = new List<TEntity>();

            resultList = resultEnumerable.ToList();

            return resultList;
        }

        private IEnumerable<TEntity> GetQuery(
            Expression<Func<TEntity, bool>> filter,
            string searchParameters,
            Func<IQueryable<TEntity>, IEnumerable<TEntity>> distinctBy,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string orderByString,
            Func<IEnumerable<TEntity>, IEnumerable<TEntity>> maxBy,
            string includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(searchParameters))
            {
                query = query.Where(searchParameters);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (!string.IsNullOrWhiteSpace(orderByString))
            {
                query = query.OrderBy(orderByString);
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
            string searchParameters = null,
            Func<IQueryable<TEntity>, IEnumerable<TEntity>> distinctBy = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string orderByString = null,
            Func<IEnumerable<TEntity>, IEnumerable<TEntity>> maxBy = null,
            string includeProperties = "")
        {
            var result = GetQuery(filter, searchParameters, distinctBy, orderBy, orderByString, maxBy, includeProperties).Count();

            return result;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, string loggedInUserName = null, bool dontSave = false)
        {
            _dbSet.AddRange(entities);

            if (!dontSave)
            {
                await SaveChangesAsync(loggedInUserName);
            }

            return entities;
        }

        public async Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null, bool dontSave = false)
        {
            _context.Entry(entity).State = EntityState.Added;

            if (!dontSave)
            {
                await SaveChangesAsync(loggedInUserName);
            }

            return entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, string loggedInUserName = null, bool dontSave = false)
        {
            _dbSet.AddRange(entities);

            if (!dontSave)
            {
                SaveChanges(loggedInUserName);
            }

            return entities;
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/data/jj592676.aspx
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dontSave"></param>
        /// <returns></returns>
        public TEntity Add(TEntity entity, string loggedInUserName = null, bool dontSave = false)
        {
            _context.Entry(entity).State = EntityState.Added;

            if (!dontSave)
            {
                SaveChanges(loggedInUserName);
            }

            var id = GetEntityIdObject(entity);

            TEntity baseEntity = Find(id);

            return baseEntity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, string loggedInUserName = null, bool dontSave = false, string regexMatchPrimaryKeyIdPattern = null)
        {
            // Call the update method already implemented and at the end 
            // await the save changes if dont save was passed as true.
            Update(entity, dontSave: false, regexMatchPrimaryKeyIdPattern: regexMatchPrimaryKeyIdPattern);

            if (!dontSave)
            {
                await SaveChangesAsync(loggedInUserName);
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

        /// <summary>
        /// https://msdn.microsoft.com/en-us/data/jj592676.aspx
        /// http://www.entityframeworktutorial.net/EntityFramework5/update-entity-graph-using-dbcontext.aspx
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dontSave"></param>
        /// <param name="regexMatchPrimaryKeyIdPattern">In order to update child models in your model this method 
        /// needs to find the primary key of those child models. If you follow the naming convention like this
        /// [Your Table Name]Id for your primary key then this will happen automatically for you.  Otherwise if 
        /// you follow a different naming convention, use this parameter to indicate the regex that will match 
        /// the primary key of your table.</param>
        /// <returns></returns>
        public TEntity Update(TEntity entity, string loggedInUserName = null, bool dontSave = false, string regexMatchPrimaryKeyIdPattern = null)
        {
            var id = GetEntityIdObject(entity);

            TEntity baseEntity = Find(id);

            _context.Entry(baseEntity).CurrentValues.SetValues(entity);

            _context.Entry(baseEntity).State = EntityState.Modified;

            // Get all of the entries that are in the state of added and check to see if 
            // their primary key exists and set them to modified if this key is not 0.
            // This prevents duplicate child elements from being added to the database.
            var entries = _context.ChangeTracker.Entries().Where(i => i.State == EntityState.Added);

            foreach (var entry in entries)
            {
                // If the convention was used [Table Name]Id for the primary key then
                // try to get this primary key and use it to set the state of the model
                // to modified so that the database can update it if the model's 
                // primary key is a number that is not 0.
                string idPropertyName = null;

                idPropertyName = RetrieveChildModelId(regexMatchPrimaryKeyIdPattern, entry);

                // Only change the state if the id name of this model can be found.
                if (idPropertyName != null && entry.CurrentValues.PropertyNames.Contains(idPropertyName))
                {
                    var entryId = entry.Property(idPropertyName).CurrentValue;

                    if (entryId != null && entryId.GetType().Name == "Int32" && (int)entryId != 0)
                    {
                        entry.State = EntityState.Modified;
                    }
                }
            }

            if (!dontSave)
            {
                SaveChanges(loggedInUserName);
            }

            return baseEntity;
        }

        /// <summary>
        /// Retrieve the primary key name of the child model.
        /// </summary>
        /// <param name="regexMatchPrimaryKeyIdPattern"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        private string RetrieveChildModelId(string regexMatchPrimaryKeyIdPattern, DbEntityEntry entry)
        {
            var entityType = entry.Entity.GetType();

            var idPropertyName = string.Empty;

            if (entry.CurrentValues.PropertyNames.Contains(entityType.Name + "Id"))
            {
                idPropertyName = entityType.Name + "Id";
            }
            else if (entry.CurrentValues.PropertyNames.Contains(entityType.Name.ToLower() + "Id"))
            {
                idPropertyName = entityType.Name.ToLower() + "Id";
            }
            else if ((regexMatchPrimaryKeyIdPattern ?? this.RegexMatchPrimaryKeyIdPattern) != null)
            {
                foreach (var propertyName in entry.CurrentValues.PropertyNames)
                {
                    if (Regex.IsMatch(propertyName, (regexMatchPrimaryKeyIdPattern ?? this.RegexMatchPrimaryKeyIdPattern)))
                    {
                        idPropertyName = propertyName;

                        break;
                    }
                }
            }

            // If nothing was found return null.
            idPropertyName = idPropertyName == string.Empty ? null : idPropertyName;

            return idPropertyName;
        }

        public TEntity Delete(TEntity entity, string loggedInUserName = null, bool dontSave = false)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);

            if (!dontSave)
            {
                SaveChanges(loggedInUserName);
            }

            return entity;
        }

        public TEntity Delete(int id, string loggedInUserName = null, bool dontSave = false)
        {
            var entity = _dbSet.Find(id);

            var result = Delete(entity, dontSave: dontSave);

            return entity;
        }

        public async Task<TEntity> DeleteAsync(int id, string loggedInUserName = null, bool dontSave = false)
        {
            var entity = await _dbSet.FindAsync(id);

            var result = await DeleteAsync(entity, dontSave: dontSave);

            return result;
        }

        private async Task<TEntity> DeleteAsync(TEntity entity, string loggedInUserName = null, bool dontSave = false)
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
                await SaveChangesAsync(loggedInUserName);
            }

            return entity;
        }

        /// <summary>
        /// Wrapper for SaveChanges adding the Validation Messages to the generated exception
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>http://stackoverflow.com/questions/10219864/ef-code-first-how-do-i-see-entityvalidationerrors-property-from-the-nuget-pac</remarks>
        public int SaveChanges(string loggedInUserName = null)
        {
            try
            {
                var saveChangesInt = _context.SaveChanges(loggedInUserName);

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

        public async Task<int> SaveChangesAsync(string loggedInUserName = null)
        {
            try
            {
                var saveChangesInt = await _context.SaveChangesAsync(loggedInUserName);

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
