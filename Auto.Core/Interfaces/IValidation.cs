using System.Collections.Generic;
using AutoClutch.Core.Objects;
using AutoClutch.Core.Interfaces;

namespace AutoClutch.Core
{
    public interface IValidation<TEntity> where TEntity : class, new()
    {
        IEnumerable<Error> Errors { get; set; }
        bool IsValid(TEntity entity, string loggedInUserName = null, IEFService<TEntity> service = null);
    }
}