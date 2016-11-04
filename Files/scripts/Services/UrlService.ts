/// <reference path="../../Scripts/Core/app.ts" />
/// <reference path="../../Scripts/Core/Base/service.ts" />

/**
 * Url module definition
 */
module UrlModule {

    /**
     * Provides an interface that defines the methods & properties of the url service
     */
    export interface IUrlService extends IBaseService {        

        /**
         * Get Media Maintenance
         */
        GetMediaMaintenance(): string;

        GetMediaFolderStructure(): string;

        GetAllFoldersByType(folderType: string): string;

        GetAllImages(): string;

        GetFoldersWithFilesAddedInLastNDays(days: number): string;

        GetImageReferencesById(imageId: number): string;

        GetAllImagesByPuprose(purpose: string): string;

        GetAllEmptyFoldersByType(folderType: string): string;
    }

    /**
    * Provides a class for the url service
    */
    export class UrlService extends BaseService implements IUrlService, IBaseService {

        // inject dependencies
        static $Inject = ["dependencyContext"];

        /**
         * Creates a url service
         * @constructor
         * @param {dependencyContext: IDependencyContext} Provides a dependency context object for injecting dependencies
         */
        constructor(dependencyContext: DependencyModule.IDependencyContext) {
            // base constructor
            super(dependencyContext);

            // set the plugin
            this.PluginKey = "UrlService";
        }      


        /**
       * Get Media Maintenance
       */
        GetMediaMaintenance(): string {
            return "/media/tabs";
        }

        GetMediaFolderStructure(): string {
            return "/media/folder/structure";
        }

        GetAllFoldersByType(folderType: string): string {
            return `/media/folders/folderType/${folderType}/all`;
        }

        GetAllEmptyFoldersByType(folderType: string): string {
            return `/media/folders/folderType/${folderType}/empty`;
        }

        GetFoldersWithFilesAddedInLastNDays(days: number): string {
            return `/media/folders/old/days/${days}`;
        }

        GetAllImages(): string {
            return "/media/images/all";
        }

        GetImageReferencesById(imageId: number): string {
            return `/media/images/references/imageId/${imageId}`;
        }

        GetAllImagesByPuprose(purpose: string): string {
            return `/media/images/purpose/${purpose}`;
        }
    }
}

// set up the service in the angular app
app.service("urlService", UrlModule.UrlService);
