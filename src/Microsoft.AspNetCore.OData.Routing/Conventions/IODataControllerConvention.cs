using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Routing.Extensions;
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
    public interface IODataControllerConvention
    {
        /// <summary>
        /// 
        /// </summary>
        //public ICollection<IODataActionConvention> Conventions { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <param name="controller"></param>
        public bool Apply(string prefix, IEdmModel model, ControllerModel controller);
    }

    /// <summary>
    /// 
    /// </summary>
    public class ODataModelAttributeConvention : IODataControllerConvention
    {
        //public ICollection<IODataActionConvention> Conventions { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <param name="controller"></param>
        public bool Apply(string prefix, IEdmModel model, ControllerModel controller)
        {
            //ODataModelAttribute odataModel = GetAttribute<ODataModelAttribute>(controller);
            //if (odataModel == null)
            //{
            //    return true; // apply to all model
            //}
            //else if (prefix == odataModel.Model)
            //{
            //    return true;
            //}

            return false;
        }
    }

}
