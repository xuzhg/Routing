using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.OData.Routing.Extensions
{
    internal class ODataApplicationModelProvider : IApplicationModelProvider
    {
        private readonly IEdmModel _model;

        public ODataApplicationModelProvider(IEdmModel model) => _model = model;

        public int Order => 1000 + 100;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            // throw new NotImplementedException();
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            //foreach (var entitySet in _model.EntityContainer.EntitySets())
            //{
            //    ControllerModel controller = context.Result.Controllers.FirstOrDefault(c => string.Equals(c.ControllerType.Name, $"{entitySet.Name}Controller", StringComparison.OrdinalIgnoreCase));
            //    if (controller == null)
            //    {
            //        continue;
            //    }

            //    Console.WriteLine(controller.ControllerName);

            //    SelectorModel selectorModel = new SelectorModel();
            //    var template = entitySet.Name + "{{**odatapath}}";
            //    selectorModel.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(template) { Name = entitySet.Name });
            //    controller.Selectors.Add(selectorModel);

            //    selectorModel.EndpointMetadata.Add(new ODataEndpointMetadata(null, null));
            //    // Look at the actions in the controller and continue the "matching" process
            //    //foreach (var action in controller.Actions)
            //    //{
            //    //}

            //}

            //    // throw new NotImplementedException();
            //    foreach (var controller in context.Result.Controllers)
            //{
            //    Console.WriteLine(controller.ControllerName);

            //    // Look at the actions in the controller and continue the "matching" process
            //    foreach (var action in controller.Actions)
            //    {
            //    }
            //}
        }
    }
}
