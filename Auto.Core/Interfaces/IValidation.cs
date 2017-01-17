using System.Collections.Generic;
using AutoClutch.Core.Objects;

namespace AutoClutch.Core
{
    public interface IValidation<TEntity> where TEntity : class, new()
    {
        IEnumerable<Error> Errors { get; set; }

        bool IsValid();
    }
}