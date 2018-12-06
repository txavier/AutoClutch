using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoClutch.Repo.Interfaces
{
    public interface IODataRepository<TEntity> : IDisposable
       where TEntity : class
    {
        IEnumerable<Error> Errors { get; set; }
        HttpClient currentHttpClient { get; set; }
        IQueryable<TEntity> Queryable();
        void SetUri(string uriString, int format = 1);
        Task<TEntity> AddAsync(TEntity entity, string loggedInUserName = null);
        Task<TEntity> UpdateAsync(string id, TEntity entity, string loggedInUserName = null);
        Task<string> DeleteAsync(string id, string loggedInUserName = null);
        TEntity Add(TEntity entity, string loggedInUserName = null);
        TEntity Update(string id, TEntity entity, string loggedInUserName = null);
        string Delete(string id, string loggedInUserName = null);
        TEntity Find(string entityId);
        Task<TEntity> FindAsync(string entityId);
    }
}