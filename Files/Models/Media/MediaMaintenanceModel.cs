using ExtendingUmbraco.Settings;

namespace ExtendingUmbraco.Models.Media
{
    /// <summary>
    /// Media Maintenance Model
    /// </summary>
    public class MediaMaintenanceModel
    {
        /// <summary>
        /// Gets the plugin.
        /// </summary>
        /// <value>
        /// The plugin.
        /// </value>
        public Plugin Plugin { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaMaintenanceModel"/> class.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        public MediaMaintenanceModel(Plugin plugin)
        {
            this.Plugin = plugin;
        }
    }
}