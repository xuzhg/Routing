﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match an <see cref="ODataSegmentTemplate"/>.
    /// </summary>
    public class FunctionImportSegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionImport">.</param>
        public FunctionImportSegmentTemplate(IEdmFunctionImport functionImport)
        {
            FunctionImport = functionImport ?? throw new ArgumentNullException(nameof(functionImport));

            IDictionary<string, string> keyMappings = new Dictionary<string, string>();
            foreach (var parameter in functionImport.Function.Parameters)
            {
                keyMappings[parameter.Name] = $"{{{parameter.Name}}}";
            }

            Template = functionImport.Name + "(" + string.Join(",", keyMappings.Select(a => $"{a.Key}={a.Value}")) + ")";
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEdmFunctionImport FunctionImport { get; }

        /// <inheritdoc />
        public override ODataPathSegment GenerateODataSegment(IEdmModel model,
            IEdmNavigationSource previous, RouteValueDictionary routeValue, QueryString queryString)
        {
            // TODO: process the parameter alias
            IList<OperationSegmentParameter> parameters = new List<OperationSegmentParameter>();
            foreach (var parameter in FunctionImport.Function.Parameters)
            {
                if (routeValue.TryGetValue(parameter.Name, out object rawValue))
                {
                    // for resource or collection resource, this method will return "ODataResourceValue, ..." we should support it.
                    if (parameter.Type.IsResourceOrCollectionResource())
                    {
                        // For FromODataUri
                        string prefixName = ODataParameterValue.ParameterValuePrefix + parameter.Name;
                        routeValue[prefixName] = new ODataParameterValue(rawValue, parameter.Type);

                        parameters.Add(new OperationSegmentParameter(parameter.Name, rawValue));
                    }
                    else
                    {
                        string strValue = rawValue as string;
                        object newValue = ODataUriUtils.ConvertFromUriLiteral(strValue, ODataVersion.V4, model, parameter.Type);

                        // for without FromODataUri, so update it, for example, remove the single quote for string value.
                        routeValue[parameter.Name] = newValue;

                        // For FromODataUri
                        string prefixName = ODataParameterValue.ParameterValuePrefix + parameter.Name;
                        routeValue[prefixName] = new ODataParameterValue(newValue, parameter.Type);

                        parameters.Add(new OperationSegmentParameter(parameter.Name, newValue));
                    }
                }
            }

            IEdmNavigationSource targetset = null; // todo

            return new OperationImportSegment(FunctionImport, targetset as IEdmEntitySetBase, parameters);
        }
    }
}
