using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.OData.Routing.Extensions;
using ODataRoutingSample.Models;
using Microsoft.OData.Edm;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.Http;

namespace ODataRoutingSample
{
    public class Startup
    {
        private IEdmModel model;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            model = EdmModelBuilder.GetEdmModel();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new MetadataApplicationModelConventionAttribute());
                options.Conventions.Add(new MetadataActionModelConvention());
            });

           // services.AddODataRouting();
           // services.AddODataRouting(model);

            services.AddODataRouting(options => options
                .AddModel(EdmModelBuilder.GetEdmModel())
                .AddModel("v1", EdmModelBuilder.GetEdmModelV1())
                .AddModel("v2{data}", EdmModelBuilder.GetEdmModelV2()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.Use(next => context =>
            {
                var endpoint = context.GetEndpoint();
                if (endpoint == null)
                {
                    return next(context);
                }


                return next(context);
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//.WithOData(model);
      //          endpoints.MapODataRoute("odata", "odata", model);
            });
        }
    }
}
