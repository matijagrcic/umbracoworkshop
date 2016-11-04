using System;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace ExtendingUmbraco.Models.Media
{
    /// <summary>
    /// Image Model
    /// </summary>
    public class ImageModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageModel"/> class.
        /// </summary>
        /// <param name="imageMedia">The image media.</param>
        public ImageModel(IMedia imageMedia, IUserService userService)
        {
            PopulateProperties(imageMedia, userService);
            Metadata = new ImageMetadataModel(imageMedia.Properties);

        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="imageMedia">The image media.</param>
        /// <param name="userService">The user service.</param>
        private void PopulateProperties(IMedia imageMedia, IUserService userService)
        {
            Id = imageMedia.Id;
            Name = imageMedia.Name;
            Path = imageMedia.Path;
            CreatorName = imageMedia.GetCreatorProfile(userService).Name;
            CreateDate = imageMedia.CreateDate;
            UpdateDate = imageMedia.UpdateDate;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

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
        /// Gets the medatada.
        /// </summary>
        /// <value>
        /// The medatada.
        /// </value>
        public ImageMetadataModel Metadata { get; private set; }
    }
}