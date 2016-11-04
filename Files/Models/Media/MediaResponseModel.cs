using System.Collections.Generic;
using System.Linq;

namespace ExtendingUmbraco.Models.Media
{
    /// <summary>
    /// Media Response Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MediaResponseModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaResponseModel{T}"/> class.
        /// </summary>
        /// <param name="mediaFiles">The media files.</param>
        public MediaResponseModel(IEnumerable<T> mediaFiles)
        {
            MediaFiles = mediaFiles;
        }

        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public int Total { get { return MediaFiles.Count(); } }

        /// <summary>
        /// Gets or sets the media files.
        /// </summary>
        /// <value>
        /// The media files.
        /// </value>
        public IEnumerable<T> MediaFiles { get; set; }

    }
}