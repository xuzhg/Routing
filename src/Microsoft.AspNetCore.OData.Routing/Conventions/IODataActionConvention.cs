// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OData.Edm;

namespace Microsoft.AspNetCore.OData.Routing.Conventions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IODataActionConvention
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
