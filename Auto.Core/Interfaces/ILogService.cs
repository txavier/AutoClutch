using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Models;
using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoClutch.Core.Interfaces
{
    public interface ILogService
    {
        void Info(Exception ex);

        IEnumerable<userActionLog> GetLatestUserActionLogs(int take = 10);

        Task<userActionLog> InfoAsync(string typeName, string typeFullName, string recordId, EventType eventType, string message, string entityName, string loggedInUserName, string toString = null);

        userActionLog Info(string typeName, string typeFullName, string recordId, EventType eventType, string message, string entityName, string loggedInUserName);
    }

    public interface ILogService<TEntity> : ILogService
    {
        new IEnumerable<userActionLog> GetLatestUserActionLogs(int take = 10);

        Task<userActionLog> InfoAsync(TEntity entity, string recordId, EventType eventType, string message = null, string entityName = null, string loggedInUserName = null, bool useToString = false);

        userActionLog Info(TEntity entity, string recordId, EventType eventType, string message = null, string entityName = null, string loggedInUserName = null);

        Task<userActionLog> InfoAsync(string message);

        userActionLog Info(string message);
    }
}
