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
    public class KeySegmentTemplate : ODataSegmentTemplate
    {
        /// <summary>
        /// Key/Value pairs:
        /// Key: entity type key name, for example ID
        /// Value: a tuple of (string, IEdmTypeReference): Item1 is the mapped name, Item2 is the key's type
        /// </summary>
        private IDictionary<string, (string, IEdmTypeReference)> _keyMappings { get; } = new Dictionary<string, (string, IEdmTypeReference)>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        public KeySegmentTemplate(IEdmEntityType entityType)
            : this(entityType, keyPrefix: "key")
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType">The type containes the key.</param>
        /// <param name="keyPrefix">The prefix for the key mapping, for example, for the navigation it count be "relatedKey".</param>
        public KeySegmentTemplate(IEdmEntityType entityType, string keyPrefix)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));

            var keys = entityType.Key().ToArray();
            if (keys.Length == 1)
            {
                Template = $"{{{keyPrefix}}}";
                _keyMappings[keys[0].Name] = ($"{keyPrefix}", keys[0].Type);
            }
            else
            {
                foreach (var key in keys)
                {
                    _keyMappings[key.Name] = ($"{keyPrefix}{key.Name}", key.Type);
                }

                Template = string.Join(",", _keyMappings.Select(a => $"{a.Key}={a.Value.Item1}"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Template { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEdmEntityType EntityType { get; }
    }
}
