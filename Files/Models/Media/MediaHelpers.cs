using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace ExtendingUmbraco.Models.Media
{
    /// <summary>
    /// Media Helpers
    /// </summary>
    public static class MediaHelpers
    {
        /// <summary>
        /// Builds the image models.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="userService">The user service.</param>
        /// <returns></returns>
        public static IEnumerable<ImageModel> BuildImageModels(this IEnumerable<IMedia> media, IUserService userService)
        {
            return media
                .Where(p => p.ContentType.Alias == MediaTypeAlias.Image)
                .Select(p => new ImageModel(p, userService));
        }

        /// <summary>
        /// Builds the folder models.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="includeChildrenDetails">if set to <c>true</c> [include children details].</param>
        /// <returns></returns>
        public static IEnumerable<UmbracoFolderModel> BuildFolderModels(this ConcurrentBag<IMedia> media, IUserService userService, bool includeChildrenDetails = false)
        {
            return media
                .Where(p => p.ContentType.Alias == MediaTypeAlias.Folder)
                .Select(p => new UmbracoFolderModel(p, userService, includeChildrenDetails));
        }

        /// <summary>
        /// Ares the empty.
        /// </summary>
        /// <param name="folders">The folders.</param>
        /// <returns></returns>
        public static IEnumerable<UmbracoFolderModel> AreEmpty(this IEnumerable<UmbracoFolderModel> folders)
        {
            return folders.Where(p => p.IsEmpty.GetValueOrDefault(true));
        }

        /// <summary>
        /// Adds the folder if contains new images.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="mediaContainer">The media container.</param>
        /// <param name="date">The date.</param>
        public static void AddFolderIfContainsNewImages(this IMedia media, List<IMedia> mediaContainer, Nullable<DateTime> date)
        {
            bool hasAnyNewMedia = media.Children()
                                .Any(p => p.ContentType.Alias == MediaTypeAlias.Image && p.UpdateDate >= date.Value);

            if (hasAnyNewMedia)
            {
                mediaContainer.Add(media);
            }
        }

        /// <summary>
        /// Adds the folder if contains new images.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="mediaContainer">The media container.</param>
        /// <param name="date">The date.</param>
        public static void AddFolderIfContainsNewImages(this IMedia media, ConcurrentBag<IMedia> mediaContainer, Nullable<DateTime> date)
        {
            bool hasAnyNewMedia = media.Children()
                                .Any(p => p.ContentType.Alias == MediaTypeAlias.Image && p.UpdateDate >= date.Value);

            if (hasAnyNewMedia)
            {
                mediaContainer.Add(media);
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="concurrentBag">The concurrent bag.</param>
        /// <param name="collection">The collection.</param>
        public static void AddRange<T>(this ConcurrentBag<T> concurrentBag, IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                concurrentBag.Add(item);
            }
        }
    }
}