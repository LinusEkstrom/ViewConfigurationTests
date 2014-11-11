using System;
using EPiServer.Shell.ViewComposition;

namespace EPiServerMVC3.ViewConfiguration
{
    public class ConfigurationComponentMatcher : IComponentMatcher
    {
        private string _plugInArea;
        private string _definitionName;

        public ConfigurationComponentMatcher(string definitionName)
            : this(definitionName, null)
        { }

        public ConfigurationComponentMatcher(string definitionName, string plugInArea)
        {
            _plugInArea = plugInArea;
            _definitionName = definitionName;
        }

        public bool MatchesContainer(IContainer container)
        {
            return String.IsNullOrEmpty(_plugInArea) || String.Equals(_plugInArea, container.PlugInArea, StringComparison.OrdinalIgnoreCase);
        }

        public bool MatchesComponent(IComponent component)
        {
            return String.Equals(_definitionName, component.DefinitionName, StringComparison.OrdinalIgnoreCase);
        }
    }
}