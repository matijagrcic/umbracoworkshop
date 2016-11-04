using System;
using System.Linq;
using umbraco.interfaces;

namespace ExtendingUmbraco.Models.Media
{
    /// <summary>
    /// Image Reference Model
    /// </summary>
    public class ImageReferenceModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageModel" /> class.
        /// </summary>
        /// <param name="imageNode">The image node.</param>
        public ImageReferenceModel(INode imageNode)
        {
            PopulateProperties(imageNode);
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="imageNode">The image node.</param>
        private void PopulateProperties(INode imageNode)
        {
            NodeId = imageNode.Id;
            NodeName = imageNode.Name;
            Path = imageNode.Path;
            CreatorName = imageNode.CreatorName;
            CreateDate = imageNode.CreateDate;
            UpdateDate = imageNode.UpdateDate;

            ImageId = GetImageId(imageNode);
            AltText = GetPropertyByAlias(imageNode, ImageReferenceAlias.altText);
            Url = GetPropertyByAlias(imageNode, ImageReferenceAlias.url);
        }

        /// <summary>
        /// Sets the image identifier.
        /// </summary>
        /// <param name="imageNode">The image node.</param>
        private int GetImageId(INode imageNode)
        {
            int imageId;
            if (int.TryParse(GetPropertyByAlias(imageNode, ImageReferenceAlias.image), out imageId))
            {
                return imageId;
            }
            return default(int);
        }

        /// <summary>
        /// Gets the property by alias.
        /// </summary>
        /// <param name="imageNode">The image node.</param>
        /// <param name="propertyAlias">The property alias.</param>
        /// <returns></returns>
        private string GetPropertyByAlias(INode imageNode, string propertyAlias)
        {
            string value = string.Empty;
            var property = imageNode.PropertiesAsList.FirstOrDefault(p => p.Alias == propertyAlias);
            if (property != null)
            {
                value = property.Value;
            }
            return value;
        }


        /// <summary>
        /// Gets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int NodeId { get; private set; }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public string NodeName { get; private set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; private set; }

        /// <summary>
        /// Gets or sets the name of the creator.
        /// </summary>
        /// <value>
        /// The name of the creator.
        /// </value>
        public string CreatorName { get; private set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>
        /// The create date.
        /// </value>
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>
        /// The update date.
        /// </value>
        public DateTime UpdateDate { get; private set; }

        /// <summary>
        /// Gets the image identifier.
        /// </summary>
        /// <value>
        /// The image identifier.
        /// </value>
        public int ImageId { get; private set; }

        /// <summary>
        /// Gets the alt text.
        /// </summary>
        /// <value>
        /// The alt text.
        /// </value>
        public string AltText { get; private set; }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; private set; }
    }
}