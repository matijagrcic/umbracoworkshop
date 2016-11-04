using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace ExtendingUmbraco.Models.Media
{
    /// <summary>
    /// Media Maintenance
    /// </summary>
    public class MediaMaintenance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaMaintenance" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="mediaService">The media service.</param>
        public MediaMaintenance(IUserService userService, IMediaService mediaService)
        {
            UserService = userService;
            MediaService = mediaService;
        }

        /// <summary>
        /// Gets or sets the user service.
        /// </summary>
        /// <value>
        /// The user service.
        /// </value>
        private IUserService UserService { get; set; }

        /// <summary>
        /// Gets or sets the media service.
        /// </summary>
        /// <value>
        /// The media service.
        /// </value>
        private IMediaService MediaService { get; set; }


        /// <summary>
        /// Gets the folder structure.
        /// </summary>
        /// <returns></returns>
        public UmbracoFolderModel GetFolderStructure()
        {
            var rootMedia = GetRootMedia();

            var concurrentRootMedia = new ConcurrentBag<IMedia>(rootMedia);

            var rootFolder = BuildRootFolder(concurrentRootMedia);
            BuildFolderStructure(rootFolder, true);

            return rootFolder;
        }

        /// <summary>
        /// Gets all umbraco folders.
        /// </summary>
        /// <param name="includeChildrenDetails">if set to <c>true</c> [include children details].</param>
        /// <param name="lastNDays">The last n days.</param>
        /// <returns></returns>
        public IEnumerable<UmbracoFolderModel> GetAllUmbracoFolders(bool includeChildrenDetails = false, int lastNDays = 0)
        {
            Nullable<DateTime> compareDate = null;
            if (lastNDays > 0)
            {
                compareDate = DateTime.UtcNow.Add(-TimeSpan.FromDays(lastNDays));
            }

            return GetAllContentByMediaTypeAlias(MediaTypeAlias.Folder, compareDate)
                .BuildFolderModels(UserService, includeChildrenDetails);
        }

        /// <summary>
        /// Gets all images.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ImageModel> GetAllImages()
        {
            return GetAllContentByMediaTypeAlias(MediaTypeAlias.Image)
                .BuildImageModels(UserService);
        }

        /// <summary>
        /// Gets all image node references.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ImageReferenceModel> GetAllImageNodeReferences()
        {
            return umbraco.uQuery.GetNodesByType("mediaNode")
                .Select(p => new ImageReferenceModel(p));
        }

        /// <summary>
        /// Gets all image node references by identifier.
        /// </summary>
        /// <param name="mediaId">The media identifier.</param>
        /// <returns></returns>
        public List<ImageReferenceModel> GetAllImageNodeReferencesById(int mediaId)
        {
            var imageNodeReferences = GetAllImageNodeReferences();
            return imageNodeReferences.Where(p => p.ImageId == mediaId).ToList();
        }


        /// <summary>
        /// Determines whether the specified purpose is used.
        /// </summary>
        /// <param name="purpose">The purpose.</param>
        /// <returns></returns>
        private bool IsUsed(string purpose)
        {
            return (purpose == MediaTypePurpose.Used);
        }

        /// <summary>
        /// Gets the images by purpose.
        /// </summary>
        /// <param name="purpose">The purpose.</param>
        /// <returns></returns>
        public List<ImageModel> GetImagesByPurpose(string purpose)
        {
            bool isUsed = IsUsed(purpose);
            var images = GetAllImages().ToList();
            var imageNodeIds = GetAllImageNodeReferences().Select(p => p.ImageId).ToList();

            return images.Where(p => imageNodeIds.Contains(p.Id) == isUsed).ToList();
        }

        /// <summary>
        /// Gets the media references by puprose.
        /// </summary>
        /// <param name="purpose">The purpose.</param>
        /// <returns></returns>
        public List<ImageReferenceModel> GetMediaReferencesByPuprose(string purpose)
        {
            bool isUsed = IsUsed(purpose);
            var imageIds = GetAllImages().Select(p => p.Id).ToList();
            var imageNodes = GetAllImageNodeReferences().ToList();

            return imageNodes.Where(p => imageIds.Contains(p.ImageId) == isUsed).ToList();
        }

        /// <summary>
        /// Gets all content by media type alias.
        /// </summary>
        /// <param name="mediaTypeAlias">The media type alias.</param>
        /// <returns></returns>
        private ConcurrentBag<IMedia> GetAllContentByMediaTypeAlias(string mediaTypeAlias, Nullable<DateTime> compareDate = null)
        {
            var rootMedia = GetRootMedia();

            var rootFolders = new ConcurrentBag<IMedia>(rootMedia.Where(p => p.ContentType.Alias == MediaTypeAlias.Folder));

            var concurrentMediaFiles = new ConcurrentBag<IMedia>();
            if (mediaTypeAlias == MediaTypeAlias.Image)
            {
                concurrentMediaFiles.AddRange(rootMedia.Where(p => p.ContentType.Alias == mediaTypeAlias));
            }
            BuilFlatContentList(concurrentMediaFiles, rootFolders, mediaTypeAlias, compareDate);

            return concurrentMediaFiles;
        }

        /// <summary>
        /// Buils the flat content list.
        /// </summary>
        /// <param name="mediaContainer">The media container.</param>
        /// <param name="folders">The folders.</param>
        /// <param name="mediaTypeAlias">The media type alias.</param>
        private void BuilFlatContentList(ConcurrentBag<IMedia> concurrentMediaContainer, ConcurrentBag<IMedia> concurrentFolders, string mediaTypeAlias, Nullable<DateTime> compareDate = null)
        {
            Parallel.ForEach(concurrentFolders, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 * 10 }, (concurrentFolder) =>
            {
                switch (mediaTypeAlias)
                {
                    case MediaTypeAlias.Folder:
                        if (compareDate.HasValue)
                        {
                            concurrentFolder.AddFolderIfContainsNewImages(concurrentMediaContainer, compareDate);
                        }
                        else
                        {
                            concurrentMediaContainer.Add(concurrentFolder);
                        }
                        break;
                    case MediaTypeAlias.Image:
                        concurrentMediaContainer.AddRange(concurrentFolder.Children().Where(p => p.ContentType.Alias == mediaTypeAlias));
                        break;
                }
                var folderMedia = new ConcurrentBag<IMedia>(concurrentFolder.Children().Where(p => p.ContentType.Alias == MediaTypeAlias.Folder));
                BuilFlatContentList(concurrentMediaContainer, folderMedia, mediaTypeAlias, compareDate);
            });
        }


        /// <summary>
        /// Builds the root folder.
        /// </summary>
        /// <param name="rootMedia">The root media.</param>
        /// <returns></returns>
        private UmbracoFolderModel BuildRootFolder(ConcurrentBag<IMedia> rootMedia)
        {
            var images = rootMedia.BuildImageModels(UserService);

            var folders = rootMedia.BuildFolderModels(UserService);

            var rootFolder = new UmbracoFolderModel(folders.ToList(), images.ToList());
            return rootFolder;
        }

        /// <summary>
        /// Gets the root media.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IMedia> GetRootMedia()
        {
            var rootMedia = MediaService.GetRootMedia();
            return rootMedia;
        }

        /// <summary>
        /// Builds the folder structure.
        /// </summary>
        /// <param name="currentFolder">The current folder.</param>
        /// <param name="isRootFolder">if set to <c>true</c> [is root folder].</param>
        private void BuildFolderStructure(UmbracoFolderModel currentFolder, bool isRootFolder = false)
        {
            if (!isRootFolder)
            {
                var currentFolderChildren = GetFolderChildren(currentFolder);
                var concurrentFolderChildren = new ConcurrentBag<IMedia>(currentFolderChildren);

                PopulateFolderWithFolders(currentFolder, concurrentFolderChildren);
                PopulateFolderWithImages(currentFolder, currentFolderChildren);
            }

            foreach (var folder in currentFolder.Folders)
            {
                BuildFolderStructure(folder);
            }
        }

        /// <summary>
        /// Populates the folder with images.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="children">The children.</param>
        private void PopulateFolderWithImages(UmbracoFolderModel folder, IEnumerable<IMedia> children)
        {
            if (children.Any(p => p.ContentType.Alias == MediaTypeAlias.Image))
            {
                var childrenImages = children.BuildImageModels(UserService);
                folder.Images.AddRange(childrenImages);
            }
        }

        /// <summary>
        /// Populates the folder with folders.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="children">The children.</param>
        private void PopulateFolderWithFolders(UmbracoFolderModel folder, ConcurrentBag<IMedia> children)
        {
            if (children.Any(p => p.ContentType.Alias == MediaTypeAlias.Folder))
            {
                var childrenFolders = children.BuildFolderModels(UserService);
                folder.Folders.AddRange(childrenFolders);
            }
        }

        /// <summary>
        /// Gets the folder children.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        private IEnumerable<IMedia> GetFolderChildren(UmbracoFolderModel folder)
        {
            var children = MediaService.GetChildren(folder.Id);
            return children;
        }

        /// <summary>
        /// Gets the virtual directory information.
        /// </summary>
        /// <param name="virtualPathName">Name of the virtual path.</param>
        /// <returns></returns>
        private DirectoryInfo GetVirtualDirectoryInfo(string virtualPathName = "~/media")
        {
            var virtualDirectoryPath = System.Web.Hosting.HostingEnvironment.MapPath(virtualPathName);
            var directoryInfo = new DirectoryInfo(virtualDirectoryPath);
            return directoryInfo;
        }

        /// <summary>
        /// Gets the empty umbraco folders.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UmbracoFolderModel> GetEmptyUmbracoFolders()
        {
            var emptyFolders = GetAllUmbracoFolders(includeChildrenDetails: true).AreEmpty();
            return emptyFolders;
        }


        /// <summary>
        /// Deletes the empty umbraco folders.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        public void DeleteEmptyUmbracoFolders(IMediaService mediaService)
        {
            var emptyFolders = GetEmptyUmbracoFolders();
            foreach (var emptyFolder in emptyFolders)
            {
                var folderContent = mediaService.GetById(emptyFolder.Id);
                mediaService.Delete(folderContent);
            }
        }


        /// <summary>
        /// Gets all physical folders.
        /// </summary>
        /// <returns></returns>
        public List<PhysicalFolder> GetAllPhysicalFolders()
        {
            var directories = GetDirectories(GetVirtualDirectoryInfo(), new List<PhysicalFolder>());
            return directories;
        }

        /// <summary>
        /// Gets the empty physical folders.
        /// </summary>
        /// <returns></returns>
        public List<PhysicalFolder> GetEmptyPhysicalFolders()
        {
            var directories = GetDirectories(GetVirtualDirectoryInfo(), new List<PhysicalFolder>(), onlyEmptyDirectories: true);
            return directories;
        }



        /// <summary>
        /// Adds the physical folder.
        /// </summary>
        /// <param name="directories">The directories.</param>
        /// <param name="currentDirectory">The current directory.</param>
        private void AddPhysicalFolder(List<PhysicalFolder> directories, DirectoryInfo currentDirectory, bool isEmpty = false)
        {
            directories.Add(new PhysicalFolder
            {
                Path = currentDirectory.FullName,
                CreationTime = currentDirectory.CreationTimeUtc,
                LastAccessTime = currentDirectory.LastAccessTimeUtc,
                LastWriteTime = currentDirectory.LastWriteTimeUtc,
                Name = currentDirectory.Name,
                IsEmpty = isEmpty
            });
        }

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <param name="currentDirectory">The current directory.</param>
        /// <param name="directories">The directories.</param>
        /// <param name="onlyEmptyDirectories">if set to <c>true</c> [only empty directories].</param>
        /// <returns></returns>
        private List<PhysicalFolder> GetDirectories(DirectoryInfo currentDirectory, List<PhysicalFolder> directories, bool onlyEmptyDirectories = false)
        {
            foreach (DirectoryInfo directoryInfo in currentDirectory.EnumerateDirectories())
            {
                GetDirectories(directoryInfo, directories, onlyEmptyDirectories);
            }

            if (onlyEmptyDirectories)
            {
                if (!currentDirectory.EnumerateFiles().Any())
                {
                    AddPhysicalFolder(directories, currentDirectory, true);
                }
            }
            else
            {
                if (!currentDirectory.EnumerateFiles().Any())
                {
                    AddPhysicalFolder(directories, currentDirectory, true);
                }
                else
                {
                    AddPhysicalFolder(directories, currentDirectory);
                }

            }

            return directories;
        }

        /// <summary>
        /// Deletes the empty physical folders.
        /// </summary>
        public void DeleteEmptyPhysicalFolders()
        {
            var emptyDirectories = GetEmptyPhysicalFolders();
            foreach (var emptyDirectory in emptyDirectories)
            {
                Directory.Delete(emptyDirectory.Path);
            }
        }

    }
}