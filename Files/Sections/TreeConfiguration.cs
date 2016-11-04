namespace ExtendingUmbraco.Sections
{
    public class TreeConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeConfiguration" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="openColor">Color of the open.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="urlName">Name of the URL.</param>
        /// <param name="nodeCaption">The node caption.</param>
        /// <param name="nodeColor">Color of the node.</param>
        /// <param name="controllerName">Name of the controller.</param>
        public TreeConfiguration(string application, string icon, string openColor, string alias, string caption, string urlName, string nodeCaption, string nodeColor, string controllerName)
        {
            this.Application = application;
            this.Icon = icon;
            this.OpenColor = openColor;
            this.Alias = alias;
            this.Caption = caption;
            this.UrlName = urlName;
            this.NodeCaption = nodeCaption;
            this.NodeColor = nodeColor;
            this.ControllerName = controllerName;
        }

        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets the icon
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        /// <remarks>
        /// See http://nicbell.github.io/ucreate/icons.html for icons
        /// </remarks>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the open icon color.
        /// </summary>
        /// <value>
        /// The open icon color
        /// </value>
        public string OpenColor { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the url name or id.
        /// </summary>
        /// <value>
        /// The url name or id.
        /// </value>
        public string UrlName { get; set; }

        /// <summary>
        /// Gets or sets the node caption.
        /// </summary>
        /// <value>
        /// The node caption.
        /// </value>
        public string NodeCaption { get; set; }

        /// <summary>
        /// Gets or sets the node icon color.
        /// </summary>
        /// <value>
        /// The node icon color.
        /// </value>
        public string NodeColor { get; set; }

        /// <summary>
        /// Gets the open icon.
        /// </summary>
        /// <value>
        /// The open icon.
        /// </value>
        public string OpenIcon
        {
            get { return string.Format("{0} {1}", this.Icon, this.OpenColor); }
        }

        /// <summary>
        /// Gets the node icon.
        /// </summary>
        /// <value>
        /// The node icon.
        /// </value>
        public string NodeIcon
        {
            get { return string.Format("{0} {1}", this.Icon, this.NodeColor); }
        }

        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        public string ControllerName { get; set; }
    }
}