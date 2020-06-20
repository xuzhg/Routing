// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match a $ref segment.
    /// </summary>
    public class ValueSegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public static ValueSegmentTemplate Instance { get; } = new ValueSegmentTemplate();

        /// <summary>
        /// 
        /// </summary>
        private ValueSegmentTemplate()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template => "$value";
    }
}
