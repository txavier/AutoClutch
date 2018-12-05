using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrackerEnabledDbContext.Common.Models;

namespace AutoClutch.Repo.Interfaces
{
    public interface IRepository<TEntity> : IDisposable
     where TEntity : class
    {
        IEnumerable<Error> Errors { get; set; }

        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null);
        void Delete(object id, string loggedInUserName = null);
        Task<bool> DeleteAsync(object id, string loggedInUserName = null);
        IQueryable<TEntity> Queryable();
        TEntity Update(object id, TEntity entity, string loggedInUserName = null);
        Task<TEntity> UpdateAsync(object id, TEntity entity, string loggedInUserName = null);
    }
}