// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Edm;

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

        public static IEnumerable<IEdmStructuredType> BaseTypes(
            this IEdmStructuredType structuralType)
        {
            IEdmStructuredType baseType = structuralType.BaseType;
            while (baseType != null)
            {
                yield return baseType;

                baseType = baseType.BaseType;
            }
        }

        public static IEnumerable<IEdmStructuredType> ThisAndBaseTypes(
            this IEdmStructuredType structuralType)
        {
            IEdmStructuredType baseType = structuralType;
            while (baseType != null)
            {
                yield return baseType;

                baseType = baseType.BaseType;
            }
        }

        public static IEnumerable<IEdmStructuredType> DerivedTypes(this IEdmStructuredType structuralType, IEdmModel model)
        {
            return model.FindAllDerivedTypes(structuralType);
        }

        public static IEdmStructuredType FindTypeInInheritance(this IEdmStructuredType structuralType, IEdmModel model, string typeName)
        {
            IEdmStructuredType baseType = structuralType;
            while (baseType != null)
            {
                if (GetName(baseType) == typeName)
                {
                    return baseType;
                }

                baseType = baseType.BaseType;
            }

            return model.FindAllDerivedTypes(structuralType).FirstOrDefault(c => GetName(c) == typeName);
        }

        private static string GetName(IEdmStructuredType type)
        {
            IEdmEntityType entityType = type as IEdmEntityType;
            if (entityType != null)
            {
                return entityType.Name;
            }

            return ((IEdmComplexType)type).Name;
        }
    }
}
