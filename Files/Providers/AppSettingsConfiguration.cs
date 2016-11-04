namespace ExtendingUmbraco.Providers
{
    public class AppSettingsConfiguration : IConfiguration
    {
        /// <summary>
        /// Refreshes the configuration.
        /// </summary>
        public void Refresh()
        {
            ConfigurationUtils.RefereshAppSettings();
        }

        /// <summary>
        /// Get the configuration value given the key name.
        /// </summary>
        /// <param name="name">Name of the Configuration</param>
        /// <returns>Configuration value</returns>
        public object GetValue(string name)
        {
            return AppSettings.GetAppSetting<object>(name);
        }

        /// <summary>
        /// Get the configuration value given the key name.
        /// </summary>
        /// <typeparam name="T">Type of the Configuration</typeparam>
        /// <param name="name">Name of the Configuration</param>
        /// <returns>Configuration value</returns>
        public T GetValue<T>(string name)
        {
            return AppSettings.GetAppSetting<T>(name);
        }

        /// <summary>
        /// Get the configuration value given the key name, if it doesn't exist, use the default value.
        /// </summary>
        /// <typeparam name="T">Type of the Configuration</typeparam>
        /// <param name="name">The name of Configuration.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Configuration value</returns>
        public T GetValue<T>(string name, T defaultValue)
        {
            return AppSettings.GetAppSetting(name, defaultValue);
        }
    }
}