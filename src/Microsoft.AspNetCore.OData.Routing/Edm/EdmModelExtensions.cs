// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.OData.Edm;
using System;

namespace Microsoft.AspNetCore.OData.Routing
{
    internal static class EdmModelExtensions
    {
        internal static string GetNavigationSourceUrl(this IEdmModel model, IEdmNavigationSource navigationSource)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (navigationSource == null)
            {
                throw new ArgumentNullException(nameof(navigationSource));
            }

            //NavigationSourceUrlAnnotation annotation = model.GetAnnotationValue<NavigationSourceUrlAnnotation>(navigationSource);
            //if (annotation == null)
            //{
            //    return navigationSource.Name;
            //}
            //else
            //{
            //    return annotation.Url;
            //}

            return null;
        }
    }
}
