using umbraco.businesslogic;

namespace ExtendingUmbraco.Sections.Attributes
{
    public class MediaMaintenanceTreeAttribute : TreeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaMaintenanceTreeAttribute"/> class.
        /// </summary>
        public MediaMaintenanceTreeAttribute()
            : base(Configuration.Application, Configuration.Alias, Configuration.Caption, Configuration.Icon, Configuration.OpenIcon)
        {
        }

        /// <summary>
        /// Gets the media tree configuration
        /// </summary>
        public static TreeConfiguration Configuration
        {
            get
            {
                return new TreeConfiguration(
                   application: "utilities",
                   icon: "icon-umb-media",
                   openColor: "color-blue",
                   alias: "mediaMaintenance",
                   caption: "Media",
                   urlName: "dashboard",
                   nodeCaption: "Maintenance",
                   nodeColor: "color-blue",
                   controllerName: "MediaMaintenanceController"
               );
            }
        }
    }
}