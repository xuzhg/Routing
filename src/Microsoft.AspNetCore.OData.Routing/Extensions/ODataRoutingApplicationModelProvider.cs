using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.OData.Routing.Extensions
{
    internal class ODataRoutingApplicationModelProvider : IApplicationModelProvider
    {
        //private readonly IEdmModel _model;

        private readonly IOptions<ODataRoutingOptions> _options;

        public ODataRoutingApplicationModelProvider(
            IOptions<ODataRoutingOptions> options)
        {
            _options = options;
        }

        public int Order => 100;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            var conventions = _options.Value.Conventions.OrderBy(c => c.Order);
            var routes = _options.Value.Models;

            // Can apply on controller
            // for all conventions, 
            foreach (var route in routes)
            {
                IEdmModel model = route.Value;
                if (model == null || model.EntityContainer == null)
                {
                    continue;
                }

                foreach (var controller in context.Result.Controllers)
                {
                    // apply to ODataModelAttribute
                    if (!CanApply(route.Key, controller))
                    {
                        continue;
                    }

                    // Add here
                    //

                    // Get conventions for all this controller

                    foreach (var convention in conventions)
                    {
                        if (convention.AppliesToController(route.Key, route.Value, controller))
                        {
                            foreach (var action in controller.Actions)
                            {
                                if (convention.AppliesToAction(route.Key, route.Value, action))
                                {
                                    ;
                                }
                            }
                        }
                    }
                }
            }

            // for all conventions, 
            //foreach (var model in models)
            //{
            //    foreach (var controller in context.Result.Controllers)
            //    {
            //        if (CanApply(model.Key, controller))
            //        {
            //            foreach (var action in controller.Actions)
            //            {
            //                foreach (var convention in conventions)
            //                {
            //                    if (convention.Apply(model.Key, model.Value, action))
            //                    {
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            Console.WriteLine("OnProvidersExecuted");
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            Console.WriteLine("OnProvidersExecuting");
        }

        private static bool CanApply(string prefix, ControllerModel controller)
        {
            ODataModelAttribute odataModel = GetAttribute<ODataModelAttribute>(controller);
            if (odataModel == null)
            {
                return true; // apply to all model
            }
            else if (prefix == odataModel.Model)
            {
                return true;
            }

            return false;
        }

        private static bool CanApply(IEdmModel model, ControllerModel controllerModel)
        {
            if (model == null || model.EntityContainer == null)
            {
                return false;
            }

            string controllerName = controllerModel.ControllerName;

            if (controllerName == "ODataOperationImport")
            {
                // Convention for the actionimport/function import
                return true;
            }

            IEdmEntitySet entitySet = model.EntityContainer.FindEntitySet(controllerName);
            if (entitySet != null)
            {
                return true;
            }

            IEdmSingleton singleton = model.EntityContainer.FindSingleton(controllerName);
            if (singleton != null)
            {
                return true;
            }

            return false;
        }

        public static T GetAttribute<T>(ControllerModel controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            T value = controller.Attributes.OfType<T>().FirstOrDefault();
            return value;
        }
    }
}
