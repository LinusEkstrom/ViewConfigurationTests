using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using EPiServer.Shell.ViewComposition;

namespace EPiServerMVC3.ViewConfiguration
{
    /// <summary>
    /// ConfigurationViewTransformerBase is used to alter the default behaviour and components for a given view.
    /// </summary>
    public class ConfigurationViewTransformerBase : IViewTransformer
    {
        private readonly IComponentManager _componentManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationViewTransformer"/> class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        public ConfigurationViewTransformerBase(IComponentManager componentManager)
        {
            Settings = new Collection<ViewTransformationSettings>();
            _componentManager = componentManager;
        }

        /// <summary>
        /// Transforms the view according to the given settings.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="principal">The principal.</param>
        public virtual void TransformView(ICompositeView view, IPrincipal principal)
        {
            var viewTransformationSettings = Settings.Where(v => v.ViewName.Equals(view.Name, StringComparison.OrdinalIgnoreCase));
            if (viewTransformationSettings == null)
            {
                return;
            }

            var effectiveSettings = viewTransformationSettings.FirstOrDefault(s => s.IsInRole(EPiServer.Security.PrincipalInfo.CurrentPrincipal));

            if (effectiveSettings == null)
            {
                return;
            }

            view.RootContainer.RemoveComponentsRecursive(effectiveSettings.ComponentsToRemove, false); 
            view.RootContainer.AddComponentsRecursive(view.Name, effectiveSettings.ComponentsToAdd, principal);
            
        }

        /// <summary>
        /// Gets a collection of <see cref="ViewTransformationSettingsCollection"/> for each view that has settings.
        /// </summary>
        /// <value>The settings.</value>
        public Collection<ViewTransformationSettings> Settings { get; private set; }

        /// <summary>
        /// Used to select the order of execution when there are several <see cref="IViewTransformer"/>s.
        /// </summary>
        /// <value>ConfigurationViewTransformer has 100.</value>
        public virtual int SortOrder
        {
            get { return 100; }
        }

    }
}