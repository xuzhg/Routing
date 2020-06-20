// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Edm;

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
    }
}
