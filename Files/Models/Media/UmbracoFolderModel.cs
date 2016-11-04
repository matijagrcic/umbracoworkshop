using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace ExtendingUmbraco.Models.Media
{
    /// <summary>
    /// Umbraco Folder Model
    /// </summary>
    public class UmbracoFolderModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoFolderModel"/> class.
        /// </summary>
        /// <param name="folders">The folders.</param>
        /// <param name="images">The images.</param>
        public UmbracoFolderModel(List<UmbracoFolderModel> folders, List<ImageModel> images)
        {
            Folders = folders;
            Images = images;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoFolderModel" /> class.
        /// </summary>
        /// <param name="media">The image media.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="includeChildrenDetails">if set to <c>true</c> [include children details].</param>
        public UmbracoFolderModel(IMedia media, IUserService userService, bool includeChildrenDetails)
        {
            Images = Enumerable.Empty<ImageModel>().ToList();
            Folders = Enumerable.Empty<UmbracoFolderModel>().ToList();

            PopulateProperties(media, userService, includeChildrenDetails);
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="media">The image media.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="includeChildrenDetails">if set to <c>true</c> [include children details].</param>
        private void PopulateProperties(IMedia media, IUserService userService, bool includeChildrenDetails)
        {
            Id = media.Id;
            Name = media.Name;
            Path = media.Path;
            CreatorName = media.GetCreatorProfile(userService).Name;
            IncludeChildrenDetails = includeChildrenDetails;

            if (includeChildrenDetails)
            {
                NumberOfFolders = media.Children().Where(p => p.ContentType.Alias == MediaTypeAlias.Folder).Count();
                NumberOfImages = media.Children().Where(p => p.ContentType.Alias == MediaTypeAlias.Image).Count();
            }
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
        /// Gets or sets the folders.
        /// </summary>
        /// <value>
        /// The folders.
        /// </value>
        public List<UmbracoFolderModel> Folders { get; set; }

        /// <summary>
        /// Gets or sets the images.
        /// </summary>
        /// <value>
        /// The images.
        /// </value>
        public List<ImageModel> Images { get; set; }

        /// <summary>
        /// Gets the number of images.
        /// </summary>
        /// <value>
        /// The number of images.
        /// </value>
        public int NumberOfImages { get; private set; }

        /// <summary>
        /// Gets the number of folders.
        /// </summary>
        /// <value>
        /// The number of folders.
        /// </value>
        public int NumberOfFolders { get; private set; }

        /// <summary>
        /// Gets the is empty.
        /// </summary>
        /// <value>
        /// The is empty.
        /// </value>
        public Nullable<bool> IsEmpty
        {
            get
            {
                if (!IncludeChildrenDetails)
                {
                    return null;
                }
                return NumberOfImages == 0 && NumberOfFolders == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [include children details].
        /// </summary>
        /// <value>
        /// <c>true</c> if [include children details]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeChildrenDetails { get; private set; }
    }
}