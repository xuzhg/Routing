// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.OData.Edm;

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match an <see cref="ODataSegmentTemplate"/>.
    /// </summary>
    public class CastSegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castType">The actual structured type.</param>
        public CastSegmentTemplate(IEdmStructuredType castType)
        {
            CastType = castType ?? throw new ArgumentNullException(nameof(castType));
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template => CastType.FullTypeName();

        /// <summary>
        /// 
        /// </summary>
        public IEdmStructuredType CastType { get; }
    }
}
