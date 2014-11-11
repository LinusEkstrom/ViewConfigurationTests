using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Cms.Shell.UI.Components;
using EPiServer.Cms.Shell.UI.CompositeViews;
using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;
using Samples;
using EPiServerMVC3.ViewConfiguration;
using EPiServer.Shell.ViewComposition.Containers;

namespace EPiServerMVC3
{
    [ViewTransformer]
    public class SampleViewTransformer : ConfigurationViewTransformerBase
    {
        public SampleViewTransformer(IComponentManager componentManager)
            : base(componentManager)
        {
            AddRulesForHomeView();
            AddRulesForDashboardView();
        }

        private void AddRulesForHomeView()
        {
            //The order of registration should affect importance.

            var viewTransformationSettings = new ViewTransformationSettings(HomeView.ViewName, new string[] { "administrators2" });

            viewTransformationSettings.AddRemoveComponentRule(new SharedBlocksComponent().DefinitionName);
            viewTransformationSettings.AddAddComponentRule(PlugInArea.NavigationDefaultGroup, new MediaComponent().DefinitionName, 1);

            Settings.Add(viewTransformationSettings);

            viewTransformationSettings = new ViewTransformationSettings(HomeView.ViewName, new string[] { "administrators" });

            viewTransformationSettings.AddRemoveComponentRule(new MediaComponent().DefinitionName);
            viewTransformationSettings.AddAddComponentRule(PlugInArea.NavigationDefaultGroup, new MediaComponent().DefinitionName, 1);

            Settings.Add(viewTransformationSettings);
        }

        private void AddRulesForDashboardView()
        {
            var viewTransformationSettings = new ViewTransformationSettings("/episerver/dashboard", new string[] { "administrators" });

            viewTransformationSettings.AddAddComponentRule(PlugInArea.DashboardDefaultTab, "EPiServer.Cms.Shell.UI.Controllers.XFormsViewerController");

            Settings.Add(viewTransformationSettings);
        }

        public override void TransformView(ICompositeView view, System.Security.Principal.IPrincipal principal)
        {
            base.TransformView(view, principal);

            var componentGroup = new ComponentGroup();

            componentGroup.Add(new MediaComponent().CreateComponent());

            var defaultTabContainer = view.RootContainer.Components[0] as IContainer;

            defaultTabContainer.Components.Add(componentGroup);
        }
    }
}