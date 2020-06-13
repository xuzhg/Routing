using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.OData.Routing
{
    /// <summary>
    /// 
    /// </summary>
    public class ODataRoutingOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, IEdmModel> Models { get; } = new Dictionary<string, IEdmModel>();

        /// <summary>
        /// Routing conventions for all models???
        /// </summary>
        public IList<IODataRoutingConventionProvider> Conventions { get; } = new List<IODataRoutingConventionProvider>
        {
            new MetadataRoutingConventionProvider(),
            new EntitySetRoutingConventionProvider(),
            new EntityRoutingConventionProvider()
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ODataRoutingOptions AddModel(IEdmModel model)
        {
            return AddModel(string.Empty, model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ODataRoutingOptions AddModel(string name, IEdmModel model)
        {
            if (Models.ContainsKey(name))
            {
                throw new Exception($"Contains the same name for the model: {name}");
            }

            Models[name] = model;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="convention"></param>
        /// <returns></returns>
        public ODataRoutingOptions AddConvention(IODataRoutingConventionProvider convention)
        {
            Conventions.Add(convention);
            return null;
        }
    }

    //public class ODataRoutingConfig
    //{
    //    /// <summary>
    //    /// Routing conventions
    //    /// </summary>
    //    public IList<IODataRoutingConventionProvider> Conventions { get; } = new List<IODataRoutingConventionProvider>();
    //}
}
