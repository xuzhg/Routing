using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.OData.Routing.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ODataModelAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public ODataModelAttribute(string model)
        {
            Model = model;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Model { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class MetadataApplicationModelConventionAttribute : Attribute, IApplicationModelConvention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public void Apply(ApplicationModel application)
        {
            Console.WriteLine(application.ToString());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MetadataActionModelConvention : IActionModelConvention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void Apply(ActionModel action)
        {
            if (action.Controller.ControllerName == "Metadata")
            {
                if (action.ActionMethod.Name == "GetMetadata")
                {
                    // We go through the list of selectors and add an attribute route to the controller if none is present
                    foreach (var selector in action.Selectors)
                    {
                        if (selector.AttributeRouteModel == null)
                        {
                            // Customers
                            var template = "$metadata";
                            selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(template) { Name = "GetMetadata" });
                        }
                    }

                    // We setup a resource filter that sets up the information in the request.
                    // This can be done in a more "endpoint" routing friendly way, where we just set some medatada on the endpoint.
                    // We don't have to parse the url with IODataPathHandler because routing already parsed it and we can construct an OData path.
                    action.Selectors.Single().EndpointMetadata.Add(new ODataEndpointMetadata(null, (_, __) => new ODataPath(MetadataSegment.Instance)));
                }

                if (action.ActionMethod.Name == "GetServiceDocument")
                {
                    // We go through the list of selectors and add an attribute route to the controller if none is present
                    foreach (var selector in action.Selectors)
                    {
                        if (selector.AttributeRouteModel == null)
                        {
                            // Customers
                            var template = "";
                            selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(template) { Name = "GetServiceDocument" });
                            }
                    }

                    // We setup a resource filter that sets up the information in the request.
                    // This can be done in a more "endpoint" routing friendly way, where we just set some medatada on the endpoint.
                    // We don't have to parse the url with IODataPathHandler because routing already parsed it and we can construct an OData path.
                    action.Selectors.Single().EndpointMetadata.Add(new ODataEndpointMetadata(null, (_, __) => new ODataPath()));
                }
            }
        }
    }
}
