// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System;

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match a $count segment.
    /// </summary>
    public class CountSegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public static CountSegmentTemplate Instance { get; } = new CountSegmentTemplate();

        /// <summary>
        /// 
        /// </summary>
        private CountSegmentTemplate()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template => "$count";
    }
}
