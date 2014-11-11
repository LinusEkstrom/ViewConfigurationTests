using System;
using System.Collections.Generic;
using System.Security.Principal;
using EPiServer.Shell.ViewComposition;

namespace EPiServerMVC3.ViewConfiguration
{
    public class ConfigurationSettingsPlugIn : IPluggableComponentDefinition
    {
        private IComponentDefinition _definition;
        private IComponentManager _componentManager;
        private string _plugInArea;
        private int _sortOrder;

        public ConfigurationSettingsPlugIn(IComponentDefinition definition, IComponentManager componentManager, string plugInArea, int sortOrder)
        {
            _definition = definition;
            _componentManager = componentManager;
            _plugInArea = plugInArea;
            _sortOrder = sortOrder;
        }

        public IComponent CreateComponent()
        {
            var component = _componentManager.CreateComponent(_definition, EPiServer.Security.PrincipalInfo.CurrentPrincipal);

            if(_sortOrder != 0)
            { 
                var componentBase = component as ComponentBase;
                
                if(componentBase != null)
                {
                    componentBase.SortOrder = _sortOrder;
                }
            }

            return component;
        }

        public bool SupportsAutomaticRegistration
        {
            get { return true; }
        }

        public bool MatchesContainer(IContainer container)
        {
            return String.Equals(_plugInArea, container.PlugInArea, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsAvailableForUserSelection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<string> AllowedRoles
        {
            get;
            set;
        }

        public bool HasAccess(IPrincipal principal)
        {
            return true;
        }
    }
}