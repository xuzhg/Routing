// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System;

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match an <see cref="ODataSegmentTemplate"/>.
    /// </summary>
    public class SingletonSegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleton"></param>
        public SingletonSegmentTemplate(IEdmSingleton singleton)
        {
            Singleton = singleton ?? throw new ArgumentNullException(nameof(singleton));
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template => Singleton.Name;

        /// <summary>
        /// 
        /// </summary>
        public IEdmSingleton Singleton { get; }
    }
}
