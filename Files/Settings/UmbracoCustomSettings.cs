using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ExtendingUmbraco.Settings
{
    public class UmbracoCustomSettings
    {
        /// <summary>
        /// Gets or sets the plugins.
        /// </summary>
        /// <value>
        /// The plugins.
        /// </value>
        [XmlElement("Plugins")]
        public PluginsConfiguration Plugins { get; set; }

        /// <summary>
        /// Gets or sets the menu rendering configuration for this custom configuration section.
        /// </summary>
        [XmlElement("MenuRendering")]
        public MenuRenderingConfiguration MenuRendering { get; set; }
    }

    public class PluginsConfiguration
    {
        [XmlElement("Plugin")]
        public List<Plugin> All { get; set; }
    }

    public class Plugin
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        [XmlElement("Folder")]
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>
        /// The controller.
        /// </value>
        [XmlElement("Controller")]
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        /// <value>
        /// The user roles.
        /// </value>
        [XmlElement("UserRoles")]
        public UserRolesConfiguration UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the tabs.
        /// </summary>
        /// <value>
        /// The tabs.
        /// </value>
        [XmlElement("Tabs")]
        public TabsConfiguration Tabs { get; set; }

        /// <summary>
        /// Gets or sets the menu rendering items.
        /// </summary>
        /// <value>
        /// The menu rendering items.
        /// </value>
        [XmlElement("Values")]
        public PluginValuesConfiguration Values { get; set; }
    }

    public class UserRolesConfiguration
    {
        [XmlElement("UserRole")]
        public List<UserRole> All { get; set; }
    }

    /// <summary>
    /// Tabs Configuration Element
    /// </summary>
    public class TabsConfiguration
    {
        [XmlElement("Tab")]
        public List<Tab> All { get; set; }
    }

    /// <summary>
    /// Values Configuration Element
    /// </summary>
    public class PluginValuesConfiguration
    {
        [XmlElement("Value")]
        public List<PluginValues> All { get; set; }
    }


    /// <summary>
    /// User Role
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlElement("Name")]
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Tab configuration
    /// </summary>
    public class Tab
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [XmlElement("Id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [XmlElement("Label")]
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [XmlElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        [XmlElement("Template")]
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the template model.
        /// </summary>
        /// <value>
        /// The template model.
        /// </value>
        [XmlElement("TemplateModel")]
        public string TemplateModel { get; set; }
    }


    /// <summary>
    /// Plugin values configuration
    /// </summary>
    public class PluginValues
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [XmlElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [XmlElement("Value")]
        public string Value { get; set; }
    }

    public class MenuRenderingConfiguration
    {
        /// <summary>
        /// Gets or sets the menu rendering items.
        /// </summary>
        /// <value>
        /// The menu rendering items.
        /// </value>
        [XmlElement("Items")]
        public MenuRenderingItems Items { get; set; }

        /// <summary>
        /// Gets or sets the menu rendering items.
        /// </summary>
        /// <value>
        /// The menu rendering items.
        /// </value>
        [XmlElement("ItemsByNodeId")]
        public MenuRenderingItems ItemsByNodeId { get; set; }

        /// <summary>
        /// Gets or sets the menu items to remove.
        /// </summary>
        /// <value>
        /// The menu items to remove.
        /// </value>
        [XmlElement("ItemsToRemove")]
        public MenuItemsToRemove MenuItemsToRemove { get; set; }
    }

    /// <summary>
    /// This class provides the menu rendering item
    /// </summary>
    public class MenuRenderingItems
    {
        /// <summary>
        /// Gets or sets the menu rendering items.
        /// </summary>
        /// <value>
        /// The menu rendering items.
        /// </value>
        [XmlElement("Item")]
        public List<MenuRenderingItem> AllItems { get; set; }
    }

    /// <summary>
    /// This class provides the menu rendering item
    /// </summary>
    public class MenuItemsToRemove
    {
        /// <summary>
        /// Gets or sets the menu rendering items.
        /// </summary>
        /// <value>
        /// The menu rendering items.
        /// </value>
        [XmlElement("Item")]
        public List<string> AllItems { get; set; }
    }

    public class MenuRenderingItem
    {
        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        [XmlElement("NodeId")]
        public int? NodeId { get; set; }

        /// <summary>
        /// Gets or sets the applicable document type aliases.
        /// </summary>
        /// <value>
        /// The included document type alias.
        /// </value>
        [XmlElement("ApplicableAliases")]
        public ApplicableAliases ApplicableAliases { get; set; }

        /// <summary>
        /// Gets or sets the policies.
        /// </summary>
        /// <value>
        /// The policies.
        /// </value>
        [XmlElement("Policies")]
        public SecurityPolicies Policies { get; set; }

        /// <summary>
        /// Gets or sets the menu alias.
        /// </summary>
        /// <remarks>
        /// This is a unique alias for the menu item
        /// </remarks>
        /// <value>
        /// The menu alias.
        /// </value>
        [XmlElement("MenuAlias")]
        public string MenuAlias { get; set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        [XmlElement("Caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <remarks>
        /// This is the url to the html corresponding to the view for an umbraco plugin.
        /// </remarks>
        /// <value>
        /// The view.
        /// </value>
        [XmlElement("View")]
        public string View { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <remarks>
        /// Icons can be checked and selected at the following url:
        /// http://nicbell.github.io/ucreate/icons.html
        /// </remarks>
        /// <value>
        /// The icon.
        /// </value>
        [XmlElement("Icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the menu item separator.
        /// </summary>
        /// <value>
        /// The menu item separator.
        /// </value>
        [XmlElement("Separator")]
        public Separator Separator { get; set; }

        /// <summary>
        /// Gets or sets the index to insert the menu item.
        /// </summary>
        /// <remarks>
        /// If the index is out of range the menu item will be added to the end.
        /// </remarks>
        /// <value>
        /// The index of the menu item.
        /// </value>
        [XmlElement("Index")]
        public ushort Index { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether render to published.
        /// </summary>
        [XmlElement("RenderToPublished")]
        public bool? RenderToPublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether render to unpublished.
        /// </summary>
        [XmlElement("RenderToUnpublished")]
        public bool? RenderToUnpublished { get; set; }
    }

    /// <summary>
    /// This class provides the container for the applicable document type aliases
    /// </summary>
    public class ApplicableAliases
    {
        /// <summary>
        /// Gets or sets all of the INCLUDED document type aliases.
        /// </summary>
        /// <remarks>
        /// These are the alias that correspond to the document types that should have the menu item inserted
        /// </remarks>
        [XmlElement("Included")]
        public List<string> AllIncluded { get; set; }

        /// <summary>
        /// Gets or sets all of the EXCLUDED document type aliases.
        /// </summary>
        /// <remarks>
        /// These are the alias that correspond to the document types that should NOT have the menu item inserted
        /// </remarks>
        [XmlElement("Excluded")]
        public List<string> AllExcluded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to apply alias to parent node.
        /// </summary>
        /// <remarks>
        /// Apply the menu item to all descendent nodes where any of the ancestors of the current node contain the specified aliases
        /// </remarks>
        /// <value>
        /// <c>true</c> if apply alias to parent node; otherwise, <c>false</c>.
        /// </value>
        [XmlElement("ApplyToDescendants")]
        public bool ApplyToDescendants { get; set; }
    }

    /// <summary>
    /// This class provides the types properties associated with a policy
    /// </summary>
    public class PolicyType
    {
        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
        public string TypeName { get; set; }
    }

    /// <summary>
    /// This class provides the container for security policies that should be adhered to before a menu item can be added
    /// </summary>
    public class SecurityPolicies
    {
        /// <summary>
        /// Gets or sets all policies.
        /// </summary>
        /// <value>
        /// All policies.
        /// </value>
        [XmlElement("Policy")]
        public List<PolicyType> AllPolicies { get; set; }

        /// <summary>
        /// Gets the policies.
        /// </summary>
        /// <value>
        /// The policies.
        /// </value>
        [XmlIgnore]
        public List<IPolicy> Policies
        {
            get { return this.AllPolicies.Select(p => (IPolicy)Activator.CreateInstance(p.Assembly, p.TypeName).Unwrap()).ToList(); }
        }
    }

    public interface IPolicy
    {
        /// <summary>
        /// Gets a value indicating whether permission has been provided to the current user.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        bool IsValid { get; }
    }

    public enum Separator
    {
        /// <summary>
        /// Indicates that there should be no separator rendered in the tree for this menu item
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the separator should be rendered before the menu item
        /// </summary>
        Before,

        /// <summary>
        /// Indicates that the separator should be rendered after the menu item
        /// </summary>
        /// <remarks>
        /// In this case we need to find the next menu item in the tree and set its SeparatorBefore flag to true
        /// </remarks>
        After
    }
}