using StructureMap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace $safeprojectname$
{
    /// <summary>
    /// http://www.amescode.com/category/structuremap
    /// </summary>
    public class StructureMapValidatableObjectAdapterFactory : ValidatableObjectAdapter
    {
        private IContainer _container;
        private object _model;

        public StructureMapValidatableObjectAdapterFactory(ModelMetadata metadata, ControllerContext context, IContainer container)
            : base(metadata, context)
        {
            this._container = container;

            this._model = base.Metadata.Model;
        }

        //public override IEnumerable<ModelValidationResult> Validate(object container)
        //{
        //    object model = base.Metadata.Model;

        //    if(model != null)
        //    {
        //        // Ask StructureMap to do setter injection for all properties decorated with SetterProperty attribute.
        //        _container.BuildUp(model);
        //    }

        //    return base.Validate(container);
        //}

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            object model = base.Metadata.Model;
            if (model != null)
            {
                IValidatableObject instance = model as IValidatableObject;
                if (instance == null)
                {
                    //the base implementation will throw an exception after 
                    //doing the same check - so let's retain that behaviour
                    return base.Validate(container);
                }
                /* replacement for the core functionality */
                ValidationContext validationContext = CreateValidationContext(instance);
                return this.ConvertResults(instance.Validate(validationContext));
            }
            else
                return base.Validate(container);  /*base returns an empty set 
                                          of values for null. */
        }

        /// <summary>
        /// Called by the Validate method to create the ValidationContext
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        protected virtual ValidationContext CreateValidationContext(object instance)
        {
            IServiceProvider serviceProvider = CreateServiceProvider(instance);
            //TODO: add virtual method perhaps for the third parameter?
            ValidationContext context = new ValidationContext(
              instance ?? Metadata.Model,
              serviceProvider,
              null);
            return context;
        }

        /// <summary>
        /// Called by the CreateValidationContext method to create an IServiceProvider
        /// instance to be passed to the ValidationContext.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        protected virtual IServiceProvider CreateServiceProvider(object container)
        {
            _container.BuildUp(_model);

            return (IServiceProvider)_container;

            //IServiceProvider serviceProvider = null;

            //IDependant dependantController = ControllerContext.Controller as IDependant;

            //if (dependantController != null && dependantController.Resolver != null)
            //{
            //    serviceProvider =
            //      new ResolverServiceProviderWrapper(dependantController.Resolver);
            //}
            //else
            //    serviceProvider = ControllerContext.Controller as IServiceProvider;

            //return serviceProvider;
        }

        //ripped from v3 RTM source
        private IEnumerable<ModelValidationResult> ConvertResults(
          IEnumerable<ValidationResult> results)
        {
            foreach (ValidationResult result in results)
            {
                if (result != ValidationResult.Success)
                {
                    if (result.MemberNames == null || !result.MemberNames.Any())
                    {
                        yield return new ModelValidationResult { Message = result.ErrorMessage };
                    }
                    else
                    {
                        foreach (string memberName in result.MemberNames)
                        {
                            yield return new ModelValidationResult
                            { Message = result.ErrorMessage, MemberName = memberName };
                        }
                    }
                }
            }
        }
    }
}