using AutoClutch.Repo.Interfaces;
using AutoClutch.Core;
using $safeprojectname$.Interfaces;
using $safeprojectname$.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerEnabledDbContext.Common.Models;

namespace $safeprojectname$.Services
{
    public class TrackerEnabledService : Service<AuditLog>, ITrackerEnabledService
    {
        public TrackerEnabledService(IRepository<AuditLog> auditLogRepository)
            : base(auditLogRepository)
        {
        }

        public int GetCountAll()
        {
            var result = base.Queryable().Count();

            return result;
        }

        public IEnumerable<BaseHistoryVM> Get(int? page, int? perPage)
        {
            var skip = (page - 1) * perPage;

            var result = base.Queryable().OrderByDescending(i => i.EventDateUTC).Skip(skip ?? 0).Take(perPage ?? 100);

            var resultAsHistoryViewModel = ConvertToHistoryViewModel(result);

            return resultAsHistoryViewModel;
        }

        public IEnumerable<BaseHistoryVM> GetAll()
        {
            var result = ConvertToHistoryViewModel(base.GetAll().OrderByDescending(i => i.EventDateUTC));

            return result;
        }

        public IEnumerable<BaseHistoryVM> Get(string typeFullName, int id)
        {
            var auditLogs = base.Get(filter: i => i.TypeFullName == typeFullName && i.RecordId == id.ToString()).OrderByDescending(i => i.EventDateUTC);

            var histories = ConvertToHistoryViewModel(auditLogs);

            return histories;
        }

        private static List<BaseHistoryVM> ConvertToHistoryViewModel(IEnumerable<AuditLog> data)
        {
            var vm = new List<BaseHistoryVM>();

            foreach (var log in data)
            {
                switch (log.EventType)
                {
                    case EventType.Added: // added
                        vm.Add(new AddedHistoryVM
                        {
                            Date = log.EventDateUTC.ToLocalTime(),
                            LogId = log.AuditLogId,
                            RecordId = log.RecordId,
                            TypeFullName = log.TypeFullName,
                            UserName = log.UserName,
                            Details = log.LogDetails.Select(x => new LogDetail { PropertyName = x.PropertyName, NewValue = x.NewValue }),
                            EventType = log.EventType.ToString()
                        });
                        break;

                    case EventType.Deleted: //deleted
                        vm.Add(new DeletedHistoryVM
                        {
                            Date = log.EventDateUTC.ToLocalTime(),
                            LogId = log.AuditLogId,
                            RecordId = log.RecordId,
                            TypeFullName = log.TypeFullName,
                            UserName = log.UserName,
                            Details = log.LogDetails.Select(x => new LogDetail { PropertyName = x.PropertyName, OldValue = x.OriginalValue }),
                            EventType = log.EventType.ToString()
                        });
                        break;

                    case EventType.Modified: //modified
                        vm.Add(new ChangedHistoryVM
                        {
                            Details = log.LogDetails.Select(x => new LogDetail { PropertyName = x.PropertyName, NewValue = x.NewValue, OldValue = x.OriginalValue }),
                            Date = log.EventDateUTC.ToLocalTime(),
                            LogId = log.AuditLogId,
                            RecordId = log.RecordId,
                            TypeFullName = log.TypeFullName,
                            UserName = log.UserName,
                            EventType = log.EventType.ToString()
                        });
                        break;

                    case EventType.SoftDeleted: // Soft deleted.
                        vm.Add(new ChangedHistoryVM
                        {
                            Details = log.LogDetails.Select(x => new LogDetail { PropertyName = x.PropertyName, NewValue = x.NewValue, OldValue = x.OriginalValue }),
                            Date = log.EventDateUTC.ToLocalTime(),
                            LogId = log.AuditLogId,
                            RecordId = log.RecordId,
                            TypeFullName = log.TypeFullName,
                            UserName = log.UserName,
                            EventType = log.EventType.ToString()
                        });
                        break;

                    case EventType.UnDeleted: // undeleted.
                        vm.Add(new ChangedHistoryVM
                        {
                            Details = log.LogDetails.Select(x => new LogDetail { PropertyName = x.PropertyName, NewValue = x.NewValue, OldValue = x.OriginalValue }),
                            Date = log.EventDateUTC.ToLocalTime(),
                            LogId = log.AuditLogId,
                            RecordId = log.RecordId,
                            TypeFullName = log.TypeFullName,
                            UserName = log.UserName,
                            EventType = log.EventType.ToString()
                        });
                        break;
                }

            }

            return vm;
        }
    }
}
