using ExtendingUmbraco.Settings;

namespace ExtendingUmbraco.Providers
{
    public class ConfigProvider : AppSettingsConfiguration, IConfigProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigProvider"/> class.
        /// </summary>
        /// <param name="loadSettings">Specify whether to load the settings.</param>
        public ConfigProvider()
        {
            // load the settings
            this.UmbracoCustomSettings = XmlSection<UmbracoCustomSettings>.GetSection("UmbracoCustomSettings");
        }

        public UmbracoCustomSettings UmbracoCustomSettings { get; private set; }
    }
}