/// <reference path="../../Scripts/Core/app.ts" />
/// <reference path="../../Scripts/Core/Base/controller.ts" />
/// <reference path="model.ts" />
/// <reference path="../../scripts/services/urlservice.ts" />

/**
 * Media Reference module definition
 */
module MediaReferenceModule {

    /**
     * Provides an interface that defines the methods and properties of the Media Reference scope
     */
    export interface IMediaReferenceScope extends IBaseScope {
        prevalues: IPrevalueFields;
        mediaReferences: any;
        getMediaReferences(): void;
        hasMediaReference(): boolean;
        openMediaReferenceInNewTab(mediaReference: any): void;
    }

    /**
     * Provides the MediaReference controller for this custom plugin data type
     */
    export class MediaReferenceController extends BaseController {

        // inject dependencies
        static $Inject = ["$scope", "dependencyContext", "urlService"];

        /**
         * Creates a Media Reference controller
         * @constructor
         * @param $scope The angular scope - provides the angular scope for the custom data type
         * @param dependencyContext The dependency context
         */
        constructor(
            $scope: MediaReferenceModule.IMediaReferenceScope,
            dependencyContext: DependencyModule.IDependencyContext, urlService: UrlModule.IUrlService) {

            super(dependencyContext);

            var viewModel = $scope;
            viewModel.mediaReferences = [];

            viewModel.prevalues = {
                ApiUrl: viewModel.model.config.ApiUrl
            }

            viewModel.hasMediaReference = () => {
                return viewModel.mediaReferences.length > 0;
            }

            viewModel.getMediaReferences = (): void => {
                var url = UmbracoApp.Common.Format(viewModel.prevalues.ApiUrl, this.GetActiveNodeId());
                this.BaseService.GetJsonAsync(url, true, (data) => {
                    viewModel.mediaReferences = data;
                });
            }

            function openInNewTab(url: string) {
                var tab = window.open(url, '_blank');
                tab.focus();
            }

            viewModel.openMediaReferenceInNewTab = (mediaReference: any) => {
                var url = `${window.location.origin}/umbraco#/content/content/edit/${mediaReference.NodeId}`;
                openInNewTab(url);
            }

            viewModel.getMediaReferences();

        }
    }
}

// attach the controller to the app
app.controller("Umbraco.Plugins.MediaReference.Controller", MediaReferenceModule.MediaReferenceController);