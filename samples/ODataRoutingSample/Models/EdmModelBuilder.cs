using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataRoutingSample.Models
{
    public static class EdmModelBuilder
    {
   //     private static IEdmModel _edmModel;

        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            return builder.GetEdmModel();
        }

        public static IEdmModel GetEdmModelV1()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }

        public static IEdmModel GetEdmModelV2()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Order>("Orders");
            return builder.GetEdmModel();
        }
    }
}
