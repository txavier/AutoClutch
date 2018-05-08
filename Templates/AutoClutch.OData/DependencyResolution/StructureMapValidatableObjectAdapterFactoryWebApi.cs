using StructureMap;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Validators;
using System.Web.Http.Metadata;

namespace $safeprojectname$
{
    /// <summary>
    /// http://www.amescode.com/category/structuremap
    /// </summary>
    public class StructureMapValidatableObjectAdapterFactoryWebApi : ValidatableObjectAdapter
    {
        private IContainer _container;

        public StructureMapValidatableObjectAdapterFactoryWebApi(IEnumerable<ModelValidatorProvider> validatorProviders, IContainer container)
            : base(validatorProviders)
        {
            this._container = container;
        }

        public override IEnumerable<ModelValidationResult> Validate(ModelMetadata metadata, object container)
        {
            object model = metadata.Model;

            if (model != null)
            {
                // Ask StructureMap to do setter injection for all properties decorated with SetterProperty attribute.
                _container.BuildUp(model);
            }

            return base.Validate(metadata, container);
        }

    }
}