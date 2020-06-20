﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System;

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match an <see cref="ODataSegmentTemplate"/>.
    /// </summary>
    public class EntitySetSegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitySet"></param>
        public EntitySetSegmentTemplate(IEdmEntitySet entitySet)
        {
            EntitySet = entitySet ?? throw new ArgumentNullException(nameof(entitySet));
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template => EntitySet.Name;

        /// <summary>
        /// 
        /// </summary>
        public IEdmEntitySet EntitySet { get; }
    }
}
