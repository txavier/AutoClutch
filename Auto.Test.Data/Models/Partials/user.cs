using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.Test.Data
{
    public partial class user : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.userId == -1)
            {
                yield return new ValidationResult("The user Id cannot be -1.", new[] { "userId" });
            }
        }
    }
}
