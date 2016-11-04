namespace ExtendingUmbraco.Providers
{
    public class AppSettings
    {
        /// <summary>
        /// Gets the app setting.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>The app setting</returns>
        public static object GetAppSetting(string settingName)
        {
            return GetAppSetting<object>(settingName, null, false);
        }

        /// <summary>
        /// Gets the app setting.
        /// </summary>
        /// <typeparam name="T">Type of setting</typeparam>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>The app setting</returns>
        public static T GetAppSetting<T>(string settingName)
        {
            return GetAppSetting(settingName, default(T), false);
        }

        /// <summary>
        /// Gets the app setting.
        /// </summary>
        /// <typeparam name="T">Type of setting</typeparam>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The app setting</returns>
        public static T GetAppSetting<T>(string settingName, T defaultValue)
        {
            return GetAppSetting(settingName, defaultValue, true);
        }

        /// <summary>
        /// Gets the app setting.
        /// </summary>
        /// <typeparam name="T">Type of setting</typeparam>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="useDefaultValue">if set to <c>true</c> [use default value].</param>
        /// <returns>The app setting</returns>
        public static T GetAppSetting<T>(string settingName, T defaultValue, bool useDefaultValue)
        {
            return useDefaultValue ? ConfigurationUtils.GetAppSetting(settingName, defaultValue) : ConfigurationUtils.GetAppSetting<T>(settingName);
        }
    }
}