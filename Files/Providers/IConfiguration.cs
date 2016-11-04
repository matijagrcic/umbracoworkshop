namespace ExtendingUmbraco.Providers
{
    public interface IConfiguration
    {
        /// <summary>
        /// Refreshes the configuration.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Get the configuration value given the key name.
        /// </summary>
        /// <param name="name">Name of the Configuration</param>
        /// <returns>Configuration value</returns>
        object GetValue(string name);

        /// <summary>
        /// Get the configuration value given the key name.
        /// </summary>
        /// <typeparam name="T">Type of the Configuration</typeparam>
        /// <param name="name">Name of the Configuration</param>
        /// <returns>Configuration value</returns>
        T GetValue<T>(string name);

        /// <summary>
        /// Get the configuration value given the key name, if it doesn't exist, use the default value.
        /// </summary>
        /// <typeparam name="T">Type of the Configuration</typeparam>
        /// <param name="name">The name of Configuration.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Configuration value</returns>
        T GetValue<T>(string name, T defaultValue);
    }
}