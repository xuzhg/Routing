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
    public class FunctionSegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="function">The type containes the key.</param>
        public FunctionSegmentTemplate(IEdmFunction function)
            : this(function, unqualifiedFunctionCall: false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function">The type containes the key.</param>
        /// <param name="unqualifiedFunctionCall">Unqualified function call boolean value.</param>
        public FunctionSegmentTemplate(IEdmFunction function, bool unqualifiedFunctionCall)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));

            if (!function.IsBound)
            {
                // TODO: shall we need this check?
                throw new InvalidOperationException($"The input function {function.Name} is not a bound function.");
            }

            int skip = function.IsBound ? 1 : 0;

            IDictionary<string, string> parametersMappings = new Dictionary<string, string>();
            foreach (var parameter in function.Parameters.Skip(skip))
            {
                parametersMappings[parameter.Name] = $"{{{parameter.Name}}}";
            }

            if (unqualifiedFunctionCall)
            {
                Template = function.Name + "(" + string.Join(",", parametersMappings.Select(a => $"{a.Key}={a.Value}")) + ")";
            }
            else
            {
                Template = function.FullName() + "(" + string.Join(",", parametersMappings.Select(a => $"{a.Key}={a.Value}")) + ")";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEdmFunction Function { get; }
    }
}
