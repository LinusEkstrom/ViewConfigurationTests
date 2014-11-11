using System.Collections.Generic;
using System.Security.Principal;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ViewComposition;

namespace EPiServerMVC3.ViewConfiguration
{
    public class ViewTransformationSettings
    {
        private readonly IComponentManager _componentManager;

        public ViewTransformationSettings(string viewName, string[] roles)
            : this(viewName, roles, ServiceLocator.Current.GetInstance<IComponentManager>())
        {
        }

        public ViewTransformationSettings(string viewName, string[] roles, IComponentManager componentManager)
        {
            ViewName = viewName;
            ComponentsToAdd = new List<IPluggableComponentDefinition>();
            ComponentsToRemove = new List<IComponentMatcher>();
            Roles = roles;
            _componentManager = componentManager;
        }

        public virtual void AddAddComponentRule(string plugInArea, string componentDefinitionName, int customSortOrder = 0)
        {
            var componentDefinition = _componentManager.GetComponentDefinition(componentDefinitionName);
            var addRule = new ConfigurationSettingsPlugIn(componentDefinition, _componentManager, plugInArea, customSortOrder);
            ComponentsToAdd.Add(addRule);
        }

        public virtual void AddRemoveComponentRule(string componentDefinition)
        {
            ComponentsToRemove.Add(new ConfigurationComponentMatcher(componentDefinition));
        }

        #region Properties

        public string ViewName { get; set; }

        public List<IPluggableComponentDefinition> ComponentsToAdd { get; set; }

        public List<IComponentMatcher> ComponentsToRemove { get; set; }

        public string[] Roles { get; set; }

        public bool IsInRole(IPrincipal principal)
        {
            foreach (string role in Roles)
            {
                if (principal.IsInRole(role))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}