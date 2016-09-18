using System.Web.Mvc;
using StructureMap;
using System.Collections;
using System.Collections.Generic;

namespace $safeprojectname$
{
    /// <summary>
    /// http://www.amescode.com/category/structuremap
    /// </summary>
    internal class StructureMapValidatableObjectAdapterFactory : ValidatableObjectAdapter
    {
        private readonly IContainer _container;

        public StructureMapValidatableObjectAdapterFactory(ModelMetadata metadata, ControllerContext context, IContainer container)
            : base(metadata, context)
        {
            this._container = container;
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            object model = base.Metadata.Model;

            if (model != null)
            {
                // Ask StructureMap to do setter injection for all properties decorated with SetterProperty attribute.
                _container.BuildUp(model);
            }

            return base.Validate(container);
        }
    }
}