// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.OData.Edm;

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match an <see cref="ODataSegmentTemplate"/>.
    /// </summary>
    public class NavigationSegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigation">The .</param>
        public NavigationSegmentTemplate(IEdmNavigationProperty navigation)
        {
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template => Navigation.Name;

        /// <summary>
        /// 
        /// </summary>
        public IEdmNavigationProperty Navigation { get; }
    }
}
