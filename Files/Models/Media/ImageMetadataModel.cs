using Umbraco.Core.Models;
using Newtonsoft.Json;

namespace ExtendingUmbraco.Models.Media
{
    /// <summary>
    /// Image Metadata Model
    /// </summary>
    public class ImageMetadataModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageMetadataModel"/> class.
        /// </summary>
        /// <param name="imageProperties">The image properties.</param>
        public ImageMetadataModel(PropertyCollection imageProperties)
        {
            ExtractImageMetadata(imageProperties);
        }

        /// <summary>
        /// Extracts the image metadata.
        /// </summary>
        /// <param name="imageProperties">The image properties.</param>
        private void ExtractImageMetadata(PropertyCollection imageProperties)
        {
            foreach (var property in imageProperties)
            {
                switch (property.Alias)
                {
                    case ImagePropertyAlias.umbracoFile:
                        var umbracoFile = JsonConvert.DeserializeObject<ExtendingUmbraco.Models.Media.UmbracoFile>((string)property.Value);
                        FilePath = umbracoFile.Src;
                        break;
                    case ImagePropertyAlias.umbracoExtension:
                        Extension = (string)property.Value;
                        break;
                    case ImagePropertyAlias.umbracoBytes:
                        Bytes = (string)property.Value;
                        break;
                    case ImagePropertyAlias.umbracoHeight:
                        Height = (string)property.Value;
                        break;
                    case ImagePropertyAlias.umbracoWidth:
                        Width = (string)property.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public string Extension { get; private set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public string Width { get; private set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public string Height { get; private set; }

        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        public string Bytes { get; private set; }

        /// <summary>
        /// Gets the megabytes.
        /// </summary>
        /// <value>
        /// The megabytes.
        /// </value>
        public string Megabytes
        {
            get
            {
                long bytes;
                if (long.TryParse(Bytes, out bytes))
                {
                    float sizeInMb = (long.Parse(Bytes) / 1024f) / 1024f;
                    return string.Format("{0} MB", sizeInMb.ToString("0.00"));
                }
                return string.Empty;
            }
        }

    }
}