using ExtendingUmbraco.Models;
using ExtendingUmbraco.Models.Media;
using ExtendingUmbraco.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace ExtendingUmbraco.Controllers
{
    [RoutePrefix("media")]
    public class MediaApiController : UmbracoApiController
    {
        private MediaMaintenance MediaMaintenance;

        public MediaApiController()
        {
            MediaMaintenance = new MediaMaintenance(Services.UserService, Services.MediaService);
        }

        [HttpGet]
        [Route("tabs")]
        public MediaMaintenanceModel GetMediaMaintenanceModel()
        {
            Settings.Plugin plugin = new ConfigProvider().UmbracoCustomSettings.Plugins.All
                             .SingleOrDefault(p => p.Controller == SectionControllers.MediaMaintenanceController);

            var mediaMaintenanceModel = new MediaMaintenanceModel(plugin);

            return mediaMaintenanceModel;
        }

        /// <summary>
        /// Gets the media folder structure.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("folder/structure")]
        public IHttpActionResult GetMediaFolderStructure()
        {
            var folderStructure = MediaMaintenance.GetFolderStructure();
            return Ok(folderStructure);
        }

        /// <summary>
        /// Gets all images.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("images/all")]
        public IHttpActionResult GetAllImages()
        {
            var images = MediaMaintenance.GetAllImages().OrderByDescending(p => p.CreateDate);
            return Ok(new MediaResponseModel<ImageModel>(images));
        }

        /// <summary>
        /// Gets all images by puprose.
        /// </summary>
        /// <param name="purpose">The purpose.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("images/purpose/{purpose?}")]
        public IHttpActionResult GetAllImagesByPurpose(string purpose = MediaTypePurpose.UnUsed)
        {
            var images = MediaMaintenance.GetImagesByPurpose(purpose);
            return Ok(new MediaResponseModel<ImageModel>(images));
        }

        /// <summary>
        /// Gets the type of all folders by.
        /// </summary>
        /// <param name="folderType">Type of the folder.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("folders/folderType/{folderType}/all")]
        public IHttpActionResult GetAllFoldersByType(string folderType)
        {
            IHttpActionResult result = BadRequest();
            switch (folderType)
            {
                case FolderType.Umbraco:
                    var umbracofolders = MediaMaintenance.GetAllUmbracoFolders();
                    result = Ok(new MediaResponseModel<UmbracoFolderModel>(umbracofolders));
                    break;
                case FolderType.Physical:
                    var emptyPhysicalFolders = MediaMaintenance.GetAllPhysicalFolders();
                    result = Ok(new MediaResponseModel<PhysicalFolder>(emptyPhysicalFolders));
                    break;
            }
            return result;
        }

        [HttpGet]
        [Route("folders/folderType/{folderType}/empty")]
        public IHttpActionResult GetAllEmptyFoldersByType(string folderType)
        {
            IHttpActionResult result = BadRequest();
            switch (folderType)
            {
                case FolderType.Umbraco:
                    var emptyUmbracoFolders = MediaMaintenance.GetEmptyUmbracoFolders();
                    result = Ok(new MediaResponseModel<UmbracoFolderModel>(emptyUmbracoFolders));
                    break;
                case FolderType.Physical:
                    var emptyPhysicalFolders = MediaMaintenance.GetEmptyPhysicalFolders();
                    result = Ok(new MediaResponseModel<PhysicalFolder>(emptyPhysicalFolders));
                    break;
            }
            return result;
        }

        [HttpDelete]
        [Route("folders/folderType/{folderType}/empty")]
        public IHttpActionResult DeleteAllEmptyFoldersByType(string folderType)
        {
            IHttpActionResult result = BadRequest();
            switch (folderType)
            {
                case FolderType.Umbraco:
                    MediaMaintenance.DeleteEmptyUmbracoFolders(this.ApplicationContext.Services.MediaService);
                    result = Ok();
                    break;
                case FolderType.Physical:
                    MediaMaintenance.DeleteEmptyPhysicalFolders();
                    result = Ok();
                    break;
            }
            return result;
        }

        /// <summary>
        /// Gets the folders with files added in last n days.
        /// </summary>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("folders/old/days/{days}")]
        public IHttpActionResult GetFoldersWithFilesAddedInLastNDays(int days)
        {
            var folders = MediaMaintenance.GetAllUmbracoFolders(lastNDays: days);
            return Ok(new MediaResponseModel<UmbracoFolderModel>(folders));
        }

        /// <summary>
        /// Gets the image references by identifier.
        /// </summary>
        /// <param name="imageId">The image identifier.</param>
        /// <returns></returns>
        [HttpGet, Route("images/references/imageId/{imageId}")]
        public IHttpActionResult GetImageReferencesById(int imageId)
        {
            var mediaReferences = MediaMaintenance.GetAllImageNodeReferencesById(imageId);
            return Ok(mediaReferences);
        }

        [HttpGet, Route("references/purpose/{purpose?}")]
        public IHttpActionResult GetMediaNodesByPurpose(string purpose = MediaTypePurpose.UnUsed)
        {
            var references = MediaMaintenance.GetMediaReferencesByPuprose(purpose);
            return Ok(references);
        }
    }
}
