// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData;
using Microsoft.OData.Edm;
using ServiceLifetime = Microsoft.OData.ServiceLifetime;

namespace Microsoft.AspNetCore.OData.Routing
{
    /// <summary>
    /// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add OData routes.
    /// </summary>
    public static class ODataEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Maps the specified OData route and the OData route attributes.
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="routeName">The name of the route to map.</param>
        /// <param name="routePrefix">The prefix to add to the OData route's path template.</param>
        /// <param name="model">The EDM model to use for parsing OData paths.</param>
        /// <returns>The input <see cref="IEndpointRouteBuilder"/>.</returns>
        public static IEndpointRouteBuilder MapODataRoute(this IEndpointRouteBuilder builder,
            string routeName,
            string routePrefix,
            IEdmModel model)
        {
            return builder.MapODataRoute(
                routeName,
                routePrefix,
                containerBuilder => containerBuilder.AddService(Microsoft.OData.ServiceLifetime.Singleton, sp => model)
                    .AddService<IEnumerable<IODataRoutingConvention>>(Microsoft.OData.ServiceLifetime.Singleton,
                        sp => ODataRoutingConventions.CreateDefaultWithAttributeRouting(routeName, builder.ServiceProvider)));
        }

        /// <summary>
        /// Maps the specified OData route and the OData route attributes. When the <paramref name="batchHandler"/> is
        /// non-<c>null</c>, it will create a '$batch' endpoint to handle the batch requests.
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="routeName">The name of the route to map.</param>
        /// <param name="routePrefix">The prefix to add to the OData route's path template.</param>
        /// <param name="model">The EDM model to use for parsing OData paths.</param>
        /// <param name="batchHandler">The <see cref="ODataBatchHandler"/>.</param>
        /// <returns>The <see cref="IEndpointRouteBuilder"/>.</returns>
        public static IEndpointRouteBuilder MapODataRoute(this IEndpointRouteBuilder builder,
            string routeName,
            string routePrefix,
            IEdmModel model,
            ODataBatchHandler batchHandler)
        {
            return builder.MapODataRoute(routeName, routePrefix, containerBuilder =>
                containerBuilder.AddService(ServiceLifetime.Singleton, sp => model)
                       .AddService(ServiceLifetime.Singleton, sp => batchHandler)
                       .AddService<IEnumerable<IODataRoutingConvention>>(ServiceLifetime.Singleton, sp =>
                           ODataRoutingConventions.CreateDefaultWithAttributeRouting(routeName, builder.ServiceProvider)));
        }

        /// <summary>
        /// Maps the specified OData route.
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="routeName">The name of the route to map.</param>
        /// <param name="routePrefix">The prefix to add to the OData route's path template.</param>
        /// <param name="model">The EDM model to use for parsing OData paths.</param>
        /// <param name="pathHandler">The <see cref="IODataPathHandler"/> to use for parsing the OData path.</param>
        /// <param name="routingConventions">
        /// The OData routing conventions to use for controller and action selection.
        /// </param>
        /// <returns>The <see cref="IEndpointRouteBuilder"/>.</returns>
        public static IEndpointRouteBuilder MapODataRoute(this IEndpointRouteBuilder builder,
            string routeName,
            string routePrefix,
            IEdmModel model,
            IODataPathHandler pathHandler,
            IEnumerable<IODataRoutingConvention> routingConventions)
        {
            return builder.MapODataRoute(routeName, routePrefix, containerBuilder =>
                containerBuilder.AddService(ServiceLifetime.Singleton, sp => model)
                       .AddService(ServiceLifetime.Singleton, sp => pathHandler)
                       .AddService(ServiceLifetime.Singleton, sp => routingConventions.ToList().AsEnumerable()));
        }

        /// <summary>
        /// Maps the specified OData route. When the <paramref name="batchHandler"/> is non-<c>null</c>, it will
        /// create a '$batch' endpoint to handle the batch requests.
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="routeName">The name of the route to map.</param>
        /// <param name="routePrefix">The prefix to add to the OData route's path template.</param>
        /// <param name="model">The EDM model to use for parsing OData paths.</param>
        /// <param name="pathHandler">The <see cref="IODataPathHandler" /> to use for parsing the OData path.</param>
        /// <param name="routingConventions">
        /// The OData routing conventions to use for controller and action selection.
        /// </param>
        /// <param name="batchHandler">The <see cref="ODataBatchHandler"/>.</param>
        /// <returns>The <see cref="IEndpointRouteBuilder"/>.</returns>
        public static IEndpointRouteBuilder MapODataRoute(this IEndpointRouteBuilder builder,
            string routeName,
            string routePrefix,
            IEdmModel model,
            IODataPathHandler pathHandler,
            IEnumerable<IODataRoutingConvention> routingConventions,
            ODataBatchHandler batchHandler)
        {
            return builder.MapODataRoute(routeName, routePrefix, containerBuilder =>
                containerBuilder.AddService(ServiceLifetime.Singleton, sp => model)
                       .AddService(ServiceLifetime.Singleton, sp => pathHandler)
                       .AddService(ServiceLifetime.Singleton, sp => routingConventions.ToList().AsEnumerable())
                       .AddService(ServiceLifetime.Singleton, sp => batchHandler));
        }

        /// <summary>
        /// Maps the specified OData route and the OData route attributes.
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="routeName">The name of the route to map.</param>
        /// <param name="routePrefix">The prefix to add to the OData route's path template.</param>
        /// <param name="configureAction">The configuring action to add the services to the root container.</param>
        /// <returns>The input <see cref="IEndpointRouteBuilder"/>.</returns>
        public static IEndpointRouteBuilder MapODataRoute(this IEndpointRouteBuilder builder,
            string routeName,
            string routePrefix,
            Action<IContainerBuilder> configureAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (routeName == null)
            {
                throw new ArgumentNullException(nameof(routeName));
            }

            // Build and configure the root container.
            IServiceProvider serviceProvider = builder.ServiceProvider;

            IPerRouteContainer perRouteContainer = serviceProvider.GetRequiredService<IPerRouteContainer>();
            if (perRouteContainer == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, SRResources.MissingODataServices, nameof(IPerRouteContainer)));
            }

            // Make sure the MetadataController is registered with the ApplicationPartManager.
            ApplicationPartManager applicationPartManager = serviceProvider.GetRequiredService<ApplicationPartManager>();
            applicationPartManager.ApplicationParts.Add(new AssemblyPart(typeof(MetadataController).Assembly));

            // Create an service provider for this route. Add the default services to the custom configuration actions.
            Action<IContainerBuilder> builderAction = ConfigureDefaultServices(builder, configureAction);

            IServiceProvider subServiceProvider = perRouteContainer.CreateODataRootContainer(routeName, builderAction);

            // Resolve the path handler and set URI resolver to it.
            IODataPathHandler pathHandler = subServiceProvider.GetRequiredService<IODataPathHandler>();

            // If settings is not on local, use the global configuration settings.
            ODataOptions options = serviceProvider.GetRequiredService<ODataOptions>();
            //if (pathHandler != null && pathHandler.UrlKeyDelimiter == null)
            //{
            //    pathHandler.UrlKeyDelimiter = options.UrlKeyDelimiter;
            //}

            // Resolve HTTP handler, create the OData route and register it.
            routePrefix = RemoveTrailingSlash(routePrefix);

            // If a batch handler is present, register the route with the batch path mapper. This will be used
            // by the batching middleware to handle the batch request. Batching still requires the injection
            // of the batching middleware via UseODataBatching().
            ODataBatchHandler batchHandler = subServiceProvider.GetService<ODataBatchHandler>();
            if (batchHandler != null)
            {
                // TODO: for the $batch, need refactor/test it for more.
                batchHandler.ODataRouteName = routeName;

                string batchPath = String.IsNullOrEmpty(routePrefix)
                    ? '/' + ODataRouteConstants.Batch
                    : '/' + routePrefix + '/' + ODataRouteConstants.Batch;

                ODataBatchPathMapping batchMapping = builder.ServiceProvider.GetRequiredService<ODataBatchPathMapping>();
                batchMapping.IsEndpointRouting = true;
                batchMapping.AddRoute(routeName, batchPath);
            }

            builder.MapDynamicControllerRoute<ODataEndpointRouteValueTransformer>(
                ODataEndpointPattern.CreateODataEndpointPattern(routeName, routePrefix));

            perRouteContainer.AddRoute(routeName, routePrefix);

            return builder;
        }

        /// <summary>
        /// Remote the trailing slash from a route prefix string.
        /// </summary>
        /// <param name="routePrefix">The route prefix string.</param>
        /// <returns>The route prefix string without a trailing slash.</returns>
        private static string RemoveTrailingSlash(string routePrefix)
        {
            if (!String.IsNullOrEmpty(routePrefix))
            {
                int prefixLastIndex = routePrefix.Length - 1;
                if (routePrefix[prefixLastIndex] == '/')
                {
                    // Remove the last trailing slash if it has one.
                    routePrefix = routePrefix.Substring(0, routePrefix.Length - 1);
                }
            }

            return routePrefix;
        }

        /// <summary>
        /// Configure the default services.
        /// </summary>
        /// <param name="routeBuilder">The <see cref="IEndpointRouteBuilder"/>.</param>
        /// <param name="configureAction">The configuring action to add the services to the root container.</param>
        /// <returns>A configuring action to add the services to the root container.</returns>
        internal static Action<IContainerBuilder> ConfigureDefaultServices(IEndpointRouteBuilder routeBuilder, Action<IContainerBuilder> configureAction)
        {
            return (builder =>
            {
                //// Add platform-specific services here. Add Configuration first as other services may rely on it.
                //// For assembly resolution, add the and internal (IWebApiAssembliesResolver) where IWebApiAssembliesResolver
                //// is transient and instantiated from ApplicationPartManager by DI.
                //builder.AddService<IWebApiAssembliesResolver, WebApiAssembliesResolver>(ServiceLifetime.Transient);
                //builder.AddService<IODataPathTemplateHandler, DefaultODataPathHandler>(ServiceLifetime.Singleton);
                //builder.AddService<IETagHandler, DefaultODataETagHandler>(ServiceLifetime.Singleton);

                //// Access the default query settings and options from the global container.
                //builder.AddService(ServiceLifetime.Singleton, sp => routeBuilder.GetDefaultQuerySettings());
                //builder.AddService(ServiceLifetime.Singleton, sp => routeBuilder.GetDefaultODataOptions());

                //// Add the default webApi services.
                //builder.AddDefaultWebApiServices();

                // Add custom actions.
                configureAction?.Invoke(builder);
            });
        }
    }
}