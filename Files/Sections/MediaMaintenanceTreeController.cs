using ExtendingUmbraco.Sections.Attributes;
using System.Net.Http.Formatting;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;

namespace ExtendingUmbraco.Sections
{
    [PluginController("utilities")]
    [Umbraco.Web.Trees.Tree("utilities", "mediaMaintenance", "Media")]
    public class MediaMaintenanceTreeController : BaseTreeController
    {
        /// <summary>
        /// The method called to render the contents of the tree structure
        /// </summary>
        /// <param name="id">The node id</param>
        /// <param name="queryStrings">All of the query string parameters passed from jsTree</param>
        /// <returns>The tree node collection</returns>
        /// <remarks>
        /// We are allowing an arbitrary number of query strings to be passed in so that developers are able to persist custom data from the front-end
        /// to the back end to be used in the query for model data.
        /// </remarks>
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            return this.GetTreeNodes(id, queryStrings, MediaMaintenanceTreeAttribute.Configuration);
        }
    }
}