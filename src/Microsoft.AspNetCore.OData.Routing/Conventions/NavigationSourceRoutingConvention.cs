// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OData.Edm;

namespace Microsoft.AspNetCore.OData.Routing.Conventions
{
    /// <summary>
    /// The base class for (entitySet/singleton) routing convention.
    /// </summary>
    public abstract class NavigationSourceRoutingConvention : IODataControllerActionConvention
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract int Order { get; }

        /// <summary>
        /// used for cache
        /// </summary>
        internal IEdmNavigationSource NavigationSource { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public bool AppliesToController(string prefix, IEdmModel model, ControllerModel controller)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            string controllerName = controller.ControllerName;
            NavigationSource = model.EntityContainer?.FindEntitySet(controllerName);

            // Cached the singleton, because we call this method first, then AppliesToAction
            // FindSingleton maybe time consuming.
            return NavigationSource != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <param name="action"></param>
        public abstract bool AppliesToAction(string prefix, IEdmModel model, ActionModel action);
    }
}
