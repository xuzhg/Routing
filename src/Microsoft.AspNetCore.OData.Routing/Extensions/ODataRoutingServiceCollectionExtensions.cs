﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;

namespace Microsoft.AspNetCore.OData.Routing.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ODataRoutingServiceCollectionExtensions
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="services"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddODataRouting(this IServiceCollection services)
        //{
        //    services.TryAddEnumerable(ServiceDescriptor.Singleton<IApplicationModelProvider>(new ODataApplicationModelProvider(model)));
        //    services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ODataEndpointMatcherPolicy>());

        //    return services;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddODataRouting(this IServiceCollection services, IEdmModel model)
        {
            services.AddSingleton<IPerRouteContainer, PerRouteContainer>();

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IApplicationModelProvider>(new ODataApplicationModelProvider(model)));
            services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ODataEndpointMatcherPolicy>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ODataEndpointRoutingMatcherPolicy>());
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddConvention<T>(this IServiceCollection services)
            where T : class, IODataControllerActionConvention
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, T>());

            return services;
        }

        ///// <summary>
        ///// Consider to
        ///// </summary>
        ///// <param name="builder"></param>
        ///// <returns></returns>
        //public static IODataBuilder AddOData(this IMvcBuilder builder)
        //{
        //    // 
        //    return builder;
        //}

        public static IServiceCollection AddOData(this IServiceCollection services, Action<ODataRoutingOptions> setupAction)
        {
            
            AddODataRoutingServices(services);
            services.Configure(setupAction);
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddODataRouting(this IServiceCollection services, Action<ODataRoutingOptions> setupAction)
        {
            AddODataRoutingServices(services);
            services.Configure(setupAction);
            return services;
        }

        static void AddODataRoutingServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IPerRouteContainer, PerRouteContainer>(); // it seems we don't need this?

            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<ODataRoutingOptions>, ODataRoutingOptionsSetup>());

            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IApplicationModelProvider, ODataRoutingApplicationModelProvider>());

            // for debug only
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IApplicationModelProvider, ODataEndpointModelDebugProvider>());

            services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ODataEndpointRoutingMatcherPolicy>());

            // OData Routing conventions
            // ~/$metadata
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, MetadataRoutingConvention>());

            // ~/EntitySet
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, EntitySetEndpointConvention>());

            // ~/EntitySet/{key}
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, EntityEndpointConvention>());

            // ~/Singleton
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, SingletonRoutingConvention>());

            // ~/EntitySet|Singleton/.../NS.Operation
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, OperationRoutingConvention>());

            // ~/OperationImport
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, OperationImportRoutingConvention>());

            // ~/EntitySet{key}|Singleton/{Property}
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, PropertyRoutingConvention>());

            // ~/EntitySet{key}|Singleton/{NavigationProperty}
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, NavigationRoutingConvention>());

            // ~/EntitySet{key}|Singleton/{NavigationProperty}/$ref
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IODataControllerActionConvention, RefRoutingConvention>());

            //services.TryAddEnumerable(
            //    ServiceDescriptor.Transient<IODataControllerActionConvention, MyConvention>());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyConvention : IODataControllerActionConvention
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => -100;

        public bool AppliesToAction(ODataControllerActionContext context)
        {
            return true; // apply to all controller
        }

        public bool AppliesToController(ODataControllerActionContext context)
        {
            return false; // continue for all others
        }
    }
}
