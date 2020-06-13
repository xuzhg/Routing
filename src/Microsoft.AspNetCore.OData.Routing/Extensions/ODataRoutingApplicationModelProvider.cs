using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
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
            var models = _options.Value.Models;

            // for all conventions, 
            foreach (var model in models)
            {
                foreach (var controller in context.Result.Controllers)
                {
                    if (CanApply(model, controller))
                    {
                        foreach (var action in controller.Actions)
                        {
                            foreach (var convention in conventions)
                            {
                                if (convention.Apply(model.Key, model.Value, action))
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("OnProvidersExecuted");
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            Console.WriteLine("OnProvidersExecuting");
        }

        private static bool CanApply(KeyValuePair<string, IEdmModel> model, ControllerModel controller)
        {
            ODataModelAttribute odataModel = GetAttribute<ODataModelAttribute>(controller);
            if (odataModel == null)
            {
                return true; // apply to all model
            }
            else if (model.Key == odataModel.Model)
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
