using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoClutch.Core.Interfaces
{
    public interface IODataService<TEntity> : IService<TEntity>, IDisposable
      where TEntity : class, new()
    {
        IQueryable<TEntity> Queryable();
        Task<TEntity> UpdateAsync(string id, TEntity entity, string loggedInUserName);
    }
}