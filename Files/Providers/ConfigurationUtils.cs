using System;
using System.ComponentModel;
using System.Configuration;

namespace ExtendingUmbraco.Providers
{
    public class ConfigurationUtils
    {
        /// <summary>
        /// Refereshes the app settings.
        /// </summary>
        public static void RefereshAppSettings()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Gets the value from the applications appSettings. This will error if the appSetting is not defined.
        /// </summary>
        /// <typeparam name="T">Type of setting</typeparam>
        /// <param name="appSettingName">Name of the appSettings key.</param>
        /// <returns>The app setting</returns>
        public static T GetAppSetting<T>(string appSettingName)
        {
            return GetAppSettingInternal(appSettingName, false, default(T));
        }

        /// <summary>
        /// Gets the value from the applications appSettings.
        /// </summary>
        /// <typeparam name="T">Type of setting</typeparam>
        /// <param name="appSettingName">Name of the appSettings key.</param>
        /// <param name="defaultValue">The default value returned if the appSetting has not been defined.</param>
        /// <returns>The app setting</returns>
        public static T GetAppSetting<T>(string appSettingName, T defaultValue)
        {
            return GetAppSettingInternal(appSettingName, true, defaultValue);
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns>The connection string</returns>
        public static string GetConnectionString(string connectionStringName)
        {
            if (connectionStringName == null)
            {
                throw new ArgumentNullException("connectionStringName");
            }

            var settings = ConfigurationManager.ConnectionStrings[connectionStringName];

            if (settings == null)
            {
                throw new Exception(string.Format("No connection string settings with the name '{0}'.", connectionStringName));
            }

            return settings.ConnectionString;
        }

        /// <summary>
        /// Gets the app setting internal.
        /// </summary>
        /// <typeparam name="T">Type of setting</typeparam>
        /// <param name="appSettingName">Name of the app setting.</param>
        /// <param name="useDefaultOnUndefined">if set to <c>true</c> [use default on undefined].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The required app setting</returns>
        private static T GetAppSettingInternal<T>(string appSettingName, bool useDefaultOnUndefined, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings[appSettingName];
            var type = typeof(T);

            // require that all appSettings are either defined or have explicitly been given a default value
            // if the type of T is nullable and the value is whitespace then return the default value
            if (value == null || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && string.IsNullOrWhiteSpace(value)))
            {
                if (useDefaultOnUndefined)
                {
                    return defaultValue;
                }

                throw new Exception(string.Format("{0} not defined in appSettings.", appSettingName));
            }

            var baseType = type.BaseType;
            if (baseType != null && (baseType.FullName != null && baseType.FullName.Equals("System.Enum")))
            {
                return (T)Enum.Parse(typeof(T), value);
            }

            var conv = TypeDescriptor.GetConverter(typeof(T));
            return (T)conv.ConvertFrom(value);
        }
    }
}