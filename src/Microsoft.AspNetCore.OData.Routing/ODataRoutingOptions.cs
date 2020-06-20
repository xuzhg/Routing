using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.OData;
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
        //public IList<IODataActionConvention> Conventions { get; } = new List<IODataActionConvention>
        //{
        //    new MetadataRoutingConventionProvider(),
        //    new EntitySetRoutingConventionProvider(),
        //    new EntityRoutingConventionProvider()
        //};

        public IList<IODataControllerActionConvention> Conventions { get; } = new List<IODataControllerActionConvention>
        {
            new MetadataRoutingConvention(),
            new SingletonRoutingConvention(),
         //   new EntitySetRoutingConvention(),
            new OperationImportRoutingConvention()
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
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

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
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <param name="configureAction"></param>
        /// <returns></returns>
        public ODataRoutingConventions AddModel(string name, IEdmModel model, Action<IContainerBuilder> configureAction)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="convention"></param>
        /// <returns></returns>
        public ODataRoutingOptions AddConvention(IODataControllerActionConvention convention)
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
