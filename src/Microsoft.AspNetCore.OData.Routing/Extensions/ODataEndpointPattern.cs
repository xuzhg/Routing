﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Globalization;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.OData.Routing
{
    internal static class ODataEndpointPattern
    {
        /// <summary>
        /// Wildcard route template for the OData Endpoint route pattern.
        /// </summary>
        public const string ODataEndpointPath = "ODataEndpointPath_";

        /// <summary>
        /// Wildcard route template for the OData path route variable.
        /// </summary>
        public static readonly string ODataEndpointTemplate = "{{**" + ODataEndpointPath + "{0}}}";

        /// <summary>
        /// Create an OData Endpoint route pattern.
        /// The route pattern is in this format: "routePrefix/{*ODataEndpointPath_routeName}"
        /// </summary>
        /// <param name="routeName">The route name. It can not be null and verify upper layer.</param>
        /// <param name="routePrefix">The route prefix. It could be null or empty</param>
        /// <returns>The OData route endpoint pattern.</returns>
        public static string CreateODataEndpointPattern(string routeName, string routePrefix)
        {
            if (routeName == null)
            {
                throw new ArgumentNullException(nameof(routeName));
            }

            return string.IsNullOrEmpty(routePrefix) ?
                string.Format(CultureInfo.InvariantCulture, ODataEndpointTemplate, routeName) :
                routePrefix + "/" + string.Format(CultureInfo.InvariantCulture, ODataEndpointTemplate, routeName);
        }

        /// <summary>
        /// Get the OData route name and path value.
        /// </summary>
        /// <param name="values">The dictionary contains route value.</param>
        /// <returns>A tuple contains the route name and path value.</returns>
        public static (string, object) GetODataRouteInfo(this RouteValueDictionary values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            string routeName = null;
            object odataPathValue = null;
            foreach (var item in values)
            {
                string keyString = item.Key;

                if (keyString.StartsWith(ODataEndpointPath, StringComparison.Ordinal))
                {
                    routeName = keyString.Substring(ODataEndpointPath.Length);
                    odataPathValue = item.Value;
                    break;
                }
            }

            return (routeName, odataPathValue);
        }
    }
}