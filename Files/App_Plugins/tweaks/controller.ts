/// <reference path="../../Scripts/Core/app.ts" />
/// <reference path="../../Scripts/Core/Base/controller.ts" />
/// <reference path="../utilities/service.ts" />
/// <reference path="model.ts" />

/**
 * Tweaks module definition
 */
module TweaksModule {

    /**
     * Provides an interface that defines the methods and properties of the Tweaks scope
     */
    export interface ITweaksScope extends IBaseScope {
        searchTerm: { value: string};
        images: IImageModel[];
        onlyImages: boolean;
        filteredImages: IImageModel[];
        loadingImages: boolean;
        getImagesForTypeahead(searchTerm: string): any[];

        imageThumbnailUrl: string;
        getImageMedia: (image: IImageModel) => any;
        numberOfImagesDisplayed: number;
        selectedTypeaheadImage: IImageModel;
        onImageSelect(product, $model, $label): void;
        selectImage(image: IImageModel): void;

        dialogOptions: any;
    }

    /**
     * Provides the Generic Lookup controller for this custom plugin data type
     */
    export class TweaksController extends BaseController {

        // inject dependencies
        static $Inject = ["$scope", "dependencyContext", "utilitiesService", "ui.bootstrap"];

        /**
         * Creates a generic lookup controller
         * @constructor
         * @param $scope The angular scope - provides the angular scope for the custom data type
         * @param dependencyContext The dependency context
         * @param urlService The urls service
         */
        constructor(
            $scope: TweaksModule.ITweaksScope,
            dependencyContext: DependencyModule.IDependencyContext,
            private utilitiesService: UtilitiesModule.IUtilitiesService) {

            super(dependencyContext);
            
            //When you invoke the $watch() method AngularJS returns a "deregistration" function. 
            //invoke "deregistration" function and your $watch() listener will be removed.
            var dialogOptionsWatch = $scope.$watch('dialogOptions', () => {
                $scope.dialogOptions = {}; //set to empty object so Umbraco doesn't fail is
                dialogOptionsWatch();
            });

            // setup the scope
            var viewModel = $scope;

            viewModel.searchTerm = {
                value: ""
            };

            viewModel.images = [];
            viewModel.filteredImages = [];
            viewModel.onlyImages = true;
            const numberOfImagesDisplayed = 10;
            viewModel.numberOfImagesDisplayed = numberOfImagesDisplayed;


            viewModel.selectImage = (image: IImageModel) => {
                var controllerElement = document.querySelector(".umb-mediapicker");
                var controllerScope = <any>angular.element(controllerElement).scope();

                //use the same functions Umbraco controller uses to add the image
                var imageModel = viewModel.getImageMedia(image);
                controllerScope.images.push(imageModel);
                controllerScope.ids.push(imageModel.id);

                controllerScope.sync();

                //hide the media picker overlay
                controllerScope.mediaPickerOverlay.show = false;
            }


            function setImageThumbnailUrl(origin: string) {                
                viewModel.imageThumbnailUrl = origin;
            }
            setImageThumbnailUrl(window.location.origin);

            this.BaseService.GetJsonAsync(utilitiesService.urlService.GetAllImages(), true, (data: IMediaResponseModel) => {
                viewModel.images = data.MediaFiles;
            });

            viewModel.getImagesForTypeahead = (searchTerm: string): IImageModel[] => {

                viewModel.loadingImages = true;
                var filteredImages = [],
                    imageId = Number(searchTerm);

                if (isNaN(imageId)) {
                    // we filter by Name, can also filter by any other value
                    //or put |filter: $viewValue in the view which will filter on all properties
                    filteredImages = _.filter(viewModel.images, (image: IImageModel) => {
                        return image.Name.toLowerCase().indexOf(searchTerm.toLowerCase()) != -1;
                    });

                } else {
                    filteredImages = _.filter(viewModel.images, (image: IImageModel) => {
                        return image.Id === imageId;
                    });
                }
                viewModel.filteredImages = filteredImages;
                viewModel.loadingImages = false;
                return filteredImages;
            };

            viewModel.getImageMedia = (image: IImageModel) => {
                return {
                    isFolder: false,
                    metaData: { umbracoFile: { Value: image.Metadata.FilePath } },
                    id: image.Id,
                    thumbnail: `${image.Metadata.FilePath}?width=500&mode=max&animationprocessmode=first`,
                    name: image.Name,
                    onlyImages: true
                };
            }

            var tweakedMediaPickerContainer = document.getElementById("tweaked-media-picker-container");
            tweakedMediaPickerContainer.onscroll = (event) => {
                if (tweakedMediaPickerContainer.scrollTop + tweakedMediaPickerContainer.offsetHeight >= tweakedMediaPickerContainer.scrollHeight) {
                    viewModel.numberOfImagesDisplayed += numberOfImagesDisplayed;
                }
            }

            viewModel.onImageSelect = (image: IImageModel, $model, $label) => {
                viewModel.selectedTypeaheadImage = image;
                viewModel.filteredImages = [image];

                viewModel.searchTerm.value = "";
            }
        }
    }
}

// attach the controller to the app
app.controller("Umbraco.Plugins.Tweaks.Controller", TweaksModule.TweaksController);