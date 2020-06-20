// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System;

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match an <see cref="ODataSegmentTemplate"/>.
    /// </summary>
    public class MetadataSegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public static MetadataSegmentTemplate Instance { get; } = new MetadataSegmentTemplate();

        /// <summary>
        /// 
        /// </summary>
        private MetadataSegmentTemplate()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template => "$metadata";
    }
}
