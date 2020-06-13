using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Routing.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.OData.Routing.Conventions
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityRoutingConventionProvider : IODataRoutingConventionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => -1000 + 100;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public bool CanApply(ControllerModel controller)
        {
            if (controller.ControllerType != typeof(MetadataController).GetTypeInfo())
            {
                return false;
            }

            Console.WriteLine(controller.ControllerName);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void Apply(ActionModel action)
        {
            if (action.Controller.ControllerType != typeof(MetadataController).GetTypeInfo())
            {
                return;
            }

            Console.WriteLine(action.Controller.ControllerName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <param name="action"></param>
        public bool Apply(string prefix, IEdmModel model, ActionModel action)
        {
            if (model.EntityContainer == null)
            {
                return false;
            }

            string controllerName = action.Controller.ControllerName;
            IEdmEntitySet entitySet = model.EntityContainer.FindEntitySet(controllerName);
            if (entitySet == null)
            {
                return false;
            }

            if (action.Parameters.Count < 1)
            {
                return false;
            }

            var entityTypeName = entitySet.EntityType().Name;
            var keys = entitySet.EntityType().Key().ToArray();

            string actionName = action.ActionMethod.Name;
            if ((actionName == "Get" ||
                actionName == $"Get{entityTypeName}" ||
                actionName == "Put" ||
                actionName == $"Put{entityTypeName}" ||
                actionName == "Patch" ||
                actionName == $"Patch{entityTypeName}" ||
                actionName == "Delete" ||
                actionName == $"Delete{entityTypeName}") &&
                keys.Length == action.Parameters.Count)
            {
                var mappings = new Dictionary<string, string>();
                if (keys.Length == 1)
                {
                    mappings[keys[0].Name] = "key";

                    // support key in parenthesis
                    string template = string.IsNullOrEmpty(prefix) ? $"{entitySet.Name}({{key}})" : $"{prefix}/{entitySet.Name}({{key}})";
                    AddSelector(action, entitySet, template, mappings);

                    // support key as segment
                    template = string.IsNullOrEmpty(prefix) ? $"{entitySet.Name}/{{key}}" : $"{prefix}/{entitySet.Name}/{{key}}";
                    AddSelector(action, entitySet, template, mappings);
                }
                else
                {

                    foreach (var key in keys)
                    {
                        mappings[key.Name] = $"key{key.Name}";
                    }

                    var keyString = string.Join(",", mappings.Select(a => $"{a.Key}={a.Value}"));

                    string template = string.IsNullOrEmpty(prefix) ? $"{entitySet.Name}({keyString})" : $"{prefix}/{entitySet.Name}({keyString})";
                    AddSelector(action, entitySet, template, mappings);
                }

                return true;
            }

            return false;
        }

        static void AddSelector(ActionModel action, IEdmEntitySet entitySet, string template, IDictionary<string, string> mappings)
        {
            SelectorModel selectorModel = new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(template) { Name = template })
            };
            selectorModel.EndpointMetadata.Add(new ODataEndpointMetadata(mappings, (rvd, mapping) => new ODataPath(
                new EntitySetSegment(entitySet),
                new KeySegment(
                    GetKeyValues(rvd, mapping, entitySet),
                    entitySet.EntityType(),
                    entitySet))));
            action.Selectors.Add(selectorModel);
        }

        static Dictionary<string, object> GetKeyValues(
                RouteValueDictionary rvd,
                IDictionary<string, string> mapping, IEdmEntitySet element)
        {
            var key = element.EntityType().Key();
            var result = new Dictionary<string, object>();
            foreach (var component in key)
            {
                var routeValueName = mapping[component.Name];
                if (rvd.TryGetValue(routeValueName, out var value))
                {
                    result[component.Name] = value;
                }
            }

            return result;
        }
    }
}
