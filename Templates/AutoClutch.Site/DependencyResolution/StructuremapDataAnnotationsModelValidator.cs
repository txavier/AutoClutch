using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using StructureMap;
using System.Collections;
using System.Collections.Generic;

namespace $safeprojectname$
{
    /// <summary>
    /// http://www.amescode.com/category/structuremap
    /// </summary>
    public class StructuremapDataAnnotationsModelValidator : DataAnnotationsModelValidator
    {
        private readonly IContainer _container;

        public StructuremapDataAnnotationsModelValidator(ModelMetadata metadata, ControllerContext context, ValidationAttribute attribute, IContainer container)
            : base(metadata, context, attribute)
        {
            this._container = container;
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            // Ask StrucutreMap to do setter injection for all properties decorated with SetterProperty attribute.

            _container.BuildUp(Attribute);

            return base.Validate(container);
        }
    }
}