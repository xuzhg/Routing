using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.OData.Routing.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    internal class ODataEndpointRoutingMatcherPolicy : MatcherPolicy, IEndpointSelectorPolicy
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Order => 1000 - 102;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoints"></param>
        /// <returns></returns>
        public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="candidates"></param>
        /// <returns></returns>
        public Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
        {
            // The goal of this method is to perform the final matching:
            // Map between route values matched by the template and the ones we want to expose to the action for binding. 
            // (tweaking the route values is fine here)
            // Invalidating the candidate if the key/function values are not valid/missing.
            // Perform overload resolution for functions by looking at the candidates and their metadata.
            for (var i = 0; i < candidates.Count; i++)
            {
                ref var candidate = ref candidates[i];
                if (!candidates.IsValidCandidate(i))
                {
                    continue;
                }

                var oDataMetadata = candidate.Endpoint.Metadata.OfType<ODataEndpointMetadata>().FirstOrDefault();
                if (oDataMetadata == null)
                {
                    continue;
                }

                var original = candidate.Endpoint.RequestDelegate;
                var name = candidate.Endpoint.DisplayName;

                var newEndpoint = new Endpoint(EndpointWithODataPath, candidate.Endpoint.Metadata, name);
                var originalValues = candidate.Values;
                var newValues = new RouteValueDictionary();
                foreach (var (key, value) in originalValues)
                {
                    //if (key.EndsWith(".Name"))
                    //{
                    //    var keyValue = originalValues[key.Replace(".Name", ".Value")];
                    //    var partName = originalValues[key];
                    //    var parameterName = oDataMetadata.ParameterMappings[oDataMetadata.ParameterMappings.Keys.Single(key => key.Name == (string)partName)];
                    //    newValues.Add(parameterName, keyValue);
                    //}

                    newValues.Add(key, value);
                }

                var oPath = oDataMetadata.GenerateODataPath(originalValues, httpContext.Request.QueryString);
                if (oPath != null)
                {
                    var odata = httpContext.Request.ODataFeature();
                    odata.Model = oDataMetadata.Model;
                    odata.IsEndpointRouting = true;
                    odata.RequestContainer = httpContext.RequestServices; // sp;
                    odata.Path = oPath;

                    //candidates.SetValidity(i, true); // Double confirm whether it's required or not?
                    continue;
                }
                else
                {
                    candidates.SetValidity(i, false);
                    continue;
                }

                //candidates.ReplaceEndpoint(i, newEndpoint, newValues);

                Task EndpointWithODataPath(HttpContext httpContext)
                {
                    var odataPath = oDataMetadata.ODataPathFactory(httpContext.GetRouteData().Values, oDataMetadata.ParameterMappings);
                    var odata = httpContext.Request.ODataFeature();
                    odata.IsEndpointRouting = true;
                    odata.RequestContainer = httpContext.RequestServices;
                    odata.Path = odataPath;
                    odata.RouteName = name;
                    var prc = httpContext.RequestServices.GetRequiredService<IPerRouteContainer>();
                    if (!prc.HasODataRootContainer(name))
                    {
                        prc.AddRoute(odata.RouteName, "");
                    }

                    return original(httpContext);
                }
            }

            return Task.CompletedTask;
        }
    }
}
