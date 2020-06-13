using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.OData.Routing.Conventions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IODataRoutingConventionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public bool CanApply(ControllerModel controller);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void Apply(ActionModel action);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <param name="action"></param>
        public bool Apply(string prefix, IEdmModel model, ActionModel action);
    }
}
