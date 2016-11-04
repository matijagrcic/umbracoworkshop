using ExtendingUmbraco.Settings;

namespace ExtendingUmbraco.Providers
{
    public interface IConfigProvider : IConfiguration
    {
        /// <summary>
        /// Gets the custom configuration settings.
        /// </summary>
        /// <value>
        /// The custom configuration settings.
        /// </value>
        UmbracoCustomSettings UmbracoCustomSettings { get; }
    }
}