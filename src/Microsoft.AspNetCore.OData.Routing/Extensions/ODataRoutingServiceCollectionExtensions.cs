using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            services.AddSingleton<IPerRouteContainer, PerRouteContainer>();

            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IApplicationModelProvider, ODataRoutingApplicationModelProvider>());

            services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ODataEndpointRoutingMatcherPolicy>());
        }
    }
}
