using System.ComponentModel.DataAnnotations;
using StructureMap;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http.Validation.Validators;
using System.Web.Http.Validation;
using System.Web.Http.Metadata;

namespace $safeprojectname$
{
    /// <summary>
    /// http://www.amescode.com/category/structuremap
    /// </summary>
    public class StructuremapDataAnnotationsModelValidatorWebApi : DataAnnotationsModelValidator
    {
        private IContainer _Container;

        public StructuremapDataAnnotationsModelValidatorWebApi(IEnumerable<ModelValidatorProvider> validatorProviders, ValidationAttribute attribute, IContainer container)
            : base(validatorProviders, attribute)
        {
            _Container = container;
        }

        public override IEnumerable<ModelValidationResult> Validate(ModelMetadata metadata, object container)
        {
            /* Ask StructureMap to do setter injection for all properties decorated with SetterProperty attribute*/
            _Container.BuildUp(Attribute);

            return base.Validate(metadata, container);
        }

    }
}