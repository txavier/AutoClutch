using System.Collections.Generic;
using AutoClutch.Core.Objects;

namespace AutoClutch.Core
{
    public interface IValidation<TEntity> where TEntity : class
    {
        IEnumerable<Error> Errors { get; set; }

        bool IsValid(TEntity entity);
    }
}