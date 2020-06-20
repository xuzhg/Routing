// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Microsoft.AspNetCore.OData.Routing.Template
{
    /// <summary>
    /// Represents a template that could match an <see cref="ODataPathTemplate"/>.
    /// </summary>
    public class ODataPathTemplate
    {
        private string _template;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataPathTemplate" /> class.
        /// </summary>
        /// <param name="segments">The path segment templates for the path.</param>
        public ODataPathTemplate(params ODataSegmentTemplate[] segments)
            : this((IList<ODataSegmentTemplate>)segments)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataPathTemplate" /> class.
        /// </summary>
        /// <param name="segments">The path segment templates for the path.</param>
        public ODataPathTemplate(IEnumerable<ODataSegmentTemplate> segments)
            : this(segments.ToList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataPathTemplate" /> class.
        /// </summary>
        /// <param name="segments">The path segments for the path.</param>
        public ODataPathTemplate(IList<ODataSegmentTemplate> segments)
        {
            if (segments == null)
            {
                throw new ArgumentNullException(nameof(segments));
            }

            Segments = new ReadOnlyCollection<ODataSegmentTemplate>(segments);
        }

        /// <summary>
        /// Gets the path segments for the OData path.
        /// </summary>
        public ReadOnlyCollection<ODataSegmentTemplate> Segments { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool KeyAsSegment { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Template
        {
            get
            {
                if (_template == null)
                {
                    _template = CalculateTemplate();
                }

                return _template;
            }
        }

        private string CalculateTemplate()
        {
            int index = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var segment in Segments)
            {
                KeySegmentTemplate keySg = segment as KeySegmentTemplate;
                if (keySg != null)
                {
                    if (KeyAsSegment)
                    {
                        sb.Append("/");
                        sb.Append(segment.Template);
                    }
                    else
                    {
                        sb.Append("(");
                        sb.Append(segment.Template);
                        sb.Append(")");
                    }
                }
                else
                {
                    if (index != 0)
                    {
                        sb.Append("/");
                    }
                    sb.Append(segment.Template);
                    index++;
                }
            }

            return sb.ToString();
        }
    }
}
