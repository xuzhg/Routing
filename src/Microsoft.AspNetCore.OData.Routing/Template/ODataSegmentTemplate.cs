// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

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
    }
}
