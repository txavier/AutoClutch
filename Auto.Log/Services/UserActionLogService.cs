﻿using AutoClutch.Core;
using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Models;
using AutoClutch.Core.Objects;
using AutoClutch.Repo.Interfaces;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoClutch.Log.Services
{
    public class UserActionLogService : EFService<userActionLog>, ILogService, IEFService<userActionLog>
    {
        public UserActionLogService(IEFRepository<userActionLog> userActionLogRepository)
            : base(userActionLogRepository)
        {

        }

        public void Info(Exception ex)
        {
            ErrorSignal.FromCurrentContext().Raise(ex);
        }

        public userActionLog Info(string typeName, string typeFullName, string recordId, EventType eventType, string message, string entityName, string loggedInUserName)
        {
            loggedInUserName = loggedInUserName?.Split("\\".ToCharArray())?.LastOrDefault();

            userActionLog userActionLog = GetUserActionLog(typeName, typeFullName, recordId, eventType, message, entityName, loggedInUserName);

            var result = Add(userActionLog, "CTMSystemAgent");

            return result;
        }

        public async Task<userActionLog> InfoAsync(string typeName, string typeFullName, string recordId, EventType eventType, string message, string entityName, string loggedInUserName, string toString = null)
        {
            loggedInUserName = loggedInUserName?.Split("\\".ToCharArray())?.LastOrDefault();

            userActionLog userActionLog = GetUserActionLog(typeName, typeFullName, recordId, eventType, message, entityName, loggedInUserName, toString);

            var result = await AddAsync(userActionLog, "CTMSystemAgent");

            return result;
        }

        private static userActionLog GetUserActionLog(string typeName, string typeFullName, string recordId, EventType eventType, string message, string entityName, string loggedInUserName, string toString = null)
        {
            var result = new userActionLog
            {
                body = GetMessage(typeName, recordId, eventType, message, entityName, loggedInUserName, toString),
                date = DateTime.Now,
                typeFullName = typeFullName,
                recordId = recordId,
                eventType = (int)eventType,
                eventTypeDisplay = eventType.ToString()
            };

            return result;
        }

        public virtual IEnumerable<userActionLog> GetLatestUserActionLogs(int take = 10)
        {
            var result = Queryable().OrderByDescending(i => i.userActionLogId).Take(take);

            return result;
        }

        private static string GetMessage(string typeName, string recordId, EventType eventType, string message = null, string entityName = null, string loggedInUserName = null, string toString = null)
        {
            loggedInUserName = loggedInUserName?.Split("\\".ToCharArray())?.LastOrDefault();

            var result = message ??
                    (toString ?? (UppercaseFirst(typeName) + " " + (entityName ?? recordId.ToString()))) + " has been " + LowercaseFirst(eventType.ToString()) + " by " + loggedInUserName + ".";

            return result;
        }

        private static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private static string LowercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToLower(s[0]) + s.Substring(1);
        }

        public async Task<userActionLog> InfoAsync(string message)
        {
            var userActionLog = new userActionLog
            {
                body = message,
                date = DateTime.Now,
            };

            var result = await AddAsync(userActionLog, "CTMSystemAgent");

            return result;
        }

        public userActionLog Info(string message)
        {
            var userActionLog = new userActionLog
            {
                body = message,
                date = DateTime.Now,
            };

            var result = Add(userActionLog, "CTMSystemAgent");

            return result;
        }
    }

    public class UserActionLogService<TEntity> : UserActionLogService, ILogService<TEntity>
        where TEntity : class
    {
        public UserActionLogService(IEFRepository<userActionLog> userActionLogRepository)
            : base(userActionLogRepository)
        {
        }

        public async Task<userActionLog> InfoAsync(TEntity entity, string recordId, EventType eventType, string message = null, string entityName = null, string loggedInUserName = null, bool useToString = false)
        {
            loggedInUserName = loggedInUserName?.Split("\\".ToCharArray())?.LastOrDefault();

            var typeFullName = entity.GetType().FullName;

            var typeName = entity.GetType().Name;

            var toString = useToString && !entity.ToString().Contains(System.Reflection.Assembly.GetAssembly(this.GetType()).GetType().Namespace + ".Core.Models") ? entity.ToString() : null;

            var result = await base.InfoAsync(typeName, typeFullName, recordId, eventType, message, entityName, loggedInUserName, toString);

            return result;
        }

        public userActionLog Info(TEntity entity, string recordId, EventType eventType, string message = null, string entityName = null, string loggedInUserName = null)
        {
            loggedInUserName = loggedInUserName?.Split("\\".ToCharArray())?.LastOrDefault();

            var typeFullName = entity.GetType().FullName;

            var typeName = entity.GetType().Name;

            var result = base.Info(typeName, typeFullName, recordId, eventType, message, entityName, loggedInUserName);

            return result;
        }

        public new async Task<userActionLog> InfoAsync(string message)
        {
            var result = await base.InfoAsync(message);

            return result;
        }

        public new userActionLog Info(string message)
        {
            var result = base.Info(message);

            return result;
        }

    }
}
