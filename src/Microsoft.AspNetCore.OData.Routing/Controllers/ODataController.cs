// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.OData.Routing
{
    /// <summary>
    /// Defines a base class for OData controllers that support writing and reading data using the OData formats.
    /// </summary>
    /// <remarks>These attributes and signature uses types that are AspNetCore-specific.</remarks>
    public abstract class ODataController : ControllerBase
    {
        /// <summary>
        /// Creates an action result with the specified values that is a response to a POST operation with an entity
        /// to an entity set.
        /// </summary>
        /// <typeparam name="TEntity">The created entity type.</typeparam>
        /// <param name="entity">The created entity.</param>
        /// <returns>A <see cref="CreatedODataResult{TEntity}"/> with the specified values.</returns>
        /// <remarks>These function uses types that are AspNetCore-specific.</remarks>
        protected virtual CreatedODataResult<TEntity> Created<TEntity>(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return new CreatedODataResult<TEntity>(entity);
        }

        /// <summary>
        /// Creates an action result with the specified values that is a response to a PUT, PATCH, or a MERGE operation
        /// on an OData entity.
        /// </summary>
        /// <typeparam name="TEntity">The updated entity type.</typeparam>
        /// <param name="entity">The updated entity.</param>
        /// <returns>An <see cref="UpdatedODataResult{TEntity}"/> with the specified values.</returns>
        /// <remarks>These function uses types that are AspNetCore-specific.</remarks>
        protected virtual UpdatedODataResult<TEntity> Updated<TEntity>(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return new UpdatedODataResult<TEntity>(entity);
        }
    }
}