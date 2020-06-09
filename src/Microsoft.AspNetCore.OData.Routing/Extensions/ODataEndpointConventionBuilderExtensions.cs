using Microsoft.AspNetCore.Builder;
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
    public static class ODataEndpointConventionBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TBuilder WithOData<TBuilder>(this TBuilder builder, IEdmModel model) where TBuilder : IEndpointConventionBuilder
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            builder.Add(endpointBuilder =>
            {
                endpointBuilder.Metadata.Add(new ODataEndpointMetadata(model));
            });

            return builder;
        }

    }
}
