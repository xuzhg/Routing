// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System;

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match a $ref segment.
    /// </summary>
    public class RefSegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public static RefSegmentTemplate Instance { get; } = new RefSegmentTemplate();

        /// <summary>
        /// 
        /// </summary>
        private RefSegmentTemplate()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template => "$ref";
    }
}
