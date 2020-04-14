// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.OData;

namespace Microsoft.AspNetCore.OData.Routing
{
    /// <summary>
    /// A class for managing per-route service containers.
    /// </summary>
    public class PerRouteContainer : IPerRouteContainer
    {
        private IDictionary<string, string> routeMapping = new Dictionary<string, string>();
        private ConcurrentDictionary<string, IServiceProvider> _perRouteContainers;
        private IServiceProvider _nonODataRouteContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerRouteContainer"/> class.
        /// </summary>
        public PerRouteContainer()
        {
            this._perRouteContainers = new ConcurrentDictionary<string, IServiceProvider>();
        }

        /// <summary>
        /// Gets or sets a function to build an <see cref="IContainerBuilder"/>
        /// </summary>
        public Func<IContainerBuilder> BuilderFactory { get; set; }

        /// <summary>
        /// Gets the container for a given route name.
        /// </summary>
        /// <param name="routeName">The route name.</param>
        /// <returns>The root container for the route name.</returns>
        protected virtual IServiceProvider GetContainer(string routeName)
        {
            if (String.IsNullOrEmpty(routeName))
            {
                return _nonODataRouteContainer;
            }

            IServiceProvider rootContainer;
            if (_perRouteContainers.TryGetValue(routeName, out rootContainer))
            {
                return rootContainer;
            }

            return null;
        }

        /// <summary>
        /// Sets the container for a given route name.
        /// </summary>
        /// <param name="routeName">The route name.</param>
        /// <param name="rootContainer">The root container to set.</param>
        /// <remarks>Used by unit tests to insert root containers.</remarks>
        protected virtual void SetContainer(string routeName, IServiceProvider rootContainer)
        {
            if (rootContainer == null)
            {
                throw new InvalidOperationException(SRResources.NullContainer);
            }

            if (String.IsNullOrEmpty(routeName))
            {
                _nonODataRouteContainer = rootContainer;
            }
            else
            {
                this._perRouteContainers.AddOrUpdate(routeName, rootContainer, (k, v) => rootContainer);
            }
        }

        /// <summary>
        /// Add a routing mapping
        /// </summary>
        /// <param name="routeName">The route name</param>
        /// <param name="routePrefix">The route prefix</param>
        public virtual void AddRoute(string routeName, string routePrefix)
        {
            routeMapping[routeName] = routePrefix;
        }

        /// <summary>
        /// Get the route prefix
        /// </summary>
        /// <param name="routeName">The route name.</param>
        /// <returns>The route prefix.</returns>
        public string GetRoutePrefix(string routeName)
        {
            return routeMapping[routeName];
        }

        /// <summary>
        /// Create a root container for a given route name.
        /// </summary>
        /// <param name="routeName">The route name.</param>
        /// <param name="configureAction">The configuration actions to apply to the container.</param>
        /// <returns>An instance of <see cref="IServiceProvider"/> to manage services for a route.</returns>
        public IServiceProvider CreateODataRootContainer(string routeName, Action<IContainerBuilder> configureAction)
        {
            IServiceProvider rootContainer = this.CreateODataRootContainer(configureAction);
            this.SetContainer(routeName, rootContainer);

            return rootContainer;
        }

        /// <summary>
        /// Create a root container not associated with a route.
        /// </summary>
        /// <param name="configureAction">The configuration actions to apply to the container.</param>
        /// <returns>An instance of <see cref="IServiceProvider"/> to manage services for a route.</returns>
        public IServiceProvider CreateODataRootContainer(Action<IContainerBuilder> configureAction)
        {
            IContainerBuilder builder = CreateContainerBuilderWithCoreServices();

            configureAction?.Invoke(builder);

            IServiceProvider rootContainer = builder.BuildContainer();
            if (rootContainer == null)
            {
                throw new InvalidOperationException(SRResources.NullContainer);
            }

            return rootContainer;
        }

        /// <summary>
        /// Check if the root container for a given route name exists.
        /// </summary>
        /// <param name="routeName">The route name.</param>
        /// <returns>true if root container for the route name exists, false otherwise.</returns>
        public bool HasODataRootContainer(string routeName)
        {
            IServiceProvider rootContainer = this.GetContainer(routeName);
            return rootContainer != null;
        }

        /// <summary>
        /// Get the root container for a given route name.
        /// </summary>
        /// <param name="routeName">The route name.</param>
        /// <returns>The root container for the route name.</returns>
        /// <remarks>
        /// This function will throw an exception if no container is found
        /// in order to localize the failure and provide a consistent error
        /// message. Use <see cref="HasODataRootContainer"/> to test of a container
        /// exists without throwing an exception.
        /// </remarks>
        public IServiceProvider GetODataRootContainer(string routeName)
        {
            IServiceProvider rootContainer = this.GetContainer(routeName);
            if (rootContainer == null)
            {
                if (String.IsNullOrEmpty(routeName))
                {
                    throw new InvalidOperationException(SRResources.MissingNonODataContainer);
                }
                else
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, SRResources.MissingODataContainer, routeName));
                }
            }

            return rootContainer;
        }

        /// <summary>
        /// Create a container builder with the default OData services.
        /// </summary>
        /// <returns>An instance of <see cref="IContainerBuilder"/> to manage services.</returns>
        protected IContainerBuilder CreateContainerBuilderWithCoreServices()
        {
            IContainerBuilder builder;
            if (this.BuilderFactory != null)
            {
                builder = this.BuilderFactory();
                if (builder == null)
                {
                    throw new InvalidOperationException(SRResources.NullContainer);
                }
            }
            else
            {
                builder = new DefaultContainerBuilder();
            }

            builder.AddDefaultODataServices();

            // Set Uri resolver to by default enabling unqualified functions/actions and case insensitive match.
            //builder.AddService(
            //    ServiceLifetime.Singleton,
            //    typeof(ODataUriResolver),
            //    sp => new UnqualifiedODataUriResolver { EnableCaseInsensitive = true });

            return builder;
        }
    }
}
