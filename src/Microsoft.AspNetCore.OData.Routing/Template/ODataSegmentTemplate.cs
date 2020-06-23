﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match an <see cref="ODataSegmentTemplate"/>.
    /// </summary>
    public abstract class ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract string Template { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="previous"></param>
        /// <param name="routeValue"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public abstract ODataPathSegment GenerateODataSegment(IEdmModel model,
            IEdmNavigationSource previous, RouteValueDictionary routeValue, QueryString queryString);
    }
}
