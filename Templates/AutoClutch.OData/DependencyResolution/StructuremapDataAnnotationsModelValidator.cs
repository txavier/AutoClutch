using System.ComponentModel.DataAnnotations;
using StructureMap;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace $safeprojectname$
{
    /// <summary>
    /// http://www.amescode.com/category/structuremap
    /// </summary>
    public class StructureMapDataAnnotationsModelValidator : DataAnnotationsModelValidator
    {
        private IContainer _Container;

        public StructureMapDataAnnotationsModelValidator(ModelMetadata metadata, ControllerContext context, ValidationAttribute attribute, IContainer container) : base(metadata, context, attribute)
        {
            _Container = container;
        }


        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            /* Ask StructureMap to do setter injection for all properties decorated with SetterProperty attribute*/
            _Container.BuildUp(Attribute);
            return base.Validate(container);
        }
    }
}