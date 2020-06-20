﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Routing.Extensions;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.OData.Routing.Conventions
{
    /// <summary>
    /// 
    /// </summary>
    public class MetadataRoutingConvention : IODataControllerActionConvention
    {
        private static TypeInfo metadataTypeInfo = typeof(MetadataController).GetTypeInfo();

        /// <summary>
        /// 
        /// </summary>
        public int Order => -1000;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public bool AppliesToController(string prefix, IEdmModel model, ControllerModel controller)
        {
            return controller?.ControllerType == metadataTypeInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <param name="action"></param>
        public bool AppliesToAction(string prefix, IEdmModel model, ActionModel action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (action.Controller.ControllerType != typeof(MetadataController).GetTypeInfo())
            {
                return false;
            }

            if (action.ActionMethod.Name == "GetMetadata")
            {
                // We go through the list of selectors and add an attribute route to the controller if none is present
                foreach (var selector in action.Selectors)
                {
                    if (selector.AttributeRouteModel == null)
                    {
                        // Customers
                        var template = string.IsNullOrEmpty(prefix) ? "$metadata" : $"{prefix}/$metadata";
                        selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(template) { Name = "GetMetadata" });
                    }
                }

                // We setup a resource filter that sets up the information in the request.
                // This can be done in a more "endpoint" routing friendly way, where we just set some medatada on the endpoint.
                // We don't have to parse the url with IODataPathHandler because routing already parsed it and we can construct an OData path.
                action.Selectors.Single().EndpointMetadata.Add(new ODataEndpointMetadata(null, (_, __) => new ODataPath(MetadataSegment.Instance)));

                return true;
            }

            if (action.ActionMethod.Name == "GetServiceDocument")
            {
                // We go through the list of selectors and add an attribute route to the controller if none is present
                foreach (var selector in action.Selectors)
                {
                    if (selector.AttributeRouteModel == null)
                    {
                        var template = string.IsNullOrEmpty(prefix) ? "" : $"{prefix}/";
                        selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(template) { Name = "GetServiceDocument" });
                    }
                }

                // We setup a resource filter that sets up the information in the request.
                // This can be done in a more "endpoint" routing friendly way, where we just set some medatada on the endpoint.
                // We don't have to parse the url with IODataPathHandler because routing already parsed it and we can construct an OData path.
                action.Selectors.Single().EndpointMetadata.Add(new ODataEndpointMetadata(null, (_, __) => new ODataPath()));

                return true;
            }

            return false;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class MetadataRoutingConventionProvider : IODataActionConvention
    {
        private static TypeInfo metadataTypeInfo = typeof(MetadataController).GetTypeInfo();

        /// <summary>
        /// 
        /// </summary>
        public int Order => -1000;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public bool CanApply(ControllerModel controller)
        {
            if (controller.ControllerType == metadataTypeInfo)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void Apply(ActionModel action)
        {
            if (action.Controller.ControllerType != typeof(MetadataController).GetTypeInfo())
            {
                return;
            }

            Console.WriteLine(action.Controller.ControllerName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <param name="action"></param>
        public bool Apply(string prefix, IEdmModel model, ActionModel action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (action.Controller.ControllerType != typeof(MetadataController).GetTypeInfo())
            {
                return false;
            }

            if (action.ActionMethod.Name == "GetMetadata")
            {
                // We go through the list of selectors and add an attribute route to the controller if none is present
                foreach (var selector in action.Selectors)
                {
                    if (selector.AttributeRouteModel == null)
                    {
                        // Customers
                        var template = string.IsNullOrEmpty(prefix) ? "$metadata" : $"{prefix}/$metadata";
                        selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(template) { Name = "GetMetadata" });
                    }
                }

                // We setup a resource filter that sets up the information in the request.
                // This can be done in a more "endpoint" routing friendly way, where we just set some medatada on the endpoint.
                // We don't have to parse the url with IODataPathHandler because routing already parsed it and we can construct an OData path.
                action.Selectors.Single().EndpointMetadata.Add(new ODataEndpointMetadata(null, (_, __) => new ODataPath(MetadataSegment.Instance)));

                return true;
            }

            if (action.ActionMethod.Name == "GetServiceDocument")
            {
                // We go through the list of selectors and add an attribute route to the controller if none is present
                foreach (var selector in action.Selectors)
                {
                    if (selector.AttributeRouteModel == null)
                    {
                        var template = string.IsNullOrEmpty(prefix) ? "" : $"{prefix}/";
                        selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(template) { Name = "GetServiceDocument" });
                    }
                }

                // We setup a resource filter that sets up the information in the request.
                // This can be done in a more "endpoint" routing friendly way, where we just set some medatada on the endpoint.
                // We don't have to parse the url with IODataPathHandler because routing already parsed it and we can construct an OData path.
                action.Selectors.Single().EndpointMetadata.Add(new ODataEndpointMetadata(null, (_, __) => new ODataPath()));

                return true;
            }

            return false;
        }
    }
}