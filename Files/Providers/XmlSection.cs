using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace ExtendingUmbraco.Providers
{
    public class XmlSection<T> : ConfigurationSection where T : class
    {
        /// <summary>
        /// Reference to the serializer
        /// </summary>
        private XmlSerializer serializer;

        /// <summary>
        /// Reference to the configurationItem
        /// </summary>
        private T configurationItem;

        /// <summary>
        /// Retrieves the specified XML section for the current application's default
        /// configuration
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>The section</returns>
        public static T GetSection(string sectionName)
        {
            var section = (XmlSection<T>)ConfigurationManager.GetSection(sectionName);
            if (null == section)
            {
                return null;
            }

            return section.configurationItem;
        }

        /// <summary>
        /// Retrieves the specified XML section for the specified System.Configuration.Configuration object
        /// </summary>
        /// <param name="sectionName">The name of the section</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The section</returns>
        public static T GetSection(string sectionName, Configuration configuration)
        {
            var section = (XmlSection<T>)configuration.GetSection(sectionName);
            if (null == section)
            {
                return null;
            }

            return section.configurationItem;
        }

        /// <summary>
        /// Sets the <see cref="T:System.Configuration.ConfigurationElement"/> object to its initial state.
        /// </summary>
        protected override void Init()
        {
            base.Init();

            var root = new XmlRootAttribute(SectionInformation.Name);
            this.serializer = new XmlSerializer(typeof(T), root);
        }

        /// <summary>
        /// Deseializes the XML section
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> object, which reads from the configuration file.</param>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// <paramref name="reader"/> found no elements in the configuration file.</exception>
        protected override void DeserializeSection(XmlReader reader)
        {
            this.configurationItem = (T)this.serializer.Deserialize(reader);
        }
    }
}