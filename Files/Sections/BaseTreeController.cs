using ExtendingUmbraco.Providers;
using ExtendingUmbraco.Settings;
using System;
using System.Linq;
using System.Net.Http.Formatting;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Security;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;
using Constants = Umbraco.Core.Constants;

namespace ExtendingUmbraco.Sections
{
    public abstract class BaseTreeController : TreeController
    {
        /// <summary>
        /// Gets the tree nodes.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="queryStrings">The query strings.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The tree node collection</returns>
        protected TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings, TreeConfiguration configuration)
        {

            // check if we're rendering the root node's children
            if (id == Constants.System.Root.ToString())
            {
                // instantiate a node tree node collection
                var nodes = new TreeNodeCollection();

                if (UserHasAllowedUserRole(configuration))
                {
                    // create a new tree node item
                    var item = this.CreateTreeNode(configuration.UrlName, id, queryStrings, configuration.NodeCaption, configuration.NodeIcon, false);

                    // add to the collection
                    nodes.Add(item);
                }

                // return
                return nodes;
            }

            // this tree doesn't support rendering more than 1 level
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns the menu structure for the node
        /// </summary>
        /// <param name="id">The node id</param>
        /// <param name="queryStrings">All of the query string parameters passed from jsTree</param>
        /// <returns>The custom menu item collection</returns>
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();

            // todo: any custom menu items if any should be added via config using the MenuItemBuilder
            return menu;
        }

        /// <summary>
        /// Users the has allowed user role.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        private bool UserHasAllowedUserRole(TreeConfiguration configuration)
        {
            var plugin = GetPluginInformation(configuration);

            return plugin.UserRoles.All.Any(p => p.Name.Contains(CurrentUser.UserType.Alias));
        }

        /// <summary>
        /// Gets the plugin information.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        private Plugin GetPluginInformation(TreeConfiguration configuration)
        {
            return this.ConfigurationProvider.UmbracoCustomSettings.Plugins.All
                             .SingleOrDefault(p => p.Controller == configuration.ControllerName);
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        private IUser CurrentUser
        {
            get
            {
                var umbracoAuthTicket = new System.Web.HttpContextWrapper(System.Web.HttpContext.Current).GetUmbracoAuthTicket();
                var currentUser = ApplicationContext.Services.UserService.GetByUsername(umbracoAuthTicket.Name);
                return currentUser;
            }
        }

        /// <summary>
        /// Gets the configuration provider.
        /// </summary>
        /// <value>
        /// The configuration provider.
        /// </value>
        private IConfigProvider ConfigurationProvider
        {
            get
            {
                return new ConfigProvider();
            }
        }
    }
}