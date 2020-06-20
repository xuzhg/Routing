using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.AspNetCore.Routing;
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
    internal class ODataEndpointMetadata
    {
        public ODataEndpointMetadata(string prefix, IEdmModel model, ODataPathTemplate template)
        {
            Prefix = prefix;
            Model = model;
            Template = template;
        }

        public ODataEndpointMetadata(IEdmModel model) { }
        /*
        public ODataEndpointMetadata(IDictionary<IEdmNamedElement, string> parameterMappings, Func<RouteValueDictionary, IDictionary<IEdmNamedElement, string>, ODataPath> odataPathFactory)
        {
            ParameterMappings = parameterMappings;
            ODataPathFactory = odataPathFactory;
        }

        public IDictionary<IEdmNamedElement, string> ParameterMappings { get; }


        public Func<RouteValueDictionary, IDictionary<IEdmNamedElement, string>, ODataPath> ODataPathFactory { get; }
        */
        public ODataEndpointMetadata(IDictionary<string, string> parameterMappings,
            Func<RouteValueDictionary, IDictionary<string, string>, ODataPath> odataPathFactory)
        {
            ParameterMappings = parameterMappings;
            ODataPathFactory = odataPathFactory;
        }

        public IDictionary<string, string> ParameterMappings { get; }
        public Func<RouteValueDictionary, IDictionary<string, string>, ODataPath> ODataPathFactory { get; }

        public string Prefix { get; }

        public IEdmModel Model { get; }

        public ODataPathTemplate Template { get; }
    }
}
