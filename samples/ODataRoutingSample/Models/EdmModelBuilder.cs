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

            builder.Action("ResetData");

            return builder.GetEdmModel();
        }

        public static IEdmModel GetEdmModelV1()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");

            var function = builder.Function("RateByOrder");
            function.Parameter<int>("order");
            function.Returns<int>();

            return builder.GetEdmModel();
        }

        public static IEdmModel GetEdmModelV2()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Order>("Orders");

            var function = builder.Function("RateByOrder");
            function.Parameter<int>("order");
            function.Returns<int>();

            return builder.GetEdmModel();
        }
    }
}
