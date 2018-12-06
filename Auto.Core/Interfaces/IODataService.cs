using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoClutch.Core.Interfaces
{
    public interface IODataService<TEntity> : IDisposable
      where TEntity : class, new()
    {
        IEnumerable<Error> Errors { get; set; }
        void Dispose();
        IQueryable<TEntity> Queryable();
        Task<TEntity> AddAsync(TEntity entity, string loggedInUserName);
        Task<TEntity> UpdateAsync(string id, TEntity entity, string loggedInUserName);
    }
}