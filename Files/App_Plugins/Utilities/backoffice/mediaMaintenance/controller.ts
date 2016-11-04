/// <reference path="../../../../Scripts/Core/app.ts" />
/// <reference path="../../../../Scripts/Core/Base/controller.ts" />
/// <reference path="../../service.ts" />
/// <reference path="model.ts" />

/**
 * Media Maintenance module definition
 */
module MediaMaintenanceModule {

    /**
     * Provides an interface that defines the methods and properties of the media maintenance scope
     */
    export interface IMediaMaintenanceScope extends IBaseScope {

        /**
         * The plugin information
         */
        Plugin: BackofficeModule.IPlugin;

        /**
         * Method to determine if we are in create mode
         */
        EditMode(): boolean;

        toggleNavigation(): void;

        mediaFiles: IMediaModel[];

        mediaFolders: IFolderModel[];

        openMediaInNewTab(media: IBaseMediaModel): void;

        showMediaReferencesTab(media: IBaseMediaModel): void;

        mediaReferences: IMediaReference[];

        selectedMedia: IMediaModel;

        hasMediaReference(): boolean;
        openMediaReferenceInNewTab(mediaReference: IMediaReference): void;

        mediaFoldersWithNewFiles: any[];
        getFoldersWithNewFiles(days: number): void;
    }

    /**
     * Provides the media maintenance angular controller for this custom plugin data type
     */
    export class MediaMaintenanceController extends BaseController {

        // inject dependencies
        static $Inject = ["$scope", "dependencyContext", "utilitiesService"];

        /**
         * Creates a media maintenance controller
         * @constructor
         * @param $scope The angular scope - provides the angular scope for the custom data type
         * @param {dependencyContext: IDependencyContext} Provides a dependency context object for injecting dependencies
         * @param {utilitiesService: IUtilitiesService} Provides a service for accessing any data required for the custom utilities dashboard
         */
        constructor($scope: IMediaMaintenanceScope,
            dependencyContext: DependencyModule.IDependencyContext,
            private utilitiesService: UtilitiesModule.IUtilitiesService) {

            // base constructor
            super(dependencyContext);

            // setup media maintenance scope
            var viewModel = $scope;

            viewModel.mediaFiles = [],
                viewModel.mediaFolders = [],
                viewModel.mediaReferences = [],
                viewModel.mediaFoldersWithNewFiles = [];

            viewModel.toggleNavigation = () => {
                this.ToggleNavigation();
            };

            function openInNewTab(url: string) {
                var tab = window.open(url, '_blank');
                tab.focus();
            }

            this.BaseService.GetJsonAsync(utilitiesService.urlService.GetMediaMaintenance(), true, (data: IMediaMaintenance) => {
                viewModel.Plugin = data.Plugin;
            });

            this.BaseService.GetJsonAsync(utilitiesService.urlService.GetAllFoldersByType("umbraco"), true, (data: IMediaResponseModel<IFolderModel>) => {
                viewModel.mediaFolders = data.MediaFiles;
            });

            this.BaseService.GetJsonAsync(utilitiesService.urlService.GetAllImages(), true, (data: IMediaResponseModel<IMediaModel>) => {
                viewModel.mediaFiles = data.MediaFiles;
            });



            viewModel.showMediaReferencesTab = (media: IBaseMediaModel) => {
                this.BaseService.GetJsonAsync(utilitiesService.urlService.GetImageReferencesById(media.Id), true, (data: IMediaReference[]) => {
                    viewModel.selectedMedia = media;
                    viewModel.mediaReferences = data;

                    this.SelectSpecificNavTab(2);
                });
            }

            viewModel.openMediaInNewTab = (media: IBaseMediaModel) => {
                var url = `${window.location.origin}/umbraco#/media/media/edit/${media.Id}`;
                openInNewTab(url);
            }

            viewModel.openMediaReferenceInNewTab = (mediaReference: IMediaReference) => {
                var url = `${window.location.origin}/umbraco#/content/content/edit/${mediaReference.NodeId}`;
                openInNewTab(url);
            }

            viewModel.hasMediaReference = () => {
                return viewModel.mediaReferences.length > 0;
            }

            viewModel.getFoldersWithNewFiles = (days: number) => {
                this.BaseService.GetJsonAsync(utilitiesService.urlService.GetFoldersWithFilesAddedInLastNDays(days), true, (data: IMediaResponseModel<IFolderModel>) => {
                    viewModel.mediaFoldersWithNewFiles = data.MediaFiles;
                });
            }

        }
    }
}

// attach the controller to the app
app.controller("Umbraco.Plugins.MediaMaintenance.Controller", MediaMaintenanceModule.MediaMaintenanceController);