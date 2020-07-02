using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.OData.Routing.Conventions
{
    internal class ODataRoutingConventionFactory
    {
    //    private readonly IODataActionConvention[] _odataRoutingConventionProviders;

        public ODataRoutingConventionFactory(
     //      IEnumerable<IODataActionConvention> odataRoutingConventionProviders,
           IOptions<ODataRoutingOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

         //   _odataRoutingConventionProviders = odataRoutingConventionProviders.OrderBy(p => p.Order).ToArray();
          //  _conventions = options.Value.Conventions;
        }


    }
}
